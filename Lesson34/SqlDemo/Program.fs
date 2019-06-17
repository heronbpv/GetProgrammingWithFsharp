open System.Configuration

[<EntryPoint>]
let main _ =
    let runtimeConnectionString = //Retrieving a connection string from the configuration file manually.
        ConfigurationManager
            .ConnectionStrings
            .["AdventureWorks"]
            .ConnectionString
    //Supplying that connection string to the data access layer.
    CustomerRepository.printCustomers(runtimeConnectionString)
    0