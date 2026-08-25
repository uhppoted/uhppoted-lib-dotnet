module Commands

open System
open System.Net
open System.Threading

open uhppoted
open argparse

type command =
    { command: string
      description: string
      f: Options * string list -> Result<unit, string> }

let CONTROLLER = 1u
let ADDRESS = IPAddress.Parse "192.168.1.125"
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
let TASK_CODE = TaskCode.Unknown
let START_DATE = DateOnly(2024, 1, 1)
let END_DATE = DateOnly(2024, 12, 31)

let resolve (controller, dest: Destination) =
    match dest with
    | _ when isNull (box dest) -> None
    | dest when isNull (dest.endpoint) -> None

    | dest when dest.endpoint.Port = 0 ->
        Some
            { controller = controller
              endpoint = Some(IPEndPoint(dest.endpoint.Address, 60000))
              protocol = Some(dest.transport) }

    | dest ->
        Some
            { controller = controller
              endpoint = Some(dest.endpoint)
              protocol = Some(dest.transport) }


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

let translate v : string = Uhppoted.Translate v

let findControllers (options, args) =
    match Uhppoted.FindControllers(options) with
    | Ok controllers ->
        printfn "find-controllers: %d" controllers.Length

        controllers
        |> Array.iter (fun v ->
            printfn "  controller %u" v.Controller
            printfn "    address  %A" v.Address
            printfn "    netmask  %A" v.Netmask
            printfn "    gateway  %A" v.Gateway
            printfn "    MAC      %A" v.MAC
            printfn "    version  %s" v.Version
            printfn "    date     %s" (YYYYMMDD v.Date)
            printfn "")

        Ok()
    | Error err -> Error(translate err)

