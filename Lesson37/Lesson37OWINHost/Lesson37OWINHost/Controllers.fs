namespace Controllers

open System.Net
open System.Net.Http
open System.Text.RegularExpressions
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
    let getAnimal name =
        if not (Regex.IsMatch(name, "^[a-zA-Z]+$")) then
            Some (Error "Animal names can only contain letters!")
        else
            //TODO: find a better way to handle the Option<Result> here.
            let result = List.tryFind (fun r -> r.Name = name) all
            if result.IsNone then
                Some (Error (sprintf "%s not found." name))
            else
                Some (Ok result.Value)

[<AutoOpen>]
module Helpers =
    let asResponse (request:HttpRequestMessage) result =
        match result with
        | Some result ->
            match result with
            | Ok result -> request.CreateResponse(HttpStatusCode.OK, result)
            | Error msg -> request.CreateResponse(HttpStatusCode.BadRequest, msg)
        | None -> request.CreateResponse(HttpStatusCode.NotFound)

[<RoutePrefix("api")>] //Setting the Web API route prefix
type AnimalsController() = 
    inherit ApiController()

    [<Route("animals/{name}")>] //Web API route
    member this.Get(name) = //Defining the GET verb request handler for the api/animals route
        AnimalsRepository.getAnimal name |> (asResponse this.Request)