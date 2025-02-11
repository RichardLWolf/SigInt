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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        cboDeviceList = New ComboBox()
        lblSdrDevices = New Label()
        panSignal = New Panel()
        btnStartStop = New Button()
        trkZoomFactor = New TrackBar()
        trkRange = New TrackBar()
        trkOffset = New TrackBar()
        grpZoom = New GroupBox()
        grpRange = New GroupBox()
        grpOffset = New GroupBox()
        btnBrowseLogs = New Button()
        lblEvents = New Label()
        CType(trkZoomFactor, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkRange, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkOffset, ComponentModel.ISupportInitialize).BeginInit()
        grpZoom.SuspendLayout()
        grpRange.SuspendLayout()
        grpOffset.SuspendLayout()
        SuspendLayout()
        ' 
        ' cboDeviceList
        ' 
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
        lblSdrDevices.Location = New Point(12, 9)
        lblSdrDevices.Name = "lblSdrDevices"
        lblSdrDevices.Size = New Size(118, 21)
        lblSdrDevices.TabIndex = 1
        lblSdrDevices.Text = "SDR Device List"
        ' 
        ' panSignal
        ' 
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(12, 70)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(1005, 421)
        panSignal.TabIndex = 2
        ' 
        ' btnStartStop
        ' 
        btnStartStop.Location = New Point(273, 34)
        btnStartStop.Name = "btnStartStop"
        btnStartStop.Size = New Size(100, 30)
        btnStartStop.TabIndex = 3
        btnStartStop.Text = "Start Monitor"
        btnStartStop.UseVisualStyleBackColor = True
        ' 
        ' trkZoomFactor
        ' 
        trkZoomFactor.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkZoomFactor.Location = New Point(6, 23)
        trkZoomFactor.Maximum = 100
        trkZoomFactor.Minimum = 1
        trkZoomFactor.Name = "trkZoomFactor"
        trkZoomFactor.Size = New Size(188, 45)
        trkZoomFactor.TabIndex = 4
        trkZoomFactor.TickFrequency = 10
        trkZoomFactor.TickStyle = TickStyle.Both
        trkZoomFactor.Value = 100
        ' 
        ' trkRange
        ' 
        trkRange.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkRange.Location = New Point(6, 23)
        trkRange.Maximum = 150
        trkRange.Minimum = 10
        trkRange.Name = "trkRange"
        trkRange.Size = New Size(188, 45)
        trkRange.TabIndex = 6
        trkRange.TickFrequency = 10
        trkRange.TickStyle = TickStyle.Both
        trkRange.Value = 100
        ' 
        ' trkOffset
        ' 
        trkOffset.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkOffset.Location = New Point(6, 23)
        trkOffset.Maximum = 0
        trkOffset.Minimum = -150
        trkOffset.Name = "trkOffset"
        trkOffset.Size = New Size(188, 45)
        trkOffset.TabIndex = 8
        trkOffset.TickFrequency = 10
        trkOffset.TickStyle = TickStyle.Both
        trkOffset.Value = -20
        ' 
        ' grpZoom
        ' 
        grpZoom.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        grpZoom.BackColor = SystemColors.ControlDark
        grpZoom.Controls.Add(trkZoomFactor)
        grpZoom.Location = New Point(12, 497)
        grpZoom.Name = "grpZoom"
        grpZoom.Size = New Size(200, 74)
        grpZoom.TabIndex = 12
        grpZoom.TabStop = False
        grpZoom.Text = "Zoom - 100%"
        ' 
        ' grpRange
        ' 
        grpRange.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        grpRange.BackColor = SystemColors.ControlDark
        grpRange.Controls.Add(trkRange)
        grpRange.Location = New Point(231, 497)
        grpRange.Name = "grpRange"
        grpRange.Size = New Size(200, 74)
        grpRange.TabIndex = 13
        grpRange.TabStop = False
        grpRange.Text = "Range - 100dB"
        ' 
        ' grpOffset
        ' 
        grpOffset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        grpOffset.BackColor = SystemColors.ControlDark
        grpOffset.Controls.Add(trkOffset)
        grpOffset.Location = New Point(450, 497)
        grpOffset.Name = "grpOffset"
        grpOffset.Size = New Size(200, 74)
        grpOffset.TabIndex = 14
        grpOffset.TabStop = False
        grpOffset.Text = "Offset  -20dB"
        ' 
        ' btnBrowseLogs
        ' 
        btnBrowseLogs.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnBrowseLogs.Location = New Point(864, 521)
        btnBrowseLogs.Name = "btnBrowseLogs"
        btnBrowseLogs.Size = New Size(153, 50)
        btnBrowseLogs.TabIndex = 17
        btnBrowseLogs.Text = "📂 Browse Logs"
        btnBrowseLogs.TextImageRelation = TextImageRelation.ImageBeforeText
        btnBrowseLogs.UseVisualStyleBackColor = True
        ' 
        ' lblEvents
        ' 
        lblEvents.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblEvents.Location = New Point(450, 34)
        lblEvents.Name = "lblEvents"
        lblEvents.Size = New Size(567, 28)
        lblEvents.TabIndex = 18
        lblEvents.Text = "No Signals Deteceted"
        lblEvents.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1029, 583)
        Controls.Add(lblEvents)
        Controls.Add(btnBrowseLogs)
        Controls.Add(grpOffset)
        Controls.Add(grpRange)
        Controls.Add(grpZoom)
        Controls.Add(btnStartStop)
        Controls.Add(panSignal)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4)
        Name = "frmMain"
        Text = "SigInt"
        CType(trkZoomFactor, ComponentModel.ISupportInitialize).EndInit()
        CType(trkRange, ComponentModel.ISupportInitialize).EndInit()
        CType(trkOffset, ComponentModel.ISupportInitialize).EndInit()
        grpZoom.ResumeLayout(False)
        grpZoom.PerformLayout()
        grpRange.ResumeLayout(False)
        grpRange.PerformLayout()
        grpOffset.ResumeLayout(False)
        grpOffset.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents panSignal As Panel
    Friend WithEvents btnStartStop As Button
    Friend WithEvents trkZoomFactor As TrackBar
    Friend WithEvents trkRange As TrackBar
    Friend WithEvents trkOffset As TrackBar
    Friend WithEvents grpZoom As GroupBox
    Friend WithEvents grpRange As GroupBox
    Friend WithEvents grpOffset As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLogFile As TextBox
    Friend WithEvents btnBrowseLogs As Button
    Friend WithEvents lblEvents As Label

End Class
