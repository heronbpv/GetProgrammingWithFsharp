//@Now you try 15.1
//Which teams won the most away games in this season?
//Data structure and creation function
type FootballResult = //Represents the result of a game
    { HomeTeam : string
      AwayTeam : string
      HomeGoals : int
      AwayGoals : int }

let create (ht, hg) (at, ag) = 
    { HomeTeam = ht
      AwayTeam = at
      HomeGoals = hg
      AwayGoals = ag }

//Results of this season's games
let results = 
    [ create ("Messiville", 1) ("Ronaldo City", 2)
      create ("Messiville", 1) ("Bale Town", 3)
      create ("Bale Town", 3) ("Ronaldo City", 1)
      create ("Bale Town", 2) ("Messiville", 1)
      create ("Ronaldo City", 4) ("Messiville", 2)
      create ("Ronaldo City", 1) ("Bale Town", 2) ]

//How to define that a team won an away game?
///This filter returns true if the away team has scored more goals in the match than the home team. False otherwise.
let hasWonAwayGame result = 
    if result.AwayGoals > result.HomeGoals then 
        true
    else 
        false

///A function to transform a list of games into another, that contains only games won by the away team.
let listTeamsWhoWonAwayGames games = 
    List.filter hasWonAwayGame games

///A function to aggregate the results of a collection of FootballResults by the away team's name and it's number of victories.
let aggregateGamesByAwayTeamVictories games = 
    List.countBy (fun game -> game.AwayTeam) games

///Sorts a list of game result aggregates by the second element, which corresponds to the number of games won by the away team.
let sortAggregateByAwayTeamWonGamesDescending results =
    List.sortByDescending (fun result -> result |> snd) results

results
|> listTeamsWhoWonAwayGames
|> aggregateGamesByAwayTeamVictories
|> sortAggregateByAwayTeamWonGamesDescending

//The above is the equivalent to the following, I believe:
results
|> List.filter (fun game -> game.AwayGoals > game.HomeGoals)
|> List.countBy (fun game -> game.AwayTeam) 
|> List.sortByDescending (fun result -> result |> snd) //This is more ad-hoc, but less clear to the above pipeline.

//Another way, more imperative, to do the above, following the book's example:
open System.Collections.Generic
type TeamSummary = {Name:string; mutable AwayWins:int}
let summary = ResizeArray()

for result in results do
    if result.AwayGoals > result.HomeGoals then
        printfn "%A" result
        let mutable found = false
        for entry in summary do
            printfn "summary"
            if entry.Name = result.AwayTeam then
                found <- true
                entry.AwayWins <- entry.AwayWins + 1
            if not found then
                printfn "%s" result.AwayTeam
                summary.Add {Name = result.AwayTeam; AwayWins = 1}
            //Both if above evaluate to unit, thus ommiting the else clause is accepted.
let comparer = 
    { new IComparer<TeamSummary> with
          member this.Compare(x, y) = 
              if x.AwayWins > y.AwayWins then -1
              elif x.AwayWins < y.AwayWins then 1
              else 0 }
summary.Sort(comparer)
summary