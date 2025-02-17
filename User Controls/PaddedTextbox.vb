Imports System.Windows.Forms
Imports System.ComponentModel


Public Class PaddedTextbox
    Inherits TextBox

    <Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    Private mfPadding As Padding = New Padding(0, 0, 10, 0) ' Default padding (right padding = 10px)

    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property TextPadding As Padding
        Get
            Return mfPadding
        End Get
        Set(value As Padding)
            mfPadding = value
            SetMargins()
            Me.Invalidate() ' Forces a repaint
        End Set
    End Property


    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetMargins()
    End Sub


    Private Sub SetMargins()
        Const EM_SETMARGINS As Integer = &HD3
        Const EC_LEFTMARGIN As Integer = 1
        Const EC_RIGHTMARGIN As Integer = 2

        ' Set both left and right margins
        Dim iMargins As Integer = (mfPadding.Left And &HFFFF) Or (mfPadding.Right << 16)
        SendMessage(Me.Handle, EM_SETMARGINS, EC_LEFTMARGIN Or EC_RIGHTMARGIN, iMargins)
    End Sub


End Class
