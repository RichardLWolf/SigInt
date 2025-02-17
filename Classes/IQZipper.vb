Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class IQZipper
    Implements IDisposable

    Public Structure IQArchiveMetadata
        Public FileName As String
        Public RecordingTimeUTC As Date
        Public CenterFrequency As UInteger
        Public SampleRateHz As Integer
        Public BufferSizeBytes As Integer
        Public TotalIQBytes As Long
        Public RecordingDurationSec As Double
        Public AppSoftwareVer As String
        Public UncompressedSize As Long
    End Structure

    Private Const METADATA_FILENAME As String = "metadata.json"
    Private Const IQDATA_FILENAME As String = "signal.iq"

    Private msArchivePath As String
    Private msArchiveFile As String
    Private mtRecordedOnUTC As Date
    Private miCenterFrequency As UInteger
    Private miSampleRate As Integer
    Private miBufferSizeBytes As Integer
    Private miTotalIQBytes As Integer
    Private miDurationSeconds As Double
    Private msAppVer As String
    Private myIQData As List(Of Byte())
    Private disposedValue As Boolean

#Region "  Public Properties "
    Public Property ArchiveFile As String
        Get
            Return msArchiveFile
        End Get
        Set(ByVal value As String)
            msArchiveFile = value.Trim
        End Set
    End Property

    Public Property RecordedOnUTC As Date
        Get
            Return mtRecordedOnUTC
        End Get
        Set(ByVal value As Date)
            mtRecordedOnUTC = value
        End Set
    End Property

    Public Property CenterFrequency As UInteger
        Get
            Return miCenterFrequency
        End Get
        Set(ByVal value As UInteger)
            miCenterFrequency = value
        End Set
    End Property

    Public Property SampleRate As Integer
        Get
            Return miSampleRate
        End Get
        Set(ByVal value As Integer)
            miSampleRate = value
        End Set
    End Property

    Public Property BufferSizeBytes As Integer
        Get
            Return miBufferSizeBytes
        End Get
        Set(value As Integer)
            miBufferSizeBytes = value
        End Set
    End Property

    Public Property TotalIQBytes As Integer
        Get
            Return miTotalIQBytes
        End Get
        Set(ByVal value As Integer)
            miTotalIQBytes = value
        End Set
    End Property

    Public Property DurationSeconds As Double
        Get
            Return miDurationSeconds
        End Get
        Set(ByVal value As Double)
            miDurationSeconds = value
        End Set
    End Property

    Public Property AppVer As String
        Get
            Return msAppVer
        End Get
        Set(ByVal value As String)
            msAppVer = value.Trim
        End Set
    End Property

    Public Property IQBuffer As List(Of Byte())
        Get
            Return myIQData
        End Get
        Set(ByVal value As List(Of Byte()))
            myIQData = value
        End Set
    End Property

