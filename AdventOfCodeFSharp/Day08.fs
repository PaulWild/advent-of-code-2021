namespace AdventOfCodeFSharp
   
open System.IO

module Day08 =

    type ExitType = Loop | OutOfBounds | Success
    
    let lines = File.ReadLines(@"./Input/day_08.txt")
                |> Seq.map (fun x -> x.Split(' '))
                |> Seq.map (fun arr  -> (arr.[0], int arr.[1]))
                |> Seq.toArray
    
    let calculateInstruction instructions accumulator index  =
        let instruction, value = instructions |> Seq.item index
        match (instruction) with
            | "jmp" -> (accumulator, index + value)
            | "acc" -> (accumulator + value, index + 1)
            | "nop" -> (accumulator, index + 1)
            | _ -> failwith "missing instruction"                        
                
    let rec runProgram instructions accumulator index seenIndexes =              
        let (acc, index) = calculateInstruction instructions accumulator index
        
        if (index > Seq.length lines || index < 0) then (accumulator, OutOfBounds) else
        if (Seq.length lines = index) then (accumulator, Success) else
        if (Set.contains index seenIndexes) then (accumulator, Loop) else
        runProgram instructions acc index (Set.add index seenIndexes)
   
    let changeInstruction (instruction, value) =
        match instruction with
            | "jmp" -> ("nop", value)
            | "acc" -> (instruction, value)
            | "nop" -> ("jmp", value)
            | _ -> failwith "missing instruction"          
   
    let alteredInstructions instructions =
        seq { for i in 0 .. Seq.length instructions do
               let newInstruction = changeInstruction (Seq.item i instructions)
               yield Array.mapi (fun idx item -> if (idx = i) then newInstruction else item) instructions
            }
      
    let part1 = runProgram lines 0 0 Set.empty
    
    let part2 =
        seq {
            for i in alteredInstructions lines do
            let (res, exitType) = runProgram i 0 0 Set.empty
            if (exitType = Success) then yield res 
            }