Imports Microsoft.FSharp.Core
Imports Microsoft.FSharp.Collections

Imports System.Console
Imports System.Net

Imports uhppoted.Uhppoted

Module Commands
    Private Const TIMEOUT = 1000
    Private Const DEBUG = True

    Sub GetControllers()
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

    Sub GetController()
        Try
            Dim address = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000)
            Dim protocol = FSharpOption(Of String).None
            Dim controller = new uhppoted.Controller(405419896, address, protocol)
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
            WriteLine("StackTrace {0}", err.StackTrace)
        End Try
    End Sub

End Module
