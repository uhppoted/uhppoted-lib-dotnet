open Commands

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"
    printfn "  - get-all-controllers  Retrieves a list of controllers accessible on the local LAN"
    printfn "  - get-controller       Retrieves the controller information for a specific controller"
    printfn "\n"

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.8.10\n"

    let arglist = args |> List.ofSeq

    match arglist with
    | "get-all-controllers" :: [] -> get_controllers ()
    | "get-controller" :: [] -> get_controller ()

    | _ -> usage ()

    0
