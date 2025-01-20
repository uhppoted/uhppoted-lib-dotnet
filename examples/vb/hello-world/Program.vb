Imports System
Imports System.Net

Imports UHPPOTE = uhppoted.Uhppoted
Imports OptionsBuilder = uhppoted.OptionsBuilder

Module Program
    Private ReadOnly Dim OPTIONS = New OptionsBuilder().
                                       WithBind(New IPEndPoint(IPAddress.Any, 0)).
                                       WithBroadcast(New IPEndPoint(IPAddress.Broadcast, 60000)).
                                       WithTimeout(1000).
                                       WithDebug(True).
                                       Build()

    Sub Main(args As String())
        Console.WriteLine("uhppoted-lib-dotnet")

        Dim result = UHPPOTE.FindControllers(OPTIONS)

        If (result.IsOk)
            Dim controllers = result.ResultValue

            Console.WriteLine("get-controllers: {0}", controllers.Length)
            For Each controller In controllers
                Console.WriteLine("  controller {0}", controller.Controller)
                Console.WriteLine("    address  {0}", controller.Address)
                Console.WriteLine("    netmask  {0}", controller.Netmask)
                Console.WriteLine("    gateway  {0}", controller.Gateway)
                Console.WriteLine("    MAC      {0}", controller.MAC)
                Console.WriteLine("    version  {0}", controller.Version)
                Console.WriteLine("    date     {0}", controller.Date)
                Console.WriteLine("")
            Next
        Else If (result.IsError)
            Throw New Exception(UHPPOTE.Translate(result.ErrorValue))
        End If

    End Sub

End Module
