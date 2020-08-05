Imports System
Imports System.Data
Imports Oracle.DataAccess.Client
Imports System.Threading
Imports System.IO
Imports System.Configuration
Imports System.Data.OleDb

Public Class COracle

    Structure _ParamMember
        Public Name As String
        Public Value As Object
        Public Size As Int32
        Public Direction As _OracleDbDirection
        Public Type As _OracleDbType
    End Structure

    Private currentDB As DB_TYPE
    Private mIni As New CINI

    Private countDisconnect As Integer
    Private oraConnect As Boolean
    Private thrShutdown As Boolean
    Private connStrMaster As String = "User Id=tas;Password=tam;Data Source=LLTLBA"
    Private connStrBackup As String = "User Id=tas;Password=tam;Data Source=LLTLBB"
    'Private mPathIni As String = "D:\LLTLB\LLTLBConfig.ini"
    Private oraConn As OracleConnection = Nothing
    Private oraServiceName As String = "N/A"

    Private thrOracle As Thread
    Private thrRunn As Boolean
    Private thrScanDB(2) As ScanDatabase

    Public Event OnConnect()
    Public Event OnDisconnect()
    Private mLog As New CLog

    Dim OraParam() As _ParamMember
    Private mTotalParam As Integer

    Public OraDysReader As OracleDataReader

#Region "Enum Database"
    'Database ENUM
    Public Enum DB_TYPE
        DB_None = -1
        DB_MASTER = 0
        DB_BACKUP = 1
    End Enum

    Public Enum _OracleDbDirection
        OraInput
        OraOutput
    End Enum

    Public Enum _OracleDbType
        OraVarchar2
        OraInt16
        OraInt32
        OraInt64
        OraDate
        OraLong
        OraDouble
        OraSingle
        OraByte
        OraDecimal
        OraBlob
    End Enum
