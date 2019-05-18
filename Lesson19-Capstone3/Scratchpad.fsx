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

///Recreates an account information, based on it's transaction history and owner's personal information.
let loadAccount ownerName accountId (transactions:Transaction list) = 
    let account = { AccountId = accountId; Balance = 0M; Owner = { Name = ownerName } }
    transactions 
    |> List.sortBy (fun transaction -> transaction.Timestamp) 
    |> List.filter (fun transaction -> transaction.Accepted) //The only transactions that influence the balance are the accepted ones.
    |> List.fold 
        (fun acc transaction -> 
            let command = transaction.Operation.Chars(0)
            match command with
            | 'd' -> deposit transaction.Amount acc
            | 'w' -> withdraw transaction.Amount acc
            | _ -> acc
        ) 
        account
//Testing the loadAccount
let transactions = 
    [ { Timestamp = DateTime.Now; Operation = "deposit"; Amount = 50M; Accepted = false; Message = "test1" } 
      { Timestamp = DateTime.Now.AddDays(-1.0); Operation = "deposit"; Amount = 50M; Accepted = true; Message = "test1" } 
      { Timestamp = DateTime.Now; Operation = "withdraw"; Amount = 50M; Accepted = true; Message = "test2" } 
      { Timestamp = DateTime.Now.AddDays(-1.0); Operation = "withdraw"; Amount = 100M; Accepted = true; Message = "test3" } 
      { Timestamp = DateTime.Now.AddDays(-3.0); Operation = "deposit"; Amount = 50M; Accepted = true; Message = "test4" } ]

transactions
|> loadAccount "heron" Guid.Empty