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
    Public Shared Function rtlsdr_get_sample_rates(ByVal device_handle As IntPtr, ByRef num_rates As Integer, ByVal sample_rates() As Integer) As Integer
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_tuner_type(ByVal dev As IntPtr) As Integer
    End Function

    <DllImport("rtlsdr.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function rtlsdr_get_tuner_gains(ByVal device_handle As IntPtr, ByVal gains_ptr As IntPtr) As Integer
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
    Public Event MonitorStarted(ByVal sender As Object, ByVal success As Boolean)
    Public Event MonitorEnded(ByVal Sender As Object)

    Private miDeviceHandle As IntPtr = IntPtr.Zero
    Private miCenterFrequency As UInteger
    Private miDeviceIndex As Integer
    Private msLogFolder As String = "C:\"
    Private moMonitorThread As Thread
    Private mbRunning As Boolean = False
    Private moSyncContext As SynchronizationContext
    Private miSignalEvents As Integer = 0
    Private miSignalWindow As Integer = 1   ' 1 "bin" about 250Hz on each side of center frequency

    Private miGainMode As Integer = 0 '0=automatic, 1=manual
    Private miGainValue As Integer = 300


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

    ''' <summary>
    ''' 0 = Automatic, 1 = Manual
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property GainMode As Integer
        Get
            Return miGainMode
        End Get
    End Property

    Public ReadOnly Property GainValue As Integer
        Get
            Return miGainValue
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


    Public Sub New(deviceIndex As Integer, Optional ByVal centerFrequency As UInteger = 1600000000 _
            , Optional ByVal sampleRate As UInteger = 2048000 _
            , Optional ByVal automaticGain As Boolean = True _
            , Optional ByVal manualGainValue As Integer = 300)

        miDeviceIndex = deviceIndex
        miCenterFrequency = centerFrequency
        miSampleRate = sampleRate
        miGainMode = If(automaticGain, 0, 1)
        miGainValue = manualGainValue


        moSyncContext = SynchronizationContext.Current ' Capture UI thread context
        msLogFolder = clsLogger.LogPath
    End Sub

    Public Sub StartMonitor()
        If mbRunning Then Exit Sub ' Prevent duplicate threads

        ReDim myIqBuffer(miBufferSize - 1) ' Initialize buffer
        mbSignalDetected = False

        moMonitorThread = New Thread(AddressOf MonitorThread)
        moMonitorThread.IsBackground = True
        moMonitorThread.Start()
    End Sub

    ' Stop monitoring
    Public Sub StopMonitor()
        mbRunning = False
        If moMonitorThread IsNot Nothing AndAlso moMonitorThread.IsAlive Then
            moMonitorThread.Join(500) ' Wait for thread to exit cleanly
        End If
    End Sub


    
    ' Public method to retrieve the latest buffer (for UI visualization)
    Public Function GetBuffer() As Byte()
        SyncLock myIqBuffer
            Return If(myIqBuffer IsNot Nothing, CType(myIqBuffer.Clone(), Byte()), Nothing)
        End SyncLock
    End Function


    ''' <summary>
    ''' Returns a list of valid manual gain settings.  RTL-SDR stores these values in tenths of dB, so for display you need to divide these values by 10D before displaying.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTunerGainsList() As List(Of Integer)
        Dim piNumGains As Integer = 0
        Dim poGainList As New List(Of Integer)
        Dim piResult As Integer
        If miDeviceHandle <> 0 Then
            piResult = rtlsdr_get_tuner_gains(miDeviceHandle, IntPtr.Zero)
            If piResult > 0 Then
                piNumGains = piResult
                Dim piGains(piNumGains - 1) As Integer
                ' Allocate memory and pass pointer
                Dim hGC As GCHandle = GCHandle.Alloc(piGains, GCHandleType.Pinned)
                Try
                    piResult = rtlsdr_get_tuner_gains(miDeviceHandle, hGC.AddrOfPinnedObject())

                    Debug.Print("Second call result: " & piResult)

                    ' Check if successful
                    If piResult > 0 Then
                        poGainList.AddRange(piGains)

                        ' Print the gain values for debugging
                        Debug.Print("Gain Values:")
                        For Each piGain As Integer In poGainList
                            Debug.Print(piGain.ToString())
                        Next
                    Else
                        Debug.Print("Second call returned an error.")
                    End If
                Finally
                    hGC.Free()
                End Try
            Else
                Debug.Print("First call returned an error.")
            End If
        End If
        Return poGainList
    End Function

    ''' <summary>
    ''' Returns a List(of Integer) with two entries (0) = min (1) = max.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFrequencyRangeList() As List(Of UInteger)
        Dim poList As New List(Of UInteger)
        Dim piMin As UInteger
        Dim piMax As UInteger

        If miDeviceHandle <> 0 Then
            Dim tunerType As Integer = rtlsdr_get_tuner_type(miDeviceHandle)
            Select Case tunerType
                Case 1 ' RTLSDR_TUNER_E4000
                    piMin = 52000000   ' 52 MHz
                    piMax = 2200000000 ' 2.2 GHz
                Case 2 ' RTLSDR_TUNER_FC0012
                    piMin = 22000000   ' 22 MHz
                    piMax = 948600000  ' 948.6 MHz
                Case 3 ' RTLSDR_TUNER_FC0013
                    piMin = 22000000   ' 22 MHz
                    piMax = 1100000000 ' 1.1 GHz
                Case 4 ' RTLSDR_TUNER_FC2580
                    piMin = 146000000  ' 146 MHz
                    piMax = 924000000  ' 924 MHz
                Case 5 ' RTLSDR_TUNER_R820T
                    piMin = 24000000   ' 24 MHz
                    piMax = 1766000000 ' 1.766 GHz
                Case Else
                    ' default something
                    piMin = 24000000   ' 24 MHz
                    piMax = 1766000000 ' 1.766 GHz
            End Select
            poList.Add(piMin)
            poList.Add(piMax)
        Else
            poList.Add(0)
            poList.Add(0)
        End If
        Return poList
    End Function

    Public Function GetSampleRates() As List(Of Integer)
        Dim piNumRates As Integer = 0
        Dim poRatesList As New List(Of Integer)
        Dim piResult As Integer

        If miDeviceHandle <> 0 Then
            piResult = rtlsdr_get_sample_rates(miDeviceHandle, piNumRates, Nothing)
            If piResult = 0 And piNumRates > 0 Then
                Dim piRates(piNumRates - 1) As Integer
                piResult = rtlsdr_get_sample_rates(miDeviceHandle, piNumRates, piRates)
                If piResult = 0 Then
                    poRatesList.AddRange(piRates)
                End If
            End If
        End If
        Return poRatesList
    End Function


    Public Function SetSampleRate(ByVal NewSampleRate As UInteger) As Boolean
        If miDeviceHandle <> 0 Then
            SyncLock myIqBuffer
                myIqBuffer = New Byte(miBufferSize - 1) {}
                rtlsdr_reset_buffer(miDeviceHandle)
            End SyncLock
            Dim piResult As Integer = rtlsdr_set_sample_rate(miDeviceHandle, NewSampleRate)
            If piResult = 0 Then
                miSampleRate = NewSampleRate
                Return True
            End If
        End If
        Return False
    End Function

    Public Function SetCenterFrequency(ByVal NewFrequency As UInteger) As Boolean
        If miDeviceHandle <> 0 Then
            SyncLock myIqBuffer
                myIqBuffer = New Byte(miBufferSize - 1) {}
                rtlsdr_reset_buffer(miDeviceHandle)
            End SyncLock
            Dim piResult As Integer = rtlsdr_set_center_freq(miDeviceHandle, NewFrequency)
            If piResult = 0 Then
                miCenterFrequency = NewFrequency
                Return True
            End If
        End If
        Return False
    End Function

    Public Function SetGainMode(ByVal AutomaticMode As Boolean) As Boolean
        If miDeviceHandle <> 0 Then
            Dim piMode As Integer = CInt(If(AutomaticMode, 0, 1))
            Dim piResult As Integer = rtlsdr_set_tuner_gain_mode(miDeviceHandle, piMode)
            If piResult = 0 Then
                SyncLock myIqBuffer
                    myIqBuffer = New Byte(miBufferSize - 1) {}
                    rtlsdr_reset_buffer(miDeviceHandle)
                End SyncLock
                miGainMode = piMode
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Sets manual gain value, should be in tenths of dB.  So for 30.0dB pass in 300.
    ''' </summary>
    ''' <param name="NewGainValue"></param>
    ''' <returns></returns>
    Public Function SetGain(ByVal NewGainValue As Integer) As Boolean
        If miDeviceHandle <> 0 Then
            Dim piResult As Integer = rtlsdr_set_tuner_gain(miDeviceHandle, NewGainValue)
            If piResult = 0 Then
                miGainValue = NewGainValue
                Return True
            End If
        End If
        Return False
    End Function


    Public Function ResetBuffer() As Boolean
        Dim piResult As Integer = 0
        If miDeviceHandle <> 0 Then
            SyncLock myIqBuffer
                piResult = rtlsdr_reset_buffer(miDeviceHandle)
                If piResult = 0 Then
                    myIqBuffer = New Byte(miBufferSize - 1) {}
                End If
            End SyncLock
        End If
        Return CBool(piResult = 0)
    End Function

    ' Background thread for monitoring SDR data
    Private Sub MonitorThread()
        Try
            ' Open the RTL-SDR device
            Dim piResult As Integer = RtlSdrApi.rtlsdr_open(miDeviceHandle, CUInt(miDeviceIndex))
            If piResult <> 0 OrElse miDeviceHandle = IntPtr.Zero Then
                clsLogger.Log("RtlSdrApi.MonitorThread", "Failed to open RTL-SDR device, result code was " & piResult & ".")
                RaiseError("Failed to open RTL-SDR device.")
                RaiseStarted(False)
                Exit Sub
            End If
            ' clear buffer and initialize myIQBuffer array
            Me.ResetBuffer()

            ' Set sample rate (e.g., 2.048 MSPS)
            piResult = rtlsdr_set_sample_rate(miDeviceHandle, miSampleRate)
            If piResult <> 0 Then
                clsLogger.Log("RtlSdrApi.MonitorThread", $"Failed to set RTL-SDR sample rate ({miSampleRate}), result code was {piResult}.")
                RaiseError("Failed to set RTL-SDR device sample rate.")
                RaiseStarted(False)
                Exit Sub
            End If

            ' Set center frequency (e.g., 1.6 GHz)
            piResult = rtlsdr_set_center_freq(miDeviceHandle, miCenterFrequency)
            If piResult <> 0 Then
                clsLogger.Log("RtlSdrApi.MonitorThread", $"Failed to set RTL-SDR center freq ({miCenterFrequency}), result code was {piResult}.")
                RaiseError("Failed to set RTL-SDR device center frequency.")
                RaiseStarted(False)
                Exit Sub
            End If

            ' Set gain mode (1 = manual, 0 = auto)
            piResult = rtlsdr_set_tuner_gain_mode(miDeviceHandle, miGainMode)
            If piResult <> 0 Then
                clsLogger.Log("RtlSdrApi.MonitorThread", $"Failed to set RTL-SDR gain mode ({miGainMode}), result code was {piResult}.")
                RaiseError("Failed to set RTL-SDR device gain mode.")
                RaiseStarted(False)
                Exit Sub
            End If

            ' see if using manual gain and set gain value if we are
            If miGainMode = 1 Then
                ' Set manual gain value in tenths of dB
                piResult = RtlSdrApi.rtlsdr_set_tuner_gain(miDeviceHandle, miGainValue)
                If piResult <> 0 Then
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"Failed to set RTL-SDR gain value ({miGainValue}), result code was {piResult}.")
                    RaiseError("Failed to set RTL-SDR device gain mode.")
                    RaiseStarted(False)
                    Exit Sub
                End If
            End If

            mtStartMonitor = DateTime.Now
            miSignalEvents = 0
            mbRunning = True
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Starting monitor IQ data stream {mtStartMonitor:MM/dd/yyyy HH:mm:ss}.")
            RaiseStarted(True)

            ' Start streaming IQ data
            Dim bytesRead As Integer = 0
            Dim piLostSignalCount As Integer = 0

            While mbRunning
                Dim tempBuffer(miBufferSize - 1) As Byte ' Temp buffer for this read
                piResult = RtlSdrApi.rtlsdr_read_sync(miDeviceHandle, tempBuffer, miBufferSize, bytesRead)
                If piResult <> 0 OrElse bytesRead <> miBufferSize Then
                    RaiseError("Error reading IQ samples.")
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"Error reading IQ data buffer, result code was {piResult}, bytes read was {bytesRead}/{miBufferSize}.  Stopping thread.")
                    Exit While
                End If

                ' Store in UI buffer
                SyncLock myIqBuffer
                    If myIqBuffer Is Nothing OrElse myIqBuffer.Length <> miBufferSize Then
                        myIqBuffer = New Byte(miBufferSize - 1) {} ' Resize buffer if needed
                    End If
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
                        piLostSignalCount = 0
                        mtRecordingStartTime = Date.Now
                        miSignalEvents = miSignalEvents + 1
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"🔹 Signal Detected! Strength: {dSignalPower}dB, noise floor: {dNoiseFloor}dB.")
                        ' Save pre-buffered IQ data
                        SyncLock mqQueue
                            myRecordingBuffer.AddRange(mqQueue.ToList())
                            mqQueue.Clear() ' Reset queue
                        End SyncLock
                        RaiseSignalChange(True)
                    Else
                        piLostSignalCount = 0
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
                        piLostSignalCount = piLostSignalCount + 1
                        If piLostSignalCount >= 3 Then
                            mbSignalDetected = False
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"🔻 Signal Lost! Strength: {dSignalPower}dB, noise floor: {dNoiseFloor}dB.")
                            StopRecording()
                        End If
                    Else
                        ' reset lost count if we've got a signal
                        piLostSignalCount = 0
                    End If
                End If
            End While
            Dim ptEnd As Date = DateTime.Now
            Dim poElapsed As TimeSpan = ptEnd.Subtract(mtStartMonitor)
            Dim psSignalText As String = If(miSignalEvents = 0, "no", miSignalEvents.ToString()) & " signal event" & If(miSignalEvents > 1, "s", "")
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Monitor thread ending at {ptEnd:MM/dd/yyyy HH:mm:ss} with {psSignalText}, {modMain.FullDisplayElapsed(poElapsed.TotalSeconds)}")

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
            RaiseEnded()
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

    Private Sub RaiseStarted(ByVal Success As Boolean)
        If moSyncContext IsNot Nothing Then
            moSyncContext.Post(Sub(state) RaiseEvent MonitorStarted(Me, Success), Nothing)
        Else
            RaiseEvent MonitorStarted(Me, Success)
        End If
    End Sub

    Private Sub RaiseEnded()
        If moSyncContext IsNot Nothing Then
            moSyncContext.Post(Sub(state) RaiseEvent MonitorEnded(Me), Nothing)
        Else
            RaiseEvent MonitorEnded(Me)
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
