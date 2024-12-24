namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

type internal IResponse =
    abstract member controller: uint32

type internal GetControllerResponse =
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
      door1Open: bool
      door2Open: bool
      door3Open: bool
      door4Open: bool
      door1Button: bool
      door2Button: bool
      door3Button: bool
      door4Button: bool
      systemError: uint8
      systemDateTime: DateTime Nullable
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      evt:
          {| index: uint32
             event: uint8
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

type GetCardResponse =
    { controller: uint32
      card: uint32
      startDate: DateOnly Nullable
      endDate: DateOnly Nullable
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal GetCardAtIndexResponse =
    { controller: uint32
      card: uint32
      startDate: DateOnly Nullable
      endDate: DateOnly Nullable
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal PutCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal DeleteCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal DeleteAllCardsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal GetEventResponse =
    { controller: uint32
      index: uint32
      event: uint8
      granted: bool
      door: uint8
      direction: uint8
      card: uint32
      timestamp: DateTime Nullable
      reason: uint8 }

    interface IResponse with
        member this.controller = this.controller

type internal GetEventIndexResponse =
    { controller: uint32
      index: uint32 }

    interface IResponse with
        member this.controller = this.controller

type internal SetEventIndexResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal RecordSpecialEventsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal GetTimeProfileResponse =
    { controller: uint32
      profile: uint8
      startDate: DateOnly Nullable
      endDate: DateOnly Nullable
      monday: bool
      tuesday: bool
      wednesday: bool
      thursday: bool
      friday: bool
      saturday: bool
      sunday: bool
      segment1Start: TimeOnly Nullable
      segment1End: TimeOnly Nullable
      segment2Start: TimeOnly Nullable
      segment2End: TimeOnly Nullable
      segment3Start: TimeOnly Nullable
      segment3End: TimeOnly Nullable
      linkedProfile: uint8 }

    interface IResponse with
        member this.controller = this.controller

type internal SetTimeProfileResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ClearTimeProfilesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal AddTaskResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ClearTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal RefreshTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal SetPCControlResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal SetInterlockResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ActivateKeypadsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal RestoreDefaultParametersResponse =
    { controller: uint32
      ok: bool }

    interface IResponse with
        member this.controller = this.controller

type internal ListenEvent =
    { controller: uint32
      door1Open: bool
      door2Open: bool
      door3Open: bool
      door4Open: bool
      door1Button: bool
      door2Button: bool
      door3Button: bool
      door4Button: bool
      systemError: uint8
      systemDateTime: DateTime Nullable
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      event:
          {| index: uint32
             event: uint8
             granted: bool
             door: uint8
             direction: uint8
             card: uint32
             timestamp: DateTime Nullable
             reason: uint8 |} }

    interface IResponse with
        member this.controller = this.controller
