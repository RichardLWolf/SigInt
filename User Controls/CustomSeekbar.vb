Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class CustomSeekbar

    ' Event to notify parent form when the value changes (like HScrollBar)
    Public Shadows Event Scroll(ByVal sender As Object, ByVal e As EventArgs)

    ' Scrollbar-like properties
    Private piMinimum As Integer = 0
    Private piMaximum As Integer = 100
    Private piValue As Integer = 0

    Public Property Minimum As Integer
        Get
            Return piMinimum
        End Get
        Set(value As Integer)
            piMinimum = value
            If piValue < piMinimum Then piValue = piMinimum
            Invalidate()
        End Set
    End Property

    Public Property Maximum As Integer
        Get
            Return piMaximum
        End Get
        Set(value As Integer)
            piMaximum = value
            If piValue > piMaximum Then piValue = piMaximum
            Invalidate()
        End Set
    End Property

    Public Property Value As Integer
        Get
            Return piValue
        End Get
        Set(value As Integer)
            piValue = Math.Max(piMinimum, Math.Min(piMaximum, value))
            Invalidate() ' Redraw seekbar
        End Set
    End Property

    Public Sub New()
        Me.DoubleBuffered = True
        Me.Height = 20 ' Default height
        Me.Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim poG As Graphics = e.Graphics
        poG.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        poG.Clear(Me.BackColor)

        ' Define bar dimensions
        Dim piBarY As Integer = Height \ 2
        Dim piBarHeight As Integer = Math.Max(4, Height \ 8)
        Dim piKnobSize As Integer = Math.Max(12, CInt(Height * 0.6))

        ' Convert Value to percentage-based position
        Dim pdSeekPosition As Double = (piValue - piMinimum) / CDbl(piMaximum - piMinimum)

        ' Draw full seek bar (gray background)
        Using poBarBrush As New SolidBrush(Color.Gray)
            poG.FillRectangle(poBarBrush, 0, piBarY - (piBarHeight \ 2), Width, piBarHeight)
        End Using

        ' Draw played portion with gradient (lighter-red → full red → lighter-red)
        Dim piProgressWidth As Integer = CInt(pdSeekPosition * Width)
        If piProgressWidth > 0 Then
            Using poGradientBrush As New LinearGradientBrush(New Rectangle(0, piBarY - (piBarHeight \ 2), piProgressWidth, piBarHeight),
                                                             Color.FromArgb(255, 200, 200),
                                                             Color.Red,
                                                             LinearGradientMode.Vertical)
                ' 3-point gradient
                Dim poBlend As New ColorBlend()
                poBlend.Positions = {0.0F, 0.5F, 1.0F}
                poBlend.Colors = {Color.FromArgb(255, 200, 200), Color.Red, Color.FromArgb(255, 200, 200)}

                poGradientBrush.InterpolationColors = poBlend
                poG.FillRectangle(poGradientBrush, 0, piBarY - (piBarHeight \ 2), piProgressWidth, piBarHeight)
            End Using
        End If

        ' Draw draggable knob (white circle)
        Dim piKnobX As Integer = CInt(pdSeekPosition * Width) - (piKnobSize \ 2)
        Using poKnobBrush As New SolidBrush(Color.CornflowerBlue)
            poG.FillEllipse(poKnobBrush, piKnobX, piBarY - (piKnobSize \ 2), piKnobSize, piKnobSize)
        End Using
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = MouseButtons.Left Then
            UpdateSeekPosition(e.X)
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If e.Button = MouseButtons.Left Then
            UpdateSeekPosition(e.X)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
    End Sub

    Private Sub UpdateSeekPosition(ByVal piMouseX As Integer)
        ' Convert mouse X position to scroll value
        Dim pdNewPosition As Double = Math.Max(0, Math.Min(1, piMouseX / CDbl(Width)))
        Value = CInt(piMinimum + (pdNewPosition * (piMaximum - piMinimum)))
        RaiseEvent Scroll(Me, EventArgs.Empty) ' Fire Scroll event
    End Sub


End Class
