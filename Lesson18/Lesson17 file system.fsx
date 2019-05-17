//@Try this 18
open System.IO
///Gets the file info of all files in a directory and it's subdirectories.
let getAllFilesFromDir path = 
    let files = Directory.EnumerateFiles (path, "*.*", SearchOption.AllDirectories)
    files 
    |> Seq.map (fun file -> (new FileInfo(file)))

//Rules type
type Rule = FileInfo -> bool * string

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