Imports System.Text
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json.Linq
Imports System.Threading.Tasks

Public Class clsThingSpeakAPI
    Private Shared ReadOnly moHttpClient As New HttpClient()
    Private Const msEncodedWriteKey As String = "XUVE@zNUFHNkhI#UEM2#MlNGNw==Z"   ' Obfuscated Write Key
    Private Const msEncodedReadKey As String = "YR1VKWjV@HRTBN#NElL$RTBIWQ==Y"    ' Obfuscated Read Key
    Private Const msChannelID As String = "2869584" ' ThingSpeak Channel ID

    Private msLatLon As String = ""
    Private msUserGUID As String = ""
    Private mdLastThingSpeakLog As DateTime = DateTime.MinValue

    ' Event Type Enum
    Public Enum EventTypeEnum
        SignalDetected = 1
        NoEvent = 0
    End Enum


    Public ReadOnly Property LastLogWrite As DateTime
        Get
            Return mdLastThingSpeakLog
        End Get
    End Property

    Public Sub New(ByVal psLatLon As String, ByVal psUserGUID As String)
        msLatLon = psLatLon
        msUserGUID = psUserGUID
    End Sub

    ' Log an Event (Signal or NoEvent)
    Public Async Function LogEventAsync(ByVal iEventType As EventTypeEnum, Optional ByVal iFrequencyHz As Integer = 0 _
                                        , Optional ByVal iSampleRate As Integer = 0, Optional ByVal dNoiseFloor As Double = 0 _
                                        , Optional ByVal iDurationSeconds As Integer = 0) As Task
        ' Validate Frequency for Signal Events
        If iEventType = EventTypeEnum.SignalDetected AndAlso Not IsValidFrequency(iFrequencyHz) Then
            clsLogger.Log("clsThingSpeakAPI.LogEventAsync", "Discarding " & iFrequencyHz & " Hz event, not in valid range.")
            Exit Function
        End If
        ' Validate for too soon since last log
        If DateTime.Now.Subtract(mdLastThingSpeakLog).TotalSeconds < 15D Then
            clsLogger.Log("clsThingSpeakAPI.LogEventAsync", $"Discarding {iFrequencyHz} Hz event, too soon since last log upload ({DateTime.Now.Subtract(mdLastThingSpeakLog).TotalSeconds} seconds).")
            Exit Function
        End If

        ' Format Data for ThingSpeak
        Dim psData As String = "field1=" & DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") &
                               "&field2=" & msUserGUID &
                               "&field3=" & msLatLon &
                               "&field4=" & iFrequencyHz &
                               "&field5=" & iSampleRate &
                               "&field6=" & dNoiseFloor &
                               "&field7=" & iDurationSeconds &
                               "&field8=" & CInt(iEventType)

        ' Send to ThingSpeak
        Await SendToThingSpeakAsync(psData)
        clsLogger.Log("clsThingSpeakAPI.LogEventAsync", $"Sent {iFrequencyHz} Hz {iEventType} event to ThingSpeak.")

    End Function

    ' Retrieve DataSet of Signal Events (Filtered by UserGUID & Date Range)
    Public Async Function GetDataSetAsync(Optional ByVal psUserGUID As String = "", Optional ByVal pdStartDate As Date = Nothing _
                                          , Optional ByVal pdEndDate As Date = Nothing) As Task(Of DataSet)
        Try
            Dim psURL As String = "https://api.thingspeak.com/channels/" & msChannelID & "/feeds.json?api_key=" & GetReadKey() & "&results=8000"
            Dim psJson As String = Await moHttpClient.GetStringAsync(psURL)
            Dim poData As JObject = JObject.Parse(psJson)

            ' Create DataTable
            Dim poTable As New DataTable("ThingSpeakData")
            poTable.Columns.Add("UTCTimestamp", GetType(DateTime))
            poTable.Columns.Add("UserGUID", GetType(String))
            poTable.Columns.Add("LatLon", GetType(String))
            poTable.Columns.Add("Frequency", GetType(Integer))
            poTable.Columns.Add("SampleRate", GetType(Integer))
            poTable.Columns.Add("NoiseFloor", GetType(Double))
            poTable.Columns.Add("DurationSeconds", GetType(Integer))
            poTable.Columns.Add("EventType", GetType(Integer))

            ' Parse JSON and add to DataTable
            For Each poItem As JObject In poData("feeds")
                Dim psTimestamp As String = poItem("created_at").ToString()
                Dim psGUID As String = poItem("field2").ToString()
                Dim pdEventTime As DateTime = DateTime.Parse(psTimestamp)

                ' Apply filters
                If (psUserGUID = "" OrElse psUserGUID = psGUID) AndAlso
                   (pdStartDate = Nothing OrElse pdEventTime >= pdStartDate) AndAlso
                   (pdEndDate = Nothing OrElse pdEventTime <= pdEndDate) Then

                    poTable.Rows.Add(pdEventTime, psGUID, poItem("field3").ToString(),
                                     CInt(poItem("field4")), CInt(poItem("field5")),
                                     CDbl(poItem("field6")), CInt(poItem("field7")),
                                     CInt(poItem("field8")))
                End If
            Next

            ' Create DataSet and return
            Dim poDataSet As New DataSet()
            poDataSet.Tables.Add(poTable)
            Return poDataSet
        Catch ex As Exception
            clsLogger.LogException("clsThingSpeakAPI.GetDataSetAsync", ex)
            Return Nothing
        End Try
    End Function

    ' Send Data to ThingSpeak using HttpClient
    Private Async Function SendToThingSpeakAsync(psData As String) As Task(Of Boolean)
        Try
            Dim psURL As String = "https://api.thingspeak.com/update?api_key=" & GetWriteKey() & "&" & psData
            Dim psResponse As String = Await moHttpClient.GetStringAsync(psURL)
            If Not String.IsNullOrEmpty(psResponse) AndAlso IsNumeric(psResponse) AndAlso CInt(psResponse) > 0 Then
                mdLastThingSpeakLog = DateTime.Now
                Return True
            Else
                clsLogger.Log("clsThingSpeakAPI.SendToThingSpeakAsync", "ThingSpeak upload failed: Unexpected response '" & psResponse & "'")
                Return False
            End If
        Catch ex As Exception
            clsLogger.LogException("clsThingSpeakAPI.SendToThingSpeakAsync", ex)
            Return False
        End Try
    End Function

    ' Decode the obfuscated Base64 key
    Private Function DecodeObfuscatedKey(psObfuscated As String) As String
        Try
            ' Remove the added obfuscation characters (@, $, #, etc.)
            Dim psCleanBase64 As String = psObfuscated.Substring(1, psObfuscated.Length - 2).Replace("@", "").Replace("$", "").Replace("#", "").Trim()

            ' Decode the Base64 string
            Dim psDecoded As String = Encoding.UTF8.GetString(Convert.FromBase64String(psCleanBase64))

            Return psDecoded
        Catch ex As Exception
            clsLogger.LogException("clsThingSpeakAPI.DecodeObfuscatedKey", ex)
            Return String.Empty ' Return empty string if decoding fails
        End Try
    End Function

    ' Get API Keys
    Private Function GetWriteKey() As String
        Return DecodeObfuscatedKey(msEncodedWriteKey)
    End Function
    Private Function GetReadKey() As String
        Return DecodeObfuscatedKey(msEncodedReadKey)
    End Function

    ' Validate Frequency
    Private Function IsValidFrequency(piFrequency As Integer) As Boolean
        Return (piFrequency >= 3000000 AndAlso piFrequency <= 30000000) OrElse  ' HF
               (piFrequency >= 100000000 AndAlso piFrequency <= 400000000) OrElse  ' VHF
               (piFrequency >= 1500000000 AndAlso piFrequency <= 1700000000)   ' L-Band
    End Function


End Class
