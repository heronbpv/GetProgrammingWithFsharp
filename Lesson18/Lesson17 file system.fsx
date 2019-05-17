//@Try this 18
open System.IO
///Gets the file info of all files in a directory and it's subdirectories.
let getAllFilesFromDir path = 
    let files = Directory.EnumerateFiles (path, "*.*", SearchOption.AllDirectories)
    files 
    |> Seq.map (fun file -> (new FileInfo(file)))

//Rules type
type Rule = FileInfo -> bool * string

open System
let rules :Rule list = 
    [fun file -> file.Length <= 800000L, "File size must be less than 800.000 bytes."
     fun file -> file.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase), "File type must be xml."
     fun file -> DateTime.op_LessThan(file.CreationTime, DateTime.Today), "File must have been created before today."] //No, that last rule does't make sense, I know...

//Aggregator, now using reduce instead of fold; adapted from the scratchpad from lesson 18.
let validateReduce (rules:Rule list) = 
    rules
    |> List.reduce (fun rule1 rule2 -> 
                       fun file -> 
                        if file |> isNull then
                            false, "Empty string"
                        else 
                            let passed, error = rule1 file
                            if passed then
                                let passed, error = rule2 file
                                if passed then
                                    true, ""
                                else
                                    false, error
                            else
                                false, error)

//The file info collections used on lesson 17.
let set1 = getAllFilesFromDir @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\obj"
let set2 = getAllFilesFromDir @"D:\Programacao\GetProgrammingWithFsharp\Lesson14-Capstone2\Capstone2\bin"

//A test used to help identify the best function to make a DateTime comparison, to be used on the filters above.
set1 |> Seq.map (fun file -> (file.CreationTime.ToString(), DateTime.UtcNow.ToString()) , DateTime.op_LessThan(file.CreationTime, DateTime.UtcNow))

//Generates the filter as the result of the rules engine processing of the list of rules
let filter = validateReduce rules >> fst //Applies the filters, then extracts the value of the first element of the resulting tuple, which is a boolean.

//Tests
let filteredSet1 = set1 |> Seq.filter filter |> List.ofSeq
let filteredSet2 = set2 |> Seq.filter filter |> List.ofSeq
//let nullSet = null |> Seq.filter filter |> List.ofSeq //Exception thrown by the Seq module function.