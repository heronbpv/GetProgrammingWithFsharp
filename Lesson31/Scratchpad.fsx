#I @"..\get-programming-fsharp-master\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"
#r @"XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"
#r @"Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"
open FSharp.Data
open XPlot.GoogleCharts

//@Now you try 31.1.2
type Films = HtmlProvider< @"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">
let deNiro = Films.GetSample()

deNiro.Tables.Film.Rows
|> Array.countBy (fun film -> film.Year.ToString())
|> Chart.SteppedArea
|> Chart.Show

//@Now you try 31.3.1
type Nuget = HtmlProvider< @"..\get-programming-fsharp-master\data\sample-package.html">
let nuget = Nuget.GetSample()
//nuget.Tables.``Version History``

let nunit = Nuget.Load @"https://www.nuget.org/packages/nunit"
//nunit.Tables.``Version History``
let ef = Nuget.Load @"https://www.nuget.org/packages/EntityFramework"
let nsjson = Nuget.Load @"https://www.nuget.org/packages/Newtonsoft.Json"

[nunit; ef; nsjson]
|> Seq.collect (fun package -> package.Tables.``Version History``.Rows)
|> Seq.sortByDescending (fun versions -> versions.Downloads)
|> Seq.take 10
|> Seq.map (fun versions -> versions.Version, versions.Downloads)
|> Chart.Column
|> Chart.Show

//@Try this 31
type DT = HtmlProvider< @"https://en.wikipedia.org/wiki/List_of_songs_recorded_by_Dream_Theater">
let dtAlbuns = DT.GetSample()

open System
dtAlbuns.Tables.List.Rows
|> Array.map (fun album -> album.Album, album.Year.ToString())
|> Array.distinct
|> Array.filter (fun (albumName, _) -> not (albumName.Equals("N/A", StringComparison.OrdinalIgnoreCase)))
|> Array.groupBy (fun (_, releaseYear) -> releaseYear)
|> Array.map (fun (year, albuns) -> year, albuns.Length)
|> Array.sortBy (fun (year, _) -> year)
|> Chart.Line
|> Chart.Show