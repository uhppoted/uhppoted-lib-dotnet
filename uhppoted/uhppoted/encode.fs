namespace uhppoted

open System
open System.Net

module Encode =
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

    let packDateTime (v: DateTime) =
        let bcd = v.ToString("yyyyMMddHHmmss")
        let bytes = Convert.FromHexString bcd

        bytes

    let get_controller_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x94)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_IPv4_request (controller: uint32) address netmask gateway =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x96)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packIPv4 address) 0 packet 8 4
        Array.blit (packIPv4 netmask) 0 packet 12 4
        Array.blit (packIPv4 gateway) 0 packet 16 4
        Array.blit (packU32 MAGIC_WORD) 0 packet 20 4

        packet

    let get_listener_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x92)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_listener_request (controller: uint32) (address: IPAddress) (port: uint16) (interval: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x90)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packIPv4 address) 0 packet 8 4
        Array.blit (packU16 port) 0 packet 12 2
        Array.blit (packU8 interval) 0 packet 14 1

        packet

    let get_time_request (controller: uint32) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x32)

        Array.blit (packU32 controller) 0 packet 4 4

        packet

    let set_time_request (controller: uint32) (datetime: DateTime) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x30)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packDateTime datetime) 0 packet 8 7

        packet

    let get_door_settings_request (controller: uint32) (door: uint8) =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x82)

        Array.blit (packU32 controller) 0 packet 4 4
        Array.blit (packU8 door) 0 packet 8 1

        packet
