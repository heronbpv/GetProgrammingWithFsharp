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