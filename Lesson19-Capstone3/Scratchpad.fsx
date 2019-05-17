#load "Domain.fs"
#load "Operations.fs"

open Capstone3.Operations
open Capstone3.Domain
open System

//Pipeline functions
let isValidCommand (command:char) = if command = 'w' then true else false
let isStopCommand (command:char) = false
let getAmount (command:char) = command, 0M
let processCommand (account:Account) (command:char, amount:decimal) = account

//Testing the pipeline
let openingAccount = {Owner = {Name = "Isaac"}; Balance = 0M; AccountId = Guid.Empty} //The test account
let account = 
    let commands = ['d'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w']
    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand) //operator "<<" read as "compose left with" or "compose backwards from", in regards to the flow direction of output to input.
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount