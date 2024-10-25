namespace uhppoted

open System.Net

type Controller =
    { controller: uint32
      address: Option<IPEndPoint>
      protocol: Option<string> }


module Uhppoted =
    let get_all_controllers (timeout: int, debug: bool) : GetControllerResponse list =
        let addr = "192.168.1.255"
        let port = 60000

        let request = Encode.get_controller_request 0u
        let replies = UDP.broadcast (request, addr, port, timeout, debug)

        replies |> List.map (fun v -> Decode.get_controller_response v)

    let get_controller (controller: Controller, timeout: int, debug: bool) =
        let broadcast = IPEndPoint(IPAddress.Parse("192.168.1.255"), 60000)
        let request = Encode.get_controller_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.get_controller_response packet)
        | Error err -> Error err
