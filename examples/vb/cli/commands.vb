Imports System.Console
Imports Microsoft.FSharp.Collections
Imports uhppoted.Uhppoted

Module Commands
    Sub GetControllers()
        Try
            Dim controllers As FSharpList(Of uhppoted.GetControllerResponse) = get_controllers()

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
            WriteLine("StackTrace {0}", err.StackTrace)
        End Try
    End Sub

End Module
