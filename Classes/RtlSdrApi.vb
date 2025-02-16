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
    Private msDeviceName As String = ""
    Private miCenterFrequency As UInteger
    Private miDeviceIndex As Integer
    Private msLogFolder As String = "C:\"
    Private moMonitorThread As Thread
    Private mbRunning As Boolean = False
    Private moSyncContext As SynchronizationContext
    Private miSignalEvents As Integer = 0
    Private miSignalWindow As Integer = 1   ' 1 "bin" about 250Hz on each side of center frequency
    Private mdMinimumEventWindow As Double = 10D ' minimum number of minutes between recording events
    Private mbShowDeviceInfo As Boolean = True

    Private miGainMode As Integer = 0 '0=automatic, 1=manual
    Private miGainValue As Integer = 300


    ' Circular buffer for pre-trigger storage
    Private mqQueue As New Queue(Of Byte())()
    Private miQueueMaxSize As Integer = 10 ' Adjust for X seconds of pre-buffering
    Private mbSignalDetected As Boolean = False
    ' Recording limit
    Private mtRecordingStartTime As Date
    Private mtRecordingEndTime As Date
    Private miMaxRecordingTime As Integer = 60 ' Max seconds to record per event
    ' File writing and recording state
    Private myRecordingBuffer As New List(Of Byte()) ' Holds IQ data during an event
    Private miSampleRate As Integer = 2048000 ' SDR sample rate (2.048 MSPS)

    Private mtStartMonitor As Date = DateTime.Now

    ' Queue to store recording data for background writing
    Private mqRecordingQueue As New Queue(Of List(Of Byte()))
    Private moWriteThread As Thread
    Private mbWritingToDisk As Boolean = False

    ' Buffer for UI visualization
    Private myIqBuffer() As Byte
    Private miBufferSize As Integer = 16384 ' Standard RTL-SDR buffer size


#Region "  Read ONLY Properties "
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

    Public ReadOnly Property RecordingElapsed As TimeSpan
        Get
            If mbSignalDetected Then
                Return DateTime.Now.Subtract(mtRecordingStartTime)
            Else
                Return TimeSpan.FromSeconds(0)
            End If
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

    Public ReadOnly Property DeviceName As String
        Get
            Return msDeviceName
        End Get
    End Property

    Public ReadOnly Property MonitorElapsed As TimeSpan
        Get
            If Me.IsRunning Then
                Return DateTime.Now.Subtract(mtStartMonitor)
            Else
                Return TimeSpan.FromSeconds(0)
            End If
        End Get
    End Property

#End Region


#Region "  Read/Write Properties "
    Public Property SignalWindow As Integer
        Get
            Return miSignalWindow
        End Get
        Set(value As Integer)
            miSignalWindow = value
        End Set
    End Property

    Public Property ShowDeviceInfo As Boolean
        Get
            Return mbShowDeviceInfo
        End Get
        Set(value As Boolean)
            mbShowDeviceInfo = value
        End Set
    End Property


