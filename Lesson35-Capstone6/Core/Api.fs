/// Provides access to the banking API.
module Capstone6.Api

open Capstone6.Domain
open Capstone6.Operations
open System

type IBankApi = 
    /// Loads an account from disk. If no account exists, an empty one is automatically created.
    abstract member LoadAccount : customer:Customer -> RatedAccount
    /// Deposits funds into an account.
    abstract member Deposit : amount:Decimal -> customer:Customer -> RatedAccount
    /// Withdraws funds from an account that is in credit.
    abstract member Withdraw : amount:Decimal -> customer:Customer -> RatedAccount
    /// Loads the transaction history for an owner.
    abstract member LoadTransactionHistory : customer:Customer -> Transaction seq

let private buildApi getAccountAndTransactions writeTransaction = 
    { new IBankApi with
          member this.LoadAccount(customer) = 
            customer.Name
            |> getAccountAndTransactions 
            |> Option.map (Operations.buildAccount customer.Name)
            |> defaultArg <|
                InCredit(CreditAccount { AccountId = Guid.NewGuid()
                                         Balance = 0M
                                         Owner = customer })
          member this.Deposit amount customer = 
            let ratedAccount = this.LoadAccount customer
            let accountId = ratedAccount.GetField (fun a -> a.AccountId)
            let owner = ratedAccount.GetField(fun a -> a.Owner)
            auditAs Deposit writeTransaction deposit amount ratedAccount accountId owner
          member this.Withdraw amount customer = 
            let account = this.LoadAccount customer 
            match account with
            | InCredit (CreditAccount account as creditAccount) -> 
                auditAs Withdraw writeTransaction withdraw amount creditAccount account.AccountId account.Owner
            | account -> 
                account
          member this.LoadTransactionHistory(customer) = 
            customer.Name
            |> getAccountAndTransactions 
            |> Option.map(fun (_,txns) -> txns)
            |> defaultArg <| Seq.empty
    }

/// Creates a SQL-enabled API.
let CreateSqlApi connectionString = 
    buildApi
        (SqlRepository.getAccountAndTransactions connectionString)
        (SqlRepository.writeTransaction connectionString)

/// Gets a handle to the file-based API.
let CreateFileApi = 
    buildApi
        (FileRepository.tryFindTransactionsOnDisk)
        (FileRepository.writeTransaction)