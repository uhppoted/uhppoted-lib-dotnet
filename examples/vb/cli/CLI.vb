Imports System.Console


Module Program
    Sub Main(args As String())
        WriteLine("**uhppoted-Lib-dotnet VB.NET CLI v0.8.10")
        WriteLIne()

        If args.Length > 0
            Select args(0)
                Case "get-all-controllers"
                    Commands.GetControllers()
                    Environment.Exit(0)

                Case "get-controller"
                    Commands.GetController()
                    Environment.Exit(0)

                case "set-IPv4" :
                    Commands.SetIPv4()
                    Environment.Exit(0)

                Case Else
                    WriteLine("** ERROR invalid command {0}", args(0))
                    WriteLine()

            End Select
        End If

        Usage()
        Environment.Exit(1)
    End Sub

    Sub Usage()
        WriteLine("Usage: dotnet run <command>")
        WriteLine()
        WriteLine("  Supported commands:")
        WriteLine("  - get-all-controllers  Retrieves a list of controllers accessible on the local LAN")
        WriteLine("  - get-controller       Retrieves the controller information for a specific controller")
        WriteLine("  - set-IPv4             Sets the controller IPv4 address, netmask and gateway")
        WriteLine()
    End Sub
End Module


