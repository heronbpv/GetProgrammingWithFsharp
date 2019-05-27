///Defines the types to be used at the boundaries of the system.
module DTO =
    open System.Collections.Generic

    type OrderItemRequest = { ItemId:int; Count:int }
    type OrderRequest = 
        { OrderId:int
          CustomerName:string
          Comment:string
          ///One of (email or telephone), or none
          EmailUpdates:string
          TelephoneUpdates:string
          Items:IEnumerable<OrderItemRequest> }

///The internal domain representation. Inherently more powerful and f# friendly.
module Domain =
    type OrderId = OrderId of int
    type ItemId = ItemId of int

    type OrderItem = { ItemId:ItemId; Count:int }
    type UpdatePreference = 
        | EmailUpdates of string
        | TelephoneUpdates of string

    type Order = 
        { 
          OrderId:OrderId
          CustomerName:string 
          Comment:string option 
          ContactPreference:UpdatePreference option
          Items:OrderItem list
        } 

module Marshall = 
    open DTO
    open Domain

    let marshall (orderRequest:OrderRequest) = 
        { 
          OrderId = 
            OrderId (orderRequest.OrderId)
          CustomerName = 
            match orderRequest.CustomerName with
            | null -> failwith "Customer name must be populated."
            | name -> name
          Comment = 
            orderRequest.Comment |> Option.ofObj
          ContactPreference = 
            let email, telephone = Option.ofObj orderRequest.EmailUpdates, Option.ofObj orderRequest.TelephoneUpdates
            match email, telephone with
            | None, None -> None
            | Some email, None -> Some (EmailUpdates email)
            | None, Some phone -> Some (TelephoneUpdates phone)
            | Some _, Some _ -> failwith "Unable to proceed - only one of telephone and email should be supplied."
          Items = 
            orderRequest.Items
            |> List.ofSeq 
            |> List.map (fun itemRequest -> { ItemId = ItemId (itemRequest.ItemId); Count = itemRequest.Count })
        }

open DTO
open Domain
open Marshall

//For whatever crazy reason, fsi was unable to process the following inside the Marshall module, so now it's outside.
//This is just a test to see the conversion in action: a order request dto will be marshalled to the internal domain representation.
let (itemsReq:OrderItemRequest list) = 
    [ { ItemId = 1; Count = 49 }; { ItemId = 2; Count = 564 }; { ItemId = 3; Count = 215 }; { ItemId = 4; Count = 9674 }; { ItemId = 5; Count = 34 } ]
let orderRequest:OrderRequest = 
    { CustomerName = "Alice"
      Comment = null
      EmailUpdates = "alice@email.yes"
      TelephoneUpdates = null
      Items = itemsReq
      OrderId = 1 }
let order = marshall orderRequest