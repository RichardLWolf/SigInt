Imports System.IO

Public Class clsLogger
    Private Shared msLogFile As String = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), My.Application.Info.AssemblyName, "Application_Log.txt")


#Region " Public Shared Methods "

    Public Shared Function LogPath() As String
        Return System.IO.Path.GetDirectoryName(msLogFile)
    End Function

    Public Shared Function LogFileName() As String
        Return msLogFile
    End Function

    Public Shared Function ValidateLogFolder() As Boolean
        Try
            If System.IO.File.Exists(LogPath) Then
                Return True
            Else
                'attempt to create
                System.IO.Directory.CreateDirectory(LogPath)
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function


    ''' <summary>
    ''' Writes directly to the log file, no formatting.
    ''' </summary>
    ''' <param name="Text"></param>
    Public Shared Sub LogPlain(ByVal Text As String)
        Try
            Using poSW As New StreamWriter(msLogFile, True)
                poSW.WriteLine(Text)
                poSW.Close()
            End Using
        Catch ex As Exception
            'don't care
        End Try
    End Sub

    ''' <summary>
    ''' Writes to the log with a timestamp in front of the text
    ''' </summary>
    ''' <param name="Text"></param>
    Public Shared Sub Log(ByVal Text As String)
        Try
            Using poSW As New StreamWriter(msLogFile, True)
                Text = "[" & Format(Now(), "MM/dd/yyyy hh:mm:ss") & "] - " & Text
                poSW.WriteLine(Text)
                poSW.Close()
            End Using
        Catch ex As System.Exception
            'don't care
        End Try
    End Sub

    ''' <summary>
    ''' Write to log with timestamp containing supplied module name in front of text.
    ''' </summary>
    ''' <param name="ModuleName"></param>
    ''' <param name="Text"></param>
    Public Shared Sub Log(ByVal ModuleName As String, ByVal Text As String)
        Try
            Using poSW As New StreamWriter(msLogFile, True)
                Text = "[" & Format(Now(), "MM/dd/yyyy hh:mm:ss") & $"|{ModuleName}] - " & Text
                poSW.WriteLine(Text)
                poSW.Close()
            End Using
        Catch ex As System.Exception
            'don't care
        End Try
    End Sub


    ''' <summary>
    ''' Writes supplied exception (and inner Ex if there) to the log file with a timestamp containing the supplied module name.
    ''' </summary>
    ''' <param name="ModuleName"></param>
    ''' <param name="Ex"></param>
    Public Shared Sub LogException(ByVal ModuleName As String, ByVal Ex As Exception)
        Try
            Using poSW As New StreamWriter(msLogFile, True)
                Dim poSB As New System.Text.StringBuilder
                poSB.Append("[").Append(Format(Now(), "MM/dd/yyyy hh:mm:ss")).Append($"|{ModuleName}] RTE: ")
                If Ex IsNot Nothing Then
                    poSB.Append(Ex.Message)
                    If Ex.InnerException IsNot Nothing Then
                        poSB.Append($"  (Inner Ex: {Ex.InnerException.Message})")
                    End If
                Else
                    poSB.Append("No exception object was received.")
                End If
                poSW.WriteLine(poSB.ToString)
                poSW.Close()
            End Using
        Catch SysEx As System.Exception
            ' don't care
        End Try
    End Sub

    ''' <summary>
    ''' Removes all data from the log file.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function ClearLog() As Boolean
        Try
            Kill(msLogFile)
            Return True
        Catch ex As System.Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Purges the log to nearest line based on supplied MaxLogSize
    ''' </summary>
    ''' <param name="MaxLogSize"></param>
    ''' <returns></returns>
    Public Shared Function PurgeLog(Optional ByVal MaxLogSize As Long = 100000) As Boolean
        Dim plFileSize As Long
        Try
            If System.IO.File.Exists(msLogFile) Then
                plFileSize = New System.IO.FileInfo(msLogFile).Length
                MaxLogSize = Math.Max(100000, MaxLogSize)
                If plFileSize > MaxLogSize Then
                    Dim psAllText As String = System.IO.File.ReadAllText(msLogFile)
                    Dim piOffset As Integer = psAllText.Length - MaxLogSize
                    Dim piStart As Integer = 0
                    For piIndex = piOffset To 0 Step -1
                        If psAllText.Substring(piIndex, 2) = vbCrLf Then
                            piStart = piIndex + 2
                            Exit For
                        End If
                    Next
                    System.IO.File.WriteAllText(msLogFile, psAllText.Substring(piStart))
                End If
            End If

        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function


    Public Shared Function LoadLog() As List(Of String)
        Dim poLog As New List(Of String)
        Try
            If System.IO.File.Exists(msLogFile) Then
                poLog = System.IO.File.ReadAllLines(msLogFile).ToList
            End If
        Catch ex As Exception
            ' don't care, just exit out
        End Try
        Return poLog
    End Function

#End Region

End Class
