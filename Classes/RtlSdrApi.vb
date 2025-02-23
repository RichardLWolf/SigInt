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

    Public Structure SDRConfiguration
        Public iDeviceIndex As Integer
        Public iCenterFrequency As UInteger
        Public iSampleRate As UInteger
        Public bAutomaticGain As Boolean
        Public iManualGainValue As Integer
        Public iSignalInitTime As Integer
        Public iSignalDetectionThreshold As Integer
        Public iSignalDetectionWindow As Integer
        Public dSignalEventResetTime As Integer
        Public iNoiseFloorBaselineInitTime As Integer
        Public dNoiseFloorThreshold As Double
        Public iNoiseFloorMinEventDuration As Integer
        Public iNoiseFloorCooldownDuration As Integer
        Public iNoiseFloorEventResetTime As Integer
        Public sDiscordWebhook As String
        Public sDiscordMention As String

        ' Constructor - must have the driver device index
        Public Sub New(iDeviceIdx As Integer)
            iDeviceIndex = iDeviceIdx
            iCenterFrequency = 1600000000
            iSampleRate = 2048000
            iSignalDetectionThreshold = 15
            iSignalDetectionWindow = 1
            iSignalInitTime = 3
            bAutomaticGain = False
            iManualGainValue = 166
            dSignalEventResetTime = 600
            iNoiseFloorBaselineInitTime = 60
            dNoiseFloorThreshold = 4.0
            iNoiseFloorMinEventDuration = 5
            iNoiseFloorCooldownDuration = 10
            iNoiseFloorEventResetTime = 30
            sDiscordWebhook = ""
            sDiscordMention = ""
        End Sub
    End Structure

    Public Structure SignalSnapshot
        Public pdAvgNoiseFloor As Double ' The calculated average noise floor
        Public pdSignalPower As Double   ' The signal strength at center frequency

        ' Constructor
        Public Sub New(pdFloor As Double, pdSignal As Double)
            pdAvgNoiseFloor = pdFloor
            pdSignalPower = pdSignal
        End Sub
    End Structure



    ' Event for UI-safe error reporting
    Public Event ErrorOccurred(ByVal sender As Object, ByVal message As String)
    Public Event RecordingEvent(ByVal sender As Object, ByVal RecordingActive As Boolean)
    Public Event MonitorStarted(ByVal sender As Object, ByVal success As Boolean)
    Public Event MonitorEnded(ByVal Sender As Object)

    Private miDeviceHandle As IntPtr = IntPtr.Zero
    Private msDeviceName As String = ""
    Private miCenterFrequency As UInteger
    Private miDeviceIndex As Integer
    Private msDiscordWebhook As String = ""
    Private msDiscordMention As String = ""
    Private msLogFolder As String = "C:\"
    Private moMonitorThread As Thread
    Private mbRunning As Boolean = False
    Private moSyncContext As SynchronizationContext
    Private miNFEvents As Integer = 0           ' noise floor event count
    Private miSignalDetectionWindow As Integer = 1   ' 1 "3 bins" about 250Hz on each side of center frequency
    Private miSignalInitTime As Integer = 3         ' Seconds to wait before looking for signal
    Private miSignalDetectionThreshold As Integer = 15 ' dB above noise floor to detect signal
    Private miSignalEventResetTime As Integer = 600        ' minimum number of seconds between signal recording events (60 to 3600)
    Private miNoiseFloorBaselineInitTime As Integer = 60  ' Time to establish baseline (seconds, 10 to 120)
    Private mdNoiseFloorThreshold As Double = 4.0         ' dB rise to trigger event (2dB to 8dB)
    Private miNoiseFloorMinEventDuration As Integer = 5   ' Seconds the rise must sustain (2 sec to 15 sec)
    Private miNoiseFloorCooldownDuration As Integer = 10  ' Seconds to pause averaging after event (5 sec to 30 sec)
    Private miNoiseFloorEventResetTime As Integer = 30    ' Quiet time before new event  (seconds, 10 to 600)
    Private miGainMode As Integer = 0 '0=automatic, 1=manual
    Private miGainValue As Integer = 300    'tenths of dB

    Private mbShowDeviceInfo As Boolean = True


    ' Circular buffer for pre-trigger storage
    Private mqQueue As New Queue(Of Byte())()
    Private miQueueMaxSize As Integer = 10 ' Adjust for X seconds of pre-buffering
    Private mbRecordingActive As Boolean = False
    ' Recording limit
    Private mtRecordingStartTime As Date
    Private mtRecordingEndTime As Date
    Private miMaxRecordingTime As Integer = 60 ' Max seconds to record per event
    ' Event Counters
    Private miSignalEvents As Integer = 0       ' singal spike event count
    Private miNoiseFloorEvents As Integer = 0   ' noise floor event count
    ' File writing and recording state
    Private myRecordingBuffer As New List(Of Byte()) ' Holds IQ data during an event
    Private miSampleRate As UInteger = 2048000 ' SDR sample rate (2.048 MSPS)

    Private mtStartMonitor As Date = DateTime.Now

    ' Queue to store recording data for background writing
    Private mqRecordingQueue As New Queue(Of List(Of Byte()))
    Private moWriteThread As Thread
    Private mbWritingToDisk As Boolean = False

    ' Queue to hold singal snapshots
    Private mqRollingPowerLevels As New Queue(Of SignalSnapshot)
    Private miMaxRollingBufferSize As Integer = 200 ' 20 seconds of buffer Default, can be adjusted
    Private mdLoopTime As Double = 0 ' Average loop time in seconds


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
            Return mbRecordingActive
        End Get
    End Property

    Public ReadOnly Property RecordingElapsed As TimeSpan
        Get
            If mbRecordingActive Then
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

    Public ReadOnly Property NoiseFloorEventCount As Integer
        Get
            Return miNoiseFloorEvents
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

    Public ReadOnly Property SampleRate As UInteger
        Get
            Return miSampleRate
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

    Public ReadOnly Property RollingPowerLevels() As List(Of SignalSnapshot)
        Get
            If mqRollingPowerLevels IsNot Nothing AndAlso mqRollingPowerLevels.Count > 0 Then
                SyncLock mqRollingPowerLevels
                    Return mqRollingPowerLevels.ToList()
                End SyncLock
            Else
                Return New List(Of SignalSnapshot)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Averate monitoring loop time in seconds.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AverageMonitorLoopTime As Double
        Get
            Return mdLoopTime
        End Get
    End Property

    ''' <summary>
    '''  Target frame count for the rolling powerlevels buffer, generally ~1/10 second per frame, 10 to 3600 frames.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property RollingPowerlevelsFrameCount As Integer
        Get
            Return miMaxRollingBufferSize
        End Get
    End Property

