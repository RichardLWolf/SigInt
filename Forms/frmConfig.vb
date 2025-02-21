Imports System.Text.RegularExpressions

Public Class frmConfig

    Dim fsDeviceName As String = ""
    Private fiGainMode As Integer
    Private fiGainValue As Integer
    Private fiCenterFreq As UInteger
    Private fiSampleRate As UInteger

    Private fiSigEventResetTime As Integer
    Private fiSigDetThresh As Integer
    Private fiSigDetWind As Integer
    Private fiSignalInitTime As Integer

    Private fiNFBaselineInitTime As Integer
    Private fdNFThreshold As Double
    Private fiNFMinEventDuration As Integer
    Private fiNFCooldownDuration As Integer
    Private fiNFEventResetTime As Integer

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

    Public ReadOnly Property SampleRate As UInteger
        Get
            Return fiSampleRate
        End Get
    End Property

    Public ReadOnly Property SignalEventResetTime As Double
        Get
            Return fiSigEventResetTime
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

    Public ReadOnly Property DetectionThreshold As Integer
        Get
            Return fiSigDetThresh
        End Get
    End Property

    Public ReadOnly Property DetectionWindow As Integer
        Get
            Return fiSigDetWind
        End Get
    End Property

    Public ReadOnly Property SignalInitTime As Integer
        Get
            Return fiSignalInitTime
        End Get
    End Property

    Public ReadOnly Property NoiseFloorThreshold As Double
        Get
            Return fdNFThreshold
        End Get
    End Property

    Public ReadOnly Property NoiseFloorMinEventDuration As Integer
        Get
            Return fiNFMinEventDuration
        End Get
    End Property

    Public ReadOnly Property NoiseFloorCooldownDuration As Integer
        Get
            Return fiNFCooldownDuration
        End Get
    End Property

    Public ReadOnly Property NoiseFloorEventResetTime As Integer
        Get
            Return fiNFEventResetTime
        End Get
    End Property

    Public ReadOnly Property NoiseFloorBaselineInitTime As Integer
        Get
            Return fiNFBaselineInitTime
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

        cboScale.Items.Clear()
        cboScale.Items.Add("Hz")
        cboScale.Items.Add("kHz")
        cboScale.Items.Add("MHz")
        cboScale.Items.Add("GHz")
        cboScale.SelectedIndex = 0


        With cboSigEventReset
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("1 min ", 60))
            .Items.Add(New KeyValuePair(Of String, Integer)("2 min ", 120))
            .Items.Add(New KeyValuePair(Of String, Integer)("3 min ", 180))
            .Items.Add(New KeyValuePair(Of String, Integer)("4 min ", 250))
            .Items.Add(New KeyValuePair(Of String, Integer)("5 min (Default)", 300))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 min", 600))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 min", 900))
            .Items.Add(New KeyValuePair(Of String, Integer)("20 min", 1200))
            .Items.Add(New KeyValuePair(Of String, Integer)("30 min", 1800))
            .Items.Add(New KeyValuePair(Of String, Integer)("45 min", 2700))
            .Items.Add(New KeyValuePair(Of String, Integer)("60 min (1 hour)", 3600))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboSigEventReset.SelectedIndex = 0

        With cboDetThresh
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("5 dB", 5))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 dB", 10))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 dB (Default)", 15))
            .Items.Add(New KeyValuePair(Of String, Integer)("20 dB", 20))
            .Items.Add(New KeyValuePair(Of String, Integer)("25 dB", 25))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboDetThresh.SelectedIndex = 0

        With cboDetWind
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("3 bins (Default)", 1))
            .Items.Add(New KeyValuePair(Of String, Integer)("5 bins", 2))
            .Items.Add(New KeyValuePair(Of String, Integer)("7 bins", 3))
            .Items.Add(New KeyValuePair(Of String, Integer)("9 bins", 4))
            .Items.Add(New KeyValuePair(Of String, Integer)("11 bins", 5))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 bins", 7))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboDetWind.SelectedIndex = 0

        With cboSampleRate
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, UInteger)("3.2 MSPS (~1.6 MHz BW)", 3200000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("2.8 MSPS (~1.4 MHz BW)", 2800000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("2.56 MSPS (~1.28 MHz BW)", 2560000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("2.4 MSPS (~1.2 MHz BW)", 2400000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("2.048 MSPS (~1.024 MHz BW) (Default)", 2048000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("1.92 MSPS (~960 kHz BW)", 1920000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("1.8 MSPS (~900 kHz BW)", 1800000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("1.4 MSPS (~700 kHz BW)", 1400000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("1.024 MSPS (~512 kHz BW)", 1024000))
            .Items.Add(New KeyValuePair(Of String, UInteger)("0.900001 MSPS (~450 kHz BW)", 900001))
            .Items.Add(New KeyValuePair(Of String, UInteger)("0.25 MSPS (~125 kHz BW)", 250000))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboSampleRate.SelectedIndex = 0

        With cboSignalInit
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("1 second", 1))
            .Items.Add(New KeyValuePair(Of String, Integer)("2 seconds", 2))
            .Items.Add(New KeyValuePair(Of String, Integer)("3 seconds (Default)", 3))
            .Items.Add(New KeyValuePair(Of String, Integer)("5 seconds", 5))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 seconds", 10))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboSignalInit.SelectedIndex = 0

        With cboNFBaselineInit
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("30 seconds", 30))
            .Items.Add(New KeyValuePair(Of String, Integer)("45 seconds", 45))
            .Items.Add(New KeyValuePair(Of String, Integer)("60 seconds (Default)", 60))
            .Items.Add(New KeyValuePair(Of String, Integer)("90 seconds", 90))
            .Items.Add(New KeyValuePair(Of String, Integer)("120 seconds", 120))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboNFBaselineInit.SelectedIndex = 0

        With cboNFDetThresh
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Double)("2.0 dB", 2.0))
            .Items.Add(New KeyValuePair(Of String, Double)("3.0 dB", 3.0))
            .Items.Add(New KeyValuePair(Of String, Double)("4.0 dB (Default)", 4.0))
            .Items.Add(New KeyValuePair(Of String, Double)("5.0 dB", 5.0))
            .Items.Add(New KeyValuePair(Of String, Double)("6.0 dB", 6.0))
            .Items.Add(New KeyValuePair(Of String, Double)("7.0 dB", 7.0))
            .Items.Add(New KeyValuePair(Of String, Double)("8.0 dB", 8.0))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboNFDetThresh.SelectedIndex = 0

        With cboNFMinDur
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("2 seconds", 2))
            .Items.Add(New KeyValuePair(Of String, Integer)("3 seconds", 3))
            .Items.Add(New KeyValuePair(Of String, Integer)("5 seconds (Default)", 5))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 seconds", 10))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 seconds", 15))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboNFMinDur.SelectedIndex = 0

        With cboNFCooldown
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("5 seconds", 5))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 seconds (Default)", 10))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 seconds", 15))
            .Items.Add(New KeyValuePair(Of String, Integer)("20 seconds", 20))
            .Items.Add(New KeyValuePair(Of String, Integer)("30 seconds", 30))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboNFCooldown.SelectedIndex = 0

        With cboNFReset
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("1 min ", 60))
            .Items.Add(New KeyValuePair(Of String, Integer)("2 min ", 120))
            .Items.Add(New KeyValuePair(Of String, Integer)("3 min ", 180))
            .Items.Add(New KeyValuePair(Of String, Integer)("4 min ", 250))
            .Items.Add(New KeyValuePair(Of String, Integer)("5 min (Default)", 300))
            .Items.Add(New KeyValuePair(Of String, Integer)("10 min", 600))
            .Items.Add(New KeyValuePair(Of String, Integer)("15 min", 900))
            .Items.Add(New KeyValuePair(Of String, Integer)("20 min", 1200))
            .Items.Add(New KeyValuePair(Of String, Integer)("30 min", 1800))
            .Items.Add(New KeyValuePair(Of String, Integer)("45 min", 2700))
            .Items.Add(New KeyValuePair(Of String, Integer)("60 min (1 hour)", 3600))
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        cboNFReset.SelectedIndex = 0

        ' Load values form config to form-level vars
        Me.SelectedDeviceName = SelectedDevice
        fiGainMode = oConfig.GainMode
        fiGainValue = oConfig.GainValue
        foGains = oSDR.GetTunerGainsList()
        Dim poFreqs As New List(Of UInteger)
        poFreqs = oSDR.GetFrequencyRangeList()
        fiMinFreq = poFreqs(0)
        fiMaxFreq = poFreqs(1)
        fiCenterFreq = oConfig.CenterFrequency
        fiSampleRate = oConfig.SampleRate
        fiSigEventResetTime = oConfig.SignalEventResetTime
        fiSigDetWind = oConfig.SignalDetectionWindow
        fiSigDetThresh = oConfig.SignalDetectionThreshold
        fiSignalInitTime = oConfig.SignalInitTime
        fbDiscordNotifications = oConfig.DiscordNotifications
        fsDiscordServerWebhook = oConfig.DiscordServerWebhook
        fsDiscordMentionID = oConfig.DiscordMentionID
        fiNFBaselineInitTime = oConfig.NoiseFloorBaselineInitTime
        fdNFThreshold = oConfig.NoiseFloorThreshold
        fiNFMinEventDuration = oConfig.NoiseFloorMinEventDuration
        fiNFCooldownDuration = oConfig.NoiseFloorCooldownDuration
        fiNFEventResetTime = oConfig.NoiseFloorEventResetTime

        If foGains.Count > 0 Then
            foGains.Sort()
            sldGain.Minimum = 0
            sldGain.Maximum = foGains.Count
            sldGain.Value = GetClosestIndex(fiGainValue, foGains)
            fiGainValue = foGains(sldGain.Value)
            sldGain.DisplayValue = False
        End If
        sldGain_ValueChanged(Nothing, Nothing)

        ' put values onto screen
        LoadUIControls()
    End Sub


    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'check values
        Dim pbError = False
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
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub btnClearFreq_Click(sender As Object, e As EventArgs) Handles btnClearFreq.Click
        txtFrequency.Text = ""
        txtFrequency.Focus()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(0)

        fiGainMode = If(poSdrCfg.bAutomaticGain, 0, 1)
        fiGainValue = poSdrCfg.iManualGainValue
        fiCenterFreq = poSdrCfg.iCenterFrequency
        fiSampleRate = poSdrCfg.iSampleRate
        fiSigEventResetTime = poSdrCfg.dSignalEventResetTime
        fiSigDetWind = poSdrCfg.iSignalDetectionWindow
        fiSigDetThresh = poSdrCfg.iSignalDetectionThreshold
        fiSignalInitTime = poSdrCfg.iSignalInitTime

        fiSigEventResetTime = poSdrCfg.dSignalEventResetTime
        fiNFBaselineInitTime = poSdrCfg.iNoiseFloorBaselineInitTime
        fdNFThreshold = poSdrCfg.dNoiseFloorThreshold
        fiNFMinEventDuration = poSdrCfg.iNoiseFloorMinEventDuration
        fiNFCooldownDuration = poSdrCfg.iNoiseFloorCooldownDuration
        fiNFEventResetTime = poSdrCfg.iNoiseFloorEventResetTime

        If String.IsNullOrEmpty(poSdrCfg.sDiscordWebhook) Then
            fsDiscordServerWebhook = ""
            fsDiscordMentionID = ""
        Else
            fsDiscordServerWebhook = poSdrCfg.sDiscordWebhook
            fsDiscordMentionID = poSdrCfg.sDiscordMention
        End If
        LoadUIControls()
    End Sub

    Private Sub cboDetThresh_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDetThresh.SelectedIndexChanged
        fiSigDetThresh = DirectCast(cboDetThresh.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboDetWind_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDetWind.SelectedIndexChanged
        fiSigDetWind = DirectCast(cboDetWind.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboSigEventReset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSigEventReset.SelectedIndexChanged
        fiSigEventResetTime = DirectCast(cboSigEventReset.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboSampleRate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSampleRate.SelectedIndexChanged
        fiSampleRate = DirectCast(cboSampleRate.SelectedItem, KeyValuePair(Of String, UInteger)).Value
    End Sub

    Private Sub cboScale_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScale.SelectedIndexChanged
        ConvertFrequency()
        ' put focus back in frequency box
        txtFrequency.SelectAll()
        txtFrequency.Focus()
    End Sub

    Private Sub cboNFBaselineInit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNFBaselineInit.SelectedIndexChanged
        fiNFBaselineInitTime = DirectCast(cboNFBaselineInit.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboNFCooldown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNFCooldown.SelectedIndexChanged
        fiNFCooldownDuration = DirectCast(cboNFCooldown.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboNFDetThresh_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNFDetThresh.SelectedIndexChanged
        fdNFThreshold = DirectCast(cboNFDetThresh.SelectedItem, KeyValuePair(Of String, Double)).Value
    End Sub

    Private Sub cboNFMinDur_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNFMinDur.SelectedIndexChanged
        fiNFMinEventDuration = DirectCast(cboNFMinDur.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboNFReset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNFReset.SelectedIndexChanged
        fiNFEventResetTime = DirectCast(cboNFReset.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub

    Private Sub cboSignalInit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSignalInit.SelectedIndexChanged
        fiSignalInitTime = DirectCast(cboSignalInit.SelectedItem, KeyValuePair(Of String, Integer)).Value
    End Sub






    Private Sub chkAutomatic_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutomatic.CheckedChanged
        lblMainGain.Visible = Not chkAutomatic.Checked
        lblGainValue.Visible = Not chkAutomatic.Checked
        sldGain.Visible = Not chkAutomatic.Checked
        fiGainMode = If(chkAutomatic.Checked, 0, 1)
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
            If Me.Visible Then
                panInner.ScrollControlIntoView(panDiscordVals)
            End If
        Else
            panDiscordVals.Visible = False
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


    Private Sub LoadUIControls()
        lblFreqRange.Text = $"({modMain.FormatHertz(fiMinFreq)} – {modMain.FormatHertz(fiMaxFreq)})"
        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.Text = String.Format("{0:#########0}", fiCenterFreq)

        ' Note: Do not set a default SelectedIndex for the comboboxes, as it will trigger the SelectedIndexChanged event and overwrite the form-level values

        For piIndex As Integer = 0 To cboSigEventReset.Items.Count - 1
            Dim kvp As KeyValuePair(Of String, Integer) = DirectCast(cboSigEventReset.Items(piIndex), KeyValuePair(Of String, Integer))
            If kvp.Value = fiSigEventResetTime Then
                cboSigEventReset.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboDetThresh.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboDetThresh.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiSigDetThresh Then
                cboDetThresh.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboDetWind.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboDetWind.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiSigDetWind Then
                cboDetWind.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboSampleRate.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, UInteger) = DirectCast(cboSampleRate.Items(piIndex), KeyValuePair(Of String, UInteger))
            If poKvp.Value = fiSampleRate Then
                cboSampleRate.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboSignalInit.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboSignalInit.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiSignalInitTime Then
                cboSignalInit.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFBaselineInit.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFBaselineInit.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiNFBaselineInitTime Then
                cboNFBaselineInit.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFDetThresh.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Double) = DirectCast(cboNFDetThresh.Items(piIndex), KeyValuePair(Of String, Double))
            If poKvp.Value = fdNFThreshold Then
                cboNFDetThresh.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFMinDur.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFMinDur.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiNFMinEventDuration Then
                cboNFMinDur.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFCooldown.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFCooldown.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiNFCooldownDuration Then
                cboNFCooldown.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFReset.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFReset.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = fiNFEventResetTime Then
                cboNFReset.SelectedIndex = piIndex
                Exit For
            End If
        Next

        chkAutomatic.Checked = CBool(fiGainMode = 0)

        If foGains.Count > 0 Then
            foGains.Sort()
            sldGain.Minimum = 0
            sldGain.Maximum = foGains.Count
            sldGain.Value = GetClosestIndex(fiGainValue, foGains)
            fiGainValue = foGains(sldGain.Value)
            sldGain.DisplayValue = False
        End If
        sldGain_ValueChanged(Nothing, Nothing)

        txtDiscordServer.Text = fsDiscordServerWebhook
        txtDiscordMention.Text = fsDiscordMentionID
        chkDiscordNotify.Checked = fbDiscordNotifications
        panDiscordVals.Visible = fbDiscordNotifications
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