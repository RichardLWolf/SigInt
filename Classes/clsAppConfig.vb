Imports System.IO
Imports Newtonsoft.Json

Public Class clsAppConfig
    ' Configuration properties (only these get serialized)
    '    Device parameters
    Public Property CenterFrequency As UInteger = 1600000000UI ' Default: 1.6 GHz
    Public Property SampleRate As Integer = 2048000 ' Default: 2.048 MSPS
    Public Property GainMode As Integer = 1     ' 0=auto, 1=manual
    Public Property GainValue As Integer = 166  ' 16.6dB
    '  Detection parameters
    Public Property SignalEventResetTime As Integer = 600       ' 10 mins between signals (1 to 3600)
    Public Property SignalDetectionThreshold As Integer = 15    ' Default: 15 dB signal spike
    Public Property SignalDetectionWindow As Integer = 1        ' 3 FFT bins to average for signal detection
    Public Property SignalInitTime As Integer = 3               ' Seconds to delay before looking for signal spike (seconds, 1 to 10)              
    Public Property NoiseFloorBaselineInitTime As Integer = 60  ' Time to establish baseline (seconds, 10 to 120)
    Public Property NoiseFloorThreshold As Double = 4.0         ' dB rise to trigger event (2dB to 8dB)
    Public Property NoiseFloorMinEventDuration As Integer = 5   ' Seconds the rise must sustain (2 sec to 15 sec)
    Public Property NoiseFloorCooldownDuration As Integer = 10  ' Seconds to pause averaging after event (5 sec to 30 sec)
    Public Property NoiseFloorEventResetTime As Integer = 30    ' Quiet time before new event  (seconds, 10 to 60)
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
        Me.ZoomLevel = poDefaultConfig.ZoomLevel
        Me.dBOffset = poDefaultConfig.dBOffset
        Me.dBRange = poDefaultConfig.dBRange
        Me.DiscordNotifications = poDefaultConfig.DiscordNotifications
        Me.DiscordServerWebhook = poDefaultConfig.DiscordServerWebhook
        Me.DiscordMentionID = poDefaultConfig.DiscordMentionID
        Me.SignalEventResetTime = poDefaultConfig.SignalEventResetTime
        Me.SignalDetectionThreshold = poDefaultConfig.SignalDetectionThreshold
        Me.SignalDetectionWindow = poDefaultConfig.SignalDetectionWindow
        Me.SignalInitTime = poDefaultConfig.SignalInitTime
        Me.NoiseFloorThreshold = poDefaultConfig.NoiseFloorThreshold
        Me.NoiseFloorMinEventDuration = poDefaultConfig.NoiseFloorMinEventDuration
        Me.NoiseFloorCooldownDuration = poDefaultConfig.NoiseFloorCooldownDuration
        Me.NoiseFloorEventResetTime = poDefaultConfig.NoiseFloorEventResetTime
        Me.NoiseFloorBaselineInitTime = poDefaultConfig.NoiseFloorBaselineInitTime

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
