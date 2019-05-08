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

//@Now you try 10.1.2
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

//@Quick check 10.1
let a = obj()
let b = a
a = b
let c = obj()
a = c
b = c
c = c
a.Equals(b)
b.Equals(a)
c.Equals(a)
c.Equals(c)

//@Now you try 10.2.3
let updatedCustomer = 
    { customer with Age = 31
                    EmailAddress = "joe@bloggs.co.uk" }
let copiedCustomer = 
    { customer with Age = 31
                    EmailAddress = "joe@bloggs.co.uk" }

let eqOperator = copiedCustomer = updatedCustomer
let eqMethodCall = updatedCustomer.Equals(copiedCustomer) //These two return true; the comparison is structural, so it fits.
let sysobRefEq = System.Object.ReferenceEquals(updatedCustomer, copiedCustomer)//This returns false, since they don't point to the same object in memory.

let alterAge customer =
    let random = //Refer to https://www.google.com/search?q=.net%20generating%20random%20number%20between%20interval
        let randomizer = new System.Random()
        randomizer.Next(18, 45)
    printfn "Altering age for customer %s; from %i to %i" customer.Forename customer.Age random
    {customer with Age = random}

let newCustomer1 = alterAge customer
let newCustomer2 = alterAge updatedCustomer

let w = //Generated through code generation
    { Forename = "autocomplete"
      Surname = Unchecked.defaultof<_>
      Age = Unchecked.defaultof<_>
      Address = Unchecked.defaultof<_>
      EmailAddress = Unchecked.defaultof<_> }

//Shadowing
let myHome = {Street = "1"; Town = "2"; City = "3"}
let myHome = { myHome with City = "4" }
let myHome = { myHome with City = "5" } //Despite the warning, this compiles just fine. Probably the litting?
