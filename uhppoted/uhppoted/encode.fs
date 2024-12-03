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

    let pack_u32 (packet: byte array) (offset: int) (v: uint32) =
        let bytes =
            [| (byte ((v >>> 0) &&& 0x00ffu))
               (byte ((v >>> 8) &&& 0x00ffu))
               (byte ((v >>> 16) &&& 0x00ffu))
               (byte ((v >>> 24) &&& 0x00ffu)) |]

        Array.blit bytes 0 packet offset 4

    let pack_u16 (packet: byte array) (offset: int) (v: uint16) =
        let bytes = [| (byte ((v >>> 0) &&& 0x00ffus)); (byte ((v >>> 8) &&& 0x00ffus)) |]
        Array.blit bytes 0 packet offset 2

    let pack_u8 (packet: byte array) (offset: int) (v: uint8) =
        let bytes = [| (byte ((v >>> 0) &&& 0x00ffuy)) |]
        Array.blit bytes 0 packet offset 1

    let pack_bool (packet: byte array) (offset: int) (v: bool) =
        let bytes =
            match v with
            | true -> [| 0x01uy |]
            | false -> [| 0x00uy |]

        Array.blit bytes 0 packet offset 1

    let pack_IPv4 (packet: byte array) (offset: int) (v: IPAddress) =
        let bytes = v.MapToIPv4().GetAddressBytes()
        Array.blit bytes 0 packet offset 4

    let pack_datetime (packet: byte array) (offset: int) (v: DateTime) =
        let bytes = bcd (v.ToString "yyyyMMddHHmmss")
        Array.blit bytes 0 packet offset 7

    let pack_date (packet: byte array) (offset: int) (v: Nullable<DateOnly>) =
        let bytes =
            match v.HasValue with
            | true -> bcd (v.Value.ToString "yyyyMMdd")
            | false -> [| 0uy; 0uy; 0uy; 0uy; 0uy |]

        Array.blit bytes 0 packet offset 4

    let pack_HHmm (packet: byte array) (offset: int) (v: Nullable<TimeOnly>) =
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
        | Uint32 u32 -> pack_u32 packet offset u32
        | Uint16 u16 -> pack_u16 packet offset u16
        | Uint8 u8 -> pack_u8 packet offset u8
        | Bool b -> pack_bool packet offset b
        | IPv4 addr -> pack_IPv4 packet offset addr
        | DateTime datetime -> pack_datetime packet offset datetime
        | DateOnly date -> pack_date packet offset date
        | TimeOnly time -> pack_HHmm packet offset time
        | _ -> ()

    let get_controller_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CONTROLLER)
        pack packet 4 controller

        packet

    let set_IPv4_request (controller: uint32) address netmask gateway =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_IPv4)
        pack packet 4 controller
        pack packet 8 address
        pack packet 12 netmask
        pack packet 16 gateway
        pack packet 20 MAGIC_WORD

        packet

    let get_listener_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_LISTENER)
        pack packet 4 controller

        packet

    let set_listener_request (controller: uint32) (address: IPAddress) (port: uint16) (interval: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_LISTENER)
        pack packet 4 controller
        pack packet 8 address
        pack packet 12 port
        pack packet 14 interval

        packet

    let get_time_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_TIME)
        pack packet 4 controller

        packet

    let set_time_request (controller: uint32) (datetime: DateTime) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_TIME)
        pack packet 4 controller
        pack packet 8 datetime

        packet

    let get_door_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_DOOR)
        pack packet 4 controller
        pack packet 8 door

        packet

    let set_door_request (controller: uint32) (door: uint8) (mode: DoorMode) (delay: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_DOOR)
        pack packet 4 controller
        pack packet 8 door
        pack packet 9 (uint8 mode)
        pack packet 10 delay

        packet

    let set_door_passcodes_request
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

    let get_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARDS)
        pack packet 4 controller

        packet

    let get_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARD)
        pack packet 4 controller
        pack packet 8 card

        packet

    let get_card_at_index_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_CARD_AT_INDEX)
        pack packet 4 controller
        pack packet 8 index

        packet

    let put_card_request
        (controller: uint32)
        (card: uint32)
        (start_date: DateOnly)
        (end_date: DateOnly)
        (door1: uint8)
        (door2: uint8)
        (door3: uint8)
        (door4: uint8)
        (pin: uint32)
        =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.PUT_CARD)
        pack packet 4 controller
        pack packet 8 card
        pack packet 12 (Nullable start_date)
        pack packet 16 (Nullable end_date)
        pack packet 20 door1
        pack packet 21 door2
        pack packet 22 door3
        pack packet 23 door4
        pack packet 24 pin

        packet

    let delete_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.DELETE_CARD)
        pack packet 4 controller
        pack packet 8 card

        packet

    let delete_all_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.DELETE_ALL_CARDS)
        pack packet 4 controller
        pack packet 8 MAGIC_WORD

        packet

    let get_event_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_EVENT)
        pack packet 4 controller
        pack packet 8 index

        packet

    let get_event_index_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_EVENT_INDEX)
        pack packet 4 controller

        packet

    let set_event_index_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_EVENT_INDEX)
        pack packet 4 controller
        pack packet 8 index
        pack packet 12 MAGIC_WORD

        packet

    let record_special_events_request (controller: uint32) (enabled: bool) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.RECORD_SPECIAL_EVENTS)
        pack packet 4 controller
        pack packet 8 enabled

        packet

    let get_time_profile_request (controller: uint32) (profile: uint8) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.GET_TIME_PROFILE)
        pack packet 4 controller
        pack packet 8 profile

        packet

    let set_time_profile_request (controller: uint32) (profile: TimeProfile) =
        let packet: byte array = Array.zeroCreate 64

        pack packet 0 (byte messages.SOM)
        pack packet 1 (byte messages.SET_TIME_PROFILE)
        pack packet 4 controller
        pack packet 8 profile.profile
        pack packet 9 profile.start_date
        pack packet 13 profile.end_date
        pack packet 17 profile.monday
        pack packet 18 profile.tuesday
        pack packet 19 profile.wednesday
        pack packet 20 profile.thursday
        pack packet 21 profile.friday
        pack packet 22 profile.saturday
        pack packet 23 profile.sunday
        pack packet 24 profile.segment1_start
        pack packet 26 profile.segment1_end
        pack packet 28 profile.segment2_start
        pack packet 30 profile.segment2_end
        pack packet 32 profile.segment3_start
        pack packet 34 profile.segment3_end
        pack packet 36 profile.linked_profile

        packet

    let clear_time_profiles_request (controller: uint32) =
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
        pack packet 8 task.start_date
        pack packet 12 task.end_date
        pack packet 16 task.monday
        pack packet 17 task.tuesday
        pack packet 18 task.wednesday
        pack packet 19 task.thursday
        pack packet 20 task.friday
        pack packet 21 task.saturday
        pack packet 22 task.sunday
        pack packet 23 task.start_time
        pack packet 25 task.door
        pack packet 26 task.task
        pack packet 27 task.more_cards

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
