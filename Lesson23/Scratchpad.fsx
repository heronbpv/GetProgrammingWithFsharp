type Address = Address of string
let myAddress = Address "1 The Street"
//let isTheSameAddress = myAddress = "1 The Street" //Compiler error -> type mismatch.
let (Address addressData) = myAddress
let isTheSameAddress = "1 The Street" = addressData //Correct way of making a comparison, by unwrapping the DU's payload first.


type Customer = 
    { CustomerId:string
      Email:string
      Telephone:string
      Address:string }

let createCustomer customerId email telephone address = 
    { CustomerId = telephone
      Email = customerId
      Telephone = address
      Address = email }

let customer = createCustomer "C-123" "nicki@myemail.com" "029-293-23" "1 The Street"