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

                Case "set-door"
                    Commands.SetDoor(slice)
                    Environment.Exit(0)

                Case "set-door-passcodes"
                    Commands.SetDoorPasscodes(slice)
                    Environment.Exit(0)

                Case "open-door"
                    Commands.OpenDoor(slice)
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

        For Each command In Commands.commands
            WriteLine("  - {0,-19}  {1}", command.command, command.description)
        Next

        WriteLine()
    End Sub
End Module


