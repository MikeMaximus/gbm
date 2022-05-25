Imports GBM.My.Resources
Imports System.IO

Public Class frmAdvancedImport

    Private oImportData As ExportData
    Private hshImportData As Hashtable
    Private hshFinalData As New Hashtable
    Private hshIgnorePaths As Hashtable
    Private bClassicMode As Boolean = True
    Private bIsLoading As Boolean = False
    Private iCurrentSort As Integer = 0
    Private oImageList As ImageList
    Private WithEvents tmFilterTimer As Timer
    Private oLastWindowState As FormWindowState

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

    Public Property ClassicMode As Boolean
        Set(value As Boolean)
            bClassicMode = value
        End Set
        Get
            Return bClassicMode
        End Get
    End Property

    Private Sub SelectToggle()
        Cursor.Current = Cursors.WaitCursor
        bIsLoading = True
        lstGames.BeginUpdate()
        For i As Integer = 0 To lstGames.Items.Count - 1
            lstGames.Items(i).Checked = chkSelectAll.Checked
            SaveChecked(lstGames.Items(i))
        Next
        lstGames.EndUpdate()
        bIsLoading = False
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

    Private Sub LoadIgnorePaths()
        hshIgnorePaths = mgrPath.GetSpecialPaths()
        If Not mgrCommon.IsUnix Then
            hshIgnorePaths.Add("SavedGames", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Saved Games")
            hshIgnorePaths.Add("LocalLow", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "Low")
        End If
    End Sub

    Private Function IgnorePath(ByVal sPath As String) As Boolean
        If hshIgnorePaths.ContainsValue(sPath.TrimEnd(Path.DirectorySeparatorChar)) Then
            Return True
        End If

        Return False
    End Function

    Private Function AutoDetect() As Integer
        Dim oApp As clsGame
        Dim bAdd As Boolean
        Dim iCount As Integer

        Cursor.Current = Cursors.WaitCursor

        For Each de As DictionaryEntry In ImportData
            bAdd = False
            oApp = DirectCast(de.Value, clsGame)
            Try
                If mgrPath.IsPopulatedRegistryKey(oApp.Path) Then
                    bAdd = True
                ElseIf oApp.AbsolutePath And oApp.FolderSave Then
                    If Directory.Exists(oApp.Path) And Not IgnorePath(oApp.Path) Then
                        bAdd = True
                    End If
                ElseIf oApp.AbsolutePath And Not oApp.FolderSave Then
                    If Directory.Exists(oApp.Path) Then
                        For Each s As String In oApp.FileType.Split(":")
                            'For performance reasons we are not using a recursive search.
                            If Directory.GetFiles(oApp.Path, s, SearchOption.TopDirectoryOnly).Length > 0 Then
                                bAdd = True
                                Exit For
                            End If
                        Next
                    End If
                End If

                If bAdd Then
                    If Not FinalData.ContainsKey(oApp.ID) Then
                        iCount += 1
                        FinalData.Add(oApp.ID, oApp)
                    End If
                End If
            Catch
                'It should never happen at this point, but if the auto detect fails due to bad data we just ignore it and move on.
            End Try
        Next

        Cursor.Current = Cursors.Default

        Return iCount
    End Function

    Private Sub LoadData(Optional ByVal sFilter As String = "", Optional ByVal bSelectedOnly As Boolean = False)
        Dim oApp As clsGame
        Dim oListViewItem As ListViewItem
        Dim sTags As String
        Dim bAddItem As Boolean

        Cursor.Current = Cursors.WaitCursor
        bIsLoading = True
        lstGames.BeginUpdate()

        lstGames.Items.Clear()
        lstGames.ListViewItemSorter = Nothing

        If bSelectedOnly Then
            chkSelectedOnly.Checked = True
        End If

        For Each de As DictionaryEntry In ImportData
            bAddItem = False
            oApp = DirectCast(de.Value, clsGame)

            sTags = String.Empty
            oApp.ImportTags.Sort(AddressOf mgrCommon.CompareImportTagsByName)
            For Each oTag As Tag In oApp.ImportTags
                sTags &= oTag.Name & ", "
            Next
            sTags = sTags.TrimEnd(New Char() {",", " "})

            If bClassicMode Then
                oListViewItem = New ListViewItem(New String() {oApp.Name, oApp.ProcessName, oApp.Path, oApp.FileType, sTags})
            Else
                oListViewItem = New ListViewItem(New String() {oApp.Name, oApp.Path, oApp.FileType, oApp.OS.ToString, sTags})
            End If

            oListViewItem.Tag = oApp.ID

            If FinalData.ContainsKey(oApp.ID) Then
                oListViewItem.Checked = True
            Else
                oListViewItem.Checked = False
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

            If chkSelectedOnly.Checked Then
                If Not oListViewItem.Checked Then
                    bAddItem = False
                End If
            End If

            'Check for hardcoded ignore tags
            If bAddItem And (mgrCommon.IsUnix And oApp.OS = clsGame.eOS.Windows) Then
                bAddItem = CheckIgnoreTags(oApp.ImportTags)
            End If

            If bAddItem Then
                lstGames.Items.Add(oListViewItem)
                SaveChecked(oListViewItem)
            End If
        Next

        lstGames.ListViewItemSorter = New ListViewItemComparer(iCurrentSort)
        lstGames.EndUpdate()
        bIsLoading = False

        UpdateSelected()
        UpdateSelectToggle()
        UpdateTotals()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub AutoSizeColumns()
        If lstGames.Columns.Count > 0 Then
            If bClassicMode Then
                lstGames.Columns(0).Width = Math.Round(lstGames.Size.Width * 0.3)
                lstGames.Columns(1).Width = Math.Round(lstGames.Size.Width * 0.15)
                lstGames.Columns(2).Width = Math.Round(lstGames.Size.Width * 0.29)
                lstGames.Columns(3).Width = Math.Round(lstGames.Size.Width * 0.13)
                lstGames.Columns(4).Width = Math.Round(lstGames.Size.Width * 0.09)
            Else
                lstGames.Columns(0).Width = Math.Round(lstGames.Size.Width * 0.36)
                lstGames.Columns(1).Width = Math.Round(lstGames.Size.Width * 0.29)
                lstGames.Columns(2).Width = Math.Round(lstGames.Size.Width * 0.13)
                lstGames.Columns(3).Width = Math.Round(lstGames.Size.Width * 0.09)
                lstGames.Columns(4).Width = Math.Round(lstGames.Size.Width * 0.09)
            End If
        End If
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
        btnDetectSavedGames.Image = Multi_Search
        btnCancel.Text = frmAdvancedImport_btnCancel
        btnCancel.Image = Multi_Cancel
        btnImport.Text = frmAdvancedImport_btnImport
        btnImport.Image = Multi_Import
        chkSelectAll.Text = frmAdvancedImport_chkSelectAll
        chkSelectedOnly.Text = frmAdvancedImport_chkSelectedOnly

        'Set Icons
        oImageList = New ImageList()
        oImageList.Images.Add(frmAdvancedImport_New)
        oImageList.Images.Add(frmAdvancedImport_Update)
        lstGames.SmallImageList = oImageList

        'Setup Columns
        If bClassicMode Then
            lstGames.Columns.Add(frmAdvancedImport_ColumnName)
            lstGames.Columns.Add(frmAdvancedImport_ColumnProcess)
            lstGames.Columns.Add(frmAdvancedImport_ColumnPath)
            lstGames.Columns.Add(frmAdvancedImport_ColumnInclude)
            lstGames.Columns.Add(frmAdvancedImport_ColumnTags)
        Else
            lstGames.Columns.Add(frmAdvancedImport_ColumnName)
            lstGames.Columns.Add(frmAdvancedImport_ColumnPath)
            lstGames.Columns.Add(frmAdvancedImport_ColumnInclude)
            lstGames.Columns.Add(frmAdvancedImport_ColumnOs)
            lstGames.Columns.Add(frmAdvancedImport_ColumnTags)
        End If
        AutoSizeColumns()

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub UpdateSelected()
        lblSelected.Text = mgrCommon.FormatString(frmAdvancedImport_Selected, FinalData.Count)
    End Sub

    Private Sub UpdateSelectToggle()
        bIsLoading = True
        If lstGames.CheckedItems.Count = lstGames.Items.Count Then
            chkSelectAll.Checked = True
        Else
            chkSelectAll.Checked = False
        End If
        bIsLoading = False
    End Sub

    Private Sub UpdateTotals()
        If txtFilter.Text = String.Empty And Not chkSelectedOnly.Checked Then
            lblGames.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count})
        Else
            lblGames.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count}) & " " & frmAdvancedImport_Filtered
        End If
    End Sub

    Private Sub frmAdvancedImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadIgnorePaths()
        LoadData(String.Empty, AutoDetect() > 0)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If Not bIsLoading Then SelectToggle()
    End Sub

    Private Sub chkSelectedOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectedOnly.CheckedChanged
        If Not bIsLoading Then LoadData(txtFilter.Text)
    End Sub

    Private Sub lstGames_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lstGames.ItemChecked
        If Not bIsLoading Then
            SaveChecked(e.Item)
            UpdateSelected()
            UpdateSelectToggle()
        End If
    End Sub

    Private Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetectSavedGames.Click
        If Not bIsLoading Then
            If AutoDetect() > 0 Then
                LoadData(txtFilter.Text, True)
            End If
        End If
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

    Private Sub frmAdvancedImport_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd
        AutoSizeColumns()
    End Sub

    Private Sub frmAdvancedImport_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        'We need this work-around because .NET doesn't fire the ResizeEnd event when using the "Maximize" or "Restore" buttons.
        If Not oLastWindowState = Me.WindowState Then
            oLastWindowState = Me.WindowState
            AutoSizeColumns()
        End If
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