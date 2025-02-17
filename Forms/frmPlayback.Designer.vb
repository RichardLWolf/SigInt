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
        panSignalCtrls = New Panel()
        panSignalSliders = New Panel()
        Label3 = New Label()
        lblDiv = New Label()
        sldOffset = New CustomSlider()
        sldRange = New CustomSlider()
        sldZoom = New CustomSlider()
        lblOffset = New Label()
        Label2 = New Label()
        Label1 = New Label()
        panSignal = New Panel()
        Panel1 = New Panel()
        sldContrast = New CustomSlider()
        Label8 = New Label()
        panWaterfall = New Panel()
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
        Panel1.SuspendLayout()
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
        panLeft.Size = New Size(304, 678)
        panLeft.TabIndex = 0
        ' 
        ' picExport
        ' 
        picExport.Image = My.Resources.Resources.wav_file
        picExport.Location = New Point(136, 224)
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
        picPause.Location = New Point(58, 224)
        picPause.Name = "picPause"
        picPause.Size = New Size(29, 29)
        picPause.SizeMode = PictureBoxSizeMode.StretchImage
        picPause.TabIndex = 27
        picPause.TabStop = False
        ToolTip1.SetToolTip(picPause, "Pause/Resume Playback")
        ' 
        ' picArchiveInfo
        ' 
        picArchiveInfo.Location = New Point(12, 268)
        picArchiveInfo.Name = "picArchiveInfo"
        picArchiveInfo.Size = New Size(275, 263)
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
        lvwArchives.Size = New Size(286, 185)
        lvwArchives.TabIndex = 23
        lvwArchives.UseCompatibleStateImageBehavior = False
        ' 
        ' picStartStop
        ' 
        picStartStop.Image = CType(resources.GetObject("picStartStop.Image"), Image)
        picStartStop.Location = New Point(9, 224)
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
        splRight.Panel1.Controls.Add(panSignalCtrls)
        splRight.Panel1.Controls.Add(panSignal)
        ' 
        ' splRight.Panel2
        ' 
        splRight.Panel2.Controls.Add(Panel1)
        splRight.Panel2.Controls.Add(panWaterfall)
        splRight.Size = New Size(962, 678)
        splRight.SplitterDistance = 300
        splRight.TabIndex = 1
        ' 
        ' hsbSeekPos
        ' 
        hsbSeekPos.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        hsbSeekPos.Location = New Point(6, 277)
        hsbSeekPos.Maximum = 100
        hsbSeekPos.Minimum = 0
        hsbSeekPos.Name = "hsbSeekPos"
        hsbSeekPos.Size = New Size(869, 20)
        hsbSeekPos.TabIndex = 5
        hsbSeekPos.Value = 0
        ' 
        ' panSignalCtrls
        ' 
        panSignalCtrls.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        panSignalCtrls.AutoScroll = True
        panSignalCtrls.AutoSizeMode = AutoSizeMode.GrowAndShrink
        panSignalCtrls.Controls.Add(panSignalSliders)
        panSignalCtrls.Location = New Point(881, 3)
        panSignalCtrls.Name = "panSignalCtrls"
        panSignalCtrls.Size = New Size(78, 289)
        panSignalCtrls.TabIndex = 4
        ' 
        ' panSignalSliders
        ' 
        panSignalSliders.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        panSignalSliders.Controls.Add(Label3)
        panSignalSliders.Controls.Add(lblDiv)
        panSignalSliders.Controls.Add(sldOffset)
        panSignalSliders.Controls.Add(sldRange)
        panSignalSliders.Controls.Add(sldZoom)
        panSignalSliders.Controls.Add(lblOffset)
        panSignalSliders.Controls.Add(Label2)
        panSignalSliders.Controls.Add(Label1)
        panSignalSliders.Location = New Point(0, 0)
        panSignalSliders.Name = "panSignalSliders"
        panSignalSliders.Size = New Size(59, 475)
        panSignalSliders.TabIndex = 21
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
        ' panSignal
        ' 
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(0, 0)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(879, 278)
        panSignal.TabIndex = 3
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Panel1.Controls.Add(sldContrast)
        Panel1.Controls.Add(Label8)
        Panel1.Location = New Point(881, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(78, 368)
        Panel1.TabIndex = 5
        ' 
        ' sldContrast
        ' 
        sldContrast.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldContrast.KnobColor = Color.DodgerBlue
        sldContrast.Location = New Point(9, 45)
        sldContrast.Maximum = 100
        sldContrast.Minimum = 0
        sldContrast.Name = "sldContrast"
        sldContrast.Size = New Size(60, 219)
        sldContrast.TabIndex = 31
        sldContrast.TextColor = Color.White
        sldContrast.TickColor = Color.LightGray
        sldContrast.TickSpacing = 10
        ToolTip1.SetToolTip(sldContrast, "Zoom level 100%=full spectrum, 10%=small spectrum")
        sldContrast.TrackColor = Color.Gray
        sldContrast.TrackHighlightColor = Color.LightGray
        sldContrast.Value = 0
        sldContrast.Vertical = True
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.BackColor = Color.Transparent
        Label8.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label8.ForeColor = Color.White
        Label8.Location = New Point(5, 21)
        Label8.Name = "Label8"
        Label8.Size = New Size(69, 21)
        Label8.TabIndex = 30
        Label8.Text = "Contrast"
        Label8.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' panWaterfall
        ' 
        panWaterfall.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panWaterfall.BackColor = Color.Black
        panWaterfall.Location = New Point(0, 0)
        panWaterfall.Name = "panWaterfall"
        panWaterfall.Size = New Size(879, 368)
        panWaterfall.TabIndex = 4
        ' 
        ' frmPlayback
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1266, 678)
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
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents panLeft As Panel
    Friend WithEvents splRight As SplitContainer
    Friend WithEvents panSignalCtrls As Panel
    Friend WithEvents panSignal As Panel
    Friend WithEvents picStartStop As PictureBox
    Friend WithEvents lvwArchives As ListView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label8 As Label
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
    Friend WithEvents sldOffset As CustomSlider
    Friend WithEvents sldRange As CustomSlider
    Friend WithEvents sldZoom As CustomSlider
    Friend WithEvents lblOffset As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents sldContrast As CustomSlider
    Friend WithEvents picBrowseFolder As PictureBox
    Friend WithEvents picExport As PictureBox
End Class
