Imports System.Console


Module Program
    Sub Main(args As String())
        WriteLine("**uhppoted-Lib-dotnet VB.NET CLI v0.8.10")
        WriteLIne()

        For Each arg In args
            If arg.Equals("get-controllers")
                Commands.GetControllers()
                Environment.Exit(0)
            End If
        Next

        Usage()
        Environment.Exit(1)
    End Sub

    Sub Usage()
        WriteLine("Usage: dotnet run <command>")
        WriteLine()
        WriteLine("  Supported commands:")
        WriteLine("  - get-controllers  Retrieves a list of controllers accessible on the local LAN")
        WriteLine()
    End Sub
End Module


