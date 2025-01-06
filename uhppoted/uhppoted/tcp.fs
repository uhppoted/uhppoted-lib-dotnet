namespace uhppoted

open System
open System.Net
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks

module internal TCP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:x2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let rec receive (stream: NetworkStream) : Async<Result<byte array, Err>> =
        async {
            try
                let! packet =
                    let buffer = Array.zeroCreate 1024

                    Async.FromBeginEnd(
                        (fun (callback, state) -> stream.BeginRead(buffer, 0, buffer.Length, callback, state)),
                        (fun (iar) ->
                            let N = stream.EndRead(iar)
                            buffer[0 .. N - 1])
                    )

                if packet.Length = 64 then
                    return Ok(packet)
                else
                    return! receive stream

            with err ->
                return Error(ReceiveError err.Message)
        }

    let sendTo (request: byte array, src: IPEndPoint, dest: IPEndPoint, timeout: int, debug: bool) =
        let socket = new TcpClient(src)

        let timer (timeout: int) : Async<Result<byte array * IPEndPoint, Err>> =
            async {
                do! Async.Sleep timeout
                return Error Err.Timeout
            }

        try
            try
                socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
                socket.Connect(dest)

                let stream = socket.GetStream()
                let rx = receive stream |> Async.StartAsTask
                let rx_timeout = timer timeout |> Async.StartAsTask

                stream.Write(request) |> ignore

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
                        | Ok(packet) when packet.Length = 64 ->
                            if debug then
                                printfn "    ... received %d bytes from %A" packet.Length dest.Address
                                dump packet

                            Ok packet
                        | Ok(_) -> Error InvalidPacket
                        | Error err -> Error err
                    else
                        Error Err.Timeout

            with err ->
                Error(ReceiveError err.Message)

        finally
            socket.Close()
