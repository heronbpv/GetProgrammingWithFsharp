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
