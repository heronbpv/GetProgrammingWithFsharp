//Mapping functions -> convert (map) a collection of a certain type to a collection of another type. 
//Sometimes, the item type may be referred to as the item shape, hence calling these shapeshifting functions seems cool as fuck.
//@Map: ('T -> 'U) -> 'T list -> 'U list; LINQ: Select; Input and output collection sizes are the same.
let numbers = [ 1..10 ]
let timesTwo n = n * 2
let outputImperative = ResizeArray() //Example imperative ideia: creates a new collection based on a transformation of the elements of another.

for number in numbers do
    outputImperative.Add(number |> timesTwo)
outputImperative

let outputFunctional = numbers |> List.map timesTwo //Equivalent functional ideia.

//@Iter: ('T -> unit) -> 'T list -> unit; LINQ: N/A; Applies side-effect to each element. Equivalent to a for each loop.
let someList = 
    [ ("Isaac", "London")
      ("Sara", "Birmingham")
      ("Tim", "London")
      ("Michelle", "Manchester") ]

//Imperative way to print all names: foreach loop
for (name, _) in someList do
    printfn "Hello, %s" name
//Functional way
someList |> List.iter (fun (name, _) -> printfn "Hello, %s" name)

//@Collect: ('T -> 'U list) -> 'T list -> 'U list; LINQ: SelectMany; Solves many-to-many relationships, by treating the results as a flat collection.
//From the book: "It takes in a list of items, and a function that returns a new collection from each item... and then merges them all back into a single list".
//Example: return all  orders for a certain set of customers
type Order = 
    { OrderId : int }

type Customer = 
    { CustomerId : int
      Orders : Order list
      Town : string }

let customers = 
    [ { CustomerId = 1
        Town = "London"
        Orders = 
            [ { OrderId = 1 }
              { OrderId = 2 } ] }
      { CustomerId = 2
        Town = "Vladvostok"
        Orders = [ { OrderId = 39 } ] }
      { CustomerId = 5
        Town = "Berlim"
        Orders = 
            [ { OrderId = 43 }
              { OrderId = 56 }
              { OrderId = 57 } ] } ]

//With a map, the result is a list of lists (see signature)
let ordersMap = customers |> List.map (fun c -> c.Orders)
//With collect, the list of lists is effectively flattened. This makes it easier to continue operations through a pipeline, since a flat list is easier to deal with
let ordersCollect = customers |> List.collect (fun c -> c.Orders)

//@Pairwise: 'T list -> ('T * 'T) list; LINQ: N/A; Specialized version of the Windowed function, for a pair. Think of it as a hovering window passing through.
//Useful to calculate the "distance" between items in a ordered collection.
//Example: calculate the number of days transpired, from a list of dates
open System

let dates = 
    [ DateTime(2010, 5, 1)
      DateTime(2010, 6, 1)
      DateTime(2010, 6, 12)
      DateTime(2010, 7, 3) ]

dates
|> List.pairwise //Puts up a window of size 2 over the list, pass trhough it, then returns the windows
|> List.map (fun (a, b) -> b - a) //Take each window, and calculate the difference between it's elements, thus obtaining the transpired days for this pair
|> List.map (fun time -> time.TotalDays) //Since the diference is in a timespan type, first extract the total days to a new collection
|> List.sum //Then finish by aggregating the number of days for a total
//Grouping functions -> perform a logic grouping of data.
//@GroupBy: ('T -> 'Key) -> 'T list -> ('Key * 'T list) list; LINQ: GroupBy; 
someList |> List.groupBy (fun (_, city) -> city)
//@CountBy: ('T -> 'Key) -> 'T list -> ('Key * int) list; LINQ: N/A; Like groupBy, but returns an aggregate as the second element of the return pair
someList |> List.countBy (fun (_, city) -> city)

//@Partition: ('T -> bool) -> 'T list -> ('T list * 'T list); LINQ: N/A; Splits the collection in two, based on a predicate. First half is the true returning one.
let londonCustomers, otherCustomers = someList |> List.partition (fun (_, city) -> city = "London")

//@Try this 16:
//First part: list all subfolders names and their sizes.
open System.IO
let listAllSubfoldersIfAny path =
    let dirInfo = new DirectoryInfo(path)
    let subDirs = dirInfo.GetDirectories()
    subDirs
    |> Array.map (fun dirInfo -> dirInfo.Name, dirInfo.GetFiles() |> Array.sumBy (fun file -> file.Length))
    |> Array.sortByDescending (fun (_,lenght) -> lenght)

listAllSubfoldersIfAny "D:\Programacao\GetProgrammingWithFsharp\Lesson16"
listAllSubfoldersIfAny "D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"

//Second part: create a data structure to hold some information about the folder and present it instead
type FolderData = 
    { Name : string
      Size : int64
      NumberOfFiles : int
      AvgFileSize : float
      Extensions : string list }
//Create a creation function for the type
let create name size nof avg exts = 
    {Name = name; Size = size; NumberOfFiles = nof; AvgFileSize = avg; Extensions = exts}

///Calculates the size of the directory, including the size of it's subdirectories.
let rec calculateSize (dirInfo:DirectoryInfo) = 
    let totalFromLocal = 
        dirInfo.GetFiles()
        |> Array.sumBy (fun file -> file.Length)
    let totalFromSubs =
        dirInfo.GetDirectories()
        |> Array.map (fun dir -> dir.FullName)
        |> Array.sumBy (fun name -> (calculateSize (new DirectoryInfo(name))))
    totalFromLocal + totalFromSubs

calculateSize (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson16"))
calculateSize (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"))

///Gets the number of files from the directory.
let countFilesInFolder (dirInfo:DirectoryInfo) = 
    dirInfo.GetFiles().Length

countFilesInFolder (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson16"))
countFilesInFolder (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"))

///Calculates the average size of files in the directory.
let calculateAvgFileSize (dirInfo:DirectoryInfo) = 
    let files = dirInfo.GetFiles()
    if files |> Array.isEmpty then
        0.0
    else
        files |> Array.averageBy (fun file -> file.Length |> float)

calculateAvgFileSize (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson16"))
calculateAvgFileSize (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"))

///Lists all file extensions in use inside the directory.
let getExtensions (dirInfo:DirectoryInfo) = 
    dirInfo.GetFiles()
    |> Array.map (fun file -> file.Extension)
    |> Array.distinct
    |> Array.toList

getExtensions (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson16"))
getExtensions (new DirectoryInfo("D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"))

let initFolderData path = 
    let dirInfo = new DirectoryInfo(path)
    create dirInfo.Name (calculateSize dirInfo) (countFilesInFolder dirInfo) (calculateAvgFileSize dirInfo) (getExtensions dirInfo)

initFolderData "D:\Programacao\GetProgrammingWithFsharp\Lesson16"
initFolderData "D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2"