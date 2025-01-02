namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation


/// Container configuration struct for controllers that require 'connected UDP' or TCP/IP.
[<Struct>]
type C =
    {
        /// Controller serial number.
        controller: uint32

        /// Optional IPv4 controller address:port. Required if the controller is not accessible via UDP broadcast.
        endpoint: Option<IPEndPoint>

        /// Optional 'protocol' to connect to controller. Valid values are currently 'udp' or 'tcp', defaults to 'udp'.
        protocol: Option<string>

    }

/// Controller system information.
[<Struct>]
type Controller =
    {
        /// Controller serial number.
        Controller: uint32

        /// IPv4 address.
        Address: IPAddress

        /// IPv4 subnet mask.
        Netmask: IPAddress

        /// IPv4 gateway address.
        Gateway: IPAddress

        /// Controller MAC address.
        MAC: PhysicalAddress

        /// Firmware version.
        Version: string

        /// Firmware release date.
        Date: DateOnly Nullable
    }

/// Controller door configuration information.
[<Struct>]
type Door =
    {
        /// Door control mode (normally open, normally closed or controlled).
        Mode: DoorMode

        /// Door unlocked delay (seconds).
        Delay: uint8
    }

/// Controller current state information.
[<Struct>]
type Status =
    {
        /// Door 1 open state
        Door1Open: bool

        /// Door 2 open state.
        Door2Open: bool

        /// Door 3 open state.
        Door3Open: bool

        /// Door 4 open state.
        Door4Open: bool

        /// Door 1 button state.
        Button1Pressed: bool

        /// Door 2 button state.
        Button2Pressed: bool

        /// Door 3 button state.
        Button3Pressed: bool

        /// Door 4 button state.
        Button4Pressed: bool

        /// System error code.
        SystemError: uint8

        /// Controller system date/time
        SystemDateTime: DateTime Nullable

        /// 'Special info' code.
        SpecialInfo: uint8

        /// Door unlock relays state.
        Relays: Relays

        /// Extender inputs state.
        Inputs: Inputs
    }

/// Controller door unlock relays.
and [<Struct>] Relays =
    { Door1: Relay
      Door2: Relay
      Door3: Relay
      Door4: Relay }

/// Controller extender inputs.
and [<Struct>] Inputs =
    { LockForced: Input
      FireAlarm: Input
      Input3: Input
      Input4: Input
      Input5: Input
      Input6: Input
      Input7: Input
      Input8: Input }

/// Container 'type' for event types.
[<Struct>]
type EventType = { Code: uint8; Text: string }

/// Container 'type' for event reason.
[<Struct>]
type EventReason = { Code: uint8; Text: string }

/// Event information.
[<Struct>]
type Event =
    { Timestamp: DateTime Nullable
      Index: uint32
      Event: EventType
      AccessGranted: bool
      Door: uint8
      Direction: Direction
      Card: uint32
      Reason: EventReason }


/// Access card information.
[<Struct>]
type Card =
    { Card: uint32
      StartDate: DateOnly Nullable
      EndDate: DateOnly Nullable
      Door1: uint8
      Door2: uint8
      Door3: uint8
      Door4: uint8
      PIN: uint32 }

/// Time profile configuration information.
[<Struct>]
type TimeProfile =
    { Profile: uint8
      StartDate: DateOnly Nullable
      EndDate: DateOnly Nullable
      Monday: bool
      Tuesday: bool
      Wednesday: bool
      Thursday: bool
      Friday: bool
      Saturday: bool
      Sunday: bool
      Segment1Start: TimeOnly Nullable
      Segment1End: TimeOnly Nullable
      Segment2Start: TimeOnly Nullable
      Segment2End: TimeOnly Nullable
      Segment3Start: TimeOnly Nullable
      Segment3End: TimeOnly Nullable
      LinkedProfile: uint8 }

/// Scheduled task configuration information.
[<Struct>]
type Task =
    { Task: TaskCode
      Door: uint8
      StartDate: DateOnly Nullable
      EndDate: DateOnly Nullable
      StartTime: TimeOnly Nullable
      Monday: bool
      Tuesday: bool
      Wednesday: bool
      Thursday: bool
      Friday: bool
      Saturday: bool
      Sunday: bool
      MoreCards: uint8 }

/// Event listener configuration information.
[<Struct>]
type Listener =
    { Endpoint: IPEndPoint
      Interval: uint8 }

/// 'Listen' event information.
[<Struct>]
type ListenerEvent =
    { Controller: uint32
      Status: Status
      Event: Nullable<Event> }

/// Handler for 'listen' events.
type OnEvent = delegate of (ListenerEvent) -> unit

/// Handler for 'listen' errors.
type OnError = delegate of (string) -> unit