let getController (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some(c) -> Uhppoted.GetController(c, options)
        | None -> Uhppoted.GetController(controller, options)

    match result with
    | Ok record ->
        printfn "get-controller"
        printfn "  controller %u" record.Controller
        printfn "    address  %A" record.Address
        printfn "    netmask  %A" record.Netmask
        printfn "    gateway  %A" record.Gateway
        printfn "    MAC      %A" record.MAC
        printfn "    version  %s" record.Version
        printfn "    date     %s" (YYYYMMDD record.Date)
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setIPv4 (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let address = argparse args "--address" ADDRESS
    let netmask = argparse args "--netmask" NETMASK
    let gateway = argparse args "--gateway" GATEWAY

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetIPv4(c, address, netmask, gateway, options)
        | None -> Uhppoted.SetIPv4(controller, address, netmask, gateway, options)

    match result with
    | Ok response ->
        printfn "set-IPv4"
        printfn "  ok"
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getListener (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetListener(c, options)
        | None -> Uhppoted.GetListener(controller, options)

    match result with
    | Ok record ->
        printfn "get-listener"
        printfn "  controller %u" controller
        printfn "    endpoint %A" record.Endpoint
        printfn "    interval %ds" record.Interval
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setListener (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let listener = argparse args "--listener" LISTENER
    let interval = argparse args "--interval" INTERVAL

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetListener(c, listener, interval, options)
        | None -> Uhppoted.SetListener(controller, listener, interval, options)

    match result with
    | Ok ok ->
        printfn "set-listener"
        printfn "  controller %u" controller
        printfn "          ok %A" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getTime (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetTime(c, options)
        | None -> Uhppoted.GetTime(controller, options)

    match result with
    | Ok datetime ->
        printfn "get-time"
        printfn "  controller %u" controller
        printfn "    datetime %s" (YYYYMMDDHHmmss datetime)
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setTime (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let now = argparse args "--datetime" DateTime.Now

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetTime(c, now, options)
        | None -> Uhppoted.SetTime(controller, now, options)

    match result with
    | Ok datetime ->
        printfn "set-time"
        printfn "  controller %u" controller
        printfn "    datetime %s" (YYYYMMDDHHmmss datetime)
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getDoor (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetDoor(c, door, options)
        | None -> Uhppoted.GetDoor(controller, door, options)

    match result with
    | Ok record ->
        printfn "get-door"
        printfn "  controller %u" controller
        printfn "        door %d" door
        printfn "        mode %s" (translate record.Mode)
        printfn "       delay %ds" record.Delay
        printfn ""
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setDoor (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR
    let mode = argparse args "--mode" MODE
    let delay = argparse args "--delay" DELAY

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetDoor(c, door, mode, delay, options)
        | None -> Uhppoted.SetDoor(controller, door, mode, delay, options)

    match result with
    | Ok record ->
        printfn "set-door"
        printfn "  controller %u" controller
        printfn "        door %d" door
        printfn "        mode %s" (translate record.Mode)
        printfn "       delay %ds" record.Delay
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setDoorPasscodes (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR
    let passcodes: uint32 array = argparse args "--passcodes" [||]

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetDoorPasscodes(c, door, passcodes, options)
        | None -> Uhppoted.SetDoorPasscodes(controller, door, passcodes, options)

    match result with
    | Ok ok ->
        printfn "set-door-passcodes"
        printfn "  controller %u" controller
        printfn "        door %u" door
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let openDoor (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.OpenDoor(c, door, options)
        | None -> Uhppoted.OpenDoor(controller, door, options)

    match result with
    | Ok ok ->
        printfn "open-door"
        printfn "  controller %u" controller
        printfn "        door %u" door
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getStatus (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let door isopen relay =
        if isopen then
            $"{translate relay},open"
        else
            $"{translate relay},closed"

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetStatus(c, options)
        | None -> Uhppoted.GetStatus(controller, options)

    match result with
    | Ok(status, event) ->
        printfn "get-status"
        printfn "         controller %u" controller
        printfn "             door 1 %s" (door status.Door1Open status.Relays.Door1)
        printfn "             door 2 %s" (door status.Door2Open status.Relays.Door2)
        printfn "             door 3 %s" (door status.Door3Open status.Relays.Door3)
        printfn "             door 4 %s" (door status.Door4Open status.Relays.Door4)
        printfn "   button 1 pressed %b" status.Button1Pressed
        printfn "   button 2 pressed %b" status.Button2Pressed
        printfn "   button 3 pressed %b" status.Button3Pressed
        printfn "   button 4 pressed %b" status.Button4Pressed
        printfn "       system error %u" status.SystemError
        printfn "   system date/time %A" (YYYYMMDDHHmmss(status.SystemDateTime))
        printfn "       special info %u" status.SpecialInfo
        printfn "        lock forced %s" (translate status.Inputs.LockForced)
        printfn "         fire alarm %s" (translate status.Inputs.FireAlarm)
        printfn ""

        if event.HasValue then
            printfn "    event index     %u" event.Value.Index
            printfn "          event     %s (%u)" event.Value.Event.Text event.Value.Event.Code
            printfn "          granted   %b" event.Value.AccessGranted
            printfn "          door      %u" event.Value.Door
            printfn "          direction %s" (translate event.Value.Direction)
            printfn "          card      %u" event.Value.Card
            printfn "          timestamp %s" (YYYYMMDDHHmmss(event.Value.Timestamp))
            printfn "          reason    %s (%u)" event.Value.Reason.Text event.Value.Reason.Code
            printfn ""
        else
            printfn "    (no event)"
            printfn ""

        Ok()
    | Error err -> Error(translate err)

let getCards (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetCards(c, options)
        | None -> Uhppoted.GetCards(controller, options)

    match result with
    | Ok cards ->
        printfn "get-cards"
        printfn "  controller %u" controller
        printfn "       cards %u" cards
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getCard (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let card = argparse args "--card" CARD

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetCard(c, card, options)
        | None -> Uhppoted.GetCard(controller, card, options)

    match result with
    | Ok record ->
        printfn "get-card"
        printfn "    controller %u" controller
        printfn "          card %u" record.Card
        printfn "    start date %s" (YYYYMMDD(record.StartDate))
        printfn "      end date %s" (YYYYMMDD(record.EndDate))
        printfn "       doors 1 %u" record.Door1
        printfn "             2 %u" record.Door2
        printfn "             3 %u" record.Door3
        printfn "             4 %u" record.Door4
        printfn "           PIN %u" record.PIN
        printfn "  first-card 1 %b" record.FirstCard1
        printfn "             2 %b" record.FirstCard2
        printfn "             3 %b" record.FirstCard3
        printfn "             4 %b" record.FirstCard4
        printfn ""
        Ok()
    | Error CardNotFound -> Error "card not found"
    | Error err -> Error(translate err)

let getCardAtIndex (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let index = argparse args "--index" CARD_INDEX

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetCardAtIndex(c, index, options)
        | None -> Uhppoted.GetCardAtIndex(controller, index, options)

    match result with
    | Ok v when v.HasValue ->
        let record = v.Value

        printfn "get-card-at-index"
        printfn "    controller %u" controller
        printfn "          card %u" record.Card
        printfn "    start date %s" (YYYYMMDD(record.StartDate))
        printfn "      end date %s" (YYYYMMDD(record.EndDate))
        printfn "       doors 1 %u" record.Door1
        printfn "             2 %u" record.Door2
        printfn "             3 %u" record.Door3
        printfn "             4 %u" record.Door4
        printfn "           PIN %u" record.PIN
        printfn "  first-card 1 %b" record.FirstCard1
        printfn "             2 %b" record.FirstCard2
        printfn "             3 %b" record.FirstCard3
        printfn "             4 %b" record.FirstCard4
        printfn ""
        Ok()
    | Ok _ -> Error "card not found"
    | Error err -> Error(translate err)

let putCard (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let card: Card = argparse args "--card" Unchecked.defaultof<_>

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.PutCard(c, card, options)
        | None -> Uhppoted.PutCard(controller, card, options)

    match result with
    | Ok ok ->
        printfn "put-card"
        printfn "    controller %u" controller
        printfn "          card %u" card.Card
        printfn "    start date %s" (YYYYMMDD(card.StartDate))
        printfn "      end date %s" (YYYYMMDD(card.EndDate))
        printfn "       doors 1 %u" card.Door1
        printfn "             2 %u" card.Door2
        printfn "             3 %u" card.Door3
        printfn "             4 %u" card.Door4
        printfn "           PIN %u" card.PIN
        printfn "  first-card 1 %b" card.FirstCard1
        printfn "             2 %b" card.FirstCard2
        printfn "             3 %b" card.FirstCard3
        printfn "             4 %b" card.FirstCard4
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let deleteCard (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let card = argparse args "--card" CARD

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.DeleteCard(c, card, options)
        | None -> Uhppoted.DeleteCard(controller, card, options)

    match result with
    | Ok ok ->
        printfn "delete-card"
        printfn "  controller %u" controller
        printfn "        card %u" card
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let deleteAllCards (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.DeleteAllCards(c, options)
        | None -> Uhppoted.DeleteAllCards(controller, options)

    match result with
    | Ok ok ->
        printfn "delete-all-cards"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getEvent (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let index = argparse args "--index" EVENT_INDEX

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetEvent(c, index, options)
        | None -> Uhppoted.GetEvent(controller, index, options)

    match result with
    | Ok record ->
        printfn "get-event"
        printfn "  controller %u" controller
        printfn "   timestamp %s" (YYYYMMDDHHmmss(record.Timestamp))
        printfn "       index %u" record.Index
        printfn "       event %s (%u)" record.Event.Text record.Event.Code
        printfn "     granted %b" record.AccessGranted
        printfn "        door %u" record.Door
        printfn "   direction %s" (translate record.Direction)
        printfn "        card %u" record.Card
        printfn "      reason %s (%u)" record.Reason.Text record.Reason.Code
        printfn ""
        Ok()
    | Error EventNotFound -> Error "event not found"
    | Error EventOverwritten -> Error "event overwritten"
    | Error err -> Error(translate err)

let getEventIndex (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetEventIndex(c, options)
        | None -> Uhppoted.GetEventIndex(controller, options)

    match result with
    | Ok index ->
        printfn "get-event-index"
        printfn "  controller %u" controller
        printfn "       index %u" index
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setEventIndex (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let index = argparse args "--index" EVENT_INDEX

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetEventIndex(c, index, options)
        | None -> Uhppoted.SetEventIndex(controller, index, options)

    match result with
    | Ok ok ->
        printfn "set-event-index"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let recordSpecialEvents (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let enable = argparse args "--enable" ENABLE

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.RecordSpecialEvents(c, enable, options)
        | None -> Uhppoted.RecordSpecialEvents(controller, enable, options)

    match result with
    | Ok ok ->
        printfn "record-special-events"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getTimeProfile (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let profile = argparse args "--profile" TIME_PROFILE_ID

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetTimeProfile(c, profile, options)
        | None -> Uhppoted.GetTimeProfile(controller, profile, options)

    match result with
    | Ok record ->
        printfn "get-time-profile"
        printfn "          controller %u" controller
        printfn "             profile %u" record.Profile
        printfn "          start date %s" (YYYYMMDD record.StartDate)
        printfn "            end date %s" (YYYYMMDD record.EndDate)
        printfn "              monday %b" record.Monday
        printfn "             tuesday %b" record.Tuesday
        printfn "           wednesday %b" record.Wednesday
        printfn "            thursday %b" record.Thursday
        printfn "              friday %b" record.Friday
        printfn "            saturday %b" record.Saturday
        printfn "              sunday %b" record.Sunday
        printfn "   segment 1 - start %s" (HHmm record.Segment1Start)
        printfn "                 end %s" (HHmm record.Segment1End)
        printfn "   segment 2 - start %s" (HHmm record.Segment2Start)
        printfn "                 end %s" (HHmm record.Segment2End)
        printfn "   segment 3 - start %s" (HHmm record.Segment3Start)
        printfn "                 end %s" (HHmm record.Segment3End)
        printfn "      linked profile %u" record.LinkedProfile
        printfn ""
        Ok()
    | Error TimeProfileNotFound -> Error "time profile does not exist"
    | Error err -> Error(translate err)

let setTimeProfile (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let profileId = argparse args "--profile" TIME_PROFILE_ID
    let startDate = argparse args "--start-date" START_DATE
    let endDate = argparse args "--end-date" END_DATE
    let weekdays: string list = argparse args "--weekdays" []
    let segments: TimeOnly array = argparse args "--segments" [||]

    let profile =
        { Profile = profileId
          StartDate = Nullable(startDate)
          EndDate = Nullable(endDate)
          Monday = List.exists (fun v -> v = "monday") weekdays
          Tuesday = List.exists (fun v -> v = "tuesday") weekdays
          Wednesday = List.exists (fun v -> v = "wednesday") weekdays
          Thursday = List.exists (fun v -> v = "thursday") weekdays
          Friday = List.exists (fun v -> v = "friday") weekdays
          Saturday = List.exists (fun v -> v = "saturday") weekdays
          Sunday = List.exists (fun v -> v = "sunday") weekdays
          Segment1Start = segments |> Array.tryItem 0 |> Option.toNullable
          Segment1End = segments |> Array.tryItem 1 |> Option.toNullable
          Segment2Start = segments |> Array.tryItem 2 |> Option.toNullable
          Segment2End = segments |> Array.tryItem 3 |> Option.toNullable
          Segment3Start = segments |> Array.tryItem 4 |> Option.toNullable
          Segment3End = segments |> Array.tryItem 5 |> Option.toNullable
          LinkedProfile = argparse args "--linked" 0uy }

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetTimeProfile(c, profile, options)
        | None -> Uhppoted.SetTimeProfile(controller, profile, options)

    match result with
    | Ok ok ->
        printfn "set-time-profile"
        printfn "  controller %u" controller
        printfn "     profile %u" profile.Profile
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let clearTimeProfiles (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.ClearTimeProfiles(c, options)
        | None -> Uhppoted.ClearTimeProfiles(controller, options)

    match result with
    | Ok ok ->
        printfn "clear-time-profiles"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let addTask (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let weekdays: string list = argparse args "--weekdays" []

    let task =
        { Task = argparse args "--task" TASK_CODE
          Door = argparse args "--door" DOOR
          StartDate = Nullable(argparse args "--start-date" START_DATE)
          EndDate = Nullable(argparse args "--end-date" END_DATE)
          StartTime = Nullable(TimeOnly(8, 30))
          Monday = List.exists (fun v -> v = "monday") weekdays
          Tuesday = List.exists (fun v -> v = "tuesday") weekdays
          Wednesday = List.exists (fun v -> v = "wednesday") weekdays
          Thursday = List.exists (fun v -> v = "thursday") weekdays
          Friday = List.exists (fun v -> v = "friday") weekdays
          Saturday = List.exists (fun v -> v = "saturday") weekdays
          Sunday = List.exists (fun v -> v = "sunday") weekdays
          MoreCards = argparse args "--more-cards" 0uy }

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.AddTask(c, task, options)
        | None -> Uhppoted.AddTask(controller, task, options)

    match result with
    | Ok ok ->
        printfn "add-task"
        printfn "  controller %u" controller
        printfn "        task %s" (translate task.Task)
        printfn "        door %u" task.Door
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let clearTaskList (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.ClearTaskList(c, options)
        | None -> Uhppoted.ClearTaskList(controller, options)

    match result with
    | Ok ok ->
        printfn "clear-tasklist"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let refreshTaskList (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.RefreshTaskList(c, options)
        | None -> Uhppoted.RefreshTaskList(controller, options)

    match result with
    | Ok ok ->
        printfn "refresh-tasklist"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setPCControl (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let enable = argparse args "--enable" false

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetPCControl(c, enable, options)
        | None -> Uhppoted.SetPCControl(controller, enable, options)

    match result with
    | Ok ok ->
        printfn "set-pc-control"
        printfn "  controller %u" controller
        printfn "      enable %b" enable
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setInterlock (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let interlock = argparse args "--interlock" Interlock.None

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetInterlock(c, interlock, options)
        | None -> Uhppoted.SetInterlock(controller, interlock, options)

    match result with
    | Ok ok ->
        printfn "set-interlock"
        printfn "  controller %u" controller
        printfn "   interlock %s" (translate interlock)
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let activateKeypads (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let keypads = argparse args "--keypads" ([]: uint8 list)

    let reader1 = keypads |> List.contains 1uy
    let reader2 = keypads |> List.contains 2uy
    let reader3 = keypads |> List.contains 3uy
    let reader4 = keypads |> List.contains 4uy

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.ActivateKeypads(c, reader1, reader2, reader3, reader4, options)
        | None -> Uhppoted.ActivateKeypads(controller, reader1, reader2, reader3, reader4, options)

    match result with
    | Ok ok ->
        printfn "activate-keypads"
        printfn "  controller %u" controller
        printfn "    reader 1 %b" reader1
        printfn "    reader 2 %b" reader2
        printfn "    reader 3 %b" reader3
        printfn "    reader 4 %b" reader4
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let getAntiPassback (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.GetAntiPassback(c, options)
        | None -> Uhppoted.GetAntiPassback(controller, options)

    match result with
    | Ok antipassback ->
        printfn "get-antipassback"
        printfn "  controller      %u" controller
        printfn "    anti-passback %s" (translate antipassback)
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setAntiPassback (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let antipassback = argparse args "--antipassback" AntiPassback.Disabled

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetAntiPassback(c, antipassback, options)
        | None -> Uhppoted.SetAntiPassback(controller, antipassback, options)

    match result with
    | Ok ok ->
        printfn "set-antipassback"
        printfn "  controller    %u" controller
        printfn "   antipassback %s" (translate antipassback)
        printfn "             ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let setFirstCard (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER
    let door = argparse args "--door" DOOR
    let firstcard: FirstCard = argparse args "--firstcard" Unchecked.defaultof<_>

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.SetFirstCard(c, door, firstcard, options)
        | None -> Uhppoted.SetFirstCard(controller, door, firstcard, options)

    match result with
    | Ok ok ->
        printfn "set-firstcard"
        printfn "  controller %u" controller
        printfn "        door %u" door
        printfn "   firstcard %A" firstcard
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let restoreDefaultParameters (options, args) =
    let dest: Destination = argparse args "--dest" Unchecked.defaultof<_>
    let controller = argparse args "--controller" CONTROLLER

    let result =
        match resolve (controller, dest) with
        | Some c -> Uhppoted.RestoreDefaultParameters(c, options)
        | None -> Uhppoted.RestoreDefaultParameters(controller, options)

    match result with
    | Ok ok ->
        printfn "restore-default-parameters"
        printfn "  controller %u" controller
        printfn "          ok %b" ok
        printfn ""
        Ok()
    | Error err -> Error(translate err)

let listen (options, args) =
    let cancel = new CancellationTokenSource()

    Console.CancelKeyPress.Add(fun args ->
        args.Cancel <- true
        cancel.Cancel())

    let eventHandler e =
        let controller = e.Controller
        let status = e.Status
        let event = e.Event

        let door isopen relay =
            if isopen then
                $"{translate relay},open"
            else
                $"{translate relay},closed"

        printfn "-- EVENT"
        printfn "         controller %u" controller
        printfn ""
        printfn "             door 1 %s" (door status.Door1Open status.Relays.Door1)
        printfn "             door 2 %s" (door status.Door2Open status.Relays.Door2)
        printfn "             door 3 %s" (door status.Door3Open status.Relays.Door3)
        printfn "             door 4 %s" (door status.Door4Open status.Relays.Door4)
        printfn "   button 1 pressed %b" status.Button1Pressed
        printfn "   button 2 pressed %b" status.Button2Pressed
        printfn "   button 3 pressed %b" status.Button3Pressed
        printfn "   button 4 pressed %b" status.Button4Pressed
        printfn "       system error %u" status.SystemError
        printfn "   system date/time %s" (YYYYMMDDHHmmss(status.SystemDateTime))
        printfn "       special info %u" status.SpecialInfo
        printfn "        lock forced %s" (translate status.Inputs.LockForced)
        printfn "         fire alarm %s" (translate status.Inputs.FireAlarm)
        printfn ""

        if event.HasValue then
            printfn "    event timestamp %s" (YYYYMMDDHHmmss(event.Value.Timestamp))
            printfn "              index %u" event.Value.Index
            printfn "              event %s (%u)" event.Value.Event.Text event.Value.Event.Code
            printfn "            granted %b" event.Value.AccessGranted
            printfn "               door %u" event.Value.Door
            printfn "          direction %s" (translate event.Value.Direction)
            printfn "               card %u" event.Value.Card
            printfn "             reason %s (%u)" event.Value.Reason.Text event.Value.Reason.Code
            printfn ""
        else
            printfn "   (no event)"
            printfn ""

    let errorHandler err = printfn "** ERROR %A" (translate err)

    let onevent: OnEvent = new OnEvent(eventHandler)
    let onerror: OnError = new OnError(errorHandler)

    match Uhppoted.Listen(onevent, onerror, cancel.Token, options) with
    | Ok _ -> Ok()
    | Error err -> Error(translate err)

let commands =
    [ { command = "find-controllers"
        description = "Retrieves a list of controllers accessible on the local LAN"
        f = findControllers }

      { command = "get-controller"
        description = "Retrieves the controller information from a controller"
        f = getController }

      { command = "set-IPv4"
        description = "Sets a controller IPv4 address, netmask and gateway"
        f = setIPv4 }

      { command = "get-listener"
        description = "Retrieves a controller event listener address:port and auto-send interval"
        f = getListener }

      { command = "set-listener"
        description = "Sets a controller event listener address:port and auto-send interval"
        f = setListener }

      { command = "get-time"
        description = "Retrieves a controller system date and time"
        f = getTime }

      { command = "set-time"
        description = "Sets a controller system date and time"
        f = setTime }

      { command = "get-door"
        description = "Retrieves a controller door mode and delay settings"
        f = getDoor }

      { command = "set-door"
        description = "Sets a controller door mode and delay"
        f = setDoor }

      { command = "set-door-passcodes"
        description = "Sets the supervisor passcodes for a controller door"
        f = setDoorPasscodes }

      { command = "open-door"
        description = "Unlocks a door controlled by a controller"
        f = openDoor }

      { command = "get-status"
        description = "Retrieves the current status of a controller"
        f = getStatus }

      { command = "get-cards"
        description = "Retrieves the number of cards stored on a controller"
        f = getCards }

      { command = "get-card"
        description = "Retrieves a card record from a controller"
        f = getCard }

      { command = "get-card-at-index"
        description = "Retrieves the card record stored at the index from a controller"
        f = getCardAtIndex }

      { command = "put-card"
        description = "Adds or updates a card record on a controller"
        f = putCard }

      { command = "delete-card"
        description = "Deletes a card record from a controller"
        f = deleteCard }

      { command = "delete-all-cards"
        description = "Deletes all card records from a controller"
        f = deleteAllCards }

      { command = "get-event"
        description = "Retrieves the event record stored at the index from a controller"
        f = getEvent }

      { command = "get-event-index"
        description = "Retrieves the current event index from a controller"
        f = getEventIndex }

      { command = "set-event-index"
        description = "Sets a controller event index"
        f = setEventIndex }

      { command = "record-special-events"
        description = "Enables events for door open/close, button press, etc"
        f = recordSpecialEvents }

      { command = "get-time-profile"
        description = "Retrieves an access time profile from a controller"
        f = getTimeProfile }

      { command = "set-time-profile"
        description = "Adds or updates an access time profile on a controller"
        f = setTimeProfile }

      { command = "clear-time-profiles"
        description = "Clears all access time profiles stored on a controller"
        f = clearTimeProfiles }

      { command = "add-task"
        description = "Adds or updates a scheduled task stored on a controller"
        f = addTask }

      { command = "clear-tasklist"
        description = "Clears all scheduled tasks from the controller task list"
        f = clearTaskList }

      { command = "refresh-tasklist"
        description = "Schedules added tasks"
        f = refreshTaskList }

      { command = "set-pc-control"
        description = "Enables (or disables) remote access control management"
        f = setPCControl }

      { command = "set-interlock"
        description = "Sets the door interlock mode for a controller"
        f = setInterlock }

      { command = "activate-keypads"
        description = "Activates the access reader keypads attached to a controller"
        f = activateKeypads }

      { command = "get-antipassback"
        description = "Retrieves the controller anti-passback mode"
        f = getAntiPassback }

      { command = "set-antipassback"
        description = "Sets the anti-passback mode for a controller"
        f = setAntiPassback }

      { command = "set-firstcard"
        description = "Sets the first card configuration for a door"
        f = setFirstCard }

      { command = "restore-default-parameters"
        description = "Restores the manufacturer defaults"
        f = restoreDefaultParameters }

      { command = "listen"
        description = "Listens for access controller events"
        f = listen } ]
