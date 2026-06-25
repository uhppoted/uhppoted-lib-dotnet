open Commands
open System.Net

open uhppoted

let defaults: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      timeout = 1000
      debug = false }

let usage () =
    printfn "Usage: dotnet run <command>\n"
    printfn "  Supported commands:\n"

    commands |> List.iter (fun v -> printfn "  - %-26s  %s" v.command v.description)

    printfn ""

let rec parse options args =
    match args with
    | "--bind" :: address :: rest ->
        parse
            { options with
                bind = IPEndPoint.Parse(address) }
            rest

    | "--broadcast" :: address :: rest ->
        parse
            { options with
                broadcast = IPEndPoint.Parse(address) }
            rest

    | "--listen" :: address :: rest ->
        parse
            { options with
                listen = IPEndPoint.Parse(address) }
            rest

    | "--timeout" :: timeout :: rest ->
        parse
            { options with
                timeout = timeout |> float |> (*) 1000.0 |> int }
            rest

    | "--debug" :: rest -> parse { options with debug = true } rest

    | _ -> options, args

let exec options args =
    match args |> List.ofSeq with
    | cmd :: opts ->
        match commands |> List.tryFind (fun v -> v.command = cmd) with
        | Some c ->
            match c.f (options, opts) with
            | Ok _ -> 0
            | Error err ->
                printfn "  ** ERROR %s" err
                1

        | None ->
            printfn "  ** ERROR unknown command %s" cmd
            usage ()
            1

    | _ ->
        printfn "  ** ERROR missing command"
        usage ()
        1

[<EntryPoint>]
let main args =
    printfn "** uhppoted-lib-dotnet F# CLI v0.9.1\n"

    let options, cmd = parse defaults (List.ofSeq args)

    exec options cmd
