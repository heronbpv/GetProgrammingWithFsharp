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

//@Now you try 36.4
open System.Net
///Downloads a page, then returns it's size.
let downloadData url = 
    //See https://social.msdn.microsoft.com/Forums/pt-BR/ad198bca-a5b5-4f62-aab9-e3fa0e75f7ee/webclient-erroquota-solicitao-foi-anulada-no-foi-possvel-criar-um-canal-seguro-para?forum=vsvbasicpt
    //Seems to be some sort of configuration that must be done to avoid errors accessing http pages (a.k.a. pages without a certificate).
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Ssl3
    async {
        let uri = new System.Uri(url)
        use client = new WebClient()
        let! page = client.AsyncDownloadData uri
        return page.Length
    }

//This pipeline will download the pages in parallel, then sum up their sizes, in a manner akin to a fork/join strategy.
[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> Array.map downloadData
|> Async.Parallel //Fork part of the pipeline
|> Async.RunSynchronously
|> Array.sum //Join part of the pipeline