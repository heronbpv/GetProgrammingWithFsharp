module Capstone3.Program

open System
open Capstone3.Domain
open Capstone3.Operations

//Pipeline functions
///Checks whether the command is one of (d)eposit, (w)ithdraw, or e(x)it.
let isValidCommand (command:char) = ['d'; 'w'; 'x'] |> List.contains command

///Checks whether the command is the e(x)it command.
let isStopCommand (command:char) = command = 'x'

///Queries the user for the amount desired, and returns a tuple of the command and said value.
let getAmount (command:char) = 
    Console.Write "\r\nEnter amount: "
    let amount = Console.ReadLine()
    let amount = decimal (amount) //Shadowing to the rescue!
    command, amount

///Applies the given pair of command and amount to the account in question.
let processCommand (account:Account) (command:char, amount:decimal) = 
    match command with
    | 'd' -> deposit amount account
    | 'w' -> withdraw amount account
    | _ -> account

[<EntryPoint>]
let main _ =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
    let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

    let openingAccount = { Owner = { Name = name }; Balance = 0M; AccountId = Guid.Empty } 
    Console.WriteLine ("Current balance is R$" + openingAccount.Balance.ToString()) //Initial print of the balance

    let closingAccount =
        let commands = seq {
            while true do
                Console.Write "(d)eposit, (w)ithdraw, or e(x)it:"
                yield Console.ReadKey().KeyChar}
        
        commands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand) //operator "<<" read as "compose left with" or "compose backwards from", in regards to the flow direction of output to input.
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount
        

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0