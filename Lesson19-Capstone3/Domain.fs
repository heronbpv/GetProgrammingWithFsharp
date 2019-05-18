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
    ///Serializes a transaction.
    let serialize transaction =
        sprintf "%O***%s***%M***%b***%s" transaction.Timestamp transaction.Operation transaction.Amount transaction.Accepted transaction.Message
    
    ///Deserializes to a transaction, or throws an exception if the string is null or empty, or has an incorrect format.
    let deserialize (text:string) = 
        if not (String.IsNullOrEmpty text) then
            let components = text.Split([|"***"|], System.StringSplitOptions.None)
            if components.Length = 5 then
                { Timestamp = Convert.ToDateTime(components.[0]); Operation = components.[1]; Amount = decimal(components.[2]); Accepted = (components.[3] = "true"); Message = components.[4] }
            else
                failwith "Invalid string."
        else
            failwith "String is empty."