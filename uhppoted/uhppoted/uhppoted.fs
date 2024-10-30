namespace uhppoted

open System.Net

type Controller =
    { controller: uint32
      address: Option<IPEndPoint>
      protocol: Option<string> }

type Config =
    { bind: IPEndPoint
      broadcast: IPEndPoint
      listen: IPEndPoint
      debug: bool }

module Uhppoted =
    let configure (debug: bool) : Config =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          debug = debug }

    let get_all_controllers (timeout: int, debug: bool) : GetControllerResponse list =
        let cfg = configure (debug)

        let request = Encode.get_controller_request 0u
        let replies = UDP.broadcast (request, cfg.broadcast, timeout, debug)

        replies |> List.map (fun v -> Decode.get_controller_response v)

    let get_controller (controller: Controller, timeout: int, debug: bool) =
        let cfg = configure (debug)
        let request = Encode.get_controller_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, cfg.broadcast, timeout, cfg.debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, cfg.debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, cfg.debug)

        match result with
        | Ok packet -> Ok(Decode.get_controller_response packet)
        | Error err -> Error err

    let set_IPv4
        (controller: Controller, address: IPAddress, netmask: IPAddress, gateway: IPAddress, timeout: int, debug: bool)
        =
        let cfg = configure (debug)
        let request = Encode.set_IPv4_request controller.controller address netmask gateway

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, cfg.broadcast, timeout, cfg.debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, cfg.debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, cfg.debug)

        match result with
        | Ok _ -> Ok()
        | Error err -> Error err

    let get_listener (controller: Controller, timeout: int, debug: bool) =
        let cfg = configure (debug)
        let request = Encode.get_listener_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, cfg.broadcast, timeout, cfg.debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, cfg.debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, cfg.debug)

        match result with
        | Ok packet -> Ok(Decode.get_listener_response packet)
        | Error err -> Error err
