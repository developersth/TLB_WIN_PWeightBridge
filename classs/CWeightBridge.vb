Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class CWeightBridge : Implements IDisposable
    Event OnMessageWeightBridge(ByVal pMsg As String)

    Structure _WeightBridge
        Dim ID As Long
        Dim ComportID As Integer
        Dim ComportNo As String
        Dim ComportSetting As String
        Dim Name As String
        Dim IsEnable As Boolean
        Dim IsConnect As Boolean
        Dim nWeightValue As String
        Dim oWeightValue As String
        Dim WeightStatus As Integer
        Dim CardReaderID As Long
        Dim unit As String
        Dim WeightString As String
        Dim ErrCount As Integer

    End Structure

    Public mWeight As _WeightBridge
    Private mLog As New CLog

    Dim mPort As CPort
    Dim mForm As FMain
    Private mList As New List(Of String)

    Public ReadOnly Property MyListDataSource As List(Of String)
        Get
            Return mList
        End Get
    End Property
    Private Sub AddList(ByVal pMsg As String)
        If mList.Count > 1000 Then
            mList.Clear()
        End If
        mList.Insert(0, Now & "->" & pMsg)
    End Sub
    Private Sub WriteLogWeightBridge(ByVal pMsg As String)
        RaiseEvent OnMessageWeightBridge(Now + pMsg)
        mLog.WriteLog(mWeight.Name, pMsg)
        AddList(pMsg)
    End Sub

#Region "Thread"
    Dim mConnect As Boolean
    Dim mShutdown As Boolean
    Dim mRunn As Boolean
    Dim mThread As Thread

    Public Sub StartThread()
        mRunn = True
        mThread = New Thread(AddressOf RunProcess)
        mThread.Name = mWeight.Name
        mThread.Start()
    End Sub

    Private Sub RunProcess()
        WriteLogWeightBridge("Initial Weight Bridge[" & mWeight.ID & "]" & _
                                      "" & mWeight.Name & _
                                      " successful.")
        WriteLogWeightBridge("Weight Bridge enable=" & mWeight.IsEnable.ToString)
        InitialWeightBridgeComport()
        While mRunn
            If mWeight.IsEnable Then
                ProcessWeightBridge()
            Else
                InitialWeightBridge()
                If mWeight.IsEnable Then
                    WriteLogWeightBridge("Weight Bridge enable=" & mWeight.IsEnable.ToString)
                End If
            End If
            Thread.Sleep(1000)
        End While
    End Sub
#End Region

