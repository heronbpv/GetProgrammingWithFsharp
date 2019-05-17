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