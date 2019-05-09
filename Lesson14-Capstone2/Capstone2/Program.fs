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

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
