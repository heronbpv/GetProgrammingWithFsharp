open System

//Defining the sum aggregator in terms of folding with an accumulator
//Imperative version
let sum inputs = 
    let mutable accumulator = 0 //An internal, mutable accumulator to represent the result of the computation, with it's initial value set.
    for input in inputs do
        //Apply the operation, sum in this case, to every item in the collection and the accumulator itself. 
        //The result is mutated onto the accumulator, effectively threading state through each iteration.
        accumulator <- accumulator + input 
    accumulator

//Some tests
let ``add 5 and 3`` = sum [5; 3]
let ``add 100 and -90`` = sum [100; -90]
let ``add -100 and -90`` = sum [-100; -90]
let ``add 0 and 0`` = sum [0; 0]
//let ``add 1 and null`` = sum [1; null] //Compiler error!
//let ``add 1 and null take 2`` = sum [1; int(Nullable<int>(null))] //Compiler error!

//@Now you try 18.1.1
//Imperative aggregator for the length function
let length inputs = //Notice that the inputs type is a generalized collection; That's because it's type has no bearing in the result of the computation below.
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + 1 //Only the implementation of the loop changes
    accumulator

//Some tests for length
let ``length of a list of 5 numbers`` = length [1; 2; 3; 4; 5]
let ``length of an array of 5 numbers`` = length [|1; 2; 3; 4; 5|]
let ``length of a list of 5 nulls`` = length [null; null; null; null; null]
let ``length of an empty list`` = length []
let ``length of a list of 2 strings`` = length ["first"; "second"]
let ``length of a list of a tuple of 2 strings`` = length [("first", "second")]

//Imperative aggregator for the max function
let max inputs = 
    let mutable accumulator = 0
    for input in inputs do
        if input > accumulator then
            accumulator <- input
    accumulator

//Some tests for max
let ``Max in a crescent list of numbers is the last number`` = max [1; 2; 3; 4; 5]
let ``Max in a decrescent list of numbers is the first number`` = max [5; 4; 3; 2; 1]
let ``Max in an unordered list of number must be result in the highest number`` = max [3; 1; 4; 6; 2; 5]
let ``Max of an empty list is 0`` = max []
//let ``Max in a list of nulls is a compilation error`` = max [null; null; null]
//let ``Max for a list of ints can't process a list of strings`` = max ["1"; "2"; "3"] //Thus showing that the current implementation is not generic enough.

//Defining the sum aggregator in terms of folding with an accumulator
//Now using the the generic fold high order function from the collections module:
let sum2 inputs = //The structure seems familiar to the imperative version, but the mutation is no longer there
    Seq.fold //Lint may complain this is the same as Seq.sum, for obvious reasons
        (fun state input -> state + input)  //Same as declaring the operator directly as a function, like this: (+); That's what the lintter is complaining about.
        0
        inputs

//The same tests as before, with the same results
let ``add 5 and 3 with sum2`` = sum2 [5; 3]
let ``add 100 and -90 with sum2`` = sum2 [100; -90]
let ``add -100 and -90 with sum2`` = sum2 [-100; -90]
let ``add 0 and 0 with sum2`` = sum2 [0; 0]
//let ``add 1 and null with sum2`` = sum2 [1; null] //Compiler error!
//let ``add 1 and null take 2 with sum2`` = sum2 [1; int(Nullable<int>(null))] //Compiler error!

//A new version, logging each iteration
let sum2log inputs = 
    Seq.fold 
        (fun state input -> 
            let newState = state + input
            printfn "Current state is %02d, input is %02d, and new state value is %02d" state input newState
            newState)
        0
        inputs

//One final test with the log version
sum2log [1..5]

//@Now you try 18.2
let length2 inputs = Seq.fold (fun state input -> state + 1) 0 inputs //So simple it can be one lined.

