//@Now you try 25.2.4
#r @"MixedSolution\CSharpProject\bin\debug\CSharpProject.dll"
open CSharpProject
let simon = Person ("Simon" + " Belmont")
simon.PrintName()

let longhand = 
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie" ]
    |> List.map (fun name -> Person name)
let shorthand = //Applies the linting tip above.
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie" ]
    //Person is a single parameter constructor, written in the c# dll, so you can omit passing an argument here as it's treated as a curried fuction.
    |> List.map Person 

open System.Collections.Generic

type PersonComparer() =
    interface IComparer<Person> with
        member this.Compare(x, y) = x.Name.CompareTo(y.Name)
let pComparer = PersonComparer() :> IComparer<Person>

pComparer.Compare(simon, Person "Fred") //This one returns 1, which means that Simon is greater than Fred.

//Gotcha: this one does not have direct access to the interface members, as they are defined in the interface only. That's why the upcast above (operator symbol: ":>").
let pComparer2 = PersonComparer()
//pComparer2.Compare //Compiler error - The field, constructor or member 'Compare' is not defined.
//This is called explicit interface implementation, which means that you always have to point out what interface you are accessing explicitly in code before using.
//Unlike in for example c#, where interface implementations are implicitly cast by default, so you can omit the casting. It's still possible to be explicit, though.
//((IComparer<Person>) pComparer2). //Trying to cast like this doesn't work; you have to use the upcast operator instead, see below.
(pComparer2 :> IComparer<Person>).Compare(simon, Person "Fred")

//@Now you try 25.3.1
type TestingFSPowerTools() = //The trick is to move the caret to the start o the interface name... which is laaaaame!
    interface IComparer<Person> with //Using the first option for stub generation.
        member this.Compare(x, y) = failwith "Not implemented yet"
    interface IEqualityComparer<Person> with //Using the second option, for lightweight generation.
        member this.Equals(x, y) = failwith "Not implemented yet"
        member this.GetHashCode(obj) = failwith "Not implemented yet"
        
        