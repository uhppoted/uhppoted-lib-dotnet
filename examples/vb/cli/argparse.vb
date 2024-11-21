Imports System.Console
Imports System.Net

Module ArgParse

    Function Parse(Of T)(args As String(), arg As String, defval As T) As T
        Dim ix As Integer = Array.IndexOf(args, arg)
        Dim u32 As UInteger
        Dim u8 As Byte
        Dim address As IPAddress
        Dim endpoint As IPEndPoint

        If ix >= 0 AndAlso ix + 1 < args.Length Then
            ix += 1
            Select Case TypeName(defval)
                Case "UInteger"
                    If UInt32.TryParse(args(ix), u32) Then
                        Return CType(CObj(u32), T)
                    End If

                Case "Byte"
                    If Byte.TryParse(args(ix), u8) Then
                        Return CType(CObj(u8), T)
                    End If

                Case "IPAddress"
#Disable Warning BC42030
                    If IPAddress.TryParse(args(ix), address) Then
                        Return CType(CObj(address), T)
                    End If
#Enable Warning BC42030

                Case "IPEndPoint"
#Disable Warning BC42030
                    If IPEndPoint.TryParse(args(ix), endpoint) Then
                        Return CType(CObj(endpoint), T)
                    End If
#Enable Warning BC42030

            End Select
        End If

        return defval
    End Function

End Module