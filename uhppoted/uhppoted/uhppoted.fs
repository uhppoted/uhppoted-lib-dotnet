namespace uhppoted

open System
open System.Net
open System.Threading

module Uhppoted =
    let private defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          endpoint = None
          protocol = None
          debug = false }

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
        let request = Encode.getControllerRequest 0u
        let result = UDP.broadcast (request, bind, broadcast, timeout, debug)

        let f =
            fun v ->
                match Decode.getControllerResponse v with
                | Ok response ->
                    let controller: Controller =
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
        let request = Encode.getControllerRequest controller

        match exec controller request Decode.getControllerResponse timeout options with
        | Ok response ->
            let record: Controller =
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
        let request = Encode.setIPv4Request controller address netmask gateway

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
        let request = Encode.getListenerRequest controller

        match exec controller request Decode.getListenerResponse timeout options with
        | Ok response ->
            Ok
                { endpoint = response.endpoint
                  interval = response.interval }

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
        let request = Encode.setListenerRequest controller address port interval

        match exec controller request Decode.setListenerResponse timeout options with
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

    /// <summary>
    /// Sets the controller date and time.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="datetime">Date and time to set.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with the DateTime value from the controller or Error.</returns>
    let SetTime (controller: uint32, datetime: DateTime, timeout: int, options: Options) =
        let request = Encode.set_time_request controller datetime

        match exec controller request Decode.set_time_response timeout options with
        | Ok response -> Ok response.datetime
        | Error err -> Error err

    /// <summary>
    /// Retrieves the control mode and unlocked delay for a door.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    let GetDoor (controller: uint32, door: uint8, timeout: int, options: Options) =
        let request = Encode.get_door_request controller door

        match exec controller request Decode.get_door_response timeout options with
        | Ok response when response.door <> door -> // incorrect door
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { mode = Enums.doorMode response.mode
                      delay = response.delay }
            )
        | Error err -> Error err

    /// <summary>
    /// Sets the control mode and unlocked delay for a door.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    let SetDoor (controller: uint32, door: uint8, mode: DoorMode, delay: uint8, timeout: int, options: Options) =
        let request = Encode.set_door_request controller door mode delay

        match exec controller request Decode.set_door_response timeout options with
        | Ok response when response.door <> door -> // incorrect door
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { mode = Enums.doorMode response.mode
                      delay = response.delay }
            )
        | Error err -> Error err

    /// <summary>
    /// Sets up to 4 passcodes for a controller door.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="passcodes">Array of up to 4 passcodes in the range [0..999999], defaulting to 0 ('none')
    ///                         if the list contains less than 4 entries.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Returns Ok with true value if the passcodes were updated or Error.
    /// </returns>
    let SetDoorPasscodes (controller: uint32, door: uint8, passcodes: uint32 array, timeout: int, options: Options) =
        let request =
            Encode.set_door_passcodes_request
                controller
                door
                (passcodes |> Array.tryItem 0 |> Option.defaultValue 0u)
                (passcodes |> Array.tryItem 1 |> Option.defaultValue 0u)
                (passcodes |> Array.tryItem 2 |> Option.defaultValue 0u)
                (passcodes |> Array.tryItem 3 |> Option.defaultValue 0u)

        match exec controller request Decode.set_door_passcodes_response timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Unlocks a door controlled by a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Returns Ok if the request was processed, error otherwise. The Ok response should be
    /// checked for 'true'
    /// </returns>
    /// <remarks>
    /// </remarks>
    let OpenDoor (controller: uint32, door: uint8, timeout: int, options: Options) =
        let request = Encode.openDoorRequest controller door

        match exec controller request Decode.openDoorResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves the current status and most recent event from a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Returns Ok with the controller status record, including the most recent event (if any), or Error.
    /// </returns>
    let GetStatus (controller: uint32, timeout: int, options: Options) =
        let request = Encode.getStatusRequest controller

        match exec controller request Decode.getStatusResponse timeout options with
        | Ok response ->
            let status: Status =
                { Door1Open = response.door1Open
                  Door2Open = response.door2Open
                  Door3Open = response.door3Open
                  Door4Open = response.door4Open
                  Button1Pressed = response.door1Button
                  Button2Pressed = response.door2Button
                  Button3Pressed = response.door3Button
                  Button4Pressed = response.door4Button
                  SystemError = response.systemError
                  SystemDateTime = response.systemDateTime
                  SequenceNumber = response.sequenceNumber
                  SpecialInfo = response.specialInfo
                  Relay1 = Enums.relay response.relays 0x01uy
                  Relay2 = Enums.relay response.relays 0x02uy
                  Relay3 = Enums.relay response.relays 0x04uy
                  Relay4 = Enums.relay response.relays 0x08uy
                  Input1 = Enums.input response.inputs 0x01uy
                  Input2 = Enums.input response.inputs 0x02uy
                  Input3 = Enums.input response.inputs 0x04uy
                  Input4 = Enums.input response.inputs 0x08uy }

            let event: Event =
                { Timestamp = response.evt.timestamp
                  Index = response.evt.index
                  Event = response.evt.event
                  AccessGranted = response.evt.granted
                  Door = response.evt.door
                  Direction = Enums.direction response.evt.direction
                  Card = response.evt.card
                  Reason = response.evt.reason }

            match event.Index with
            | 0u -> Ok(status, Nullable())
            | _ -> Ok(status, Nullable(event))

        | Error err -> Error err

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
        let request = Encode.getCardsRequest controller

        match exec controller request Decode.getCardsResponse timeout options with
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
        let request = Encode.getCardRequest controller card

        match exec controller request Decode.getCardResponse timeout options with
        | Ok response when response.card = 0u -> // not found
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { Card = response.card
                      StartDate = response.start_date
                      EndDate = response.end_date
                      Door1 = response.door1
                      Door2 = response.door2
                      Door3 = response.door3
                      Door4 = response.door4
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
        let request = Encode.getCardAtIndexRequest controller index

        match exec controller request Decode.getCardAtIndexResponse timeout options with
        | Ok response when response.card = 0u -> // not found
            Ok(Nullable())
        | Ok response when response.card = 0xffffffffu -> // deleted
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { Card = response.card
                      StartDate = response.start_date
                      EndDate = response.end_date
                      Door1 = response.door1
                      Door2 = response.door2
                      Door3 = response.door3
                      Door4 = response.door4
                      PIN = response.PIN }
            )
        | Error err -> Error err

    /// <summary>
    /// Adds or updates a card record on a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="card">Card record to add or update.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let PutCard (controller: uint32, card: Card, timeout: int, options: Options) =
        let request = Encode.putCardRequest controller card

        match exec controller request Decode.putCardResponse timeout options with
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
        let request = Encode.deleteCardRequest controller card

        match exec controller request Decode.deleteCardResponse timeout options with
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
        let request = Encode.deleteAllCardsRequest controller

        match exec controller request Decode.deleteAllCardsResponse timeout options with
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
        let request = Encode.getEventRequest controller index

        match exec controller request Decode.getEventResponse timeout options with
        | Ok response when response.event = 0x00uy -> // not found
            Ok(Nullable())
        | Ok response when response.event = 0xffuy -> // overwritten
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { Timestamp = response.timestamp
                      Index = response.index
                      Event = response.event
                      AccessGranted = response.granted
                      Door = response.door
                      Direction = Enums.direction response.direction
                      Card = response.card
                      Reason = response.reason }
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
        let request = Encode.getEventIndexRequest controller

        match exec controller request Decode.getEventIndexResponse timeout options with
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
        let request = Encode.setEventIndexRequest controller index

        match exec controller request Decode.setEventIndexResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Enables or disables events for door open and close, button presses, etc.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="enable">true to enabled 'special events'.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with true if the 'special events' mode was set or Error.
    /// </returns>
    let RecordSpecialEvents (controller: uint32, enable: bool, timeout: int, options: Options) =
        let request = Encode.recordSpecialEventsRequest controller enable

        match exec controller request Decode.recordSpecialEventsResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves a time profile from a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="profile">Time profile ID [2..254].</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with time profile, Ok(null) if the requested profile does not exist or Error if the request failed.
    /// </returns>
    let GetTimeProfile (controller: uint32, profile: uint8, timeout: int, options: Options) =
        let request = Encode.getTimeProfileRequest controller profile

        match exec controller request Decode.getTimeProfileResponse timeout options with
        | Ok response when response.profile = 0x00uy -> // not found
            Ok(Nullable())
        | Ok response when response.profile <> profile -> // incorrect profile
            Ok(Nullable())
        | Ok response ->
            Ok(
                Nullable
                    { profile = response.profile
                      start_date = response.start_date
                      end_date = response.end_date
                      monday = response.monday
                      tuesday = response.tuesday
                      wednesday = response.wednesday
                      thursday = response.thursday
                      friday = response.friday
                      saturday = response.saturday
                      sunday = response.sunday
                      segment1_start = response.segment1_start
                      segment1_end = response.segment1_end
                      segment2_start = response.segment2_start
                      segment2_end = response.segment2_end
                      segment3_start = response.segment3_start
                      segment3_end = response.segment3_end
                      linked_profile = response.linked_profile }
            )
        | Error err -> Error err

    /// <summary>
    /// Adds or updates an access time profile on a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="profile">Access time profile to add or update.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with true if the time profile was added/updated, or Error.
    /// </returns>
    let SetTimeProfile (controller: uint32, profile: TimeProfile, timeout: int, options: Options) =
        let request = Encode.setTimeProfileRequest controller profile

        match exec controller request Decode.setTimeProfileResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Clears all access time profiles stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ClearTimeProfiles (controller: uint32, timeout: int, options: Options) =
        let request = Encode.clearTimeProfilesRequest controller

        match exec controller request Decode.clearTimeProfilesResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Adds or updates a scheduled task on a controller. Added tasks are not scheduled to run
    /// until the tasklist has been refreshed.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="task">Task definition to add or update.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Ok with true if the task was added/updated, or Error.
    /// </returns>
    let AddTask (controller: uint32, task: Task, timeout: int, options: Options) =
        let request = Encode.addTaskRequest controller task

        match exec controller request Decode.addTaskResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Clears all scheduled tasks from the controller task list.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ClearTaskList (controller: uint32, timeout: int, options: Options) =
        let request = Encode.clearTaskListRequest controller

        match exec controller request Decode.clearTaskListResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Schedules added tasks.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let RefreshTaskList (controller: uint32, timeout: int, options: Options) =
        let request = Encode.refreshTaskListRequest controller

        match exec controller request Decode.refreshTaskListResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Enables/disables remote access control management. The access controller will revert to standalone access
    /// control managment if it does not receive a command from the 'PC' at least every 30 seconds.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="enable">Enables or disables remote access control management.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let SetPCControl (controller: uint32, enable: bool, timeout: int, options: Options) =
        let request = Encode.setPCControlRequest controller enable

        match exec controller request Decode.setPCControlResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Sets the access controller door interlocks.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="interlock">Door interlocks (none, 1&2, 3&4, 1&2 and 3&4, 1&2&3 or 1&2&3&4.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let SetInterlock (controller: uint32, interlock: Interlock, timeout: int, options: Options) =
        let request = Encode.setInterlockRequest controller interlock

        match exec controller request Decode.setInterlockResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Activates/deactivates the access reader keypads attached to an access controller.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="reader1">Activates/deactivates the keypad for reader 1.</param>
    /// <param name="reader2">Activates/deactivates the keypad for reader 2.</param>
    /// <param name="reader3">Activates/deactivates the keypad for reader 3.</param>
    /// <param name="reader4">Activates/deactivates the keypad for reader 4.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ActivateKeypads
        (controller: uint32, reader1: bool, reader2: bool, reader3: bool, reader4: bool, timeout: int, options: Options)
        =
        let request =
            Encode.activateKeypadsRequest controller reader1 reader2 reader3 reader4

        match exec controller request Decode.activateKeypadsResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Restores the manufacturer defaults.
    /// </summary>
    /// <param name="controller">Controller ID.</param>
    /// <param name="timeout">Operation timeout (ms).</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let RestoreDefaultParameters (controller: uint32, timeout: int, options: Options) =
        let request = Encode.restoreDefaultParametersRequest controller

        match exec controller request Decode.restoreDefaultParametersResponse timeout options with
        | Ok response -> Ok response.ok
        | Error err -> Error err

    /// <summary>
    /// Listens for events from access controllers and dispatches received events to a handler.
    /// </summary>
    /// <param name="callback">External event handler function.</param>
    /// <param name="stop">Cancellation token to terminate event listener.</param>
    /// <param name="options">Bind, broadcast and listen addresses and (optionally) destination address and transport protocol.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let Listen (callback: Result<Event, string> -> unit, stop: CancellationToken, options: Options) =
        let bind = options.listen
        let debug = options.debug

        let handler (packet: byte array) =
            match Decode.listenEvent packet with
            | Ok e ->
                if e.event.index <> 0u then
                    let event: Event =
                        { Timestamp = e.event.timestamp
                          Index = e.event.index
                          Event = e.event.event
                          AccessGranted = e.event.granted
                          Door = e.event.door
                          Direction = Enums.direction e.event.direction
                          Card = e.event.card
                          Reason = e.event.reason }

                    callback (Ok event)

            | Error err -> printfn "OOOOPS %A" err

        UDP.listen bind handler stop debug
