namespace uhppoted

open System
open System.Net
open System.Net.Sockets
open System.Threading

module UDP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:X2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let rec receive_all (socket: UdpClient) (packets: (byte[] * IPEndPoint) list) =
        async {
            try
                let! result =
                    Async.FromBeginEnd(
                        (fun (callback, state) -> socket.BeginReceive(callback, state)),
                        (fun (iar) ->
                            let addr = ref Unchecked.defaultof<IPEndPoint>
                            let packet = socket.EndReceive(iar, addr)
                            (packet, !addr))
                    )

                let (packet, remote) = result

                return! receive_all socket ((packet, remote) :: packets)

            with
            | :? ObjectDisposedException -> return List.rev packets
            | err ->
                printfn "** ERROR  %s" err.Message
                return List.rev packets
        }

    let broadcast (request: byte array, addr: string, port: int, timeout: int, debug: bool) : byte array list =
        let socket = new UdpClient()

        if debug then
            printfn "    ... sent %d bytes to %A" request.Length addr
            dump request

        try
            let rx = receive_all socket [] |> Async.StartAsTask

            socket.EnableBroadcast <- true
            socket.Send(request, request.Length, addr, port) |> ignore

            Thread.Sleep timeout
            socket.Close()

            let received = rx.Result
            let received = received |> List.filter (fun (packet, address) -> packet.Length = 64)

            if debug then
                received
                |> List.iter (fun (packet, address) ->
                    printfn "    ... received %d bytes from %A" packet.Length address
                    dump packet)

            received |> List.map fst

        with error ->
            printfn "%s" error.Message
            []
