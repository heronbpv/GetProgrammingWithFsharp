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
          TelephneUpdates:string
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

