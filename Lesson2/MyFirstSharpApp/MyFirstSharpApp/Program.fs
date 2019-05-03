// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let items = argv.Length
    printfn "Passed in %d items: %A" items argv
    0 // return an integer exit code
