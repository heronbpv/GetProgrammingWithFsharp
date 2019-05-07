module Car

open System

//TODO: Create helper functions to provide the building blocks to implement driveTo.
///Returns the distance to a given destination, or throws an exception in case the destination is unknown.
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Stadium" then 25
    elif destination = "Office" then 50
    else failwith "Unknown destination!"

///Calculates the remaining petrol, after a certain distance has been travelled. Throws an exception in case the remaining petrol is less the distance to travel.
let calculateRemainingPetrol(currentPetrol, distance) = 
    if currentPetrol > distance then currentPetrol - distance
    else failwith "Oops! You've run out of petrol!"

/// Drives to a given destination given a starting amount of petrol
let driveTo (petrol, destination) = 
    let distance = getDistance(destination)
    let remain = calculateRemainingPetrol(petrol, distance)
    if destination = "Gas" then remain + 50
    else remain