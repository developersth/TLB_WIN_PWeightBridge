Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Class CCardReader : Implements IDisposable

    Event OnMessageCardReader(ByVal pMsg As String)
    Event OnWriteComport(ByVal pMsgWrite As String)
    Event OnShowDataLoad(ByVal pLoadHeaderNo As Long)
    Event OnShowDataUnload(ByVal pUnloadNo As Long)

#Region "Enum CR"
    Public Enum CRSTEPPROCESSES
        cspNone = 0
        cspCheckSwip
        cspInvalid
    End Enum

    Public Enum CRFIXTYPES
        cftFixBay = 0
        cftFixIsLand
    End Enum

    Public Enum CRPOSITION
        cpLEFT = 0
        cpLRIGHT
    End Enum

#End Region
    Public Enum CardType
        CardNone = 0
        CardLoading = 1
        CardReady = 2
    End Enum

    Public Enum LoadingStep
        None = 0
        LoadingNotFound = -1
        CardWeightIn1 = 21
        CardWeightIn2 = 31
        CardWeightInSuccess = 51
        CardWeightOut1 = 71
        CardWeightOut2 = 72
        CardWeightOutSuccess = 81
    End Enum

    Public Enum UnLoadingStep
        None = 0
        CardWeightIn1 = 21
        CardWeightIn2 = 22
        CardWeightInSuccess = 51
        CardWeightOut1 = 71
        CardWeightOut2 = 72
        CardWeightOutSuccess = 81
    End Enum

    Public Enum WeightStep
        WeightNone = 0
        WeightIn = 1
        WeightOut = 2
    End Enum

    Structure _CRWeight
        Dim ID As Integer
        Dim ComportID As Integer
        Dim ComportNo As String
        Dim ComportNo1 As String
        Dim ComportSetting As String
        Dim ComportOpen As Boolean
        Dim CONTROL_COMPORT As String
        Dim CONTROL_COMPORT1 As String
        Dim WeightComport As String
        Dim Name As String
        Dim IsEnable As Boolean
        Dim Address As Integer
        Dim LoadingStep As LoadingStep
        Dim UnLoadingStep As UnLoadingStep
        Dim nTU_CARD_CODE As Long
        Dim oTU_CARD_CODE As Long
        Dim IsConnect As Boolean
        Dim IsTimeout As Boolean
        Dim TimeOutCard As Integer
        Dim TimeOutKey As Integer
        Dim TimeOutDisplayWeightOut As Integer
        Dim DateTimeStart As Date
        Dim WeightCountStable As Integer
        Dim DisCount As Integer

        Dim MsgSend As String
        Dim MsgRecv As String
        Dim TimeSend As DateTime

        Dim RET_CARD_TYPE As CardType    '0=none,1=load, 2=unlad, 3=dumpfilling
        Dim RET_CHECK As Integer
        Dim RET_MSG As String
        Dim RET_WEIGHT_STEP As WeightStep  '0=ShowMsgWelCome 
        Dim RET_CR_MSG As String
        Dim RET_WEIGHT_IN As Long
        Dim RET_WEIGHT_OUT As Long
        Dim RET_LOAD_HEADER_NO As Double
        Dim RET_UNLOAD_NO As String
        Dim RET_LOAD_TYPE As String
        Dim RET_CR_DET As String

        Dim mWeightValue As String
        Dim mWeightStatus As String
        Dim mInfrared1 As String
        Dim mInfrared2 As String
        Dim mInfrared3 As String
        Dim mInfraredStatus As String
    End Structure

    Private mLog As New CLog
    Private MercuryProtocal As New MercuryCardReader.MercuryProtocol
    Private MercuryLid As New CMercuryLib.MercuryLib
    Private mWeightBridge As CWeightBridge
    Private mList As New List(Of String)
    Public mCRWeight As _CRWeight

    Dim mSTX_Position As Integer, mETX_Position As Integer
    Dim mResponseTime As Date
    Dim mResponse As Boolean
    Dim mThreadLock As Object
    Dim mMessageCardReader As String

    Dim mPort As CPort
    Dim mForm As FMain

#Region "Thread "
    Dim mThread As Thread
    Dim mConnect As Boolean
    Dim mShutdown As Boolean
    Dim mRunn As Boolean

    Public Sub StartThread()
        mRunn = True

        mThread = New Thread(AddressOf RunProcess)
        mThread.Name = mCRWeight.Name
        mThread.Start()

    End Sub

    Private Sub RunProcess()
        InitialCardReaderComport()
        While (mRunn = True)
            If CheckEnableCR() Then
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                Thread.Sleep(300)
                'ReadFromCardReader(mCRWeight.MsgRecv)
                ReadFromCardReader()
                If mCRWeight.IsConnect Then
                    SendToCardReader(MercuryProtocal.MakeCursorInvisible)
                    SendToCardReader(MercuryProtocal.SetKeyToNum)
                    SendToCardReader(MercuryProtocal.DeleteAllStoreMessage)
                    SendToCardReader(MercuryProtocal.ClearDisplay)
                    SendToCardReader(MercuryProtocal.SetLargeCharacterSize)
                    SendToCardReader(MercuryProtocal.SetFontThai)
                    ClearLastLine()
                    ProcessCRWeight()
                End If

            Else
                Thread.Sleep(3000)
                InitialCardReader()
                If mCRWeight.IsEnable = False Then
                    WriteLogCardReader("Card Reader enable=" & mCRWeight.IsEnable.ToString)
                End If
            End If
            Thread.Sleep(1000)
        End While
    End Sub
    Private Function CheckEnableCR() As Boolean
        Dim ret As Boolean

        ret = True

        Return ret
    End Function
#End Region

