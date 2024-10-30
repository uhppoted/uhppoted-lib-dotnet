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
    let rec udp_receive (socket: UdpClient) (logger: TextWriter) =
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

                logger.WriteLine("** incoming {0} {1}", remote, packet)

                let response = Responses.get_controller
                socket.Send(response, response.Length, remote) |> ignore

                return! udp_receive socket logger
            with
            | :? ObjectDisposedException -> logger.WriteLine("** UDP SOCKET CLOSED")
            | err -> logger.WriteLine("** ERROR {0}", err.Message)
        }

    let rec tcp_receive (stream: NetworkStream) (logger: TextWriter) =
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
                    if packet.Length = 64 then
                        let response = Responses.get_controller
                        stream.Write(response) |> ignore

                    return! tcp_receive stream logger

                return ()
            with err ->
                logger.WriteLine("*** ERROR {0}", Error err.Message)
                return ()
        }

    let tcp_listen (socket: TcpListener) (logger: TextWriter) =
        async {
            try
                socket.Start()
                logger.WriteLine("** listening")

                while true do
                    let! connection = socket.AcceptTcpClientAsync() |> Async.AwaitTask
                    let stream = connection.GetStream()

                    tcp_receive stream logger |> Async.Start
            with
            | :? ObjectDisposedException -> logger.WriteLine("** TCP SOCKET CLOSED")
            | err -> logger.WriteLine("** ERROR {0}", err.Message)
        }

    let initialise (logger: TextWriter) : Emulator =
        let udp = new UdpClient(59999)
        let udprx = udp_receive udp logger |> Async.StartAsTask

        let tcp = new TcpListener(IPAddress.Any, 59999)
        let tcprx = tcp_listen tcp logger |> Async.StartAsTask

        { udp = (udp, udprx)
          tcp = (tcp, tcprx) }

    let terminate (emulator: Emulator) = printfn "... closing UDP emulator"
