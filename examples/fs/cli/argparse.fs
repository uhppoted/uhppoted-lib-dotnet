module argparse

open System
open System.Net

let (|UInt32|_|) (value: string) =
    match UInt32.TryParse value with
    | true, v -> Some(v)
    | _ -> None

let (|Byte|_|) (value: string) =
    match Byte.TryParse value with
    | true, v -> Some(v)
    | _ -> None

let (|Boolean|_|) (value: string) =
    match Boolean.TryParse value with
    | true, v -> Some(v)
    | _ -> None

let (|IPAddress|_|) (value: string) =
    match IPAddress.TryParse value with
    | true, v -> Some(v)
    | _ -> None

let (|IPEndPoint|_|) (value: string) =
    match IPEndPoint.TryParse value with
    | true, v -> Some(v)
    | _ -> None

let (|DateTime|_|) (value: string) =
    match DateTime.TryParse value with
    | true, v -> Some(v)
    | _ -> None

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
        | _ -> defval

    | _ :: rest -> argparse rest flag defval

    | [] -> defval
