#load @"Scripts/load-project-debug.fsx"
open FSharp.Data

//@Now you try 33.1.1
//For whatever reason, at the time of writing this, the type provider can't load the relative path once sent to fsi, even though the code compiles...
type HtmlPage = HtmlProvider< @"D:\Programacao\GetProgrammingWithFsharp\get-programming-fsharp-master\data\sample-package.html">

///Gets the HTML page of a specified NuGet package
//let getNugetPackagePage = //Something that bothers me: whenever I use the function composition operator, I lose visual information about the params being used.
//    sprintf "https://www.nuget.org/packages/%s" >> HtmlPage.Load
let getNugetPackagePage packageName = //Refactoring the original idea, using pipes instead. That way, I can keep propagating the package name to the functions that needs it.
    sprintf "https://www.nuget.org/packages/%s" packageName
    |> HtmlPage.Load

///Provides a enumeration based on the elements of the Version table of a NuGet package HTML page.
let getVersionsListFromNugetPackage (packagePage:HtmlPage) =
    packagePage.Tables.``Version History``.Rows

///Loads a collection of versions from a given NuGet package name.
//let loadVersionsListForNugetPackage = //Maybe refactor the above code to be internal to this function?
//    getNugetPackagePage >> getVersionsListFromNugetPackage
let loadVersionsListForNugetPackage packageName = //Same as getNugetPackageName
    getNugetPackagePage packageName
    |> getVersionsListFromNugetPackage

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

//@Now you try 33.2.3
open System
type PackageVersion = 
    | CurrentVersion
    | Prerelease
    | Old

type VersionDetails = 
    {
        Version: Version //This comes from the System namespace!
        Downloads: decimal
        PackageVersion: PackageVersion
        LastUpdated: DateTime
    }

type NuGetPackage = 
    {
        PackageName: string
        Versions: VersionDetails []
    }

let parse isCurrentVersion (versionText:string) = //Added the isCurrentVersion parameter, since the (this version) text is no more; Now, the responsibility is on the caller to know
    let splitString text separator = if String.IsNullOrWhiteSpace text then [||] else text.Split [|separator|];
    let tokens = splitString versionText ' ' |> Array.rev //Reverse the array, since the version info is always at the end.
    match tokens |> List.ofArray with
    | [] -> 
        failwith "Must be at least two elements to a version"
    | x::_ -> 
        let tokens = splitString x '-'
        match (tokens |> List.ofArray, isCurrentVersion) with
        | x::_, true -> (Version.Parse x), CurrentVersion
        | x::xs, false when not xs.IsEmpty -> (Version.Parse x), Prerelease
        | x::_, false -> (Version.Parse x), Old
        | _ -> failwith "Unknown version format"

//Tests - verifying the functionality of parse
(getDetailsForCurrentVersion "Newtonsoft.Json").Version |> parse true
(getDetailsForCurrentVersion "EntityFramework").Version |> parse true
(getDetailsForCurrentVersion "FSharp.Data").Version |> parse true

(getDetailsForCurrentVersion "Newtonsoft.Json").Version |> parse false
(getDetailsForCurrentVersion "EntityFramework").Version |> parse false
(getDetailsForCurrentVersion "FSharp.Data").Version |> parse false

let enrich (packageName:string) (rows:HtmlPage.VersionHistory.Row []) = 
    let versionsList = 
        rows
        |> Array.mapi
            (fun index versionRow -> 
                let isCurrentVersion = index = 0
                let version, packageVersion = parse isCurrentVersion versionRow.Version
                { Version = version; Downloads = versionRow.Downloads; PackageVersion = packageVersion; LastUpdated = versionRow.``Last updated`` } )
    { PackageName = packageName; Versions = versionsList }

enrich "Newtonsoft.Json" (loadVersionsListForNugetPackage "Newtonsoft.Json")
enrich "EntityFramework" (loadVersionsListForNugetPackage "EntityFramework")
enrich "FSharp.Data" (loadVersionsListForNugetPackage "FSharp.Data")

//Pasting these functions here again to test, and to avoid moving the now you try 33.2.3 code around!
let loadVersionsListForNugetPackage2 packageName = 
    //And, by using pipes, I can both be explicit about what is necessary at the function declaration, and propagate the param to the functions that need it
    getNugetPackagePage packageName
    |> getVersionsListFromNugetPackage 
    |> enrich packageName
    |> (fun package -> package.Versions)

let getDetailsForVersion2 versionText = 
    loadVersionsListForNugetPackage2 >> Seq.tryFind (fun version -> version.Version.ToString().Equals(versionText, StringComparison.OrdinalIgnoreCase))

getDetailsForVersion2 "12.0.2" "Newtonsoft.Json"
getDetailsForVersion2 "6.3.0" "EntityFramework"
getDetailsForVersion2 "3.1.1" "FSharp.Data"

let getDetailsForCurrentVersion2 = //This implementation is unchanged from the above. Hooray!
    loadVersionsListForNugetPackage2 >> Seq.head 

getDetailsForCurrentVersion2 "Newtonsoft.Json"
getDetailsForCurrentVersion2 "EntityFramework"
getDetailsForCurrentVersion2 "FSharp.Data"
(getDetailsForCurrentVersion2 "Newtonsoft.Json") = (getDetailsForVersion2 "12.0.2" "Newtonsoft.Json" |> Option.get)