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
        | :? IPEndPoint as a -> address <- Some a
        | :? string as p -> protocol <- Some p
        | _ -> ()

        this

    member this.WithAddress(a: IPEndPoint) =
        address <- Some a
        this

    member this.WithProtocol(p: string) =
        protocol <- Some p
        this

    member this.build() =
        { controller = controller
          address = address
          protocol = protocol }
