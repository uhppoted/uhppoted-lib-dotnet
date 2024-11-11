module Commands

open System
open System.Net
open uhppoted

type command =
    { command: string
      description: string
      f: string list -> Result<unit, string> }

let CONTROLLER = 405419896u
let ADDRESS = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000))
let PROTOCOL = Some("udp")
let TIMEOUT = 1000
let MODE = 2uy
let DELAY = 7uy

let OPTIONS: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      debug = true }

let YYYYMMDD (date: Nullable<DateOnly>) =
    if date.HasValue then
        date.Value.ToString("yyyy-MM-dd")
    else
        "---"

let YYYYMMDDHHmmss (datetime: Nullable<DateTime>) =
    if datetime.HasValue then
        datetime.Value.ToString("yyyy-MM-dd HH:mm:ss")
    else
        "---"

let argparse args flag defval = defval

let get_controllers args =
    match Uhppoted.get_all_controllers (TIMEOUT, OPTIONS) with
    | Ok controllers ->
        printf "get-all-controllers: %d\n" controllers.Length

        controllers
        |> Array.iter (fun v ->
            printf "  controller %u\n" v.controller
            printf "    address  %A\n" v.address
            printf "    netmask  %A\n" v.netmask
            printf "    gateway  %A\n" v.gateway
            printf "    MAC      %A\n" v.MAC
            printf "    version  %s\n" v.version
            printf "    date     %s\n" (YYYYMMDD v.date)
            printf "\n")

        Ok()
    | Error err -> Error(err)

let get_controller args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_controller (controller, TIMEOUT, OPTIONS) with
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
        Ok()
    | Error err ->
        printf "  ** ERROR %A\n" err
        Error(err)

let set_IPv4 args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let address = IPAddress.Parse("192.168.1.100")
    let netmask = IPAddress.Parse("255.255.255.0")
    let gateway = IPAddress.Parse("192.168.1.1")

    match Uhppoted.set_IPv4 (controller, address, netmask, gateway, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "set-IPv4\n"
        printf "  ok\n"
        printf "\n"
        Ok()
    | Error err ->
        printf "  ** ERROR %A\n" err
        Error(err)

let get_listener args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_listener (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "get-listener\n"
        printf "  controller %u\n" response.controller
        printf "    endpoint %A\n" response.endpoint
        printf "    interval %ds\n" response.interval
        printf "\n"
        Ok()
    | Error err ->
        printf "  ** ERROR %A\n" err
        Error(err)

let set_listener args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let endpoint = IPEndPoint.Parse("192.168.1.100:60001")
    let interval = 30uy

    match Uhppoted.set_listener (controller, endpoint, interval, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "set-listener\n"
        printf "  controller %u\n" response.controller
        printf "          ok %A\n" response.ok
        printf "\n"
        Ok()
    | Error err ->
        printf "  ** ERROR %A\n" err
        Error(err)

let get_time args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_time (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "get-time\n"
        printf "  controller %u\n" response.controller
        printf "    datetime %s\n" (YYYYMMDDHHmmss response.datetime)
        printf "\n"
        Ok()
    | Error err -> Error(err)

let set_time args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let datetime = DateTime.Now

    match Uhppoted.set_time (controller, datetime, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "set-time\n"
        printf "  controller %u\n" response.controller
        printf "    datetime %s\n" (YYYYMMDDHHmmss response.datetime)
        printf "\n"
        Ok()
    | Error err -> Error(err)

let get_door args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy

    match Uhppoted.get_door (controller, door, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "get-door\n"
        printf "  controller %u\n" response.controller
        printf "        door %d\n" response.door
        printf "        mode %d\n" response.mode
        printf "       delay %ds\n" response.delay
        printf "\n"
        Ok()
    | Error err -> Error(err)

let set_door args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy
    let mode = MODE
    let delay = DELAY

    match Uhppoted.set_door (controller, door, mode, delay, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "set-door\n"
        printf "  controller %u\n" response.controller
        printf "        door %d\n" response.door
        printf "        mode %d\n" response.mode
        printf "       delay %ds\n" response.delay
        printf "\n"
        Ok()
    | Error err -> Error(err)

let set_door_passcodes args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy
    let passcodes = [| 1234u; 545321u; 0u; 999999u |]

    match
        Uhppoted.set_door_passcodes (
            controller,
            door,
            passcodes[0],
            passcodes[1],
            passcodes[2],
            passcodes[3],
            TIMEOUT,
            OPTIONS
        )
    with
    | Ok response ->
        printf "set-door-passcodes\n"
        printf "  controller %u\n" response.controller
        printf "          ok %b\n" response.ok
        printf "\n"
        Ok()
    | Error err -> Error(err)

let open_door args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy

    match Uhppoted.open_door (controller, door, TIMEOUT, OPTIONS) with
    | Ok response ->
        printf "open-door\n"
        printf "  controller %u\n" response.controller
        printf "          ok %b\n" response.ok
        printf "\n"
        Ok()
    | Error err -> Error(err)

let get_status args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_status (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-status"
        printfn "         controller %u" response.controller
        printfn "        door 1 open %b" response.door1_open
        printfn "        door 2 open %b" response.door2_open
        printfn "        door 3 open %b" response.door3_open
        printfn "        door 4 open %b" response.door3_open
        printfn "   button 1 pressed %b" response.door1_button
        printfn "   button 2 pressed %b" response.door1_button
        printfn "   button 3 pressed %b" response.door1_button
        printfn "   button 4 pressed %b" response.door1_button
        printfn "       system error %u" response.system_error
        printfn "   system date/time %A" (YYYYMMDDHHmmss(response.system_datetime))
        printfn "       sequence no. %u" response.sequence_number
        printfn "       special info %u" response.special_info
        printfn "             relays %02x" response.relays
        printfn "             inputs %02x" response.inputs
        printfn ""
        printfn "    event index     %u" response.evt.index
        printfn "          event     %u" response.evt.event_type
        printfn "          granted   %b" response.evt.granted
        printfn "          door      %u" response.evt.door
        printfn "          direction %u" response.evt.direction
        printfn "          card      %u" response.evt.card
        printfn "          timestamp %A" (YYYYMMDDHHmmss(response.evt.timestamp))
        printfn "          reason    %u" response.evt.reason
        printfn ""
        Ok()
    | Error err -> Error(err)

let commands =
    [ { command = "get-all-controllers"
        description = "Retrieves a list of controllers accessible on the local LAN"
        f = get_controllers }
      { command = "get-controller"
        description = "Retrieves the controller information for a specific controller"
        f = get_controller }
      { command = "set-IPv4"
        description = "Sets the controller IPv4 address, netmask and gateway"
        f = set_IPv4 }
      { command = "get-listener"
        description = "Retrieves the controller event listener address:port and auto-send interval"
        f = get_listener }
      { command = "set-listener"
        description = "Sets the controller event listener address:port and auto-send interval"
        f = set_listener }
      { command = "get-time"
        description = "Retrieves the controller system date and time"
        f = get_time }
      { command = "set-time"
        description = "Sets the controller system date and time"
        f = set_time }
      { command = "get-door"
        description = "Retrieves a controller door mode and delay settings"
        f = get_door }
      { command = "set-door"
        description = "Sets a controller door mode and delay"
        f = set_door }
      { command = "set-door-passcodes"
        description = "Sets the supervisor passcodes for a controller door"
        f = set_door_passcodes }
      { command = "open-door"
        description = "Unlocks a door controlled by a controller"
        f = open_door }
      { command = "get-status"
        description = "Retrieves the current status of the controller"
        f = get_status } ]
