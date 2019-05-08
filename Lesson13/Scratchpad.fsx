//@Now you try 13.2.1
type Customer = 
    { Age : int }

let where filter customers = 
    seq { 
        for customer in customers do
            if filter customer then yield customer
    }

let customers = 
    [ { Age = 21 }
      { Age = 35 }
      { Age = 36 } ]

let isOver35 customer = customer.Age > 35

customers |> where isOver35
customers |> where (fun customer -> customer.Age > 35)

//@Now you try 13.3
open System

let printCustomerAge writer customer = 
    if customer.Age < 12 then writer "Child!"
    elif customer.Age >= 12 && customer.Age < 18 then writer "Teenager!"
    else writer "Adult!"

let customers2 = 
    [ { Age = 8 }
      { Age = 17 }
      { Age = 36 } ]

customers2 |> List.iter (printCustomerAge Console.WriteLine)

open System.IO

let writeToFile text = 
    let path = Path.Combine(__SOURCE_DIRECTORY__, "output.txt")
    File.WriteAllText(path, text)

let printToFile = printCustomerAge writeToFile

printToFile { Age = 21 }
let outputed = File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "output.txt"))