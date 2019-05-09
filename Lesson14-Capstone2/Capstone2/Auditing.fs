namespace Capstone2.Auditing

///Audit functions
[<AutoOpen>]
module Auditing = 
    open System.IO
    open Capstone2.Domain
    
    ///Registers the audinting message in a file. The directory will be one level above the executable's current location in the filesystem.
    ///The directory name is the name of the owner of the account.
    let fileSystemAudit account message = 
        let path = Path.Combine (Directory.GetParent(__SOURCE_DIRECTORY__).FullName, account.Owner.Name)
        let dir = Directory.CreateDirectory (path)
        let path = Path.Combine (dir.FullName, (account.Id.ToString() + ".txt")) //Works, because of shadowing. Careful!
        File.AppendAllText(path, (message + "\n"))
    
    ///Prints the audit message to the default console output.
    let consoleAudit account message = 
        printfn "Account %s: %s" (account.Id.ToString()) message