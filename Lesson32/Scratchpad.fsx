(** 
 * localdb CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE]; 
 * returns Windows NT user or group 'NT AUTHORITY\NETWORK SERVICE' not found. Check the name again.
 * Switch the instruction to: 
CREATE USER [NETWORK SERVICE] 
            WITHOUT LOGIN 
            WITH DEFAULT_SCHEMA = dbo 
GO
 * Then, change the references to [NT AUTHORITY\NETWORK SERVICE] to [NETWORK SERVICE].
 * The database is generated empty, though...
 **)

 #I @"..\get-programming-fsharp-master\packages"
 #r @"FSharp.Data.SqlClient\lib\net40\FSharp.Data.SqlClient.dll"
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