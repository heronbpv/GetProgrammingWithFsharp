type Address = 
    { Street : string
      Town : string
      City : string }

type Customer = 
    { Forename : string
      Surname : string
      Age : int
      Address : Address
      EmailAddress : string }

let customer = 
    { Forename = "Joe"
      Surname = "Bloggs"
      Age = 30
      Address = 
          { Street = "The Street"
            Town = "The Town"
            City = "The City" }
      EmailAddress = "joe@bloggs.com" }

let city = customer.Address.City
let town = customer.Address.Town
let street = customer.Address.Street

//Now you try 10.1.2
type Car = 
    { Model : string
      Manufacturer : string
      Engine : string
      EngineSize : string
      Color : string
      NumberOfDoors : int }
let car = 
    { Model = "Fiat Novo Uno"
      Manufacturer = "Fiat"
      Engine = "engine1"
      EngineSize = "medium"
      Color = "blue"
      NumberOfDoors = 4 }