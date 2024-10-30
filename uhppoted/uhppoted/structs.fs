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
