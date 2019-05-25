#load "Domain.fs"
#load "Operations.fs"

open Capstone4.Operations
open Capstone4.Domain
open System

///Represents the commands avaiable to the user.
type Command = 
    | Withdraw
    | Deposit
    | Exit

///Parses a given char to one of the valid commands. Values accepted: (w)ithdraw, (d)eposit, e(x)it
let tryParseCommand = function
    | 'w' -> Some Withdraw
    | 'd' -> Some Deposit
    | 'x' -> Some Exit
    | _   -> None

tryParseCommand 'd'
tryParseCommand '0'