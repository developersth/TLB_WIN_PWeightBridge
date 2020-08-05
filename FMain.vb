Imports CMercuryLib
Imports System.Data
Imports System.Threading
Imports CMercuryLib.MercuryLib
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms

Public Class FMain

    Dim WithEvents mCardReaderWeight As CCardReader
    Public WithEvents mWeightBridge As CWeightBridge
    Public OraDb As New COracle
    Private mLoadHeaderNo As Double
    Private mUnloadNo As String


    Private Sub ExitProgram()
        'If MsgBox("ท่านต้องการที่จะทำการ LogOff ออกจากโปรแกรม Weight Bridge หรือไม่ ?", vbInformation + vbYesNo + vbDefaultButton2) = vbNo Then
        '    Exit Sub
        'End If
        Me.UseWaitCursor = True
        mRunn = False
        mThread.Abort()
        mCardReaderWeight.Dispose()
        mWeightBridge.Dispose()
        mCardReaderWeight = Nothing
        mWeightBridge = Nothing
        mOralcle.Dispose()
        mOralcle = Nothing
        Thread.Sleep(1000)
        End
    End Sub

    Private Sub ClearObject()
        dgvLine.Rows.Clear()
        ClearTextBox()
        lblCardType.Text = ""
    End Sub

    Private Sub ClearTextBox()
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Panel Then
                For Each txt As Control In ctrl.Controls
                    If TypeOf txt Is TextBox Then
                        txt.Text = ""

                    End If
                Next
            End If
        Next
    End Sub

    Private Sub ShowWeightValue()
        Dim vValue As String
        Dim vLen As Integer

        Try
            vValue = mWeightBridge.WeightValue.ToString
            If vValue <> -1 Then
                vLen = UcMulti7Segment1.ucDigitCount - vValue.Length

                vValue = vValue.PadLeft(vLen, " ")
            Else
                vValue = ""
                vValue = vValue.PadLeft(UcMulti7Segment1.ucDigitCount, "-")
            End If
            UcMulti7Segment1.ucValue = vValue
        Catch ex As Exception
            vValue = ""
            vValue = vValue.PadLeft(UcMulti7Segment1.ucDigitCount, "-")
            UcMulti7Segment1.ucValue = vValue
        End Try

    End Sub

    Private Sub Minimize()
        Me.WindowState = FormWindowState.Minimized
    End Sub


#Region "Thread "
    Dim mConnect As Boolean
    Dim mShutdown As Boolean
    Dim mRunn As Boolean
    Dim mThread As Thread

    Public Sub StartThread()
        mRunn = True
        mThread = New Thread(AddressOf RunProcess)
        mThread.Name = Me.Text
        mThread.Start()
    End Sub
    Public Shared Property Current As Cursor
    Private Sub RunProcess()
        While (mRunn)
            'mCardReaderWeight.ReadFromCardReader(spCard.ReadExisting)
            If (OraDb.ConnectStatus()) Then
                mRunn = False
                Thread.Sleep(2000)
                Current = Cursors.WaitCursor

                mWeightBridge = New CWeightBridge(Me)
                mWeightBridge.InitialWeightBridge()
                mWeightBridge.StartThread()

                mCardReaderWeight = New CCardReader(Me)
                mCardReaderWeight.InitialCardReader()
                mCardReaderWeight.StartThread()
                Current = Cursors.Default
            End If
        End While
    End Sub
#End Region

