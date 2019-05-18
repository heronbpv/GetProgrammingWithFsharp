open System
let arrayOfChars = [| for c in 'a'..'z' -> Char.ToUpper c |]
let arrayOfChars2:Char[] = //Using for..in..do doesn't work here, the syntax is different. Also, lot's of warnings and errors for code that doesn't generate an array.
    [| for c in 'a'..'z' do 
           let x = Char.ToUpper c
           x |> ignore |]
let listOfSquares = [ for i in 1..10 -> i*i ]
let seqOfStrings = seq { for i in 2..4..20 -> sprintf "Number %d" i } //Arbitrary seq of strings based on every fourth number between 2 and 20.
seqOfStrings |> List.ofSeq //Using List.ofSeq to evaluate the sequence; remember that sequences are lazy by default.

//@Now you try 20.2.3
let getCreditLimit customer = 
    match customer with
    //| _, 1 -> 250
    | "medium", 1 -> 500
    | "good", 0 | "good", 1 -> 750
    | "good", 2 -> 1000
    | "good", _ -> 2000
    //| _, 2 -> 250
    | _ -> 250

let limit1 = getCreditLimit ("medium", 1)
let failure = getCreditLimit ("bad",  -1) //Will throw an exception with the last match commented.