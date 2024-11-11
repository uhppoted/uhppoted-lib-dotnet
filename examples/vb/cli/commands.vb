Imports System.Console
Imports System.Net

Imports uhppoted
Imports uhppoted.Uhppoted

Public Structure Command
    Public ReadOnly command As String
    Public ReadOnly description As String

    Public Sub New(command As String, description As String)
        Me.command = command
        Me.description = description
    End Sub
End Structure

Module Commands
    Private Const TIMEOUT = 1000
    Private Dim OPTIONS = New uhppoted.OptionsBuilder().
                                       WithDebug(true).
                                       build()

    Public Dim commands As New List(Of Command) From {
           New Command("get-all-controllers", "Retrieves a list of controllers accessible on the local LAN"),
           New Command("get-controller", "Retrieves the controller information for a specific controller"),
           New Command("set-IPv4", "Sets the controller IPv4 address, netmask and gateway"),
           New Command("get-listener", "Retrieves the controller event listener address:port and auto-send interval"),
           New Command("set-listener", "Sets the controller event listener address:port and auto-send interval"),
           New Command("get-time", "Retrieves the controller system date and time"),
           New Command("set-time", "Sets the controller system date and time"),
           New Command("get-door", "Retrieves a controller door mode and delay settings"),
           New Command("set-door", "Sets a controller door mode and delay"),
           New Command("set-door-passcodes", "Sets the supervisor passcodes for a controller door"),
           New Command("open-door", "Unlocks a door controlled by a controller"),
           New Command("get-status", "Retrieves the current status of the controller")
       }

    Sub GetControllers(args As String())
        Try
            Dim result = get_all_controllers(TIMEOUT, OPTIONS)

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

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetController(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim result = get_controller(controller, TIMEOUT, OPTIONS)

            If (result.IsOk)
                Dim response = result.ResultValue

                WriteLine("get-controller")
                WriteLine("  controller {0}", response.controller)
                WriteLine("    address  {0}", response.address)
                WriteLine("    netmask  {0}", response.netmask)
                WriteLine("    gateway  {0}", response.gateway)
                WriteLine("    MAC      {0}", response.MAC)
                WriteLine("    version  {0}", response.version)
                WriteLine("    date     {0}", YYYYMMDD(response.date))
                WriteLine()
            Else If (result.IsError)
                Throw New Exception(result.ErrorValue)
            End If

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub SetIPv4(args As String())
        Try
            Dim controller = New ControllerBuilder(405419896).
                                 With(IPEndPoint.Parse("192.168.1.100:60000")).
                                 With("udp").build()

            Dim address = IPAddress.Parse("192.168.1.100")
            Dim netmask = IPAddress.Parse("255.255.255.0")
            Dim gateway = IPAddress.Parse("192.168.1.1")
            Dim result = set_IPv4(controller, address, netmask, gateway, TIMEOUT, OPTIONS)

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

            Dim result = get_listener(controller, TIMEOUT, OPTIONS)

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
            Dim result = set_listener(controller, endpoint, interval, TIMEOUT, OPTIONS)

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

            Dim result = get_time(controller, TIMEOUT, OPTIONS)

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

            Dim result = set_time(controller, now, TIMEOUT, OPTIONS)

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
            Dim result = get_door(controller, door, TIMEOUT, OPTIONS)

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
            Dim result = set_door(controller, door, mode, delay, TIMEOUT, OPTIONS)

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
            Dim result = set_door_passcodes(controller, door, passcodes(0), passcodes(1), passcodes(2), passcodes(3), TIMEOUT, OPTIONS)

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
            Dim result = open_door(controller, door, TIMEOUT, OPTIONS)

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

            Dim result = get_status(controller, TIMEOUT, OPTIONS)

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
