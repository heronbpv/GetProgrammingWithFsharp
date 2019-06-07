#I @"..\get-programming-fsharp-master\packages"
//Now you try 30.2.2
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data
open System

type Football = CsvProvider< @"..\get-programming-fsharp-master\data\FootballResults.csv", HasHeaders = true>
let data = Football.GetSample().Rows |> Seq.toArray

//@Question: Which were the highest scores by home teams across the season? 
data
|> Array.iter (fun game -> printfn "%s" game.``Full Time Result``) //Full time represents who won the game; (D)raw, (A)way, or (H)ome.

data 
|> Array.filter (fun game -> game.``Full Time Result``.Equals("H", StringComparison.OrdinalIgnoreCase)) //Search all game where the home team won
|> Array.sortByDescending (fun game -> game.``Full Time Home Goals``)
//|> Array.take 3 //Take 3 here seems to exclude one game, that has the same score as the third one (6-1). Is there another way?
|> Array.groupBy (fun game -> sprintf "%i-%i" game.``Full Time Home Goals`` game.``Full Time Away Goals``) //Let's try grouping first...
|> Array.take 3 //Now I have the three highest scores games. Let's assume that draws appear in the final listing
|> Array.map snd
|> Array.collect id //This sequence of map and collect flattens the second array of games generated by the groupBy.
|> Array.iter (fun game -> 
                printfn "Home Team '%s' vs Away Team '%s'; Score (Home-Away) - %i-%i" game.``Home Team`` game.``Away Team`` game.``Full Time Home Goals`` game.``Full Time Away Goals``)

//Visualizing the data
#r @"Newtonsoft.Json\lib\net45\Newtonsoft.Json.dll"
#r @"XPlot.GoogleCharts\lib\net45\XPlot.GoogleCharts.dll"
#r @"Google.DataTable.Net.Wrapper\lib\Google.DataTable.Net.Wrapper.dll"

open XPlot.GoogleCharts
//@Question: Which three teams won at home the most over the whole season?
data
|> Seq.filter (fun game -> game.``Full Time Home Goals`` > game.``Full Time Away Goals``)
|> Seq.countBy (fun game -> game.``Home Team``) //countBy generates a sequence of tuples. This one represents the number of wins
|> Seq.sortByDescending snd
|> Seq.take 10
|> Chart.Column
|> Chart.Show

//@Try this 30
//@Question: Which are the top 5 teams that scored the most goals? Display the results as a pie cart.

let awayTeamsAndGoals = 
    data 
    |> Array.map (fun game -> game.``Away Team``, game.``Full Time Away Goals``)
let homeTeamsAndGoals =
    data 
    |> Array.map (fun game -> game.``Home Team``, game.``Full Time Home Goals``)

awayTeamsAndGoals
|> Array.append homeTeamsAndGoals
|> Array.groupBy fst //The first element is the team name, the second is an array of team names and goals. Not the most optimal...
|> Array.map (fun (team, goals) -> team, (goals |> Array.map snd |> Array.sum)) //Reduces the second element of the tuple to a single number. Is there a better way?
|> Array.take 5
|> Chart.Pie
|> Chart.Show