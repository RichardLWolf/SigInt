Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' start the unhandled exception handler
        clsUEH.StartUEH()

    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        clsUEH.StopUEH()
    End Sub
End Class