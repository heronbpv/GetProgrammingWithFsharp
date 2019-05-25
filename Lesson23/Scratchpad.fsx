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