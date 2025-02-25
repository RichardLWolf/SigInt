Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class ctlRoundTrackbar

    ' Properties
    Private miValue As Integer = 50
    Private miMin As Integer = 0
    Private miMax As Integer = 100
    Private miKnobSize As Integer = 24
    Private miTrackWidth As Integer = 3
    Private miTickSpacing As Integer = 10
    Private miInnerPadding As Integer = 5
    Private mbMouseDown As Boolean = False
    Private mbShowValue As Boolean = False
    Private moTextColor As Color = Color.LightGoldenrodYellow
    Private moOrientation As Orientation = Orientation.Horizontal
    Private miTickSize As Integer = 25 ' Default percentage size of tick marks


    ' Customizable Colors
    <Category("Appearance")>
    Public Property TrackColor As Color = Color.Gray
    <Category("Appearance")>
    Public Property KnobColor As Color = Color.RoyalBlue
    <Category("Appearance")>
    Public Property TickColor As Color = Color.LightGray
    <Category("Appearance")>
    Public Property BackgroundColor As Color = Color.FromArgb(43, 43, 43)

    ' Inner Padding Property
    <Category("Layout")>
    Public Property InnerPadding As Integer
        Get
            Return miInnerPadding
        End Get
        Set(value As Integer)
            miInnerPadding = Math.Max(0, value)
            Me.Invalidate()
        End Set
    End Property

    ' Horizontal or vertical layout
    <Category("Layout")>
    Public Property Orientation As Orientation
        Get
            Return moOrientation
        End Get
        Set(value As Orientation)
            moOrientation = value
            Me.Invalidate()
        End Set
    End Property


    ' Value Property
    <Category("Behavior")>
    Public Property Value As Integer
        Get
            Return miValue
        End Get
        Set(value As Integer)
            miValue = Math.Max(miMin, Math.Min(value, miMax))
            Me.Invalidate()
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Set
    End Property

    ' Min & Max Properties
    <Category("Behavior")>
    Public Property Minimum As Integer
        Get
            Return miMin
        End Get
        Set(value As Integer)
            miMin = value
            If miValue < miMin Then miValue = miMin
            Me.Invalidate()
        End Set
    End Property

    <Category("Behavior")>
    Public Property Maximum As Integer
        Get
            Return miMax
        End Get
        Set(value As Integer)
            miMax = value
            If miValue > miMax Then miValue = miMax
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property TrackWidth As Integer
        Get
            Return miTrackWidth
        End Get
        Set(value As Integer)
            miTrackWidth = value
            Me.Invalidate()
        End Set
    End Property

    ' Tick Mark Spacing
    <Category("Appearance")>
    Public Property TickSpacing As Integer
        Get
            Return miTickSpacing
        End Get
        Set(value As Integer)
            miTickSpacing = Math.Max(1, value)
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property KnobSize As Integer
        Get
            Return miKnobSize
        End Get
        Set(value As Integer)
            miKnobSize = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property ShowValueInKnob As Boolean
        Get
            Return mbShowValue
        End Get
        Set(value As Boolean)
            mbShowValue = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property KnobTextColor As Color
        Get
            Return moTextColor
        End Get
        Set(value As Color)
            moTextColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property TickSize As Integer
        Get
            Return miTickSize
        End Get
        Set(value As Integer)
            miTickSize = Math.Max(1, Math.Min(value, 100)) ' Restrict between 1% and 100%
            Me.Invalidate()
        End Set
    End Property


    ' Events
    Public Event ValueChanged As EventHandler

    ' Constructor
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.DoubleBuffer, True)
        Me.Size = New Size(200, 50)
    End Sub

    ' Paint Override
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim poGfx As Graphics = e.Graphics
        poGfx.SmoothingMode = SmoothingMode.AntiAlias
        poGfx.Clear(BackgroundColor)

        Dim poDrawArea As Rectangle = New Rectangle(InnerPadding, InnerPadding, Width - 2 * InnerPadding, Height - 2 * InnerPadding)
        Dim poKnobRect As Rectangle
        Dim poLighterKnobColor As Color = Color.FromArgb(KnobColor.A, Math.Min(KnobColor.R + 75, 255), Math.Min(KnobColor.G + 75, 255), Math.Min(KnobColor.B + 75, 255))

        If moOrientation = Orientation.Horizontal Then
            ' Horizontal layout
            Dim piTrackLeft As Integer = poDrawArea.Left + (miKnobSize \ 2)
            Dim piTrackRight As Integer = poDrawArea.Right - (miKnobSize \ 2)
            Dim piTrackY As Integer = poDrawArea.Top + (poDrawArea.Height \ 2)
            Dim piTickLength As Integer = CInt((poDrawArea.Height - miTrackWidth) * (miTickSize / 100.0))
            Dim piTickStart As Integer = piTrackY - (piTickLength \ 2)
            Dim piTickEnd As Integer = piTrackY + (piTickLength \ 2)

            ' Draw Track
            Dim poTrackRect As New Rectangle(piTrackLeft, piTrackY - (miTrackWidth \ 2), piTrackRight - piTrackLeft, miTrackWidth)
            Using poTrackBrush As New SolidBrush(TrackColor)
                poGfx.FillRectangle(poTrackBrush, poTrackRect)
            End Using

            ' Draw Tick Marks
            Using poPen As New Pen(TickColor, 1)
                Dim piTickCount As Integer = (miMax - miMin) \ miTickSpacing
                For piIndex As Integer = 0 To piTickCount
                    Dim piTickX As Integer = piTrackLeft + CInt((piIndex * miTickSpacing / (miMax - miMin)) * (piTrackRight - piTrackLeft))
                    poGfx.DrawLine(poPen, New Point(piTickX, piTickStart), New Point(piTickX, piTickEnd))
                Next
            End Using

            ' Draw Knob
            Dim piKnobX As Integer = piTrackLeft + CInt(((miValue - miMin) / (miMax - miMin)) * (piTrackRight - piTrackLeft))
            poKnobRect = New Rectangle(piKnobX - (miKnobSize \ 2), piTrackY - (miKnobSize \ 2), miKnobSize, miKnobSize)
            Using poKnobBrush As New LinearGradientBrush(poKnobRect, poLighterKnobColor, KnobColor, LinearGradientMode.Vertical)
                poGfx.FillEllipse(poKnobBrush, poKnobRect)
            End Using
        Else
            ' Vertical layout
            Dim piTrackTop As Integer = poDrawArea.Top + (miKnobSize \ 2)
            Dim piTrackBottom As Integer = poDrawArea.Bottom - (miKnobSize \ 2)
            Dim piTrackX As Integer = poDrawArea.Left + (poDrawArea.Width \ 2)
            Dim piTickLength As Integer = CInt((poDrawArea.Width - miTrackWidth) * (miTickSize / 100.0))
            Dim piTickStart As Integer = piTrackX - (piTickLength \ 2)
            Dim piTickEnd As Integer = piTrackX + (piTickLength \ 2)

            ' Draw Track
            Dim poTrackRect As New Rectangle(piTrackX - (miTrackWidth \ 2), piTrackTop, miTrackWidth, piTrackBottom - piTrackTop)
            Using poTrackBrush As New SolidBrush(TrackColor)
                poGfx.FillRectangle(poTrackBrush, poTrackRect)
            End Using

            ' Draw Tick Marks
            Using poPen As New Pen(TickColor, 1)
                Dim piTickCount As Integer = (miMax - miMin) \ miTickSpacing
                For piIndex As Integer = 0 To piTickCount
                    Dim piTickY As Integer = piTrackBottom - CInt((piIndex * miTickSpacing / (miMax - miMin)) * (piTrackBottom - piTrackTop))
                    poGfx.DrawLine(poPen, New Point(piTickStart, piTickY), New Point(piTickEnd, piTickY))
                Next
            End Using

            ' Draw Knob
            Dim piKnobY As Integer = piTrackBottom - CInt(((miValue - miMin) / (miMax - miMin)) * (piTrackBottom - piTrackTop))
            poKnobRect = New Rectangle(piTrackX - (miKnobSize \ 2), piKnobY - (miKnobSize \ 2), miKnobSize, miKnobSize)
            Using poKnobBrush As New LinearGradientBrush(poKnobRect, poLighterKnobColor, KnobColor, LinearGradientMode.Vertical)
                poGfx.FillEllipse(poKnobBrush, poKnobRect)
            End Using
        End If

        ' Draw Value Inside Knob (same for both orientations)
        If mbShowValue Then
            Dim psValueText As String = miValue.ToString().Trim
            Using poCustomFont As New Font(Font.FontFamily, miKnobSize * 0.4F, FontStyle.Bold)
                Dim poTextPos As New PointF(poKnobRect.X + (poKnobRect.Width / 2), poKnobRect.Y + (poKnobRect.Height / 2))
                Using poBrush As New SolidBrush(moTextColor)
                    Using poFmt As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                        poGfx.DrawString(psValueText, poCustomFont, poBrush, poTextPos, poFmt)
                    End Using
                End Using
            End Using
        End If
    End Sub


    ' Mouse Handling
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        mbMouseDown = True
        UpdateValue(e.Location)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If mbMouseDown Then
            UpdateValue(e.Location)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        mbMouseDown = False
    End Sub

    ' Update Value Based on Mouse Position
    ' Update Value Based on Mouse Position
    Private Sub UpdateValue(oMousePos As Point)
        If moOrientation = Orientation.Horizontal Then
            Dim piTrackLeft As Integer = InnerPadding + (miKnobSize \ 2)
            Dim piTrackRight As Integer = Width - InnerPadding - (miKnobSize \ 2)
            Dim piNewValue As Integer = miMin + CInt(((oMousePos.X - piTrackLeft) / (piTrackRight - piTrackLeft)) * (miMax - miMin))
            Me.Value = Math.Max(miMin, Math.Min(piNewValue, miMax))
        Else
            Dim piTrackTop As Integer = InnerPadding + (miKnobSize \ 2)
            Dim piTrackBottom As Integer = Height - InnerPadding - (miKnobSize \ 2)
            Dim piNewValue As Integer = miMax - CInt(((oMousePos.Y - piTrackTop) / (piTrackBottom - piTrackTop)) * (miMax - miMin))
            Me.Value = Math.Max(miMin, Math.Min(piNewValue, miMax))
        End If
    End Sub

End Class
