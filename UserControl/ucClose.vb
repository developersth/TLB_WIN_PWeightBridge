Public Class ucClose

    Event OnClickClose()

    Private _picMouseHover As New PictureBox
    Private _picMouseLeave As New PictureBox
#Region "ucClose Property"
    'Property MenuPicMouseHover As Bitmap
    '    Get
    '        Return _picMouseHover.Image
    '    End Get
    '    Set(ByVal value As Bitmap)
    '        _picMouseHover.Size = PictureBox1.Size
    '        _picMouseHover.Image = value
    '    End Set

    'End Property

    'Property MenuPicMouseLeave As Bitmap
    '    Get
    '        Return _picMouseLeave.Image
    '    End Get
    '    Set(ByVal value As Bitmap)
    '        _picMouseLeave.Image = value
    '        PictureBox1.Image = _picMouseLeave.Image
    '    End Set

    'End Property
#End Region

#Region "ucClose Events"
    Private Sub DispostObj()
        _picMouseHover.Dispose()
        _picMouseLeave.Dispose()
    End Sub

    Private Sub ucClick()
        RaiseEvent OnClickClose()
    End Sub
#End Region

    Private Sub ucClose_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'DispostObj()
    End Sub

    Private Sub ucClose_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'PictureBox1.Width = Me.Width
        'PictureBox1.Height = Me.Height
        InitializeObj()
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Dim p As Point
        'p.X = 0
        'p.Y = 0
        'PictureBox1.Location = p

        'PictureBox1.Width = Me.Width
        'PictureBox1.Height = Me.Height
        InitializeObj()
        Me.BackColor = Color.Transparent
    End Sub

    Private Sub InitializeObj()
        Dim p As Point
        p.X = 0
        p.Y = 0
        PictureBox1.Location = p

        PictureBox1.Width = Me.Width
        PictureBox1.Height = Me.Height

        _picMouseHover.Size = PictureBox1.Size
        _picMouseHover.SizeMode = PictureBoxSizeMode.StretchImage
        _picMouseHover.Image = My.Resources.close_h

        _picMouseLeave.Size = PictureBox1.Size
        _picMouseLeave.SizeMode = PictureBoxSizeMode.StretchImage
        _picMouseLeave.Image = My.Resources.close_n
        'PictureBox2.Width = PictureBox1.Width / 2
        'PictureBox2.Height = PictureBox1.Height / 2
        PictureBox1.Image = _picMouseLeave.Image
        'p.Y = (PictureBox1.Height / 2) - (PictureBox2.Height / 2)
        'p.X = (PictureBox1.Width / 2) - (PictureBox2.Width / 2)
        'PictureBox2.Location = p
        Me.Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Private Sub ucClose_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'PictureBox1.Width = Me.Width
        'PictureBox1.Height = Me.Height
        InitializeObj()
    End Sub

    Private Sub PictureBox1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        'RaiseEvent OnClickClose()
        ucClick()
    End Sub

    Private Sub PictureBox1_MouseHover1(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseHover
        PictureBox1.Image = _picMouseHover.Image
    End Sub

    Private Sub PictureBox1_MouseLeave1(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.Image = _picMouseLeave.Image
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
