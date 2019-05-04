let x = "a test string"
let y = 1

let rng = System.Random 1
let callRngsus () = rng.Next 100

//@4.2.2
open System
open System.Net
open System.Windows.Forms

let fsharpOrg = 
    let webClient = new WebClient()
    webClient.DownloadString(Uri "http://www.giantitp.com/forums/forumdisplay.php?26-Gaming-(Other)")

let form = 
    let browser = new WebBrowser(ScriptErrorsSuppressed = true, Dock = DockStyle.Fill, DocumentText = fsharpOrg)
    let x = new Form(Text = "Hello from GitP!")
    x.Controls.Add browser
    x

form.Show()