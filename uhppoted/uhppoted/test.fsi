namespace FSharp

namespace uhppoted

/// <summary>Defines the door control modes for an access controller.</summary>
[<Struct>]
type DoorMode =
    | Unknown = 0
    | NormallyOpen = 1
    | NormallyClosed = 2
    | Controlled = 3

/// <summary>Defines whether a door was unlocked for entrance or exit.</summary>
[<Struct>]
type Direction =
    | Unknown = 0
    | In = 1
    | Out = 2

/// <summary>Defines the door interlocks for an access controller.</summary>
[<Struct>]
type Interlock =
    | None = 0
    | Doors12 = 1
    | Doors34 = 2
    | Doors12And34 = 3
    | Doors123 = 4
    | Doors1234 = 8
    | Unknown = 255

/// <summary>Defines whether a relay is active or inactive.</summary>
[<Struct>]
type Relay =
    | Unknown = 0
    | Inactive = 1
    | Active = 2

/// <summary>Defines whether an input contact is open or closed.</summary>
[<Struct>]
type Input =
    | Unknown = 0
    | Clear = 1
    | Set = 2

/// <summary>Defines the known task codes for the scheduled tasks.
[<Struct>]
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

    val doorMode: v: uint8 -> DoorMode

    val interlock: v: uint8 -> Interlock

    val direction: v: uint8 -> Direction

    val relay: v: uint8 -> mask: uint8 -> Relay

    val input: v: uint8 -> mask: uint8 -> Input

namespace uhppoted

[<Struct>]
type C =
    {

        /// Controller serial number.
        controller: uint32

        /// Optional IPv4 controller address:port. Required if the controller is not accessible via UDP broadcast.
        endpoint: Option<System.Net.IPEndPoint>

        /// Optional 'protocol' to connect to controller. Valid values are currently 'udp' or 'tcp', defaults to 'udp'.
        protocol: Option<string>
    }

[<Struct>]
type Controller =
    {

        /// Controller serial number.
        Controller: uint32

        /// IPv4 address.
        Address: System.Net.IPAddress

        /// IPv4 subnet mask.
        Netmask: System.Net.IPAddress

        /// IPv4 gateway address.
        Gateway: System.Net.IPAddress

        /// Controller MAC address.
        MAC: System.Net.NetworkInformation.PhysicalAddress

        /// Firmware version.
        Version: string

        /// Firmware release date.
        Date: System.Nullable<System.DateOnly>
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
      SystemDateTime: System.Nullable<System.DateTime>
      SequenceNumber: uint32
      SpecialInfo: uint8
      Relays: Relays
      Inputs: Inputs }

and [<Struct>] Relays =
    { Door1: Relay
      Door2: Relay
      Door3: Relay
      Door4: Relay }

and [<Struct>] Inputs =
    { LockForced: Input
      FireAlarm: Input
      Input3: Input
      Input4: Input
      Input5: Input
      Input6: Input
      Input7: Input
      Input8: Input }

[<Struct>]
type EventType = { Code: uint8; Text: string }

[<Struct>]
type EventReason = { Code: uint8; Text: string }

[<Struct>]
type Event =
    { Timestamp: System.Nullable<System.DateTime>
      Index: uint32
      Event: EventType
      AccessGranted: bool
      Door: uint8
      Direction: Direction
      Card: uint32
      Reason: EventReason }

[<Struct>]
type Card =
    { Card: uint32
      StartDate: System.Nullable<System.DateOnly>
      EndDate: System.Nullable<System.DateOnly>
      Door1: uint8
      Door2: uint8
      Door3: uint8
      Door4: uint8
      PIN: uint32 }

[<Struct>]
type TimeProfile =
    { Profile: uint8
      StartDate: System.Nullable<System.DateOnly>
      EndDate: System.Nullable<System.DateOnly>
      Monday: bool
      Tuesday: bool
      Wednesday: bool
      Thursday: bool
      Friday: bool
      Saturday: bool
      Sunday: bool
      Segment1Start: System.Nullable<System.TimeOnly>
      Segment1End: System.Nullable<System.TimeOnly>
      Segment2Start: System.Nullable<System.TimeOnly>
      Segment2End: System.Nullable<System.TimeOnly>
      Segment3Start: System.Nullable<System.TimeOnly>
      Segment3End: System.Nullable<System.TimeOnly>
      LinkedProfile: uint8 }