#Region "Card Reader"

    Public WriteOnly Property ReceiveData
        Set(ByVal value)
            mCRWeight.MsgRecv = value
        End Set

    End Property

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

    Public Sub WriteLogCardReader(ByVal pMsg As String)
        RaiseEvent OnMessageCardReader(MessageCardReder)
        mMessageCardReader = pMsg
        mLog.WriteLog(mCRWeight.Name, pMsg)
        AddList(pMsg)
    End Sub

    Private Function MessageCardReder() As String
        Return Now + ">" + mMessageCardReader
    End Function

    Public Sub InitialCardReader()
        Dim vIni As New CINI

        'mCRWeight.ID = vIni.INIRead(mPathIni, "CARDREADER", "ID")
        'mCRWeight.TimeOutCard = vIni.INIRead(mPathIni, "TIMEOUT", "CARD")
        'mCRWeight.TimeOutKey = vIni.INIRead(mPathIni, "TIMEOUT", "KEY")

        Dim strSQL As String = "SELECT T.*,C.COMPORT_SETTING,C.COMP_ID  " & _
                     "from view_cr_initail_wb t,view_cr_comporrt_wb C " & _
                     " where t.CARD_READER_ID = C.CARD_READER_ID " & _
                     " AND ROWNUM =1" & _
                     " order by C.COMP_ID"

        Dim vDataSet As New DataSet
        Dim dt As DataTable

        Try
            'WriteLogCardReader("Initial Card Reader[" & mCRWeight.ID & "]")
            If mOralcle.OpenDys(strSQL, "TableName", vDataSet) Then
                dt = vDataSet.Tables(0)
                If dt.Rows.Count > 0 Then
                    mCRWeight.ID = dt.Rows(0).Item("CARD_READER_ID").ToString
                    mCRWeight.Address = dt.Rows(0).Item("CARD_READER_ADDRESS").ToString
                    mCRWeight.ComportNo = dt.Rows(0).Item("COMPORT_NO").ToString
                    mCRWeight.ComportNo1 = dt.Rows(0).Item("COMPORT_NO1").ToString
                    mCRWeight.Name = dt.Rows(0).Item("CARD_READER_NAME").ToString
                    'mCRWeight.ComportSetting = dt.Rows(0).Item("comport_setting").ToString
                    mCRWeight.ComportSetting = dt.Rows(0).Item("COMPORT_SETTING").ToString
                    'mCRWeight.IsEnable = Convert.ToBoolean(dt.Rows(0).Item("is_enabled"))
                    WriteLogCardReader("Initial Card Reader[" & mCRWeight.ID & "]" & _
                                       "Address=" & mCRWeight.Address & ": " & mCRWeight.ComportNo & " " & mCRWeight.ComportSetting & "" & _
                                       " successful.")
                End If
            Else

                WriteLogCardReader("No data Card Reader[" & mCRWeight.ID & "]")
                InitialCardReader()
            End If
        Catch ex As Exception

        End Try

        vIni = Nothing
        If vDataSet IsNot Nothing Then vDataSet.Dispose()
    End Sub

    Public Sub DisplayToCardReaderLoading(ByVal pDisplay As Integer)
        Dim vMsg As String = ""

        Select Case pDisplay
            Case LoadingStep.LoadingNotFound
                vMsg &= MercuryProtocal.ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "99" & vbNewLine & "           No data found          "
                vMsg &= MercuryProtocal.MoveCursor(4, 1) & " Card: " & mCRWeight.oTU_CARD_CODE.ToString
                vMsg &= MercuryProtocal.MoveCursor(5, 1) & " " & mCRWeight.RET_CR_MSG
                SendToCardReader(vMsg.ToUpper())
                Thread.Sleep(5000)
                'SendToCardReader(MercuryProtocal.ClearDisplay())
                ClearDisplay()
                Thread.Sleep(500)
                vMsg = ""
            Case LoadingStep.CardWeightIn1
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "[" & mCRWeight.RET_CARD_TYPE & "-" & LoadingStep.CardWeightIn1 & "] " & mCRWeight.Name & ShowDisplayDateTime()
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & CardTypeMsg() & MercuryProtocal.MoveCursor(2, 14) & ": weight in"
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "card         : " & mCRWeight.oTU_CARD_CODE & "     "
                vMsg &= MercuryProtocal.MoveCursor(4, 1) & "weight in    : " & mWeightBridge.WeightValue & "     "
                vMsg &= MercuryProtocal.MoveCursor(5, 1) & "             please wait                  "
            Case LoadingStep.CardWeightIn2
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "[" & mCRWeight.RET_CARD_TYPE & "-" & LoadingStep.CardWeightIn2 & "] " & mCRWeight.Name & ShowDisplayDateTime()
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & CardTypeMsg() & MercuryProtocal.MoveCursor(2, 14) & ": weight in"
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "card         : " & mCRWeight.oTU_CARD_CODE & "     "
                vMsg &= MercuryProtocal.MoveCursor(4, 1) & "weight in    : " & mWeightBridge.WeightValue & "     "
                vMsg &= MercuryProtocal.MoveCursor(7, 1) & "         Please Touch Card                  "
            Case LoadingStep.CardWeightInSuccess
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เครื่องอ่านบัตรเครื่องชั่ง"
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & mCRWeight.RET_CR_MSG & "     "
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "แตะบัตรชั่งเข้าเรียบร้อยแล้ว "
                SendToCardReader(vMsg.ToUpper())
                'SendToCardReader("")
            Case LoadingStep.CardWeightOut1
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "[" & mCRWeight.RET_CARD_TYPE & "-" & LoadingStep.CardWeightOut1 & "] " & mCRWeight.Name & ShowDisplayDateTime()
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & CardTypeMsg() & MercuryProtocal.MoveCursor(2, 14) & ": weight out"
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "card         : " & mCRWeight.oTU_CARD_CODE & "     "
                vMsg &= MercuryProtocal.MoveCursor(4, 1) & "weight in    : " & mCRWeight.RET_WEIGHT_IN & "     "
                vMsg &= MercuryProtocal.MoveCursor(5, 1) & "weight out   : " & mWeightBridge.WeightValue & "     "
                vMsg &= MercuryProtocal.MoveCursor(6, 1) & "             please wait                  "
            Case LoadingStep.CardWeightOut2
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "[" & mCRWeight.RET_CARD_TYPE & "-" & LoadingStep.CardWeightOut2 & "] " & mCRWeight.Name & ShowDisplayDateTime()
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & CardTypeMsg() & MercuryProtocal.MoveCursor(2, 14) & ": weight out"
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "card         : " & mCRWeight.oTU_CARD_CODE & "     "
                vMsg &= MercuryProtocal.MoveCursor(4, 1) & "weight in    : " & mCRWeight.RET_WEIGHT_IN & "     "
                vMsg &= MercuryProtocal.MoveCursor(5, 1) & "weight out   : " & mWeightBridge.WeightValue & "     "
                vMsg &= MercuryProtocal.MoveCursor(7, 1) & "         Please Touch Card                  "
            Case LoadingStep.CardWeightOutSuccess
                'ClearDisplay()
                vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เครื่องอ่านบัตรเครื่องชั่ง"
                vMsg &= MercuryProtocal.MoveCursor(2, 1) & mCRWeight.RET_CR_MSG & "     "
                vMsg &= MercuryProtocal.MoveCursor(3, 1) & "แตะบัตรชั่งออกเรียบร้อยแล้ว "
                SendToCardReader(vMsg.ToUpper())
                'SendToCardReader("")
        End Select

        If vMsg.Length > 0 Then
            SendToCardReader(vMsg.ToUpper())
        End If
    End Sub
    Private Function DisplayInfaredStatus() As String
        Dim f(2) As String
        Dim s As String

        f(0) = IIf(mCRWeight.mInfrared1 = 1, "<1>", "*")
        f(1) = IIf(mCRWeight.mInfrared2 = 1, "<2>", "*")
        f(2) = IIf(mCRWeight.mInfrared3 = 1, "<3>", "*")
        s = IIf(mCRWeight.mInfraredStatus = 1, "อยู่ในตำแหน่ง", "")
        Return "ตำแหน่ง:[" & f(0) & f(1) & f(2) & "] " & s
    End Function


    Private Sub ProcessCRWeight()
        Dim vMsg As String
        While (mRunn)
            If mCRWeight.IsConnect Then

                mCRWeight.MsgRecv = ReadMercury()
                mCRWeight.nTU_CARD_CODE = CheckCard(mCRWeight.MsgRecv)
                'Check Sensor
                If GetWeightStatus(mWeightBridge.mWeight.ID) Then
                    mCRWeight.RET_LOAD_TYPE = 2
                    If mCRWeight.nTU_CARD_CODE > 0 Then
                        mCRWeight.oTU_CARD_CODE = mCRWeight.nTU_CARD_CODE
                        'M_CR_WEIGHT_CHECK_CARD()
                        P_WEIGHT_CHECK_TU()
                        If mCRWeight.RET_CHECK <> 0 Then
                            mCRWeight.RET_LOAD_TYPE = 0
                            DisplayNoDataFound()
                            ClearData()
                        Else
                            mCRWeight.RET_LOAD_TYPE = 1
                        End If
                    Else
                        'mCRWeight.RET_LOAD_TYPE = 0
                    End If
                Else
                    mCRWeight.RET_LOAD_TYPE = 0
                End If

                Select Case mCRWeight.RET_LOAD_TYPE
                    Case CardType.CardNone
                        'ClearDisplay()
                        vMsg = ""
                        vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เครื่องอ่านบัตรเครื่องชั่ง".PadRight(32)
                        vMsg &= MercuryProtocal.MoveCursor(2, 1) & DisplayInfaredStatus.PadRight(32)
                        vMsg &= MercuryProtocal.MoveCursor(3, 1) & "01 พร้อมให้รถขึ้นชั่ง" & DisplayDateTime.PadRight(5)
                        SendToCardReader(vMsg.ToUpper())
                    Case CardType.CardReady
                        'ClearDisplay()
                        vMsg = ""
                        vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เครื่องอ่านบัตรเครื่องชั่ง".PadRight(32)
                        vMsg &= MercuryProtocal.MoveCursor(2, 1) & DisplayInfaredStatus.PadRight(32)
                        vMsg &= MercuryProtocal.MoveCursor(3, 1) & "กรุณาแตะบัตร     " & DisplayDateTime.PadRight(5)
                        SendToCardReader(vMsg.ToUpper())
                    Case CardType.CardLoading
                        ClearDisplay()
                        WriteLogCardReader("เริ่มการชั่ง...")
                        ProcessWeightLoad()
                        ClearData()
                        WriteLogCardReader("จบการชั่ง...")
                        RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
                        mCRWeight.RET_LOAD_TYPE = 0
                    Case Else
                        vMsg = ""
                        vMsg &= MercuryProtocal.ClearDisplay
                        vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เกิดข้อผิดพลาด"
                        vMsg &= MercuryProtocal.MoveCursor(2, 1) & "บัตร: " & mCRWeight.oTU_CARD_CODE.ToString
                        SendToCardReader(vMsg.ToUpper)
                        'Thread.Sleep(5000)
                        SetTrafficLightRedNoDataFound()
                        mCRWeight.RET_LOAD_TYPE = 0
                        ClearDisplay()
                End Select
                Thread.Sleep(300)
            Else
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                Thread.Sleep(300)
                'ReadFromCardReader(mCRWeight.MsgRecv)
                ReadFromCardReader()
                Thread.Sleep(1000)
                If mCRWeight.IsConnect Then
                    SendToCardReader(MercuryProtocal.MakeCursorInvisible)
                    SendToCardReader(MercuryProtocal.SetKeyToNum)
                    SendToCardReader(MercuryProtocal.DeleteAllStoreMessage)
                    SendToCardReader(MercuryProtocal.ClearDisplay)
                    ClearLastLine()
                End If
            End If
        End While
        SetTrafficLightGreenOFF()
        SetTrafficLightRedOFF()
    End Sub

