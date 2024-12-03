Imports System.Console
Imports System.Net
Imports DoorMode = uhppoted.DoorMode

Module ArgParse

    Function Parse(Of T)(args As String(), arg As String, defval As T) As T
        Dim ix As Integer = Array.IndexOf(args, arg)
        Dim u32 As UInteger
        Dim u8 As Byte
        Dim bool As Boolean
        Dim address As IPAddress
        Dim endpoint As IPEndPoint
        Dim dt As DateTime

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

                Case "Boolean"
                    If Boolean.TryParse(args(ix), bool) Then
                        Return CType(CObj(bool), T)
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

                Case "DateTime"
                    If DateTime.TryParse(args(ix), dt) Then
                        Return CType(CObj(dt), T)
                    End If

                Case "DoorMode" :
                    Select Case args(ix).ToLowerInvariant()
                        Case "normally-open"
                            Return CType(CObj(DoorMode.NormallyOpen), T)

                        Case "normally-closed"
                            Return CType(CObj(DoorMode.NormallyClosed), T)

                        Case "controlled"
                            Return CType(CObj(DoorMode.Controlled), T)
                    End Select

                Case "UInteger()"
                    Dim parsed As New List(Of UInteger)()
                    For Each token In args(ix).Split(","c)
                        If Uint32.TryParse(token.Trim(), u32) Then
                            parsed.Add(u32)
                        End If
                    Next

                    Return CType(CObj(parsed.ToArray()), T)

            End Select
        End If

        return defval
    End Function

End Module