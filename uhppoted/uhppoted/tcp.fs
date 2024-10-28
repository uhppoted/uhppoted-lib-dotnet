namespace uhppoted

open System
open System.Net
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks

module TCP =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:x2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let rec receive (stream: NetworkStream) : Async<Result<byte array, string>> =
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
                return Error err.Message
        }

    let send_to (request: byte array, addrPort: IPEndPoint, timeout: int, debug: bool) =
        let timer (timeout: int) : Async<Result<byte array * IPEndPoint, string>> =
            async {
                do! Async.Sleep timeout
                return Error "timeout"
            }

        try
            let socket = new TcpClient()

            socket.Connect(addrPort)

            let stream = socket.GetStream()
            let rx = receive stream |> Async.StartAsTask
            let rx_timeout = timer timeout |> Async.StartAsTask

            stream.Write(request) |> ignore

            if debug then
                printfn "    ... sent %d bytes to %A" request.Length addrPort.Address
                dump request

            let completed =
                Task.WhenAny(rx, rx_timeout) |> Async.AwaitTask |> Async.RunSynchronously

            socket.Close()

            if completed = rx then
                match rx.Result with
                | Ok(packet) when packet.Length = 64 ->
                    if debug then
                        printfn "    ... received %d bytes from %A" packet.Length addrPort.Address
                        dump packet

                    Ok packet
                | Ok(_) -> Error "invalid packet"
                | Error err -> Error err
            else
                Error "timeout waiting for reply from controller"

        with error ->
            Error error.Message
