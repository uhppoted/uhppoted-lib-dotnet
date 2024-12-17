Imports System.Console
Imports System.Net
Imports System.Threading

Imports UHPPOTE = uhppoted.Uhppoted
Imports OptionsBuilder = uhppoted.OptionsBuilder
Imports DoorMode = uhppoted.DoorMode
Imports Interlock = uhppoted.Interlock

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
    Private Const TASK_ID as Byte = 0

    Private ReadOnly Dim IPv4_ADDRESS = IPAddress.Parse("192.168.1.10")
    Private ReadOnly Dim IPv4_NETMASK = IPAddress.Parse("255.255.255.0")
    Private ReadOnly Dim IPv4_GATEWAY = IPAddress.Parse("192.168.1.1")
    Private ReadOnly Dim EVENT_LISTENER = IPEndPoint.Parse("192.168.1.250:60001")
    Private ReadOnly Dim START_DATE As DateOnly = New DateOnly(2024, 1, 1)
    Private ReadOnly Dim END_DATE As DateOnly = New DateOnly(2024, 12, 31)

    Private ReadOnly Dim OPTIONS = New OptionsBuilder().
                                WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000")).
                                WithProtocol("udp").
                                WithTimeout(1000).
                                WithDebug(True).
                                Build()

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
           New Command("restore-default-parameters", "Restores the manufacturer defaults", AddressOf RestoreDefaultParameters),
           New Command("listen", "Listens for access controller events", AddressOf Listen)
       }

    Sub FindControllers(args As String())
        Dim result = UHPPOTE.FindControllers(OPTIONS)

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
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetController(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetController(controller, OPTIONS)

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
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetIPv4(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim address = ArgParse.Parse(args, "--address", IPv4_ADDRESS)
        Dim netmask = ArgParse.Parse(args, "--netmask", IPv4_NETMASK)
        Dim gateway = ArgParse.Parse(args, "--gateway", IPv4_GATEWAY)
        Dim result = UHPPOTE.SetIPv4(controller, address, netmask, gateway, OPTIONS)

        If (result.IsOk)
            WriteLine("set-IPv4")
            WriteLine("  ok")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetListener(controller, OPTIONS)

        If (result.IsOk)
            Dim record = result.ResultValue

            WriteLine("get-listener")
            WriteLine("  controller {0}", controller)
            WriteLine("    endpoint {0}", record.Endpoint)
            WriteLine("    interval {0}s", record.Interval)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim listener = ArgParse.Parse(args, "--listener", EVENT_LISTENER)
        Dim interval = ArgParse.Parse(args, "--interval", EVENT_INTERVAL)
        Dim result = UHPPOTE.SetListener(controller, listener, interval, OPTIONS)

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
        Dim result = UHPPOTE.GetTime(controller, OPTIONS)

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
        Dim result = UHPPOTE.SetTime(controller, now, OPTIONS)

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
        Dim result = UHPPOTE.GetDoor(controller, door, OPTIONS)

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
        Dim result = UHPPOTE.SetDoor(controller, door, mode, delay, OPTIONS)

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
        Dim passcodes As UInteger() = ArgParse.Parse(args, "--passcodes", new UInteger() {})
        Dim result = UHPPOTE.SetDoorPasscodes(controller, door, passcodes, OPTIONS)

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
        Dim result = UHPPOTE.OpenDoor(controller, door, OPTIONS)

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
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetStatus(controller, OPTIONS)

        If (result.IsOk)
            Dim status = result.ResultValue.Item1
            Dim evt = result.ResultValue.Item2

            WriteLine("get-status")
            WriteLine("         controller {0}", controller)
            WriteLine("        door 1 open {0}", status.Door1Open)
            WriteLine("        door 2 open {0}", status.Door2Open)
            WriteLine("        door 3 open {0}", status.Door3Open)
            WriteLine("        door 4 open {0}", status.Door4Open)
            WriteLine("   button 1 pressed {0}", status.Button1Pressed)
            WriteLine("   button 2 pressed {0}", status.Button2Pressed)
            WriteLine("   button 3 pressed {0}", status.Button3Pressed)
            WriteLine("   button 4 pressed {0}", status.Button4Pressed)
            WriteLine("       system error {0}", status.SystemError)
            WriteLine("   system date/time {0}", YYYYMMDDHHmmss(status.SystemDateTime))
            WriteLine("       sequence no. {0}", status.SequenceNumber)
            WriteLine("       special info {0}", status.SpecialInfo)
            WriteLine("            relay 1 {0}", status.Relay1)
            WriteLine("            relay 2 {0}", status.Relay2)
            WriteLine("            relay 3 {0}", status.Relay3)
            WriteLine("            relay 4 {0}", status.Relay4)
            WriteLine("            input 1 {0}", status.Input1)
            WriteLine("            input 2 {0}", status.Input2)
            WriteLine("            input 3 {0}", status.Input3)
            WriteLine("            input 4 {0}", status.Input4)
            WriteLine()

            If evt.HasValue Then
                WriteLine("    event index     {0}", evt.Value.Index)
                WriteLine("          event     {0}", evt.Value.Event)
                WriteLine("          granted   {0}", evt.Value.AccessGranted)
                WriteLine("          door      {0}", evt.Value.Door)
                WriteLine("          direction {0}", evt.Value.Direction)
                WriteLine("          card      {0}", evt.Value.Card)
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(evt.Value.Timestamp))
                WriteLine("          reason    {0}", evt.Value.Reason)
                WriteLine()
            Else
                WriteLine("    (no event)")
                WriteLine()
            End If

        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetCards(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetCards(controller, OPTIONS)

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
        Dim result = UHPPOTE.GetCard(controller, card, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", record.Card)
            WriteLine("  start date {0}", (YYYYMMDD(record.StartDate)))
            WriteLine("    end date {0}", (YYYYMMDD(record.EndDate)))
            WriteLine("      door 1 {0}", record.Door1)
            WriteLine("      door 2 {0}", record.Door2)
            WriteLine("      door 3 {0}", record.Door3)
            WriteLine("      door 4 {0}", record.Door4)
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
        Dim index = ArgParse.Parse(args, "--card", CARD_INDEX)
        Dim result = UHPPOTE.GetCardAtIndex(controller, index, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card-at-index")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", record.Card)
            WriteLine("  start date {0}", (YYYYMMDD(record.StartDate)))
            WriteLine("    end date {0}", (YYYYMMDD(record.EndDate)))
            WriteLine("      door 1 {0}", record.Door1)
            WriteLine("      door 2 {0}", record.Door2)
            WriteLine("      door 3 {0}", record.Door3)
            WriteLine("      door 4 {0}", record.Door4)
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
        Dim cardNumber = ArgParse.Parse(args, "--card", CARD_NUMBER)
        Dim startDate = ArgParse.Parse(args, "--start-date", START_DATE)
        Dim endDate = ArgParse.Parse(args, "--end-date", END_DATE)
        Dim permissions = ArgParse.Parse(args, "--permissions", New Dictionary(Of Integer, Byte))
        Dim PIN = ArgParse.Parse(args, "--PIN", CUint(0))

        Dim u8 As Byte
        Dim door1 As Byte = If(permissions.TryGetValue(1, u8), u8, 0)
        Dim door2 As Byte = If(permissions.TryGetValue(2, u8), u8, 0)
        Dim door3 As Byte = If(permissions.TryGetValue(3, u8), u8, 0)
        Dim door4 As Byte = If(permissions.TryGetValue(4, u8), u8, 0)

        Dim card = New uhppoted.CardBuilder(cardNumber).
                                WithStartDate(startDate).
                                WithEndDate(endDate).
                                WithDoor1(door1).
                                WithDoor2(door2).
                                WithDoor3(door3).
                                WithDoor4(door4).
                                WithPIN(PIN).
                                Build()

        Dim result = UHPPOTE.PutCard(controller, card, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("put-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", card.Card)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub DeleteCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)
        Dim result = UHPPOTE.DeleteCard(controller, card, OPTIONS)

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

        Dim result = UHPPOTE.DeleteAllCards(controller, OPTIONS)

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
        Dim result = UHPPOTE.GetEvent(controller, index, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-event")
            WriteLine("  controller {0}", controller)
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.Timestamp)))
            WriteLine("       index {0}", record.Index)
            WriteLine("       event {0}", record.Event)
            WriteLine("     granted {0}", record.AccessGranted)
            WriteLine("        door {0}", record.Door)
            WriteLine("   direction {0}", record.Direction)
            WriteLine("        card {0}", record.Card)
            WriteLine("      reason {0}", record.Reason)
            WriteLine()
        Else If (result.IsOk)
            Throw New Exception("event not found")
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub GetEventIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim result = UHPPOTE.GetEventIndex(controller, OPTIONS)

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
        Dim result = UHPPOTE.SetEventIndex(controller, index, OPTIONS)

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
        Dim result = UHPPOTE.RecordSpecialEvents(controller, enable, OPTIONS)

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
        Dim result = UHPPOTE.GetTimeProfile(controller, profile, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

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
        Dim start_date As DateOnly = ArgParse.Parse(args, "--start_date", START_DATE)
        Dim end_date As DateOnly = ArgParse.Parse(args, "--end_date", END_DATE)
        Dim weekdays = ArgParse.Parse(args, "--weekdays", New Weekdays(True, True, False, False, True, False, False))
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

        Dim result = UHPPOTE.SetTimeProfile(controller, profile, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-time-profile")
            WriteLine("  controller {0}", controller)
            WriteLine("     profile {0}", profile.Profile)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub ClearTimeProfiles(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.ClearTimeProfiles(controller, OPTIONS)

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
        Dim start_date As DateOnly = ArgParse.Parse(args, "--start_date", START_DATE)
        Dim end_date As DateOnly = ArgParse.Parse(args, "--end_date", END_DATE)
        Dim start_time = ArgParse.Parse(args, "--start_time", TimeOnly.Parse("00:00"))
        Dim weekdays = ArgParse.Parse(args, "--weekdays", New Weekdays(True, True, False, False, True, False, False))
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

        Dim result = UHPPOTE.AddTask(controller, task, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("add-task")
            WriteLine("  controller {0}", controller)
            WriteLine("        task {0}", task.Task)
            WriteLine("        door {0}", task.Door)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub ClearTaskList(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.ClearTaskList(controller, OPTIONS)

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

        Dim result = UHPPOTE.RefreshTaskList(controller, OPTIONS)

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

    Sub SetPCControl(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim enable = ArgParse.Parse(args, "--enable", False)
        Dim result = UHPPOTE.SetPCControl(controller, enable, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-pc-control")
            WriteLine("  controller {0}", controller)
            WriteLine("      enable {0}", enable)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub SetInterlock(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim interlock As Interlock = ArgParse.Parse(args, "--interlock", Interlock.None)
        Dim result = UHPPOTE.SetInterlock(controller, interlock, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("set-interlock")
            WriteLine("  controller {0}", controller)
            WriteLine("   interlock {0}", interlock)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub ActivateKeypads(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim keypads = ArgParse.Parse(args, "--keypads", New List(Of Byte)({1, 2, 4}))

        Dim reader1 = keypads.Contains(1)
        Dim reader2 = keypads.Contains(2)
        Dim reader3 = keypads.Contains(3)
        Dim reader4 = keypads.Contains(4)

        Dim result = UHPPOTE.ActivateKeypads(controller, reader1, reader2, reader3, reader4, OPTIONS)

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
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub RestoreDefaultParameters(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = UHPPOTE.RestoreDefaultParameters(controller, OPTIONS)

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("restore-default-parameters")
            WriteLine("  controller {0}", controller)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(result.ErrorValue)
        End If
    End Sub

    Sub Listen(args As String())
        Dim cancel = New CancellationTokenSource()

        AddHandler Console.CancelKeyPress, Sub(sender, e)
                                               e.Cancel = True
                                               cancel.Cancel()
                                           End Sub

        Dim onevent As New uhppoted.OnEvent(AddressOf eventHandler)
        Dim onerror As New uhppoted.OnError(AddressOf errorHandler)

        UHPPOTE.Listen(onevent, onerror, cancel.Token, OPTIONS)
    End Sub

    Private Sub eventHandler(e As uhppoted.ListenerEvent)
        Dim controller = e.Controller
        Dim status = e.Status
        Dim evt = e.Event

        WriteLine("-- EVENT")
        WriteLine($"         controller {controller}")
        WriteLine("")

        WriteLine($"        door 1 open {status.Door1Open}")
        WriteLine($"        door 2 open {status.Door2Open}")
        WriteLine($"        door 3 open {status.Door3Open}")
        WriteLine($"        door 4 open {status.Door4Open}")
        WriteLine($"   button 1 pressed {status.Button1Pressed}")
        WriteLine($"   button 2 pressed {status.Button2Pressed}")
        WriteLine($"   button 3 pressed {status.Button3Pressed}")
        WriteLine($"   button 4 pressed {status.Button4Pressed}")
        WriteLine($"       system error {status.SystemError}")
        WriteLine($"   system date/time {YYYYMMDDHHmmss(status.SystemDateTime)}")
        WriteLine($"       sequence no. {status.SequenceNumber}")
        WriteLine($"       special info {status.SpecialInfo}")
        WriteLine($"            relay 1 {status.Relay1}")
        WriteLine($"            relay 2 {status.Relay2}")
        WriteLine($"            relay 3 {status.Relay3}")
        WriteLine($"            relay 4 {status.Relay4}")
        WriteLine($"            input 1 {status.Input1}")
        WriteLine($"            input 2 {status.Input2}")
        WriteLine($"            input 3 {status.Input3}")
        WriteLine($"            input 4 {status.Input4}")
        WriteLine("")

        If evt.HasValue Then
            Dim v = evt.Value
            WriteLine($"    event timestamp {YYYYMMDDHHmmss(v.Timestamp)}")
            WriteLine($"              index {v.Index}")
            WriteLine($"              event {v.Event}")
            WriteLine($"            granted {v.AccessGranted}")
            WriteLine($"               door {v.Door}")
            WriteLine($"          direction {v.Direction}")
            WriteLine($"               card {v.Card}")
            WriteLine($"             reason {v.Reason}")
            WriteLine("")
        Else
            WriteLine("   (no event)")
            WriteLine("")
        End If
    End Sub

    Private Sub errorHandler(err)
        WriteLine("** ERROR {0}", err)
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

End Module