#Region "Database"
    Private Sub ShowDataGrid(ByVal pLoadHeaderNo As Long, ByVal pCardType As Integer)
        Select Case pCardType
            Case 1
                ClearObject()
                ShowDataLoad()

        End Select

    End Sub

    Private Sub ShowDataLoad()
        Dim strSQL As String
        Dim vDataSet As New DataSet
        Dim dt As DataTable

        'mLoadHeaderNo = 510176146
        If mLoadHeaderNo = 0 Then
            Exit Sub
        End If
        lblCardType.Text = "LOAD"
        strSQL = "select t.load_header_no,t.compartment_no,t.preset ,t.preset,t.TU_ID," & _
                         "t.PRODUCT,t.desity15c,t.DESCRIPTION,t.do_no,t.tank_name " & _
                         " from tas.view_oil_load_lines t" & _
                         " where t.load_header_no=" & mLoadHeaderNo & _
                         " Order by t.compartment_no"
        '|^   ลำดับ    |^ หมายเลข DO      |^    หมายเลขผลิตภัณฑ์        |^ ผลิตภัณฑ์        |^ ปริมาณสั่งเติม     |^ ช่องจ่าย  |^     Desity15C  
        Try
            If mOralcle.OpenDys(strSQL, "TableName", vDataSet) Then
                dt = vDataSet.Tables("TableName")
                With dgvLine
                    '.Rows.Clear()
                    If dt.Rows.Count > 1 Then
                        If (.Rows.Count - 1) <> dt.Rows.Count Then
                            .Rows.Clear()
                            .Rows.Add(dt.Rows.Count)
                        End If
                        '.Rows.Add(dt.Rows.Count)
                    End If
                    For i As Integer = 0 To dt.Rows.Count - 1
                        .Rows(i).Cells(0).Value = dt.Rows(i).Item("compartment_no").ToString
                        .Rows(i).Cells(1).Value = dt.Rows(i).Item("do_no").ToString
                        .Rows(i).Cells(2).Value = dt.Rows(i).Item("TU_ID").ToString
                        .Rows(i).Cells(3).Value = dt.Rows(i).Item("PRODUCT").ToString
                        .Rows(i).Cells(4).Value = dt.Rows(i).Item("preset").ToString
                        .Rows(i).Cells(5).Value = dt.Rows(i).Item("tank_name").ToString
                        .Rows(i).Cells(6).Value = dt.Rows(i).Item("desity15c").ToString
                    Next
                End With
            End If

            strSQL = "select" & _
                " h.*,l.DO_NO from view_oil_load_headers h,tas.view_oil_load_lines l" & _
                " where    h.load_header_no = l.Load_Header_No" & _
                " and h.load_header_no=" & mLoadHeaderNo & _
                " and rownum = 1"
            If mOralcle.OpenDys(strSQL, "TableName", vDataSet) Then
                dt = vDataSet.Tables("TableName")
                If dt.Rows.Count > 0 Then
                    'txtCarrier.Text = dt.Rows(0).Item("carrier_name").ToString
                    'txtCustomer.Text = dt.Rows(0).Item("customer_name").ToString
                    txtDoNo.Text = dt.Rows(0).Item("do_no").ToString
                    txtStatus_Sap.Text = dt.Rows(0).Item("status_send_sap").ToString
                    txtDriver.Text = dt.Rows(0).Item("driver_name").ToString
                    txtLoadNo.Text = mLoadHeaderNo
                    txtSeal.Text = dt.Rows(0).Item("seal_number").ToString
                    'txtTareWeight.Text = IIf(IsDBNull(dt.Rows(0).Item("tare_weight")), "", dt.Rows(0).Item("tare_weight"))
                    txtTuCardCode.Text = IIf(IsDBNull(dt.Rows(0).Item("tu_card_no")), "", dt.Rows(0).Item("tu_card_no"))
                    txtTuID.Text = dt.Rows(0).Item("tu_id")

                    txtWeightIn.Text = IIf(IsDBNull(dt.Rows(0).Item("weight_in")), 0, dt.Rows(0).Item("weight_in"))
                    txtWeightOut.Text = IIf(IsDBNull(dt.Rows(0).Item("weight_out")), 0, dt.Rows(0).Item("weight_out"))
                    txtWeightNet.Text = Math.Abs(txtWeightOut.Text - txtWeightIn.Text).ToString
                End If
            End If
        Catch ex As Exception

        End Try

        If vDataSet IsNot Nothing Then vDataSet.Dispose()
        If dt IsNot Nothing Then dt.Dispose()
    End Sub

