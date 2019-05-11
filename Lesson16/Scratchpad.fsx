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

//