#End Region


    Public Shared Function GetArchiveFileList(ByVal sArchiveFolder As String) As List(Of IQArchiveMetadata)
        Dim poList As New List(Of IQArchiveMetadata)

        Try
            ' Get list of zip files in the folder
            Dim poFileList As New List(Of String)
            For Each psFile As String In Directory.GetFiles(sArchiveFolder, "IQ_Record_*.zip")
                If IsValidRecordingFile(psFile) Then
                    poFileList.Add(psFile)
                End If
            Next
            ' read the metadata from each file
            For Each psFile As String In poFileList
                Dim poFile As IQArchiveMetadata = ExtractMetadata(psFile)
                If poFile.TotalIQBytes > 0 Then
                    poList.Add(poFile)
                End If
            Next
        Catch ex As Exception
            clsLogger.LogException("IQZipper.GetFileList", ex)
        End Try

        Return poList
    End Function



    Public Sub New(ByVal sArchivePath As String)
        msArchivePath = sArchivePath
    End Sub


    Public Function LoadArchive(ByVal ArchiveFileName As String) As Boolean
        Try
            If Not System.IO.File.Exists(ArchiveFileName) Then Return False
            Dim poData As IQArchiveMetadata = ExtractMetadata(ArchiveFileName)
            If poData.TotalIQBytes < 1 OrElse poData.BufferSizeBytes < 1 Then Return False

            msArchiveFile = ArchiveFileName
            msArchivePath = System.IO.Path.GetDirectoryName(ArchiveFileName)
            mtRecordedOnUTC = poData.RecordingTimeUTC
            miCenterFrequency = poData.CenterFrequency
            miSampleRate = poData.SampleRateHz
            miBufferSizeBytes = poData.BufferSizeBytes
            miTotalIQBytes = poData.TotalIQBytes
            miDurationSeconds = poData.RecordingDurationSec
            msAppVer = poData.AppSoftwareVer

            ' Initialize myIQData
            myIQData = New List(Of Byte())()
            ' Load IQ data from zip to memory
            Using poZip As ZipArchive = ZipFile.OpenRead(ArchiveFileName)
                Dim poEntry As ZipArchiveEntry = poZip.GetEntry(IQDATA_FILENAME)
                If poEntry IsNot Nothing Then
                    Using poStream As Stream = poEntry.Open()
                        Using poMemoryStream As New MemoryStream()
                            poStream.CopyTo(poMemoryStream)
                            Dim poBaRawData As Byte() = poMemoryStream.ToArray()
                            ' Use the stored buffer size to reconstruct the List(Of Byte())
                            Dim piOffset As Integer = 0
                            While piOffset < poBaRawData.Length
                                Dim piLength As Integer = Math.Min(miBufferSizeBytes, poBaRawData.Length - piOffset)
                                Dim poBaChunk As Byte() = New Byte(piLength - 1) {}
                                Buffer.BlockCopy(poBaRawData, piOffset, poBaChunk, 0, piLength)
                                myIQData.Add(poBaChunk)
                                piOffset += miBufferSizeBytes
                            End While
                        End Using
                        poStream.Close()
                    End Using
                Else
                    Return False
                End If
            End Using

            Return True

        Catch ex As Exception
            clsLogger.LogException($"IQZipper.LoadArchive({ArchiveFileName})", ex)
        End Try

        Return False
    End Function



    Public Function SaveArchive() As Boolean
        If String.IsNullOrEmpty(msArchivePath) OrElse myIQData.Count < 1 OrElse miSampleRate = 0 OrElse miCenterFrequency = 0 Or BufferSizeBytes = 0 Then
            Return False
        End If

        Try
            ' create filename based on timestamp.
            Dim psTimestamp As String = mtRecordedOnUTC.ToString("yyyyMMdd_HHmmss.fff") & "Z"
            Dim psOutputFile As String = System.IO.Path.Combine(msArchivePath, $"IQ_Record_{psTimestamp}.zip")
            msArchiveFile = psOutputFile

            Using poFS As New FileStream(psOutputFile, FileMode.Create)
                Using poZip As New ZipArchive(poFS, ZipArchiveMode.Create)
                    ' Create IQ data entry
                    Dim poIqEntry As ZipArchiveEntry = poZip.CreateEntry(IQDATA_FILENAME, CompressionLevel.Optimal)
                    Using poEntryStream As Stream = poIqEntry.Open()
                        For Each poChunk In myIQData
                            poEntryStream.Write(poChunk, 0, poChunk.Length)
                        Next
                    End Using

                    ' Create metadata entry
                    Dim poMetadataEntry As ZipArchiveEntry = poZip.CreateEntry(METADATA_FILENAME, CompressionLevel.Optimal)
                    Using poMetadataStream As Stream = poMetadataEntry.Open()
                        Using poWriter As New StreamWriter(poMetadataStream)
                            Dim psAppVersion As String = $"{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}"
                            Dim psMetadata As String = $"
                                {{
                                    ""UTC_Start_Time"": ""{mtRecordedOnUTC:yyyy-MM-ddTHH:mm:ss.fffZ}"",
                                    ""Center_Frequency_Hz"": {miCenterFrequency},
                                    ""Sample_Rate_Hz"": {miSampleRate},
                                    ""Buffer_Size_Bytes"": {miBufferSizeBytes},
                                    ""Total_IQ_Bytes"": {myIQData.Sum(Function(b) b.Length)},
                                    ""Recording_Duration_S"": {miDurationSeconds:F2},
                                    ""Software_Version"": ""{psAppVersion}""
                                }}"
                            poWriter.Write(psMetadata)
                        End Using
                    End Using
                End Using
            End Using
            Return True

        Catch ex As Exception
            clsLogger.LogException("IQZipper.SaveArchive", ex)
        End Try
        Return False
    End Function

    Public Function ExportToWavFile(ByVal sFileName As String) As Boolean
        If myIQData Is Nothing OrElse myIQData.Count = 0 Then
            clsLogger.Log("IQZipper.ExportToWavFile - Cannot export to WAV format, no IQ data.")
            Return False
        End If

        Try
            ' Define WAV file parameters
            Dim iChannels As Integer = 2  ' I and Q channels (stereo)
            Dim iBitsPerSample As Integer = 16
            Dim iByteRate As Integer = (miSampleRate * iChannels * (iBitsPerSample / 8))
            Dim iBlockAlign As Integer = (iChannels * (iBitsPerSample / 8))
            Dim iDataSize As Integer = myIQData.Sum(Function(x) x.Length)

            Dim iFileSize As Integer = 36 + iDataSize

            Using oFileStream As New FileStream(sFileName, FileMode.Create)
                Using oBinaryWriter As New BinaryWriter(oFileStream)
                    ' Write WAV Header
                    oBinaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"))
                    oBinaryWriter.Write(iFileSize) ' File size - 8
                    oBinaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"))

                    ' Format chunk
                    oBinaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("fmt "))
                    oBinaryWriter.Write(16) ' Subchunk1Size
                    oBinaryWriter.Write(CShort(1)) ' Audio format (PCM)
                    oBinaryWriter.Write(CShort(iChannels)) ' Channels (2 for I/Q)
                    oBinaryWriter.Write(miSampleRate) ' Sample rate
                    oBinaryWriter.Write(iByteRate) ' Byte rate
                    oBinaryWriter.Write(CShort(iBlockAlign)) ' Block align
                    oBinaryWriter.Write(CShort(iBitsPerSample)) ' Bits per sample

                    ' Data chunk
                    oBinaryWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"))
                    oBinaryWriter.Write(iDataSize) ' Data size
                    ' Flatten List(Of Byte()) into a single Byte() array and write it
                    Dim abFlattened() As Byte = IQBuffer.SelectMany(Function(x) x).ToArray()
                    oBinaryWriter.Write(abFlattened) ' Raw IQ data

                End Using
            End Using
            Return True

        Catch ex As Exception
            clsLogger.LogException("IQZipper.ExportToWavFile", ex)
        End Try

        Return False
    End Function


    Private Shared Function IsValidRecordingFile(sFilePath As String) As Boolean
        ' Ensure filename follows UTC format pattern
        Dim sFileName As String = Path.GetFileNameWithoutExtension(sFilePath)
        Dim sPattern As String = "^IQ_Record_\d{8}_\d{6}\.\d{3}Z$" ' Regex pattern

        Return Regex.IsMatch(sFileName, sPattern)
    End Function

    Public Shared Function LoadJsonFromZip(sZipPath As String) As String
        Try
            Using poZip As ZipArchive = ZipFile.OpenRead(sZipPath)
                Dim poEntry As ZipArchiveEntry = poZip.GetEntry(METADATA_FILENAME)
                If poEntry IsNot Nothing Then
                    Using oReader As New StreamReader(poEntry.Open())
                        Return oReader.ReadToEnd()
                    End Using
                End If
            End Using
        Catch ex As Exception
            clsLogger.LogException("IQZipper.LoadJsonFromZip", ex)
        End Try

        Return Nothing ' JSON file not found
    End Function

    Public Shared Function ExtractMetadata(sArchiveFilename As String) As IQArchiveMetadata
        Dim sJson As String = LoadJsonFromZip(sArchiveFilename)
        Dim poIQ As New IQArchiveMetadata
        poIQ.FileName = sArchiveFilename

        Try
            If String.IsNullOrEmpty(sJson) Then Return poIQ

            Dim oJson As JObject = JObject.Parse(sJson)
            poIQ.RecordingTimeUTC = oJson.Value(Of DateTime)("UTC_Start_Time")
            poIQ.CenterFrequency = oJson.Value(Of Double)("Center_Frequency_Hz")
            poIQ.SampleRateHz = oJson.Value(Of Double)("Sample_Rate_Hz")
            poIQ.BufferSizeBytes = oJson.Value(Of Integer)("Buffer_Size_Bytes")
            poIQ.TotalIQBytes = oJson.Value(Of Long)("Total_IQ_Bytes")
            poIQ.RecordingDurationSec = oJson.Value(Of Double)("Recording_Duration_S")
            poIQ.AppSoftwareVer = oJson.Value(Of String)("Software_Version")
            poIQ.UncompressedSize = GetIQFileSize(sArchiveFilename)

        Catch ex As Exception
            clsLogger.LogException("IQZipper.ExtractMetadata", ex)
        End Try

        Return poIQ
    End Function

    Public Shared Function GetIQFileSize(sArchiveFilename As String) As Long
        Using oZip As ZipArchive = ZipFile.OpenRead(sArchiveFilename)
            Dim oEntry As ZipArchiveEntry = oZip.GetEntry(IQDATA_FILENAME)

            If oEntry IsNot Nothing Then
                Return oEntry.Length ' Uncompressed file size
            End If
        End Using

        Return -1 ' File not found
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
            End If

            myIQData.Clear()
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
