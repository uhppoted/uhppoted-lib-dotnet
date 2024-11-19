Imports System.Console
Imports System.Net

Module ArgParse

    Function Parse(Of T)(args As String(), arg As String, defval As T) As T
        Dim ix As Integer = Array.IndexOf(args, arg)
        Dim u32 As UInteger
        Dim addr As IPAddress

        If ix >= 0 AndAlso ix + 1 < args.Length Then
            ix += 1
            Select Case TypeName(defval)
                Case "UInteger"
                    If UInt32.TryParse(args(ix), u32) Then
                        Return CType(CObj(u32), T)
                    End If

                Case "IPAddress"
#Disable Warning BC42030
                    If IPAddress.TryParse(args(ix), addr) Then
                        Return CType(CObj(addr), T)
                    End If
#Enable Warning BC42030

            End Select
        End If

        return defval
    End Function

End Module