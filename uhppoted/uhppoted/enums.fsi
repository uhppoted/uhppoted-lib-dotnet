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
    | Unknown = 0
    | ControlDoor = 1
    | UnlockDoor = 2
    | LockDoor = 3
    | DisableTimeProfiles = 4
    | EnableTimeProfiles = 5
    | EnableCardNoPIN = 6
    | EnableCardInPIN = 7
    | EnableCardInOutPIN = 8
    | EnableMoreCards = 9
    | DisableMoreCards = 10
    | TriggerOnce = 11
    | DisablePushbutton = 12
    | EnablePushbutton = 13

module internal Enums =
    val internal doorMode: v: uint8 -> DoorMode
    val internal interlock: v: uint8 -> Interlock
    val internal direction: v: uint8 -> Direction
    val internal relay: v: uint8 -> mask: uint8 -> Relay
    val internal input: v: uint8 -> mask: uint8 -> Input
