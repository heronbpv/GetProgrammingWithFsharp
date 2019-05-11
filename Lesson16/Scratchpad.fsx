//Mapping functions -> convert (map) a collection of a certain type to a collection of another type. 
//Sometimes, the item type may be referred to as the item shape, hence calling these shapeshifting functions seems cool as fuck.

//@Map: Signature ('T -> 'U) -> 'T list -> 'U list; LINQ equivalent: Select; Obs.: Input and output collection sizes are the same.
let numbers = [1..10]
let timesTwo n = n * 2
let outputImperative = ResizeArray() //Example imperative ideia: creates a new collection based on a transformation of the elements of another.
for number in numbers do
    outputImperative.Add (number |> timesTwo)
outputImperative
let outputFunctional = numbers |> List.map timesTwo //Equivalent functional ideia.

//@Iter: Signature ('T -> unit) -> 'T list -> unit; LINQ equivalent: N/A; Obs.: Applies side-effect to each element. Equivalent to a for each loop.
let someList =[("Isaac", "London"); ("Sara", "Birmingham"); ("Tim", "London"); ("Michelle", "Manchester")]

//Imperative way to print all names: foreach loop
for (name, _) in someList do
    printfn "Hello, %s" name
//Functional way
someList
|> List.iter (fun (name, _) -> printfn "Hello, %s" name)

//@Collect: Signature ('T -> 'U list) -> 'T list -> 'U list; LINQ equivalent: SelectMany; Obs.: Solves many-to-many relationships, by treating the results as a flat collection.
//From the book: "It takes in a list of items, and a function that returns a new collection from each item... and then merges them all back into a single list".
//Example: return all  orders for a certain set of customers
type Order = {OrderId:int}
type Customer = {CustomerId:int; Orders:Order list; Town:string}
let customers = 
    [{CustomerId = 1; Town = "London"; Orders = [{OrderId=1}; {OrderId=2}]}
     {CustomerId = 2; Town = "Vladvostok"; Orders = [{OrderId=39}]}
     {CustomerId = 5; Town = "Berlim"; Orders = [{OrderId=43}; {OrderId=56}; {OrderId=57}]}]
//With a map, the result is a list of lists (see signature)
let ordersMap = customers |> List.map (fun c -> c.Orders)
//With collect, the list of lists is effectively flattened. This makes it easier to continue operations through a pipeline, since a flat list is easier to deal with
let ordersCollect = customers |> List.collect (fun c -> c.Orders)