#End Region

    Sub New()
        oraConnect = False
        thrShutdown = False
        thrRunn = False
        countDisconnect = 0
        currentDB = DB_TYPE.DB_None
        'ScanDatabase()
        StartThread()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

    Public Sub Dispose()
        Close()
    End Sub
    Public Function GetCurrDirectory() As String
        Dim sDirectory As String
        sDirectory = Environment.CurrentDirectory()

        Return sDirectory
    End Function
    Private Sub StartThread()
        If thrRunn Then Exit Sub
        thrScanDB(0) = New ScanDatabase("tas", "tam", "LLTLBA")
        thrScanDB(1) = New ScanDatabase("tas", "tam", "LLTLBB")
        thrOracle = New Thread(New ThreadStart(AddressOf RunProcess))
        thrRunn = True
        thrOracle.Name = "thrOracle"
        thrOracle.Start()
    End Sub

    Private Sub RunProcess()
        While (Not thrShutdown)
            'If CurrentDB = DB_TYPE.DB_None Then
            '    CurrentDB = GetSelectServerDb()
            'End If

            'If CurrentDB <> DB_TYPE.DB_None Then
            ScanActiveDatabase()
            'Reconnect()
            'If bConnect Or bShutdown Then
            If thrShutdown Then
                Exit While
            End If
            'End If

            System.Threading.Thread.Sleep(5000)
        End While
        'If bConnect Then
        '    RaiseEvent OnConnect()
        '    thrRunn = False
        'End If
    End Sub

    Public ReadOnly Property ConnectStatus() As Boolean
        Get
            Return oraConnect
        End Get
    End Property
    Public Function ConnectServiceName() As String
        Select Case currentDB
            Case DB_TYPE.DB_MASTER
                oraServiceName = "Server A"
                oraServiceName = oraServiceName & " [" & oraConn.DatabaseName & "]"
            Case DB_TYPE.DB_BACKUP
                oraServiceName = "Server B"
                oraServiceName = oraServiceName & " [" & oraConn.DatabaseName & "]"
            Case Else
                oraServiceName = "Server = N/A"
        End Select
        Return oraServiceName.ToUpper
    End Function

    Public Function Connect() As Boolean
        Dim vMsg As String
        Select Case currentDB
            Case DB_TYPE.DB_MASTER
                vMsg = "Database Connected-> Master"
                oraConn = New OracleConnection(connStrMaster)
                oraServiceName = "Server A"
            Case DB_TYPE.DB_BACKUP
                vMsg = "Database Connected-> Backup"
                oraConn = New OracleConnection(connStrBackup)
                oraServiceName = "Server B"
            Case Else
                vMsg = "Database Connected-> None"
                oraConn = New OracleConnection("")
                oraServiceName = "N/A"
        End Select
        Try
            'oraConn = New OracleConnection(oraConnectStr_Master)
            oraConn.Open()
            oraConnect = True
            'AddEventMessage("System", vMsg)
        Catch e As Exception
            'Console.WriteLine("Error: {0}", e.Message)
            oraConnect = False
            StartThread()
        End Try
        Return oraConnect
    End Function

    Public Sub Close()
        thrOracle.Abort()
        thrOracle = Nothing
        thrShutdown = True
        oraConnect = False
        If oraConn IsNot Nothing Then
            oraConn.Close()
            oraConn.Dispose()
            oraConn = Nothing
        End If
    End Sub

    Private Sub Reconnect()
        If Not oraConnect Then
            Try
                oraConn.Close()
            Catch e As Exception
            End Try
            Connect()
        End If
    End Sub

    Private Sub CheckExecute(ByVal pExe As Boolean)
        If pExe Then
            countDisconnect = 0
            If Not oraConnect Then
                oraConnect = True
            End If
        Else
            countDisconnect += 1
            If countDisconnect >= 3 Then
                If oraConnect Then
                    oraConnect = False
                    RaiseEvent OnDisconnect()
                    StartThread()
                End If
                countDisconnect = 0
            End If
        End If
    End Sub

    Public Function OpenDys(ByVal pStrSQL As String, ByVal pTableName As String, ByRef pDataSet As DataSet, _
                            Optional ByRef SQL_Execution_Error As String = "") As Boolean
        Dim oda As OracleDataAdapter
        Dim ds As New DataSet
        Dim bCheck As Boolean = False
        'ds = Nothing
        SQL_Execution_Error = ""
        If oraConnect Then
            Try
                oda = New OracleDataAdapter(pStrSQL, oraConn)
                oda.Fill(ds, pTableName)
                pDataSet = ds
                CheckExecute(True)
                bCheck = True
                'Return True
            Catch ex As Exception
                mLog.WriteSQLMessage("<Open Dynaset Error> " & pStrSQL)
                mLog.WriteSQLMessage("<Open Dynaset Error> " & ex.ToString)
                SQL_Execution_Error = ex.ToString
                CheckExecute(False)
                MsgBox(SQL_Execution_Error, MsgBoxStyle.Information)
                'Return False
            End Try

            pDataSet = ds
            ds = Nothing
            oda = Nothing

        End If
        Return bCheck
    End Function

    Public Function OpenDys(ByVal pStrSQL As String, ByVal pMaxRecord As Integer, ByVal pTableName As String, ByRef pDataSet As DataSet, _
                            Optional ByRef SQL_Execution_Error As String = "") As Boolean
        Dim oda As OracleDataAdapter
        Dim ds As New DataSet
        Dim bCheck As Boolean = False
        'ds = Nothing
        SQL_Execution_Error = ""
        If oraConnect Then
            Try
                oda = New OracleDataAdapter(pStrSQL, oraConn)
                oda.Fill(ds, 0, pMaxRecord, pTableName)
                pDataSet = ds
                CheckExecute(True)
                bCheck = True
                'Return True
            Catch ex As Exception
                mLog.WriteSQLMessage("<Open Dynaset Error> " & pStrSQL)
                mLog.WriteSQLMessage("<Open Dynaset Error> " & ex.ToString)
                SQL_Execution_Error = ex.ToString
                CheckExecute(False)
                MsgBox(SQL_Execution_Error, MsgBoxStyle.Information)
                'Return False
            End Try

            pDataSet = ds
            ds = Nothing
            oda = Nothing
            Return bCheck
        End If
    End Function

    Public Function ExeSQL(ByVal pStrSQL As String, Optional ByRef pSQL_Execution_Error As String = "") As Boolean

        Dim oCommand As OracleCommand

        If oraConnect Then
            oCommand = New OracleCommand(pStrSQL, oraConn)
            Try
                oCommand.ExecuteNonQuery()
                CheckExecute(True)
                Return True
            Catch e As Exception
                mLog.WriteSQLMessage("<ExeSQL Error> " & pStrSQL)
                mLog.WriteSQLMessage("<ExeSQL Error> " & e.ToString)
                pSQL_Execution_Error = e.ToString
                CheckExecute(False)
                MsgBox(pSQL_Execution_Error, MsgBoxStyle.Information)
                Return False
            End Try

        Else
            Return False
        End If
    End Function

    Public Function ExeSQL(ByVal pStrSQL As String, ByRef pParam As COraParameter, Optional ByVal pSQL_Execution_Error As String = "") As Boolean

        Dim OraCmd As OracleCommand
        Dim i As Integer
        Dim bCheck As Boolean = False

        If oraConnect Then
            OraCmd = New OracleCommand()

            Try
                If pParam IsNot Nothing Then  'execute with parameter
                    For Each p As OracleParameter In pParam.OraParam
                        OraCmd.Parameters.Add(p)
                    Next
                End If
                OraCmd.CommandText = pStrSQL
                OraCmd.CommandType = CommandType.Text
                OraCmd.Connection = oraConn
                OraCmd.ExecuteNonQuery()

                bCheck = True
                CheckExecute(True)
                pSQL_Execution_Error = "Execute Successful."
            Catch ex As Exception
                mLog.WriteSQLMessage("<ExeSQL Error> " & pStrSQL)
                mLog.WriteSQLMessage("<ExeSQL Error> " & ex.ToString)
                pSQL_Execution_Error = ex.ToString
                CheckExecute(False)
                bCheck = False
            End Try


            OraCmd.Dispose()
            OraCmd = Nothing

        End If

        Return bCheck
    End Function

    Private Function GetOracleDbType(ByVal iOracleDbType As _OracleDbType) As OracleDbType
        If iOracleDbType = _OracleDbType.OraByte Then Return OracleDbType.Byte
        If iOracleDbType = _OracleDbType.OraBlob Then Return OracleDbType.Blob
        If iOracleDbType = _OracleDbType.OraDate Then Return OracleDbType.Date
        If iOracleDbType = _OracleDbType.OraDecimal Then Return OracleDbType.Decimal
        If iOracleDbType = _OracleDbType.OraDouble Then Return OracleDbType.Double
        If iOracleDbType = _OracleDbType.OraInt16 Then Return OracleDbType.Int16
        If iOracleDbType = _OracleDbType.OraInt32 Then Return OracleDbType.Int32
        If iOracleDbType = _OracleDbType.OraInt64 Then Return OracleDbType.Int64
        If iOracleDbType = _OracleDbType.OraLong Then Return OracleDbType.Long
        If iOracleDbType = _OracleDbType.OraSingle Then Return OracleDbType.Single
        If iOracleDbType = _OracleDbType.OraVarchar2 Then Return OracleDbType.Varchar2
        Return OracleDbType.Varchar2
    End Function

    Private Function GetOracleDbDirection(ByVal iOracleDbDirection As _OracleDbDirection) As ParameterDirection
        If iOracleDbDirection = _OracleDbDirection.OraInput Then Return ParameterDirection.Input
        If iOracleDbDirection = _OracleDbDirection.OraOutput Then Return ParameterDirection.Output
        Return ParameterDirection.Input
    End Function

