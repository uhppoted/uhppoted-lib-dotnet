namespace uhppoted

/// <summary>Defines the door control modes for an access controller.</summary>
type DoorMode =
    | Unknown = 0
    | NormallyOpen = 1
    | NormallyClosed = 2
    | Controlled = 3

/// <summary>Defines whether a door was unlocked for entrance or exit.</summary>
type Direction =
    | Unknown = 0
    | In = 1
    | Out = 2

/// <summary>Defines the door interlocks for an access controller.</summary>
type Interlock =
    | None = 0
    | Doors12 = 1
    | Doors34 = 2
    | Doors12And34 = 3
    | Doors123 = 4
    | Doors1234 = 8
    | Unknown = 255

/// <summary>Defines whether a relay is active or inactive.</summary>
type Relay =
    | Unknown = 0
    | Inactive = 1
    | Active = 2

/// <summary>Defines whether an input contact is open or closed.</summary>
type Input =
    | Unknown = 0
    | Clear = 1
    | Set = 2

/// <summary>Defines the known task codes for scheduled tasks.
type TaskCode =
    | ControlDoor = 0
    | UnlockDoor = 1
    | LockDoor = 2
    | DisableTimeProfiles = 3
    | EnableTimeProfiles = 4
    | EnableCardNoPIN = 5
    | EnableCardInPIN = 6
    | EnableCardInOutPIN = 7
    | EnableMoreCards = 8
    | DisableMoreCards = 9
    | TriggerOnce = 10
    | DisablePushbutton = 11
    | EnablePushbutton = 12
    | Unknown = 255

module internal Enums =
    val internal doorMode: v: uint8 -> DoorMode
    val internal interlock: v: uint8 -> Interlock
    val internal direction: v: uint8 -> Direction
    val internal relay: v: uint8 -> mask: uint8 -> Relay
    val internal input: v: uint8 -> mask: uint8 -> Input