#End Region


#Region "  Read/Write Properties "
    Public Property ShowDeviceInfo As Boolean
        Get
            Return mbShowDeviceInfo
        End Get
        Set(value As Boolean)
            mbShowDeviceInfo = value
        End Set
    End Property

    ''' <summary>
    ''' The minimum time (in seconds) to wait between recordings (60 to 3600).
    ''' </summary>
    ''' <returns></returns>
    Public Property SignalEventResetTime As Integer
        Get
            Return miSignalEventResetTime
        End Get
        Set(value As Integer)
            miSignalEventResetTime = Math.Min(3600, Math.Max(60, value))
        End Set
    End Property

    ''' <summary>
    ''' The number of FFT bins to average for signal detection (1 to 7).  1 = 3 bins, 7 = 15 bins.
    ''' </summary>
    ''' <returns></returns>
    ''' </summary>
    ''' <returns></returns>
    ''' The maximum time (in seconds) to record per event (10 to 600).
    ''' </summary>
    ''' <returns></returns>
    Public Property SignalDetectionWindow As Integer
        Get
            Return miSignalDetectionWindow
        End Get
        Set(value As Integer)
            miSignalDetectionWindow = Math.Min(7, Math.Max(1, value))
        End Set
    End Property

    ''' <summary>
    ''' Number of dB above noise floor required to trigger a signal detection (5 to 25).
    ''' </summary>
    ''' <returns></returns>
    Public Property SignalDetectionThreshold As Integer
        Get
            Return miSignalDetectionThreshold
        End Get
        Set(value As Integer)
            miSignalDetectionThreshold = Math.Min(25, Math.Max(5, value))
        End Set
    End Property


    ''' <summary>
    ''' Seconds to wait before looking for signal spike (seconds, 1 to 10)
    ''' </summary>
    ''' <returns></returns>
    Public Property SignalInitTime As Integer
        Get
            Return miSignalInitTime
        End Get
        Set(ByVal value As Integer)
            miSignalInitTime = Math.Min(10, Math.Max(1, value))
        End Set
    End Property

    ''' <summary>
    ''' Seconds to establish baseline noise floor (10 to 120)
    ''' </summary>
    ''' <returns></returns>
    Public Property NoiseFloorBaselineInitTime As Integer
        Get
            Return miNoiseFloorBaselineInitTime
        End Get
        Set(ByVal value As Integer)
            miNoiseFloorBaselineInitTime = Math.Min(120, Math.Max(30, value))
        End Set
    End Property

    ''' <summary>
    ''' dB rise to trigger event (2dB to 8dB)
    ''' </summary>
    ''' <returns></returns>
    Public Property NoiseFloorThreshold As Double
        Get
            Return mdNoiseFloorThreshold
        End Get
        Set(ByVal value As Double)
            mdNoiseFloorThreshold = Math.Min(8D, Math.Max(2D, value))
        End Set
    End Property

    ''' <summary>
    ''' Seconds the rise must sustain before recording is triggerd (2 sec to 15 sec)
    ''' </summary>
    ''' <returns></returns>
    Public Property NoiseFloorMinEventDuration As Integer
        Get
            Return miNoiseFloorMinEventDuration
        End Get
        Set(ByVal value As Integer)
            miNoiseFloorMinEventDuration = Math.Min(15, Math.Max(2, value))
        End Set
    End Property

    ''' <summary>
    ''' Seconds to pause averaging after event (5 sec to 30 sec)
    ''' </summary>
    ''' <returns></returns>
    Public Property NoiseFloorCooldownDuration As Integer
        Get
            Return miNoiseFloorCooldownDuration
        End Get
        Set(ByVal value As Integer)
            miNoiseFloorCooldownDuration = Math.Min(30, Math.Max(5, value))
        End Set
    End Property

    ''' <summary>
    ''' Seconds to wait between noise floor event detections (60-3600)
    ''' </summary>
    ''' <returns></returns>
    Public Property NoiseFloorEventResetTime As Integer
        Get
            Return miNoiseFloorEventResetTime
        End Get
        Set(ByVal value As Integer)
            miNoiseFloorEventResetTime = Math.Min(3600, Math.Max(10, value))
        End Set
    End Property
#End Region



    Public Sub New(ByVal oSdrConfig As SDRConfiguration)
        ' copy configuration to class, readonly properties
        miDeviceIndex = oSdrConfig.iDeviceIndex
        miCenterFrequency = oSdrConfig.iCenterFrequency
        miSampleRate = oSdrConfig.iSampleRate
        miGainMode = If(oSdrConfig.bAutomaticGain, 0, 1)
        miGainValue = oSdrConfig.iManualGainValue
        ' read/write properties, set via properties in order to enforce value limits
        Me.SignalInitTime = oSdrConfig.iSignalInitTime
        Me.SignalEventResetTime = oSdrConfig.dSignalEventResetTime
        Me.SignalDetectionThreshold = oSdrConfig.iSignalDetectionThreshold
        Me.SignalDetectionWindow = oSdrConfig.iSignalDetectionWindow
        Me.NoiseFloorBaselineInitTime = oSdrConfig.iNoiseFloorBaselineInitTime
        Me.NoiseFloorThreshold = oSdrConfig.dNoiseFloorThreshold
        Me.NoiseFloorMinEventDuration = oSdrConfig.iNoiseFloorMinEventDuration
        Me.NoiseFloorCooldownDuration = oSdrConfig.iNoiseFloorCooldownDuration
        Me.NoiseFloorEventResetTime = oSdrConfig.iNoiseFloorEventResetTime
        ' not properties
        msDiscordWebhook = oSdrConfig.sDiscordWebhook
        msDiscordMention = oSdrConfig.sDiscordMention

        ' get the name of the device, function returns pointer to string var
        Dim oDeviceNamePtr As IntPtr = rtlsdr_get_device_name(miDeviceIndex)
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
        mbRecordingActive = False

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

        ' Check for stuck buffer (all 127/128 values)
        If buffer.All(Function(x) x = 127 OrElse x = 128) Then
            clsLogger.Log("ConvertRawToPowerLevels", "⚠️ Raw IQ buffer is all 127/128! Returning default noise.")
            Return Enumerable.Repeat(-80.0, piFftSize).ToArray()
        End If

        ' Convert raw IQ data to complex values
        For piIndex As Integer = 0 To buffer.Length - 2 Step 2
            Dim pdInPhase As Double = (buffer(piIndex) - 127.5) / 127.5
            Dim pdQuad As Double = (buffer(piIndex + 1) - 127.5) / 127.5
            poComplexData(piIndex \ 2) = New Complex(pdInPhase, pdQuad)
        Next

        ' Perform FFT
        Fourier.Forward(poComplexData, FourierOptions.NoScaling)

        ' Normalize FFT output
        Dim pdScaleFactor As Double = 1.0 / Math.Sqrt(piFftSize)
        For piIndex = 0 To piFftSize - 1
            poComplexData(piIndex) *= pdScaleFactor
        Next

        ' Convert to dB
        For piIndex = 0 To piFftSize - 1
            Dim pdRealPart As Double = poComplexData(piIndex).Real
            Dim pdImagPart As Double = poComplexData(piIndex).Imaginary
            pdPowerValues(piIndex) = pdRealPart * pdRealPart + pdImagPart * pdImagPart ' Compute power

            ' Prevent Log10(0) errors
            pdPowerValues(piIndex) = 10 * Math.Log10(pdPowerValues(piIndex) + 0.000000000001) - 30 ' Adjusted reference
        Next

        ' Rearrange FFT bins (shift zero frequency to center)
        Dim pdPowerShifted(piFftSize - 1) As Double
        Dim piHalfSize As Integer = piFftSize \ 2
        Array.Copy(pdPowerValues, piHalfSize, pdPowerShifted, 0, piHalfSize)
        Array.Copy(pdPowerValues, 0, pdPowerShifted, piHalfSize, piHalfSize)


        ' 🔍 Debug log if ALL FFT bins are zero or <= -100 dB
        If pdPowerShifted.All(Function(x) x <= -100) Then
            clsLogger.Log("ConvertRawToPowerLevels", "⚠️ FFT Power Shifted Values are all <= -100 dB! Possible data issue.")
        End If

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
            mtRecordingStartTime = DateTime.MinValue
            mtRecordingEndTime = DateTime.MinValue
            miSignalEvents = 0
            miNoiseFloorEvents = 0
            mbRunning = True
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Starting monitor IQ data stream {mtStartMonitor:MM/dd/yyyy HH:mm:ss} - {modMain.FormatHertz(miCenterFrequency)}, {modMain.FormatMSPS(miSampleRate)}, {modMain.FormatBytes(miBufferSize)} buffer.")
            RaiseStarted(True)

            ' Start streaming IQ data
            Dim piBytesRead As Integer = 0
            Dim piLostSignalCount As Integer = 0
            Dim ptMonitorStart As Date = DateTime.Now
            Dim piSignalDetectCount As Integer = 0
            Dim pdNoiseFloorAverage As Double = Double.NaN  ' Running average noise floor
            Dim ptLastSignalEvent As DateTime = DateTime.MinValue
            Dim ptLastNFEvent As DateTime = DateTime.MinValue
            Dim piNFResetTimer As Integer = 0
            Dim pbNoiseFloorEventActive As Boolean = False
            Dim pbSignalEventActive As Boolean = False
            Dim ptNoiseFloorEventStart As Date = Date.MinValue
            Dim poNoiseFloorEventElapsed As TimeSpan = TimeSpan.FromSeconds(0)
            Dim pbUpdateNoiseFloorAverage As Boolean = True
            ' Set the initial rolling buffer size
            UpdateRollingBufferSize(miMaxRollingBufferSize)
            ' Make sure we're not recording
            mbRecordingActive = False
            ' loop FPS timing vars
            Dim piFrameCounter As Integer = 0
            Dim ptLastFrameTime As DateTime = DateTime.Now


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

                ' Analyze data
                Dim pdPowerLevels() As Double = ConvertRawToPowerLevels(tempBuffer)
                Dim pdNoiseFloor As Double = CalculateNoiseFloor(pdPowerLevels) ' Get noise level for this buffer chunk
                Dim pdSignalPower As Double = CalculatePowerAtFrequency(pdPowerLevels) ' Get signal power at center frequency
                Dim ptNow As DateTime = DateTime.Now
                mdLoopTime = 0D

                ' Sanitize the noise floor value in case the device returns bad/partial data buffer
                If Double.IsNaN(pdNoiseFloor) OrElse Double.IsInfinity(pdNoiseFloor) Then
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"⚠️ Invalid Noise Floor Calculation: {pdNoiseFloor}")
                    pdNoiseFloor = -80D ' Set to a safe default dB value
                End If

                '  Add current chunk values to rolling buffer
                If Not Double.IsNaN(pdSignalPower) AndAlso Not Double.IsInfinity(pdSignalPower) Then
                    SyncLock mqRollingPowerLevels
                        mqRollingPowerLevels.Enqueue(New SignalSnapshot(pdNoiseFloor, pdSignalPower))
                        If mqRollingPowerLevels.Count > miMaxRollingBufferSize Then
                            mqRollingPowerLevels.Dequeue()
                        End If
                    End SyncLock
                End If

                ' ***** NOISE FLOOR MONITORING *****
                ' If we're outside the cooldown window, update the noise floor average
                If Not mbRecordingActive AndAlso pbUpdateNoiseFloorAverage Then
                    If Double.IsNaN(pdNoiseFloorAverage) Then
                        pdNoiseFloorAverage = pdNoiseFloor
                    Else
                        pdNoiseFloorAverage = (pdNoiseFloorAverage * 0.9) + (pdNoiseFloor * 0.1) ' Exponential moving average
                    End If
                End If

                ' Detect noise floor rise event
                If Not mbRecordingActive AndAlso pdNoiseFloor > (pdNoiseFloorAverage + mdNoiseFloorThreshold) Then
                    If ptNoiseFloorEventStart = Date.MinValue Then
                        ptNoiseFloorEventStart = ptNow
                    End If
                    poNoiseFloorEventElapsed = ptNow.Subtract(ptNoiseFloorEventStart)
                ElseIf poNoiseFloorEventElapsed.TotalSeconds > 0 Then
                    poNoiseFloorEventElapsed = poNoiseFloorEventElapsed.Subtract(TimeSpan.FromSeconds(1)) ' Gradual decay to allow for fluctuations
                Else
                    ptNoiseFloorEventStart = Date.MinValue
                    poNoiseFloorEventElapsed = TimeSpan.FromSeconds(0)
                End If

                ' Noise floor event detection, make sure it's been eleveated for at least specified min duration
                ' and it's been at least reset time since last event and we're not recording and it's => threshold
                If poNoiseFloorEventElapsed.TotalSeconds >= miNoiseFloorMinEventDuration _
                AndAlso Not mbRecordingActive _
                AndAlso Not pbNoiseFloorEventActive _
                AndAlso ptNow.Subtract(ptLastNFEvent).TotalSeconds >= miNoiseFloorEventResetTime _
                AndAlso Math.Abs(pdNoiseFloorAverage - pdNoiseFloor) >= mdNoiseFloorThreshold Then
                    ' Log the noise floor event
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"⚠️ Noise Floor Event Detected! Noise floor: {pdNoiseFloor:F4}dB, Avg. floor: {pdNoiseFloorAverage:F4}dB, Threshold: {mdNoiseFloorThreshold}dB.")
                    miNoiseFloorEvents += 1
                    ptLastNFEvent = ptNow
                    pbNoiseFloorEventActive = True     ' trigger event recording
                    pbUpdateNoiseFloorAverage = False  'don't add risen event into floor average while event is ongoing
                End If

                ' If noise floor normalizes, resume averaging if past cooldown
                If Not pbUpdateNoiseFloorAverage _
                AndAlso Not mbRecordingActive _
                AndAlso ptLastNFEvent <> DateTime.MinValue _
                AndAlso (ptNow.Subtract(ptLastNFEvent).TotalSeconds >= miNoiseFloorCooldownDuration) Then
                    pbUpdateNoiseFloorAverage = True
                    If Double.IsInfinity(pdNoiseFloorAverage) OrElse Double.IsNaN(pdNoiseFloorAverage) Then
                        pdNoiseFloorAverage = pdNoiseFloor ' Reset to actual noise floor in case of error
                    End If
                    clsLogger.Log("RtlSdrApi.MonitorThread", $"✅ Noise Floor Event Cleared. Resuming normal floor averaging.")
                End If

                ' ***** SIGNAL DETECTION *****
                ' If we are not active already and not already recording
                ' and it's been at least misignalInitTime since we started
                ' and it's been at least mdsignalEventResetTime since the last signal event
                If Not pbSignalEventActive _
                AndAlso Not mbRecordingActive _
                AndAlso pdSignalPower > (pdNoiseFloor + miSignalDetectionThreshold) _
                AndAlso ptNow.Subtract(ptMonitorStart).TotalSeconds > miSignalInitTime _
                AndAlso ptNow.Subtract(ptLastSignalEvent).TotalSeconds > miSignalEventResetTime Then
                    piSignalDetectCount += 1
                    If piSignalDetectCount >= 3 Then
                        clsLogger.Log("RtlSdrApi.MonitorThread", $"⚠️ Signal Detected! Strength: {pdSignalPower:F4}dB, Noise Floor: {pdNoiseFloor:F4}dB.")
                        ptLastSignalEvent = DateTime.Now
                        miSignalEvents += 1
                        pbSignalEventActive = True
                    End If
                Else
                    piSignalDetectCount = 0
                End If

                ' If we're recording, check for signal loss and duration limit
                If mbRecordingActive Then
                    ' Save new IQ data to recording buffer
                    SyncLock myRecordingBuffer
                        myRecordingBuffer.Add(tempBuffer)
                    End SyncLock

                    ' check for singal loss
                    If pbSignalEventActive OrElse pbNoiseFloorEventActive Then
                        ' some event is active, reset lost signal counter
                        piLostSignalCount = 0
                        ' Check for max recording time
                        If ptNow.Subtract(mtRecordingStartTime).TotalSeconds > miMaxRecordingTime Then
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"⏳ Max recording time reached. Stopping recording.")
                            ' reset both signal flags
                            pbSignalEventActive = False
                            pbNoiseFloorEventActive = False
                            StopRecording(pdNoiseFloorAverage)
                        End If
                    Else
                        ' check for lost singal abort
                        piLostSignalCount += 1
                        If piLostSignalCount >= 3 Then
                            clsLogger.Log("RtlSdrApi.MonitorThread", $"🔻 Signal Lost! Strength: {pdSignalPower:F4}dB, Noise Floor: {pdNoiseFloor:F4}dB.")
                            ' reset both signal flags
                            pbSignalEventActive = False
                            pbNoiseFloorEventActive = False
                            StopRecording(pdNoiseFloorAverage)
                            RaiseSignalChange(False)
                        End If
                    End If
                Else
                    ' see if we have a new signal to record
                    If pbSignalEventActive OrElse pbNoiseFloorEventActive Then
                        ' Start recording
                        mbRecordingActive = True
                        piLostSignalCount = 0
                        mtRecordingStartTime = ptNow
                        Select Case True
                            Case pbSignalEventActive And Not pbNoiseFloorEventActive
                                clsLogger.Log("RtlSdrApi.MonitorThread", $"🎙️ Recording started due to Signal Detection Event.")
                            Case pbNoiseFloorEventActive And Not pbSignalEventActive
                                clsLogger.Log("RtlSdrApi.MonitorThread", $"🎙️ Recording started due to Noise Floor Event.")
                            Case Else
                                ' both active
                                clsLogger.Log("RtlSdrApi.MonitorThread", $"🎙️ Recording started due to simultaneous Signal and Noise Floor Events.")
                        End Select
                        ' Save pre-buffered IQ data
                        SyncLock mqQueue
                            myRecordingBuffer.AddRange(mqQueue.ToList())
                            mqQueue.Clear()
                        End SyncLock
                        ' Save new IQ data
                        SyncLock myRecordingBuffer
                            myRecordingBuffer.Add(tempBuffer)
                        End SyncLock
                        ' notify owner
                        RaiseSignalChange(True)
                    End If
                End If
                ' clac loop FPS
                Dim ptFpsNow As DateTime = DateTime.Now
                Dim pdElapsed As Double = (ptFpsNow - ptLastFrameTime).TotalSeconds
                ptLastFrameTime = ptFpsNow
                ' Running average of loop time (smoothing factor 0.1)
                If piFrameCounter > 10 Then
                    mdLoopTime = (mdLoopTime * 0.9) + (pdElapsed * 0.1)
                Else
                    mdLoopTime = pdElapsed ' First few frames, just assign
                End If
                piFrameCounter += 1
                ' Check rolling queue size in case FPS gets too high or low
                If piFrameCounter Mod 50 = 0 Then ' Every 50 frames (~5 sec intervals)
                    UpdateRollingBufferSize(mdLoopTime)
                End If
                ' Do we need to pause for the cause?
                ' Calculate the desired delay to maintain ~10ms per loop (100 FPS max)
                Dim piTargetLoopTime As Integer = 10 ' Target in milliseconds
                Dim piSleepTime As Integer = Math.Max(5, piTargetLoopTime - CInt(pdElapsed * 1000)) ' Enforce minimum 5ms sleep
                Debug.WriteLine($"Monitor sleep time is {piSleepTime:G}, pdElapsed loop time is: {pdElapsed:F5}")
                Thread.Sleep(piSleepTime)
                If piSleepTime > 0 Then
                    Thread.Sleep(piSleepTime)
                End If
            End While

            ' Log end of monitoring session
            Dim ptEnd As Date = DateTime.Now
            Dim poElapsed As TimeSpan = ptEnd.Subtract(mtStartMonitor)
            Dim psEnding As String = $"Monitor thread ending at {ptEnd:MM/dd/yyyy HH:mm:ss}, {modMain.FullDisplayElapsed(poElapsed.TotalSeconds)} elapsed.  "
            Dim psEventText As String = $"Average noise floor was {pdNoiseFloorAverage:F4} dB.  "
            If miSignalEvents = 0 And miNoiseFloorEvents = 0 Then
                psEventText = "No events were recorded."
            Else
                psEventText = If(miSignalEvents = 0, "No", miSignalEvents.ToString()) & " signal event" & If(miSignalEvents <> 1, "s", "")
                psEventText = psEventText & " and " & If(miNoiseFloorEvents = 0, "no", miNoiseFloorEvents.ToString()) & " noise floor event" & If(miNoiseFloorEvents <> 1, "s", "")
                psEventText = psEventText & " were recorded."
            End If
            psEnding = psEnding & psEventText
            clsLogger.Log("RtlSdrApi.MonitorThread", psEnding)

        Catch ex As Exception
            clsLogger.LogException("rtlSdrApi.MonitorThread", ex)

        Finally
            ' Cleanup SDR device
            If miDeviceHandle <> IntPtr.Zero Then
                Dim piResult As Integer = RtlSdrApi.rtlsdr_close(miDeviceHandle)
                miDeviceHandle = IntPtr.Zero
            End If
            myRecordingBuffer.Clear()
            mqQueue.Clear 
            mbRecordingActive = False
            mbRunning = False
            RaiseEnded()
            clsLogger.Log("RtlSdrApi.MonitorThread", $"Exiting monitor thread.")
        End Try
    End Sub


    Private Sub UpdateRollingBufferSize(ByVal dLoopTime As Double)
        If dLoopTime > 0 Then
            ' Calculate how many frames fit into the desired time window
            Dim piNewSize As Integer = CInt(miMaxRollingBufferSize / dLoopTime)

            ' Enforce reasonable limits (minimum 10 frames, max 1 hour)
            piNewSize = Math.Max(10, Math.Min(3600, piNewSize))

            SyncLock mqRollingPowerLevels
                miMaxRollingBufferSize = piNewSize
                ' Trim queue if it's too large
                While mqRollingPowerLevels.Count > miMaxRollingBufferSize
                    mqRollingPowerLevels.Dequeue()
                End While
            End SyncLock
        End If
    End Sub



    ' Public method to adjust buffer size dynamically
    Public Sub SetRollingBufferSize(ByVal piNewSize As Integer)
        If piNewSize > 10 AndAlso piNewSize <= 3600 Then ' Enforce limits (10 sec to 1 hour)
            SyncLock mqRollingPowerLevels
                miMaxRollingBufferSize = piNewSize
                ' Trim excess if needed
                While mqRollingPowerLevels.Count > miMaxRollingBufferSize
                    mqRollingPowerLevels.Dequeue()
                End While
            End SyncLock
        End If
    End Sub



    Private Sub StopRecording(ByVal dAvgNoiseFloor As Double)
        mbRecordingActive = False
        mtRecordingEndTime = DateTime.Now
        Dim poEl As TimeSpan = mtRecordingStartTime.Subtract(mtStartMonitor)

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
            moWriteThread = New Thread(Sub() ProcessRecordingQueue(ptStart, ptEnd, dAvgNoiseFloor, poEl.TotalSeconds))
            moWriteThread.IsBackground = True
            moWriteThread.Start()
        End If

        RaiseSignalChange(False)
    End Sub

    ' background thread for writing recorded data out to disk
    Private Sub ProcessRecordingQueue(ByVal tRecordingStartTime As Date, ByVal tRecordingEndTime As Date, ByVal dAverageNoiseFloor As Double, ByVal oSessionElapsedSec As Double)
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
                        .GainMode = If(miGainMode = 0, "auto", String.Format("{0:#0.0} dB", miGainValue / 10))
                        .TotalIQBytes = recordedData.Sum(Function(b) b.Length)
                        .DurationSeconds = tRecordingEndTime.Subtract(tRecordingStartTime).TotalSeconds
                        .AverageNoiseFloor = dAverageNoiseFloor
                        .SessionElapsedSeconds = oSessionElapsedSec
                        .IQBuffer = recordedData
                    End With
                    If poZipper.SaveArchive Then
                        clsLogger.Log("RtlSdrApi.ProcessRecordingQueue", $"✅ Saved to {poZipper.ArchiveFile} (Start: {adjustedStartTime.ToLocalTime}, length: {poZipper.TotalIQBytes}, duration: {poZipper.DurationSeconds} seconds.)")
                        If msDiscordWebhook <> "" Then
                            Dim psNotificaiton As String = $"Event detected and recorded at frequency {modMain.FormatHertz(miCenterFrequency)}.  Data Size: {modMain.FormatBytes(poZipper.TotalIQBytes)}, duration: {poZipper.DurationSeconds:G2} seconds."
                            Call modMain.SendDiscordNotification(psNotificaiton, msDiscordWebhook, msDiscordMention)
                        End If
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

        For iOffset As Integer = -miSignalDetectionWindow To miSignalDetectionWindow
            Dim iBin As Integer = iCenterBin + iOffset
            If iBin >= 0 AndAlso iBin < iFftBins Then
                ' Convert dB power values back to linear scale for summation
                dPowerSum += 10 ^ (dPowerLevels(iBin) / 10)
                iBinsUsed += 1
            End If
        Next

        ' Prevent division by zero
        If iBinsUsed = 0 Then Return Double.NegativeInfinity

        Dim dSelectedBins As Double() = dPowerLevels.Skip(iCenterBin - miSignalDetectionWindow).Take(2 * miSignalDetectionWindow + 1).ToArray()
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
            clsLogger.Log("CalculateNoiseFloor", $"⚠️ Insufficient FFT bins ({iFftBins}). Returning -80 dB default.")
            Return -80
        End If

        Dim dNoisePowerSum As Double = 0

        ' Count valid bins in the first and last 500 bins
        Dim iValidBins As Integer = dPowerLevels.Take(500).Count(Function(x) x > -100) +
                                dPowerLevels.Skip(iFftBins - 500).Count(Function(x) x > -100)

        ' Require at least 100 valid bins across both sections
        If iValidBins < 100 Then
            clsLogger.Log("CalculateNoiseFloor", $"⚠️ Not enough valid FFT bins in noise range! ({iValidBins}/1000 found) Returning -80 dB default.")
            Return -80
        End If

        ' Sum power from the first 500 bins (convert dB → linear scale)
        For i As Integer = 0 To 499
            If dPowerLevels(i) > -100 Then ' Ignore extremely low power bins
                dNoisePowerSum += 10 ^ (dPowerLevels(i) / 10)
            End If
        Next

        ' Sum power from the last 500 bins (convert dB → linear scale)
        For i As Integer = iFftBins - 500 To iFftBins - 1
            If dPowerLevels(i) > -100 Then ' Ignore extremely low power bins
                dNoisePowerSum += 10 ^ (dPowerLevels(i) / 10)
            End If
        Next

        ' Compute average noise power in linear scale
        Dim dAvgNoisePower As Double = dNoisePowerSum / 1000

        ' Prevent division by zero
        If dAvgNoisePower <= 0 Then
            clsLogger.Log("CalculateNoiseFloor", $"⚠️ Noise Power Sum = {dNoisePowerSum}, returning default -80 dB")
            Return -80
        End If

        ' Convert back to dB and return
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
            moSyncContext.Post(Sub(state) RaiseEvent RecordingEvent(Me, SignalFound), Nothing)
        Else
            RaiseEvent RecordingEvent(Me, SignalFound)
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
