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

    let pack_u8 (v: uint8) = [| (byte ((v >>> 0) &&& 0x00ffuy)) |]

    let pack_u16 (v: uint16) =
        [| (byte ((v >>> 0) &&& 0x00ffus)); (byte ((v >>> 8) &&& 0x00ffus)) |]

    let pack_u32 (v: uint32) =
        [| (byte ((v >>> 0) &&& 0x00ffu))
           (byte ((v >>> 8) &&& 0x00ffu))
           (byte ((v >>> 16) &&& 0x00ffu))
           (byte ((v >>> 24) &&& 0x00ffu)) |]

    let pack_bool (v: bool) =
        match v with
        | true -> [| 0x01uy |]
        | false -> [| 0x00uy |]


    let pack_IPv4 (v: IPAddress) = v.MapToIPv4().GetAddressBytes()

    let pack_date (v: Nullable<DateOnly>) =
        match v.HasValue with
        | true -> bcd (v.Value.ToString "yyyyMMdd")
        | false -> [| 0uy; 0uy; 0uy; 0uy; 0uy |]

    let pack_datetime (v: DateTime) = bcd (v.ToString "yyyyMMddHHmmss")

    let pack_HHmm (v: Nullable<TimeOnly>) =
        match v.HasValue with
        | true -> bcd (v.Value.ToString "HHmm")
        | false -> [| 0uy; 0uy |]

    let get_controller_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CONTROLLER)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let set_IPv4_request (controller: uint32) address netmask gateway =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_IPv4)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_IPv4 address) 0 packet 8 4
        Array.blit (pack_IPv4 netmask) 0 packet 12 4
        Array.blit (pack_IPv4 gateway) 0 packet 16 4
        Array.blit (pack_u32 MAGIC_WORD) 0 packet 20 4

        packet

    let get_listener_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_LISTENER)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let set_listener_request (controller: uint32) (address: IPAddress) (port: uint16) (interval: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_LISTENER)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_IPv4 address) 0 packet 8 4
        Array.blit (pack_u16 port) 0 packet 12 2
        Array.blit (pack_u8 interval) 0 packet 14 1

        packet

    let get_time_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_TIME)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let set_time_request (controller: uint32) (datetime: DateTime) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_TIME)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_datetime datetime) 0 packet 8 7

        packet

    let get_door_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_DOOR)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 door) 0 packet 8 1

        packet

    let set_door_request (controller: uint32) (door: uint8) (mode: uint8) (delay: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_DOOR)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 door) 0 packet 8 1
        Array.blit (pack_u8 mode) 0 packet 9 1
        Array.blit (pack_u8 delay) 0 packet 10 1

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

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_DOOR_PASSCODES)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 door) 0 packet 8 1

        if passcode1 <= 999999u then
            Array.blit (pack_u32 passcode1) 0 packet 12 4

        if passcode2 <= 999999u then
            Array.blit (pack_u32 passcode2) 0 packet 16 4

        if passcode3 <= 999999u then
            Array.blit (pack_u32 passcode3) 0 packet 20 4

        if passcode4 <= 999999u then
            Array.blit (pack_u32 passcode4) 0 packet 24 4

        packet

    let open_door_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.OPEN_DOOR)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 door) 0 packet 8 1

        packet

    let get_status_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_STATUS)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let get_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARDS)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let get_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARD)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 card) 0 packet 8 4

        packet

    let get_card_at_index_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARD_AT_INDEX)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 index) 0 packet 8 4

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

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.PUT_CARD)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 card) 0 packet 8 4
        Array.blit (pack_date (Nullable start_date)) 0 packet 12 4
        Array.blit (pack_date (Nullable end_date)) 0 packet 16 4
        Array.blit (pack_u8 door1) 0 packet 20 1
        Array.blit (pack_u8 door2) 0 packet 21 1
        Array.blit (pack_u8 door3) 0 packet 22 1
        Array.blit (pack_u8 door4) 0 packet 23 1
        Array.blit (pack_u32 pin) 0 packet 24 4

        packet

    let delete_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.DELETE_CARD)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 card) 0 packet 8 4

        packet

    let delete_all_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.DELETE_ALL_CARDS)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 MAGIC_WORD) 0 packet 8 4

        packet

    let get_event_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_EVENT)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 index) 0 packet 8 4

        packet

    let get_event_index_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_EVENT_INDEX)

        Array.blit (pack_u32 controller) 0 packet 4 4

        packet

    let set_event_index_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_EVENT_INDEX)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 index) 0 packet 8 4
        Array.blit (pack_u32 MAGIC_WORD) 0 packet 12 4

        packet

    let record_special_events_request (controller: uint32) (enabled: bool) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.RECORD_SPECIAL_EVENTS)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_bool enabled) 0 packet 8 1

        packet

    let get_time_profile_request (controller: uint32) (profile: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_TIME_PROFILE)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 profile) 0 packet 8 1

        packet

    let set_time_profile_request (controller: uint32) (profile: TimeProfile) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_TIME_PROFILE)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u8 profile.profile) 0 packet 8 1
        Array.blit (pack_date profile.start_date) 0 packet 9 4
        Array.blit (pack_date profile.end_date) 0 packet 13 4
        Array.blit (pack_bool profile.monday) 0 packet 17 1
        Array.blit (pack_bool profile.tuesday) 0 packet 18 1
        Array.blit (pack_bool profile.wednesday) 0 packet 19 1
        Array.blit (pack_bool profile.thursday) 0 packet 20 1
        Array.blit (pack_bool profile.friday) 0 packet 21 1
        Array.blit (pack_bool profile.saturday) 0 packet 22 1
        Array.blit (pack_bool profile.sunday) 0 packet 23 1
        Array.blit (pack_HHmm profile.segment1_start) 0 packet 24 2
        Array.blit (pack_HHmm profile.segment1_end) 0 packet 26 2
        Array.blit (pack_HHmm profile.segment2_start) 0 packet 28 2
        Array.blit (pack_HHmm profile.segment2_end) 0 packet 30 2
        Array.blit (pack_HHmm profile.segment3_start) 0 packet 32 2
        Array.blit (pack_HHmm profile.segment3_end) 0 packet 34 2
        Array.blit (pack_u8 profile.linked_profile) 0 packet 36 1

        packet

    let clear_time_profiles_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.CLEAR_TIME_PROFILES)

        Array.blit (pack_u32 controller) 0 packet 4 4
        Array.blit (pack_u32 MAGIC_WORD) 0 packet 8 4

        packet
