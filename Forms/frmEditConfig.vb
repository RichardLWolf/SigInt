Public Class frmEditConfig

    Private Const MIN_FREQUENCY_HZ As Integer = 24000000   ' 24 MHz
    Private Const MAX_FREQUENCY_HZ As Integer = 1766000000 ' 1.766 GHz


    Private foConfig As DeviceConfig

    Public ReadOnly Property ConfigValues As DeviceConfig
        Get
            Return foConfig
        End Get
    End Property

    Public Sub ReadyForm(ByVal oConfig As DeviceConfig)
        ' create a copy of what's passed in to avoid screwing with it while we're editing.
        foConfig = oConfig.Clone()

        SetFormTitle()

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
            .Items.Add(New KeyValuePair(Of String, Integer)("3.2 MSPS (~1.6 MHz BW)", 3200000))
            .Items.Add(New KeyValuePair(Of String, Integer)("2.8 MSPS (~1.4 MHz BW)", 2800000))
            .Items.Add(New KeyValuePair(Of String, Integer)("2.56 MSPS (~1.28 MHz BW)", 2560000))
            .Items.Add(New KeyValuePair(Of String, Integer)("2.4 MSPS (~1.2 MHz BW)", 2400000))
            .Items.Add(New KeyValuePair(Of String, Integer)("2.048 MSPS (~1.024 MHz BW) (Default)", 2048000))
            .Items.Add(New KeyValuePair(Of String, Integer)("1.92 MSPS (~960 kHz BW)", 1920000))
            .Items.Add(New KeyValuePair(Of String, Integer)("1.8 MSPS (~900 kHz BW)", 1800000))
            .Items.Add(New KeyValuePair(Of String, Integer)("1.4 MSPS (~700 kHz BW)", 1400000))
            .Items.Add(New KeyValuePair(Of String, Integer)("1.024 MSPS (~512 kHz BW)", 1024000))
            .Items.Add(New KeyValuePair(Of String, Integer)("0.900001 MSPS (~450 kHz BW)", 900001))
            .Items.Add(New KeyValuePair(Of String, Integer)("0.25 MSPS (~125 kHz BW)", 250000))
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

        With cboSignalGain
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, Integer)("* Automatic Gain *", -1))
            .Items.Add(New KeyValuePair(Of String, Integer)("0.0 dB", 0))
            .Items.Add(New KeyValuePair(Of String, Integer)("0.9 dB", 9))
            .Items.Add(New KeyValuePair(Of String, Integer)("1.4 dB", 14))
            .Items.Add(New KeyValuePair(Of String, Integer)("2.7 dB", 27))
            .Items.Add(New KeyValuePair(Of String, Integer)("3.7 dB", 37))
            .Items.Add(New KeyValuePair(Of String, Integer)("7.7 dB", 77))
            .Items.Add(New KeyValuePair(Of String, Integer)("8.7 dB", 87))
            .Items.Add(New KeyValuePair(Of String, Integer)("12.5 dB", 125))
            .Items.Add(New KeyValuePair(Of String, Integer)("14.4 dB", 144))
            .Items.Add(New KeyValuePair(Of String, Integer)("15.7 dB", 157))
            .Items.Add(New KeyValuePair(Of String, Integer)("16.6 dB", 166))
            .Items.Add(New KeyValuePair(Of String, Integer)("19.7 dB", 197))
            .Items.Add(New KeyValuePair(Of String, Integer)("20.7 dB", 207))
            .Items.Add(New KeyValuePair(Of String, Integer)("22.9 dB", 229))
            .Items.Add(New KeyValuePair(Of String, Integer)("25.4 dB", 254))
            .Items.Add(New KeyValuePair(Of String, Integer)("28.0 dB", 280))
            .Items.Add(New KeyValuePair(Of String, Integer)("29.7 dB", 297))
            .Items.Add(New KeyValuePair(Of String, Integer)("32.8 dB", 328))
            .Items.Add(New KeyValuePair(Of String, Integer)("33.8 dB", 338))
            .Items.Add(New KeyValuePair(Of String, Integer)("36.4 dB", 364))
            .Items.Add(New KeyValuePair(Of String, Integer)("37.2 dB", 372))
            .Items.Add(New KeyValuePair(Of String, Integer)("38.6 dB", 386))
            .Items.Add(New KeyValuePair(Of String, Integer)("40.2 dB", 402))
            .Items.Add(New KeyValuePair(Of String, Integer)("42.1 dB", 421))
            .Items.Add(New KeyValuePair(Of String, Integer)("43.4 dB", 434))
            .Items.Add(New KeyValuePair(Of String, Integer)("43.9 dB", 439))
            .Items.Add(New KeyValuePair(Of String, Integer)("44.5 dB", 445))
            .Items.Add(New KeyValuePair(Of String, Integer)("48.0 dB", 480))
            .Items.Add(New KeyValuePair(Of String, Integer)("49.6 dB", 496))

            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With

        ToolTip1.SetToolTip(txtFrequency, $"Range is {modMain.FormatHertz(MIN_FREQUENCY_HZ)} - {modMain.FormatHertz(MAX_FREQUENCY_HZ)}")
        ToolTip1.SetToolTip(lblFrequency, $"Range is {modMain.FormatHertz(MIN_FREQUENCY_HZ)} - {modMain.FormatHertz(MAX_FREQUENCY_HZ)}")

        ' put values onto screen
        LoadUIControls()
    End Sub




    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'check values
        Dim pbError = False
        Dim piFreq As UInteger = 0

        If txtConfigName.Text.Trim = "" Then
            MsgBox("Please enter a configuration name before applying changes.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Name")
            txtConfigName.Focus()
            pbError = True
        End If

        UInteger.TryParse(txtFrequency.Tag, piFreq)
        If piFreq < MIN_FREQUENCY_HZ Or piFreq > MAX_FREQUENCY_HZ Then
            MsgBox("Please enter a valid center frequency before applying changes.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Frequency")
            txtFrequency.Focus()
            pbError = True
        End If

        If Not pbError Then
            ' copy UI to config class
            With foConfig
                .ConfigurationName = txtConfigName.Text.Trim
                .CenterFrequency = piFreq
                If cboSignalGain.SelectedIndex < 1 Then
                    .GainMode = 0 ' automatic
                Else
                    .GainMode = 1 ' manual
                    .GainValue = DirectCast(cboSignalGain.SelectedItem, KeyValuePair(Of String, Integer)).Value
                End If

                .SampleRate = DirectCast(cboSampleRate.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .SignalEventResetTime = DirectCast(cboSigEventReset.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .SignalDetectionThreshold = DirectCast(cboDetThresh.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .SignalDetectionWindow = DirectCast(cboDetWind.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .SignalInitTime = DirectCast(cboSignalInit.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .NoiseFloorBaselineInitTime = DirectCast(cboNFBaselineInit.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .NoiseFloorThreshold = DirectCast(cboNFDetThresh.SelectedItem, KeyValuePair(Of String, Double)).Value
                .NoiseFloorMinEventDuration = DirectCast(cboNFMinDur.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .NoiseFloorCooldownDuration = DirectCast(cboNFCooldown.SelectedItem, KeyValuePair(Of String, Integer)).Value
                .NoiseFloorEventResetTime = DirectCast(cboNFReset.SelectedItem, KeyValuePair(Of String, Integer)).Value
            End With
            ' return OK
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub btnClearFreq_Click(sender As Object, e As EventArgs) Handles btnClearFreq.Click
        txtFrequency.Text = ""
        txtFrequency.Focus()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim poCfg As New DeviceConfig
        poCfg.ConfigurationKey = foConfig.ConfigurationKey
        poCfg.ConfigurationName = foConfig.ConfigurationName
        foConfig = poCfg
        SetFormTitle()
        ' put values onto screen
        LoadUIControls()
    End Sub


    Private Sub cboScale_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScale.SelectedIndexChanged
        ConvertFrequency()
        ' put focus back in frequency box
        txtFrequency.SelectAll()
        txtFrequency.Focus()
    End Sub

    Private Sub txtConfigName_TextChanged(sender As Object, e As EventArgs) Handles txtConfigName.TextChanged
        SetFormTitle()
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

















    Private Sub SetFormTitle()
        Me.Text = $"Editing Configuration - {txtConfigName.Text}"
    End Sub


    Private Sub LoadUIControls()
        txtConfigName.Text = foConfig.ConfigurationName
        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.Text = String.Format("{0:#########0}", foConfig.CenterFrequency)
        txtFrequency.Tag = foConfig.CenterFrequency

        SetFormTitle()

        ' Note: Do not set a default SelectedIndex for the comboboxes, as it will trigger the SelectedIndexChanged event and overwrite the form-level values

        For piIndex As Integer = 0 To cboSigEventReset.Items.Count - 1
            Dim kvp As KeyValuePair(Of String, Integer) = DirectCast(cboSigEventReset.Items(piIndex), KeyValuePair(Of String, Integer))
            If kvp.Value = foConfig.SignalEventResetTime Then
                cboSigEventReset.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboDetThresh.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboDetThresh.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.SignalDetectionThreshold Then
                cboDetThresh.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboDetWind.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboDetWind.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.SignalDetectionWindow Then
                cboDetWind.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboSampleRate.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboSampleRate.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.SampleRate Then
                cboSampleRate.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboSignalInit.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboSignalInit.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.SignalInitTime Then
                cboSignalInit.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFBaselineInit.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFBaselineInit.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.NoiseFloorBaselineInitTime Then
                cboNFBaselineInit.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFDetThresh.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Double) = DirectCast(cboNFDetThresh.Items(piIndex), KeyValuePair(Of String, Double))
            If poKvp.Value = foConfig.NoiseFloorThreshold Then
                cboNFDetThresh.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFMinDur.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFMinDur.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.NoiseFloorMinEventDuration Then
                cboNFMinDur.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFCooldown.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFCooldown.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.NoiseFloorCooldownDuration Then
                cboNFCooldown.SelectedIndex = piIndex
                Exit For
            End If
        Next

        For piIndex = 0 To cboNFReset.Items.Count - 1
            Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboNFReset.Items(piIndex), KeyValuePair(Of String, Integer))
            If poKvp.Value = foConfig.NoiseFloorEventResetTime Then
                cboNFReset.SelectedIndex = piIndex
                Exit For
            End If
        Next

        If foConfig.GainMode = 0 Then
            ' automatic gain
            cboSignalGain.SelectedIndex = 0
        Else
            ' start at 1 since 0 is automatic
            For piIndex = 1 To cboSignalGain.Items.Count - 1
                Dim poKvp As KeyValuePair(Of String, Integer) = DirectCast(cboSignalGain.Items(piIndex), KeyValuePair(Of String, Integer))
                If poKvp.Value = foConfig.GainValue Then
                    cboSignalGain.SelectedIndex = piIndex
                    Exit For
                End If
            Next
        End If
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
        If pbError OrElse (pdInput < MIN_FREQUENCY_HZ OrElse pdInput > MAX_FREQUENCY_HZ) Then
            lblFrequency.Text = "* Invalid *"
            lblFrequency.ForeColor = Color.LightSalmon
            txtFrequency.Tag = 0
        Else
            lblFrequency.Text = String.Format("{0:#,###,###,##0} Hz", pdInput)
            lblFrequency.ForeColor = Color.White
            txtFrequency.Tag = pdInput
        End If
    End Sub





    '
End Class


'