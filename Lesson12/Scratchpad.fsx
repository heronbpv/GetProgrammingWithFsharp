//@Try this 12
#I __SOURCE_DIRECTORY__
#load ".\Lesson12ConsoleApp\Calculator.fs"

open Calculator

let x = add 10 11 //Without the AutoOpen attribute, this call doesn't compile since the definition is not in scope.
let y = Operations.add 10 11 //This is the way to call functions in the submodule if it's not opened yet.

//In terms of succinctness, the AutoOpen attribute enables the caller to abstract the modules internal structure, letting him focus on the logic. 
//Also, it reduces the number of potential open statements.