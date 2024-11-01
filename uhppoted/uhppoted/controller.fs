namespace uhppoted

open System.Net

type Controller =
    { controller: uint32
      address: Option<IPEndPoint>
      protocol: Option<string> }

type ControllerBuilder(controller: uint32) =
    let mutable address: Option<IPEndPoint> = None
    let mutable protocol: Option<string> = None

    member this.With(value: obj) =
        match value with
        | :? IPEndPoint as v -> address <- Some v
        | :? string as v -> protocol <- Some v
        | _ -> ()

        this

    member this.WithAddress(v: IPEndPoint) =
        address <- Some v
        this

    member this.WithProtocol(v: string) =
        protocol <- Some v
        this

    member this.build() =
        { controller = controller
          address = address
          protocol = protocol }
