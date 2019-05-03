// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let items = argv.Length
    printfn "Passed in %d items: %A" items argv

    for x = 1 to argv.Length do
        printfn "Position: %d; Value: %s; Lenght: %d" (x) argv.[x-1] argv.[x-1].Length

    //A proper, functional way of doing the "Try this" of lesson 2
//    argv
//    |> Array.iteri (fun x y -> printfn "Position: %d; Value: %s; Lenght: %d" (x + 1) y y.Length) 

    0 // return an integer exit code