#Region "Serial Port"

    Public Sub InitialWeightBridgeComport()
        WriteLogWeightBridge("Open comport[" + mWeight.ComportNo + "]")
        OpenPort()
        'mPort = New CPort(mForm.spWeight)
        'mPort.InitialPort(mWeight.ComportNo, mWeight.ComportSetting)
        'mPort.StartThread()
        'Thread.Sleep(1000)
    End Sub

    Private Sub OpenPort()
        Try
            'mSp = New Ports.SerialPort
            With mForm.spWeight
                Dim sPli() As String = mWeight.ComportSetting.Split(",")
                .PortName = mWeight.ComportNo
                .BaudRate = sPli(0)
                Select Case sPli(1).ToString.ToUpper()
                    Case "E" : .Parity = Ports.Parity.Even
                    Case "M" : .Parity = Ports.Parity.Mark
                    Case "O" : .Parity = Ports.Parity.Odd
                    Case "S" : .Parity = Ports.Parity.Space
                    Case Else : .Parity = Ports.Parity.None
                End Select
                .DataBits = sPli(2)
                Select Case sPli(3).Trim
                    Case "1" : .StopBits = Ports.StopBits.One
                    Case "1.5" : .StopBits = Ports.StopBits.OnePointFive
                    Case "2" : .StopBits = Ports.StopBits.Two
                    Case Else : .StopBits = Ports.StopBits.None
                End Select
            End With

            mForm.spWeight.ReadTimeout = 500
            mForm.spWeight.WriteTimeout = 1000
            mForm.spWeight.Open()
            mConnect = True
            mWeight.IsConnect = True
            UPDATE_WEIGHTBRIDGE_CONNECT(1)
            'RaiseEvent OnMessageComport("Open comport[" + mArgumentPort.portNo + " : " + mArgumentPort.portSetting + "] successfull.")
            WriteLogWeightBridge("Open comport[" + mWeight.ComportNo + " : " + mWeight.ComportSetting + "] successfull.")
            mWeight.IsEnable = True
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "WeightBridge"
    Public Property WeightString
        Get
            Return mWeight.WeightString
        End Get
        Set(ByVal value)
            mWeight.WeightString = value
        End Set
    End Property

    Private Sub ProcessWeightBridge()
        Dim vRecv As String = ""
        While mRunn
            Try
                vRecv = mForm.spWeight.ReadExisting()
                mForm.spWeight.DiscardInBuffer()
                SplitWeight(vRecv)
                ShowWeightValue()
                If mWeight.nWeightValue <> mWeight.oWeightValue Then
                    mWeight.oWeightValue = mWeight.nWeightValue
                    Thread.Sleep(1000)
                Else
                    Thread.Sleep(3000)
                End If
            Catch ex As Exception
                If Not mForm.spWeight.IsOpen Then
                    OpenPort()
                End If
            End Try
        End While
    End Sub

    Private Sub ShowWeightValue()
        Dim vValue As String
        Dim vLen As Integer

        Try
            GetWeightBridgeValue()
            vValue = WeightValue.ToString
            If vValue <> -1 Then
                vLen = mForm.UcMulti7Segment1.ucDigitCount - vValue.Length
                vValue = vValue.PadLeft(vLen, " ")
            Else
                vValue = ""
                vValue = vValue.PadLeft(mForm.UcMulti7Segment1.ucDigitCount, "-")
            End If
            mForm.UcMulti7Segment1.ucValue = vValue
        Catch ex As Exception
            vValue = ""
            vValue = vValue.PadLeft(mForm.UcMulti7Segment1.ucDigitCount, "-")
            mForm.UcMulti7Segment1.ucValue = vValue
        End Try

    End Sub

    Public ReadOnly Property WeightValue As String
        Get
            Return mWeight.nWeightValue
        End Get

    End Property

    Public ReadOnly Property WeightStatus
        Get
            Return mWeight.WeightStatus
        End Get

    End Property

    Public Property WeightCardReaderID
        Get
            Return mWeight.CardReaderID
        End Get
        Set(ByVal value)
            mWeight.CardReaderID = value
        End Set
    End Property

    Public ReadOnly Property WeightBridgeID As String
        Get
            Return mWeight.ID
        End Get

    End Property


    Private Sub SplitWeight(ByRef pRecv As String)
        Dim vStr1() As String
        Dim vStr2() As String
        Try
            'pRecv = "0     00    00)0     00    00)0     60    00)0     60    00)0     60    00)0     60    00)0     60    00)0     60    00)0     00    00)0     70    00)"


            

            'mWeight.WeightString = "50000"
            'UPDATE_WB_VALUE(mWeight.WeightString)

            vStr1 = Split(pRecv, ")0")

            If vStr1.Length > 0 Then
                vStr1 = Split(pRecv, ")0")
                vStr2 = Split(vStr1(1), " ")
                mWeight.WeightString = vStr1(1).Substring(1, 7).Trim()
                UPDATE_WB_VALUE(mWeight.WeightString)
                mWeight.WeightString = mWeight.WeightString
            End If

            If mWeight.IsConnect = False Then
                mWeight.IsConnect = True
                mWeight.IsEnable = True
                UPDATE_WEIGHTBRIDGE_CONNECT(1)
            End If
        Catch ex As Exception
            mWeight.nWeightValue = -1
            mWeight.WeightString = ""
            UPDATE_WB_VALUE(-1)
            If mWeight.IsConnect = True Then
                mWeight.IsConnect = False
                mWeight.IsEnable = False
                UPDATE_WEIGHTBRIDGE_CONNECT(0)
            End If
        End Try

    End Sub
#End Region

