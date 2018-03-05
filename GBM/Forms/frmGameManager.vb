Imports GBM.My.Resources
Imports System.Collections.Specialized
Imports System.IO

Public Class frmGameManager

    Private oSettings As mgrSettings
    Private sBackupFolder As String
    Private bPendingRestores As Boolean = False
    Private oCurrentBackupItem As clsBackup
    Private oCurrentGame As clsGame
    Private oTagsToSave As New List(Of KeyValuePair(Of String, String))
    Private bDisableExternalFunctions As Boolean = False
    Private bTriggerBackup As Boolean = False
    Private bTriggerRestore As Boolean = False
    Private oBackupList As New List(Of clsGame)
    Private oRestoreList As New Hashtable
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
        ViewTemp = 6
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

    Private Function ConvertToRelativePath(ByVal sSavePath As String, ByVal sAppPath As String) As String
        Dim sPath As String = sSavePath

        'Determine a relative path if possible
        If sAppPath <> String.Empty And sSavePath <> String.Empty Then
            If Not mgrPath.IsAbsolute(sSavePath) Then
                sPath = mgrPath.DetermineRelativePath(sAppPath, sSavePath)
            End If
        End If

        Return sPath
    End Function

    Private Sub CheckManifestandUpdate(ByVal oOriginalApp As clsGame, ByVal oNewApp As clsGame)
        Dim oBackupItems As List(Of clsBackup)
        Dim sDirectory As String
        Dim sNewDirectory As String
        Dim sFileName As String
        Dim sNewFileName As String

        'If there is an ID change, check and update the manifest
        If oNewApp.ID <> oOriginalApp.ID Then
            'Local
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Local) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Local)
                'The local manifest will only have one entry per game, therefore this runs only once
                For Each oBackupItem As clsBackup In oBackupItems
                    'Rename Current Backup File & Folder
                    sFileName = BackupFolder & oBackupItem.FileName

                    'Rename Backup File
                    sNewFileName = Path.GetDirectoryName(sFileName) & Path.DirectorySeparatorChar & Path.GetFileName(sFileName).Replace(oOriginalApp.ID, oNewApp.ID)
                    If File.Exists(sFileName) And Not sFileName = sNewFileName Then
                        FileSystem.Rename(sFileName, sNewFileName)
                    End If

                    'Rename Directory
                    sDirectory = Path.GetDirectoryName(sFileName)
                    sNewDirectory = sDirectory.Replace(oOriginalApp.ID, oNewApp.ID)
                    If sDirectory <> sNewDirectory Then
                        If Directory.Exists(sDirectory) And Not sDirectory = sNewDirectory Then
                            FileSystem.Rename(sDirectory, sNewDirectory)
                        End If
                    End If

                    oBackupItem.MonitorID = oNewApp.ID
                    oBackupItem.FileName = oBackupItem.FileName.Replace(oOriginalApp.ID, oNewApp.ID)
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Local)
                Next
            End If

            'Remote
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Remote) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Remote)

                For Each oBackupItem As clsBackup In oBackupItems
                    oBackupItem.MonitorID = oNewApp.ID
                    oBackupItem.FileName = oBackupItem.FileName.Replace(oOriginalApp.ID, oNewApp.ID)
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Remote)
                Next
            End If
        End If
    End Sub

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
            Dim oTemporaryList As OrderedDictionary = mgrCommon.GenericClone(GameData)
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
            Dim oTemporaryList As OrderedDictionary = mgrCommon.GenericClone(GameData)
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
                                          frmGameManager_Executable, sDefaultFolder, False, False)

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

        sNewPath = mgrCommon.OpenFolderBrowser("GM_Process_Path", frmGameManager_ChooseExePath, sDefaultFolder, False, False)

        If sNewPath <> String.Empty Then txtAppPath.Text = sNewPath
    End Sub

    Private Sub SavePathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = txtSavePath.Text
        Dim sNewPath As String

        If txtSavePath.Text <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("GM_Save_Path", frmGameManager_ChooseSaveFolder, sDefaultFolder, False, False)

        If sNewPath <> String.Empty Then
            txtSavePath.Text = sNewPath
            txtSavePath.Text = ConvertToRelativePath(txtSavePath.Text, txtAppPath.Text)
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
                                              frmGameManager_Icon, sDefaultFolder, False, False)
        Else
            sNewPath = mgrCommon.OpenFileBrowser("GM_Icon", frmGameManager_ChooseCustomIcon, "png",
                                              "PNG", sDefaultFolder, False, False)
        End If

        If sNewPath <> String.Empty Then
            txtIcon.Text = sNewPath
            If IO.File.Exists(sNewPath) Then
                pbIcon.Image = Image.FromFile(sNewPath)
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
        Dim sFileName As String
        Dim oProcessStartInfo As ProcessStartInfo

        sFileName = BackupFolder & CurrentBackupItem.FileName

        If File.Exists(sFileName) Then
            oProcessStartInfo = New ProcessStartInfo
            oProcessStartInfo.FileName = sFileName
            oProcessStartInfo.UseShellExecute = True
            oProcessStartInfo.Verb = "open"
            Process.Start(oProcessStartInfo)
        Else
            mgrCommon.ShowMessage(frmGameManager_ErrorNoBackupExists, MsgBoxStyle.Exclamation)
        End If
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

        If Path.IsPathRooted(txtSavePath.Text) Then
            If Directory.Exists(txtSavePath.Text) Then
                sRoot = txtSavePath.Text
            End If
        Else
            If txtAppPath.Text <> String.Empty Then
                If Directory.Exists(txtAppPath.Text & Path.DirectorySeparatorChar & txtSavePath.Text) Then
                    sRoot = txtAppPath.Text & Path.DirectorySeparatorChar & txtSavePath.Text
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

        frm.ShowDialog()

        txtBox.Text = frm.BuilderString
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
                sProcess = CurrentGame.TrueProcess
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
        Dim oProcessStartInfo As ProcessStartInfo

        If CurrentBackupItem.AbsolutePath Then
            sPath = CurrentBackupItem.RestorePath
        Else
            If FindRestorePath() Then
                sPath = CurrentBackupItem.RelativeRestorePath
            End If
        End If

        If Directory.Exists(sPath) Then
            oProcessStartInfo = New ProcessStartInfo
            oProcessStartInfo.FileName = sPath
            oProcessStartInfo.UseShellExecute = True
            oProcessStartInfo.Verb = "open"
            Process.Start(oProcessStartInfo)
        Else
            mgrCommon.ShowMessage(frmGameManager_ErrorNoRestorePathExists, MsgBoxStyle.Exclamation)
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
            FillTagsbyList(frm.TagList)
        Else
            'Only update visible tags if one item is selected
            If lstGames.SelectedItems.Count = 1 Then FillTagsbyID(CurrentGame.ID)

            'If a tag filter is enabled, reload list to reflect changes
            If optCustom.Checked Then
                LoadData()
            End If

            'If the selected game(s) no longer match the filter, disable the form
            If lstGames.SelectedIndex = -1 Then eCurrentMode = eModes.Disabled
            ModeChange()
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
                lblBackupFileData.Text = frmGameManager_ErrorNoBackupExists
            End If

            lblRestorePathData.Text = CurrentBackupItem.RestorePath
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

            btnOpenBackupFile.Enabled = True
            btnOpenRestorePath.Enabled = True
            btnRestore.Enabled = True
            btnDeleteBackup.Enabled = True

            If File.Exists(sFileName) Then
                lblBackupFileData.Text = Path.GetFileName(CurrentBackupItem.FileName) & " (" & mgrCommon.FormatDiskSpace(mgrCommon.GetFileSize(sFileName)) & ")"
            Else
                lblBackupFileData.Text = frmGameManager_ErrorNoBackupExists
            End If

            lblRestorePathData.Text = CurrentBackupItem.RestorePath
        Else
            oComboItems.Add(New KeyValuePair(Of String, String)(String.Empty, frmGameManager_None))
            lblBackupFileData.Text = String.Empty
            lblRestorePathData.Text = String.Empty
            btnOpenBackupFile.Enabled = False
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

    End Sub

    Private Sub DeleteAllBackups()
        Dim oBackupData As List(Of clsBackup)
        Dim oBackup As clsBackup

        If mgrCommon.ShowMessage(frmGameManager_ConfirmBackupDeleteAll, CurrentGame.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            oBackupData = mgrManifest.DoManifestGetByMonitorID(CurrentGame.ID, mgrSQLite.Database.Remote)

            For Each oBackup In oBackupData
                'Delete the specific remote manifest entry
                mgrManifest.DoManifestDeletebyManifestID(oBackup, mgrSQLite.Database.Remote)
                'Delete referenced backup file from the backup folder
                mgrCommon.DeleteFile(BackupFolder & oBackup.FileName)
                'Check for sub-directory and delete if empty (we need to do this every pass just in case the user had a mix of settings at one point)
                mgrCommon.DeleteDirectoryByBackup(BackupFolder, oBackup)
            Next

            'Delete local manifest entry
            mgrManifest.DoManifestDeletebyMonitorID(CurrentBackupItem, mgrSQLite.Database.Local)

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

        'Core
        txtID.Text = oApp.ID
        txtName.Text = oApp.Name
        txtProcess.Text = oApp.TrueProcess
        chkRegEx.Checked = oApp.IsRegEx
        txtParameter.Text = oApp.Parameter
        txtSavePath.Text = oApp.Path
        txtFileType.Text = oApp.FileType
        txtExclude.Text = oApp.ExcludeList
        chkFolderSave.Checked = oApp.FolderSave
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

        'Extra
        txtAppPath.Text = oApp.ProcessPath
        txtCompany.Text = oApp.Company
        txtVersion.Text = oApp.Version
        txtIcon.Text = oApp.Icon

        FillTagsbyID(oData.Key)

        'Icon
        If IO.File.Exists(oApp.Icon) Then
            pbIcon.Image = Image.FromFile(oApp.Icon)
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

    Private Sub FillTagsbyID(ByVal sID As String)
        Dim hshTags As Hashtable
        Dim oTag As clsTag
        Dim sTags As String = String.Empty
        Dim cTrim() As Char = {",", " "}

        hshTags = mgrGameTags.GetTagsByGame(sID)

        For Each de As DictionaryEntry In hshTags
            oTag = DirectCast(de.Value, clsTag)
            sTags &= "#" & oTag.Name & ", "
        Next

        lblTags.Text = sTags.TrimEnd(cTrim)
    End Sub

    Private Sub FillTagsbyList(ByVal oList As List(Of KeyValuePair(Of String, String)))
        Dim sTags As String = String.Empty
        Dim cTrim() As Char = {",", " "}

        For Each kp As KeyValuePair(Of String, String) In oList
            sTags &= "#" & kp.Value & ", "
        Next

        lblTags.Text = sTags.TrimEnd(cTrim)
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
                DirectCast(ctl, ComboBox).DataSource = Nothing
            End If
        Next
    End Sub

    Private Sub ModeChange()
        IsLoading = True

        Select Case eCurrentMode
            Case eModes.Add
                oTagsToSave.Clear()
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
                btnDeleteBackup.Enabled = False
                btnOpenBackupFile.Enabled = False
                btnOpenRestorePath.Enabled = False
                chkEnabled.Checked = True
                chkMonitorOnly.Checked = False
                btnTags.Enabled = True
                lblTags.Text = String.Empty
                lblTags.Visible = True
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = False
                btnExport.Enabled = False
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
                btnDeleteBackup.Enabled = False
                btnOpenBackupFile.Enabled = False
                btnOpenRestorePath.Enabled = False
                btnTags.Enabled = True
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
                lblTags.Visible = True
                btnImport.Enabled = True
                btnExport.Enabled = True
            Case eModes.ViewTemp
                grpFilter.Enabled = True
                lstGames.Enabled = True
                lblQuickFilter.Enabled = True
                txtQuickFilter.Enabled = True
                grpConfig.Enabled = False
                chkEnabled.Enabled = False
                chkMonitorOnly.Enabled = False
                grpExtra.Enabled = False
                grpStats.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = False
                btnBackup.Enabled = False
                btnOpenRestorePath.Enabled = False
                btnTags.Enabled = False
                lblTags.Visible = False
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
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
                lblTags.Visible = False
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = True
                btnExport.Enabled = True
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
                lblTags.Visible = False
                btnImport.Enabled = True
                btnExport.Enabled = True
        End Select

        lstGames.Focus()

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
            lblSavePath.Enabled = False
            txtSavePath.Enabled = False
            btnSavePathBrowse.Enabled = False
            btnInclude.Enabled = False
            btnExclude.Enabled = False
        Else
            chkFolderSave.Enabled = True
            chkTimeStamp.Enabled = True
            lblSavePath.Enabled = True
            txtSavePath.Enabled = True
            btnSavePathBrowse.Enabled = True
            btnInclude.Enabled = True
            btnExclude.Enabled = True
        End If
        VerifyCleanFolder()
    End Sub

    Private Sub TimeStampModeChange()
        If chkTimeStamp.Checked Then
            nudLimit.Visible = True
            lblLimit.Visible = True
            nudLimit.Value = 5
        Else
            nudLimit.Visible = False
            nudLimit.Value = nudLimit.Minimum
            lblLimit.Visible = False
        End If
    End Sub

    Private Sub VerifyCleanFolder()
        If Not bIsLoading Then
            If (chkFolderSave.Checked = True And txtExclude.Text = String.Empty And txtSavePath.Text <> String.Empty) And Not chkMonitorOnly.Checked Then
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
        If Path.HasExtension(txtProcess.Text) Then
            If txtProcess.Text.ToLower.EndsWith(".exe") Then
                oApp.ProcessName = Path.GetFileNameWithoutExtension(txtProcess.Text)
            Else
                oApp.ProcessName = txtProcess.Text
            End If
        Else
            oApp.ProcessName = txtProcess.Text
        End If
        oApp.Parameter = txtParameter.Text
        oApp.Path = txtSavePath.Text
        'Only do a simple root check here in case the user doesn't really understand creating a proper configuration
        oApp.AbsolutePath = Path.IsPathRooted(txtSavePath.Text)
        oApp.FileType = txtFileType.Text
        oApp.ExcludeList = txtExclude.Text
        oApp.FolderSave = chkFolderSave.Checked
        oApp.CleanFolder = chkCleanFolder.Checked
        oApp.AppendTimeStamp = chkTimeStamp.Checked
        oApp.BackupLimit = nudLimit.Value
        oApp.Comments = txtComments.Text
        oApp.IsRegEx = chkRegEx.Checked
        oApp.Enabled = chkEnabled.Checked
        oApp.MonitorOnly = chkMonitorOnly.Checked
        oApp.ProcessPath = txtAppPath.Text
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
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oApp, False) Then
                    bSuccess = True
                    CheckManifestandUpdate(oCurrentGame, oApp)
                    mgrMonitorList.DoListUpdate(oApp, CurrentGame.ID)
                    eCurrentMode = eModes.View
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
            mgrMonitorList.SyncMonitorLists(Settings.SyncFields)
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
                mgrMonitorList.SyncMonitorLists(Settings.SyncFields)
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
                mgrMonitorList.SyncMonitorLists(Settings.SyncFields)
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

        If txtProcess.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmGameManager_ErrorValidProcess, MsgBoxStyle.Exclamation)
            txtProcess.Focus()
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

        If oApp.Parameter <> String.Empty Then
            If mgrMonitorList.DoDuplicateParameterCheck(oApp.ProcessName, oApp.Parameter, , sCurrentID) Then
                mgrCommon.ShowMessage(frmGameManager_ErrorProcessParameterDupe, MsgBoxStyle.Exclamation)
                Return False
            End If
        End If

        If oApp.IsRegEx Then
            If Not mgrCommon.IsRegExValid(oApp.ProcessName) Then
                If mgrCommon.ShowMessage(frmGameManager_ErrorRegExFailure, MsgBoxStyle.Exclamation, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    Process.Start(mgrCommon.FormatString(AppURL_RegExr, oApp.ProcessName))
                End If
                Return False
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
                If oRemoteBackupData.Contains(oData.Value) Then
                    oGameBackup = DirectCast(oRemoteBackupData(oData.Value), clsBackup)
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
                For Each de As DictionaryEntry In RestoreList
                    oGame = DirectCast(de.Key, clsGame)
                Next
                If Not mgrRestore.CheckManifest(oGame.Name) Then
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestoreAnyway, oGame.Name)
                Else
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestore, oGame.Name)
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
                mgrMonitorList.SyncMonitorLists(Settings.SyncFields)
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

    Private Sub ImportOfficialGameList()
        Dim sImportURL As String

        If mgrCommon.IsUnix Then
            sImportURL = App_URLImportLinux
        Else
            sImportURL = App_URLImport
        End If

        If mgrCommon.ShowMessage(frmGameManager_ConfirmOfficialImport, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(sImportURL, True) Then
                mgrMonitorList.SyncMonitorLists(Settings.SyncFields)
                LoadData()
                LoadBackupData()
            End If
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Name = frmGameManager_FormName

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
        btnOpenBackupFile.Text = frmGameManager_btnOpenBackupFile
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
        cmsFile.Text = frmGameManager_cmsFile
        lblQuickFilter.Text = frmGameManager_lblQuickFilter
        lblLimit.Text = frmGameManager_lblLimit
        cmsDeleteOne.Text = frmGameManager_cmsDeleteOne
        cmsDeleteAll.Text = frmGameManager_cmsDeleteAll
        lblComments.Text = frmGameManager_lblComments
        chkRegEx.Text = frmGameManager_chkRegEx
        btnGameID.Text = frmGameManager_btnGameID

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
            btnOpenBackupFile.Visible = False
            btnOpenRestorePath.Visible = False
        End If

        LoadBackupData()

        'Event will take care of initial load
        If PendingRestores Then
            optPendingRestores.Checked = True
        Else
            optAllGames.Checked = True
        End If

        AssignDirtyHandlers(grpConfig.Controls)
        AssignDirtyHandlers(grpExtra.Controls)
        AssignDirtyHandlers(grpStats.Controls)
        AssignDirtyHandlersMisc()

        LoadData(False)
        ModeChange()
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

    Private Sub btnOpenBackupFile_Click(sender As Object, e As EventArgs) Handles btnOpenBackupFile.Click
        OpenBackupFile()
    End Sub

    Private Sub btnOpenRestorePath_Click(sender As Object, e As EventArgs) Handles btnOpenRestorePath.Click
        OpenRestorePath()
    End Sub

    Private Sub btnTags_Click(sender As Object, e As EventArgs) Handles btnTags.Click
        OpenTags()
    End Sub

    Private Sub btnDeleteBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteBackup.Click
        If cboRemoteBackup.Items.Count > 1 Then
            cmsDeleteBackup.Show(btnDeleteBackup, New Drawing.Point(btnDeleteBackup.Size.Width - Math.Floor(btnDeleteBackup.Size.Width * 0.1), btnDeleteBackup.Size.Height - Math.Floor(btnDeleteBackup.Size.Height * 0.5)), ToolStripDropDownDirection.AboveRight)
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

    Private Sub chkFolderSave_CheckedChanged(sender As Object, e As EventArgs) Handles chkFolderSave.CheckedChanged
        FolderSaveModeChange()
    End Sub

    Private Sub chkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles chkTimeStamp.CheckedChanged
        TimeStampModeChange()
    End Sub

    Private Sub cboRemoteBackup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRemoteBackup.SelectedIndexChanged
        If Not bIsLoading Then
            UpdateBackupInfo(DirectCast(cboRemoteBackup.SelectedItem, KeyValuePair(Of String, String)).Key)
        End If
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        cmsImport.Show(btnImport, New Drawing.Point(btnImport.Size.Width - Math.Floor(btnImport.Size.Width * 0.1), btnImport.Size.Height - Math.Floor(btnImport.Size.Height * 0.5)), ToolStripDropDownDirection.AboveRight)
    End Sub

    Private Sub cmsOfficial_Click(sender As Object, e As EventArgs) Handles cmsOfficial.Click
        ImportOfficialGameList()
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
        If Not tmFilterTimer.Enabled Then
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub txtSavePath_TextChanged(sender As Object, e As EventArgs) Handles txtSavePath.TextChanged
        VerifyCleanFolder()
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        lstGames.DataSource = Nothing
        FormatAndFillList()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub frmGameManager_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtQuickFilter.Focus()
    End Sub

    Private Sub chkMonitorOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkMonitorOnly.CheckedChanged
        MonitorOnlyModeChange()
    End Sub
End Class
