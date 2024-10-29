namespace uhppoted

open System.Net

module Encode =
    [<Literal>]
    let MAGIC_WORD = 0x55aaaa55u

    let packU32 (v: uint32) =
        [| (byte ((v >>> 0) &&& 0x00ffu))
           (byte ((v >>> 8) &&& 0x00ffu))
           (byte ((v >>> 16) &&& 0x00ffu))
           (byte ((v >>> 24) &&& 0x00ffu)) |]

    let packIPv4 (v: IPAddress) = v.MapToIPv4().GetAddressBytes()

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
