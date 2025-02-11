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
        cboDeviceList = New ComboBox()
        lblSdrDevices = New Label()
        panSignal = New Panel()
        btnStartStop = New Button()
        trkZoomFactor = New TrackBar()
        lblZoom = New Label()
        lblRange = New Label()
        trkRange = New TrackBar()
        lblOffset = New Label()
        trkOffset = New TrackBar()
        lblContrast = New Label()
        trkContrast = New TrackBar()
        panWaterfall = New Panel()
        CType(trkZoomFactor, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkRange, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkOffset, ComponentModel.ISupportInitialize).BeginInit()
        CType(trkContrast, ComponentModel.ISupportInitialize).BeginInit()
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
        panSignal.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panSignal.BackColor = Color.Black
        panSignal.Location = New Point(12, 70)
        panSignal.Name = "panSignal"
        panSignal.Size = New Size(940, 300)
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
        trkZoomFactor.Location = New Point(982, 33)
        trkZoomFactor.Maximum = 100
        trkZoomFactor.Minimum = 1
        trkZoomFactor.Name = "trkZoomFactor"
        trkZoomFactor.Orientation = Orientation.Vertical
        trkZoomFactor.Size = New Size(45, 139)
        trkZoomFactor.TabIndex = 4
        trkZoomFactor.TickFrequency = 10
        trkZoomFactor.TickStyle = TickStyle.None
        trkZoomFactor.Value = 100
        ' 
        ' lblZoom
        ' 
        lblZoom.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblZoom.AutoSize = True
        lblZoom.Location = New Point(982, 9)
        lblZoom.Name = "lblZoom"
        lblZoom.Size = New Size(50, 21)
        lblZoom.TabIndex = 5
        lblZoom.Text = "100%"
        ' 
        ' lblRange
        ' 
        lblRange.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblRange.AutoSize = True
        lblRange.Location = New Point(977, 176)
        lblRange.Name = "lblRange"
        lblRange.Size = New Size(54, 21)
        lblRange.TabIndex = 7
        lblRange.Text = "Range"
        ' 
        ' trkRange
        ' 
        trkRange.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkRange.Location = New Point(982, 201)
        trkRange.Maximum = 150
        trkRange.Minimum = 10
        trkRange.Name = "trkRange"
        trkRange.Orientation = Orientation.Vertical
        trkRange.Size = New Size(45, 139)
        trkRange.TabIndex = 6
        trkRange.TickFrequency = 10
        trkRange.TickStyle = TickStyle.None
        trkRange.Value = 100
        ' 
        ' lblOffset
        ' 
        lblOffset.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblOffset.AutoSize = True
        lblOffset.Location = New Point(977, 343)
        lblOffset.Name = "lblOffset"
        lblOffset.Size = New Size(52, 21)
        lblOffset.TabIndex = 9
        lblOffset.Text = "Offset"
        ' 
        ' trkOffset
        ' 
        trkOffset.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkOffset.Location = New Point(982, 368)
        trkOffset.Maximum = 0
        trkOffset.Minimum = -150
        trkOffset.Name = "trkOffset"
        trkOffset.Orientation = Orientation.Vertical
        trkOffset.Size = New Size(45, 139)
        trkOffset.TabIndex = 8
        trkOffset.TickFrequency = 10
        trkOffset.TickStyle = TickStyle.None
        trkOffset.Value = -20
        ' 
        ' lblContrast
        ' 
        lblContrast.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblContrast.AutoSize = True
        lblContrast.Location = New Point(958, 510)
        lblContrast.Name = "lblContrast"
        lblContrast.Size = New Size(69, 21)
        lblContrast.TabIndex = 11
        lblContrast.Text = "Contrast"
        ' 
        ' trkContrast
        ' 
        trkContrast.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trkContrast.Location = New Point(982, 535)
        trkContrast.Maximum = 100
        trkContrast.Minimum = 10
        trkContrast.Name = "trkContrast"
        trkContrast.Orientation = Orientation.Vertical
        trkContrast.Size = New Size(45, 139)
        trkContrast.TabIndex = 10
        trkContrast.TickFrequency = 10
        trkContrast.TickStyle = TickStyle.None
        trkContrast.Value = 100
        ' 
        ' panWaterfall
        ' 
        panWaterfall.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panWaterfall.BackColor = Color.Black
        panWaterfall.Location = New Point(12, 384)
        panWaterfall.Name = "panWaterfall"
        panWaterfall.Size = New Size(940, 300)
        panWaterfall.TabIndex = 12
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(9F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1029, 696)
        Controls.Add(panWaterfall)
        Controls.Add(lblContrast)
        Controls.Add(trkContrast)
        Controls.Add(lblOffset)
        Controls.Add(trkOffset)
        Controls.Add(lblRange)
        Controls.Add(trkRange)
        Controls.Add(lblZoom)
        Controls.Add(trkZoomFactor)
        Controls.Add(btnStartStop)
        Controls.Add(panSignal)
        Controls.Add(lblSdrDevices)
        Controls.Add(cboDeviceList)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Margin = New Padding(4)
        Name = "frmMain"
        Text = "SigInt"
        CType(trkZoomFactor, ComponentModel.ISupportInitialize).EndInit()
        CType(trkRange, ComponentModel.ISupportInitialize).EndInit()
        CType(trkOffset, ComponentModel.ISupportInitialize).EndInit()
        CType(trkContrast, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cboDeviceList As ComboBox
    Friend WithEvents lblSdrDevices As Label
    Friend WithEvents panSignal As Panel
    Friend WithEvents btnStartStop As Button
    Friend WithEvents trkZoomFactor As TrackBar
    Friend WithEvents lblZoom As Label
    Friend WithEvents lblRange As Label
    Friend WithEvents trkRange As TrackBar
    Friend WithEvents lblOffset As Label
    Friend WithEvents trkOffset As TrackBar
    Friend WithEvents lblContrast As Label
    Friend WithEvents trkContrast As TrackBar
    Friend WithEvents panWaterfall As Panel

End Class
