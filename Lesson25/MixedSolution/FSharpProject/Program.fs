// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let tony = CSharpProject.Person "Tony"
    tony.PrintName()
    0 // return an integer exit code
