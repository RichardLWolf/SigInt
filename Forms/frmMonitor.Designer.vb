<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMonitor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMonitor))
        panSignal = New Panel()
        lblEvents = New Label()
        Panel1 = New Panel()
        Label3 = New Label()
        lblDiv = New Label()
        lblOffset = New Label()
        Label2 = New Label()
        Label1 = New Label()
        sldZoom = New ctlRoundTrackbar()
        picStartStop = New PictureBox()
        ToolTip1 = New ToolTip(components)
        panRollingGraph = New Panel()
        lblConfiguration = New Label()
        sldRange = New ctlRoundTrackbar()
        sldOffset = New ctlRoundTrackbar()
        Panel1.SuspendLayout()
        CType(picStartStop, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' panSignal
        ' 
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(12, 70)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(950, 369)
        panSignal.TabIndex = 2
        ' 
        ' lblEvents
        ' 
        lblEvents.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblEvents.ForeColor = Color.Coral
        lblEvents.Location = New Point(77, 22)
        lblEvents.Name = "lblEvents"
        lblEvents.Size = New Size(945, 28)
        lblEvents.TabIndex = 18
        lblEvents.Text = "Click the play button to begin monitoring."
        lblEvents.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Panel1.BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(lblDiv)
        Panel1.Controls.Add(lblOffset)
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(sldZoom)
        Panel1.Controls.Add(sldRange)
        Panel1.Controls.Add(sldOffset)
        Panel1.Location = New Point(968, 70)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(59, 525)
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
        sldZoom.InnerPadding = 0
        sldZoom.KnobColor = Color.RoyalBlue
        sldZoom.KnobSize = 30
        sldZoom.KnobTextColor = Color.LightGoldenrodYellow
        sldZoom.Location = New Point(8, 12)
        sldZoom.Maximum = 100
        sldZoom.Minimum = 0
        sldZoom.Name = "sldZoom"
        sldZoom.Orientation = Orientation.Vertical
        sldZoom.ShowValueInKnob = True
        sldZoom.Size = New Size(41, 130)
        sldZoom.TabIndex = 0
        sldZoom.TickColor = Color.LightGray
        sldZoom.TickSize = 25
        sldZoom.TickSpacing = 10
        sldZoom.TrackColor = Color.Gray
        sldZoom.TrackWidth = 3
        sldZoom.Value = 0
        ' 
        ' picStartStop
        ' 
        picStartStop.Image = CType(resources.GetObject("picStartStop.Image"), Image)
        picStartStop.Location = New Point(12, 12)
        picStartStop.Name = "picStartStop"
        picStartStop.Size = New Size(48, 48)
        picStartStop.SizeMode = PictureBoxSizeMode.StretchImage
        picStartStop.TabIndex = 21
        picStartStop.TabStop = False
        ToolTip1.SetToolTip(picStartStop, "Start/Stop monitoring")
        ' 
        ' panRollingGraph
        ' 
        panRollingGraph.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panRollingGraph.BackColor = Color.Black
        panRollingGraph.Location = New Point(12, 445)
        panRollingGraph.Name = "panRollingGraph"
        panRollingGraph.Size = New Size(949, 150)
        panRollingGraph.TabIndex = 26
        ToolTip1.SetToolTip(panRollingGraph, "Rolling signal graph: Shows signal strength (yellow) and noise floor (cyan) over time. Each point represents the average power level at that moment.")
        ' 
        ' lblConfiguration
        ' 
        lblConfiguration.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblConfiguration.ForeColor = Color.WhiteSmoke
        lblConfiguration.Location = New Point(77, 51)
        lblConfiguration.Name = "lblConfiguration"
        lblConfiguration.Size = New Size(884, 16)
        lblConfiguration.TabIndex = 27
        lblConfiguration.Text = "Configuration"
        lblConfiguration.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' sldRange
        ' 
        sldRange.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldRange.InnerPadding = 0
        sldRange.KnobColor = Color.RoyalBlue
        sldRange.KnobSize = 30
        sldRange.KnobTextColor = Color.LightGoldenrodYellow
        sldRange.Location = New Point(8, 169)
        sldRange.Maximum = 100
        sldRange.Minimum = 0
        sldRange.Name = "sldRange"
        sldRange.Orientation = Orientation.Vertical
        sldRange.ShowValueInKnob = True
        sldRange.Size = New Size(41, 130)
        sldRange.TabIndex = 30
        sldRange.TickColor = Color.LightGray
        sldRange.TickSize = 25
        sldRange.TickSpacing = 10
        sldRange.TrackColor = Color.Gray
        sldRange.TrackWidth = 3
        sldRange.Value = 0
        ' 
        ' sldOffset
        ' 
        sldOffset.BackgroundColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        sldOffset.InnerPadding = 0
        sldOffset.KnobColor = Color.RoyalBlue
        sldOffset.KnobSize = 30
        sldOffset.KnobTextColor = Color.LightGoldenrodYellow
        sldOffset.Location = New Point(8, 329)
        sldOffset.Maximum = 100
        sldOffset.Minimum = 0
        sldOffset.Name = "sldOffset"
        sldOffset.Orientation = Orientation.Vertical
        sldOffset.ShowValueInKnob = True
        sldOffset.Size = New Size(41, 130)
        sldOffset.TabIndex = 31
        sldOffset.TickColor = Color.LightGray
        sldOffset.TickSize = 25
        sldOffset.TickSpacing = 10
        sldOffset.TrackColor = Color.Gray
        sldOffset.TrackWidth = 3
        sldOffset.Value = 0
        ' 
        ' frmMonitor
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(43), CByte(43), CByte(43))
        ClientSize = New Size(1029, 597)
        Controls.Add(lblConfiguration)
        Controls.Add(panRollingGraph)
        Controls.Add(picStartStop)
        Controls.Add(Panel1)
        Controls.Add(lblEvents)
        Controls.Add(panSignal)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmMonitor"
        Text = "SigInt Monitor"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(picStartStop, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub
    Friend WithEvents panSignal As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLogFile As TextBox
    Friend WithEvents lblEvents As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents lblOffset As Label
    Friend WithEvents lblDiv As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents picStartStop As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents panRollingGraph As Panel
    Friend WithEvents lblConfiguration As Label
    Friend WithEvents sldZoom As ctlRoundTrackbar
    Friend WithEvents sldRange As ctlRoundTrackbar
    Friend WithEvents sldOffset As ctlRoundTrackbar

End Class
