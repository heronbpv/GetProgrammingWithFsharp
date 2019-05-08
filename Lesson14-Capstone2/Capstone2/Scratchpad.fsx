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

//Audit functions
open System.IO
let fileSystemAudit account message = 
    let path = Path.Combine (Directory.GetParent(__SOURCE_DIRECTORY__).FullName, account.Owner.Name)
    let dir = Directory.CreateDirectory (path)
    let path = Path.Combine (dir.FullName, (account.Id.ToString() + ".txt")) //Works, because of shadowing. Careful!
    File.WriteAllText(path, message)
let consoleAudit account message = 
    printfn "Account %s: %s" (account.Id.ToString()) message

printfn "%s" (__SOURCE_DIRECTORY__ + Path.DirectorySeparatorChar.ToString() +
                        ".." +
                        Path.DirectorySeparatorChar.ToString() +
                        "capstoneAudit" +
                        Path.DirectorySeparatorChar.ToString() +
                        bobAccount.Owner.Name +
                        Path.DirectorySeparatorChar.ToString() +
                        bobAccount.Id.ToString() + ".txt")

fileSystemAudit bobAccount "Testing audit on bobaccount with file"
consoleAudit bobAccount "Testing audit on bobaccount with console"