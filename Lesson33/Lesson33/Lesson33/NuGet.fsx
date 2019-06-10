#load @"Scripts/load-project-debug.fsx"
open FSharp.Data

//@Now you try 33.1.1
//For whatever reason, at the time of writing this, the type provider can't load the relative path once sent to fsi, even though the code compiles...
type HtmlPage = HtmlProvider< @"D:\Programacao\GetProgrammingWithFsharp\get-programming-fsharp-master\data\sample-package.html">

let getPackagePage name = 
    let package = HtmlPage.Load(sprintf "https://www.nuget.org/packages/%s" name)
    package

///Retrieves information about the number of downloads of a given package.
let getDownloadsForPackage name = 
    let package = getPackagePage name
    package.Tables.``Version History``.Rows
    |> Seq.sumBy (fun versionHistory -> versionHistory.Downloads)

//Tests - seems to be working... no idea if the numbers are accurate, though!
getDownloadsForPackage "Newtonsoft.Json"
getDownloadsForPackage "EntityFramework"
getDownloadsForPackage "FSharp.Data"

open System
let getDetailsForVersion name text =
    let package = getPackagePage name
    package.Tables.``Version History``.Rows
    |> Seq.tryFind (fun versionHistory -> versionHistory.Version.Contains text)

//Tests - seems like the text inside the version column now is only the version numbers, which sadly kind of dumbs down this example...
getDetailsForVersion "Newtonsoft.Json" "4.9"
getDetailsForVersion "EntityFramework" "3.0"
getDetailsForVersion "FSharp.Data" "1.0"