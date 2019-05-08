//@Now you try 13.2.1
type Customer = 
    { Age : int }

let where filter customers = 
    seq {
        for customer in customers do
            if filter customer then
                yield customer
    }
let customers = [ {Age = 21}; {Age = 35}; {Age = 36} ]
let isOver35 customer = customer.Age > 35
customers |> where isOver35
customers |> where (fun customer -> customer.Age > 35)

//@Now you try 13.3
open System
let printCustomerAge customer = 
    if customer.Age < 12 then Console.WriteLine("Child!")
    elif customer.Age >= 12 && customer.Age < 18 then Console.WriteLine("Teenager!")
    else Console.WriteLine("Adult!")

let customers2 = [{Age = 8}; {Age = 17}; {Age = 36}]
customers2 |> List.iter printCustomerAge