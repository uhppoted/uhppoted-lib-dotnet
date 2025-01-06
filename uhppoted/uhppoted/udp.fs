namespace uhppoted

open System
open System.Net
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks

module internal UDP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:x2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let rec receiveAll (socket: UdpClient) (closing: CancellationToken) (packets: (byte[] * IPEndPoint) list) =
        async {
            try
                if closing.IsCancellationRequested || socket.Client.SafeHandle.IsInvalid then
                    return ()

                let! (packet, remote) =
                    Async.FromBeginEnd(
                        (fun (callback, state) -> socket.BeginReceive(callback, state)),
                        (fun (iar) ->
                            let addr = ref Unchecked.defaultof<IPEndPoint>
                            let packet = socket.EndReceive(iar, addr)
                            (packet, !addr))
                    )

                return! receiveAll socket closing ((packet, remote) :: packets)

            with
            | :? ObjectDisposedException -> return Ok(List.rev packets)
            | err -> return Error(ReceiveError err.Message)
        }

    let rec receive (socket: UdpClient) : Async<Result<byte array * IPEndPoint, Err>> =
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

                if packet.Length = 64 then
                    return Ok(packet, remote)
                else
                    return! receive socket

            with err ->
                return Error(ReceiveError err.Message)
        }

    let rec receiveEvent
        (socket: UdpClient)
        (handler: byte array * IPEndPoint -> unit)
        (closing: CancellationToken)
        : Async<Result<unit, Err>> =
        async {
            try
                if closing.IsCancellationRequested || socket.Client.SafeHandle.IsInvalid then
                    return ()

                let! (packet, remote) =
                    Async.FromBeginEnd(
                        (fun (callback, state) -> socket.BeginReceive(callback, state)),
                        (fun (iar) ->
                            let addr = ref Unchecked.defaultof<IPEndPoint>
                            let packet = socket.EndReceive(iar, addr)
                            (packet, !addr))
                    )

                if packet.Length = 64 then
                    handler (packet, remote)

                return! receiveEvent socket handler closing

            with
            | :? ObjectDisposedException -> return Ok()
            | err -> return Error(ReceiveError err.Message)
        }

    let broadcast (request: byte array, bind: IPEndPoint, broadcast: IPEndPoint, timeout: int, debug: bool) =
        let socket = new UdpClient(bind)
        let closing = new CancellationTokenSource()

        try
            let rx = receiveAll socket closing.Token [] |> Async.StartAsTask

            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
            socket.EnableBroadcast <- true
            socket.Send(request, request.Length, broadcast) |> ignore

            if debug then
                printfn "    ... sent %d bytes to %A" request.Length broadcast.Address
                dump request

            Thread.Sleep timeout
            closing.Cancel()
            socket.Close()

            match rx.Result with
            | Ok received ->
                let replies = received |> List.filter (fun (packet, address) -> packet.Length = 64)

                if debug then
                    replies
                    |> List.iter (fun (packet, addr) ->
                        printfn "    ... received %d bytes from %A" packet.Length addr.Address
                        dump packet)

                replies |> List.map fst |> Ok
            | Error err -> Error err

        with err ->
            Error(ReceiveError err.Message)

    let broadcastTo (request: byte array, bind: IPEndPoint, broadcast: IPEndPoint, timeout: int, debug: bool) =
        try
            let x: Err = Err.Timeout
            let socket = new UdpClient(bind)

            let timer (timeout: int) : Async<Result<byte array * IPEndPoint, Err>> =
                async {
                    do! Async.Sleep timeout
                    return Error Err.Timeout
                }

            try
                try
                    let rx = receive socket |> Async.StartAsTask
                    let rx_timeout = timer timeout |> Async.StartAsTask

                    socket.EnableBroadcast <- true
                    socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
                    socket.Send(request, request.Length, broadcast) |> ignore

                    if debug then
                        printfn "    ... sent %d bytes to %A" request.Length broadcast.Address
                        dump request

                    // set-IPv4 does not return a reply
                    if request[1] = 0x96uy then
                        Ok([||])
                    else
                        let completed =
                            Task.WhenAny(rx, rx_timeout) |> Async.AwaitTask |> Async.RunSynchronously

                        if completed = rx then
                            match rx.Result with
                            | Ok(packet, addr) when packet.Length = 64 ->
                                if debug then
                                    printfn "    ... received %d bytes from %A" packet.Length addr.Address
                                    dump packet

                                Ok packet
                            | Ok(_, _) -> Error Err.InvalidPacket
                            | Error err -> Error err
                        else
                            Error Err.Timeout

                with err ->
                    Error(ReceiveError err.Message)
            finally
                socket.Close()
        with err ->
            Error(ReceiveError err.Message)

    let sendTo (request: byte array, src: IPEndPoint, dest: IPEndPoint, timeout: int, debug: bool) =
        let socket = new UdpClient(src)

        let timer (timeout: int) : Async<Result<byte array * IPEndPoint, Err>> =
            async {
                do! Async.Sleep timeout
                return Error Err.Timeout
            }

        try
            try
                socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
                socket.Connect(dest)

                let rx = receive socket |> Async.StartAsTask
                let rx_timeout = timer timeout |> Async.StartAsTask

                socket.Send(request, request.Length) |> ignore

                if debug then
                    printfn "    ... sent %d bytes to %A" request.Length dest.Address
                    dump request

                // set-IPv4 does not return a reply
                if request[1] = 0x96uy then
                    Ok([||])
                else
                    let completed =
                        Task.WhenAny(rx, rx_timeout) |> Async.AwaitTask |> Async.RunSynchronously

                    if completed = rx then
                        match rx.Result with
                        | Ok(packet, addr) when packet.Length = 64 ->
                            if debug then
                                printfn "    ... received %d bytes from %A" packet.Length addr.Address
                                dump packet

                            Ok packet
                        | Ok(_, _) -> Error InvalidPacket
                        | Error err -> Error err
                    else
                        Error Err.Timeout

            with err ->
                Error(ReceiveError err.Message)
        finally
            socket.Close()

    let listen (bind: IPEndPoint) (callback: byte array -> unit) (token: CancellationToken) (debug: bool) =
        let socket = new UdpClient(bind)
        let closing = new CancellationTokenSource()

        socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)

        let handler (packet: byte array, addr: IPEndPoint) =
            if debug then
                printfn "    ... event %d bytes from %A" packet.Length addr.Address
                dump packet

            callback packet

        try
            receiveEvent socket handler closing.Token |> Async.StartAsTask |> ignore

            token.WaitHandle.WaitOne() |> ignore
            closing.Cancel()
            socket.Close()
            Ok()

        with err ->
            Error(ListenError err.Message)
