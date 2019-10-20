Imports GBM.My.Resources
Imports System.Collections.Specialized
Imports System.IO

Public Class frmGameManager

    Private oSettings As mgrSettings
    Private sBackupFolder As String
    Private bPendingRestores As Boolean = False
    Private oCurrentBackupItem As clsBackup
    Private oLastPlayedGame As clsGame
    Private oCurrentGame As clsGame
    Private oTagsToSave As New List(Of KeyValuePair(Of String, String))
    Private oProcessesToSave As New List(Of KeyValuePair(Of String, String))
    Private oConfigLinksToSave As New List(Of KeyValuePair(Of String, String))
    Private bDisableExternalFunctions As Boolean = False
    Private bTriggerBackup As Boolean = False
    Private bTriggerRestore As Boolean = False
    Private bTriggerImportBackup As Boolean = False
    Private bIgnoreConfigLinks As Boolean = False
    Private oBackupList As New List(Of clsGame)
    Private oRestoreList As New Hashtable
    Private oImportBackupList As New Hashtable
    Private oGameData As OrderedDictionary
    Private oLocalBackupData As SortedList
    Private oRemoteBackupData As SortedList
    Private bIsDirty As Boolean = False
    Private bIsLoading As Boolean = False
    Private oCurrentIncludeTagFilters As New List(Of clsTag)
    Private oCurrentExcludeTagFilters As New List(Of clsTag)
    Private oCurrentFilters As New List(Of clsGameFilter)
    Private eCurrentFilter As frmFilter.eFilterType = frmFilter.eFilterType.BaseFilter
    Private bCurrentAndOperator As Boolean = False
    Private bCurrentSortAsc As Boolean = True
    Private sCurrentSortField As String = "Name"
    Private WithEvents tmFilterTimer As Timer

    Private Enum eModes As Integer
        View = 1
        Edit = 2
        Add = 3
        Disabled = 4
        MultiSelect = 5
    End Enum

    Private eCurrentMode As eModes = eModes.Disabled

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Property PendingRestores As Boolean
        Get
            Return bPendingRestores
        End Get
        Set(value As Boolean)
            bPendingRestores = value
        End Set
    End Property

    Property CurrentBackupItem As clsBackup
        Get
            Return oCurrentBackupItem
        End Get
        Set(value As clsBackup)
            oCurrentBackupItem = value
        End Set
    End Property

    Property LastPlayedGame As clsGame
        Get
            Return oLastPlayedGame
        End Get
        Set(value As clsGame)
            oLastPlayedGame = value
        End Set
    End Property

    Property CurrentGame As clsGame
        Get
            Return oCurrentGame
        End Get
        Set(value As clsGame)
            oCurrentGame = value
        End Set
    End Property

    Private Property BackupFolder As String
        Get
            Return Settings.BackupFolder & Path.DirectorySeparatorChar
        End Get
        Set(value As String)
            sBackupFolder = value
        End Set
    End Property

    Private Property GameData As OrderedDictionary
        Get
            Return oGameData
        End Get
        Set(value As OrderedDictionary)
            oGameData = value
        End Set
    End Property

    Property DisableExternalFunctions As Boolean
        Get
            Return bDisableExternalFunctions
        End Get
        Set(value As Boolean)
            bDisableExternalFunctions = value
        End Set
    End Property

    Property TriggerBackup As Boolean
        Get
            Return bTriggerBackup
        End Get
        Set(value As Boolean)
            bTriggerBackup = value
        End Set
    End Property

    Property TriggerRestore As Boolean
        Get
            Return bTriggerRestore
        End Get
        Set(value As Boolean)
            bTriggerRestore = value
        End Set
    End Property

    Property TriggerImportBackup As Boolean
        Get
            Return bTriggerImportBackup
        End Get
        Set(value As Boolean)
            bTriggerImportBackup = value
        End Set
    End Property

    Property IgnoreConfigLinks As Boolean
        Get
            Return bIgnoreConfigLinks
        End Get
        Set(value As Boolean)
            bIgnoreConfigLinks = value
        End Set
    End Property

    Property BackupList As List(Of clsGame)
        Get
            Return oBackupList
        End Get
        Set(value As List(Of clsGame))
            oBackupList = value
        End Set
    End Property

    Property RestoreList As Hashtable
        Get
            Return oRestoreList
        End Get
        Set(value As Hashtable)
            oRestoreList = value
        End Set
    End Property

    Property ImportBackupList As Hashtable
        Get
            Return oImportBackupList
        End Get
        Set(value As Hashtable)
            oImportBackupList = value
        End Set
    End Property

    Private Property IsDirty As Boolean
        Get
            Return bIsDirty
        End Get
        Set(value As Boolean)
            bIsDirty = value
        End Set
    End Property

    Private Property IsLoading As Boolean
        Get
            Return bIsLoading
        End Get
        Set(value As Boolean)
            bIsLoading = value
        End Set
    End Property

    Private Sub LoadBackupData()
        oRemoteBackupData = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Remote)
        oLocalBackupData = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Local)
    End Sub

    Private Function HandleSavePath(ByVal sSavePath As String, ByVal sAppPath As String) As String
        Dim sPath As String = sSavePath

        If Not mgrPath.IsAbsolute(sSavePath) Then
            'Determine a relative path if possible
            If sAppPath <> String.Empty And sSavePath <> String.Empty Then
                sPath = mgrPath.DetermineRelativePath(sAppPath, sSavePath)
            End If
        Else
            If Not oSettings.ShowResolvedPaths Then
                sPath = mgrPath.ReverseSpecialPaths(sPath)
            End If
        End If

        Return sPath
    End Function

    Private Sub HandleWineConfig()
        If mgrCommon.IsUnix And cboOS.SelectedValue = clsGame.eOS.Windows And Not eCurrentMode = eModes.Add Then
            btnWineConfig.Visible = True
        Else
            btnWineConfig.Visible = False
        End If
    End Sub

    Private Function CheckManifestandUpdate(ByVal oOriginalApp As clsGame, ByVal oNewApp As clsGame, ByVal bUseGameID As Boolean) As Boolean
        Dim oBackupItems As List(Of clsBackup)
        Dim sDirectory As String
        Dim sNewDirectory As String
        Dim sFileName As String
        Dim sNewFileName As String
        Dim sNewAppItem As String
        Dim sOriginalAppItem As String

        'If there is a valid change, check and update the manifest
        If (oNewApp.ID <> oOriginalApp.ID) Or (oNewApp.FileSafeName <> oOriginalApp.FileSafeName) Then
            'Choose how to perform file & folder renames
            If bUseGameID Then
                sNewAppItem = oNewApp.ID
                sOriginalAppItem = oOriginalApp.ID
            Else
                sNewAppItem = oNewApp.FileSafeName
                sOriginalAppItem = oOriginalApp.FileSafeName
            End If

            'Remote
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Remote) Then

                'Check for existing folder
                sDirectory = BackupFolder & sOriginalAppItem
                sNewDirectory = sDirectory.Replace(sOriginalAppItem, sNewAppItem)
                If Directory.Exists(sNewDirectory) Then
                    If mgrCommon.ShowMessage(frmGameManager_ErrorRenameFolderExists, New String() {sDirectory, sNewDirectory}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        mgrCommon.DeleteDirectory(sNewDirectory, True)
                    Else
                        Return False
                    End If
                End If

                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Remote)

                'Check for existing files
                For Each oBackupItem As clsBackup In oBackupItems
                    sFileName = BackupFolder & oBackupItem.FileName
                    sNewFileName = Path.GetDirectoryName(sFileName) & Path.DirectorySeparatorChar & Path.GetFileName(sFileName).Replace(sOriginalAppItem, sNewAppItem)
                    If File.Exists(sNewFileName) Then
                        If mgrCommon.ShowMessage(frmGameManager_ErrorRenameFilesExist, New String() {sOriginalAppItem, sNewAppItem}, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            Return False
                        End If
                        Exit For
                    End If
                Next

                'Rename files                
                For Each oBackupItem As clsBackup In oBackupItems
                    'Rename Current Backup File
                    sFileName = BackupFolder & oBackupItem.FileName
                    sNewFileName = Path.GetDirectoryName(sFileName) & Path.DirectorySeparatorChar & Path.GetFileName(sFileName).Replace(sOriginalAppItem, sNewAppItem)
                    If File.Exists(sFileName) And Not sFileName = sNewFileName Then
                        If File.Exists(sNewFileName) Then mgrCommon.DeleteFile(sNewFileName)
                        FileSystem.Rename(sFileName, sNewFileName)
                    End If
                    oBackupItem.MonitorID = oNewApp.ID
                    oBackupItem.FileName = oBackupItem.FileName.Replace(sOriginalAppItem, sNewAppItem)
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Remote)
                Next

                'Rename folder                
                If Directory.Exists(sDirectory) And Not sDirectory = sNewDirectory Then
                    FileSystem.Rename(sDirectory, sNewDirectory)
                End If
            End If

            'Local
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Local) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Local)
                'The local manifest will only have one entry per game, therefore this runs only once
                For Each oBackupItem As clsBackup In oBackupItems
                    oBackupItem.MonitorID = oNewApp.ID
                    oBackupItem.FileName = oBackupItem.FileName.Replace(sOriginalAppItem, sNewAppItem)
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Local)
                Next
            End If
        End If

        Return True
    End Function

    Private Sub LoadData(Optional ByVal bRetainFilter As Boolean = True)
        Dim oRestoreData As New SortedList
        Dim oGame As clsGame
        Dim frm As frmFilter

        If optCustom.Checked Then
            If Not bRetainFilter Then
                frm = New frmFilter

                frm.IncludeTagFilters = oCurrentIncludeTagFilters
                frm.ExcludeTagFilters = oCurrentExcludeTagFilters
                frm.GameFilters = oCurrentFilters
                frm.FilterType = eCurrentFilter
                frm.AndOperator = bCurrentAndOperator
                frm.SortAsc = bCurrentSortAsc
                frm.SortField = sCurrentSortField

                frm.ShowDialog()

                oCurrentIncludeTagFilters = frm.IncludeTagFilters
                oCurrentExcludeTagFilters = frm.ExcludeTagFilters
                oCurrentFilters = frm.GameFilters
                eCurrentFilter = frm.FilterType
                bCurrentAndOperator = frm.AndOperator
                bCurrentSortAsc = frm.SortAsc
                sCurrentSortField = frm.SortField
            End If
        Else
            oCurrentIncludeTagFilters.Clear()
            oCurrentExcludeTagFilters.Clear()
            oCurrentFilters.Clear()
            eCurrentFilter = frmFilter.eFilterType.BaseFilter
            bCurrentSortAsc = True
            sCurrentSortField = "Name"
        End If

        GameData = mgrMonitorList.ReadFilteredList(oCurrentIncludeTagFilters, oCurrentExcludeTagFilters, oCurrentFilters, eCurrentFilter, bCurrentAndOperator, bCurrentSortAsc, sCurrentSortField)

        If optPendingRestores.Checked Then
            oRestoreData = mgrRestore.CompareManifests

            'Only show games with data to restore
            Dim oTemporaryList As OrderedDictionary = mgrMonitorList.ReadFilteredList(oCurrentIncludeTagFilters, oCurrentExcludeTagFilters, oCurrentFilters, eCurrentFilter, bCurrentAndOperator, bCurrentSortAsc, sCurrentSortField)
            For Each de As DictionaryEntry In oTemporaryList
                oGame = DirectCast(de.Value, clsGame)
                If Not oRestoreData.ContainsKey(oGame.ID) Then
                    GameData.Remove(de.Key)
                Else
                    oRestoreData.Remove(oGame.ID)
                End If
            Next
        ElseIf optBackupData.Checked Then
            'Only show games with backup data
            Dim oTemporaryList As OrderedDictionary = mgrMonitorList.ReadFilteredList(oCurrentIncludeTagFilters, oCurrentExcludeTagFilters, oCurrentFilters, eCurrentFilter, bCurrentAndOperator, bCurrentSortAsc, sCurrentSortField)
            oRestoreData = oRemoteBackupData.Clone

            For Each de As DictionaryEntry In oTemporaryList
                oGame = DirectCast(de.Value, clsGame)
                If Not oRemoteBackupData.ContainsKey(oGame.ID) Then
                    GameData.Remove(de.Key)
                Else
                    oRestoreData.Remove(oGame.ID)
                End If
            Next
        End If

        lstGames.DataSource = Nothing
        FormatAndFillList()
    End Sub

    Private Sub ProcessBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = txtAppPath.Text
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFileBrowser("GM_Process", frmGameManager_ChooseExe, "exe",
                                          frmGameManager_Executable, sDefaultFolder, False)

        If sNewPath <> String.Empty Then
            txtAppPath.Text = Path.GetDirectoryName(sNewPath)
            txtProcess.Text = Path.GetFileName(sNewPath)
        End If

    End Sub

    Private Sub ProcessPathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = txtAppPath.Text
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("GM_Process_Path", frmGameManager_ChooseExePath, sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtAppPath.Text = sNewPath
    End Sub

    Private Sub SavePathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = mgrPath.ReplaceSpecialPaths(txtSavePath.Text)
        Dim sNewPath As String

        If txtSavePath.Text <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("GM_Save_Path", frmGameManager_ChooseSaveFolder, sDefaultFolder, False)

        If sNewPath <> String.Empty Then
            txtSavePath.Text = sNewPath
            txtSavePath.Text = HandleSavePath(txtSavePath.Text, txtAppPath.Text)
        End If
    End Sub

    Private Sub IconBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = txtAppPath.Text
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        'Unix Handler
        If Not mgrCommon.IsUnix Then
            sNewPath = mgrCommon.OpenFileBrowser("GM_Icon", frmGameManager_ChooseCustomIcon, "ico",
                                              frmGameManager_Icon, sDefaultFolder, False)
        Else
            sNewPath = mgrCommon.OpenFileBrowser("GM_Icon", frmGameManager_ChooseCustomIcon, "png",
                                              "PNG", sDefaultFolder, False)
        End If

        If sNewPath <> String.Empty Then
            txtIcon.Text = sNewPath
            If IO.File.Exists(sNewPath) Then
                pbIcon.Image = mgrCommon.SafeIconFromFile(sNewPath)
            End If
        End If
    End Sub

    Private Function HandleDirty() As MsgBoxResult
        Dim oResult As MsgBoxResult

        oResult = mgrCommon.ShowMessage(App_ConfirmDirty, MsgBoxStyle.YesNoCancel)

        Select Case oResult
            Case MsgBoxResult.Yes
                IsDirty = False
            Case MsgBoxResult.No
                IsDirty = False
            Case MsgBoxResult.Cancel
                'No Change
        End Select

        Return oResult

    End Function

    Private Sub FormatAndFillList()
        IsLoading = True

        Dim oApp As clsGame
        Dim oData As KeyValuePair(Of String, String)
        Dim oList As New List(Of KeyValuePair(Of String, String))
        Dim sFilter As String = txtQuickFilter.Text

        For Each de As DictionaryEntry In GameData
            oApp = DirectCast(de.Value, clsGame)
            oData = New KeyValuePair(Of String, String)(oApp.ID, oApp.Name)
            'Apply the quick filter if applicable
            If sFilter = String.Empty Then
                oList.Add(oData)
            Else
                If oApp.Name.ToLower.Contains(sFilter.ToLower) Then
                    oList.Add(oData)
                End If
            End If
        Next

        lstGames.BeginUpdate()
        lstGames.ValueMember = "Key"
        lstGames.DisplayMember = "Value"

        'Due to a control bug with Mono we need to fill the list box differently on Linux
        If mgrCommon.IsUnix Then
            lstGames.Items.Clear()
            For Each kp As KeyValuePair(Of String, String) In oList
                lstGames.Items.Add(kp)
            Next
        Else
            lstGames.DataSource = oList
        End If

        lstGames.EndUpdate()
        lstGames.ClearSelected()
        IsLoading = False
    End Sub

    Private Sub OpenGameIDEdit()
        Dim sCurrentID As String
        Dim sNewID As String

        If txtID.Text = String.Empty Then
            txtID.Text = Guid.NewGuid.ToString
        End If

        sCurrentID = txtID.Text

        sNewID = InputBox(frmGameManager_GameIDEditInfo, frmGameManager_GameIDEditTitle, sCurrentID)

        If sNewID <> String.Empty Then
            txtID.Text = sNewID

            If sCurrentID <> sNewID Then
                UpdateGenericButtonLabel(frmGameManager_btnGameID, btnGameID, True)
            End If
        End If

    End Sub

    Private Sub OpenBackupFile()
        Dim sFileName As String = BackupFolder & CurrentBackupItem.FileName

        mgrCommon.OpenInOS(sFileName, frmGameManager_ErrorNoBackupFileExists)
    End Sub

    Private Sub OpenBackupFolder()
        Dim sFileName As String = BackupFolder & Path.GetDirectoryName(CurrentBackupItem.FileName)

        mgrCommon.OpenInOS(sFileName, frmGameManager_ErrorNoBackupFolderExists)
    End Sub

    Private Sub UpdateBuilderButtonLabel(ByVal sBuilderString As String, ByVal sLabel As String, ByVal btn As Button, ByVal bDirty As Boolean)
        Dim iCount As Integer = sBuilderString.Split(":").Length

        If sBuilderString <> String.Empty And iCount > 0 Then
            btn.Text = sLabel & " " & mgrCommon.FormatString(frmGameManager_ItemsExist, iCount)
        Else
            btn.Text = sLabel & " " & frmGameManager_Items
        End If

        If bDirty Then
            btn.Font = New Font(FontFamily.GenericSansSerif, 8.25, FontStyle.Bold)
        Else
            btn.Font = New Font(FontFamily.GenericSansSerif, 8.25, FontStyle.Regular)
        End If
    End Sub

    Private Sub UpdateGenericButtonLabel(ByVal sLabel As String, ByVal btn As Button, ByVal bDirty As Boolean)
        btn.Text = sLabel

        If bDirty Then
            btn.Font = New Font(FontFamily.GenericSansSerif, 8.25, FontStyle.Bold)
        Else
            btn.Font = New Font(FontFamily.GenericSansSerif, 8.25, FontStyle.Regular)
        End If
    End Sub

    Private Function GetBuilderRoot() As String
        Dim sRoot As String = String.Empty
        Dim sPath As String = mgrPath.ValidatePath(txtSavePath.Text)

        If Not Settings.ShowResolvedPaths Then sPath = mgrPath.ReplaceSpecialPaths(sPath)

        If Path.IsPathRooted(sPath) Then
            If Directory.Exists(sPath) Then
                sRoot = sPath
            End If
        Else
            If txtAppPath.Text <> String.Empty Then
                If Directory.Exists(txtAppPath.Text & Path.DirectorySeparatorChar & sPath) Then
                    sRoot = txtAppPath.Text & Path.DirectorySeparatorChar & sPath
                End If
            End If
        End If

        Return sRoot
    End Function

    Private Sub OpenBuilder(ByVal sFormText As String, ByRef txtBox As TextBox)
        Dim frm As New frmIncludeExclude
        frm.FormName = sFormText
        frm.BuilderString = txtBox.Text
        frm.RootFolder = GetBuilderRoot()
        frm.RecurseSubFolders = chkRecurseSubFolders.Checked
        frm.ShowDialog()

        txtBox.Text = frm.BuilderString
        chkRecurseSubFolders.Checked = frm.RecurseSubFolders
        VerifyCleanFolder()
    End Sub

    Private Function FindRestorePath() As Boolean
        Dim sProcess As String
        Dim sRestorePath As String
        Dim bNoAuto As Boolean

        If Not CurrentBackupItem.AbsolutePath Then
            If CurrentGame.ProcessPath <> String.Empty Then
                CurrentBackupItem.RelativeRestorePath = CurrentGame.ProcessPath & Path.DirectorySeparatorChar & CurrentBackupItem.RestorePath
            Else
                sProcess = CurrentGame.ProcessName
                If mgrCommon.IsProcessNotSearchable(CurrentGame) Then bNoAuto = True
                sRestorePath = mgrPath.ProcessPathSearch(CurrentBackupItem.Name, sProcess, mgrCommon.FormatString(frmGameManager_ErrorPathNotSet, CurrentBackupItem.Name), bNoAuto)

                If sRestorePath <> String.Empty Then
                    CurrentBackupItem.RelativeRestorePath = sRestorePath & Path.DirectorySeparatorChar & CurrentBackupItem.RestorePath
                    txtAppPath.Text = sRestorePath
                Else
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Private Sub OpenRestorePath()
        Dim sPath As String = String.Empty

        If CurrentBackupItem.AbsolutePath Then
            sPath = CurrentBackupItem.RestorePath
        Else
            If FindRestorePath() Then
                sPath = CurrentBackupItem.RelativeRestorePath
            End If
        End If

        mgrCommon.OpenInOS(sPath, frmGameManager_ErrorNoRestorePathExists)
    End Sub

    Private Sub OpenProcesses()
        Dim frm As New frmGameProcesses
        Dim oApp As clsGame
        Dim sMonitorIDS As New List(Of String)

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDS.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.ProcessList = oProcessesToSave
        Else
            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(GameData(oData.Key), clsGame)
                sMonitorIDS.Add(oApp.ID)
            Next
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDS
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oProcessesToSave = frm.ProcessList
        Else
            ModeChange()
        End If
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmGameTags
        Dim oApp As clsGame
        Dim sMonitorIDs As New List(Of String)

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDs.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.TagList = oTagsToSave
        Else
            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(GameData(oData.Key), clsGame)
                sMonitorIDs.Add(oApp.ID)
            Next
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDs
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oTagsToSave = frm.TagList
            lblTags.Text = mgrGameTags.PrintTagsbyList(frm.TagList)
        Else
            'Sync
            mgrMonitorList.SyncMonitorLists(Settings)

            'Only update visible tags if one item is selected
            If lstGames.SelectedItems.Count = 1 Then lblTags.Text = mgrGameTags.PrintTagsbyID(CurrentGame.ID)

            'If a tag filter is enabled, reload list to reflect changes
            If optCustom.Checked Then
                LoadData()
            End If

            'If the selected game(s) no longer match the filter, disable the form
            If lstGames.SelectedIndex = -1 Then eCurrentMode = eModes.Disabled
            ModeChange()
        End If

    End Sub

    Private Sub OpenConfigLinks()
        Dim frm As New frmConfigLinks
        Dim oApp As clsGame
        Dim sMonitorIDs As New List(Of String)

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDs.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.ConfigLinkList = oConfigLinksToSave
        Else
            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(GameData(oData.Key), clsGame)
                sMonitorIDs.Add(oApp.ID)
            Next
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDs
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oConfigLinksToSave = frm.ConfigLinkList
        Else
            'Sync
            mgrMonitorList.SyncMonitorLists(Settings)
            ModeChange()
        End If
    End Sub

    Public Sub OpenWineConfiguration()
        Dim frm As New frmWineConfiguration
        frm.Settings = oSettings
        frm.MonitorID = oCurrentGame.ID
        frm.ShowDialog()
    End Sub

    Public Sub VerifyBackups(ByVal oApp As clsGame)
        Dim oCurrentBackup As clsBackup
        Dim oCurrentBackups As List(Of clsBackup)
        Dim oBackupsRemoved As New List(Of clsBackup)

        oCurrentBackups = mgrManifest.DoManifestGetByMonitorID(oApp.ID, mgrSQLite.Database.Remote)

        Cursor.Current = Cursors.WaitCursor

        For Each oCurrentBackup In oCurrentBackups
            If Not File.Exists(BackupFolder & oCurrentBackup.FileName) Then
                oBackupsRemoved.Add(oCurrentBackup)
                mgrManifest.DoManifestDeleteByManifestID(oCurrentBackup, mgrSQLite.Database.Remote)
            End If
        Next

        If oBackupsRemoved.Count > 0 Then
            For Each oCurrentBackup In oBackupsRemoved
                oCurrentBackups.Remove(oCurrentBackup)
                If oCurrentBackups.Count = 0 Then
                    mgrManifest.DoManifestDeleteByMonitorID(oCurrentBackup, mgrSQLite.Database.Local)
                End If
            Next
            LoadBackupData()
            GetBackupInfo(oApp)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub SetBackupRestorePath(ByVal oApp As clsGame)
        Dim sttRestorePath As String

        If Not CurrentBackupItem.AbsolutePath And oApp.ProcessPath <> String.Empty Then
            lblRestorePathData.Text = oApp.ProcessPath & Path.DirectorySeparatorChar & CurrentBackupItem.RestorePath
        Else
            If oSettings.ShowResolvedPaths Then
                lblRestorePathData.Text = CurrentBackupItem.RestorePath
                sttRestorePath = CurrentBackupItem.TruePath
            Else
                lblRestorePathData.Text = CurrentBackupItem.TruePath
                sttRestorePath = CurrentBackupItem.RestorePath
            End If
            If CurrentBackupItem.AbsolutePath Then ttFullPath.SetToolTip(lblRestorePathData, sttRestorePath)
        End If
    End Sub

    Private Sub GetBackupInfo(ByVal oApp As clsGame)
        Dim oBackupInfo As clsBackup
        Dim oCurrentBackup As clsBackup
        Dim oCurrentBackups As List(Of clsBackup)
        Dim sFileName As String
        Dim oComboItems As New List(Of KeyValuePair(Of String, String))
        Dim bLocalData As Boolean = False
        Dim bRemoteData As Boolean = False


        'cboRemoteBackup
        cboRemoteBackup.ValueMember = "Key"
        cboRemoteBackup.DisplayMember = "Value"

        If oRemoteBackupData.Contains(oApp.ID) Then
            bRemoteData = True
            oCurrentBackups = mgrManifest.DoManifestGetByMonitorID(oApp.ID, mgrSQLite.Database.Remote)

            For Each oCurrentBackup In oCurrentBackups
                oComboItems.Add(New KeyValuePair(Of String, String)(oCurrentBackup.ManifestID, mgrCommon.FormatString(frmGameManager_BackupTimeAndName, New String() {oCurrentBackup.DateUpdated, oCurrentBackup.UpdatedBy})))
            Next

            CurrentBackupItem = DirectCast(oRemoteBackupData(oApp.ID), clsBackup)

            sFileName = BackupFolder & CurrentBackupItem.FileName

            btnOpenBackup.Enabled = True
            btnOpenRestorePath.Enabled = True
            btnRestore.Enabled = True
            btnDeleteBackup.Enabled = True

            If File.Exists(sFileName) Then
                lblBackupFileData.Text = Path.GetFileName(CurrentBackupItem.FileName) & " (" & mgrCommon.FormatDiskSpace(mgrCommon.GetFileSize(sFileName)) & ")"
            Else
                lblBackupFileData.Text = frmGameManager_ErrorNoBackupFileExists
            End If

            SetBackupRestorePath(oApp)
        Else
            oComboItems.Add(New KeyValuePair(Of String, String)(String.Empty, frmGameManager_None))
            lblBackupFileData.Text = String.Empty
            lblRestorePathData.Text = String.Empty
            btnOpenBackup.Enabled = False
            btnOpenRestorePath.Enabled = False
            btnRestore.Enabled = False
            btnDeleteBackup.Enabled = False
        End If

        cboRemoteBackup.DataSource = oComboItems

        If oLocalBackupData.Contains(oApp.ID) Then
            bLocalData = True
            oBackupInfo = DirectCast(oLocalBackupData(oApp.ID), clsBackup)
            lblLocalBackupData.Text = mgrCommon.FormatString(frmGameManager_BackupTimeAndName, New String() {oBackupInfo.DateUpdated, oBackupInfo.UpdatedBy})
        Else
            lblLocalBackupData.Text = frmGameManager_Unknown
        End If

        If Not bRemoteData And Not bLocalData Then
            btnMarkAsRestored.Enabled = False
            lblLocalBackupData.ForeColor = Color.Black
        ElseIf Not bRemoteData And bLocalData Then
            btnMarkAsRestored.Enabled = False
            lblLocalBackupData.ForeColor = Color.Black
        ElseIf oComboItems(0).Value <> lblLocalBackupData.Text Then
            lblLocalBackupData.ForeColor = Color.Red
            btnMarkAsRestored.Enabled = True
        Else
            lblLocalBackupData.ForeColor = Color.Green
            btnMarkAsRestored.Enabled = False
        End If

        If chkMonitorOnly.Checked Then
            btnImportBackup.Enabled = False
        Else
            btnImportBackup.Enabled = True
        End If

        If mgrPath.IsSupportedRegistryPath(oApp.TruePath) Then
            btnImportBackup.Enabled = False
            btnOpenBackup.Enabled = False
            btnOpenRestorePath.Enabled = False
        End If

    End Sub

    Private Sub UpdateBackupInfo(ByVal sManifestID As String)
        Dim sFileName As String

        If sManifestID <> String.Empty Then
            CurrentBackupItem = mgrManifest.DoManifestGetByManifestID(sManifestID, mgrSQLite.Database.Remote)

            sFileName = BackupFolder & CurrentBackupItem.FileName

            If File.Exists(sFileName) Then
                lblBackupFileData.Text = Path.GetFileName(CurrentBackupItem.FileName) & " (" & mgrCommon.FormatDiskSpace(mgrCommon.GetFileSize(sFileName)) & ")"
            Else
                lblBackupFileData.Text = frmGameManager_ErrorNoBackupFileExists
            End If

            SetBackupRestorePath(CurrentGame)
        End If
    End Sub

    Private Sub DeleteAllBackups()
        Dim oBackupData As List(Of clsBackup)
        Dim oBackup As clsBackup

        If mgrCommon.ShowMessage(frmGameManager_ConfirmBackupDeleteAll, CurrentGame.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            oBackupData = mgrManifest.DoManifestGetByMonitorID(CurrentGame.ID, mgrSQLite.Database.Remote)

            For Each oBackup In oBackupData
                'Delete the specific remote manifest entry
                mgrManifest.DoManifestDeleteByManifestID(oBackup, mgrSQLite.Database.Remote)
                'Delete referenced backup file from the backup folder
                mgrCommon.DeleteFile(BackupFolder & oBackup.FileName)
                'Check for sub-directory and delete if empty (we need to do this every pass just in case the user had a mix of settings at one point)
                mgrCommon.DeleteDirectoryByBackup(BackupFolder, oBackup)
            Next

            'Delete local manifest entry
            mgrManifest.DoManifestDeleteByMonitorID(CurrentBackupItem, mgrSQLite.Database.Local)

            LoadBackupData()
            FillData()
        End If
    End Sub

    Private Sub DeleteBackup()
        If mgrCommon.ShowMessage(frmGameManager_ConfirmBackupDelete, Path.GetFileName(CurrentBackupItem.FileName), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            'Delete the specific remote manifest entry
            mgrManifest.DoManifestDeleteByManifestID(CurrentBackupItem, mgrSQLite.Database.Remote)

            'If a remote manifest entry no longer exists for this game, delete the local entry
            If Not mgrManifest.DoManifestCheck(CurrentBackupItem.MonitorID, mgrSQLite.Database.Remote) Then
                mgrManifest.DoManifestDeleteByMonitorID(CurrentBackupItem, mgrSQLite.Database.Local)
            End If

            'Delete referenced backup file from the backup folder
            mgrCommon.DeleteFile(BackupFolder & CurrentBackupItem.FileName)

            'Check for sub-directory and delete if empty
            mgrCommon.DeleteDirectoryByBackup(BackupFolder, CurrentBackupItem)

            LoadBackupData()
            FillData()
        End If
    End Sub

    Private Sub FillData()
        IsLoading = True

        Dim oData As KeyValuePair(Of String, String) = lstGames.SelectedItems(0)
        Dim oApp As clsGame = DirectCast(GameData(oData.Key), clsGame)
        Dim sttPath As String

        'Core
        txtID.Text = oApp.ID
        txtName.Text = oApp.Name
        txtProcess.Text = oApp.ProcessName
        chkRegEx.Checked = oApp.IsRegEx
        txtParameter.Text = oApp.Parameter
        cboOS.SelectedValue = CInt(oApp.OS)
        If oSettings.ShowResolvedPaths Then
            txtSavePath.Text = oApp.Path
            sttPath = oApp.TruePath
        Else
            txtSavePath.Text = oApp.TruePath
            sttPath = oApp.Path
        End If
        If oApp.AbsolutePath Then ttFullPath.SetToolTip(txtSavePath, sttPath)
        txtFileType.Text = oApp.FileType
        txtExclude.Text = oApp.ExcludeList
        chkFolderSave.Checked = oApp.FolderSave
        chkRecurseSubFolders.Checked = oApp.RecurseSubFolders
        chkCleanFolder.Checked = oApp.CleanFolder
        chkTimeStamp.Checked = oApp.AppendTimeStamp
        nudLimit.Value = oApp.BackupLimit
        txtComments.Text = oApp.Comments
        chkEnabled.Checked = oApp.Enabled
        chkMonitorOnly.Checked = oApp.MonitorOnly

        'Update Buttons
        UpdateBuilderButtonLabel(oApp.FileType, frmGameManager_IncludeShortcut, btnInclude, False)
        UpdateBuilderButtonLabel(oApp.ExcludeList, frmGameManager_ExcludeShortcut, btnExclude, False)
        UpdateGenericButtonLabel(frmGameManager_btnGameID, btnGameID, False)
        HandleWineConfig()

        'Extra
        If oSettings.ShowResolvedPaths Then
            txtAppPath.Text = oApp.ProcessPath
            ttFullPath.SetToolTip(txtAppPath, oApp.TrueProcessPath)
        Else
            txtAppPath.Text = oApp.TrueProcessPath
            ttFullPath.SetToolTip(txtAppPath, oApp.ProcessPath)
        End If
        txtCompany.Text = oApp.Company
        txtVersion.Text = oApp.Version
        txtIcon.Text = oApp.Icon

        lblTags.Text = mgrGameTags.PrintTagsbyID(oData.Key)

        'Icon
        If IO.File.Exists(oApp.Icon) Then
            pbIcon.Image = mgrCommon.SafeIconFromFile(oApp.Icon)
        Else
            pbIcon.Image = Icon_Unknown
        End If

        'Stats
        nudHours.Value = oApp.Hours
        GetBackupInfo(oApp)

        'Set Current
        CurrentGame = oApp

        IsLoading = False
    End Sub

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        If Not IsLoading And Not eCurrentMode = eModes.MultiSelect Then
            IsDirty = True
            If Not eCurrentMode = eModes.Add Then EditApp()
        End If
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                AddHandler DirectCast(ctl, TextBox).TextChanged, AddressOf DirtyCheck_ValueChanged
            ElseIf TypeOf ctl Is CheckBox Then
                AddHandler DirectCast(ctl, CheckBox).CheckedChanged, AddressOf DirtyCheck_ValueChanged
            ElseIf TypeOf ctl Is NumericUpDown Then
                AddHandler DirectCast(ctl, NumericUpDown).ValueChanged, AddressOf DirtyCheck_ValueChanged
            End If
        Next
    End Sub

    Private Sub AssignDirtyHandlersMisc()
        AddHandler chkEnabled.CheckedChanged, AddressOf DirtyCheck_ValueChanged
        AddHandler chkMonitorOnly.CheckedChanged, AddressOf DirtyCheck_ValueChanged
        AddHandler cboOS.SelectedValueChanged, AddressOf DirtyCheck_ValueChanged
    End Sub

    Private Sub WipeControls(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Text = String.Empty
            ElseIf TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Checked = False
            ElseIf TypeOf ctl Is Label Then
                If ctl.Tag = "wipe" Then DirectCast(ctl, Label).Text = String.Empty
            ElseIf TypeOf ctl Is NumericUpDown Then
                DirectCast(ctl, NumericUpDown).Value = DirectCast(ctl, NumericUpDown).Minimum
            ElseIf TypeOf ctl Is ComboBox Then
                If ctl.Tag = "wipe" Then DirectCast(ctl, ComboBox).DataSource = Nothing
            End If
        Next
    End Sub

    Private Sub ModeChange(Optional ByVal bNoFocusChange As Boolean = False)
        IsLoading = True

        Select Case eCurrentMode
            Case eModes.Add
                oTagsToSave.Clear()
                oProcessesToSave.Clear()
                grpFilter.Enabled = False
                lstGames.Enabled = False
                lblQuickFilter.Enabled = False
                txtQuickFilter.Enabled = False
                grpConfig.Enabled = True
                chkMonitorOnly.Enabled = True
                grpExtra.Enabled = True
                grpStats.Enabled = True
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                chkCleanFolder.Enabled = False
                pbIcon.Image = Icon_Unknown
                chkEnabled.Enabled = True
                chkMonitorOnly.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                btnBackup.Enabled = False
                btnMarkAsRestored.Enabled = False
                btnRestore.Enabled = False
                btnImportBackup.Enabled = False
                btnDeleteBackup.Enabled = False
                btnOpenBackup.Enabled = False
                btnOpenRestorePath.Enabled = False
                chkEnabled.Checked = True
                chkMonitorOnly.Checked = False
                chkRecurseSubFolders.Checked = True
                btnTags.Enabled = True
                btnLink.Enabled = True
                lblTags.Text = String.Empty
                lblTags.Visible = True
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = False
                btnExport.Enabled = False
                cboOS.SelectedValue = CInt(mgrCommon.GetCurrentOS)
                HandleWineConfig()
            Case eModes.Edit
                grpFilter.Enabled = False
                lstGames.Enabled = False
                lblQuickFilter.Enabled = False
                txtQuickFilter.Enabled = False
                grpConfig.Enabled = True
                chkEnabled.Enabled = True
                chkMonitorOnly.Enabled = True
                grpExtra.Enabled = True
                grpStats.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                btnBackup.Enabled = False
                btnMarkAsRestored.Enabled = False
                btnRestore.Enabled = False
                btnImportBackup.Enabled = False
                btnDeleteBackup.Enabled = False
                btnOpenBackup.Enabled = False
                btnOpenRestorePath.Enabled = False
                btnTags.Enabled = True
                btnLink.Enabled = True
                lblTags.Visible = True
                btnImport.Enabled = False
                btnExport.Enabled = False
            Case eModes.View
                grpFilter.Enabled = True
                lstGames.Enabled = True
                lblQuickFilter.Enabled = True
                txtQuickFilter.Enabled = True
                grpConfig.Enabled = True
                chkEnabled.Enabled = True
                chkMonitorOnly.Enabled = True
                grpExtra.Enabled = True
                grpStats.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
                btnBackup.Enabled = True
                btnTags.Enabled = True
                btnLink.Enabled = True
                lblTags.Visible = True
                btnImport.Enabled = True
                btnExport.Enabled = True
            Case eModes.Disabled
                grpFilter.Enabled = True
                lstGames.Enabled = True
                lblQuickFilter.Enabled = True
                txtQuickFilter.Enabled = True
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                pbIcon.Image = Icon_Unknown
                btnSave.Enabled = False
                btnCancel.Enabled = False
                grpConfig.Enabled = False
                chkEnabled.Enabled = False
                chkMonitorOnly.Enabled = False
                grpExtra.Enabled = False
                grpStats.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
                btnBackup.Enabled = False
                btnRestore.Enabled = False
                btnMarkAsRestored.Enabled = False
                btnTags.Enabled = False
                btnLink.Enabled = False
                lblTags.Visible = False
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = True
                btnExport.Enabled = True
                cboOS.SelectedValue = CInt(mgrCommon.GetCurrentOS)
                UpdateGenericButtonLabel(frmGameManager_IncludeShortcut, btnInclude, False)
                UpdateGenericButtonLabel(frmGameManager_ExcludeShortcut, btnExclude, False)
                UpdateGenericButtonLabel(frmGameManager_btnGameID, btnGameID, False)
            Case eModes.MultiSelect
                lstGames.Enabled = True
                lblQuickFilter.Enabled = False
                txtQuickFilter.Enabled = False
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                pbIcon.Image = Icon_Unknown
                btnSave.Enabled = True
                btnCancel.Enabled = False
                grpConfig.Enabled = False
                chkMonitorOnly.Enabled = True
                chkMonitorOnly.Checked = False
                chkEnabled.Enabled = True
                chkEnabled.Checked = False
                grpExtra.Enabled = False
                grpStats.Enabled = False
                btnAdd.Enabled = False
                btnDelete.Enabled = True
                btnBackup.Enabled = True
                btnRestore.Enabled = True
                btnMarkAsRestored.Enabled = True
                btnTags.Enabled = True
                btnLink.Enabled = True
                lblTags.Visible = False
                btnImport.Enabled = True
                btnExport.Enabled = True
        End Select

        If Not bNoFocusChange Then lstGames.Focus()

        IsLoading = False
    End Sub

    Private Sub FolderSaveModeChange()
        If chkFolderSave.Checked Then
            btnInclude.Enabled = False
            If txtFileType.Text <> String.Empty Then
                txtFileType.Text = String.Empty
                UpdateBuilderButtonLabel(txtFileType.Text, frmGameManager_IncludeShortcut, btnInclude, False)
            End If
        Else
            btnInclude.Enabled = True
        End If
        VerifyCleanFolder()
    End Sub

    Private Sub MonitorOnlyModeChange()
        If chkMonitorOnly.Checked Then
            chkFolderSave.Enabled = False
            chkTimeStamp.Enabled = False
            lblLimit.Enabled = False
            nudLimit.Enabled = False
            lblSavePath.Enabled = False
            txtSavePath.Enabled = False
            btnSavePathBrowse.Enabled = False
            btnInclude.Enabled = False
            btnExclude.Enabled = False
            chkCleanFolder.Enabled = False
            btnWineConfig.Enabled = False
        Else
            chkFolderSave.Enabled = True
            chkTimeStamp.Enabled = True
            lblLimit.Enabled = True
            nudLimit.Enabled = True
            lblSavePath.Enabled = True
            txtSavePath.Enabled = True
            btnSavePathBrowse.Enabled = True
            btnInclude.Enabled = True
            btnExclude.Enabled = True
            btnWineConfig.Enabled = True
            FolderSaveModeChange()
        End If
    End Sub

    Private Sub RegistryModeChange()
        If mgrPath.IsSupportedRegistryPath(txtSavePath.Text) Then
            cboOS.SelectedValue = CInt(clsGame.eOS.Windows)
            chkFolderSave.Checked = True
            chkFolderSave.Enabled = False
            btnInclude.Enabled = False
            btnExclude.Enabled = False
        Else
            chkFolderSave.Enabled = True
            btnInclude.Enabled = True
            btnExclude.Enabled = True
        End If
    End Sub

    Private Sub TimeStampModeChange()
        If chkTimeStamp.Checked Then
            nudLimit.Visible = True
            lblLimit.Visible = True
            nudLimit.Value = 0
        Else
            nudLimit.Visible = False
            nudLimit.Value = nudLimit.Minimum
            lblLimit.Visible = False
        End If
    End Sub

    Private Sub VerifyImportBackup()
        If Not bIsLoading Then
            If chkMonitorOnly.Checked Then
                btnImportBackup.Enabled = False
            Else
                btnImportBackup.Enabled = True
            End If
        End If
    End Sub

    Private Sub VerifyCleanFolder()
        If Not bIsLoading Then
            If (chkFolderSave.Checked = True And txtExclude.Text = String.Empty And txtSavePath.Text <> String.Empty) And Not mgrPath.IsSupportedRegistryPath(txtSavePath.Text) And Not chkMonitorOnly.Checked Then
                chkCleanFolder.Enabled = True
            Else
                chkCleanFolder.Checked = False
                chkCleanFolder.Enabled = False
            End If
        End If
    End Sub

    Private Sub EditApp()
        eCurrentMode = eModes.Edit
        ModeChange()
    End Sub

    Private Sub AddApp()
        eCurrentMode = eModes.Add
        ModeChange()
        txtName.Focus()
    End Sub

    Private Sub CancelEdit()
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveApp()
                Case MsgBoxResult.No
                    If lstGames.SelectedItems.Count > 0 Then
                        eCurrentMode = eModes.View
                        ModeChange()
                        FillData()
                        lstGames.Focus()
                    Else
                        eCurrentMode = eModes.Disabled
                        ModeChange()
                    End If
                Case MsgBoxResult.Cancel
                    'Do Nothing
            End Select
        Else
            If lstGames.SelectedItems.Count > 0 Then
                eCurrentMode = eModes.View
                ModeChange()
                FillData()
                lstGames.Focus()
            Else
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SaveConfigLinks(ByVal sID As String)
        Dim oConfigLink As clsConfigLink
        Dim oConfigLinks As List(Of clsConfigLink)

        If oConfigLinksToSave.Count > 0 Then
            oConfigLinks = New List(Of clsConfigLink)
            For Each kp As KeyValuePair(Of String, String) In oConfigLinksToSave
                oConfigLink = New clsConfigLink
                oConfigLink.MonitorID = sID
                oConfigLink.LinkID = kp.Key
                oConfigLinks.Add(oConfigLink)
            Next
            mgrConfigLinks.DoConfigLinkAddBatch(oConfigLinks)
        End If
    End Sub

    Private Sub SaveProcesses(ByVal sID As String)
        Dim oGameProcess As clsGameProcess
        Dim oGameProcesses As List(Of clsGameProcess)

        If oProcessesToSave.Count > 0 Then
            oGameProcesses = New List(Of clsGameProcess)
            For Each kp As KeyValuePair(Of String, String) In oProcessesToSave
                oGameProcess = New clsGameProcess
                oGameProcess.MonitorID = sID
                oGameProcess.ProcessID = kp.Key
                oGameProcesses.Add(oGameProcess)
            Next
            mgrGameProcesses.DoGameProcessAddBatch(oGameProcesses)
        End If
    End Sub

    Private Sub SaveTags(ByVal sID As String)
        Dim oGameTag As clsGameTag
        Dim oGameTags As List(Of clsGameTag)

        If oTagsToSave.Count > 0 Then
            oGameTags = New List(Of clsGameTag)
            For Each kp As KeyValuePair(Of String, String) In oTagsToSave
                oGameTag = New clsGameTag
                oGameTag.MonitorID = sID
                oGameTag.TagID = kp.Key
                oGameTags.Add(oGameTag)
            Next
            mgrGameTags.DoGameTagAddBatch(oGameTags)
        End If
    End Sub

    Private Sub SaveApp()
        Dim oData As KeyValuePair(Of String, String)
        Dim oApp As New clsGame
        Dim bSuccess As Boolean = False

        If txtID.Text <> String.Empty Then
            oApp.ID = txtID.Text
        End If

        oApp.Name = txtName.Text
        oApp.IsRegEx = chkRegEx.Checked

        If Not oApp.IsRegEx Then
            txtProcess.Text = mgrPath.ValidateFileName(txtProcess.Text)
            If Path.HasExtension(txtProcess.Text) Then
                If txtProcess.Text.ToLower.EndsWith(".exe") Then
                    txtProcess.Text = Path.GetFileNameWithoutExtension(txtProcess.Text)
                End If
            End If
        End If

        oApp.ProcessName = txtProcess.Text
        oApp.Parameter = txtParameter.Text
        oApp.OS = CType(cboOS.SelectedValue, clsGame.eOS)
        oApp.Path = mgrPath.ValidatePath(txtSavePath.Text)

        'If we have a registry path, trim any trailing backslashes because they cause export failures
        If mgrPath.IsSupportedRegistryPath(oApp.Path) Then
            oApp.Path = oApp.Path.TrimEnd("\")
        End If

        'We need to handle a special case here when working with Windows configurations in Linux
        If mgrCommon.IsUnix And mgrVariables.CheckForReservedVariables(oApp.Path) And oApp.OS = clsGame.eOS.Windows Then
            oApp.AbsolutePath = True
        Else
            'Only do a simple root check here in case the user doesn't really understand creating a proper configuration
            oApp.AbsolutePath = Path.IsPathRooted(oApp.Path)
        End If

        oApp.FileType = txtFileType.Text
        oApp.ExcludeList = txtExclude.Text
        oApp.FolderSave = chkFolderSave.Checked
        oApp.RecurseSubFolders = chkRecurseSubFolders.Checked
        oApp.CleanFolder = chkCleanFolder.Checked
        oApp.AppendTimeStamp = chkTimeStamp.Checked
        oApp.BackupLimit = nudLimit.Value
        oApp.Comments = txtComments.Text
        oApp.Enabled = chkEnabled.Checked
        oApp.MonitorOnly = chkMonitorOnly.Checked
        oApp.ProcessPath = mgrPath.ValidatePath(txtAppPath.Text)
        oApp.Company = txtCompany.Text
        oApp.Version = txtVersion.Text
        oApp.Icon = txtIcon.Text
        oApp.Hours = CDbl(nudHours.Value)

        Select Case eCurrentMode
            Case eModes.Add
                If CoreValidatation(oApp, True) Then
                    bSuccess = True
                    mgrMonitorList.DoListAdd(oApp)
                    SaveTags(oApp.ID)
                    SaveProcesses(oApp.ID)
                    SaveConfigLinks(oApp.ID)
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oApp, False) Then
                    If CheckManifestandUpdate(oCurrentGame, oApp, oSettings.UseGameID) Then
                        bSuccess = True
                        mgrMonitorList.DoListUpdate(oApp, CurrentGame.ID)
                        eCurrentMode = eModes.View
                    End If
                End If
            Case eModes.MultiSelect
                Dim sMonitorIDs As New List(Of String)
                For Each oData In lstGames.SelectedItems
                    sMonitorIDs.Add(oData.Key)
                Next

                If mgrCommon.ShowMessage(frmGameManager_ConfirmMultiSave, New String() {sMonitorIDs.Count, mgrCommon.BooleanYesNo(oApp.Enabled), mgrCommon.BooleanYesNo(oApp.MonitorOnly)}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    bSuccess = True
                    mgrMonitorList.DoListUpdateMulti(sMonitorIDs, oApp)
                    eCurrentMode = eModes.Disabled
                End If
        End Select

        If bSuccess Then
            mgrMonitorList.SyncMonitorLists(Settings)
            LoadBackupData()
            IsDirty = False
            LoadData()
            If eCurrentMode = eModes.View Then
                lstGames.SelectedItem = New KeyValuePair(Of String, String)(oApp.ID, oApp.Name)
            Else
                ModeChange()
            End If
            'If the addition doesn't match the current filter we should go into disabled mode as it can't be selected to view
            If lstGames.SelectedIndex = -1 Then
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub DeleteApp()
        Dim oData As KeyValuePair(Of String, String)
        Dim oApp As clsGame

        If lstGames.SelectedItems.Count = 1 Then
            oData = lstGames.SelectedItems(0)
            oApp = DirectCast(GameData(oData.Key), clsGame)

            If mgrCommon.ShowMessage(frmGameManager_ConfirmGameDelete, oApp.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrMonitorList.DoListDelete(oApp.ID)
                mgrMonitorList.SyncMonitorLists(Settings,, False)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        ElseIf lstGames.SelectedItems.Count > 1 Then
            Dim sMonitorIDs As New List(Of String)

            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(GameData(oData.Key), clsGame)
                sMonitorIDs.Add(oApp.ID)
            Next

            If mgrCommon.ShowMessage(frmGameManager_ConfirmMultiGameDelete, sMonitorIDs.Count, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrMonitorList.DoListDeleteMulti(sMonitorIDs)
                mgrMonitorList.SyncMonitorLists(Settings,, False)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SwitchApp()
        If Not bIsLoading Then
            If lstGames.SelectedItems.Count = 1 Then
                eCurrentMode = eModes.View
                FillData()
                ModeChange()
                VerifyCleanFolder()
            ElseIf lstGames.SelectedItems.Count > 1 Then
                eCurrentMode = eModes.MultiSelect
                ModeChange()
            End If
        End If
    End Sub

    Private Function CoreValidatation(ByVal oApp As clsGame, ByVal bNewGame As Boolean) As Boolean
        Dim sCurrentID As String

        If bNewGame Then
            sCurrentID = String.Empty
        Else
            sCurrentID = CurrentGame.ID
        End If

        If txtName.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmGameManager_ErrorValidName, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        If (chkFolderSave.Checked = False And txtFileType.Text = String.Empty) And Not chkMonitorOnly.Checked Then
            mgrCommon.ShowMessage(frmGameManager_ErrorNoItems, MsgBoxStyle.Exclamation)
            btnInclude.Focus()
            Return False
        End If

        If mgrMonitorList.DoDuplicateListCheck(oApp.ID, , sCurrentID) Then
            mgrCommon.ShowMessage(frmGameManager_ErrorGameDupe, oApp.ID, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        If Not txtProcess.Text.Trim = String.Empty And oApp.IsRegEx Then
            If Not mgrCommon.IsRegExValid(oApp.ProcessName) Then
                If mgrCommon.ShowMessage(frmGameManager_ErrorRegExFailure, MsgBoxStyle.Exclamation, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    Process.Start(mgrCommon.FormatString(AppURL_RegExr, oApp.ProcessName))
                End If
                Return False
            End If
        End If

        If txtProcess.Text.Trim = String.Empty Then
            'Show one time warning
            If Not (oSettings.SuppressMessages And mgrSettings.eSuppressMessages.EmptyProcessWarning) = mgrSettings.eSuppressMessages.EmptyProcessWarning Then
                mgrCommon.ShowMessage(frmGameManager_WarningEmptyProcess, MsgBoxStyle.Information)
                oSettings.SuppressMessages = oSettings.SetMessageField(oSettings.SuppressMessages, mgrSettings.eSuppressMessages.EmptyProcessWarning)
                oSettings.SaveSettings()
            End If
        End If

        Return True
    End Function

    Private Sub MarkAsRestored()
        Dim oData As KeyValuePair(Of String, String)
        Dim oGameBackup As clsBackup
        Dim oMarkList As New List(Of clsBackup)
        Dim bWasUpdated As Boolean = False

        If lstGames.SelectedItems.Count > 0 Then
            For Each oData In lstGames.SelectedItems
                If oRemoteBackupData.Contains(oData.Key) Then
                    oGameBackup = DirectCast(oRemoteBackupData(oData.Key), clsBackup)
                    oMarkList.Add(oGameBackup)
                End If
            Next

            If oMarkList.Count = 1 Then
                If mgrCommon.ShowMessage(frmGameManager_ConfirmMark, oMarkList(0).Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    bWasUpdated = True
                    If mgrManifest.DoManifestCheck(oMarkList(0).MonitorID, mgrSQLite.Database.Local) Then
                        mgrManifest.DoManifestUpdateByMonitorID(oMarkList(0), mgrSQLite.Database.Local)
                    Else
                        mgrManifest.DoManifestAdd(oMarkList(0), mgrSQLite.Database.Local)
                    End If
                End If
            ElseIf oMarkList.Count > 1 Then
                If mgrCommon.ShowMessage(frmGameManager_ConfirmMultiMark, oMarkList.Count, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    bWasUpdated = True
                    For Each oGameBackup In oMarkList
                        If mgrManifest.DoManifestCheck(oGameBackup.MonitorID, mgrSQLite.Database.Local) Then
                            mgrManifest.DoManifestUpdateByMonitorID(oGameBackup, mgrSQLite.Database.Local)
                        Else
                            mgrManifest.DoManifestAdd(oGameBackup, mgrSQLite.Database.Local)
                        End If
                    Next
                End If
            Else
                mgrCommon.ShowMessage(frmGameManager_ErrorNoBackupData, MsgBoxStyle.Information)
            End If

            'Don't bother updating unless we actually did something
            If bWasUpdated Then
                LoadBackupData()
                If optAllGames.Checked Then
                    If lstGames.SelectedItems.Count = 1 Then
                        lstGames.SelectedIndex = lstGames.Items.IndexOf(New KeyValuePair(Of String, String)(CurrentGame.ID, CurrentGame.Name))
                        FillData()
                    End If
                Else
                    eCurrentMode = eModes.Disabled
                    ModeChange()
                    LoadData()
                End If
            End If
        End If
    End Sub

    Private Sub TriggerSelectedImportBackup()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim oBackup As New mgrBackup
        Dim sConfirm As String = frmGameManager_ConfirmBackupImport
        Dim sFile As String
        Dim sFiles As String()

        ImportBackupList.Clear()

        sFiles = mgrCommon.OpenMultiFileBrowser("GM_ImportBackup", frmGameManager_Choose7zImport, "7z",
                                          frmGameManager_7zBackup, sDefaultFolder, True)

        If sFiles.Length > 0 Then
            For Each sFile In sFiles
                If Not ImportBackupList.Contains(sFile) Then
                    ImportBackupList.Add(sFile, oCurrentGame)
                End If
            Next

            If sFiles.Length > 1 And Not CurrentGame.AppendTimeStamp Then
                mgrCommon.ShowMessage(frmGameManager_WarningImportBackupSaveMulti, MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If sFiles.Length = 1 And Not CurrentGame.AppendTimeStamp And mgrManifest.DoManifestCheck(CurrentGame.ID, mgrSQLite.Database.Remote) Then
                sConfirm = frmGameManager_ConfirmBackupImportOverwriteSingle
            End If

            If mgrCommon.ShowMessage(sConfirm, oCurrentGame.CroppedName, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Me.TriggerImportBackup = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub TriggerSelectedBackup(Optional ByVal bPrompt As Boolean = True)
        Dim oData As KeyValuePair(Of String, String)
        Dim sMsg As String = String.Empty
        Dim oGame As clsGame
        Dim bDoBackup As Boolean = False

        If lstGames.SelectedItems.Count > 0 Then
            BackupList.Clear()

            For Each oData In lstGames.SelectedItems
                oGame = DirectCast(GameData(oData.Key), clsGame)
                If Not oGame.MonitorOnly Then BackupList.Add(oGame)
            Next

            If BackupList.Count = 1 Then
                bDoBackup = True
                sMsg = mgrCommon.FormatString(frmGameManager_ConfirmBackup, BackupList(0).Name)
            ElseIf BackupList.Count > 1 Then
                bDoBackup = True
                sMsg = mgrCommon.FormatString(frmGameManager_ConfirmMultiBackup, BackupList.Count)
            Else
                mgrCommon.ShowMessage(frmGameManager_ErrorNoValidBackup, MsgBoxStyle.Information)
            End If

            If bDoBackup Then
                If bPrompt Then
                    If mgrCommon.ShowMessage(sMsg, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        TriggerBackup = True
                        Me.Close()
                    End If
                Else
                    TriggerBackup = True
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub TriggerSelectedRestore(Optional ByVal bPrompt As Boolean = True)
        Dim oData As KeyValuePair(Of String, String)
        Dim sMsg As String = String.Empty
        Dim oGame As clsGame
        Dim oBackup As clsBackup
        Dim bDoRestore As Boolean = False

        If lstGames.SelectedItems.Count > 0 Then
            RestoreList.Clear()

            For Each oData In lstGames.SelectedItems
                If oRemoteBackupData.Contains(oData.Key) Then
                    oGame = DirectCast(GameData(oData.Key), clsGame)
                    oBackup = DirectCast(oRemoteBackupData(oData.Key), clsBackup)
                    If Not oGame.MonitorOnly Then RestoreList.Add(oGame, oBackup)
                End If
            Next

            If RestoreList.Count = 1 Then
                bDoRestore = True
                oGame = New clsGame
                oBackup = New clsBackup
                For Each de As DictionaryEntry In RestoreList
                    oGame = DirectCast(de.Key, clsGame)
                    oBackup = DirectCast(de.Value, clsBackup)
                Next

                'Replace backup entry with currently selected backup item in case the user wants to restore an older backup.
                If Not (oBackup.DateUpdatedUnix = oCurrentBackupItem.DateUpdatedUnix) Then
                    RestoreList.Clear()
                    RestoreList.Add(oGame, oCurrentBackupItem)
                End If

                If Not mgrRestore.CheckManifest(oGame.Name) Then
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestoreAnyway, oGame.Name)
                Else
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestore, oGame.Name)
                End If

                If mgrConfigLinks.CheckForLinks(oGame.ID) And (oBackup.DateUpdatedUnix > oCurrentBackupItem.DateUpdatedUnix) Then
                    If mgrCommon.ShowMessage(mgrCommon.FormatString(frmGameManager_RestoreLinkWarning, oGame.CroppedName), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        IgnoreConfigLinks = True
                    Else
                        bDoRestore = False
                    End If
                End If
            ElseIf RestoreList.Count > 1 Then
                bDoRestore = True
                sMsg = mgrCommon.FormatString(frmGameManager_ConfirmMultiRestore, RestoreList.Count)
            Else
                mgrCommon.ShowMessage(frmGameManager_ErrorNoBackupData, MsgBoxStyle.Information)
            End If

            If bDoRestore Then
                If bPrompt Then
                    If mgrCommon.ShowMessage(sMsg, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        TriggerRestore = True
                        Me.Close()
                    End If
                Else
                    TriggerRestore = True
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub ImportGameListFile()
        Dim sLocation As String

        sLocation = mgrCommon.OpenFileBrowser("XML_Import", frmGameManager_ChooseImportXML, "xml", frmGameManager_XML, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)

        If sLocation <> String.Empty Then
            If mgrMonitorList.DoImport(sLocation, False) Then
                mgrMonitorList.SyncMonitorLists(Settings)
                LoadData()
                LoadBackupData()
            End If
        End If

    End Sub

    Private Sub ExportGameList()
        Dim sLocation As String

        sLocation = mgrCommon.SaveFileBrowser("XML_Export", frmGameManager_ChooseExportXML, "xml", frmGameManager_XML, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmGameManager_DefaultExportFileName & " " & Date.Now.ToString("dd-MMM-yyyy"))

        If sLocation <> String.Empty Then
            mgrMonitorList.ExportMonitorList(sLocation)
        End If

    End Sub

    Private Sub ImportOfficialGameList(ByVal sImportUrl As String)
        If mgrCommon.ShowMessage(frmGameManager_ConfirmOfficialImport, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(sImportUrl, True) Then
                mgrMonitorList.SyncMonitorLists(Settings)
                LoadData()
                LoadBackupData()
            End If
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Name = frmGameManager_FormName
        Me.Icon = GBM_Icon

        'Set Form text
        grpFilter.Text = frmGameManager_grpFilter
        grpConfig.Text = frmGameManager_grpConfig
        grpExtra.Text = frmGameManager_grpExtra
        grpStats.Text = frmGameManager_grpStats
        btnExport.Text = frmGameManager_btnExport
        btnImport.Text = frmGameManager_btnImport
        optCustom.Text = frmGameManager_optCustom
        optBackupData.Text = frmGameManager_optBackupData
        optPendingRestores.Text = frmGameManager_optPendingRestores
        optAllGames.Text = frmGameManager_optAllGames
        btnTags.Text = frmGameManager_btnTags
        chkEnabled.Text = frmGameManager_chkEnabled
        btnCancel.Text = frmGameManager_btnCancel
        chkMonitorOnly.Text = frmGameManager_chkMonitorOnly
        btnMarkAsRestored.Text = frmGameManager_btnMarkAsRestored
        btnRestore.Text = frmGameManager_btnRestore
        btnSave.Text = frmGameManager_btnSave
        lblRestorePath.Text = frmGameManager_lblRestorePath
        btnOpenRestorePath.Text = frmGameManager_btnOpenRestorePath
        btnOpenBackup.Text = frmGameManager_btnOpenBackup
        btnDeleteBackup.Text = frmGameManager_btnDeleteBackup
        lblBackupFile.Text = frmGameManager_lblBackupFile
        lblRemote.Text = frmGameManager_lblRemote
        lblLocalData.Text = frmGameManager_lblLocalData
        btnIconBrowse.Text = frmGameManager_btnIconBrowse
        lblVersion.Text = frmGameManager_lblVersion
        lblCompany.Text = frmGameManager_lblCompany
        lblIcon.Text = frmGameManager_lblIcon
        btnAppPathBrowse.Text = frmGameManager_btnAppPathBrowse
        lblGamePath.Text = frmGameManager_lblGamePath
        lblHours.Text = frmGameManager_lblHours
        btnExclude.Text = frmGameManager_btnExclude
        btnInclude.Text = frmGameManager_btnInclude
        btnSavePathBrowse.Text = frmGameManager_btnSavePathBrowse
        btnProcessBrowse.Text = frmGameManager_btnProcessBrowse
        lblSavePath.Text = frmGameManager_lblSavePath
        lblProcess.Text = frmGameManager_lblProcess
        lblParameter.Text = frmGameManager_lblParameter
        lblName.Text = frmGameManager_lblName
        chkTimeStamp.Text = frmGameManager_chkTimeStamp
        chkFolderSave.Text = frmGameManager_chkFolderSave
        chkCleanFolder.Text = frmGameManager_chkCleanFolder
        btnBackup.Text = frmGameManager_btnBackup
        btnClose.Text = frmGameManager_btnClose
        btnDelete.Text = frmGameManager_btnDelete
        btnAdd.Text = frmGameManager_btnAdd
        cmsOfficial.Text = frmGameManager_cmsOfficial
        cmsOfficialLinux.Text = frmGameManager_cmsOfficialLinux
        cmsOfficialWindows.Text = frmGameManager_cmsOfficialWindows
        cmsFile.Text = frmGameManager_cmsFile
        lblQuickFilter.Text = frmGameManager_lblQuickFilter
        lblLimit.Text = frmGameManager_lblLimit
        cmsDeleteOne.Text = frmGameManager_cmsDeleteOne
        cmsDeleteAll.Text = frmGameManager_cmsDeleteAll
        lblComments.Text = frmGameManager_lblComments
        chkRegEx.Text = frmGameManager_chkRegEx
        btnGameID.Text = frmGameManager_btnGameID
        btnImportBackup.Text = frmGameManager_btnImportBackup
        btnLink.Text = frmGameManager_btnLink
        lblOS.Text = frmGameManager_lblOS
        btnWineConfig.Text = frmGameManager_btnWineConfig
        cmsOpenBackupFile.Text = frmGameManager_cmsOpenBackupFile
        cmsOpenBackupFolder.Text = frmGameManager_cmsOpenBackupFolder
        cmsProcess.Text = frmGameManager_cmsProcess
        cmsConfiguration.Text = frmGameManager_cmsConfiguration

        'Init Combos
        Dim oComboItems As New List(Of KeyValuePair(Of Integer, String))

        'cboOS
        cboOS.ValueMember = "Key"
        cboOS.DisplayMember = "Value"

        oComboItems.Add(New KeyValuePair(Of Integer, String)(clsGame.eOS.Windows, App_WindowsOS))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(clsGame.eOS.Linux, App_LinuxOS))

        cboOS.DataSource = oComboItems

        If Not mgrCommon.IsUnix Then
            cboOS.Enabled = False
            btnWineConfig.Visible = False
        End If

        'Init Official Import Menu
        If mgrCommon.IsUnix Then
            cmsOfficial.Text = cmsOfficial.Text.TrimEnd(".")
            RemoveHandler cmsOfficial.Click, AddressOf cmsOfficialWindows_Click
        Else
            cmsOfficialLinux.Visible = False
            cmsOfficialWindows.Visible = False
        End If

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False

    End Sub

    Private Sub frmGameManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()

        If DisableExternalFunctions Then
            btnBackup.Visible = False
            btnRestore.Visible = False
            btnMarkAsRestored.Visible = False
            btnDeleteBackup.Visible = False
            btnOpenBackup.Visible = False
            btnOpenRestorePath.Visible = False
            btnImportBackup.Visible = False
        End If

        LoadBackupData()

        'Event will take care of initial load (on Windows)
        If PendingRestores Then
            optPendingRestores.Checked = True
        Else
            optAllGames.Checked = True
        End If

        AssignDirtyHandlers(grpConfig.Controls)
        AssignDirtyHandlers(grpExtra.Controls)
        AssignDirtyHandlers(grpStats.Controls)
        AssignDirtyHandlersMisc()

        'Mono doesn't fire events in the same way as .NET, so we'll to do this to get an initial load on Linux and prevent multiple loads in Windows.
        If mgrCommon.IsUnix Then
            LoadData(False)
            ModeChange()
        End If
    End Sub

    Private Sub lstGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstGames.SelectedIndexChanged
        SwitchApp()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddApp()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteApp()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        CancelEdit()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveApp()
    End Sub

    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        TriggerSelectedBackup()
    End Sub

    Private Sub frmGameManager_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveApp()
                Case MsgBoxResult.No
                    'Do Nothing
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub btnProcessBrowse_Click(sender As Object, e As EventArgs) Handles btnProcessBrowse.Click
        ProcessBrowse()
    End Sub

    Private Sub btnSavePathBrowse_Click(sender As Object, e As EventArgs) Handles btnSavePathBrowse.Click
        SavePathBrowse()
    End Sub

    Private Sub btnAppPathBrowse_Click(sender As Object, e As EventArgs) Handles btnAppPathBrowse.Click
        ProcessPathBrowse()
    End Sub

    Private Sub btnIconBrowse_Click(sender As Object, e As EventArgs) Handles btnIconBrowse.Click
        IconBrowse()
    End Sub

    Private Sub btnOpenBackup_Click(sender As Object, e As EventArgs) Handles btnOpenBackup.Click
        mgrCommon.OpenButtonSubMenu(cmsOpenBackup, btnOpenBackup)
    End Sub

    Private Sub cmsOpenBackupFile_Click(sender As Object, e As EventArgs) Handles cmsOpenBackupFile.Click
        OpenBackupFile()
    End Sub

    Private Sub cmsOpenBackupFolder_Click(sender As Object, e As EventArgs) Handles cmsOpenBackupFolder.Click
        OpenBackupFolder()
    End Sub

    Private Sub btnOpenRestorePath_Click(sender As Object, e As EventArgs) Handles btnOpenRestorePath.Click
        OpenRestorePath()
    End Sub

    Private Sub btnLink_Click(sender As Object, e As EventArgs) Handles btnLink.Click
        mgrCommon.OpenButtonSubMenu(cmsLink, btnLink)
    End Sub

    Private Sub btnTags_Click(sender As Object, e As EventArgs) Handles btnTags.Click
        OpenTags()
    End Sub

    Private Sub cmsLink_Click(sender As Object, e As EventArgs) Handles cmsConfiguration.Click
        OpenConfigLinks()
    End Sub

    Private Sub cmsProcess_Click(sender As Object, e As EventArgs) Handles cmsProcess.Click
        OpenProcesses()
    End Sub

    Private Sub btnWineConfig_Click(sender As Object, e As EventArgs) Handles btnWineConfig.Click
        OpenWineConfiguration()
    End Sub

    Private Sub btnDeleteBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteBackup.Click
        If cboRemoteBackup.Items.Count > 1 Then
            mgrCommon.OpenButtonSubMenu(cmsDeleteBackup, btnDeleteBackup)
        Else
            DeleteBackup()
        End If
    End Sub

    Private Sub cmsDeleteOne_Click(sender As Object, e As EventArgs) Handles cmsDeleteOne.Click
        DeleteBackup()
    End Sub

    Private Sub cmsDeleteAll_Click(sender As Object, e As EventArgs) Handles cmsDeleteAll.Click
        DeleteAllBackups()
    End Sub

    Private Sub btnMarkAsRestored_Click(sender As Object, e As EventArgs) Handles btnMarkAsRestored.Click
        MarkAsRestored()
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        TriggerSelectedRestore()
    End Sub

    Private Sub optGamesFilter_Click(sender As Object, e As EventArgs) Handles optPendingRestores.Click, optAllGames.Click, optBackupData.Click, optCustom.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        LoadData(False)
    End Sub

    Private Sub btnInclude_Click(sender As Object, e As EventArgs) Handles btnInclude.Click
        Dim sInclude As String = txtFileType.Text
        OpenBuilder(frmGameManager_Include, txtFileType)
        UpdateBuilderButtonLabel(txtFileType.Text, frmGameManager_IncludeShortcut, btnInclude, (sInclude <> txtFileType.Text))
    End Sub

    Private Sub btnExclude_Click(sender As Object, e As EventArgs) Handles btnExclude.Click
        Dim sExclude As String = txtExclude.Text
        OpenBuilder(frmGameManager_Exclude, txtExclude)
        UpdateBuilderButtonLabel(txtExclude.Text, frmGameManager_ExcludeShortcut, btnExclude, (sExclude <> txtExclude.Text))
    End Sub

    Private Sub btnImportBackup_Click(sender As Object, e As EventArgs) Handles btnImportBackup.Click
        TriggerSelectedImportBackup()
    End Sub

    Private Sub chkFolderSave_CheckedChanged(sender As Object, e As EventArgs) Handles chkFolderSave.CheckedChanged
        FolderSaveModeChange()
    End Sub

    Private Sub chkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles chkTimeStamp.CheckedChanged
        TimeStampModeChange()
    End Sub

    Private Sub cboRemoteBackup_Enter(sender As Object, e As EventArgs) Handles cboRemoteBackup.Enter, cboRemoteBackup.Click
        If Not oCurrentGame Is Nothing Then VerifyBackups(oCurrentGame)
    End Sub

    Private Sub cboRemoteBackup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRemoteBackup.SelectedIndexChanged
        If Not bIsLoading Then
            UpdateBackupInfo(DirectCast(cboRemoteBackup.SelectedItem, KeyValuePair(Of String, String)).Key)
        End If
    End Sub

    Private Sub cboOS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOS.SelectedIndexChanged
        If Not bIsLoading And Not eCurrentMode = eModes.Add Then
            HandleWineConfig()
        End If
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        mgrCommon.OpenButtonSubMenu(cmsImport, btnImport)
    End Sub

    Private Sub cmsOfficialWindows_Click(sender As Object, e As EventArgs) Handles cmsOfficialWindows.Click, cmsOfficial.Click
        'Show one time warning about Windows configs in Linux
        If mgrCommon.IsUnix Then
            If Not (oSettings.SuppressMessages And mgrSettings.eSuppressMessages.WinConfigsInLinux) = mgrSettings.eSuppressMessages.WinConfigsInLinux Then
                mgrCommon.ShowMessage(frmGameManager_WarningWinConfigsInLinux, MsgBoxStyle.Information)
                oSettings.SuppressMessages = oSettings.SetMessageField(oSettings.SuppressMessages, mgrSettings.eSuppressMessages.WinConfigsInLinux)
                oSettings.SaveSettings()
            End If
        End If

        ImportOfficialGameList(App_URLImport)
    End Sub

    Private Sub cmsOfficialLinux_Click(sender As Object, e As EventArgs) Handles cmsOfficialLinux.Click
        ImportOfficialGameList(App_URLImportLinux)
    End Sub

    Private Sub cmsFile_Click(sender As Object, e As EventArgs) Handles cmsFile.Click
        ImportGameListFile()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportGameList()
    End Sub

    Private Sub btnGameID_Click(sender As Object, e As EventArgs) Handles btnGameID.Click
        OpenGameIDEdit()
    End Sub

    Private Sub txtQuickFilter_TextChanged(sender As Object, e As EventArgs) Handles txtQuickFilter.TextChanged
        If Not eCurrentMode = eModes.Disabled Then
            eCurrentMode = eModes.Disabled
            ModeChange(True)
            lstGames.ClearSelected()
        End If

        If Not tmFilterTimer.Enabled Then
            lstGames.Enabled = False
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub txtSavePath_TextChanged(sender As Object, e As EventArgs) Handles txtSavePath.TextChanged
        ttFullPath.RemoveAll()
        RegistryModeChange()
        VerifyCleanFolder()
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        lstGames.DataSource = Nothing
        FormatAndFillList()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
        lstGames.Enabled = True
    End Sub

    Private Sub frmGameManager_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        'Load Last Played Game
        If Not LastPlayedGame Is Nothing Then
            lstGames.SelectedItem = New KeyValuePair(Of String, String)(LastPlayedGame.ID, LastPlayedGame.Name)
        End If

        txtQuickFilter.Focus()
    End Sub

    Private Sub chkMonitorOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkMonitorOnly.CheckedChanged
        MonitorOnlyModeChange()
    End Sub
End Class
