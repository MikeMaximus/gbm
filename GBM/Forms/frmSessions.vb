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
        Dim dHours As Double
        Dim dTotalHours As Double

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
            dHours = Math.Round(dEnd.Subtract(dStart).TotalHours, 2)
            dTotalHours += dHours
            dgSessions.Rows.Add(New Object() {dr("MonitorID"), dr("Name"), dr("Start"), dStart, dr("End"), dEnd, dHours})
        Next

        lblTotalHours.Text = mgrCommon.FormatString(frmSessions_lblTotalHours, dTotalHours)

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

    Private Sub DoSort(ByRef bToggle As Boolean, ByVal iCol As Integer, ByVal iType As RowCompareHelper.iDataType)
        bToggle = Not bToggle
        If bToggle Then
            dgSessions.Sort(New RowCompareHelper(SortOrder.Ascending, iCol, iType))
            dgSessions.Columns(iCol).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        Else
            dgSessions.Sort(New RowCompareHelper(SortOrder.Descending, iCol, iType))
            dgSessions.Columns(iCol).HeaderCell.SortGlyphDirection = SortOrder.Descending
        End If
    End Sub

    Private Sub HandleSort(ByVal iCol As Integer)
        ClearManualSortGlyphs()

        Select Case iCol
            Case iStartDisplayCol
                DoSort(bStartSortAsc, iStartDisplayCol, RowCompareHelper.iDataType.DateTimeType)
            Case iEndDisplayCol
                DoSort(bEndSortAsc, iEndDisplayCol, RowCompareHelper.iDataType.DateTimeType)
            Case iHoursCol
                DoSort(bHoursSortAsc, iHoursCol, RowCompareHelper.iDataType.DecimalType)
        End Select
    End Sub

    Private Sub ExportGrid()
        Dim frm As New frmSessionExport
        Dim sLocation As String

        frm.ShowDialog()

        If frm.DialogResult = DialogResult.OK Then

            If frm.XML Then
                sLocation = mgrCommon.SaveFileBrowser("Session_Export", frmSessions_ChooseExportLocation, "xml", frmSessions_XML, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmSessions_DefaultExportFileName)
            Else
                sLocation = mgrCommon.SaveFileBrowser("Session_Export", frmSessions_ChooseExportLocation, "csv", frmSessions_CSV, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmSessions_DefaultExportFileName)
            End If

            If sLocation <> String.Empty Then
                If frm.XML Then
                    mgrSessions.ExportAsXML(sLocation, frm.Unix, dgSessions)
                Else
                    mgrSessions.ExportAsCSV(sLocation, frm.Unix, frm.Headers, dgSessions)
                End If
            End If
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
                LoadData()
            End If
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportGrid()
    End Sub

    Private Sub dgSessions_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgSessions.ColumnHeaderMouseClick
        HandleSort(e.ColumnIndex)
    End Sub

    'The Mono version of the DataGridView control automatically treats all data as a string for sorting purposes.
    'This class manually handles column sorting by data type.
    Private Class RowCompareHelper
        Implements System.Collections.IComparer

        'We need to manually define data types as the column ValueType doesn't work in Mono either.
        Public Enum iDataType As Integer
            StringType = 1
            DateTimeType = 2
            IntType = 3
            DecimalType = 4
        End Enum

        Private iSortOrderModifier As Integer = 1
        Private iSortCol As Integer = 0
        Private iDataTypeCol As iDataType = iDataType.StringType

        Public Sub New(ByVal sortOrder As SortOrder, ByVal iCol As Integer, ByVal iType As iDataType)
            iSortCol = iCol
            iDataTypeCol = iType

            If sortOrder = SortOrder.Descending Then
                iSortOrderModifier = -1
            ElseIf sortOrder = SortOrder.Ascending Then
                iSortOrderModifier = 1
            End If
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim iCompareResult As Integer
            Dim dgRow1 As DataGridViewRow = CType(x, DataGridViewRow)
            Dim dgRow2 As DataGridViewRow = CType(y, DataGridViewRow)

            Select Case iDataTypeCol
                Case iDataType.DecimalType
                    iCompareResult = If(CDec(dgRow1.Cells(iSortCol).Value) < CDec(dgRow2.Cells(iSortCol).Value), -1, 1)
                Case iDataType.IntType
                    iCompareResult = If(CInt(dgRow1.Cells(iSortCol).Value) < CInt(dgRow2.Cells(iSortCol).Value), -1, 1)
                Case iDataType.StringType
                    iCompareResult = String.Compare(CStr(dgRow1.Cells(iSortCol).Value), CStr(dgRow2.Cells(iSortCol).Value))
                Case iDataType.DateTimeType
                    iCompareResult = Date.Compare(CDate(dgRow1.Cells(iSortCol).Value), CDate(dgRow2.Cells(iSortCol).Value))
            End Select

            Return iCompareResult * iSortOrderModifier
        End Function
    End Class

End Class