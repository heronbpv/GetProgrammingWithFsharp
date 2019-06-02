#load @"Scripts/load-project-debug.fsx"

open Capstone5.Operations
open Capstone5.Domain
open System
open Newtonsoft.Json

let txn = { Operation = "withdraw"; Amount = 100M; Timestamp = DateTime.UtcNow }
let serialized = txn |> JsonConvert.SerializeObject
let deserialized = JsonConvert.DeserializeObject<Transaction> serialized