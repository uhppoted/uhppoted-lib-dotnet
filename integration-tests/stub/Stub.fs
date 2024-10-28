namespace stub

open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading.Tasks

type Emulator = { udp: UdpClient; task: Task }

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

    let initialise (logger: TextWriter) : Emulator =
        let socket = new UdpClient(59999)
        let rx = udp_receive socket logger |> Async.StartAsTask

        { udp = socket; task = rx }

    let terminate (emulator: Emulator) = printfn "... closing UDP emulator"
