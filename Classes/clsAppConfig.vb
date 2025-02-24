Imports System.IO
Imports System.Threading
Imports Newtonsoft.Json

Public Class clsAppConfig
    ' Define config file path as a Shared (Static) constant
    Private Shared ReadOnly msConfigFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), My.Application.Info.AssemblyName, "config.json")


    Public Property DeviceSettings As Dictionary(Of String, DeviceConfig) = New Dictionary(Of String, DeviceConfig)
    '  Discord Notifications
    Public Property DiscordNotifications As Boolean = False
    Public Property DiscordServerWebhook As String = ""
    Public Property DiscordMentionID As String = ""



    Public Shared Function Load() As clsAppConfig
        If Not File.Exists(msConfigFile) Then
            Return MakeNewInstance()
        End If

        Try
            Dim psJson As String = File.ReadAllText(msConfigFile)
            Return JsonConvert.DeserializeObject(Of clsAppConfig)(psJson)

        Catch ex As Exception
            clsLogger.LogException("clsAppConfig.Load", ex)
            Return MakeNewInstance()
        End Try
    End Function


    ' Return conifg data for a specific configuration
    Public Function GetDeviceConfig(ByVal sConfigId As String) As DeviceConfig
        If DeviceSettings.ContainsKey(sConfigId) Then
            Return DeviceSettings(sConfigId)
        Else
            Dim newConfig As New DeviceConfig()
            DeviceSettings.Add(sConfigId, newConfig)
            Return newConfig
        End If
    End Function

    ' Update or add configuration data for a specific configuration
    Public Sub SetDeviceConfig(ByVal oConfig As DeviceConfig)
        If DeviceSettings.ContainsKey(oConfig.ConfigurationKey) Then
            ' Update existing configuration
            DeviceSettings(oConfig.ConfigurationKey) = oConfig
        Else
            ' Add new configuration
            DeviceSettings.Add(oConfig.ConfigurationKey, oConfig)
        End If
        Save()
    End Sub

    Public Sub RemoveDeviceConfig(ByVal ConfigID As String)
        If DeviceSettings.ContainsKey(ConfigID) Then
            ' Update existing configuration
            DeviceSettings.Remove(ConfigID)
            If DeviceSettings.Count = 0 Then
                ' make sure at least one entry
                SetDeviceConfig(New DeviceConfig)
            End If
        End If
        Save()
    End Sub

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

    Public Sub ResetToDefaults(ByVal ConfigID As String)
        If DeviceSettings.ContainsKey(ConfigID) Then
            Dim poCurrCfg As DeviceConfig = Me.GetDeviceConfig(ConfigID)
            Dim poDefaultConfig As New DeviceConfig()
            poDefaultConfig.ConfigurationName = poCurrCfg.ConfigurationName
            poDefaultConfig.ConfigurationKey = poCurrCfg.ConfigurationKey
            ' update internal
            Me.SetDeviceConfig(poDefaultConfig)
        End If
        ' save changes
        Save()
    End Sub

    Private Shared Function MakeNewInstance() As clsAppConfig
        Dim poNewCfg As New clsAppConfig()
        Dim poCfg As New DeviceConfig
        poCfg.ConfigurationName = "1.6GHz Monitor"
        poNewCfg.SetDeviceConfig(poCfg)
        Return poNewCfg
    End Function

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

''' <summary>
''' Class to hold individual configuration settings
''' </summary>
Public Class DeviceConfig
    '    Driver supplied properties
    Public Property ConfigurationKey As String = Guid.NewGuid.ToString
    Public Property ConfigurationName As String = "New Configuration"
    '    Device parameters
    Public Property CenterFrequency As UInteger = 1600000000UI ' Default: 1.6 GHz
    Public Property SampleRate As Integer = 2048000 ' Default: 2.048 MSPS
    Public Property GainMode As Integer = 1     ' 0=auto, 1=manual
    Public Property GainValue As Integer = 166  ' 16.6dB
    '  Detection parameters
    Public Property SignalEventResetTime As Integer = 300       ' 5 mins between signal events (1 to 3600)
    Public Property SignalDetectionThreshold As Integer = 15    ' Default: 15 dB signal spike
    Public Property SignalDetectionWindow As Integer = 1        ' 3 FFT bins to average for signal detection
    Public Property SignalInitTime As Integer = 3               ' Seconds to delay before looking for signal spike (seconds, 1 to 10)              
    Public Property NoiseFloorBaselineInitTime As Integer = 60  ' Time to establish baseline (seconds, 10 to 120)
    Public Property NoiseFloorThreshold As Double = 4.0         ' dB rise to trigger event (2dB to 8dB)
    Public Property NoiseFloorMinEventDuration As Integer = 5   ' Seconds the rise must sustain (2 sec to 15 sec)
    Public Property NoiseFloorCooldownDuration As Integer = 10  ' Seconds to pause averaging after event (5 sec to 30 sec)
    Public Property NoiseFloorEventResetTime As Integer = 300   ' Quiet time before new event  (seconds, 10 to 60)
    '   UI Preferences
    Public Property ZoomLevel As Integer = 0    ' Default: Full view (0 to 100)
    Public Property dBOffset As Integer = -20   ' Default: -20 dB  (0 to -100)
    Public Property dBRange As Integer = 100    ' Default: 100 dB graphed (10-150)
    Public Property MaxRollingBufferSize As Integer = 200       ' For rolling buffer display ~7 seconds of buffer Default at ~50 FPS, not configurable at this time

    Public Function Clone() As DeviceConfig
        ' return a new shallow copy of this class
        Return DirectCast(Me.MemberwiseClone(), DeviceConfig)
    End Function
End Class


'