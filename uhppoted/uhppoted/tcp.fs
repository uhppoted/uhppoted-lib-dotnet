namespace uhppoted

open System
open System.IO
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

    let rec receive (stream: NetworkStream) (cancel: CancellationToken) : Async<Result<Option<byte array>, Err>> =
        async {
            try
                let buffer = Array.zeroCreate<byte> 1024
                let! N = stream.ReadAsync(buffer, 0, buffer.Length, cancel) |> Async.AwaitTask

                if N <= 0 then return Ok(None)
                else if N = 64 then return Ok(Some(buffer.[0..63]))
                else return! receive stream cancel

            with
            | :? OperationCanceledException -> return Ok(None)

            | :? IOException as x -> return Error(ReceiveError x.Message)

            | :? SocketException as x ->
                stream.Close()
                return Error(ReceiveError x.Message)

            | err -> return Error(ReceiveError err.Message)
        }

    let sendTo (request: byte array, src: IPEndPoint, dest: IPEndPoint, timeout: int, debug: bool) =
        let mutable sockets: TcpClient list = []

        use _ =
            { new System.IDisposable with
                member _.Dispose() =
                    sockets |> List.iter (fun s -> s.Dispose()) }

        let bind =
            let rec loop retries =
                let socket = new TcpClient(src)
                let endpoint = socket.Client.LocalEndPoint :?> IPEndPoint

                sockets <- socket :: sockets

                if endpoint = dest && src.Port <> 0 then
                    failwithf "TCP bind address:port %A is the same as destination" endpoint
                elif endpoint = dest && retries >= 5 then
                    failwithf "TCP bind address:port %A is the same as destination" endpoint
                elif endpoint = dest then
                    loop (retries + 1)
                else
                    socket

            loop 0

        try
            let socket = bind
            let cancel = new CancellationTokenSource()

            let timer (timeout: int) : Async<Result<byte array * IPEndPoint, Err>> =
                async {
                    do! Async.Sleep timeout
                    return Error Err.Timeout
                }

            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)
            socket.Connect(dest)

            let stream = socket.GetStream()
            let rx = receive stream cancel.Token |> Async.StartAsTask
            let rx_timeout = timer timeout |> Async.StartAsTask

            stream.Write(request) |> ignore

            if debug then
                printfn "    ... sent %d bytes to %A" request.Length dest.Address
                dump request

            // NTS: set-IPv4 does not return a reply
            if request[1] = 0x96uy then
                cancel.Cancel()
                Ok([||])
            else
                let completed =
                    Task.WhenAny(rx, rx_timeout) |> Async.AwaitTask |> Async.RunSynchronously

                if completed = rx then
                    match rx.Result with
                    | Ok(Some packet) ->
                        if debug then
                            printfn "    ... received %d bytes from %A" packet.Length dest.Address
                            dump packet

                        Ok packet
                    | Ok None -> Error InvalidPacket
                    | Error err -> Error err
                else
                    cancel.Cancel()
                    Error Err.Timeout

        with err ->
            Error(ReceiveError err.Message)
