Imports System.Net.Http

Public Class frmViewLog
    Private foLogEntries As New List(Of String)

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Try
            Clipboard.Clear()
            Clipboard.SetText(String.Join(vbCrLf, foLogEntries))
            MsgBox("Log copied to clipboard", MessageBoxButtons.OK + MessageBoxIcon.Information, "Log Copied")
        Catch ex As Exception
            MsgBox("Error copying log to Windows clipboard.", MessageBoxButtons.OK + MessageBoxIcon.Exclamation, "Error")
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Call LoadLogFile()
        If txtSearch.Text <> "" Then
            txtSearch.Text = "" 'textchanged event will take care of it 
        Else
            txtSearch_TextChanged(Nothing, Nothing) ' force it to reload
        End If
    End Sub


    Private Sub frmViewLog_Load(sender As Object, e As EventArgs) Handles Me.Load
        lvwLog.WatermarkImage = New Bitmap(My.Resources.sigint_icon, New Size(128, 128))

        With lvwLog
            .View = View.Details
            .MultiSelect = False
            .FullRowSelect = True
            .GridLines = True
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("DATE", "Timestamp", 180)
            .Columns.Add("SOURCE", "Source", 200)
            .Columns.Add("TEXT", "Message", 600)
        End With

        LoadLogFile()
        txtSearch.Text = ""
        txtSearch_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub lvwLog_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvwLog.DrawColumnHeader
        Using oBackBrush As New SolidBrush(Color.FromArgb(30, 30, 30)) ' #1E1E1E Dark Header
            e.Graphics.FillRectangle(oBackBrush, e.Bounds)
        End Using

        Using oTextBrush As New SolidBrush(Color.FromArgb(255, 165, 0)) ' #FFA500 Orange Text
            Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            e.Graphics.DrawString(e.Header.Text, lvwLog.Font, oTextBrush, e.Bounds, sf)
        End Using
    End Sub


    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim psFilter As String = txtSearch.Text.Trim().ToLower()
        Dim poLvItems As New List(Of ListViewItem)
        Dim piCount As Integer = 0


        If String.IsNullOrEmpty(psFilter) Then
            ' Show all log entries
            For Each psLine As String In foLogEntries
                Dim poItem As ListViewItem = ParseLogEntry(psLine)
                poItem.BackColor = If(piCount Mod 2 = 0, Color.FromArgb(50, 50, 50), Color.FromArgb(43, 43, 43))
                poLvItems.Add(poItem)
            Next
        Else
            ' Filter by search term
            For Each psLine As String In foLogEntries
                If psLine.Contains(psFilter, StringComparison.CurrentCultureIgnoreCase) Then
                    Dim poItem As ListViewItem = ParseLogEntry(psLine)
                    poItem.BackColor = If(piCount Mod 2 = 0, Color.FromArgb(50, 50, 50), Color.FromArgb(43, 43, 43))
                    poLvItems.Add(poItem)
                End If
            Next
        End If
        AddBulkToListview(poLvItems)
    End Sub

    Private Function ParseLogEntry(ByVal sLine As String) As ListViewItem
        Dim lvItem As New ListViewItem()
        Dim psTimestamp As String = ""
        Dim psSource As String = ""
        Dim psMessage As String = sLine.Trim()

        If sLine.StartsWith("[") AndAlso sLine.Contains("|") AndAlso sLine.Contains("] - ") Then
            Dim piStart As Integer = sLine.IndexOf("[") + 1
            Dim piMid As Integer = sLine.IndexOf("|")
            Dim piEnd As Integer = sLine.IndexOf("] - ")
            psTimestamp = sLine.Substring(piStart, piMid - piStart).Trim()
            psSource = sLine.Substring(piMid + 1, piEnd - piMid - 1).Trim()
            psMessage = sLine.Substring(piEnd + 4).Trim()
        End If
        lvItem.Text = psTimestamp
        lvItem.SubItems.Add(psSource)
        lvItem.SubItems.Add(psMessage)

        Return lvItem
    End Function

    Private Sub LoadLogFile()
        Try
            foLogEntries = clsLogger.LoadLog()

        Catch ex As Exception
            clsLogger.LogException("frmViewLog.LoadLogFile", ex)
        End Try
    End Sub




    Private Delegate Sub Del_AddBulkToListview(ByVal oItems As List(Of ListViewItem))
    Private Sub AddBulkToListview(ByVal oItems As List(Of ListViewItem))
        If lvwLog.InvokeRequired Then
            Dim poCB As New Del_AddBulkToListview(AddressOf AddBulkToListview)
            Dim poParm(0) As Object
            poParm(0) = oItems
            lvwLog.Invoke(poCB, poParm)
        Else
            lvwLog.SuspendLayout()
            lvwLog.Items.Clear()
            If oItems IsNot Nothing AndAlso oItems.Count > 0 Then
                lvwLog.Items.AddRange(oItems.ToArray())
            End If
            lvwLog.ResumeLayout(True)
            If lvwLog.Items.Count > 0 Then
                lvwLog.Items(lvwLog.Items.Count - 1).EnsureVisible()
            End If
        End If
    End Sub


End Class