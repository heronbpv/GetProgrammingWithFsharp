// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#I @"..\packages\" //Using directive #I to add the relative package path to the library include path of fsi, thus simplifying the paths below.
#r @"Humanizer.Core.2.6.2\lib\netstandard1.0\Humanizer.dll"
#r @"Newtonsoft.Json.12.0.2\lib\net45\Newtonsoft.Json.dll" //This also makes this script more shareable, since there's no absolute paths here.
#load "Library1.fs" //Remember that, in order for this directive to compile, you have to import it's dependences in order. Newtonsoft.Json is one such example.

Library1.getPerson()

// Define your library scripting code here
//@Now you try 26.1.2
//Exploring the Humanizer package
open Humanizer
let x1 = "ScriptsAreAGreatWayToExplorePackages".Humanize()
let x2 = "ScriptsAreAGreatWayToExplorePackages
            ForSureItWouldBeaGoodIdeaAmiRight?".Humanize(LetterCasing.Title)
