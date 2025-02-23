Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' start the unhandled exception handler
        clsUEH.StartUEH()


    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Stop the unhandled Exception Handler
        clsUEH.StopUEH()
    End Sub

    Private Sub lnkSigIntRepository_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkSigIntRepository.LinkClicked
        Try
            System.Diagnostics.Process.Start("https://github.com/RichardLWolf/SigInt")
        Catch ex As Exception
            MsgBox("Failed to open Windows default browser.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "System Error")
        End Try
    End Sub
End Class