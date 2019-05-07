//@Now you try 9.2
let parse (person:string) = 
    if not (isNull person) then
        let split = person.Split([|' '|], System.StringSplitOptions.RemoveEmptyEntries)
        let playerName, game, score = split.[0], split.[1], int(split.[2])
        playerName, game, score
    else
        "", "", 0

let test = parse("Mary Asteroids 2500")
let firstIsMary, secondIsAsteroids, thirdIs2500 = test

let testNull = parse(null)
let firstIsEmpty, secondAlso, thirdIsZero = testNull

//Nested tuples
let nameAndAge = ("Joe", "Bloggs"), 28
let name, age = nameAndAge
let (forename, surname), theAge = nameAndAge
let typeOfNameAndAge = nameAndAge.GetType()

//Wildcards
let nameAndAge2 = "Jane", "Smith", 25
let forename2, surname2, _ = nameAndAge2
let _, _, _ = nameAndAge2 //You can discard the whole tuple if you wish. Not that it makes any sense doing it this way, but it's possible.

//Mapping out out parameters
open System
let isSuccessful, convertedValue = Int32.TryParse("15")