#Region "Change Avtive database Server"

    Private Sub ScanActiveDatabase()
        Dim NewDB As DB_TYPE
        NewDB = SelectServer()
        If currentDB <> NewDB Then
            oraConnect = False
            currentDB = NewDB
            If currentDB <> DB_TYPE.DB_None Then
                Reconnect()
            End If
        Else
            If currentDB <> DB_TYPE.DB_None Then
                'isConnectedDB = GetConectServer(currentDB)
                'isMasterDatabase = GetIsMaster(currentDB)
                'If Not isConnectedDB Then
                If Not oraConnect Then
                    'currentDB = DB_TYPE.DB_None
                    oraConnect = False
                    Reconnect()
                End If
                'Connect()
            End If
        End If
    End Sub

    Private Function SelectServer() As DB_TYPE
        Dim ret As Integer = -1
        'ret = mIni.INIRead("D:\LLTLB\LLTLBConfig.ini", "SELECT", "SERVER", "")
        'Select Case ret
        '    Case 0
        '        GetSelectServerDb = DB_TYPE.DB_MASTER
        '    Case 1
        '        GetSelectServerDb = DB_TYPE.DB_BACKUP
        '    Case Else
        '        GetSelectServerDb = DB_TYPE.DB_None
        'End Select
        'GetSelectServerDb = DB_TYPE.DB_MASTER
        If thrScanDB(0).MasterDatabase <> -1 Or thrScanDB(1).MasterDatabase <> -1 Then 'check database connection (-1 = disconnect)
            If thrScanDB(0).MasterDatabase = 1 And thrScanDB(1).MasterDatabase = 1 Then 'compare last update_date
                Dim vResult As Integer = DateTime.Compare(thrScanDB(0).UpdateDate, thrScanDB(1).UpdateDate)
                If vResult >= 0 Then
                    ret = DB_TYPE.DB_MASTER
                Else
                    ret = DB_TYPE.DB_BACKUP
                End If
            Else
                If thrScanDB(0).MasterDatabase = 1 Then
                    ret = DB_TYPE.DB_MASTER
                End If
                If thrScanDB(1).MasterDatabase = 1 Then
                    ret = DB_TYPE.DB_BACKUP
                End If
            End If
        End If
        Return ret
    End Function

    Private Function GetConectServer(ByVal iServer As DB_TYPE) As Boolean
        'Dim ret As String

        'ret = 0
        'Select Case iServer
        '    Case DB_TYPE.DB_None
        '        ret = 0
        '    Case DB_TYPE.DB_MASTER
        '        ret = ReadIni("C:\WINDOWS\system32\TLASConfig.ini", "MASTER", "CONNECT", "")
        '    Case DB_TYPE.DB_SUBMASTER
        '        ret = ReadIni("C:\WINDOWS\system32\TLASConfig.ini", "BACKUP", "CONNECT", "")
        'End Select

        'GetConectServer = IIf(ret = "0", False, True)
        Return True
    End Function

    Private Function GetIsMaster(ByVal iServer As DB_TYPE) As Boolean
        Dim ret As String

        ret = 0
        'Select Case iServer
        '    Case DB_TYPE.DB_None
        '        ret = 0
        '    Case DB_TYPE.DB_MASTER
        '        ret = ReadIni("C:\WINDOWS\system32\TLASConfig.ini", "MASTER", "ISMASTER", "")
        '    Case DB_TYPE.DB_SUBMASTER
        '        ret = ReadIni("C:\WINDOWS\system32\TLASConfig.ini", "BACKUP", "ISMASTER", "")
        'End Select

        'GetIsMaster = IIf(ret = "0", False, True)

        Return True
    End Function

