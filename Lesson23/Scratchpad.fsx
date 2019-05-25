//type Address = Address of string
//let myAddress = Address "1 The Street"
////let isTheSameAddress = myAddress = "1 The Street" //Compiler error -> type mismatch.
//let (Address addressData) = myAddress
//let isTheSameAddress = "1 The Street" = addressData //Correct way of making a comparison, by unwrapping the DU's payload first.

//@Now you try 23.1.2

type CustomerId = CustomerId of string
//type Email = Email of string
//type Telephone = Telephone of string

//@Now you try 23.1.3
type ContactDetails =
    | Address of string
    | Telephone of string
    | Email of string

type Customer = 
    { CustomerId:CustomerId
      PrimaryContactDetails:ContactDetails
      SecondaryContactDetails:ContactDetails option }

let createCustomer customerId primaryContact secondaryContact = 
    { CustomerId = customerId
      PrimaryContactDetails = primaryContact
      SecondaryContactDetails = secondaryContact }

let customer = createCustomer (CustomerId "C-123") (Email "nicki@myemail.com") None
let another = createCustomer (CustomerId "C-456") (Email "josy@anotheremail.com") (Some (Telephone "029-293-23"))

type GenuineCustomer = GenuineCustomer of Customer

let validateCustomer = 
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" ->
        Some (GenuineCustomer customer)
    | Address _ 
    | Telephone _ -> //Albeit the situation seems the same, since a when clause is in use above, it's not possible to fall through to the email case with these two.
        Some (GenuineCustomer customer)
    | Email _ -> None

let sendWelcomeEmail (GenuineCustomer customer) = //This all but guarantees that the only customers who can be sent welcome emails are the validated ones.
    printfn "Hello, %A, and welcome to our site" customer.CustomerId

