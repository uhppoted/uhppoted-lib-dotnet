module Commands

open System
open System.Net
open uhppoted
open argparse

type command =
    { command: string
      description: string
      f: string list -> Result<unit, string> }

let CONTROLLER = 1u
let ENDPOINT = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000))
let PROTOCOL = Some("udp")
let TIMEOUT = 1000

let ADDRESS = IPAddress.Parse "192.168.1.10"
let NETMASK = IPAddress.Parse "255.255.255.0"
let GATEWAY = IPAddress.Parse "192.168.1.1"
let LISTENER = IPEndPoint.Parse("192.168.1.250:60001")
let INTERVAL = 0uy

let DOOR = 1uy
let MODE = DoorMode.Controlled
let DELAY = 5uy
let CARD = 1u
let CARD_INDEX = 1u
let EVENT_INDEX = 1u
let ENABLE = true
let TIME_PROFILE_ID = 2uy
let TASK_ID = 0uy

let OPTIONS: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      endpoint = None
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

let HHmm (time: Nullable<TimeOnly>) =
    if time.HasValue then
        time.Value.ToString("HH:mm")
    else
        "---"

let find_controllers args =
    let timeout = TIMEOUT
    let options = OPTIONS

    match Uhppoted.FindControllers(timeout, options) with
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
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetController(controller, timeout, options) with
    | Ok record ->
        printfn "get-controller"
        printfn "  controller %u" record.controller
        printfn "    address  %A" record.address
        printfn "    netmask  %A" record.netmask
        printfn "    gateway  %A" record.gateway
        printfn "    MAC      %A" record.MAC
        printfn "    version  %s" record.version
        printfn "    date     %s" (YYYYMMDD record.date)
        printfn ""
        Ok()
    | Error err -> Error err

let set_IPv4 args =
    let controller = argparse args "--controller" CONTROLLER
    let address = argparse args "--address" ADDRESS
    let netmask = argparse args "--netmask" NETMASK
    let gateway = argparse args "--gateway" GATEWAY
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.SetIPv4(controller, address, netmask, gateway, timeout, options) with
    | Ok response ->
        printfn "set-IPv4"
        printfn "  ok"
        printfn ""
        Ok()
    | Error err -> Error err

let get_listener args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetListener(controller, TIMEOUT, OPTIONS) with
    | Ok record ->
        printfn "get-listener"
        printfn "  controller %u" controller
        printfn "    endpoint %A" record.endpoint
        printfn "    interval %ds" record.interval
        printfn ""
        Ok()
    | Error err -> Error err

let set_listener args =
    let controller = argparse args "--controller" CONTROLLER
    let listener = argparse args "--listener" LISTENER
    let interval = argparse args "--interval" INTERVAL

    match Uhppoted.SetListener(controller, listener, interval, TIMEOUT, OPTIONS) with
    | Ok ok ->
        printfn "set-listener"
        printfn "  controller %u" controller
        printfn "          ok %A" ok
        printfn ""
        Ok()
    | Error err -> Error err

let get_time args =
    let controller = argparse args "--controller" CONTROLLER

    match Uhppoted.GetTime(controller, TIMEOUT, OPTIONS) with
    | Ok datetime ->
        printfn "get-time"
        printfn "  controller %u" controller
        printfn "    datetime %s" (YYYYMMDDHHmmss datetime)
        printfn ""
        Ok()
    | Error err -> Error err

let set_time args =
    let controller = argparse args "--controller" CONTROLLER
    let now = argparse args "--datetime" DateTime.Now

    match Uhppoted.SetTime(controller, now, TIMEOUT, OPTIONS) with
    | Ok datetime ->
        printfn "set-time"
        printfn "  controller %u" controller
        printfn "    datetime %s" (YYYYMMDDHHmmss datetime)
        printfn ""
        Ok()
    | Error err -> Error err

let get_door args =
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR

    match Uhppoted.GetDoor(controller, door, TIMEOUT, OPTIONS) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-door"
        printfn "  controller %u" controller
        printfn "        door %d" door
        printfn "        mode %A" record.mode
        printfn "       delay %ds" record.delay
        printfn ""
        printfn ""
        Ok()
    | Ok _ -> Error "door does not exist"
    | Error err -> Error err

