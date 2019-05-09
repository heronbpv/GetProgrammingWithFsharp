namespace Capstone2.Operations

///Functions that operate on the types defined in the domain module.
[<AutoOpen>]
module Operations = 
    open Capstone2.Domain

    ///Deposits a certain amount into an Account.
    let deposit amount account =
        { account with Balance = account.Balance + amount }
    
    ///Withdraws a certain amount from a given Account.
    let withdraw amount account =
        if amount <= account.Balance then { account with Balance = account.Balance - amount }
        else account