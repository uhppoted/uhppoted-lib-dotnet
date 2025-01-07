namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

type Err =
    | Timeout
    | ReceiveError of error: string
    | ListenError of error: string
    | PacketError of error: string
    | InvalidPacket
    | InvalidResponse
    | InvalidControllerType of error: string
    | CardNotFound
    | EventNotFound
    | EventOverwritten

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
    {
        /// Door lock forced.
        LockForced: Input

        /// Fire alarm.
        FireAlarm: Input

        Input3: Input
        Input4: Input
        Input5: Input
        Input6: Input
        Input7: Input
        Input8: Input
    }

/// Container 'type' for event types.
[<Struct>]
type EventType =
    {
        /// Event type code.
        Code: uint8

        /// Event type description.
        Text: string
    }

/// Container 'type' for event reason.
[<Struct>]
type EventReason =
    {
        /// Event reason code.
        Code: uint8

        /// Event reason description.
        Text: string
    }

/// Event information.
[<Struct>]
type Event =
    {
        /// Event timestamp.
        Timestamp: DateTime Nullable

        /// Event index.
        Index: uint32

        /// Event type.
        Event: EventType

        /// Access granted/refused for 'swipe' events.
        AccessGranted: bool

        /// Event door.
        Door: uint8

        /// Direction (in/out) for access events.
        Direction: Direction

        /// Card number for 'swipe' events.
        Card: uint32

        /// Event reason.
        Reason: EventReason
    }


/// Access card information.
[<Struct>]
type Card =
    {
        /// Card number.
        Card: uint32

        /// Date from which card is valid.
        StartDate: DateOnly Nullable

        /// Date after which card is no longer valid.
        EndDate: DateOnly Nullable

        /// Access permissions for door 1 (0:none, 1:24/7, 2-254: time profile).
        Door1: uint8

        /// Access permissions for door 2 (0:none, 1:24/7, 2-254: time profile).
        Door2: uint8

        /// Access permissions for door 3 (0:none, 1:24/7, 2-254: time profile).
        Door3: uint8

        /// Access permissions for door 4 (0:none, 1:24/7, 2-254: time profile).
        Door4: uint8

        /// Optional PIN code for card reader keypad (1-999999, 0 for none).
        PIN: uint32
    }

/// Time profile configuration information.
[<Struct>]
type TimeProfile =
    {
        /// Time profile ID (2-254)
        Profile: uint8

        /// Date from which profile is active.
        StartDate: DateOnly Nullable

        //// Date after which profile is no longer active.
        EndDate: DateOnly Nullable

        /// Profile is active on Mondays if true.
        Monday: bool

        /// Profile is active on Tuesdays if true.
        Tuesday: bool

        /// Profile is active on Wednesdays if true.
        Wednesday: bool

        /// Profile is active on Thursdays if true.
        Thursday: bool

        /// Profile is active on Fridays if true.
        Friday: bool

        /// Profile is active on Saturdays if true.
        Saturday: bool

        /// Profile is active on Sundays if true.
        Sunday: bool

        /// Start time for first active time segment (HH:mm).
        Segment1Start: TimeOnly Nullable

        /// End time for first active time segment (HH:mm).
        Segment1End: TimeOnly Nullable

        /// Start time for second active time segment (HH:mm).
        Segment2Start: TimeOnly Nullable

        /// End time for second active time segment (HH:mm).
        Segment2End: TimeOnly Nullable

        /// Start time for third active time segment (HH:mm).
        Segment3Start: TimeOnly Nullable

        /// End time for third active time segment (HH:mm).
        Segment3End: TimeOnly Nullable

        /// Profile ID of 'extension' profile (0 if none).
        LinkedProfile: uint8
    }

/// Scheduled task configuration information.
[<Struct>]
type Task =
    {
        /// Task activity code.
        Task: TaskCode

        /// Door (1-4) to which task is assigned.
        Door: uint8

        /// Date from which task is enabled.
        StartDate: DateOnly Nullable

        /// Date after which task is no longer enabled.
        EndDate: DateOnly Nullable

        /// Time at which task triggers.
        StartTime: TimeOnly Nullable

        /// Triggers on Mondays if true.
        Monday: bool

        /// Triggers on Tuesdays if true.
        Tuesday: bool

        /// Triggers on Wednesdays if true.
        Wednesday: bool

        /// Triggers on Thursdays if true.
        Thursday: bool

        /// Triggers on Fridays if true.
        Friday: bool

        /// Triggers on Saturdays if true.
        Saturday: bool

        /// Triggers on Sundays if true.
        Sunday: bool

        /// Number of 'more cards' for 'more cards' task.
        MoreCards: uint8
    }

/// Event listener configuration information.
[<Struct>]
type Listener =
    {
        /// IPv4 address:port of event listener.
        Endpoint: IPEndPoint

        /// Interval (seconds) at which the controller state is sent to the event listener.
        Interval: uint8
    }

/// 'Listen' event information.
[<Struct>]
type ListenerEvent =
    {
        /// Originating controller ID.
        Controller: uint32

        /// Controller state at time of event.
        Status: Status

        /// Event that triggered the event. May be null in the rare condition that the auto-send
        /// interval is not zero and the controller has no events.
        Event: Nullable<Event>
    }

/// Handler for 'listen' events.
type OnEvent = delegate of (ListenerEvent) -> unit

/// Handler for 'listen' errors.
type OnError = delegate of (Err) -> unit
