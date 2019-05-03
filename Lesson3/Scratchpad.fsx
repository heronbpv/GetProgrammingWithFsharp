let text = "Hello World!"
text.Length

let greetPerson name age = 
    sprintf "Hello, %s. You are %d years old" name age

let greeting = greetPerson "Fred" 25


//Try this
let countWords (text:string) =
    let count = if text |> isNull then 0 else text.Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries).Length 
    count

//Testing countWords in a REPL environment
countWords "This is a test"
countWords " "
countWords null
countWords "thisisatest"
countWords "thisis   atestwith   toomanyspaces"
countWords "thistest.separatesinaperiod"
countWords "thistest. containsaperiod"

//File creation
open System.IO
open System

let writeToFile name text =
    let path = Path.Combine(__SOURCE_DIRECTORY__, name)
    File.WriteAllText(path, text, System.Text.Encoding.UTF8) //Forgetting to use path instead of name here has cost me a lot of time...

//Formats text to be saved later
let formatText (text:string) = 
    sprintf "%0d|%s" (countWords text) text

//Testing the formatText function
formatText "This is a test"
formatText " "
formatText null
formatText "thisisatest"
formatText "thisis   atestwith   toomanyspaces"
formatText "thistest.separatesinaperiod"
formatText "thistest. containsaperiod"

//Testing the file creation functions
formatText "This is a test"
|> writeToFile "FirstTest.txt"

