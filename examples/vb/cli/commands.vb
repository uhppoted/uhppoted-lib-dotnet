Imports System.Console
Imports System.Net
Imports System.Threading

Imports UHPPOTE = uhppoted.Uhppoted
Imports Options = uhppoted.Options
Imports C = uhppoted.C
Imports CBuilder = uhppoted.CBuilder
Imports DoorMode = uhppoted.DoorMode
Imports Interlock = uhppoted.Interlock
Imports AntiPassback = uhppoted.AntiPassback
Imports Card = uhppoted.Card
Imports FirstCard = uhppoted.FirstCard
Imports Err = uhppoted.Err

Public Structure Command
    Public ReadOnly command As String
    Public ReadOnly description As String
    Public ReadOnly f As Action(Of Options, String())

    Public Sub New(command As String, description As String, f As Action(Of Options, String()))
        Me.command = command
        Me.description = description
        Me.f = f
    End Sub
End Structure

Module Commands
    Private Const CONTROLLER_ID As UInt32 = 1
    Private Const EVENT_INTERVAL as Byte = 0
    Private Const DOOR As Byte = 1
    Private Const MODE As Uhppoted.DoorMode = Uhppoted.DoorMode.Controlled
    Private Const DELAY As Byte = 5
    Private Const CARD_NUMBER As UInt32 = 1
    Private Const CARD_INDEX As UInt32 = 1
    Private Const EVENT_INDEX As UInt32 = 1
    Private Const ENABLE as Boolean = True
    Private Const TIME_PROFILE_ID as Byte = 0
    Private Const TASK_CODE as Uhppoted.TaskCode = Uhppoted.TaskCode.Unknown

    Private ReadOnly Dim IPv4_ADDRESS = IPAddress.Parse("192.168.1.10")
    Private ReadOnly Dim IPv4_NETMASK = IPAddress.Parse("255.255.255.0")
    Private ReadOnly Dim IPv4_GATEWAY = IPAddress.Parse("192.168.1.1")
    Private ReadOnly Dim EVENT_LISTENER = IPEndPoint.Parse("192.168.1.250:60001")
    Private ReadOnly Dim START_DATE As DateOnly = New DateOnly(2024, 1, 1)
    Private ReadOnly Dim END_DATE As DateOnly = New DateOnly(2024, 12, 31)

    Public Dim commands As New List(Of Command) From {
           New Command("find-controllers", "Retrieves a list of controllers accessible on the local LAN", AddressOf FindControllers),
           New Command("get-controller", "Retrieves the controller information from a controller", AddressOf GetController),
           New Command("set-IPv4", "Sets a controller IPv4 address, netmask and gateway", AddressOf SetIPv4),
           New Command("get-listener", "Retrieves a controller event listener address:port and auto-send interval", AddressOf GetListener),
           New Command("set-listener", "Sets a controller event listener address:port and auto-send interval", AddressOf SetListener),
           New Command("get-time", "Retrieves a controller system date and time", AddressOf GetTime),
           New Command("set-time", "Sets a controller system date and time", AddressOf SetTime),
           New Command("get-door", "Retrieves a controller door mode and delay settings", AddressOf GetDoor),
           New Command("set-door", "Sets a controller door mode and delay", AddressOf SetDoor),
           New Command("set-door-passcodes", "Sets the supervisor passcodes for a controller door", AddressOf SetDoorPasscodes),
           New Command("open-door", "Unlocks a door controlled by a controller", AddressOf OpenDoor),
           New Command("get-status", "Retrieves the current status of a controller", AddressOf GetStatus),
           New Command("get-cards", "Retrieves the number of cards stored on a controller", AddressOf GetCards),
           New Command("get-card", "Retrieves a card record from a controller", AddressOf GetCard),
           New Command("get-card-at-index", "Retrieves the card record stored at the index from a controller", AddressOf GetCardAtIndex),
           New Command("put-card", "Adds or updates a card record on controller", AddressOf PutCard),
           New Command("delete-card", "Deletes a card record from a controller", AddressOf DeleteCard),
           New Command("delete-all-cards", "Deletes all card records from a controller", AddressOf DeleteAllCards),
           New Command("get-event", "Retrieves the event record stored at the index from a controller", AddressOf GetEvent),
           New Command("get-event-index", "Retrieves the current event index from a controller", AddressOf GetEventIndex),
           New Command("set-event-index", "Sets a controller event index", AddressOf SetEventIndex),
           New Command("record-special-events", "Enables events for door open/close, button press, etc", AddressOf RecordSpecialEvents),
           New Command("get-time-profile", "Retrieves an access time profile from a controller", AddressOf GetTimeProfile),
           New Command("set-time-profile", "Adds or updates an access time profile on a controller", AddressOf SetTimeProfile),
           New Command("clear-time-profiles", "Clears all access time profiles stored on a controller", AddressOf ClearTimeProfiles),
           New Command("add-task", "Adds or updates a scheduled task stored on a controller", AddressOf AddTask),
           New Command("clear-tasklist", "Clears all scheduled tasks from the controller task list", AddressOf ClearTaskList),
           New Command("refresh-tasklist", "Schedules added tasks", AddressOf RefreshTaskList),
           New Command("set-pc-control", "Enables (or disables) remote access control management", AddressOf SetPCControl),
           New Command("set-interlock", "Sets the door interlock mode for a controller", AddressOf SetInterlock),
           New Command("activate-keypads", "Activates the access reader keypads attached to a controller", AddressOf ActivateKeypads),
           New Command("get-antipassback", "Retrieves the controller anti-passback mode", AddressOf GetAntiPassback),
           New Command("set-antipassback", "Sets the anti-passback mode for a controller", AddressOf SetAntiPassback),
           New Command("set-firstcard", "Sets the first card configuration for a controller managed door", AddressOf SetFirstCard),
           New Command("restore-default-parameters", "Restores the manufacturer defaults", AddressOf RestoreDefaultParameters),
           New Command("listen", "Listens for access controller events", AddressOf Listen)
       }

    Sub FindControllers(options As Options, args As String())
        Dim result = UHPPOTE.FindControllers(options)

        If (result.IsOk)
            Dim controllers = result.ResultValue

            WriteLine("get-controllers: {0}", controllers.Length)
            For Each controller In controllers
                WriteLine("  controller {0}", controller.Controller)
                WriteLine("    address  {0}", controller.Address)
                WriteLine("    netmask  {0}", controller.Netmask)
                WriteLine("    gateway  {0}", controller.Gateway)
                WriteLine("    MAC      {0}", controller.MAC)
                WriteLine("    version  {0}", controller.Version)
                WriteLine("    date     {0}", YYYYMMDD(controller.Date))
                WriteLine()
            Next
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetController(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                         UHPPOTE.GetController(c, options),
                         UHPPOTE.GetController(controller, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-controller")
            WriteLine("  controller {0}", record.Controller)
            WriteLine("    address  {0}", record.Address)
            WriteLine("    netmask  {0}", record.Netmask)
            WriteLine("    gateway  {0}", record.Gateway)
            WriteLine("    MAC      {0}", record.MAC)
            WriteLine("    version  {0}", record.Version)
            WriteLine("    date     {0}", YYYYMMDD(record.Date))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetIPv4(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim address = ArgParse.Parse(args, "--address", IPv4_ADDRESS)
        Dim netmask = ArgParse.Parse(args, "--netmask", IPv4_NETMASK)
        Dim gateway = ArgParse.Parse(args, "--gateway", IPv4_GATEWAY)

        Dim result = If(resolve(controller, dest, c),
                       UHPPOTE.SetIPv4(c, address, netmask, gateway, options),
                       UHPPOTE.SetIPv4(controller, address, netmask, gateway, options))

        If (result.IsOk)
            WriteLine("set-IPv4")
            WriteLine("  ok")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetListener(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                       UHPPOTE.GetListener(c, options),
                       UHPPOTE.GetListener(controller, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-listener")
            WriteLine("  controller {0}", controller)
            WriteLine("    endpoint {0}", record.Endpoint)
            WriteLine("    interval {0}s", record.Interval)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetListener(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim listener = ArgParse.Parse(args, "--listener", EVENT_LISTENER)
        Dim interval = ArgParse.Parse(args, "--interval", EVENT_INTERVAL)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetListener(c, listener, interval, options),
                        UHPPOTE.SetListener(controller, listener, interval, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-listener")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetTime(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetTime(c, options),
                        UHPPOTE.GetTime(controller, options))

        If (result.IsOk)
            Dim datetime = result.ResultValue

            WriteLine("get-time")
            WriteLine("  controller {0}", controller)
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetTime(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim now = ArgParse.Parse(args, "--datetime", DateTime.Now)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetTime(c, now, options),
                        UHPPOTE.SetTime(controller, now, options))

        If (result.IsOk)
            Dim datetime = result.ResultValue

            WriteLine("set-time")
            WriteLine("  controller {0}", controller)
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetDoor(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetDoor(c, door, options),
                        UHPPOTE.GetDoor(controller, door, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-door")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("        mode {0}", translate(record.Mode))
            WriteLine("       delay {0}s", record.Delay)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetDoor(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim mode As DoorMode = ArgParse.Parse(args, "--mode", MODE)
        Dim delay As Byte = ArgParse.Parse(args, "--delay", DELAY)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetDoor(c, door, mode, delay, options),
                        UHPPOTE.SetDoor(controller, door, mode, delay, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("set-door")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("        mode {0}", translate(record.Mode))
            WriteLine("       delay {0}s", record.Delay)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetDoorPasscodes(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim passcodes As UInteger() = ArgParse.Parse(args, "--passcodes", new UInteger() {})

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetDoorPasscodes(c, door, passcodes, options),
                        UHPPOTE.SetDoorPasscodes(controller, door, passcodes, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-door-passcodes")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub OpenDoor(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.OpenDoor(c, door, options),
                        UHPPOTE.OpenDoor(controller, door, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("open-door")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetStatus(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetStatus(c, options),
                        UHPPOTE.GetStatus(controller, options))

        If (result.IsOk)
            Dim status = result.ResultValue.Item1
            Dim evt = result.ResultValue.Item2

            WriteLine("get-status")
            WriteLine("         controller {0}", controller)
            WriteLine("             door 1 {1},{0}", If(status.Door1Open, "open", "closed"), translate(status.Relays.Door1))
            WriteLine("             door 2 {1},{0}", If(status.Door2Open, "open", "closed"), translate(status.Relays.Door2))
            WriteLine("             door 3 {1},{0}", If(status.Door3Open, "open", "closed"), translate(status.Relays.Door3))
            WriteLine("             door 4 {1},{0}", If(status.Door4Open, "open", "closed"), translate(status.Relays.Door4))
            WriteLine("   button 1 pressed {0}", status.Button1Pressed)
            WriteLine("   button 2 pressed {0}", status.Button2Pressed)
            WriteLine("   button 3 pressed {0}", status.Button3Pressed)
            WriteLine("   button 4 pressed {0}", status.Button4Pressed)
            WriteLine("       system error {0}", status.SystemError)
            WriteLine("   system date/time {0}", YYYYMMDDHHmmss(status.SystemDateTime))
            WriteLine("       special info {0}", status.SpecialInfo)
            WriteLine("        lock forced {0}", translate(status.Inputs.LockForced))
            WriteLine("         fire alarm {0}", translate(status.Inputs.FireAlarm))
            WriteLine()

            If evt.HasValue Then
                WriteLine("    event index     {0}", evt.Value.Index)
                WriteLine("          event     {0} ({1})", evt.Value.Event.Text, evt.Value.Event.Code)
                WriteLine("          granted   {0}", evt.Value.AccessGranted)
                WriteLine("          door      {0}", evt.Value.Door)
                WriteLine("          direction {0}", translate(evt.Value.Direction))
                WriteLine("          card      {0}", evt.Value.Card)
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(evt.Value.Timestamp))
                WriteLine("          reason    {0}", translate(evt.Value.Reason))
                WriteLine()
            Else
                WriteLine("    (no event)")
                WriteLine()
            End If

        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetCards(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetCards(c, options),
                        UHPPOTE.GetCards(controller, options))

        If (result.IsOk)
            Dim cards = result.ResultValue

            WriteLine("get-cards")
            WriteLine("  controller {0}", controller)
            WriteLine("       cards {0}", cards)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetCard(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetCard(c, card, options),
                        UHPPOTE.GetCard(controller, card, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-card")
            WriteLine("    controller {0}", controller)
            WriteLine("          card {0}", record.Card)
            WriteLine("    start date {0}", (YYYYMMDD(record.StartDate)))
            WriteLine("      end date {0}", (YYYYMMDD(record.EndDate)))
            WriteLine("       doors 1 {0}", record.Door1)
            WriteLine("             2 {0}", record.Door2)
            WriteLine("             3 {0}", record.Door3)
            WriteLine("             4 {0}", record.Door4)
            WriteLine("           PIN {0}", record.PIN)
            WriteLine("  first-card 1 {0}", record.FirstCard1)
            WriteLine("             2 {0}", record.FirstCard2)
            WriteLine("             3 {0}", record.FirstCard3)
            WriteLine("             4 {0}", record.FirstCard4)
            WriteLine()
        Else If (result.IsError AndAlso result.ErrorValue is Err.CardNotFound)
            Throw New Exception("card not found")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetCardAtIndex(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim index = ArgParse.Parse(args, "--index", CARD_INDEX)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetCardAtIndex(c, index, options),
                        UHPPOTE.GetCardAtIndex(controller, index, options))

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card-at-index")
            WriteLine("    controller {0}", controller)
            WriteLine("          card {0}", record.Card)
            WriteLine("    start date {0}", (YYYYMMDD(record.StartDate)))
            WriteLine("      end date {0}", (YYYYMMDD(record.EndDate)))
            WriteLine("       doors 1 {0}", record.Door1)
            WriteLine("             2 {0}", record.Door2)
            WriteLine("             3 {0}", record.Door3)
            WriteLine("             4 {0}", record.Door4)
            WriteLine("           PIN {0}", record.PIN)
            WriteLine("  first-card 1 {0}", record.FirstCard1)
            WriteLine("             2 {0}", record.FirstCard2)
            WriteLine("             3 {0}", record.FirstCard3)
            WriteLine("             4 {0}", record.FirstCard4)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("card not found")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub PutCard(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim card = ArgParse.Parse(args, "--card", new Card())

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.PutCard(c, card, options),
                        UHPPOTE.PutCard(controller, card, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("put-card")
            WriteLine("    controller {0}", controller)
            WriteLine("          card {0}", card.Card)
            WriteLine("    start date {0}", (YYYYMMDD(card.StartDate)))
            WriteLine("      end date {0}", (YYYYMMDD(card.EndDate)))
            WriteLine("       doors 1 {0}", card.Door1)
            WriteLine("             2 {0}", card.Door2)
            WriteLine("             3 {0}", card.Door3)
            WriteLine("             4 {0}", card.Door4)
            WriteLine("           PIN {0}", card.PIN)
            WriteLine("  first-card 1 {0}", card.FirstCard1)
            WriteLine("             2 {0}", card.FirstCard2)
            WriteLine("             3 {0}", card.FirstCard3)
            WriteLine("             4 {0}", card.FirstCard4)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub DeleteCard(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.DeleteCard(c, card, options),
                        UHPPOTE.DeleteCard(controller, card, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("delete-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", card)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub DeleteAllCards(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.DeleteAllCards(c, options),
                        UHPPOTE.DeleteAllCards(controller, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("delete-all-cards")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetEvent(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetEvent(c, index, options),
                        UHPPOTE.GetEvent(controller, index, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-event")
            WriteLine("  controller {0}", controller)
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.Timestamp)))
            WriteLine("       index {0}", record.Index)
            WriteLine("       event {0} ({1})", record.Event.Text, record.Event.Code)
            WriteLine("     granted {0}", record.AccessGranted)
            WriteLine("        door {0}", record.Door)
            WriteLine("   direction {0}", translate(record.Direction))
            WriteLine("        card {0}", record.Card)
            WriteLine("      reason {0}", translate(record.Reason))
            WriteLine()
        Else If (result.IsError And result.ErrorValue is Err.EventNotFound)
            Throw New Exception("event not found")
        Else If (result.IsError And result.ErrorValue is Err.EventOverwritten)
            Throw New Exception("event overwritten")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetEventIndex(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetEventIndex(c, options),
                        UHPPOTE.GetEventIndex(controller, options))

        If (result.IsOk)
            Dim index = result.ResultValue

            WriteLine("get-event-index")
            WriteLine("  controller {0}", controller)
            WriteLine("       index {0}", index)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetEventIndex(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetEventIndex(c, index, options),
                        UHPPOTE.SetEventIndex(controller, index, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-event-index")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub RecordSpecialEvents(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim enable As Boolean = ArgParse.Parse(args, "--enable", ENABLE)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.RecordSpecialEvents(c, enable, options),
                        UHPPOTE.RecordSpecialEvents(controller, enable, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("record-special-events")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetTimeProfile(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim profile = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetTimeProfile(c, profile, options),
                        UHPPOTE.GetTimeProfile(controller, profile, options))

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-time-profile")
            WriteLine("          controller {0}", controller)
            WriteLine("             profile {0}", record.Profile)
            WriteLine("          start date {0}", YYYYMMDD(record.StartDate))
            WriteLine("            end date {0}", YYYYMMDD(record.EndDate))
            WriteLine("              monday {0}", record.Monday)
            WriteLine("             tuesday {0}", record.Tuesday)
            WriteLine("           wednesday {0}", record.Wednesday)
            WriteLine("            thursday {0}", record.Thursday)
            WriteLine("              friday {0}", record.Friday)
            WriteLine("            saturday {0}", record.Saturday)
            WriteLine("              sunday {0}", record.Sunday)
            WriteLine("   segment 1 - start {0}", HHmm(record.Segment1Start))
            WriteLine("                 end {0}", HHmm(record.Segment1End))
            WriteLine("   segment 2 - start {0}", HHmm(record.Segment2Start))
            WriteLine("                 end {0}", HHmm(record.Segment2End))
            WriteLine("   segment 3 - start {0}", HHmm(record.Segment3Start))
            WriteLine("                 end {0}", HHmm(record.Segment3End))
            WriteLine("      linked profile {0}", record.LinkedProfile)
            WriteLine()
        Else If (result.IsError And result.ErrorValue is Err.TimeProfileNotFound)
            Throw New Exception("time profile does not exist")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetTimeProfile(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim profile_id = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID)
        Dim linked As Byte = ArgParse.Parse(args, "--linked", CType(0, Byte))
        Dim start_date As DateOnly = ArgParse.Parse(args, "--start_date", START_DATE)
        Dim end_date As DateOnly = ArgParse.Parse(args, "--end_date", END_DATE)
        Dim weekdays = ArgParse.Parse(args, "--weekdays", new String() {})
        Dim segments = ArgParse.Parse(args, "--segments", New TimeOnly(6) {})

        Dim monday = weekdays.Contains("monday")
        Dim tuesday = weekdays.Contains("tuesday")
        Dim wednesday = weekdays.Contains("wednesday")
        Dim thursday = weekdays.Contains("thursday")
        Dim friday = weekdays.Contains("friday")
        Dim saturday = weekdays.Contains("saturday")
        Dim sunday = weekdays.Contains("sunday")

        Dim profile = New uhppoted.TimeProfileBuilder(profile_id).
                                   WithStartDate(start_date).
                                   WithEndDate(end_date).
                                   WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday).
                                   WithSegment1(segments(0), segments(1)).
                                   WithSegment2(segments(2), segments(3)).
                                   WithSegment3(segments(4), segments(5)).
                                   WithLinkedProfile(linked).
                                   build()

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetTimeProfile(c, profile, options),
                        UHPPOTE.SetTimeProfile(controller, profile, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-time-profile")
            WriteLine("  controller {0}", controller)
            WriteLine("     profile {0}", profile.Profile)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub ClearTimeProfiles(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.ClearTimeProfiles(c, options),
                        UHPPOTE.ClearTimeProfiles(controller, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("clear-time-profiles")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub AddTask(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim taskCode As Byte = ArgParse.Parse(args, "--task", TASK_CODE)
        Dim doorId As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim startDate As DateOnly = ArgParse.Parse(args, "--start_date", START_DATE)
        Dim endDate As DateOnly = ArgParse.Parse(args, "--end_date", END_DATE)
        Dim startTime = ArgParse.Parse(args, "--start_time", TimeOnly.Parse("00:00"))
        Dim weekdays = ArgParse.Parse(args, "--weekdays", new String() {})
        Dim moreCards As Byte = ArgParse.Parse(args, "--more-cards", CType(0, Byte))

        Dim monday = weekdays.Contains("monday")
        Dim tuesday = weekdays.Contains("tuesday")
        Dim wednesday = weekdays.Contains("wednesday")
        Dim thursday = weekdays.Contains("thursday")
        Dim friday = weekdays.Contains("friday")
        Dim saturday = weekdays.Contains("saturday")
        Dim sunday = weekdays.Contains("sunday")

        Dim task = New uhppoted.TaskBuilder(taskCode, doorId).
                                WithStartDate(startDate).
                                WithEndDate(endDate).
                                WithStartTime(startTime).
                                WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday).
                                WithMoreCards(moreCards).
                                build()

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.AddTask(c, task, options),
                        UHPPOTE.AddTask(controller, task, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("add-task")
            WriteLine("  controller {0}", controller)
            WriteLine("        task {0}", translate(task.Task))
            WriteLine("        door {0}", task.Door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub ClearTaskList(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.ClearTaskList(c, options),
                        UHPPOTE.ClearTaskList(controller, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("clear-tasklist")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub RefreshTaskList(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.RefreshTaskList(c, options),
                        UHPPOTE.RefreshTaskList(controller, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("refresh-tasklist")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetPCControl(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim enable = ArgParse.Parse(args, "--enable", False)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetPCControl(c, enable, options),
                        UHPPOTE.SetPCControl(controller, enable, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-pc-control")
            WriteLine("  controller {0}", controller)
            WriteLine("      enable {0}", enable)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetInterlock(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim interlock As Interlock = ArgParse.Parse(args, "--interlock", Interlock.None)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetInterlock(c, interlock, options),
                        UHPPOTE.SetInterlock(controller, interlock, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-interlock")
            WriteLine("  controller {0}", controller)
            WriteLine("   interlock {0}", translate(interlock))
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub ActivateKeypads(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim keypads = ArgParse.Parse(args, "--keypads", new Byte() {})

        Dim reader1 = keypads.Contains(1)
        Dim reader2 = keypads.Contains(2)
        Dim reader3 = keypads.Contains(3)
        Dim reader4 = keypads.Contains(4)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.ActivateKeypads(c, reader1, reader2, reader3, reader4, options),
                        UHPPOTE.ActivateKeypads(controller, reader1, reader2, reader3, reader4, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("activate-keypads")
            WriteLine("  controller {0}", controller)
            WriteLine("    reader 1 {0}", reader1)
            WriteLine("    reader 2 {0}", reader2)
            WriteLine("    reader 3 {0}", reader3)
            WriteLine("    reader 4 {0}", reader4)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetAntiPassback(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.GetAntiPassback(c, options),
                        UHPPOTE.GetAntiPassback(controller, options))

        If (result.IsOk)
            Dim antipassback = result.ResultValue

            WriteLine("get-antipassback")
            WriteLine("  controller      {0}", controller)
            WriteLine("    anti-passback {0}", translate(antipassback))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetAntiPassback(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim antipassback As AntiPassback = ArgParse.Parse(args, "--antipassback", AntiPassback.Disabled)

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetAntiPassback(c, antipassback, options),
                        UHPPOTE.SetAntiPassback(controller, antipassback, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-antipassback")
            WriteLine("  controller      {0}", controller)
            WriteLine("    anti-passback {0}", translate(antipassback))
            WriteLine("               ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub SetFirstCard(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim firstcard As FirstCard = ArgParse.Parse(args, "--firstcard", New FirstCard())

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.SetFirstCard(c, door, firstcard, options),
                        UHPPOTE.SetFirstCard(controller, door, firstcard, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-firstcard")
            WriteLine("  controller  {0}", controller)
            WriteLine("    firstcard {0}", firstcard)
            WriteLine("           ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub RestoreDefaultParameters(options As Options, args As String())
        Dim dest = ArgParse.Parse(args, "--dest", new Destination())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim c As C = Nothing

        Dim result = If(resolve(controller, dest, c),
                        UHPPOTE.RestoreDefaultParameters(c, options),
                        UHPPOTE.RestoreDefaultParameters(controller, options))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("restore-default-parameters")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub Listen(options As Options, args As String())
        Dim cancel = New CancellationTokenSource()

        AddHandler Console.CancelKeyPress, Sub(sender, e)
                                               e.Cancel = True
                                               cancel.Cancel()
                                           End Sub

        Dim onevent As New uhppoted.OnEvent(AddressOf eventHandler)
        Dim onerror As New uhppoted.OnError(AddressOf errorHandler)

        UHPPOTE.Listen(onevent, onerror, cancel.Token, options)
    End Sub

    Private Sub eventHandler(e As uhppoted.ListenerEvent)
        Dim controller = e.Controller
        Dim status = e.Status
        Dim evt = e.Event

        WriteLine("-- EVENT")
        WriteLine($"         controller {controller}")
        WriteLine("")

        WriteLine("              door 1 {1},{0}", If(status.Door1Open, "open", "closed"), translate(status.Relays.Door1))
        WriteLine("              door 2 {1},{0}", If(status.Door2Open, "open", "closed"), translate(status.Relays.Door2))
        WriteLine("              door 3 {1},{0}", If(status.Door3Open, "open", "closed"), translate(status.Relays.Door3))
        WriteLine("              door 4 {1},{0}", If(status.Door4Open, "open", "closed"), translate(status.Relays.Door4))
        WriteLine($"   button 1 pressed {status.Button1Pressed}")
        WriteLine($"   button 2 pressed {status.Button2Pressed}")
        WriteLine($"   button 3 pressed {status.Button3Pressed}")
        WriteLine($"   button 4 pressed {status.Button4Pressed}")
        WriteLine($"       system error {status.SystemError}")
        WriteLine($"   system date/time {YYYYMMDDHHmmss(status.SystemDateTime)}")
        WriteLine($"       special info {status.SpecialInfo}")
        WriteLine($"        lock forced {translate(status.Inputs.LockForced)}")
        WriteLine($"         fire alarm {translate(status.Inputs.FireAlarm)}")
        WriteLine("")

        If evt.HasValue Then
            Dim v = evt.Value
            WriteLine($"    event timestamp {YYYYMMDDHHmmss(v.Timestamp)}")
            WriteLine($"              index {v.Index}")
            WriteLine($"              event {v.Event.Text} ({v.Event.Code})")
            WriteLine($"            granted {v.AccessGranted}")
            WriteLine($"               door {v.Door}")
            WriteLine($"          direction {translate(v.Direction)}")
            WriteLine($"               card {v.Card}")
            WriteLine($"             reason {translate(v.Reason)}")
            WriteLine("")
        Else
            WriteLine("   (no event)")
            WriteLine("")
        End If
    End Sub

    Private Function resolve(controller As UInt32, dest As Destination, ByRef c As uhppoted.C) As Boolean
        if (dest.endpoint Is Nothing) Then
            c = Nothing
            return False
        End If

        if dest.endpoint.Port = 0 Then
            c = New CBuilder(controller).
                    WithEndPoint(new IPEndPoint(dest.endpoint.Address, 60000)).
                    WithProtocol(If(dest.transport, "")).
                    Build()

            return True
        End If

        c = New CBuilder(controller).
                WithEndPoint(dest.endpoint).
                WithProtocol(If(dest.transport, "")).
                Build()

        return True
    End Function

    Private Sub errorHandler(err)
        WriteLine("** ERROR {0}", translate(err))
    End Sub

    Private Function YYYYMMDD(v As Nullable(Of DateOnly)) As String
        If v.HasValue Then
            Return v.Value.ToString("yyyy-MM-dd")
        Else
            Return "---"
        End If
    End Function

    Private Function YYYYMMDDHHmmss(datetime As DateTime?) As String
        If datetime.HasValue Then
            Return datetime.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Return "---"
        End If
    End Function

    Private Function HHmm(time As TimeOnly?) As String
        If time.HasValue Then
            Return time.Value.ToString("HH:mm")
        Else
            Return "---"
        End If
    End Function

    Private Function translate(val) As String
        return UHPPOTE.Translate(val)
    End Function

End Module
