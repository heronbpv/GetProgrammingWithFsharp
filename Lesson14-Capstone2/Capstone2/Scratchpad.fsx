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