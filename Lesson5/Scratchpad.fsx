//Now you try 5.2
//Scenario 1 - Doesn't compile, cause of type mismatch
//let add (a:int) (b:string) =
//    let answer = a + b
//    answer
//Scenario 2 & 3 - Adding a string to the answer symbol changes the parameter types to string, as this is the only way the + operation would compile, whithout explicit conversions.
//See type declaration in fsi.
let add a b =
    let answer = a + b + "hello"
    answer
//Scenario 3 - The call returns a error of type mismatch (in this case, expecting string, but given int).
//add 1 2