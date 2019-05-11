//Mapping functions -> convert (map) a collection of a certain type to a collection of another type. 
//Sometimes, the item type may be referred to as the item shape, hence calling these shapeshifting functions seems cool as fuck.

//@Map: Signature ('T -> 'U) -> 'T list -> 'U list; LINQ equivalent: Select; Input and output collection sizes are the same
let numbers = [1..10]
let timesTwo n = n * 2
let outputImperative = ResizeArray() //Example imperative ideia.
for number in numbers do
    outputImperative.Add (number |> timesTwo)
outputImperative
let outputFunctional = numbers |> List.map timesTwo //Equivalent functional ideia.