module Commands

open System
open System.Net
open uhppoted

let YYYYMMDD date =
    match date with
    | Some(v: DateOnly) -> v.ToString("yyyy-MM-dd")
    | None -> "---"

let TIMEOUT = 1000
let DEBUG = true

let get_controllers () =
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

let get_controller () =
    let controller =
        { controller = 405419896u
          address = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000))
          protocol = Some("tcp") }

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
