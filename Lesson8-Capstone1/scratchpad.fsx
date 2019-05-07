//@8.4.1
open System

/// Gets the distance to a given destination 
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Stadium" then 25
    elif destination = "Office" then 50
    else failwith "Unknown destination!"

// Couple of quick tests
let isHome25 = getDistance("Home") = 25
let isStadium25 = getDistance("Stadium") = 25
let isGas10 = getDistance("Gas") = 10
let isOffice50 = getDistance("Office") = 50
let isException = getDistance("Moon")
let nullCheck = getDistance(null)

//@8.4.2
let calculateRemainingPetrol(currentPetrol, distance) = 
    if currentPetrol > distance then currentPetrol - distance
    else failwith "Oops! You've run out of petrol!"

let hasPetrol = calculateRemainingPetrol(100, 50) > 0
let hasNoPetrol = calculateRemainingPetrol(50, 50) 

//@8.4.3 && @8.4.4
let distanceToGas = getDistance("Gas")
let gasTest1 = calculateRemainingPetrol(25, distanceToGas)
let gasTest2 = calculateRemainingPetrol(5, distanceToGas)

let driveTo (petrol:int, destination:string) :int = 
    let distance = getDistance(destination)
    let remain = calculateRemainingPetrol(petrol, distance)
    if destination = "Gas" then remain + 50
    else remain

let gasTest3 = driveTo(25, "Gas")
let gasTest4 = driveTo(5, "Gas")
let gasTest5 = driveTo(100, null)

let a = driveTo(100, "Office")
let b = driveTo(a, "Stadium")
let c = driveTo(b, "Gas")
let answer = driveTo(c, "Home") = 40