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
      Relays: uint8
      Inputs: uint8
      EventIndex: uint32
      EventTimestamp: DateTime Nullable
      EventType: uint8
      EventAccessGranted: bool
      EventDoor: uint8
      EventDirection: uint8
      EventCard: uint32
      EventReason: uint8 }

[<Struct>]
type Card =
    { card: uint32
      start_date: DateOnly Nullable
      end_date: DateOnly Nullable
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

[<Struct>]
type Event =
    { timestamp: DateTime Nullable
      index: uint32
      event_type: uint8
      access_granted: bool
      door: uint8
      direction: uint8
      card: uint32
      reason: uint8 }

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

type internal IResponse =
    abstract member controller: uint32

type internal GetControllerResponse =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: DateOnly Nullable }

    interface IResponse with
        member this.controller = this.controller

type GetListenerResponse =
    { controller: uint32
      endpoint: IPEndPoint
      interval: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetListenerResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type GetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

    interface IResponse with
        member this.controller = this.controller

type SetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

    interface IResponse with
        member this.controller = this.controller

type GetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetDoorPasscodesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type OpenDoorResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type GetStatusResponse =
    { controller: uint32
      door1_open: bool
      door2_open: bool
      door3_open: bool
      door4_open: bool
      door1_button: bool
      door2_button: bool
      door3_button: bool
      door4_button: bool
      system_error: uint8
      system_datetime: DateTime Nullable
      sequence_number: uint32
      special_info: uint8
      relays: uint8
      inputs: uint8
      evt:
          {| index: uint32
             event_type: uint8
             granted: bool
             door: uint8
             direction: uint8
             card: uint32
             timestamp: DateTime Nullable
             reason: uint8 |} }

    interface IResponse with
        member this.controller = this.controller

type GetCardsResponse =
    { controller: uint32
      cards: uint32 }

    interface IResponse with
        member this.controller = this.controller

type GetCardResponse =
    { controller: uint32
      card: uint32
      start_date: DateOnly Nullable
      end_date: DateOnly Nullable
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal GetCardAtIndexResponse =
    { controller: uint32
      card: uint32
      start_date: DateOnly Nullable
      end_date: DateOnly Nullable
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal PutCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal DeleteCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal DeleteAllCardsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal GetEventResponse =
    { controller: uint32
      index: uint32
      event: uint8
      granted: bool
      door: uint8
      direction: uint8
      card: uint32
      timestamp: DateTime Nullable
      reason: uint8 }

    interface IResponse with
        member this.controller = this.controller

type internal GetEventIndexResponse =
    { controller: uint32
      index: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal SetEventIndexResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal RecordSpecialEventsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal GetTimeProfileResponse =
    { controller: uint32
      profile: uint8
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

    interface IResponse with
        member this.controller = this.controller

type internal SetTimeProfileResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ClearTimeProfilesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal AddTaskResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ClearTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal RefreshTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal SetPCControlResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal SetInterlockResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ActivateKeypadsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller
