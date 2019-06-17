open FSharp.Data

//let [<Literal>] Conn = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type GetCustomers = SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", "Name=AdventureWorks">

[<EntryPoint>]
let main _ = 
    let customers = GetCustomers.Create()
    
    customers.Execute()
    |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
    
    0