#End Region

End Class



Public Class COraParameter
    Public OraParam() As Oracle.DataAccess.Client.OracleParameter
    Public OraDirection As ParameterDirection
    Public OraDbType As OracleDbType

    Public Sub New(Optional ByVal pLenght As Integer = 0)
        If pLenght > 0 Then
            ReDim OraParam(pLenght)
        End If
    End Sub

    Protected Overrides Sub Finalize()
        OraParam = Nothing
        MyBase.Finalize()
    End Sub

    Public Sub CreateOracleParameter(ByVal pLenght As Integer)
        If pLenght > 0 Then
            ReDim OraParam(pLenght - 1)
            For i As Integer = 0 To pLenght - 1
                OraParam(i) = New Oracle.DataAccess.Client.OracleParameter
            Next
        End If
    End Sub

    Public Sub RemoveOracleParam()
        Try
            Dim i As Integer
            For i = 0 To OraParam.Length
                OraParam(i).Dispose()
            Next
        Catch ex As Exception

        End Try
        OraParam = Nothing
    End Sub

    Public Sub AddOracleParameter(ByVal pIndex As Integer, ByVal pName As String _
                                  , ByVal pDbType As Oracle.DataAccess.Client.OracleDbType _
                                  , ByVal pDbDirection As ParameterDirection, ByVal pSize As Integer)
        Try
            If (pIndex < OraParam.Length) Then
                With OraParam(pIndex)
                    .ParameterName = pName
                    .OracleDbType = pDbType
                    .Size = pSize
                    .Direction = pDbDirection
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub AddOracleParameter(ByVal pIndex As Integer, ByVal pName As String _
                                  , ByVal pDbType As Oracle.DataAccess.Client.OracleDbType _
                                  , ByVal pDbDirection As ParameterDirection)
        Try
            If (pIndex < OraParam.Length) Then
                With OraParam(pIndex)
                    .ParameterName = pName
                    .OracleDbType = pDbType
                    .Direction = pDbDirection
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub AddOracleParameter(ByVal pIndex As Integer, ByVal pName As String, _
                                  ByVal pDbType As Oracle.DataAccess.Client.OracleDbType _
                                  , ByVal pSize As Integer)
        Try
            If (pIndex < OraParam.Length) Then
                With OraParam(pIndex)
                    .ParameterName = pName
                    .OracleDbType = pDbType
                    .Size = pSize
                    .Direction = ParameterDirection.Output
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub AddOracleParameter(ByVal pIndex As Integer, ByVal pName As String, _
                                  ByVal pDbType As Oracle.DataAccess.Client.OracleDbType)
        Try
            If (pIndex < OraParam.Length) Then
                With OraParam(pIndex)
                    .ParameterName = pName
                    .OracleDbType = pDbType
                    .Direction = ParameterDirection.Output
                End With
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub SetOracleParameterValue(ByVal pIndex As Integer, ByRef pValue As Object)
        OraParam(pIndex).Value = pValue
    End Sub

    Public Sub SetOracleParameterValue(ByVal pName As String, ByRef pValue As Object)
        For i As Integer = 0 To OraParam.Length - 1
            If OraParam(i).ParameterName = pName Then
                OraParam(i).Value = pValue
                Exit For
            End If
        Next
    End Sub

    Public Sub GetOracleParameterValue(ByVal pIndex As Integer, ByRef pParam As OracleParameter)
        pParam = OraParam(pIndex)
    End Sub

    Public Sub GetOracleParameterValue(ByVal pName As String, ByRef pParam As OracleParameter)
        For Each p As OracleParameter In OraParam
            If p.ParameterName = pName Then
                pParam = p
                Exit For
            End If
        Next
    End Sub

