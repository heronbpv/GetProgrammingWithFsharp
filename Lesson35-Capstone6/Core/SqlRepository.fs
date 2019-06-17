﻿module internal Capstone6.SqlRepository

open Capstone6.Domain
open FSharp.Data
open System

[<AutoOpen>]
module private DB =
    let [<Literal>] Conn = "Name=AccountsDb"
    type AccountsDb = SqlProgrammabilityProvider<Conn>
    type GetAccountId = SqlCommandProvider<"SELECT TOP 1 AccountId FROM dbo.Account WHERE Owner = @owner", Conn, SingleRow = true>
    type FindTransactions = SqlCommandProvider<"SELECT Timestamp, OperationId, Amount FROM dbo.AccountTransaction WHERE AccountId = @accountId", Conn>
    type FindTransactionsByOwner = SqlCommandProvider<"SELECT a.AccountId, at.Timestamp, at.OperationId, at.Amount FROM dbo.Account a LEFT JOIN dbo.AccountTransaction at on a.AccountId = at.AccountId WHERE Owner = @owner", Conn>
    type DbOperations = SqlEnumProvider<"SELECT Description, OperationId FROM dbo.Operation", Conn>


let getAccountAndTransactions (owner:string) : (Guid * Transaction seq) option =
    let transactionsByOwner = FindTransactionsByOwner.Create(Conn).Execute(owner) |> Seq.toList
    match transactionsByOwner with
    | [] -> 
        None
    | [ transaction ] when (transaction.Amount, transaction.OperationId, transaction.Timestamp) = (None, None, None) ->
        Some (transaction.AccountId, seq [])
    | x :: xs ->
        let transactions = 
            let getOperation operationId = 
                match operationId with
                | DbOperations.Withdraw -> Withdraw
                | DbOperations.Deposit -> Deposit
                | x -> failwith (sprintf "Invalid operation Id '%i' from database." x)
            seq {
                yield { Timestamp = defaultArg x.Timestamp DateTime.MinValue
                        //The book mentions hardcoding the operation as deposit, instead of converting like here, so assume the default to be 2 here.
                        Operation = getOperation (defaultArg x.OperationId 2) 
                        Amount = defaultArg x.Amount 0M }
                for transaction in xs do
                    yield { Timestamp = defaultArg transaction.Timestamp DateTime.MinValue
                            Operation = getOperation (defaultArg transaction.OperationId 2)
                            Amount = defaultArg transaction.Amount 0M }
            }
        Some (x.AccountId, transactions)

let writeTransaction (accountId:Guid) (owner:string) (transaction:Transaction) =
    ()