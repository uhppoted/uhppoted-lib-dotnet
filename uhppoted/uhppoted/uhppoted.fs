namespace uhppoted

open System
open System.Net

module Uhppoted =
    let private defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          endpoint = None
          protocol = None
          debug = false }

    let private exex
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

    let private exec
        (controller: uint32)
        request
        (decode: byte[] -> Result<'b, string>)
        timeout
        options
        : Result<'b, string> when 'b :> IResponse =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug

        let result =
            match options.endpoint, options.protocol with
            | None, _ -> UDP.broadcast_to (request, bind, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, bind, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, bind, addr, timeout, debug)

        match result with
        | Ok packet ->
            match decode packet with
            | Ok response when response.controller = controller -> Ok response
            | Ok _ -> Error "invalid response"
            | Error err -> Error err
        | Error err -> Error err

    /// <summary>
    /// Retrieves a list of controllers on the local LAN accessible via a UDP broadcast.
    /// </summary>
    /// <param name="timeout">The timeout duration in milliseconds to wait for all replies.</param>
    /// <param name="options">Bind, broadcast and listen address options.</param>
    /// <returns>A result with an array of GetControllerResponse records or error.</returns>
    /// <remarks>
    /// Invalid individual responses are silently discarded.
    /// </remarks>
    let FindControllers (timeout: int, options: Options) =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.get_controller_request 0u
        let result = UDP.broadcast (request, bind, broadcast, timeout, debug)

        let f =
            fun v ->
                match Decode.get_controller_response v with
                | Ok response ->
                    let controller: ControllerRecord =
                        { controller = response.controller
                          address = response.address
                          netmask = response.netmask
                          gateway = response.gateway
                          MAC = response.MAC
                          version = response.version
                          date = response.date }

                    Some(controller)

                | _ -> None

        match result with
        | Ok replies -> replies |> List.choose (f) |> List.toArray |> Ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves the IPv4 configuration, MAC address and version information for an access controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with a Controller record or Error.</returns>
    /// <remarks></remarks>
    let GetController (controller: uint32, timeout: int, options: Options) =
        let request = Encode.get_controller_request controller

        match exec controller request Decode.get_controller_response timeout options with
        | Ok response ->
            let record: ControllerRecord =
                { controller = response.controller
                  address = response.address
                  netmask = response.netmask
                  gateway = response.gateway
                  MAC = response.MAC
                  version = response.version
                  date = response.date }

            Ok record
        | Error err -> Error err

    /// <summary>
    /// Sets the controller IPv4 address, netmask and gateway address..
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="address">Controller IPv4 address.</param>
    /// <param name="netmask">Controller IPv4 netmask.</param>
    /// <param name="gateway">Gateway IPv4 address.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok or Error.</returns>
    /// <remarks>
    /// The controller does not return a response to this request - provided no network (or other) errors occur,
    /// it is assumed to be successful.
    /// </remarks>
    let SetIPv4
        (controller: uint32, address: IPAddress, netmask: IPAddress, gateway: IPAddress, timeout: int, options: Options)
        =
        let bind = options.bind
        let broadcast = options.broadcast
        let debug = options.debug
        let request = Encode.set_IPv4_request controller address netmask gateway

        let result =
            match options.endpoint, options.protocol with
            | None, _ -> UDP.broadcast_to (request, bind, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.send_to (request, bind, addr, timeout, debug)
            | Some(addr), _ -> UDP.send_to (request, bind, addr, timeout, debug)

        match result with
        | Ok _ -> Ok()
        | Error err -> Error err

    /// <summary>
    /// Retrieves the controller event listener endpoint and auto-send interval.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with an Listener record or Error.</returns>
    let GetListener (controller: uint32, timeout: int, options: Options) =
        let request = Encode.get_listener_request controller

        match exec controller request Decode.get_listener_response timeout options with
        | Ok response ->
            let record: Listener =
                { endpoint = response.endpoint
                  interval = response.interval }

            Ok record
        | Error err -> Error err

    /// <summary>
    /// Sets the controller event listener IPv4 endpoint and the auto-send interval. The auto-send interval is the interval
    /// at which the controller sends the current status (including the most recent event) to the configured event listener.
    /// (events are always sent as the occur).
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="endpoint">IPv4 endpoint of event listener.</param>
    /// <param name="interval">Auto-send interval (seconds). A zero interval disables auto-send.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with true if the event listener endpoint was updated or Error.</returns>
    let SetListener (controller: uint32, endpoint: IPEndPoint, interval: uint8, timeout: int, options: Options) =
        let address = endpoint.Address
        let port = uint16 endpoint.Port
        let request = Encode.set_listener_request controller address port interval

        match exec controller request Decode.set_listener_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves the controller current date and time.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with a DateTime value or Error.</returns>
    let GetTime (controller: uint32, timeout: int, options: Options) =
        let request = Encode.get_time_request controller

        match exec controller request Decode.get_time_response timeout options with
        | Ok response -> Ok response.datetime
        | Error err -> Error err

    let set_time (controller: Controller, datetime: DateTime, timeout: int, options: Options) =
        let request = Encode.set_time_request controller.controller datetime

        exex controller request Decode.set_time_response timeout options

    let get_door (controller: Controller, door: uint8, timeout: int, options: Options) =
        let request = Encode.get_door_request controller.controller door

        exex controller request Decode.get_door_response timeout options

    let set_door (controller: Controller, door: uint8, mode: uint8, delay: uint8, timeout: int, options: Options) =
        let request = Encode.set_door_request controller.controller door mode delay

        exex controller request Decode.set_door_response timeout options

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

        exex controller request Decode.set_door_passcodes_response timeout options

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

        exex controller request Decode.open_door_response timeout options

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

        exex controller request Decode.get_status_response timeout options

    /// <summary>
    /// Retrieves the number of card records stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID and (optionally) address and transport protocol.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Optional bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with the number of cards stored on the controller or Error.
    /// </returns>
    let GetCards (controller: uint32, timeout: int, options: Options) =
        let request = Encode.get_cards_request controller

        match exec controller request Decode.get_cards_response timeout options with
        | Ok response -> Ok response.cards
        | Error err -> Error err

    /// <summary>
    /// Retrieves the card record for the requested card number.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Card record matching the card number (or null if not found) or an error if the request failed.
    /// </returns>
    let GetCard (controller: uint32, card: uint32, timeout: int, options: Options) =
        let request = Encode.get_card_request controller card

        match exec controller request Decode.get_card_response timeout options with
        | Ok response when response.card = 0u -> // not found
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { card = response.card
                      startdate = response.startdate
                      enddate = response.enddate
                      door1 = response.door1
                      door2 = response.door2
                      door3 = response.door3
                      door4 = response.door4
                      PIN = response.PIN }
            )
        | Error err -> Error err

    /// <summary>
    /// Retrieves the card record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Card record at the index (or null if not found or deleted) or an error if the request failed.
    /// </returns>
    let GetCardAtIndex (controller: uint32, index: uint32, timeout: int, options: Options) =
        let request = Encode.get_card_at_index_request controller index

        match exec controller request Decode.get_card_at_index_response timeout options with
        | Ok response when response.card = 0u -> // not found
            Ok(Nullable())
        | Ok response when response.card = 0xffffffffu -> // deleted
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { card = response.card
                      startdate = response.startdate
                      enddate = response.enddate
                      door1 = response.door1
                      door2 = response.door2
                      door3 = response.door3
                      door4 = response.door4
                      PIN = response.PIN }
            )
        | Error err -> Error err

    /// <summary>
    /// Adds or updates a card record on a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="startdate">Date from which card is valid.</param>
    /// <param name="enddate">Date after which card is invalid.</param>
    /// <param name="door1">Access permissions for door 1 (0: none, 1:all, [2..254]: time profile).</param>
    /// <param name="door2">Access permissions for door 2 (0: none, 1:all, [2..254]: time profile).</param>
    /// <param name="door3">Access permissions for door 3 (0: none, 1:all, [2..254]: time profile).</param>
    /// <param name="door4">Access permissions for door 4 (0: none, 1:all, [2..254]: time profile).</param>
    /// <param name="pin">Optional keypad PIN code [0..999999] (0 is none).</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let PutCard
        (
            controller: uint32,
            card: uint32,
            startdate: DateOnly,
            enddate: DateOnly,
            door1: uint8,
            door2: uint8,
            door3: uint8,
            door4: uint8,
            pin: uint32,
            timeout: int,
            options: Options
        ) =
        let request =
            Encode.put_card_request controller card startdate enddate door1 door2 door3 door4 pin

        match exec controller request Decode.put_card_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Deletes a card record from a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card number to delete.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let DeleteCard (controller: uint32, card: uint32, timeout: int, options: Options) =
        let request = Encode.delete_card_request controller card

        match exec controller request Decode.delete_card_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Deletes all card records from a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let DeleteAllCards (controller: uint32, timeout: int, options: Options) =
        let request = Encode.delete_all_cards_request controller

        match exec controller request Decode.delete_all_cards_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves the event record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="index">Index of event to retrieve.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Event record at the index (or null if not found or deleted) or an error if the request failed. Returns an Ok(null) if the event
    // record does not exist or if the event has been overwritten.
    /// </returns>
    let GetEvent (controller: uint32, index: uint32, timeout: int, options: Options) =
        let request = Encode.get_event_request controller index

        match exec controller request Decode.get_event_response timeout options with
        | Ok response when response.event = 0x00uy -> // not found
            Ok(Nullable())
        | Ok response when response.event = 0xffuy -> // overwritten
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { timestamp = response.timestamp
                      index = response.index
                      event_type = response.event
                      access_granted = response.granted
                      door = response.door
                      direction = response.direction
                      card = response.card
                      reason = response.reason }
            )
        | Error err -> Error err

    /// <summary>
    /// Retrieves the current event index from the controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with current controller event indexEvent record at the index or Error.
    /// </returns>
    let GetEventIndex (controller: uint32, timeout: int, options: Options) =
        let request = Encode.get_event_index_request controller

        match exec controller request Decode.get_event_index_response timeout options with
        | Ok response -> Ok response.index
        | Error err -> Error err

    /// <summary>
    /// Sets the controller event index.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="index">Event index.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with true if the event index was updated (false if it was unchanged) or Error.
    /// </returns>
    let SetEventIndex (controller: uint32, index: uint32, timeout: int, options: Options) =
        let request = Encode.set_event_index_request controller index

        match exec controller request Decode.set_event_index_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err
