Imports Microsoft.FSharp.Core
Imports Microsoft.FSharp.Collections

Imports System.Console
Imports System.Net

Imports uhppoted.Uhppoted

Module Commands
    Private Const TIMEOUT = 1000
    Private Const DEBUG = True

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

    Sub GetControllers(args As String())
        Try
            Dim controllers As FSharpList(Of uhppoted.GetControllerResponse) = get_all_controllers(TIMEOUT, DEBUG)

            WriteLine("get-controllers: {0}", controllers.Length)
            For Each controller In controllers
                WriteLine("  controller {0}", controller.controller)
                WriteLine("    address  {0}", controller.address)
                WriteLine("    netmask  {0}", controller.netmask)
                WriteLine("    gateway  {0}", controller.gateway)
                WriteLine("    MAC      {0}", controller.MAC)
                WriteLine("    version  {0}", controller.version)
                WriteLine("    date     {0}", controller.date)
                WriteLine()
            Next

        Catch Err As Exception
            WriteLine("Exception  {0}", err.Message)
        End Try
    End Sub

    Sub GetController(args As String())
        Try
            Dim addr = IPEndPoint.Parse("192.168.1.100:60000")
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, addr, protocol)
            Dim result = get_controller(controller, TIMEOUT, DEBUG)

            If (result.IsOk)
                Dim response = result.ResultValue
                WriteLine("get-controller")
                WriteLine("  controller {0}", response.controller)
                WriteLine("    address  {0}", response.address)
                WriteLine("    netmask  {0}", response.netmask)
                WriteLine("    gateway  {0}", response.gateway)
                WriteLine("    MAC      {0}", response.MAC)
                WriteLine("    version  {0}", response.version)
                WriteLine("    date     {0}", response.date)
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
            Dim addr = IPEndPoint.Parse("192.168.1.100:60000")
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, addr, protocol)
            Dim address = IPAddress.Parse("192.168.1.100")
            Dim netmask = IPAddress.Parse("255.255.255.0")
            Dim gateway = IPAddress.Parse("192.168.1.1")
            Dim result = set_IPv4(controller, address, netmask, gateway, TIMEOUT, DEBUG)

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
            Dim addr = IPEndPoint.Parse("192.168.1.100:60000")
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, addr, protocol)
            Dim result = get_listener(controller, TIMEOUT, DEBUG)

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
            Dim addr = IPEndPoint.Parse("192.168.1.100:60000")
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, addr, protocol)
            Dim endpoint = IPEndPoint.Parse("192.168.1.100:60001")
            Dim interval = 30
            Dim result = set_listener(controller, endpoint, interval, TIMEOUT, DEBUG)

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
            Dim addr = IPEndPoint.Parse("192.168.1.100:60000")
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, addr, protocol)
            Dim result = get_time(controller, TIMEOUT, DEBUG)

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

End Module
