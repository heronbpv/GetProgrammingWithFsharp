module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
let tryLoadAccountFromDisk = FileRepository.tryFindTransactionsOnDisk >> Option.map (Operations.loadAccount) //Lifting loadAccount to option to work with findTransactionsOnDisk

[<AutoOpen>]
module CommandParsing =
    //These two functions are now unnecessary.   
    //let isValidCommand cmd = List.map tryParseCommand cmd
    // isStopCommand = (=) 'x'
        
    ///Represents the commands avaiable to the user.
    type Command = 
        | AccountCommand of BankOperation
        | Exit
    let tryGetBankOperation = function
        | AccountCommand op -> Some op
        | Exit -> None

    ///Parses a given char to one of the valid commands. Values accepted: (w)ithdraw, (d)eposit, e(x)it
    let tryParseCommand = function
        | 'w' -> Some (AccountCommand Withdraw)
        | 'd' -> Some (AccountCommand Deposit)
        | 'x' -> Some (Exit)
        | _   -> None

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }
    
    let tryGetAmount command =
        Console.WriteLine()
        Console.Write "Enter Amount: "
        let amount = Console.ReadLine() |> Decimal.TryParse //Tentativelly converting the value, safely returns if it fails.
        match amount with
        | true, amount -> Some (command, amount)
        | false, _ -> None
        //command, Console.ReadLine() |> Decimal.Parse

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine() 
        match tryLoadAccountFromDisk owner with
        | Some account -> account
        | None -> { Balance = 0M; AccountId = Guid.NewGuid(); Owner = { Name = owner } }
    
    printfn "Current balance is £%M" openingAccount.Balance

    let processCommand account (command, amount) =
        printfn ""
        let account =
//            if command = 'd' then account |> depositWithAudit amount
//            else account |> withdrawWithAudit amount
            match command with
            | Withdraw -> account |> withdrawWithAudit amount
            | Deposit -> account |> depositWithAudit amount
        printfn "Current balance is R$%M" account.Balance
        account

    let closingAccount =
        commands
        |> Seq.choose tryParseCommand
        |> Seq.takeWhile (not << ((=) Exit))
        |> Seq.choose tryGetBankOperation
        |> Seq.choose tryGetAmount
        |> Seq.fold processCommand openingAccount
    
    printfn ""
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0