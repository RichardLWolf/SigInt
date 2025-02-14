Imports System.Runtime.InteropServices
Imports System.IO
Imports System.IO.Compression
Imports System.Threading
Imports MathNet.Numerics.IntegralTransforms
Imports System.Numerics

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

    ' Reset buffer (flushes stale data before starting capture)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_reset_buffer(dev As IntPtr) As Integer
    End Function

    ' Set tuner gain mode (0 = auto, 1 = manual)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_tuner_gain_mode(dev As IntPtr, manualGain As Integer) As Integer
    End Function

    ' Set tuner gain (valid range: ~0 to 500)
    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_set_tuner_gain(dev As IntPtr, gain As Integer) As Integer
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_sample_rates(ByVal device_index As Integer, ByRef num_rates As Integer, ByVal sample_rates() As Integer) As Integer
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_freq_range(ByVal device_index As Integer, ByRef min_freq As Integer, ByRef max_freq As Integer) As Integer
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_tuner_gains(ByVal device_index As Integer, ByRef num_gains As Integer, ByVal gains() As Integer) As Integer
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


    Private miDeviceHandle As IntPtr = IntPtr.Zero
    Private miCenterFrequency As UInteger
    Private miDeviceIndex As Integer
    Private msLogFolder As String = "C:\"
    Private moMonitorThread As Thread
    Private mbRunning As Boolean = False
    Private moSyncContext As SynchronizationContext
    Private miSignalEvents As Integer = 0
    Private miSignalWindow As Integer = 1   ' 1 "bin" about 250Hz on each side of center frequency

    ' Circular buffer for pre-trigger storage
    Private mqQueue As New Queue(Of Byte())()
    Private miQueueMaxSize As Integer = 10 ' Adjust for X seconds of pre-buffering
    Private mbSignalDetected As Boolean = False
    ' Recording limit
    Private mtRecordingStartTime As Date
    Private miMaxRecordingTime As Integer = 60 ' Max seconds to record per event
    ' File writing and recording state
    Private myRecordingBuffer As New List(Of Byte()) ' Holds IQ data during an event
    Private miSampleRate As Integer = 2048000 ' SDR sample rate (2.048 MSPS)

    Private mtStartMonitor As Date = DateTime.Now

    ' Buffer for UI visualization
    Private myIqBuffer() As Byte
    Private miBufferSize As Integer = 16384 ' Standard RTL-SDR buffer size



    Public ReadOnly Property IsRunning As Boolean
        Get
            Return mbRunning
        End Get
    End Property

    Public ReadOnly Property IsRecording As Boolean
        Get
            Return mbSignalDetected
        End Get
    End Property

    Public ReadOnly Property SignalEventCount As Integer
        Get
            Return miSignalEvents
        End Get
    End Property

    ''' <summary>
    ''' Center for the frequency sample in Mhz
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CenterFrequency As Double
        Get
            Return miCenterFrequency
        End Get
    End Property

    Public ReadOnly Property SampleRate As Double
        Get
            Return CDbl(miSampleRate)
        End Get
    End Property


    Public Property SignalWindow As Integer
        Get
            Return miSignalWindow
        End Get
        Set(value As Integer)
            miSignalWindow = value
        End Set
    End Property

    ' Public method to retrieve the latest buffer (for UI visualization)
    Public Function GetBuffer() As Byte()
        SyncLock myIqBuffer
            Return If(myIqBuffer IsNot Nothing, CType(myIqBuffer.Clone(), Byte()), Nothing)
        End SyncLock
    End Function



    Public Sub New(deviceIndex As Integer, Optional ByVal centerFrequency As UInteger = 1600000000, Optional ByVal sampleRate As UInteger = 2048000)
        miCenterFrequency = centerFrequency
        miDeviceIndex = deviceIndex

        moSyncContext = SynchronizationContext.Current ' Capture UI thread context
        msLogFolder = clsLogger.LogPath
    End Sub

    Public Sub StartMonitor()
        If mbRunning Then Exit Sub ' Prevent duplicate threads

        ReDim myIqBuffer(miBufferSize - 1) ' Initialize buffer
        mbSignalDetected = False

        moMonitorThread = New Thread(AddressOf MonitorThread)
        moMonitorThread.IsBackground = True
        mbRunning = True
        moMonitorThread.Start()
    End Sub

    ' Stop monitoring
    Public Sub StopMonitor()
        mbRunning = False
        If moMonitorThread IsNot Nothing AndAlso moMonitorThread.IsAlive Then
            moMonitorThread.Join(500) ' Wait for thread to exit cleanly
        End If
    End Sub

    ' Background thread for monitoring SDR data
    Private Sub MonitorThread()
        Try
            ' Open the RTL-SDR device
            Dim result As Integer = RtlSdrApi.rtlsdr_open(miDeviceHandle, CUInt(miDeviceIndex))
            If result <> 0 Then
                clsLogger.Log("RtlSdrApi.MonitorThread", "Failed to open RTL-SDR device, result code was " & result & ".")
                RaiseError("Failed to open RTL-SDR device.")
                Exit Sub
            End If

            ' Reset buffer (important to clear any stale data)
            result = rtlsdr_reset_buffer(miDeviceHandle)

            ' Set sample rate (e.g., 2.048 MSPS)
            result = RtlSdrApi.rtlsdr_set_sample_rate(miDeviceHandle, 2048000)

            ' Set center frequency (e.g., 1.6 GHz)
            result = RtlSdrApi.rtlsdr_set_center_freq(miDeviceHandle, 1600000000)

            ' Enable manual gain mode (1 = manual, 0 = auto)
            result = RtlSdrApi.rtlsdr_set_tuner_gain_mode(miDeviceHandle, 1)

            ' Set gain manually (try values between 100-500)
            result = RtlSdrApi.rtlsdr_set_tuner_gain(miDeviceHandle, 300)

            ' Set center frequency
            RtlSdrApi.rtlsdr_set_center_freq(miDeviceHandle, miCenterFrequency)

            mtStartMonitor = DateTime.Now
            miSignalEvents = 0
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Starting monitor IQ data stream {mtStartMonitor:MM/dd/yyyy HH:mm:ss}.")

            ' Start streaming IQ data
            Dim bytesRead As Integer = 0

            While mbRunning
                Dim tempBuffer(miBufferSize - 1) As Byte ' Temp buffer for this read
                result = RtlSdrApi.rtlsdr_read_sync(miDeviceHandle, tempBuffer, miBufferSize, bytesRead)
                If result <> 0 Then
                    RaiseError("Error reading IQ samples.")
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"Error reading IQ data buffer, result code was {result}, stopping thread.")
                    Exit While
                End If

                ' Store in UI buffer
                SyncLock myIqBuffer
                    Array.Copy(tempBuffer, myIqBuffer, miBufferSize)
                End SyncLock

                ' Store in circular queue
                SyncLock mqQueue
                    mqQueue.Enqueue(tempBuffer)
                    If mqQueue.Count > miQueueMaxSize Then
                        mqQueue.Dequeue() ' Remove oldest buffer
                    End If
                End SyncLock

                ' Analyze data for a signal at center frequency
                Dim dNoiseFloor As Double = CalculateNoiseFloor(tempBuffer) ' Get noise level
                Dim dSignalPower As Double = CalculatePowerAtFrequency(tempBuffer) ' Get signal power
                ' Detect signal if power is significantly above noise floor
                If dSignalPower > (dNoiseFloor + 10) Then
                    ' Signal detected (10 dB above noise floor)
                    If Not mbSignalDetected Then
                        mbSignalDetected = True
                        mtRecordingStartTime = Date.Now
                        miSignalEvents = miSignalEvents + 1
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"🔹 Signal Detected! Strength: {dSignalPower}dB, noise floor: {dNoiseFloor}dB.")
                        ' Save pre-buffered IQ data
                        SyncLock mqQueue
                            myRecordingBuffer.AddRange(mqQueue.ToList())
                            mqQueue.Clear() ' Reset queue
                        End SyncLock
                        RaiseSignalChange(True)
                    End If
                    ' check of max time
                    If (DateTime.Now - mtRecordingStartTime).TotalSeconds > miMaxRecordingTime Then
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"⏳ Max recording time reached. Stopping recording.")
                        StopRecording()
                    Else
                        ' Save new IQ data
                        SyncLock myRecordingBuffer
                            myRecordingBuffer.Add(tempBuffer)
                        End SyncLock
                    End If
                Else
                    If mbSignalDetected Then
                        mbSignalDetected = False
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"🔻 Signal Lost! Strength: {dSignalPower}dB, noise floor: {dNoiseFloor}dB.")
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
            If miDeviceHandle <> IntPtr.Zero Then
                RtlSdrApi.rtlsdr_close(miDeviceHandle)
                miDeviceHandle = IntPtr.Zero
            End If
            mbRunning = False
            If mbSignalDetected Then
                RaiseSignalChange(False)
            End If
            mbSignalDetected = False
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Monitor thread ended.")
        End Try
    End Sub

    Private Sub StopRecording()
        mbSignalDetected = False
        SaveIqDataToZip()
        RaiseSignalChange(False)
    End Sub

    Private Sub SaveIqDataToZip()
        Try
            ' Calculate pre-buffer time in seconds
            Dim samplesPerBuffer As Integer = miBufferSize \ 2 ' IQ samples per buffer
            Dim timePerBuffer As Double = samplesPerBuffer / miSampleRate
            Dim preBufferTime As Double = timePerBuffer * miQueueMaxSize

            ' Adjust the timestamp for accurate event timing

            Dim adjustedStartTime As DateTime = mtRecordingStartTime.AddSeconds(-preBufferTime)
            Dim adjustedStartTimeUtc As DateTime = adjustedStartTime.ToUniversalTime()

            ' Generate timestamped filename with adjusted start time
            Dim timestamp As String = adjustedStartTimeUtc.ToString("yyyyMMdd_HHmmss.fff") & "Z"
            Dim outputFile As String = System.IO.Path.Combine(msLogFolder, $"IQ_Record_{timestamp}.zip")

            Using fs As New FileStream(outputFile, FileMode.Create)
                Using zip As New ZipArchive(fs, ZipArchiveMode.Create)
                    Dim iqentry As ZipArchiveEntry = zip.CreateEntry("signal.iq", CompressionLevel.Optimal)

                    Using entryStream As Stream = iqentry.Open()
                        SyncLock myRecordingBuffer
                            For Each chunk In myRecordingBuffer
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
    ""Center_Frequency_Hz"": {miCenterFrequency},
    ""Sample_Rate_Hz"": {miSampleRate},
    ""Total_IQ_Bytes"": {myRecordingBuffer.Sum(Function(b) b.Length)},
    ""Recording_Duration_S"": {(DateTime.Now - mtRecordingStartTime).TotalSeconds:F2},
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
            SyncLock myRecordingBuffer
                myRecordingBuffer.Clear()
            End SyncLock
        End Try
    End Sub


    Private Function CalculatePowerAtFrequency(buffer As Byte()) As Double
        Dim iSampleCount As Integer = buffer.Length \ 2 ' Number of IQ samples
        Dim aoIQData(iSampleCount - 1) As Complex

        ' Convert bytes to normalized IQ values (-1 to 1)
        For i As Integer = 0 To buffer.Length - 2 Step 2
            Dim dInPhase As Double = (buffer(i) - 127.5) / 127.5
            Dim dQuad As Double = (buffer(i + 1) - 127.5) / 127.5
            aoIQData(i \ 2) = New Complex(dInPhase, dQuad)
        Next

        ' Perform FFT
        Fourier.Forward(aoIQData, FourierOptions.NoScaling)

        ' Frequency resolution (Hz per bin)
        Dim dFreqResolution As Double = miSampleRate / iSampleCount
        Dim iCenterBin As Integer = CInt(Math.Round(miCenterFrequency / dFreqResolution))

        ' Calculate power over the defined window
        Dim dPowerSum As Double = 0
        Dim iBinsUsed As Integer = 0

        For iOffset As Integer = -miSignalWindow To miSignalWindow
            Dim iBin As Integer = iCenterBin + iOffset
            If iBin >= 0 AndAlso iBin < iSampleCount Then
                dPowerSum += aoIQData(iBin).Magnitude * aoIQData(iBin).Magnitude
                iBinsUsed += 1
            End If
        Next

        ' Average power across selected bins
        Dim dAvgPower As Double = dPowerSum / iBinsUsed

        ' Convert to dB scale
        Return 10 * Math.Log10(dAvgPower)
    End Function

    Private Function CalculateNoiseFloor(buffer As Byte()) As Double
        Dim iSampleCount As Integer = buffer.Length \ 2 ' Number of IQ samples
        If iSampleCount < 1000 Then
            Return -70 'insufficent data to determine noise floor, return a reasonable default
        End If

        Dim aoIQData(iSampleCount - 1) As Complex

        ' Convert bytes to normalized IQ values (-1 to 1)
        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
            Dim dInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
            Dim dQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
            aoIQData(piIndex \ 2) = New Complex(dInPhase, dQuad)
        Next

        ' Perform FFT
        Fourier.Forward(aoIQData, FourierOptions.NoScaling)

        ' Frequency resolution (Hz per bin)
        Dim dFreqResolution As Double = miSampleRate / iSampleCount

        ' Use the first 500 and last 500 bins as "quiet" noise bins
        Dim iNoiseBinCount As Integer = 1000 ' Number of bins to average
        Dim dNoisePowerSum As Double = 0

        ' Sum power from first 500 and last 500 bins
        For i As Integer = 0 To 499
            dNoisePowerSum += aoIQData(i).Magnitude * aoIQData(i).Magnitude
        Next
        For i As Integer = iSampleCount - 500 To iSampleCount - 1
            dNoisePowerSum += aoIQData(i).Magnitude * aoIQData(i).Magnitude
        Next

        ' Compute average noise power
        Dim dAvgNoisePower As Double = dNoisePowerSum / iNoiseBinCount

        ' Convert to dB
        Return 10 * Math.Log10(dAvgNoisePower)
    End Function




    ' Retrieve stored pre-trigger data (for recording)
    Public Function GetPreTriggerData() As List(Of Byte())
        SyncLock mqQueue
            Return mqQueue.ToList()
        End SyncLock
    End Function

    ' Raises an error event on the UI thread
    Private Sub RaiseError(message As String)
        If moSyncContext IsNot Nothing Then
            moSyncContext.Post(Sub(state) RaiseEvent ErrorOccurred(Me, message), Nothing)
        Else
            RaiseEvent ErrorOccurred(Me, message)
        End If
        StopMonitor() ' Ensure the thread is stopped on error
    End Sub


    ' Raises an error event on the UI thread
    Private Sub RaiseSignalChange(ByVal SignalFound As Boolean)
        If moSyncContext IsNot Nothing Then
            moSyncContext.Post(Sub(state) RaiseEvent SignalChange(Me, SignalFound), Nothing)
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
