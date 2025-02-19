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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
        btnApply = New Button()
        btnCancel = New Button()
        lblCenter = New Label()
        lblFrequency = New Label()
        cboScale = New ComboBox()
        btnClearFreq = New Button()
        lblFreqRange = New Label()
        chkAutomatic = New CheckBox()
        sldGain = New CustomSlider()
        lblMainGain = New Label()
        lblGainValue = New Label()
        Label3 = New Label()
        Label1 = New Label()
        Label2 = New Label()
        cboMinEventWindow = New ComboBox()
        txtFrequency = New PaddedTextbox()
        Label4 = New Label()
        chkDiscordNotify = New CheckBox()
        lblWebHook = New Label()
        txtDiscordServer = New TextBox()
        txtDiscordMention = New TextBox()
        lblMention = New Label()
        ToolTip1 = New ToolTip(components)
        cboDetThresh = New ComboBox()
        cboDetWind = New ComboBox()
        cboSampleRate = New ComboBox()
        btnReset = New Button()
        lblMentionInfo = New Label()
        panInner = New Panel()
        Label8 = New Label()
        panDiscordVals = New Panel()
        Label6 = New Label()
        Label5 = New Label()
        Label9 = New Label()
        Label7 = New Label()
        panInner.SuspendLayout()
        panDiscordVals.SuspendLayout()
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
        btnApply.Location = New Point(12, 629)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 50)
        btnApply.TabIndex = 1
        btnApply.Text = "&APPLY"
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
        btnCancel.Location = New Point(510, 629)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 2
        btnCancel.Text = "CA&NCEL"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' lblCenter
        ' 
        lblCenter.AutoSize = True
        lblCenter.Location = New Point(11, 15)
        lblCenter.Name = "lblCenter"
        lblCenter.Size = New Size(132, 21)
        lblCenter.TabIndex = 0
        lblCenter.Text = "&Center Frequency"
        ' 
        ' lblFrequency
        ' 
        lblFrequency.BorderStyle = BorderStyle.Fixed3D
        lblFrequency.Location = New Point(146, 12)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(146, 29)
        lblFrequency.TabIndex = 1
        lblFrequency.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' cboScale
        ' 
        cboScale.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboScale.DropDownStyle = ComboBoxStyle.DropDownList
        cboScale.ForeColor = Color.White
        cboScale.FormattingEnabled = True
        cboScale.Items.AddRange(New Object() {"Hz", "Khz", "Mhz", "Ghz"})
        cboScale.Location = New Point(464, 12)
        cboScale.Margin = New Padding(4)
        cboScale.MaxDropDownItems = 4
        cboScale.Name = "cboScale"
        cboScale.Size = New Size(81, 29)
        cboScale.TabIndex = 4
        ' 
        ' btnClearFreq
        ' 
        btnClearFreq.BackColor = Color.WhiteSmoke
        btnClearFreq.FlatAppearance.BorderSize = 0
        btnClearFreq.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnClearFreq.FlatStyle = FlatStyle.Flat
        btnClearFreq.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClearFreq.ForeColor = Color.LightCoral
        btnClearFreq.Location = New Point(434, 14)
        btnClearFreq.Name = "btnClearFreq"
        btnClearFreq.Size = New Size(23, 25)
        btnClearFreq.TabIndex = 3
        btnClearFreq.Text = "✖"
        btnClearFreq.UseVisualStyleBackColor = False
        ' 
        ' lblFreqRange
        ' 
        lblFreqRange.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblFreqRange.Location = New Point(92, 40)
        lblFreqRange.Name = "lblFreqRange"
        lblFreqRange.Size = New Size(255, 21)
        lblFreqRange.TabIndex = 5
        lblFreqRange.Text = "(--- range ---)"
        lblFreqRange.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' chkAutomatic
        ' 
        chkAutomatic.AutoSize = True
        chkAutomatic.Location = New Point(11, 174)
        chkAutomatic.Name = "chkAutomatic"
        chkAutomatic.Size = New Size(136, 25)
        chkAutomatic.TabIndex = 9
        chkAutomatic.Text = "Automatic &Gain"
        chkAutomatic.UseVisualStyleBackColor = True
        ' 
        ' sldGain
        ' 
        sldGain.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldGain.KnobColor = Color.DodgerBlue
        sldGain.Location = New Point(153, 226)
        sldGain.Maximum = 100
        sldGain.Minimum = 0
        sldGain.Name = "sldGain"
        sldGain.Size = New Size(382, 41)
        sldGain.TabIndex = 12
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
        lblMainGain.Location = New Point(26, 202)
        lblMainGain.Name = "lblMainGain"
        lblMainGain.Size = New Size(172, 21)
        lblMainGain.TabIndex = 10
        lblMainGain.Text = "&Manual Gain Value (dB)"
        ' 
        ' lblGainValue
        ' 
        lblGainValue.BorderStyle = BorderStyle.Fixed3D
        lblGainValue.Location = New Point(30, 232)
        lblGainValue.Name = "lblGainValue"
        lblGainValue.Size = New Size(105, 29)
        lblGainValue.TabIndex = 11
        lblGainValue.Text = "0 dB"
        lblGainValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.BackColor = Color.Silver
        Label3.Location = New Point(3, 145)
        Label3.Name = "Label3"
        Label3.Size = New Size(567, 3)
        Label3.TabIndex = 8
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.BackColor = Color.Silver
        Label1.Location = New Point(4, 301)
        Label1.Name = "Label1"
        Label1.Size = New Size(567, 3)
        Label1.TabIndex = 13
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(11, 319)
        Label2.Name = "Label2"
        Label2.Size = New Size(257, 21)
        Label2.TabIndex = 14
        Label2.Text = "Minimum &Event Recording Window"
        ' 
        ' cboMinEventWindow
        ' 
        cboMinEventWindow.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboMinEventWindow.DropDownStyle = ComboBoxStyle.DropDownList
        cboMinEventWindow.ForeColor = Color.White
        cboMinEventWindow.FormattingEnabled = True
        cboMinEventWindow.Location = New Point(26, 344)
        cboMinEventWindow.Margin = New Padding(4)
        cboMinEventWindow.MaxDropDownItems = 4
        cboMinEventWindow.Name = "cboMinEventWindow"
        cboMinEventWindow.Size = New Size(324, 29)
        cboMinEventWindow.TabIndex = 15
        ToolTip1.SetToolTip(cboMinEventWindow, "How much time must pass after recording a signal before a new signal may be detected.")
        ' 
        ' txtFrequency
        ' 
        txtFrequency.BackColor = Color.WhiteSmoke
        txtFrequency.ForeColor = Color.Black
        txtFrequency.Location = New Point(310, 12)
        txtFrequency.Name = "txtFrequency"
        txtFrequency.Size = New Size(147, 29)
        txtFrequency.TabIndex = 2
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.BackColor = Color.Silver
        Label4.Location = New Point(4, 510)
        Label4.Name = "Label4"
        Label4.Size = New Size(567, 3)
        Label4.TabIndex = 20
        ' 
        ' chkDiscordNotify
        ' 
        chkDiscordNotify.AutoSize = True
        chkDiscordNotify.Location = New Point(12, 531)
        chkDiscordNotify.Name = "chkDiscordNotify"
        chkDiscordNotify.Size = New Size(348, 25)
        chkDiscordNotify.TabIndex = 21
        chkDiscordNotify.Text = "&Send Discord Notification On Signal Detection"
        chkDiscordNotify.UseVisualStyleBackColor = True
        ' 
        ' lblWebHook
        ' 
        lblWebHook.AutoSize = True
        lblWebHook.Location = New Point(3, 6)
        lblWebHook.Name = "lblWebHook"
        lblWebHook.Size = New Size(292, 21)
        lblWebHook.TabIndex = 22
        lblWebHook.Text = "&Discord Server Webhook URL (Required)"
        ' 
        ' txtDiscordServer
        ' 
        txtDiscordServer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDiscordServer.Location = New Point(6, 30)
        txtDiscordServer.Name = "txtDiscordServer"
        txtDiscordServer.Size = New Size(493, 29)
        txtDiscordServer.TabIndex = 16
        ToolTip1.SetToolTip(txtDiscordServer, "How To Create a webhook in Discord:" & vbCrLf & vbCrLf & "Go to Server Settings → Integrations → Webhooks." & vbCrLf & "Click ""New Webhook"" and copy the Webhook URL." & vbCrLf & vbCrLf)
        ' 
        ' txtDiscordMention
        ' 
        txtDiscordMention.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDiscordMention.Location = New Point(6, 86)
        txtDiscordMention.Name = "txtDiscordMention"
        txtDiscordMention.Size = New Size(493, 29)
        txtDiscordMention.TabIndex = 24
        ToolTip1.SetToolTip(txtDiscordMention, resources.GetString("txtDiscordMention.ToolTip"))
        ' 
        ' lblMention
        ' 
        lblMention.AutoSize = True
        lblMention.Location = New Point(3, 62)
        lblMention.Name = "lblMention"
        lblMention.Size = New Size(331, 21)
        lblMention.TabIndex = 23
        lblMention.Text = "Discord C&hannel @UserID/@RoleID (Optional)"
        ' 
        ' ToolTip1
        ' 
        ToolTip1.AutoPopDelay = 10000
        ToolTip1.InitialDelay = 500
        ToolTip1.ReshowDelay = 100
        ' 
        ' cboDetThresh
        ' 
        cboDetThresh.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDetThresh.DropDownStyle = ComboBoxStyle.DropDownList
        cboDetThresh.ForeColor = Color.White
        cboDetThresh.FormattingEnabled = True
        cboDetThresh.Location = New Point(27, 402)
        cboDetThresh.Margin = New Padding(4)
        cboDetThresh.MaxDropDownItems = 4
        cboDetThresh.Name = "cboDetThresh"
        cboDetThresh.Size = New Size(324, 29)
        cboDetThresh.TabIndex = 17
        ToolTip1.SetToolTip(cboDetThresh, "The minimum power level above the noise floor required to consider a signal detected.")
        ' 
        ' cboDetWind
        ' 
        cboDetWind.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDetWind.DropDownStyle = ComboBoxStyle.DropDownList
        cboDetWind.ForeColor = Color.White
        cboDetWind.FormattingEnabled = True
        cboDetWind.Location = New Point(26, 460)
        cboDetWind.Margin = New Padding(4)
        cboDetWind.MaxDropDownItems = 4
        cboDetWind.Name = "cboDetWind"
        cboDetWind.Size = New Size(324, 29)
        cboDetWind.TabIndex = 19
        ToolTip1.SetToolTip(cboDetWind, "The number of FFT bins around the center frequency used to calculate the signal power.")
        ' 
        ' cboSampleRate
        ' 
        cboSampleRate.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSampleRate.DropDownStyle = ComboBoxStyle.DropDownList
        cboSampleRate.ForeColor = Color.White
        cboSampleRate.FormattingEnabled = True
        cboSampleRate.Location = New Point(26, 86)
        cboSampleRate.Margin = New Padding(4)
        cboSampleRate.MaxDropDownItems = 4
        cboSampleRate.Name = "cboSampleRate"
        cboSampleRate.Size = New Size(324, 29)
        cboSampleRate.TabIndex = 7
        ToolTip1.SetToolTip(cboSampleRate, "Mega Samples Per Second (how many samples per second the SDR captures)." & vbCrLf & "NOTE: Larger sample sizes will result in larger signal archive sizes.")
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
        btnReset.Location = New Point(261, 629)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(100, 50)
        btnReset.TabIndex = 3
        btnReset.Text = "RESET"
        ToolTip1.SetToolTip(btnReset, "Reset all settings to default values.")
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' lblMentionInfo
        ' 
        lblMentionInfo.AutoSize = True
        lblMentionInfo.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblMentionInfo.Location = New Point(32, 118)
        lblMentionInfo.Name = "lblMentionInfo"
        lblMentionInfo.Size = New Size(367, 75)
        lblMentionInfo.TabIndex = 25
        lblMentionInfo.Text = resources.GetString("lblMentionInfo.Text")
        lblMentionInfo.UseMnemonic = False
        ' 
        ' panInner
        ' 
        panInner.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panInner.AutoScroll = True
        panInner.AutoScrollMargin = New Size(0, 25)
        panInner.BackColor = Color.FromArgb(CByte(55), CByte(55), CByte(55))
        panInner.Controls.Add(Label8)
        panInner.Controls.Add(panDiscordVals)
        panInner.Controls.Add(cboSampleRate)
        panInner.Controls.Add(Label5)
        panInner.Controls.Add(btnClearFreq)
        panInner.Controls.Add(cboDetWind)
        panInner.Controls.Add(Label9)
        panInner.Controls.Add(cboDetThresh)
        panInner.Controls.Add(Label7)
        panInner.Controls.Add(lblCenter)
        panInner.Controls.Add(lblFrequency)
        panInner.Controls.Add(cboScale)
        panInner.Controls.Add(Label3)
        panInner.Controls.Add(lblFreqRange)
        panInner.Controls.Add(chkDiscordNotify)
        panInner.Controls.Add(chkAutomatic)
        panInner.Controls.Add(Label4)
        panInner.Controls.Add(sldGain)
        panInner.Controls.Add(lblMainGain)
        panInner.Controls.Add(cboMinEventWindow)
        panInner.Controls.Add(lblGainValue)
        panInner.Controls.Add(Label2)
        panInner.Controls.Add(Label1)
        panInner.Controls.Add(txtFrequency)
        panInner.Location = New Point(12, 12)
        panInner.Name = "panInner"
        panInner.Size = New Size(598, 584)
        panInner.TabIndex = 0
        ' 
        ' Label8
        ' 
        Label8.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label8.BackColor = Color.Silver
        Label8.Location = New Point(4, 787)
        Label8.Name = "Label8"
        Label8.Size = New Size(567, 3)
        Label8.TabIndex = 27
        ' 
        ' panDiscordVals
        ' 
        panDiscordVals.BackColor = Color.Transparent
        panDiscordVals.Controls.Add(Label6)
        panDiscordVals.Controls.Add(lblWebHook)
        panDiscordVals.Controls.Add(txtDiscordServer)
        panDiscordVals.Controls.Add(lblMention)
        panDiscordVals.Controls.Add(txtDiscordMention)
        panDiscordVals.Controls.Add(lblMentionInfo)
        panDiscordVals.Location = New Point(24, 556)
        panDiscordVals.Name = "panDiscordVals"
        panDiscordVals.Size = New Size(511, 215)
        panDiscordVals.TabIndex = 26
        panDiscordVals.Tag = ""
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label6.BackColor = Color.Silver
        Label6.Location = New Point(-24, 234)
        Label6.Name = "Label6"
        Label6.Size = New Size(567, 3)
        Label6.TabIndex = 27
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(11, 61)
        Label5.Name = "Label5"
        Label5.Size = New Size(97, 21)
        Label5.TabIndex = 6
        Label5.Text = "Sample &Rate"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(11, 435)
        Label9.Name = "Label9"
        Label9.Size = New Size(256, 21)
        Label9.TabIndex = 18
        Label9.Text = "Signal Detection &Window (FFT Bins)" & vbCrLf
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(12, 377)
        Label7.Name = "Label7"
        Label7.Size = New Size(273, 21)
        Label7.TabIndex = 16
        Label7.Text = "&Detection Threshold (dB Above Noise)" & vbCrLf
        ' 
        ' frmConfig
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(622, 691)
        Controls.Add(btnReset)
        Controls.Add(panInner)
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
        panInner.ResumeLayout(False)
        panInner.PerformLayout()
        panDiscordVals.ResumeLayout(False)
        panDiscordVals.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnApply As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblCenter As Label
    Friend WithEvents lblFrequency As Label
    Friend WithEvents cboScale As ComboBox
    Friend WithEvents btnClearFreq As Button
    Friend WithEvents lblFreqRange As Label
    Friend WithEvents chkAutomatic As CheckBox
    Friend WithEvents sldGain As CustomSlider
    Friend WithEvents lblMainGain As Label
    Friend WithEvents lblGainValue As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cboMinEventWindow As ComboBox
    Friend WithEvents txtFrequency As PaddedTextbox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkDiscordNotify As CheckBox
    Friend WithEvents lblWebHook As Label
    Friend WithEvents txtDiscordServer As TextBox
    Friend WithEvents txtDiscordMention As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblMention As Label
    Friend WithEvents lblMentionInfo As Label
    Friend WithEvents panInner As Panel
    Friend WithEvents cboDetThresh As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cboDetWind As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cboSampleRate As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents btnReset As Button
    Friend WithEvents panDiscordVals As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
End Class