let set_door args =
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR
    let mode = argparse args "--mode" MODE
    let delay = argparse args "--delay" DELAY

    match Uhppoted.SetDoor(controller, door, mode, delay, TIMEOUT, OPTIONS) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "set-door"
        printfn "  controller %u" controller
        printfn "        door %d" door
        printfn "        mode %A" record.mode
        printfn "       delay %ds" record.delay
        printfn ""
        Ok()
    | Ok _ -> Error "door not updated"
    | Error err -> Error err

let set_door_passcodes args =
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR
    let passcodes: uint32 array = argparse args "--passcodes" [||]

    match
        Uhppoted.SetDoorPasscodes(
            controller,
            door,
            passcodes |> Array.tryItem 0 |> Option.defaultValue 0u,
            passcodes |> Array.tryItem 1 |> Option.defaultValue 0u,
            passcodes |> Array.tryItem 2 |> Option.defaultValue 0u,
            passcodes |> Array.tryItem 3 |> Option.defaultValue 0u,
            TIMEOUT,
            OPTIONS
        )
    with
    | Ok ok ->
        printfn "set-door-passcodes"
        printfn "  controller %u" controller
        printfn "        door %u" door
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error err

let open_door args =
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR

    match Uhppoted.OpenDoor(controller, door, TIMEOUT, OPTIONS) with
    | Ok ok ->
        printfn "open-door"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error err

let get_status args =
    let controller = argparse args "--controller" CONTROLLER

    match Uhppoted.GetStatus(controller, TIMEOUT, OPTIONS) with
    | Ok record ->
        printfn "get-status"
        printfn "         controller %u" controller
        printfn "        door 1 open %b" record.Door1Open
        printfn "        door 2 open %b" record.Door2Open
        printfn "        door 3 open %b" record.Door3Open
        printfn "        door 4 open %b" record.Door3Open
        printfn "   button 1 pressed %b" record.Button1Pressed
        printfn "   button 2 pressed %b" record.Button2Pressed
        printfn "   button 3 pressed %b" record.Button3Pressed
        printfn "   button 4 pressed %b" record.Button4Pressed
        printfn "       system error %u" record.SystemError
        printfn "   system date/time %A" (YYYYMMDDHHmmss(record.SystemDateTime))
        printfn "       sequence no. %u" record.SequenceNumber
        printfn "       special info %u" record.SpecialInfo
        printfn "             relays %02x" record.Relays
        printfn "             inputs %02x" record.Inputs
        printfn ""
        printfn "    event index     %u" record.EventIndex
        printfn "          event     %u" record.EventType
        printfn "          granted   %b" record.EventAccessGranted
        printfn "          door      %u" record.EventDoor
        printfn "          direction %u" record.EventDirection
        printfn "          card      %u" record.EventCard
        printfn "          timestamp %A" (YYYYMMDDHHmmss(record.EventTimestamp))
        printfn "          reason    %u" record.EventReason
        printfn ""
        Ok()
    | Error err -> Error err

let get_cards args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetCards(controller, TIMEOUT, OPTIONS) with
    | Ok cards ->
        printfn "get-cards"
        printfn "  controller %u" controller
        printfn "       cards %u" cards
        printfn ""
        Ok()
    | Error err -> Error(err)

let get_card args =
    let controller = argparse args "--controller" CONTROLLER
    let card = argparse args "--card" CARD
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetCard(controller, card, TIMEOUT, OPTIONS) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-card"
        printfn "  controller %u" controller
        printfn "        card %u" record.card
        printfn "  start date %s" (YYYYMMDD(record.start_date))
        printfn "    end date %s" (YYYYMMDD(record.end_date))
        printfn "      door 1 %u" record.door1
        printfn "      door 2 %u" record.door2
        printfn "      door 3 %u" record.door3
        printfn "      door 4 %u" record.door4
        printfn "         PIN %u" record.PIN
        printfn ""
        Ok()
    | Ok _ -> Error "card not found"
    | Error err -> Error(err)

let get_card_at_index args =
    let controller = argparse args "--controller" CONTROLLER
    let index = CARD_INDEX
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetCardAtIndex(controller, index, timeout, options) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-card-at-index"
        printfn "  controller %u" controller
        printfn "        card %u" record.card
        printfn "  start date %s" (YYYYMMDD(record.start_date))
        printfn "    end date %s" (YYYYMMDD(record.end_date))
        printfn "      door 1 %u" record.door1
        printfn "      door 2 %u" record.door2
        printfn "      door 3 %u" record.door3
        printfn "      door 4 %u" record.door4
        printfn "         PIN %u" record.PIN
        printfn ""
        Ok()
    | Ok _ -> Error "card not found"
    | Error err -> Error(err)

