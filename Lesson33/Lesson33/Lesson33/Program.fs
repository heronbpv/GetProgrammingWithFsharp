﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open NuGet
[<EntryPoint>]
let main _ = 
    getDetailsForCurrentVersion "entityframework" |> printfn "%A"

    0 // return an integer exit code
