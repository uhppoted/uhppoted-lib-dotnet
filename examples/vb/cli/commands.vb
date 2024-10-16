Imports System.Console

Imports uhppoted.Uhppoted

Module Commands
    Sub GetControllers()
        Try
            get_controllers()
        Catch Err As Exception
            WriteLine("Exception caught: " + err.Message)
            WriteLine("StackTrace: " + err.StackTrace)
        End Try
    End Sub

End Module
