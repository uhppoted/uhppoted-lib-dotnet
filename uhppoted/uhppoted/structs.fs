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
      date: DateOnly Nullable }

type GetListenerResponse =
    { controller: uint32
      endpoint: IPEndPoint
      interval: uint8 }

type SetListenerResponse = { controller: uint32; ok: bool }

type GetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

type SetTimeResponse =
    { controller: uint32
      datetime: DateTime Nullable }

type GetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }

type SetDoorResponse =
    { controller: uint32
      door: uint8
      mode: uint8
      delay: uint8 }
