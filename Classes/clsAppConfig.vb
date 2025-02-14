Imports System.IO
Imports Newtonsoft.Json

Public Class clsAppConfig
    ' Configuration properties (only these get serialized)
    Public Property CenterFrequency As UInteger = 1600000000UI ' Default: 1.6 GHz
    Public Property SampleRate As Integer = 2048000 ' Default: 2.048 MSPS
    Public Property ZoomLevel As Integer = 0 ' Default: Full view (0 to 100)
    Public Property dBOffset As Integer = -20 ' Default: -20 dB  (0 to -100)
    Public Property dBRange As Integer = 100 ' Default: 100 dB graphed (10-150)

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
        Me.ZoomLevel = poDefaultConfig.ZoomLevel
        Me.dBOffset = poDefaultConfig.dBOffset
        Me.dBRange = poDefaultConfig.dBRange
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
