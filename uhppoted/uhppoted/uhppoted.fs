namespace uhppoted

open System.Net

module Uhppoted =
    let get_all_controllers () : GetControllerResponse list =
        let addr = "192.168.1.255"
        let port = 60000
        let timeout = 1000
        let debug = true

        let request = Encode.get_controller_request 0u
        let replies = UDP.broadcast (request, addr, port, timeout, debug)

        replies |> List.map (fun v -> Decode.get_controller_response v)

    let get_controller (controller: uint32) =
        let addr = "192.168.1.255"
        let port = 60000
        let addrPort = IPEndPoint(IPAddress.Parse("192.168.1.100"), port)
        let timeout = 1000
        let debug = true

        let request = Encode.get_controller_request controller
        let result = UDP.broadcast_to (request, addrPort, timeout, debug)
        // let result = UDP.send_to (request, addrPort, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.get_controller_response packet)
        | Error err -> Error err