#Region "Weight Load"
    Private Sub ProcessWeightLoad()
        'While mCRWeight.LoadingStep <> LoadingStep.LoadingNone
        Select Case mCRWeight.RET_WEIGHT_STEP
            Case WeightStep.WeightIn
                WriteLogCardReader("Weight in.")
                mCRWeight.LoadingStep = LoadingStep.CardWeightInSuccess
                LoadWeight()
            Case WeightStep.WeightOut
                WriteLogCardReader("Weight out.")
                mCRWeight.LoadingStep = LoadingStep.CardWeightOutSuccess
                LoadWeightOut()
        End Select
        'End While
    End Sub

    Private Sub LoadWeight()
        While mCRWeight.LoadingStep <> LoadingStep.None
            Select Case mCRWeight.LoadingStep
                Case LoadingStep.CardWeightInSuccess
                    RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
                    DisplayToCardReaderLoading(mCRWeight.LoadingStep)
                    WriteLogCardReader(mCRWeight.RET_CR_MSG)
                    Thread.Sleep(8000)
                    ClearDisplay()
                    mCRWeight.LoadingStep = LoadingStep.None
                    'RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
            End Select
        End While
    End Sub

    'Private Sub LoadWeightIn1()
    '    Dim vKey As String = ""

    '    SetTrafficLightRedOn()
    '    While mCRWeight.LoadingStep = LoadingStep.CardWeightIn1
    '        'DisplayToCardReaderLoading(mCRWeight.LoadingStep)
    '        mCRWeight.DateTimeStart = Now
    '        mCRWeight.WeightCountStable = 0
    '        While mCRWeight.IsTimeout = False
    '            DisplayToCardReaderLoading(mCRWeight.LoadingStep)
    '            mWeightBridge.GetWeightBridgeValue()
    '            If mWeightBridge.WeightStatus = 0 Then      'weight stable
    '                mCRWeight.WeightCountStable += 1
    '                If mCRWeight.WeightCountStable > 1 Then
    '                    mCRWeight.LoadingStep = LoadingStep.CardWeightIn2
    '                    Exit While
    '                End If
    '            Else
    '                mCRWeight.WeightCountStable = 0
    '            End If
    '            mCRWeight.MsgRecv = ReadMercury()
    '            vKey = CheckKeyPress(mCRWeight.MsgRecv)
    '            If vKey = "F3" Then 'check F3 to cancel
    '                WriteLogCardReader("Cancel weight in1")
    '                mCRWeight.LoadingStep = LoadingStep.None
    '                SetTrafficLightRedOFF()
    '                Exit While
    '            End If

    '            If ((Now - mCRWeight.DateTimeStart).TotalSeconds < mCRWeight.TimeOutCard) Then
    '                mCRWeight.IsTimeout = False
    '            Else
    '                mCRWeight.IsTimeout = True
    '                mCRWeight.LoadingStep = LoadingStep.None
    '            End If
    '            Thread.Sleep(500)
    '        End While

    '    End While

    '    If mCRWeight.IsTimeout Then
    '        SetTrafficLightRedNoDataFound()
    '        ClearDisplay()
    '        WriteLogCardReader("Time out weight in1")
    '        mCRWeight.LoadingStep = LoadingStep.None
    '    End If
    'End Sub

    'Private Sub LoadWeightIn2()
    '    Dim vKey As String = ""

    '    SetTrafficLightRedOFF()
    '    Thread.Sleep(1000)
    '    SetTrafficLightGreenOn()
    '    While mCRWeight.LoadingStep = LoadingStep.CardWeightIn2
    '        'DisplayToCardReaderLoading(mCRWeight.LoadingStep)
    '        mCRWeight.DateTimeStart = Now
    '        mCRWeight.WeightCountStable = 0
    '        While mCRWeight.IsTimeout = False
    '            DisplayToCardReaderLoading(mCRWeight.LoadingStep)
    '            mWeightBridge.GetWeightBridgeValue()
    '            If mWeightBridge.WeightStatus <> 0 Then
    '                mCRWeight.WeightCountStable += 1
    '                If mCRWeight.WeightCountStable > 1 Then
    '                    mCRWeight.LoadingStep = LoadingStep.CardWeightIn1
    '                    SetTrafficLightGreenOFF()
    '                    Exit While
    '                End If
    '            End If

    '            mCRWeight.MsgRecv = ReadMercury()
    '            mCRWeight.nTU_CARD_CODE = CheckCard(mCRWeight.MsgRecv)
    '            If mCRWeight.nTU_CARD_CODE > 0 Then
    '                mCRWeight.oTU_CARD_CODE = mCRWeight.nTU_CARD_CODE
    '                If M_WEIGHT_CHECK_VEHICLE() Then
    '                    If mCRWeight.RET_CHECK = 0 Then
    '                        mCRWeight.LoadingStep = LoadingStep.CardWeightInSuccess
    '                        mCRWeight.IsTimeout = False
    '                        SetTrafficLightGreenOFF()
    '                        Exit While
    '                    Else
    '                        mCRWeight.DateTimeStart = Now
    '                        DisplayNoDataFound()

    '                    End If
    '                End If
    '            Else
    '                vKey = CheckKeyPress(mCRWeight.MsgRecv)
    '            End If

    '            If vKey = "F3" Then 'check F3 to cancel
    '                WriteLogCardReader("Cancel weight in2")
    '                mCRWeight.LoadingStep = LoadingStep.None
    '                SetTrafficLightGreenOFF()
    '                Exit While
    '            End If

    '            If ((Now - mCRWeight.DateTimeStart).TotalSeconds < mCRWeight.TimeOutCard) Then
    '                mCRWeight.IsTimeout = False
    '            Else
    '                mCRWeight.IsTimeout = True
    '                mCRWeight.LoadingStep = LoadingStep.None
    '            End If
    '            Thread.Sleep(500)
    '        End While

    '    End While

    '    If mCRWeight.IsTimeout Then
    '        SetTrafficLightRedNoDataFound()
    '        SetTrafficLightGreenOFF()
    '        ClearDisplay()
    '        WriteLogCardReader("Time out weight in2")
    '        mCRWeight.LoadingStep = LoadingStep.None
    '    End If
    'End Sub

    Private Sub LoadWeightOut()
        While mCRWeight.LoadingStep <> LoadingStep.None
            Select Case mCRWeight.LoadingStep
                'Case LoadingStep.CardWeightOut1
                '    'ClearDisplay()
                '    RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
                '    LoadWeightOut1()
                '    ClearDisplay()
                'Case LoadingStep.CardWeightOut2
                '    'ClearDisplay()
                '    RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
                '    LoadWeightOut2()
                '    ClearDisplay()
                Case LoadingStep.CardWeightOutSuccess
                    RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
                    DisplayToCardReaderLoading(mCRWeight.LoadingStep)
                    WriteLogCardReader(mCRWeight.RET_CR_MSG)
                    Thread.Sleep(20000)
                    ClearDisplay()
                    mCRWeight.LoadingStep = LoadingStep.None
                    'RaiseEvent OnShowDataLoad(mCRWeight.RET_LOAD_HEADER_NO)
            End Select
        End While
    End Sub

    Private Sub LoadWeightOut1()
        Dim vKey As String = ""

        SetTrafficLightRedOn()
        While mCRWeight.LoadingStep = LoadingStep.CardWeightOut1
            'DisplayToCardReaderLoading(mCRWeight.LoadingStep)
            mCRWeight.DateTimeStart = Now
            mCRWeight.WeightCountStable = 0
            While mCRWeight.IsTimeout = False
                DisplayToCardReaderLoading(mCRWeight.LoadingStep)
                mWeightBridge.GetWeightBridgeValue()
                If mWeightBridge.WeightStatus = 0 Then      'weight stable
                    mCRWeight.WeightCountStable += 1
                    If mCRWeight.WeightCountStable > 1 Then
                        mCRWeight.LoadingStep = LoadingStep.CardWeightOut2
                        Exit While
                    End If
                Else
                    mCRWeight.WeightCountStable = 0
                End If
                mCRWeight.MsgRecv = ReadMercury()
                vKey = CheckKeyPress(mCRWeight.MsgRecv)
                If vKey = "F3" Then 'check F3 to cancel
                    WriteLogCardReader("Cancel weight out1")
                    mCRWeight.LoadingStep = LoadingStep.None
                    SetTrafficLightRedOFF()
                    Exit While
                End If

                If ((Now - mCRWeight.DateTimeStart).TotalSeconds < mCRWeight.TimeOutCard) Then
                    mCRWeight.IsTimeout = False
                Else
                    mCRWeight.IsTimeout = True
                    mCRWeight.LoadingStep = LoadingStep.None
                End If
                Thread.Sleep(1000)
            End While

        End While

        If mCRWeight.IsTimeout Then
            SetTrafficLightRedNoDataFound()
            ClearDisplay()
            WriteLogCardReader("Time out weight out1")
            mCRWeight.LoadingStep = LoadingStep.None
        End If
    End Sub

    Private Sub LoadWeightOut2()
        Dim vKey As String = ""

        SetTrafficLightRedOFF()
        Thread.Sleep(1000)
        SetTrafficLightGreenOn()
        While mCRWeight.LoadingStep = LoadingStep.CardWeightOut2
            'DisplayToCardReaderLoading(mCRWeight.LoadingStep)
            mCRWeight.DateTimeStart = Now
            mCRWeight.WeightCountStable = 0
            While mCRWeight.IsTimeout = False
                DisplayToCardReaderLoading(mCRWeight.LoadingStep)
                mWeightBridge.GetWeightBridgeValue()
                If mWeightBridge.WeightStatus <> 0 Then
                    mCRWeight.WeightCountStable += 1
                    If mCRWeight.WeightCountStable > 1 Then
                        mCRWeight.LoadingStep = LoadingStep.CardWeightOut1
                        SetTrafficLightGreenOFF()
                        Exit While
                    End If
                End If

                mCRWeight.MsgRecv = ReadMercury()
                mCRWeight.nTU_CARD_CODE = CheckCard(mCRWeight.MsgRecv)
                If mCRWeight.nTU_CARD_CODE > 0 Then
                    mCRWeight.oTU_CARD_CODE = mCRWeight.nTU_CARD_CODE
                    If M_WEIGHT_CHECK_VEHICLE() Then
                        If mCRWeight.RET_CHECK = 0 Then
                            mCRWeight.LoadingStep = LoadingStep.CardWeightOutSuccess
                            mCRWeight.IsTimeout = False
                            SetTrafficLightGreenOFF()
                            Exit While
                        Else
                            mCRWeight.DateTimeStart = Now
                            DisplayNoDataFound()
                        End If
                    End If
                Else
                    vKey = CheckKeyPress(mCRWeight.MsgRecv)
                End If

                If vKey = "F3" Then 'check F3 to cancel
                    WriteLogCardReader("Cancel weight out2")
                    mCRWeight.LoadingStep = LoadingStep.None
                    SetTrafficLightGreenOFF()
                    Exit While
                End If

                If ((Now - mCRWeight.DateTimeStart).TotalSeconds < mCRWeight.TimeOutCard) Then
                    mCRWeight.IsTimeout = False
                Else
                    mCRWeight.IsTimeout = True
                    mCRWeight.LoadingStep = LoadingStep.None
                End If
                Thread.Sleep(500)
            End While

        End While

        If mCRWeight.IsTimeout Then
            SetTrafficLightRedNoDataFound()
            SetTrafficLightGreenOFF()
            ClearDisplay()
            WriteLogCardReader("Time out weight out2")
            mCRWeight.LoadingStep = LoadingStep.None
        End If
    End Sub
