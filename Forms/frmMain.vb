


Public Class frmMain

    Private foAppConfig As clsAppConfig


    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        ' close any open windows
        For Each foForm As Form In Application.OpenForms.Cast(Of Form).ToList()
            If foForm IsNot Me Then foForm.Close()
        Next
        ' close main form
        Me.Close()
    End Sub

    Private Sub btnMinimize_Click(sender As Object, e As EventArgs) Handles btnMinimize.Click
        For Each foForm As Form In Application.OpenForms.Cast(Of Form).ToList()
            If foForm IsNot Me Then foForm.WindowState = FormWindowState.Minimized
        Next
        ' minimuze main form
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub btnMonitor_Click(sender As Object, e As EventArgs) Handles btnMonitor.Click
        If cboDeviceList.SelectedItem Is Nothing Then
            MsgBox("Please select a RTL-SDR device from the list.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "No Device Selected")
            cboDeviceList.Focus()
        Else
            Dim poDev As RtlSdrApi.SdrDevice = cboDeviceList.SelectedItem
            If poDev.DeviceIndex < 0 Then
                MsgBox("Please plug in a RTL-SDR device and click the refresh button.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "No Devices Detected")
                cboDeviceList.Focus()
            Else
                If cboConfigs.SelectedItem Is Nothing Then
                    MsgBox("Please select a configuration to use from the list or create a new one.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "No Configuration Selected")
                    cboConfigs.Focus()
                Else
                    Dim poCfg = DirectCast(cboConfigs.SelectedItem.value, DeviceConfig)
                    If poCfg Is Nothing Then
                        MsgBox("Please select a configuration to use from the list or create a new one.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "No Configuration Selected")
                        cboConfigs.Focus()
                    Else
                        Dim psFormTitle = $"SigInt ({poDev.DeviceIndex})-{poDev.DeviceName} Monitor"
                        ' we already have this form up?
                        For Each poForm In Application.OpenForms.Cast(Of Form).ToList
                            If TypeOf poForm Is frmMonitor Then
                                If poForm.Text = psFormTitle Then
                                    ' reapply selected configuration to the existing form
                                    Dim poMonFrm = DirectCast(poForm, frmMonitor)
                                    poMonFrm.ApplyConfiguration(poCfg, foAppConfig.DiscordServerWebhook, foAppConfig.DiscordMentionID)
                                    poMonFrm.WindowState = FormWindowState.Normal
                                    poMonFrm.Visible = True
                                    poMonFrm.BringToFront()
                                    poMonFrm.Focus()
                                    Exit Sub
                                End If
                            End If
                        Next
                        ' open a new monitor form
                        Dim poFrm As New frmMonitor
                        poFrm.Text = $"SigInt ({poDev.DeviceIndex})-{poDev.DeviceName} Monitor"
                        poFrm.ReadyForm(poDev.DeviceIndex, poCfg, foAppConfig.DiscordServerWebhook, foAppConfig.DiscordMentionID _
                                        , foAppConfig.ThingSpeakEnabled, foAppConfig.UserGUID, foAppConfig.UserLat, foAppConfig.UserLon)
                        poFrm.Show(Me)
                        poFrm.Focus()
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup logger
        If clsLogger.ValidateLogFolder() = False Then
            MsgBox($"Failed to create program log file folder:{ControlChars.CrLf}{clsLogger.LogFileName}")
        End If
        ' keep the size down
        clsLogger.PurgeLog()

        ' start the unhandled exception handler
        clsUEH.StartUEH()
        ' load devices
        GetRtlSdrDeviceList()
        ' Load config
        foAppConfig = clsAppConfig.Load()
        LoadConfigs()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Stop the unhandled Exception Handler
        clsUEH.StopUEH()
    End Sub

    Private Sub lnkSigIntRepository_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkSigIntRepository.LinkClicked
        Try
            Dim sUrl As String = "https://github.com/RichardLWolf/SigInt"
            Dim poProcessStart As New ProcessStartInfo(sUrl) With {
            .UseShellExecute = True  ' Required in .NET Core/.NET 5+ to open URLs
        }
            Process.Start(poProcessStart)
        Catch ex As Exception
            MsgBox("Failed to open link: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub



    Private Sub picAdd_Click(sender As Object, e As EventArgs) Handles picAdd.Click
        Dim poCfg As New DeviceConfig
        Using poFrm As New frmEditConfig
            poFrm.ReadyForm(poCfg)
            If poFrm.ShowDialog(Me) = DialogResult.OK Then
                poCfg = poFrm.ConfigValues
                foAppConfig.SetDeviceConfig(poCfg)
                LoadConfigs()
                ' relsect the one we just edited
                For piIndex As Integer = 0 To cboConfigs.Items.Count - 1
                    Dim poKvp As KeyValuePair(Of String, DeviceConfig) = DirectCast(cboConfigs.Items(piIndex), KeyValuePair(Of String, DeviceConfig))
                    If poKvp.Value.ConfigurationKey = poCfg.ConfigurationKey Then
                        cboConfigs.SelectedIndex = piIndex
                        Exit For
                    End If
                Next
            End If
        End Using
    End Sub

    Private Sub picAdd_MouseEnter(sender As Object, e As EventArgs) Handles picAdd.MouseEnter
        picAdd.Image = My.Resources.add2_blue
    End Sub

    Private Sub picAdd_MouseLeave(sender As Object, e As EventArgs) Handles picAdd.MouseLeave
        picAdd.Image = My.Resources.add2
    End Sub

    Private Sub picBrowseFolder_Click(sender As Object, e As EventArgs) Handles picBrowseFolder.Click, lblExplorer.Click
        Try
            Process.Start("explorer.exe", clsLogger.LogPath)
        Catch ex As Exception
            MsgBox("Failed to start windows process in folder " & clsLogger.LogPath, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Cannot Start Process")
        End Try
    End Sub

    Private Sub picBrowseFolder_MouseEnter(sender As Object, e As EventArgs) Handles picBrowseFolder.MouseEnter, lblExplorer.MouseEnter
        picBrowseFolder.Image = My.Resources.folder_blue
        lblExplorer.BackColor = Color.FromArgb(128, 100, 149, 237)
    End Sub

    Private Sub picBrowseFolder_MouseLeave(sender As Object, e As EventArgs) Handles picBrowseFolder.MouseLeave, lblExplorer.MouseLeave
        picBrowseFolder.Image = My.Resources.folder_green
        lblExplorer.BackColor = Color.Transparent
    End Sub

    Private Sub picDelete_Click(sender As Object, e As EventArgs) Handles picDelete.Click
        If cboConfigs.SelectedItem IsNot Nothing Then
            Dim poCfg As DeviceConfig = cboConfigs.SelectedItem.value
            If MsgBox($"Are you sure you wish to remove the {poCfg.ConfigurationName} configuration from the list?  This cannot be undone.{vbCrLf}Remove selected configuration?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Configuration Removal") = MsgBoxResult.Yes Then
                foAppConfig.RemoveDeviceConfig(poCfg.ConfigurationKey)
                LoadConfigs()
            End If
        Else
            MsgBox("Please select a configuration from the dropdown.")
            cboConfigs.Focus()
        End If
    End Sub

    Private Sub picDelete_MouseEnter(sender As Object, e As EventArgs) Handles picDelete.MouseEnter
        picDelete.Image = My.Resources.trash_blue
    End Sub

    Private Sub picDelete_MouseLeave(sender As Object, e As EventArgs) Handles picDelete.MouseLeave
        picDelete.Image = My.Resources.trash_red
    End Sub

    Private Sub picEdit_Click(sender As Object, e As EventArgs) Handles picEdit.Click
        If cboConfigs.SelectedItem IsNot Nothing Then
            Dim poCfg As DeviceConfig = DirectCast(cboConfigs.SelectedItem, KeyValuePair(Of String, DeviceConfig)).Value
            Using poFrm As New frmEditConfig
                poFrm.ReadyForm(poCfg)
                If poFrm.ShowDialog(Me) = DialogResult.OK Then
                    poCfg = poFrm.ConfigValues
                    foAppConfig.SetDeviceConfig(poCfg)
                    LoadConfigs()
                    ' relsect the one we just edited
                    For piIndex As Integer = 0 To cboConfigs.Items.Count - 1
                        Dim poKvp As KeyValuePair(Of String, DeviceConfig) = DirectCast(cboConfigs.Items(piIndex), KeyValuePair(Of String, DeviceConfig))
                        If poKvp.Value.ConfigurationKey = poCfg.ConfigurationKey Then
                            cboConfigs.SelectedIndex = piIndex
                            Exit For
                        End If
                    Next
                End If
            End Using
        Else
            MsgBox("Please select a configuration from the dropdown.")
            cboConfigs.Focus()
        End If
    End Sub

    Private Sub picEdit_MouseEnter(sender As Object, e As EventArgs) Handles picEdit.MouseEnter
        picEdit.Image = My.Resources.pencil2_blue
    End Sub

    Private Sub picEdit_MouseLeave(sender As Object, e As EventArgs) Handles picEdit.MouseLeave
        picEdit.Image = My.Resources.pencil2
    End Sub

    Private Sub picGenConfig_Click(sender As Object, e As EventArgs) Handles picGenConfig.Click, lblGenConfig.Click
        Using poFrm As New frmAppConfig
            poFrm.ReadyForm(foAppConfig)
            If poFrm.ShowDialog(Me) = DialogResult.OK Then
                foAppConfig.DiscordNotifications = poFrm.DiscordNotifications
                foAppConfig.DiscordMentionID = poFrm.DiscordMentionID
                foAppConfig.DiscordServerWebhook = poFrm.DiscordServerWebhook
                foAppConfig.ThingSpeakEnabled = poFrm.ThingSpeakEabled
                foAppConfig.UserLat = poFrm.UserLatitude
                foAppConfig.UserLon = poFrm.UserLongitude
                If String.IsNullOrEmpty(foAppConfig.UserGUID) Then
                    foAppConfig.UserGUID = Guid.NewGuid.ToString
                End If
                foAppConfig.Save()
            End If
        End Using
    End Sub

    Private Sub picGenConfig_MouseEnter(sender As Object, e As EventArgs) Handles picGenConfig.MouseEnter, lblGenConfig.MouseEnter
        picGenConfig.Image = My.Resources.gear_blue
        lblGenConfig.BackColor = Color.FromArgb(128, 100, 149, 237)
    End Sub

    Private Sub picGenConfig_MouseLeave(sender As Object, e As EventArgs) Handles picGenConfig.MouseLeave, lblGenConfig.MouseLeave
        picGenConfig.Image = My.Resources.gear
        lblGenConfig.BackColor = Color.Transparent
    End Sub


    Private Sub picPlayback_Click(sender As Object, e As EventArgs) Handles picPlayback.Click, lblPlayBack.Click
        For Each poForm As Form In Application.OpenForms.Cast(Of Form).ToList()
            If TypeOf (poForm) Is frmPlayback Then
                poForm.WindowState = FormWindowState.Normal
                poForm.Visible = True
                poForm.BringToFront()
                poForm.Focus()
                Exit Sub
            End If
        Next
        Dim poFrm As New frmPlayback
        poFrm.ReadyForm()
        poFrm.Show(Me)
    End Sub

    Private Sub picPlayback_MouseEnter(sender As Object, e As EventArgs) Handles picPlayback.MouseEnter, lblPlayBack.MouseEnter
        picPlayback.Image = My.Resources.microphone2_blue
        lblPlayBack.BackColor = Color.FromArgb(128, 100, 149, 237)
    End Sub

    Private Sub picPlayback_MouseLeave(sender As Object, e As EventArgs) Handles picPlayback.MouseLeave, lblPlayBack.MouseLeave
        picPlayback.Image = My.Resources.microphone2
        lblPlayBack.BackColor = Color.Transparent
    End Sub


    Private Async Sub picRefresh_Click(sender As Object, e As EventArgs) Handles picRefresh.Click
        Me.Cursor = Cursors.WaitCursor
        picRefresh.Image = My.Resources.refresh
        picRefresh.Enabled = False

        ' Run the device refresh in the background
        Await Task.Run(Sub() GetRtlSdrDeviceList())

        picRefresh.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub picRefresh_MouseEnter(sender As Object, e As EventArgs) Handles picRefresh.MouseEnter
        picRefresh.Image = My.Resources.refresh_blue
    End Sub

    Private Sub picRefresh_MouseLeave(sender As Object, e As EventArgs) Handles picRefresh.MouseLeave
        picRefresh.Image = My.Resources.refresh
    End Sub

    Private Sub picViewLog_Click(sender As Object, e As EventArgs) Handles picViewLog.Click, lblViewLog.Click
        Using poFrm As New frmViewLog
            poFrm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub picViewLog_MouseEnter(sender As Object, e As EventArgs) Handles picViewLog.MouseEnter, lblViewLog.MouseEnter
        picViewLog.Image = My.Resources.scroll_view_hover
        lblViewLog.BackColor = Color.FromArgb(128, 100, 149, 237)
    End Sub

    Private Sub picViewLog_MouseLeave(sender As Object, e As EventArgs) Handles picViewLog.MouseLeave, lblViewLog.MouseLeave
        picViewLog.Image = My.Resources.scroll_view
        lblViewLog.BackColor = Color.Transparent
    End Sub






    Private Sub LoadConfigs()
        With cboConfigs
            .Items.Clear()
            For Each poCfg As DeviceConfig In foAppConfig.DeviceSettings.Values
                .Items.Add(New KeyValuePair(Of String, DeviceConfig)(poCfg.ConfigurationName, poCfg))
            Next
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With
        If cboConfigs.Items.Count > 0 Then
            cboConfigs.SelectedIndex = 0
        End If
    End Sub

    Private Sub GetRtlSdrDeviceList()
        Dim poDevs As List(Of RtlSdrApi.SdrDevice) = RtlSdrApi.GetDevices()

        ' UI update must be invoked on the main thread
        If cboDeviceList.InvokeRequired Then
            cboDeviceList.Invoke(Sub() PopulateDeviceList(poDevs))
        Else
            PopulateDeviceList(poDevs)
        End If
    End Sub

    Private Sub PopulateDeviceList(poDevs As List(Of RtlSdrApi.SdrDevice))
        cboDeviceList.Items.Clear()
        If poDevs.Count = 0 Then
            cboDeviceList.Items.Add(New RtlSdrApi.SdrDevice("No SDR devices found.", -1))
        Else
            cboDeviceList.Items.AddRange(poDevs.Cast(Of Object).ToArray())
        End If
        cboDeviceList.SelectedIndex = 0
    End Sub

    Private Sub lnkThingSpeak_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkThingSpeak.LinkClicked
        Dim psURL As String = "https://thingspeak.mathworks.com/channels/2869584"
        Try
            Process.Start(New ProcessStartInfo With {
                .FileName = psURL,
                .UseShellExecute = True
            })
        Catch ex As Exception
            MessageBox.Show($"Failed to launch process, please visit the site below via your Internet browser:{vbCrLf}{vbCrLf}{psURL}", "Failed To Start Process", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
End Class