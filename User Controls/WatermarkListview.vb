Imports System.Runtime.InteropServices

Public Class WatermarkListview
    Inherits ListView

    'Structure needed to set the listviews background watermark image
    'Public Structure LVBKIMAGE
    '    Public ulFlags As Int32
    '    Public hbm As IntPtr
    '    Public pszImage As String
    '    Public cchImageMax As Int32
    '    Public xOffsetPercent As Int32
    '    Public yOffsetPercent As Int32
    'End Structure
    <StructLayout(LayoutKind.Sequential)>
    Public Structure LVBKIMAGE
        Public ulFlags As Int32
        Public hbm As IntPtr
        <MarshalAs(UnmanagedType.LPWStr)> Public pszImage As String
        Public cchImageMax As Int32
        Public xOffsetPercent As Int32
        Public yOffsetPercent As Int32
    End Structure

    'Constant Declarations
    Private Const LVM_FIRST As Int32 = &H1000
    Private Const LVM_SETBKIMAGEW As Int32 = (LVM_FIRST + 138)
    Private Const LVBKIF_TYPE_WATERMARK As Int32 = &H10000000

    'API Declarations
    'Private Declare Sub CoInitialize Lib "ole32.dll" (ByVal pvReserved As Int32)
    <DllImport("ole32.dll")>
    Private Shared Function CoInitialize(ByVal pvReserved As IntPtr) As Integer
    End Function
    'Private Declare Sub CoUninitialize Lib "ole32.dll" ()
    <DllImport("ole32.dll")>
    Private Shared Sub CoUninitialize()
    End Sub
    'Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hwnd As Int32, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Overloads Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    ' stuff for set header image
    Private Const LVM_GETHEADER = 4127
    Private Const HDM_SETIMAGELIST = 4616
    Private Const LVM_SETCOLUMN = 4122
    Private Const LVCF_FMT = 1
    Private Const LVCF_IMAGE = 16
    Private Const LVCFMT_IMAGE = 2048
    Private Const LVCFMT_BITMAP_ON_RIGHT = 4096
    Public Enum WindowsArrow
        ArrowUP = 1024
        ArrowDown = 512
    End Enum
    Public Enum HorizontalImageAlignment
        Left = 0
        Right = 4096
    End Enum
    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack:=8, CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Structure LVCOLUMN
        Dim mask As Integer
        Dim fmt As Integer
        Dim cx As Integer
        Dim pszText As IntPtr
        Dim cchTextMax As Integer
        Dim iSubItem As Integer
        Dim iImage As Integer
        Dim iOrder As Integer
    End Structure
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Overloads Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As UInteger,
                                          ByVal wParam As IntPtr, ByRef lParam As LVCOLUMN) As IntPtr
    End Function



    Private vWatermarkImage As Bitmap
    Private vWatermarkAlpha As Integer
    Private foSorter As InternalListviewSorter




    <Configuration.DefaultSettingValue("200")>
    Public Property WatermarkAlpha() As Integer
        Get
            Return vWatermarkAlpha
        End Get
        Set(ByVal value As Integer)
            vWatermarkAlpha = value
            SetBkImage()
        End Set
    End Property

    Public Property WatermarkImage() As Bitmap
        Get
            Return vWatermarkImage
        End Get
        Set(ByVal value As Bitmap)
            vWatermarkImage = value
            SetBkImage()
        End Set
    End Property

    <Configuration.DefaultSettingValue("1"), System.ComponentModel.Browsable(False)>
    Public Property SortDirection As System.Windows.Forms.SortOrder
        Get
            If foSorter IsNot Nothing Then
                Return foSorter.Order
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Windows.Forms.SortOrder)
            If foSorter IsNot Nothing Then
                foSorter.Order = value
            End If
        End Set
    End Property

    <Configuration.DefaultSettingValue("0"), System.ComponentModel.Browsable(False)>
    Public Property SortColumn As Integer
        Get
            If foSorter IsNot Nothing Then
                Return foSorter.SortColumn
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Integer)
            If foSorter IsNot Nothing Then
                foSorter.SortColumn = value
            End If
        End Set
    End Property


    Public Sub InitializeSorter(ByVal ColumnNumber As Integer, ByVal ColSortOrder As System.Windows.Forms.SortOrder,
                                Optional ByVal [ForceStringCompare] As Boolean = False)
        foSorter = New InternalListviewSorter(ColumnNumber, ColSortOrder, [ForceStringCompare])
        Me.ListViewItemSorter = foSorter
    End Sub


    Public Shadows Sub Sort()
        MyBase.Sort()
        If Me.View = View.Details And foSorter IsNot Nothing Then
            If foSorter.Order = System.Windows.Forms.SortOrder.Ascending Then
                Call SetColumnHeaderImage(foSorter.SortColumn, Right, 0, True, , WindowsArrow.ArrowUP)
            Else
                Call SetColumnHeaderImage(foSorter.SortColumn, Right, 0, True, , WindowsArrow.ArrowDown)
            End If
        End If
    End Sub


    Private Sub SetBkImage()
        Try
            If Not WatermarkImage Is Nothing Then
                Dim hBMP As IntPtr = GetBMP(WatermarkImage)
                If Not hBMP = IntPtr.Zero Then
                    Dim lv As New LVBKIMAGE
                    lv.hbm = hBMP
                    lv.ulFlags = LVBKIF_TYPE_WATERMARK
                    Dim lvPTR As IntPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(lv))
                    System.Runtime.InteropServices.Marshal.StructureToPtr(lv, lvPTR, False)
                    SendMessage(Me.Handle, LVM_SETBKIMAGEW, 0, lvPTR)
                    System.Runtime.InteropServices.Marshal.FreeCoTaskMem(lvPTR)
                End If
            Else
                Dim lv As New LVBKIMAGE
                lv.hbm = IntPtr.Zero
                lv.ulFlags = LVBKIF_TYPE_WATERMARK
                Dim lvPTR As IntPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(lv))
                System.Runtime.InteropServices.Marshal.StructureToPtr(lv, lvPTR, False)
                SendMessage(Me.Handle, LVM_SETBKIMAGEW, 0, lvPTR)
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(lvPTR)
            End If
        Catch ex As Exception
            ' failure is ALWYAS an option
        End Try
    End Sub


    Private Function GetBMP(ByVal FromImage As Image) As IntPtr
        Try
            Dim bmp As Bitmap = New Bitmap(FromImage.Width, FromImage.Height)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Me.BackColor)
            g.DrawImage(FromImage, 0, 0, bmp.Width, bmp.Height)
            g.FillRectangle(New SolidBrush(Color.FromArgb(WatermarkAlpha, Me.BackColor.R, Me.BackColor.G, Me.BackColor.B)), New RectangleF(0, 0, bmp.Width, bmp.Height))
            g.Dispose()
            Return bmp.GetHbitmap
            bmp.Dispose()
        Catch ex As Exception
            ' failure is ALWYAS an option
        End Try
    End Function

    Public Sub New()
        Me.WatermarkAlpha = 200
        MyBase.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        CoInitialize(IntPtr.Zero)
        SetBkImage()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        CoUninitialize()
        If vWatermarkImage IsNot Nothing Then
            vWatermarkImage.Dispose()
            vWatermarkImage = Nothing
        End If
        If foSorter IsNot Nothing Then
            foSorter = Nothing
        End If
    End Sub

    Private Shadows Sub cWatermarkListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles Me.ColumnClick
        If Me.View = View.Details And foSorter IsNot Nothing Then
            If e.Column = foSorter.SortColumn Then
                ' Reverse the current sort direction for this column.
                If foSorter.Order = SortOrder.Ascending Then
                    foSorter.Order = SortOrder.Descending
                Else
                    foSorter.Order = SortOrder.Ascending
                End If
            Else
                ' Set the column number that is to be sorted; default to ascending.
                foSorter.SortColumn = e.Column
                foSorter.Order = SortOrder.Ascending
            End If
            ' Perform the sort with these new sort options.
            Me.Sort()
        End If
    End Sub

    Private Sub cWatermarkListView_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleCreated
        SetBkImage()
    End Sub


    Public Sub SetColumnHeaderImage(ByVal Column As Integer,
                ByVal ImageAlignment As HorizontalImageAlignment, ByVal ImageIndex As Integer,
                ByVal UseWinArrows As Boolean, Optional ByVal ImageList1 As ImageList = Nothing,
                Optional ByVal WinArrow As WindowsArrow = Nothing)
        Dim hwnd As IntPtr
        Dim lret As IntPtr
        Dim piCurrCol As Integer
        Dim col As LVCOLUMN

        If UseWinArrows Then
            'The header control includes all columns.
            'Get a handle to the header control.
            hwnd = SendMessage(Me.Handle, LVM_GETHEADER, 0, 0)
            'When using the Windows Arrows there is no need to add imagelist to the header control.
            For piCurrCol = 0 To Me.Columns.Count - 1
                col.mask = LVCF_FMT Or LVCF_IMAGE
                If Column = piCurrCol Then
                    col.fmt = WinArrow + HorizontalImageAlignment.Right
                    col.iImage = ImageIndex
                Else
                    col.fmt = 0
                    col.iImage = -1
                End If
                col.cchTextMax = 0
                col.cx = 0
                col.iOrder = 0
                col.iSubItem = 0
                col.pszText = IntPtr.op_Explicit(0)
                lret = SendMessage(Me.Handle, LVM_SETCOLUMN, piCurrCol, col)
            Next
        Else
            'Assign the ImageList to the header control.
            'The header control includes all columns.
            'Get a handle to the header control.
            hwnd = SendMessage(Me.Handle, LVM_GETHEADER, 0, 0)
            'Add the ImageList to the header control.
            lret = SendMessage(hwnd, HDM_SETIMAGELIST, 0, (ImageList1.Handle).ToInt32)
            For piCurrCol = 0 To Me.Columns.Count - 1
                col.mask = LVCF_FMT Or LVCF_IMAGE
                'The image to use from the Image List.
                If piCurrCol = Column Then
                    col.fmt = LVCFMT_IMAGE + ImageAlignment
                    col.iImage = ImageIndex
                Else
                    col.fmt = 0
                    col.iImage = -1
                End If
                col.cchTextMax = 0
                col.cx = 0
                col.iOrder = 0
                col.iSubItem = 0
                col.pszText = IntPtr.op_Explicit(0)
                'Send the LVM_SETCOLUMN message.
                'The column to which you are assigning the image is defined in the third parameter.
                lret = SendMessage(Me.Handle, LVM_SETCOLUMN, piCurrCol, col)
            Next
        End If
    End Sub


    ''' <summary>
    ''' Returns the first listitem starting from StartingIndex that LIKE pattern matches the supplied string.
    ''' </summary>
    ''' <param name="TextPatternToMatch">Pattern to match against, standard LIKE parameters (?,*,#,[charlist]).</param>
    ''' <param name="IncludeSubItems">True to search subitem text as well.</param>
    ''' <param name="StartingIndex">Index to being search from.</param>
    ''' <returns>Returns the matching listitem or NOTHING.</returns>
    ''' <remarks></remarks>
    Public Function FindItemWithTextLike(ByVal TextPatternToMatch As String, Optional ByVal IncludeSubItems As Boolean = False, Optional ByVal StartingIndex As Integer = 0) As ListViewItem
        Dim piIndex As Integer
        Dim piSub As Integer
        Dim poMatched As ListViewItem = Nothing

        If StartingIndex < Me.Items.Count Then
            For piIndex = StartingIndex To Me.Items.Count - 1
                If Me.Items(piIndex).Text.ToLower Like TextPatternToMatch.ToLower Then
                    poMatched = Me.Items(piIndex)
                Else
                    For piSub = 0 To Me.Items(piIndex).SubItems.Count - 1
                        If Me.Items(piIndex).SubItems(piSub).Text.ToLower Like TextPatternToMatch.ToLower Then
                            poMatched = Me.Items(piIndex)
                            Exit For
                        End If
                    Next
                End If
                If poMatched IsNot Nothing Then
                    Exit For
                End If
            Next
        End If
        Return poMatched
    End Function






End Class



Friend Class InternalListviewSorter
    Implements System.Collections.IComparer

    Private m_ColumnNumber As Integer
    Private m_SortOrder As SortOrder
    Private m_ForceStringCompare As Boolean

    Public Sub New(ByVal column_number As Integer, ByVal _
        sort_order As SortOrder, Optional ByVal [ForceStringCompare] As Boolean = False)
        m_ColumnNumber = column_number
        m_SortOrder = sort_order
        m_ForceStringCompare = [ForceStringCompare]
    End Sub

    ' Compare the items in the appropriate column
    ' for objects x and y.
    Public Function Compare(ByVal x As Object, ByVal y As _
        Object) As Integer Implements _
        System.Collections.IComparer.Compare
        Dim item_x As ListViewItem = DirectCast(x,
            ListViewItem)
        Dim item_y As ListViewItem = DirectCast(y,
            ListViewItem)

        ' Get the sub-item values.
        Dim string_x As String
        If item_x.SubItems.Count <= m_ColumnNumber Then
            string_x = ""
        Else
            string_x = item_x.SubItems(m_ColumnNumber).Text
        End If

        Dim string_y As String
        If item_y.SubItems.Count <= m_ColumnNumber Then
            string_y = ""
        Else
            string_y = item_y.SubItems(m_ColumnNumber).Text
        End If

        ' Compare them.
        If m_ForceStringCompare Then
            If m_SortOrder = SortOrder.Ascending Then
                Return String.Compare(string_x, string_y)
            Else
                Return String.Compare(string_y, string_x)
            End If
        Else
            If m_SortOrder = SortOrder.Ascending Then
                If IsNumeric(string_x) And IsNumeric(string_y) _
                    Then
                    Return Val(string_x).CompareTo(Val(string_y))
                ElseIf IsDate(string_x) And IsDate(string_y) _
                    Then
                    Return DateTime.Parse(string_x).CompareTo(DateTime.Parse(string_y))
                Else
                    Return String.Compare(string_x, string_y)
                End If
            Else
                If IsNumeric(string_x) And IsNumeric(string_y) _
                    Then
                    Return Val(string_y).CompareTo(Val(string_x))
                ElseIf IsDate(string_x) And IsDate(string_y) _
                    Then
                    Return DateTime.Parse(string_y).CompareTo(DateTime.Parse(string_x))
                Else
                    Return String.Compare(string_y, string_x)
                End If
            End If
        End If
    End Function

    Public Property SortColumn() As Integer
        Get
            Return m_ColumnNumber
        End Get
        Set(ByVal Value As Integer)
            m_ColumnNumber = Value
        End Set
    End Property

    Public Property Order() As SortOrder
        Get
            Return m_SortOrder
        End Get
        Set(ByVal Value As SortOrder)
            m_SortOrder = Value
        End Set
    End Property

    Public Property ForceStringCompare() As Boolean
        Get
            ForceStringCompare = m_ForceStringCompare
        End Get
        Set(ByVal value As Boolean)
            m_ForceStringCompare = value
        End Set
    End Property
End Class


