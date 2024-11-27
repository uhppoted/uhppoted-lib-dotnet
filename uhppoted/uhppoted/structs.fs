namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

[<Struct>]
type ControllerRecord =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: DateOnly Nullable }


[<Struct>]
type Door = { mode: uint8; delay: uint8 }

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

type TimeProfileBuilder(profile: uint8) =
    let mutable start_date: Nullable<DateOnly> = Nullable()
    let mutable end_date: Nullable<DateOnly> = Nullable()
    let mutable monday: bool = false
    let mutable tuesday: bool = false
    let mutable wednesday: bool = false
    let mutable thursday: bool = false
    let mutable friday: bool = false
    let mutable saturday: bool = false
    let mutable sunday: bool = false
    let mutable segment1_start: Nullable<TimeOnly> = Nullable()
    let mutable segment1_end: Nullable<TimeOnly> = Nullable()
    let mutable segment2_start: Nullable<TimeOnly> = Nullable()
    let mutable segment2_end: Nullable<TimeOnly> = Nullable()
    let mutable segment3_start: Nullable<TimeOnly> = Nullable()
    let mutable segment3_end: Nullable<TimeOnly> = Nullable()
    let mutable linked: uint8 = 0uy

    member this.WithStartDate(v: DateOnly) =
        start_date <- Nullable(v)
        this

    member this.WithEndDate(v: DateOnly) =
        end_date <- Nullable(v)
        this

    member this.WithWeekdays(mon: bool, tue: bool, wed: bool, thurs: bool, fri: bool, sat: bool, sun: bool) =
        monday <- mon
        tuesday <- tue
        wednesday <- wed
        thursday <- thurs
        friday <- fri
        saturday <- sat
        sunday <- sun
        this

    member this.WithSegment1(start_time: TimeOnly, end_time: TimeOnly) =
        segment1_start <- Nullable(start_time)
        segment1_end <- Nullable(end_time)
        this

    member this.WithSegment2(start_time: TimeOnly, end_time: TimeOnly) =
        segment2_start <- Nullable(start_time)
        segment2_end <- Nullable(end_time)
        this

    member this.WithSegment3(start_time: TimeOnly, end_time: TimeOnly) =
        segment3_start <- Nullable(start_time)
        segment3_end <- Nullable(end_time)
        this

    member this.WithLinkedProfile(v: uint8) =
        linked <- v
        this

    member this.build() =
        { profile = profile
          start_date = start_date
          end_date = end_date
          monday = monday
          tuesday = tuesday
          wednesday = wednesday
          thursday = thursday
          friday = friday
          saturday = saturday
          sunday = sunday
          segment1_start = segment1_start
          segment1_end = segment1_end
          segment2_start = segment2_start
          segment2_end = segment2_end
          segment3_start = segment3_start
          segment3_end = segment3_end
          linked_profile = linked }

[<Struct>]
type Listener =
    { endpoint: IPEndPoint
      interval: uint8 }

type DoorMode =
    | NormallyOpen = 1
    | NormallyClosed = 2
    | Controlled = 3

type Direction =
    | Unknown = 0
    | In = 1
    | Out = 2

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
