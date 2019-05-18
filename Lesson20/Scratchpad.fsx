open System
let arrayOfChars = [| for c in 'a'..'z' -> Char.ToUpper c |]
let arrayOfChars2:Char[] = //Using for..in..do doesn't work here, the syntax is different. Also, lot's of warnings and errors for code that doesn't generate an array.
    [| for c in 'a'..'z' do 
           let x = Char.ToUpper c
           x |> ignore |]
let listOfSquares = [ for i in 1..10 -> i*i ]
let seqOfStrings = seq { for i in 2..4..20 -> sprintf "Number %d" i } //Arbitrary seq of strings based on every fourth number between 2 and 20.
seqOfStrings |> List.ofSeq //Using List.ofSeq to evaluate the sequence; remember that sequences are lazy by default.

//@Now you try 20.2.3
let getCreditLimit customer = 
    match customer with
    //| _, 1 -> 250
    | "medium", 1 -> 500
    | "good", 0 | "good", 1 -> 750
    | "good", 2 -> 1000
    | "good", _ -> 2000
    //| _, 2 -> 250
    | _ -> 250

let limit1 = getCreditLimit ("medium", 1)
let failure = getCreditLimit ("bad",  -1) //Will throw an exception with the last match commented.

//@Now you try 20.3.1
type Customer = { Balance:int; Name:string }

//If-then-else version of the function
let handleCustomers customers = 
    if List.isEmpty customers then failwith "No customers supplied." //Interestingly, after the call for List.isEmpty, the compiler infers the argument to be a list...
    elif customers.Length = 1 then printfn "Customer name: %s" (customers |> List.head).Name //... so, calling the Length property here didn't raise a compiler error. Nice!
    elif customers.Length = 2 then printfn "Balance: %d" (customers |> List.sumBy (fun customer -> customer.Balance))
    else printfn "Customers: %d" customers.Length

//Some tests
let customers = //A list of three customers to use on the tests below; This amount will make it easy to manipulate the length using head and tail operations.
    [ { Balance = 10; Name = "Alice" }
      { Balance = 20; Name = "Bob" }
      { Balance = 30; Name = "Cali" } ]
//let ``An empty list throws an exception`` = handleCustomers []
let ``A list of length 1 prints the customer's name`` = customers |> List.head |> List.singleton |> handleCustomers //List.head returns the element, so I use singleton to "enlist" it. '-'
let ``A list of length 2 prints the total balance between customers`` = customers |> List.tail |> handleCustomers //Since this list has three elements, tail returns a list of two.
let ``A list of any other length returns the number of customers`` = handleCustomers customers 

//Pattern matching version of the function. Notice how the bidding on the match clauses facilitate the overall function usage.
let handleCustomers customers = 
    match customers with
    | [] -> failwith "No customers supplied."
    | [customer] -> printfn "Customer name: %s" customer.Name
    | [first; second] -> printfn "Balance: %d" (first.Balance + second.Balance)
    | customers -> printfn "Customers: %d" customers.Length  //Rerunning the tests above return the same results in fsi.