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
//Imperative aggregator for the lenght function
let lenght inputs = //Notice that the inputs type is a generalized collection; That's because it's type has no bearing in the result of the computation below.
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + 1 //Only the implementation of the loop changes
    accumulator

//Some tests for lenght
let ``Lenght of a list of 5 numbers`` = lenght [1; 2; 3; 4; 5]
let ``Lenght of an array of 5 numbers`` = lenght [|1; 2; 3; 4; 5|]
let ``Lenght of a list of 5 nulls`` = lenght [null; null; null; null; null]
let ``Lenght of an empty list`` = lenght []
let ``Lenght of a list of 2 strings`` = lenght ["first"; "second"]
let ``Lenght of a list of a tuple of 2 strings`` = lenght [("first", "second")]

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