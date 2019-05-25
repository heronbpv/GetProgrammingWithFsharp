module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
let loadAccountFromDisk = FileRepository.findTransactionsOnDisk >> Operations.loadAccount

[<AutoOpen>]
module CommandParsing =
    //These two functions are now unnecessary.   
    //let isValidCommand cmd = List.map tryParseCommand cmd
    // isStopCommand = (=) 'x'
    
    ///Represents the commands avaiable to the user.
    type Command = 
        | Withdraw
        | Deposit
        | Exit

    ///Parses a given char to one of the valid commands. Values accepted: (w)ithdraw, (d)eposit, e(x)it
    let tryParseCommand = function
        | 'w' -> Some Withdraw
        | 'd' -> Some Deposit
        | 'x' -> Some Exit
        | _   -> None

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }
    
    let getAmount command =
        Console.WriteLine()
        Console.Write "Enter Amount: "
        command, Console.ReadLine() |> Decimal.Parse

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        Console.ReadLine() |> loadAccountFromDisk
    
    printfn "Current balance is £%M" openingAccount.Balance

    let processCommand account (command, amount) =
        printfn ""
        let account =
//            if command = 'd' then account |> depositWithAudit amount
//            else account |> withdrawWithAudit amount
            match command with
            | Withdraw -> account |> withdrawWithAudit amount
            | Deposit -> account |> depositWithAudit amount
            | Exit -> account
        printfn "Current balance is R$%M" account.Balance
        account

    let closingAccount =
        commands
        |> Seq.choose tryParseCommand
        |> Seq.takeWhile (not << ((=) Exit))
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0