let put_card args =
    let controller = argparse args "--controller" CONTROLLER
    let card = argparse args "--card" CARD
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
            endpoint = ENDPOINT
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

let delete_card args =
    let controller = argparse args "--controller" CONTROLLER
    let card = argparse args "--card" CARD
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.DeleteCard(controller, card, timeout, options) with
    | Ok ok ->
        printfn "delete-card"
        printfn "  controller %u" controller
        printfn "        card %u" card
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let delete_all_cards args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.DeleteAllCards(controller, timeout, options) with
    | Ok ok ->
        printfn "delete-all-cards"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let get_event args =
    let controller = argparse args "--controller" CONTROLLER
    let index = argparse args "--index" EVENT_INDEX
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetEvent(controller, index, timeout, options) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-event"
        printfn "  controller %u" controller
        printfn "   timestamp %s" (YYYYMMDDHHmmss(record.timestamp))
        printfn "       index %u" record.index
        printfn "       event %u" record.event_type
        printfn "     granted %b" record.access_granted
        printfn "        door %u" record.door
        printfn "   direction %u" record.direction
        printfn "        card %u" record.card
        printfn "      reason %u" record.reason
        printfn ""
        Ok()
    | Ok _ -> Error "event not found"
    | Error err -> Error(err)

let get_event_index args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetEventIndex(controller, timeout, options) with
    | Ok index ->
        printfn "get-event-index"
        printfn "  controller %u" controller
        printfn "       index %u" index
        printfn ""
        Ok()
    | Error err -> Error(err)

let set_event_index args =
    let controller = argparse args "--controller" CONTROLLER
    let index = argparse args "--index" EVENT_INDEX
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.SetEventIndex(controller, index, timeout, options) with
    | Ok ok ->
        printfn "set-event-index"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let record_special_events args =
    let controller = argparse args "--controller" CONTROLLER
    let enable = argparse args "--enable" ENABLE
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.RecordSpecialEvents(controller, enable, timeout, options) with
    | Ok ok ->
        printfn "record-special-events"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let get_time_profile args =
    let controller = argparse args "--controller" CONTROLLER
    let profile = argparse args "--profile" TIME_PROFILE_ID
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.GetTimeProfile(controller, profile, timeout, options) with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-time-profile"
        printfn "          controller %u" controller
        printfn "             profile %u" record.profile
        printfn "          start date %s" (YYYYMMDD record.start_date)
        printfn "            end date %s" (YYYYMMDD record.end_date)
        printfn "              monday %b" record.monday
        printfn "             tuesday %b" record.tuesday
        printfn "           wednesday %b" record.wednesday
        printfn "            thursday %b" record.thursday
        printfn "              friday %b" record.friday
        printfn "            saturday %b" record.saturday
        printfn "              sunday %b" record.sunday
        printfn "   segment 1 - start %s" (HHmm record.segment1_start)
        printfn "                 end %s" (HHmm record.segment1_end)
        printfn "   segment 2 - start %s" (HHmm record.segment2_start)
        printfn "                 end %s" (HHmm record.segment2_end)
        printfn "   segment 3 - start %s" (HHmm record.segment3_start)
        printfn "                 end %s" (HHmm record.segment3_end)
        printfn "      linked profile %u" record.linked_profile
        printfn ""
        Ok()
    | Ok _ -> Error "time profile does not exist"
    | Error err -> Error(err)

