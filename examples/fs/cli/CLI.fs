open Commands

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"

    commands |> List.iter (fun v -> printfn "  - %-21s  %s" v.command v.description)

    printfn ""

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.8.10\n"

    match args |> List.ofSeq with
    | cmd :: opts ->
        match commands |> List.tryFind (fun v -> v.command = cmd) with
        | Some c ->
            match c.f (opts) with
            | Ok _ -> 0
            | Error err ->
                printfn "  ** ERROR %A" err
                1

        | None ->
            printfn "  ** ERROR unknown command %s" cmd
            usage ()
            1

    | _ ->
        printfn "  ** ERROR missing command"
        usage ()
        1
