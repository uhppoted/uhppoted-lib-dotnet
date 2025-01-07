namespace uhppoted

module Uhppoted =
    val internal resolve: controller: 'T -> Result<C, Err>

    /// Retrieves a list of controllers on the local LAN accessible via a UDP broadcast.
    val FindControllers: options: Options -> Result<Controller array, Err>

    /// Retrieves the IPv4 configuration, MAC address and version information for an access controller.
    val GetController: controller: 'T * options: Options -> Result<Controller, Err>

    /// Sets the controller IPv4 address, netmask and gateway address..
    val SetIPv4:
        controller: 'T *
        address: System.Net.IPAddress *
        netmask: System.Net.IPAddress *
        gateway: System.Net.IPAddress *
        options: Options ->
            Result<unit, Err>

    /// Retrieves the controller event listener endpoint and auto-send interval.
    val GetListener: controller: 'T * options: Options -> Result<Listener, Err>

    /// Sets the controller event listener IPv4 endpoint and the auto-send interval.
    val SetListener:
        controller: 'T * endpoint: System.Net.IPEndPoint * interval: uint8 * options: Options -> Result<bool, Err>

    /// Retrieves the controller current date and time.
    val GetTime: controller: 'T * options: Options -> Result<System.Nullable<System.DateTime>, Err>

    /// Sets the controller date and time.
    val SetTime:
        controller: 'T * datetime: System.DateTime * options: Options -> Result<System.Nullable<System.DateTime>, Err>

    /// Retrieves the control mode and unlocked delay for a door.
    val GetDoor: controller: 'T * door: uint8 * options: Options -> Result<Door, Err>

    /// Sets the control mode and unlocked delay for a door.
    val SetDoor: controller: 'T * door: uint8 * mode: DoorMode * delay: uint8 * options: Options -> Result<Door, Err>

    /// Sets up to 4 passcodes for a controller door.
    val SetDoorPasscodes: controller: 'T * door: uint8 * passcodes: uint32 array * options: Options -> Result<bool, Err>

    /// Unlocks a door controlled by a controller.
    val OpenDoor: controller: 'T * door: uint8 * options: Options -> Result<bool, Err>

    /// Retrieves the current status and most recent event from a controller.
    val GetStatus: controller: 'T * options: Options -> Result<(Status * System.Nullable<Event>), Err>

    /// Retrieves the number of card records stored on a controller.
    val GetCards: controller: 'T * options: Options -> Result<uint32, Err>

    /// <summary>
    /// Retrieves the card record for the requested card number.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record matching the card number (or null if not found) or an error if the request failed.
    /// </returns>
    val GetCard: controller: 'T * card: uint32 * options: Options -> Result<Card, Err>

    /// <summary>
    /// Retrieves the card record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Card record at the index (or null if not found or deleted) or an error if the request failed.
    /// </returns>
    val GetCardAtIndex: controller: 'T * index: uint32 * options: Options -> Result<System.Nullable<Card>, Err>

    /// <summary>
    /// Adds or updates a card record on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card record to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val PutCard: controller: 'T * card: Card * options: Options -> Result<bool, Err>

    /// <summary>
    /// Deletes a card record from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="card">Card number to delete.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val DeleteCard: controller: 'T * card: uint32 * options: Options -> Result<bool, Err>

    /// <summary>
    /// Deletes all card records from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val DeleteAllCards: controller: 'T * options: Options -> Result<bool, Err>

    /// <summary>
    /// Retrieves the event record at the supplied index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="index">Index of event to retrieve.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Event record at the index, EventNotFound if the index is greater than the last stored event,
    /// EventOverwritten if the index is before the first stored event or an error if the request failed.
    /// </returns>
    val GetEvent: controller: 'T * index: uint32 * options: Options -> Result<Event, Err>

    /// <summary>
    /// Retrieves the current event index from the controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with current controller event indexEvent record at the index or Error.
    /// </returns>
    val GetEventIndex: controller: 'T * options: Options -> Result<uint32, Err>

    /// <summary>
    /// Sets the controller event index.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="index">Event index.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the event index was updated (false if it was unchanged) or Error.
    /// </returns>
    val SetEventIndex: controller: 'T * index: uint32 * options: Options -> Result<bool, Err>

    /// <summary>
    /// Enables or disables events for door open and close, button presses, etc.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="enable">true to enabled 'special events'.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the 'special events' mode was set or Error.
    /// </returns>
    val RecordSpecialEvents: controller: 'T * enable: bool * options: Options -> Result<bool, Err>

    /// <summary>
    /// Retrieves a time profile from a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Time profile ID [2..254].</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with time profile, Error TimeProfileNotFoundif the requested profile does not exist or
    /// Error if the request failed.
    /// </returns>
    val GetTimeProfile: controller: 'T * profile: uint8 * options: Options -> Result<TimeProfile, Err>

    /// <summary>
    /// Adds or updates an access time profile on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="profile">Access time profile to add or update.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Ok with true if the time profile was added/updated, or Error.
    /// </returns>
    val SetTimeProfile: controller: 'T * profile: TimeProfile * options: Options -> Result<bool, Err>

    /// <summary>
    /// Clears all access time profiles stored on a controller.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val ClearTimeProfiles: controller: 'T * options: Options -> Result<bool, Err>

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
    val AddTask: controller: 'T * task: Task * options: Options -> Result<bool, Err>

    /// <summary>
    /// Clears all scheduled tasks from the controller task list.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val ClearTaskList: controller: 'T * options: Options -> Result<bool, Err>

    /// <summary>
    /// Schedules added tasks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val RefreshTaskList: controller: 'T * options: Options -> Result<bool, Err>

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
    val SetPCControl: controller: 'T * enable: bool * options: Options -> Result<bool, Err>

    /// <summary>
    /// Sets the access controller door interlocks.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="interlock">Door interlocks (none, 1&2, 3&4, 1&2 and 3&4, 1&2&3 or 1&2&3&4.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val SetInterlock: controller: 'T * interlock: Interlock * options: Options -> Result<bool, Err>

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
    val ActivateKeypads:
        controller: 'T * reader1: bool * reader2: bool * reader3: bool * reader4: bool * options: Options ->
            Result<bool, Err>

    /// <summary>
    /// Restores the manufacturer defaults.
    /// </summary>
    /// <param name="controller">Controller ID or struct with controller ID, endpoint and protocol.</param>
    /// <param name="options">Bind, broadcast and listen addresses.</param>
    /// <returns>
    /// Result with the boolean success/fail result or an Error if the request failed.
    /// </returns>
    val RestoreDefaultParameters: controller: 'T * options: Options -> Result<bool, Err>

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
    val Listen:
        onevent: OnEvent * onerror: OnError * stop: System.Threading.CancellationToken * options: Options ->
            Result<unit, Err>

    /// <summary>
    /// Translates an enum into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="value">enumerated value to translate.</param>
    /// <returns>
    /// Human readable string or "unknown (<value>)".
    /// </returns>
    val Translate: v: 'T -> string
