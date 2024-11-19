Imports System.Console
Imports System.Net

Imports UHPPOTE = uhppoted.Uhppoted
Imports ControllerBuilder = uhppoted.ControllerBuilder
Imports OptionsBuilder = uhppoted.OptionsBuilder

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
    Private Const CARD_NUMBER As UInt32 = 10058400
    Private Const CARD_INDEX As UInt32 = 1
    Private Const TIMEOUT = 1000

    Private Dim OPTIONS = New OptionsBuilder().
                                WithDestination(IPEndPoint.Parse("192.168.1.100:60000")).
                                WithProtocol("udp").
                                WithDebug(true).
                                build()

    Public Dim commands As New List(Of Command) From {
           New Command("find-controllers", "Retrieves a list of controllers accessible on the local LAN", AddressOf FindControllers),
           New Command("get-controller", "Retrieves the controller information for a specific controller", AddressOf GetController),
           New Command("set-IPv4", "Sets the controller IPv4 address, netmask and gateway", AddressOf SetIPv4),
           New Command("get-listener", "Retrieves the controller event listener address:port and auto-send interval", AddressOf GetListener),
           New Command("set-listener", "Sets the controller event listener address:port and auto-send interval", AddressOf SetListener),
           New Command("get-time", "Retrieves the controller system date and time", AddressOf GetTime),
           New Command("set-time", "Sets the controller system date and time", AddressOf SetTime),
           New Command("get-door", "Retrieves a controller door mode and delay settings", AddressOf GetDoor),
           New Command("set-door", "Sets a controller door mode and delay", AddressOf SetDoor),
           New Command("set-door-passcodes", "Sets the supervisor passcodes for a controller door", AddressOf SetDoorPasscodes),
           New Command("open-door", "Unlocks a door controlled by a controller", AddressOf OpenDoor),
           New Command("get-status", "Retrieves the current status of the controller", AddressOf GetStatus),
           New Command("get-cards", "Retrieves the number of cards stored on the controller", AddressOf GetCards),
           New Command("get-card", "Retrieves a card record from the controller", AddressOf GetCard),
           New Command("get-card-at-index", "Retrieves the card record stored at the index from the controller", AddressOf GetCardAtIndex),
           New Command("put-card", "Adds or updates a card record on controller", AddressOf PutCard),
           New Command("delete-card", "Deletes a card record from a controller", AddressOf DeleteCard),
           New Command("delete-all-cards", "Deletes all card records from a controller", AddressOf DeleteAllCards)
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
        Dim card = CARD_NUMBER

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
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim address = IPAddress.Parse("192.168.1.100")
            Dim netmask = IPAddress.Parse("255.255.255.0")
            Dim gateway = IPAddress.Parse("192.168.1.1")
            Dim result = UHPPOTE.set_IPv4(controller, address, netmask, gateway, TIMEOUT, OPTIONS)

            If (result.IsOk)
                WriteLine("set-IPv4")
                WriteLine("  ok")
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetListener(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim result = UHPPOTE.get_listener(controller, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-listener")
                WriteLine("  controller {0}", response.controller)
                WriteLine("    endpoint {0}", response.endpoint)
                WriteLine("    interval {0}s", response.interval)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub SetListener(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim endpoint = IPEndPoint.Parse("192.168.1.100:60001")
            Dim interval = 30
            Dim result = UHPPOTE.set_listener(controller, endpoint, interval, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("set-listener")
                WriteLine("  controller {0}", response.controller)
                WriteLine("          ok {0}", response.ok)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetTime(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim result = UHPPOTE.get_time(controller, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-time")
                WriteLine("  controller {0}", response.controller)
                WriteLine("    datetime {0}", YYYYMMDDHHmmss(response.datetime))
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub SetTime(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()
            Dim now = DateTime.Now
            Dim result = UHPPOTE.set_time(controller, now, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("set-time")
                WriteLine("  controller {0}", response.controller)
                WriteLine("    datetime {0}", YYYYMMDDHHmmss(response.datetime))
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetDoor(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()
            Dim door = 4
            Dim result = UHPPOTE.get_door(controller, door, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-door")
                WriteLine("  controller {0}", response.controller)
                WriteLine("        door {0}", response.door)
                WriteLine("        mode {0}", response.mode)
                WriteLine("       delay {0}s", response.delay)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub SetDoor(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()
            Dim door = 4
            Dim mode = 2
            Dim delay = 7
            Dim result = UHPPOTE.set_door(controller, door, mode, delay, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("set-door")
                WriteLine("  controller {0}", response.controller)
                WriteLine("        door {0}", response.door)
                WriteLine("        mode {0}", response.mode)
                WriteLine("       delay {0}s", response.delay)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub SetDoorPasscodes(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()
            Dim door = 4
            Dim passcodes As UInteger() = {12345, 54321, 0, 999999}
            Dim result = UHPPOTE.set_door_passcodes(controller, door, passcodes(0), passcodes(1), passcodes(2), passcodes(3), TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("set-door-passcodes")
                WriteLine("  controller {0}", response.controller)
                WriteLine("          ok {0}", response.ok)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub OpenDoor(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()
            Dim door = 4
            Dim result = UHPPOTE.open_door(controller, door, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("open-door")
                WriteLine("  controller {0}", response.controller)
                WriteLine("          ok {0}", response.ok)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
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
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim result = UHPPOTE.get_cards(controller, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-cards")
                WriteLine("  controller {0}", response.controller)
                WriteLine("       cards {0}", response.cards)
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetCard(args As String())
        Dim controller = ArgParse.Parse(args, "--controller", CONTROLLER_ID)
        Dim card = 10058400
        Dim result = UHPPOTE.GetCard(controller, card, TIMEOUT, OPTIONS)

        If (result.IsOk And result.ResultValue.HasValue)
            Dim record = result.ResultValue.Value

            WriteLine("get-card")
            WriteLine("  controller {0}", controller)
            WriteLine("        card {0}", record.card)
            WriteLine("  start date {0}", (YYYYMMDD(record.startdate)))
            WriteLine("    end date {0}", (YYYYMMDD(record.enddate)))
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
            WriteLine("  start date {0}", (YYYYMMDD(record.startdate)))
            WriteLine("    end date {0}", (YYYYMMDD(record.enddate)))
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
        Dim card = CARD_NUMBER
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
        Dim card = CARD_NUMBER

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

End Module
