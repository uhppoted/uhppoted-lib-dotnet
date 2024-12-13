namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

[<Struct>]
type Controller =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: DateOnly Nullable }


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
    { profile: uint8
      start_date: DateOnly Nullable
      end_date: DateOnly Nullable
      monday: bool
      tuesday: bool
      wednesday: bool
      thursday: bool
      friday: bool
      saturday: bool
      sunday: bool
      segment1_start: TimeOnly Nullable
      segment1_end: TimeOnly Nullable
      segment2_start: TimeOnly Nullable
      segment2_end: TimeOnly Nullable
      segment3_start: TimeOnly Nullable
      segment3_end: TimeOnly Nullable
      linked_profile: uint8 }

[<Struct>]
type Task =
    { task: uint8
      door: uint8
      start_date: DateOnly Nullable
      end_date: DateOnly Nullable
      start_time: TimeOnly Nullable
      monday: bool
      tuesday: bool
      wednesday: bool
      thursday: bool
      friday: bool
      saturday: bool
      sunday: bool
      more_cards: uint8 }

[<Struct>]
type Listener =
    { endpoint: IPEndPoint
      interval: uint8 }

[<Struct>]
type ListenerEvent =
    { Controller: uint32
      Status: Status
      Event: Nullable<Event> }

type OnEvent = delegate of (ListenerEvent) -> unit
type OnError = delegate of (string) -> unit
