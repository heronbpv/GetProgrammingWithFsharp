//@Now you try 7.2.3
open System
let describeAge age =
    let ageDescription =
        if age < 18 then "Child!"
        elif age < 65 then "Adult!"
        else "OAP!"
    let greeting = "Hello"
    Console.WriteLine("{0}! You are a '{1}'.", greeting, ageDescription)

let x = ()
let y = describeAge 17
x = y //Since both are unit, they are equal. X by virtue of direct value binding, y cause the result of function describeAge returns unit (see function signature on fsi).