<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        cboDeviceList = New ComboBox()
        lblSdrDevices = New Label()
        panSignal = New Panel()
        lblEvents = New Label()
        Panel1 = New Panel()
        Label3 = New Label()
        lblDiv = New Label()
        sldOffset = New CustomSlider()
        sldRange = New CustomSlider()
        sldZoom = New CustomSlider()
        lblOffset = New Label()
        Label2 = New Label()
        Label1 = New Label()
        picStartStop = New PictureBox()
        picBrowseFolder = New PictureBox()
        ToolTip1 = New ToolTip(components)
        picConfig = New PictureBox()
        picPlayback = New PictureBox()
        picViewLog = New PictureBox()
        Panel1.SuspendLayout()
        CType(picStartStop, ComponentModel.ISupportInitialize).BeginInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).BeginInit()
        CType(picConfig, ComponentModel.ISupportInitialize).BeginInit()
        CType(picPlayback, ComponentModel.ISupportInitialize).BeginInit()
        CType(picViewLog, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
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
        cboDeviceList.Size = New Size(253, 29)
        cboDeviceList.TabIndex = 0
        ' 
        ' lblSdrDevices
        ' 
        lblSdrDevices.AutoSize = True
        lblSdrDevices.ForeColor = Color.White
        lblSdrDevices.Location = New Point(12, 9)
        lblSdrDevices.Name = "lblSdrDevices"
        lblSdrDevices.Size = New Size(135, 21)
        lblSdrDevices.TabIndex = 1
        lblSdrDevices.Text = "Select SDR Device"
        ' 
        ' panSignal
        ' 
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(12, 70)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(950, 465)
        panSignal.TabIndex = 2
        ' 
        ' lblEvents
        ' 
        lblEvents.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblEvents.ForeColor = Color.Coral
        lblEvents.Location = New Point(574, 34)
        lblEvents.Name = "lblEvents"
        lblEvents.Size = New Size(443, 28)
        lblEvents.TabIndex = 18
        lblEvents.Text = "No Signals Deteceted"
        lblEvents.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Panel1.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(lblDiv)
        Panel1.Controls.Add(sldOffset)
        Panel1.Controls.Add(sldRange)
        Panel1.Controls.Add(sldZoom)
        Panel1.Controls.Add(lblOffset)
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(Label1)
        Panel1.Location = New Point(968, 70)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(59, 475)
        Panel1.TabIndex = 20
        ' 
        ' Label3
        ' 
        Label3.BackColor = Color.Silver
        Label3.Location = New Point(1, 305)
        Label3.Name = "Label3"
        Label3.Size = New Size(56, 3)
        Label3.TabIndex = 29
        ' 
        ' lblDiv
        ' 
        lblDiv.BackColor = Color.Silver
        lblDiv.Location = New Point(1, 149)
        lblDiv.Name = "lblDiv"
        lblDiv.Size = New Size(56, 3)
        lblDiv.TabIndex = 28
        ' 
        ' sldOffset
        ' 
        sldOffset.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldOffset.KnobColor = Color.DodgerBlue
        sldOffset.Location = New Point(4, 339)
        sldOffset.Maximum = 50
        sldOffset.Minimum = -100
        sldOffset.Name = "sldOffset"
        sldOffset.Size = New Size(50, 120)
        sldOffset.TabIndex = 27
        sldOffset.TextColor = Color.White
        sldOffset.TickColor = Color.LightGray
        sldOffset.TickSpacing = 10
        ToolTip1.SetToolTip(sldOffset, "Y-Axis dB sttarting offset")
        sldOffset.TrackColor = Color.Gray
        sldOffset.TrackHighlightColor = Color.LightGray
        sldOffset.Value = -20
        sldOffset.Vertical = True
        ' 
        ' sldRange
        ' 
        sldRange.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldRange.KnobColor = Color.DodgerBlue
        sldRange.Location = New Point(4, 179)
        sldRange.Maximum = 150
        sldRange.Minimum = 10
        sldRange.Name = "sldRange"
        sldRange.Size = New Size(50, 120)
        sldRange.TabIndex = 26
        sldRange.TextColor = Color.White
        sldRange.TickColor = Color.LightGray
        sldRange.TickSpacing = 10
        ToolTip1.SetToolTip(sldRange, "dB range (Y-axis) to display")
        sldRange.TrackColor = Color.Gray
        sldRange.TrackHighlightColor = Color.LightGray
        sldRange.Value = 100
        sldRange.Vertical = True
        ' 
        ' sldZoom
        ' 
        sldZoom.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldZoom.KnobColor = Color.DodgerBlue
        sldZoom.Location = New Point(4, 24)
        sldZoom.Maximum = 100
        sldZoom.Minimum = 0
        sldZoom.Name = "sldZoom"
        sldZoom.Size = New Size(50, 120)
        sldZoom.TabIndex = 25
        sldZoom.TextColor = Color.White
        sldZoom.TickColor = Color.LightGray
        sldZoom.TickSpacing = 10
        ToolTip1.SetToolTip(sldZoom, "Zoom level 100%=full spectrum, 10%=small spectrum")
        sldZoom.TrackColor = Color.Gray
        sldZoom.TrackHighlightColor = Color.LightGray
        sldZoom.Value = 0
        sldZoom.Vertical = True
        ' 
        ' lblOffset
        ' 
        lblOffset.AutoSize = True
        lblOffset.BackColor = Color.Transparent
        lblOffset.ForeColor = Color.White
        lblOffset.Location = New Point(3, 315)
        lblOffset.Name = "lblOffset"
        lblOffset.Size = New Size(52, 21)
        lblOffset.TabIndex = 24
        lblOffset.Text = "Offset"
        lblOffset.TextAlign = ContentAlignment.MiddleCenter
        ToolTip1.SetToolTip(lblOffset, "Y-Axis dB sttarting offset")
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.ForeColor = Color.White
        Label2.Location = New Point(2, 155)
        Label2.Name = "Label2"
        Label2.Size = New Size(54, 21)
        Label2.TabIndex = 22
        Label2.Text = "Range"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ToolTip1.SetToolTip(Label2, "dB range (Y-axis) to display")
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.ForeColor = Color.White
        Label1.Location = New Point(4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(51, 21)
        Label1.TabIndex = 20
        Label1.Text = "Zoom"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ToolTip1.SetToolTip(Label1, "Zoom level 100%=full spectrum, 10%=small spectrum")
        ' 
        ' picStartStop
        ' 
        picStartStop.Image = CType(resources.GetObject("picStartStop.Image"), Image)
        picStartStop.Location = New Point(273, 34)
        picStartStop.Name = "picStartStop"
        picStartStop.Size = New Size(29, 29)
        picStartStop.SizeMode = PictureBoxSizeMode.StretchImage
        picStartStop.TabIndex = 21
        picStartStop.TabStop = False
        ToolTip1.SetToolTip(picStartStop, "Start/Stop monitoring")
        ' 
        ' picBrowseFolder
        ' 
        picBrowseFolder.Image = CType(resources.GetObject("picBrowseFolder.Image"), Image)
        picBrowseFolder.Location = New Point(398, 34)
        picBrowseFolder.Name = "picBrowseFolder"
        picBrowseFolder.Size = New Size(29, 29)
        picBrowseFolder.SizeMode = PictureBoxSizeMode.StretchImage
        picBrowseFolder.TabIndex = 22
        picBrowseFolder.TabStop = False
        ToolTip1.SetToolTip(picBrowseFolder, "Browse log folder")
        ' 
        ' picConfig
        ' 
        picConfig.Image = My.Resources.Resources.gear_gray
        picConfig.Location = New Point(308, 35)
        picConfig.Name = "picConfig"
        picConfig.Size = New Size(29, 29)
        picConfig.SizeMode = PictureBoxSizeMode.StretchImage
        picConfig.TabIndex = 23
        picConfig.TabStop = False
        ToolTip1.SetToolTip(picConfig, "Device configuration")
        ' 
        ' picPlayback
        ' 
        picPlayback.Image = My.Resources.Resources.microphone2
        picPlayback.Location = New Point(447, 35)
        picPlayback.Name = "picPlayback"
        picPlayback.Size = New Size(29, 29)
        picPlayback.SizeMode = PictureBoxSizeMode.StretchImage
        picPlayback.TabIndex = 24
        picPlayback.TabStop = False
        ToolTip1.SetToolTip(picPlayback, "Playback recorded archives")
        ' 
        ' picViewLog
        ' 
        picViewLog.Image = My.Resources.Resources.scroll_view
        picViewLog.Location = New Point(363, 34)
        picViewLog.Name = "picViewLog"
        picViewLog.Size = New Size(29, 29)
        picViewLog.SizeMode = PictureBoxSizeMode.StretchImage
        picViewLog.TabIndex = 25
        picViewLog.TabStop = False
        ToolTip1.SetToolTip(picViewLog, "View Application Log")
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1029, 547)
        Controls.Add(picViewLog)
        Controls.Add(picPlayback)
        Controls.Add(picConfig)
        Controls.Add(picBrowseFolder)
        Controls.Add(picStartStop)
        Controls.Add(Panel1)
        Controls.Add(lblEvents)
        Controls.Add(panSignal)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmMain"
        Text = "SigInt"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(picStartStop, ComponentModel.ISupportInitialize).EndInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).EndInit()
        CType(picConfig, ComponentModel.ISupportInitialize).EndInit()
        CType(picPlayback, ComponentModel.ISupportInitialize).EndInit()
        CType(picViewLog, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents panSignal As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLogFile As TextBox
    Friend WithEvents lblEvents As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents lblOffset As Label
    Friend WithEvents sldZoom As CustomSlider
    Friend WithEvents sldRange As CustomSlider
    Friend WithEvents sldOffset As CustomSlider
    Friend WithEvents lblDiv As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents picStartStop As PictureBox
    Friend WithEvents picBrowseFolder As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents picConfig As PictureBox
    Friend WithEvents picPlayback As PictureBox
    Friend WithEvents picViewLog As PictureBox

End Class
