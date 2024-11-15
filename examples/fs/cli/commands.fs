module Commands

open System
open System.Net
open uhppoted
open argparse

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
let CARD = 10058400u
let CARD_INDEX = 1u

let OPTIONS: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      destination = None
      protocol = None
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

let find_controllers args =
    match Uhppoted.FindControllers(TIMEOUT, OPTIONS) with
    | Ok controllers ->
        printfn "find-controllers: %d" controllers.Length

        controllers
        |> Array.iter (fun v ->
            printfn "  controller %u" v.controller
            printfn "    address  %A" v.address
            printfn "    netmask  %A" v.netmask
            printfn "    gateway  %A" v.gateway
            printfn "    MAC      %A" v.MAC
            printfn "    version  %s" v.version
            printfn "    date     %s" (YYYYMMDD v.date)
            printfn "")

        Ok()
    | Error err -> Error err

let get_controller args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_controller (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-controller"
        printfn "  controller %u" response.controller
        printfn "    address  %A" response.address
        printfn "    netmask  %A" response.netmask
        printfn "    gateway  %A" response.gateway
        printfn "    MAC      %A" response.MAC
        printfn "    version  %s" response.version
        printfn "    date     %s" (YYYYMMDD response.date)
        printfn ""
        Ok()
    | Error err -> Error err

let set_IPv4 args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let address = argparse args "--address" (IPAddress.Parse "192.168.1.100")
    let netmask = argparse args "--netmask" (IPAddress.Parse "255.255.255.0")
    let gateway = argparse args "--gateway" (IPAddress.Parse "192.168.1.1")

    match Uhppoted.set_IPv4 (controller, address, netmask, gateway, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "set-IPv4"
        printfn "  ok"
        printfn ""
        Ok()
    | Error err -> Error err

let get_listener args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_listener (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-listener"
        printfn "  controller %u" response.controller
        printfn "    endpoint %A" response.endpoint
        printfn "    interval %ds" response.interval
        printfn ""
        Ok()
    | Error err -> Error err

let set_listener args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let endpoint = IPEndPoint.Parse("192.168.1.100:60001")
    let interval = 30uy

    match Uhppoted.set_listener (controller, endpoint, interval, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "set-listener"
        printfn "  controller %u" response.controller
        printfn "          ok %A" response.ok
        printfn ""
        Ok()
    | Error err -> Error err

let get_time args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_time (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-time"
        printfn "  controller %u" response.controller
        printfn "    datetime %s" (YYYYMMDDHHmmss response.datetime)
        printfn ""
        Ok()
    | Error err -> Error err

let set_time args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let datetime = DateTime.Now

    match Uhppoted.set_time (controller, datetime, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "set-time"
        printfn "  controller %u" response.controller
        printfn "    datetime %s" (YYYYMMDDHHmmss response.datetime)
        printfn ""
        Ok()
    | Error err -> Error err

let get_door args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy

    match Uhppoted.get_door (controller, door, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-door"
        printfn "  controller %u" response.controller
        printfn "        door %d" response.door
        printfn "        mode %d" response.mode
        printfn "       delay %ds" response.delay
        printfn ""
        Ok()
    | Error err -> Error err

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
        printfn "set-door"
        printfn "  controller %u" response.controller
        printfn "        door %d" response.door
        printfn "        mode %d" response.mode
        printfn "       delay %ds" response.delay
        printfn ""
        Ok()
    | Error err -> Error err

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
        printfn "set-door-passcodes"
        printfn "  controller %u" response.controller
        printfn "          ok %b" response.ok
        printfn ""
        Ok()
    | Error err -> Error err

let open_door args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let door = 4uy

    match Uhppoted.open_door (controller, door, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "open-door"
        printfn "  controller %u" response.controller
        printfn "          ok %b" response.ok
        printfn ""
        Ok()
    | Error err -> Error err

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
    | Error err -> Error err

let get_cards args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    match Uhppoted.get_cards (controller, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-cards"
        printfn "  controller %u" response.controller
        printfn "       cards %u" response.cards
        printfn ""
        Ok()
    | Error err -> Error(err)

let get_card args =
    let controller =
        { controller = argparse args "--controller" CONTROLLER
          address = ADDRESS
          protocol = PROTOCOL }

    let card = CARD

    match Uhppoted.GetCard(controller, card, TIMEOUT, OPTIONS) with
    | Ok response ->
        printfn "get-card"
        printfn "  controller %u" response.controller
        printfn "        card %u" response.card
        printfn "  start date %s" (YYYYMMDD(response.startdate))
        printfn "    end date %s" (YYYYMMDD(response.enddate))
        printfn "      door 1 %u" response.door1
        printfn "      door 2 %u" response.door2
        printfn "      door 3 %u" response.door3
        printfn "      door 4 %u" response.door4
        printfn "         PIN %u" response.PIN
        printfn ""
        Ok()
    | Error err -> Error(err)

let get_card_at_index args =
    let controller = argparse args "--controller" CONTROLLER
    let index = CARD_INDEX

    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            destination = ADDRESS
            protocol = PROTOCOL }

    match Uhppoted.GetCardAtIndex(controller, index, timeout, options) with
    | Ok v when v.HasValue ->
        let card = v.Value
        printfn "get-card-at-index"
        printfn "  controller %u" controller
        printfn "        card %u" card.card
        printfn "  start date %s" (YYYYMMDD(card.startdate))
        printfn "    end date %s" (YYYYMMDD(card.enddate))
        printfn "      door 1 %u" card.door1
        printfn "      door 2 %u" card.door2
        printfn "      door 3 %u" card.door3
        printfn "      door 4 %u" card.door4
        printfn "         PIN %u" card.PIN
        printfn ""
        Ok()
    | Ok _ -> Error "card not found"
    | Error err -> Error(err)

let put_card args =
    let controller = argparse args "--controller" CONTROLLER
    let card = CARD
    let startdate = DateOnly(2024, 1, 1)
    let enddate = DateOnly(2024, 12, 31)
    let door1 = 1uy
    let door2 = 0uy
    let door3 = 17uy
    let door4 = 1uy
    let PIN = 7531u

    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            destination = ADDRESS
            protocol = PROTOCOL }

    match Uhppoted.PutCard(controller, card, startdate, enddate, door1, door2, door3, door4, PIN, timeout, options) with
    | Ok ok ->
        printfn "put-card"
        printfn "  controller %u" controller
        printfn "        card %u" card
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let commands =
    [ { command = "find-controllers"
        description = "Retrieves a list of controllers accessible on the local LAN"
        f = find_controllers }

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
        f = get_status }

      { command = "get-cards"
        description = "Retrieves the number of cards stored on the controller"
        f = get_cards }

      { command = "get-card"
        description = "Retrieves a card record from the controller"
        f = get_card }

      { command = "get-card-at-index"
        description = "Retrieves the card record stored at the index from the controller"
        f = get_card_at_index }

      { command = "put-card"
        description = "Adds or updates a card record on controller"
        f = put_card } ]
