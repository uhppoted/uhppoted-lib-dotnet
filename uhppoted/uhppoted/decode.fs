namespace uhppoted

open System
open System.Net

module Decode =
    let unpackU16 (slice: byte array) =
        let u16 = uint16 0
        let u16 = u16 + uint16 slice[0]
        let u16 = u16 + (uint16 slice[1] <<< 8)
        u16

    let unpackU32 (slice: byte array) =
        let u32 = uint32 0
        let u32 = u32 + uint32 slice[0]
        let u32 = u32 + (uint32 slice[1] <<< 8)
        let u32 = u32 + (uint32 slice[2] <<< 16)
        let u32 = u32 + (uint32 slice[3] <<< 24)
        u32

    let unpackIPv4 (slice: byte array) =
        IPAddress(slice[0..3])

    let unpack_version (slice: byte array) =
        $"v%x{slice[0]}.%02x{slice[1]}"

    let unpackMAC (slice: byte array) =
        let MAC: byte array = Array.zeroCreate 6
        MAC[0] <- slice[0]
        MAC[1] <- slice[1]
        MAC[2] <- slice[2]
        MAC[3] <- slice[3]
        MAC[4] <- slice[4]
        MAC[5] <- slice[5]
        MAC
        
    let unpack_date (slice: byte array) =
        try
          let bcd = $"%02x{slice[0]}%02x{slice[1]}-%02x{slice[2]}-%02x{slice[3]}"
          let date = DateOnly.ParseExact(bcd, "yyyy-MM-dd")
          Some(date)
        with _ ->
           None

    let get_controller_response (packet: byte array) : GetControllerResponse  = 
        { 
          controller = unpackU32 packet[4..7]
          address = unpackIPv4 packet[8..11]
          netmask = unpackIPv4 packet[12..15]
          gateway = unpackIPv4 packet[16..19]
          MAC = unpackMAC packet[20..25]
          version = unpack_version packet[26..27]
          date = unpack_date(packet[28..31])
        }
