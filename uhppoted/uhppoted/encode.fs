namespace uhppoted

open System
open System.Net
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal Encode =
    [<Literal>]
    let MAGIC_WORD = 0x55aaaa55u

    let packU8 (v: uint8) = [| (byte ((v >>> 0) &&& 0x00ffuy)) |]

    let packU16 (v: uint16) =
        [| (byte ((v >>> 0) &&& 0x00ffus)); (byte ((v >>> 8) &&& 0x00ffus)) |]

    let packU32 (v: uint32) =
        [| (byte ((v >>> 0) &&& 0x00ffu))
           (byte ((v >>> 8) &&& 0x00ffu))
           (byte ((v >>> 16) &&& 0x00ffu))
           (byte ((v >>> 24) &&& 0x00ffu)) |]

    let packIPv4 (v: IPAddress) = v.MapToIPv4().GetAddressBytes()

    let packDate (v: DateOnly) =
        let bcd = v.ToString("yyyyMMdd")
        let bytes = Convert.FromHexString bcd

        bytes

    let packDateTime (v: DateTime) =
        let bcd = v.ToString("yyyyMMddHHmmss")
        let bytes = Convert.FromHexString bcd

        bytes

    let get_controller_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CONTROLLER)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_IPv4_request (controller: uint32) address netmask gateway =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_IPv4)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packIPv4 address) 0 packet 8 4
        Array.blit (packIPv4 netmask) 0 packet 12 4
        Array.blit (packIPv4 gateway) 0 packet 16 4
        Array.blit (packU32 MAGIC_WORD) 0 packet 20 4

        packet

    let get_listener_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_LISTENER)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_listener_request (controller: uint32) (address: IPAddress) (port: uint16) (interval: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_LISTENER)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packIPv4 address) 0 packet 8 4
        Array.blit (packU16 port) 0 packet 12 2
        Array.blit (packU8 interval) 0 packet 14 1

        packet

    let get_time_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_TIME)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_time_request (controller: uint32) (datetime: DateTime) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_TIME)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packDateTime datetime) 0 packet 8 7

        packet

    let get_door_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_DOOR)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU8 door) 0 packet 8 1

        packet

    let set_door_request (controller: uint32) (door: uint8) (mode: uint8) (delay: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.SET_DOOR)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU8 door) 0 packet 8 1
        Array.blit (packU8 mode) 0 packet 9 1
        Array.blit (packU8 delay) 0 packet 10 1

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

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU8 door) 0 packet 8 1

        if passcode1 <= 999999u then
            Array.blit (packU32 passcode1) 0 packet 12 4

        if passcode2 <= 999999u then
            Array.blit (packU32 passcode2) 0 packet 16 4

        if passcode3 <= 999999u then
            Array.blit (packU32 passcode3) 0 packet 20 4

        if passcode4 <= 999999u then
            Array.blit (packU32 passcode4) 0 packet 24 4

        packet

    let open_door_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.OPEN_DOOR)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU8 door) 0 packet 8 1

        packet

    let get_status_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_STATUS)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let get_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARDS)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let get_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARD)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 card) 0 packet 8 4

        packet

    let get_card_at_index_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_CARD_AT_INDEX)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 index) 0 packet 8 4

        packet

    let put_card_request
        (controller: uint32)
        (card: uint32)
        (startdate: DateOnly)
        (enddate: DateOnly)
        (door1: uint8)
        (door2: uint8)
        (door3: uint8)
        (door4: uint8)
        (pin: uint32)
        =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.PUT_CARD)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 card) 0 packet 8 4
        Array.blit (packDate startdate) 0 packet 12 4
        Array.blit (packDate enddate) 0 packet 16 4
        Array.blit (packU8 door1) 0 packet 20 1
        Array.blit (packU8 door2) 0 packet 21 1
        Array.blit (packU8 door3) 0 packet 22 1
        Array.blit (packU8 door4) 0 packet 23 1
        Array.blit (packU32 pin) 0 packet 24 4

        packet

    let delete_card_request (controller: uint32) (card: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.DELETE_CARD)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 card) 0 packet 8 4

        packet

    let delete_all_cards_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.DELETE_ALL_CARDS)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 MAGIC_WORD) 0 packet 8 4

        packet

    let get_event_request (controller: uint32) (index: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte messages.SOM)
        Array.set packet 1 (byte messages.GET_EVENT)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU32 index) 0 packet 8 4

        packet
