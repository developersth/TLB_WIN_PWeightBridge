<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FDiag
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FDiag))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lblCRComport = New System.Windows.Forms.Label()
        Me.txtCRRecv = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCRSend = New System.Windows.Forms.TextBox()
        Me.ListMain = New System.Windows.Forms.ListBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblWBComport = New System.Windows.Forms.Label()
        Me.txtWBRecv = New System.Windows.Forms.TextBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblCRComport)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtCRRecv)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtCRSend)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListMain)
        Me.SplitContainer1.Panel1.Controls.Add(Me.PictureBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblWBComport)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtWBRecv)
        Me.SplitContainer1.Panel2.Controls.Add(Me.PictureBox2)
        Me.SplitContainer1.Size = New System.Drawing.Size(734, 332)
        Me.SplitContainer1.SplitterDistance = 249
        Me.SplitContainer1.SplitterWidth = 20
        Me.SplitContainer1.TabIndex = 1
        '
        'lblCRComport
        '
        Me.lblCRComport.AutoSize = True
        Me.lblCRComport.Location = New System.Drawing.Point(69, 169)
        Me.lblCRComport.Name = "lblCRComport"
        Me.lblCRComport.Size = New System.Drawing.Size(32, 13)
        Me.lblCRComport.TabIndex = 7
        Me.lblCRComport.Text = "Send"
        '
        'txtCRRecv
        '
        Me.txtCRRecv.Location = New System.Drawing.Point(66, 213)
        Me.txtCRRecv.Name = "txtCRRecv"
        Me.txtCRRecv.ReadOnly = True
        Me.txtCRRecv.Size = New System.Drawing.Size(654, 20)
        Me.txtCRRecv.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 216)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Receive"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 187)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Send"
        '
        'txtCRSend
        '
        Me.txtCRSend.Location = New System.Drawing.Point(66, 187)
        Me.txtCRSend.Name = "txtCRSend"
        Me.txtCRSend.ReadOnly = True
        Me.txtCRSend.Size = New System.Drawing.Size(654, 20)
        Me.txtCRSend.TabIndex = 3
        '
        'ListMain
        '
        Me.ListMain.FormattingEnabled = True
        Me.ListMain.Location = New System.Drawing.Point(66, 3)
        Me.ListMain.Name = "ListMain"
        Me.ListMain.Size = New System.Drawing.Size(657, 160)
        Me.ListMain.TabIndex = 2
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(10, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(45, 43)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'lblWBComport
        '
        Me.lblWBComport.AutoSize = True
        Me.lblWBComport.Location = New System.Drawing.Point(69, 13)
        Me.lblWBComport.Name = "lblWBComport"
        Me.lblWBComport.Size = New System.Drawing.Size(32, 13)
        Me.lblWBComport.TabIndex = 8
        Me.lblWBComport.Text = "Send"
        '
        'txtWBRecv
        '
        Me.txtWBRecv.Location = New System.Drawing.Point(66, 29)
        Me.txtWBRecv.Name = "txtWBRecv"
        Me.txtWBRecv.ReadOnly = True
        Me.txtWBRecv.Size = New System.Drawing.Size(654, 20)
        Me.txtWBRecv.TabIndex = 7
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(10, 6)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(42, 50)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'Timer1
        '
        '
        'FDiag
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 332)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FDiag"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Diagnostic"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtCRRecv As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCRSend As System.Windows.Forms.TextBox
    Friend WithEvents ListMain As System.Windows.Forms.ListBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtWBRecv As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblCRComport As System.Windows.Forms.Label
    Friend WithEvents lblWBComport As System.Windows.Forms.Label
End Class