#End Region



    Public Sub New(deviceIndex As Integer, Optional ByVal centerFrequency As UInteger = 1600000000 _
            , Optional ByVal sampleRate As UInteger = 2048000 _
            , Optional ByVal automaticGain As Boolean = True _
            , Optional ByVal manualGainValue As Integer = 300)

        miDeviceIndex = deviceIndex
        miCenterFrequency = centerFrequency
        miSampleRate = sampleRate
        miGainMode = If(automaticGain, 0, 1)
        miGainValue = manualGainValue

        ' get the name of the device, function returns pointer to string var
        Dim oDeviceNamePtr As IntPtr = rtlsdr_get_device_name(deviceIndex)
        ' Convert the IntPtr to a string
        msDeviceName = Marshal.PtrToStringAnsi(oDeviceNamePtr)
        ' Capture UI thread context
        moSyncContext = SynchronizationContext.Current
        ' grab log foder path from the logger class so recordings go in the same folder.
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

    Public Function GetCurrentPowerLevels() As Double()
        Return ConvertRawToPowerLevels(Me.GetBuffer)
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

    Public Shared Function ConvertRawToPowerLevels(ByVal buffer As Byte()) As Double()
        If buffer.Length < 2 Then
            Return Nothing
        End If

        Dim piFftSize As Integer = buffer.Length \ 2
        Dim poComplexData(piFftSize - 1) As Complex
        Dim pdPowerValues(piFftSize - 1) As Double


        ' Convert raw IQ data to complex values
        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
            Dim pdInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
            Dim pdQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
            poComplexData(piIndex \ 2) = New Complex(pdInPhase, pdQuad)
        Next
        ' Perform FFT on the complex data
        Fourier.Forward(poComplexData, FourierOptions.NoScaling)
        ' Convert complex to dB values
        For piIndex = 0 To piFftSize - 1
            Dim pdRealPart As Double = poComplexData(piIndex).Real
            Dim pdImagPart As Double = poComplexData(piIndex).Imaginary
            pdPowerValues(piIndex) = pdRealPart * pdRealPart + pdImagPart * pdImagPart ' Compute power
            If pdPowerValues(piIndex) = 0 Then
                pdPowerValues(piIndex) = Double.NegativeInfinity
            Else
                pdPowerValues(piIndex) = 10 * Math.Log10(pdPowerValues(piIndex)) - 70
            End If
        Next
        ' Rearrange FFT bins (shift zero frequency to center)
        Dim pdPowerShifted(piFftSize - 1) As Double
        Dim piHalfSize As Integer = piFftSize \ 2
        Array.Copy(pdPowerValues, piHalfSize, pdPowerShifted, 0, piHalfSize)
        Array.Copy(pdPowerValues, 0, pdPowerShifted, piHalfSize, piHalfSize)

        Return pdPowerShifted
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
            Dim piBytesRead As Integer = 0
            Dim piLostSignalCount As Integer = 0
            Dim ptMonitorStart As Date = DateTime.Now
            Dim ptLastRecording As Date = DateTime.MinValue
            Dim piDetectCount As Integer = 0
            mbSignalDetected = False

            While mbRunning
                Dim tempBuffer(miBufferSize - 1) As Byte ' Temp buffer for this read
                piResult = RtlSdrApi.rtlsdr_read_sync(miDeviceHandle, tempBuffer, miBufferSize, piBytesRead)
                If piResult <> 0 OrElse piBytesRead <> miBufferSize Then
                    RaiseError("Error reading IQ samples.")
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"Error reading IQ data buffer, result code was {piResult}, bytes read was {piBytesRead}/{miBufferSize}.  Stopping thread.")
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
                Dim pdPowerLevels() As Double = ConvertRawToPowerLevels(tempBuffer)
                Dim dNoiseFloor As Double = CalculateNoiseFloor(pdPowerLevels) ' Get noise level
                Dim dSignalPower As Double = CalculatePowerAtFrequency(pdPowerLevels) ' Get signal power at center frequency
                ' Detect signal if power is significantly above noise floor and we've been buffering for at least 3 seconds
                Debug.WriteLine($"Noise floor is {dNoiseFloor}dB and Center signal is {dSignalPower}db.")
                If dSignalPower > (dNoiseFloor + 15) AndAlso Now.Subtract(ptMonitorStart).TotalSeconds > 3 Then
                    piDetectCount = piDetectCount + 1
                Else
                    piDetectCount = 0
                End If
                If piDetectCount > 3 OrElse mbSignalDetected Then
                    Dim ptNow As DateTime = DateTime.Now
                    ' Ensure a minimum delay between recordings (e.g., 10 minutes)
                    If (ptLastRecording = DateTime.MinValue) OrElse (ptNow.Subtract(ptLastRecording).TotalMinutes >= mdMinimumEventWindow) Then
                        Debug.WriteLine($"🔹 Signal Detected! Strength: {dSignalPower}dB, noise floor: {dNoiseFloor}dB.")
                        If Not mbSignalDetected Then
                            mbSignalDetected = True
                            piLostSignalCount = 0
                            mtRecordingStartTime = ptNow
                            miSignalEvents += 1
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"🔹 Signal Detected! Strength: {dSignalPower:F4}dB, noise floor: {dNoiseFloor:F4}dB.")
                            ' Save pre-buffered IQ data and the current data
                            SyncLock mqQueue
                                myRecordingBuffer.AddRange(mqQueue.ToList())
                                mqQueue.Clear() ' Reset queue
                            End SyncLock
                            RaiseSignalChange(True)
                        End If
                        piLostSignalCount = 0
                        ' Save new IQ data
                        SyncLock myRecordingBuffer
                            myRecordingBuffer.Add(tempBuffer)
                        End SyncLock
                        ' Check for max recording time
                        If DateTime.Now.Subtract(mtRecordingStartTime).TotalSeconds > miMaxRecordingTime Then
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"⏳ Max recording time reached. Stopping recording.")
                            mbSignalDetected = False
                            ptLastRecording = DateTime.Now
                            StopRecording()
                        End If
                    Else
                        Debug.WriteLine($"Skipping recording; last recorded {ptLastRecording}.")
                    End If
                Else
                    If mbSignalDetected Then
                        piLostSignalCount += 1
                        If piLostSignalCount >= 3 Then
                            ' we lost the signal
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"🔻 Signal Lost! Strength: {dSignalPower:F4}dB, noise floor: {dNoiseFloor:F4}dB.")
                            mbSignalDetected = False
                            ptLastRecording = DateTime.Now ' Update last recording timestamp
                            StopRecording()
                        End If
                    End If
                End If
            End While
            ' Log end of monitoring session
            Dim ptEnd As Date = DateTime.Now
            Dim poElapsed As TimeSpan = ptEnd.Subtract(mtStartMonitor)
            Dim psSignalText As String = If(miSignalEvents = 0, "no", miSignalEvents.ToString()) & " signal event" & If(miSignalEvents > 1, "s", "")
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Monitor thread ending at {ptEnd:MM/dd/yyyy HH:mm:ss} with {psSignalText}, {modMain.FullDisplayElapsed(poElapsed.TotalSeconds)}.")

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
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Exiting monitor thread.")
        End Try
    End Sub

    Private Sub StopRecording()
        mbSignalDetected = False
        mtRecordingEndTime = DateTime.Now

        ' Lock and queue the recorded buffer for background processing
        SyncLock mqRecordingQueue
            If myRecordingBuffer.Count > 0 Then
                mqRecordingQueue.Enqueue(New List(Of Byte())(myRecordingBuffer)) ' Copy buffer into queue
                myRecordingBuffer.Clear() ' Clear for next recording
            End If
        End SyncLock

        ' Start the write thread if it's not already running
        If moWriteThread Is Nothing OrElse Not moWriteThread.IsAlive Then
            Dim ptStart As Date = mtRecordingStartTime
            Dim ptEnd As Date = mtRecordingEndTime
            moWriteThread = New Thread(Sub() ProcessRecordingQueue(ptStart, ptEnd))
            moWriteThread.IsBackground = True
            moWriteThread.Start()
        End If

        RaiseSignalChange(False)
    End Sub

    ' background thread for writing recorded data out to disk
    Private Sub ProcessRecordingQueue(ByVal tRecordingStartTime As Date, ByVal tRecordingEndTime As Date)
        mbWritingToDisk = True
        Try
            While True
                Dim recordedData As List(Of Byte()) = Nothing

                ' Retrieve next recording from queue
                SyncLock mqRecordingQueue
                    If mqRecordingQueue.Count > 0 Then
                        recordedData = mqRecordingQueue.Dequeue()
                    Else
                        Exit While ' No more recordings to process, exit loop
                    End If
                End SyncLock

                If recordedData IsNot Nothing AndAlso recordedData.Count > 0 Then
                    Dim poZipper As New IQZipper(msLogFolder)
                    ' Calculate pre-buffer time in seconds
                    Dim samplesPerBuffer As Integer = miBufferSize \ 2 ' IQ samples per buffer
                    Dim timePerBuffer As Double = samplesPerBuffer / miSampleRate
                    Dim preBufferTime As Double = timePerBuffer * miQueueMaxSize
                    ' Adjust the timestamp for accurate event timing
                    Dim adjustedStartTime As DateTime = tRecordingStartTime.AddSeconds(-preBufferTime).ToUniversalTime()
                    With poZipper
                        .RecordedOnUTC = adjustedStartTime
                        .CenterFrequency = miCenterFrequency
                        .SampleRate = miSampleRate
                        .BufferSizeBytes = miBufferSize
                        .TotalIQBytes = recordedData.Sum(Function(b) b.Length)
                        .DurationSeconds = tRecordingEndTime.Subtract(tRecordingStartTime).TotalSeconds
                        .IQBuffer = recordedData
                    End With
                    If poZipper.SaveArchive Then
                        clsLogger.Log("RtlSdrApi.ProcessRecordingQueue", $"✅ Saved to {poZipper.ArchiveFile} (Start: {adjustedStartTime}, length: {poZipper.TotalIQBytes}, duration: {poZipper.DurationSeconds} seconds.)")
                    Else
                        RaiseError($"Error saving IQ data archive {poZipper.ArchiveFile} to folder.")
                    End If
                End If
            End While
        Catch ex As Exception
            clsLogger.LogException("RtlSdrAPI.ProcessRecordingQueue", ex)
        Finally
            mbWritingToDisk = False
        End Try
    End Sub

    '    Private Sub SaveIqDataToZip(recordedData As List(Of Byte()))
    '        Try
    '            ' Calculate pre-buffer time in seconds
    '            Dim samplesPerBuffer As Integer = miBufferSize \ 2 ' IQ samples per buffer
    '            Dim timePerBuffer As Double = samplesPerBuffer / miSampleRate
    '            Dim preBufferTime As Double = timePerBuffer * miQueueMaxSize

    '            ' Adjust the timestamp for accurate event timing
    '            Dim adjustedStartTime As DateTime = mtRecordingStartTime.AddSeconds(-preBufferTime).ToUniversalTime()
    '            Dim timestamp As String = adjustedStartTime.ToString("yyyyMMdd_HHmmss.fff") & "Z"
    '            Dim outputFile As String = System.IO.Path.Combine(msLogFolder, $"IQ_Record_{timestamp}.zip")

    '            Using fs As New FileStream(outputFile, FileMode.Create)
    '                Using zip As New ZipArchive(fs, ZipArchiveMode.Create)
    '                    ' Create IQ data entry
    '                    Dim iqEntry As ZipArchiveEntry = zip.CreateEntry("signal.iq", CompressionLevel.Optimal)
    '                    Using entryStream As Stream = iqEntry.Open()
    '                        For Each chunk In recordedData
    '                            entryStream.Write(chunk, 0, chunk.Length)
    '                        Next
    '                    End Using

    '                    ' Create metadata entry
    '                    Dim metadataEntry As ZipArchiveEntry = zip.CreateEntry("metadata.json", CompressionLevel.Optimal)
    '                    Using metadataStream As Stream = metadataEntry.Open()
    '                        Using writer As New StreamWriter(metadataStream)
    '                            Dim appVersion As String = $"{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}"
    '                            Dim metadata As String = $"
    '{{
    '    ""UTC_Start_Time"": ""{adjustedStartTime:yyyy-MM-ddTHH:mm:ss.fffZ}"",
    '    ""Center_Frequency_Hz"": {miCenterFrequency},
    '    ""Sample_Rate_Hz"": {miSampleRate},
    '    ""Total_IQ_Bytes"": {recordedData.Sum(Function(b) b.Length)},
    '    ""Recording_Duration_S"": {(DateTime.Now - mtRecordingStartTime).TotalSeconds:F2},
    '    ""Software_Version"": ""{appVersion}""
    '}}"
    '                            writer.Write(metadata)
    '                        End Using
    '                    End Using
    '                End Using
    '            End Using
    '            clsLogger.Log("RtlSdrApi.SaveIqDataToZip", $"✅ Saved to {outputFile} (Start: {adjustedStartTime})")
    '        Catch ex As Exception
    '            clsLogger.LogException("RtlSdrAPI.SaveIqDataToZip", ex)
    '            RaiseError("Error saving IQ data: " & ex.Message)
    '        End Try
    '    End Sub


    Private Function CalculatePowerAtFrequency(dPowerLevels As Double()) As Double
        Dim iFftBins As Integer = dPowerLevels.Length ' FFT bins after shift

        ' Ensure we have valid power data
        If iFftBins = 0 Then Return Double.NegativeInfinity

        ' Frequency resolution (Hz per bin)
        Dim dFreqResolution As Double = miSampleRate / iFftBins

        ' Correct bin index for center frequency
        Dim iCenterBin As Integer = iFftBins \ 2  'CInt(Math.Round((miCenterFrequency - (miCenterFrequency - (miSampleRate / 2))) / dFreqResolution))

        ' Ensure the calculated bin is within valid bounds
        If iCenterBin < 0 OrElse iCenterBin >= iFftBins Then
            Return Double.NegativeInfinity ' Return lowest power if out of range
        End If

        ' Calculate power over the defined window
        Dim dPowerSum As Double = 0
        Dim iBinsUsed As Integer = 0

        For iOffset As Integer = -miSignalWindow To miSignalWindow
            Dim iBin As Integer = iCenterBin + iOffset
            If iBin >= 0 AndAlso iBin < iFftBins Then
                ' Convert dB power values back to linear scale for summation
                dPowerSum += 10 ^ (dPowerLevels(iBin) / 10)
                iBinsUsed += 1
            End If
        Next

        ' Prevent division by zero
        If iBinsUsed = 0 Then Return Double.NegativeInfinity

        Dim dSelectedBins As Double() = dPowerLevels.Skip(iCenterBin - miSignalWindow).Take(2 * miSignalWindow + 1).ToArray()
        Dim dAvgPower As Double = Median(dSelectedBins)

        Return dAvgPower
    End Function

    Private Function Median(dValues As Double()) As Double
        Array.Sort(dValues)
        Dim iMid As Integer = dValues.Length \ 2
        If dValues.Length Mod 2 = 0 Then
            Return (dValues(iMid - 1) + dValues(iMid)) / 2
        Else
            Return dValues(iMid)
        End If
    End Function


    Private Function CalculateNoiseFloor(dPowerLevels As Double()) As Double
        Dim iFftBins As Integer = dPowerLevels.Length ' FFT bins after shift

        ' Ensure enough bins are available
        If iFftBins < 1000 Then
            Return -70 ' Insufficient data, return a reasonable default
        End If

        ' Define how many bins to use for noise calculation (e.g., first 500 + last 500)
        Dim iNoiseBinCount As Integer = 1000
        Dim dNoisePowerSum As Double = 0

        ' Sum power from the first 500 and last 500 bins
        For i As Integer = 0 To 499
            dNoisePowerSum += dPowerLevels(i)
        Next
        For i As Integer = iFftBins - 500 To iFftBins - 1
            dNoisePowerSum += dPowerLevels(i)
        Next

        ' Compute average noise power
        Dim dAvgNoisePower As Double = dNoisePowerSum / iNoiseBinCount

        ' Convert to dB and return
        Return dAvgNoisePower
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
