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

