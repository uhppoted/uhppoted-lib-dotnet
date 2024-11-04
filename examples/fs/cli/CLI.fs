open Commands

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"
    printfn "  - get-all-controllers  Retrieves a list of controllers accessible on the local LAN"
    printfn "  - get-controller       Retrieves the controller information for a specific controller"
    printfn "  - set-IPv4             Sets the controller IPv4 address, netmask and gateway"
    printfn "  - get-listener         Retrieves the controller event listener address:port and auto-send interval"
    printfn "  - set-listener         Sets the controller event listener address:port and auto-send interval"
    printfn "  - get-time             Retrieves the controller system date and time"
    printfn "  - set-time             Sets the controller system date and time"
    printfn "\n"

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.8.10\n"

    let arglist = args |> List.ofSeq

    match arglist with
    | "get-all-controllers" :: _ -> get_controllers (arglist[1..])
    | "get-controller" :: _ -> get_controller (arglist[1..])
    | "set-IPv4" :: _ -> set_IPv4 (arglist[1..])
    | "get-listener" :: _ -> get_listener (arglist[1..])
    | "set-listener" :: _ -> set_listener (arglist[1..])
    | "get-time" :: _ -> get_time (arglist[1..])
    | "set-time" :: _ -> set_time (arglist[1..])
    | _ -> usage ()

    0
