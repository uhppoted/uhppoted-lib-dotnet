namespace uhppoted

open System
open System.Net
open System.Net.NetworkInformation

type GetControllerResponse =
    { controller: uint32
      address: IPAddress
      netmask: IPAddress
      gateway: IPAddress
      MAC: PhysicalAddress
      version: string
      date: Option<DateOnly> }

type GetListenerResponse =
    { controller: uint32
      endpoint: IPEndPoint
      interval: uint8 }

type SetListenerResponse = { controller: uint32; ok: bool }

type GetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }
