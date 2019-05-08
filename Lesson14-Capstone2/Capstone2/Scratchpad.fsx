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
let deposit (amount:decimal) (account:Account) :Account =
    { Id = Guid.Empty; Owner = {Id = Guid.Empty; Name = "Sam"; Age = 25}; Balance = 10M }
let withdraw (amount:decimal) (account:Account) :Account =
    { Id = Guid.Empty; Owner = {Id = Guid.Empty; Name = "Sam"; Age = 25}; Balance = 10M }