#End Region

#Region "Weight Unload"

#End Region

#Region "Traffic Light"
    Private Sub SetTrafficLightRedOn()
        SendToCardReader(MercuryLid.WriteDigitalOutput1ON)
    End Sub

    Private Sub SetTrafficLightRedOFF()
        SendToCardReader(MercuryLid.WriteDigitalOutput1OFF)
    End Sub

    Private Sub SetTrafficLightGreenOn()
        SendToCardReader(MercuryLid.WriteDigitalOutput2ON)
    End Sub

    Private Sub SetTrafficLightGreenOFF()
        SendToCardReader(MercuryLid.WriteDigitalOutput2OFF)
    End Sub

    Private Sub SetTrafficLightRedNoDataFound()
        SetTrafficLightRedOn()
        Thread.Sleep(1000)
        SetTrafficLightRedOFF()
        Thread.Sleep(1000)
        SetTrafficLightRedOn()
        Thread.Sleep(1000)
        SetTrafficLightRedOFF()
        Thread.Sleep(1000)
        SetTrafficLightRedOn()
        Thread.Sleep(1000)
        SetTrafficLightRedOFF()
    End Sub

    Private Sub SetTrafficLightGreenPass()
        SetTrafficLightGreenOn()
        Thread.Sleep(3000)
        SetTrafficLightGreenOFF()
    End Sub
