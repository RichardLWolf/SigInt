Imports System.Runtime.InteropServices
Imports MathNet.Numerics
Imports MathNet.Numerics.IntegralTransforms
Imports System.Drawing
Imports System.Numerics
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports System.Configuration
Imports System.Drawing.Design



Public Class frmMain
    Private WithEvents foSDR As RtlSdrApi = Nothing
    Private foSignalBMP As Bitmap = Nothing
    Private foRollingBMP As Bitmap = Nothing
    'Private foWaterfallBMP As Bitmap = Nothing
    Private foBitmapsLock As New Object()

    'Private fdZoomFactor As Double = 0 ' no zoom, show full graph, to 1.0 (show double resolution).
    'Private fddBOffset As Double = -20 ' Adjusts the dB scaling this value is added to the Y-axis start (0 to -100) 
    'Private fddBRange As Double = 100   ' dB range to display on Y-axis (100dB range, 10dB minimum to 150dB maximum)
    Private foNotify As New NotifyIcon With {.Icon = SystemIcons.Information}

    Private foConfig As clsAppConfig

    Private foBmpRend As clsRenderWaveform


    Private Sub foSDR_ErrorOccurred(sender As Object, message As String) Handles foSDR.ErrorOccurred
        MsgBox($"An error occurred monitoring the RTL-SDR:{ControlChars.CrLf}{message}", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "SDR Error")
    End Sub

    Private Sub foSDR_MonitorEnded(Sender As Object) Handles foSDR.MonitorEnded
        picStartStop.Image = My.Resources.media_play_green
        picConfig.Image = My.Resources.gear_gray
        Me.Enabled = True
        Me.Cursor = Cursors.Arrow
        panSignal.Invalidate()
        panRollingGraph.Invalidate()
    End Sub


    Private Sub foSDR_SignalChange(sender As Object, SignalFound As Boolean) Handles foSDR.RecordingEvent
        'update signal count
        If foSDR.SignalEventCount = 0 And foSDR.NoiseFloorEventCount = 0 Then
            lblEvents.Text = "No signals found during this session."
        Else
            Dim psLbl As String = ""
            If foSDR.SignalEventCount > 0 Then
                psLbl = psLbl & $"{foSDR.SignalEventCount} signal event{IIf(foSDR.SignalEventCount > 1, "s", "")}"
            End If
            If foSDR.NoiseFloorEventCount > 0 Then
                psLbl = $"{If(psLbl = "", "", ", ")}{foSDR.NoiseFloorEventCount} noise floor event{IIf(foSDR.NoiseFloorEventCount > 1, "s", "")}"
            End If
            psLbl = psLbl & " detected this session."
            lblEvents.Text = psLbl
            If SignalFound Then
                foNotify.Text = psLbl
                foNotify.ShowBalloonTip(3000, "SigInt Event", psLbl, ToolTipIcon.Info)
            End If
        End If
    End Sub

    Private Sub foSDR_MonitorStarted(sender As Object, success As Boolean) Handles foSDR.MonitorStarted
        Me.Cursor = Cursors.Arrow
        Me.Enabled = True
        If success Then
            picStartStop.Image = My.Resources.media_stop_red
            picConfig.Image = My.Resources.gear
            Dim worker As New Threading.Thread(AddressOf Worker_GenerateBitmap)
            worker.IsBackground = True
            worker.Start()
        Else
            picStartStop.Image = My.Resources.media_play_green
            picConfig.Image = My.Resources.gear_gray
        End If
    End Sub


    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
            foSDR.StopMonitor()
        End If
        ' save off current UI config 
        foConfig.ZoomLevel = sldZoom.Value
        foConfig.dBOffset = sldOffset.Value
        foConfig.dBRange = sldRange.Value
        foConfig.Save()
        ' shut down unhandled exception handler
        clsUEH.StopUEH()
    End Sub


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        foConfig = clsAppConfig.Load

        ' start the unhandled exception handler
        clsUEH.StartUEH()

        DoubleBuffered = True

        Dim fi As System.Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        If fi IsNot Nothing Then fi.SetValue(panSignal, True, Nothing)

        cboDeviceList.Items.Clear()
        Dim poDevs As List(Of RtlSdrApi.SdrDevice) = RtlSdrApi.GetDevices()
        If poDevs.Count = 0 Then
            cboDeviceList.Items.Add(New RtlSdrApi.SdrDevice("No SDR devices found.", -1))
        Else
            cboDeviceList.Items.AddRange(poDevs.Cast(Of Object).ToArray())
        End If
        cboDeviceList.SelectedIndex = 0

        ' Initialize bitmap rendering class
        foBmpRend = New clsRenderWaveform(panSignal.Width, panSignal.Height)

        sldZoom.Value = foConfig.ZoomLevel
        sldOffset.Value = foConfig.dBOffset
        sldRange.Value = foConfig.dBRange
        sldOffset_ValueChanged(Nothing, Nothing)
        sldRange_ValueChanged(Nothing, Nothing)
        sldZoom_ValueChanged(Nothing, Nothing)


        If clsLogger.ValidateLogFolder() = False Then
            MsgBox($"Failed to create program log file folder:{ControlChars.CrLf}{clsLogger.LogFileName}")
        End If
        clsLogger.PurgeLog()
    End Sub

    Private Sub panRollingGraph_Paint(sender As Object, e As PaintEventArgs) Handles panRollingGraph.Paint
        Dim g As Graphics = e.Graphics

        If foSDR Is Nothing OrElse Not foSDR.IsRunning Then
            ' Show "DISCONNECTED" message
            g.Clear(Color.Black) ' Clear panel background
        Else
            ' Draw the latest signal bitmap if available
            If foRollingBMP IsNot Nothing Then
                SyncLock foRollingBMP
                    g.DrawImage(foRollingBMP, 0, 0, panRollingGraph.Width, panRollingGraph.Height)
                End SyncLock
            End If
        End If
    End Sub


    Private Sub panSignal_Paint(sender As Object, e As PaintEventArgs) Handles panSignal.Paint
        Dim g As Graphics = e.Graphics

        If foSDR Is Nothing OrElse Not foSDR.IsRunning Then
            ' Show "DISCONNECTED" message
            g.Clear(Color.Black) ' Clear panel background
            Dim text As String = "** DISCONNECTED **"
            Dim font As New Font("Segoe UI", 14, FontStyle.Bold)
            Dim textSize As SizeF = g.MeasureString(text, font)
            Dim textPos As New PointF((panSignal.Width - textSize.Width) / 2, (panSignal.Height - textSize.Height) / 2)
            g.DrawString(text, font, Brushes.Red, textPos)
        Else
            ' Draw the latest signal bitmap if available
            If foSignalBMP IsNot Nothing Then
                SyncLock foBitmapsLock
                    g.DrawImage(foSignalBMP, 0, 0, panSignal.Width, panSignal.Height)
                End SyncLock
            End If
        End If
    End Sub

    Private Sub panRollingGraph_Resize(sender As Object, e As EventArgs) Handles panRollingGraph.Resize
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning = False Then
            panRollingGraph.Invalidate()
        End If

    End Sub

    Private Sub panSignal_Resize(sender As Object, e As EventArgs) Handles panSignal.Resize
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning = False Then
            panSignal.Invalidate()
        End If
    End Sub

    Private Sub picBrowseFolder_MouseClick(sender As Object, e As MouseEventArgs) Handles picBrowseFolder.MouseClick
        Try
            Process.Start("explorer.exe", clsLogger.LogPath)
        Catch ex As Exception
            MsgBox("Failed to start windows process in folder " & clsLogger.LogPath, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Cannot Start Process")
        End Try
    End Sub

    Private Sub picBrowseFolder_MouseEnter(sender As Object, e As EventArgs) Handles picBrowseFolder.MouseEnter
        picBrowseFolder.Image = My.Resources.folder_blue
    End Sub

    Private Sub picBrowseFolder_MouseLeave(sender As Object, e As EventArgs) Handles picBrowseFolder.MouseLeave
        picBrowseFolder.Image = My.Resources.folder_green
    End Sub

    Private Sub picConfig_Click(sender As Object, e As EventArgs) Handles picConfig.Click
        If cboDeviceList.SelectedItem IsNot Nothing Then
            If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
                Using poFrm As New frmConfig
                    Dim poItem As RtlSdrApi.SdrDevice = cboDeviceList.SelectedItem
                    poFrm.ReadyForm(poItem.DeviceName, foSDR, foConfig)
                    If poFrm.ShowDialog(Me) = DialogResult.OK Then
                        ' stop monitor
                        foSDR.StopMonitor()
                        Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(poItem.DeviceIndex)
                        With poSdrCfg
                            .iDeviceIndex = poItem.DeviceIndex
                            .iCenterFrequency = poFrm.CenterFreq
                            .iSampleRate = poFrm.SampleRate
                            .iSignalInitTime = poFrm.SignalInitTime
                            .iSignalDetectionThreshold = poFrm.DetectionThreshold
                            .iSignalDetectionWindow = poFrm.DetectionWindow
                            .bAutomaticGain = If(poFrm.GainMode = 0, True, False)
                            .iManualGainValue = poFrm.GainValue
                            .dSignalEventResetTime = poFrm.SignalEventResetTime
                            .iNoiseFloorBaselineInitTime = poFrm.NoiseFloorBaselineInitTime
                            .iNoiseFloorCooldownDuration = poFrm.NoiseFloorCooldownDuration
                            .iNoiseFloorEventResetTime = poFrm.NoiseFloorEventResetTime
                            .iNoiseFloorMinEventDuration = poFrm.NoiseFloorMinEventDuration
                            .dNoiseFloorThreshold = poFrm.NoiseFloorThreshold
                            .sDiscordWebhook = poFrm.DiscordServerWebhook
                            .sDiscordMention = poFrm.DiscordMentionID
                        End With
                        foSDR = New RtlSdrApi(poSdrCfg)
                        foSDR.StartMonitor()
                        ' update config values from API class as it will enforce property value limits
                        foConfig.CenterFrequency = foSDR.CenterFrequency
                        foConfig.SampleRate = foSDR.SampleRate
                        foConfig.GainMode = foSDR.GainMode
                        foConfig.GainValue = foSDR.GainValue
                        foConfig.SignalEventResetTime = foSDR.SignalEventResetTime
                        foConfig.SignalDetectionThreshold = foSDR.SignalDetectionThreshold
                        foConfig.SignalDetectionWindow = foSDR.SignalDetectionWindow
                        foConfig.SignalInitTime = foSDR.SignalInitTime
                        foConfig.NoiseFloorBaselineInitTime = foSDR.NoiseFloorBaselineInitTime
                        foConfig.NoiseFloorCooldownDuration = foSDR.NoiseFloorCooldownDuration
                        foConfig.NoiseFloorEventResetTime = foSDR.NoiseFloorEventResetTime
                        foConfig.NoiseFloorMinEventDuration = foSDR.NoiseFloorMinEventDuration
                        foConfig.NoiseFloorThreshold = foSDR.NoiseFloorThreshold
                        foConfig.DiscordNotifications = poFrm.DiscordNotifications
                        If Not foConfig.DiscordNotifications Then
                            foConfig.DiscordServerWebhook = ""
                            foConfig.DiscordMentionID = ""
                        Else
                            foConfig.DiscordServerWebhook = poFrm.DiscordServerWebhook
                            foConfig.DiscordMentionID = poFrm.DiscordMentionID
                        End If
                        ' update config
                        foConfig.Save()
                    End If
                End Using
            Else
                MsgBox("Please begin monitoring before adjusting device settings.")
            End If
        Else
            cboDeviceList.Focus()
            MsgBox("Please select a RTL-SDR device from the list.")
        End If
    End Sub

    Private Sub picConfig_MouseEnter(sender As Object, e As EventArgs) Handles picConfig.MouseEnter
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
            picConfig.Image = My.Resources.gear_blue
        Else
            picConfig.Image = My.Resources.gear_gray
        End If
    End Sub

    Private Sub picConfig_MouseLeave(sender As Object, e As EventArgs) Handles picConfig.MouseLeave
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
            picConfig.Image = My.Resources.gear
        Else
            picConfig.Image = My.Resources.gear_gray
        End If
    End Sub

    Private Sub picPlayback_Click(sender As Object, e As EventArgs) Handles picPlayback.Click
        Using poFrm As New frmPlayback
            poFrm.ReadyForm()
            poFrm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub picPlayback_MouseEnter(sender As Object, e As EventArgs) Handles picPlayback.MouseEnter
        picPlayback.Image = My.Resources.microphone2_blue
    End Sub

    Private Sub picPlayback_MouseLeave(sender As Object, e As EventArgs) Handles picPlayback.MouseLeave
        picPlayback.Image = My.Resources.microphone2
    End Sub

    Private Sub picViewLog_Click(sender As Object, e As EventArgs) Handles picViewLog.Click
        Using poFrm As New frmViewLog
            poFrm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub picViewLog_MouseEnter(sender As Object, e As EventArgs) Handles picViewLog.MouseEnter
        picViewLog.Image = My.Resources.scroll_view_hover
    End Sub

    Private Sub picViewLog_MouseLeave(sender As Object, e As EventArgs) Handles picViewLog.MouseLeave
        picViewLog.Image = My.Resources.scroll_view
    End Sub


    Private Sub picStartStop_Click(sender As Object, e As EventArgs) Handles picStartStop.MouseClick
        If cboDeviceList.SelectedItem IsNot Nothing Then
            Dim poItem As RtlSdrApi.SdrDevice = cboDeviceList.SelectedItem
            If poItem.DeviceIndex < 0 Then
                MsgBox("Please select a device from the dropdown.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
                cboDeviceList.Focus()
            Else
                If foSDR Is Nothing Then
                    Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(poItem.DeviceIndex)
                    With poSdrCfg
                        .iDeviceIndex = poItem.DeviceIndex
                        .iCenterFrequency = foConfig.CenterFrequency
                        .iSampleRate = foConfig.SampleRate
                        .iSignalInitTime = foConfig.SignalInitTime
                        .iSignalDetectionThreshold = foConfig.SignalDetectionThreshold
                        .iSignalDetectionWindow = foConfig.SignalDetectionWindow
                        .bAutomaticGain = If(foConfig.GainMode = 0, True, False)
                        .iManualGainValue = foConfig.GainValue
                        .dSignalEventResetTime = foConfig.SignalEventResetTime
                        .iNoiseFloorBaselineInitTime = foConfig.NoiseFloorBaselineInitTime
                        .iNoiseFloorCooldownDuration = foConfig.NoiseFloorCooldownDuration
                        .iNoiseFloorEventResetTime = foConfig.NoiseFloorEventResetTime
                        .iNoiseFloorMinEventDuration = foConfig.NoiseFloorMinEventDuration
                        .dNoiseFloorThreshold = foConfig.NoiseFloorThreshold
                        .sDiscordWebhook = foConfig.DiscordServerWebhook
                        .sDiscordMention = foConfig.DiscordMentionID
                    End With
                    foSDR = New RtlSdrApi(poSdrCfg)
                End If
                If foSDR.IsRunning Then
                    foSDR.StopMonitor()
                Else
                    foSDR.StartMonitor()
                    Me.Cursor = Cursors.WaitCursor
                    Me.Enabled = False
                End If
            End If
        Else
            MsgBox("Please select a device from the dropdown.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
            cboDeviceList.Focus()
        End If
    End Sub

    Private Sub picStartStop_MouseEnter(sender As Object, e As EventArgs) Handles picStartStop.MouseEnter
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
            picStartStop.Image = My.Resources.media_stop
        Else
            picStartStop.Image = My.Resources.media_play
        End If
    End Sub

    Private Sub picStartStop_MouseLeave(sender As Object, e As EventArgs) Handles picStartStop.MouseLeave
        If foSDR IsNot Nothing AndAlso foSDR.IsRunning Then
            picStartStop.Image = My.Resources.media_stop_red
        Else
            picStartStop.Image = My.Resources.media_play_green
        End If
    End Sub

    Private Sub sldOffset_ValueChanged(sender As Object, e As EventArgs) Handles sldOffset.ValueChanged
        If foBmpRend IsNot Nothing Then
            foBmpRend.dBOffset = sldOffset.Value
        End If
    End Sub

    Private Sub sldRange_ValueChanged(sender As Object, e As EventArgs) Handles sldRange.ValueChanged
        If foBmpRend IsNot Nothing Then
            foBmpRend.dBRange = sldRange.Value
        End If
    End Sub

    Private Sub sldZoom_ValueChanged(sender As Object, e As EventArgs) Handles sldZoom.ValueChanged
        If foBmpRend IsNot Nothing Then
            foBmpRend.ZoomFactor = (sldZoom.Value * 0.009D) + 0.1D ' Convert range 0-100 → 0.1-1.0
        End If
    End Sub

    Private Sub Worker_GenerateBitmap()
        While foSDR IsNot Nothing AndAlso foSDR.IsRunning
            Dim buffer As Byte() = foSDR.GetBuffer()
            If buffer IsNot Nothing AndAlso buffer.Length > 0 Then
                Dim panelSize As Size = GetSignalPanelSize()
                Dim poRollingSize As Size = GetRollingPanelSize()
                SyncLock foBitmapsLock
                    ' Signal graph
                    foSignalBMP = GenerateSignalBitmap(buffer, panelSize.Width, panelSize.Height)
                    ' update the rolling graph bitmap
                    Dim piTimeWindow As Integer = CInt(foSDR.RollingPowerlevelsFrameCount * foSDR.AverageMonitorLoopTime)
                    foRollingBMP = clsRenderWaveform.RenderRollingGraph(poRollingSize.Width, poRollingSize.Height, foSDR.RollingPowerLevels, piTimeWindow)
                End SyncLock
            Else
                clsLogger.Log("frmMain.Worker_GenerateBitmap", "Supplied data buffer was null or zero length.")
            End If

            UpdateSpectrum()
            System.Threading.Thread.Sleep(50) ' Adjust refresh rate as needed
        End While
    End Sub

    Private Sub UpdateSpectrum()
        If Me.InvokeRequired Then
            ' Marshal the update to the UI thread
            Me.BeginInvoke(New MethodInvoker(AddressOf UpdateSpectrum))
            Exit Sub
        End If

        If foSignalBMP IsNot Nothing Then
            If panSignal IsNot Nothing AndAlso panSignal.Handle <> IntPtr.Zero Then
                Using g As Graphics = panSignal.CreateGraphics()
                    SyncLock foBitmapsLock
                        g.DrawImageUnscaled(foSignalBMP, 0, 0) ' Direct draw, no Paint flickering
                    End SyncLock
                End Using
            End If
        End If

        If foRollingBMP IsNot Nothing Then
            If panRollingGraph IsNot Nothing AndAlso panRollingGraph.Handle <> IntPtr.Zero Then
                Using g As Graphics = panRollingGraph.CreateGraphics()
                    SyncLock foBitmapsLock
                        If foRollingBMP IsNot Nothing Then
                            g.DrawImageUnscaled(foRollingBMP, 0, 0) ' Direct draw, no Paint flickering
                        End If
                    End SyncLock
                End Using
            End If
        End If
    End Sub


    Private Function GenerateSignalBitmap(buffer As Byte(), BitmapWidth As Integer, BitmapHeight As Integer) As Bitmap
        If BitmapWidth <= 0 Or BitmapHeight <= 0 Then Return foSignalBMP ' Return cached bitmap if invalid

        Dim piFftSize As Integer = buffer.Length \ 2
        Dim pdPowerValues() As Double = RtlSdrApi.ConvertRawToPowerLevels(buffer)
        Return foBmpRend.RenderGraph(BitmapWidth, BitmapHeight, pdPowerValues, foSDR.SampleRate, foSDR.CenterFrequency _
                                     , foSDR.IsRecording, foSDR.RecordingElapsed _
                                     , foSDR.DeviceName, foSDR.MonitorElapsed)
    End Function



    Private Function GetSignalPanelSize() As Size
        If panSignal.InvokeRequired Then
            ' Invoke on the UI thread if called from a worker thread
            Return CType(panSignal.Invoke(Function() GetSignalPanelSize()), Size)
        Else
            ' Directly return panel size if already on UI thread
            Return New Size(panSignal.Width, panSignal.Height)
        End If
    End Function

    Private Function GetRollingPanelSize() As Size
        If panRollingGraph.InvokeRequired Then
            ' Invoke on the UI thread if called from a worker thread
            Return CType(panRollingGraph.Invoke(Function() GetRollingPanelSize()), Size)
        Else
            ' Directly return panel size if already on UI thread
            Return New Size(panRollingGraph.Width, panRollingGraph.Height)
        End If
    End Function

End Class
''
