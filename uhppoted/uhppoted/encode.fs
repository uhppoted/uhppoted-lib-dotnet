namespace uhppoted

module Encode =
    let packU32 v =
        [| (byte ((v >>> 0) &&& 0x00ff))
           (byte ((v >>> 8) &&& 0x00ff))
           (byte ((v >>> 16) &&& 0x00ff))
           (byte ((v >>> 24) &&& 0x00ff)) |]

    let get_controller_request controller =
        let packet: byte array = Array.zeroCreate 64

        Array.set packet 0 (byte 0x17)
        Array.set packet 1 (byte 0x94)

        Array.blit (packU32 controller) 0 packet 4 4

        packet
