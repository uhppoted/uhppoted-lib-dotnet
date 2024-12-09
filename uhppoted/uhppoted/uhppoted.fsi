namespace uhppoted

open System
open System.Net

/// <summary>Defines the door control modes for an access controller.</summary>
type DoorMode =
    /// <summary>Unknown door control mode.</summary>
    | Unknown = 0

    /// <summary>Door is always unlocked.</summary>
    | NormallyOpen = 1

    /// <summary>Door is always locked.</summary>
    | NormallyClosed = 2

    /// <summary>Door lock is managed by access controller.</summary>
    | Controlled = 3

[<Struct>]
type Controller =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: Nullable<DateOnly> }

type Options =
    {
        /// IPv4 endpoint to which to bind. Default value is INADDR_ANY (0.0.0.0:0).
        bind: IPEndPoint

        /// IPv4 endpoint to which to broadcast UDP requests. Default value is '255.255.255.255:60000'.
        broadcast: IPEndPoint

        /// IPv4 endpoint on which to listen for controller events. Defaults to '0.0.0.0:60001'.
        listen: IPEndPoint

        /// Optional IPv4 controller address:port. Required if the controller is not accessible via UDP broadcast.
        endpoint: Option<IPEndPoint>

        /// Optional 'protocol' to connect to controller. Valid values are currently 'udp' or 'tcp', defaulting to 'udp'.
        protocol: Option<string>

        /// Logs controller requests and responses to the console if enabled.
        debug: bool
    }

module Uhppoted =

    /// Retrieves a list of controllers on the local LAN accessible via a UDP broadcast.
    val FindControllers :
        timeout: int *
        options: Options ->
        Result<Controller array, string>

    /// Retrieves the IPv4 configuration, MAC address and version information for an access controller.
    val GetController :
        controller: uint32 *
        timeout: int *
        options: Options ->
        Result<Controller, string>

    /// Sets the controller IPv4 address, netmask, and gateway address.
    val SetIPv4 :
        controller: uint32 *
        address: IPAddress *
        netmask: IPAddress *
        gateway: IPAddress *
        timeout: int *
        options: Options ->
        Result<unit, string>

    /// Retrieves the controller event listener endpoint and auto-send interval.
    val GetListener :
        controller: uint32 *
        timeout: int *
        options: Options ->
        Result<Listener, string>

    /// Sets the controller event listener IPv4 endpoint and auto-send interval.
    val SetListener :
        controller: uint32 *
        endpoint: IPEndPoint *
        interval: uint8 *
        timeout: int *
        options: Options ->
        Result<bool, string>

    /// Retrieves the controller's current date and time.
    val GetTime :
        controller: uint32 *
        timeout: int *
        options: Options ->
        Result<DateTime, string>

    /// Sets the controller's date and time.
    val SetTime :
        controller: uint32 *
        datetime: DateTime *
        timeout: int *
        options: Options ->
        Result<DateTime, string>

    /// Retrieves the control mode and unlocked delay for a door.
    val GetDoor :
        controller: uint32 *
        door: uint8 *
        timeout: int *
        options: Options ->
        Result<Nullable<Door>, string>

    /// Sets the control mode and unlocked delay for a door.
    val SetDoor :
        controller: uint32 *
        door: uint8 *
        mode: DoorMode *
        delay: uint8 *
        timeout: int *
        options: Options ->
        Result<Nullable<Door>, string>

    /// Sets up to 4 passcodes for a controller door.
    val SetDoorPasscodes :
        controller: uint32 *
        door: uint8 *
        passcodes: uint32 array *
        timeout: int *
        options: Options ->
        Result<bool, string>

    /// Unlocks a door controlled by a controller.
    val UnlockDoor :
        controller: uint32 *
        door: uint8 *
        timeout: int *
        options: Options ->
        Result<unit, string>
