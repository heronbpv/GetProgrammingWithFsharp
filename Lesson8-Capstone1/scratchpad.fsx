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

