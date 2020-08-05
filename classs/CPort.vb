Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports System.Net.Sockets

Public Class CPort : Implements IDisposable

    'Event OnMessageComport(ByVal pMsg As String)
    Event OnDataReceive(ByVal pMsgRecv As String)
    Event OnConnect()
    Event OnDisconnect()

    Structure _argumentPort
        Dim IsTCP As Boolean
        Public portNo As String
        Public portSetting As String
        Public portType As String
        Public IP_Port As Integer
        Public IP_Address As String
        Public IsOpen As Boolean
        Public IsIdle As Boolean
        Public IsEnable As Boolean
    End Structure

    Dim mArgumentPort As _argumentPort
    Dim mSp As Ports.SerialPort
    Dim mResponseTime As DateTime
    Dim mResponse As Boolean

#Region "Thread"
    Dim mConnect As Boolean
    Dim mShutdown As Boolean
    Dim mRunn As Boolean
    Dim mThreadComport As Thread

    Public Sub StartThread()
        mRunn = True
        mResponseTime = Now
        mResponse = True
        mThreadComport = New Thread(AddressOf RunProcess)
        mThreadComport.Name = mArgumentPort.portNo
        mThreadComport.Start()

    End Sub

    Private Sub RunProcess()
        Thread.Sleep(1000)
        While mRunn
            If mConnect Then
                Exit While
            End If

            OpenPort()
            Thread.Sleep(3000)
        End While

        mThreadComport.Abort()
    End Sub
#End Region

    Public Sub InitialPort(ByVal pComportNo As String, ByVal pComportSetting As String)
        mArgumentPort.portNo = pComportNo
        mArgumentPort.portSetting = pComportSetting
    End Sub

    Private Sub OpenPort()
        Try
            'mSp = New Ports.SerialPort
            With mSp
                Dim sPli() As String = mArgumentPort.portSetting.Split(",")
                .PortName = mArgumentPort.portNo
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

            mSp.ReadTimeout = 500
            mSp.WriteTimeout = 1000
            mSp.Open()
            mConnect = True
            mArgumentPort.IsOpen = True
            'RaiseEvent OnMessageComport("Open comport[" + mArgumentPort.portNo + " : " + mArgumentPort.portSetting + "] successfull.")
            RaiseEvent OnConnect()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ClosePort()
        Try
            mSp.DiscardOutBuffer()
            mSp.Close()
            mArgumentPort.IsOpen = False
            'RaiseEvent OnMessageComport("Close comport[" + mArgumentPort.portNo + " : " + mArgumentPort.portSetting +
            '                  "] successfull.")
            RaiseEvent OnDisconnect()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub SendData(ByVal pMsgSend As String)
        Try
            If IsOpen() Then
                mSp.Write(pMsgSend)
                mSp.DiscardOutBuffer()
            End If
        Catch ex As Exception
            CheckResponse()
        End Try
    End Sub

    Public Sub SendData(ByVal pMsgSend() As Byte)
        Try
            If IsOpen() Then
                mSp.Write(pMsgSend, 0, pMsgSend.Length)
                mSp.DiscardOutBuffer()
            End If
        Catch ex As Exception
            CheckResponse()
        End Try
    End Sub

    Public Sub ReceiveData(ByRef pMsgRecv As String)
        Try
            If IsOpen() Then
                pMsgRecv = mSp.ReadExisting
                mSp.DiscardInBuffer()
            Else
                CheckResponse()
            End If
        Catch ex As Exception
            CheckResponse()
            pMsgRecv = ""
        End Try
    End Sub

    Public Function IsOpen() As Boolean
        Return mArgumentPort.IsOpen
    End Function

    Private Sub CheckResponse()
        Dim vDateTime As DateTime = Now
        Dim vDiff As Double = (vDateTime - mResponseTime).TotalSeconds
        If vDiff > 5 And mResponse = True Then
            mResponseTime = Now
            mResponse = False
            If mSp.IsOpen Then
                ClosePort()
                'StartThread()
            End If
            StartThread()
        End If
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
            ClosePort()
            mShutdown = True
            mRunn = False
            mSp.Dispose()
            ' Call the appropriate methods to clean up  
            ' unmanaged resources here. 
            ' If disposing is false,  
            ' only the following code is executed.

            ' Note disposing has been done.
            disposed = True

        End If
    End Sub
#End Region

    'Private Sub mSp_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles mSp.DataReceived
    '    RaiseEvent OnDataReceive(mSp.ReadExisting())
    '    mSp.DiscardInBuffer()
    'End Sub

    Public Sub New(ByRef pSerialPort As Ports.SerialPort)
        'mSp = New Ports.SerialPort
        mSp = pSerialPort
    End Sub
End Class
