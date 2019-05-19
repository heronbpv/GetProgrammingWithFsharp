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

//Option module hofs
//Option map
let describe score = 
    match score with
    | 0 -> "Low Risk"
    | score when score < 0 -> "Safe"
    | score when score > 0 -> "High Risk"
    | _ -> "Low Risk"
   
let description customer = 
    match customer.Score with
    | Some score -> Some (describe score)
    | None -> None

let description2 customer = customer.Score |> Option.map describe
let optionalDescribe = Option.map describe

let results2 = customers |> List.map description2
let results3 = customers |> List.map (fun customer -> optionalDescribe customer.Score)
let results4 = customers |> List.map description //These functions all do the same thing.

//Option bind
//The book references some drivers.[0] value; no idea from where, so here's a default nonsense implementation. Also, the type is a Customer, somehow...
let tryFindCustomer cId = if cId = 10 then (Some { Name = "Driver zero guy"; Score = Some 10 }) else None 
let getSafetyScore customer = customer.Score
let score = tryFindCustomer 10 |> Option.bind getSafetyScore
let scoreMap = tryFindCustomer 10 |> Option.map getSafetyScore //Testing with map; this creates an Some (Some 10) as a result; so, bind actually flattened the result to Some 10 only.

//Option filter
let test1 = Some 5 |> Option.filter (fun x -> x > 5)
let test2 = Some 5 |> Option.filter (fun x -> x = 5)

