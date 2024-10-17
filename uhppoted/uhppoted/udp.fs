namespace uhppoted

open System
open System.Net.Sockets

module UDP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:X2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let broadcast (request: byte array) =
        let socket = new UdpClient()
        let addr = "192.168.1.255"
        let port = 60000

        dump request

        try
            socket.EnableBroadcast <- true
            socket.Send(request, request.Length, addr, port) |> ignore
        with error ->
            printfn "%s" error.Message
