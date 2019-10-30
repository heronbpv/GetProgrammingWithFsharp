printfn "Loading data!"
System.Threading.Thread.Sleep 5000
printfn "Loaded data!"
printfn "My name is Simon."

async {
    printfn "Loading data!"
    System.Threading.Thread.Sleep 5000 //A call to Async.Sleep here would not block for the given time.
    printfn "Loaded data!"             //This shows that async calls are never blocking!
}
|> Async.Start
printfn "My name is Simon."

let asyncHello : Async<string> = async { return "Hello!" }
//let length = asyncHello.Length //error FS0039: The field, constructor or member 'Length' is not defined -> It's because the async value was not unwraped yet.
let text = asyncHello |> Async.RunSynchronously //Unwraps the async, to extract the underlying computation, which returns a string.
let lengthTwo = text.Length //The resulting value now has the expected properties!

open System.Threading

let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text
let doWork () = 
    printThread "Starting long running work."
    Thread.Sleep 5000
    "HELLO"
let asyncLength:Async<int> = 
    printThread "1) Creating async block."
    let asyncBlock = 
        async {
            printThread "In block!"
            let text = doWork ()
            return (text + " WORLD").Length
        }
    printThread "2) Created async block."
    asyncBlock

let length = asyncLength |> Async.RunSynchronously

let getTextAsync = async { return "HELLO" }
let printHelloWorldAsync = 
    async {
        let! text = getTextAsync
        return printfn "%s WORLD" text
    }
printHelloWorldAsync |> Async.Start

let random = System.Random()
let pickANumberAsync = async { return random.Next 10 }
let createFiftyNumbers = 
    let workflows = [ for i in 1 .. 10 -> pickANumberAsync ]
    async {
        let! numbers = workflows |> Async.Parallel
        return printfn "Total is %d" (numbers |> Array.sum)
    }
createFiftyNumbers |> Async.Start

//@Now you try 36.4 and @Now you try 36.5.1. Decided to refator these two examples into one, with some parameterization to control which side to use: Async or Task.
open System.Net
///Array of sites used for the examples
let sites = [|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]

///A simple union to differentiate operations using Async workflows or Tasks.
type AsyncOrTask = Async | Task

///Downloads a page then returns it's size.
let downloadData asyncOrTask url = 
    //See https://social.msdn.microsoft.com/Forums/pt-BR/ad198bca-a5b5-4f62-aab9-e3fa0e75f7ee/webclient-erroquota-solicitao-foi-anulada-no-foi-possvel-criar-um-canal-seguro-para?forum=vsvbasicpt
    //Seems to be some sort of configuration that must be done to avoid errors accessing http pages (a.k.a. pages without a certificate).
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Ssl3
    async {
        let uri = new System.Uri(url)
        use client = new WebClient()
        let! page = //Asynchornously downloads the page; Decices how based on the asyncOrTask argument
            match asyncOrTask with
            | Async -> client.AsyncDownloadData uri
            | Task  -> client.DownloadDataTaskAsync uri |> Async.AwaitTask
        return page.Length
    }

///Applies a Fork/Join strategy to a collection of asynchronous workflows, then runs them
let parallelExecution asyncOrTask arr = 
    arr
    |> Async.Parallel         
    |> fun x -> //Runs the asynchronous computation, either as an async or task type.
       match asyncOrTask with 
       | Async -> x |> Async.RunSynchronously
       | Task  -> x |> Async.StartAsTask |> (fun y -> y.Result)

///Downloads an array of sites asynchronously in parallel, calculates their sizes (in bytes), then join the results and sum them up.
let downloadedBytes sites asyncOrTask = 
    sites
    |> Array.map (downloadData asyncOrTask) //Returns an array of async expressions, which will compute the length of each referenced page when executed.
    |> parallelExecution asyncOrTask
    |> Array.sum

//Strangely enough, the results are different at the time of this comment (30/10/2019 16:47)    
let asyncResults = downloadedBytes sites Async //208.824 bytes
let taskResults = downloadedBytes sites Task   //208.879 bytes

printfn "You downloaded %d characters" asyncResults
printfn "You downloaded %d characters" taskResults