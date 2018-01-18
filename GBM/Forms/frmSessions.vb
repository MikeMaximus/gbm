Imports GBM.My.Resources
Imports System.Globalization
Imports System.ComponentModel

Public Class frmSessions

    Private bInitFinished As Boolean = False
    Private WithEvents tmFilterTimer As Timer
    Private bStartSortAsc As Boolean = True
    Private iStartDataCol As Integer
    Private iStartDisplayCol As Integer
    Private bEndSortAsc As Boolean = True
    Private iEndDataCol As Integer
    Private iEndDisplayCol As Integer
    Private bHoursSortAsc As Boolean = True
    Private iHoursCol As Integer

    Private Sub FormatGrid()
        'Build Columns
        dgSessions.Columns.Add("MonitorID", frmSessions_ColumnMonitorID)
        dgSessions.Columns.Add("Name", frmSessions_ColumnGameName)
        dgSessions.Columns.Add("StartUnix", frmSessions_ColumnStart)
        dgSessions.Columns.Add("Start", frmSessions_ColumnStart)
        dgSessions.Columns.Add("EndUnix", frmSessions_ColumnEnd)
        dgSessions.Columns.Add("End", frmSessions_ColumnEnd)
        dgSessions.Columns.Add("Hours", frmSessions_ColumnHours)

        'Get Column Indexes
        iStartDataCol = dgSessions.Columns.IndexOf(dgSessions.Columns("StartUnix"))
        iStartDisplayCol = dgSessions.Columns.IndexOf(dgSessions.Columns("Start"))
        iEndDataCol = dgSessions.Columns.IndexOf(dgSessions.Columns("EndUnix"))
        iEndDisplayCol = dgSessions.Columns.IndexOf(dgSessions.Columns("End"))
        iHoursCol = dgSessions.Columns.IndexOf(dgSessions.Columns("Hours"))

        'Set Sorting
        dgSessions.Columns("Start").SortMode = DataGridViewColumnSortMode.Programmatic
        dgSessions.Columns("End").SortMode = DataGridViewColumnSortMode.Programmatic
        dgSessions.Columns("Hours").SortMode = DataGridViewColumnSortMode.Programmatic

        'Hide Columns
        dgSessions.Columns("MonitorID").Visible = False
        dgSessions.Columns("StartUnix").Visible = False
        dgSessions.Columns("EndUnix").Visible = False
    End Sub

    Private Sub LoadData()
        Dim oData As DataSet
        Dim sFilter As String
        Dim dStart As DateTime
        Dim dEnd As DateTime
        Dim iHours As Double
        Dim iTotalHours As Double

        If txtFilter.Text = String.Empty Then
            oData = mgrSessions.GetSessionRange(dtpStart.Value, dtpEnd.Value)
        Else
            sFilter = txtFilter.Text.ToLower
            oData = mgrSessions.GetSessionsByGameNameAndRange(sFilter, dtpStart.Value, dtpEnd.Value)
        End If

        dgSessions.Rows.Clear()

        For Each dr As DataRow In oData.Tables(0).Rows
            dStart = mgrCommon.UnixToDate(dr("Start"))
            dEnd = mgrCommon.UnixToDate(dr("End"))
            iHours = Math.Round(dEnd.Subtract(dStart).TotalHours, 2)
            iTotalHours += iHours
            dgSessions.Rows.Add(New Object() {dr("MonitorID"), dr("Name"), dr("Start"), dStart, dr("End"), dEnd, iHours})
        Next

        lblTotalHours.Text = mgrCommon.FormatString(frmSessions_lblTotalHours, iTotalHours)

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
        Dim dtRecent As DateTime = dtMaxDate.Subtract(TimeSpan.FromDays(7))
        Dim sDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern & " " & CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern

        If dtRecent < dtMinDate Then
            dtRecent = dtMinDate
        End If

        bInitFinished = False

        'Init Date Fields
        dtpStart.Format = DateTimePickerFormat.Custom
        dtpEnd.Format = DateTimePickerFormat.Custom
        dtpStart.CustomFormat = sDateTimeFormat
        dtpEnd.CustomFormat = sDateTimeFormat

        'Setting max or min dates breaks the control in Mono
        If Not mgrCommon.IsUnix Then
            dtpStart.MinDate = dtMinDate
            dtpStart.MaxDate = dtMaxDate
            dtpEnd.MinDate = dtMinDate
            dtpEnd.MaxDate = dtMaxDate
        End If

        dtpStart.Value = dtRecent
        dtpEnd.Value = dtMaxDate

        'Init Text Filter
        txtFilter.Text = String.Empty

        bInitFinished = True
    End Sub

    Private Sub Reset()
        ClearManualSortGlyphs()
        ResetFilterFields()
        LoadData()
    End Sub

    Private Sub DeleteSession()
        Dim oSession As clsSession
        Dim oSessions As New List(Of clsSession)

        For Each dgvRow As DataGridViewRow In dgSessions.SelectedRows
            oSession = New clsSession
            oSession.MonitorID = dgvRow.Cells(0).Value
            oSession.SessionStart = CInt(dgvRow.Cells(iStartDataCol).Value)
            oSession.SessionEnd = CInt(dgvRow.Cells(iEndDataCol).Value)
            oSessions.Add(oSession)
        Next

        If oSessions.Count > 0 Then
            mgrSessions.DeleteSession(oSessions)
        End If
    End Sub

    Private Sub ClearManualSortGlyphs()
        dgSessions.Columns(iStartDisplayCol).HeaderCell.SortGlyphDirection = SortOrder.None
        dgSessions.Columns(iEndDisplayCol).HeaderCell.SortGlyphDirection = SortOrder.None
        dgSessions.Columns(iHoursCol).HeaderCell.SortGlyphDirection = SortOrder.None
    End Sub

    Private Function GetSortOrder(ByVal bToggle As Boolean, ByVal iCol As Integer) As ListSortDirection
        Dim oSortType As ListSortDirection

        If bToggle Then
            oSortType = ListSortDirection.Ascending
            dgSessions.Columns(iCol).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        Else
            oSortType = ListSortDirection.Descending
            dgSessions.Columns(iCol).HeaderCell.SortGlyphDirection = SortOrder.Descending
        End If

        Return oSortType
    End Function

    Private Sub HandleSort(ByVal iCol As Integer)
        ClearManualSortGlyphs()

        Select Case iCol
            Case iStartDisplayCol
                bStartSortAsc = Not bStartSortAsc
                dgSessions.Sort(dgSessions.Columns(iStartDataCol), GetSortOrder(bStartSortAsc, iCol))
            Case iEndDisplayCol
                bEndSortAsc = Not bEndSortAsc
                dgSessions.Sort(dgSessions.Columns(iEndDataCol), GetSortOrder(bEndSortAsc, iCol))
            Case iHoursCol
                bHoursSortAsc = Not bHoursSortAsc
                If bHoursSortAsc Then
                    dgSessions.Sort(New RowComparer(SortOrder.Ascending, iHoursCol))
                    dgSessions.Columns(iHoursCol).HeaderCell.SortGlyphDirection = SortOrder.Ascending
                Else
                    dgSessions.Sort(New RowComparer(SortOrder.Descending, iHoursCol))
                    dgSessions.Columns(iHoursCol).HeaderCell.SortGlyphDirection = SortOrder.Descending
                End If
        End Select
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
                LoadData()
            End If
        End If
    End Sub

    Private Sub dgSessions_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgSessions.ColumnHeaderMouseClick
        HandleSort(e.ColumnIndex)
    End Sub

    Private Class RowComparer
        Implements System.Collections.IComparer

        Private sortOrderModifier As Integer = 1
        Private iSortCol As Integer = 0

        Public Sub New(ByVal sortOrder As SortOrder, ByVal iCol As Integer)
            iSortCol = iCol

            If sortOrder = SortOrder.Descending Then
                sortOrderModifier = -1
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrderModifier = 1
            End If
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements System.Collections.IComparer.Compare

            Dim DataGridViewRow1 As DataGridViewRow = CType(x, DataGridViewRow)
            Dim DataGridViewRow2 As DataGridViewRow = CType(y, DataGridViewRow)

            Dim CompareResult As Integer = If(CDec(DataGridViewRow1.Cells(iSortCol).Value) < CDec(DataGridViewRow2.Cells(iSortCol).Value), -1, 1)

            Return CompareResult * sortOrderModifier
        End Function
    End Class

End Class