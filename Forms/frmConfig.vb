Imports System.Text.RegularExpressions

Public Class frmConfig

    Dim fsDeviceName As String = ""
    Private fiGainMode As Integer
    Private fiGainValue As Integer
    Private fiCenterFreq As UInteger
    Private fiSampleRate As Integer
    Private fdMinEventWindow As Double
    Private fbDiscordNotifications As Boolean
    Private fsDiscordServerWebhook As String
    Private fsDiscordMentionID As String


    Private foGains As New List(Of Integer)
    Private fiMinFreq As UInteger
    Private fiMaxFreq As UInteger

    Public ReadOnly Property GainMode As Integer
        Get
            Return fiGainMode
        End Get
    End Property

    Public ReadOnly Property GainValue As Integer
        Get
            Return fiGainValue
        End Get
    End Property

    Public ReadOnly Property CenterFreq As UInteger
        Get
            Return fiCenterFreq
        End Get
    End Property

    Public ReadOnly Property SampleRate As Integer
        Get
            Return fiSampleRate
        End Get
    End Property

    Public ReadOnly Property MinEventWindow As Double
        Get
            Return fdMinEventWindow
        End Get
    End Property

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

    Public Property SelectedDeviceName As String
        Get
            Return fsDeviceName
        End Get
        Set(value As String)
            fsDeviceName = value
            Me.Text = fsDeviceName & " Configuration"
        End Set
    End Property

    Public Sub ReadyForm(ByVal SelectedDevice As String, ByVal oSDR As RtlSdrApi, ByVal oConfig As clsAppConfig)
        Me.SelectedDeviceName = SelectedDevice

        fiGainMode = oSDR.GainMode
        fiGainValue = oSDR.GainValue
        foGains = oSDR.GetTunerGainsList()
        Dim poFreqs As New List(Of UInteger)
        poFreqs = oSDR.GetFrequencyRangeList()
        fiMinFreq = poFreqs(0)
        fiMaxFreq = poFreqs(1)
        fiCenterFreq = oSDR.CenterFrequency
        fiSampleRate = oSDR.SampleRate
        fdMinEventWindow = oSDR.MinimumEventWindow
        fbDiscordNotifications = oConfig.DiscordNotifications
        fsDiscordServerWebhook = oConfig.DiscordServerWebhook
        fsDiscordMentionID = oConfig.DiscordMentionID


        cboScale.Items.Clear()
        cboScale.Items.Add("Hz")
        cboScale.Items.Add("kHz")
        cboScale.Items.Add("MHz")
        cboScale.Items.Add("GHz")
        cboScale.SelectedIndex = 0

        lblFreqRange.Text = $"({modMain.FormatHertz(fiMinFreq)} – {modMain.FormatHertz(fiMaxFreq)})"

        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.Text = String.Format("{0:#########0}", fiCenterFreq)

        cboMinEventWindow.Items.Clear()
        With cboMinEventWindow
            .Items.Add(New KeyValuePair(Of String, Double)("30 sec", 0.5D))
            .Items.Add(New KeyValuePair(Of String, Double)("1 min", 1D))
            .Items.Add(New KeyValuePair(Of String, Double)("2 min", 2D))
            .Items.Add(New KeyValuePair(Of String, Double)("5 min", 5D))
            .Items.Add(New KeyValuePair(Of String, Double)("10 min (Default)", 10D))
            .Items.Add(New KeyValuePair(Of String, Double)("15 min", 15D))
            .Items.Add(New KeyValuePair(Of String, Double)("20 min", 20D))
            .Items.Add(New KeyValuePair(Of String, Double)("30 min", 30D))
            .Items.Add(New KeyValuePair(Of String, Double)("45 min", 45D))
            .Items.Add(New KeyValuePair(Of String, Double)("60 min (1 hour)", 60D))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        For i As Integer = 0 To cboMinEventWindow.Items.Count - 1
            Dim kvp As KeyValuePair(Of String, Double) = DirectCast(cboMinEventWindow.Items(i), KeyValuePair(Of String, Double))
            If kvp.Value = fdMinEventWindow Then
                cboMinEventWindow.SelectedIndex = i
                Exit For
            End If
        Next

        If foGains.Count > 0 Then
            foGains.Sort()
            sldGain.Minimum = 0
            sldGain.Maximum = foGains.Count
            sldGain.Value = GetClosestIndex(fiGainValue, foGains)
            fiGainValue = foGains(sldGain.Value)
            sldGain.DisplayValue = False
        End If
        sldGain_ValueChanged(Nothing, Nothing)
        chkAutomatic.Checked = CBool(oSDR.GainMode = 0)
        chkDiscordNotify.Checked = oConfig.DiscordNotifications
        txtDiscordServer.Text = oConfig.DiscordServerWebhook
        txtDiscordMention.Text = oConfig.DiscordMentionID
    End Sub




    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'check values
        Dim pbError As Boolean = False
        If fiCenterFreq < fiMinFreq Or fiCenterFreq > fiMaxFreq Then
            MsgBox("Please enter a valid center frequency before applying changes.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Frequency")
            pbError = True
        End If

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
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub btnClearFreq_Click(sender As Object, e As EventArgs) Handles btnClearFreq.Click
        txtFrequency.Text = ""
        txtFrequency.Focus()
    End Sub

    Private Sub cboMinEventWindow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMinEventWindow.SelectedIndexChanged
        fdMinEventWindow = DirectCast(cboMinEventWindow.SelectedItem, KeyValuePair(Of String, Double)).Value
    End Sub

    Private Sub cboScale_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScale.SelectedIndexChanged
        ConvertFrequency()
        ' put focus back in frequency box
        txtFrequency.SelectAll()
        txtFrequency.Focus()
    End Sub

    Private Sub chkAutomatic_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutomatic.CheckedChanged
        lblMainGain.Visible = Not chkAutomatic.Checked
        lblGainValue.Visible = Not chkAutomatic.Checked
        sldGain.Visible = Not chkAutomatic.Checked
        fiGainMode = If(chkAutomatic.Checked, 0, 1)
    End Sub

    Private Sub chkDiscordNotify_CheckedChanged(sender As Object, e As EventArgs) Handles chkDiscordNotify.CheckedChanged
        fbDiscordNotifications = CBool(chkDiscordNotify.Checked)
        If fbDiscordNotifications Then
            If fsDiscordServerWebhook = "" Then
                txtDiscordServer.Text = "https://discord.com/api/webhooks/"
                txtDiscordServer.SelectionStart = txtDiscordServer.Text.Length
                txtDiscordMention.Text = ""
                txtDiscordServer.Focus()
            End If
        Else
            txtDiscordServer.Text = ""
            txtDiscordMention.Text = ""
        End If
    End Sub

    Private Sub sldGain_ValueChanged(sender As Object, e As EventArgs) Handles sldGain.ValueChanged
        If sldGain.Value >= 0 And sldGain.Value < foGains.Count Then
            fiGainValue = foGains(sldGain.Value)
            lblGainValue.Text = String.Format("{0:#0.0} dB", fiGainValue / 10)
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

    Private Sub txtFrequency_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFrequency.KeyDown
        If Not (e.KeyCode >= Keys.D0 AndAlso e.KeyCode <= Keys.D9 OrElse
            e.KeyCode >= Keys.NumPad0 AndAlso e.KeyCode <= Keys.NumPad9 OrElse
            e.KeyCode = Keys.Back OrElse e.KeyCode = Keys.Delete OrElse
            e.KeyCode = Keys.Decimal OrElse e.KeyCode = Keys.OemPeriod OrElse
            e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right) Then
            e.SuppressKeyPress = True ' Block the key
        End If
    End Sub

    Private Sub txtFrequency_TextChanged(sender As Object, e As EventArgs) Handles txtFrequency.TextChanged
        ConvertFrequency()
    End Sub

    Private Sub ConvertFrequency()
        Dim pdInput As Double = 0
        Dim psText As String = txtFrequency.Text.Trim()
        Dim pbError As Boolean = False

        If psText = String.Empty OrElse cboScale.SelectedIndex < 0 Then
            Exit Sub
        End If

        ' Validate input
        If Not Double.TryParse(psText, pdInput) Then
            pbError = True
        Else
            ' Get selected unit
            Dim fsUnit As String = cboScale.Items(cboScale.SelectedIndex).ToString()

            ' Convert to Hz based on the selected unit
            Try
                Select Case fsUnit
                    Case "Hz"
                        ' No conversion needed
                    Case "kHz"
                        pdInput *= 1000
                    Case "MHz"
                        pdInput *= 1000000
                    Case "GHz"
                        pdInput *= 1000000000
                End Select
            Catch ex As Exception
                ' probably overflow of some kind
                pbError = True
            End Try
        End If

        ' Ensure within min/max limits
        If pbError OrElse ((pdInput < fiMinFreq OrElse pdInput > fiMaxFreq) And fiMinFreq <> 0 And fiMaxFreq <> 0) Then
            lblFrequency.Text = "* Invalid *"
            lblFrequency.ForeColor = Color.LightSalmon
            fiCenterFreq = 0
        Else
            lblFrequency.Text = String.Format("{0:#,###,###,##0} Hz", pdInput)
            lblFrequency.ForeColor = Color.White
            fiCenterFreq = pdInput
        End If
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