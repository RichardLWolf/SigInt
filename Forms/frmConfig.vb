Public Class frmConfig

    Dim fsDeviceName As String = ""
    Private fiGainMode As Integer
    Private fiGainValue As Integer
    Private fiCenterFreq As UInteger
    Private fiSampleRate As Integer

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



    Public Property SelectedDeviceName As String
        Get
            Return fsDeviceName
        End Get
        Set(value As String)
            fsDeviceName = value
            Me.Text = fsDeviceName & " Configuration"
        End Set
    End Property

    Public Sub ReadyForm(ByVal SelectedDevice As String, ByVal oSDR As RtlSdrApi)
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

        cboScale.Items.Clear()
        cboScale.Items.Add("Hz")
        cboScale.Items.Add("kHz")
        cboScale.Items.Add("MHz")
        cboScale.Items.Add("GHz")
        cboScale.SelectedIndex = 0
        lblFreqRange.Text = $"({modMain.FormatHertz(fiMinFreq)} – {modMain.FormatHertz(fiMaxFreq)})"
        txtFrequency.Text = String.Format("{0:#########0}", fiCenterFreq)

        If foGains.Count > 0 Then
            foGains.Sort()
            sldGain.Minimum = 0
            sldGain.Maximum = foGains.Count
            sldGain.Value = GetClosestIndex(fiGainValue, foGains)
            fiGainValue = foGains(sldGain.Value)
            sldGain.DisplayValue = False
        End If
        sldGain_ValueChanged(Nothing, Nothing)
        chkAutomatic.Checked = (oSDR.GainMode = 0)
    End Sub

    Private Sub btnClearFreq_Click(sender As Object, e As EventArgs) Handles btnClearFreq.Click
        txtFrequency.Text = ""
        txtFrequency.Focus()
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

    Private Sub sldGain_ValueChanged(sender As Object, e As EventArgs) Handles sldGain.ValueChanged
        If sldGain.Value >= 0 And sldGain.Value < foGains.Count Then
            fiGainValue = foGains(sldGain.Value)
            lblGainValue.Text = String.Format("{0:#0.0} dB", fiGainValue / 10)
        End If
    End Sub

    Private Sub txtFrequency_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFrequency.KeyDown
        If Not ((e.KeyCode >= Keys.D0 AndAlso e.KeyCode <= Keys.D9) OrElse
            (e.KeyCode >= Keys.NumPad0 AndAlso e.KeyCode <= Keys.NumPad9) OrElse
            e.KeyCode = Keys.Back OrElse e.KeyCode = Keys.Delete OrElse
            e.KeyCode = Keys.Decimal OrElse e.KeyCode = Keys.OemPeriod OrElse
            e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right) Then
            e.SuppressKeyPress = True ' Block the key
        End If
    End Sub

    Private Sub txtFrequency_TextChanged(sender As Object, e As EventArgs) Handles txtFrequency.TextChanged
        Call ConvertFrequency()
    End Sub

    Private Sub ConvertFrequency()
        Dim pdInput As Double = 0
        Dim psText As String = txtFrequency.Text.Trim()
        Dim pbError As Boolean = False

        If psText = String.Empty OrElse cboScale.SelectedItem Is Nothing Then
            Exit Sub
        End If

        ' Validate input
        If Not Double.TryParse(psText, pdInput) Then
            pbError = True
        Else
            ' Get selected unit
            Dim fsUnit As String = cboScale.SelectedItem.ToString()

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

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        'check values
        Dim pbError As Boolean = False
        If fiCenterFreq < fiMinFreq Or fiCenterFreq > fiMaxFreq Then
            MsgBox("Please enter a valid center frequency before applying changes.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid Frequency")
            pbError = True
        End If

        If Not pbError Then
            Me.DialogResult = DialogResult.OK
        End If
    End Sub
End Class