End Class
Class ScanDatabase

    Dim thrScanDB As Thread
    Dim dbUser As String
    Dim dbPwd As String
    Dim dbName As String
    Dim oleConn As OleDbConnection
    'Dim chkDatabase As Integer = -1

    Public Sub New(ByVal pUser As String, ByVal pPwd As String, ByVal pDbName As String)
        dbUser = pUser
        dbPwd = pPwd
        dbName = pDbName
        StartThread()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        thrScanDB.Abort()
    End Sub

    Protected Sub StartThread()
        thrScanDB = New Thread(AddressOf RunProcess)
        thrScanDB.Start()
    End Sub

    Protected Sub RunProcess()
        While (1)
            oleConn = New OleDbConnection
            Dim oleDataReader As OleDbDataReader = Nothing
            Try
                oleConn.ConnectionString = "Provider=OraOLEDB.Oracle;User ID=" & dbUser &
                                                ";Password=" & dbPwd & ";Data Source=" & dbName & ";"
                oleConn.Open()

                Dim strSQL As String = "select t.config_data,t.update_date from tas.tas_config t where t.config_id=90" 'check database 1=master active

                If OpenDys(strSQL, oleDataReader) Then
                    If oleDataReader.HasRows Then
                        oleDataReader.Read()
                        _MasterDatabase = oleDataReader.Item("config_data")
                        _UpdateDate = oleDataReader.Item("update_date")

                        oleDataReader.Close()
                    End If
                Else
                    _MasterDatabase = -1
                End If
                Thread.Sleep(3000)

            Catch ex As Exception
                _MasterDatabase = -1
            End Try
            oleConn.Close()
            'oleDataReader.Close()
        End While
    End Sub

    Protected Function OpenDys(ByVal pStrSQL As String, ByRef pDataReader As OleDbDataReader, _
                            Optional ByRef pSQL_Execution_Error As String = "") As Boolean

        Dim oleCommand As New OleDbCommand
        Dim oleDataAdapter As New OleDbDataAdapter

        Dim vCheck As Boolean = False


        pSQL_Execution_Error = ""
        oleCommand.CommandTimeout = 3
        If oleConn.State = ConnectionState.Open Then
            Try
                oleCommand = New OleDbCommand(pStrSQL, oleConn)
                pDataReader = oleCommand.ExecuteReader()
                vCheck = True
                'Return True
            Catch ex As Exception
                mLog.WriteErrMessage("[Open Dynaset Error] " & pStrSQL)
                mLog.WriteErrMessage("[Open Dynaset Error] " & ex.ToString)
                pSQL_Execution_Error = ex.ToString

                'MsgBox(pSQL_Execution_Error, MsgBoxStyle.Information)
                'Return False
            End Try
        End If
        oleDataAdapter = Nothing

        Return vCheck
    End Function

#Region "property"
    Dim _MasterDatabase As Integer = -1
    Public ReadOnly Property MasterDatabase As Integer
        Get
            Return _MasterDatabase
        End Get
    End Property

    Dim _UpdateDate As Date
    Public ReadOnly Property UpdateDate As Date
        Get
            Return _UpdateDate
        End Get

    End Property
#End Region

End Class