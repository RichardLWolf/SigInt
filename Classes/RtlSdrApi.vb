Imports System.Runtime.InteropServices
Imports System.IO
Imports System.IO.Compression
Imports System.Threading

Public Class RtlSdrApi

#Region " rtlsdr.dll imports  "
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_device_count() As UInteger
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_device_name(index As UInteger) As IntPtr
    End Function

    ' Open the device
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_open(ByRef dev As IntPtr, index As UInt32) As Integer
    End Function

    ' Close the device
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_close(dev As IntPtr) As Integer
    End Function

    ' Set center frequency
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_center_freq(dev As IntPtr, freq As UInt32) As Integer
    End Function

    ' Get center frequency
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_center_freq(dev As IntPtr) As UInt32
    End Function

    ' Set sample rate
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_sample_rate(dev As IntPtr, rate As UInt32) As Integer
    End Function

    ' Read synchronous IQ data
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_read_sync(dev As IntPtr, buf As Byte(), buf_len As Integer, ByRef n_read As Integer) As Integer
    End Function

    ' Declare the existing methods if not already defined
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_open(ByRef dev As IntPtr, index As Integer) As Integer
    End Function

    ' 1️⃣ Reset buffer (flushes stale data before starting capture)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_reset_buffer(dev As IntPtr) As Integer
    End Function

    ' 2️⃣ Set tuner gain mode (0 = auto, 1 = manual)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_tuner_gain_mode(dev As IntPtr, manualGain As Integer) As Integer
    End Function

    ' 3️⃣ Set tuner gain (valid range: ~0 to 500)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_tuner_gain(dev As IntPtr, gain As Integer) As Integer
    End Function
