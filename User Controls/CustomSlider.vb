Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class CustomSlider
    Inherits UserControl

    ' Properties
    Private piValue As Integer = 50
    Private piMin As Integer = 0
    Private piMax As Integer = 100
    Private piKnobSize As Integer = 24 ' Increased knob size
    Private pbVertical As Boolean = False
    Private piTrackWidth As Integer = 3 ' Reduced track width
    Private piTickSpacing As Integer = 10
    Private pbMouseDown As Boolean = False
    Private fbShowValue As Boolean = True

    ' Customizable Colors
    Public Property TrackColor As Color = Color.Gray
    Public Property TrackHighlightColor As Color = Color.LightGray ' For slight 3D effect
    Public Property KnobColor As Color = Color.DodgerBlue
    Public Property TextColor As Color = Color.White
    Public Property BackgroundColor As Color = Color.FromArgb(43, 43, 43)
    Public Property TickColor As Color = Color.LightGray

    ' Value Property
    <Category("Behavior")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property Value As Integer
        Get
            Return piValue
        End Get
        Set(value As Integer)
            If value < piMin Then value = piMin
            If value > piMax Then value = piMax
            piValue = value
            Invalidate()
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Set
    End Property

    ' Min & Max Properties
    <Category("Behavior")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property Minimum As Integer
        Get
            Return piMin
        End Get
        Set(value As Integer)
            piMin = value
            If piValue < piMin Then piValue = piMin
            Invalidate()
        End Set
    End Property

    <Category("Behavior")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property Maximum As Integer
        Get
            Return piMax
        End Get
        Set(value As Integer)
            piMax = value
            If piValue > piMax Then piMax = piValue
            Invalidate()
        End Set
    End Property

    ' Tick Mark Spacing Property
    <Category("Appearance")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property TickSpacing As Integer
        Get
            Return piTickSpacing
        End Get
        Set(value As Integer)
            piTickSpacing = Math.Max(1, value)
            Invalidate()
        End Set
    End Property

    ' Orientation Property
    <Category("Appearance")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property Vertical As Boolean
        Get
            Return pbVertical
        End Get
        Set(value As Boolean)
            pbVertical = value
            Invalidate()
        End Set
    End Property

    ' Show/hide the current value inside the knob
    <Category("Appearance")>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <DefaultValue(False)>
    Public Property DisplayValue As Boolean
        Get
            Return fbShowValue
        End Get
        Set(value As Boolean)
            fbShowValue = value
            Invalidate()
        End Set
    End Property



    ' Events
    Public Event ValueChanged As EventHandler

    ' Constructor
    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.DoubleBuffer, True)
        Me.Size = New Size(150, 40)
    End Sub

    ' Paint Override
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim gfx As Graphics = e.Graphics
        gfx.SmoothingMode = SmoothingMode.AntiAlias
        gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        gfx.Clear(BackgroundColor)

        Dim trackRect As Rectangle
        Dim knobRect As Rectangle
        Dim firstTickPos As Integer = piTickSpacing
        Dim lastTickPos As Integer = If(pbVertical, Height - piTickSpacing, Width - piTickSpacing)
        Dim trackPos As Integer = firstTickPos + CInt(((piValue - piMin) / (piMax - piMin)) * (lastTickPos - firstTickPos))


        If pbVertical Then
            trackRect = New Rectangle((Width - piTrackWidth) \ 2, firstTickPos + (piKnobSize \ 2), piTrackWidth, (lastTickPos - firstTickPos) - piKnobSize)
            knobRect = New Rectangle((Width - piKnobSize) \ 2, Math.Max(0, Height - trackPos - (piKnobSize \ 2)), piKnobSize, piKnobSize)
        Else
            trackRect = New Rectangle(firstTickPos + (piKnobSize \ 2), (Height - piTrackWidth) \ 2, (lastTickPos - firstTickPos) - piKnobSize, piTrackWidth)
            knobRect = New Rectangle(Math.Max(0, trackPos - (piKnobSize \ 2)), (Height - piKnobSize) \ 2, piKnobSize, piKnobSize)
        End If

        ' Draw subtle track shadow for 3D effect
        Using brush As New LinearGradientBrush(trackRect, TrackHighlightColor, TrackColor, LinearGradientMode.Vertical)
            gfx.FillRectangle(brush, trackRect)
        End Using

        ' Draw Tick Marks Outside Track
        Using pen As New Pen(TickColor, 1)
            Dim tickCount As Integer = (piMax - piMin) \ piTickSpacing
            For i As Integer = 0 To tickCount
                Dim tickPos As Integer = CInt((i * piTickSpacing / (piMax - piMin)) * (If(pbVertical, Height, Width)))
                If pbVertical Then
                    gfx.DrawLine(pen, New Point((Width - piTrackWidth) \ 2 - 15, Height - tickPos), New Point((Width - piTrackWidth) \ 2 - 10, Height - tickPos))
                    ' Right side tick marks (mirrored)
                    gfx.DrawLine(pen, New Point((Width + piTrackWidth) \ 2 + 10, Height - tickPos), New Point((Width + piTrackWidth) \ 2 + 15, Height - tickPos))
                Else
                    gfx.DrawLine(pen, New Point(tickPos, (Height - piTrackWidth) \ 2 - 15), New Point(tickPos, (Height - piTrackWidth) \ 2 - 10))
                    ' Bottom tick marks (mirrored)
                    gfx.DrawLine(pen, New Point(tickPos, (Height + piTrackWidth) \ 2 + 10), New Point(tickPos, (Height + piTrackWidth) \ 2 + 15))
                End If
            Next
        End Using

        ' Draw Knob
        Using brush As New SolidBrush(KnobColor)
            gfx.FillEllipse(brush, knobRect)
        End Using

        ' Draw Value Inside Knob with Dynamic Font Sizing
        If fbShowValue Then
            Dim valueText As String = piValue.ToString()
            Dim maxTextSize As Single = piKnobSize * 0.5F
            Dim fontSize As Single = maxTextSize
            Dim textSize As SizeF
            Using testFont As New Font(Font.FontFamily, fontSize, FontStyle.Bold)
                textSize = gfx.MeasureString(valueText, testFont)
            End Using
            While textSize.Width > piKnobSize - 4 OrElse textSize.Height > piKnobSize - 4
                fontSize -= 0.5F
                Using testFont As New Font(Font.FontFamily, fontSize, FontStyle.Bold)
                    textSize = gfx.MeasureString(valueText, testFont)
                End Using
                If fontSize < 6 Then Exit While
            End While

            Using customFont As New Font(Font.FontFamily, fontSize, FontStyle.Bold)
                Dim textPos As PointF = New PointF(knobRect.X + (knobRect.Width - textSize.Width) \ 2, knobRect.Y + (knobRect.Height - textSize.Height) \ 2)
                Using brush As New SolidBrush(TextColor)
                    gfx.DrawString(valueText, customFont, brush, textPos)
                End Using
            End Using
        End If

    End Sub

    ' Mouse Handling
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        pbMouseDown = True
        UpdateValue(e.Location)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If pbMouseDown Then
            UpdateValue(e.Location)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        pbMouseDown = False
    End Sub

    ' Update Value Based on Mouse Position
    Private Sub UpdateValue(mousePos As Point)
        Dim newValue As Integer
        If pbVertical Then
            newValue = piMax - CInt((mousePos.Y / Height) * (piMax - piMin))
        Else
            newValue = piMin + CInt((mousePos.X / Width) * (piMax - piMin))
        End If
        Value = newValue
    End Sub

    ' Keyboard Handling
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        Select Case e.KeyCode
            Case Keys.Left, Keys.Down
                Value -= 1
            Case Keys.Right, Keys.Up
                Value += 1
            Case Keys.Home
                Value = piMin
            Case Keys.End
                Value = piMax
        End Select
    End Sub

End Class
