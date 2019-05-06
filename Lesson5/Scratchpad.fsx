//@Now you try 5.2
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

//@Now you try 5.3
//Original version
//let sayHello(someValue) =
//    let innerFunction(number) = 
//        if number > 10 then "Isaac"
//        elif number > 20 then "Fred"
//        else "Sara"
//    let resultOfInner = 
//        if someValue < 10.0 then innerFunction(5)
//        else innerFunction(15)
//    "Hello " + resultOfInner
//
//let result = sayHello(10.5)

(** 
 * Scenario 1 - change string Isaac for 123. Result: the compiler infers the ifelif expression to return an int (since it's the type of the first return), so all other returning values
 * must be int as well, hence the compiler error. Plus, at the call site "Hello " + resultOfInner, since resultOfInner now is an int thanks to the changes to innerFunction (which now 
 * returns int), a type mismatch error occurs.
 **)
//let sayHello(someValue) =
//    let innerFunction(number) = 
//        if number > 10 then 123
//        elif number > 20 then "Fred"
//        else "Sara"
//    let resultOfInner = 
//        if someValue < 10.0 then innerFunction(5)
//        else innerFunction(15)
//    "Hello " + resultOfInner
//
//let result = sayHello(10.5)

(**
 * Scenario 2 - change string Fred to 123. Result: since the type inference mechanism operates from top to bottom, left to right, the return type of innerFunction is inferred to be int,
 * as is the return of the first brach of the ifelif expression. So, once the value of the string was changed to an int in the second expression, the error is now local only, since the
 * string return takes precedence in determining the returning type.
 **)
//let sayHello(someValue) =
//    let innerFunction(number) = 
//        if number > 10 then "Isaac"
//        elif number > 20 then 123
//        else "Sara"
//    let resultOfInner = 
//        if someValue < 10.0 then innerFunction(5)
//        else innerFunction(15)
//    "Hello " + resultOfInner
//
//let result = sayHello(10.5)

(**
 * Scenario 3 - change float 10.0 to 10. Result: since the first usage of someValue refers to it as an int (by virtue of using the comparison operator on a basic number, which is implied
 * to be int by the compiler rules engine), the signature of the sayHello function changes to int -> string, instead of float -> string, thus resulting in the error at it's call site.
 **)
//let sayHello(someValue) =
//    let innerFunction(number) = 
//        if number > 10 then "Isaac"
//        elif number > 20 then "Fred"
//        else "Sara"
//    let resultOfInner = 
//        if someValue < 10 then innerFunction(5)
//        else innerFunction(15)
//    "Hello " + resultOfInner
//
//let result = sayHello(10.5)