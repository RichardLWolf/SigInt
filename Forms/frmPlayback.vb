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

    Private Sub hsbSeekPos_Scroll(sender As Object, e As ScrollEventArgs) Handles hsbSeekPos.Scroll
        If Not fbPlaying Then Exit Sub
        ' alter current "position" to new value based on hsb value and buffer size
        ' then update the bitmap and re-render
    End Sub


    Private Sub picArchiveInfo_Paint(sender As Object, e As PaintEventArgs) Handles picArchiveInfo.Paint
        Dim poG As Graphics = e.Graphics

        poG.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        poG.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        poG.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        poG.Clear(Me.BackColor)
        If Not fbPlaying OrElse foArchive Is Nothing Then
            Exit Sub
        End If

        Dim poRect As New Rectangle(0, 0, picArchiveInfo.Width, picArchiveInfo.Height)
        Dim piRadius As Integer = 15 'Corner radius

        Using poBackBrush As New SolidBrush(Color.CornflowerBlue)
            Using poPath As New GraphicsPath()
                poPath.AddArc(poRect.X, poRect.Y, piRadius, piRadius, 180, 90) 'Top left corner
                poPath.AddArc(poRect.X + poRect.Width - piRadius, poRect.Y, piRadius, piRadius, 270, 90) 'Top right corner
                poPath.AddArc(poRect.X + poRect.Width - piRadius, poRect.Y + poRect.Height - piRadius, piRadius, piRadius, 0, 90) 'Bottom right corner
                poPath.AddArc(poRect.X, poRect.Y + poRect.Height - piRadius, piRadius, piRadius, 90, 90) 'Bottom left corner
                poPath.CloseFigure() 'Close the figure
                e.Graphics.FillPath(poBackBrush, poPath) 'fill it
            End Using
        End Using

        ' TODO: drawsting the complete archive medata info (making it look like someone "modern" made it ;) (i.e. various font sizes, colors to convey the information)

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
                ' reset form flags 
                fbPaused = False
                fbPlaying = True
                picStartStop_MouseLeave(Nothing, Nothing)

                ' Set scrollbar min/max based on total buffer chunks
                hsbSeekPos.Minimum = 0
                hsbSeekPos.Maximum = foArchive.IQBuffer.Count - 1
                hsbSeekPos.Value = 0 ' Start at the beginning

                ' start playback thread
                Dim poThread As New System.Threading.Thread(AddressOf PlaybackRecording)
                poThread.IsBackground = True
                poThread.Start()
            End If
        Else
            ' stop playback thread
            fbPlaying = False
            fbPaused = False
            ' give render thread time to exit
            System.Threading.Thread.Sleep(150)
            ' reset buttons
            picStartStop.Image = My.Resources.media_play
            picPause.Image = My.Resources.media_pause
            ' clear archive (will claer IQBuffer)
            foArchive.Dispose()
            foArchive = Nothing
            ' clear bitmaps
            foRenderer.Dispose()
            foRenderer = Nothing
            ' black display
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






    Private Sub PlaybackRecording()
        Dim plChunkSize As Integer = foArchive.BufferSizeBytes ' Size of each buffer chunk
        Dim plTotalChunks As Integer = foArchive.IQBuffer.Count ' Total chunks
        Dim plSampleRate As Integer = foArchive.SampleRate ' Samples per second
        Dim plDelayMs As Integer = CInt(1000 * (plChunkSize / plSampleRate)) ' Compute delay per chunk

        Dim plCurrentChunk As Integer = 0

        While fbPlaying
            If fbPaused Then
                Threading.Thread.Sleep(100) ' Sleep while paused
                Continue While
            End If

            ' Restart from the beginning if we reach the end
            If plCurrentChunk >= plTotalChunks Then
                plCurrentChunk = 0 ' Reset playback position
                SetHsbSeekValue(0) ' Reset scrollbar to beginning
            End If

            ' Get current buffer chunk
            Dim poChunk As Byte() = foArchive.IQBuffer(plCurrentChunk)

            ' Send data to renderer
            Dim panelSize As Size = GetSignalPanelSize()
            If panelSize.Width > 0 AndAlso panelSize.Height > 0 Then
                SyncLock foBitmapsLock
                    Dim pdPowerValues() As Double = RtlSdrApi.ConvertRawToPowerLevels(poChunk)
                    ' make sure stop wasn't pressed while we were calculating dB levels
                    If foArchive Is Nothing OrElse fbPlaying = False Then
                        Exit While
                    End If
                    ' otherwise update the bitmap
                    foSignalBmp = foRenderer.RenderGraph(panelSize.Width, panelSize.Height, pdPowerValues _
                                                        , foArchive.SampleRate, foArchive.CenterFrequency _
                                                        , False, Nothing, "", Nothing)
                End SyncLock
            End If

            ' Update UI
            UpdateSpectrum()

            ' Move scrollbar to reflect position
            SetHsbSeekValue(CInt((plCurrentChunk / plTotalChunks) * hsbSeekPos.Maximum))

            ' Wait to maintain real-time playback speed
            Threading.Thread.Sleep(plDelayMs)

            ' Move to next chunk
            plCurrentChunk += 1
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
        If panSignal.InvokeRequired Then
            ' Invoke on the UI thread if called from a worker thread
            Return CType(panSignal.Invoke(Function() GetSignalPanelSize()), Size)
        Else
            ' Directly return panel size if already on UI thread
            Return New Size(panSignal.Width, panSignal.Height)
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