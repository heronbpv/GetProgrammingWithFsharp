//@Now you try 6.2
open System.Windows.Forms
let form = new Form()
form.Show()
form.Width <- 400
form.Height <- 400
form.Text <- "Hello from F#!"

let form2 = new Form(Text = "Hello from F#!", Width = 300, Height = 300)
form2.Show

//@Now you try 6.3.1
//let mutable petrol = 100.0
//let drive(distance) = 
//    if distance = "far" then petrol <- petrol / 2.0
//    elif distance = "medium" then petrol <- petrol - 10.0
//    else petrol <- petrol - 1.0
//
//drive("far")
//drive("medium")
//drive("short")
//petrol

//@Now you try 6.3.2
let drive(petrol, distance) = 
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

let petrol = 100.0
let firstState = drive(petrol, "far")
let secondState = drive(firstState, "medium")
let finalState = drive(secondState, "short")