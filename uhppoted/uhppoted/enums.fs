namespace uhppoted

/// <summary>Defines the door control modes for an access controller.</summary>
type DoorMode =
    /// <summary>Unknown door control mode.</summary>
    | Unknown = 0

    /// <summary>Door is always unlocked.</summary>
    | NormallyOpen = 1

    /// <summary>Door is always locked.</summary>
    | NormallyClosed = 2

    /// <summary>Door lock is managed by access controller.</summary>
    | Controlled = 3


/// <summary>Defines whether a door was unlocked for entrance or exit.</summary>
type Direction =
    /// <summary>Unknown direction.</summary>
    | Unknown = 0

    /// <summary>Access granted for entrance.</summary>
    | In = 1

    /// <summary>Access granted for exit.</summary>
    | Out = 2


/// <summary>Defines the door interlocks for an access controller.</summary>
type Interlock =
    /// <summary>No interlocks.</summary>
    | None = 0

    /// <summary>Interlocks doors 1 & 2.</summary>
    | Doors12 = 1

    /// <summary>Interlocks doors 3 & 4.</summary>
    | Doors34 = 2

    /// <summary>Interlocks doors 1 & 2 and doors 3 & 4.</summary>
    | Doors12And34 = 3

    /// <summary>Interlock between doors 1, 2 & 3./// </summary>
    | Doors123 = 4

    /// <summary>Interlocks all doors./// </summary>
    | Doors1234 = 8

    /// <summary>Unknown interlock mode.</summary>
    | Unknown = 255

module internal Enums =
    let internal toDoorMode (v: uint8) : DoorMode =
        match v with
        | 1uy -> DoorMode.NormallyOpen
        | 2uy -> DoorMode.NormallyClosed
        | 3uy -> DoorMode.Controlled
        | _ -> DoorMode.Unknown

    let internal toInterlock (v: uint8) : Interlock =
        match v with
        | 0uy -> Interlock.None
        | 1uy -> Interlock.Doors12
        | 2uy -> Interlock.Doors34
        | 3uy -> Interlock.Doors12And34
        | 4uy -> Interlock.Doors123
        | 8uy -> Interlock.Doors1234
        | _ -> Interlock.Unknown

    let internal toDirection (v: uint8) : Direction =
        match v with
        | 1uy -> Direction.In
        | 2uy -> Direction.Out
        | _ -> Direction.Unknown
