namespace uhppoted

open System.Net

type Options =
    { bind: IPEndPoint
      broadcast: IPEndPoint
      listen: IPEndPoint
      destination: Option<IPEndPoint>
      protocol: Option<string>
      debug: bool }

type OptionsBuilder() =
    let mutable bind: IPEndPoint = IPEndPoint(IPAddress.Any, 0)
    let mutable broadcast: IPEndPoint = IPEndPoint(IPAddress.Broadcast, 60000)
    let mutable listen: IPEndPoint = IPEndPoint(IPAddress.Any, 60001)
    let mutable destination: Option<IPEndPoint> = None
    let mutable protocol: Option<string> = None
    let mutable debug: bool = false

    member this.WithBind(v: IPEndPoint) =
        bind <- v
        this

    member this.WithBroadcast(v: IPEndPoint) =
        broadcast <- v
        this

    member this.WithListen(v: IPEndPoint) =
        listen <- v
        this

    member this.WithDestination(v: IPEndPoint) =
        destination <- Some(v)
        this

    member this.WithProtocol(v: string) =
        protocol <- Some(v)
        this

    member this.WithDebug(v: bool) =
        debug <- v
        this

    member this.build() =
        { bind = bind
          broadcast = broadcast
          listen = listen
          destination = destination
          protocol = protocol
          debug = debug }
