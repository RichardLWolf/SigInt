Public Class clsUEH
    ''' <summary>
    ''' Start trapping for Unhandled Exceptions.
    ''' </summary>
    Public Shared Sub StartUEH()
        If Not System.Diagnostics.Debugger.IsAttached Then
            Try
                ' Unhandled exception errors
                AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UnhandledExceptionEventRaised
                ' Application errors
                AddHandler Application.ThreadException, AddressOf ApplicationThreadEventRaised
            Catch poEx As Exception
                clsLogger.LogException("clsUEH.StartUEH", poEx)
            End Try
        Else
            clsLogger.Log("clsUEH.StartUEH", "Windows indicates that an external debugger is attached to this program's process. Application will not trap for unhandled exceptions during this run.")
        End If
    End Sub

    ''' <summary>
    ''' Stop trapping Unhandled Exceptions.
    ''' </summary>
    Public Shared Sub StopUEH()
        If Not System.Diagnostics.Debugger.IsAttached Then
            Try
                RemoveHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UnhandledExceptionEventRaised
                RemoveHandler Application.ThreadException, AddressOf ApplicationThreadEventRaised
            Catch poEx As Exception
                clsLogger.LogException("clsUEH.StopUEH", poEx)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Handles unhandled exceptions occurring in non-UI threads.
    ''' </summary>
    Public Shared Sub UnhandledExceptionEventRaised(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        If e.IsTerminating Then
            Dim poEx As Exception = TryCast(e.ExceptionObject, Exception)
            If poEx IsNot Nothing Then
                clsLogger.LogException("clsUEH.UnhandledExceptionEventRaised", poEx)
                SendDiscordNotification(poEx)
            Else
                clsLogger.Log("clsUEH.UnhandledExceptionEventRaised", "Unhandled exception event raised, exception object was null.")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Handles UI thread exceptions.
    ''' </summary>
    Public Shared Sub ApplicationThreadEventRaised(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
        Dim poEx As Exception = e.Exception
        If poEx IsNot Nothing Then
            clsLogger.LogException("clsUEH.ApplicationThreadEventRaised", poEx)
            SendDiscordNotification(poEx)
        Else
            clsLogger.Log("clsUEH.ApplicationThreadEventRaised", "Unhandled exception event raised, exception object was null.")
        End If
        ' Exit the application immediately
        Application.Exit()
    End Sub

    ''' <summary>
    ''' Sends a Discord notification if configured.
    ''' </summary>
    Private Shared Sub SendDiscordNotification(ByVal poEx As Exception)
        Dim poCfg As clsAppConfig = clsAppConfig.Load()
        If poCfg.DiscordNotifications AndAlso Not String.IsNullOrEmpty(poCfg.DiscordServerWebhook) Then
            Dim sMessage As String = $"SigInt has encountered an unexpected error and has shut down.{vbCrLf}Exception: {poEx.Message}"
            Call modMain.SendDiscordNotification(sMessage, poCfg.DiscordServerWebhook, poCfg.DiscordMentionID)
        End If
    End Sub
End Class
