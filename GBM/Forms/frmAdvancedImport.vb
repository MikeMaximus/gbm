Imports GBM.My.Resources
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
    Private bDataLoaded As Boolean
    Private oLudusavi As mgrLudusavi
    Private oOfficialXML As mgrXML
    Private oListCache As New List(Of ListViewItem)

    Private WithEvents tmFilterTimer As Timer

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

        For Each de As DictionaryEntry In hshImportData
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
                            If Directory.GetDirectories(oApp.Path, s, SearchOption.TopDirectoryOnly).Length > 0 Or Directory.GetFiles(oApp.Path, s, SearchOption.TopDirectoryOnly).Length > 0 Then
                                bAdd = True
                                Exit For
                            End If
                        Next
                    End If
                End If

                If bAdd Then
                    If Not hshFinalData.ContainsKey(oApp.ID) Then
                        iCount += 1
                        hshFinalData.Add(oApp.ID, oApp)
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
            bDataLoaded = True
        Else
            bDataLoaded = False
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
                    Return False
                End If
                hshCompareFrom = oOfficialXML.ConvertedList
                oImportInfo = oOfficialXML.ImportInfo
                bOfficialMode = True
            Case mgrMonitorList.eImportTypes.Ludusavi
                oLudusavi = New mgrLudusavi(ImportPath, LudusaviOptions)
                If Not oLudusavi.ReadLudusaviManifest() Then
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
                        DirectCast(hshImportData(oFromItem.ID), clsGame).ImportUpdate = True
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

    Private Sub FillGrid(Optional ByVal bSelectedOnly As Boolean = False)
        Dim oApp As clsGame
        Dim oListViewItem As ListViewItem
        Dim sProcess As String
        Dim sTags As String
        Dim bAddItem As Boolean

        IsLoading = True

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

            If oApp.ImportUpdate Then
                oListViewItem.ImageIndex = 1
                oListViewItem.Checked = True
            Else
                oListViewItem.ImageIndex = 0
            End If

            If txtFilter.Text = String.Empty Then
                bAddItem = True
            Else
                If oApp.Name.ToLower.Contains(txtFilter.Text.ToLower) Or oApp.ProcessName.ToLower.Contains(txtFilter.Text.ToLower) Or sTags.ToLower.Contains(txtFilter.Text.ToLower) Then
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
                oListCache.Add(oListViewItem)
                SaveChecked(oListViewItem)
            End If
        Next

        oListCache.Sort(New ListViewItemComparer(iCurrentSort))
        lstGames.VirtualListSize = oListCache.Count
        If oListCache.Count > 0 Then lstGames.EnsureVisible(0)

        IsLoading = False

        UpdateSelectToggle()
        UpdateTotals()
    End Sub

    Private Sub ImportConfigurations()
        mgrSync.DoListAddUpdateSync(hshFinalData)
        mgrTags.DoTagAddImport(hshFinalData)
        mgrConfigLinks.DoConfigLinkImport(hshFinalData)
        mgrSync.SyncMonitorLists()
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
            lblFilter.Enabled = True
            txtFilter.Enabled = True
            btnClearSelected.Enabled = True
            btnDetectSavedGames.Enabled = True
            btnImport.Enabled = True
            btnCancel.Enabled = True
            lstGames.Enabled = True
            lstGames.Focus()
        Else
            Me.UseWaitCursor = True
            chkSelectAll.Enabled = False
            chkSelectedOnly.Enabled = False
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

        'Set Icons
        oImageList = New ImageList()
        oImageList.Images.Add(frmAdvancedImport_New)
        oImageList.Images.Add(frmAdvancedImport_Update)
        lstGames.SmallImageList = oImageList

        'Set ListView
        lstGames.VirtualMode = True
        lstGames.OwnerDraw = True

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub UpdateTotals()
        If txtFilter.Text = String.Empty And Not chkSelectedOnly.Checked Then
            lblStatus.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count})
        Else
            lblStatus.Text = mgrCommon.FormatString(frmAdvancedImport_Configs, New String() {lstGames.Items.Count, hshImportData.Count}) & " " & frmAdvancedImport_Filtered
        End If
    End Sub

    Private Sub frmAdvancedImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadIgnorePaths()
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
        If Not IsLoading Then
            SaveAllChecked()
            FillGrid()
        End If
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

    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        SaveAllChecked()
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

        If bDataLoaded Then
            SetColumns()
            SetFormTitle()

            If bOfficialMode Then
                bSelectedOnly = AutoDetect() > 0
            End If

            FillGrid(bSelectedOnly)
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
            FillGrid(True)
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

    Private Sub lstGames_MouseClick(sender As Object, e As MouseEventArgs) Handles lstGames.MouseClick
        Dim oListView As ListView = DirectCast(sender, ListView)
        Dim oListViewItem As ListViewItem = oListView.GetItemAt(e.X, e.Y)
        If Not oListViewItem Is Nothing Then
            If e.X < (oListViewItem.Bounds.Left + 16) Then
                oListViewItem.Checked = Not oListViewItem.Checked
                oListView.Invalidate(oListViewItem.Bounds)
            End If
        End If
    End Sub

    Private Sub lstGames_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstGames.MouseDoubleClick
        Dim oListView As ListView = DirectCast(sender, ListView)
        Dim oListViewItem As ListViewItem = oListView.GetItemAt(e.X, e.Y)
        If Not oListViewItem Is Nothing Then
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
        e.DrawDefault = True
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