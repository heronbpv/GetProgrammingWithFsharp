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

//@Map (the collection type, not the function)
//This collection is immutable by default, and has support for some of the F# collection operations (map, filter, partition, etc.)
let inv3 = ["Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45] |> Map.ofList
let apples = inv3.["Apples"]
let pineapples = inv3.["Pineapples"] //Throws an exception -> KeyNotFound

let newInv3 = 
    inv3
    |> Map.add "Pineapples" 0.87
    |> Map.remove "Apples"

let cheapFruit, expensiveFruit = 
    newInv3
    |> Map.partition (fun fruit cost -> cost < 0.3) //The arguments are the key -> value pair of the map, curried thanks to the predicate definition.

//@Now you try 17.2.1
open System.IO
let listOfDirsOnPartition partition = 
    let directories = Directory.EnumerateDirectories partition
    directories 
    |> Seq.map ((fun path -> new DirectoryInfo(path)) >> (fun dirInfo -> dirInfo.Name, dirInfo.CreationTimeUtc)) //I don't think this is as clear as simply two separate maps, Linter...
    |> Map.ofSeq
    |> Map.map (fun dirName dirDtUtc -> (System.DateTime.UtcNow - dirDtUtc).TotalDays)

listOfDirsOnPartition @"C:\" //Remember to put the @ symbol before path strings to ensure they work properly.

//@Set
//Represent the mathematical notion of sets, including it's operations.
//Sets are collections of distinct values, so repetitions are removed by default
let myBasket = ["Apples"; "Apples"; "Apples"; "Oranges"; "Bananas"; "Pineapples"] |> Set.ofList
let anotherBasket = ["Kiwi"; "Bananas"; "Grapes"] |> Set.ofList
let fruitsInAllBaskets = myBasket + anotherBasket //Union operation.
let fruitsOnlyInMyBasket = myBasket - anotherBasket
let fruitsOnlyInAnotherBasket = anotherBasket - myBasket //Difference operations, one for each side; results differ, so careful!
let fruitsOnBothBaskets = myBasket |> Set.intersect anotherBasket //Intersection operation.
let isAnotherBasketASubsetOfMyOwn = myBasket |> Set.isSubset anotherBasket //Subset operation.

//@Try this 17
open System.IO
///Gets the file info of all files in a directory and it's subdirectories.
let getAllFilesFromDir path = 
    let files = Directory.EnumerateFiles (path, "*.*", SearchOption.AllDirectories)
    files 
    |> Seq.map (fun file -> (new FileInfo(file)))

///Creates a set of all file types within a folder
let createFileTypeSet path =
    getAllFilesFromDir path
    |> Seq.map (fun file -> file.Extension)
    |> Set.ofSeq

//Create two sets of file types from different folders
let set1 = createFileTypeSet @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\obj"
let set2 = createFileTypeSet @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\bin"

//Now find which types are used between them
set1 |> Set.intersect set2