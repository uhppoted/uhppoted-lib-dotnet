namespace uhppoted

open System
open System.Net

type GetControllerResponse = { 
  controller: uint32
  address  :  IPAddress
  netmask :  IPAddress
  gateway :  IPAddress
  MAC: byte array
  version: string
  date: Option<DateOnly>
}