#End Region


    Public Structure SdrDevice
        Public DeviceName As String
        Public DeviceIndex As Integer

        Public Sub New(DevName As String, DevIndex As Integer)
            Me.DeviceName = DevName
            Me.DeviceIndex = DevIndex
        End Sub

        Public Overrides Function ToString() As String
            Return DeviceName
        End Function
    End Structure

    ' Event for UI-safe error reporting
    Public Event ErrorOccurred(ByVal sender As Object, ByVal message As String)
    Public Event SignalChange(ByVal sender As Object, ByVal SignalFound As Boolean)


    Private pDeviceHandle As IntPtr = IntPtr.Zero
    Private pCenterFrequency As UInteger
    Private pDeviceIndex As Integer
    Private msLogFolder As String = "C:\"
    Private pMonitorThread As Thread
    Private pRunning As Boolean = False
    Private pSyncContext As SynchronizationContext
    Private miSignalEvents As Integer = 0

    ' Circular buffer for pre-trigger storage
    Private pIqQueue As New Queue(Of Byte())()
    Private pQueueMaxSize As Integer = 10 ' Adjust for X seconds of pre-buffering
    ' Signal detection parameters
    Private pPowerThreshold As Double = 5000 ' Adjust as needed
    Private pSignalDetected As Boolean = False
    ' Recording limit
    Private pRecordingStartTime As Date
    Private pMaxRecordingTime As Integer = 60 ' Max seconds to record per event
    ' File writing and recording state
    Private pRecordingBuffer As New List(Of Byte()) ' Holds IQ data during an event
    Private pSampleRate As Integer = 2048000 ' SDR sample rate (2.048 MSPS)

    Private mtStartMonitor As Date = DateTime.Now

    ' Buffer for UI visualization
    Private pIqBuffer() As Byte
    Private pBufferSize As Integer = 16384 ' Standard RTL-SDR buffer size



    Public ReadOnly Property IsRunning As Boolean
        Get
            Return pRunning
        End Get
    End Property

    Public ReadOnly Property IsRecording As Boolean
        Get
            Return pSignalDetected
        End Get
    End Property

    Public ReadOnly Property SignalEventCount As Integer
        Get
            Return miSignalEvents
        End Get
    End Property

    Public Property PowerThreshold As Double
        Get
            Return pPowerThreshold
        End Get
        Set(value As Double)
            pPowerThreshold = value
        End Set
    End Property

    ' Public method to retrieve the latest buffer (for UI visualization)
    Public Function GetBuffer() As Byte()
        SyncLock pIqBuffer
            Return If(pIqBuffer IsNot Nothing, CType(pIqBuffer.Clone(), Byte()), Nothing)
        End SyncLock
    End Function



    Public Sub New(centerFrequency As UInteger, deviceIndex As Integer)
        pCenterFrequency = centerFrequency
        pDeviceIndex = deviceIndex
        pSyncContext = SynchronizationContext.Current ' Capture UI thread context
        msLogFolder = clsLogger.LogPath
    End Sub

    Public Sub StartMonitor()
        If pRunning Then Exit Sub ' Prevent duplicate threads

        ReDim pIqBuffer(pBufferSize - 1) ' Initialize buffer
        pSignalDetected = False

        pMonitorThread = New Thread(AddressOf MonitorThread)
        pMonitorThread.IsBackground = True
        pRunning = True
        pMonitorThread.Start()
    End Sub

    ' Stop monitoring
    Public Sub StopMonitor()
        pRunning = False
        If pMonitorThread IsNot Nothing AndAlso pMonitorThread.IsAlive Then
            pMonitorThread.Join(500) ' Wait for thread to exit cleanly
        End If
    End Sub

    ' Background thread for monitoring SDR data
    Private Sub MonitorThread()
        Try
            ' Open the RTL-SDR device
            Dim result As Integer = RtlSdrApi.rtlsdr_open(pDeviceHandle, CUInt(pDeviceIndex))
            If result <> 0 Then
                clsLogger.Log("RtlSdrApi.MonitorThread", "Failed to open RTL-SDR device, result code was " & result & ".")
                RaiseError("Failed to open RTL-SDR device.")
                Exit Sub
            End If

            ' Reset buffer (important to clear any stale data)
            result = rtlsdr_reset_buffer(pDeviceHandle)

            ' Set sample rate (e.g., 2.048 MSPS)
            result = RtlSdrApi.rtlsdr_set_sample_rate(pDeviceHandle, 2048000)

            ' Set center frequency (e.g., 1.6 GHz)
            result = RtlSdrApi.rtlsdr_set_center_freq(pDeviceHandle, 1600000000)

            ' Enable manual gain mode (1 = manual, 0 = auto)
            result = RtlSdrApi.rtlsdr_set_tuner_gain_mode(pDeviceHandle, 1)

            ' Set gain manually (try values between 100-500)
            result = RtlSdrApi.rtlsdr_set_tuner_gain(pDeviceHandle, 300)

            ' Set center frequency
            RtlSdrApi.rtlsdr_set_center_freq(pDeviceHandle, pCenterFrequency)

            mtStartMonitor = DateTime.Now
            miSignalEvents = 0
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Starting monitor IQ data stream {mtStartMonitor:MM/dd/yyyy HH:mm:ss}.")

            ' Start streaming IQ data
            Dim bytesRead As Integer = 0

            While pRunning
                Dim tempBuffer(pBufferSize - 1) As Byte ' Temp buffer for this read
                result = RtlSdrApi.rtlsdr_read_sync(pDeviceHandle, tempBuffer, pBufferSize, bytesRead)
                If result <> 0 Then
                    RaiseError("Error reading IQ samples.")
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"Error reading IQ data buffer, result code was {result}, stopping thread.")
                    Exit While
                End If

                ' Store in UI buffer
                SyncLock pIqBuffer
                    Array.Copy(tempBuffer, pIqBuffer, pBufferSize)
                End SyncLock

                ' Store in circular queue
                SyncLock pIqQueue
                    pIqQueue.Enqueue(tempBuffer)
                    If pIqQueue.Count > pQueueMaxSize Then
                        pIqQueue.Dequeue() ' Remove oldest buffer
                    End If
                End SyncLock

                ' Analyze signal power
                Dim power As Double = CalculateSignalPower(tempBuffer)

                ' Check if power exceeds threshold
                If power > pPowerThreshold Then
                    If Not pSignalDetected Then
                        pSignalDetected = True
                        pRecordingStartTime = Date.Now
                        miSignalEvents = miSignalEvents + 1
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"🔹 Signal Detected! Power: {power}.")
                        ' Save pre-buffered IQ data
                        SyncLock pIqQueue
                            pRecordingBuffer.AddRange(pIqQueue.ToList())
                            pIqQueue.Clear() ' Reset queue
                        End SyncLock
                        RaiseSignalChange(True)
                    End If
                    ' check of max time
                    If (DateTime.Now - pRecordingStartTime).TotalSeconds > pMaxRecordingTime Then
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"⏳ Max recording time reached. Stopping recording.")
                        StopRecording()
                    Else
                        ' Save new IQ data
                        SyncLock pRecordingBuffer
                            pRecordingBuffer.Add(tempBuffer)
                        End SyncLock
                    End If
                Else
                    If pSignalDetected Then
                        pSignalDetected = False
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"🔻 Signal Lost! Power: {power}.")
                        StopRecording()
                    End If
                End If
            End While
            Dim ptEnd As Date = DateTime.Now
            Dim poElapsed As TimeSpan = ptEnd.Subtract(mtStartMonitor)
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Monitor thread ending at {ptEnd:MM/dd/yyyy HH:mm:ss} with {IIf(miSignalEvents = 0, "no", miSignalEvents)} signal event{IIf(miSignalEvents > 1, "s", "")}, {modMain.FullDisplayElapsed(poElapsed.TotalSeconds)}")

        Catch ex As Exception
            clsLogger.LogException("rtlSdrApi.MonitorThread", ex)

        Finally
            ' Cleanup SDR device
            If pDeviceHandle <> IntPtr.Zero Then
                RtlSdrApi.rtlsdr_close(pDeviceHandle)
                pDeviceHandle = IntPtr.Zero
            End If
            pRunning = False
            If pSignalDetected Then
                RaiseSignalChange(False)
            End If
            pSignalDetected = False
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Monitor thread ended.")
        End Try
    End Sub

    Private Sub StopRecording()
        pSignalDetected = False
        SaveIqDataToZip()
        RaiseSignalChange(False)
    End Sub

    Private Sub SaveIqDataToZip()
        Try
            ' Calculate pre-buffer time in seconds
            Dim samplesPerBuffer As Integer = pBufferSize \ 2 ' IQ samples per buffer
            Dim timePerBuffer As Double = samplesPerBuffer / pSampleRate
            Dim preBufferTime As Double = timePerBuffer * pQueueMaxSize

            ' Adjust the timestamp for accurate event timing

            Dim adjustedStartTime As DateTime = pRecordingStartTime.AddSeconds(-preBufferTime)
            Dim adjustedStartTimeUtc As DateTime = adjustedStartTime.ToUniversalTime()

            ' Generate timestamped filename with adjusted start time
            Dim timestamp As String = adjustedStartTimeUtc.ToString("yyyyMMdd_HHmmss.fff") & "Z"
            Dim outputFile As String = System.IO.Path.Combine(msLogFolder, $"IQ_Record_{timestamp}.zip")

            Using fs As New FileStream(outputFile, FileMode.Create)
                Using zip As New ZipArchive(fs, ZipArchiveMode.Create)
                    Dim iqentry As ZipArchiveEntry = zip.CreateEntry("signal.iq", CompressionLevel.Optimal)

                    Using entryStream As Stream = iqentry.Open()
                        SyncLock pRecordingBuffer
                            For Each chunk In pRecordingBuffer
                                entryStream.Write(chunk, 0, chunk.Length)
                            Next
                        End SyncLock
                    End Using
                    ' Add a Metadata File with recording information
                    Dim metadataEntry As ZipArchiveEntry = zip.CreateEntry("metadata.json", CompressionLevel.Optimal)
                    Dim appVersion As String = $"{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}"
                    Using metadataStream As Stream = metadataEntry.Open()
                        Using writer As New StreamWriter(metadataStream)
                            ' Create metadata content
                            Dim metadata As String = $"
{{
    ""UTC_Start_Time"": ""{adjustedStartTimeUtc:yyyy-MM-ddTHH:mm:ss.fffZ}"",
    ""Center_Frequency_Hz"": {pCenterFrequency},
    ""Sample_Rate_Hz"": {pSampleRate},
    ""Total_IQ_Bytes"": {pRecordingBuffer.Sum(Function(b) b.Length)},
    ""Recording_Duration_S"": {(DateTime.Now - pRecordingStartTime).TotalSeconds:F2},
    ""Power_Threshold"": {pPowerThreshold},
    ""Software_Version"": ""{appVersion}""
}}"
                            writer.Write(metadata)
                        End Using
                    End Using
                End Using
            End Using
            clsLogger.Log("RtlSdrApi.SaveIqDataToZip", $"✅ Saved to {outputFile} (Start: {adjustedStartTime})")

        Catch ex As Exception
            clsLogger.LogException("RtlSdrAPI.SaveIqDataToZip", ex)
            RaiseError("Error saving IQ data: " & ex.Message)

        Finally
            ' Clear recording buffer
            SyncLock pRecordingBuffer
                pRecordingBuffer.Clear()
            End SyncLock
        End Try
    End Sub


    Private Function CalculateSignalPower(buffer As Byte()) As Double
        Dim power As Double = 0
        For i As Integer = 0 To buffer.Length - 2 Step 2
            Dim piInPhase As Double = buffer(i) - 127 ' Convert byte to signed value
            Dim piQuad As Double = buffer(i + 1) - 127
            power += (piInPhase * piInPhase + piQuad * piQuad) ' Compute magnitude
        Next
        Return power / (buffer.Length / 2) ' Normalize
    End Function

    ' Retrieve stored pre-trigger data (for recording)
    Public Function GetPreTriggerData() As List(Of Byte())
        SyncLock pIqQueue
            Return pIqQueue.ToList()
        End SyncLock
    End Function

    ' Raises an error event on the UI thread
    Private Sub RaiseError(message As String)
        If pSyncContext IsNot Nothing Then
            pSyncContext.Post(Sub(state) RaiseEvent ErrorOccurred(Me, message), Nothing)
        Else
            RaiseEvent ErrorOccurred(Me, message)
        End If
        StopMonitor() ' Ensure the thread is stopped on error
    End Sub


    ' Raises an error event on the UI thread
    Private Sub RaiseSignalChange(ByVal SignalFound As Boolean)
        If pSyncContext IsNot Nothing Then
            pSyncContext.Post(Sub(state) RaiseEvent SignalChange(Me, SignalFound), Nothing)
        Else
            RaiseEvent SignalChange(Me, SignalFound)
        End If
    End Sub












    Public Shared Function GetDevices() As List(Of SdrDevice)
        Dim poDevices As New List(Of SdrDevice)
        Try
            Dim deviceCount As UInteger = RtlSdrApi.rtlsdr_get_device_count()
            If deviceCount > 0 Then
                For i As UInteger = 0 To deviceCount - 1
                    Dim namePtr As IntPtr = RtlSdrApi.rtlsdr_get_device_name(i)
                    Dim deviceName As String = Marshal.PtrToStringAnsi(namePtr)
                    poDevices.Add(New SdrDevice(deviceName, i))
                Next
            End If
        Catch ex As Exception
            Debug.WriteLine("RTE getting SDR device list")
        End Try

        Return poDevices
    End Function
End Class
