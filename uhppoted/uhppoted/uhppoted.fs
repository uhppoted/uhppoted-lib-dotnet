namespace uhppoted

open System
open System.Net

module Uhppoted =
    let defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          debug = false }

    let get_all_controllers (timeout: int, options: Options) : GetControllerResponse list =
        let broadcast = options.broadcast
        let debug = options.debug

        let request = Encode.get_controller_request 0u
        let replies = UDP.broadcast (request, broadcast, timeout, debug)

        replies |> List.map (fun v -> Decode.get_controller_response v)

    let get_controller (controller: Controller, timeout: int, options: Options) =
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.get_controller_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.get_controller_response packet)
        | Error err -> Error err

    let set_IPv4
        (
            controller: Controller,
            address: IPAddress,
            netmask: IPAddress,
            gateway: IPAddress,
            timeout: int,
            options: Options
        ) =
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.set_IPv4_request controller.controller address netmask gateway

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok _ -> Ok()
        | Error err -> Error err

    let get_listener (controller: Controller, timeout: int, options: Options) =
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.get_listener_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.get_listener_response packet)
        | Error err -> Error err

    let set_listener (controller: Controller, endpoint: IPEndPoint, interval: uint8, timeout: int, options: Options) =
        let broadcast = options.broadcast
        let debug = options.debug
        let address = endpoint.Address
        let port = uint16 endpoint.Port

        let request =
            Encode.set_listener_request controller.controller address port interval

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.set_listener_response packet)
        | Error err -> Error err

    let get_time (controller: Controller, timeout: int, options: Options) =
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.get_time_request controller.controller

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.get_time_response packet)
        | Error err -> Error err

    let set_time (controller: Controller, datetime: DateTime, timeout: int, options: Options) =
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.set_time_request controller.controller datetime

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, addr, timeout, debug)

        match result with
        | Ok packet -> Ok(Decode.set_time_response packet)
        | Error err -> Error err
