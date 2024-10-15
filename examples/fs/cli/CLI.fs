open Commands

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"
    printfn "  - get-controllers  Retrieves a list of controllers accessible on the local LAN"
    printfn "\n"

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.8.10\n"

    let arglist = args |> List.ofSeq

    match arglist with
    | "get-controllers" :: [] -> get_controllers ()

    | _ -> usage ()

    0
