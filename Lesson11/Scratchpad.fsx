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