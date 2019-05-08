//@Now you try 11.2
open System
open System.IO
let writeToFile (date:DateTime) fileName text = 
    let formatedFileName = sprintf "%s-%s.txt" (date.ToString("yyyyMMdd")) fileName
    let path = Path.Combine(__SOURCE_DIRECTORY__, formatedFileName)
    File.WriteAllText(path, text)

writeToFile DateTime.UtcNow.Date "test1" "this is a test of the function writeToFile"

let writeToToday = writeToFile DateTime.UtcNow.Date
let writeToTomorrow = writeToFile (DateTime.UtcNow.Date.AddDays 1.)
let writeToTodayHelloWorld = writeToToday "hello-world"

writeToToday "first-file" "The quick brown fox jumped over the lazy dog"
writeToTomorrow "second-file" "The quick brown fox jumped over the lazy dog"
writeToTodayHelloWorld "The quick brown fox jumped over the lazy dog"

writeToFile DateTime.UtcNow.Date null null //DateTime can't be null. Sweet!

//@Now you try 11.2.2
//Code ported from the scratchpad from lesson6, now you try 6.3.1, then adapted to not use mutability and support curry.
let drive distance petrol = 
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let startPetrol = 100.0

startPetrol
|> drive "far"
|> drive "medium"
|> drive "short"
