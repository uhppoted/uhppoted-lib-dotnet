Imports System.Console
Imports System.Net

Imports UHPPOTE = uhppoted.Uhppoted
Imports ControllerBuilder = uhppoted.ControllerBuilder
Imports OptionsBuilder = uhppoted.OptionsBuilder
Imports DoorMode = uhppoted.DoorMode

Public Structure Command
    Public ReadOnly command As String
    Public ReadOnly description As String
    Public ReadOnly f As Action(Of String())

    Public Sub New(command As String, description As String, f As Action(Of String()))
        Me.command = command
        Me.description = description
        Me.f = f
    End Sub
End Structure

Public Structure Weekdays
    Public ReadOnly monday As Boolean
    Public ReadOnly tuesday As Boolean
    Public ReadOnly wednesday As Boolean
    Public ReadOnly thursday As Boolean
    Public ReadOnly friday As Boolean
    Public ReadOnly saturday As Boolean
    Public ReadOnly sunday As Boolean

    Public Sub New(monday As Boolean, tuesday As Boolean, wednesday As Boolean, thursday As Boolean, friday As Boolean, saturday As Boolean, sunday As Boolean)
        Me.monday = monday
        Me.tuesday = tuesday
        Me.wednesday = wednesday
        Me.thursday = thursday
        Me.friday = friday
        Me.saturday = saturday
        Me.sunday = sunday
    End Sub
End Structure

Public Structure TimeSegment
    Public ReadOnly StartTime As TimeOnly
    Public ReadOnly EndTime As TimeOnly

    Public Sub New(startTime As TimeOnly, endTime As TimeOnly)
        Me.StartTime = startTime
        Me.EndTime = endTime
    End Sub
End Structure

