Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

Public Class frmAppConfig
    Private fbDiscordNotifications As Boolean
    Private fsDiscordServerWebhook As String
    Private fsDiscordMentionID As String
    Private fbThingSpeakEnabled As Boolean
    Private fdUserLat As Double
    Private fdUserLon As Double


    Public ReadOnly Property DiscordNotifications As Boolean
        Get
            Return fbDiscordNotifications
        End Get
    End Property

    Public ReadOnly Property DiscordServerWebhook As String
        Get
            Return fsDiscordServerWebhook
        End Get
    End Property

    Public ReadOnly Property DiscordMentionID As String
        Get
            Return fsDiscordMentionID
        End Get
    End Property

    Public ReadOnly Property ThingSpeakEabled As Boolean
        Get
            Return fbThingSpeakEnabled
        End Get
    End Property

    Public ReadOnly Property UserLatitude As Double
        Get
            Return fdUserLat
        End Get
    End Property

    Public ReadOnly Property UserLongitude As Double
        Get
            Return fdUserLon
        End Get
    End Property

    Public Sub ReadyForm(ByVal oAppConfig As clsAppConfig)
        fbDiscordNotifications = oAppConfig.DiscordNotifications
        fsDiscordServerWebhook = oAppConfig.DiscordServerWebhook
        fsDiscordMentionID = oAppConfig.DiscordMentionID
        fbThingSpeakEnabled = oAppConfig.ThingSpeakEnabled
        fdUserLat = oAppConfig.UserLat
        fdUserLon = oAppConfig.UserLon

        ' put values onto screen
        LoadUIControls()
    End Sub


    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'check values
        Dim pbError = False

        If chkDiscordNotify.Checked = True Then
            If String.IsNullOrEmpty(fsDiscordServerWebhook) OrElse IsValidDiscordWebhook(fsDiscordServerWebhook) = False Then
                MsgBox("You must enter a valid Discord server webhook when using Discord notifications.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Discord Webhook")
                pbError = True
            End If
            If String.IsNullOrEmpty(fsDiscordMentionID) = False Then
                If Not IsValidDiscordMentionID(fsDiscordMentionID) Then
                    MsgBox($"You must enter a valid Discord NUMERIC User or Role ID number.  For Example:{vbCrLf}User ID:   <@123456789012345678>{vbCrLf}Role ID:  <@&987654321098765432>", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Discord Webhook")
                    pbError = True
                End If
            End If
            fbDiscordNotifications = True
        Else
            fbDiscordNotifications = False
            fsDiscordMentionID = ""
            fsDiscordServerWebhook = ""
        End If

        Dim pdLat As Double = 0
        Dim pdLon As Double = 0
        If chkThingSpeak.Checked Then
            If txtLat.Text.Trim <> "" Then
                If IsValidLatitude(txtLat.Text.Trim) Then
                    Double.TryParse(txtLat.Text.Trim, pdLat)
                Else
                    MsgBox("Please enter a valid latitude in Decimal Degrees format, leave lat/lon blank to not report your location.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Latitude")
                    txtLat.Focus()
                    pbError = True
                End If
            End If
            If txtLon.Text.Trim <> "" Then
                If IsValidLongitude(txtLon.Text.Trim) Then
                    Double.TryParse(txtLon.Text.Trim, pdLon)
                Else
                    MsgBox("Please enter a valid longitude in Decimal Degrees format, leave lat/lon blank to not report your location.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Latitude")
                    txtLon.Focus()
                    pbError = True
                End If
            End If
            If Not pbError Then
                fbThingSpeakEnabled = True
                fdUserLat = pdLat
                fdUserLon = pdLon
            End If
        Else
            fbThingSpeakEnabled = False
            fdUserLon = 0D
            fdUserLat = 0D
        End If

        If Not pbError Then
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Async Sub btnLookupIP_Click(sender As Object, e As EventArgs) Handles btnLookupIP.Click
        Dim poCoords As (Double, Double) = Await GetLocationFromIPAsync()
        txtLat.Text = poCoords.Item1.ToString()
        txtLon.Text = poCoords.Item2.ToString()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(0)

        fbDiscordNotifications = False
        fsDiscordServerWebhook = ""
        fsDiscordMentionID = ""
        fbThingSpeakEnabled = False
        fdUserLat = 0D
        fdUserLon = 0D

        LoadUIControls()
    End Sub

    Private Sub btnTestDiscord_Click(sender As Object, e As EventArgs) Handles btnTestDiscord.Click
        Dim pbError = False
        If String.IsNullOrEmpty(fsDiscordServerWebhook) OrElse IsValidDiscordWebhook(fsDiscordServerWebhook) = False Then
            MsgBox("You must enter a valid Discord server webhook when using Discord notifications.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Discord Webhook")
            pbError = True
        End If
        If String.IsNullOrEmpty(fsDiscordMentionID) = False Then
            If Not IsValidDiscordMentionID(fsDiscordMentionID) Then
                MsgBox($"You must enter a valid Discord NUMERIC User or Role ID number.  For Example:{vbCrLf}User ID:   <@123456789012345678>{vbCrLf}Role ID:  <@&987654321098765432>", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Discord Webhook")
                pbError = True
            End If
        End If
        If Not pbError Then
            Call modMain.SendDiscordNotification("If you see this message your Discord Webhook is setup properly!", fsDiscordServerWebhook, fsDiscordMentionID)
            MsgBox("Test message sent to Discord Webhook!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Test Sent")
        End If
    End Sub


    Private Sub chkDiscordNotify_CheckedChanged(sender As Object, e As EventArgs) Handles chkDiscordNotify.CheckedChanged
        fbDiscordNotifications = chkDiscordNotify.Checked
        If fbDiscordNotifications Then
            If fsDiscordServerWebhook = "" Then
                txtDiscordServer.Text = "https://discord.com/api/webhooks/"
                txtDiscordServer.SelectionStart = txtDiscordServer.Text.Length
                txtDiscordMention.Text = ""
            Else
                txtDiscordServer.Text = fsDiscordServerWebhook
                txtDiscordMention.Text = fsDiscordMentionID
            End If
            panDiscordVals.Visible = True
            txtDiscordServer.Focus()
        Else
            panDiscordVals.Visible = False
        End If
    End Sub


    Private Sub chkThingSpeak_CheckedChanged(sender As Object, e As EventArgs) Handles chkThingSpeak.CheckedChanged
        panThingSpeak.Visible = chkThingSpeak.Checked
    End Sub

    Private Sub txtDiscordMention_GotFocus(sender As Object, e As EventArgs) Handles txtDiscordMention.GotFocus
        If txtDiscordMention.Text.Trim = "" Then
            txtDiscordMention.Text = "<@>"
            ' Force the cursor position AFTER all focus events have been processed
            BeginInvoke(New Action(Sub()
                                       txtDiscordMention.SelectionStart = 2
                                       txtDiscordMention.SelectionLength = 0
                                   End Sub))
        End If
    End Sub

    Private Sub txtDiscordMention_TextChanged(sender As Object, e As EventArgs) Handles txtDiscordMention.TextChanged
        fsDiscordMentionID = txtDiscordMention.Text.Trim
    End Sub

    Private Sub txtDiscordServer_TextChanged(sender As Object, e As EventArgs) Handles txtDiscordServer.TextChanged
        fsDiscordServerWebhook = txtDiscordServer.Text.Trim
    End Sub


    Private Sub LoadUIControls()
        txtDiscordServer.Text = fsDiscordServerWebhook
        txtDiscordMention.Text = fsDiscordMentionID
        chkDiscordNotify.Checked = fbDiscordNotifications
        panDiscordVals.Visible = fbDiscordNotifications
        chkThingSpeak.Checked = fbThingSpeakEnabled
        If fbThingSpeakEnabled Then
            If fdUserLat = 0 Or fdUserLon = 0 Then
                txtLat.Text = ""
                txtLon.Text = ""
            Else
                txtLat.Text = fdUserLat.ToString("F6")
                txtLon.Text = fdUserLon.ToString("F6")
            End If
        Else
            txtLat.Text = ""
            txtLon.Text = ""
        End If
        chkDiscordNotify_CheckedChanged(Nothing, Nothing)
        chkThingSpeak_CheckedChanged(Nothing, Nothing)
    End Sub


    Private Function IsValidDiscordWebhook(ByVal sWebhookURL As String) As Boolean
        Dim sPattern As String = "^https:\/\/discord\.com\/api\/webhooks\/\d{17,20}\/[\w-]+$"
        Return Regex.IsMatch(sWebhookURL, sPattern)
    End Function

    Private Function IsValidDiscordMentionID(ByVal sMentionID As String) As Boolean
        Dim sPattern As String = "^<@&?\d{17,20}>$"
        Return Regex.IsMatch(sMentionID, sPattern)
    End Function

    Public Async Function GetLocationFromIPAsync() As Task(Of (Double, Double))
        Try
            Dim poClient As New HttpClient()
            Dim psJson As String = Await poClient.GetStringAsync("https://ipinfo.io/json")
            Dim poData As JObject = JObject.Parse(psJson)
            Dim asLatLon() As String = poData("loc").ToString().Split(","c)
            Return (CDbl(asLatLon(0)), CDbl(asLatLon(1)))

        Catch ex As Exception
            clsLogger.LogException("frmEditConfig.GetLocationFromIPAsync", ex)
            Return (0D, 0D)
        End Try
    End Function


    Private Function IsValidLatitude(psText As String) As Boolean
        Dim pdLat As Double
        If Double.TryParse(psText, pdLat) AndAlso pdLat >= -90 AndAlso pdLat <= 90 Then
            Return True
        End If
        Return False
    End Function

    Private Function IsValidLongitude(psText As String) As Boolean
        Dim pdLon As Double
        If Double.TryParse(psText, pdLon) AndAlso pdLon >= -180 AndAlso pdLon <= 180 Then
            Return True
        End If
        Return False
    End Function

    Private Sub lnkThingSpeak_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkThingSpeak.LinkClicked
        'Dim poNew As New clsThingSpeakAPI(String.Format("{0:F6},{1:F6}", fdUserLat, fdUserLon), "TESTGUID")
        'Call poNew.LogEventAsync(clsThingSpeakAPI.EventTypeEnum.SignalDetected, 1600000000I, 2048000, -76.9068, 16384)

        Dim psURL As String = "https://thingspeak.mathworks.com/channels/2869584"
        Try
            Process.Start(New ProcessStartInfo With {
                .FileName = psURL,
                .UseShellExecute = True
            })
        Catch ex As Exception
            MessageBox.Show($"Failed to launch process, please visit the site below via your Internet browser:{vbCrLf}{vbCrLf}{psURL}", "Failed To Start Process", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub



    '

End Class

'