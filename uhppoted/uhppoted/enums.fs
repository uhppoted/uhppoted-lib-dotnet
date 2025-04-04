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


/// <summary>Defines whether a relay is active or inactive.</summary>
type Relay =
    /// <summary>Unknown state.</summary>
    | Unknown = 0

    /// <summary>Relay is in the 'inactive' state.</summary>
    | Inactive = 1

    /// <summary>Relay is in the 'active' state.</summary>
    | Active = 2


/// <summary>Defines whether an input contact is open or closed.</summary>
type Input =
    /// <summary>Unknown state.</summary>
    | Unknown = 0

    /// <summary>Input contact is 'unmade'.</summary>
    | Clear = 1

    /// <summary>Input contact is 'made'.</summary>
    | Set = 2


/// <summary>Defines the known task codes for the scheduled tasks.
type TaskCode =
    /// <summary>Sets a door mode to 'controlled.</summary>
    | ControlDoor = 0

    /// <summary>Sets a door mode to 'normally open'.</summary>
    | UnlockDoor = 1

    /// <summary>Sets a door mode to 'normally closed'.</summary>
    | LockDoor = 2

    /// <summary>Disables time profiles.</summary>
    | DisableTimeProfiles = 3

    /// <summary>Enables time profiles.</summary>
    | EnableTimeProfiles = 4

    /// <summary>Allows card entry without a PIN.</summary>
    | EnableCardNoPIN = 5

    /// <summary>Requires a card+PIN for IN access.</summary>
    | EnableCardInPIN = 6

    /// <summary>Requires a card+PIN for both IN and OUT access.</summary>
    | EnableCardInOutPIN = 7

    /// <summary>Enables 'more cards' access.</summary>
    | EnableMoreCards = 8

    /// <summary>Disables 'more cards' access.</summary>
    | DisableMoreCards = 9

    /// <summary>Trigger 'once'.</summary>
    | TriggerOnce = 10

    /// <summary>Disables pushbutton access.</summary>
    | DisablePushbutton = 11

    /// <summary>Enables pushbutton access.</summary>
    | EnablePushbutton = 12

    /// <summary>Unknown task code.</summary>
    | Unknown = 255


/// <summary>Defines the anti-passback modes for an access controller.</summary>
type AntiPassback =
    /// <summary>No anti-passback.</summary>
    | Disabled = 0

    /// <summary>Doors 1 and 2 are anti-passbacked and doors 3 and 4 are anti-passbacked.</summary>
    | Doors12_34 = 1

    /// <summary>Doors 1 and 3 are anti-passbacked with doors 2 and 4.</summary>
    | Doors13_24 = 2

    /// <summary>Door 1 is anti-passbacked with doors 2 and 3.</summary>
    | Doors1_23 = 3

    /// <summary>Door 1 is anti-passbacked with doors 2,3 and 4.</summary>
    | Doors1_234 = 4

    /// <summary>Unknown antipassback mode.</summary>
    | Unknown = 255

module internal Enums =
    let internal doorMode (v: uint8) : DoorMode =
        match v with
        | 1uy -> DoorMode.NormallyOpen
        | 2uy -> DoorMode.NormallyClosed
        | 3uy -> DoorMode.Controlled
        | _ -> DoorMode.Unknown

    let internal interlock (v: uint8) : Interlock =
        match v with
        | 0uy -> Interlock.None
        | 1uy -> Interlock.Doors12
        | 2uy -> Interlock.Doors34
        | 3uy -> Interlock.Doors12And34
        | 4uy -> Interlock.Doors123
        | 8uy -> Interlock.Doors1234
        | _ -> Interlock.Unknown

    let internal antipassback (v: uint8) : AntiPassback =
        match v with
        | 0uy -> AntiPassback.Disabled
        | 1uy -> AntiPassback.Doors12_34
        | 2uy -> AntiPassback.Doors13_24
        | 3uy -> AntiPassback.Doors1_23
        | 4uy -> AntiPassback.Doors1_234
        | _ -> AntiPassback.Unknown

    let internal direction (v: uint8) : Direction =
        match v with
        | 1uy -> Direction.In
        | 2uy -> Direction.Out
        | _ -> Direction.Unknown

    let internal relay (v: uint8) (mask: uint8) : Relay =
        if v &&& mask = mask then Relay.Active else Relay.Inactive

    let internal input (v: uint8) (mask: uint8) : Input =
        if v &&& mask = mask then Input.Set else Input.Clear
