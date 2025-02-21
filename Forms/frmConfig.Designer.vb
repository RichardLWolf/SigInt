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
        Label2 = New Label()
        cboSigEventReset = New ComboBox()
        txtFrequency = New PaddedTextbox()
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
        cboSignalInit = New ComboBox()
        cboNFBaselineInit = New ComboBox()
        cboNFDetThresh = New ComboBox()
        cboNFMinDur = New ComboBox()
        cboNFCooldown = New ComboBox()
        cboNFReset = New ComboBox()
        lblMentionInfo = New Label()
        panInner = New Panel()
        Label20 = New Label()
        Label19 = New Label()
        Label18 = New Label()
        Label17 = New Label()
        Label16 = New Label()
        Label14 = New Label()
        Label15 = New Label()
        Label4 = New Label()
        Label13 = New Label()
        Label8 = New Label()
        Label1 = New Label()
        Label3 = New Label()
        Label10 = New Label()
        panDiscordVals = New Panel()
        Label6 = New Label()
        Label5 = New Label()
        Label9 = New Label()
        Label7 = New Label()
        Label11 = New Label()
        Label12 = New Label()
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
        lblCenter.Location = New Point(24, 51)
        lblCenter.Name = "lblCenter"
        lblCenter.Size = New Size(132, 21)
        lblCenter.TabIndex = 2
        lblCenter.Text = "&Center Frequency"
        ' 
        ' lblFrequency
        ' 
        lblFrequency.BorderStyle = BorderStyle.Fixed3D
        lblFrequency.Location = New Point(159, 48)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(146, 29)
        lblFrequency.TabIndex = 3
        lblFrequency.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' cboScale
        ' 
        cboScale.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboScale.DropDownStyle = ComboBoxStyle.DropDownList
        cboScale.ForeColor = Color.White
        cboScale.FormattingEnabled = True
        cboScale.Items.AddRange(New Object() {"Hz", "Khz", "Mhz", "Ghz"})
        cboScale.Location = New Point(477, 48)
        cboScale.Margin = New Padding(4)
        cboScale.MaxDropDownItems = 4
        cboScale.Name = "cboScale"
        cboScale.Size = New Size(81, 29)
        cboScale.TabIndex = 6
        ' 
        ' btnClearFreq
        ' 
        btnClearFreq.BackColor = Color.WhiteSmoke
        btnClearFreq.FlatAppearance.BorderSize = 0
        btnClearFreq.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnClearFreq.FlatStyle = FlatStyle.Flat
        btnClearFreq.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClearFreq.ForeColor = Color.LightCoral
        btnClearFreq.Location = New Point(447, 50)
        btnClearFreq.Name = "btnClearFreq"
        btnClearFreq.Size = New Size(23, 25)
        btnClearFreq.TabIndex = 5
        btnClearFreq.Text = "✖"
        btnClearFreq.UseVisualStyleBackColor = False
        ' 
        ' lblFreqRange
        ' 
        lblFreqRange.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblFreqRange.Location = New Point(105, 76)
        lblFreqRange.Name = "lblFreqRange"
        lblFreqRange.Size = New Size(255, 21)
        lblFreqRange.TabIndex = 7
        lblFreqRange.Text = "(--- range ---)"
        lblFreqRange.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' chkAutomatic
        ' 
        chkAutomatic.AutoSize = True
        chkAutomatic.Location = New Point(24, 168)
        chkAutomatic.Name = "chkAutomatic"
        chkAutomatic.Size = New Size(136, 25)
        chkAutomatic.TabIndex = 10
        chkAutomatic.Text = "Automatic &Gain"
        chkAutomatic.UseVisualStyleBackColor = True
        ' 
        ' sldGain
        ' 
        sldGain.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldGain.KnobColor = Color.DodgerBlue
        sldGain.Location = New Point(166, 220)
        sldGain.Maximum = 100
        sldGain.Minimum = 0
        sldGain.Name = "sldGain"
        sldGain.Size = New Size(382, 41)
        sldGain.TabIndex = 13
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
        lblMainGain.Location = New Point(39, 196)
        lblMainGain.Name = "lblMainGain"
        lblMainGain.Size = New Size(172, 21)
        lblMainGain.TabIndex = 11
        lblMainGain.Text = "&Manual Gain Value (dB)"
        ' 
        ' lblGainValue
        ' 
        lblGainValue.BorderStyle = BorderStyle.Fixed3D
        lblGainValue.Location = New Point(43, 226)
        lblGainValue.Name = "lblGainValue"
        lblGainValue.Size = New Size(105, 29)
        lblGainValue.TabIndex = 12
        lblGainValue.Text = "0 dB"
        lblGainValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(24, 508)
        Label2.Name = "Label2"
        Label2.Size = New Size(225, 21)
        Label2.TabIndex = 22
        Label2.Text = "Event Reset Duration (Minutes)"
        ' 
        ' cboSigEventReset
        ' 
        cboSigEventReset.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSigEventReset.DropDownStyle = ComboBoxStyle.DropDownList
        cboSigEventReset.ForeColor = Color.White
        cboSigEventReset.FormattingEnabled = True
        cboSigEventReset.Location = New Point(39, 533)
        cboSigEventReset.Margin = New Padding(4)
        cboSigEventReset.MaxDropDownItems = 4
        cboSigEventReset.Name = "cboSigEventReset"
        cboSigEventReset.Size = New Size(324, 29)
        cboSigEventReset.TabIndex = 23
        ToolTip1.SetToolTip(cboSigEventReset, "How much time must pass after recording a signal before a new signal may be detected.")
        ' 
        ' txtFrequency
        ' 
        txtFrequency.BackColor = Color.WhiteSmoke
        txtFrequency.ForeColor = Color.Black
        txtFrequency.Location = New Point(323, 48)
        txtFrequency.Name = "txtFrequency"
        txtFrequency.Size = New Size(147, 29)
        txtFrequency.TabIndex = 4
        txtFrequency.TextAlign = HorizontalAlignment.Right
        txtFrequency.TextPadding = New Padding(0, 0, 25, 0)
        ' 
        ' chkDiscordNotify
        ' 
        chkDiscordNotify.AutoSize = True
        chkDiscordNotify.Location = New Point(24, 1024)
        chkDiscordNotify.Name = "chkDiscordNotify"
        chkDiscordNotify.Size = New Size(429, 25)
        chkDiscordNotify.TabIndex = 38
        chkDiscordNotify.Text = "&Send Discord Notification On Signal Detection / App Error"
        chkDiscordNotify.UseVisualStyleBackColor = True
        ' 
        ' lblWebHook
        ' 
        lblWebHook.AutoSize = True
        lblWebHook.Location = New Point(3, 6)
        lblWebHook.Name = "lblWebHook"
        lblWebHook.Size = New Size(292, 21)
        lblWebHook.TabIndex = 0
        lblWebHook.Text = "&Discord Server Webhook URL (Required)"
        ' 
        ' txtDiscordServer
        ' 
        txtDiscordServer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDiscordServer.Location = New Point(6, 30)
        txtDiscordServer.Name = "txtDiscordServer"
        txtDiscordServer.Size = New Size(490, 29)
        txtDiscordServer.TabIndex = 1
        ToolTip1.SetToolTip(txtDiscordServer, "How To Create a webhook in Discord:" & vbCrLf & vbCrLf & "Go to Server Settings → Integrations → Webhooks." & vbCrLf & "Click ""New Webhook"" and copy the Webhook URL." & vbCrLf & vbCrLf)
        ' 
        ' txtDiscordMention
        ' 
        txtDiscordMention.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDiscordMention.Location = New Point(6, 86)
        txtDiscordMention.Name = "txtDiscordMention"
        txtDiscordMention.Size = New Size(490, 29)
        txtDiscordMention.TabIndex = 3
        ToolTip1.SetToolTip(txtDiscordMention, resources.GetString("txtDiscordMention.ToolTip"))
        ' 
        ' lblMention
        ' 
        lblMention.AutoSize = True
        lblMention.Location = New Point(3, 62)
        lblMention.Name = "lblMention"
        lblMention.Size = New Size(331, 21)
        lblMention.TabIndex = 2
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
        cboDetThresh.Location = New Point(40, 475)
        cboDetThresh.Margin = New Padding(4)
        cboDetThresh.MaxDropDownItems = 4
        cboDetThresh.Name = "cboDetThresh"
        cboDetThresh.Size = New Size(324, 29)
        cboDetThresh.TabIndex = 21
        ToolTip1.SetToolTip(cboDetThresh, "The minimum power level above the noise floor required to consider a signal detected.")
        ' 
        ' cboDetWind
        ' 
        cboDetWind.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDetWind.DropDownStyle = ComboBoxStyle.DropDownList
        cboDetWind.ForeColor = Color.White
        cboDetWind.FormattingEnabled = True
        cboDetWind.Location = New Point(39, 417)
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
        cboSampleRate.Location = New Point(39, 122)
        cboSampleRate.Margin = New Padding(4)
        cboSampleRate.MaxDropDownItems = 4
        cboSampleRate.Name = "cboSampleRate"
        cboSampleRate.Size = New Size(324, 29)
        cboSampleRate.TabIndex = 9
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
        ' cboSignalInit
        ' 
        cboSignalInit.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboSignalInit.DropDownStyle = ComboBoxStyle.DropDownList
        cboSignalInit.ForeColor = Color.White
        cboSignalInit.FormattingEnabled = True
        cboSignalInit.Location = New Point(39, 359)
        cboSignalInit.Margin = New Padding(4)
        cboSignalInit.MaxDropDownItems = 4
        cboSignalInit.Name = "cboSignalInit"
        cboSignalInit.Size = New Size(324, 29)
        cboSignalInit.TabIndex = 17
        ToolTip1.SetToolTip(cboSignalInit, "How much time to wait before looking for singal spikes once monitoring starts.")
        ' 
        ' cboNFBaselineInit
        ' 
        cboNFBaselineInit.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFBaselineInit.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFBaselineInit.ForeColor = Color.White
        cboNFBaselineInit.FormattingEnabled = True
        cboNFBaselineInit.Location = New Point(39, 667)
        cboNFBaselineInit.Margin = New Padding(4)
        cboNFBaselineInit.MaxDropDownItems = 4
        cboNFBaselineInit.Name = "cboNFBaselineInit"
        cboNFBaselineInit.Size = New Size(324, 29)
        cboNFBaselineInit.TabIndex = 27
        ToolTip1.SetToolTip(cboNFBaselineInit, "How much time to spend establishing a noise floor baseline before looking for events.")
        ' 
        ' cboNFDetThresh
        ' 
        cboNFDetThresh.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFDetThresh.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFDetThresh.ForeColor = Color.White
        cboNFDetThresh.FormattingEnabled = True
        cboNFDetThresh.Location = New Point(39, 725)
        cboNFDetThresh.Margin = New Padding(4)
        cboNFDetThresh.MaxDropDownItems = 4
        cboNFDetThresh.Name = "cboNFDetThresh"
        cboNFDetThresh.Size = New Size(324, 29)
        cboNFDetThresh.TabIndex = 29
        ToolTip1.SetToolTip(cboNFDetThresh, "How much noise floor dB rise must occur before declaring an event.")
        ' 
        ' cboNFMinDur
        ' 
        cboNFMinDur.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFMinDur.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFMinDur.ForeColor = Color.White
        cboNFMinDur.FormattingEnabled = True
        cboNFMinDur.Location = New Point(39, 783)
        cboNFMinDur.Margin = New Padding(4)
        cboNFMinDur.MaxDropDownItems = 4
        cboNFMinDur.Name = "cboNFMinDur"
        cboNFMinDur.Size = New Size(324, 29)
        cboNFMinDur.TabIndex = 31
        ToolTip1.SetToolTip(cboNFMinDur, "How long the rise must last before declaring an event" & vbCrLf)
        ' 
        ' cboNFCooldown
        ' 
        cboNFCooldown.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFCooldown.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFCooldown.ForeColor = Color.White
        cboNFCooldown.FormattingEnabled = True
        cboNFCooldown.Location = New Point(39, 841)
        cboNFCooldown.Margin = New Padding(4)
        cboNFCooldown.MaxDropDownItems = 4
        cboNFCooldown.Name = "cboNFCooldown"
        cboNFCooldown.Size = New Size(324, 29)
        cboNFCooldown.TabIndex = 33
        ToolTip1.SetToolTip(cboNFCooldown, "How long to wait after an event before resuming noise floor averaging.")
        ' 
        ' cboNFReset
        ' 
        cboNFReset.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboNFReset.DropDownStyle = ComboBoxStyle.DropDownList
        cboNFReset.ForeColor = Color.White
        cboNFReset.FormattingEnabled = True
        cboNFReset.Location = New Point(39, 899)
        cboNFReset.Margin = New Padding(4)
        cboNFReset.MaxDropDownItems = 4
        cboNFReset.Name = "cboNFReset"
        cboNFReset.Size = New Size(324, 29)
        cboNFReset.TabIndex = 35
        ToolTip1.SetToolTip(cboNFReset, "How long to wait before a new event may be detected.")
        ' 
        ' lblMentionInfo
        ' 
        lblMentionInfo.AutoSize = True
        lblMentionInfo.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblMentionInfo.Location = New Point(32, 118)
        lblMentionInfo.Name = "lblMentionInfo"
        lblMentionInfo.Size = New Size(367, 75)
        lblMentionInfo.TabIndex = 4
        lblMentionInfo.Text = resources.GetString("lblMentionInfo.Text")
        lblMentionInfo.UseMnemonic = False
        ' 
        ' panInner
        ' 
        panInner.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panInner.AutoScroll = True
        panInner.AutoScrollMargin = New Size(0, 25)
        panInner.BackColor = Color.FromArgb(CByte(55), CByte(55), CByte(55))
        panInner.Controls.Add(cboNFReset)
        panInner.Controls.Add(Label20)
        panInner.Controls.Add(cboNFCooldown)
        panInner.Controls.Add(Label19)
        panInner.Controls.Add(cboNFMinDur)
        panInner.Controls.Add(Label18)
        panInner.Controls.Add(cboNFDetThresh)
        panInner.Controls.Add(Label17)
        panInner.Controls.Add(cboNFBaselineInit)
        panInner.Controls.Add(Label16)
        panInner.Controls.Add(Label14)
        panInner.Controls.Add(Label15)
        panInner.Controls.Add(Label4)
        panInner.Controls.Add(Label13)
        panInner.Controls.Add(Label8)
        panInner.Controls.Add(cboSignalInit)
        panInner.Controls.Add(Label1)
        panInner.Controls.Add(Label3)
        panInner.Controls.Add(Label10)
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
        panInner.Controls.Add(lblFreqRange)
        panInner.Controls.Add(chkDiscordNotify)
        panInner.Controls.Add(chkAutomatic)
        panInner.Controls.Add(sldGain)
        panInner.Controls.Add(lblMainGain)
        panInner.Controls.Add(cboSigEventReset)
        panInner.Controls.Add(lblGainValue)
        panInner.Controls.Add(Label2)
        panInner.Controls.Add(txtFrequency)
        panInner.Controls.Add(Label11)
        panInner.Controls.Add(Label12)
        panInner.Location = New Point(12, 12)
        panInner.Name = "panInner"
        panInner.Size = New Size(598, 584)
        panInner.TabIndex = 0
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(24, 874)
        Label20.Name = "Label20"
        Label20.Size = New Size(225, 21)
        Label20.TabIndex = 34
        Label20.Text = "Event Reset Duration (Minutes)"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(24, 816)
        Label19.Name = "Label19"
        Label19.Size = New Size(293, 21)
        Label19.TabIndex = 32
        Label19.Text = "Averaging Cooldown Duration (Seconds)"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(24, 758)
        Label18.Name = "Label18"
        Label18.Size = New Size(257, 21)
        Label18.TabIndex = 30
        Label18.Text = "Minimum Event Duration (Seconds)"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(24, 700)
        Label17.Name = "Label17"
        Label17.Size = New Size(181, 21)
        Label17.TabIndex = 28
        Label17.Text = "Detection Threshold (dB)"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(24, 642)
        Label16.Name = "Label16"
        Label16.Size = New Size(274, 21)
        Label16.TabIndex = 26
        Label16.Text = "Detection Initialization Time (Seconds)"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.BackColor = Color.CornflowerBlue
        Label14.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label14.ForeColor = Color.White
        Label14.Location = New Point(11, 599)
        Label14.Name = "Label14"
        Label14.Padding = New Padding(4, 4, 4, 6)
        Label14.Size = New Size(184, 31)
        Label14.TabIndex = 24
        Label14.Text = "Noise Floor Detection"
        ' 
        ' Label15
        ' 
        Label15.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label15.BackColor = Color.RoyalBlue
        Label15.Location = New Point(190, 613)
        Label15.Name = "Label15"
        Label15.Size = New Size(377, 3)
        Label15.TabIndex = 25
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = Color.CornflowerBlue
        Label4.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label4.ForeColor = Color.White
        Label4.Location = New Point(5, 973)
        Label4.Name = "Label4"
        Label4.Padding = New Padding(4, 4, 4, 6)
        Label4.Size = New Size(180, 31)
        Label4.TabIndex = 36
        Label4.Text = "Discord Notifications"
        ' 
        ' Label13
        ' 
        Label13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label13.BackColor = Color.RoyalBlue
        Label13.Location = New Point(145, 987)
        Label13.Name = "Label13"
        Label13.Size = New Size(416, 3)
        Label13.TabIndex = 37
        ' 
        ' Label8
        ' 
        Label8.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label8.BackColor = Color.Silver
        Label8.Location = New Point(4, 1349)
        Label8.Name = "Label8"
        Label8.Size = New Size(567, 3)
        Label8.TabIndex = 40
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(24, 334)
        Label1.Name = "Label1"
        Label1.Size = New Size(274, 21)
        Label1.TabIndex = 16
        Label1.Text = "Detection Initialization Time (Seconds)"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.CornflowerBlue
        Label3.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label3.ForeColor = Color.White
        Label3.Location = New Point(11, 292)
        Label3.Name = "Label3"
        Label3.Padding = New Padding(4, 4, 4, 6)
        Label3.Size = New Size(145, 31)
        Label3.TabIndex = 14
        Label3.Text = "Signal Detection"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.BackColor = Color.CornflowerBlue
        Label10.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label10.ForeColor = Color.White
        Label10.Location = New Point(12, 11)
        Label10.Name = "Label10"
        Label10.Padding = New Padding(4, 4, 4, 6)
        Label10.Size = New Size(97, 31)
        Label10.TabIndex = 0
        Label10.Text = "Frequency"
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
        panDiscordVals.Location = New Point(36, 1049)
        panDiscordVals.Name = "panDiscordVals"
        panDiscordVals.Size = New Size(508, 215)
        panDiscordVals.TabIndex = 39
        panDiscordVals.Tag = ""
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label6.BackColor = Color.Silver
        Label6.Location = New Point(-24, 234)
        Label6.Name = "Label6"
        Label6.Size = New Size(564, 3)
        Label6.TabIndex = 5
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(24, 97)
        Label5.Name = "Label5"
        Label5.Size = New Size(97, 21)
        Label5.TabIndex = 8
        Label5.Text = "Sample &Rate"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(24, 392)
        Label9.Name = "Label9"
        Label9.Size = New Size(256, 21)
        Label9.TabIndex = 18
        Label9.Text = "Signal Detection &Window (FFT Bins)" & vbCrLf
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(25, 450)
        Label7.Name = "Label7"
        Label7.Size = New Size(273, 21)
        Label7.TabIndex = 20
        Label7.Text = "&Detection Threshold (dB Above Noise)" & vbCrLf
        ' 
        ' Label11
        ' 
        Label11.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label11.BackColor = Color.RoyalBlue
        Label11.Location = New Point(102, 25)
        Label11.Name = "Label11"
        Label11.Size = New Size(466, 3)
        Label11.TabIndex = 1
        ' 
        ' Label12
        ' 
        Label12.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label12.BackColor = Color.RoyalBlue
        Label12.Location = New Point(151, 306)
        Label12.Name = "Label12"
        Label12.Size = New Size(416, 3)
        Label12.TabIndex = 15
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
        DoubleBuffered = True
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
    Friend WithEvents Label2 As Label
    Friend WithEvents cboSigEventReset As ComboBox
    Friend WithEvents txtFrequency As PaddedTextbox
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
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents cboSignalInit As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents cboNFBaselineInit As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents cboNFDetThresh As ComboBox
    Friend WithEvents Label17 As Label
    Friend WithEvents cboNFMinDur As ComboBox
    Friend WithEvents Label18 As Label
    Friend WithEvents cboNFCooldown As ComboBox
    Friend WithEvents Label19 As Label
    Friend WithEvents cboNFReset As ComboBox
    Friend WithEvents Label20 As Label
End Class
