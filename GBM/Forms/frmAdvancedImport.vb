﻿Imports GBM.My.Resources
Imports System.IO

Public Class frmAdvancedImport

    Private sImportPath As String
    Private eImportType As mgrMonitorList.eImportTypes
    Private oLudusaviOptions As clsLudusaviOptions
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
    Private iGamesDetected As Integer
    Private bDataLoaded As Boolean

    Public Property ImportPath As String
        Get
            Return sImportPath
        End Get
        Set(value As String)
            sImportPath = value
        End Set
    End Property

    Public Property ImportType As mgrMonitorList.eImportTypes
        Get
            Return eImportType
        End Get
        Set(value As mgrMonitorList.eImportTypes)
            eImportType = value
        End Set
    End Property

    Public Property LudusaviOptions As clsLudusaviOptions
        Get
            Return oLudusaviOptions
        End Get
        Set(value As clsLudusaviOptions)
            oLudusaviOptions = value
        End Set
    End Property

    Private Property ImportInfo As ExportData
        Set(value As ExportData)
            oImportData = value
        End Set
        Get
            Return oImportData
        End Get
    End Property

    Private Property ImportData As Hashtable
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

    Private Property ClassicMode As Boolean
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

        bIsLoading = True
        ImportInfo = New ExportData

        Select Case Path.GetExtension(ImportPath)
            Case ".xml"
                If Not mgrXML.DeserializeAndImport(ImportPath, ImportInfo, hshCompareFrom) Then
                    Return False
                End If
                ClassicMode = True
            Case ".yaml", ".yml"
                If Not mgrLudusavi.ReadLudusaviManifest(ImportPath, LudusaviOptions, ImportInfo, hshCompareFrom) Then
                    Return False
                End If
                ClassicMode = False
            Case Else
                mgrCommon.ShowMessage(mgrMonitorList_ErrorImportFileType, MsgBoxStyle.Exclamation)
                Return False
        End Select

        hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)

        ImportData = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.ID) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.ID), clsGame)
                If oFromItem.MinimalEquals(oToItem) Then
                    If oFromItem.CoreEquals(oToItem) Then
                        ImportData.Remove(oFromItem.ID)
                    Else
                        DirectCast(ImportData(oFromItem.ID), clsGame).ImportUpdate = True
                        'These fields need to be set via the object or they will be lost when the configuration is updated
                        DirectCast(ImportData(oFromItem.ID), clsGame).Hours = oToItem.Hours
                        DirectCast(ImportData(oFromItem.ID), clsGame).CleanFolder = oToItem.CleanFolder
                    End If

                End If
            End If
        Next

        bIsLoading = False

        If ImportData.Count > 0 Then
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

            If oApp.Parameter <> String.Empty Then
                sProcess = oApp.ProcessName & " (" & oApp.Parameter & ")"
            Else
                sProcess = oApp.ProcessName
            End If

            If bClassicMode Then
                oListViewItem = New ListViewItem(New String() {oApp.Name, sProcess, oApp.Path, oApp.FileType, sTags})
            Else
                oListViewItem = New ListViewItem(New String() {oApp.Name, sProcess, oApp.Path, oApp.FileType, oApp.OS.ToString, sTags})
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
            lstGames.BeginUpdate()
            If bClassicMode Then
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
        If ImportInfo.Exported <> 0 Then
            Me.Text &= " [" & mgrCommon.UnixToDate(ImportInfo.Exported).Date & "]"
        End If
    End Sub

    Private Sub SetColumns()
        'Setup Columns
        If bClassicMode Then
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
            lblSelected.Visible = True
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
            lblSelected.Visible = False
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
        If bIsLoading Then e.Cancel = True
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If Not bIsLoading Then SelectToggle()
    End Sub

    Private Sub chkSelectedOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectedOnly.CheckedChanged
        If Not bIsLoading Then FillGrid()
    End Sub

    Private Sub lstGames_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lstGames.ItemChecked
        If Not bIsLoading Then
            SaveChecked(e.Item)
            UpdateSelected()
            UpdateSelectToggle()
        End If
    End Sub

    Private Sub btnClearSelected_Click(sender As Object, e As EventArgs) Handles btnClearSelected.Click
        txtFilter.Text = String.Empty
    End Sub

    Private Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetectSavedGames.Click
        If Not bIsLoading Then
            lblStatus.Text = frmAdvancedImport_DetectingSavedGames
            ToggleForm(False)
            bwDetect.RunWorkerAsync()
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

            If bClassicMode Then
                bSelectedOnly = AutoDetect() > 0
            End If

            FillGrid(bSelectedOnly)
            ToggleForm(True)
        Else
            mgrCommon.ShowMessage(mgrMonitorList_ImportNothing, MsgBoxStyle.Information)
            Me.Close()
        End If

    End Sub

    Private Sub bwDetect_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwDetect.DoWork
        iGamesDetected = AutoDetect()
    End Sub

    Private Sub bwDetect_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDetect.RunWorkerCompleted
        If iGamesDetected > 0 Then
            FillGrid(True)
        Else
            mgrCommon.ShowMessage(frmAdvancedImport_WarningNoSavesDetected, MsgBoxStyle.Information)
        End If
        ToggleForm(True)
        UpdateTotals()
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