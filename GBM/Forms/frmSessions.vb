Imports GBM.My.Resources
Imports System.Globalization

Public Class frmSessions

    Private bInitFinished As Boolean = False
    Private WithEvents tmFilterTimer As Timer

    Private Sub FormatGrid()
        dgSessions.Columns.Add("MonitorID", frmSessions_ColumnMonitorID)
        dgSessions.Columns.Add("Name", frmSessions_ColumnGameName)
        dgSessions.Columns.Add("Start", frmSessions_ColumnStart)
        dgSessions.Columns.Add("End", frmSessions_ColumnEnd)

        'Hide the ID
        dgSessions.Columns("MonitorID").Visible = False
    End Sub

    Private Sub LoadData()
        Dim oData As DataSet
        Dim sFilter As String

        If txtFilter.Text = String.Empty Then
            oData = mgrSessions.GetSessionRange(dtpStart.Value, dtpEnd.Value)
        Else
            sFilter = txtFilter.Text.ToLower
            oData = mgrSessions.GetSessionsByGameNameAndRange(sFilter, dtpStart.Value, dtpEnd.Value)
        End If

        dgSessions.Rows.Clear()

        For Each dr As DataRow In oData.Tables(0).Rows
            dgSessions.Rows.Add(New Object() {dr("MonitorID"), dr("Name"), mgrCommon.UnixToDate(dr("Start")), mgrCommon.UnixToDate(dr("End"))})
        Next

        dgSessions.AutoResizeColumns()
    End Sub

    Private Sub SetForm()
        Me.Text = frmSessions_Name

        'Init Labels
        lblFilter.Text = frmSessions_lblFilter
        lblDateRange.Text = frmSessions_lblDateRange
        btnDelete.Text = frmSessions_btnDelete
        btnReset.Text = frmSessions_btnReset
        btnClose.Text = frmSessions_btnClose

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False


    End Sub

    Private Sub ResetFilterFields()
        Dim dtMinDate As DateTime = mgrSessions.GetMinimumDateTime
        Dim dtMaxDate As DateTime = mgrSessions.GetMaximumDateTime
        Dim sDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern & " " & CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern

        bInitFinished = False

        'Init Date Fields
        dtpStart.Format = DateTimePickerFormat.Custom
        dtpEnd.Format = DateTimePickerFormat.Custom
        dtpStart.CustomFormat = sDateTimeFormat
        dtpEnd.CustomFormat = sDateTimeFormat
        dtpStart.MinDate = dtMinDate
        dtpStart.MaxDate = dtMaxDate
        dtpEnd.MinDate = dtMinDate
        dtpEnd.MaxDate = dtMaxDate
        dtpStart.Value = dtMinDate
        dtpEnd.Value = dtMaxDate

        'Init Text Filter
        txtFilter.Text = String.Empty

        bInitFinished = True
    End Sub

    Private Sub Reset()
        ResetFilterFields()
        LoadData()
    End Sub

    Private Sub DeleteSession()
        Dim oSession As clsSession
        Dim oSessions As New List(Of clsSession)

        For Each dgvRow As DataGridViewRow In dgSessions.SelectedRows
            oSession = New clsSession
            oSession.MonitorID = dgvRow.Cells(0).Value
            oSession.SessionStart = dgvRow.Cells(2).Value
            oSession.SessionEnd = dgvRow.Cells(3).Value
            oSessions.Add(oSession)
        Next

        If oSessions.Count > 0 Then
            mgrSessions.DeleteSession(oSessions)
        End If
    End Sub

    Private Sub frmSession_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        ResetFilterFields()
        FormatGrid()
        LoadData()
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        LoadData()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub dtpStart_ValueChanged(sender As Object, e As EventArgs) Handles dtpStart.ValueChanged
        If bInitFinished Then LoadData()
    End Sub

    Private Sub dtpEnd_ValueChanged(sender As Object, e As EventArgs) Handles dtpEnd.ValueChanged
        If bInitFinished Then LoadData()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgSessions.SelectedRows.Count > 0 Then
            If mgrCommon.ShowMessage(frmSessions_ConfirmDelete, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                DeleteSession()
                Reset()
            End If
        End If
    End Sub
End Class