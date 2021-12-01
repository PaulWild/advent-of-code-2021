module AdventOfCodeFSharpFoo
   
open System
open System.Diagnostics
open AdventOfCodeFSharp
open AdventOfCodeFSharp.Day08

[<EntryPoint>]
let main argv =
    let foo =  Stopwatch()
    foo.Start()
    let (part1Val,_) = part1
    foo.Stop()
    
    let foo2 =  Stopwatch()
    foo2.Start()
    let part2Val = Seq.head part2
    foo2.Stop()
    
    printfn "Answer to Day 8 Part 1: %d in %dms" part1Val foo.ElapsedMilliseconds
    printfn "Answer to Day 8 Part 2: %d in %dms" part2Val foo2.ElapsedMilliseconds
    0 // return an integer exit code