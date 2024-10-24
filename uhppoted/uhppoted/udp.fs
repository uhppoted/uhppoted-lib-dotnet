namespace uhppoted

open System
open System.Net
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks

module UDP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:x2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let rec receive_all (socket: UdpClient) (packets: (byte[] * IPEndPoint) list) =
        async {
            try
                let! (packet, remote) =
                    Async.FromBeginEnd(
                        (fun (callback, state) -> socket.BeginReceive(callback, state)),
                        (fun (iar) ->
                            let addr = ref Unchecked.defaultof<IPEndPoint>
                            let packet = socket.EndReceive(iar, addr)
                            (packet, !addr))
                    )

                return! receive_all socket ((packet, remote) :: packets)

            with
            | :? ObjectDisposedException -> return List.rev packets
            | err ->
                printfn "** ERROR  %s" err.Message
                return List.rev packets
        }

    let receive (socket: UdpClient) : Async<Result<byte array * IPEndPoint, string>> =
        async {
            try
                let! (packet, remote) =
                    Async.FromBeginEnd(
                        (fun (callback, state) -> socket.BeginReceive(callback, state)),
                        (fun (iar) ->
                            let addr = ref Unchecked.defaultof<IPEndPoint>
                            let packet = socket.EndReceive(iar, addr)
                            (packet, !addr))
                    )

                return Ok((packet, remote))
            with err ->
                return Error err.Message
        }

    let broadcast (request: byte array, addr: string, port: int, timeout: int, debug: bool) : byte array list =
        let socket = new UdpClient()

        try
            let rx = receive_all socket [] |> Async.StartAsTask

            socket.EnableBroadcast <- true
            socket.Send(request, request.Length, addr, port) |> ignore

            if debug then
                printfn "    ... sent %d bytes to %s" request.Length addr
                dump request

            Thread.Sleep timeout
            socket.Close()

            let received = rx.Result
            let received = received |> List.filter (fun (packet, address) -> packet.Length = 64)

            if debug then
                received
                |> List.iter (fun (packet, addr) ->
                    printfn "    ... received %d bytes from %A" packet.Length addr
                    dump packet)

            received |> List.map fst

        with error ->
            printfn "%s" error.Message
            []

    let broadcast_to (request: byte array, addr: string, port: int, timeout: int, debug: bool) =
        let timer (timeout: int) : Async<Result<byte array * IPEndPoint, string>> =
            async {
                do! Async.Sleep timeout
                return Error "timeout"
            }

        try
            let socket = new UdpClient()
            let rx = receive socket |> Async.StartAsTask
            let rx_timeout = timer timeout |> Async.StartAsTask

            socket.EnableBroadcast <- true
            socket.Send(request, request.Length, addr, port) |> ignore

            if debug then
                printfn "    ... sent %d bytes to %s" request.Length addr
                dump request

            let completed =
                Task.WhenAny(rx, rx_timeout) |> Async.AwaitTask |> Async.RunSynchronously

            socket.Close()

            if completed = rx then
                match rx.Result with
                | Ok(packet, addr) when packet.Length = 64 ->
                    if debug then
                        printfn "    ... received %d bytes from %A" packet.Length addr
                        dump packet

                    Ok packet
                | Ok(_, _) -> Error "invalid packet"
                | Error err -> Error err
            else
                Error "timeout waiting for reply from controller"

        with error ->
            Error error.Message
