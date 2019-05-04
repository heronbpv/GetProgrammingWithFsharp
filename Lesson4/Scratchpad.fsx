let x = "a test string"
let y = 1

let rng = System.Random 1
let callRngsus () = rng.Next 100

//@4.2.2
open System
open System.Net
open System.Windows.Forms

let getSource path = 
    let webClient = new WebClient()
    webClient.DownloadString(Uri path)

let createFormFromSource header source = 
    let browser = new WebBrowser(ScriptErrorsSuppressed = true, Dock = DockStyle.Fill, DocumentText = source)
    let form = new Form(Text = header)
    form.Controls.Add browser
    form

let form = 
    getSource "http://www.giantitp.com/forums/forumdisplay.php?26-Gaming-(Other)"
    |> createFormFromSource "Hello from GitP!"

form.Show()