//Introducing options (a.k.a. maybe monads)
let aNumber :int = 10
let maybeANumber :int option = Some 10

let calculateAnnualPremiumUsd = function //Despite the warning, all bases are covered... I think!
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None -> 
        printfn "No score supplied! Using temporary premium."
        300

let some = calculateAnnualPremiumUsd (Some 250)
let none = calculateAnnualPremiumUsd None
let maybe = calculateAnnualPremiumUsd maybeANumber
//let error = calculateAnnualPremiumUsd aNumber //Passing a non-optional value raises a compiler error.

//@Now you try 22.2.2
type Customer = { Name:string; Score:int option }
let customers = [ { Name = "Alice"; Score = None }; { Name = "Bob"; Score = Some 10 } ]
let calculateAnnualPremiumUsdFromCustomer (customer:Customer) = 
    match customer.Score with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None -> 
        printfn "No score supplied for '%s'! Using temporary premium." customer.Name
        300

let results = customers |> List.iter (fun customer -> printfn "Customer: %s; Score: %d" customer.Name (calculateAnnualPremiumUsdFromCustomer customer))

//Option module hofs
//Option map
let describe score = 
    match score with
    | 0 -> "Low Risk"
    | score when score < 0 -> "Safe"
    | score when score > 0 -> "High Risk"
    | _ -> "Low Risk"
   
let description customer = 
    match customer.Score with
    | Some score -> Some (describe score)
    | None -> None

let description2 customer = customer.Score |> Option.map describe
let optionalDescribe = Option.map describe

let results2 = customers |> List.map description2
let results3 = customers |> List.map (fun customer -> optionalDescribe customer.Score)
let results4 = customers |> List.map description //These functions all do the same thing.

//Option bind
//The book references some drivers.[0] value; no idea from where, so here's a default nonsense implementation. Also, the type is a Customer, somehow...
let tryFindCustomer cId = if cId = 10 then (Some { Name = "Driver zero guy"; Score = Some 10 }) else None 
let getSafetyScore customer = customer.Score
let score = tryFindCustomer 10 |> Option.bind getSafetyScore
let scoreMap = tryFindCustomer 10 |> Option.map getSafetyScore //Testing with map; this creates an Some (Some 10) as a result; so, bind actually flattened the result to Some 10 only.

//Option filter
let test1 = Some 5 |> Option.filter (fun x -> x > 5)
let test2 = Some 5 |> Option.filter (fun x -> x = 5)

//@Now you try 22.4.2
let tryLoadCustomer cId =
    if cId > 2 && cId < 7 then Some (sprintf "Customer %d" cId)
    else None
let customerIds = [1..10]
customerIds |> List.choose tryLoadCustomer

//@Try this 22
open System
open System.IO
let fileLoader path = 
    if File.Exists path then Some (FileInfo(path))
    else None

let displayFileInfo path = 
    let optionFile = fileLoader path
    match optionFile with
    | Some file -> sprintf "File name: %s;\r\n Size: %d; Parent: %s; Extension: %s" file.Name file.Length file.DirectoryName file.Extension
    | None -> "No file found."

displayFileInfo @"D:\cyruz\Livros\Oficiais\[White Paper]Machine Learning at Microsoft with ML.NET.pdf"
displayFileInfo @"D:\Programacao\GetProgrammingWithFsharp\Lesson22\Quick Checks.txt"

//Code ported from the Try this 18 Lesson18 exercise, file "Lesson17 file system.fsx". Original comments removed for brevity.
let getAllFilesFromDir path = 
    let files = Directory.EnumerateFiles (path, "*.*", SearchOption.AllDirectories)
    files 
    |> Seq.map (fun file -> (new FileInfo(file)))
type Rule = FileInfo -> bool * string option //Rule type updated to accept string option. All modifications necessary cascade from this change.
let rules :Rule list = 
    [fun file -> file.Length <= 800000L, Some "File size must be less than 800.000 bytes."
     fun file -> file.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase), Some "File type must be xml."
     fun file -> DateTime.op_LessThan(file.CreationTime, DateTime.Today), Some "File must have been created before today."]
let validateReduce (rules:Rule list) = 
    rules
    |> List.reduce (fun rule1 rule2 -> 
                       fun file -> 
                        if file |> isNull then
                            false, Some "Empty string"
                        else 
                            let passed, error = rule1 file
                            if passed then
                                let passed, error = rule2 file
                                if passed then
                                    true, None
                                else
                                    false, error
                            else
                                false, error)
let set1 = getAllFilesFromDir @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\obj"
let set2 = getAllFilesFromDir @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\bin"
let filter = validateReduce rules >> fst 

//Tests
let filteredSet1 = set1 |> Seq.filter filter |> List.ofSeq
let filteredSet2 = set2 |> Seq.filter filter |> List.ofSeq
//let nullSet = null |> Seq.filter filter |> List.ofSeq  //Results are the same as before.