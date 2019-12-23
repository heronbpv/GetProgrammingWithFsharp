open System
open Lesson37WebAPI
open Microsoft.Owin.Hosting

[<EntryPoint>]
let main _ =
    //Configures this console application to launch the web app host at port 9000 
    use app = WebApp.Start<Startup>(url =  "http://localhost:9000/")
    printfn "Listening on localhost:9000!"
    Console.ReadLine() |> ignore //Blocking the console from quitting!
    0 // return an integer exit code
