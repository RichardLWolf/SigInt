<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPlayback
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlayback))
        panLeft = New Panel()
        picExport = New PictureBox()
        picBrowseFolder = New PictureBox()
        picPause = New PictureBox()
        picArchiveInfo = New PictureBox()
        picRefresh = New PictureBox()
        lblHeading = New Label()
        lvwArchives = New ListView()
        picStartStop = New PictureBox()
        splRight = New SplitContainer()
        hsbSeekPos = New CustomSeekbar()
        panSignal = New Panel()
        panWaterfall = New Panel()
        panSignalCtrls = New Panel()
        panSignalSliders = New Panel()
        Label4 = New Label()
        Label5 = New Label()
        Label3 = New Label()
        lblDiv = New Label()
        lblOffset = New Label()
        Label2 = New Label()
        Label1 = New Label()
        sldZoom = New ctlRoundTrackbar()
        sldRange = New ctlRoundTrackbar()
        sldOffset = New ctlRoundTrackbar()
        sldContrast = New ctlRoundTrackbar()
        ToolTip1 = New ToolTip(components)
        panLeft.SuspendLayout()
        CType(picExport, ComponentModel.ISupportInitialize).BeginInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).BeginInit()
        CType(picPause, ComponentModel.ISupportInitialize).BeginInit()
        CType(picArchiveInfo, ComponentModel.ISupportInitialize).BeginInit()
        CType(picRefresh, ComponentModel.ISupportInitialize).BeginInit()
        CType(picStartStop, ComponentModel.ISupportInitialize).BeginInit()
        CType(splRight, ComponentModel.ISupportInitialize).BeginInit()
        splRight.Panel1.SuspendLayout()
        splRight.Panel2.SuspendLayout()
        splRight.SuspendLayout()
        panSignalCtrls.SuspendLayout()
        panSignalSliders.SuspendLayout()
        SuspendLayout()
        ' 
        ' panLeft
        ' 
        panLeft.Controls.Add(picExport)
        panLeft.Controls.Add(picBrowseFolder)
        panLeft.Controls.Add(picPause)
        panLeft.Controls.Add(picArchiveInfo)
        panLeft.Controls.Add(picRefresh)
        panLeft.Controls.Add(lblHeading)
        panLeft.Controls.Add(lvwArchives)
        panLeft.Controls.Add(picStartStop)
        panLeft.Dock = DockStyle.Left
        panLeft.Location = New Point(0, 0)
        panLeft.Name = "panLeft"
        panLeft.Size = New Size(304, 650)
        panLeft.TabIndex = 0
        ' 
        ' picExport
        ' 
        picExport.Image = My.Resources.Resources.wav_file
        picExport.Location = New Point(136, 249)
        picExport.Name = "picExport"
        picExport.Size = New Size(29, 29)
        picExport.SizeMode = PictureBoxSizeMode.StretchImage
        picExport.TabIndex = 29
        picExport.TabStop = False
        ToolTip1.SetToolTip(picExport, "Export to WAV file")
        ' 
        ' picBrowseFolder
        ' 
        picBrowseFolder.Image = CType(resources.GetObject("picBrowseFolder.Image"), Image)
        picBrowseFolder.Location = New Point(221, 1)
        picBrowseFolder.Name = "picBrowseFolder"
        picBrowseFolder.Size = New Size(29, 29)
        picBrowseFolder.SizeMode = PictureBoxSizeMode.StretchImage
        picBrowseFolder.TabIndex = 28
        picBrowseFolder.TabStop = False
        ToolTip1.SetToolTip(picBrowseFolder, "Select Archive Folder")
        ' 
        ' picPause
        ' 
        picPause.Image = My.Resources.Resources.media_pause
        picPause.Location = New Point(58, 249)
        picPause.Name = "picPause"
        picPause.Size = New Size(29, 29)
        picPause.SizeMode = PictureBoxSizeMode.StretchImage
        picPause.TabIndex = 27
        picPause.TabStop = False
        ToolTip1.SetToolTip(picPause, "Pause/Resume Playback")
        ' 
        ' picArchiveInfo
        ' 
        picArchiveInfo.Location = New Point(12, 315)
        picArchiveInfo.Name = "picArchiveInfo"
        picArchiveInfo.Size = New Size(280, 285)
        picArchiveInfo.TabIndex = 26
        picArchiveInfo.TabStop = False
        ' 
        ' picRefresh
        ' 
        picRefresh.Image = My.Resources.Resources.refresh
        picRefresh.Location = New Point(269, 3)
        picRefresh.Name = "picRefresh"
        picRefresh.Size = New Size(29, 29)
        picRefresh.SizeMode = PictureBoxSizeMode.StretchImage
        picRefresh.TabIndex = 25
        picRefresh.TabStop = False
        ToolTip1.SetToolTip(picRefresh, "Refresh List")
        ' 
        ' lblHeading
        ' 
        lblHeading.AutoSize = True
        lblHeading.Location = New Point(12, 9)
        lblHeading.Name = "lblHeading"
        lblHeading.Size = New Size(116, 21)
        lblHeading.TabIndex = 24
        lblHeading.Text = "Signal Archives"
        ' 
        ' lvwArchives
        ' 
        lvwArchives.BackColor = Color.DarkGray
        lvwArchives.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lvwArchives.ForeColor = Color.White
        lvwArchives.Location = New Point(12, 33)
        lvwArchives.Name = "lvwArchives"
        lvwArchives.Size = New Size(286, 210)
        lvwArchives.TabIndex = 23
        lvwArchives.UseCompatibleStateImageBehavior = False
        ' 
        ' picStartStop
        ' 
        picStartStop.Image = CType(resources.GetObject("picStartStop.Image"), Image)
        picStartStop.Location = New Point(9, 249)
        picStartStop.Name = "picStartStop"
        picStartStop.Size = New Size(29, 29)
        picStartStop.SizeMode = PictureBoxSizeMode.StretchImage
        picStartStop.TabIndex = 22
        picStartStop.TabStop = False
        ToolTip1.SetToolTip(picStartStop, "Start/Stop Playing")
        ' 
        ' splRight
        ' 
        splRight.Dock = DockStyle.Fill
        splRight.Location = New Point(304, 0)
        splRight.Name = "splRight"
        splRight.Orientation = Orientation.Horizontal
        ' 
        ' splRight.Panel1
        ' 
        splRight.Panel1.Controls.Add(hsbSeekPos)
        splRight.Panel1.Controls.Add(panSignal)
        ' 
        ' splRight.Panel2
        ' 
        splRight.Panel2.Controls.Add(panWaterfall)
        splRight.Size = New Size(962, 650)
        splRight.SplitterDistance = 287
        splRight.TabIndex = 1
        ' 
        ' hsbSeekPos
        ' 
        hsbSeekPos.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        hsbSeekPos.Location = New Point(0, 264)
        hsbSeekPos.Maximum = 100
        hsbSeekPos.Minimum = 0
        hsbSeekPos.Name = "hsbSeekPos"
        hsbSeekPos.Size = New Size(900, 20)
        hsbSeekPos.TabIndex = 5
        hsbSeekPos.Value = 0
        ' 
        ' panSignal
        ' 
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(0, 0)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(900, 264)
        panSignal.TabIndex = 3
        ' 
        ' panWaterfall
        ' 
        panWaterfall.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panWaterfall.BackColor = Color.Black
        panWaterfall.Location = New Point(0, 0)
        panWaterfall.Name = "panWaterfall"
        panWaterfall.Size = New Size(900, 359)
        panWaterfall.TabIndex = 4
        ' 
        ' panSignalCtrls
        ' 
        panSignalCtrls.AutoScroll = True
        panSignalCtrls.AutoSizeMode = AutoSizeMode.GrowAndShrink
        panSignalCtrls.Controls.Add(panSignalSliders)
        panSignalCtrls.Dock = DockStyle.Right
        panSignalCtrls.Location = New Point(1205, 0)
        panSignalCtrls.Name = "panSignalCtrls"
        panSignalCtrls.Size = New Size(61, 650)
        panSignalCtrls.TabIndex = 4
        ' 
        ' panSignalSliders
        ' 
        panSignalSliders.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        panSignalSliders.Controls.Add(Label4)
        panSignalSliders.Controls.Add(Label5)
        panSignalSliders.Controls.Add(Label3)
        panSignalSliders.Controls.Add(lblDiv)
        panSignalSliders.Controls.Add(lblOffset)
        panSignalSliders.Controls.Add(Label2)
        panSignalSliders.Controls.Add(Label1)
        panSignalSliders.Controls.Add(sldZoom)
        panSignalSliders.Controls.Add(sldRange)
        panSignalSliders.Controls.Add(sldOffset)
        panSignalSliders.Controls.Add(sldContrast)
        panSignalSliders.Location = New Point(0, 0)
        panSignalSliders.Name = "panSignalSliders"
        panSignalSliders.Size = New Size(60, 645)
        panSignalSliders.TabIndex = 21
        ' 
        ' Label4
        ' 
        Label4.BackColor = Color.Silver
        Label4.Location = New Point(1, 479)
        Label4.Name = "Label4"
        Label4.Size = New Size(56, 3)
        Label4.TabIndex = 31
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = Color.Transparent
        Label5.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label5.ForeColor = Color.White
        Label5.Location = New Point(-2, 488)
        Label5.Name = "Label5"
        Label5.Size = New Size(64, 20)
        Label5.TabIndex = 30
        Label5.Text = "Contrast"
        Label5.TextAlign = ContentAlignment.MiddleCenter
        ToolTip1.SetToolTip(Label5, "Waterfall signal strength contrast")
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
        ' sldZoom
        ' 
        sldZoom.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldZoom.InnerPadding = 5
        sldZoom.KnobColor = Color.RoyalBlue
        sldZoom.KnobSize = 24
        sldZoom.KnobTextColor = Color.LightGoldenrodYellow
        sldZoom.Location = New Point(2, 11)
        sldZoom.Maximum = 100
        sldZoom.Minimum = 0
        sldZoom.Name = "sldZoom"
        sldZoom.Orientation = Orientation.Vertical
        sldZoom.ShowValueInKnob = True
        sldZoom.Size = New Size(53, 135)
        sldZoom.TabIndex = 32
        sldZoom.TickColor = Color.LightGray
        sldZoom.TickSize = 25
        sldZoom.TickSpacing = 10
        sldZoom.TrackColor = Color.Gray
        sldZoom.TrackWidth = 3
        sldZoom.Value = 50
        ' 
        ' sldRange
        ' 
        sldRange.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldRange.InnerPadding = 5
        sldRange.KnobColor = Color.RoyalBlue
        sldRange.KnobSize = 24
        sldRange.KnobTextColor = Color.LightGoldenrodYellow
        sldRange.Location = New Point(3, 167)
        sldRange.Maximum = 100
        sldRange.Minimum = 0
        sldRange.Name = "sldRange"
        sldRange.Orientation = Orientation.Vertical
        sldRange.ShowValueInKnob = True
        sldRange.Size = New Size(53, 135)
        sldRange.TabIndex = 33
        sldRange.TickColor = Color.LightGray
        sldRange.TickSize = 25
        sldRange.TickSpacing = 10
        sldRange.TrackColor = Color.Gray
        sldRange.TrackWidth = 3
        sldRange.Value = 50
        ' 
        ' sldOffset
        ' 
        sldOffset.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldOffset.InnerPadding = 5
        sldOffset.KnobColor = Color.RoyalBlue
        sldOffset.KnobSize = 24
        sldOffset.KnobTextColor = Color.LightGoldenrodYellow
        sldOffset.Location = New Point(2, 324)
        sldOffset.Maximum = 100
        sldOffset.Minimum = 0
        sldOffset.Name = "sldOffset"
        sldOffset.Orientation = Orientation.Vertical
        sldOffset.ShowValueInKnob = True
        sldOffset.Size = New Size(53, 135)
        sldOffset.TabIndex = 34
        sldOffset.TickColor = Color.LightGray
        sldOffset.TickSize = 25
        sldOffset.TickSpacing = 10
        sldOffset.TrackColor = Color.Gray
        sldOffset.TrackWidth = 3
        sldOffset.Value = 50
        ' 
        ' sldContrast
        ' 
        sldContrast.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldContrast.InnerPadding = 5
        sldContrast.KnobColor = Color.RoyalBlue
        sldContrast.KnobSize = 24
        sldContrast.KnobTextColor = Color.LightGoldenrodYellow
        sldContrast.Location = New Point(3, 498)
        sldContrast.Maximum = 100
        sldContrast.Minimum = 0
        sldContrast.Name = "sldContrast"
        sldContrast.Orientation = Orientation.Vertical
        sldContrast.ShowValueInKnob = True
        sldContrast.Size = New Size(53, 135)
        sldContrast.TabIndex = 35
        sldContrast.TickColor = Color.LightGray
        sldContrast.TickSize = 25
        sldContrast.TickSpacing = 10
        sldContrast.TrackColor = Color.Gray
        sldContrast.TrackWidth = 3
        sldContrast.Value = 50
        ' 
        ' frmPlayback
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1266, 650)
        Controls.Add(panSignalCtrls)
        Controls.Add(splRight)
        Controls.Add(panLeft)
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ForeColor = Color.White
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmPlayback"
        StartPosition = FormStartPosition.CenterParent
        Text = "Signal Playback"
        panLeft.ResumeLayout(False)
        panLeft.PerformLayout()
        CType(picExport, ComponentModel.ISupportInitialize).EndInit()
        CType(picBrowseFolder, ComponentModel.ISupportInitialize).EndInit()
        CType(picPause, ComponentModel.ISupportInitialize).EndInit()
        CType(picArchiveInfo, ComponentModel.ISupportInitialize).EndInit()
        CType(picRefresh, ComponentModel.ISupportInitialize).EndInit()
        CType(picStartStop, ComponentModel.ISupportInitialize).EndInit()
        splRight.Panel1.ResumeLayout(False)
        splRight.Panel2.ResumeLayout(False)
        CType(splRight, ComponentModel.ISupportInitialize).EndInit()
        splRight.ResumeLayout(False)
        panSignalCtrls.ResumeLayout(False)
        panSignalSliders.ResumeLayout(False)
        panSignalSliders.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents panLeft As Panel
    Friend WithEvents splRight As SplitContainer
    Friend WithEvents panSignalCtrls As Panel
    Friend WithEvents panSignal As Panel
    Friend WithEvents picStartStop As PictureBox
    Friend WithEvents lvwArchives As ListView
    Friend WithEvents panWaterfall As Panel
    Friend WithEvents picRefresh As PictureBox
    Friend WithEvents lblHeading As Label
    Friend WithEvents picArchiveInfo As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents picPause As PictureBox
    Friend WithEvents hsbSeekPos As CustomSeekbar
    Friend WithEvents panSignalSliders As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents lblDiv As Label
    Friend WithEvents lblOffset As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents picBrowseFolder As PictureBox
    Friend WithEvents picExport As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents sldZoom As ctlRoundTrackbar
    Friend WithEvents sldRange As ctlRoundTrackbar
    Friend WithEvents sldOffset As ctlRoundTrackbar
    Friend WithEvents sldContrast As ctlRoundTrackbar
End Class
