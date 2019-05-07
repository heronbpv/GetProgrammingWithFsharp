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