Imports System.IO
Imports Newtonsoft.Json

Public Class clsAppConfig
    ' Configuration properties (only these get serialized)
    '    Device parameters
    Public Property CenterFrequency As UInteger = 1600000000UI ' Default: 1.6 GHz
    Public Property SampleRate As Integer = 2048000 ' Default: 2.048 MSPS
    Public Property GainMode As Integer = 0     ' 0=auto, 1=manual
    Public Property GainValue As Integer = 300  ' 30.0dB
    Public Property MinEventWindow As Double = 10D  ' 10 mins between signals
    Public Property DetectionThreshold As Integer = 15 ' Default: 15 dB
    Public Property DetectionWindow As Integer = 1 ' 3 FFT bins to average for signal detection
    '   UI Preferences
    Public Property ZoomLevel As Integer = 0    ' Default: Full view (0 to 100)
    Public Property dBOffset As Integer = -20   ' Default: -20 dB  (0 to -100)
    Public Property dBRange As Integer = 100    ' Default: 100 dB graphed (10-150)
    '  Discord Notifications
    Public Property DiscordNotifications As Boolean = False
    Public Property DiscordServerWebhook As String = ""
    Public Property DiscordMentionID As String = ""



    ' Define config file path as a Shared (Static) constant
    Private Shared ReadOnly msConfigFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), My.Application.Info.AssemblyName, "config.json")

    Public Shared Function Load() As clsAppConfig
        If Not File.Exists(msConfigFile) Then
            Return New clsAppConfig() ' Return defaults if no file exists
        End If

        Try
            Dim psJson As String = File.ReadAllText(msConfigFile)
            Return JsonConvert.DeserializeObject(Of clsAppConfig)(psJson)

        Catch ex As Exception
            clsLogger.LogException("clsAppConfig.Load", ex)
            Return New clsAppConfig() ' Return defaults if error
        End Try
    End Function

    Public Sub Save()
        Try
            If ValidateConfigFolder() Then
                Dim psJson As String = JsonConvert.SerializeObject(Me, Formatting.Indented)
                File.WriteAllText(msConfigFile, psJson)
            End If

        Catch ex As Exception
            clsLogger.LogException("clsAppConfig.Save", ex)
        End Try
    End Sub

    Public Sub ResetToDefaults()
        Dim poDefaultConfig As New clsAppConfig()
        Me.CenterFrequency = poDefaultConfig.CenterFrequency
        Me.SampleRate = poDefaultConfig.SampleRate
        Me.GainMode = poDefaultConfig.GainMode
        Me.GainValue = poDefaultConfig.GainValue
        Me.MinEventWindow = poDefaultConfig.MinEventWindow
        Me.ZoomLevel = poDefaultConfig.ZoomLevel
        Me.dBOffset = poDefaultConfig.dBOffset
        Me.dBRange = poDefaultConfig.dBRange
        Me.DiscordNotifications = poDefaultConfig.DiscordNotifications
        Me.DiscordServerWebhook = poDefaultConfig.DiscordServerWebhook
        Me.DiscordMentionID = poDefaultConfig.DiscordMentionID
        Me.DetectionThreshold = poDefaultConfig.DetectionThreshold
        Me.DetectionWindow = poDefaultConfig.DetectionWindow
        Save()
    End Sub

    Public Shared Function ValidateConfigFolder() As Boolean
        Try
            Dim psFolder As String = System.IO.Path.GetDirectoryName(msConfigFile)
            If System.IO.Directory.Exists(psFolder) Then
                Return True
            Else
                System.IO.Directory.CreateDirectory(psFolder)
                Return True
            End If

        Catch ex As Exception
            clsLogger.LogException("clsAppConfig.ValidateConfigFile", ex)
            Return False
        End Try
    End Function

End Class
