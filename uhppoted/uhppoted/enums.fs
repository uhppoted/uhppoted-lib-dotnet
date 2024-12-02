namespace uhppoted

type DoorMode =
    | Unknown = 0
    | NormallyOpen = 1
    | NormallyClosed = 2
    | Controlled = 3

type Direction =
    | Unknown = 0
    | In = 1
    | Out = 2

module internal Enums =
    let internal toDoorMode (v: uint8) : DoorMode =
        match v with
        | 1uy -> DoorMode.NormallyOpen
        | 2uy -> DoorMode.NormallyClosed
        | 3uy -> DoorMode.Controlled
        | _ -> DoorMode.Unknown
