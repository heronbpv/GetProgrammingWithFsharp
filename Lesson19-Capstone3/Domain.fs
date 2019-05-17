namespace Capstone3.Domain

open System

type Customer = { Name : string }
type Account = { AccountId : Guid; Owner : Customer; Balance : decimal }
type Transaction = 
    { Timestamp : DateTime
      Operation : string
      Amount : decimal
      Accepted : bool
      Message : string }

module Transactions = 
    let serialized transaction =
        sprintf "%O***%s***%M***%b***%s" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted transaction.Message