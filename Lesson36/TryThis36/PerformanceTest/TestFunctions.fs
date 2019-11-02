module PerformanceTest.TestFunctions

open System
open System.Diagnostics
open System.Net
open System.Threading
open System.Threading.Tasks

///Evaluates a given function f with parameter xs, and returns the amount of time, in milliseconds, that said function takes to complete.
let evaluateTimeToProcess f xs =
    let timer = new Stopwatch()
    timer.Start()
    f xs
    timer.Stop()
    timer.ElapsedMilliseconds

///Synchronously downloads each page from the given list of urls, then prints to the standard console output.
let synchronousDownload urls =
    //Bypass for the lack of certification configuration in my local machine
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Ssl3
    use client = new WebClient()
    for url in urls do
        let uri = new Uri(url)
        client.DownloadData(uri) |> ignore
        printfn "[ID:%d]Normal - Downloaded %s" Thread.CurrentThread.ManagedThreadId url

///Asynchronously downloads each page from the given list of urls, then prints to the standard console output.
let asynchronousDownload urls =
    //Bypass for the lack of certification configuration in my local machine
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Ssl3
    async {
        use client = new WebClient()
        for url in urls do
            let uri = new Uri(url)
            let! _ = client.AsyncDownloadData(uri) //Uses let! to make the asynchronous call, then discard it.
            return printfn "[ID:%d]Async - Downloaded %s" Thread.CurrentThread.ManagedThreadId url
    }
    |> Async.Start

///Uses multiple threads to download each page from the given list of urls, then prints to the standard console output.
let multithreadedDownload urls =
    //Bypass for the lack of certification configuration in my local machine
    ServicePointManager.SecurityProtocol <- SecurityProtocolType.Tls12 ||| SecurityProtocolType.Ssl3
    use client = new WebClient()
    for url in urls do
        let uri = new Uri(url)
        Task.Run(fun _ -> client.DownloadData(uri)) |> ignore
        printfn "[ID:%d]MT - Downloaded %s" Thread.CurrentThread.ManagedThreadId url