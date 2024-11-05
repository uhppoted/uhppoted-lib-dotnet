Imports System.Console
Imports System.Linq

Module Program
    Sub Main(args As String())
        WriteLine("**uhppoted-Lib-dotnet VB.NET CLI v0.8.10")
        WriteLine()

        If args.Length > 0
            Dim cmd = args(0)
            Dim slice = args.Skip(1).ToArray()

            Select cmd
                Case "get-all-controllers"
                    Commands.GetControllers(slice)
                    Environment.Exit(0)

                Case "get-controller"
                    Commands.GetController(slice)
                    Environment.Exit(0)

                case "set-IPv4" :
                    Commands.SetIPv4(slice)
                    Environment.Exit(0)

                Case "get-listener"
                    Commands.GetListener(slice)
                    Environment.Exit(0)

                Case "set-listener"
                    Commands.SetListener(slice)
                    Environment.Exit(0)

                Case "get-time"
                    Commands.GetTime(slice)
                    Environment.Exit(0)

                Case "set-time"
                    Commands.SetTime(slice)
                    Environment.Exit(0)

                Case "get-door"
                    Commands.GetDoor(slice)
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
        WriteLine("  - get-listener         Retrieves the controller event listener address:port and auto-send interval")
        WriteLine("  - set-listener         Sets the controller event listener address:port and auto-send interval")
        WriteLine("  - get-time             Retrieves the controller system date and time")
        WriteLine("  - set-time             Sets the controller system date and time")
        WriteLine("  - get-door             Retrieves a controller door mode and delay settings")
        WriteLine()
    End Sub
End Module


