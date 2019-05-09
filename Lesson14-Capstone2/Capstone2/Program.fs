// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

//Orchestration
open Capstone2.Domain
open Capstone2.Operations
open Capstone2.Auditing

open System 
///Greets the user and sets up it's account and customer registration
let greeting () = 
    Console.WriteLine("Welcome to your banking system. Please, identify yourself.")
    Console.WriteLine("Name: ")
    let name = Console.ReadLine()
    Console.WriteLine("Age: ")
    let age = int(Console.ReadLine())
    Console.WriteLine("Now, register the initial balance for your account.")
    Console.WriteLine("Bucks: ")
    let balance = decimal(Console.ReadLine())

    let user = {Id = Guid.Empty; Age = age; Name = name}
    {Id = Guid.Empty; Balance = balance; Owner = user}

[<EntryPoint>]
let main _ =    
    let mutable account = greeting()
    while true do
        Console.WriteLine("Greetings, {0}. What would you like to do with your account?", account.Owner.Name)
        Console.WriteLine("Options (inform the number to the desired action):")
        Console.WriteLine("1)Withdraw")
        Console.WriteLine("2)Deposit")
        Console.WriteLine("3)Exit")
        let option = Console.ReadLine()

        //The action inner loop, isolated for further ideation.
        let action (option:string) =
            Console.WriteLine("Action: {0}. How much?", option)
            Console.Write("Bucks: ")
            let amount = decimal(Console.ReadLine())

            account <-
                if option = "1" then account |> withdrawWithConsoleAudit amount
                elif option = "2" then account |> depositWithConsoleAudit amount
                else account
        
        //Using pattern matching a little ahead of time; this way typos and other mistakes don't brake the app so hard.
        match option with
        | "1" | "2" -> action option
        | "3" -> Environment.Exit 0
        | _ -> Console.WriteLine("Invalid command. Try again.")

    0
