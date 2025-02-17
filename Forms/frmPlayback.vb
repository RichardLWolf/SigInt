Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D


Public Class frmPlayback

    Private fsArchivePath As String
    Private fbLoading As Boolean = False
    Private fbPlaying As Boolean = False
    Private fbPaused As Boolean = False
    Private foArchive As IQZipper
    Private foRenderer As clsRenderWaveform
    Private foBitmapsLock As New Object()      'sync object for bitmap updatings
    Private foSignalBmp As Bitmap
    Private foWaterfallBmp As Bitmap
    Private foPlaybackThread As System.Threading.Thread
    Private flCurrentChunk As Long   ' current play position


    Public Sub ReadyForm()
        fsArchivePath = clsLogger.LogPath

        With lvwArchives
            .View = View.Details
            .MultiSelect = False
            .FullRowSelect = True
            .GridLines = True
            .Items.Clear()
            .SmallImageList = Nothing
            .Columns.Clear()
            .Columns.Add("DATE", "Recorded On", 145)
            .Columns.Add("FREQ", "Center", 80)
            .Columns.Add("DUR", "Duration", 65)
        End With
        ReloadListview()
    End Sub


    Private Sub frmPlayback_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        fbPlaying = False
        ' Ensure thread has stopped
        If foPlaybackThread IsNot Nothing AndAlso foPlaybackThread.IsAlive Then
            foPlaybackThread.Join(1000)
        End If

        foPlaybackThread = Nothing

    End Sub


    Private Sub hsbSeekPos_MouseDown(sender As Object, e As MouseEventArgs) Handles hsbSeekPos.MouseDown
        If Not fbPlaying Then Exit Sub
        ' pause playback 
        fbPaused = True
    End Sub

    Private Sub hsbSeekPos_MouseUp(sender As Object, e As MouseEventArgs) Handles hsbSeekPos.MouseUp
        If Not fbPlaying Then Exit Sub
        ' resume playback
        fbPaused = False
    End Sub

    Private Sub hsbSeekPos_Scroll(sender As Object, e As EventArgs) Handles hsbSeekPos.Scroll
        If Not fbPlaying Then Exit Sub

        ' Calculate new chunk index based on scrollbar position and then grab it
        Dim plNewChunk As Integer = CInt((hsbSeekPos.Value / hsbSeekPos.Maximum) * foArchive.IQBuffer.Count)
        Dim poChunk As Byte() = foArchive.IQBuffer(plNewChunk)
        flCurrentChunk = plNewChunk

        ' Update playback position
        SyncLock foBitmapsLock
            Dim panelSize As Size = GetSignalPanelSize()
            Dim poWaterSize As Size = GetWaterfallPanelSize()
            Dim pdPowerValues() As Double = RtlSdrApi.ConvertRawToPowerLevels(poChunk)
            ' otherwise update the bitmap
            foSignalBmp = foRenderer.RenderGraph(panelSize.Width, panelSize.Height, pdPowerValues _
                                                        , foArchive.SampleRate, foArchive.CenterFrequency _
                                                        , False, Nothing, "", Nothing _
                                                        , poWaterSize.Width, poWaterSize.Height)
            foWaterfallBmp = foRenderer.WaterfallBitmap
        End SyncLock
        UpdateSpectrum()
        flCurrentChunk = plNewChunk
    End Sub


    Private Sub picArchiveInfo_Paint(sender As Object, e As PaintEventArgs) Handles picArchiveInfo.Paint
        Dim poG As Graphics = e.Graphics

        ' Enable high-quality rendering
        poG.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        poG.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        poG.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Clear background
        poG.Clear(Me.BackColor)

        ' Don't draw if nothing is playing
        If Not fbPlaying OrElse foArchive Is Nothing Then Exit Sub

        ' Define panel area
        Dim poRect As New Rectangle(0, 0, picArchiveInfo.Width, picArchiveInfo.Height)
        Dim piRadius As Integer = 25 ' Corner radius

        ' Gradient background
        Using poBackBrush As New LinearGradientBrush(poRect, Color.FromArgb(50, 50, 100), Color.FromArgb(30, 30, 60), LinearGradientMode.Vertical)
            Using poPath As New GraphicsPath()
                poPath.AddArc(poRect.X, poRect.Y, piRadius, piRadius, 180, 90)
                poPath.AddArc(poRect.X + poRect.Width - piRadius, poRect.Y, piRadius, piRadius, 270, 90)
                poPath.AddArc(poRect.X + poRect.Width - piRadius, poRect.Y + poRect.Height - piRadius, piRadius, piRadius, 0, 90)
                poPath.AddArc(poRect.X, poRect.Y + poRect.Height - piRadius, piRadius, piRadius, 90, 90)
                poPath.CloseFigure()
                poG.FillPath(poBackBrush, poPath)
            End Using
        End Using

        ' Define fonts
        Dim poTitleFont As New Font("Segoe UI", 12, FontStyle.Bold)
        Dim poDataFont As New Font("Segoe UI", 10, FontStyle.Regular)
        Dim poLabelFont As New Font("Segoe UI", 9, FontStyle.Italic)

        ' Define colors
        Dim poLabelBrush As New SolidBrush(Color.LightGray)
        Dim poDataBrush As New SolidBrush(Color.White)

        ' Define text positions
        Dim piXOffset As Integer = 15
        Dim piYOffset As Integer = 15
        Dim piSpacing As Integer = 22 ' Spacing between rows

        ' Format data
        Dim psFilename As String = IO.Path.GetFileName(foArchive.ArchiveFile)
        Dim poTitleRect As New RectangleF(piXOffset, piYOffset, poRect.Width - (piXOffset * 2), 25) ' Limit width for title

        ' Ensure title fits within available space
        psFilename = FitTextToWidth(poG, psFilename, poTitleFont, poTitleRect.Width)

        ' Draw Title (Filename)
        poG.DrawString(psFilename, poTitleFont, poDataBrush, poTitleRect)

        piYOffset += piSpacing + 5 ' Extra spacing for title

        ' Draw Labels and Data
        DrawMetadataLine(poG, "Recorded On:", foArchive.RecordedOnUTC.ToLocalTime.ToString("MMM dd, yyyy HH:mm:ss"), poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "Center Freq:", modMain.FormatHertz(foArchive.CenterFrequency), poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "Sample Rate:", (foArchive.SampleRate / 1_000_000).ToString("0.0###") & " MSPS", poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "Buffer Size:", FormatBytes(foArchive.BufferSizeBytes), poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "Total Size:", FormatBytes(foArchive.TotalIQBytes), poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "Duration:", TimeSpan.FromSeconds(foArchive.DurationSeconds).ToString("mm\:ss"), poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)
        piYOffset += piSpacing

        DrawMetadataLine(poG, "App Version:", foArchive.AppVer, poLabelFont, poDataFont, poLabelBrush, poDataBrush, piXOffset, piYOffset)

        ' Cleanup
        poTitleFont.Dispose()
        poDataFont.Dispose()
        poLabelFont.Dispose()
        poLabelBrush.Dispose()
        poDataBrush.Dispose()
    End Sub

    Private Sub picPause_Click(sender As Object, e As EventArgs) Handles picPause.Click
        If Not fbPlaying Then Exit Sub
        If fbPaused Then
            fbPaused = False
            picPause.Image = My.Resources.media_pause
        Else
            fbPaused = True
            picPause.Image = My.Resources.media_play_green
        End If
    End Sub

    Private Sub picPause_MouseEnter(sender As Object, e As EventArgs) Handles picPause.MouseEnter
        If Not fbPlaying Then Exit Sub
        If fbPaused Then
            picPause.Image = My.Resources.media_play
        Else
            picPause.Image = My.Resources.media_pause_blue
        End If
    End Sub

    Private Sub picPause_MouseLeave(sender As Object, e As EventArgs) Handles picPause.MouseLeave
        If Not fbPlaying Then Exit Sub
        If fbPaused Then
            picPause.Image = My.Resources.media_play_green
        Else
            picPause.Image = My.Resources.media_pause
        End If
    End Sub

    Private Sub picRefresh_Click(sender As Object, e As EventArgs) Handles picRefresh.Click
        ReloadListview()
    End Sub

    Private Sub picRefresh_MouseEnter(sender As Object, e As EventArgs) Handles picRefresh.MouseEnter
        picRefresh.Image = My.Resources.refresh_blue
    End Sub

    Private Sub picRefresh_MouseLeave(sender As Object, e As EventArgs) Handles picRefresh.MouseLeave
        picRefresh.Image = My.Resources.refresh
    End Sub

    Private Sub picStartStop_Click(sender As Object, e As EventArgs) Handles picStartStop.Click
        If Not fbPlaying Then
            Dim poSelectedItem As New IQZipper.IQArchiveMetadata
            If lvwArchives.SelectedItems.Count > 0 Then
                If lvwArchives.SelectedItems(0).Tag IsNot Nothing AndAlso TypeOf (lvwArchives.SelectedItems(0).Tag) Is IQZipper.IQArchiveMetadata Then
                    poSelectedItem = DirectCast(lvwArchives.SelectedItems(0).Tag, IQZipper.IQArchiveMetadata)
                End If
            End If
            If String.IsNullOrEmpty(poSelectedItem.FileName) Then
                MsgBox("Please select a signal archive to playback.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "No Archive Selected")
                lvwArchives.Focus()
                Exit Sub
            Else
                foArchive = New IQZipper(fsArchivePath)
                If Not foArchive.LoadArchive(poSelectedItem.FileName) Then
                    MsgBox("An error occurred loading the archive, please select another.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Invalid or Corrupt Archive")
                    lvwArchives.Focus()
                    Exit Sub
                End If
                picArchiveInfo.Invalidate() 'cause the "info" label to fill in wiht the foArchive data.
                ' ready bitmap rendering
                foRenderer = New clsRenderWaveform(picStartStop.Width, picStartStop.Height)
                ' sync sliders to match renderer
                sldContrast.Value = foRenderer.WaterfallContrast
                sldZoom.Value = foRenderer.ZoomFactor
                sldOffset.Value = foRenderer.dBOffset
                sldRange.Value = foRenderer.dBRange
                ' reset form flags 
                fbPaused = False
                fbPlaying = True
                picStartStop_MouseLeave(Nothing, Nothing)

                ' Set scrollbar min/max based on total buffer chunks
                hsbSeekPos.Minimum = 0
                hsbSeekPos.Maximum = foArchive.IQBuffer.Count - 1
                hsbSeekPos.Value = 0 ' Start at the beginning

                ' start playback thread
                foPlaybackThread = New System.Threading.Thread(AddressOf PlaybackRecording)
                foPlaybackThread.IsBackground = True
                foPlaybackThread.Start()
            End If
        Else
            ' stop playback thread
            fbPlaying = False
            fbPaused = False
            ' wait for render thread to exit
            If foPlaybackThread IsNot Nothing AndAlso foPlaybackThread.IsAlive Then
                foPlaybackThread.Join(1000) ' Blocks until thread exits
            End If
            foPlaybackThread = Nothing
            ' reset ui controls 
            picStartStop.Image = My.Resources.media_play
            picPause.Image = My.Resources.media_pause
            hsbSeekPos.Value = 0
            ' clear archive (will claer IQBuffer)
            foArchive.Dispose()
            foArchive = Nothing
            ' clear bitmaps
            foRenderer.Dispose()
            foRenderer = Nothing
            ' clear info display
            picArchiveInfo.Invalidate()
            ' black graph display
            UpdateSpectrum()
        End If
    End Sub

    Private Sub picStartStop_MouseEnter(sender As Object, e As EventArgs) Handles picStartStop.MouseEnter
        If fbPlaying Then
            picStartStop.Image = My.Resources.media_stop
        Else
            picStartStop.Image = My.Resources.media_play
        End If
    End Sub

    Private Sub picStartStop_MouseLeave(sender As Object, e As EventArgs) Handles picStartStop.MouseLeave
        If fbPlaying Then
            picStartStop.Image = My.Resources.media_stop_red
        Else
            picStartStop.Image = My.Resources.media_play_green
        End If
    End Sub

    Private Sub sldOffset_ValueChanged(sender As Object, e As EventArgs) Handles sldOffset.ValueChanged
        If foRenderer IsNot Nothing Then
            foRenderer.dBOffset = sldOffset.Value
        End If
    End Sub

    Private Sub sldRange_ValueChanged(sender As Object, e As EventArgs) Handles sldRange.ValueChanged
        If foRenderer IsNot Nothing Then
            foRenderer.dBRange = sldRange.Value
        End If
    End Sub

    Private Sub sldZoom_ValueChanged(sender As Object, e As EventArgs) Handles sldZoom.ValueChanged
        If foRenderer IsNot Nothing Then
            foRenderer.ZoomFactor = sldZoom.Value * 0.009D + 0.1D ' Convert range 0-100 → 0.1-1.0
        End If
    End Sub

    Private Sub sldContrast_ValueChanged(sender As Object, e As EventArgs) Handles sldContrast.ValueChanged
        If foRenderer IsNot Nothing Then
            foRenderer.WaterfallContrast = sldContrast.Value
        End If
    End Sub





    Private Sub PlaybackRecording()
        Dim plChunkSize As Integer = foArchive.BufferSizeBytes ' Size of each buffer chunk
        Dim plTotalChunks As Integer = foArchive.IQBuffer.Count ' Total chunks
        Dim plSampleRate As Integer = foArchive.SampleRate ' Samples per second
        Dim plDelayMs As Integer = CInt(1000 * (plChunkSize / plSampleRate)) ' Compute delay per chunk

        flCurrentChunk = 0

        While fbPlaying
            If fbPaused Then
                Threading.Thread.Sleep(100) ' Sleep while paused
                Continue While
            End If

            ' Restart from the beginning if we reach the end
            If flCurrentChunk >= plTotalChunks Then
                flCurrentChunk = 0 ' Reset playback position
                SetHsbSeekValue(0) ' Reset scrollbar to beginning
            End If

            If Not fbPlaying Then Exit While

            ' Get current buffer chunk
            Dim poChunk As Byte() = foArchive.IQBuffer(flCurrentChunk)

            ' Send data to renderer
            If Not fbPlaying Then Exit While
            Dim panelSize As Size = GetSignalPanelSize()
            Dim poWaterSize As Size = GetWaterfallPanelSize()
            If Not fbPlaying Then Exit While
            If panelSize.Width > 0 AndAlso panelSize.Height > 0 Then
                SyncLock foBitmapsLock
                    Dim pdPowerValues() As Double = RtlSdrApi.ConvertRawToPowerLevels(poChunk)
                    If Not fbPlaying Then Exit While
                    ' otherwise update the bitmap
                    foSignalBmp = foRenderer.RenderGraph(panelSize.Width, panelSize.Height, pdPowerValues _
                                                        , foArchive.SampleRate, foArchive.CenterFrequency _
                                                        , False, Nothing, "", Nothing _
                                                        , poWaterSize.Width, poWaterSize.Height)
                    foWaterfallBmp = foRenderer.WaterfallBitmap
                End SyncLock
            End If
            If Not fbPlaying Then Exit While

            ' Update UI
            UpdateSpectrum()

            If Not fbPlaying Then Exit While
            ' Move scrollbar to reflect position
            SetHsbSeekValue(CInt((flCurrentChunk / plTotalChunks) * hsbSeekPos.Maximum))

            If Not fbPlaying Then Exit While
            ' Wait to maintain real-time playback speed
            Threading.Thread.Sleep(plDelayMs)

            ' Move to next chunk
            flCurrentChunk += 1
            If Not fbPlaying Then Exit While
        End While
    End Sub


    Private Sub SetHsbSeekValue(ByVal NewVal As Integer)
        If Me.InvokeRequired Then
            ' Marshal the update to the UI thread
            Me.BeginInvoke(New MethodInvoker(Sub() SetHsbSeekValue(NewVal)))
            Exit Sub
        End If
        hsbSeekPos.Value = NewVal
    End Sub


    Private Sub UpdateSpectrum()
        If Me.InvokeRequired Then
            ' Marshal the update to the UI thread
            Me.BeginInvoke(New MethodInvoker(AddressOf UpdateSpectrum))
            Exit Sub
        End If

        If panSignal Is Nothing OrElse panSignal.Handle = IntPtr.Zero OrElse panWaterfall Is Nothing OrElse panWaterfall.Handle = IntPtr.Zero Then
            Exit Sub
        End If

        SyncLock foBitmapsLock
            Using g As Graphics = panSignal.CreateGraphics()
                If fbPlaying Then
                    If foSignalBmp IsNot Nothing Then
                        g.DrawImageUnscaled(foSignalBmp, 0, 0) ' Direct draw, no Paint flickering
                    End If
                Else
                    g.Clear(Color.Black)
                End If
            End Using
            Using g As Graphics = panWaterfall.CreateGraphics()
                If fbPlaying Then
                    If foWaterfallBmp IsNot Nothing Then
                        g.DrawImageUnscaled(foWaterfallBmp, 0, 0) ' Direct draw, no Paint flickering
                    End If
                Else
                    g.Clear(Color.Black)
                End If
            End Using
        End SyncLock
    End Sub

    Private Sub DrawMetadataLine(ByRef poG As Graphics, ByVal psLabel As String, ByVal psData As String,
                             ByVal poLabelFont As Font, ByVal poDataFont As Font,
                             ByVal poLabelBrush As Brush, ByVal poDataBrush As Brush,
                             ByVal piX As Integer, ByVal piY As Integer)
        ' Draw label
        poG.DrawString(psLabel, poLabelFont, poLabelBrush, piX, piY)

        ' Offset for data to align consistently
        Dim piDataOffset As Integer = 120 ' Adjust spacing as needed
        poG.DrawString(psData, poDataFont, poDataBrush, piX + piDataOffset, piY)
    End Sub

    Private Function FitTextToWidth(poG As Graphics, psText As String, poFont As Font, pdMaxWidth As Single) As String
        Dim poTextSize As SizeF = poG.MeasureString(psText, poFont)

        ' If text fits, return as is
        If poTextSize.Width <= pdMaxWidth Then
            Return psText
        End If

        ' Try reducing font size until it fits
        Dim pdFontSize As Single = poFont.Size
        While poTextSize.Width > pdMaxWidth And pdFontSize > 8
            pdFontSize -= 0.5F ' Reduce font size step by step
            Using poNewFont As New Font(poFont.FontFamily, pdFontSize, poFont.Style)
                poTextSize = poG.MeasureString(psText, poNewFont)
            End Using
        End While

        ' If font size is already small and text still doesn't fit, trim it
        If poTextSize.Width > pdMaxWidth Then
            While poTextSize.Width > pdMaxWidth And psText.Length > 3
                psText = psText.Substring(0, psText.Length - 2) & "…"
                poTextSize = poG.MeasureString(psText, poFont)
            End While
        End If

        Return psText
    End Function

    Private Sub ReloadListview()
        If fbLoading Then Exit Sub
        fbLoading = True
        lvwArchives.Items.Clear()
        lvwArchives.Items.Add("Loading...")
        lvwArchives.Refresh()
        Dim poLoadThread As New System.Threading.Thread(AddressOf LoadListview)
        poLoadThread.IsBackground = True
        poLoadThread.Start()
    End Sub

    Private Function GetSignalPanelSize() As Size
        If panSignal Is Nothing OrElse panSignal.IsDisposed Then
            Return New Size(0, 0)
        End If
        If panSignal.InvokeRequired Then
            Try
                ' Ensure the control is still valid before invoking
                If panSignal Is Nothing OrElse panSignal.IsDisposed Then
                    Return New Size(0, 0)
                End If

                ' Use Invoke, but catch any disposal-related exceptions
                Return CType(panSignal.Invoke(Function() GetSignalPanelSize()), Size)
            Catch ex As ObjectDisposedException
                ' The control was disposed before the invoke completed
                Return New Size(0, 0)
            Catch ex As InvalidOperationException
                ' The form is closing, and Invoke is no longer allowed
                Return New Size(0, 0)
            End Try
        Else
            ' Ensure the control is still valid
            If panSignal Is Nothing OrElse panSignal.Handle = IntPtr.Zero Then
                Return New Size(0, 0)
            End If

            Return New Size(panSignal.Width, panSignal.Height)
        End If
    End Function


    Private Function GetWaterfallPanelSize() As Size
        If panWaterfall Is Nothing OrElse panWaterfall.IsDisposed Then
            Return New Size(0, 0)
        End If
        If panWaterfall.InvokeRequired Then
            Try
                ' Ensure the control is still valid before invoking
                If panWaterfall Is Nothing OrElse panWaterfall.IsDisposed Then
                    Return New Size(0, 0)
                End If

                ' Use Invoke, but catch any disposal-related exceptions
                Return CType(panWaterfall.Invoke(Function() GetWaterfallPanelSize()), Size)
            Catch ex As ObjectDisposedException
                ' The control was disposed before the invoke completed
                Return New Size(0, 0)
            Catch ex As InvalidOperationException
                ' The form is closing, and Invoke is no longer allowed
                Return New Size(0, 0)
            End Try
        Else
            ' Ensure the control is still valid
            If panWaterfall Is Nothing OrElse panWaterfall.Handle = IntPtr.Zero Then
                Return New Size(0, 0)
            End If

            Return New Size(panWaterfall.Width, panWaterfall.Height)
        End If
    End Function


    Private Sub LoadListview()
        Dim poFiles As List(Of IQZipper.IQArchiveMetadata) = IQZipper.GetArchiveFileList(fsArchivePath)
        Dim poList As New List(Of ListViewItem)

        For Each poArc As IQZipper.IQArchiveMetadata In poFiles
            Dim poItem As New ListViewItem
            poItem.Tag = poArc
            poItem.Text = String.Format("{0:MMM-dd-yyyy HH:mm:ss}", poArc.RecordingTimeUTC.ToLocalTime)
            poItem.SubItems.Add(modMain.FormatHertz(poArc.CenterFrequency))
            Dim poTS As TimeSpan = TimeSpan.FromSeconds(poArc.RecordingDurationSec)
            poItem.SubItems.Add($"{poTS.Minutes:D2}:{poTS.Seconds:D2}")
            poList.Add(poItem)
        Next
        AddBulkToListview(poList)
        fbLoading = False
    End Sub

    Private Delegate Sub Del_AddBulkToListview(ByVal oItems As List(Of ListViewItem))
    Private Sub AddBulkToListview(ByVal oItems As List(Of ListViewItem))
        If lvwArchives.InvokeRequired Then
            Dim poCB As New Del_AddBulkToListview(AddressOf AddBulkToListview)
            Dim poParm(0) As Object
            poParm(0) = oItems
            lvwArchives.Invoke(poCB, poParm)
        Else
            lvwArchives.Items.Clear()
            If oItems.Count > 0 Then
                lvwArchives.Items.AddRange(oItems.ToArray())
            Else
                lvwArchives.Items.Add("No signal archives found")
            End If
            lvwArchives.Refresh()
        End If
    End Sub


End Class