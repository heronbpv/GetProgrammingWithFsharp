namespace Capstone2.Domain

///Type definitions that model the problem.
[<AutoOpen>]
module Domain = 
    open System
    ///A simple customer.
    type Customer = 
        { Id : Guid
          Name : string
          Age : int }
    ///The account associated with a customer.
    type Account = 
        { Id : Guid
          Balance : decimal
          Owner : Customer }