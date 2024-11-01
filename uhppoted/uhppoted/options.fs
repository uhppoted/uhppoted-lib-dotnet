namespace uhppoted

open System.Net

type Options =
    { bind: IPEndPoint
      broadcast: IPEndPoint
      listen: IPEndPoint
      debug: bool }

type OptionsBuilder() =
    let mutable bind: IPEndPoint = IPEndPoint(IPAddress.Any, 0)
    let mutable broadcast: IPEndPoint = IPEndPoint(IPAddress.Broadcast, 60000)
    let mutable listen: IPEndPoint = IPEndPoint(IPAddress.Any, 60001)
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

    member this.WithDebug(v: bool) =
        debug <- v
        this

    member this.build() =
        { bind = bind
          broadcast = broadcast
          listen = listen
          debug = debug }
