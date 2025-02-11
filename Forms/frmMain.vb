Imports System.Runtime.InteropServices
Imports MathNet.Numerics
Imports MathNet.Numerics.IntegralTransforms
Imports System.Drawing
Imports System.Numerics
Imports System.Drawing.Imaging


Public Class frmMain
    Private WithEvents foSDR As RtlSdrApi = Nothing
    Private foSignalBMP As Bitmap = Nothing
    Private foWaterfallBMP As Bitmap = Nothing
    Private foBitmapsLock As New Object()

    Private pZoomFactor As Double = 1.0 ' Default 100% view (1.0 = full spectrum)
    Private pDbOffset As Double = -10 ' Adjusts the dB scaling visually
    Private pDbRange As Double = 100 ' 100dB range default (10db to 150db)
    Private pContrast As Double = 1.0 ' Adjusts signal intensity scaling



    Private Sub btnStartStop_Click(sender As Object, e As EventArgs) Handles btnStartStop.Click
        If cboDeviceList.SelectedItem IsNot Nothing Then
            Dim poItem As RtlSdrApi.SdrDevice = cboDeviceList.SelectedItem
            If poItem.DeviceIndex < 0 Then
                MsgBox("Please select a device from the dropdown.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
                cboDeviceList.Focus()
            Else
                If foSDR Is Nothing Then
                    foSDR = New RtlSdrApi(CLng(1.6 * 10 ^ 9), poItem.DeviceIndex) ' 1.6 GHz = 1,600,000,000 Hz 
                    pContrast = trkContrast.Value / 100
                    pDbOffset = trkOffset.Value
                    pDbRange = trkRange.Value
                    pZoomFactor = trkZoomFactor.Value / 100.0 ' Convert range 10-100 → 0.1-1.0
                End If

                If foSDR.IsRunning Then
                    foSDR.StopMonitor()
                    btnStartStop.Text = "START"
                Else
                    btnStartStop.Text = "STOP"
                    foSDR.StartMonitor()
                    Dim worker As New System.Threading.Thread(AddressOf Worker_GenerateBitmap)
                    worker.IsBackground = True
                    worker.Start()
                End If
                panSignal.Invalidate()
            End If
        Else
            MsgBox("Please select a device from the dropdown.", MsgBoxStyle.OkOnly, "No SDR Device Selected")
            cboDeviceList.Focus()
        End If




    End Sub


    Private Sub foSDR_ErrorOccurred(sender As Object, message As String) Handles foSDR.ErrorOccurred
        Debug.WriteLine("######>>>> SDRDLL ERROR: " & message)
    End Sub

    Private Sub foSDR_SignalChange(sender As Object, SignalFound As Boolean) Handles foSDR.SignalChange
        Debug.WriteLine("##  Signal Found Change: " & SignalFound)
    End Sub




    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True

        Dim fi As System.Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        If fi IsNot Nothing Then fi.SetValue(panSignal, True, Nothing)
        If fi IsNot Nothing Then fi.SetValue(panWaterfall, True, Nothing)

        cboDeviceList.Items.Clear()
        Dim poDevs As List(Of RtlSdrApi.SdrDevice) = RtlSdrApi.GetDevices()
        If poDevs.Count = 0 Then
            cboDeviceList.Items.Add(New RtlSdrApi.SdrDevice("No SDR devices found.", -1))
        Else
            cboDeviceList.Items.AddRange(poDevs.Cast(Of Object).ToArray())
        End If
        cboDeviceList.SelectedIndex = 0
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

    Private Sub panWaterfall_Paint(sender As Object, e As PaintEventArgs) Handles panWaterfall.Paint
        Dim g As Graphics = e.Graphics

        If foSDR Is Nothing OrElse Not foSDR.IsRunning Then
            g.Clear(Color.Black) ' Clear panel background
        Else
            ' Draw the latest signal bitmap if available
            If foWaterfallBMP IsNot Nothing Then
                SyncLock foBitmapsLock
                    g.DrawImage(foWaterfallBMP, 0, 0, panWaterfall.Width, panWaterfall.Height)
                End SyncLock
            End If
        End If
    End Sub

    Private Sub panWaterfall_SizeChanged(sender As Object, e As EventArgs) Handles panWaterfall.SizeChanged
        SyncLock foBitmapsLock
            If foWaterfallBMP IsNot Nothing Then
                foWaterfallBMP.Dispose()
                foWaterfallBMP = Nothing
            End If
        End SyncLock
    End Sub

    Private Sub trkContrast_Scroll(sender As Object, e As EventArgs) Handles trkContrast.Scroll
        pContrast = trkContrast.Value / 100.0
        UpdateSpectrum()
    End Sub

    Private Sub trkOffset_Scroll(sender As Object, e As EventArgs) Handles trkOffset.Scroll
        pDbOffset = trkOffset.Value
        UpdateSpectrum()
    End Sub

    Private Sub trkRange_Scroll(sender As Object, e As EventArgs) Handles trkRange.Scroll
        pDbRange = trkRange.Value
        UpdateSpectrum()
    End Sub

    Private Sub trkZoomFactor_Scroll(sender As Object, e As EventArgs) Handles trkZoomFactor.Scroll
        ' convert to zoom factor
        pZoomFactor = trkZoomFactor.Value / 100.0 ' Convert range 10-100 → 0.1-1.0
        lblZoom.Text = $"{trkZoomFactor.Value}%"
        UpdateSpectrum()
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
                Debug.WriteLine("NO BUFFER DATA")
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

        SyncLock foBitmapsLock
            If foSignalBMP IsNot Nothing Then
                If panSignal IsNot Nothing AndAlso panSignal.Handle <> IntPtr.Zero Then
                    Using g As Graphics = panSignal.CreateGraphics()
                        SyncLock foSignalBMP
                            g.DrawImageUnscaled(foSignalBMP, 0, 0) ' Direct draw, no Paint flickering
                        End SyncLock
                    End Using
                End If
            End If
            If foWaterfallBMP IsNot Nothing Then
                If panWaterfall IsNot Nothing AndAlso panWaterfall.Handle <> IntPtr.Zero Then
                    Using g As Graphics = panWaterfall.CreateGraphics()
                        SyncLock foWaterfallBMP
                            g.DrawImageUnscaled(foWaterfallBMP, 0, 0) ' Direct draw, no Paint flickering
                        End SyncLock
                    End Using
                End If
            End If
        End SyncLock
    End Sub

    Private Function GenerateSignalBitmap(buffer As Byte(), width As Integer, height As Integer) As Bitmap
        Dim bmp As New Bitmap(width, height)
        Dim centerFreq As Double = 1600000000.0 ' 1.6 GHz
        Dim sampleRate As Double = 2048000.0 ' 2.048 MHz
        Dim fftSize As Integer = buffer.Length \ 2
        Dim binWidth As Double = sampleRate / fftSize ' Base bin width

        ' Determine how many FFT bins to display based on zoom factor
        Dim displayBins As Integer = Math.Max(1, CInt((fftSize \ 2) * pZoomFactor)) 'pZoomFactor is 0.1 to 1.0
        Dim zoomedBinWidth As Double = sampleRate / displayBins ' Adjust bin width dynamically

        ' Define range parameters
        Dim adjustedMaxDb As Double = pDbOffset ' This will be from 0 to -150
        Dim adjustedMinDb As Double = adjustedMaxDb - pDbRange ' pDbRange controls the spread from 10dB to 150dB

        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.Black)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Dim font As New Font("Segoe UI", 10, FontStyle.Bold)
            Dim textBrush As Brush = Brushes.White
            Dim gridPen As New Pen(Color.Gray, 1) With {.DashStyle = Drawing2D.DashStyle.Dash}
            Dim centerPen As New Pen(Color.Red, 2)
            Dim waveformPen As New Pen(Color.White, 2)

            ' Graph bounds
            Dim graphRect As New Rectangle(55, 10, width - 110, height - 45)

            ' Adjust Y-axis ticks dynamically based on pDbRange
            Dim tickSpacing As Double = Math.Max(10, pDbRange / 10)

            ' Draw Y-axis Grid and Labels
            Dim rightAlign As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
            Dim leftAlign As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
            For dBValue As Double = adjustedMinDb To adjustedMaxDb Step tickSpacing
                Dim y As Integer = graphRect.Bottom - CInt((dBValue - adjustedMinDb) / (adjustedMaxDb - adjustedMinDb) * graphRect.Height)
                Dim dBText As String = CInt(dBValue).ToString() & " dB"
                ' Draw left-hand dB label (right-aligned)
                g.DrawString(dBText, font, textBrush, New PointF(graphRect.Left - 5, y), rightAlign)
                ' draw right-hand side
                g.DrawString(dBText, font, textBrush, New PointF(graphRect.Right + 5, y), leftAlign)
                ' draw grid line
                g.DrawLine(gridPen, graphRect.Left, y, graphRect.Right, y)
            Next

            ' Draw X-axis Grid and Labels (Frequencies, ensuring proper spacing)
            ' Determine the new frequency range after zooming
            Dim displayFreqSpan As Double = sampleRate * pZoomFactor ' Actual frequency span displayed
            Dim minFreq As Double = centerFreq - (displayFreqSpan / 2)
            Dim maxFreq As Double = centerFreq + (displayFreqSpan / 2)
            ' Define how many frequency labels to display dynamically (adjustable)
            Dim numLabels As Integer = 8 ' Keep a reasonable number of labels
            Dim freqStep As Double = displayFreqSpan / numLabels ' Frequency step between labels
            Dim centerAlign As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Near}
            ' Draw X-axis Grid and Labels (Frequencies, dynamically spaced)
            For i As Integer = 0 To numLabels
                ' Compute the X-position for this label
                Dim x As Integer = graphRect.Left + CInt((i / numLabels) * graphRect.Width)
                ' Compute the corresponding frequency for this position
                Dim freq As Double = minFreq + (i * freqStep)
                ' Draw vertical grid line
                g.DrawLine(gridPen, x, graphRect.Top, x, graphRect.Bottom)
                ' Format and draw frequency label centered
                'g.DrawString((freq / 1000000000.0).ToString("0.000000") & " GHz", font, textBrush, x - 20, graphRect.Bottom + 10)
                ' Draw centered frequency label
                g.DrawString((freq / 1000000000.0).ToString("0.000000") & " GHz", font, textBrush, New PointF(x, graphRect.Bottom + 15), centerAlign)

            Next


            ' Draw Center Frequency Marker
            Dim centerX As Integer = graphRect.Left + CInt(graphRect.Width / 2)
            g.DrawLine(centerPen, centerX, graphRect.Top, centerX, graphRect.Bottom)


            ' Convert raw IQ data to complex values
            Dim complexData(fftSize - 1) As Complex
            For i As Integer = 0 To buffer.Length - 2 Step 2
                Dim pdInPhase As Double = (buffer(i) - 127.5) / 127.5
                Dim pdQuad As Double = (buffer(i + 1) - 127.5) / 127.5
                complexData(i \ 2) = New Complex(pdInPhase, pdQuad)
            Next

            ' Perform FFT
            Fourier.Forward(complexData, FourierOptions.NoScaling)

            ' Convert FFT output to dB power levels
            Dim powerLevels(displayBins - 1) As Double
            Dim binRatio As Double = (fftSize \ 2) / displayBins

            ' Apply Zoom by Averaging FFT Bins
            For i As Integer = 0 To displayBins - 1
                Dim sum As Double = 0
                Dim count As Integer = 0
                For j As Integer = 0 To Math.Max(1, CInt(binRatio)) - 1
                    Dim idx As Integer = (i * CInt(binRatio)) + j
                    If idx < fftSize \ 2 Then
                        'sum += 20 * Math.Log10(Math.Max(complexData(idx).Magnitude, 0.0000000001))
                        sum += 10 * Math.Log10(Math.Max(complexData(idx).Magnitude ^ 2, 0.0000000001))
                        count += 1
                    End If
                Next
                If count > 0 Then powerLevels(i) = sum / count
                'powerLevels(i) = Math.Max(adjustedMinDb, Math.Min(adjustedMaxDb, powerLevels(i))) ' Apply dB range limits
                powerLevels(i) = Math.Max(adjustedMinDb, Math.Min(adjustedMaxDb, powerLevels(i) - 70)) ' Apply dB range limits and normalize the noise floor 
            Next

            ' Draw Signal Graph with Gradient Fill
            Dim path As New Drawing2D.GraphicsPath()
            Dim points(displayBins - 1) As PointF
            Dim baseY As Integer = graphRect.Bottom

            For i As Integer = 0 To displayBins - 1
                Dim x As Integer = graphRect.Left + CInt((i / (displayBins - 1.0)) * graphRect.Width)
                Dim y As Integer = graphRect.Bottom - CInt((powerLevels(i) - adjustedMinDb) / (adjustedMaxDb - adjustedMinDb) * graphRect.Height)
                points(i) = New PointF(x, y)
            Next
            Dim gradientBounds As New Rectangle(graphRect.Left, CInt(points.Min(Function(p) p.Y)), Math.Max(1, graphRect.Width), Math.Max(1, graphRect.Bottom - CInt(points.Min(Function(p) p.Y))))
            Dim gradientBrush As New Drawing2D.LinearGradientBrush(gradientBounds, Color.Blue, Color.Transparent, Drawing2D.LinearGradientMode.Vertical)



            ' Fill with Gradient Below the Curve
            path.AddLines(points)
            path.AddLine(points.Last(), New PointF(points.Last().X, baseY))
            path.AddLine(New PointF(points(0).X, baseY), points(0))
            g.FillPath(gradientBrush, path)

            ' Draw Waveform Line
            If points.Count > 1 Then
                g.DrawLines(waveformPen, points)
            End If

            ' update waterfall image
            Call GenerateWaterfallBitmap(powerLevels)
        End Using


        Return bmp
    End Function


    Private Sub GenerateWaterfallBitmap(pdPowerLevels As Double())
        ' Ensure bitmap is initialized
        If foWaterfallBMP Is Nothing Then
            Dim poSize As Size = GetWaterfallPanelSize()
            foWaterfallBMP = New Bitmap(poSize.Width, poSize.Height)
        End If

        SyncLock foBitmapsLock
            ' Lock bitmap for pixel manipulation
            Dim rect As New Rectangle(0, 0, foWaterfallBMP.Width, foWaterfallBMP.Height)
            Dim bmpData As BitmapData = foWaterfallBMP.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)
            Dim stride As Integer = bmpData.Stride
            Dim pixels As IntPtr = bmpData.Scan0
            Dim byteArray As Byte() = New Byte(Math.Abs(stride) * foWaterfallBMP.Height - 1) {}

            ' Copy bitmap to array
            Marshal.Copy(pixels, byteArray, 0, byteArray.Length)

            ' Shift all existing rows down by one row
            Buffer.BlockCopy(byteArray, 0, byteArray, stride, byteArray.Length - stride)

            ' Insert new FFT row at the top
            For i As Integer = 0 To pdPowerLevels.Length - 1
                Dim power As Double = pdPowerLevels(i)
                Dim color As Color = GetWaterfallColor(power)

                Dim offset As Integer = i * 3 ' First row (topmost)
                byteArray(offset) = color.B
                byteArray(offset + 1) = color.G
                byteArray(offset + 2) = color.R
            Next
            ' Copy modified array back to bitmap
            Marshal.Copy(byteArray, 0, pixels, byteArray.Length)
            foWaterfallBMP.UnlockBits(bmpData)
        End SyncLock
    End Sub

    Private Function GetWaterfallColor(pdPower As Double) As Color
        Dim minPower As Double = -140 ' dBFS range (adjust as needed)
        Dim maxPower As Double = -40

        ' Normalize power and apply contrast scaling
        Dim norm As Double = ((pdPower - minPower) / (maxPower - minPower)) ^ pContrast
        norm = Math.Max(0, Math.Min(1, norm)) ' Clamp between 0 and 1

        ' Map to color gradient
        If norm < 0.3 Then
            Return Color.FromArgb(0, 0, CInt(255 * (norm / 0.3))) ' Blue to Cyan
        ElseIf norm < 0.7 Then
            Return Color.FromArgb(CInt(255 * ((norm - 0.3) / 0.4)), 255, 255) ' Cyan to Yellow
        Else
            Return Color.FromArgb(255, CInt(255 * ((1 - norm) / 0.3)), 0) ' Yellow to Red
        End If
    End Function




    '' version 2
    'Private Function GenerateSignalBitmap(buffer As Byte(), width As Integer, height As Integer) As Bitmap
    '    Dim bmp As New Bitmap(width, height)
    '    Dim centerFreq As Double = 1600000000.0 ' 1.6 GHz
    '    Dim sampleRate As Double = 2048000.0 ' 2.048 MHz
    '    Dim fftSize As Integer = buffer.Length \ 2
    '    Dim binWidth As Double = sampleRate / fftSize ' Base bin width

    '    ' Determine how many FFT bins to display based on zoom factor
    '    Dim displayBins As Integer = Math.Max(1, CInt((fftSize \ 2) * pZoomFactor))
    '    Dim zoomedBinWidth As Double = sampleRate / displayBins ' Adjust bin width dynamically

    '    Using g As Graphics = Graphics.FromImage(bmp)
    '        g.Clear(Color.Black)
    '        Dim font As New Font("Arial", 10, FontStyle.Bold)
    '        Dim textBrush As Brush = Brushes.White
    '        Dim gridPen As New Pen(Color.Gray, 1) With {.DashStyle = Drawing2D.DashStyle.Dash}
    '        Dim centerPen As New Pen(Color.Red, 2)

    '        ' Graph bounds
    '        Dim graphRect As New Rectangle(50, 10, width - 60, height - 40)

    '        ' FFT Processing
    '        Dim complexData(fftSize - 1) As Complex

    '        ' Convert raw IQ data to complex values
    '        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
    '            Dim pdInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
    '            Dim pdQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
    '            complexData(piIndex \ 2) = New Complex(pdInPhase, pdQuad)
    '        Next

    '        ' Perform FFT
    '        Fourier.Forward(complexData, FourierOptions.NoScaling)

    '        ' Convert FFT output to dB power levels
    '        Dim powerLevels(displayBins - 1) As Double

    '        ' Apply Zoom by Averaging FFT Bins
    '        Dim binRatio As Double = (fftSize \ 2) / displayBins ' Determines how many bins to merge
    '        For i As Integer = 0 To displayBins - 1
    '            Dim sum As Double = 0
    '            Dim count As Integer = 0
    '            For j As Integer = 0 To Math.Max(1, CInt(binRatio)) - 1
    '                Dim idx As Integer = (i * CInt(binRatio)) + j
    '                If idx < fftSize \ 2 Then
    '                    sum += 20 * Math.Log10(Math.Max(complexData(idx).Magnitude, 0.0000000001))
    '                    count += 1
    '                End If
    '            Next
    '            If count > 0 Then powerLevels(i) = sum / count ' Average adjacent bins
    '            powerLevels(i) = Math.Max(pMinDb, Math.Min(pMaxDb, powerLevels(i))) ' Apply dB range limits
    '        Next

    '        ' Adjust dB Scale Based on Offset & Contrast
    '        For i As Integer = 0 To powerLevels.Length - 1
    '            powerLevels(i) = (powerLevels(i) + pDbOffset) * pContrast
    '            powerLevels(i) = Math.Max(pMinDb, Math.Min(pMaxDb, powerLevels(i))) ' Keep values in range
    '        Next

    '        ' Dynamically Adjust Grid Line Count Based on Zoom
    '        Dim baseGridLines As Integer = 8 ' Default at 100% zoom
    '        Dim numGridLines As Integer = Math.Max(7, Math.Min(11, CInt(baseGridLines * (1.0 / pZoomFactor))))

    '        ' Frequency Grid
    '        Dim freqStep As Double = (zoomedBinWidth * displayBins) / (numGridLines - 1)
    '        For i As Integer = 0 To numGridLines - 1
    '            Dim freqLabel As String = $"{(centerFreq - (zoomedBinWidth * displayBins) / 2 + i * freqStep) / 1000000000.0:F3} GHz"
    '            Dim x As Integer = graphRect.Left + CInt(i * graphRect.Width / numGridLines)
    '            g.DrawLine(gridPen, x, graphRect.Top, x, graphRect.Bottom)
    '            g.DrawString(freqLabel, font, textBrush, x - 15, graphRect.Bottom + 5)
    '        Next

    '        ' dB Grid
    '        Dim dbStep As Double = (pMaxDb - pMinDb) / (numGridLines - 1)
    '        For i As Integer = 0 To numGridLines - 1
    '            Dim dbLabel As String = $"{CInt(pMaxDb - i * dbStep)} dB"
    '            Dim y As Integer = graphRect.Bottom - CInt(i * graphRect.Height / numGridLines)
    '            g.DrawLine(gridPen, graphRect.Left, y, graphRect.Right, y)
    '            g.DrawString(dbLabel, font, textBrush, graphRect.Left - 45, y - 10)
    '        Next

    '        ' Plot Signal Data
    '        Using signalPen As New Pen(Color.Lime, 2)
    '            For i As Integer = 1 To powerLevels.Length - 1
    '                Dim x1 As Integer = graphRect.Left + CInt((i - 1) * graphRect.Width / powerLevels.Length)
    '                Dim y1 As Integer = graphRect.Bottom - CInt((powerLevels(i - 1) - pMaxDb) * graphRect.Height / (pMinDb - pMaxDb))
    '                Dim x2 As Integer = graphRect.Left + CInt(i * graphRect.Width / powerLevels.Length)
    '                Dim y2 As Integer = graphRect.Bottom - CInt((powerLevels(i) - pMaxDb) * graphRect.Height / (pMinDb - pMaxDb))

    '                g.DrawLine(signalPen, x1, y1, x2, y2)
    '            Next
    '        End Using

    '        ' Draw Center Frequency Marker
    '        Dim centerX As Integer = graphRect.Left + CInt(graphRect.Width / 2)
    '        g.DrawLine(centerPen, centerX, graphRect.Top, centerX, graphRect.Bottom)

    '        ' Indicate recording status
    '        If foSDR.IsRecording Then
    '            Using poRecFont As New Font("Segoe UI", 10, FontStyle.Bold)
    '                g.DrawString("● RECORDING", poRecFont, Brushes.Red, 10, 10)
    '            End Using
    '        End If

    '    End Using

    '    Return bmp
    'End Function







    'Private Function GenerateSignalBitmap(buffer As Byte(), width As Integer, height As Integer) As Bitmap
    '    Dim bmp As New Bitmap(width, height)
    '    Dim centerFreq As Double = 1600000000.0 ' 1.6 GHz
    '    Dim sampleRate As Double = 2048000.0 ' 2.048 MHz
    '    Dim fftSize As Integer = buffer.Length \ 2
    '    Dim binWidth As Double = sampleRate / fftSize ' Base bin width

    '    ' Determine how many FFT bins to display based on zoom factor
    '    Dim displayBins As Integer = Math.Max(1, CInt((fftSize \ 2) * pZoomFactor))
    '    Dim zoomedBinWidth As Double = sampleRate / displayBins ' New bin width

    '    Using g As Graphics = Graphics.FromImage(bmp)
    '        g.Clear(Color.Black)
    '        Dim font As New Font("Arial", 10, FontStyle.Bold)
    '        Dim textBrush As Brush = Brushes.White
    '        Dim gridPen As New Pen(Color.Gray, 1) With {.DashStyle = Drawing2D.DashStyle.Dash}
    '        Dim centerPen As New Pen(Color.Red, 2)

    '        ' Graph bounds
    '        Dim graphRect As New Rectangle(50, 10, width - 60, height - 40)

    '        ' FFT Processing
    '        Dim complexData(fftSize - 1) As Complex

    '        ' Convert raw IQ data to complex values
    '        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
    '            Dim pdInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
    '            Dim pdQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
    '            complexData(piIndex \ 2) = New Complex(pdInPhase, pdQuad)
    '        Next

    '        ' Perform FFT
    '        Fourier.Forward(complexData, FourierOptions.NoScaling)

    '        ' Convert FFT output to dB power levels
    '        Dim powerLevels(displayBins - 1) As Double
    '        Dim minDb As Double = -80
    '        Dim maxDb As Double = 10

    '        ' **Apply Zoom by Averaging FFT Bins**
    '        Dim binRatio As Double = (fftSize \ 2) / displayBins ' Determines how many bins to merge
    '        For i As Integer = 0 To displayBins - 1
    '            Dim sum As Double = 0
    '            Dim count As Integer = 0
    '            For j As Integer = 0 To Math.Max(1, CInt(binRatio)) - 1 ' Ensure at least 1 bin
    '                Dim idx As Integer = (i * CInt(binRatio)) + j
    '                If idx < fftSize \ 2 Then
    '                    sum += 20 * Math.Log10(Math.Max(complexData(idx).Magnitude, 0.0000000001))
    '                    count += 1
    '                End If
    '            Next
    '            If count > 0 Then powerLevels(i) = sum / count ' Average adjacent bins
    '            powerLevels(i) = Math.Max(minDb, Math.Min(maxDb, powerLevels(i))) ' Clamp
    '        Next

    '        ' Draw Freq Grid
    '        Dim numGridLines As Integer = 10
    '        Dim freqStep As Double = (zoomedBinWidth * displayBins) / (numGridLines - 1)
    '        Dim dbStep As Double = (maxDb - minDb) / (numGridLines - 1)


    '        For i As Integer = 0 To numGridLines - 1
    '            ' Vertical Grid (Frequency)
    '            Dim freqLabel As String = $"{(centerFreq - (zoomedBinWidth * displayBins) / 2 + i * freqStep) / 1000000000.0:F3} GHz"
    '            Dim x As Integer = graphRect.Left + CInt(i * graphRect.Width / numGridLines)
    '            g.DrawLine(gridPen, x, graphRect.Top, x, graphRect.Bottom)
    '            g.DrawString(freqLabel, font, textBrush, x - 15, graphRect.Bottom + 5)

    '            ' Horizontal Grid (dB Scale)
    '            Dim dbLabel As String = $"{CInt(maxDb - i * dbStep)} dB"
    '            Dim y As Integer = graphRect.Bottom - CInt(i * graphRect.Height / numGridLines)
    '            g.DrawLine(gridPen, graphRect.Left, y, graphRect.Right, y)
    '            g.DrawString(dbLabel, font, textBrush, graphRect.Left - 45, y - 10)
    '        Next




    '        ' Plot Signal Data
    '        Using signalPen As New Pen(Color.Lime, 2)
    '            For i As Integer = 1 To powerLevels.Length - 1
    '                Dim x1 As Integer = graphRect.Left + CInt((i - 1) * graphRect.Width / powerLevels.Length)
    '                Dim y1 As Integer = graphRect.Bottom - CInt((powerLevels(i - 1) - maxDb) * graphRect.Height / (minDb - maxDb))
    '                Dim x2 As Integer = graphRect.Left + CInt(i * graphRect.Width / powerLevels.Length)
    '                Dim y2 As Integer = graphRect.Bottom - CInt((powerLevels(i) - maxDb) * graphRect.Height / (minDb - maxDb))

    '                g.DrawLine(signalPen, x1, y1, x2, y2)
    '            Next
    '        End Using

    '        ' Draw Center Frequency Marker
    '        Dim centerX As Integer = graphRect.Left + CInt(graphRect.Width / 2)
    '        g.DrawLine(centerPen, centerX, graphRect.Top, centerX, graphRect.Bottom)

    '        ' Indicate recording status
    '        If foSDR.IsRecording Then
    '            Using poRecFont As New Font("Segoe UI", 10, FontStyle.Bold)
    '                g.DrawString("● RECORDING", poRecFont, Brushes.Red, 10, 10)
    '            End Using
    '        End If

    '    End Using

    '    Return bmp
    'End Function





    Private Function GetSignalPanelSize() As Size
        If panSignal.InvokeRequired Then
            ' Invoke on the UI thread if called from a worker thread
            Return CType(panSignal.Invoke(Function() GetSignalPanelSize()), Size)
        Else
            ' Directly return panel size if already on UI thread
            Return New Size(panSignal.Width, panSignal.Height)
        End If
    End Function

    Private Function GetWaterfallPanelSize() As Size
        If panWaterfall.InvokeRequired Then
            ' Invoke on the UI thread if called from a worker thread
            Return CType(panWaterfall.Invoke(Function() GetWaterfallPanelSize()), Size)
        Else
            ' Directly return panel size if already on UI thread
            Return New Size(panWaterfall.Width, panWaterfall.Height)
        End If
    End Function























    ''
End Class
''
