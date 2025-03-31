namespace uhppoted

open System
open System.Net
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal Encode =
    [<Literal>]
    let MAGIC_WORD = 0x55aaaa55u

    let bcd (v: string) = Convert.FromHexString v

    let packU32 (packet: byte array) (offset: int) (v: uint32) =
        let bytes =
            [| (byte ((v >>> 0) &&& 0x00ffu))
               (byte ((v >>> 8) &&& 0x00ffu))
               (byte ((v >>> 16) &&& 0x00ffu))
               (byte ((v >>> 24) &&& 0x00ffu)) |]

        Array.blit bytes 0 packet offset 4

    let packU16 (packet: byte array) (offset: int) (v: uint16) =
        let bytes = [| (byte ((v >>> 0) &&& 0x00ffus)); (byte ((v >>> 8) &&& 0x00ffus)) |]
        Array.blit bytes 0 packet offset 2

    let packU8 (packet: byte array) (offset: int) (v: uint8) =
        let bytes = [| (byte ((v >>> 0) &&& 0x00ffuy)) |]
        Array.blit bytes 0 packet offset 1

    let packBool (packet: byte array) (offset: int) (v: bool) =
        let bytes =
            match v with
            | true -> [| 0x01uy |]
            | false -> [| 0x00uy |]

        Array.blit bytes 0 packet offset 1

    let packIPv4 (packet: byte array) (offset: int) (v: IPAddress) =
        let bytes = v.MapToIPv4().GetAddressBytes()
        Array.blit bytes 0 packet offset 4

    let packDateTime (packet: byte array) (offset: int) (v: DateTime) =
        let bytes = bcd (v.ToString "yyyyMMddHHmmss")
        Array.blit bytes 0 packet offset 7

    let packDateOnly (packet: byte array) (offset: int) (v: Nullable<DateOnly>) =
        let bytes =
            match v.HasValue with
            | true -> bcd (v.Value.ToString "yyyyMMdd")
            | false -> [| 0uy; 0uy; 0uy; 0uy; 0uy |]

        Array.blit bytes 0 packet offset 4

    let packTimeOnly (packet: byte array) (offset: int) (v: Nullable<TimeOnly>) =
        let bytes =
            match v.HasValue with
            | true -> bcd (v.Value.ToString "HHmm")
            | false -> [| 0uy; 0uy |]

        Array.blit bytes 0 packet offset 2


    let (|Uint32|_|) (v: obj) =
        match v with
        | :? uint32 as u32 -> Some u32
        | _ -> None

    let (|Uint16|_|) (v: obj) =
        match v with
        | :? uint16 as u16 -> Some u16
        | _ -> None

    let (|Uint8|_|) (v: obj) =
        match v with
        | :? uint8 as u8 -> Some u8
        | _ -> None

    let (|Bool|_|) (v: obj) =
        match v with
        | :? bool as b -> Some b
        | _ -> None

    let (|IPv4|_|) (v: obj) =
        match v with
        | :? IPAddress as addr -> Some addr
        | _ -> None

    let (|DateTime|_|) (v: obj) =
        match v with
        | :? DateTime as datetime -> Some datetime
        | _ -> None

    let (|DateOnly|_|) (v: obj) =
        match v with
        | :? Nullable<DateOnly> as date -> Some date
        | _ -> None

    let (|TimeOnly|_|) (v: obj) =
        match v with
        | :? Nullable<TimeOnly> as time -> Some time
        | _ -> None


    let pack (packet: byte array) (offset: int) (v: 'T) =
        match box v with
        | Uint32 u32 -> packU32 packet offset u32
        | Uint16 u16 -> packU16 packet offset u16
        | Uint8 u8 -> packU8 packet offset u8
        | Bool b -> packBool packet offset b
        | IPv4 addr -> packIPv4 packet offset addr
        | DateTime datetime -> packDateTime packet offset datetime
        | DateOnly date -> packDateOnly packet offset date
        | TimeOnly time -> packTimeOnly packet offset time
        | _ -> ()

    let getControllerRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CONTROLLER)
        pack packet 4 controller

        packet

    let setIPv4Request (controller: uint32) address netmask gateway =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_IPv4)
        pack packet 4 controller
        pack packet 8 address
        pack packet 12 netmask
        pack packet 16 gateway
        pack packet 20 MAGIC_WORD

        packet

    let getListenerRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_LISTENER)
        pack packet 4 controller

        packet

    let setListenerRequest (controller: uint32) (address: IPAddress) (port: uint16) (interval: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_LISTENER)
        pack packet 4 controller
        pack packet 8 address
        pack packet 12 port
        pack packet 14 interval

        packet

    let getTimeRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_TIME)
        pack packet 4 controller

        packet

    let setTimeRequest (controller: uint32) (datetime: DateTime) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_TIME)
        pack packet 4 controller
        pack packet 8 datetime

        packet

    let getDoorRequest (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_DOOR)
        pack packet 4 controller
        pack packet 8 door

        packet

    let setDoorRequest (controller: uint32) (door: uint8) (mode: DoorMode) (delay: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_DOOR)
        pack packet 4 controller
        pack packet 8 door
        pack packet 9 (uint8 mode)
        pack packet 10 delay

        packet

    let setDoorPasscodesRequest
        (controller: uint32)
        (door: uint8)
        (passcode1: uint32)
        (passcode2: uint32)
        (passcode3: uint32)
        (passcode4: uint32)
        =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_DOOR_PASSCODES)
        pack packet 4 controller
        pack packet 8 door

        if passcode1 <= 999999u then
            pack packet 12 passcode1

        if passcode2 <= 999999u then
            pack packet 16 passcode2

        if passcode3 <= 999999u then
            pack packet 20 passcode3

        if passcode4 <= 999999u then
            pack packet 24 passcode4

        packet

    let openDoorRequest (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.OPEN_DOOR)
        pack packet 4 controller
        pack packet 8 door

        packet

    let getStatusRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_STATUS)
        pack packet 4 controller

        packet

    let getCardsRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARDS)
        pack packet 4 controller

        packet

    let getCardRequest (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARD)
        pack packet 4 controller
        pack packet 8 card

        packet

    let getCardAtIndexRequest (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARD_AT_INDEX)
        pack packet 4 controller
        pack packet 8 index

        packet

    let putCardRequest (controller: uint32) (card: Card) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.PUT_CARD)
        pack packet 4 controller
        pack packet 8 card.Card
        pack packet 12 card.StartDate
        pack packet 16 card.EndDate
        pack packet 20 card.Door1
        pack packet 21 card.Door2
        pack packet 22 card.Door3
        pack packet 23 card.Door4
        pack packet 24 card.PIN

        packet

    let deleteCardRequest (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.DELETE_CARD)
        pack packet 4 controller
        pack packet 8 card

        packet

    let deleteAllCardsRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.DELETE_ALL_CARDS)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet

    let getEventRequest (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_EVENT)
        pack packet 4 controller
        pack packet 8 index

        packet

    let getEventIndexRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_EVENT_INDEX)
        pack packet 4 controller

        packet

    let setEventIndexRequest (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_EVENT_INDEX)
        pack packet 4 controller
        pack packet 8 index
        pack packet 12 MAGIC_WORD

        packet

    let recordSpecialEventsRequest (controller: uint32) (enabled: bool) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.RECORD_SPECIAL_EVENTS)
        pack packet 4 controller
        pack packet 8 enabled

        packet

    let getTimeProfileRequest (controller: uint32) (profile: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_TIME_PROFILE)
        pack packet 4 controller
        pack packet 8 profile

        packet

    let setTimeProfileRequest (controller: uint32) (profile: TimeProfile) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_TIME_PROFILE)
        pack packet 4 controller
        pack packet 8 profile.Profile
        pack packet 9 profile.StartDate
        pack packet 13 profile.EndDate
        pack packet 17 profile.Monday
        pack packet 18 profile.Tuesday
        pack packet 19 profile.Wednesday
        pack packet 20 profile.Thursday
        pack packet 21 profile.Friday
        pack packet 22 profile.Saturday
        pack packet 23 profile.Sunday
        pack packet 24 profile.Segment1Start
        pack packet 26 profile.Segment1End
        pack packet 28 profile.Segment2Start
        pack packet 30 profile.Segment2End
        pack packet 32 profile.Segment3Start
        pack packet 34 profile.Segment3End
        pack packet 36 profile.LinkedProfile

        packet

    let clearTimeProfilesRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.CLEAR_TIME_PROFILES)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet

    let addTaskRequest (controller: uint32) (task: Task) =
        let mutable packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.ADD_TASK)
        pack packet 4 controller
        pack packet 8 task.StartDate
        pack packet 12 task.EndDate
        pack packet 16 task.Monday
        pack packet 17 task.Tuesday
        pack packet 18 task.Wednesday
        pack packet 19 task.Thursday
        pack packet 20 task.Friday
        pack packet 21 task.Saturday
        pack packet 22 task.Sunday
        pack packet 23 task.StartTime
        pack packet 25 task.Door
        pack packet 26 (uint8 task.Task)
        pack packet 27 task.MoreCards

        packet

    let clearTaskListRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.CLEAR_TASKLIST)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet

    let refreshTaskListRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.REFRESH_TASKLIST)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet

    let setPCControlRequest (controller: uint32) (enable: bool) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_PC_CONTROL)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD
        pack packet 12 enable

        packet

    let setInterlockRequest (controller: uint32) (interlock: Interlock) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_INTERLOCK)
        pack packet 4 controller
        pack packet 8 (uint8 interlock)

        packet

    let activateKeypadsRequest (controller: uint32) (reader1: bool) (reader2: bool) (reader3: bool) (reader4: bool) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.ACTIVATE_KEYPADS)
        pack packet 4 controller
        pack packet 8 reader1
        pack packet 9 reader2
        pack packet 10 reader3
        pack packet 11 reader4

        packet

    let getAntiPassbackRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_ANTIPASSBACK)
        pack packet 4 controller

        packet

    let setAntiPassbackRequest (controller: uint32) (antipassback: AntiPassback) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_ANTIPASSBACK)
        pack packet 4 controller
        pack packet 8 (uint8 antipassback)

        packet

    let restoreDefaultParametersRequest (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.RESTORE_DEFAULT_PARAMETERS)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet
