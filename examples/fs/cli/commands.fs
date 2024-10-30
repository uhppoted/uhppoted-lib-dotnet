module Commands

open System
open System.Net
open uhppoted

let CONTROLLER = 405419896u
let ADDRESS = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000))
let PROTOCOL = Some("udp")
let TIMEOUT = 1000
let DEBUG = true

let YYYYMMDD date =
    match date with
    | Some(v: DateOnly) -> v.ToString("yyyy-MM-dd")
    | None -> "---"

let argparse args flag defval = defval

let get_controllers args =
    let controllers = Uhppoted.get_all_controllers (TIMEOUT, DEBUG)

    printf "get-all-controllers: %d\n" controllers.Length

    controllers
    |> List.iter (fun response ->
        printf "  controller %u\n" response.controller
        printf "    address  %A\n" response.address
        printf "    netmask  %A\n" response.netmask
        printf "    gateway  %A\n" response.gateway
        printf "    MAC      %A\n" response.MAC
        printf "    version  %s\n" response.version
        printf "    date     %s\n" (YYYYMMDD response.date)
        printf "\n")

let get_controller args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_controller (controller, TIMEOUT, DEBUG) with
    | Ok response ->
        printf "get-controller\n"
        printf "  controller %u\n" response.controller
        printf "    address  %A\n" response.address
        printf "    netmask  %A\n" response.netmask
        printf "    gateway  %A\n" response.gateway
        printf "    MAC      %A\n" response.MAC
        printf "    version  %s\n" response.version
        printf "    date     %s\n" (YYYYMMDD response.date)
        printf "\n"

    | Error err -> printf "  ** ERROR %A\n" err

let set_IPv4 args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let address = IPAddress.Parse("192.168.1.100")
    let netmask = IPAddress.Parse("255.255.255.0")
    let gateway = IPAddress.Parse("192.168.1.1")

    match Uhppoted.set_IPv4 (controller, address, netmask, gateway, TIMEOUT, DEBUG) with
    | Ok response ->
        printf "set-IPv4\n"
        printf "  ok\n"
        printf "\n"

    | Error err -> printf "  ** ERROR %A\n" err

let get_listener args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_listener (controller, TIMEOUT, DEBUG) with
    | Ok response ->
        printf "get-listener\n"
        printf "  controller %u\n" response.controller
        printf "    endpoint %A\n" response.endpoint
        printf "    interval %ds\n" response.interval
        printf "\n"

    | Error err -> printf "  ** ERROR %A\n" err
