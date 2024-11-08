open Commands

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"

    commands |> List.iter (fun v -> printfn "  - %-19s  %s" v.command v.description)

    printfn ""

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.8.10\n"

    let arglist = args |> List.ofSeq

    let result =
        match arglist with
        | "get-all-controllers" :: _ -> get_controllers (arglist[1..])
        | "get-controller" :: _ -> get_controller (arglist[1..])
        | "set-IPv4" :: _ -> set_IPv4 (arglist[1..])
        | "get-listener" :: _ -> get_listener (arglist[1..])
        | "set-listener" :: _ -> set_listener (arglist[1..])
        | "get-time" :: _ -> get_time (arglist[1..])
        | "set-time" :: _ -> set_time (arglist[1..])
        | "get-door" :: _ -> get_door (arglist[1..])
        | "set-door" :: _ -> set_door (arglist[1..])
        | "set-door-passcodes" :: _ -> set_door_passcodes (arglist[1..])
        | "open-door" :: _ -> open_door (arglist[1..])
        | _ -> Error "invalid command"

    match result with
    | Ok _ -> 0
    | Error "invalid command" ->
        usage ()
        1
    | Error err ->
        printf "  ** ERROR %A\n" err
        1
