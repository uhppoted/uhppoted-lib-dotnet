namespace uhppoted

open System
open System.Net

module Uhppoted =
    let private defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          debug = false }

    let private exec (controller: Controller) request (timeout: int) (options: Options) =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug

        match controller.address, controller.protocol with
        | None, _ -> UDP.broadcast_to (request, bind, broadcast, timeout, debug)
        | Some(addr), Some("tcp") -> TCP.send_to (request, bind, addr, timeout, debug)
        | Some(addr), _ -> UDP.send_to (request, bind, addr, timeout, debug)

    /// <summary>
    /// Retrieves a list of controllers on the local LAN accessible via a UDP broadcast.
    /// </summary>
    /// <param name="timeout">The timeout duration in milliseconds to wait for all replies.</param>
    /// <param name="options">Bind, broadcast and listen address options.</param>
    /// <returns>A result with an array of GetControllerResponse records or error.</returns>
    /// <example>
    /// F#:
    /// let timeout = 5000
    /// let options = { broadcast = IPAddress.Parse("255.255.255.255"); debug = true }
    /// match get_all_controllers(timeout, options) with
    /// | Ok controllers -> controllers |> Array.iter (fun controller ->
    ///                                                    printfn "controller ID: %u, version: %s" controller.controller controller.version
    /// | Error err -> printfn "%A" err
    /// )
    /// </example>
    /// <example>
    /// C#:
    /// var timeout = 5000;
    /// var options = new Options { broadcast = IPAddress.Parse("255.255.255.255"), debug = true };
    /// var result = get_all_controllers(timeout, options);
    /// if (result.IsOk)
    ///     {
    ///       var controllers = result.ResultValue;
    ///
    ///       foreach (var controller in controllers)
    ///       {
    ///         Console.WriteLine($"Controller ID: {controller.controller}, Version: {controller.version}");
    ///       }
    ///     }
    ///     else if (result.IsError)
    ///     {
    ///         throw new Exception(result.ErrorValue);
    ///     }
    /// </example>
    /// <example>
    /// VB.NET:
    /// Dim timeout As Integer = 5000
    /// Dim options As New Options With { .broadcast = IPAddress.Parse("255.255.255.255"), .debug = True }
    /// Dim result = get_all_controllers(TIMEOUT, OPTIONS)
    ///
    /// If (result.IsOk)
    ///    Dim controllers = result.ResultValue
    ///    For Each controller In controllers
    ///        Console.WriteLine($"Controller ID: {controller.controller}, Version: {controller.version}")
    ///    Next
    /// Else If (result.IsError)
    ///    Throw New Exception(result.ErrorValue)
    /// End If
    /// </example>
    /// <remarks>
    /// Invalid individual responses are silently discarded.
    /// </remarks>
    let get_all_controllers (timeout: int, options: Options) =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.get_controller_request 0u
        let result = UDP.broadcast (request, bind, broadcast, timeout, debug)

        let f =
            fun v ->
                match Decode.get_controller_response v with
                | Ok response -> Some(response)
                | _ -> None

        match result with
        | Ok replies -> replies |> List.choose (f) |> List.toArray |> Ok
        | Error err -> Error err

    let get_controller (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_controller_request controller.controller
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.get_controller_response packet
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
        let request = Encode.set_IPv4_request controller.controller address netmask gateway
        let result = exec controller request timeout options

        match result with
        | Ok _ -> Ok()
        | Error err -> Error err

    let get_listener (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_listener_request controller.controller
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.get_listener_response packet
        | Error err -> Error err

    let set_listener (controller: Controller, endpoint: IPEndPoint, interval: uint8, timeout: int, options: Options) =
        let address = endpoint.Address
        let port = uint16 endpoint.Port

        let request =
            Encode.set_listener_request controller.controller address port interval

        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.set_listener_response packet
        | Error err -> Error err

    let get_time (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_time_request controller.controller
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.get_time_response packet
        | Error err -> Error err

    let set_time (controller: Controller, datetime: DateTime, timeout: int, options: Options) =
        let request = Encode.set_time_request controller.controller datetime
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.set_time_response packet
        | Error err -> Error err

    let get_door (controller: Controller, door: uint8, timeout: int, options: Options) =
        let request = Encode.get_door_request controller.controller door
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.get_door_response packet
        | Error err -> Error err

    let set_door (controller: Controller, door: uint8, mode: uint8, delay: uint8, timeout: int, options: Options) =
        let request = Encode.set_door_request controller.controller door mode delay
        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.set_door_response packet
        | Error err -> Error err

    /// <summary>
    /// Sets up to 4 passcodes for a controller door.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="passcode1">Passcode [0..999999] (0 is 'none').</param>
    /// <param name="passcode2">Passcode [0..999999] (0 is 'none').</param>
    /// <param name="passcode3">Passcode [0..999999] (0 is 'none').</param>
    /// <param name="passcode4">Passcode [0..999999] (0 is 'none').</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok if the request was processed, error otherwise. The Ok response should be
    /// checked for 'true'
    /// </returns>
    /// <example>
    /// <code language="fsharp">
    /// let controller = { controller = 405419896u; address = None; protocol = None }
    /// let options = { broadcast = IPAddress.Broadcast; debug = true }
    /// match set_door_passcodes controller 4uy 12345u 54321u 0u 999999u 5000 options with
    /// | Ok response -> printfn "set door passcodes %A" response.ok
    /// | Error err -> printfn "error setting door passcodes: %A" err
    /// </code>
    /// <code language="csharp">
    /// var controller = new ControllerBuilder(405419896).build();
    /// var options = new OptionsBuilder().build();
    /// var result = set_door_passcodes(controller, 4, 12345, 54321, 0, 999999, 5000, options);
    /// if (result.IsOk)
    /// {
    ///     Console.WriteLine($"set-door-passcodes: {result.ResultValue.ok}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
    /// }
    /// </code>
    /// <code language="vbnet">
    /// Dim controller As New ControllerBuilder(405419896u).build()
    /// Dim options As New OptionsBuilder().build()
    /// Dim result = set_door_passcodes(controller, 4, 12345UI, 54321UI, 0UI, 999999UI, 5000, options)
    /// If result.IsOk Then
    ///     Console.WriteLine($"set-door-passcodes: {result.ResultValue.ok}")
    /// Else
    ///     Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
    /// End If
    /// </code>
    let set_door_passcodes
        (
            controller: Controller,
            door: uint8,
            passcode1: uint32,
            passcode2: uint32,
            passcode3: uint32,
            passcode4: uint32,
            timeout: int,
            options: Options
        ) =
        let request =
            Encode.set_door_passcodes_request controller.controller door passcode1 passcode2 passcode3 passcode4

        let result = exec controller request timeout options

        match result with
        | Ok packet -> Decode.set_door_passcodes_response packet
        | Error err -> Error err
