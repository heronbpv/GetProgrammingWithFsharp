module NuGet

open FSharp.Data
//I probably have to re-do this using the path builder from System.IO, right?
[<LiteralAttribute>]
let Path = @"D:\Programacao\GetProgrammingWithFsharp\get-programming-fsharp-master\data\sample-package.html"
type private NuGetHtmlPage = HtmlProvider<Path>

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

///Gets the HTML page of a specified NuGet package
let getNugetPackagePage packageName = //Refactoring the original idea, using pipes instead. That way, I can keep propagating the package name to the functions that needs it.
    sprintf "https://www.nuget.org/packages/%s" packageName
    |> NuGetHtmlPage.Load

let getVersionsListFromNugetPackage (packagePage:NuGetHtmlPage) =
    packagePage.Tables.``Version History``.Rows

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

//Converts a version row from the NuGet package page to a internal representation -> Don't put triple-slashes here, as it leads to a build error! 
//See link: https://github.com/swlaschin/ImpossibleCompilerError/blob/master/src/ImpossibleCompilerError/MyModule.fs
let enrich (packageName:string) (rows:NuGetHtmlPage.VersionHistory.Row []) = 
    let versionsList = 
        rows
        |> Array.mapi
            (fun index versionRow -> 
                let isCurrentVersion = index = 0
                let version, packageVersion = parse isCurrentVersion versionRow.Version
                { Version = version; Downloads = versionRow.Downloads; PackageVersion = packageVersion; LastUpdated = versionRow.``Last updated`` } )
    { PackageName = packageName; Versions = versionsList }

///Loads the table of versions from a given NuGet package name, using an internal representation
let loadVersionsListForNugetPackage packageName = 
    getNugetPackagePage packageName
    |> getVersionsListFromNugetPackage 
    |> enrich packageName
    |> (fun package -> package.Versions)

///Retrieves information about the number of downloads of a given NuGet package.
let getDownloadsForPackage = 
    loadVersionsListForNugetPackage >> Seq.sumBy (fun versionHistory -> versionHistory.Downloads)

///Searches the versions list for a specific version of a given NuGet package.
let getDetailsForVersion versionText = 
    loadVersionsListForNugetPackage >> Seq.tryFind (fun version -> version.Version.ToString().Equals(versionText, StringComparison.OrdinalIgnoreCase))

///Checks information about the current version of a given NuGet package.
let getDetailsForCurrentVersion = 
    loadVersionsListForNugetPackage >> Seq.head 