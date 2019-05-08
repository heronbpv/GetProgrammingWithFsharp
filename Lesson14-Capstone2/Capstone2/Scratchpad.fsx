//Type definitions that model the problem.
open System

type Customer = 
    { Id : Guid
      Name : string
      Age : int }

type Account = 
    { Id : Guid
      Balance : decimal
      Owner : Customer }

let bob = {Id = Guid.NewGuid(); Name = "Bob"; Age = 21}
let jane = {Id = Guid.NewGuid(); Name = "Jane"; Age = 29}
let bobAccount = {Id = Guid.NewGuid(); Balance = 100M; Owner = bob}
let janeAccount = {Id = Guid.NewGuid(); Balance = 70M; Owner = jane}

//Functions that operate on said types
let deposit amount account =
    { account with Balance = account.Balance + amount }
let withdraw amount account =
    if amount <= account.Balance then { account with Balance = account.Balance - amount }
    else account

//Testing the functions
janeAccount |> deposit 50M |> withdraw 25M |> deposit 10M
bobAccount |> withdraw 100M |> withdraw 10M |> deposit 25M |> withdraw 175M

//Audit functions
open System.IO
let fileSystemAudit account message = 
    let path = Path.Combine (Directory.GetParent(__SOURCE_DIRECTORY__).FullName, account.Owner.Name)
    let dir = Directory.CreateDirectory (path)
    let path = Path.Combine (dir.FullName, (account.Id.ToString() + ".txt")) //Works, because of shadowing. Careful!
    File.AppendAllText(path, (message + "\n"))
let consoleAudit account message = 
    printfn "Account %s: %s" (account.Id.ToString()) message

printfn "%s" (__SOURCE_DIRECTORY__ + Path.DirectorySeparatorChar.ToString() +
                        ".." +
                        Path.DirectorySeparatorChar.ToString() +
                        "capstoneAudit" +
                        Path.DirectorySeparatorChar.ToString() +
                        bobAccount.Owner.Name +
                        Path.DirectorySeparatorChar.ToString() +
                        bobAccount.Id.ToString() + ".txt")

fileSystemAudit bobAccount "Testing audit on bobaccount with file"
consoleAudit bobAccount "Testing audit on bobaccount with console"

//Orquestration
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

bobAccount
|> depositWithConsoleAudit 100M
|> withdrawWithConsoleAudit 50M
|> withdrawWithConsoleAudit 150M
|> withdrawWithConsoleAudit 75M
|> depositWithConsoleAudit 80M
|> withdrawWithConsoleAudit 60M

bobAccount
|> depositWithFileAudit 100M
|> withdrawWithFileAudit 50M
|> withdrawWithFileAudit 150M
|> withdrawWithFileAudit 75M
|> depositWithFileAudit 80M
|> withdrawWithFileAudit 60M