Module Commands
    Private Const TIMEOUT = 1000

    Private Const CONTROLLER_ID As UInt32 = 1
    Private Const EVENT_INTERVAL as Byte = 0
    Private Const DOOR As Byte = 1
    Private Const MODE As Uhppoted.DoorMode = Uhppoted.DoorMode.Controlled
    Private Const DELAY As Byte = 5
    Private Const CARD_NUMBER As UInt32 = 1
    Private Const CARD_INDEX As UInt32 = 1
    Private Const EVENT_INDEX As UInt32 = 1
    Private Const ENABLE as Boolean = true
    Private Const TIME_PROFILE_ID as Byte = 0
    Private Const TASK_ID as Byte = 0

    Private ReadOnly Dim IPv4_ADDRESS = IPAddress.Parse("192.168.1.10")
    Private ReadOnly Dim IPv4_NETMASK = IPAddress.Parse("255.255.255.0")
    Private ReadOnly Dim IPv4_GATEWAY = IPAddress.Parse("192.168.1.1")
    Private ReadOnly Dim EVENT_LISTENER = IPEndPoint.Parse("192.168.1.250:60001")

    Private ReadOnly Dim OPTIONS = New OptionsBuilder().
                                WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000")).
                                WithProtocol("udp").
                                WithDebug(true).
                                build()

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
           New Command("refresh-tasklist", "Schedules added tasks", AddressOf RefreshTaskList)
       }

    Sub FindControllers(args As String())
        Dim result = UHPPOTE.FindControllers(TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim controllers = result.ResultValue

            WriteLine("get-controllers: {0}", controllers.Length)
            For Each controller In controllers
                WriteLine("  controller {0}", controller.controller)
                WriteLine("    address  {0}", controller.address)
                WriteLine("    netmask  {0}", controller.netmask)
                WriteLine("    gateway  {0}", controller.gateway)
                WriteLine("    MAC      {0}", controller.MAC)
                WriteLine("    version  {0}", controller.version)
                WriteLine("    date     {0}", YYYYMMDD(controller.date))
                WriteLine()
            Next
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetController(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetController(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-controller")
            WriteLine("  controller {0}", record.controller)
            WriteLine("    address  {0}", record.address)
            WriteLine("    netmask  {0}", record.netmask)
            WriteLine("    gateway  {0}", record.gateway)
            WriteLine("    MAC      {0}", record.MAC)
            WriteLine("    version  {0}", record.version)
            WriteLine("    date     {0}", YYYYMMDD(record.date))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetIPv4(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim address = ArgParse.Parse(args, "--address", IPv4_ADDRESS)
        Dim netmask = ArgParse.Parse(args, "--netmask", IPv4_NETMASK)
        Dim gateway = ArgParse.Parse(args, "--gateway", IPv4_GATEWAY)
        Dim result = UHPPOTE.SetIPv4(controller, address, netmask, gateway, TIMEOUT, OPTIONS)

        If (result.IsOk)
            WriteLine("set-IPv4")
            WriteLine("  ok")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetListener(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-listener")
            WriteLine("  controller {0}", controller)
            WriteLine("    endpoint {0}", record.endpoint)
            WriteLine("    interval {0}s", record.interval)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim listener = ArgParse.Parse(args, "--listener", EVENT_LISTENER)
        Dim interval = ArgParse.Parse(args, "--interval", EVENT_INTERVAL)
        Dim result = UHPPOTE.SetListener(controller, listener, interval, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-listener")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetTime(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetTime(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim datetime = result.ResultValue

            WriteLine("get-time")
            WriteLine("  controller {0}", controller)
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetTime(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim now = ArgParse.Parse(args, "--datetime", DateTime.Now)
        Dim result = UHPPOTE.SetTime(controller, now, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim datetime = result.ResultValue

            WriteLine("set-time")
            WriteLine("  controller {0}", controller)
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime))
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim result = UHPPOTE.GetDoor(controller, door, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-door")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("        mode {0}", record.mode)
            WriteLine("       delay {0}s", record.delay)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("door does not exist")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim mode As DoorMode = ArgParse.Parse(args, "--mode", MODE)
        Dim delay As Byte = ArgParse.Parse(args, "--delay", DELAY)
        Dim result = UHPPOTE.SetDoor(controller, door, mode, delay, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("set-door")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("        mode {0}", record.mode)
            WriteLine("       delay {0}s", record.delay)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("door not updated")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetDoorPasscodes(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim passcodes As UInteger() = ArgParse.Parse(args, "--passcodes", new UInteger() {0, 0, 0, 0})
        Dim result = UHPPOTE.SetDoorPasscodes(controller, door, passcodes(0), passcodes(1), passcodes(2), passcodes(3), TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-door-passcodes")
            WriteLine("  controller {0}", controller)
            WriteLine("        door {0}", door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub OpenDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim result = UHPPOTE.OpenDoor(controller, door, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("open-door")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetStatus(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim result = UHPPOTE.get_status(controller, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-status")
                WriteLine("         controller {0}", response.controller)
                WriteLine("        door 1 open {0}", response.door1_open)
                WriteLine("        door 2 open {0}", response.door2_open)
                WriteLine("        door 3 open {0}", response.door3_open)
                WriteLine("        door 4 open {0}", response.door3_open)
                WriteLine("   button 1 pressed {0}", response.door1_button)
                WriteLine("   button 2 pressed {0}", response.door1_button)
                WriteLine("   button 3 pressed {0}", response.door1_button)
                WriteLine("   button 4 pressed {0}", response.door1_button)
                WriteLine("       system error {0}", response.system_error)
                WriteLine("   system date/time {0}", YYYYMMDDHHmmss(response.system_datetime))
                WriteLine("       sequence no. {0}", response.sequence_number)
                WriteLine("       special info {0}", response.special_info)
                WriteLine("             relays {0:X}", response.relays)
                WriteLine("             inputs {0:X}", response.inputs)
                WriteLine()
                WriteLine("    event index     {0}", response.evt.index)
                WriteLine("          event     {0}", response.evt.event_type)
                WriteLine("          granted   {0}", response.evt.granted)
                WriteLine("          door      {0}", response.evt.door)
                WriteLine("          direction {0}", response.evt.direction)
                WriteLine("          card      {0}", response.evt.card)
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(response.evt.timestamp))
                WriteLine("          reason    {0}", response.evt.reason)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetCards(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetCards(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim cards = result.ResultValue

            WriteLine("get-cards")
            WriteLine("  controller {0}", controller)
            WriteLine("       cards {0}", cards)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)
        Dim result = UHPPOTE.GetCard(controller, card, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", record.card)
            WriteLine("  start date {0}", (YYYYMMDD(record.start_date)))
            WriteLine("    end date {0}", (YYYYMMDD(record.end_date)))
            WriteLine("      door 1 {0}", record.door1)
            WriteLine("      door 2 {0}", record.door2)
            WriteLine("      door 3 {0}", record.door3)
            WriteLine("      door 4 {0}", record.door4)
            WriteLine("         PIN {0}", record.PIN)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("card not found")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetCardAtIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = CARD_INDEX
        Dim result = UHPPOTE.GetCardAtIndex(controller, index, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card-at-index")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", record.card)
            WriteLine("  start date {0}", (YYYYMMDD(record.start_date)))
            WriteLine("    end date {0}", (YYYYMMDD(record.end_date)))
            WriteLine("      door 1 {0}", record.door1)
            WriteLine("      door 2 {0}", record.door2)
            WriteLine("      door 3 {0}", record.door3)
            WriteLine("      door 4 {0}", record.door4)
            WriteLine("         PIN {0}", record.PIN)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("card not found")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub PutCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)
        Dim startdate = New DateOnly(2024, 1, 1)
        Dim enddate = New DateOnly(2024, 12, 31)
        Dim door1 = 1
        Dim door2 = 0
        Dim door3 = 17
        Dim door4 = 1
        Dim PIN = 7531

        Dim result = UHPPOTE.PutCard(controller, card, startdate, enddate, door1, door2, door3, door4, PIN, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("put-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", card)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub DeleteCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)
        Dim result = UHPPOTE.DeleteCard(controller, card, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("delete-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", card)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub DeleteAllCards(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.DeleteAllCards(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("delete-all-cards")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetEvent(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)
        Dim result = UHPPOTE.GetEvent(controller, index, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-event")
            WriteLine("  controller {0}", controller)
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.timestamp)))
            WriteLine("       index {0}", record.index)
            WriteLine("       event {0}", record.event_type)
            WriteLine("     granted {0}", record.access_granted)
            WriteLine("        door {0}", record.door)
            WriteLine("   direction {0}", record.direction)
            WriteLine("        card {0}", record.card)
            WriteLine("      reason {0}", record.reason)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("event not found")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetEventIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetEventIndex(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim index = result.ResultValue

            WriteLine("get-event-index")
            WriteLine("  controller {0}", controller)
            WriteLine("       index {0}", index)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetEventIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)
        Dim result = UHPPOTE.SetEventIndex(controller, index, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-event-index")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub RecordSpecialEvents(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim enable As Boolean = ArgParse.Parse(args, "--enable", ENABLE)
        Dim result = UHPPOTE.RecordSpecialEvents(controller, enable, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("record-special-events")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetTimeProfile(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim profile = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID)
        Dim result = UHPPOTE.GetTimeProfile(controller, profile, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-time-profile")
            WriteLine("          controller {0}", controller)
            WriteLine("             profile {0}", record.profile)
            WriteLine("          start date {0}", YYYYMMDD(record.start_date))
            WriteLine("            end date {0}", YYYYMMDD(record.end_date))
            WriteLine("              monday {0}", record.monday)
            WriteLine("             tuesday {0}", record.tuesday)
            WriteLine("           wednesday {0}", record.wednesday)
            WriteLine("            thursday {0}", record.thursday)
            WriteLine("              friday {0}", record.friday)
            WriteLine("            saturday {0}", record.saturday)
            WriteLine("              sunday {0}", record.sunday)
            WriteLine("   segment 1 - start {0}", HHmm(record.segment1_start))
            WriteLine("                 end {0}", HHmm(record.segment1_end))
            WriteLine("   segment 2 - start {0}", HHmm(record.segment2_start))
            WriteLine("                 end {0}", HHmm(record.segment2_end))
            WriteLine("   segment 3 - start {0}", HHmm(record.segment3_start))
            WriteLine("                 end {0}", HHmm(record.segment3_end))
            WriteLine("      linked profile {0}", record.linked_profile)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("time profile does not exist")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetTimeProfile(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim profile_id = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID)
        Dim linked As Byte = ArgParse.Parse(args, "--linked", CType(0, Byte))
        Dim start_date = ArgParse.Parse(args, "--start_date", DateOnly.Parse("2024-01-01"))
        Dim end_date = ArgParse.Parse(args, "--end_date", DateOnly.Parse("2024-12-31"))
        Dim weekdays = ArgParse.Parse(args, "--weekdays", New Weekdays(true, true, false, false, true, false, false))
        Dim segments = ArgParse.Parse(args, "--segments", New TimeSegment() {
            New TimeSegment(TimeOnly.Parse("08:30"), TimeOnly.Parse("09:45")),
            New TimeSegment(TimeOnly.Parse("12:15"), TimeOnly.Parse("13:15")),
            New TimeSegment(TimeOnly.Parse("14:00"), TimeOnly.Parse("18:00"))
        })

        Dim monday = weekdays.monday
        Dim tuesday = weekdays.tuesday
        Dim wednesday = weekdays.wednesday
        Dim thursday = weekdays.thursday
        Dim friday = weekdays.friday
        Dim saturday = weekdays.saturday
        Dim sunday = weekdays.sunday

        Dim profile = New uhppoted.TimeProfileBuilder(profile_id).
                                   WithStartDate(start_date).
                                   WithEndDate(end_date).
                                   WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday).
                                   WithSegment1(segments(0).StartTime, segments(0).EndTime).
                                   WithSegment2(segments(1).StartTime, segments(1).EndTime).
                                   WithSegment3(segments(2).StartTime, segments(2).EndTime).
                                   WithLinkedProfile(linked).
                                   build()

        Dim result = UHPPOTE.SetTimeProfile(controller, profile, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-time-profile")
            WriteLine("  controller {0}", controller)
            WriteLine("     profile {0}", profile.profile)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub ClearTimeProfiles(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.ClearTimeProfiles(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("clear-time-profiles")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub AddTask(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim _task_id As Byte = ArgParse.Parse(args, "--task", TASK_ID)
        Dim door_id As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim start_date = ArgParse.Parse(args, "--start_date", DateOnly.Parse("2024-01-01"))
        Dim end_date = ArgParse.Parse(args, "--end_date", DateOnly.Parse("2024-12-31"))
        Dim start_time = ArgParse.Parse(args, "--start_time", TimeOnly.Parse("00:00"))
        Dim weekdays = ArgParse.Parse(args, "--weekdays", New Weekdays(true, true, false, false, true, false, false))
        Dim more_cards As Byte = ArgParse.Parse(args, "--more-cards", CType(0, Byte))

        Dim monday = weekdays.monday
        Dim tuesday = weekdays.tuesday
        Dim wednesday = weekdays.wednesday
        Dim thursday = weekdays.thursday
        Dim friday = weekdays.friday
        Dim saturday = weekdays.saturday
        Dim sunday = weekdays.sunday

        Dim task = New uhppoted.TaskBuilder(_task_id, door_id).
                                WithStartDate(start_date).
                                WithEndDate(end_date).
                                WithStartTime(start_time).
                                WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday).
                                WithMoreCards(more_cards).
                                build()

        Dim result = UHPPOTE.AddTask(controller, task, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("add-task")
            WriteLine("  controller {0}", controller)
            WriteLine("        task {0}", task.task)
            WriteLine("        door {0}", task.door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub ClearTaskList(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.ClearTaskList(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("clear-tasklist")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub RefreshTaskList(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.RefreshTaskList(controller, TIMEOUT, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("refresh-tasklist")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Private Function YYYYMMDD(v As Nullable(Of DateOnly)) As String
        If v.HasValue Then
            Return v.Value.ToString("yyyy-MM-dd")
        Else
            Return "---"
        End If
    End Function

    Public Function YYYYMMDDHHmmss(datetime As DateTime?) As String
        If datetime.HasValue Then
            Return datetime.Value.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Return "---"
        End If
    End Function

    Public Function HHmm(time As TimeOnly?) As String
        If time.HasValue Then
            Return time.Value.ToString("HH:mm")
        Else
            Return "---"
        End If
    End Function

End Module
