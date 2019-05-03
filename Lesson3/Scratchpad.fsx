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