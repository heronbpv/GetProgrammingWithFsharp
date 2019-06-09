(** 
 * Import the database .bak, found here "https://github.com/Microsoft/sql-server-samples/releases/tag/adventureworks"(the 2016 LT version), 
 * by following the instructions found here "https://docs.microsoft.com/en-gb/visualstudio/data-tools/install-sql-server-sample-databases?view=vs-2015".
 **)

#I @"..\get-programming-fsharp-master\packages"
#r @"FSharp.Data.SqlClient\lib\net40\FSharp.Data.SqlClient.dll"
//Using the SqlClient
//@Now you try 32.2.1
open FSharp.Data
let [<Literal>] Conn = "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type GetCostumers = SqlCommandProvider<"SELECT * FROM SalesLT.Customer", Conn>

 
let customers = GetCostumers.Create(Conn).Execute() |> Seq.toArray
let customer = customers.[0]
printfn "%s %s works for %s" customer.FirstName customer.LastName (Option.get customer.CompanyName) //Option.defaultValue seems to not been available... beware exceptions!

//@Now you try 32.2.2
type AdventureWorks = SqlProgrammabilityProvider<Conn>
let ProductCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()
ProductCategory.AddRow("Mittens", Some 3)
ProductCategory.AddRow("Long Shorts", Some 3)
ProductCategory.AddRow("Wooly Hats", Some 4)
ProductCategory.Update()

type Categories = SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>
let woolyHats = Categories.``Wooly Hats``
printfn "Wooly Hats has ID %d" woolyHats


//Using the SqlProvider
#r @"SQLProvider\lib\FSharp.Data.SqlProvider.dll"
open FSharp.Data.Sql
type AdventureWorks2 = SqlDataProvider<ConnectionString = Conn, UseOptionTypes = true>
let context = AdventureWorks2.GetDataContext()

let customers2 =
    query {
        for customer in context.SalesLt.Customer do
        take 10
    }
    |> Seq.toArray

let customer2 = customers2.[0]

printfn "%s %s works for %s" customer2.FirstName customer2.LastName (Option.get customer2.CompanyName) //Same results as before

let result =
    query {
        for customer in context.SalesLt.Customer do
        where (customer.CompanyName = Some "Sharp Bikes")
        select (customer.FirstName, customer.LastName)
        distinct
    }
    |> Seq.toArray //This is necessary to convert from the IQueryable type that's the result of the query expression

let category = context.SalesLt.ProductCategory.Create()
category.ParentProductCategoryId <- Some 3
category.Name <- "Scarf"
context.SubmitUpdates()