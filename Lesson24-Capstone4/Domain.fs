namespace Capstone4.Domain

open System

type Customer = { Name : string }
type Account = { AccountId : Guid; Owner : Customer; Balance : decimal }
type Transaction = { Timestamp : DateTime; Operation : string; Amount : decimal; }
type CreditAccount = CreditAccount of Account
type RatedAccount = 
    | InCredit of CreditAccount
    | Overdrawn of Account

module Transactions =
    /// Serializes a transaction
    let serialize transaction =
        sprintf "%O***%s***%M" transaction.Timestamp transaction.Operation transaction.Amount
    
    /// Deserializes a transaction
    let deserialize (fileContents:string) =
        let parts = fileContents.Split([|"***"|], StringSplitOptions.None)
        { Timestamp = DateTime.Parse parts.[0]
          Operation = parts.[1]
          Amount = Decimal.Parse parts.[2] }