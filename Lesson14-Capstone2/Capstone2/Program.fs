// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

//Orchestration
open Capstone2.Domain
open Capstone2.Operations
open Capstone2.Auditing

let auditAs operationName audit operation amount account = 
    audit account ("Orchestrator starting operation '" + operationName + " " + amount.ToString() + " bucks' on account. Current balance: '" + account.Balance.ToString() + " bucks'.")
    let currentBalance = account.Balance
    let result = operation amount account
    audit account ("Orchestrator attempted operation...")
    if currentBalance <> result.Balance then 
        audit account ("... Operation successful. New balance '" + result.Balance.ToString() + " bucks'.")
    else 
        audit account ("... Operation failed. Transaction aborted. Balance remains: '" + result.Balance.ToString() + " bucks'.")
    result

let withdrawWithConsoleAudit = auditAs "withdraw" consoleAudit withdraw
let depositWithConsoleAudit = auditAs "deposit" consoleAudit deposit

let withdrawWithFileAudit = auditAs "withdraw" fileSystemAudit withdraw
let depositWithFileAudit = auditAs "deposit" fileSystemAudit deposit

open System 
let greeting () = 
    Console.WriteLine("Welcome to your banking system. Please, identify yourself.")
    Console.WriteLine("Name: ")
    let name = Console.ReadLine()
    Console.WriteLine("Age: ")
    let age = int(Console.ReadLine())
    Console.WriteLine("Now, register the initial balance for your account.")
    Console.WriteLine("Bucks: ")
    let balance = decimal(Console.ReadLine())

    let user = {Id = Guid.Empty; Age = age; Name = name}
    {Id = Guid.Empty; Balance = balance; Owner = user}

[<EntryPoint>]
let main _ =    
    let mutable account = greeting()
    while true do
        Console.WriteLine("Greetings, {0}. What would you like to do with your account?", account.Owner.Name)
        Console.WriteLine("Options (inform the number to the desired action)")
        Console.WriteLine("1)Withdraw")
        Console.WriteLine("2)Deposit")
        Console.WriteLine("3)Exit")
        let action = Console.ReadLine()

        if action = "3" then Environment.Exit 0

        Console.WriteLine("Action: {0}. How much?", action)
        Console.WriteLine("Bucks: ")
        let amount = decimal(Console.ReadLine())

        account <-
            if action = "1" then account |> withdrawWithConsoleAudit amount
            elif action = "2" then account |> depositWithConsoleAudit amount
            else account
    0
