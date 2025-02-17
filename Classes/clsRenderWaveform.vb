Imports System.Drawing
Imports System.Numerics
Imports System.Drawing.Imaging

Public Class clsRenderWaveform


    Implements IDisposable

    Private mSignalBitmap As Bitmap
    Private miSigWidth As Integer
    Private miSigHeight As Integer

    Private mWaterBitmap As Bitmap
    Private miWaterWidth As Integer
    Private miWaterHeight As Integer

    Private mdZoomFactor As Double = 0 ' no zoom, show full graph, to 1.0 (show double resolution).
    Private mddBOffset As Double = -20 ' Adjusts the dB scaling this value is added to the Y-axis start (0 to -100) 
    Private mddBRange As Double = 100   ' dB range to display on Y-axis (100dB range, 10dB minimum to 150dB maximum)
    Private miKernelSize As Integer = 7 ' guassian smoothing kernel size
    Private mfSmoothingTension As Single = 0.75F
    Private miContrast As Integer = 25  ' Contrast for waterfall bitmap; values 0 to 100

    Private miDebugToggle As Integer = 0

    Private moLabelFont As New Font("Arial", 10, FontStyle.Regular)
    Private moRecordFont As New Font("Arial", 12, FontStyle.Bold)
    Private moInfoFont As New Font("Segoe UI", 10, FontStyle.Regular)
    Private moLabelBrush As New SolidBrush(Color.White)
    Private moInfoBrush As New SolidBrush(Color.LightBlue)
    Private moRecordBrush As New SolidBrush(Color.Red)
    Private moAxisPen As New Pen(Color.White, 2)
    Private moGridPen As New Pen(Color.LightGray, 1) With {.DashStyle = Drawing2D.DashStyle.Dot}
    Private moCenterFreqPen As New Pen(Color.Red, 1)
    Private moWavePen As New Pen(Color.White, 1)
    Private moStrFmtCenter As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center, .FormatFlags = StringFormatFlags.NoWrap}
    Private moStrFmtRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center, .FormatFlags = StringFormatFlags.NoWrap}
    Private moStrFmtLeft As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .FormatFlags = StringFormatFlags.NoWrap}
    Private mbRenderWaterfall As Boolean = False


    ''' <summary>
    '''  1.0 = full zoom out, 0.10 = full zoom in
    ''' </summary>
    ''' <returns></returns>
    Public Property ZoomFactor As Double
        Get
            Return mdZoomFactor
        End Get
        Set(value As Double)
            mdZoomFactor = Math.Min(1D, Math.Max(0.1D, value))
        End Set
    End Property

    ''' <summary>
    ''' Adjusts the dB scaling this value is added to the Y-axis start (0 to -100) 
    ''' </summary>
    ''' <returns></returns>
    Public Property dBOffset As Double
        Get
            Return mddBOffset
        End Get
        Set(value As Double)
            mddBOffset = Math.Min(0, Math.Max(-100, value))
        End Set
    End Property

    ''' <summary>
    ''' The dB range to display on Y-axis (100dB range, 10dB minimum to 150dB maximum)
    ''' </summary>
    ''' <returns></returns>
    Public Property dBRange As Double
        Get
            Return mddBRange
        End Get
        Set(value As Double)
            mddBRange = Math.Min(150, Math.Max(10, value))
        End Set
    End Property

    Public Property WaterfallContrast As Integer
        Get
            Return miContrast
        End Get
        Set(value As Integer)
            miContrast = Math.Min(100, Math.Max(0, value))
        End Set
    End Property

    Public ReadOnly Property WaterfallBitmap As Bitmap
        Get
            SyncLock mWaterBitmap
                Return mWaterBitmap
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Kernel size to use for Gaussian smoothing function of the waveform.  Set to 0 to disable smoothing.
    ''' </summary>
    ''' <returns></returns>
    Public Property GussianKernelSize As Integer
        Get
            Return miKernelSize
        End Get
        Set(value As Integer)
            miKernelSize = value
        End Set
    End Property


    ''' <summary>
    ''' Smoothing factor to be used for the 
    ''' </summary>
    ''' <returns></returns>
    Public Property SmoothingTension As Single
        Get
            Return mfSmoothingTension
        End Get
        Set(value As Single)
            mfSmoothingTension = value
        End Set
    End Property

    Public Property LabelFont As Font
        Get
            Return moLabelFont
        End Get
        Set(ByVal value As Font)
            moLabelFont = value
        End Set
    End Property

    Public Property RecordFont As Font
        Get
            Return moRecordFont
        End Get
        Set(ByVal value As Font)
            moRecordFont = value
        End Set
    End Property

    Public Property LabelBrush As SolidBrush
        Get
            Return moLabelBrush
        End Get
        Set(ByVal value As SolidBrush)
            moLabelBrush = value
        End Set
    End Property

    Public Property RecordBrush As SolidBrush
        Get
            Return moRecordBrush
        End Get
        Set(ByVal value As SolidBrush)
            moRecordBrush = value
        End Set
    End Property

    Public Property AxisPen As Pen
        Get
            Return moAxisPen
        End Get
        Set(ByVal value As Pen)
            moAxisPen = value
        End Set
    End Property

    Public Property GridPen As Pen
        Get
            Return moGridPen
        End Get
        Set(ByVal value As Pen)
            moGridPen = value
        End Set
    End Property

    Public Property CenterFreqPen As Pen
        Get
            Return moCenterFreqPen
        End Get
        Set(ByVal value As Pen)
            moCenterFreqPen = value
        End Set
    End Property

    Public Property WavePen As Pen
        Get
            Return moWavePen
        End Get
        Set(ByVal value As Pen)
            moWavePen = value
        End Set
    End Property



    Public Sub New(ByVal iSignalBmpWidth As Integer, ByVal iSignalBmpHeight As Integer, Optional ByVal iWaterfallBmpWidth As Integer = 0, Optional iWaterfallBmpHeight As Integer = 0)
        miSigWidth = iSignalBmpWidth
        miSigHeight = iSignalBmpHeight
        mSignalBitmap = New Bitmap(miSigWidth, miSigHeight)
        ' optional waterfall bitmap rendering
        If iWaterfallBmpHeight > 0 And iWaterfallBmpWidth > 0 Then
            ResizeWaterfallBitmap(iWaterfallBmpWidth, iWaterfallBmpHeight)
        End If
    End Sub

    ''' <summary>
    ''' Renders the signal graph, resizing the bitmap if needed.  Pass Nothing for elapsed values (oRecordingElapsed, oElapsed) to supress their rendering.
    ''' </summary>
    ''' <returns>Updated Bitmap</returns>
    Public Function RenderGraph(ByVal iWidth As Integer, ByVal iHeight As Integer _
                                , ByVal ddBPowerValues() As Double, dSampleRate As Double, iCenterFrequency As UInteger _
                                , Optional ByVal bIsRecording As Boolean = False, Optional ByVal oRecordingElapsed As TimeSpan = Nothing _
                                , Optional ByVal sDeviceName As String = "", Optional ByVal oElapsed As TimeSpan = Nothing _
                                , Optional ByVal iWaterfallBmpWidth As Integer = 0, Optional ByVal iWaterfallBmpHeight As Integer = 0) As Bitmap

        ' Bail if invalid size (window may be minimized)
        If iWidth < 10 OrElse iHeight < 10 Then
            Return Nothing
        End If

        ' Resize bitmap only if dimensions changed
        If iWidth <> miSigWidth OrElse iHeight <> miSigHeight Then
            ResizeBitmap(iWidth, iHeight)
        End If

        If iWaterfallBmpHeight > 0 And iWaterfallBmpWidth > 0 Then
            If iWaterfallBmpHeight <> miWaterHeight OrElse iWaterfallBmpWidth <> miWaterWidth Then
                ResizeWaterfallBitmap(iWaterfallBmpWidth, iWaterfallBmpHeight)
            End If
        End If

        ' Apply gaussian smooth to the levels
        Dim pdSmoothedPowerValues() As Double
        If miKernelSize > 0 Then
            pdSmoothedPowerValues = GaussianSmooth(ddBPowerValues)
        Else
            pdSmoothedPowerValues = ddBPowerValues
        End If

        ' we now have dB power values for the full buffer now, draw the bitmap.
        Using poGraphics As Graphics = Graphics.FromImage(mSignalBitmap)
            ' how to render
            poGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            poGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            poGraphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            poGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            poGraphics.Clear(Color.Black)

            ' Define fonts and brushes
            ' Measure dB label width 
            Dim piDbLabelWidth As Integer = CInt(poGraphics.MeasureString("-888 dB", moLabelFont).Width)
            Dim piDbLabelHeight As Integer = CInt(poGraphics.MeasureString("-888 dB", moLabelFont).Height)
            Dim piWaveformAreaWidth As Integer = (piDbLabelWidth + 5) * 2 ' Both left & right
            ' Measure Frequency label size 
            Dim piFreqLabelHeight As Integer = CInt(poGraphics.MeasureString("8.888888 GHz", moLabelFont).Height) + 5 'give text some riser space
            Dim piFreqLabelWidth As Integer = CInt(poGraphics.MeasureString("8.888888 GHz", moLabelFont).Width)


            ' Define our workpace, working area with 5px margins left, right, top and 10px bottom.
            Dim poWorkingRect As New Rectangle(5, 5 + piDbLabelHeight, iWidth - 10, iHeight - 10 - piDbLabelHeight)
            ' Calculate graph area to allow for labels around it 
            Dim piGraphWidth As Integer = poWorkingRect.Width - piWaveformAreaWidth
            Dim piGraphHeight As Integer = poWorkingRect.Height - piFreqLabelHeight
            Dim poGraphRect As New Rectangle(poWorkingRect.X + (piWaveformAreaWidth \ 2), poWorkingRect.Y, piGraphWidth, piGraphHeight)

            ' Define the part of the waveform we will be rendering
            Dim piTotalBins As Integer = Math.Max(3, pdSmoothedPowerValues.Count * (1 - mdZoomFactor))
            Dim piCenterBin As Integer = pdSmoothedPowerValues.Count \ 2 ' Middle of FFT
            Dim piStartBin As Integer = Math.Max(0, piCenterBin - (piTotalBins \ 2))
            Dim piEndBin As Integer = Math.Min(pdSmoothedPowerValues.Count - 1, piCenterBin + (piTotalBins \ 2))
            Dim pdBinsPerPixel As Double = piTotalBins / poGraphRect.Width
            Dim pdFreqResolution As Double = dSampleRate / pdSmoothedPowerValues.Count

            ' Build point array for the waveform
            Dim poGraphPoints(poGraphRect.Width - 1) As Point
            Dim piX As Integer = poGraphRect.Left

            For piPixelIndex As Integer = 0 To poGraphRect.Width - 1
                ' Determine which FFT bins map to this pixel
                Dim pdBinStart As Double = piStartBin + (piPixelIndex * pdBinsPerPixel)
                Dim pdBinEnd As Double = piStartBin + ((piPixelIndex + 1) * pdBinsPerPixel)

                ' Convert to integer bin indices
                Dim piBinStart As Integer = Math.Max(0, Math.Floor(pdBinStart))
                Dim piBinEnd As Integer = Math.Min(pdSmoothedPowerValues.Count - 1, Math.Ceiling(pdBinEnd))

                ' Compute average power level for this pixel
                Dim pdAvgPower As Double = 0
                Dim piCount As Integer = 0
                For piBin As Integer = piBinStart To piBinEnd
                    If Double.IsNegativeInfinity(pdSmoothedPowerValues(piBin)) = False Then
                        pdAvgPower += pdSmoothedPowerValues(piBin)
                        piCount += 1
                    End If
                Next
                If piCount > 0 Then
                    pdAvgPower /= piCount
                Else
                    pdAvgPower = mddBOffset - mddBRange ' Assign minimum dB value to prevent overflow
                End If

                ' Convert dB power to Y coordinate
                Dim piY As Integer = poGraphRect.Top + CInt((mddBOffset - pdAvgPower) * (poGraphRect.Height / mddBRange))

                ' Store the waveform point
                poGraphPoints(piPixelIndex) = New Point(piX, piY)

                ' Move X position
                piX += 1
            Next

            ' start rendering graph
            ' Draw axes
            poGraphics.DrawLine(moAxisPen, poGraphRect.Left, poGraphRect.Bottom, poGraphRect.Right, poGraphRect.Bottom) ' X-axis
            poGraphics.DrawLine(moAxisPen, poGraphRect.Left, poGraphRect.Top, poGraphRect.Left, poGraphRect.Bottom) ' Left Y-axis
            poGraphics.DrawLine(moAxisPen, poGraphRect.Right, poGraphRect.Top, poGraphRect.Right, poGraphRect.Bottom) ' Right Y-axis

            ' Y-axis labels 
            Dim piTickCountY As Integer = CInt(mddBRange \ 10) ' One tick every 10 dB
            For piTickIndex As Integer = 0 To piTickCountY
                ' dB value at this tick
                Dim pdDbValue As Double = mddBOffset - (piTickIndex * 10)

                ' Convert dB to Y-coordinate
                Dim piYPos As Integer = poGraphRect.Top + CInt((mddBOffset - pdDbValue) * (poGraphRect.Height / mddBRange))

                ' Draw grid line
                poGraphics.DrawLine(moGridPen, poGraphRect.Left, piYPos, poGraphRect.Right, piYPos)

                ' Draw dB labels (left & right)
                Dim psLabel As String = pdDbValue.ToString("0") & " dB"
                poGraphics.DrawString(psLabel, moLabelFont, moLabelBrush, New RectangleF(poGraphRect.Left - piDbLabelWidth - 7, piYPos - 10, piDbLabelWidth, 20), moStrFmtCenter)
                poGraphics.DrawString(psLabel, moLabelFont, moLabelBrush, New RectangleF(poGraphRect.Right + 5, piYPos - 10, piDbLabelWidth, 20), moStrFmtCenter)
            Next
            ' x-axis labels, determine tick spacing based on available width and avoid overlap
            Dim piTickSpacing As Integer = Math.Max(1, poGraphPoints.Length \ 8) ' Aim for ~8 labels
            For piIndex As Integer = 0 To poGraphPoints.Length - 1 Step piTickSpacing
                ' Get the bin number corresponding to this tick mark
                Dim piBin As Integer = piStartBin + CInt(piIndex * pdBinsPerPixel)

                ' Compute frequency for this bin
                Dim pdTickFreq As Double = iCenterFrequency + ((piBin - piCenterBin) * pdFreqResolution)

                ' Get the X position from our point array
                Dim piXPos As Integer = poGraphPoints(piIndex).X

                ' Draw grid line
                poGraphics.DrawLine(moGridPen, piXPos, poGraphRect.Top, piXPos, poGraphRect.Bottom)

                ' Draw frequency label
                Dim psLabel As String = modMain.FormatHertz(pdTickFreq) '(pdTickFreq / 1000000000.0).ToString("0.000000") & " GHz"
                poGraphics.DrawString(psLabel, moLabelFont, moLabelBrush, New RectangleF(piXPos - (piFreqLabelWidth / 2), poGraphRect.Bottom + 7, piFreqLabelWidth, 20), moStrFmtCenter)
            Next

            ' draw red center frequency line
            Dim piCenterLine As Integer = poGraphPoints(poGraphPoints.Count \ 2).X
            poGraphics.DrawLine(moCenterFreqPen, piCenterLine, poGraphRect.Top, piCenterLine, poGraphRect.Bottom)

            ' Draw recording text 
            If bIsRecording AndAlso oRecordingElapsed <> Nothing Then
                Dim psRec As String = $"* RECORDING * {oRecordingElapsed.Minutes:D2}:{oRecordingElapsed.Seconds:D2}"
                Dim piYVal As Integer = CInt(poGraphics.MeasureString(psRec, moRecordFont).Height) - 10
                poGraphics.DrawString(psRec, moRecordFont, moRecordBrush, poGraphRect.Left, piYVal, moStrFmtLeft)
            End If

            ' draw device name and elased
            If oElapsed <> Nothing Then
                Dim psDev As String = ""
                If sDeviceName.Trim <> "" Then
                    psDev = sDeviceName & " - "
                End If
                Dim psInfo As String = String.Format("{0}{1:#0}:{2:D2}:{3:D2}:{4:D2}", psDev, oElapsed.Days, oElapsed.Hours, oElapsed.Minutes, oElapsed.Seconds)
                Dim piYVal As Integer = CInt(poGraphics.MeasureString(psInfo, moInfoFont).Height) - 5
                poGraphics.DrawString(psInfo, moInfoFont, moInfoBrush, poGraphRect.Right, piYVal, moStrFmtRight)
            End If


            ' Draw signal waveform now, start with gradient under waveform
            ' Clone the graph points for gradient fill
            Dim poGradientPoints() As Point = DirectCast(poGraphPoints.Clone(), Point())
            ' Set all gradient points to graph bottom
            For i As Integer = 0 To poGradientPoints.Length - 1
                poGradientPoints(i).Y = poGraphRect.Bottom - 1
            Next

            ' Find the highest point in the waveform
            Dim piYMin As Integer = poGraphPoints.Min(Function(p) p.Y) ' Highest peak

            ' Define gradient range from peak signal down to the bottom of the graph
            Dim poGradientRect As New Rectangle(poGraphRect.Left, piYMin, poGraphRect.Width, Math.Max(1, poGraphRect.Bottom - piYMin))

            ' Build the gradient path (mirroring poGraphPoints)
            Using poGradientPath As New Drawing2D.GraphicsPath()
                poGradientPath.AddLines(poGraphPoints)
                poGradientPath.AddLines(poGraphPoints.Select(Function(p) New Point(p.X, poGraphRect.Bottom - 1)).Reverse().ToArray()) ' Close the path
                poGradientPath.CloseFigure() ' Ensure the path is closed properly
                ' Define gradient fill (fades from blue to transparent, now within signal area)
                Using poGradientBrush As New Drawing2D.LinearGradientBrush(poGradientRect, Color.FromArgb(180, 0, 0, 255), Color.Transparent, Drawing2D.LinearGradientMode.Vertical)
                    ' Fill the area under the waveform
                    poGraphics.FillPath(poGradientBrush, poGradientPath)
                End Using
            End Using

            ' Draw actual waveform
            ' Check if smoothing should be applied
            If mfSmoothingTension > 0 Then
                ' Apply smoothing
                Dim poSmoothPath As New Drawing2D.GraphicsPath()
                poSmoothPath.AddCurve(poGraphPoints, mfSmoothingTension)
                poGraphics.DrawPath(moWavePen, poSmoothPath)
            Else
                ' No smoothing - draw direct lines
                poGraphics.DrawLines(moWavePen, poGraphPoints)
            End If
        End Using

        If mWaterBitmap IsNot Nothing AndAlso miWaterHeight > 0 AndAlso miWaterWidth > 0 Then
            GenerateWaterfallBitmap(ddBPowerValues)
            'DebugWaterfall()
        End If



        Return mSignalBitmap
    End Function

    Private Sub DebugWaterfall()
        If mWaterBitmap Is Nothing OrElse miWaterHeight = 0 OrElse miWaterWidth = 0 Then Exit Sub

        SyncLock mWaterBitmap
            ' First, shift the existing bitmap down by 1 row
            Using poGraphics As Graphics = Graphics.FromImage(mWaterBitmap)
                poGraphics.CompositingMode = Drawing2D.CompositingMode.SourceCopy ' Ensures overwrite
                poGraphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor ' Prevents blurring
                poGraphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.None ' Precise pixel shifts

                ' Shift entire bitmap down by 1 row
                poGraphics.DrawImage(mWaterBitmap, New Rectangle(0, 1, miWaterWidth, miWaterHeight - 1),
                                 New Rectangle(0, 0, miWaterWidth, miWaterHeight - 1), GraphicsUnit.Pixel)

                ' now draw alternating color line on the top

                poGraphics.DrawLine(New Pen(If(miDebugToggle Mod 2 = 0, Color.Red, Color.Blue)), 0, 0, mWaterBitmap.Width - 1, 0)
                miDebugToggle += 1
            End Using
        End SyncLock

    End Sub

    Private Sub GenerateWaterfallBitmap(ByVal ddBPowerValues() As Double)
        If mWaterBitmap Is Nothing OrElse miWaterHeight = 0 OrElse miWaterWidth = 0 Then Exit Sub

        SyncLock mWaterBitmap
            ' Lock bitmap data for direct pixel manipulation
            Dim poBitmapData As BitmapData = mWaterBitmap.LockBits(New Rectangle(0, 0, miWaterWidth, miWaterHeight),
                                                                   ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
            Try
                Dim piStride As Integer = poBitmapData.Stride
                Dim piBytes As Integer = piStride * miWaterHeight
                Dim poPixelData(piBytes - 1) As Byte

                ' Read bitmap into memory
                System.Runtime.InteropServices.Marshal.Copy(poBitmapData.Scan0, poPixelData, 0, piBytes)

                ' Shift image **DOWN** by one row (Fixes the "water fountain" effect)
                Buffer.BlockCopy(poPixelData, 0, poPixelData, piStride, piBytes - piStride)

                ' Precompute min/max power levels
                Dim pdMinPower As Double = ddBPowerValues.Min()
                Dim pdMaxPower As Double = ddBPowerValues.Max()
                Dim pdPowerRange As Double = Math.Max(1, pdMaxPower - pdMinPower) ' Avoid division by zero
                Dim pdContrastFactor As Double = Math.Max(0.5, miContrast / 25.0)

                ' Debug Logging (remove after testing)
                Debug.Print($"Power Range: Min={pdMinPower}, Max={pdMaxPower}, Range={pdPowerRange}")

                ' Scale power values and map to colors
                Dim pdBinsPerPixel As Double = ddBPowerValues.Length / miWaterWidth
                Dim piTopRowOffset As Integer = 0 ' New row goes at the **top**

                For x As Integer = 0 To miWaterWidth - 1
                    ' Select a single representative bin instead of averaging (Performance Boost)
                    Dim piBin As Integer = Math.Max(0, Math.Min(ddBPowerValues.Length - 1, CInt(x * pdBinsPerPixel)))
                    Dim pdAvgPower As Double = ddBPowerValues(piBin)

                    ' Normalize power range (Log scale for better contrast)
                    Dim pdNormalizedPower As Double = (pdAvgPower - pdMinPower) / pdPowerRange
                    pdNormalizedPower = Math.Pow(pdNormalizedPower, 0.5) * pdContrastFactor
                    pdNormalizedPower = Math.Max(0.0, Math.Min(1.0, pdNormalizedPower)) ' Clamp

                    ' Convert to 0-255 intensity
                    Dim piIntensity As Integer = CInt(pdNormalizedPower * 255)

                    ' Map intensity to color scale (blue → yellow → red)
                    Dim piOffset As Integer = piTopRowOffset + (x * 4)
                    If piIntensity < 128 Then
                        poPixelData(piOffset) = CByte(255 - (piIntensity * 2)) ' Blue
                        poPixelData(piOffset + 1) = CByte(piIntensity * 2)     ' Green
                        poPixelData(piOffset + 2) = 0                          ' Red
                    Else
                        poPixelData(piOffset) = 0
                        poPixelData(piOffset + 1) = CByte(255 - ((piIntensity - 128) * 2)) ' Green
                        poPixelData(piOffset + 2) = CByte((piIntensity - 128) * 2)         ' Red
                    End If
                    poPixelData(piOffset + 3) = 255 ' Alpha
                Next

                ' Copy modified data back to bitmap
                System.Runtime.InteropServices.Marshal.Copy(poPixelData, 0, poBitmapData.Scan0, piBytes)
            Finally
                mWaterBitmap.UnlockBits(poBitmapData)
            End Try
        End SyncLock
    End Sub



    Private Function GaussianSmooth(ByVal pdData() As Double) As Double()
        Dim piHalfSize As Integer = miKernelSize \ 2
        Dim pdKernel(miKernelSize - 1) As Double
        Dim pdSmoothed(pdData.Length - 1) As Double

        ' Create Gaussian kernel weights
        Dim pdSigma As Double = miKernelSize / 2.0
        Dim pdSum As Double = 0
        For i As Integer = -piHalfSize To piHalfSize
            pdKernel(i + piHalfSize) = Math.Exp(-(i * i) / (2 * pdSigma * pdSigma))
            pdSum += pdKernel(i + piHalfSize)
        Next

        ' Normalize kernel
        For i As Integer = 0 To miKernelSize - 1
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
            ' Use nearest valid value instead of directly copying
            pdSmoothed(i) = pdData(piHalfSize)
            pdSmoothed(pdData.Length - 1 - i) = pdData(pdData.Length - piHalfSize - 1)
        Next

        Return pdSmoothed
    End Function

    ''' <summary>
    ''' Resizes the bitmap if the panel size changes.
    ''' </summary>
    ''' <param name="iWidth">New width</param>
    ''' <param name="iHeight">New height</param>
    Private Sub ResizeBitmap(ByVal iWidth As Integer, ByVal iHeight As Integer)
        Try
            If mSignalBitmap IsNot Nothing Then
                mSignalBitmap.Dispose()
                mSignalBitmap = Nothing
            End If
            miSigWidth = iWidth
            miSigHeight = iHeight
            mSignalBitmap = New Bitmap(miSigWidth, miSigHeight)
        Catch ex As Exception
            clsLogger.LogException("clsRenderWaveform.ResizeBitmap", ex)
        End Try
    End Sub

    Private Sub ResizeWaterfallBitmap(ByVal iWidth As Integer, ByVal iHeight As Integer)
        Try
            If mWaterBitmap IsNot Nothing Then
                mWaterBitmap.Dispose()
                mWaterBitmap = Nothing
            End If
            miWaterWidth = iWidth
            miWaterHeight = iHeight
            mWaterBitmap = New Bitmap(miWaterWidth, miWaterHeight)
            Using g As Graphics = Graphics.FromImage(mWaterBitmap)
                g.Clear(Color.DarkGray)
            End Using

        Catch ex As Exception
            clsLogger.LogException("clsRenderWaveform.ResizeWaterfallBitmap", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cleans up the bitmap resources.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        If mSignalBitmap IsNot Nothing Then
            mSignalBitmap.Dispose()
            mSignalBitmap = Nothing
        End If
        If mWaterBitmap IsNot Nothing Then
            mWaterBitmap.Dispose()
            mWaterBitmap = Nothing
        End If
        ' Dispose of fonts, brushes, and pens
        moLabelFont.Dispose()
        moRecordFont.Dispose()
        moInfoFont.Dispose()
        moLabelBrush.Dispose()
        moRecordBrush.Dispose()
        moInfoBrush.Dispose()
        moAxisPen.Dispose()
        moGridPen.Dispose()
        moCenterFreqPen.Dispose()
        moWavePen.Dispose()
    End Sub

End Class
