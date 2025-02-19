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
        Label5 = New Label()
        txtDiscordServer = New TextBox()
        txtDiscordMention = New TextBox()
        Label6 = New Label()
        ToolTip1 = New ToolTip(components)
        Label8 = New Label()
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
        btnApply.TabIndex = 19
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
        btnCancel.Location = New Point(454, 629)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 50)
        btnCancel.TabIndex = 20
        btnCancel.Text = "CA&NCEL"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' lblCenter
        ' 
        lblCenter.AutoSize = True
        lblCenter.Location = New Point(12, 28)
        lblCenter.Name = "lblCenter"
        lblCenter.Size = New Size(132, 21)
        lblCenter.TabIndex = 0
        lblCenter.Text = "&Center Frequency"
        ' 
        ' lblFrequency
        ' 
        lblFrequency.BorderStyle = BorderStyle.Fixed3D
        lblFrequency.Location = New Point(150, 24)
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
        cboScale.Location = New Point(473, 25)
        cboScale.Margin = New Padding(4)
        cboScale.MaxDropDownItems = 4
        cboScale.Name = "cboScale"
        cboScale.Size = New Size(81, 29)
        cboScale.TabIndex = 3
        ' 
        ' btnClearFreq
        ' 
        btnClearFreq.BackColor = Color.WhiteSmoke
        btnClearFreq.FlatAppearance.BorderSize = 0
        btnClearFreq.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnClearFreq.FlatStyle = FlatStyle.Flat
        btnClearFreq.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClearFreq.ForeColor = Color.LightCoral
        btnClearFreq.Location = New Point(443, 27)
        btnClearFreq.Name = "btnClearFreq"
        btnClearFreq.Size = New Size(23, 25)
        btnClearFreq.TabIndex = 12
        btnClearFreq.Text = "✖"
        btnClearFreq.UseVisualStyleBackColor = False
        ' 
        ' lblFreqRange
        ' 
        lblFreqRange.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblFreqRange.Location = New Point(96, 53)
        lblFreqRange.Name = "lblFreqRange"
        lblFreqRange.Size = New Size(255, 21)
        lblFreqRange.TabIndex = 4
        lblFreqRange.Text = "(--- range ---)"
        lblFreqRange.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' chkAutomatic
        ' 
        chkAutomatic.AutoSize = True
        chkAutomatic.Location = New Point(12, 112)
        chkAutomatic.Name = "chkAutomatic"
        chkAutomatic.Size = New Size(136, 25)
        chkAutomatic.TabIndex = 6
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
        sldGain.TabIndex = 9
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
        lblMainGain.TabIndex = 7
        lblMainGain.Text = "&Manual Gain Value (dB)"
        ' 
        ' lblGainValue
        ' 
        lblGainValue.BorderStyle = BorderStyle.Fixed3D
        lblGainValue.Location = New Point(27, 175)
        lblGainValue.Name = "lblGainValue"
        lblGainValue.Size = New Size(103, 29)
        lblGainValue.TabIndex = 8
        lblGainValue.Text = "0 dB"
        lblGainValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.BackColor = Color.Silver
        Label3.Location = New Point(12, 88)
        Label3.Name = "Label3"
        Label3.Size = New Size(540, 3)
        Label3.TabIndex = 5
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.BackColor = Color.Silver
        Label1.Location = New Point(13, 244)
        Label1.Name = "Label1"
        Label1.Size = New Size(540, 3)
        Label1.TabIndex = 10
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 261)
        Label2.Name = "Label2"
        Label2.Size = New Size(257, 21)
        Label2.TabIndex = 11
        Label2.Text = "Minimum &Event Recording Window"
        ' 
        ' cboMinEventWindow
        ' 
        cboMinEventWindow.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboMinEventWindow.DropDownStyle = ComboBoxStyle.DropDownList
        cboMinEventWindow.ForeColor = Color.White
        cboMinEventWindow.FormattingEnabled = True
        cboMinEventWindow.Location = New Point(27, 286)
        cboMinEventWindow.Margin = New Padding(4)
        cboMinEventWindow.MaxDropDownItems = 4
        cboMinEventWindow.Name = "cboMinEventWindow"
        cboMinEventWindow.Size = New Size(324, 29)
        cboMinEventWindow.TabIndex = 12
        ' 
        ' txtFrequency
        ' 
        txtFrequency.BackColor = Color.WhiteSmoke
        txtFrequency.ForeColor = Color.Black
        txtFrequency.Location = New Point(319, 25)
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
        Label4.Location = New Point(12, 337)
        Label4.Name = "Label4"
        Label4.Size = New Size(540, 3)
        Label4.TabIndex = 13
        ' 
        ' chkDiscordNotify
        ' 
        chkDiscordNotify.AutoSize = True
        chkDiscordNotify.Location = New Point(13, 361)
        chkDiscordNotify.Name = "chkDiscordNotify"
        chkDiscordNotify.Size = New Size(348, 25)
        chkDiscordNotify.TabIndex = 14
        chkDiscordNotify.Text = "&Send Discord Notification On Signal Detection"
        chkDiscordNotify.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(27, 389)
        Label5.Name = "Label5"
        Label5.Size = New Size(292, 21)
        Label5.TabIndex = 15
        Label5.Text = "&Discord Server Webhook URL (Required)"
        ' 
        ' txtDiscordServer
        ' 
        txtDiscordServer.Location = New Point(30, 413)
        txtDiscordServer.Name = "txtDiscordServer"
        txtDiscordServer.Size = New Size(514, 29)
        txtDiscordServer.TabIndex = 16
        ToolTip1.SetToolTip(txtDiscordServer, "How To Create a webhook in Discord:" & vbCrLf & vbCrLf & "Go to Server Settings → Integrations → Webhooks." & vbCrLf & "Click ""New Webhook"" and copy the Webhook URL." & vbCrLf & vbCrLf)
        ' 
        ' txtDiscordMention
        ' 
        txtDiscordMention.Location = New Point(33, 469)
        txtDiscordMention.Name = "txtDiscordMention"
        txtDiscordMention.Size = New Size(514, 29)
        txtDiscordMention.TabIndex = 18
        ToolTip1.SetToolTip(txtDiscordMention, resources.GetString("txtDiscordMention.ToolTip"))
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(30, 445)
        Label6.Name = "Label6"
        Label6.Size = New Size(331, 21)
        Label6.TabIndex = 17
        Label6.Text = "Discord C&hannel @UserID/@RoleID (Optional)"
        ' 
        ' ToolTip1
        ' 
        ToolTip1.AutoPopDelay = 10000
        ToolTip1.InitialDelay = 500
        ToolTip1.ReshowDelay = 100
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label8.Location = New Point(56, 501)
        Label8.Name = "Label8"
        Label8.Size = New Size(374, 75)
        Label8.TabIndex = 22
        Label8.Text = "User ID format should be ""<@&...>"" and Role ID should be ""<@&...>""" & vbCrLf & "The ""..."" will always be 17-20 numeric digits." & vbCrLf & vbCrLf & "User Mention: <@123456789012345678>" & vbCrLf & "Role Mention: <@&987654321098765432>"
        Label8.UseMnemonic = False
        ' 
        ' frmConfig
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(566, 691)
        Controls.Add(Label8)
        Controls.Add(txtDiscordMention)
        Controls.Add(Label6)
        Controls.Add(txtDiscordServer)
        Controls.Add(Label5)
        Controls.Add(chkDiscordNotify)
        Controls.Add(Label4)
        Controls.Add(btnClearFreq)
        Controls.Add(cboMinEventWindow)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(lblGainValue)
        Controls.Add(lblMainGain)
        Controls.Add(sldGain)
        Controls.Add(chkAutomatic)
        Controls.Add(lblFreqRange)
        Controls.Add(Label3)
        Controls.Add(cboScale)
        Controls.Add(lblFrequency)
        Controls.Add(lblCenter)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Controls.Add(txtFrequency)
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
    Friend WithEvents Label5 As Label
    Friend WithEvents txtDiscordServer As TextBox
    Friend WithEvents txtDiscordMention As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
End Class
