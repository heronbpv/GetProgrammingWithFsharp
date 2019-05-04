let x = "a test string"
let y = 1

let rng = System.Random 1
let callRngsus () = rng.Next 100

//Winforms example
open System
open System.Net
open System.Windows.Forms

let webClient = new WebClient()
let fsharpOrg = webClient.DownloadString(Uri "http://www.giantitp.com/forums/forumdisplay.php?26-Gaming-(Other)")
let browser = new WebBrowser(ScriptErrorsSuppressed = true, Dock = DockStyle.Fill, DocumentText = fsharpOrg)
let form = new Form(Text = "Hello from GitP!")
form.Controls.Add browser
form.Show()