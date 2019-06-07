#I @"..\get-programming-fsharp-master\packages"
//Now you try 30.2.2
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

type Football = CsvProvider< @"..\get-programming-fsharp-master\data\FootballResults.csv">
let data = Football.GetSample().Rows |> Seq.toArray
data.[0]