#End Region

    Private Function ReadMercury() As String
        Dim s As String = ""
        SendToCardReader(MercuryProtocal.SendNextQueueBlock)
        Thread.Sleep(350)
        ReadFromCardReader(s)

        Return s
    End Function

    Private Function CheckCard(ByVal pRecv As String) As String
        Dim s As String = ""
        s = Convert.ToInt32(CheckTouchCard(pRecv))
        If s = 0 Then s = CheckEnterCard(pRecv)
        If IsNumeric(s) Then
            Return s
        Else
            s = "0"
        End If
        Return s
    End Function

    Private Function CheckTouchCard(ByVal pRecv As String) As String
        Dim CardCode As String = "0"
        Try
            If pRecv <> "" Then
                Dim vIndex As Integer = pRecv.IndexOf(Char.ConvertFromUtf32(2) + mCRWeight.Address.ToString("00D"))
                If vIndex > -1 And pRecv.Length > 5 Then
                    If mSTX_Position <= mETX_Position Then
                        Dim vTemp As String = pRecv.Substring(mSTX_Position, mETX_Position + 1)
                        vIndex = pRecv.IndexOf("DC")
                        If vIndex >= 0 Then
                            vTemp = vTemp.Substring(vIndex + 2, mETX_Position - vIndex - 2 - 1).Trim
                            'CardCode = Integer.Parse(vTemp, System.Globalization.NumberStyles.HexNumber).ToString
                            CardCode = Integer.Parse(vTemp.Substring(vTemp.Length - 5, 4), System.Globalization.NumberStyles.HexNumber).ToString()
                            WriteLogCardReader("Touch Card code = " + CardCode)
                        End If
                    Else
                        mETX_Position = pRecv.IndexOf(Char.ConvertFromUtf32(3), mSTX_Position)
                        Dim vTemp As String = pRecv.Substring(mSTX_Position, mETX_Position - 2)
                        vIndex = pRecv.IndexOf("DC")
                        If vIndex >= 0 Then
                            vTemp = vTemp.Substring(vIndex + 1).Trim
                            CardCode = Integer.Parse(vTemp.Substring(vTemp.Length - 5, 4), System.Globalization.NumberStyles.HexNumber).ToString()
                            ' CardCode = Integer.Parse(vTemp, System.Globalization.NumberStyles.HexNumber).ToString
                            WriteLogCardReader("Touch Card code = " + CardCode)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim log As New CLog
            log.WriteErrMessage(ex.Source & "->" & ex.Message)
        End Try
        Return CardCode
    End Function

    Private Function CheckEnterCard(ByVal pRecv As String) As String
        Dim vMsg As String = ""

        mCRWeight.IsTimeout = False
        Dim vKey As String = ""
        vKey = CheckKeyPress(pRecv)
        If vKey = "F6" Then
            SendToCardReader(MercuryProtocal.DeleteAllStoreMessage)
            Thread.Sleep(300)
            WriteLogCardReader("Enter Key = " + vKey)
            WriteLogCardReader("Enter card code")
            vMsg += DisplayDateTime()
            'vMsg += MercuryProtocal.MoveCursor(3, 1) + "      Welcome to Sakchaisit System          "
            vMsg += MercuryProtocal.MoveCursor(7, 1) + "         Please Enter Card                  "
            SendToCardReader(vMsg.ToUpper)
            mCRWeight.DateTimeStart = Now
            While (mCRWeight.IsTimeout = False)
                SendToCardReader(MercuryProtocal.MoveCursor(1, 20) + Now.ToString())
                SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                Thread.Sleep(300)
                'ReadFromCardReader(mCRWeight.MsgRecv)
                ReadFromCardReader()
                vKey = CheckKeyPress(mCRWeight.MsgRecv)
                If vKey = "F7" Then
                    WriteLogCardReader("Enter key = " + vKey)
                    WriteLogCardReader("Cancel enter card code")
                    SendToCardReader(MercuryProtocal.ClearDisplay())
                    SendToCardReader(MercuryProtocal.DeleteAllStoreMessage)
                    Thread.Sleep(300)
                    SendToCardReader(MercuryProtocal.SendNextQueueBlock)
                    ClearLastLine()
                    Exit While
                Else
                    If vKey <> "" Then
                        WriteLogCardReader("Enter key = " + vKey)
                        SendToCardReader(MercuryProtocal.ClearDisplay())
                        ClearLastLine()
                        Return vKey
                    End If
                End If

                If ((Now - mCRWeight.DateTimeStart).TotalSeconds < mCRWeight.TimeOutKey) Then
                    mCRWeight.IsTimeout = False
                Else
                    mCRWeight.IsTimeout = True
                End If
            End While
            If mCRWeight.IsTimeout Then
                SendToCardReader(MercuryProtocal.ClearDisplay())
                ClearLastLine()
                WriteLogCardReader("Time out enter card code")
                mCRWeight.IsTimeout = False
            End If
        Else
            If vKey <> "" Then
                SendToCardReader(MercuryProtocal.ClearDisplay())
                ClearLastLine()
                WriteLogCardReader("Enter Key = " + vKey)
                Return vKey
            End If
        End If

        Return "0"
    End Function

    Private Function CheckKeyPress(ByVal pRecv As String) As String
        Dim vKey As String = ""
        If pRecv <> "" Then
            Dim vIndex As Integer = pRecv.IndexOf(Char.ConvertFromUtf32(2) + mCRWeight.Address.ToString("00"))
            If vIndex > -1 And pRecv.Length > 5 Then
                vKey = MercuryProtocal.CheckKeyPress(pRecv)
            End If
        End If
        Return vKey
    End Function

    Private Sub ClearLastLine()
        SendToCardReader(MercuryProtocal.MoveCursor(8, 1) & "                                               ")
    End Sub

    Private Function DisplayDateTime() As String
        Return "  " & Now.ToLongTimeString
    End Function


    Private Sub ClearDisplay()
        SendToCardReader(MercuryProtocal.ClearDisplay)
    End Sub
    Private Function ShowDisplayDateTime() As String

        mCRWeight.TimeSend = DateTime.Now
        If ((mCRWeight.TimeSend.Second) Mod 3 = 0) Then
            Return MercuryProtocal.MoveCursor(1, 20) + Now.ToString()
            Exit Function
        End If
        Return ""
    End Function
    Private Sub ClearData()
        With mCRWeight
            .RET_CHECK = -1
            .RET_CARD_TYPE = 0
            .RET_CR_MSG = ""
            .RET_MSG = 0
            .RET_WEIGHT_IN = 0
            .RET_WEIGHT_OUT = 0
            .LoadingStep = LoadingStep.None
            .UnLoadingStep = UnLoadingStep.None
            .RET_LOAD_HEADER_NO = 0
            .RET_UNLOAD_NO = 0
        End With
    End Sub

    Private Sub DisplayNoDataFound()
        Dim vMsg As String
        vMsg &= MercuryProtocal.ClearDisplay()
        vMsg &= MercuryProtocal.MoveCursor(1, 1) & "เกิดข้อผิดพลาด"
        vMsg &= MercuryProtocal.MoveCursor(2, 1) & " Card: " & mCRWeight.oTU_CARD_CODE.ToString
        vMsg &= MercuryProtocal.MoveCursor(3, 1) & mCRWeight.RET_CR_MSG
        SendToCardReader(vMsg.ToUpper())
        SetTrafficLightRedNoDataFound()
        'SendToCardReader(MercuryProtocal.ClearDisplay())
        ClearDisplay()
        Thread.Sleep(500)
    End Sub

    Private Function CardTypeMsg() As String
        Select Case mCRWeight.RET_CARD_TYPE
            Case CardType.CardNone
                Return ""
            Case CardType.CardLoading
                Return "Load"
        End Select
    End Function
