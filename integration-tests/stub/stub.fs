namespace stub

open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading
open System.Threading.Tasks

type Emulator =
    { udp: Option<(UdpClient * Task * CancellationTokenSource)>
      tcp: Option<(TcpListener * Task * CancellationTokenSource)> }

module Stub =
    let dump packet (logger: TextWriter) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:x2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            logger.WriteLine("    {0}  {1}", left, right)

    let find packet =
        match Messages.find packet with
        | Some(responses) -> Some(responses)
        | _ -> None

    let rec recv (socket: UdpClient) (cancel: CancellationToken) (logger: TextWriter) =
        async {
            try
                let! result = socket.ReceiveAsync(cancel).AsTask() |> Async.AwaitTask

                match result.Buffer.Length with
                | 64 ->
                    let packet = result.Buffer.[0..63]
                    let remote = result.RemoteEndPoint

                    match find packet with
                    | Some(replies) ->
                        for reply in replies do
                            socket.Send(reply, reply.Length, remote) |> ignore
                    | _ ->
                        logger.WriteLine("*** ERROR unknown packet")
                        dump packet logger

                | N -> logger.WriteLine($"*** WARN invalid packet size {N}")

                return! recv socket cancel logger
            with
            | :? OperationCanceledException -> socket.Close()

            | :? ObjectDisposedException -> socket.Close()

            | :? SocketException as x ->
                logger.WriteLine($"** ERROR udp::recv socket exception: {x.Message}")
                socket.Close()

            | err ->
                logger.WriteLine($"** ERROR udp::recv {err.Message}")
                socket.Close()
        }

    let rec read (stream: NetworkStream) (cancel: CancellationToken) (logger: TextWriter) =
        async {
            try
                let buffer = Array.zeroCreate<byte> 1024
                let! N = stream.ReadAsync(buffer, 0, buffer.Length, cancel) |> Async.AwaitTask

                if N <= 0 then
                    stream.Close()
                    return ()
                else if N <> 64 then
                    logger.WriteLine($"*** WARN invalid packet size {N}")
                else
                    let packet = buffer.[0..63]

                    match find packet with
                    | Some(replies) ->
                        for reply in replies do
                            stream.Write(reply) |> ignore
                    | _ ->
                        logger.WriteLine("*** ERROR unknown packet")
                        dump packet logger

                    return! read stream cancel logger
            with
            | :? OperationCanceledException ->
                logger.WriteLine("** INFO TCP receive operation cancelled.")
                stream.Close()

            | :? IOException as x ->
                logger.WriteLine($"** ERROR tcp::recv IO exception: {x.Message}")
                stream.Close()

            | :? SocketException as x ->
                logger.WriteLine($"** ERROR tcp::recv socket exception: {x.Message}")
                stream.Close()

            | err ->
                logger.WriteLine($"** ERROR tcp::recv {err.Message}")
                stream.Close()
        }

    let listen (socket: TcpListener) (cancel: CancellationToken) (logger: TextWriter) =
        async {
            try
                socket.Start()

                while true do
                    let! connection = (socket.AcceptTcpClientAsync(cancel).AsTask()) |> Async.AwaitTask
                    let stream = connection.GetStream()

                    read stream cancel logger |> Async.Start

            with
            | :? OperationCanceledException -> socket.Server.Close()
            | :? ObjectDisposedException -> socket.Server.Close()
            | :? SocketException as x when x.SocketErrorCode = SocketError.OperationAborted ->
                logger.WriteLine("** INFO tcp::accept operation aborted.")
                socket.Server.Close()
            | err ->
                logger.WriteLine($"** ERROR tcp::accept {err.Message}")
                socket.Server.Close()
        }

    let init (mode: string) (logger: TextWriter) : Result<Emulator, string> =
        match mode with
        | "broadcast" ->
            let cts = new CancellationTokenSource()
            let udp = new UdpClient(39999)

            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)

            let udprx = recv udp cts.Token logger |> Async.StartAsTask

            Ok
                { udp = Some(udp, udprx, cts)
                  tcp = None }

        | "connected udp" ->
            let cts = new CancellationTokenSource()
            let udp = new UdpClient(39998)

            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)

            let udprx = recv udp cts.Token logger |> Async.StartAsTask

            Ok
                { udp = Some(udp, udprx, cts)
                  tcp = None }

        | "tcp" ->
            let cts = new CancellationTokenSource()
            let tcp = new TcpListener(IPAddress.Any, 39997)

            tcp.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true)

            let tcprx = listen tcp cts.Token logger |> Async.StartAsTask

            Ok
                { udp = None
                  tcp = Some(tcp, tcprx, cts) }

        | _ ->
            logger.WriteLine("unknown emulator mode ({0})", mode)
            Ok { udp = None; tcp = None }

    let rec initialise (mode: string) (logger: TextWriter) (count: int) : Result<Emulator, string> =
        if count > 0 then
            try
                match (init mode logger) with
                | Ok emulator ->
                    Thread.Sleep 1000 // delay to let async listener start properly (TCP)
                    Ok emulator
                | _ -> Error "could not initialise stub"
            with err ->
                logger.WriteLine("  ** WARN {0} ... retrying...", err.Message)
                Thread.Sleep(30000)
                (initialise mode logger (count - 1))
        else
            Error "could not initialise emulator"

    let terminate (emulator: Emulator) (logger: TextWriter) =
        match emulator.udp with
        | Some(udp, udprx, cts) ->
            cts.Cancel()

            if not (udprx.Wait(5000)) then
                logger.WriteLine("  ** WARN udp::recv cancel timeout")
                udp.Close()
        | None -> ()

        match emulator.tcp with
        | Some(tcp, tcprx, cts) ->
            cts.Cancel()

            if not (tcprx.Wait(5000)) then
                logger.WriteLine("  ** WARN tcp::read cancel timeout")
                tcp.Server.Close()

        | None -> ()

    let event (emulator: Emulator) (event: byte array) (logger: TextWriter) =
        let listener = IPEndPoint.Parse("127.0.0.1:60001")
        let socket = new UdpClient()

        try
            socket.Send(event, event.Length, listener) |> ignore
        finally
            socket.Close()
