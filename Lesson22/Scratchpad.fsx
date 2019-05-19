//Introducing options (a.k.a. maybe monads)
let aNumber :int = 10
let maybeANumber :int option = Some 10

let calculateAnnualPremiumUsd = function //Despite the warning, all bases are covered... I think!
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None -> 
        printfn "No score supplied! Using temporary premium."
        300

let some = calculateAnnualPremiumUsd (Some 250)
let none = calculateAnnualPremiumUsd None
let maybe = calculateAnnualPremiumUsd maybeANumber
//let error = calculateAnnualPremiumUsd aNumber //Passing a non-optional value raises a compiler error.

//@Now you try 22.2.2
type Customer = { Name:string; Score:int option }
let customers = [ { Name = "Alice"; Score = None }; { Name = "Bob"; Score = Some 10 } ]
let calculateAnnualPremiumUsdFromCustomer (customer:Customer) = 
    match customer.Score with
    | Some 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None -> 
        printfn "No score supplied for '%s'! Using temporary premium." customer.Name
        300

let results = customers |> List.iter (fun customer -> printfn "Customer: %s; Score: %d" customer.Name (calculateAnnualPremiumUsdFromCustomer customer))