//@Try this 18
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