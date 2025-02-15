Imports System.Runtime.InteropServices
Imports MathNet.Numerics
Imports MathNet.Numerics.IntegralTransforms
Imports System.Drawing
Imports System.Numerics
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports System.Configuration



Public Class frmMain
    Private WithEvents foSDR As RtlSdrApi = Nothing
    Private foSignalBMP As Bitmap = Nothing
    'Private foWaterfallBMP As Bitmap = Nothing
    Private foBitmapsLock As New Object()

    Private fdZoomFactor As Double = 0 ' no zoom, show full graph, to 1.0 (show double resolution).
    Private fddBOffset As Double = -20 ' Adjusts the dB scaling this value is added to the Y-axis start (0 to -100) 
    Private fddBRange As Double = 100   ' dB range to display on Y-axis (100dB range, 10dB minimum to 150dB maximum)
    Private foNotify As New NotifyIcon With {.Icon = SystemIcons.Information}

    Private foConfig As clsAppConfig

    Private Sub foSDR_ErrorOccurred(sender As Object, message As String) Handles foSDR.ErrorOccurred
        MsgBox($"An error occurred monitoring the RTL-SDR:{ControlChars.CrLf}{message}", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "SDR Error")
    End Sub

    Private Sub foSDR_MonitorEnded(Sender As Object) Handles foSDR.MonitorEnded
        picStartStop.Image = My.Resources.media_play_green
        picConfig.Image = My.Resources.gear_gray
        Me.Enabled = True
        Me.Cursor = Cursors.Arrow
        panSignal.Invalidate()
    End Sub


    Private Sub foSDR_SignalChange(sender As Object, SignalFound As Boolean) Handles foSDR.SignalChange
        'update signal count
        If foSDR.SignalEventCount = 0 Then
            lblEvents.Text = "No signals found during this session."
        Else
            lblEvents.Text = $"{foSDR.SignalEventCount} event{IIf(foSDR.SignalEventCount > 1, "s", "")} detected this session."
            If SignalFound Then
                foNotify.ShowBalloonTip(3000, "1.6Ghz Signal Detected", "Signal 10dB above noise floor was detected.", ToolTipIcon.Info)
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
    End Sub


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        foConfig = clsAppConfig.Load

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
                    poFrm.ReadyForm(poItem.DeviceName, foSDR)
                    If poFrm.ShowDialog(Me) = DialogResult.OK Then
                        ' stop monitor
                        foSDR.StopMonitor()
                        foSDR = New RtlSdrApi(poItem.DeviceIndex, poFrm.CenterFreq, , If(poFrm.GainMode = 0, True, False), poFrm.GainValue)
                        foSDR.StartMonitor()
                        foConfig.CenterFrequency = foSDR.CenterFrequency
                        foConfig.GainMode = poFrm.GainMode
                        foConfig.GainValue = foSDR.GainValue
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

    Private Sub picStartStop_Click(sender As Object, e As EventArgs) Handles picStartStop.MouseClick
        If cboDeviceList.SelectedItem IsNot Nothing Then
            Dim poItem As RtlSdrApi.SdrDevice = cboDeviceList.SelectedItem
            If poItem.DeviceIndex < 0 Then
                MsgBox("Please select a device from the dropdown.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
                cboDeviceList.Focus()
            Else
                If foSDR Is Nothing Then
                    foSDR = New RtlSdrApi(poItem.DeviceIndex, foConfig.CenterFrequency,, IIf(foConfig.GainMode = 0, True, False), foConfig.GainValue)
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
        fddBOffset = sldOffset.Value
    End Sub

    Private Sub sldRange_ValueChanged(sender As Object, e As EventArgs) Handles sldRange.ValueChanged
        fddBRange = sldRange.Value
    End Sub

    Private Sub sldZoom_ValueChanged(sender As Object, e As EventArgs) Handles sldZoom.ValueChanged
        '     convert to zoom factor
        fdZoomFactor = sldZoom.Value / 100.0 ' Convert range 10-100 → 0.1-1.0
    End Sub

    Private Sub Worker_GenerateBitmap()
        While foSDR IsNot Nothing AndAlso foSDR.IsRunning
            Dim buffer As Byte() = foSDR.GetBuffer()
            If buffer IsNot Nothing AndAlso buffer.Length > 0 Then
                Dim panelSize As Size = GetSignalPanelSize()
                SyncLock foBitmapsLock
                    foSignalBMP = GenerateSignalBitmap(buffer, panelSize.Width, panelSize.Height)
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
    End Sub


    Private Function GenerateSignalBitmap(buffer As Byte(), BitmapWidth As Integer, BitmapHeight As Integer) As Bitmap
        If BitmapWidth <= 0 Or BitmapHeight <= 0 Then Return foSignalBMP ' Return cached bitmap if invalid

        Dim piFftSize As Integer = buffer.Length \ 2
        Dim poComplexData(piFftSize - 1) As Complex
        Dim pdPowerValues(piFftSize - 1) As Double

        ' Convert raw IQ data to complex values
        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
            Dim pdInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
            Dim pdQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
            poComplexData(piIndex \ 2) = New Complex(pdInPhase, pdQuad)
        Next
        ' Perform FFT on the complex data
        Fourier.Forward(poComplexData, FourierOptions.NoScaling)
        ' Convert complex to dB values
        For piIndex = 0 To piFftSize - 1
            Dim pdRealPart As Double = poComplexData(piIndex).Real
            Dim pdImagPart As Double = poComplexData(piIndex).Imaginary
            pdPowerValues(piIndex) = pdRealPart * pdRealPart + pdImagPart * pdImagPart ' Compute power
            If pdPowerValues(piIndex) = 0 Then
                pdPowerValues(piIndex) = Double.NegativeInfinity
            Else
                pdPowerValues(piIndex) = 10 * Math.Log10(pdPowerValues(piIndex)) - 70
            End If
        Next
        ' Apply gaussian smooth to the levels
        pdPowerValues = GaussianSmooth(pdPowerValues, 7)
        ' we now have dB power values for the full buffer now.
        Return RenderSpectrumBitmap(BitmapWidth, BitmapHeight, pdPowerValues)
    End Function

    Public Function RenderSpectrumBitmap(ByVal BitmapWidth As Integer, ByVal BitmapHeight As Integer, ByVal pdPowerValues() As Double) As Bitmap
        ' Create graphics objects
        Dim poBitmap As New Bitmap(BitmapWidth, BitmapHeight)
        Using poGraphics As Graphics = Graphics.FromImage(poBitmap)
            ' how to render
            poGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            poGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            poGraphics.Clear(Color.Black)

            ' Define fonts and brushes
            Dim poLabelFont As New Font("Arial", 10, FontStyle.Regular)
            Dim poLabelBrush As New SolidBrush(Color.White)
            Dim poAxisPen As New Pen(Color.White, 2)
            Dim poGridPen As New Pen(Color.LightGray, 1) With {.DashStyle = Drawing2D.DashStyle.Dot}
            Dim poCenterFreqPen As New Pen(Color.Red, 1)
            ' Measure dB label width 
            Dim piDbLabelWidth As Integer = CInt(poGraphics.MeasureString("-888 dB", poLabelFont).Width)
            Dim piDbLabelHeight As Integer = CInt(poGraphics.MeasureString("-888 dB", poLabelFont).Height)
            Dim piWaveformAreaWidth As Integer = (piDbLabelWidth + 5) * 2 ' Both left & right
            ' Measure Frequency label size 
            Dim piFreqLabelHeight As Integer = CInt(poGraphics.MeasureString("8.888888 GHz", poLabelFont).Height) + 5 'give text some riser space
            Dim piFreqLabelWidth As Integer = CInt(poGraphics.MeasureString("8.888888 GHz", poLabelFont).Width)


            ' Define our workpace, working area with 5px margins left, right, top and 10px bottom.
            Dim poWorkingRect As New Rectangle(5, 5 + piDbLabelHeight, BitmapWidth - 10, BitmapHeight - 10 - piDbLabelHeight)
            ' Calculate graph area to allow for labels around it 
            Dim piGraphWidth As Integer = poWorkingRect.Width - piWaveformAreaWidth
            Dim piGraphHeight As Integer = poWorkingRect.Height - piFreqLabelHeight
            Dim poGraphRect As New Rectangle(poWorkingRect.X + (piWaveformAreaWidth \ 2), poWorkingRect.Y, piGraphWidth, piGraphHeight)
            ' Define string format for centering text
            Dim poCenterFormat As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .FormatFlags = StringFormatFlags.NoWrap}


            ' Define the part of the waveform we will be rendering
            Dim piTotalBins As Integer = Math.Max(3, pdPowerValues.Count * (1 - fdZoomFactor))
            Dim piCenterBin As Integer = pdPowerValues.Count \ 2 ' Middle of FFT
            Dim piStartBin As Integer = Math.Max(0, piCenterBin - (piTotalBins \ 2))
            Dim piEndBin As Integer = Math.Min(pdPowerValues.Count - 1, piCenterBin + (piTotalBins \ 2))
            Dim pdBinsPerPixel As Double = piTotalBins / poGraphRect.Width
            Dim pdFreqResolution As Double = foSDR.SampleRate / pdPowerValues.Count

            ' Build point array for the waveform
            Dim poGraphPoints(poGraphRect.Width - 1) As Point
            Dim poGradientPoints(poGraphRect.Width - 1) As Point
            Dim piX As Integer = poGraphRect.Left

            For piPixelIndex As Integer = 0 To poGraphRect.Width - 1
                ' Determine which FFT bins map to this pixel
                Dim pdBinStart As Double = piStartBin + (piPixelIndex * pdBinsPerPixel)
                Dim pdBinEnd As Double = piStartBin + ((piPixelIndex + 1) * pdBinsPerPixel)

                ' Convert to integer bin indices
                Dim piBinStart As Integer = Math.Max(0, Math.Floor(pdBinStart))
                Dim piBinEnd As Integer = Math.Min(pdPowerValues.Count - 1, Math.Ceiling(pdBinEnd))

                ' Compute average power level for this pixel
                Dim pdAvgPower As Double = 0
                Dim piCount As Integer = 0
                For piBin As Integer = piBinStart To piBinEnd
                    If Double.IsNegativeInfinity(pdPowerValues(piBin)) = False Then
                        pdAvgPower += pdPowerValues(piBin)
                        piCount += 1
                    End If
                Next
                If piCount > 0 Then
                    pdAvgPower /= piCount
                Else
                    pdAvgPower = fddBOffset - fddBRange ' Assign minimum dB value to prevent overflow
                End If

                ' Convert dB power to Y coordinate
                'Dim piY As Integer = poGraphRect.Bottom - CInt((pdAvgPower - fddBOffset) * (poGraphRect.Height / fddBRange))
                Dim piY As Integer = poGraphRect.Top + CInt((fddBOffset - pdAvgPower) * (poGraphRect.Height / fddBRange))


                ' Store the waveform point
                poGraphPoints(piPixelIndex) = New Point(piX, piY)

                ' Store the gradient bottom point
                poGradientPoints(piPixelIndex) = New Point(piX, poGraphRect.Bottom)

                ' Move X position
                piX += 1
            Next

            ' start rendering graph
            ' Draw axes
            poGraphics.DrawLine(poAxisPen, poGraphRect.Left, poGraphRect.Bottom, poGraphRect.Right, poGraphRect.Bottom) ' X-axis
            poGraphics.DrawLine(poAxisPen, poGraphRect.Left, poGraphRect.Top, poGraphRect.Left, poGraphRect.Bottom) ' Left Y-axis
            poGraphics.DrawLine(poAxisPen, poGraphRect.Right, poGraphRect.Top, poGraphRect.Right, poGraphRect.Bottom) ' Right Y-axis

            ' Y-axis labels 
            Dim piTickCountY As Integer = CInt(fddBRange \ 10) ' One tick every 10 dB
            For piTickIndex As Integer = 0 To piTickCountY
                ' dB value at this tick
                Dim pdDbValue As Double = fddBOffset - (piTickIndex * 10)

                ' Convert dB to Y-coordinate
                Dim piYPos As Integer = poGraphRect.Top + CInt((fddBOffset - pdDbValue) * (poGraphRect.Height / fddBRange))

                ' Draw grid line
                poGraphics.DrawLine(poGridPen, poGraphRect.Left, piYPos, poGraphRect.Right, piYPos)

                ' Draw dB labels (left & right)
                Dim psLabel As String = pdDbValue.ToString("0") & " dB"
                poGraphics.DrawString(psLabel, poLabelFont, poLabelBrush, New RectangleF(poGraphRect.Left - piDbLabelWidth - 7, piYPos - 10, piDbLabelWidth, 20), poCenterFormat)
                poGraphics.DrawString(psLabel, poLabelFont, poLabelBrush, New RectangleF(poGraphRect.Right + 5, piYPos - 10, piDbLabelWidth, 20), poCenterFormat)
            Next
            ' x-axis labels, determine tick spacing based on available width and avoid overlap
            Dim piTickSpacing As Integer = Math.Max(1, poGraphPoints.Length \ 8) ' Aim for ~8 labels
            For piIndex As Integer = 0 To poGraphPoints.Length - 1 Step piTickSpacing
                ' Get the bin number corresponding to this tick mark
                Dim piBin As Integer = piStartBin + CInt(piIndex * pdBinsPerPixel)

                ' Compute frequency for this bin
                Dim pdTickFreq As Double = foSDR.CenterFrequency + ((piBin - piCenterBin) * pdFreqResolution)

                ' Get the X position from our point array
                Dim piXPos As Integer = poGraphPoints(piIndex).X

                ' Draw grid line
                poGraphics.DrawLine(poGridPen, piXPos, poGraphRect.Top, piXPos, poGraphRect.Bottom)

                ' Draw frequency label
                Dim psLabel As String = modMain.FormatHertz(pdTickFreq) '(pdTickFreq / 1000000000.0).ToString("0.000000") & " GHz"
                poGraphics.DrawString(psLabel, poLabelFont, poLabelBrush, New RectangleF(piXPos - (piFreqLabelWidth / 2), poGraphRect.Bottom + 7, piFreqLabelWidth, 20), poCenterFormat)
            Next

            ' draw red center frequency line
            Dim piCenterLine As Integer = poGraphPoints(poGradientPoints.Count \ 2).X
            poGraphics.DrawLine(poCenterFreqPen, piCenterLine, poGraphRect.Top, piCenterLine, poGraphRect.Bottom)

            ' Draw signal waveform now, start with gradient under waveform
            ' Combine waveform and bottom points to form the gradient fill area
            Dim poGradientPath As New Drawing2D.GraphicsPath()
            poGradientPath.AddLines(poGraphPoints)
            poGradientPath.AddLines(poGradientPoints.Reverse().ToArray()) ' Close the path
            ' Define gradient fill (fades from blue to black)
            Using poGradientBrush As New Drawing2D.LinearGradientBrush(poGraphRect, Color.Blue, Color.Transparent, Drawing2D.LinearGradientMode.Vertical)
                ' Fill the area under the waveform
                poGraphics.FillPath(poGradientBrush, poGradientPath)
            End Using
            ' Draw actual waveform
            Dim poSmoothPath As New Drawing2D.GraphicsPath()
            poSmoothPath.AddCurve(poGraphPoints, 0.75F) ' Smooth tension factor

            ' Draw the smoothed waveform
            Using poWavePen As New Pen(Color.White, 1)
                poGraphics.DrawPath(poWavePen, poSmoothPath)
            End Using
        End Using

        Return poBitmap
    End Function

    Function GaussianSmooth(ByVal pdData() As Double, ByVal piKernelSize As Integer) As Double()
        Dim piHalfSize As Integer = piKernelSize \ 2
        Dim pdKernel(piKernelSize - 1) As Double
        Dim pdSmoothed(pdData.Length - 1) As Double

        ' Create Gaussian kernel weights
        Dim pdSigma As Double = piKernelSize / 2.0
        Dim pdSum As Double = 0
        For i As Integer = -piHalfSize To piHalfSize
            pdKernel(i + piHalfSize) = Math.Exp(-(i * i) / (2 * pdSigma * pdSigma))
            pdSum += pdKernel(i + piHalfSize)
        Next

        ' Normalize kernel
        For i As Integer = 0 To piKernelSize - 1
            pdKernel(i) /= pdSum
        Next

        ' Apply Gaussian blur
        For i As Integer = piHalfSize To pdData.Length - piHalfSize - 1
            Dim pdWeightedSum As Double = 0
            For j As Integer = -piHalfSize To piHalfSize
                pdWeightedSum += pdData(i + j) * pdKernel(j + piHalfSize)
            Next
            pdSmoothed(i) = pdWeightedSum
        Next

        ' Copy edges
        For i As Integer = 0 To piHalfSize - 1
            pdSmoothed(i) = pdData(i)
            pdSmoothed(pdData.Length - 1 - i) = pdData(pdData.Length - 1 - i)
        Next

        Return pdSmoothed
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

End Class
''
