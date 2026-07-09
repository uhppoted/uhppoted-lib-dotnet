Imports System.Console
Imports System.Linq
Imports System.Net

Imports Options = uhppoted.Options
Imports OptionsBuilder = uhppoted.OptionsBuilder

Module Program
    Sub Main(args As String())
        WriteLine("**uhppoted-Lib-dotnet VB.NET CLI v0.9.0")
        WriteLine()

        Dim builder = New OptionsBuilder().
                          WithBind(New IPEndPoint(IPAddress.Any, 0)).
                          WithBroadcast(New IPEndPoint(IPAddress.Broadcast, 60000)).
                          WithTimeout(1000).
                          WithDebug(False)

        args = Parse(args, builder)
        Dim options = builder.Build()

        If args.Length > 0
            Dim cmd = args(0)
            Dim slice = args.Skip(1).ToArray()

            For Each command In Commands.commands
                If command.command = cmd Then
                    Try
                        command.f(options, slice)
                    Catch Err As Exception
                        WriteLine("  ** ERROR  {0}", err.Message)
                    End Try

                    Environment.Exit(0)
                End If
            Next
        End If

        Usage()
        Environment.Exit(1)
    End Sub

    Sub Usage()
        WriteLine("Usage: dotnet run <command>")
        WriteLine()
        WriteLine("  Supported commands:")

        For Each command In Commands.commands
            WriteLine("  - {0,-26}  {1}", command.command, command.description)
        Next

        WriteLine()
    End Sub


    Function Parse(args As String(), ByRef builder As OptionsBuilder) As String()
        Dim arg as String
        Dim ix = 0

        While ix < args.Length
            arg = args(ix)

            Select Case arg
                Case "--bind"
                    ix = ix + 1
                    If ix < args.Length Then
                        arg = args(ix)
                        ix = ix + 1
                        builder.WithBind(IPEndPoint.Parse(arg))
                    End If

                Case "--broadcast"
                    ix = ix + 1
                    If ix < args.Length Then
                        arg = args(ix)
                        ix = ix + 1
                        builder.WithBroadcast(IPEndPoint.Parse(arg))
                    End If

                Case "--listen"
                    ix = ix + 1
                    If ix < args.Length Then
                        arg = args(ix)
                        ix = ix + 1
                        builder.WithListen(IPEndPoint.Parse(arg))
                    End If

                Case "--timeout"
                    ix = ix + 1
                    If ix < args.Length Then
                        arg = args(ix)
                        ix = ix + 1
                        builder.WithTimeout(CInt(1000 * CDbl(Val(arg))))
                    End If

                Case "--debug"
                    ix = ix + 1
                    builder.WithDebug(True)

                Case Else
                    Exit While
            End Select
        End While

        Return args.Skip(ix).ToArray()
    End Function

End Module


