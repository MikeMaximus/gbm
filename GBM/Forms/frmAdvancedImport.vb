Imports GBM.My.Resources
Imports System.IO

Public Class frmAdvancedImport

    Private oImportData As ExportData
    Private hshImportData As Hashtable
    Private hshFinalData As New Hashtable
    Private bSelectAll As Boolean = True
    Private bIsLoading As Boolean = False
    Private iCurrentSort As Integer = 0
    Private oImageList As ImageList
    Private WithEvents tmFilterTimer As Timer

    Public Property ImportInfo As ExportData
        Set(value As ExportData)
            oImportData = value
        End Set
        Get
            Return oImportData
        End Get
    End Property

    Public Property ImportData As Hashtable
        Set(value As Hashtable)
            hshImportData = value
        End Set
        Get
            Return hshImportData
        End Get
    End Property

    Public ReadOnly Property FinalData As Hashtable
        Get
            Return hshFinalData
        End Get
    End Property

    Private Sub SelectToggle()
        Cursor.Current = Cursors.WaitCursor
        lstGames.BeginUpdate()
        bSelectAll = Not bSelectAll
        For i As Integer = 0 To lstGames.Items.Count - 1
            lstGames.Items(i).Checked = bSelectAll
        Next
        lstGames.EndUpdate()
        Cursor.Current = Cursors.Default
        UpdateSelected()
    End Sub

    Private Sub SaveChecked(ByVal oItem As ListViewItem)
        If oItem.Checked Then
            If Not FinalData.ContainsKey(oItem.Tag) Then FinalData.Add(oItem.Tag, ImportData(oItem.Tag))
        Else
            FinalData.Remove(oItem.Tag)
        End If
    End Sub

    Private Function CheckIgnoreTags(ByVal oTags As List(Of Tag)) As Boolean
        Dim oTag As Tag
        Dim sTag As String
        Dim sIgnoreTags() As String = {"DOSBox", "ScummVM"}

        For Each oTag In oTags
            For Each sTag In sIgnoreTags
                If oTag.Name = sTag Then
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    Private Sub LoadData(Optional ByVal sFilter As String = "", Optional ByVal bSelectedOnly As Boolean = False, Optional ByVal bAutoDetect As Boolean = False)
        Dim oApp As clsGame
        Dim oListViewItem As ListViewItem
        Dim sTags As String
        Dim bAddItem As Boolean
        Dim bResetSelectAll As Boolean = False

        Cursor.Current = Cursors.WaitCursor
        lstGames.BeginUpdate()

        lstGames.Clear()

        lstGames.Columns.Add(frmAdvancedImport_ColumnName, 315)
        lstGames.Columns.Add(frmAdvancedImport_ColumnProcess, 130)
        lstGames.Columns.Add(frmAdvancedImport_ColumnTags, 290)

        For Each de As DictionaryEntry In ImportData
            bAddItem = False
            oApp = DirectCast(de.Value, clsGame)

            sTags = String.Empty
            oApp.ImportTags.Sort(AddressOf mgrCommon.CompareImportTagsByName)
            For Each oTag As Tag In oApp.ImportTags
                sTags &= oTag.Name & ", "
            Next
            sTags = sTags.TrimEnd(New Char() {",", " "})

            oListViewItem = New ListViewItem(New String() {oApp.Name, oApp.ProcessName, sTags})
            oListViewItem.Tag = oApp.ID

            If FinalData.ContainsKey(oApp.ID) Then
                oListViewItem.Checked = True
            Else
                oListViewItem.Checked = False
            End If

            If bAutoDetect Then
                If oApp.AbsolutePath Then
                    If Directory.Exists(oApp.Path) Then
                        oListViewItem.Checked = True
                        SaveChecked(oListViewItem)
                    End If
                End If
            End If

            If oApp.ImportUpdate Then
                oListViewItem.ImageIndex = 1
                oListViewItem.Checked = True
            Else
                oListViewItem.ImageIndex = 0
            End If

            If sFilter = String.Empty Then
                bAddItem = True
            Else
                If oApp.Name.ToLower.Contains(sFilter.ToLower) Or oApp.ProcessName.ToLower.Contains(sFilter.ToLower) Or sTags.ToLower.Contains(sFilter.ToLower) Then
                    bAddItem = True
                End If
            End If

            If bSelectedOnly Then
                If Not oListViewItem.Checked Then
                    bAddItem = False
                End If
            End If

            'Check for hardcoded ignore tags
            If bAddItem And (mgrCommon.IsUnix And oApp.OS = clsGame.eOS.Windows) Then
                bAddItem = CheckIgnoreTags(oApp.ImportTags)
            End If

            If bAddItem Then
                If oListViewItem.Checked Then bResetSelectAll = True
                lstGames.Items.Add(oListViewItem)
            End If
        Next

        'Change the status of the "Select All" checkbox depending on the status of the items filter results.  Set loading flag so we don't trigger any events
        bIsLoading = True
        If Not bResetSelectAll And bSelectAll Then
            bSelectAll = False
            chkSelectAll.Checked = False
        ElseIf bResetSelectAll And Not bSelectAll Then
            bSelectAll = True
            chkSelectAll.Checked = True
        End If
        bIsLoading = False

        lstGames.ListViewItemSorter = New ListViewItemComparer(iCurrentSort)
        lstGames.EndUpdate()
        UpdateSelected()

        If txtFilter.Text = String.Empty And Not chkSelectedOnly.Checked Then
            lblGames.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, lstGames.Items.Count)
        Else
            lblGames.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, lstGames.Items.Count) & " " & frmAdvancedImport_Filtered
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmAdvancedImport_FormName
        Me.Icon = GBM_Icon

        'Add configuration date to title if applicable
        If ImportInfo.Exported <> 0 Then
            Me.Text &= " [" & mgrCommon.UnixToDate(ImportInfo.Exported).Date & "]"
        End If

        'Set Form Text
        lblFilter.Text = frmAdvancedImport_lblFilter
        btnDetectSavedGames.Text = frmAdvancedImport_btnDetectSavedGames
        btnCancel.Text = frmAdvancedImport_btnCancel
        btnImport.Text = frmAdvancedImport_btnImport
        chkSelectAll.Text = frmAdvancedImport_chkSelectAll
        chkSelectedOnly.Text = frmAdvancedImport_chkSelectedOnly

        'Set Icons
        oImageList = New ImageList()
        oImageList.Images.Add(Icon_New)
        oImageList.Images.Add(Icon_Update)
        lstGames.SmallImageList = oImageList

        chkSelectAll.Checked = True

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub UpdateSelected()
        lblSelected.Text = mgrCommon.FormatString(frmAdvancedImport_Selected, FinalData.Count)
    End Sub

    Private Sub frmAdvancedImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bIsLoading = True
        SetForm()
        LoadData(String.Empty, False, True)
        bIsLoading = False
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If Not bIsLoading Then SelectToggle()
    End Sub

    Private Sub chkSelectedOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectedOnly.CheckedChanged
        LoadData(txtFilter.Text, chkSelectedOnly.Checked, False)
    End Sub

    Private Sub lstGames_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lstGames.ItemChecked
        SaveChecked(e.Item)
        If Not bIsLoading Then
            UpdateSelected()
        End If
    End Sub

    Private Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetectSavedGames.Click
        LoadData(txtFilter.Text, chkSelectedOnly.Checked, True)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If FinalData.Count > 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub lstGames_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lstGames.ColumnClick
        iCurrentSort = e.Column
        lstGames.ListViewItemSorter = New ListViewItemComparer(e.Column)
    End Sub

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        LoadData(txtFilter.Text, chkSelectedOnly.Checked)
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
    End Sub
End Class

' Column Sorter
Class ListViewItemComparer
    Implements IComparer

    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Return String.Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
    End Function
End Class