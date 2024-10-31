namespace stub

open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading.Tasks

type Emulator =
    { udp: (UdpClient * Task)
      tcp: (TcpListener * Task) }

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
        match Requests.find packet with
        | Some(key) -> Responses.find key
        | _ -> None

    let rec recv (socket: UdpClient) (logger: TextWriter) =
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

                match find packet with
                | Some(reply) when reply.Length = 64 -> socket.Send(reply, reply.Length, remote) |> ignore
                | Some(_) -> ()
                | _ ->
                    logger.WriteLine("*** ERROR unknown packet")
                    dump packet logger

                return! recv socket logger
            with
            | :? ObjectDisposedException -> logger.WriteLine("** UDP SOCKET CLOSED")
            | err -> logger.WriteLine("** ERROR {0}", err.Message)
        }

    let rec read (stream: NetworkStream) (logger: TextWriter) =
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

                if packet.Length > 0 then
                    match find packet with
                    | Some(reply) when reply.Length = 64 -> stream.Write(reply) |> ignore
                    | Some(_) -> ()
                    | _ ->
                        logger.WriteLine("*** ERROR unknown packet")
                        dump packet logger

                    return! read stream logger

                return ()
            with err ->
                logger.WriteLine("*** ERROR {0}", Error err.Message)
                return ()
        }

    let listen (socket: TcpListener) (logger: TextWriter) =
        async {
            try
                socket.Start()

                while true do
                    let! connection = socket.AcceptTcpClientAsync() |> Async.AwaitTask
                    let stream = connection.GetStream()

                    read stream logger |> Async.Start
            with
            | :? ObjectDisposedException -> logger.WriteLine("** TCP SOCKET CLOSED")
            | err -> logger.WriteLine("** ERROR {0}", err.Message)
        }

    let initialise (logger: TextWriter) : Emulator =
        let udp = new UdpClient(59999)
        let udprx = recv udp logger |> Async.StartAsTask

        let tcp = new TcpListener(IPAddress.Any, 59999)
        let tcprx = listen tcp logger |> Async.StartAsTask

        { udp = (udp, udprx)
          tcp = (tcp, tcprx) }

    let terminate (emulator: Emulator) = printfn "... closing UDP emulator"
