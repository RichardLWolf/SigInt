<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
        btnApply = New Button()
        btnCancel = New Button()
        lblCenter = New Label()
        lblFrequency = New Label()
        txtFrequency = New TextBox()
        cboScale = New ComboBox()
        btnClearFreq = New Button()
        Label3 = New Label()
        lblFreqRange = New Label()
        chkAutomatic = New CheckBox()
        sldGain = New CustomSlider()
        lblMainGain = New Label()
        lblGainValue = New Label()
        SuspendLayout()
        ' 
        ' btnApply
        ' 
        btnApply.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnApply.BackColor = Color.LightBlue
        btnApply.FlatAppearance.BorderColor = Color.Black
        btnApply.FlatAppearance.BorderSize = 0
        btnApply.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnApply.FlatStyle = FlatStyle.Flat
        btnApply.ForeColor = Color.Black
        btnApply.Location = New Point(12, 248)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 50)
        btnApply.TabIndex = 0
        btnApply.Text = "APPLY"
        btnApply.UseVisualStyleBackColor = False
        ' 
        ' btnCancel
        ' 
        btnCancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnCancel.BackColor = Color.Pink
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.FlatAppearance.BorderColor = Color.Black
        btnCancel.FlatAppearance.BorderSize = 0
        btnCancel.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.ForeColor = Color.Black
        btnCancel.Location = New Point(454, 248)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 1
        btnCancel.Text = "CANCEL"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' lblCenter
        ' 
        lblCenter.AutoSize = True
        lblCenter.Location = New Point(12, 28)
        lblCenter.Name = "lblCenter"
        lblCenter.Size = New Size(132, 21)
        lblCenter.TabIndex = 2
        lblCenter.Text = "&Center Frequency"
        ' 
        ' lblFrequency
        ' 
        lblFrequency.BorderStyle = BorderStyle.Fixed3D
        lblFrequency.Location = New Point(150, 24)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(146, 29)
        lblFrequency.TabIndex = 3
        lblFrequency.Text = "1,600,000,000 Hz"
        lblFrequency.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtFrequency
        ' 
        txtFrequency.BackColor = Color.WhiteSmoke
        txtFrequency.ForeColor = Color.Black
        txtFrequency.Location = New Point(309, 25)
        txtFrequency.Name = "txtFrequency"
        txtFrequency.Size = New Size(132, 29)
        txtFrequency.TabIndex = 4
        txtFrequency.Text = "1600000000"
        txtFrequency.TextAlign = HorizontalAlignment.Right
        ' 
        ' cboScale
        ' 
        cboScale.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboScale.ForeColor = Color.White
        cboScale.FormattingEnabled = True
        cboScale.Items.AddRange(New Object() {"Hz", "Khz", "Mhz", "Ghz"})
        cboScale.Location = New Point(473, 25)
        cboScale.Margin = New Padding(4)
        cboScale.MaxDropDownItems = 4
        cboScale.Name = "cboScale"
        cboScale.Size = New Size(81, 29)
        cboScale.TabIndex = 11
        ' 
        ' btnClearFreq
        ' 
        btnClearFreq.FlatAppearance.BorderSize = 0
        btnClearFreq.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnClearFreq.FlatStyle = FlatStyle.Flat
        btnClearFreq.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClearFreq.ForeColor = Color.LightCoral
        btnClearFreq.Location = New Point(442, 26)
        btnClearFreq.Name = "btnClearFreq"
        btnClearFreq.Size = New Size(23, 23)
        btnClearFreq.TabIndex = 12
        btnClearFreq.Text = "✖"
        btnClearFreq.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.BackColor = Color.Silver
        Label3.Location = New Point(12, 88)
        Label3.Name = "Label3"
        Label3.Size = New Size(540, 3)
        Label3.TabIndex = 30
        ' 
        ' lblFreqRange
        ' 
        lblFreqRange.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblFreqRange.Location = New Point(96, 53)
        lblFreqRange.Name = "lblFreqRange"
        lblFreqRange.Size = New Size(255, 21)
        lblFreqRange.TabIndex = 31
        lblFreqRange.Text = "(24 Hz to 1.7 Ghz)"
        lblFreqRange.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' chkAutomatic
        ' 
        chkAutomatic.AutoSize = True
        chkAutomatic.Location = New Point(12, 112)
        chkAutomatic.Name = "chkAutomatic"
        chkAutomatic.Size = New Size(136, 25)
        chkAutomatic.TabIndex = 32
        chkAutomatic.Text = "Automatic &Gain"
        chkAutomatic.UseVisualStyleBackColor = True
        ' 
        ' sldGain
        ' 
        sldGain.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldGain.KnobColor = Color.DodgerBlue
        sldGain.Location = New Point(150, 169)
        sldGain.Maximum = 100
        sldGain.Minimum = 0
        sldGain.Name = "sldGain"
        sldGain.Size = New Size(394, 41)
        sldGain.TabIndex = 33
        sldGain.TextColor = Color.White
        sldGain.TickColor = Color.LightGray
        sldGain.TickSpacing = 10
        sldGain.TrackColor = Color.Gray
        sldGain.TrackHighlightColor = Color.LightGray
        sldGain.Value = 0
        ' 
        ' lblMainGain
        ' 
        lblMainGain.AutoSize = True
        lblMainGain.Location = New Point(27, 145)
        lblMainGain.Name = "lblMainGain"
        lblMainGain.Size = New Size(172, 21)
        lblMainGain.TabIndex = 34
        lblMainGain.Text = "&Manual Gain Value (dB)"
        ' 
        ' lblGainValue
        ' 
        lblGainValue.BorderStyle = BorderStyle.Fixed3D
        lblGainValue.Location = New Point(27, 175)
        lblGainValue.Name = "lblGainValue"
        lblGainValue.Size = New Size(103, 29)
        lblGainValue.TabIndex = 35
        lblGainValue.Text = "0 dB"
        lblGainValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' frmConfig
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(566, 310)
        Controls.Add(lblGainValue)
        Controls.Add(lblMainGain)
        Controls.Add(sldGain)
        Controls.Add(chkAutomatic)
        Controls.Add(lblFreqRange)
        Controls.Add(Label3)
        Controls.Add(btnClearFreq)
        Controls.Add(cboScale)
        Controls.Add(txtFrequency)
        Controls.Add(lblFrequency)
        Controls.Add(lblCenter)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        FormBorderStyle = FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmConfig"
        StartPosition = FormStartPosition.CenterParent
        Text = "Configuration"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnApply As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblCenter As Label
    Friend WithEvents lblFrequency As Label
    Friend WithEvents txtFrequency As TextBox
    Friend WithEvents cboScale As ComboBox
    Friend WithEvents btnClearFreq As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents lblFreqRange As Label
    Friend WithEvents chkAutomatic As CheckBox
    Friend WithEvents sldGain As CustomSlider
    Friend WithEvents lblMainGain As Label
    Friend WithEvents lblGainValue As Label
End Class
