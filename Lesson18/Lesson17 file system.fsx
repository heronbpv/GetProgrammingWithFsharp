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
    [fun file -> file.Length <= 100L, "File size must be less than 100KB."
     fun file -> file.Extension = ".txt", "File type must be txt."
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

set1 |> Seq.map (fun file -> (file.CreationTime.ToString(), DateTime.UtcNow.ToString()) , DateTime.op_LessThan(file.CreationTime, DateTime.UtcNow))