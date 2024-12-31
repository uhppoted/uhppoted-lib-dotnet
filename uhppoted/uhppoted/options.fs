namespace uhppoted

open System.Net

/// Container class for the network configuration used to connect to an access controller.
type Options =
    {
        /// IPv4 endpoint to which to bind. Default value is INADDR_ANY (0.0.0.0:0).
        bind: IPEndPoint

        /// IPv4 endpoint to which to broadcast UDP requests. Default value is '255.255.255.255:60000'.
        broadcast: IPEndPoint

        /// IPv4 endpoint on which to listen for controller events. Defaults to '0.0.0.0:60001.
        listen: IPEndPoint

        /// Operation timeout (milliseconds).
        timeout: int

        /// Logs controller requests and responses to the console if enabled.
        debug: bool
    }

/// Convenience 'Options' builder implementation for C# and VB.NET.
type OptionsBuilder() =
    let mutable bind: IPEndPoint = IPEndPoint(IPAddress.Any, 0)
    let mutable broadcast: IPEndPoint = IPEndPoint(IPAddress.Broadcast, 60000)
    let mutable listen: IPEndPoint = IPEndPoint(IPAddress.Any, 60001)
    let mutable timeout: int = 1000
    let mutable debug: bool = false

    /// Sets the `bind` endpoint.
    /// - Parameter `endpoint`: IPv4 'bind' endpoint.
    /// - Returns: The updated builder instance.
    member this.WithBind(endpoint: IPEndPoint) =
        bind <- endpoint
        this

    /// Sets the `broadcast` endpoint.
    /// - Parameter `endpoint`: IPv4 'broadcast' endpoint.
    /// - Returns: The updated builder instance.
    member this.WithBroadcast(endpoint: IPEndPoint) =
        broadcast <- endpoint
        this

    /// Sets the `listen` endpoint.
    /// - Parameter `endpoint`: IPv4 'listen' endpoint.
    /// - Returns: The updated builder instance.
    member this.WithListen(endpoint: IPEndPoint) =
        listen <- endpoint
        this

    /// Sets the operation timeout.
    /// - Parameter `ms`: Operation timeout (milliseconds).
    /// - Returns: The updated builder instance.
    member this.WithTimeout(ms: int) =
        timeout <- ms
        this

    /// Enables (or disables) logging of controller requests and responses to the console.
    /// - Parameter `enable`: `true` to enable debugging; `false` to disable.
    /// - Returns: The updated builder instance.
    member this.WithDebug(enable: bool) =
        debug <- enable
        this

    /// Builds the `Options` instance.
    /// - Returns: An `Options` instance populated with the current settings.
    member this.Build() =
        { bind = bind
          broadcast = broadcast
          listen = listen
          timeout = timeout
          debug = debug }
