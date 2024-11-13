namespace uhppoted

open System
open System.Net

module Uhppoted =
    let private defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          debug = false }

    let private exec
        (controller: Controller)
        request
        (decode: byte[] -> Result<'b, string>)
        timeout
        options
        : Result<'b, string> when 'b :> IResponse =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, bind, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, bind, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, bind, addr, timeout, debug)

        match result with
        | Ok packet ->
            match decode packet with
            | Ok response when response.controller = controller.controller -> Ok response
            | Ok _ -> Error "invalid response"
            | Error err -> Error err
        | Error err -> Error err

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

        exec controller request Decode.get_controller_response timeout options

    let set_IPv4
        (
            controller: Controller,
            address: IPAddress,
            netmask: IPAddress,
            gateway: IPAddress,
            timeout: int,
            options: Options
        ) =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.set_IPv4_request controller.controller address netmask gateway

        let result =
            match controller.address, controller.protocol with
            | None, _ -> UDP.broadcast_to (request, bind, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, bind, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, bind, addr, timeout, debug)

        match result with
        | Ok _ -> Ok()
        | Error err -> Error err

    let get_listener (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_listener_request controller.controller

        exec controller request Decode.get_listener_response timeout options

    let set_listener (controller: Controller, endpoint: IPEndPoint, interval: uint8, timeout: int, options: Options) =
        let address = endpoint.Address
        let port = uint16 endpoint.Port

        let request =
            Encode.set_listener_request controller.controller address port interval

        exec controller request Decode.set_listener_response timeout options

    let get_time (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_time_request controller.controller

        exec controller request Decode.get_time_response timeout options

    let set_time (controller: Controller, datetime: DateTime, timeout: int, options: Options) =
        let request = Encode.set_time_request controller.controller datetime

        exec controller request Decode.set_time_response timeout options

    let get_door (controller: Controller, door: uint8, timeout: int, options: Options) =
        let request = Encode.get_door_request controller.controller door

        exec controller request Decode.get_door_response timeout options

    let set_door (controller: Controller, door: uint8, mode: uint8, delay: uint8, timeout: int, options: Options) =
        let request = Encode.set_door_request controller.controller door mode delay

        exec controller request Decode.set_door_response timeout options

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

        exec controller request Decode.set_door_passcodes_response timeout options

    /// <summary>
    /// Unlocks a door controlled by a controller.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok if the request was processed, error otherwise. The Ok response should be
    /// checked for 'true'
    /// </returns>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// <code language="fsharp">
    /// let controller = { controller = 405419896u; address = None; protocol = None }
    /// let options = { broadcast = IPAddress.Broadcast; debug = true }
    /// let result = open_door controller 4uy 5000 options
    /// match result with
    /// | Ok response -> printfn "Door opened successfully: %A" response
    /// | Error e -> printfn "Error: %s" e
    /// </code>
    /// <code language="csharp">
    /// var controller = new ControllerBuilder(405419896).build();
    /// var options = new OptionsBuilder().build();
    /// var result = open_door(controller, 1, 5000, options);
    /// if (result.IsOk)
    /// {
    ///     Console.WriteLine("open-door: {0}",result.Value.ok);
    /// }
    /// else
    /// {
    ///     Console.WriteLine("open-door: error {0}",result.Error);
    /// }
    /// </code>
    /// <code language="vbnet">
    /// Dim controller As New ControllerBuilder(405419896u).build()
    /// Dim options As New OptionsBuilder().build()
    /// Dim result = open_door(controller, 1, 3000, options)
    /// If result.IsOk Then
    ///     Console.WriteLine("open-door: {0}",result.Value.ok)
    /// Else
    ///     Console.WriteLine("open-door: error {0}",result.Error);
    /// End If
    /// </code>
    /// </example>
    let open_door (controller: Controller, door: uint8, timeout: int, options: Options) =
        let request = Encode.open_door_request controller.controller door

        exec controller request Decode.open_door_response timeout options

    /// <summary>
    /// Retrieves the current status of a controller.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// The controller status record, including the most recent event (if any).
    /// </returns>
    /// <example>
    /// <code language="fsharp">
    /// let controller = { controller = 405419896u; address = None; protocol = None }
    /// let options = { broadcast = IPAddress.Broadcast; debug = true }
    /// let result = get_status controller 5000 options
    /// match result with
    /// | Ok response -> printfn "get-status: ok %A" response
    /// | Error e -> printfn "get-cards: error %s" e
    /// </code>
    /// <code language="csharp">
    /// var controller = new ControllerBuilder(405419896).build();
    /// var options = new OptionsBuilder().build();
    /// var result = get_status(controller, 1, 5000, options);
    /// if (result.IsOk)
    /// {
    ///     Console.WriteLine("get-status: {0}",result.Value.ok);
    /// }
    /// else
    /// {
    ///     Console.WriteLine("get-status: error {0}",result.Error);
    /// }
    /// </code>
    /// <code language="vbnet">
    /// Dim controller As New ControllerBuilder(405419896u).build()
    /// Dim options As New OptionsBuilder().build()
    /// Dim result = get_status(controller, 1, 5000, options)
    /// If result.IsOk Then
    ///     Console.WriteLine("get-status: {0}",result.Value.ok)
    /// Else
    ///     Console.WriteLine("get-status: error {0}",result.Error);
    /// End If
    /// </code>
    /// </example>
    let get_status (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_status_request controller.controller

        exec controller request Decode.get_status_response timeout options

    /// <summary>
    /// Retrieves the number of card records stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Number of cards stored on the controller (or an error).
    /// </returns>
    /// <example>
    /// <code language="fsharp">
    /// let controller = { controller = 405419896u; address = None; protocol = None }
    /// let options = { broadcast = IPAddress.Broadcast; debug = true }
    /// let result = get_cards controller 5000 options
    /// match result with
    /// | Ok response -> printfn "get-cards: ok %A" response
    /// | Error e -> printfn "get-cards: error %A" e
    /// </code>
    /// <code language="csharp">
    /// var controller = new ControllerBuilder(405419896).build();
    /// var options = new OptionsBuilder().build();
    /// var result = get_cards(controller, 5000, options);
    /// if (result.IsOk)
    /// {
    ///     Console.WriteLine("get-cards: {0}",result.Value.ok);
    /// }
    /// else
    /// {
    ///     Console.WriteLine("get-cards: error {0}",result.Error);
    /// }
    /// </code>
    /// <code language="vbnet">
    /// Dim controller As New ControllerBuilder(405419896u).build()
    /// Dim options As New OptionsBuilder().build()
    /// Dim result = get_cards(controller, 5000, options)
    /// If result.IsOk Then
    ///     Console.WriteLine("get-cards: {0}",result.Value.ok)
    /// Else
    ///     Console.WriteLine("get-cards: error {0}",result.Error);
    /// End If
    /// </code>
    /// </example>
    let get_cards (controller: Controller, timeout: int, options: Options) =
        let request = Encode.get_cards_request controller.controller

        exec controller request Decode.get_cards_response timeout options

    /// <summary>
    /// Retrieves the card record for the requested card number.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record matching card number (or an error).
    /// </returns>
    /// <example>
    /// <code language="fsharp">
    /// let controller = { controller = 405419896u; address = None; protocol = None }
    /// let card = 10058400u
    /// let timeout = 5000
    /// let options = { broadcast = IPAddress.Broadcast; debug = true }
    /// let result = GetCard controller card timeout options
    /// match result with
    /// | Ok response -> printfn "get-card: ok %A" response
    /// | Error e -> printfn "get-card: error %A" e
    /// </code>
    /// <code language="csharp">
    /// var controller = new ControllerBuilder(405419896).build();
    /// var card = 10058400
    /// var timeout = 5000
    /// var options = new OptionsBuilder().build();
    /// var result = GetCard(controller, card, timeout, options);
    /// if (result.IsOk)
    /// {
    ///     Console.WriteLine("get-card: {0}",result.Value);
    /// }
    /// else
    /// {
    ///     Console.WriteLine("get-card: error {0}",result.Error);
    /// }
    /// </code>
    /// <code language="vbnet">
    /// Dim controller As New ControllerBuilder(405419896).build()
    /// Dim card = 10058400
    /// Dim timeout = 5000
    /// Dim options As New OptionsBuilder().build()
    /// Dim result = GetCard(controller, card, timeout, options)
    /// If result.IsOk Then
    ///     Console.WriteLine("get-card: {0}",result.Value)
    /// Else
    ///     Console.WriteLine("get-card: error {0}",result.Error);
    /// End If
    /// </code>
    /// </example>
    let GetCard (controller: Controller, card: uint32, timeout: int, options: Options) =
        let request = Encode.get_card_request controller.controller card

        match (exec controller request Decode.get_card_response timeout options) with
        | Ok response when response.card = 0u -> Error "card not found"
        | Ok response -> Ok response
        | Error err -> Error err
