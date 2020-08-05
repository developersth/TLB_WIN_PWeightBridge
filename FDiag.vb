
Public Class FDiag
    Dim mWeight As CWeightBridge
    Dim mCRWeight As CCardReader
    Dim mLog As New CLog

    Public Sub Initial(ByRef pCWeightBridge As CWeightBridge, ByRef pCCardReader As CCardReader)
        mWeight = pCWeightBridge
        mCRWeight = pCCardReader

        lblCRComport.Text = mCRWeight.mCRWeight.ComportNo & ":" & mCRWeight.mCRWeight.ComportSetting
        lblWBComport.Text = mWeight.mWeight.ComportNo & ":" & mWeight.mWeight.ComportSetting
        Timer1.Interval = 1000
        Timer1.Enabled = True
    End Sub

    Private Sub ShowDiagnostic()
        Try
            txtWBRecv.Text = Now & "->" & mWeight.WeightString
            txtCRSend.Text = Now & "->" & mCRWeight.mCRWeight.MsgSend
            txtCRRecv.Text = Now & "->" & mCRWeight.mCRWeight.MsgRecv

            ListMain.DataSource = Nothing
            ListMain.DataSource = mCRWeight.MyListDataSource
            WriteComportLog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ShowDiagnostic()
    End Sub

    Private Sub WriteComportLog()
        mLog.WriteComportLog(mCRWeight.mCRWeight.ComportNo, txtCRSend.Text)
        mLog.WriteComportLog(mCRWeight.mCRWeight.ComportNo, txtCRRecv.Text)

        mLog.WriteComportLog(mWeight.mWeight.ComportNo, txtWBRecv.Text)
    End Sub

    Private Sub ListMain_Click(sender As Object, e As EventArgs) Handles ListMain.Click
        Timer1.Stop()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Timer1.Start()
    End Sub
End Class