let set_time_profile args =
    let controller = argparse args "--controller" CONTROLLER

    let profile =
        { profile = argparse args "--profile" TIME_PROFILE_ID
          start_date = argparse args "--start-date" (Nullable(DateOnly(2024, 1, 1)))
          end_date = argparse args "--end-date" (Nullable(DateOnly(2024, 12, 31)))
          monday = true
          tuesday = true
          wednesday = false
          thursday = true
          friday = false
          saturday = true
          sunday = true
          segment1_start = Nullable(TimeOnly(8, 30))
          segment1_end = Nullable(TimeOnly(9, 45))
          segment2_start = Nullable(TimeOnly(12, 15))
          segment2_end = Nullable(TimeOnly(13, 15))
          segment3_start = Nullable(TimeOnly(14, 0))
          segment3_end = Nullable(TimeOnly(18, 0))
          linked_profile = argparse args "--linked" 0uy }

    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.SetTimeProfile(controller, profile, timeout, options) with
    | Ok ok ->
        printfn "set-time-profile"
        printfn "  controller %u" controller
        printfn "     profile %u" profile.profile
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let clear_time_profiles args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.ClearTimeProfiles(controller, timeout, options) with
    | Ok ok ->
        printfn "clear-time-profiles"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let add_task args =
    let controller = argparse args "--controller" CONTROLLER

    let task =
        { task = argparse args "--task" TASK_ID
          door = argparse args "--door" DOOR
          start_date = argparse args "--start-date" (Nullable(DateOnly(2024, 1, 1)))
          end_date = argparse args "--end-date" (Nullable(DateOnly(2024, 12, 31)))
          start_time = Nullable(TimeOnly(8, 30))
          monday = true
          tuesday = true
          wednesday = false
          thursday = true
          friday = false
          saturday = true
          sunday = true
          more_cards = argparse args "--more-cards" 0uy }

    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.AddTask(controller, task, timeout, options) with
    | Ok ok ->
        printfn "add-task"
        printfn "  controller %u" controller
        printfn "        task %u" task.task
        printfn "        door %u" task.door
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let clearTaskList args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.ClearTaskList(controller, timeout, options) with
    | Ok ok ->
        printfn "clear-tasklist"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let refreshTaskList args =
    let controller = argparse args "--controller" CONTROLLER
    let timeout = TIMEOUT

    let options =
        { OPTIONS with
            endpoint = ENDPOINT
            protocol = PROTOCOL }

    match Uhppoted.RefreshTaskList(controller, timeout, options) with
    | Ok ok ->
        printfn "refresh-tasklist"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(err)

let commands =
    [ { command = "find-controllers"
        description = "Retrieves a list of controllers accessible on the local LAN"
        f = find_controllers }

      { command = "get-controller"
        description = "Retrieves the controller information from a controller"
        f = get_controller }

      { command = "set-IPv4"
        description = "Sets a controller IPv4 address, netmask and gateway"
        f = set_IPv4 }

      { command = "get-listener"
        description = "Retrieves a controller event listener address:port and auto-send interval"
        f = get_listener }

      { command = "set-listener"
        description = "Sets a controller event listener address:port and auto-send interval"
        f = set_listener }

      { command = "get-time"
        description = "Retrieves a controller system date and time"
        f = get_time }

      { command = "set-time"
        description = "Sets a controller system date and time"
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
        description = "Retrieves the current status of a controller"
        f = get_status }

      { command = "get-cards"
        description = "Retrieves the number of cards stored on a controller"
        f = get_cards }

      { command = "get-card"
        description = "Retrieves a card record from a controller"
        f = get_card }

      { command = "get-card-at-index"
        description = "Retrieves the card record stored at the index from a controller"
        f = get_card_at_index }

      { command = "put-card"
        description = "Adds or updates a card record on a controller"
        f = put_card }

      { command = "delete-card"
        description = "Deletes a card record from a controller"
        f = delete_card }

      { command = "delete-all-cards"
        description = "Deletes all card records from a controller"
        f = delete_all_cards }

      { command = "get-event"
        description = "Retrieves the event record stored at the index from a controller"
        f = get_event }

      { command = "get-event-index"
        description = "Retrieves the current event index from a controller"
        f = get_event_index }

      { command = "set-event-index"
        description = "Sets a controller event index"
        f = set_event_index }

      { command = "record-special-events"
        description = "Enables events for door open/close, button press, etc"
        f = record_special_events }

      { command = "get-time-profile"
        description = "Retrieves an access time profile from a controller"
        f = get_time_profile }

      { command = "set-time-profile"
        description = "Adds or updates an access time profile on a controller"
        f = set_time_profile }

      { command = "clear-time-profiles"
        description = "Clears all access time profiles stored on a controller"
        f = clear_time_profiles }

      { command = "add-task"
        description = "Adds or updates a scheduled task stored on a controller"
        f = add_task }

      { command = "clear-tasklist"
        description = "Clears all scheduled tasks from the controller task list"
        f = clearTaskList }

      { command = "refresh-tasklist"
        description = "Schedules added tasks"
        f = refreshTaskList } ]
