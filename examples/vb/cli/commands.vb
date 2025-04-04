Imports System.Console
Imports System.Net
Imports System.Threading

Imports UHPPOTE = uhppoted.Uhppoted
Imports OptionsBuilder = uhppoted.OptionsBuilder
Imports CBuilder = uhppoted.CBuilder
Imports DoorMode = uhppoted.DoorMode
Imports Interlock = uhppoted.Interlock
Imports AntiPassback = uhppoted.AntiPassback
Imports Err = uhppoted.Err

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

    Private ReadOnly Dim OPTIONS = New OptionsBuilder().
                                       WithBind(New IPEndPoint(IPAddress.Any, 0)).
                                       WithBroadcast(New IPEndPoint(IPAddress.Broadcast, 60000)).
                                       WithTimeout(1000).
                                       WithDebug(True).
                                       Build()

    Private ReadOnly CONTROLLERS As New Dictionary(Of UInteger, Uhppoted.C) From {
        {303986753UI, New CBuilder(303986753UI).
                          WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                          WithProtocol("udp").
                          Build()
        },
        {201020304UI, New CBuilder(201020304UI).
                          WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                          WithProtocol("tcp").
                          Build()
        }
     }

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
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetController(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetController(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetController(controller, OPTIONS))

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

    Sub SetIPv4(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim address = ArgParse.Parse(args, "--address", IPv4_ADDRESS)
        Dim netmask = ArgParse.Parse(args, "--netmask", IPv4_NETMASK)
        Dim gateway = ArgParse.Parse(args, "--gateway", IPv4_GATEWAY)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                       UHPPOTE.SetIPv4(CONTROLLERS(controller), address, netmask, gateway, OPTIONS),
                       UHPPOTE.SetIPv4(controller, address, netmask, gateway, OPTIONS))

        If (result.IsOk)
            WriteLine("set-IPv4")
            WriteLine("  ok")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                       UHPPOTE.GetListener(CONTROLLERS(controller), OPTIONS),
                       UHPPOTE.GetListener(controller, OPTIONS))

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

    Sub SetListener(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim listener = ArgParse.Parse(args, "--listener", EVENT_LISTENER)
        Dim interval = ArgParse.Parse(args, "--interval", EVENT_INTERVAL)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetListener(CONTROLLERS(controller), listener, interval, OPTIONS),
                        UHPPOTE.SetListener(controller, listener, interval, OPTIONS))

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

    Sub GetTime(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetTime(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetTime(controller, OPTIONS))

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

    Sub SetTime(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim now = ArgParse.Parse(args, "--datetime", DateTime.Now)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetTime(CONTROLLERS(controller), now, OPTIONS),
                        UHPPOTE.SetTime(controller, now, OPTIONS))

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

    Sub GetDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetDoor(CONTROLLERS(controller), door, OPTIONS),
                        UHPPOTE.GetDoor(controller, door, OPTIONS))

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

    Sub SetDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim mode As DoorMode = ArgParse.Parse(args, "--mode", MODE)
        Dim delay As Byte = ArgParse.Parse(args, "--delay", DELAY)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetDoor(CONTROLLERS(controller), door, mode, delay, OPTIONS),
                        UHPPOTE.SetDoor(controller, door, mode, delay, OPTIONS))

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

    Sub SetDoorPasscodes(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)
        Dim passcodes As UInteger() = ArgParse.Parse(args, "--passcodes", new UInteger() {})

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetDoorPasscodes(CONTROLLERS(controller), door, passcodes, OPTIONS),
                        UHPPOTE.SetDoorPasscodes(controller, door, passcodes, OPTIONS))

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

    Sub OpenDoor(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim door As Byte = ArgParse.Parse(args, "--door", DOOR)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.OpenDoor(CONTROLLERS(controller), door, OPTIONS),
                        UHPPOTE.OpenDoor(controller, door, OPTIONS))

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

    Sub GetStatus(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetStatus(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetStatus(controller, OPTIONS))

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

    Sub GetCards(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetCards(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetCards(controller, OPTIONS))

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

    Sub GetCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetCard(CONTROLLERS(controller), card, OPTIONS),
                        UHPPOTE.GetCard(controller, card, OPTIONS))

        If (result.IsOk)
            Dim record = result.ResultValue

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
        Else If (result.IsError AndAlso result.ErrorValue is Err.CardNotFound)
            Throw New Exception("card not found")
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub GetCardAtIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = ArgParse.Parse(args, "--card", CARD_INDEX)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetCardAtIndex(CONTROLLERS(controller), index, OPTIONS),
                        UHPPOTE.GetCardAtIndex(controller, index, OPTIONS))

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
            Throw New Exception(translate(result.ErrorValue))
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


        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.PutCard(CONTROLLERS(controller), card, OPTIONS),
                        UHPPOTE.PutCard(controller, card, OPTIONS))

        If (result.IsOk)
            Dim ok = result.ResultValue

            WriteLine("put-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", card.Card)
            WriteLine("          ok {0}", ok)
            WriteLine()
        Else If (result.IsError)
            Throw New Exception(translate(result.ErrorValue))
        End If
    End Sub

    Sub DeleteCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = ArgParse.Parse(args, "--card", CARD_NUMBER)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.DeleteCard(CONTROLLERS(controller), card, OPTIONS),
                        UHPPOTE.DeleteCard(controller, card, OPTIONS))

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

    Sub DeleteAllCards(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.DeleteAllCards(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.DeleteAllCards(controller, OPTIONS))

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

    Sub GetEvent(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetEvent(CONTROLLERS(controller), index, OPTIONS),
                        UHPPOTE.GetEvent(controller, index, OPTIONS))

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

    Sub GetEventIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetEventIndex(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetEventIndex(controller, OPTIONS))

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

    Sub SetEventIndex(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim index = ArgParse.Parse(args, "--index", EVENT_INDEX)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetEventIndex(CONTROLLERS(controller), index, OPTIONS),
                        UHPPOTE.SetEventIndex(controller, index, OPTIONS))

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

    Sub RecordSpecialEvents(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim enable As Boolean = ArgParse.Parse(args, "--enable", ENABLE)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.RecordSpecialEvents(CONTROLLERS(controller), enable, OPTIONS),
                        UHPPOTE.RecordSpecialEvents(controller, enable, OPTIONS))

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

    Sub GetTimeProfile(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim profile = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetTimeProfile(CONTROLLERS(controller), profile, OPTIONS),
                        UHPPOTE.GetTimeProfile(controller, profile, OPTIONS))

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

    Sub SetTimeProfile(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
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

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetTimeProfile(CONTROLLERS(controller), profile, OPTIONS),
                        UHPPOTE.SetTimeProfile(controller, profile, OPTIONS))

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

    Sub ClearTimeProfiles(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.ClearTimeProfiles(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.ClearTimeProfiles(controller, OPTIONS))

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

    Sub AddTask(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
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

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.AddTask(CONTROLLERS(controller), task, OPTIONS),
                        UHPPOTE.AddTask(controller, task, OPTIONS))

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

    Sub ClearTaskList(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.ClearTaskList(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.ClearTaskList(controller, OPTIONS))

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

    Sub RefreshTaskList(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.RefreshTaskList(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.RefreshTaskList(controller, OPTIONS))

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

    Sub SetPCControl(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim enable = ArgParse.Parse(args, "--enable", False)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetPCControl(CONTROLLERS(controller), enable, OPTIONS),
                        UHPPOTE.SetPCControl(controller, enable, OPTIONS))

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

    Sub SetInterlock(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim interlock As Interlock = ArgParse.Parse(args, "--interlock", Interlock.None)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetInterlock(CONTROLLERS(controller), interlock, OPTIONS),
                        UHPPOTE.SetInterlock(controller, interlock, OPTIONS))

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

    Sub ActivateKeypads(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim keypads = ArgParse.Parse(args, "--keypads", new Byte() {})

        Dim reader1 = keypads.Contains(1)
        Dim reader2 = keypads.Contains(2)
        Dim reader3 = keypads.Contains(3)
        Dim reader4 = keypads.Contains(4)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.ActivateKeypads(CONTROLLERS(controller), reader1, reader2, reader3, reader4, OPTIONS),
                        UHPPOTE.ActivateKeypads(controller, reader1, reader2, reader3, reader4, OPTIONS))

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

    Sub GetAntiPassback(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.GetAntiPassback(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.GetAntiPassback(controller, OPTIONS))

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

    Sub SetAntiPassback(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim antipassback As AntiPassback = ArgParse.Parse(args, "--antipassback", AntiPassback.Disabled)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.SetAntiPassback(CONTROLLERS(controller), antipassback, OPTIONS),
                        UHPPOTE.SetAntiPassback(controller, antipassback, OPTIONS))

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

    Sub RestoreDefaultParameters(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)

        Dim result = If(CONTROLLERS.ContainsKey(controller),
                        UHPPOTE.RestoreDefaultParameters(CONTROLLERS(controller), OPTIONS),
                        UHPPOTE.RestoreDefaultParameters(controller, OPTIONS))

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
