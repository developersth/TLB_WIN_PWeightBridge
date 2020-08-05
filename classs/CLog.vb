Imports System
Imports System.IO
Imports System.Threading

Public Class CLog
    Private PathLog As String
    Private PathComportLog As String
    Private CurrentDate As Date
    'Private mThreadLog As Thread
    'Private bCancelDeleteLog As Boolean = False

    Public Sub New()
        InitialLogPath()
        'mThreadLog = New Thread(New ThreadStart(AddressOf RunProcessLog))
        'mThreadLog.Start()
    End Sub

    'Private Sub RunProcessLog()
    '    While (1)
    '        If bCancelDeleteLog Then
    '            Exit While
    '        End If
    '        MainMaintailData()
    '        System.Threading.Thread.Sleep(60000)
    '    End While
    'End Sub

    Private Sub InitialLogPath()
        PathLog = My.Application.Info.DirectoryPath & "\Log"
        Directory.CreateDirectory(PathLog)
        ScanDeleteLog(PathLog, 120)

        PathComportLog = My.Application.Info.DirectoryPath & "\ComportLog"
        Directory.CreateDirectory(PathComportLog)
        ScanDeleteLog(PathComportLog, 120)
    End Sub

    Public Sub MainMaintailData()
        If CheckDelete() Then
            ScanDeleteLog(PathLog, 30)
        End If
    End Sub

    Private Function CheckDelete() As Boolean
        If CurrentDate.Day <> Now.Day Then
            CurrentDate = Now
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ScanDeleteLog(ByVal pPathFile As String, ByVal pNumDay As Integer)
        Dim DateLastModified As DateTime
        Dim DateDelete As DateTime

        Try
            DateDelete = DateTime.Now.AddDays(-1 * pNumDay)
            Dim fileEntries As String() = Directory.GetFiles(pPathFile)
            For Each Filename As String In fileEntries
                DateLastModified = File.GetCreationTime(Filename)
                If DateLastModified < DateDelete Then
                    File.Delete(Filename)
                End If
            Next

        Catch ex As Exception

        End Try
    End Sub

    Public Sub WriteSQLMessage(ByVal pMsg As String)
        'Me.ListView1.Items.Insert(0, Format(Now, "dd/MM/yyyy HH:mm:ss"))
        'Me.ListView1.Items.Item(0).SubItems.Add(mSource)
        'Me.ListView1.Items.Item(0).SubItems.Add(mMsg)
        Try
            Dim m_FILE_NAME As String
            m_FILE_NAME = PathLog & "\SQL_" & Format(DateTime.Now, "dd-MM-yyyy") & ".log"
            Dim sw As StreamWriter = File.AppendText(m_FILE_NAME)
            'sw.WriteLine(Format(Now, "dd/MM/yyyy HH:mm:ss") & " " & mSource & " " & mMsg)
            sw.WriteLine(DateTime.Now & " " & pMsg)
            sw.Close()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub WriteErrMessage(ByVal pMsg As String)
        'Me.ListView1.Items.Insert(0, Format(Now, "dd/MM/yyyy HH:mm:ss"))
        'Me.ListView1.Items.Item(0).SubItems.Add(mSource)
        'Me.ListView1.Items.Item(0).SubItems.Add(mMsg)
        Try
            Dim m_FILE_NAME As String
            m_FILE_NAME = PathLog & "\Err_" & Format(DateTime.Now, "dd-MM-yyyy") & ".log"
            Dim sw As StreamWriter = File.AppendText(m_FILE_NAME)
            'sw.WriteLine(Format(Now, "dd/MM/yyyy HH:mm:ss") & " " & mSource & " " & mMsg)
            sw.WriteLine(DateTime.Now & " " & pMsg)
            sw.Close()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub WriteLog(ByVal pFileName As String, ByVal pMsg As String)
        Try
            Dim m_FILE_NAME As String
            m_FILE_NAME = PathLog & "\Log" & pFileName & "_" & Format(DateTime.Now, "dd-MM-yyyy") & ".log"
            Dim sw As StreamWriter = File.AppendText(m_FILE_NAME)
            'sw.WriteLine(Format(Now, "dd/MM/yyyy HH:mm:ss") & " " & mSource & " " & mMsg)
            sw.WriteLine("<" & pFileName & "><" & DateTime.Now & ">>" & pMsg)
            sw.Close()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub WriteComportLog(ByVal pFileName As String, ByVal pMsg As String)
        Try
            Dim m_FILE_NAME As String
            m_FILE_NAME = PathComportLog & "\Log" & pFileName & "_" & Format(DateTime.Now, "dd-MM-yyyy") & ".log"
            Dim sw As StreamWriter = File.AppendText(m_FILE_NAME)
            'sw.WriteLine(Format(Now, "dd/MM/yyyy HH:mm:ss") & " " & mSource & " " & mMsg)
            sw.WriteLine("<" & pFileName & "><" & DateTime.Now & ">>" & pMsg)
            sw.Close()
        Catch ex As Exception

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        'bCancelDeleteLog = False
        MyBase.Finalize()
    End Sub
End Class
