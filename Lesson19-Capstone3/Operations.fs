module Capstone3.Operations

open System
open Capstone3.Domain

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount account =
    let message = sprintf "\r\nPerforming a %s operation for R$%M... Transaction not yet accepted.\r\n" operationName amount
    let transaction = { Timestamp = DateTime.UtcNow; Operation = operationName; Amount = amount; Accepted = false; Message = message }
    let audit = audit account.AccountId account.Owner.Name
    audit transaction
    let updatedAccount = operation amount account
    
    let accountIsUnchanged = (updatedAccount = account)

    if accountIsUnchanged then 
        let message = sprintf "\r\nTransaction rejected!\r\n"
        let transaction = { transaction with Timestamp = DateTime.UtcNow; Message = message }
        audit transaction
    else 
        let message = sprintf "\r\nTransaction accepted! Balance is now R$%M.\r\n" updatedAccount.Balance
        let transaction = { transaction with Timestamp = DateTime.UtcNow; Accepted = true; Message = message }
        audit transaction

    updatedAccount

//let loadAccount ownerName accountId (transactions:Transaction list) = 
    