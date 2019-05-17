#load "Domain.fs"
#load "Operations.fs"

open Capstone3.Operations
open Capstone3.Domain
open System

//Pipeline functions
///Checks whether the command is one of (d)eposit, (w)ithdraw, or e(x)it.
let isValidCommand (command:char) = ['d'; 'w'; 'x'] |> List.contains command

///Checks whether the command is the e(x)it command.
let isStopCommand (command:char) = command = 'x'

///Returns a tuple of the command and the associated predefined value, or zero.
let getAmount (command:char) = 
    match command with
    | 'd' -> command, 50M
    | 'w' -> command, 25M
    | _ -> command, 0M

///Applies the given pair of command and amount to the account in question.
let processCommand (account:Account) (command:char, amount:decimal) = 
    match command with
    | 'd' -> deposit amount account
    | 'w' -> withdraw amount account
    | _ -> account

//Testing the pipeline
let openingAccount = {Owner = {Name = "Isaac"}; Balance = 0M; AccountId = Guid.Empty} //The test account
let account = 
    let commands = ['d'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w']
    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand) //operator "<<" read as "compose left with" or "compose backwards from", in regards to the flow direction of output to input.
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount