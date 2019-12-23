namespace Controllers

open System.Net
open System.Net.Http
open System.Web.Http

//Creating a type from which to expose data
type Animal = { Name:string; Species:string }

module AnimalsRepository =
    let all =
        [
            { Name = "Fido"; Species = "Dog" }
            { Name = "Felix"; Species = "Cat" }
        ]
    let getAll() = all
    let getAnimal name = all |> List.tryFind (fun r -> r.Name = name)

[<AutoOpen>]
module Helpers =
    let asResponse (request:HttpRequestMessage) result =
        match result with
        | Some result -> request.CreateResponse(HttpStatusCode.OK, result)
        | None -> request.CreateResponse(HttpStatusCode.NotFound)

[<RoutePrefix("api")>] //Setting the Web API route prefix
type AnimalsController() = 
    inherit ApiController()

    [<Route("animals")>] //Web API route
    member this.Get(name) = //Defining the GET verb request handler for the api/animals route
        AnimalsRepository.getAnimal name |> (asResponse this.Request)