[<Struct>]
type Task =
    { Task: TaskCode
      Door: uint8
      StartDate: System.Nullable<System.DateOnly>
      EndDate: System.Nullable<System.DateOnly>
      StartTime: System.Nullable<System.TimeOnly>
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
    { Endpoint: System.Net.IPEndPoint
      Interval: uint8 }

[<Struct>]
type ListenerEvent =
    { Controller: uint32
      Status: Status
      Event: System.Nullable<Event> }

type OnEvent = delegate of ListenerEvent -> unit

type OnError = delegate of string -> unit

namespace uhppoted

/// Convenience 'C' struct builder implementation for C# and VB.NET.
type CBuilder =

    new: controller: uint32 -> CBuilder

    /// Builds a `C` struct.
    /// - Returns: A `C` struct initialised with the controller ID and optional endpoint and protocol.
    member Build: unit -> C

    /// Sets the optional controller endpoint.
    /// - Parameter `e`: IPv4 controller address:port.
    /// - Returns: The updated builder instance.
    member WithEndPoint: e: System.Net.IPEndPoint -> CBuilder

    /// Sets the optional connection protocol.
    /// - Parameter `p`: 'udp' or 'tcp'.
    /// - Returns: The updated builder instance.
    member WithProtocol: p: string -> CBuilder

/// Convenience Card struct builder implementation for C# and VB.NET.
type CardBuilder =

    new: card: uint32 -> CardBuilder

    /// Builds a Card record.
    /// - Returns: A Card struct initialised with the card number, start and end dates, door
    member Build: unit -> Card

    /// Sets the card date time profile for door 1:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 1 (defaults to 0).
    /// - Returns: The updated builder instance.
    member WithDoor1: profile: uint8 -> CardBuilder

    /// Sets the card date time profile for door 2:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 2 (defaults to 0).
    /// - Returns: The updated builder instance.
    member WithDoor2: profile: uint8 -> CardBuilder

    /// Sets the card date time profile for door 3:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 3 (defaults to 0).
    /// - Returns: The updated builder instance.
    member WithDoor3: profile: uint8 -> CardBuilder

    /// Sets the card date time profile for door 4:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 4 (defaults to 0).
    /// - Returns: The updated builder instance.
    member WithDoor4: profile: uint8 -> CardBuilder

    /// Sets the card date after which the card is no longer valid.
    /// - Parameter `date`: Card 'end date'.
    /// - Returns: The updated builder instance.
    member WithEndDate: date: System.DateOnly -> CardBuilder

    /// Sets the (optional) card PIN code for use with a card reader keypad (0 is 'none').
    /// - Parameter `pin`: PIN code [0..999999]  (defaults to 0).
    /// - Returns: The updated builder instance.
    member WithPIN: pin: uint32 -> CardBuilder

    /// Sets the card date from which the card is valid.
    /// - Parameter `date`: Card 'start date'.
    /// - Returns: The updated builder instance.
    member WithStartDate: date: System.DateOnly -> CardBuilder

type TimeProfileBuilder =

    new: profile: uint8 -> TimeProfileBuilder

    member Build: unit -> TimeProfile

    member WithEndDate: v: System.DateOnly -> TimeProfileBuilder

    member WithLinkedProfile: profile: uint8 -> TimeProfileBuilder

    member WithSegment1: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder

    member WithSegment2: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder

    member WithSegment3: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder

    member WithStartDate: v: System.DateOnly -> TimeProfileBuilder

    member WithWeekdays:
        mon: bool * tue: bool * wed: bool * thurs: bool * fri: bool * sat: bool * sun: bool -> TimeProfileBuilder

type TaskBuilder =

    new: task: TaskCode * door: uint8 -> TaskBuilder

    member Build: unit -> Task

    member WithEndDate: date: System.DateOnly -> TaskBuilder

    member WithMoreCards: cards: uint8 -> TaskBuilder

    member WithStartDate: date: System.DateOnly -> TaskBuilder

    member WithStartTime: time: System.TimeOnly -> TaskBuilder

    member WithWeekdays:
        mon: bool * tue: bool * wed: bool * thurs: bool * fri: bool * sat: bool * sun: bool -> TaskBuilder

namespace uhppoted

/// Container class for the network configuration used to connect to an access controller.
type Options =
    {

        /// IPv4 endpoint to which to bind. Default value is INADDR_ANY (0.0.0.0:0).
        bind: System.Net.IPEndPoint

        /// IPv4 endpoint to which to broadcast UDP requests. Default value is '255.255.255.255:60000'.
        broadcast: System.Net.IPEndPoint

        /// IPv4 endpoint on which to listen for controller events. Defaults to '0.0.0.0:60001.
        listen: System.Net.IPEndPoint

        /// Operation timeout (milliseconds).
        timeout: int

        /// Logs controller requests and responses to the console if enabled.
        debug: bool
    }

/// Convenience 'Options' builder implementation for C# and VB.NET.
type OptionsBuilder =

    new: unit -> OptionsBuilder

    /// Builds the `Options` instance.
    /// - Returns: An `Options` instance populated with the current settings.
    member Build: unit -> Options

    /// Sets the `bind` endpoint.
    /// - Parameter `endpoint`: IPv4 'bind' endpoint.
    /// - Returns: The updated builder instance.
    member WithBind: endpoint: System.Net.IPEndPoint -> OptionsBuilder

    /// Sets the `broadcast` endpoint.
    /// - Parameter `endpoint`: IPv4 'broadcast' endpoint.
    /// - Returns: The updated builder instance.
    member WithBroadcast: endpoint: System.Net.IPEndPoint -> OptionsBuilder

    /// Enables (or disables) logging of controller requests and responses to the console.
    /// - Parameter `enable`: `true` to enable debugging; `false` to disable.
    /// - Returns: The updated builder instance.
    member WithDebug: enable: bool -> OptionsBuilder

    /// Sets the `listen` endpoint.
    /// - Parameter `endpoint`: IPv4 'listen' endpoint.
    /// - Returns: The updated builder instance.
    member WithListen: endpoint: System.Net.IPEndPoint -> OptionsBuilder

    /// Sets the operation timeout.
    /// - Parameter `ms`: Operation timeout (milliseconds).
    /// - Returns: The updated builder instance.
    member WithTimeout: ms: int -> OptionsBuilder

namespace uhppoted

module internal internationalisation =

    val translate: e: string -> string

    /// <summary>
    /// Translates an event type into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="event">Event type.</param>
    /// <returns>
    /// Human readable event type string or "unknown (<code>)".
    /// </returns>
    val TranslateEventType: event: uint8 -> string

    /// <summary>
    /// Translates an event reason code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="reason">Event reason code.</param>
    /// <returns>
    /// Human readable reason string or "unknown (<code>)".
    /// </returns>
    val TranslateEventReason: reason: uint8 -> string

    /// <summary>
    /// Translates an event door direction code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="direction">Door direction code.</param>
    /// <returns>
    /// Human readable direction string or "unknown (<direction>)"
    /// </returns>
    val TranslateDoorDirection: direction: uint8 -> string

    /// <summary>
    /// Translates a door control mode code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="mode">Door control mode.</param>
    /// <returns>
    /// Human readable door control mode string or "unknown (<code>)".
    /// </returns>
    val TranslateDoorMode: mode: uint8 -> string

    /// <summary>
    /// Translates a door interlock code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="interlock">Door interlock code.</param>
    /// <returns>
    /// Human readable door interlock string or "unknown (<code>)".
    /// </returns>
    val TranslateDoorInterlock: interlock: uint8 -> string

    /// <summary>
    /// Translates a task code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="task">Task code.</param>
    /// <returns>
    /// Human readable task description string or "unknown (<code>)".
    /// </returns>
    val TranslateTaskCode: task: uint8 -> string

    /// <summary>
    /// Translates a relay open/closed state into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="relay">Relay state.</param>
    /// <returns>
    /// Human readable relay state or "unknown (<code>)".
    /// </returns>
    val TranslateRelayState: relay: uint8 -> string

    /// <summary>
    /// Translates an input on/off state into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="input">Input state.</param>
    /// <returns>
    /// Human readable input state or "unknown (<code>)".
    /// </returns>
    val TranslateInputState: input: uint8 -> string

namespace uhppoted

type internal IResponse =

    abstract controller: uint32

type internal GetControllerResponse =
    { controller: uint32
      address: System.Net.IPAddress
      netmask: System.Net.IPAddress
      gateway: System.Net.IPAddress
      MAC: System.Net.NetworkInformation.PhysicalAddress
      version: string
      date: System.Nullable<System.DateOnly> }

    interface IResponse

type GetListenerResponse =
    { controller: uint32
      endpoint: System.Net.IPEndPoint
      interval: uint8 }

    interface IResponse

type SetListenerResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type GetTimeResponse =
    { controller: uint32
      datetime: System.Nullable<System.DateTime> }

    interface IResponse

type SetTimeResponse =
    { controller: uint32
      datetime: System.Nullable<System.DateTime> }

    interface IResponse

type GetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse

type SetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse

type SetDoorPasscodesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type OpenDoorResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type GetStatusResponse =
    { controller: uint32
      door1Open: bool
      door2Open: bool
      door3Open: bool
      door4Open: bool
      door1Button: bool
      door2Button: bool
      door3Button: bool
      door4Button: bool
      systemError: uint8
      systemDateTime: System.Nullable<System.DateTime>
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      evt:
          {| card: uint32
             direction: uint8
             door: uint8
             event: uint8
             granted: bool
             index: uint32
             reason: uint8
             timestamp: System.Nullable<System.DateTime> |} }

    interface IResponse

type GetCardsResponse =
    { controller: uint32
      cards: uint32 }

    interface IResponse

type GetCardResponse =
    { controller: uint32
      card: uint32
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse

type internal GetCardAtIndexResponse =
    { controller: uint32
      card: uint32
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse

type internal PutCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal DeleteCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal DeleteAllCardsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal GetEventResponse =
    { controller: uint32
      index: uint32
      event: uint8
      granted: bool
      door: uint8
      direction: uint8
      card: uint32
      timestamp: System.Nullable<System.DateTime>
      reason: uint8 }

    interface IResponse

type internal GetEventIndexResponse =
    { controller: uint32
      index: uint32 }

    interface IResponse

type internal SetEventIndexResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RecordSpecialEventsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal GetTimeProfileResponse =
    { controller: uint32
      profile: uint8
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      monday: bool
      tuesday: bool
      wednesday: bool
      thursday: bool
      friday: bool
      saturday: bool
      sunday: bool
      segment1Start: System.Nullable<System.TimeOnly>
      segment1End: System.Nullable<System.TimeOnly>
      segment2Start: System.Nullable<System.TimeOnly>
      segment2End: System.Nullable<System.TimeOnly>
      segment3Start: System.Nullable<System.TimeOnly>
      segment3End: System.Nullable<System.TimeOnly>
      linkedProfile: uint8 }

    interface IResponse

type internal SetTimeProfileResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ClearTimeProfilesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal AddTaskResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ClearTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RefreshTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal SetPCControlResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal SetInterlockResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ActivateKeypadsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RestoreDefaultParametersResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ListenEvent =
    { controller: uint32
      door1Open: bool
      door2Open: bool
      door3Open: bool
      door4Open: bool
      door1Button: bool
      door2Button: bool
      door3Button: bool
      door4Button: bool
      systemError: uint8
      systemDateTime: System.Nullable<System.DateTime>
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      event:
          {| card: uint32
             direction: uint8
             door: uint8
             event: uint8
             granted: bool
             index: uint32
             reason: uint8
             timestamp: System.Nullable<System.DateTime> |} }

    interface IResponse

namespace uhppoted

module internal messages =

    [<Literal>]
    val SOM: byte = 23uy

    [<Literal>]
    val SOM_v6_62: byte = 25uy

    [<Literal>]
    val GET_STATUS: byte = 32uy

    [<Literal>]
    val SET_TIME: byte = 48uy

    [<Literal>]
    val GET_TIME: byte = 50uy

    [<Literal>]
    val OPEN_DOOR: byte = 64uy

    [<Literal>]
    val PUT_CARD: byte = 80uy

    [<Literal>]
    val DELETE_CARD: byte = 82uy

    [<Literal>]
    val DELETE_ALL_CARDS: byte = 84uy

    [<Literal>]
    val GET_CARDS: byte = 88uy

    [<Literal>]
    val GET_CARD: byte = 90uy

    [<Literal>]
    val GET_CARD_AT_INDEX: byte = 92uy

    [<Literal>]
    val SET_DOOR: byte = 128uy

    [<Literal>]
    val GET_DOOR: byte = 130uy

    [<Literal>]
    val SET_TIME_PROFILE: byte = 136uy

    [<Literal>]
    val CLEAR_TIME_PROFILES: byte = 138uy

    [<Literal>]
    val SET_DOOR_PASSCODES: byte = 140uy

    [<Literal>]
    val RECORD_SPECIAL_EVENTS: byte = 142uy

    [<Literal>]
    val SET_LISTENER: byte = 144uy

    [<Literal>]
    val GET_LISTENER: byte = 146uy

    [<Literal>]
    val GET_CONTROLLER: byte = 148uy

    [<Literal>]
    val SET_IPv4: byte = 150uy

    [<Literal>]
    val GET_TIME_PROFILE: byte = 152uy

    [<Literal>]
    val SET_PC_CONTROL: byte = 160uy

    [<Literal>]
    val SET_INTERLOCK: byte = 162uy

    [<Literal>]
    val ACTIVATE_KEYPADS: byte = 164uy

    [<Literal>]
    val CLEAR_TASKLIST: byte = 166uy

    [<Literal>]
    val ADD_TASK: byte = 168uy

    [<Literal>]
    val SET_FIRST_CARD: byte = 170uy

    [<Literal>]
    val REFRESH_TASKLIST: byte = 172uy

    [<Literal>]
    val GET_EVENT: byte = 176uy

    [<Literal>]
    val SET_EVENT_INDEX: byte = 178uy

    [<Literal>]
    val GET_EVENT_INDEX: byte = 180uy

    [<Literal>]
    val RESTORE_DEFAULT_PARAMETERS: byte = 200uy

    [<Literal>]
    val LISTEN_EVENT: byte = 32uy

namespace uhppoted

module internal Encode =

    [<Literal>]
    val MAGIC_WORD: uint32 = 1437248085u

    val bcd: v: string -> byte array

    val packU32: packet: byte array -> offset: int -> v: uint32 -> unit

    val packU16: packet: byte array -> offset: int -> v: uint16 -> unit

    val packU8: packet: byte array -> offset: int -> v: uint8 -> unit

    val packBool: packet: byte array -> offset: int -> v: bool -> unit

    val packIPv4: packet: byte array -> offset: int -> v: System.Net.IPAddress -> unit

    val packDateTime: packet: byte array -> offset: int -> v: System.DateTime -> unit

    val packDateOnly: packet: byte array -> offset: int -> v: System.Nullable<System.DateOnly> -> unit

    val packTimeOnly: packet: byte array -> offset: int -> v: System.Nullable<System.TimeOnly> -> unit

    val (|Uint32|_|): v: obj -> uint32 option

    val (|Uint16|_|): v: obj -> uint16 option

    val (|Uint8|_|): v: obj -> uint8 option

    val (|Bool|_|): v: obj -> bool option

    val (|IPv4|_|): v: obj -> System.Net.IPAddress option

    val (|DateTime|_|): v: obj -> System.DateTime option

    val (|DateOnly|_|): v: obj -> System.Nullable<System.DateOnly> option

    val (|TimeOnly|_|): v: obj -> System.Nullable<System.TimeOnly> option

    val pack: packet: byte array -> offset: int -> v: 'T -> unit

    val getControllerRequest: controller: uint32 -> byte array

    val setIPv4Request: controller: uint32 -> address: 'a -> netmask: 'b -> gateway: 'c -> byte array

    val getListenerRequest: controller: uint32 -> byte array

    val setListenerRequest:
        controller: uint32 -> address: System.Net.IPAddress -> port: uint16 -> interval: uint8 -> byte array

    val getTimeRequest: controller: uint32 -> byte array

    val setTimeRequest: controller: uint32 -> datetime: System.DateTime -> byte array

    val getDoorRequest: controller: uint32 -> door: uint8 -> byte array

    val setDoorRequest: controller: uint32 -> door: uint8 -> mode: DoorMode -> delay: uint8 -> byte array

    val setDoorPasscodesRequest:
        controller: uint32 ->
        door: uint8 ->
        passcode1: uint32 ->
        passcode2: uint32 ->
        passcode3: uint32 ->
        passcode4: uint32 ->
            byte array

    val openDoorRequest: controller: uint32 -> door: uint8 -> byte array

    val getStatusRequest: controller: uint32 -> byte array

    val getCardsRequest: controller: uint32 -> byte array

    val getCardRequest: controller: uint32 -> card: uint32 -> byte array

    val getCardAtIndexRequest: controller: uint32 -> index: uint32 -> byte array

    val putCardRequest: controller: uint32 -> card: Card -> byte array

    val deleteCardRequest: controller: uint32 -> card: uint32 -> byte array

    val deleteAllCardsRequest: controller: uint32 -> byte array

    val getEventRequest: controller: uint32 -> index: uint32 -> byte array

    val getEventIndexRequest: controller: uint32 -> byte array

    val setEventIndexRequest: controller: uint32 -> index: uint32 -> byte array

    val recordSpecialEventsRequest: controller: uint32 -> enabled: bool -> byte array

    val getTimeProfileRequest: controller: uint32 -> profile: uint8 -> byte array

    val setTimeProfileRequest: controller: uint32 -> profile: TimeProfile -> byte array

    val clearTimeProfilesRequest: controller: uint32 -> byte array

    val addTaskRequest: controller: uint32 -> task: Task -> byte array

    val clearTaskListRequest: controller: uint32 -> byte array

    val refreshTaskListRequest: controller: uint32 -> byte array

    val setPCControlRequest: controller: uint32 -> enable: bool -> byte array

    val setInterlockRequest: controller: uint32 -> interlock: Interlock -> byte array

    val activateKeypadsRequest:
        controller: uint32 -> reader1: bool -> reader2: bool -> reader3: bool -> reader4: bool -> byte array

    val restoreDefaultParametersRequest: controller: uint32 -> byte array

namespace uhppoted

module internal Decode =

    val unpackU8: slice: byte array -> byte

    val unpackU16: slice: byte array -> uint16

    val unpackU32: slice: byte array -> uint32

    val unpackBool: slice: byte array -> bool

    val unpackIPv4: slice: byte array -> System.Net.IPAddress

    val unpackVersion: slice: byte array -> string

    val unpackMAC: slice: byte array -> System.Net.NetworkInformation.PhysicalAddress

    val unpackDate: slice: byte array -> System.Nullable<System.DateOnly>

    val unpackDateTime: slice: byte array -> System.Nullable<System.DateTime>

    val unpackYYMMDD: slice: byte array -> System.Nullable<System.DateOnly>

    val unpackHHmm: slice: byte array -> System.Nullable<System.TimeOnly>

    val unpackHHmmss: slice: byte array -> System.Nullable<System.TimeOnly>

    val getControllerResponse: packet: byte array -> Result<GetControllerResponse, string>

    val getListenerResponse: packet: byte array -> Result<GetListenerResponse, string>

    val setListenerResponse: packet: byte array -> Result<SetListenerResponse, string>

    val getTimeResponse: packet: byte array -> Result<GetTimeResponse, string>

    val setTimeResponse: packet: byte array -> Result<SetTimeResponse, string>

    val getDoorResponse: packet: byte array -> Result<GetDoorResponse, string>

    val setDoorResponse: packet: byte array -> Result<SetDoorResponse, string>

    val setDoorPasscodesResponse: packet: byte array -> Result<SetDoorPasscodesResponse, string>

    val openDoorResponse: packet: byte array -> Result<OpenDoorResponse, string>

    val getStatusResponse: packet: byte array -> Result<GetStatusResponse, string>

    val getCardsResponse: packet: byte array -> Result<GetCardsResponse, string>

    val getCardResponse: packet: byte array -> Result<GetCardResponse, string>

    val getCardAtIndexResponse: packet: byte array -> Result<GetCardAtIndexResponse, string>

    val putCardResponse: packet: byte array -> Result<PutCardResponse, string>

    val deleteCardResponse: packet: byte array -> Result<DeleteCardResponse, string>

    val deleteAllCardsResponse: packet: byte array -> Result<DeleteAllCardsResponse, string>

    val getEventResponse: packet: byte array -> Result<GetEventResponse, string>

    val getEventIndexResponse: packet: byte array -> Result<GetEventIndexResponse, string>

    val setEventIndexResponse: packet: byte array -> Result<SetEventIndexResponse, string>

    val recordSpecialEventsResponse: packet: byte array -> Result<RecordSpecialEventsResponse, string>

    val getTimeProfileResponse: packet: byte array -> Result<GetTimeProfileResponse, string>

    val setTimeProfileResponse: packet: byte array -> Result<SetTimeProfileResponse, string>

    val clearTimeProfilesResponse: packet: byte array -> Result<ClearTimeProfilesResponse, string>

    val addTaskResponse: packet: byte array -> Result<AddTaskResponse, string>

    val clearTaskListResponse: packet: byte array -> Result<ClearTaskListResponse, string>

    val refreshTaskListResponse: packet: byte array -> Result<RefreshTaskListResponse, string>

    val setPCControlResponse: packet: byte array -> Result<SetPCControlResponse, string>

    val setInterlockResponse: packet: byte array -> Result<SetInterlockResponse, string>

    val activateKeypadsResponse: packet: byte array -> Result<ActivateKeypadsResponse, string>

    val restoreDefaultParametersResponse: packet: byte array -> Result<RestoreDefaultParametersResponse, string>

    val listenEvent: packet: byte array -> Result<ListenEvent, string>

namespace uhppoted

module internal UDP =

    val dump: packet: byte array -> unit

    val receiveAll:
        socket: System.Net.Sockets.UdpClient ->
        closing: System.Threading.CancellationToken ->
        packets: (byte array * System.Net.IPEndPoint) list ->
            Async<Result<(byte array * System.Net.IPEndPoint) list, string>>

    val receive: socket: System.Net.Sockets.UdpClient -> Async<Result<(byte array * System.Net.IPEndPoint), string>>

    val receiveEvent:
        socket: System.Net.Sockets.UdpClient ->
        handler: (byte array * System.Net.IPEndPoint -> unit) ->
        closing: System.Threading.CancellationToken ->
            Async<Result<unit, string>>

    val broadcast:
        request: byte array *
        bind: System.Net.IPEndPoint *
        broadcast: System.Net.IPEndPoint *
        timeout: int *
        debug: bool ->
            Result<byte array list, string>

    val broadcastTo:
        request: byte array *
        bind: System.Net.IPEndPoint *
        broadcast: System.Net.IPEndPoint *
        timeout: int *
        debug: bool ->
            Result<byte array, string>

    val sendTo:
        request: byte array * src: System.Net.IPEndPoint * dest: System.Net.IPEndPoint * timeout: int * debug: bool ->
            Result<byte array, string>

    val listen:
        bind: System.Net.IPEndPoint ->
        callback: (byte array -> unit) ->
        token: System.Threading.CancellationToken ->
        debug: bool ->
            Result<unit, string>

namespace uhppoted

module internal TCP =

    val dump: packet: byte array -> unit

    val receive: stream: System.Net.Sockets.NetworkStream -> Async<Result<byte array, string>>

    val sendTo:
        request: byte array * src: System.Net.IPEndPoint * dest: System.Net.IPEndPoint * timeout: int * debug: bool ->
            Result<byte array, string>

namespace uhppoted

module Uhppoted =

    val private defaults: Options

    val internal resolve: controller: 'T -> Result<C, string>

    val private exec:
        controller: C ->
        request: byte array ->
        decode: (byte array -> Result<'b, string>) ->
        options: Options ->
            Result<'b, string>
            when 'b :> IResponse

    /// <summary>
    /// Retrieves a list of controllers on the local LAN accessible via a UDP broadcast.
    /// </summary>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>A result with an array of GetControllerResponse records or error.</returns>
    /// <remarks>
    /// Invalid individual responses are silently discarded.
    /// </remarks>
    val FindControllers: options: Options -> Result<Controller array, string>

    /// <summary>
    /// Retrieves the IPv4 configuration, MAC address and version information for an access controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with a Controller record or Error.</returns>
    /// <remarks></remarks>
    val GetController: controller: 'T * options: Options -> Result<Controller, string>

    /// <summary>
    /// Sets the controller IPv4 address, netmask and gateway address..
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="address">Controller IPv4 address.</param>
    /// <param name="netmask">Controller IPv4 netmask.</param>
    /// <param name="gateway">Gateway IPv4 address.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok or Error.</returns>
    /// <remarks>
    /// The controller does not return a response to this request - provided no network (or other) errors occur,
    /// it is assumed to be successful.
    /// </remarks>
    val SetIPv4:
        controller: 'T *
        address: System.Net.IPAddress *
        netmask: System.Net.IPAddress *
        gateway: System.Net.IPAddress *
        options: Options ->
            Result<unit, string>

    /// <summary>
    /// Retrieves the controller event listener endpoint and auto-send interval.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with an Listener record or Error.</returns>
    val GetListener: controller: 'T * options: Options -> Result<Listener, string>

    /// <summary>
    /// Sets the controller event listener IPv4 endpoint and the auto-send interval. The auto-send interval is the interval
    /// at which the controller sends the current status (including the most recent event) to the configured event listener.
    /// (events are always sent as the occur).
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="endpoint">IPv4 endpoint of event listener.</param>
    /// <param name="interval">Auto-send interval (seconds). A zero interval disables auto-send.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with true if the event listener endpoint was updated or Error.</returns>
    val SetListener:
        controller: 'T * endpoint: System.Net.IPEndPoint * interval: uint8 * options: Options -> Result<bool, string>

    /// <summary>
    /// Retrieves the controller current date and time.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with a DateTime value or Error.</returns>
    val GetTime: controller: 'T * options: Options -> Result<System.Nullable<System.DateTime>, string>

    /// <summary>
    /// Sets the controller date and time.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="datetime">Date and time to set.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the DateTime value from the controller or Error.</returns>
    val SetTime:
        controller: 'T * datetime: System.DateTime * options: Options ->
            Result<System.Nullable<System.DateTime>, string>

    /// <summary>
    /// Retrieves the control mode and unlocked delay for a door.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    val GetDoor: controller: 'T * door: uint8 * options: Options -> Result<System.Nullable<Door>, string>

    /// <summary>
    /// Sets the control mode and unlocked delay for a door.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    val SetDoor:
        controller: 'T * door: uint8 * mode: DoorMode * delay: uint8 * options: Options ->
            Result<System.Nullable<Door>, string>

    /// <summary>
    /// Sets up to 4 passcodes for a controller door.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="passcodes">Array of up to 4 passcodes in the range [0..999999], defaulting to 0 ('none')
    ///                         if the list contains less than 4 entries.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok with true value if the passcodes were updated or Error.
    /// </returns>
    val SetDoorPasscodes:
        controller: 'T * door: uint8 * passcodes: uint32 array * options: Options -> Result<bool, string>

    /// <summary>
    /// Unlocks a door controlled by a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok if the request was processed, error otherwise. The Ok response should be
    /// checked for 'true'
    /// </returns>
    /// <remarks>
    /// </remarks>
    val OpenDoor: controller: 'T * door: uint8 * options: Options -> Result<bool, string>

    /// <summary>
    /// Retrieves the current status and most recent event from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok with the controller status record, including the most recent event (if any), or Error.
    /// </returns>
    val GetStatus: controller: 'T * options: Options -> Result<(Status * System.Nullable<Event>), string>

    /// <summary>
    /// Retrieves the number of card records stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with the number of cards stored on the controller or Error.
    /// </returns>
    val GetCards: controller: 'T * options: Options -> Result<uint32, string>

    /// <summary>
    /// Retrieves the card record for the requested card number.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record matching the card number (or null if not found) or an error if the request failed.
    /// </returns>
    val GetCard: controller: 'T * card: uint32 * options: Options -> Result<System.Nullable<Card>, string>

    /// <summary>
    /// Retrieves the card record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record at the index (or null if not found or deleted) or an error if the request failed.
    /// </returns>
    val GetCardAtIndex: controller: 'T * index: uint32 * options: Options -> Result<System.Nullable<Card>, string>

    /// <summary>
    /// Adds or updates a card record on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card record to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val PutCard: controller: 'T * card: Card * options: Options -> Result<bool, string>

    /// <summary>
    /// Deletes a card record from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to delete.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val DeleteCard: controller: 'T * card: uint32 * options: Options -> Result<bool, string>

    /// <summary>
    /// Deletes all card records from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val DeleteAllCards: controller: 'T * options: Options -> Result<bool, string>

    /// </returns>
    val GetEvent: controller: 'T * index: uint32 * options: Options -> Result<System.Nullable<Event>, string>

    /// <summary>
    /// Retrieves the current event index from the controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with current controller event indexEvent record at the index or Error.
    /// </returns>
    val GetEventIndex: controller: 'T * options: Options -> Result<uint32, string>

    /// <summary>
    /// Sets the controller event index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="index">Event index.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the event index was updated (false if it was unchanged) or Error.
    /// </returns>
    val SetEventIndex: controller: 'T * index: uint32 * options: Options -> Result<bool, string>

    /// <summary>
    /// Enables or disables events for door open and close, button presses, etc.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="enable">true to enabled 'special events'.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the 'special events' mode was set or Error.
    /// </returns>
    val RecordSpecialEvents: controller: 'T * enable: bool * options: Options -> Result<bool, string>

    /// <summary>
    /// Retrieves a time profile from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Time profile ID [2..254].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with time profile, Ok(null) if the requested profile does not exist or Error if the request failed.
    /// </returns>
    val GetTimeProfile:
        controller: 'T * profile: uint8 * options: Options -> Result<System.Nullable<TimeProfile>, string>

    /// <summary>
    /// Adds or updates an access time profile on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Access time profile to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the time profile was added/updated, or Error.
    /// </returns>
    val SetTimeProfile: controller: 'T * profile: TimeProfile * options: Options -> Result<bool, string>

    /// <summary>
    /// Clears all access time profiles stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val ClearTimeProfiles: controller: 'T * options: Options -> Result<bool, string>

    /// <summary>
    /// Adds or updates a scheduled task on a controller. Added tasks are not scheduled to run
    /// until the tasklist has been refreshed.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="task">Task definition to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the task was added/updated, or Error.
    /// </returns>
    val AddTask: controller: 'T * task: Task * options: Options -> Result<bool, string>

    /// <summary>
    /// Clears all scheduled tasks from the controller task list.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val ClearTaskList: controller: 'T * options: Options -> Result<bool, string>

    /// <summary>
    /// Schedules added tasks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val RefreshTaskList: controller: 'T * options: Options -> Result<bool, string>

    /// <summary>
    /// Enables/disables remote access control management. The access controller will revert to standalone access
    /// control managment if it does not receive a command from the 'PC' at least every 30 seconds.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="enable">Enables or disables remote access control management.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val SetPCControl: controller: 'T * enable: bool * options: Options -> Result<bool, string>

    /// <summary>
    /// Sets the access controller door interlocks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="interlock">Door interlocks (none, 1&2, 3&4, 1&2 and 3&4, 1&2&3 or 1&2&3&4.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val SetInterlock: controller: 'T * interlock: Interlock * options: Options -> Result<bool, string>

    /// <summary>
    /// Activates/deactivates the access reader keypads attached to an access controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="reader1">Activates/deactivates the keypad for reader 1.</param>
    /// <param name="reader2">Activates/deactivates the keypad for reader 2.</param>
    /// <param name="reader3">Activates/deactivates the keypad for reader 3.</param>
    /// <param name="reader4">Activates/deactivates the keypad for reader 4.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val ActivateKeypads:
        controller: 'T * reader1: bool * reader2: bool * reader3: bool * reader4: bool * options: Options ->
            Result<bool, string>

    /// <summary>
    /// Restores the manufacturer defaults.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val RestoreDefaultParameters: controller: 'T * options: Options -> Result<bool, string>

    /// <summary>
    /// Listens for events from access controllers and dispatches received events to a handler.
    /// </summary>
    /// <param name="onevent">External event handler function.</param>
    /// <param name="onerror">External error handler function.</param>
    /// <param name="stop">Cancellation token to terminate event listener.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val Listen:
        onevent: OnEvent * onerror: OnError * stop: System.Threading.CancellationToken * options: Options ->
            Result<unit, string>

    /// <summary>
    /// Translates an enum into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="value">enumerated value to translate.</param>
    /// <returns>
    /// Human readable string or "unknown (<value>)".
    /// </returns>
    val Translate: v: 'T -> string