#Region "Database"
    Public Sub InitialWeightBridge()

        Dim vIni As New CINI

        'mWeight.ID = vIni.INIRead(mPathIni, "WEIGHT_ID", "WID")

        'WriteLogWeightBridge("Weight Bridge enable=" & mWeight.IsEnable.ToString)

        'Dim strSQL As String = "select t.weight_bridge_id,t.weight_bridge_name,t.is_enabled" & _
        '                     " ,t.comp_id,t.comport_no,t.comport_setting" & _
        '                     " from tas.view_wb_initail t " & _
        '                     " where t.card_reader_id=" & mWeight.CardReaderID
        Dim strSQL As String = "select WEIGHT_BRIDGE_ID,WEIGHT_BRIDGE_NAME " & _
                     ",COMP_ID,COMPORT_NO,COMPORT_SETTING,IS_ENABLED" & _
                     " from tas.view_wb_comport " & _
                     " where rownum = 1"
        Dim vDataSet As New DataSet
        Dim dt As DataTable
        Try
            If mOralcle.OpenDys(strSQL, "TableName", vDataSet) Then
                dt = vDataSet.Tables(0)
                If dt.Rows.Count > 0 Then
                    mWeight.ComportID = dt.Rows(0).Item("comp_id").ToString
                    mWeight.ComportNo = dt.Rows(0).Item("comport_no").ToString
                    mWeight.ComportSetting = dt.Rows(0).Item("comport_setting").ToString
                    mWeight.ID = dt.Rows(0).Item("weight_bridge_id")
                    mWeight.IsEnable = dt.Rows(0).Item("is_enabled").ToString
                    mWeight.Name = dt.Rows(0).Item("weight_bridge_name")
                End If
            End If
        Catch ex As Exception

        End Try

        vIni = Nothing
        If vDataSet IsNot Nothing Then vDataSet.Dispose()
        If dt IsNot Nothing Then dt.Dispose()
    End Sub

    Public Function UPDATE_WB_VALUE(ByVal pWeight As Integer) As Boolean
        Dim strSQL As String = "begin steqi.UPDATE_WB_VALUE(" & _
                            mWeight.ID & "," & pWeight & _
                            ");end;"
        Try
            Return mOralcle.ExeSQL(strSQL)
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function UPDATE_WEIGHTBRIDGE_CONNECT(ByVal pStatus As Integer) As Boolean
        'Dim strSQL As String = "begin tas.UPDATE_WEIGHTBRIDGE_CONNECT(" & _
        '                    mWeight.ID & "," & pStatus & _
        '                    ");end;"
        Dim strSQL As String
        strSQL = "BEGIN TAS.UPDATE_WB_CONNECT(" & mWeight.ID & "," & pStatus & "); END;"

        Try
            Return mOralcle.ExeSQL(strSQL)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub GetWeightBridgeValue()

        Dim strSQL As String = _
                               "select w.weight_value,w.weight_status " & _
                               "from steqi.view_weight_value w " & _
                               "where w.weight_bridge_id=" & mWeight.ID
        Dim vDataSet As New DataSet
        Dim dt As DataTable
        Try
            If mOralcle.OpenDys(strSQL, "TableName", vDataSet) Then
                dt = vDataSet.Tables(0)
                If dt.Rows.Count > 0 Then
                    mWeight.nWeightValue = dt.Rows(0).Item("weight_value").ToString
                    mWeight.WeightStatus = dt.Rows(0).Item("weight_status").ToString
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Sub New(ByRef f As FMain)
        mForm = f
    End Sub

    Protected Overrides Sub Finalize()
        mLog.WriteLog(mWeight.Name, "<---------Stop process--------->")
        mLog = Nothing
        MyBase.Finalize()
    End Sub

#Region "construct and deconstruct"
    Private disposed As Boolean = False

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' This object will be cleaned up by the Dispose method. 
        ' Therefore, you should call GC.SupressFinalize to 
        ' take this object off the finalization queue  
        ' and prevent finalization code for this object 
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        ' Check to see if Dispose has already been called. 
        If Not Me.disposed Then
            ' If disposing equals true, dispose all managed  
            ' and unmanaged resources. 
            UPDATE_WEIGHTBRIDGE_CONNECT(0)
            mLog.WriteLog(mWeight.Name, "<---------Stop process--------->")

            If mForm.spWeight.IsOpen Then
                mForm.spWeight.Close()
            End If
            mShutdown = True
            mRunn = False
            mLog = Nothing
            If mPort IsNot Nothing Then
                mPort.Dispose()
                mPort = Nothing
            End If
            ' Call the appropriate methods to clean up  
            ' unmanaged resources here. 
            ' If disposing is false,  
            ' only the following code is executed.

            ' Note disposing has been done.
            disposed = True

        End If
    End Sub
#End Region

End Class
