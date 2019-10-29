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