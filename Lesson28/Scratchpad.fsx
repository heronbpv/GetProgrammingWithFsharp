open System.Collections.Generic

type OrderItemRequest = { ItemId:int; Count:int }
type OrderRequest = 
    { OrderId:int
      CustomerName:string //mandatory
      Comment:string //optional
      ///One of (email or telephone), or none
      EmailUpdates:string
      TelephneUpdates:string
      Items:IEnumerable<OrderItemRequest> //mandatory
    } 