#End Region

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        MyBase.Dispose()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ExitProgram()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        StartThread()
        'ShowDataGrid(510176146, 1)
    End Sub

    Private Sub Form1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then 'press ESC
            ExitProgram()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Try

            ToolStripStatusLabel1.Text = "Database connect = " & mOralcle.ConnectServiceName & "  " & Now.ToString()
            ShowWeightValue()
            ShowDataLoad()
            'ShowWeightValue()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub mCardReaderWeight_OnMessageCardReader(ByVal pMsg As String) Handles mCardReaderWeight.OnMessageCardReader
        Try
            ToolStripStatusLabel2.Text = pMsg
        Catch ex As Exception
            'ToolStripStatusLabel2.Text = ex.Message
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Minimize()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ExitProgram()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mCardReaderWeight.InitialCardReader()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ShowDataGrid(510020290, 1)
    End Sub

    Private Sub spWeight_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles spWeight.DataReceived
        'Thread.Sleep(100)
        'mWeightBridge.WeightString = spWeight.ReadExisting()
        'spWeight.DiscardInBuffer()
    End Sub

    Private Sub spCard_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles spCard.DataReceived
        'mCardReaderWeight.mCRWeight.MsgRecv = spCard.ReadExisting
        'Thread.Sleep(100)
        'mCardReaderWeight.ReceiveData = spCard.ReadExisting
        'spCard.DiscardInBuffer()
    End Sub

    Private Sub mCardReaderWeight_OnShowDataLoad(ByVal pLoadHeaderNo As Long) Handles mCardReaderWeight.OnShowDataLoad
        mLoadHeaderNo = pLoadHeaderNo
        'If pLoadHeaderNo > 0 Then
        '    ShowDataLoad(pLoadHeaderNo)
        'Else
        '    'ClearObject()
        'End If
    End Sub

    Private Sub mCardReaderWeight_OnShowDataUnload(ByVal pUnloadNo As Long) Handles mCardReaderWeight.OnShowDataUnload
        mUnloadNo = pUnloadNo
        'If pUnloadNo > 0 Then
        '    ShowDataUnload(pUnloadNo)
        'Else
        '    'ClearObject()
        'End If
    End Sub

    Private Sub mCardReaderWeight_OnWriteComport(ByVal pMsgWrite As String) Handles mCardReaderWeight.OnWriteComport
        'spCard.Write(pMsgWrite)
    End Sub

    Private Sub btnDiag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiag.Click
        Dim f As New FDiag
        f.Initial(Me.mWeightBridge, Me.mCardReaderWeight)
        f.ShowDialog()
    End Sub

    Private Sub picMin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Minimize()
    End Sub

    Private Sub picExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ExitProgram()
    End Sub


    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try

        
        spCard.PortName = "COM2"
        With spCard
            .BaudRate = 9600
            .DataBits = 8
            .Parity = IO.Ports.Parity.None
            .StopBits = IO.Ports.StopBits.One
            .Open()
        End With

        Dim m As New CMercuryLib.MercuryLib

        Dim b() As Byte = m.BuildMsg(1, m.MoveCursor(2, 1) & "TEST")
        Dim s As String = ASCIIEncoding.ASCII.GetString(b)
        spCard.Write(b, 0, b.Length)
            b = m.BuildMsg(1, m.ClearDisplay)
            spCard.Write(b, 0, b.Length)
        b = m.BuildMsg(1, m.SendNextQueueBlock)
        's = spCard.ReadExisting()
        spCard.Write(b, 0, b.Length)
            Dim ss As String = spCard.ReadExisting()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UcClose1_OnClickClose() Handles UcClose1.OnClickClose
        ExitProgram()
    End Sub

    Private Sub UcMinimize1_OnCilckMin() Handles UcMinimize1.OnCilckMin
        Minimize()
    End Sub

    Private Sub UcMinimize1_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
