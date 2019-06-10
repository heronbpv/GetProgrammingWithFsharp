#load @"Scripts/load-project-debug.fsx"
open FSharp.Data

//@Now you try 33.1.1
//For whatever reason, at the time of writing this, the type provider can't load the relative path once sent to fsi, even though the code compiles...
type HtmlPage = HtmlProvider< @"D:\Programacao\GetProgrammingWithFsharp\get-programming-fsharp-master\data\sample-package.html">

///Gets the HTML page of a specified NuGet package
let getNugetPackagePage = //Something that bothers me: whenever I use the function composition operator, I lose visual information about the params being used.
    sprintf "https://www.nuget.org/packages/%s" >> HtmlPage.Load

///Provides a enumeration based on the elements of the Version table of a NuGet package HTML page.
let getVersionsListFromNugetPackage (packagePage:HtmlPage) =
    packagePage.Tables.``Version History``.Rows

///Loads a collection of versions from a given NuGet package name.
let loadVersionsListForNugetPackage = //Maybe refactor the above code to be internal to this function?
    getNugetPackagePage >> getVersionsListFromNugetPackage

///Retrieves information about the number of downloads of a given NuGet package.
let getDownloadsForPackage = 
    loadVersionsListForNugetPackage >> Seq.sumBy (fun versionHistory -> versionHistory.Downloads)

//Tests - seems to be working... no idea if the numbers are accurate, though!
getDownloadsForPackage "Newtonsoft.Json"
getDownloadsForPackage "EntityFramework"
getDownloadsForPackage "FSharp.Data"

///Searches the versions list for a specific version of a given NuGet package.
let getDetailsForVersion versionText = 
    //This change broke the tests, because the function composition operator is forcing the implied name parameter to be the second one...
    loadVersionsListForNugetPackage >> Seq.tryFind (fun versionHistory -> versionHistory.Version.Contains versionText)

//Tests - the calls returns the version text, number of downloads of the specific version and the date/time object representing it's release date.
//Tests changed due to API refactoring. Good or Bad thing?
getDetailsForVersion "4.9" "Newtonsoft.Json"
getDetailsForVersion "3.0" "EntityFramework"
getDetailsForVersion "1.0" "FSharp.Data"

///Checks information about the current version of a given NuGet package.
let getDetailsForCurrentVersion = //And here is a example that got broken by time, at least the suggested solution in the book: there's no more a (this version) row.
    //getDetailsForVersion "(this version)"
    //So, instead, after inspecting the nuget page for one of the examples, I made this change: instead of the above call, just get the head of the collection
    loadVersionsListForNugetPackage >> Seq.head //This is because the first element, the head, is always the most recent package version, at least at the time of writing!

//Tests - correctly returns the most recent package version, see comments on the function!
getDetailsForCurrentVersion "Newtonsoft.Json"
getDetailsForCurrentVersion "EntityFramework"
getDetailsForCurrentVersion "FSharp.Data"