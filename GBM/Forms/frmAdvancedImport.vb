﻿Imports GBM.My.Resources
Imports System.IO

Public Class frmAdvancedImport

    Private oImportInfo As ExportData
    Private hshImportData As Hashtable
    Private hshFinalData As New Hashtable
    Private hshIgnorePaths As Hashtable
    Private bOfficialMode As Boolean = True
    Private iCurrentSort As Integer = 0
    Private oImageList As ImageList
    Private oLastWindowState As FormWindowState
    Private iGamesDetected As Integer
    Private oLudusavi As mgrLudusavi
    Private oOfficialXML As mgrXML
    Private oListCache As New List(Of ListViewItem)
    Private oAutoDetected As New List(Of String)
    Private oUpdated As New List(Of String)
    Private oIgnored As List(Of String)
    Private iCurrentIgnored As Integer
    Private iLastSelection As Integer

    Private WithEvents tmFilterTimer As Timer

    Private Property DataLoaded As Boolean
    Private Property IsLoading As Boolean = False
    Public Property ImportPath As String
    Public Property ImportType As mgrMonitorList.eImportTypes
    Public Property LudusaviOptions As clsLudusaviOptions

    Private Sub SelectToggle()
        IsLoading = True

        For i As Integer = 0 To lstGames.Items.Count - 1
            oListCache(i).Checked = chkSelectAll.Checked
            SaveChecked(oListCache(i))
        Next
        lstGames.Refresh()

        IsLoading = False
    End Sub

    Private Sub UpdateSelectToggle()
        Dim bSelectAll As Boolean = True

        IsLoading = True

        For Each oListViewItem As ListViewItem In oListCache
            If Not oListViewItem.Checked Then
                bSelectAll = False
                Exit For
            End If
        Next
        chkSelectAll.Checked = bSelectAll

        IsLoading = False
    End Sub

    Private Sub SaveChecked(ByVal oItem As ListViewItem)
        If oItem.Checked Then
            If Not hshFinalData.ContainsKey(oItem.Tag) Then hshFinalData.Add(oItem.Tag, hshImportData(oItem.Tag))
        Else
            hshFinalData.Remove(oItem.Tag)
        End If
    End Sub

    Private Sub SaveAllChecked()
        For Each oListViewItem As ListViewItem In oListCache
            SaveChecked(oListViewItem)
        Next
    End Sub

    Private Sub HandleIgnoreLabel()
        Dim iIgnored As Integer

        If lstGames.SelectedIndices.Count > 0 Then
            For Each i As Integer In lstGames.SelectedIndices
                If oIgnored.Contains(oListCache(i).Tag) Then
                    iIgnored += 1
                End If
            Next

            If iIgnored = 0 Then
                cmiIgnore.Text = frmAdvancedImport_cmiIgnore
            ElseIf iIgnored = lstGames.SelectedIndices.Count Then
                cmiIgnore.Text = frmAdvancedImport_cmiIgnore_Reverse
            Else
                cmiIgnore.Text = frmAdvancedImport_cmiIgnore_Toggle
            End If
        End If
    End Sub

    Private Sub ToggleIgnore()
        Dim sID As String
        Dim oAdded As New List(Of String)
        Dim oRemoved As New List(Of String)

        If lstGames.SelectedIndices.Count > 0 Then
            For Each i As Integer In lstGames.SelectedIndices
                sID = oListCache(i).Tag
                If oIgnored.Contains(sID) Then
                    oRemoved.Add(sID)
                    oIgnored.Remove(sID)
                Else
                    oAdded.Add(sID)
                    oIgnored.Add(sID)
                End If
            Next
            If oAdded.Count > 0 Then mgrImportIgnore.DoIgnoreListAddBatch(oAdded)
            If oRemoved.Count > 0 Then mgrImportIgnore.DoIgnoreListRemoveBatch(oRemoved)
            FillGrid()
            lstGames.SelectedIndices.Clear()
            lstGames.Refresh()
        End If
    End Sub

    Private Function CheckIgnoreID(ByVal sID As String) As Boolean
        If oIgnored.Contains(sID) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadIgnores()
        hshIgnorePaths = mgrPath.GetSpecialPaths()
        If Not mgrCommon.IsUnix Then
            hshIgnorePaths.Add("SavedGames", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Saved Games")
            hshIgnorePaths.Add("LocalLow", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "Low")
        End If
        oIgnored = mgrImportIgnore.DoGetIgnoreList()
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

        oAutoDetected.Clear()

        For Each de As DictionaryEntry In hshImportData
            bAdd = False
            oApp = DirectCast(de.Value, clsGame)
            Try
                If Not oIgnored.Contains(oApp.ID) Then
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
                                If Directory.GetDirectories(oApp.Path, s, SearchOption.TopDirectoryOnly).Length > 0 Or Directory.GetFiles(oApp.Path, s, SearchOption.TopDirectoryOnly).Length > 0 Then
                                    bAdd = True
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If bAdd Then
                        oAutoDetected.Add(oApp.ID)
                        If Not hshFinalData.ContainsKey(oApp.ID) Then
                            iCount += 1
                            hshFinalData.Add(oApp.ID, oApp)
                        End If
                    End If
                End If
            Catch
                'It should never happen at this point, but if the auto detect fails due to bad data we just ignore it and move on.
            End Try
        Next

        Return iCount
    End Function

    Private Sub LoadData()
        If LoadFromLocation() Then
            DataLoaded = True
        Else
            DataLoaded = False
        End If
    End Sub

    Private Function LoadFromLocation() As Boolean
        Dim hshCompareFrom As New Hashtable
        Dim hshCompareTo As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame

        IsLoading = True
        oImportInfo = New ExportData

        Select Case ImportType
            Case mgrMonitorList.eImportTypes.Official
                oOfficialXML = New mgrXML(ImportPath)
                If Not oOfficialXML.DeserializeAndImport() Then
                    IsLoading = False
                    Return False
                End If
                hshCompareFrom = oOfficialXML.ConvertedList
                oImportInfo = oOfficialXML.ImportInfo
                bOfficialMode = True
            Case mgrMonitorList.eImportTypes.Ludusavi
                oLudusavi = New mgrLudusavi(ImportPath, LudusaviOptions)
                If Not oLudusavi.ReadLudusaviManifest() Then
                    IsLoading = False
                    Return False
                End If
                hshCompareFrom = oLudusavi.ConvertedList
                oImportInfo = oLudusavi.ImportInfo
                bOfficialMode = False
        End Select

        hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)

        hshImportData = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.ID) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.ID), clsGame)
                If oFromItem.MinimalEquals(oToItem) Then
                    If oFromItem.CoreEquals(oToItem) Then
                        hshImportData.Remove(oFromItem.ID)
                    Else
                        oUpdated.Add(oFromItem.ID)
                        'These fields need to be set via the object or they will be lost when the configuration is updated
                        DirectCast(hshImportData(oFromItem.ID), clsGame).Hours = oToItem.Hours
                        DirectCast(hshImportData(oFromItem.ID), clsGame).CleanFolder = oToItem.CleanFolder
                    End If
                End If
            End If
        Next

        IsLoading = False

        If hshImportData.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub AssignImage(ByRef oListItem As ListViewItem)
        If oIgnored.Contains(oListItem.Tag) Then
            oListItem.ImageIndex = 2
        ElseIf oUpdated.Contains(oListItem.Tag) Then
            oListItem.ImageIndex = 1
        ElseIf oAutoDetected.Contains(oListItem.Tag) Then
            oListItem.ImageIndex = 3
        Else
            oListItem.ImageIndex = 0
        End If
    End Sub

    Private Sub FillGrid(Optional ByVal bSelectedOnly As Boolean = False, Optional ByVal bSaveChecked As Boolean = True)
        Dim oApp As clsGame
        Dim oListViewItem As ListViewItem
        Dim sProcess As String
        Dim sTags As String
        Dim bAddItem As Boolean

        IsLoading = True

        If bSaveChecked Then
            SaveAllChecked()
        End If

        iCurrentIgnored = 0
        oListCache.Clear()

        If bSelectedOnly Then
            chkSelectedOnly.Checked = True
        End If

        For Each de As DictionaryEntry In hshImportData
            bAddItem = False
            oApp = DirectCast(de.Value, clsGame)

            sTags = String.Empty
            oApp.ImportTags.Sort(AddressOf mgrCommon.CompareImportTagsByName)
            For Each oTag As Tag In oApp.ImportTags
                sTags &= oTag.Name & ", "
            Next
            sTags = sTags.TrimEnd(New Char() {",", " "})

            If oApp.Parameter <> String.Empty Then
                sProcess = oApp.ProcessName & " (" & oApp.Parameter & ")"
            Else
                sProcess = oApp.ProcessName
            End If

            If bOfficialMode Then
                oListViewItem = New ListViewItem(New String() {oApp.Name, sProcess, oApp.Path, oApp.FileType, sTags})
            Else
                oListViewItem = New ListViewItem(New String() {oApp.Name, sProcess, oApp.Path, oApp.FileType, oApp.OS.ToString, sTags})
            End If

            oListViewItem.Tag = oApp.ID

            If hshFinalData.ContainsKey(oApp.ID) Then
                oListViewItem.Checked = True
            Else
                oListViewItem.Checked = False
            End If

            If oUpdated.Contains(oApp.ID) Then
                oListViewItem.Checked = True
            End If

            If txtFilter.Text = String.Empty Then
                bAddItem = True
            Else
                If txtFilter.Text.StartsWith("#") Then
                    If sTags.ToLower.Contains(txtFilter.Text.Remove(0, 1).ToLower) Then
                        bAddItem = True
                    End If
                ElseIf oApp.Name.ToLower.Contains(txtFilter.Text.ToLower) Or oApp.ProcessName.ToLower.Contains(txtFilter.Text.ToLower) Then
                    bAddItem = True
                End If
            End If

            If CheckIgnoreID(oApp.ID) Then
                iCurrentIgnored += 1
                oListViewItem.Checked = False
                If chkHideIgnored.Checked Then
                    bAddItem = False
                End If
            End If

            If chkSelectedOnly.Checked Then
                If Not oListViewItem.Checked Then
                    bAddItem = False
                End If
            End If

            If bAddItem Then
                AssignImage(oListViewItem)
                oListCache.Add(oListViewItem)
                SaveChecked(oListViewItem)
            End If
        Next

        oListCache.Sort(New ListViewItemComparer(iCurrentSort))
        lstGames.VirtualListSize = oListCache.Count
        If oListCache.Count > iLastSelection Then
            lstGames.EnsureVisible(iLastSelection)
        ElseIf oListCache.Count > 0 Then
            lstGames.EnsureVisible(0)
        End If

        IsLoading = False

        UpdateSelectToggle()
        UpdateTotals()
    End Sub

    Private Sub ImportConfigurations()
        mgrSync.DoListAddUpdateSync(hshFinalData)
        mgrTags.DoTagAddImport(hshFinalData)
        mgrConfigLinks.DoConfigLinkImport(hshFinalData)
        mgrSync.SyncData()
    End Sub

    Private Sub AutoSizeColumns()
        If lstGames.Columns.Count > 0 Then
            lstGames.BeginUpdate()
            If bOfficialMode Then
                lstGames.Columns(0).Width = Math.Round(lstGames.Size.Width * 0.3)
                lstGames.Columns(1).Width = Math.Round(lstGames.Size.Width * 0.15)
                lstGames.Columns(2).Width = Math.Round(lstGames.Size.Width * 0.29)
                lstGames.Columns(3).Width = Math.Round(lstGames.Size.Width * 0.13)
                lstGames.Columns(4).Width = Math.Round(lstGames.Size.Width * 0.09)
            Else
                lstGames.Columns(0).Width = Math.Round(lstGames.Size.Width * 0.3)
                lstGames.Columns(1).Width = Math.Round(lstGames.Size.Width * 0.15)
                lstGames.Columns(2).Width = Math.Round(lstGames.Size.Width * 0.2)
                lstGames.Columns(3).Width = Math.Round(lstGames.Size.Width * 0.13)
                lstGames.Columns(4).Width = Math.Round(lstGames.Size.Width * 0.09)
                lstGames.Columns(5).Width = Math.Round(lstGames.Size.Width * 0.09)
            End If
            lstGames.EndUpdate()
        End If
    End Sub

    Private Sub SetFormTitle()
        'Add configuration date to title if applicable
        If oImportInfo.Exported <> 0 Then
            Me.Text &= " [" & mgrCommon.UnixToDate(oImportInfo.Exported).Date & "]"
        End If
    End Sub

    Private Sub SetColumns()
        'Setup Columns
        If bOfficialMode Then
            lstGames.Columns.Add(frmAdvancedImport_ColumnName)
            lstGames.Columns.Add(frmAdvancedImport_ColumnProcess)
            lstGames.Columns.Add(frmAdvancedImport_ColumnPath)
            lstGames.Columns.Add(frmAdvancedImport_ColumnInclude)
            lstGames.Columns.Add(frmAdvancedImport_ColumnTags)
        Else
            lstGames.Columns.Add(frmAdvancedImport_ColumnName)
            lstGames.Columns.Add(frmAdvancedImport_ColumnProcess)
            lstGames.Columns.Add(frmAdvancedImport_ColumnPath)
            lstGames.Columns.Add(frmAdvancedImport_ColumnInclude)
            lstGames.Columns.Add(frmAdvancedImport_ColumnOs)
            lstGames.Columns.Add(frmAdvancedImport_ColumnTags)
        End If
        AutoSizeColumns()
    End Sub

    Private Sub ToggleForm(ByVal bOn As Boolean)
        If bOn Then
            Me.UseWaitCursor = False
            chkSelectAll.Enabled = True
            chkSelectedOnly.Enabled = True
            chkHideIgnored.Enabled = True
            lblFilter.Enabled = True
            txtFilter.Enabled = True
            btnClearSelected.Enabled = True
            btnDetectSavedGames.Enabled = True
            btnImport.Enabled = True
            btnCancel.Enabled = True
            lstGames.Enabled = True
            txtFilter.Focus()
        Else
            Me.UseWaitCursor = True
            chkSelectAll.Enabled = False
            chkSelectedOnly.Enabled = False
            chkHideIgnored.Enabled = False
            lblFilter.Enabled = False
            txtFilter.Enabled = False
            btnClearSelected.Enabled = False
            btnDetectSavedGames.Enabled = False
            btnImport.Enabled = False
            btnCancel.Enabled = False
            lstGames.Enabled = False
        End If
    End Sub

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = frmAdvancedImport_FormName
        Me.Icon = GBM_Icon

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
        chkHideIgnored.Text = frmAdvancedImport_chkHideIgnored
        cmiIgnore.Text = frmAdvancedImport_cmiIgnore

        'Set Icons
        oImageList = New ImageList()
        oImageList.Images.Add(frmAdvancedImport_New)
        oImageList.Images.Add(frmAdvancedImport_Update)
        oImageList.Images.Add(frmAdvancedImport_Ignored)
        oImageList.Images.Add(frmAdvancedImport_Detected)
        lstGames.SmallImageList = oImageList

        'Set ListView
        lstGames.VirtualMode = True
        lstGames.OwnerDraw = True

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False

        'Defaults
        chkHideIgnored.Checked = True
    End Sub

    Private Sub UpdateTotals()
        If txtFilter.Text = String.Empty And Not chkSelectedOnly.Checked Then
            lblStatus.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count})
        Else
            lblStatus.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count}) & " " & frmAdvancedImport_Filtered
        End If

        If chkHideIgnored.Checked Then
            lblStatus.Text &= " " & mgrCommon.FormatString(frmAdvancedImport_IgnoredCount, iCurrentIgnored.ToString)
        End If
    End Sub

    Private Sub frmAdvancedImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadIgnores()
    End Sub

    Private Sub frmAdvancedImport_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ToggleForm(False)
        lblStatus.Text = frmAdvancedImport_Processing
        bwLoader.RunWorkerAsync()
    End Sub

    Private Sub frmAdvancedImport_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsLoading Then e.Cancel = True
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If Not IsLoading Then SelectToggle()
    End Sub

    Private Sub chkSelectedOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectedOnly.CheckedChanged
        If Not IsLoading Then FillGrid()
    End Sub

    Private Sub chkHideIgnored_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideIgnored.CheckedChanged
        If DataLoaded And Not IsLoading Then FillGrid()
    End Sub

    Private Sub btnClearSelected_Click(sender As Object, e As EventArgs) Handles btnClearSelected.Click
        txtFilter.Text = String.Empty
    End Sub

    Private Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetectSavedGames.Click
        If Not IsLoading Then
            lblStatus.Text = frmAdvancedImport_DetectingSavedGames
            ToggleForm(False)
            SaveAllChecked()
            bwDetect.RunWorkerAsync()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If Not IsLoading Then
            SaveAllChecked()
            If hshFinalData.Count > 0 Then
                lblStatus.Text = frmAdvancedImport_Importing
                ToggleForm(False)
                bwImport.RunWorkerAsync()
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub lstGames_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lstGames.ColumnClick
        iCurrentSort = e.Column
        oListCache.Sort(New ListViewItemComparer(e.Column))
        lstGames.Refresh()
    End Sub

    Private Sub cmsIgnore_Click(sender As Object, e As EventArgs) Handles cmiIgnore.Click
        ToggleIgnore()
    End Sub

    Private Sub cmsOptions_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmsOptions.Opening
        HandleIgnoreLabel()
    End Sub

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        FillGrid()
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

    Private Sub bwLoader_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwLoader.DoWork
        LoadData()
    End Sub

    Private Sub bwLoader_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoader.RunWorkerCompleted
        Dim bSelectedOnly As Boolean = False

        If DataLoaded Then
            SetColumns()
            SetFormTitle()

            If bOfficialMode Then
                bSelectedOnly = AutoDetect() > 0
            End If

            FillGrid(bSelectedOnly, False)
            ToggleForm(True)
        Else
            mgrCommon.ShowMessage(frmAdvancedImport_ImportNothing, MsgBoxStyle.Information)
            Me.Close()
        End If

    End Sub

    Private Sub bwDetect_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwDetect.DoWork
        iGamesDetected = AutoDetect()
    End Sub

    Private Sub bwDetect_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDetect.RunWorkerCompleted
        If iGamesDetected > 0 Then
            txtFilter.Text = String.Empty
            FillGrid(True, False)
        Else
            mgrCommon.ShowMessage(frmAdvancedImport_WarningNoSavesDetected, MsgBoxStyle.Information)
        End If
        ToggleForm(True)
        UpdateTotals()
    End Sub

    Private Sub bwImport_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwImport.DoWork
        ImportConfigurations()
    End Sub

    Private Sub bwImport_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwImport.RunWorkerCompleted
        mgrCommon.ShowMessage(frmAdvancedImport_ImportComplete, MsgBoxStyle.Information)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub lstGames_RetrieveVirtualItem(sender As Object, e As RetrieveVirtualItemEventArgs) Handles lstGames.RetrieveVirtualItem
        e.Item = oListCache(e.ItemIndex)
    End Sub

    Private Sub lstGames_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lstGames.ItemChecked
        'In Mono, the checkbox part of a ListViewItem object does not trigger mouse click events, so we need this to help determine last scroll position.
        If mgrCommon.IsUnix Then iLastSelection = e.Item.Index
    End Sub

    Private Sub lstGames_MouseClick(sender As Object, e As MouseEventArgs) Handles lstGames.MouseClick
        Dim oListView As ListView = DirectCast(sender, ListView)
        Dim oListViewItem As ListViewItem = oListView.GetItemAt(e.X, e.Y)
        If Not oListViewItem Is Nothing Then
            iLastSelection = oListViewItem.Index
            If e.X < (oListViewItem.Bounds.Left + 16) Then
                oListViewItem.Checked = Not oListViewItem.Checked
                oListView.Invalidate(oListViewItem.Bounds)
            Else
                If e.Button = MouseButtons.Right Then
                    cmsOptions.Show(Cursor.Position)
                End If
            End If
        End If
    End Sub

    Private Sub lstGames_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstGames.MouseDoubleClick
        Dim oListView As ListView = DirectCast(sender, ListView)
        Dim oListViewItem As ListViewItem = oListView.GetItemAt(e.X, e.Y)
        If Not oListViewItem Is Nothing Then
            iLastSelection = oListViewItem.Index
            oListView.Invalidate(oListViewItem.Bounds)
        End If
    End Sub

    Private Sub lstGames_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lstGames.DrawItem
        e.DrawDefault = True
        If Not mgrCommon.IsUnix Then
            If Not e.Item.Checked Then
                e.Item.Checked = True
                e.Item.Checked = False
            End If
        End If
    End Sub

    Private Sub lstGames_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lstGames.DrawSubItem
        e.DrawDefault = True
    End Sub

    Private Sub lstGames_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lstGames.DrawColumnHeader
        'Don't overwrite the dark mode draw
        If Not mgrDarkMode.UseDarkMode Then e.DrawDefault = True
    End Sub

    Private Sub frmAdvancedImport_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class

' Column Sorter
Class ListViewItemComparer
    Implements IComparer(Of ListViewItem)

    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As ListViewItem, ByVal y As ListViewItem) As Integer Implements IComparer(Of ListViewItem).Compare
        Return String.Compare(x.SubItems(col).Text, y.SubItems(col).Text)
    End Function
End Class