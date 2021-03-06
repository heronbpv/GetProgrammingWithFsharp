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

//@Now you try 21.2.2
let describe disk = 
    match disk with
    | HardDisk (5400, _) -> "I'm a slow hard disk."
    | HardDisk (_, 7) -> "I have 7 spindles!"
    | HardDisk _ -> "I'm a hard disk."
    | MMC 1 -> "I have only 1 pin."
    | MMC pins when pins < 5 -> "I'm an MMC with a few pins."
    | MMC pins -> sprintf "I'm a MMC with %d pins." pins
    | SolidState -> "I'm a newfangled SSD."

//Testing it:
let describeSsd = describe ssd1
let describeMmc1 = describe (mmc1 1)
let describeMmc2 = describe mmc2
let describeHardDisk1 = describe hardDisk1

//Nested DUs
type MMCDisk =
    | RsMmc
    | MmcPlus
    | SecureMMC

type NewDisks = MMC of MMCDisk * NumberOfPins:int //Style declaration for single-case DUs. You can even drop the initial pipe.

let seekNewDisks = function //Alternative sintax for functions with a single match with clause. The argument type is inferred by the compiler thanks to the cases.
    | MMC (MmcPlus, 3) -> "Seeking quietly but slowly."
    | MMC (MmcPlus, _) -> "Seeking as a MmcPlus should."
    | MMC (SecureMMC, 6) -> "Seeking quietly with 6 pins."
    | MMC (SecureMMC, _) -> "Seeking as a SecureMMC should."
    | MMC (RsMmc, _) -> "Seeking as a RsMmc should."

//Sharing data across DU cases is impossible with DUs alone, combine them with records to achieve this (similar to properties on the base class).
type EvolvedDisks =
    | HardDisk of RPM:int * Platters:int
    | SolidState
    | MMC of MMCDisk * NumberOfPins:int
type EvolvedDiskInfo = { Manufacturer:string; SizeGb:int; DiskData:EvolvedDisks }
type EvolvedComputer = { Manufacturer:string; Disks:EvolvedDiskInfo list }
let myEvolvedPc = 
    { Manufacturer = "Evolved Computers Inc."
      Disks = 
        [ { Manufacturer = "HardDisks Corp."
            SizeGb = 100
            DiskData = HardDisk (5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState }
          { Manufacturer = "MicroDisks Corp."
            SizeGb = 40
            DiskData = MMC (RsMmc, 4) } ] }

printfn "%A" myEvolvedPc

//Enums
type Printer =
    | Inkjet = 0
    | Laserjet = 1
    | DotMatrix = 2
    | SomeNewTypeAddedLater = 3

let describePrinter = 
    let knownPrinters x = //Inner function, to list which values of the enum where known at the time of the function creation.
        x = Printer.Inkjet
        && x = Printer.Laserjet
        && x = Printer.DotMatrix
    //Interestingly enough, the compiler only recognized the instances of the enum once I fully qualified each case... 
    //So it has this difference going on between them and DUs as well.
    function 
    | Printer.Inkjet -> sprintf "Enum value is %s" (Printer.Inkjet.ToString())
    | Printer.Laserjet -> sprintf "Enum value is %s" (Printer.Laserjet.ToString())
    | Printer.DotMatrix -> sprintf "Enum value is %s" (Printer.DotMatrix.ToString())
    //A conditional catchall case. Done this way, the compiler will complain of incomplete cases for enums, but the match with will never fail at runtime after adding a new enum later on.
    | x when not <| knownPrinters x -> sprintf "Undefined enum value %A, defaulting to %s" x (Printer.Inkjet.ToString()) //Uncomment to test what happens when the catch all doesn't exist.
    //Wonder which one is better in this case: failing at runtime thanks to the new enum value, or returning a default value after meeting a new entry. No compiler savior in any case...

let ``Result for Inkjet`` = describePrinter Printer.Inkjet
let ``Result for SomeNewTypeAddedLater`` = describePrinter Printer.SomeNewTypeAddedLater

let mutable x = Printer()
Printer.TryParse("9", &x)
let ``Result for some arbitrary int value`` = describePrinter x //Generating a unknown value for the enum, then passing it to the function