#End Region

#Region "Serial Port"

    Public Sub InitialCardReaderComport()
        WriteLogCardReader("Open comport[" + mCRWeight.ComportNo + "]")
        OpenPort()
    End Sub

    Private Sub OpenPort()
        Try
            With mForm.spCard
                Dim sPli() As String = mCRWeight.ComportSetting.Split(",")
                .PortName = mCRWeight.ComportNo
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

            mForm.spCard.ReadTimeout = 500
            mForm.spCard.WriteTimeout = 1000
            mForm.spCard.Open()
            mConnect = True

            'RaiseEvent OnMessageComport("Open comport[" + mArgumentPort.portNo + " : " + mArgumentPort.portSetting + "] successfull.")
            WriteLogCardReader("Open comport[" + mCRWeight.ComportNo + " : " + mCRWeight.ComportSetting + "] successfull.")
        Catch ex As Exception

        End Try
    End Sub

    Public Function SendToCardReader(ByVal pMsg As String) As Boolean
        '01R[?9;1z Y
        '01R[?9;1z W
        '01R[?9;1z W
        Try
            If mForm.spCard.IsOpen = False Then
                Return False
            End If
            Dim objEncoding As Encoding = Encoding.GetEncoding("Windows-874")
            Dim b() As Byte = BuildMessage(mCRWeight.Address, pMsg)
            mForm.spCard.Write(b, 0, b.Length)
            'mPort.SendData(b)
            mCRWeight.MsgSend = objEncoding.GetString(b)
            DisplaySuccess = True
            'RaiseEvent OnWriteComport(mCRWeight.SendCmd)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ReadFromCardReader(ByRef pRecv As String) As Boolean
        Dim vCheck As Boolean = False
        Dim vRecv As String = ""
        Try
            pRecv = mForm.spCard.ReadExisting
            mForm.spCard.DiscardInBuffer()
            mCRWeight.MsgRecv = Trim(pRecv)
            If CheckBlockRecv(mCRWeight.MsgRecv) Then
                mCRWeight.DisCount = 0
                CheckResponse(True)
                vCheck = True
            Else
                mCRWeight.DisCount += 1
                If mCRWeight.DisCount > 5 Then
                    CheckResponse(False)
                End If
            End If
        Catch ex As Exception

        End Try
        Return vCheck
    End Function

    Private Function ReadFromCardReader() As Boolean
        Dim vCheck As Boolean = False
        Dim vRecv As String = ""
        Try
            vRecv = mForm.spCard.ReadExisting
            mForm.spCard.DiscardInBuffer()
            mCRWeight.MsgRecv = vRecv
            If CheckBlockRecv(vRecv) Then
                CheckResponse(True)
                vCheck = True
            Else
                CheckResponse(False)
            End If
        Catch ex As Exception

        End Try
        Return vCheck
    End Function
    Private Function BuildMessage(ByVal pCRAddress As Integer, ByVal pMsg As String) As Byte()
        Dim vSTX As Byte = 2
        Dim R As Byte = 82
        Dim vETX As Byte = 3
        Dim objEndcoding As Encoding = Encoding.GetEncoding("Windows-874")

        Dim b() As Byte = objEndcoding.GetBytes(pMsg)
        'STX + Address + Address + R + MSG.Length + DMY + CSUM + ETX
        Dim vMsg(1 + 2 + 1 + (b.Length - 1) + 1 + 1 + 1) As Byte
        Dim vAddr() As Byte
        Try
            For i As Integer = 0 To vMsg.Length - 1
                Select Case i
                    Case 0
                        vMsg(i) = vSTX
                    Case 1
                        vAddr = objEndcoding.GetBytes(Format(pCRAddress, "00"))
                        vMsg(i) = vAddr(0)
                        vMsg(i + 1) = vAddr(1)
                    Case 2
                        vMsg(i) = vMsg(i)
                    Case 3
                        vMsg(i) = R
                    Case Else
                        If i = vMsg.Length - 3 Then
                            vMsg(i) = Convert.ToByte(32)
                        ElseIf i = vMsg.Length - 2 Then
                            vMsg(i) = 0
                        ElseIf i = vMsg.Length - 1 Then
                            vMsg(i) = vETX
                        Else
                            vMsg(i) = b(i - 4)
                        End If
                End Select
            Next

            CalCSUM(vMsg, vMsg(vMsg.Length - 2))
        Catch ex As Exception

        End Try
        Return vMsg
    End Function

    Private Sub CalCSUM(ByVal pMsg() As Byte, ByRef pCSUM As Byte)
        Dim vCSUM As Integer = 0
        For i As Integer = 0 To pMsg.Length - 2
            vCSUM += pMsg(i)
        Next

        vCSUM = vCSUM And &H7F
        vCSUM = Not vCSUM
        pCSUM = vCSUM And 127
        pCSUM += 1
    End Sub

    Public Function CheckBlockRecv(ByVal pRecv As String) As Boolean
        Dim vCheck As Boolean = False
        Dim vCheckPos As Integer

        pRecv.Trim()
        If pRecv.Length >= 5 Then
            mSTX_Position = pRecv.IndexOf(Char.ConvertFromUtf32(2))
            mETX_Position = pRecv.IndexOf(Char.ConvertFromUtf32(3))
            vCheckPos = pRecv.IndexOf(Char.ConvertFromUtf32(2) + mCRWeight.Address.ToString("00"))

            If vCheckPos >= 0 Then
                vCheck = True
            End If
        End If
        Return vCheck
    End Function

    Private Sub CheckResponse(ByVal pResponse As Boolean)
        If pResponse Then
            mResponseTime = DateTime.Now
            If mResponse <> pResponse Then
                mCRWeight.IsConnect = True
                P_UPDATE_CARDREADER_CONNECT(1)
                mCRWeight.RET_CARD_TYPE = CardType.CardNone
                mResponse = pResponse
                WriteLogCardReader(mCRWeight.Name + "connect = " + mCRWeight.IsConnect.ToString)
            End If
        Else
            Dim vDateTime As DateTime = Now
            If mResponse = True Then
                mResponse = False
                mResponseTime = DateTime.Now
            End If
            Dim vDiff As Double = (vDateTime - mResponseTime).TotalSeconds
            If vDiff > 30 And mResponse = False Then
                mResponseTime = Now
                mCRWeight.IsConnect = False
                mResponse = False
                P_UPDATE_CARDREADER_CONNECT(0)
                mCRWeight.RET_CARD_TYPE = CardType.CardNone
                WriteLogCardReader(mCRWeight.Name + "connect = " + mCRWeight.IsConnect.ToString)
            End If
        End If
    End Sub
#End Region

