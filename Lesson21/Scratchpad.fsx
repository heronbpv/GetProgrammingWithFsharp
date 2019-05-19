type Disk = { SizeGb:int }
type Computer = { Manufacturer:string; Disks:Disk list }

let myPc = 
    { Manufacturer = "Computers Inc."
      Disks = 
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }

type DiskDU = 
    | HardDisk of RPM:int * Platters:int
    | SolidState
    | MMC of NumberOfPins:int

//@Now you try 21.2.1
let hardDisk1 = HardDisk (250, 7)
let hardDisk2 = HardDisk (RPM = 250, Platters = 7)//You can see, and reference, the named arguments of the DU constructor, but there's no auto-completion support for them.
let mmc1 = MMC //This returns a function, expecting the argument
let mmc2 = mmc1 5 //This is a mmc DU case, with 5 pins. Notice that the named argument' name on the case is lost in intellisense for the function derived.
let ssd1 = SolidState

let seek disk =
    match disk with
    | HardDisk (5400, 5) -> "Seeking very slowly!"
    | HardDisk (rpm, 7) -> sprintf "I have 7 spindles and RPM %d!" rpm
    | HardDisk _ -> "Seeking loudly at a reasonable speed!"
    | MMC 3 -> "Seeking. I have 3 pins!"
    | MMC _ -> "Seeking quietly but slowly."
    | SolidState -> "Already found it!"

let seekSdd = seek ssd1
let seekMMC = seek (mmc1 3)