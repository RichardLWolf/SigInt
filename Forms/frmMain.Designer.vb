<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        lblSdrDevices = New Label()
        cboDeviceList = New ComboBox()
        Label1 = New Label()
        cboConfigs = New ComboBox()
        picEdit = New PictureBox()
        picDelete = New PictureBox()
        ToolTip1 = New ToolTip(components)
        picAdd = New PictureBox()
        picRefresh = New PictureBox()
        picViewLog = New PictureBox()
        picPlayback = New PictureBox()
        picBrowseFolder = New PictureBox()
        btnExit = New Button()
        btnMonitor = New Button()
        picGenConfig = New PictureBox()
        btnMinimize = New Button()
        lnkSigIntRepository = New LinkLabel()
        lblViewLog = New Label()
        lblExplorer = New Label()
        lblPlayBack = New Label()
        Label12 = New Label()
        Label2 = New Label()
        lblGenConfig = New Label()
        CType(picEdit, ComponentModel.ISupportInitialize).BeginInit()
        CType(picDelete, ComponentModel.ISupportInitialize).BeginInit()
        CType(picAdd, ComponentModel.ISupportInitialize).BeginInit()
        CType(picRefresh, ComponentModel.ISupportInitialize).BeginInit()
        CType(picViewLog, ComponentModel.ISupportInitialize).BeginInit()
        CType(picPlayback, ComponentModel.ISupportInitialize).BeginInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).BeginInit()
        CType(picGenConfig, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblSdrDevices
        ' 
        lblSdrDevices.AutoSize = True
        lblSdrDevices.ForeColor = Color.White
        lblSdrDevices.Location = New Point(12, 9)
        lblSdrDevices.Name = "lblSdrDevices"
        lblSdrDevices.Size = New Size(135, 21)
        lblSdrDevices.TabIndex = 3
        lblSdrDevices.Text = "Select SDR Device"
        ' 
        ' cboDeviceList
        ' 
        cboDeviceList.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboDeviceList.DropDownStyle = ComboBoxStyle.DropDownList
        cboDeviceList.ForeColor = Color.White
        cboDeviceList.FormattingEnabled = True
        cboDeviceList.Location = New Point(13, 34)
        cboDeviceList.Margin = New Padding(4)
        cboDeviceList.Name = "cboDeviceList"
        cboDeviceList.Size = New Size(463, 29)
        cboDeviceList.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.ForeColor = Color.White
        Label1.Location = New Point(13, 90)
        Label1.Name = "Label1"
        Label1.Size = New Size(264, 21)
        Label1.TabIndex = 5
        Label1.Text = "Select Monitor Configuration Setting"
        ' 
        ' cboConfigs
        ' 
        cboConfigs.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        cboConfigs.DropDownStyle = ComboBoxStyle.DropDownList
        cboConfigs.ForeColor = Color.White
        cboConfigs.FormattingEnabled = True
        cboConfigs.Location = New Point(14, 115)
        cboConfigs.Margin = New Padding(4)
        cboConfigs.Name = "cboConfigs"
        cboConfigs.Size = New Size(462, 29)
        cboConfigs.TabIndex = 4
        ' 
        ' picEdit
        ' 
        picEdit.Image = My.Resources.Resources.pencil2
        picEdit.Location = New Point(487, 115)
        picEdit.Name = "picEdit"
        picEdit.Size = New Size(29, 29)
        picEdit.SizeMode = PictureBoxSizeMode.StretchImage
        picEdit.TabIndex = 24
        picEdit.TabStop = False
        ToolTip1.SetToolTip(picEdit, "Edit selected configuration")
        ' 
        ' picDelete
        ' 
        picDelete.Image = My.Resources.Resources.trash_red
        picDelete.Location = New Point(579, 115)
        picDelete.Name = "picDelete"
        picDelete.Size = New Size(29, 29)
        picDelete.SizeMode = PictureBoxSizeMode.StretchImage
        picDelete.TabIndex = 25
        picDelete.TabStop = False
        ToolTip1.SetToolTip(picDelete, "Remove selected configuration")
        ' 
        ' picAdd
        ' 
        picAdd.Image = My.Resources.Resources.add2
        picAdd.Location = New Point(522, 115)
        picAdd.Name = "picAdd"
        picAdd.Size = New Size(29, 29)
        picAdd.SizeMode = PictureBoxSizeMode.StretchImage
        picAdd.TabIndex = 26
        picAdd.TabStop = False
        ToolTip1.SetToolTip(picAdd, "Add new configuration")
        ' 
        ' picRefresh
        ' 
        picRefresh.Image = My.Resources.Resources.refresh
        picRefresh.Location = New Point(487, 34)
        picRefresh.Name = "picRefresh"
        picRefresh.Size = New Size(29, 29)
        picRefresh.SizeMode = PictureBoxSizeMode.StretchImage
        picRefresh.TabIndex = 30
        picRefresh.TabStop = False
        ToolTip1.SetToolTip(picRefresh, "Refresh Device List")
        ' 
        ' picViewLog
        ' 
        picViewLog.Image = My.Resources.Resources.scroll_view
        picViewLog.Location = New Point(14, 189)
        picViewLog.Name = "picViewLog"
        picViewLog.Size = New Size(29, 29)
        picViewLog.SizeMode = PictureBoxSizeMode.StretchImage
        picViewLog.TabIndex = 33
        picViewLog.TabStop = False
        ToolTip1.SetToolTip(picViewLog, "View Application Log")
        ' 
        ' picPlayback
        ' 
        picPlayback.Image = My.Resources.Resources.microphone2
        picPlayback.Location = New Point(12, 239)
        picPlayback.Name = "picPlayback"
        picPlayback.Size = New Size(29, 29)
        picPlayback.SizeMode = PictureBoxSizeMode.StretchImage
        picPlayback.TabIndex = 32
        picPlayback.TabStop = False
        ToolTip1.SetToolTip(picPlayback, "Playback recorded archives")
        ' 
        ' picBrowseFolder
        ' 
        picBrowseFolder.Image = CType(resources.GetObject("picBrowseFolder.Image"), Image)
        picBrowseFolder.Location = New Point(280, 189)
        picBrowseFolder.Name = "picBrowseFolder"
        picBrowseFolder.Size = New Size(29, 29)
        picBrowseFolder.SizeMode = PictureBoxSizeMode.StretchImage
        picBrowseFolder.TabIndex = 31
        picBrowseFolder.TabStop = False
        ToolTip1.SetToolTip(picBrowseFolder, "Browse log folder")
        ' 
        ' btnExit
        ' 
        btnExit.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnExit.BackColor = Color.Pink
        btnExit.DialogResult = DialogResult.Cancel
        btnExit.FlatAppearance.BorderColor = Color.Black
        btnExit.FlatAppearance.BorderSize = 0
        btnExit.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnExit.FlatStyle = FlatStyle.Flat
        btnExit.ForeColor = Color.Black
        btnExit.Location = New Point(508, 319)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(100, 50)
        btnExit.TabIndex = 28
        btnExit.Text = "CLOSE &APP"
        ToolTip1.SetToolTip(btnExit, "Close app and all monitors" & vbCrLf)
        btnExit.UseVisualStyleBackColor = False
        ' 
        ' btnMonitor
        ' 
        btnMonitor.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnMonitor.BackColor = Color.LightBlue
        btnMonitor.FlatAppearance.BorderColor = Color.Black
        btnMonitor.FlatAppearance.BorderSize = 0
        btnMonitor.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnMonitor.FlatStyle = FlatStyle.Flat
        btnMonitor.ForeColor = Color.Black
        btnMonitor.Location = New Point(373, 319)
        btnMonitor.Name = "btnMonitor"
        btnMonitor.Size = New Size(100, 50)
        btnMonitor.TabIndex = 27
        btnMonitor.Text = "&MONITOR"
        ToolTip1.SetToolTip(btnMonitor, "Click to monitor selected device using the selected configuration")
        btnMonitor.UseVisualStyleBackColor = False
        ' 
        ' picGenConfig
        ' 
        picGenConfig.Image = My.Resources.Resources.gear
        picGenConfig.Location = New Point(280, 238)
        picGenConfig.Name = "picGenConfig"
        picGenConfig.Size = New Size(29, 29)
        picGenConfig.SizeMode = PictureBoxSizeMode.StretchImage
        picGenConfig.TabIndex = 39
        picGenConfig.TabStop = False
        ToolTip1.SetToolTip(picGenConfig, "Browse log folder")
        ' 
        ' btnMinimize
        ' 
        btnMinimize.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnMinimize.BackColor = Color.LightGray
        btnMinimize.FlatAppearance.BorderColor = Color.Black
        btnMinimize.FlatAppearance.BorderSize = 0
        btnMinimize.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue
        btnMinimize.FlatStyle = FlatStyle.Flat
        btnMinimize.ForeColor = Color.Black
        btnMinimize.Location = New Point(240, 319)
        btnMinimize.Name = "btnMinimize"
        btnMinimize.Size = New Size(100, 50)
        btnMinimize.TabIndex = 41
        btnMinimize.Text = "MINIMI&ZE"
        ToolTip1.SetToolTip(btnMinimize, "Minimize all app windows")
        btnMinimize.UseVisualStyleBackColor = False
        ' 
        ' lnkSigIntRepository
        ' 
        lnkSigIntRepository.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lnkSigIntRepository.AutoSize = True
        lnkSigIntRepository.LinkColor = Color.CornflowerBlue
        lnkSigIntRepository.Location = New Point(12, 334)
        lnkSigIntRepository.Name = "lnkSigIntRepository"
        lnkSigIntRepository.Size = New Size(163, 21)
        lnkSigIntRepository.TabIndex = 29
        lnkSigIntRepository.TabStop = True
        lnkSigIntRepository.Text = "Visit SigInt Repository"
        lnkSigIntRepository.VisitedLinkColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        ' 
        ' lblViewLog
        ' 
        lblViewLog.AutoSize = True
        lblViewLog.Location = New Point(49, 188)
        lblViewLog.Name = "lblViewLog"
        lblViewLog.Padding = New Padding(5)
        lblViewLog.Size = New Size(166, 31)
        lblViewLog.TabIndex = 34
        lblViewLog.Text = "View Application Log"
        ' 
        ' lblExplorer
        ' 
        lblExplorer.AutoSize = True
        lblExplorer.Location = New Point(315, 188)
        lblExplorer.Name = "lblExplorer"
        lblExplorer.Padding = New Padding(5)
        lblExplorer.Size = New Size(136, 31)
        lblExplorer.TabIndex = 35
        lblExplorer.Text = "Open Log Folder"
        ' 
        ' lblPlayBack
        ' 
        lblPlayBack.AutoSize = True
        lblPlayBack.Location = New Point(47, 237)
        lblPlayBack.Name = "lblPlayBack"
        lblPlayBack.Padding = New Padding(5)
        lblPlayBack.Size = New Size(207, 31)
        lblPlayBack.TabIndex = 36
        lblPlayBack.Text = "Playback Recorded Archive"
        ' 
        ' Label12
        ' 
        Label12.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label12.BackColor = Color.RoyalBlue
        Label12.Location = New Point(12, 174)
        Label12.Name = "Label12"
        Label12.Size = New Size(461, 2)
        Label12.TabIndex = 37
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.BackColor = Color.RoyalBlue
        Label2.Location = New Point(12, 284)
        Label2.Name = "Label2"
        Label2.Size = New Size(461, 2)
        Label2.TabIndex = 38
        ' 
        ' lblGenConfig
        ' 
        lblGenConfig.AutoSize = True
        lblGenConfig.Location = New Point(315, 237)
        lblGenConfig.Name = "lblGenConfig"
        lblGenConfig.Padding = New Padding(5)
        lblGenConfig.Size = New Size(148, 31)
        lblGenConfig.TabIndex = 40
        lblGenConfig.Text = "App Configuration"
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(620, 382)
        Controls.Add(btnMinimize)
        Controls.Add(lblGenConfig)
        Controls.Add(picGenConfig)
        Controls.Add(Label2)
        Controls.Add(Label12)
        Controls.Add(lblPlayBack)
        Controls.Add(lblExplorer)
        Controls.Add(lblViewLog)
        Controls.Add(picViewLog)
        Controls.Add(picPlayback)
        Controls.Add(picBrowseFolder)
        Controls.Add(picRefresh)
        Controls.Add(lnkSigIntRepository)
        Controls.Add(btnExit)
        Controls.Add(btnMonitor)
        Controls.Add(picAdd)
        Controls.Add(picDelete)
        Controls.Add(picEdit)
        Controls.Add(Label1)
        Controls.Add(cboConfigs)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        MaximizeBox = False
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "SigInt - RTL-SDR Signal Monitoring Utility"
        CType(picEdit, ComponentModel.ISupportInitialize).EndInit()
        CType(picDelete, ComponentModel.ISupportInitialize).EndInit()
        CType(picAdd, ComponentModel.ISupportInitialize).EndInit()
        CType(picRefresh, ComponentModel.ISupportInitialize).EndInit()
        CType(picViewLog, ComponentModel.ISupportInitialize).EndInit()
        CType(picPlayback, ComponentModel.ISupportInitialize).EndInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).EndInit()
        CType(picGenConfig, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cboConfigs As ComboBox
    Friend WithEvents picEdit As PictureBox
    Friend WithEvents picDelete As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents picAdd As PictureBox
    Friend WithEvents btnExit As Button
    Friend WithEvents btnMonitor As Button
    Friend WithEvents lnkSigIntRepository As LinkLabel
    Friend WithEvents picRefresh As PictureBox
    Friend WithEvents picViewLog As PictureBox
    Friend WithEvents picPlayback As PictureBox
    Friend WithEvents picBrowseFolder As PictureBox
    Friend WithEvents lblViewLog As Label
    Friend WithEvents lblExplorer As Label
    Friend WithEvents lblPlayBack As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblGenConfig As Label
    Friend WithEvents picGenConfig As PictureBox
    Friend WithEvents btnMinimize As Button
End Class