#Region "Database"

    Public Sub P_UPDATE_CARDREADER_CONNECT(ByVal pStatus As Integer)
        Dim strSQL As String = "begin tas.P_UPDATE_CARDREADER_CONNECT(" & _
                            mCRWeight.ID & "," & pStatus & ",'" & Environment.MachineName & "'" & _
                            ");end;"
        mOralcle.ExeSQL(strSQL)
    End Sub

    Public Function M_CR_WEIGHT_CHECK_CARD() As Boolean
        Dim strSQL As String = "begin load.M_CR_WEIGHT_CHECK_CARD(" & _
                            mCRWeight.nTU_CARD_CODE & "," & mCRWeight.ID & _
                            ",:RET_CARD_TYPE,:RET_LOAD_HEADER_NO,:RET_UNLOAD_NO,:RET_WEIGHT_STEP,:RET_WEIGHT_IN" & _
                            ",:RET_CHECK,:RET_MSG" & _
                            ");end;"
        Dim p As New COraParameter
        Dim vCheck As Boolean = False
        Try
            p.CreateOracleParameter(7)

            p.AddOracleParameter(0, "RET_CARD_TYPE", Oracle.DataAccess.Client.OracleDbType.Int16)
            p.AddOracleParameter(1, "RET_LOAD_HEADER_NO", Oracle.DataAccess.Client.OracleDbType.Double)
            p.AddOracleParameter(2, "RET_UNLOAD_NO", Oracle.DataAccess.Client.OracleDbType.Varchar2, 32)
            p.AddOracleParameter(3, "RET_WEIGHT_STEP", Oracle.DataAccess.Client.OracleDbType.Int16)
            p.AddOracleParameter(4, "RET_WEIGHT_IN", Oracle.DataAccess.Client.OracleDbType.Int32)
            p.AddOracleParameter(5, "RET_CHECK", Oracle.DataAccess.Client.OracleDbType.Int16)
            p.AddOracleParameter(6, "RET_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)

            If mOralcle.ExeSQL(strSQL, p) Then
                mCRWeight.RET_CARD_TYPE = IIf(IsDBNull(p.OraParam(0).Value), 0, Convert.ToInt16(p.OraParam(0).Value.ToString))
                mCRWeight.RET_LOAD_HEADER_NO = IIf(IsDBNull(p.OraParam(1).Value), 0, Convert.ToInt64(p.OraParam(1).Value.ToString))
                mCRWeight.RET_UNLOAD_NO = IIf(IsDBNull(p.OraParam(2).Value), "0", p.OraParam(2).Value.ToString)
                mCRWeight.RET_CHECK = IIf(IsDBNull(p.OraParam(5).Value), 0, Convert.ToInt16(p.OraParam(5).Value.ToString))
                mCRWeight.RET_MSG = IIf(IsDBNull(p.OraParam(6).Value), "", p.OraParam(6).Value.ToString)
                mCRWeight.RET_WEIGHT_STEP = IIf(IsDBNull(p.OraParam(3).Value), 0, Convert.ToInt16(p.OraParam(3).Value.ToString))
                mCRWeight.RET_WEIGHT_IN = IIf(IsDBNull(p.OraParam(4).Value), 0, Convert.ToInt32(p.OraParam(4).Value.ToString))
                vCheck = True
            End If
            If Not String.IsNullOrEmpty(mCRWeight.RET_MSG) And String.Equals(mCRWeight.RET_MSG, "null") = False Then
                WriteLogCardReader(mCRWeight.RET_MSG)
            End If
            If Not String.IsNullOrEmpty(mCRWeight.RET_CR_MSG) And String.Equals(mCRWeight.RET_CR_MSG, "null") = False Then
                WriteLogCardReader(mCRWeight.RET_CR_MSG)
            End If
        Catch ex As Exception
            mLog.WriteErrMessage(ex.Message)
        End Try
        p.RemoveOracleParam()
        Return vCheck

    End Function
    Public Function P_WEIGHT_CHECK_TU()
        Dim strSQL As String = "begin load.P_WEIGHT_CHECK_TU('" & _
                    mCRWeight.nTU_CARD_CODE & "'," & mCRWeight.ID & _
                     ",0," & _
                     "0," & _
                    "sysdate," & _
                    "TAS.SYSTEM_USER," & _
                    "TAS.SERVER_NAME" & _
                    ",:RET_LOAD_NO,:RET_WEIGHT_TYPE" & _
                    ",:RET_CHECK,:RET_MSG,:RET_CR_MSG" & _
                    ");end;"
        Dim p As New COraParameter
        Dim vCheck As Boolean = False
        Try
            p.CreateOracleParameter(5)

            p.AddOracleParameter(0, "RET_LOAD_NO", Oracle.DataAccess.Client.OracleDbType.Int32)
            p.AddOracleParameter(1, "RET_WEIGHT_TYPE", Oracle.DataAccess.Client.OracleDbType.Int16)
            p.AddOracleParameter(2, "RET_CHECK", Oracle.DataAccess.Client.OracleDbType.Int16)
            p.AddOracleParameter(3, "RET_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)
            p.AddOracleParameter(4, "RET_CR_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)


            If mOralcle.ExeSQL(strSQL, p) Then
                mCRWeight.RET_LOAD_HEADER_NO = IIf(IsDBNull(p.OraParam(0).Value), 0, Convert.ToInt32(p.OraParam(0).Value.ToString))
                mCRWeight.RET_LOAD_TYPE = IIf(IsDBNull(p.OraParam(1).Value), 0, Convert.ToInt16(p.OraParam(1).Value.ToString))
                mCRWeight.RET_WEIGHT_STEP = IIf(IsDBNull(p.OraParam(1).Value), 0, Convert.ToInt16(p.OraParam(1).Value.ToString))
                mCRWeight.RET_CHECK = IIf(IsDBNull(p.OraParam(2).Value), 0, Convert.ToInt16(p.OraParam(2).Value.ToString))
                mCRWeight.RET_MSG = IIf(IsDBNull(p.OraParam(3).Value), 0, p.OraParam(3).Value.ToString)
                mCRWeight.RET_CR_MSG = IIf(IsDBNull(p.OraParam(4).Value), "", p.OraParam(4).Value.ToString)
                vCheck = True
            End If
            If Not String.IsNullOrEmpty(mCRWeight.RET_MSG) And String.Equals(mCRWeight.RET_MSG, "null") = False Then
                WriteLogCardReader(mCRWeight.RET_MSG)
            End If
            If Not String.IsNullOrEmpty(mCRWeight.RET_CR_MSG) And String.Equals(mCRWeight.RET_CR_MSG, "null") = False Then
                WriteLogCardReader(mCRWeight.RET_CR_MSG)
            End If
        Catch ex As Exception
            mLog.WriteErrMessage(ex.Message)
        End Try
        p.RemoveOracleParam()
        Return vCheck
    End Function
    Public Function M_WEIGHT_CHECK_VEHICLE() As Boolean
        Dim strSQL As String = "begin load.M_WEIGHT_CHECK_VEHICLE(" & _
                            mCRWeight.nTU_CARD_CODE & "," & mCRWeight.ID & ",0" & _
                            ",to_date('" & Now.ToString & "','DD/MM/YYYY HH24:MI:SS')" & _
                            "," & mWeightBridge.WeightValue & ",'','' " & _
                            ",:RET_WEIGHT_IN,:RET_WEIGHT_OUT" & _
                            ",:RET_CHECK,:RET_MSG,:RET_CR_MSG" & _
                            ");end;"
        Dim p As New COraParameter
        Dim vCheck As Boolean = False

        p.CreateOracleParameter(5)
        p.AddOracleParameter(0, "RET_WEIGHT_IN", Oracle.DataAccess.Client.OracleDbType.Int32)
        p.AddOracleParameter(1, "RET_WEIGHT_OUT", Oracle.DataAccess.Client.OracleDbType.Int32)
        p.AddOracleParameter(2, "RET_CHECK", Oracle.DataAccess.Client.OracleDbType.Int16)
        p.AddOracleParameter(3, "RET_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)
        p.AddOracleParameter(4, "RET_CR_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)

        If mOralcle.ExeSQL(strSQL, p) Then
            mCRWeight.RET_CHECK = IIf(IsDBNull(p.OraParam(2).Value), 0, Convert.ToInt32(p.OraParam(2).Value.ToString))
            If mCRWeight.RET_CHECK = 0 Then
                mCRWeight.RET_WEIGHT_IN = p.OraParam(0).Value.ToString
                mCRWeight.RET_WEIGHT_OUT = IIf(IsDBNull(p.OraParam(1).Value.ToString), 0, p.OraParam(1).Value.ToString)
            End If
            mCRWeight.RET_MSG = IIf(IsDBNull(p.OraParam(3).Value.ToString), "", p.OraParam(3).Value.ToString)
            mCRWeight.RET_CR_MSG = IIf(IsDBNull(p.OraParam(4).Value.ToString), "", p.OraParam(4).Value.ToString)
            vCheck = True
        End If
        If Not String.IsNullOrEmpty(mCRWeight.RET_MSG) And String.Equals(mCRWeight.RET_MSG, "null") = False Then
            WriteLogCardReader(mCRWeight.RET_MSG)
        End If
        If Not String.IsNullOrEmpty(mCRWeight.RET_CR_MSG) And String.Equals(mCRWeight.RET_CR_MSG, "null") = False Then
            WriteLogCardReader(mCRWeight.RET_CR_MSG)
        End If
        p.RemoveOracleParam()
        Return vCheck

    End Function
    Public Function GetWeightStatus(ByVal iWB_ID As String) As Boolean
        Dim vDataSet As New DataSet
        Dim dt As DataTable
        Dim strSql As String
        Dim vCheck As Boolean = False
        Try
            strSql = "select WEIGHT_VALUE,WEIGHT_STATUS " & _
                     ",INFRARED_1,INFRARED_2,INFRARED_3,INFRARED_STATUS " & _
                     " from steqi.view_wb_monitor " & _
                     " where WEIGHT_BRIDGE_ID=" & iWB_ID

            If mOralcle.OpenDys(strSql, "TableName", vDataSet) Then
                dt = vDataSet.Tables("TableName")
                With mCRWeight
                    .mWeightValue = dt.Rows(0).Item("WEIGHT_VALUE").ToString
                    .mWeightStatus = dt.Rows(0).Item("WEIGHT_STATUS").ToString
                    .mInfrared1 = dt.Rows(0).Item("INFRARED_1").ToString
                    .mInfrared2 = dt.Rows(0).Item("INFRARED_2").ToString
                    .mInfrared3 = dt.Rows(0).Item("INFRARED_3").ToString
                    .mInfraredStatus = dt.Rows(0).Item("INFRARED_STATUS").ToString
                    If .mWeightStatus = 0 And .mInfraredStatus = 1 Then
                        vCheck = True
                    Else
                        vCheck = False
                    End If
                End With
            End If
            vDataSet = Nothing
        Catch exc As Exception
            'MsgBox(exc.Message, MsgBoxStyle.Critical)
        End Try
        Return vCheck
    End Function
    Public Function M_WEIGHT_CHECK_VEHICLE_UNLOAD() As Boolean
        Dim strSQL As String = "begin load.M_WEIGHT_CHECK_VEHICLE_UNLOAD(" & _
                            mCRWeight.nTU_CARD_CODE & "," & mCRWeight.ID & ",0" & _
                            ",to_date('" & Now.ToString & "','DD/MM/YYYY HH24:MI:SS')" & _
                            "," & mWeightBridge.WeightValue & ",'','' " & _
                            ",:RET_WEIGHT_IN,:RET_WEIGHT_OUT" & _
                            ",:RET_CHECK,:RET_MSG,:RET_CR_MSG" & _
                            ");end;"
        Dim p As New COraParameter
        Dim vCheck As Boolean = False

        p.CreateOracleParameter(5)
        p.AddOracleParameter(0, "RET_WEIGHT_IN", Oracle.DataAccess.Client.OracleDbType.Int32)
        p.AddOracleParameter(1, "RET_WEIGHT_OUT", Oracle.DataAccess.Client.OracleDbType.Int32)
        p.AddOracleParameter(2, "RET_CHECK", Oracle.DataAccess.Client.OracleDbType.Int16)
        p.AddOracleParameter(3, "RET_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)
        p.AddOracleParameter(4, "RET_CR_MSG", Oracle.DataAccess.Client.OracleDbType.Varchar2, 512)

        If mOralcle.ExeSQL(strSQL, p) Then
            mCRWeight.RET_CHECK = IIf(IsDBNull(p.OraParam(2).Value), 0, Convert.ToInt32(p.OraParam(2).Value.ToString))
            If mCRWeight.RET_CHECK = 0 Then
                mCRWeight.RET_WEIGHT_IN = p.OraParam(0).Value.ToString
                mCRWeight.RET_WEIGHT_OUT = IIf(IsDBNull(p.OraParam(1).Value.ToString), 0, p.OraParam(1).Value.ToString)

            End If
            mCRWeight.RET_MSG = IIf(IsDBNull(p.OraParam(3).Value.ToString), "", p.OraParam(3).Value.ToString)
            mCRWeight.RET_CR_MSG = IIf(IsDBNull(p.OraParam(4).Value.ToString), "", p.OraParam(4).Value.ToString)
            vCheck = True
        End If
        If Not String.IsNullOrEmpty(mCRWeight.RET_MSG) And String.Equals(mCRWeight.RET_MSG, "null") = False Then
            WriteLogCardReader(mCRWeight.RET_MSG)
        End If
        If Not String.IsNullOrEmpty(mCRWeight.RET_CR_MSG) And String.Equals(mCRWeight.RET_CR_MSG, "null") = False Then
            WriteLogCardReader(mCRWeight.RET_CR_MSG)
        End If
        p.RemoveOracleParam()
        Return vCheck

    End Function
#End Region

    Protected Overrides Sub Finalize()
        mLog.WriteLog(mCRWeight.Name, "<---------Stop process--------->")
        mShutdown = True
        mRunn = False
        mLog = Nothing
        'mWeightBridge = Nothing
        MyBase.Finalize()
    End Sub

    Public Sub New(ByRef f As FMain)
        mWeightBridge = f.mWeightBridge
        mForm = f
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
            mRunn = False
            Thread.Sleep(300)
            P_UPDATE_CARDREADER_CONNECT(0)
            SendToCardReader((MercuryProtocal.ClearDisplay & MercuryProtocal.MoveCursor(1, 1) & "[99]" & _
                             MercuryProtocal.MoveCursor(3, 1) & "              System Offline       ").ToUpper)
            Thread.Sleep(1000)
            If mForm.spCard.IsOpen Then
                mForm.spCard.Close()
            End If

            mLog.WriteLog(mCRWeight.Name, "<---------Stop process--------->")
            mShutdown = True
            'mRunn = False
            If mPort IsNot Nothing Then
                mPort.Dispose()
                mPort = Nothing
            End If
            mLog = Nothing

            ' Call the appropriate methods to clean up  
            ' unmanaged resources here. 
            ' If disposing is false,  
            ' only the following code is executed.

            ' Note disposing has been done.
            disposed = True

        End If
    End Sub
#End Region

    'Private Sub mPort_OnConnect() Handles mPort.OnConnect
    '    WriteLogCardReader("Open comport[" + mCRWeight.ComportNo + " : " + mCRWeight.ComportSetting + "] successfull.")
    'End Sub

    'Private Sub mPort_OnDataReceive(ByVal pMsgRecv As String) Handles mPort.OnDataReceive
    '    ReadFromCardReader(pMsgRecv)
    'End Sub
End Class
