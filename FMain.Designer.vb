<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMain
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FMain))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.spCard = New System.IO.Ports.SerialPort(Me.components)
        Me.spWeight = New System.IO.Ports.SerialPort(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvLine = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblCardType = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSeal = New System.Windows.Forms.TextBox()
        Me.txtWeightNet = New System.Windows.Forms.TextBox()
        Me.txtWeightOut = New System.Windows.Forms.TextBox()
        Me.txtWeightIn = New System.Windows.Forms.TextBox()
        Me.txtDriver = New System.Windows.Forms.TextBox()
        Me.txtTuCardCode = New System.Windows.Forms.TextBox()
        Me.txtLoadNo = New System.Windows.Forms.TextBox()
        Me.txtTuID = New System.Windows.Forms.TextBox()
        Me.txtDoNo = New System.Windows.Forms.TextBox()
        Me.txtStatus_Sap = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnDiag = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.UcMinimize1 = New PWeightBridge.ucMinimize()
        Me.UcClose1 = New PWeightBridge.ucClose()
        Me.UcMulti7Segment1 = New PWeightBridge.ucMulti7Segment()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.dgvLine, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.Transparent
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 746)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1024, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.ForeColor = System.Drawing.Color.White
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.RightToLeftAutoMirrorImage = True
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(321, 17)
        Me.ToolStripStatusLabel1.Text = "???"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.ForeColor = System.Drawing.Color.White
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(120, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'spCard
        '
        '
        'spWeight
        '
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label1.Location = New System.Drawing.Point(119, 158)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(137, 42)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Label1"
        Me.Label1.Visible = False
        '
        'dgvLine
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvLine.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLine.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7})
        Me.dgvLine.Location = New System.Drawing.Point(29, 561)
        Me.dgvLine.Name = "dgvLine"
        Me.dgvLine.RowHeadersVisible = False
        Me.dgvLine.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvLine.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLine.Size = New System.Drawing.Size(958, 129)
        Me.dgvLine.TabIndex = 7
        '
        'Column1
        '
        Me.Column1.HeaderText = "Comp No."
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        Me.Column2.HeaderText = "หมายเลข DO"
        Me.Column2.Name = "Column2"
        '
        'Column3
        '
        Me.Column3.HeaderText = "ทะเบียนรถ"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 120
        '
        'Column4
        '
        Me.Column4.HeaderText = "ผลิตภัณฑ์"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 200
        '
        'Column5
        '
        Me.Column5.HeaderText = "ปริมาณสั่งเติม"
        Me.Column5.Name = "Column5"
        '
        'Column6
        '
        Me.Column6.HeaderText = "ชื่อถังจ่าย"
        Me.Column6.Name = "Column6"
        '
        'Column7
        '
        Me.Column7.HeaderText = "Desity15C"
        Me.Column7.Name = "Column7"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label2.Location = New System.Drawing.Point(71, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 18)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "สถานะ SAP"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label3.Location = New System.Drawing.Point(59, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 18)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "หมายเลข DO"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label4.Location = New System.Drawing.Point(81, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 18)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "ทะเบียนรถ"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCardType
        '
        Me.lblCardType.AutoSize = True
        Me.lblCardType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblCardType.Location = New System.Drawing.Point(25, 8)
        Me.lblCardType.Name = "lblCardType"
        Me.lblCardType.Size = New System.Drawing.Size(90, 20)
        Me.lblCardType.TabIndex = 4
        Me.lblCardType.Text = "Card Type"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label11.Location = New System.Drawing.Point(5, 384)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(114, 18)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "รายละเอียดช่องเติม"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtSeal)
        Me.Panel1.Controls.Add(Me.txtWeightNet)
        Me.Panel1.Controls.Add(Me.txtWeightOut)
        Me.Panel1.Controls.Add(Me.txtWeightIn)
        Me.Panel1.Controls.Add(Me.txtDriver)
        Me.Panel1.Controls.Add(Me.txtTuCardCode)
        Me.Panel1.Controls.Add(Me.txtLoadNo)
        Me.Panel1.Controls.Add(Me.txtTuID)
        Me.Panel1.Controls.Add(Me.txtDoNo)
        Me.Panel1.Controls.Add(Me.txtStatus_Sap)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.Label19)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.lblCardType)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(29, 203)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(958, 340)
        Me.Panel1.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
        Me.Label5.Location = New System.Drawing.Point(520, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 18)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "หมายเลข Load"
        '
        'txtSeal
        '
        Me.txtSeal.BackColor = System.Drawing.Color.White
        Me.txtSeal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtSeal.Location = New System.Drawing.Point(154, 151)
        Me.txtSeal.Name = "txtSeal"
        Me.txtSeal.ReadOnly = True
        Me.txtSeal.Size = New System.Drawing.Size(303, 26)
        Me.txtSeal.TabIndex = 37
        Me.txtSeal.Text = "0"
        Me.txtSeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtWeightNet
        '
        Me.txtWeightNet.BackColor = System.Drawing.Color.White
        Me.txtWeightNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtWeightNet.Location = New System.Drawing.Point(625, 229)
        Me.txtWeightNet.Name = "txtWeightNet"
        Me.txtWeightNet.ReadOnly = True
        Me.txtWeightNet.Size = New System.Drawing.Size(303, 26)
        Me.txtWeightNet.TabIndex = 36
        Me.txtWeightNet.Text = "0"
        Me.txtWeightNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtWeightOut
        '
        Me.txtWeightOut.BackColor = System.Drawing.Color.White
        Me.txtWeightOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtWeightOut.Location = New System.Drawing.Point(625, 190)
        Me.txtWeightOut.Name = "txtWeightOut"
        Me.txtWeightOut.ReadOnly = True
        Me.txtWeightOut.Size = New System.Drawing.Size(303, 26)
        Me.txtWeightOut.TabIndex = 35
        Me.txtWeightOut.Text = "0"
        Me.txtWeightOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtWeightIn
        '
        Me.txtWeightIn.BackColor = System.Drawing.Color.White
        Me.txtWeightIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtWeightIn.Location = New System.Drawing.Point(625, 151)
        Me.txtWeightIn.Name = "txtWeightIn"
        Me.txtWeightIn.ReadOnly = True
        Me.txtWeightIn.Size = New System.Drawing.Size(303, 26)
        Me.txtWeightIn.TabIndex = 34
        Me.txtWeightIn.Text = "0"
        Me.txtWeightIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDriver
        '
        Me.txtDriver.BackColor = System.Drawing.Color.White
        Me.txtDriver.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtDriver.Location = New System.Drawing.Point(625, 112)
        Me.txtDriver.Name = "txtDriver"
        Me.txtDriver.ReadOnly = True
        Me.txtDriver.Size = New System.Drawing.Size(303, 26)
        Me.txtDriver.TabIndex = 31
        Me.txtDriver.Text = "0"
        Me.txtDriver.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTuCardCode
        '
        Me.txtTuCardCode.BackColor = System.Drawing.Color.White
        Me.txtTuCardCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtTuCardCode.Location = New System.Drawing.Point(625, 73)
        Me.txtTuCardCode.Name = "txtTuCardCode"
        Me.txtTuCardCode.ReadOnly = True
        Me.txtTuCardCode.Size = New System.Drawing.Size(303, 26)
        Me.txtTuCardCode.TabIndex = 30
        Me.txtTuCardCode.Text = "0"
        Me.txtTuCardCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtLoadNo
        '
        Me.txtLoadNo.BackColor = System.Drawing.Color.White
        Me.txtLoadNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtLoadNo.Location = New System.Drawing.Point(625, 34)
        Me.txtLoadNo.Name = "txtLoadNo"
        Me.txtLoadNo.ReadOnly = True
        Me.txtLoadNo.Size = New System.Drawing.Size(303, 26)
        Me.txtLoadNo.TabIndex = 29
        Me.txtLoadNo.Text = "0"
        Me.txtLoadNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTuID
        '
        Me.txtTuID.BackColor = System.Drawing.Color.White
        Me.txtTuID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtTuID.Location = New System.Drawing.Point(155, 112)
        Me.txtTuID.Name = "txtTuID"
        Me.txtTuID.ReadOnly = True
        Me.txtTuID.Size = New System.Drawing.Size(303, 26)
        Me.txtTuID.TabIndex = 22
        Me.txtTuID.Text = "0"
        Me.txtTuID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDoNo
        '
        Me.txtDoNo.BackColor = System.Drawing.Color.White
        Me.txtDoNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtDoNo.Location = New System.Drawing.Point(155, 73)
        Me.txtDoNo.Name = "txtDoNo"
        Me.txtDoNo.ReadOnly = True
        Me.txtDoNo.Size = New System.Drawing.Size(303, 26)
        Me.txtDoNo.TabIndex = 21
        Me.txtDoNo.Text = "0"
        Me.txtDoNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStatus_Sap
        '
        Me.txtStatus_Sap.BackColor = System.Drawing.Color.White
        Me.txtStatus_Sap.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtStatus_Sap.Location = New System.Drawing.Point(155, 34)
        Me.txtStatus_Sap.Name = "txtStatus_Sap"
        Me.txtStatus_Sap.ReadOnly = True
        Me.txtStatus_Sap.Size = New System.Drawing.Size(303, 26)
        Me.txtStatus_Sap.TabIndex = 20
        Me.txtStatus_Sap.Text = "0"
        Me.txtStatus_Sap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label12.Location = New System.Drawing.Point(53, 155)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(92, 18)
        Me.Label12.TabIndex = 19
        Me.Label12.Text = "หมายเลข Seal"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label13.Location = New System.Drawing.Point(487, 229)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(119, 18)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "น้ำหนักสุทธิ(การชั่ง)"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label14.Location = New System.Drawing.Point(563, 190)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label14.Size = New System.Drawing.Size(46, 18)
        Me.Label14.TabIndex = 17
        Me.Label14.Text = "ชั่งออก"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label15.Location = New System.Drawing.Point(569, 151)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label15.Size = New System.Drawing.Size(40, 18)
        Me.Label15.TabIndex = 16
        Me.Label15.Text = "ชั่งเข้า"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label18.Location = New System.Drawing.Point(531, 116)
        Me.Label18.Name = "Label18"
        Me.Label18.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label18.Size = New System.Drawing.Size(88, 18)
        Me.Label18.TabIndex = 13
        Me.Label18.Text = "พนักงานขับรถ"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label19.Location = New System.Drawing.Point(536, 77)
        Me.Label19.Name = "Label19"
        Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label19.Size = New System.Drawing.Size(83, 18)
        Me.Label19.TabIndex = 12
        Me.Label19.Text = "หมายเลขบัตร"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(896, 139)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 34)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'btnDiag
        '
        Me.btnDiag.BackgroundImage = CType(resources.GetObject("btnDiag.BackgroundImage"), System.Drawing.Image)
        Me.btnDiag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDiag.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnDiag.Location = New System.Drawing.Point(694, 172)
        Me.btnDiag.Name = "btnDiag"
        Me.btnDiag.Size = New System.Drawing.Size(26, 25)
        Me.btnDiag.TabIndex = 10
        Me.btnDiag.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(796, 139)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(94, 34)
        Me.Button2.TabIndex = 14
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'UcMinimize1
        '
        Me.UcMinimize1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UcMinimize1.BackColor = System.Drawing.Color.Transparent
        Me.UcMinimize1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.UcMinimize1.Location = New System.Drawing.Point(896, -4)
        Me.UcMinimize1.Name = "UcMinimize1"
        Me.UcMinimize1.Size = New System.Drawing.Size(63, 51)
        Me.UcMinimize1.TabIndex = 16
        '
        'UcClose1
        '
        Me.UcClose1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UcClose1.BackColor = System.Drawing.Color.Transparent
        Me.UcClose1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.UcClose1.Location = New System.Drawing.Point(947, -3)
        Me.UcClose1.Name = "UcClose1"
        Me.UcClose1.Size = New System.Drawing.Size(68, 51)
        Me.UcClose1.TabIndex = 15
        '
        'UcMulti7Segment1
        '
        Me.UcMulti7Segment1.Location = New System.Drawing.Point(262, 113)
        Me.UcMulti7Segment1.Name = "UcMulti7Segment1"
        Me.UcMulti7Segment1.Size = New System.Drawing.Size(427, 84)
        Me.UcMulti7Segment1.TabIndex = 11
        Me.UcMulti7Segment1.ucColorBackground = System.Drawing.Color.Black
        Me.UcMulti7Segment1.ucColorDark = System.Drawing.Color.Black
        Me.UcMulti7Segment1.ucColorLight = System.Drawing.Color.Red
        Me.UcMulti7Segment1.ucDecimalShow = True
        Me.UcMulti7Segment1.ucDigitCount = 6
        Me.UcMulti7Segment1.ucElementPadding = New System.Windows.Forms.Padding(4, 4, 12, 4)
        Me.UcMulti7Segment1.ucElementWidth = 10
        Me.UcMulti7Segment1.ucValue = "------"
        '
        'FMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.PWeightBridge.My.Resources.Resources.BG_SUB
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1024, 768)
        Me.Controls.Add(Me.UcMinimize1)
        Me.Controls.Add(Me.UcClose1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.UcMulti7Segment1)
        Me.Controls.Add(Me.btnDiag)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.dgvLine)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WEIGHT BRIDGE"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.dgvLine, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents spCard As System.IO.Ports.SerialPort
    Friend WithEvents spWeight As System.IO.Ports.SerialPort
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvLine As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblCardType As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtTuID As System.Windows.Forms.TextBox
    Friend WithEvents txtDoNo As System.Windows.Forms.TextBox
    Friend WithEvents txtStatus_Sap As System.Windows.Forms.TextBox
    Friend WithEvents txtSeal As System.Windows.Forms.TextBox
    Friend WithEvents txtWeightNet As System.Windows.Forms.TextBox
    Friend WithEvents txtWeightOut As System.Windows.Forms.TextBox
    Friend WithEvents txtWeightIn As System.Windows.Forms.TextBox
    Friend WithEvents txtDriver As System.Windows.Forms.TextBox
    Friend WithEvents txtTuCardCode As System.Windows.Forms.TextBox
    Friend WithEvents txtLoadNo As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnDiag As System.Windows.Forms.Button
    Friend WithEvents UcMulti7Segment1 As PWeightBridge.ucMulti7Segment
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents UcClose1 As PWeightBridge.ucClose
    Friend WithEvents UcMinimize1 As PWeightBridge.ucMinimize
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
