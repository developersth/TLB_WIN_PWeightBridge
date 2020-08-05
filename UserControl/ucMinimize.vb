Public Class ucMinimize

    Event OnCilckMin()

    Private _picMouseHover As New PictureBox
    Private _picMouseLeave As New PictureBox

#Region "ucMinimize Property"
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

    'Property PicMenuIcon As Bitmap
    '    Get
    '        Return _picIcon.Image
    '    End Get
    '    Set(ByVal value As Bitmap)
    '        _picIcon.Size = PictureBox2.Size
    '        _picIcon.Image = value
    '    End Set
    'End Property
#Region "ucMinimize Events"
    Private Sub DispostObj()
        _picMouseHover.Dispose()
        _picMouseLeave.Dispose()
    End Sub

    Private Sub ucClick()
        RaiseEvent OnCilckMin()
    End Sub
#End Region

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
        _picMouseHover.Image = My.Resources.minimize_h

        _picMouseLeave.Size = PictureBox1.Size
        _picMouseLeave.SizeMode = PictureBoxSizeMode.StretchImage
        _picMouseLeave.Image = My.Resources.minimize_n

        'PictureBox2.Width = PictureBox1.Width / 2
        'PictureBox2.Height = PictureBox1.Height / 2
        PictureBox1.Image = _picMouseLeave.Image
        p.Y = (PictureBox1.Height / 2) - (PictureBox2.Height / 2)
        p.X = (PictureBox1.Width / 2) - (PictureBox2.Width / 2)
        PictureBox2.Location = p

        Me.Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Private Sub ucMinimize_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'DispostObj()
    End Sub

    Private Sub ucMinimize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'PictureBox1.Width = Me.Width
        'PictureBox1.Height = Me.Height
        InitializeObj()
    End Sub

    Private Sub PictureBox1_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseHover
        'PictureBox1.Image = New Bitmap(My.Resources.minimize_h)
        PictureBox1.Image = _picMouseHover.Image
    End Sub

    Private Sub PictureBox1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave
        'PictureBox1.Image = New Bitmap(My.Resources.minimize_n)
        PictureBox1.Image = _picMouseLeave.Image
    End Sub

    Private Sub ucMinimize_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'PictureBox1.Width = Me.Width
        'PictureBox1.Height = Me.Height
        InitializeObj()
    End Sub

    Private Sub PictureBox1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        'RaiseEvent OnCilckMin()
        ucClick()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
