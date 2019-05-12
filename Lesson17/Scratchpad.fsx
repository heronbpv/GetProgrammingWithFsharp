//@Dictionary
//Tradicional, BCL, mutable version.
open System.Collections.Generic
let inventory = Dictionary<string, float>()

inventory.Add("Apples", 0.33)
inventory.Add("Oranges", 0.23)
inventory.Add("Bananas", 0.45)

inventory

inventory.Remove "Oranges"

inventory

let bananas = inventory.["Bananas"]
let oranges = inventory.["Oranges"] //Throws an exception -> KeyNotFound

//Declaring an immutable dictionary.
let inv2:IDictionary<string, float> = 
    ["Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45]
    |> dict

let bananas2 = inv2.["Bananas"]
inv2.Add("Pineapples", 0.85)
inv2.Remove("Bananas") //These last two throw exceptions.

//Neat little trick: initializing a dictionary using dict to avoid the create then fill pattern of the BCL type.
["Apples", 10; "Bananas", 20; "Grapes", 15] |> dict |> Dictionary
