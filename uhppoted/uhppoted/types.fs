namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

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


[<Struct>]
type Door = { mode: DoorMode; delay: uint8 }

[<Struct>]
type Status =
    { Door1Open: bool
      Door2Open: bool
      Door3Open: bool
      Door4Open: bool
      Button1Pressed: bool
      Button2Pressed: bool
      Button3Pressed: bool
      Button4Pressed: bool
      SystemError: uint8
      SystemDateTime: DateTime Nullable
      SequenceNumber: uint32
      SpecialInfo: uint8
      Relay1: Relay
      Relay2: Relay
      Relay3: Relay
      Relay4: Relay
      Input1: Input
      Input2: Input
      Input3: Input
      Input4: Input }

[<Struct>]
type Event =
    { Timestamp: DateTime Nullable
      Index: uint32
      Event: uint8
      AccessGranted: bool
      Door: uint8
      Direction: Direction
      Card: uint32
      Reason: uint8 }


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

[<Struct>]
type Task =
    { Task: uint8
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

[<Struct>]
type Listener =
    { Endpoint: IPEndPoint
      Interval: uint8 }

[<Struct>]
type ListenerEvent =
    { Controller: uint32
      Status: Status
      Event: Nullable<Event> }

type OnEvent = delegate of (ListenerEvent) -> unit
type OnError = delegate of (string) -> unit
