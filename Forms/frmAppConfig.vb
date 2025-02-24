Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions

Public Class frmAppConfig
    Private fbDiscordNotifications As Boolean
    Private fsDiscordServerWebhook As String
    Private fsDiscordMentionID As String

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


    Public Sub ReadyForm(ByVal oAppConfig As clsAppConfig)
        fbDiscordNotifications = oAppConfig.DiscordNotifications
        fsDiscordServerWebhook = oAppConfig.DiscordServerWebhook
        fsDiscordMentionID = oAppConfig.DiscordMentionID

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

        If Not pbError Then
            DialogResult = DialogResult.OK
        End If
    End Sub


    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(0)

        fbDiscordNotifications = False
        fsDiscordServerWebhook = ""
        fsDiscordMentionID = ""
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
    End Sub


    Private Function IsValidDiscordWebhook(ByVal sWebhookURL As String) As Boolean
        Dim sPattern As String = "^https:\/\/discord\.com\/api\/webhooks\/\d{17,20}\/[\w-]+$"
        Return Regex.IsMatch(sWebhookURL, sPattern)
    End Function

    Private Function IsValidDiscordMentionID(ByVal sMentionID As String) As Boolean
        Dim sPattern As String = "^<@&?\d{17,20}>$"
        Return Regex.IsMatch(sMentionID, sPattern)
    End Function


    '

End Class

'