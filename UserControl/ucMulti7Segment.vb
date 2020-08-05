
Imports _7SegmentLib._7Segment

Public Class ucMulti7Segment
    Dim m7Segment As New MultiDigit7Segment

#Region "Property"

    Private _ucColorBackground As Color
    Public Property ucColorBackground() As Color
        Get
            Return m7Segment.ColorBackground
        End Get
        Set(ByVal value As Color)
            m7Segment.ColorBackground = value
        End Set
    End Property

    Private _ucColorDark As Color
    Public Property ucColorDark() As Color
        Get
            Return m7Segment.ColorDark
        End Get
        Set(ByVal value As Color)
            m7Segment.ColorDark = value
        End Set
    End Property

    Private _ucColorLight() As Color
    Public Property ucColorLight() As Color
        Get
            Return m7Segment.ColorLight
        End Get
        Set(ByVal value As Color)
            m7Segment.ColorLight = value
        End Set
    End Property

    Public Property ucValue() As String
        Get
            Return m7Segment.Value
        End Get
        Set(ByVal value As String)
            m7Segment.Value = value
        End Set
    End Property

    'Public Property ucDecimalOn() As Boolean
    '    Get
    '        Return m7Segment.DecimalOn
    '    End Get
    '    Set(ByVal value As Boolean)
    '        m7Segment.DecimalOn = value
    '    End Set
    'End Property

    Public Property ucDecimalShow() As Boolean
        Get
            Return m7Segment.DecimalShow
        End Get
        Set(ByVal value As Boolean)
            m7Segment.DecimalShow = value
        End Set
    End Property

    Public Property ucElementWidth() As Integer
        Get
            Return m7Segment.ElementWidth
        End Get
        Set(ByVal value As Integer)
            m7Segment.ElementWidth = value
        End Set
    End Property

    Public Property ucDigitCount() As Integer
        Get
            Return m7Segment.ArrayCount
        End Get
        Set(ByVal value As Integer)
            m7Segment.ArrayCount = value
        End Set
    End Property

    Public Property ucElementPadding() As Padding
        Get
            Return m7Segment.ElementPadding
        End Get
        Set(ByVal value As Padding)
            m7Segment.ElementPadding = value
        End Set
    End Property
#End Region

    Private Sub ucMulti7Segment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Controls.Add(m7Segment)
    End Sub

    Private Sub ucMulti7Segment_PaddingChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PaddingChanged
        'm7Segment.Padding = Me.Padding
    End Sub

    Private Sub ucMulti7Segment_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        m7Segment.Width = Me.Width
        m7Segment.Height = Me.Height
    End Sub
End Class
