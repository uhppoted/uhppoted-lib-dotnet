Imports System.Console
Imports System.Linq

Module Program
    Sub Main(args As String())
        WriteLine("**uhppoted-Lib-dotnet VB.NET CLI v0.8.10")
        WriteLine()

        If args.Length > 0
            Dim cmd = args(0)
            Dim slice = args.Skip(1).ToArray()

            For Each command In Commands.commands
                If command.command = cmd Then
                    Try
                        command.f(slice)
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
            WriteLine("  - {0,-21}  {1}", command.command, command.description)
        Next

        WriteLine()
    End Sub
End Module


