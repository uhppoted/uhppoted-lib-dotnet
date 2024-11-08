namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

type IResponse =
    abstract member controller: uint32

type GetControllerResponse =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: DateOnly Nullable }

    interface IResponse with
        member this.controller = this.controller

type GetListenerResponse =
    { controller: uint32
      endpoint: IPEndPoint
      interval: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetListenerResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type GetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

    interface IResponse with
        member this.controller = this.controller

type SetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

    interface IResponse with
        member this.controller = this.controller

type GetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse with
        member this.controller = this.controller

type SetDoorPasscodesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type OpenDoorResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller
