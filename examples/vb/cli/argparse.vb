Imports System.Console

Module ArgParse

    Function Parse(Of T)(args As String(), arg As String, defval As T) As T
        Dim ix As Integer = Array.IndexOf(args, arg)

        If ix >= 0 AndAlso ix + 1 < args.Length Then
            ix += 1
            Select Case TypeName(defval)
                Case "UInteger"
                    Dim v As UInteger
                    If UInt32.TryParse(args(ix), v) Then
                        Return CType(CObj(v), T)
                    End If
            End Select
        End If

        return defval
    End Function

End Module