//Test results are the same as before
let ``length of a list of 5 numbers with length2`` = length2 [1; 2; 3; 4; 5]
let ``length of an array of 5 numbers with length2`` = length2 [|1; 2; 3; 4; 5|]
let ``length of a list of 5 nulls with length2`` = length2 [null; null; null; null; null]
let ``length of an empty list with length2`` = length2 []
let ``length of a list of 2 strings with length2`` = length2 ["first"; "second"]
let ``length of a list of a tuple of 2 strings with length2`` = length2 [("first", "second")]

let max2 inputs = Seq.fold (fun state input -> if input > state then input else state) 0 inputs

//Same test results as the original max function
let ``Max in a crescent list of numbers is the last number with max2`` = max2 [1..5]
let ``Max in a decrescent list of numbers is the first number with max2`` = max2 [5..-1..1]
let ``Max in an unordered list of number must be result in the highest number with max2`` = max2 [3; 1; 4; 6; 2; 5]
let ``Max of an empty list is 0 with max2`` = max2 []
//let ``Max in a list of nulls is a compilation error with max2`` = max2 [null; null; null]
//let ``Max for a list of ints can't process a list of strings with max2`` = max2 ["1"; "2"; "3"]

//Folding functions: a rules engine example
//Challenge: given a list of functions that have the same signature, give me a single function that runs all of them together.
open System
type Rule = string -> bool * string //The rule format definition, as a type alias
let rules :Rule list=               //A list of rules in said format
    [fun text -> (text.Split ' ').Length = 3, "Must be three words"
     fun text -> text.Length <= 30, "Max length is 30 characters"
     fun text -> text |> Seq.filter Char.IsLetter |> Seq.forall Char.IsUpper, "All letters must be caps"]

//Solution 1 - manual function composition
let validateManual (rules:Rule list) text =
    if String.IsNullOrEmpty text then
        false, "Empty string"
    else
        let passed, error = rules.[0] text //Pattern matching on the result of the first function applied to argument word
        if not passed then                 //Cascading verification, at each function; growths linearly with the collection size.
            false, error
        else
            let passed, error = rules.[1] text
            if not passed then
                false, error
            else
                let passed, error = rules.[2] text
                if not passed then
                    false, error
                else
                    true, ""

//Testing
let ``superFunc manual error on first validation`` = validateManual rules "nope"
let ``superFunc manual error on second validation`` = validateManual rules "nopenopenopenopenopenopenope nopenopenopenopenopenopenope nopenopenopenopenopenope"
let ``superFunc manual error on third validation`` = validateManual rules "nope nope nope"
let ``superFunc manual error on null argument`` = validateManual rules null
let ``superFunc manual error on empty string argument`` = validateManual rules ""
let ``superFunc manual successful`` = validateManual rules "BIG FAT NOPE"

//Solution 2 - using fold
let validateFold (rules:Rule list) = //Could be done with reduce, as per the book example
    rules
    |> List.fold (fun rule1 rule2 -> //Using fold with the value of the first element of the collection, one just have to define the first level of the cascade.
                   fun text -> 
                    if String.IsNullOrEmpty text then
                        false, "Empty string"
                    else 
                        let passed, error = rule1 text
                        if passed then
                            let passed, error = rule2 text //In case of a collection with only one function, if it's true then the function is evaluated twice, which's bad.
                            if passed then
                                true, ""
                            else
                                false, error
                        else
                            false, error) 
                 rules.[0]

//Testing
let ``superFunc with fold, error on first validation`` = validateFold rules "nope"
let ``superFunc with fold, error on second validation`` = validateFold rules "nopenopenopenopenopenopenope nopenopenopenopenopenopenope nopenopenopenopenopenope"
let ``superFunc with fold, error on third validation`` = validateFold rules "nope nope nope"
let ``superFunc with fold, error on null argument`` = validateFold rules null
let ``superFunc with fold, error on empty string argument`` = validateFold rules ""
let ``superFunc with fold, successful`` = validateFold rules "BIG FAT NOPE"