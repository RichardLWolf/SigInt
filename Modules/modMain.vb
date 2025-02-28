Imports System.Security.Cryptography.Pkcs
Imports System.Net.Http
Imports System.Text


Module modMain

    Function FormatHertz(ByVal piFrequencyHz As UInteger) As String
        Select Case piFrequencyHz
            Case Is >= 1_000_000_000UI
                Dim pdFreq As Double = piFrequencyHz / 1000000000UI
                Return String.Format("{0:G6} GHz", pdFreq)

            Case Is >= 1_000_000UI
                Dim pdFreq As Double = piFrequencyHz / 1000000UI
                Return String.Format("{0:G6} MHz", pdFreq)

            Case Is >= 1_000UI
                Dim pdFreq As Double = piFrequencyHz / 1000UI
                Return String.Format("{0:G6} kHz", pdFreq)

            Case Else
                Return piFrequencyHz.ToString("0") & " Hz"
        End Select
    End Function

    Public Function FormatMSPS(ByVal iSampleRate As UInteger) As String
        If iSampleRate >= 1000000 Then
            Return $"{iSampleRate / 1000000.0:G4} MSPS"
        ElseIf iSampleRate >= 1000 Then
            Return $"{iSampleRate / 1000.0:G4} kSPS"
        Else
            Return $"{iSampleRate} SPS"
        End If
    End Function

    Public Function FormatBytes(ByVal plBytes As Long) As String
        Dim psSuffixes() As String = {"B", "KB", "MB", "GB"}
        Dim pdSize As Double = plBytes
        Dim piIndex As Integer = 0
        While pdSize >= 1024 AndAlso piIndex < psSuffixes.Length - 1
            pdSize /= 1024
            piIndex += 1
        End While
        Return $"{pdSize:0.##} {psSuffixes(piIndex)}"
    End Function

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


    Public Function GetClosestIndex(ByVal piValue As Integer, ByVal poList As List(Of Integer)) As Integer
        Dim iIndex As Integer = poList.BinarySearch(piValue)

        ' If exact match found, return its index
        If iIndex >= 0 Then Return iIndex

        ' If not found, BinarySearch returns the bitwise complement of the next larger element
        iIndex = Not iIndex

        ' If index is 0, all elements are larger than piValue (nothing before it)
        If iIndex = 0 Then Return 0 ' No valid index (nothing <= piValue) just give the first entry

        ' Otherwise, return the closest smaller index
        Return iIndex - 1
    End Function




    Public Async Function SendDiscordNotification(ByVal sMessage As String, ByVal sWebhookURL As String, ByVal sMentionID As String) As Task
        If String.IsNullOrWhiteSpace(sWebhookURL) Then
            clsLogger.Log("modMain.SendDiscordNotification", "Discord Webhook URL is not set, cannot send notification.")
            Exit Function
        End If

        ' Add mention if provided
        If Not String.IsNullOrWhiteSpace(sMentionID) Then
            sMessage = sMentionID & " " & sMessage
        End If

        Dim oHttpClient As New HttpClient()
        Dim sJson As String = "{""content"":""" & sMessage & """}"
        Dim oContent As New StringContent(sJson, Encoding.UTF8, "application/json")

        Try
            Dim oResponse As HttpResponseMessage = Await oHttpClient.PostAsync(sWebhookURL, oContent)
            If oResponse.StatusCode = Net.HttpStatusCode.NoContent Then
                clsLogger.Log("modMain.SendDiscordNotification", $"✔ - Sent Discord message to Mention ID.")
            Else
                clsLogger.Log("modMain.SendDiscordNotification", $"❌ - FAILED to send Discord message, server response: {oResponse.StatusCode} - {oResponse.StatusCode.ToString}.")
            End If
        Catch ex As Exception
            clsLogger.LogException("modMain.SendDiscordNotification", ex)
        End Try
    End Function

End Module
