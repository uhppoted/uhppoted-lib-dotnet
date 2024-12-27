namespace uhppoted

open System
open System.Net
open System.Threading
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module Uhppoted =
    let private defaults =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
          listen = IPEndPoint(IPAddress.Any, 60001)
          timeout = 1000
          debug = false }

    let internal resolve (controller: 'T) : Result<C, string> =
        match box controller with
        | :? uint32 as u32 ->
            Ok
                { controller = u32
                  endpoint = None
                  protocol = None }
        | :? C as c -> Ok c
        | _ -> Error $"unsupported controller type ({typeof<'T>.FullName}) - expected uint32 or struct"

    let private exec
        (controller: C)
        request
        (decode: byte[] -> Result<'b, string>)
        options
        : Result<'b, string> when 'b :> IResponse =
        let bind = options.bind
        let broadcast = options.broadcast
        let timeout = options.timeout
        let debug = options.debug

        let endpoint = controller.endpoint
        let protocol = controller.protocol

        let result =
            match endpoint, protocol with
            | None, _ -> UDP.broadcastTo (request, bind, broadcast, timeout, debug)
            | Some(addr), Some("tcp") -> TCP.sendTo (request, bind, addr, timeout, debug)
            | Some(addr), _ -> UDP.sendTo (request, bind, addr, timeout, debug)

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
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>A result with an array of GetControllerResponse records or error.</returns>
    /// <remarks>
    /// Invalid individual responses are silently discarded.
    /// </remarks>
    let FindControllers (options: Options) =
        let bind = options.bind
        let broadcast = options.broadcast
        let timeout = options.timeout
        let debug = options.debug
        let request = Encode.getControllerRequest 0u
        let result = UDP.broadcast (request, bind, broadcast, timeout, debug)

        let f =
            fun v ->
                match Decode.getControllerResponse v with
                | Ok response ->
                    let controller: Controller =
                        { Controller = response.controller
                          Address = response.address
                          Netmask = response.netmask
                          Gateway = response.gateway
                          MAC = response.MAC
                          Version = response.version
                          Date = response.date }

                    Some(controller)

                | _ -> None

        match result with
        | Ok replies -> replies |> List.choose (f) |> List.toArray |> Ok
        | Error err -> Error err

    /// <summary>
    /// Retrieves the IPv4 configuration, MAC address and version information for an access controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with a Controller record or Error.</returns>
    /// <remarks></remarks>
    let GetController (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getControllerRequest c.controller

            match exec c request Decode.getControllerResponse options with
            | Error err -> Error err
            | Ok response ->
                let record: Controller =
                    { Controller = response.controller
                      Address = response.address
                      Netmask = response.netmask
                      Gateway = response.gateway
                      MAC = response.MAC
                      Version = response.version
                      Date = response.date }

                Ok record

    /// <summary>
    /// Sets the controller IPv4 address, netmask and gateway address..
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="address">Controller IPv4 address.</param>
    /// <param name="netmask">Controller IPv4 netmask.</param>
    /// <param name="gateway">Gateway IPv4 address.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok or Error.</returns>
    /// <remarks>
    /// The controller does not return a response to this request - provided no network (or other) errors occur,
    /// it is assumed to be successful.
    /// </remarks>
    let SetIPv4 (controller: 'T, address: IPAddress, netmask: IPAddress, gateway: IPAddress, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let bind = options.bind
            let broadcast = options.broadcast
            let timeout = options.timeout
            let debug = options.debug

            let endpoint = c.endpoint
            let protocol = c.protocol
            let request = Encode.setIPv4Request c.controller address netmask gateway

            let result =
                match endpoint, protocol with
                | None, _ -> UDP.broadcastTo (request, bind, broadcast, timeout, debug)
                | Some(addr), Some("tcp") -> TCP.sendTo (request, bind, addr, timeout, debug)
                | Some(addr), _ -> UDP.sendTo (request, bind, addr, timeout, debug)

            match result with
            | Ok _ -> Ok()
            | Error err -> Error err

    /// <summary>
    /// Retrieves the controller event listener endpoint and auto-send interval.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with an Listener record or Error.</returns>
    let GetListener (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getListenerRequest c.controller

            match exec c request Decode.getListenerResponse options with
            | Ok response ->
                Ok
                    { Endpoint = response.endpoint
                      Interval = response.interval }
            | Error err -> Error err


    /// <summary>
    /// Sets the controller event listener IPv4 endpoint and the auto-send interval. The auto-send interval is the interval
    /// at which the controller sends the current status (including the most recent event) to the configured event listener.
    /// (events are always sent as the occur).
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="endpoint">IPv4 endpoint of event listener.</param>
    /// <param name="interval">Auto-send interval (seconds). A zero interval disables auto-send.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with true if the event listener endpoint was updated or Error.</returns>
    let SetListener (controller: 'T, endpoint: IPEndPoint, interval: uint8, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let address = endpoint.Address
            let port = uint16 endpoint.Port
            let request = Encode.setListenerRequest c.controller address port interval

            match exec c request Decode.setListenerResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Retrieves the controller current date and time.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with a DateTime value or Error.</returns>
    let GetTime (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getTimeRequest c.controller

            match exec c request Decode.getTimeResponse options with
            | Ok response -> Ok response.datetime
            | Error err -> Error err

    /// <summary>
    /// Sets the controller date and time.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="datetime">Date and time to set.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the DateTime value from the controller or Error.</returns>
    let SetTime (controller: 'T, datetime: DateTime, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setTimeRequest c.controller datetime

            match exec c request Decode.setTimeResponse options with
            | Ok response -> Ok response.datetime
            | Error err -> Error err

    /// <summary>
    /// Retrieves the control mode and unlocked delay for a door.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    let GetDoor (controller: 'T, door: uint8, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getDoorRequest c.controller door

            match exec c request Decode.getDoorResponse options with
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
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door ID [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>Ok with the door mode and unlock delay (or null) or Error.</returns>
    let SetDoor (controller: 'T, door: uint8, mode: DoorMode, delay: uint8, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setDoorRequest c.controller door mode delay

            match exec c request Decode.setDoorResponse options with
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
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="passcodes">Array of up to 4 passcodes in the range [0..999999], defaulting to 0 ('none')
    ///                         if the list contains less than 4 entries.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok with true value if the passcodes were updated or Error.
    /// </returns>
    let SetDoorPasscodes (controller: 'T, door: uint8, passcodes: uint32 array, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request =
                Encode.setDoorPasscodesRequest
                    c.controller
                    door
                    (passcodes |> Array.tryItem 0 |> Option.defaultValue 0u)
                    (passcodes |> Array.tryItem 1 |> Option.defaultValue 0u)
                    (passcodes |> Array.tryItem 2 |> Option.defaultValue 0u)
                    (passcodes |> Array.tryItem 3 |> Option.defaultValue 0u)

            match exec c request Decode.setDoorPasscodesResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Unlocks a door controlled by a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="door">Door number [1..4].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok if the request was processed, error otherwise. The Ok response should be
    /// checked for 'true'
    /// </returns>
    /// <remarks>
    /// </remarks>
    let OpenDoor (controller: 'T, door: uint8, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.openDoorRequest c.controller door

            match exec c request Decode.openDoorResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Retrieves the current status and most recent event from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Returns Ok with the controller status record, including the most recent event (if any), or Error.
    /// </returns>
    let GetStatus (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getStatusRequest c.controller

            match exec c request Decode.getStatusResponse options with
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
                      Event =
                        { Code = response.evt.event
                          Text = internationalisation.TranslateEventType(response.evt.event) }
                      AccessGranted = response.evt.granted
                      Door = response.evt.door
                      Direction = Enums.direction response.evt.direction
                      Card = response.evt.card
                      Reason =
                        { Code = response.evt.reason
                          Text = internationalisation.TranslateEventReason(response.evt.reason) } }

                match event.Index with
                | 0u -> Ok(status, Nullable())
                | _ -> Ok(status, Nullable(event))

            | Error err -> Error err

    /// <summary>
    /// Retrieves the number of card records stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with the number of cards stored on the controller or Error.
    /// </returns>
    let GetCards (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getCardsRequest c.controller

            match exec c request Decode.getCardsResponse options with
            | Ok response -> Ok response.cards
            | Error err -> Error err

    /// <summary>
    /// Retrieves the card record for the requested card number.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record matching the card number (or null if not found) or an error if the request failed.
    /// </returns>
    let GetCard (controller: 'T, card: uint32, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getCardRequest c.controller card

            match exec c request Decode.getCardResponse options with
            | Ok response when response.card = 0u -> // not found
                Ok(Nullable())
            | Ok response ->
                Ok(
                    Nullable
                        { Card = response.card
                          StartDate = response.startDate
                          EndDate = response.endDate
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
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record at the index (or null if not found or deleted) or an error if the request failed.
    /// </returns>
    let GetCardAtIndex (controller: 'T, index: uint32, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getCardAtIndexRequest c.controller index

            match exec c request Decode.getCardAtIndexResponse options with
            | Ok response when response.card = 0u -> // not found
                Ok(Nullable())
            | Ok response when response.card = 0xffffffffu -> // deleted
                Ok(Nullable())
            | Ok response ->
                Ok(
                    Nullable
                        { Card = response.card
                          StartDate = response.startDate
                          EndDate = response.endDate
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
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card record to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let PutCard (controller: 'T, card: Card, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.putCardRequest c.controller card

            match exec c request Decode.putCardResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Deletes a card record from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to delete.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let DeleteCard (controller: 'T, card: uint32, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.deleteCardRequest c.controller card

            match exec c request Decode.deleteCardResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Deletes all card records from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let DeleteAllCards (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.deleteAllCardsRequest c.controller

            match exec c request Decode.deleteAllCardsResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Retrieves the event record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="index">Index of event to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Event record at the index (or null if not found or deleted) or an error if the request failed. Returns an Ok(null) if the event
    // record does not exist or if the event has been overwritten.
    /// </returns>
    let GetEvent (controller: 'T, index: uint32, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getEventRequest c.controller index

            match exec c request Decode.getEventResponse options with
            | Ok response when response.event = 0x00uy -> // not found
                Ok(Nullable())
            | Ok response when response.event = 0xffuy -> // overwritten
                Ok(Nullable())
            | Ok response ->
                Ok(
                    Nullable
                        { Timestamp = response.timestamp
                          Index = response.index
                          Event =
                            { Code = response.event
                              Text = internationalisation.TranslateEventType(response.event) }
                          AccessGranted = response.granted
                          Door = response.door
                          Direction = Enums.direction response.direction
                          Card = response.card
                          Reason =
                            { Code = response.reason
                              Text = internationalisation.TranslateEventReason(response.reason) } }
                )
            | Error err -> Error err

    /// <summary>
    /// Retrieves the current event index from the controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with current controller event indexEvent record at the index or Error.
    /// </returns>
    let GetEventIndex (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getEventIndexRequest c.controller

            match exec c request Decode.getEventIndexResponse options with
            | Ok response -> Ok response.index
            | Error err -> Error err

    /// <summary>
    /// Sets the controller event index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="index">Event index.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the event index was updated (false if it was unchanged) or Error.
    /// </returns>
    let SetEventIndex (controller: 'T, index: uint32, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setEventIndexRequest c.controller index

            match exec c request Decode.setEventIndexResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Enables or disables events for door open and close, button presses, etc.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="enable">true to enabled 'special events'.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the 'special events' mode was set or Error.
    /// </returns>
    let RecordSpecialEvents (controller: 'T, enable: bool, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.recordSpecialEventsRequest c.controller enable

            match exec c request Decode.recordSpecialEventsResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Retrieves a time profile from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Time profile ID [2..254].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with time profile, Ok(null) if the requested profile does not exist or Error if the request failed.
    /// </returns>
    let GetTimeProfile (controller: 'T, profile: uint8, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.getTimeProfileRequest c.controller profile

            match exec c request Decode.getTimeProfileResponse options with
            | Ok response when response.profile = 0x00uy -> // not found
                Ok(Nullable())
            | Ok response when response.profile <> profile -> // incorrect profile
                Ok(Nullable())
            | Ok response ->
                Ok(
                    Nullable
                        { Profile = response.profile
                          StartDate = response.startDate
                          EndDate = response.endDate
                          Monday = response.monday
                          Tuesday = response.tuesday
                          Wednesday = response.wednesday
                          Thursday = response.thursday
                          Friday = response.friday
                          Saturday = response.saturday
                          Sunday = response.sunday
                          Segment1Start = response.segment1Start
                          Segment1End = response.segment1End
                          Segment2Start = response.segment2Start
                          Segment2End = response.segment2End
                          Segment3Start = response.segment3Start
                          Segment3End = response.segment3End
                          LinkedProfile = response.linkedProfile }
                )
            | Error err -> Error err

    /// <summary>
    /// Adds or updates an access time profile on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Access time profile to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the time profile was added/updated, or Error.
    /// </returns>
    let SetTimeProfile (controller: 'T, profile: TimeProfile, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setTimeProfileRequest c.controller profile

            match exec c request Decode.setTimeProfileResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Clears all access time profiles stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ClearTimeProfiles (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.clearTimeProfilesRequest c.controller

            match exec c request Decode.clearTimeProfilesResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Adds or updates a scheduled task on a controller. Added tasks are not scheduled to run
    /// until the tasklist has been refreshed.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="task">Task definition to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the task was added/updated, or Error.
    /// </returns>
    let AddTask (controller: 'T, task: Task, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.addTaskRequest c.controller task

            match exec c request Decode.addTaskResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Clears all scheduled tasks from the controller task list.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ClearTaskList (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.clearTaskListRequest c.controller

            match exec c request Decode.clearTaskListResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Schedules added tasks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let RefreshTaskList (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.refreshTaskListRequest c.controller

            match exec c request Decode.refreshTaskListResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Enables/disables remote access control management. The access controller will revert to standalone access
    /// control managment if it does not receive a command from the 'PC' at least every 30 seconds.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="enable">Enables or disables remote access control management.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let SetPCControl (controller: 'T, enable: bool, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setPCControlRequest c.controller enable

            match exec c request Decode.setPCControlResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Sets the access controller door interlocks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="interlock">Door interlocks (none, 1&2, 3&4, 1&2 and 3&4, 1&2&3 or 1&2&3&4.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let SetInterlock (controller: 'T, interlock: Interlock, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.setInterlockRequest c.controller interlock

            match exec c request Decode.setInterlockResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Activates/deactivates the access reader keypads attached to an access controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="reader1">Activates/deactivates the keypad for reader 1.</param>
    /// <param name="reader2">Activates/deactivates the keypad for reader 2.</param>
    /// <param name="reader3">Activates/deactivates the keypad for reader 3.</param>
    /// <param name="reader4">Activates/deactivates the keypad for reader 4.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let ActivateKeypads (controller: 'T, reader1: bool, reader2: bool, reader3: bool, reader4: bool, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request =
                Encode.activateKeypadsRequest c.controller reader1 reader2 reader3 reader4

            match exec c request Decode.activateKeypadsResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Restores the manufacturer defaults.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let RestoreDefaultParameters (controller: 'T, options: Options) =
        match resolve controller with
        | Error err -> Error err
        | Ok c ->
            let request = Encode.restoreDefaultParametersRequest c.controller

            match exec c request Decode.restoreDefaultParametersResponse options with
            | Ok response -> Ok response.ok
            | Error err -> Error err

    /// <summary>
    /// Listens for events from access controllers and dispatches received events to a handler.
    /// </summary>
    /// <param name="onevent">External event handler function.</param>
    /// <param name="onerror">External error handler function.</param>
    /// <param name="stop">Cancellation token to terminate event listener.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    let Listen (onevent: OnEvent, onerror: OnError, stop: CancellationToken, options: Options) =
        let bind = options.listen
        let debug = options.debug

        let handler (packet: byte array) =
            match Decode.listenEvent packet with
            | Ok e ->
                let status: Status =
                    { Door1Open = e.door1Open
                      Door2Open = e.door2Open
                      Door3Open = e.door3Open
                      Door4Open = e.door4Open
                      Button1Pressed = e.door1Button
                      Button2Pressed = e.door2Button
                      Button3Pressed = e.door3Button
                      Button4Pressed = e.door4Button
                      SystemError = e.systemError
                      SystemDateTime = e.systemDateTime
                      SequenceNumber = e.sequenceNumber
                      SpecialInfo = e.specialInfo
                      Relay1 = Enums.relay e.relays 0x01uy
                      Relay2 = Enums.relay e.relays 0x02uy
                      Relay3 = Enums.relay e.relays 0x04uy
                      Relay4 = Enums.relay e.relays 0x08uy
                      Input1 = Enums.input e.inputs 0x01uy
                      Input2 = Enums.input e.inputs 0x02uy
                      Input3 = Enums.input e.inputs 0x04uy
                      Input4 = Enums.input e.inputs 0x08uy }

                let event: ListenerEvent =
                    match e.event.index with
                    | 0u ->
                        { Controller = e.controller
                          Status = status
                          Event = Nullable() }
                    | _ ->
                        { Controller = e.controller
                          Status = status
                          Event =
                            Nullable(
                                { Timestamp = e.event.timestamp
                                  Index = e.event.index
                                  Event =
                                    { Code = e.event.event
                                      Text = internationalisation.TranslateEventType(e.event.event) }
                                  AccessGranted = e.event.granted
                                  Door = e.event.door
                                  Direction = Enums.direction e.event.direction
                                  Card = e.event.card
                                  Reason =
                                    { Code = e.event.reason
                                      Text = internationalisation.TranslateEventReason(e.event.reason) } }
                            ) }

                onevent.Invoke(event)

            | Error err -> onerror.Invoke(err)


        UDP.listen bind handler stop debug

    /// <summary>
    /// Translates an enum into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="value">enumerated value to translate.</param>
    /// <returns>
    /// Human readable string or "unknown (<value>)".
    /// </returns>
    let Translate (v: 'T) =
        match box v with
        | :? DoorMode as mode -> internationalisation.TranslateDoorMode(uint8 mode)
        | :? Direction as direction -> internationalisation.TranslateDoorDirection(uint8 direction)
        | :? Interlock as interlock -> internationalisation.TranslateDoorInterlock(uint8 interlock)
        | :? Relay as relay -> internationalisation.TranslateRelayState(uint8 relay)
        | :? Input as input -> internationalisation.TranslateInputState(uint8 input)
        | :? TaskCode as task -> internationalisation.TranslateTaskCode(uint8 task)
        | :? EventType as event -> internationalisation.TranslateEventType(event.Code)
        | :? EventReason as reason -> internationalisation.TranslateEventReason(reason.Code)
        | _ -> $"#{v}"
