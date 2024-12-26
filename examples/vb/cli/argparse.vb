Imports System.Console
Imports System.Net
Imports DoorMode = uhppoted.DoorMode
Imports Interlock = uhppoted.Interlock
Imports TaskCode = uhppoted.TaskCode

Module ArgParse

    Function Parse(Of T)(args As String(), arg As String, defval As T) As T
        Dim ix As Integer = Array.IndexOf(args, arg)
        Dim u32 As UInteger
        Dim u8 As Byte
        Dim bool As Boolean
        Dim address As IPAddress
        Dim endpoint As IPEndPoint
        Dim adatetime As DateTime
        Dim adate As DateOnly
        Dim passcodes As New List(Of UInteger)()
        Dim keypads As New List(Of Byte)()
        Dim permissions As New Dictionary(Of Integer, Byte)()

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
                    If DateTime.TryParse(args(ix), adatetime) Then
                        Return CType(CObj(adatetime), T)
                    End If

                Case "DateOnly"
                    If DateOnly.TryParse(args(ix), adate) Then
                        Return CType(CObj(adate), T)
                    End If

                Case "DoorMode" :
                    Select Case args(ix).ToLowerInvariant()
                        Case "normally-open"
                            Return CType(CObj(DoorMode.NormallyOpen), T)

                        Case "normally-closed"
                            Return CType(CObj(DoorMode.NormallyClosed), T)

                        Case "controlled"
                            Return CType(CObj(DoorMode.Controlled), T)

                        Case Else
                            Return defval
                    End Select

                Case "Interlock" :
                    Select Case args(ix).ToLowerInvariant()
                        Case "none"
                            Return CType(CObj(Interlock.None), T)

                        Case "1&2"
                            Return CType(CObj(Interlock.Doors12), T)

                        Case "3&4"
                            Return CType(CObj(Interlock.Doors34), T)

                        Case "1&2,3&4"
                            Return CType(CObj(Interlock.Doors12And34), T)

                        Case "1&2&3"
                            Return CType(CObj(Interlock.Doors123), T)

                        Case "1&2&3&4"
                            Return CType(CObj(Interlock.Doors1234), T)

                        Case Else
                            Return defval
                    End Select

                case "TaskCode" :
                    Select Case args(ix).ToLowerInvariant()
                        Case "control door"
                            return CType(CObj(TaskCode.ControlDoor), T)

                        Case "unlock door"
                            return CType(CObj(TaskCode.UnlockDoor), T)

                        Case "lock door"
                            return CType(CObj(TaskCode.LockDoor), T)

                        Case "disable time profiles"
                            return CType(CObj(TaskCode.DisableTimeProfiles), T)

                        Case "enable time profiles"
                            return CType(CObj(TaskCode.EnableTimeProfiles), T)

                        Case "enable card + no PIN"
                            return CType(CObj(TaskCode.EnableCardNoPIN), T)

                        Case "enable card + IN PIN"
                            return CType(CObj(TaskCode.EnableCardInPIN), T)

                        Case "enable card + IN/OUT PIN"
                            return CType(CObj(TaskCode.EnableCardInOutPIN), T)

                        Case "enable more cards"
                            return CType(CObj(TaskCode.EnableMoreCards), T)

                        Case "disable more cards"
                            return CType(CObj(TaskCode.DisableMoreCards), T)

                        Case "trigger once"
                            return CType(CObj(TaskCode.TriggerOnce), T)

                        Case "disable pushbutton"
                            return CType(CObj(TaskCode.DisablePushbutton), T)

                        Case "enable pushbutton"
                            return CType(CObj(TaskCode.EnablePushbutton), T)
                    End Select

                Case "UInteger()"
                    For Each token In args(ix).Split(","c)
                        If Uint32.TryParse(token.Trim(), u32) Then
                            passcodes.Add(u32)
                        End If
                    Next
                    Return CType(CObj(passcodes.ToArray()), T)

                Case "Byte()"
                    For Each token In args(ix).Split(","c)
                        If Byte.TryParse(token.Trim(), u8) Then
                            keypads.Add(u8)
                        End If
                    Next
                    Return CType(CObj(keypads.ToArray()), T)

                Case "Dictionary(Of Integer,Byte)"
                    For Each token In args(ix).Split(","c)
                        Dim permission = token.Split(":"c)
                        Dim door As Integer
                        Dim profile As Byte

                        If permission.Length > 0 Then
                            If Int32.TryParse(permission(0).Trim(), door) Then
                                If permission.Length > 1 Then
                                    If Byte.TryParse(permission(1).Trim(), profile) Then
                                        permissions(door) = profile
                                    End If
                                Else
                                    permissions(door) = 1
                                End If
                            End If
                        End If
                    Next
                    Return CType(CObj(permissions), T)

            End Select
        End If

        return defval
    End Function

End Module