<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditConfig
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditConfig))
        Label10 = New Label()
        Label11 = New Label()
        Label1 = New Label()
        txtConfigName = New TextBox()
        cboSampleRate = New ComboBox()
        Label5 = New Label()
        btnClearFreq = New Button()
        lblCenter = New Label()
        lblFrequency = New Label()
        cboScale = New ComboBox()
        txtFrequency = New PaddedTextbox()
        Label3 = New Label()
        Label12 = New Label()
        cboSignalInit = New ComboBox()
        Label2 = New Label()
        cboDetWind = New ComboBox()
        Label9 = New Label()
        cboDetThresh = New ComboBox()
        Label7 = New Label()
        cboSigEventReset = New ComboBox()
        Label4 = New Label()
        cboNFReset = New ComboBox()
        Label20 = New Label()
        cboNFCooldown = New ComboBox()
        Label19 = New Label()
        cboNFMinDur = New ComboBox()
        Label18 = New Label()
        cboNFDetThresh = New ComboBox()
        Label17 = New Label()
        cboNFBaselineInit = New ComboBox()
        Label16 = New Label()
        Label14 = New Label()
        Label15 = New Label()
        btnReset = New Button()
        btnCancel = New Button()
        btnApply = New Button()
        cboSignalGain = New ComboBox()
        Label6 = New Label()
        ToolTip1 = New ToolTip(components)
        SuspendLayout()
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.BackColor = Color.CornflowerBlue
        Label10.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label10.ForeColor = Color.White
        Label10.Location = New Point(12, 9)
        Label10.Name = "Label10"
        Label10.Padding = New Padding(4, 4, 4, 6)
        Label10.Size = New Size(125, 31)
        Label10.TabIndex = 0
        Label10.Text = "Configuration"
        ' 
        ' Label11
        ' 
        Label11.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label11.BackColor = Color.RoyalBlue
        Label11.Location = New Point(102, 23)
        Label11.Name = "Label11"
        Label11.Size = New Size(829, 3)
        Label11.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 50)
        Label1.Name = "Label1"
        Label1.Size = New Size(152, 21)
        Label1.TabIndex = 2
        Label1.Text = "Configuration Name"
        ' 
        ' txtConfigName
        ' 
        txtConfigName.Location = New Point(170, 47)
        txtConfigName.MaxLength = 100
        txtConfigName.Name = "txtConfigName"
        txtConfigName.Size = New Size(205, 29)
        txtConfigName.TabIndex = 3
        ' 
        ' cboSampleRate
        ' 
        cboSampleRate.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSampleRate.DropDownStyle = ComboBoxStyle.DropDownList
        cboSampleRate.ForeColor = Color.White
        cboSampleRate.FormattingEnabled = True
        cboSampleRate.Location = New Point(532, 84)
        cboSampleRate.Margin = New Padding(4)
        cboSampleRate.MaxDropDownItems = 4
        cboSampleRate.Name = "cboSampleRate"
        cboSampleRate.Size = New Size(311, 29)
        cboSampleRate.TabIndex = 12
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(428, 87)
        Label5.Name = "Label5"
        Label5.Size = New Size(97, 21)
        Label5.TabIndex = 11
        Label5.Text = "Sample &Rate"
        ' 
        ' btnClearFreq
        ' 
        btnClearFreq.BackColor = Color.WhiteSmoke
        btnClearFreq.FlatAppearance.BorderSize = 0
        btnClearFreq.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnClearFreq.FlatStyle = FlatStyle.Flat
        btnClearFreq.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClearFreq.ForeColor = Color.LightCoral
        btnClearFreq.Location = New Point(820, 48)
        btnClearFreq.Name = "btnClearFreq"
        btnClearFreq.Size = New Size(23, 25)
        btnClearFreq.TabIndex = 7
        btnClearFreq.Text = "✖"
        btnClearFreq.UseVisualStyleBackColor = False
        ' 
        ' lblCenter
        ' 
        lblCenter.AutoSize = True
        lblCenter.Location = New Point(397, 49)
        lblCenter.Name = "lblCenter"
        lblCenter.Size = New Size(132, 21)
        lblCenter.TabIndex = 4
        lblCenter.Text = "&Center Frequency"
        ' 
        ' lblFrequency
        ' 
        lblFrequency.BorderStyle = BorderStyle.Fixed3D
        lblFrequency.Location = New Point(532, 46)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(146, 29)
        lblFrequency.TabIndex = 5
        lblFrequency.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' cboScale
        ' 
        cboScale.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboScale.DropDownStyle = ComboBoxStyle.DropDownList
        cboScale.ForeColor = Color.White
        cboScale.FormattingEnabled = True
        cboScale.Items.AddRange(New Object() {"Hz", "Khz", "Mhz", "Ghz"})
        cboScale.Location = New Point(850, 46)
        cboScale.Margin = New Padding(4)
        cboScale.MaxDropDownItems = 4
        cboScale.Name = "cboScale"
        cboScale.Size = New Size(81, 29)
        cboScale.TabIndex = 8
        ' 
        ' txtFrequency
        ' 
        txtFrequency.BackColor = Color.WhiteSmoke
        txtFrequency.ForeColor = Color.Black
        txtFrequency.Location = New Point(696, 46)
        txtFrequency.Name = "txtFrequency"
        txtFrequency.Size = New Size(147, 29)
        txtFrequency.TabIndex = 6
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.CornflowerBlue
        Label3.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label3.ForeColor = Color.White
        Label3.Location = New Point(12, 138)
        Label3.Name = "Label3"
        Label3.Padding = New Padding(4, 4, 4, 6)
        Label3.Size = New Size(145, 31)
        Label3.TabIndex = 13
        Label3.Text = "Signal Detection"
        ' 
        ' Label12
        ' 
        Label12.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label12.BackColor = Color.RoyalBlue
        Label12.Location = New Point(139, 152)
        Label12.Name = "Label12"
        Label12.Size = New Size(792, 3)
        Label12.TabIndex = 14
        ' 
        ' cboSignalInit
        ' 
        cboSignalInit.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSignalInit.DropDownStyle = ComboBoxStyle.DropDownList
        cboSignalInit.ForeColor = Color.White
        cboSignalInit.FormattingEnabled = True
        cboSignalInit.Location = New Point(12, 210)
        cboSignalInit.Margin = New Padding(4)
        cboSignalInit.MaxDropDownItems = 4
        cboSignalInit.Name = "cboSignalInit"
        cboSignalInit.Size = New Size(300, 29)
        cboSignalInit.TabIndex = 16
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 185)
        Label2.Name = "Label2"
        Label2.Size = New Size(274, 21)
        Label2.TabIndex = 15
        Label2.Text = "Detection Initialization Time (Seconds)"
        ' 
        ' cboDetWind
        ' 
        cboDetWind.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDetWind.DropDownStyle = ComboBoxStyle.DropDownList
        cboDetWind.ForeColor = Color.White
        cboDetWind.FormattingEnabled = True
        cboDetWind.Location = New Point(320, 210)
        cboDetWind.Margin = New Padding(4)
        cboDetWind.MaxDropDownItems = 4
        cboDetWind.Name = "cboDetWind"
        cboDetWind.Size = New Size(300, 29)
        cboDetWind.TabIndex = 18
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(320, 185)
        Label9.Name = "Label9"
        Label9.Size = New Size(256, 21)
        Label9.TabIndex = 17
        Label9.Text = "Signal Detection &Window (FFT Bins)" & vbCrLf
        ' 
        ' cboDetThresh
        ' 
        cboDetThresh.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDetThresh.DropDownStyle = ComboBoxStyle.DropDownList
        cboDetThresh.ForeColor = Color.White
        cboDetThresh.FormattingEnabled = True
        cboDetThresh.Location = New Point(628, 210)
        cboDetThresh.Margin = New Padding(4)
        cboDetThresh.MaxDropDownItems = 4
        cboDetThresh.Name = "cboDetThresh"
        cboDetThresh.Size = New Size(300, 29)
        cboDetThresh.TabIndex = 20
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(628, 185)
        Label7.Name = "Label7"
        Label7.Size = New Size(273, 21)
        Label7.TabIndex = 19
        Label7.Text = "&Detection Threshold (dB Above Noise)" & vbCrLf
        ' 
        ' cboSigEventReset
        ' 
        cboSigEventReset.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSigEventReset.DropDownStyle = ComboBoxStyle.DropDownList
        cboSigEventReset.ForeColor = Color.White
        cboSigEventReset.FormattingEnabled = True
        cboSigEventReset.Location = New Point(12, 268)
        cboSigEventReset.Margin = New Padding(4)
        cboSigEventReset.MaxDropDownItems = 4
        cboSigEventReset.Name = "cboSigEventReset"
        cboSigEventReset.Size = New Size(300, 29)
        cboSigEventReset.TabIndex = 22
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(12, 243)
        Label4.Name = "Label4"
        Label4.Size = New Size(225, 21)
        Label4.TabIndex = 21
        Label4.Text = "Event Reset Duration (Minutes)"
        ' 
        ' cboNFReset
        ' 
        cboNFReset.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFReset.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFReset.ForeColor = Color.White
        cboNFReset.FormattingEnabled = True
        cboNFReset.Location = New Point(320, 456)
        cboNFReset.Margin = New Padding(4)
        cboNFReset.MaxDropDownItems = 4
        cboNFReset.Name = "cboNFReset"
        cboNFReset.Size = New Size(300, 29)
        cboNFReset.TabIndex = 34
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(320, 431)
        Label20.Name = "Label20"
        Label20.Size = New Size(225, 21)
        Label20.TabIndex = 33
        Label20.Text = "Event Reset Duration (Minutes)"
        ' 
        ' cboNFCooldown
        ' 
        cboNFCooldown.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFCooldown.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFCooldown.ForeColor = Color.White
        cboNFCooldown.FormattingEnabled = True
        cboNFCooldown.Location = New Point(12, 456)
        cboNFCooldown.Margin = New Padding(4)
        cboNFCooldown.MaxDropDownItems = 4
        cboNFCooldown.Name = "cboNFCooldown"
        cboNFCooldown.Size = New Size(300, 29)
        cboNFCooldown.TabIndex = 32
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(12, 431)
        Label19.Name = "Label19"
        Label19.Size = New Size(293, 21)
        Label19.TabIndex = 31
        Label19.Text = "Averaging Cooldown Duration (Seconds)"
        ' 
        ' cboNFMinDur
        ' 
        cboNFMinDur.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFMinDur.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFMinDur.ForeColor = Color.White
        cboNFMinDur.FormattingEnabled = True
        cboNFMinDur.Location = New Point(628, 398)
        cboNFMinDur.Margin = New Padding(4)
        cboNFMinDur.MaxDropDownItems = 4
        cboNFMinDur.Name = "cboNFMinDur"
        cboNFMinDur.Size = New Size(300, 29)
        cboNFMinDur.TabIndex = 30
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(628, 373)
        Label18.Name = "Label18"
        Label18.Size = New Size(257, 21)
        Label18.TabIndex = 29
        Label18.Text = "Minimum Event Duration (Seconds)"
        ' 
        ' cboNFDetThresh
        ' 
        cboNFDetThresh.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFDetThresh.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFDetThresh.ForeColor = Color.White
        cboNFDetThresh.FormattingEnabled = True
        cboNFDetThresh.Location = New Point(320, 398)
        cboNFDetThresh.Margin = New Padding(4)
        cboNFDetThresh.MaxDropDownItems = 4
        cboNFDetThresh.Name = "cboNFDetThresh"
        cboNFDetThresh.Size = New Size(300, 29)
        cboNFDetThresh.TabIndex = 28
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(320, 373)
        Label17.Name = "Label17"
        Label17.Size = New Size(263, 21)
        Label17.TabIndex = 27
        Label17.Text = "Detection Threshold (dB Above Avg.)"
        ' 
        ' cboNFBaselineInit
        ' 
        cboNFBaselineInit.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFBaselineInit.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFBaselineInit.ForeColor = Color.White
        cboNFBaselineInit.FormattingEnabled = True
        cboNFBaselineInit.Location = New Point(12, 398)
        cboNFBaselineInit.Margin = New Padding(4)
        cboNFBaselineInit.MaxDropDownItems = 4
        cboNFBaselineInit.Name = "cboNFBaselineInit"
        cboNFBaselineInit.Size = New Size(300, 29)
        cboNFBaselineInit.TabIndex = 26
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(12, 373)
        Label16.Name = "Label16"
        Label16.Size = New Size(274, 21)
        Label16.TabIndex = 25
        Label16.Text = "Detection Initialization Time (Seconds)"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.BackColor = Color.CornflowerBlue
        Label14.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label14.ForeColor = Color.White
        Label14.Location = New Point(12, 328)
        Label14.Name = "Label14"
        Label14.Padding = New Padding(4, 4, 4, 6)
        Label14.Size = New Size(184, 31)
        Label14.TabIndex = 23
        Label14.Text = "Noise Floor Detection"
        ' 
        ' Label15
        ' 
        Label15.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label15.BackColor = Color.RoyalBlue
        Label15.Location = New Point(170, 342)
        Label15.Name = "Label15"
        Label15.Size = New Size(761, 3)
        Label15.TabIndex = 24
        ' 
        ' btnReset
        ' 
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnReset.BackColor = SystemColors.ActiveBorder
        btnReset.FlatAppearance.BorderColor = Color.Black
        btnReset.FlatAppearance.BorderSize = 0
        btnReset.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.ForeColor = Color.Black
        btnReset.Location = New Point(422, 524)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(100, 50)
        btnReset.TabIndex = 43
        btnReset.Text = "RESET"
        btnReset.UseVisualStyleBackColor = False
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
        btnCancel.Location = New Point(833, 524)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 44
        btnCancel.Text = "CA&NCEL"
        btnCancel.UseVisualStyleBackColor = False
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
        btnApply.Location = New Point(12, 524)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 50)
        btnApply.TabIndex = 42
        btnApply.Text = "&APPLY"
        btnApply.UseVisualStyleBackColor = False
        ' 
        ' cboSignalGain
        ' 
        cboSignalGain.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSignalGain.DropDownStyle = ComboBoxStyle.DropDownList
        cboSignalGain.ForeColor = Color.White
        cboSignalGain.FormattingEnabled = True
        cboSignalGain.Location = New Point(170, 83)
        cboSignalGain.Margin = New Padding(4)
        cboSignalGain.MaxDropDownItems = 4
        cboSignalGain.Name = "cboSignalGain"
        cboSignalGain.Size = New Size(205, 29)
        cboSignalGain.TabIndex = 10
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(75, 87)
        Label6.Name = "Label6"
        Label6.Size = New Size(89, 21)
        Label6.TabIndex = 9
        Label6.Text = "Signal &Gain"
        ' 
        ' frmEditConfig
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(945, 586)
        Controls.Add(btnClearFreq)
        Controls.Add(Label6)
        Controls.Add(cboSignalGain)
        Controls.Add(btnReset)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Controls.Add(Label14)
        Controls.Add(cboNFReset)
        Controls.Add(Label20)
        Controls.Add(cboNFCooldown)
        Controls.Add(Label19)
        Controls.Add(cboNFMinDur)
        Controls.Add(Label18)
        Controls.Add(cboNFDetThresh)
        Controls.Add(Label17)
        Controls.Add(cboNFBaselineInit)
        Controls.Add(Label16)
        Controls.Add(cboSignalInit)
        Controls.Add(Label2)
        Controls.Add(cboDetWind)
        Controls.Add(Label9)
        Controls.Add(cboDetThresh)
        Controls.Add(Label7)
        Controls.Add(cboSigEventReset)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(cboSampleRate)
        Controls.Add(Label5)
        Controls.Add(lblCenter)
        Controls.Add(lblFrequency)
        Controls.Add(cboScale)
        Controls.Add(txtFrequency)
        Controls.Add(txtConfigName)
        Controls.Add(Label1)
        Controls.Add(Label10)
        Controls.Add(Label11)
        Controls.Add(Label12)
        Controls.Add(Label15)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.WhiteSmoke
        FormBorderStyle = FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmEditConfig"
        StartPosition = FormStartPosition.CenterParent
        Text = "Editing Configuration"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtConfigName As TextBox
    Friend WithEvents cboSampleRate As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents btnClearFreq As Button
    Friend WithEvents lblCenter As Label
    Friend WithEvents lblFrequency As Label
    Friend WithEvents cboScale As ComboBox
    Friend WithEvents txtFrequency As PaddedTextbox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents cboSignalInit As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cboDetWind As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cboDetThresh As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cboSigEventReset As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cboNFReset As ComboBox
    Friend WithEvents Label20 As Label
    Friend WithEvents cboNFCooldown As ComboBox
    Friend WithEvents Label19 As Label
    Friend WithEvents cboNFMinDur As ComboBox
    Friend WithEvents Label18 As Label
    Friend WithEvents cboNFDetThresh As ComboBox
    Friend WithEvents Label17 As Label
    Friend WithEvents cboNFBaselineInit As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents btnReset As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents cboSignalGain As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
