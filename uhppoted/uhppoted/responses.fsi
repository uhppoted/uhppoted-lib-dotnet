namespace uhppoted

type internal IResponse =
    abstract controller: uint32

type internal GetControllerResponse =
    { controller: uint32
      address: System.Net.IPAddress
      netmask: System.Net.IPAddress
      gateway: System.Net.IPAddress
      MAC: System.Net.NetworkInformation.PhysicalAddress
      version: string
      date: System.Nullable<System.DateOnly> }

    interface IResponse

type GetListenerResponse =
    { controller: uint32
      endpoint: System.Net.IPEndPoint
      interval: uint8 }

    interface IResponse

type SetListenerResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type GetTimeResponse =
    { controller: uint32
      datetime: System.Nullable<System.DateTime> }

    interface IResponse

type SetTimeResponse =
    { controller: uint32
      datetime: System.Nullable<System.DateTime> }

    interface IResponse

type GetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse

type SetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

    interface IResponse

type SetDoorPasscodesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type OpenDoorResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

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
      systemDateTime: System.Nullable<System.DateTime>
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      evt:
          {| card: uint32
             direction: uint8
             door: uint8
             event: uint8
             granted: bool
             index: uint32
             reason: uint8
             timestamp: System.Nullable<System.DateTime> |} }

    interface IResponse

type GetCardsResponse =
    { controller: uint32
      cards: uint32 }

    interface IResponse

type GetCardResponse =
    { controller: uint32
      card: uint32
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse

type internal GetCardAtIndexResponse =
    { controller: uint32
      card: uint32
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      door1: uint8
      door2: uint8
      door3: uint8
      door4: uint8
      PIN: uint32 }

    interface IResponse

type internal PutCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal DeleteCardResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal DeleteAllCardsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal GetEventResponse =
    { controller: uint32
      index: uint32
      event: uint8
      granted: bool
      door: uint8
      direction: uint8
      card: uint32
      timestamp: System.Nullable<System.DateTime>
      reason: uint8 }

    interface IResponse

type internal GetEventIndexResponse =
    { controller: uint32
      index: uint32 }

    interface IResponse

type internal SetEventIndexResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RecordSpecialEventsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal GetTimeProfileResponse =
    { controller: uint32
      profile: uint8
      startDate: System.Nullable<System.DateOnly>
      endDate: System.Nullable<System.DateOnly>
      monday: bool
      tuesday: bool
      wednesday: bool
      thursday: bool
      friday: bool
      saturday: bool
      sunday: bool
      segment1Start: System.Nullable<System.TimeOnly>
      segment1End: System.Nullable<System.TimeOnly>
      segment2Start: System.Nullable<System.TimeOnly>
      segment2End: System.Nullable<System.TimeOnly>
      segment3Start: System.Nullable<System.TimeOnly>
      segment3End: System.Nullable<System.TimeOnly>
      linkedProfile: uint8 }

    interface IResponse

type internal SetTimeProfileResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ClearTimeProfilesResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal AddTaskResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ClearTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RefreshTaskListResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal SetPCControlResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal SetInterlockResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal ActivateKeypadsResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

type internal RestoreDefaultParametersResponse =
    { controller: uint32
      ok: bool }

    interface IResponse

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
      systemDateTime: System.Nullable<System.DateTime>
      sequenceNumber: uint32
      specialInfo: uint8
      relays: uint8
      inputs: uint8
      event:
          {| card: uint32
             direction: uint8
             door: uint8
             event: uint8
             granted: bool
             index: uint32
             reason: uint8
             timestamp: System.Nullable<System.DateTime> |} }

    interface IResponse
