Imports System.Security.Cryptography.Pkcs

Module modMain


    Public Function FullDisplayElapsed(ByVal TotalSeconds As Long) As String
        Dim piDays As Integer
        Dim piHours As Integer
        Dim piMins As Integer
        Dim piSeconds As Long
        Dim piCurr As Long
        Dim psTemp As String

        piCurr = TotalSeconds
        piDays = piCurr \ 86400 ' (60 seconds * 60 mins in an hour times 24 hours in a day)
        piCurr = piCurr - (piDays * 86400)
        piHours = piCurr \ 3600 ' 3600 seconds in an hour
        piCurr = piCurr - (piHours * 3600)
        piMins = piCurr \ 60    ' 60 seconds in a minute
        piCurr = piCurr - (piMins * 60)
        piSeconds = piCurr

        psTemp = ""
        If piDays > 0 Then
            If piHours > 0 Or piMins > 0 Then
                psTemp = psTemp & IIf(piDays = 1, "1 day, ", Format(piDays, "#,##0") & " days, ")
            Else
                psTemp = psTemp & IIf(piDays = 1, "1 day", Format(piDays, "#,##0") & " days")
            End If
        End If
        If piHours > 0 Then
            If piMins > 0 Or piSeconds > 0 Then
                If piSeconds > 0 Then
                    psTemp = psTemp & Format(piHours, "#0") & IIf(piHours = 1, " hour, ", " hours, ")
                Else
                    psTemp = psTemp & Format(piHours, "#0") & IIf(piHours = 1, " hour and ", " hours and ")
                End If
            Else
                psTemp = psTemp & Format(piHours, "#0") & IIf(piHours = 1, " hour", " hours")
            End If
        End If
        If piMins > 0 Then
            If piSeconds > 0 Then
                psTemp = psTemp & Format(piMins, "#0") & IIf(piMins = 1, " minute and ", " minutes and ")
            Else
                psTemp = psTemp & Format(piMins, "#0") & IIf(piMins = 1, " minute", " minutes")
            End If
        End If
        If piSeconds > 0 Then
            psTemp = psTemp & Format(piSeconds, "#0") & IIf(piSeconds = 1, " second", " seconds")
        Else
            If psTemp = "" Then
                psTemp = "<1 Second"
            End If
        End If
        Return psTemp
    End Function


End Module
