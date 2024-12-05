module argparse

open System
open System.Net
open uhppoted

let (|UInt32|_|) (value: string) =
    match UInt32.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|Byte|_|) (value: string) =
    match Byte.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|Boolean|_|) (value: string) =
    match Boolean.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|IPAddress|_|) (value: string) =
    match IPAddress.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|IPEndPoint|_|) (value: string) =
    match IPEndPoint.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DateTime|_|) (value: string) =
    match DateTime.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DateOnly|_|) (value: string) =
    match DateOnly.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DoorMode|_|) (value: string) =
    match value.ToLowerInvariant() with
    | "normally-open" -> Some DoorMode.NormallyOpen
    | "normally-closed" -> Some DoorMode.NormallyClosed
    | "controlled" -> Some DoorMode.Controlled
    | _ -> None

let (|Uint32List|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Array.choose (fun v ->
                match System.UInt32.TryParse(v.Trim()) with
                | true, parsed -> Some parsed
                | _ -> None)

        Some parsed
    with _ ->
        None

let (|Permissions|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Seq.choose (fun v ->
                let parts = v.Split(':')

                match parts with
                | [| key; value |] ->
                    match System.Int32.TryParse(key.Trim()), System.Byte.TryParse(value.Trim()) with
                    | (true, k), (true, v) -> Some(k, v)
                    | _ -> None
                | [| key |] ->
                    match System.Int32.TryParse(key.Trim()) with
                    | true, k -> Some(k, 1uy)
                    | _ -> None
                | _ -> None)
            |> Map.ofSeq

        Some parsed
    with _ ->
        None

let rec argparse (args: string list) flag (defval: 'T) : 'T =
    match args with
    | arg :: value :: rest when arg = flag ->
        match box defval, value with
        | :? uint32, UInt32 parsed -> unbox parsed
        | :? byte, Byte parsed -> unbox parsed
        | :? bool, Boolean parsed -> unbox parsed
        | :? IPAddress, IPAddress parsed -> unbox parsed
        | :? IPEndPoint, IPEndPoint parsed -> unbox parsed
        | :? DateTime, DateTime parsed -> unbox parsed
        | :? DateOnly, DateOnly parsed -> unbox parsed
        | :? DoorMode, DoorMode parsed -> unbox parsed
        | _ when typeof<'T> = typeof<System.UInt32[]> ->
            match value with
            | Uint32List parsed -> unbox parsed
            | _ -> defval
        | _ when typeof<'T> = typeof<Map<Int32, Byte>> ->
            match value with
            | Permissions parsed -> unbox parsed
            | _ -> defval
        | _ -> defval

    | _ :: rest -> argparse rest flag defval

    | [] -> defval
