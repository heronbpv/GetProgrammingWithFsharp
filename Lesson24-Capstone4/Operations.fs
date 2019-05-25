module Capstone4.Operations

open System
open Capstone4.Domain

///Represents the possible bank operations supported by the system.
type BankOperation = 
    | Withdraw
    | Deposit

let tryParseBankOperation = function
    | "withdraw" -> Some Withdraw
    | "deposit" -> Some Deposit
    | _ -> None

let classifyAccount account =
    if account.Balance >= 0M then (InCredit(CreditAccount account))
    else Overdrawn account

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount (CreditAccount account) =
    { account with Balance = account.Balance - amount }
    |> classifyAccount

/// Deposits an amount into an account
let deposit amount account =
    let account =
        match account with
        | InCredit (CreditAccount account) -> account
        | Overdrawn account -> account
    { account with Balance = account.Balance + amount }
    |> classifyAccount

let withdrawSafe amount ratedAccount =
    match ratedAccount with
    | InCredit account -> 
        account |> withdraw amount
    | Overdrawn _ ->
        ratedAccount

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount account =
    let updatedAccount = operation amount account
    let account = //Assumes that the operations are now over RatedAccount types; Process them, then safely unwraps for access of the original account type.
        match updatedAccount with
        | InCredit (CreditAccount account) -> 
            account
        | Overdrawn account ->
            account
        
    let transaction = { Operation = operationName; Amount = amount; Timestamp = DateTime.UtcNow }
    
    audit account.AccountId account.Owner.Name transaction
    updatedAccount

/// Creates an account from a historical set of transactions
let loadAccount (owner, accountId, transactions) =
    let openingAccount = { AccountId = accountId; Balance = 0M; Owner = { Name = owner } }
    
    transactions
    |> Seq.sortBy(fun txn -> txn.Timestamp)
    |> Seq.fold(fun account txn ->
        let operation = tryParseBankOperation txn.Operation
        match operation with
        | Some Withdraw -> account |> withdrawSafe txn.Amount
        | Some Deposit -> account |> deposit txn.Amount
        | None -> account //In case of an illegal operation, ignore it for now.
        
        (*if txn.Operation = "withdraw" then account |> withdraw txn.Amount
        else account |> deposit txn.Amount*)) 
        (classifyAccount openingAccount)