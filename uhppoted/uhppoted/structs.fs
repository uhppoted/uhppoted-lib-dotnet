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

type GetStatusResponse =
    { controller: uint32
      door1_open: bool
      door2_open: bool
      door3_open: bool
      door4_open: bool
      door1_button: bool
      door2_button: bool
      door3_button: bool
      door4_button: bool
      system_error: uint8
      system_datetime: DateTime Nullable
      sequence_number: uint32
      special_info: uint8
      relays: uint8
      inputs: uint8
      evt:
          {| index: uint32
             event_type: uint8
             granted: bool
             door: uint8
             direction: uint8
             card: uint32
             timestamp: DateTime Nullable
             reason: uint8 |} }

    interface IResponse with
        member this.controller = this.controller

type GetCardsResponse =
    { controller: uint32
      cards: uint32 }

    interface IResponse with
        member this.controller = this.controller
