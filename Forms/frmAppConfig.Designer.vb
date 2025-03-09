<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAppConfig
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAppConfig))
        btnApply = New Button()
        btnCancel = New Button()
        chkDiscordNotify = New CheckBox()
        lblWebHook = New Label()
        txtDiscordServer = New TextBox()
        txtDiscordMention = New TextBox()
        lblMention = New Label()
        ToolTip1 = New ToolTip(components)
        btnReset = New Button()
        btnTestDiscord = New Button()
        btnLookupIP = New Button()
        txtLon = New TextBox()
        txtLat = New TextBox()
        chkThingSpeak = New CheckBox()
        lblMentionInfo = New Label()
        Label4 = New Label()
        Label13 = New Label()
        panDiscordVals = New Panel()
        Label6 = New Label()
        lblLon = New Label()
        lblLat = New Label()
        Label8 = New Label()
        Label1 = New Label()
        panThingSpeak = New Panel()
        lnkThingSpeak = New LinkLabel()
        panDiscordVals.SuspendLayout()
        panThingSpeak.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnApply
        ' 
        btnApply.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnApply.BackColor = Color.LightBlue
        btnApply.FlatAppearance.BorderColor = Color.Black
        btnApply.FlatAppearance.BorderSize = 0
        btnApply.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnApply.FlatStyle = FlatStyle.Flat
        btnApply.ForeColor = Color.Black
        btnApply.Location = New Point(12, 543)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 50)
        btnApply.TabIndex = 4
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
        btnCancel.Location = New Point(450, 543)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 6
        btnCancel.Text = "CA&NCEL"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' chkDiscordNotify
        ' 
        chkDiscordNotify.AutoSize = True
        chkDiscordNotify.Location = New Point(31, 60)
        chkDiscordNotify.Name = "chkDiscordNotify"
        chkDiscordNotify.Size = New Size(429, 25)
        chkDiscordNotify.TabIndex = 2
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
        ' btnReset
        ' 
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnReset.BackColor = SystemColors.ActiveBorder
        btnReset.FlatAppearance.BorderColor = Color.Black
        btnReset.FlatAppearance.BorderSize = 0
        btnReset.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.ForeColor = Color.Black
        btnReset.Location = New Point(231, 543)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(100, 50)
        btnReset.TabIndex = 5
        btnReset.Text = "CLEAR"
        ToolTip1.SetToolTip(btnReset, "Reset all settings to default values.")
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' btnTestDiscord
        ' 
        btnTestDiscord.FlatAppearance.BorderSize = 0
        btnTestDiscord.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnTestDiscord.FlatStyle = FlatStyle.Flat
        btnTestDiscord.Image = My.Resources.Resources.bell
        btnTestDiscord.Location = New Point(449, 156)
        btnTestDiscord.Name = "btnTestDiscord"
        btnTestDiscord.Size = New Size(48, 48)
        btnTestDiscord.TabIndex = 6
        ToolTip1.SetToolTip(btnTestDiscord, "Click To Send Test Message")
        btnTestDiscord.UseVisualStyleBackColor = True
        ' 
        ' btnLookupIP
        ' 
        btnLookupIP.BackColor = SystemColors.ActiveBorder
        btnLookupIP.FlatAppearance.BorderColor = Color.Black
        btnLookupIP.FlatAppearance.BorderSize = 0
        btnLookupIP.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnLookupIP.FlatStyle = FlatStyle.Flat
        btnLookupIP.ForeColor = Color.Black
        btnLookupIP.Location = New Point(308, 40)
        btnLookupIP.Name = "btnLookupIP"
        btnLookupIP.Size = New Size(29, 30)
        btnLookupIP.TabIndex = 49
        btnLookupIP.Text = "🌐"
        ToolTip1.SetToolTip(btnLookupIP, "Attempt to get location from IP.")
        btnLookupIP.UseVisualStyleBackColor = False
        ' 
        ' txtLon
        ' 
        txtLon.Location = New Point(170, 41)
        txtLon.MaxLength = 100
        txtLon.Name = "txtLon"
        txtLon.Size = New Size(123, 29)
        txtLon.TabIndex = 47
        ToolTip1.SetToolTip(txtLon, "Your logitude position in Decimal Degrees (DD) format.")
        ' 
        ' txtLat
        ' 
        txtLat.Location = New Point(18, 41)
        txtLat.MaxLength = 100
        txtLat.Name = "txtLat"
        txtLat.Size = New Size(123, 29)
        txtLat.TabIndex = 45
        ToolTip1.SetToolTip(txtLat, "Your latitude position in Decimal Degrees (DD) format.")
        ' 
        ' chkThingSpeak
        ' 
        chkThingSpeak.AutoSize = True
        chkThingSpeak.Location = New Point(12, 369)
        chkThingSpeak.Name = "chkThingSpeak"
        chkThingSpeak.Size = New Size(206, 25)
        chkThingSpeak.TabIndex = 43
        chkThingSpeak.Text = "Participate in data &Upload"
        ToolTip1.SetToolTip(chkThingSpeak, "Check this to participate in uploading your event " & vbCrLf & "data to ThinkSpeak")
        chkThingSpeak.UseVisualStyleBackColor = True
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
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = Color.CornflowerBlue
        Label4.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label4.ForeColor = Color.White
        Label4.Location = New Point(12, 9)
        Label4.Name = "Label4"
        Label4.Padding = New Padding(4, 4, 4, 6)
        Label4.Size = New Size(165, 31)
        Label4.TabIndex = 0
        Label4.Text = "Event Notifications"
        ' 
        ' Label13
        ' 
        Label13.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label13.BackColor = Color.RoyalBlue
        Label13.Location = New Point(152, 23)
        Label13.Name = "Label13"
        Label13.Size = New Size(398, 3)
        Label13.TabIndex = 1
        ' 
        ' panDiscordVals
        ' 
        panDiscordVals.BackColor = Color.Transparent
        panDiscordVals.Controls.Add(btnTestDiscord)
        panDiscordVals.Controls.Add(Label6)
        panDiscordVals.Controls.Add(lblWebHook)
        panDiscordVals.Controls.Add(txtDiscordServer)
        panDiscordVals.Controls.Add(lblMention)
        panDiscordVals.Controls.Add(txtDiscordMention)
        panDiscordVals.Controls.Add(lblMentionInfo)
        panDiscordVals.Location = New Point(43, 85)
        panDiscordVals.Name = "panDiscordVals"
        panDiscordVals.Size = New Size(508, 215)
        panDiscordVals.TabIndex = 3
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
        ' lblLon
        ' 
        lblLon.AutoSize = True
        lblLon.Location = New Point(170, 17)
        lblLon.Name = "lblLon"
        lblLon.Size = New Size(152, 21)
        lblLon.TabIndex = 46
        lblLon.Text = "Your L&ongitude (DD)"
        ' 
        ' lblLat
        ' 
        lblLat.AutoSize = True
        lblLat.Location = New Point(18, 17)
        lblLat.Name = "lblLat"
        lblLat.Size = New Size(138, 21)
        lblLat.TabIndex = 44
        lblLat.Text = "Your L&atitude (DD)"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.BackColor = Color.CornflowerBlue
        Label8.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label8.ForeColor = Color.White
        Label8.Location = New Point(12, 322)
        Label8.Name = "Label8"
        Label8.Padding = New Padding(4, 4, 4, 6)
        Label8.Size = New Size(208, 31)
        Label8.TabIndex = 42
        Label8.Text = "ThingSpeak Data Upload"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.BackColor = Color.RoyalBlue
        Label1.Location = New Point(170, 336)
        Label1.Name = "Label1"
        Label1.Size = New Size(381, 3)
        Label1.TabIndex = 48
        ' 
        ' panThingSpeak
        ' 
        panThingSpeak.Controls.Add(txtLat)
        panThingSpeak.Controls.Add(btnLookupIP)
        panThingSpeak.Controls.Add(lblLat)
        panThingSpeak.Controls.Add(txtLon)
        panThingSpeak.Controls.Add(lblLon)
        panThingSpeak.Location = New Point(31, 396)
        panThingSpeak.Name = "panThingSpeak"
        panThingSpeak.Size = New Size(520, 100)
        panThingSpeak.TabIndex = 50
        ' 
        ' lnkThingSpeak
        ' 
        lnkThingSpeak.AutoSize = True
        lnkThingSpeak.LinkColor = Color.LightCyan
        lnkThingSpeak.Location = New Point(378, 357)
        lnkThingSpeak.Name = "lnkThingSpeak"
        lnkThingSpeak.Size = New Size(173, 21)
        lnkThingSpeak.TabIndex = 51
        lnkThingSpeak.TabStop = True
        lnkThingSpeak.Text = "Mathworks ThingSpeak"
        ToolTip1.SetToolTip(lnkThingSpeak, "Vist ThingSpeak Website")
        ' 
        ' frmAppConfig
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(562, 605)
        Controls.Add(lnkThingSpeak)
        Controls.Add(panThingSpeak)
        Controls.Add(chkThingSpeak)
        Controls.Add(Label8)
        Controls.Add(Label1)
        Controls.Add(Label4)
        Controls.Add(Label13)
        Controls.Add(btnReset)
        Controls.Add(panDiscordVals)
        Controls.Add(btnCancel)
        Controls.Add(chkDiscordNotify)
        Controls.Add(btnApply)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        FormBorderStyle = FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmAppConfig"
        StartPosition = FormStartPosition.CenterParent
        Text = "App Configuration"
        panDiscordVals.ResumeLayout(False)
        panDiscordVals.PerformLayout()
        panThingSpeak.ResumeLayout(False)
        panThingSpeak.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnApply As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents chkDiscordNotify As CheckBox
    Friend WithEvents lblWebHook As Label
    Friend WithEvents txtDiscordServer As TextBox
    Friend WithEvents txtDiscordMention As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblMention As Label
    Friend WithEvents lblMentionInfo As Label
    Friend WithEvents panDiscordVals As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents btnReset As Button
    Friend WithEvents btnTestDiscord As Button
    Friend WithEvents btnLookupIP As Button
    Friend WithEvents txtLon As TextBox
    Friend WithEvents lblLon As Label
    Friend WithEvents txtLat As TextBox
    Friend WithEvents lblLat As Label
    Friend WithEvents chkThingSpeak As CheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents panThingSpeak As Panel
    Friend WithEvents lnkThingSpeak As LinkLabel
End Class
