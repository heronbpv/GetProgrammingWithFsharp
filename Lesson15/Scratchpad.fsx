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
///This filter returns true if the away team has scored more goals in the match than the home team.
let hasWonAwayGame (result:FootballResult) = 
    if result.AwayGoals > result.HomeGoals then 
        true
    else 
        false

///A function to transform a list of games into another, that contains only games won by the away team.
let listTeamsWhoWonAwayGames (games:FootballResult list) = 
    List.filter hasWonAwayGame games


results
|> listTeamsWhoWonAwayGames