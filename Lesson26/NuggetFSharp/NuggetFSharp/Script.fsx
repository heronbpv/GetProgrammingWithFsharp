// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#r @"D:\Programacao\GetProgrammingWithFsharp\Lesson26\NuggetFSharp\packages\Humanizer.Core.2.6.2\lib\netstandard1.0\Humanizer.dll"
#r @"D:\Programacao\GetProgrammingWithFsharp\Lesson26\NuggetFSharp\packages\Newtonsoft.Json.12.0.2\lib\net45\Newtonsoft.Json.dll"
#load "Library1.fs"

Library1.getPerson()

// Define your library scripting code here
//@Now you try 26.1.2
//Exploring the Humanizer package
open Humanizer
let x1 = "ScriptsAreAGreatWayToExplorePackages".Humanize()
let x2 = "ScriptsAreAGreatWayToExplorePackages
            ForSureItWouldBeaGoodIdeaAmiRight?".Humanize(LetterCasing.Title)
