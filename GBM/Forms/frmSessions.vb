Imports GBM.My.Resources

Public Class frmSessions

    Private WithEvents tmFilterTimer As Timer

    Private Sub FormatGrid()
        dgSessions.Columns.Add("Name", frmSessions_ColumnGameName)
        dgSessions.Columns.Add("Start", frmSessions_ColumnStart)
        dgSessions.Columns.Add("End", frmSessions_ColumnEnd)
    End Sub

    Private Sub LoadData()
        Dim oData As DataSet
        Dim sFilter As String

        If txtFilter.Text = String.Empty Then
            oData = mgrSessions.GetSessions
        Else
            sFilter = txtFilter.Text.ToLower
            oData = mgrSessions.GetSessionsByGameName(sFilter)
        End If

        dgSessions.Rows.Clear()

        For Each dr As DataRow In oData.Tables(0).Rows
            dgSessions.Rows.Add(New Object() {dr("Name"), mgrCommon.UnixToDate(dr("Start")), mgrCommon.UnixToDate(dr("End"))})
        Next

        dgSessions.AutoResizeColumns()
    End Sub

    Private Sub SetForm()
        Me.Text = frmSessions_Name

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        LoadData()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub frmSession_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        FormatGrid()
        LoadData()
    End Sub

End Class