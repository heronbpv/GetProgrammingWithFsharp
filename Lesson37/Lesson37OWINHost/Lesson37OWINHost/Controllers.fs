namespace Controllers

open System.Web.Http

//Creating a type from which to expose data
type Animal = { Name:string; Species:string }

[<RoutePrefix("api")>] //Setting the Web API route prefix
type AnimalsController() = 
    inherit ApiController()

    [<Route("animals")>] //Web API route
    member __.Get() = //Defining the GET verb request handler for the api/animals route
        [
            { Name = "Fido"; Species = "Dog" }
            { Name = "Felix"; Species = "Cat" }
        ]