Imports System.Runtime.InteropServices
Imports MathNet.Numerics
Imports MathNet.Numerics.IntegralTransforms
Imports System.Drawing
Imports System.Numerics
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports System.Configuration
Imports System.Drawing.Design



Public Class frmMonitor
    Private WithEvents foSDR As RtlSdrApi = Nothing
    Private foSignalBMP As Bitmap = Nothing
    Private foRollingBMP As Bitmap = Nothing
    Private foBitmapsLock As New Object()
    Private foThingSpeak As clsThingSpeakAPI

    Private foNotify As New NotifyIcon With {.Icon = SystemIcons.Information}

    Private foAppConfig As clsAppConfig
    Private foConfig As DeviceConfig

    Private foBmpRend As clsRenderWaveform

    Private fiSdrDriverDeviceIndex As Integer
    Private fsDiscordWebHook As String = ""
    Private fsDiscordMention As String = ""
    Private fbUseThingSpeak As Boolean = False
    Private fsUserGUID As String = ""
    Private fdUserLat As Double = 0D
    Private fdUserLon As Double = 0D


    Public Sub ReadyForm(ByVal iSdrDeviceIndex As Integer, ByVal oConfigToUse As DeviceConfig _
                         , ByVal sDiscordWebHook As String, ByVal sDiscordMentionID As String _
                         , ByVal bThingSpeak As Boolean, ByVal sUserGUID As String, ByVal dUserLat As Double, ByVal dUserLon As Double)

        foConfig = oConfigToUse
        fiSdrDriverDeviceIndex = iSdrDeviceIndex
        fsDiscordMention = sDiscordMentionID
        fsDiscordWebHook = sDiscordWebHook
        fbUseThingSpeak = bThingSpeak
        fsUserGUID = sUserGUID
        fdUserLat = dUserLat
        fdUserLon = dUserLon


        ' make sure we're doublebuffered
        Me.DoubleBuffered = True
        Dim fi As System.Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        If fi IsNot Nothing Then fi.SetValue(panSignal, True, Nothing)
        If fi IsNot Nothing Then fi.SetValue(panRollingGraph, True, Nothing)

        ' Initialize bitmap rendering class
        foBmpRend = New clsRenderWaveform(panSignal.Width, panSignal.Height)

        ' update config
        SetConfigLabel()

        sldZoom.Minimum = 0
        sldZoom.Maximum = 100
        sldZoom.Value = foConfig.ZoomLevel

        sldOffset.Minimum = -100
        sldOffset.Maximum = 0
        sldOffset.Value = foConfig.dBOffset

        sldRange.Minimum = 10
        sldRange.Maximum = 150
        sldRange.Value = foConfig.dBRange

        sldOffset_ValueChanged(Nothing, Nothing)
        sldRange_ValueChanged(Nothing, Nothing)
        sldZoom_ValueChanged(Nothing, Nothing)
    End Sub

    Public Sub ApplyConfiguration(ByVal oConfigToUse As DeviceConfig, ByVal sDiscordWebHook As String, ByVal sDiscordMentionID As String)
        If foSDR IsNot Nothing Then
            If foSDR.IsRunning Then
                foSDR.StopMonitor()
            End If
            foSDR = Nothing
        End If
        ' load up new configs
        foConfig = oConfigToUse
        fsDiscordMention = sDiscordMentionID
        fsDiscordWebHook = sDiscordWebHook
        ' update config
        SetConfigLabel()
        panSignal.Invalidate()
        panRollingGraph.Invalidate()
    End Sub


    Private Sub foSDR_ErrorOccurred(sender As Object, message As String) Handles foSDR.ErrorOccurred
        MsgBox($"An error occurred monitoring the RTL-SDR:{ControlChars.CrLf}{message}", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "SDR Error")
    End Sub

    Private Sub foSDR_MonitorEnded(Sender As Object) Handles foSDR.MonitorEnded
        picStartStop.Image = My.Resources.media_play_green
        Me.Enabled = True
        Me.Cursor = Cursors.Arrow
        panSignal.Invalidate()
        panRollingGraph.Invalidate()
    End Sub

    Private Sub foSDR_SignalChange(sender As Object, SignalFound As Boolean) Handles foSDR.RecordingEvent
        UpdateEventCount()
        If SignalFound Then
            foNotify.Text = "New Event Detected."
            foNotify.ShowBalloonTip(3000, "SigInt Event", foNotify.Text, ToolTipIcon.Info)
        End If
    End Sub

    Private Sub foSDR_MonitorStarted(sender As Object, success As Boolean) Handles foSDR.MonitorStarted
        Me.Cursor = Cursors.Arrow
        Me.Enabled = True
        If success Then
            picStartStop.Image = My.Resources.media_stop_red
            Dim worker As New Threading.Thread(AddressOf Worker_GenerateBitmap)
            worker.IsBackground = True
            worker.Start()
        Else
            picStartStop.Image = My.Resources.media_play_green
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
        Dim poAppCfg As clsAppConfig = clsAppConfig.Load()
        poAppCfg.SetDeviceConfig(foConfig)
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DoubleBuffered = True

        Dim fi As System.Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        If fi IsNot Nothing Then fi.SetValue(panSignal, True, Nothing)

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
            ' clear display
            g.Clear(Color.Black) ' Clear panel background
        Else
            ' Draw the latest signal bitmap if available
            If foRollingBMP IsNot Nothing Then
                SyncLock foRollingBMP
                    g.DrawImage(foRollingBMP, 0, 0, panRollingGraph.Width, panRollingGraph.Height)
                End SyncLock
            Else
                g.Clear(Color.Black)
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

    Private Sub picStartStop_Click(sender As Object, e As EventArgs) Handles picStartStop.MouseClick
        If fiSdrDriverDeviceIndex < 0 Then
            MsgBox("Cannot start device.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
        Else
            If foSDR Is Nothing Then
                Dim poSdrCfg As New RtlSdrApi.SDRConfiguration(fiSdrDriverDeviceIndex)
                With poSdrCfg
                    .iDeviceIndex = fiSdrDriverDeviceIndex
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
                    .sDiscordWebhook = fsDiscordWebHook
                    .sDiscordMention = fsDiscordMention
                    .bThingSpeakEnabled = fbUseThingSpeak
                End With
                foSDR = New RtlSdrApi(poSdrCfg)
            End If
            If foSDR.IsRunning Then
                foSDR.StopMonitor()
            Else
                foSDR.StartMonitor()
                UpdateEventCount()
                ' ready ThingSpeak recording
                If fbUseThingSpeak AndAlso foThingSpeak Is Nothing Then
                    foThingSpeak = New clsThingSpeakAPI(String.Format("{0:F6},{1:F6}", fdUserLat, fdUserLon), fsUserGUID)
                End If
                Me.Cursor = Cursors.WaitCursor
                Me.Enabled = False
            End If
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
            foBmpRend.ZoomFactor = sldZoom.Value * 0.009D + 0.1D ' Convert range 0-100 → 0.1-1.0
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
        Try
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
        Catch ex As Exception
            ' assume disposed object error because form was closed or end-tasked.
        End Try
    End Sub


    Private Function GenerateSignalBitmap(buffer As Byte(), BitmapWidth As Integer, BitmapHeight As Integer) As Bitmap
        If BitmapWidth <= 0 Or BitmapHeight <= 0 Then Return foSignalBMP ' Return cached bitmap if invalid

        Dim piFftSize As Integer = buffer.Length \ 2
        Dim pdPowerValues() As Double = RtlSdrApi.ConvertRawToPowerLevels(buffer)
        Return foBmpRend.RenderGraph(BitmapWidth, BitmapHeight, pdPowerValues, foSDR.SampleRate, foSDR.CenterFrequency _
                                     , foSDR.IsRecording, foSDR.RecordingElapsed _
                                     , foSDR.DeviceName, foSDR.MonitorElapsed)
    End Function


    Private Sub UpdateEventCount()
        If foSDR IsNot Nothing Then
            If foSDR.SignalEventCount = 0 And foSDR.NoiseFloorEventCount = 0 Then
                lblEvents.Text = "No events detected during this session."
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
            End If
        Else
            lblEvents.Text = "No events detected during this session."
        End If
    End Sub

    Private Sub SetConfigLabel()
        If foConfig Is Nothing Then
            lblConfiguration.Text = ""
        Else
            lblConfiguration.Text = $"{foConfig.ConfigurationName} - {modMain.FormatHertz(foConfig.CenterFrequency)} - {modMain.FormatMSPS(foConfig.SampleRate)}"
        End If
    End Sub


    Private Function GetSignalPanelSize() As Size
        Try
            If panSignal.InvokeRequired Then
                ' Invoke on the UI thread if called from a worker thread
                Return CType(panSignal.Invoke(Function() GetSignalPanelSize()), Size)
            Else
                ' Directly return panel size if already on UI thread
                Return New Size(panSignal.Width, panSignal.Height)
            End If
        Catch ex As Exception
            Return New Size(0, 0)
        End Try
    End Function

    Private Function GetRollingPanelSize() As Size
        Try
            If panRollingGraph.InvokeRequired Then
                ' Invoke on the UI thread if called from a worker thread
                Return CType(panRollingGraph.Invoke(Function() GetRollingPanelSize()), Size)
            Else
                ' Directly return panel size if already on UI thread
                Return New Size(panRollingGraph.Width, panRollingGraph.Height)
            End If
        Catch ex As Exception
            Return New Size(0, 0)
        End Try
    End Function

End Class
''
