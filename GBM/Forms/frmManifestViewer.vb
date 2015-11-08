Public Class frmManifestViewer

    Private bShutdown As Boolean = False
    Private oLocalManifest As SortedList
    Private oRemoteManfiest As SortedList
    Private oRestoreInfo As clsBackup
    Private oDataTable As DataTable

    Property RestoreInfo As clsBackup
        Get
            Return oRestoreInfo
        End Get
        Set(value As clsBackup)
            oRestoreInfo = value
        End Set
    End Property

    Property LocalManifestData As SortedList
        Get
            Return oLocalManifest
        End Get
        Set(value As SortedList)
            oLocalManifest = value
        End Set
    End Property

    Property RemoteManifestData As SortedList
        Get
            Return oRemoteManfiest
        End Get
        Set(value As SortedList)
            oRemoteManfiest = value
        End Set
    End Property

    Private Sub FormatManifest(ByVal iManifest As Integer)
        Dim oRow As Object()
        Dim oLoadData As SortedList

        If iManifest = 1 Then
            oLoadData = LocalManifestData
        Else
            oLoadData = RemoteManifestData
        End If

        oDataTable = New DataTable

        'Setup Columns
        oDataTable.Columns.Add("Name")
        oDataTable.Columns.Add("Backup Path")
        oDataTable.Columns.Add("Restore Path")
        oDataTable.Columns.Add("Absolute Path")
        oDataTable.Columns.Add("Date Updated")
        oDataTable.Columns.Add("Updated By")


        'Setup Data Types
        oDataTable.Columns(0).DataType = GetType(String)
        oDataTable.Columns(1).DataType = GetType(String)
        oDataTable.Columns(2).DataType = GetType(String)
        oDataTable.Columns(3).DataType = GetType(Boolean)
        oDataTable.Columns(4).DataType = GetType(DateTime)
        oDataTable.Columns(5).DataType = GetType(String)



        For Each o As clsBackup In oLoadData.Values
            oRow = New Object() {o.Name, o.FileName, o.RestorePath, o.AbsolutePath, o.DateUpdated, o.UpdatedBy}
            oDataTable.Rows.Add(oRow)
        Next

        'Sort
        oDataTable.DefaultView.Sort = "Name asc"
        dgView.DataSource = oDataTable

        'Setup Column Widths        
        dgView.Columns(0).MinimumWidth = 100
        dgView.Columns(1).MinimumWidth = 175
        dgView.Columns(2).MinimumWidth = 175
        dgView.Columns(3).MinimumWidth = 60
        dgView.Columns(4).MinimumWidth = 125
        dgView.Columns(5).MinimumWidth = 100

        dgView.Columns(0).FillWeight = 100
        dgView.Columns(1).FillWeight = 100
        dgView.Columns(2).FillWeight = 100
        dgView.Columns(3).FillWeight = 25
        dgView.Columns(4).FillWeight = 50
        dgView.Columns(5).FillWeight = 50

    End Sub

    Private Sub LoadCombo()
        Dim oCombo As New List(Of KeyValuePair(Of Integer, String))
        oCombo.Add(New KeyValuePair(Of Integer, String)(1, "Local Manifest"))
        oCombo.Add(New KeyValuePair(Of Integer, String)(2, "Remote Manifest"))
        cboManifest.DataSource = oCombo
        cboManifest.ValueMember = "key"
        cboManifest.DisplayMember = "value"
    End Sub

    Private Sub frmManifestInfo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        dgView.ReadOnly = True
        dgView.AllowUserToAddRows = False
        lblError.Text = String.Empty
        LoadCombo()
        FormatManifest(CInt(cboManifest.SelectedValue))
    End Sub

    Private Sub dgView_CellStateChanged(sender As Object, e As DataGridViewCellStateChangedEventArgs) Handles dgView.CellStateChanged
        lblError.ForeColor = Color.Black

        Select Case dgView.Columns(e.Cell.ColumnIndex).Index
            Case 0
                lblError.Text = "The name of the application."
            Case 1
                lblError.Text = "The location of the backup file to restore."
            Case 2
                lblError.Text = "When the backup file was updated from this computer."
            Case 3
                lblError.Text = "The name of this computer."
            Case 4
                lblError.Text = "When the local backup file was last updated."
            Case 5
                lblError.Text = "The name of the computer that performed the last backup."
        End Select
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmGameInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If Not bShutdown Then
            If MsgBox("Are you sure you want to close?", MsgBoxStyle.YesNo, "ABM") = MsgBoxResult.Yes Then
                bShutdown = True
            End If
        End If

        If Not bShutdown Then
            e.Cancel = True
        End If
    End Sub

    Private Sub cboManifest_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cboManifest.SelectionChangeCommitted
        FormatManifest(CInt(cboManifest.SelectedValue))
    End Sub
End Class