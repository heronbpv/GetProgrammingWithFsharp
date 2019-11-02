open System
open PerformanceTest.TestFunctions
let urls = [|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"
             ; "https://github.com/"; "https://docs.microsoft.com"; "http://www.giantitp.com"
             ; "https://mangadex.org"; "https://readms.net/"; "https://www.webtoons.com"
             ; "https://www.youtube.com"|]

[<EntryPoint>]
let main _ = 
    printfn "Comparing the time to download 10 pages"
    
    printfn "Synchronous call:"
    let timeSync = evaluateTimeToProcess synchronousDownload urls
    printfn "Elapsed time: %d ms" timeSync
    
    printfn "Asynchronous call:"
    let timeAsync = evaluateTimeToProcess asynchronousDownload urls
    printfn "Elapsed time: %d ms" timeAsync
    
    printfn "Multithreaded call:"
    let multi = evaluateTimeToProcess multithreadedDownload urls
    printfn "Elapsed time: %d ms" multi
    
    printfn "Execution completed!"
    
    Console.ReadLine() |> ignore
    0 // return an integer exit code
