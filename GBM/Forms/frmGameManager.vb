Imports GBM.My.Resources
Imports System.Collections.Specialized
Imports System.IO

Public Class frmGameManager
    Private Enum eModes As Integer
        View = 1
        Edit = 2
        Add = 3
        Disabled = 4
        MultiSelect = 5
    End Enum

    Private sBackupFolder As String
    Private oTagsToSave As New List(Of KeyValuePair(Of String, String))
    Private oProcessesToSave As New List(Of KeyValuePair(Of String, String))
    Private oConfigLinksToSave As New List(Of KeyValuePair(Of String, String))
    Private oLocalBackupData As SortedList
    Private oRemoteBackupData As SortedList
    Private oCurrentIncludeTagFilters As New List(Of clsTag)
    Private oCurrentExcludeTagFilters As New List(Of clsTag)
    Private oCurrentFilters As New List(Of clsGameFilter)
    Private eCurrentFilter As frmFilter.eFilterType = frmFilter.eFilterType.BaseFilter
    Private bCurrentAndOperator As Boolean = False
    Private bCurrentSortAsc As Boolean = True
    Private sCurrentSortField As String = "Name"
    Private WithEvents tmFilterTimer As Timer
    Private eCurrentMode As eModes = eModes.Disabled
    Private eLastMode As eModes = eModes.Disabled

    Private Property GameData As OrderedDictionary
    Private Property IsDirty As Boolean = False
    Private Property IsLoading As Boolean = False
    Private Property InitialLoad As Boolean = True
    Private Property BackupFolder As String
        Get
            Return mgrSettings.BackupFolder & Path.DirectorySeparatorChar
        End Get
        Set(value As String)
            sBackupFolder = value
        End Set
    End Property

    Public Property PendingRestores As Boolean = False
    Public Property CurrentBackupItem As clsBackup
    Public Property OpenToGame As clsGame
    Public Property CurrentGame As clsGame
    Public Property DisableExternalFunctions As Boolean = False
    Public Property TriggerBackup As Boolean = False
    Public Property TriggerRestore As Boolean = False
    Public Property TriggerImportBackup As Boolean = False
    Public Property NoRestoreQueue As Boolean = False
    Public Property BackupList As List(Of clsGame) = New List(Of clsGame)
    Public Property RestoreList As Hashtable = New Hashtable
    Public Property ImportBackupGame As clsGame
    Public Property ImportBackupList As String()

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
            If Not mgrSettings.ShowResolvedPaths Then
                sPath = mgrPath.ReverseSpecialPaths(sPath)
            End If
        End If

        Return sPath
    End Function

    Private Sub HandleWineConfig()
        If mgrCommon.IsUnix And cboOS.SelectedValue = clsGame.eOS.Windows And Not eCurrentMode = eModes.Add Then
            cmsWineConfig.Enabled = True
        Else
            cmsWineConfig.Enabled = False
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

        'Handle File Changes
        If (bUseGameID And (oNewApp.ID <> oOriginalApp.ID)) Or (Not bUseGameID And (oNewApp.FileSafeName <> oOriginalApp.FileSafeName)) Then
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
                Directory.CreateDirectory(sNewDirectory)

                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Remote)

                'Check for existing files
                For Each oBackupItem As clsBackup In oBackupItems
                    sFileName = BackupFolder & oBackupItem.FileName
                    sNewFileName = Path.GetDirectoryName(sFileName) & Path.DirectorySeparatorChar & Path.GetFileName(sFileName).Replace(sOriginalAppItem, sNewAppItem)
                    If File.Exists(sNewFileName) Then
                        If mgrCommon.ShowMessage(frmGameManager_ErrorRenameFilesExist, sNewAppItem, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            Return False
                        End If
                        Exit For
                    End If
                Next

                'We need to copy the files, then delete the original to work around file locking issues with cloud clients.               
                For Each oBackupItem As clsBackup In oBackupItems
                    sFileName = BackupFolder & oBackupItem.FileName
                    sNewFileName = sNewDirectory & Path.DirectorySeparatorChar & Path.GetFileName(sFileName).Replace(sOriginalAppItem, sNewAppItem)
                    If File.Exists(sFileName) And Not sFileName = sNewFileName Then
                        'Copy the file using the new name, then delete the old file when successful.
                        If mgrCommon.CopyFile(sFileName, sNewFileName, True) Then
                            mgrCommon.DeleteFile(sFileName)
                            oBackupItem.FileName = oBackupItem.FileName.Replace(sOriginalAppItem, sNewAppItem)
                            mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Remote)
                        End If
                    End If
                Next

                'Delete the old folder if it's empty
                If Directory.Exists(sDirectory) And Not sDirectory = sNewDirectory Then
                    mgrCommon.DeleteEmptyDirectory(sDirectory)
                End If
            End If

            'Local
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Local) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Local)
                'The local manifest will only have one entry per game, therefore this runs only once
                For Each oBackupItem As clsBackup In oBackupItems
                    oBackupItem.FileName = oBackupItem.FileName.Replace(sOriginalAppItem, sNewAppItem)
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Local)
                Next
            End If
        End If

        'Handle ID Change
        If oNewApp.ID <> oOriginalApp.ID Then
            'Local
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Local) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Local)
                'The local manifest will only have one entry per game, therefore this runs only once
                For Each oBackupItem As clsBackup In oBackupItems
                    oBackupItem.MonitorID = oNewApp.ID
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Local)
                Next
            End If

            'Remote
            If mgrManifest.DoManifestCheck(oOriginalApp.ID, mgrSQLite.Database.Remote) Then
                oBackupItems = mgrManifest.DoManifestGetByMonitorID(oOriginalApp.ID, mgrSQLite.Database.Remote)
                For Each oBackupItem As clsBackup In oBackupItems
                    oBackupItem.MonitorID = oNewApp.ID
                    mgrManifest.DoManifestUpdateByManifestID(oBackupItem, mgrSQLite.Database.Remote)
                Next
            End If
        End If

        Return True
    End Function

    Private Sub LoadData(Optional ByVal bRetainFilter As Boolean = True)
        Dim oRestoreData As New SortedList
        Dim oGame As clsGame
        Dim frm As frmFilter

        If cboFilters.SelectedValue = 3 Then
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

        GameData = mgrMonitorList.ReadFilteredList(oCurrentIncludeTagFilters, oCurrentExcludeTagFilters, oCurrentFilters, eCurrentFilter, bCurrentAndOperator, bCurrentSortAsc, sCurrentSortField, txtSearch.Text)

        If cboFilters.SelectedValue = 2 Then
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
        ElseIf cboFilters.SelectedValue = 1 Then
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

        FormatAndFillList()
    End Sub

    Private Sub ProcessBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = mgrPath.ReplaceSpecialPaths(txtAppPath.Text)
        Dim sNewPath As String
        Dim oExtensions As New SortedList

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        oExtensions.Add(frmGameManager_Executable, "*.exe")
        sNewPath = mgrCommon.OpenFileBrowser("GM_Process", frmGameManager_ChooseExe, oExtensions, 1, sDefaultFolder, False)

        If sNewPath <> String.Empty Then
            txtAppPath.Text = Path.GetDirectoryName(sNewPath)
            txtProcess.Text = Path.GetFileName(sNewPath)
        End If

    End Sub

    Private Sub ProcessPathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = mgrPath.ReplaceSpecialPaths(txtAppPath.Text)
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
        Dim sCurrentPath As String = mgrPath.ReplaceSpecialPaths(txtAppPath.Text)
        Dim sNewPath As String
        Dim oExtensions As New SortedList

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        'Unix Handler
        If Not mgrCommon.IsUnix Then
            oExtensions.Add(frmGameManager_Executable, "*.exe")
            oExtensions.Add(frmGameManager_Icon, "*.ico")
            oExtensions.Add(frmGameManager_Image, "*.bmp;*.gif;*.jpg;*.png;*.tif")
            sNewPath = mgrCommon.OpenFileBrowser("GM_Icon", frmGameManager_ChooseCustomIcon, oExtensions, 2, sDefaultFolder, False)
        Else
            oExtensions.Add(frmGameManager_Image, "*.gif;*.jpg;*.png;*.tif")
            sNewPath = mgrCommon.OpenFileBrowser("GM_Icon", frmGameManager_ChooseCustomIcon, oExtensions, 1, sDefaultFolder, False)
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
            Case MsgBoxResult.No
                IsDirty = False
        End Select

        Return oResult

    End Function

    Private Sub FormatAndFillList()
        IsLoading = True

        Dim oApp As clsGame
        Dim oData As KeyValuePair(Of String, String)
        Dim oList As New List(Of KeyValuePair(Of String, String))

        For Each de As DictionaryEntry In GameData
            oApp = DirectCast(de.Value, clsGame)
            oData = New KeyValuePair(Of String, String)(oApp.ID, oApp.Name)
            oList.Add(oData)
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

        'Handle Mode
        If CurrentGame Is Nothing Then
            eCurrentMode = eModes.Disabled
            ModeChange()
        Else
            lstGames.SelectedItem = New KeyValuePair(Of String, String)(CurrentGame.ID, CurrentGame.Name)
            If lstGames.SelectedIndex = -1 And Not eCurrentMode = eModes.Disabled Then
                eCurrentMode = eModes.Disabled
                ModeChange()
            Else
                SwitchApp()
            End If
        End If

        'Automatically select the game on a single filter match
        If Not txtSearch.Text = String.Empty Then
            If lstGames.Items.Count = 1 Then
                lstGames.SelectedIndex = 0
                SwitchApp()
            End If
        End If
    End Sub

    Private Sub OpenGameIDEdit()
        Dim sCurrentID As String
        Dim sNewID As String

        If txtID.Text = String.Empty Then
            txtID.Text = Guid.NewGuid.ToString
        End If

        sCurrentID = txtID.Text

        sNewID = InputBox(frmGameManager_GameIDEditInfo, frmGameManager_GameIDEditTitle, sCurrentID).Trim

        If sNewID <> String.Empty And sCurrentID <> sNewID Then
            txtID.Text = sNewID
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
            btn.Text = sLabel
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
        Dim sPath As String = String.Empty
        Dim sAppPath As String = mgrPath.ValidatePath(txtAppPath.Text)

        'Use the wine save path when we are in Linux working with a Windows configuration
        If mgrCommon.IsUnix And cboOS.SelectedValue = clsGame.eOS.Windows Then
            Dim oWineData As clsWineData = mgrWineData.DoWineDataGetbyID(CurrentGame.ID)
            sPath = mgrPath.ValidatePath(oWineData.SavePath)
        End If

        If sPath = String.Empty Then
            sPath = mgrPath.ValidatePath(txtSavePath.Text)
        End If

        If Not mgrSettings.ShowResolvedPaths Then
            sPath = mgrPath.ReplaceSpecialPaths(sPath)
            sAppPath = mgrPath.ReplaceSpecialPaths(sAppPath)
        End If

        If Path.IsPathRooted(sPath) Then
            If Directory.Exists(sPath) Then
                sRoot = sPath
            End If
        Else
            If sAppPath <> String.Empty Then
                If Directory.Exists(sAppPath & Path.DirectorySeparatorChar & sPath) Then
                    sRoot = sAppPath & Path.DirectorySeparatorChar & sPath
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
        ModeChangeHandler()
    End Sub

    Private Function FindRestorePath() As Boolean
        Dim sProcess As String
        Dim sRestorePath As String
        Dim bNoAuto As Boolean

        If Not CurrentBackupItem.AbsolutePath Then
            If CurrentGame.ProcessPath <> String.Empty Then
                CurrentBackupItem.RelativeRestorePath = CurrentGame.ProcessPath & Path.DirectorySeparatorChar & CurrentBackupItem.Path
            Else
                sProcess = CurrentGame.ProcessName
                If mgrCommon.IsProcessNotSearchable(CurrentGame) Then bNoAuto = True
                sRestorePath = mgrPath.ProcessPathSearch(CurrentBackupItem.Name, sProcess, mgrCommon.FormatString(frmGameManager_ErrorPathNotSet, CurrentBackupItem.Name), bNoAuto)

                If sRestorePath <> String.Empty Then
                    CurrentBackupItem.RelativeRestorePath = sRestorePath & Path.DirectorySeparatorChar & CurrentBackupItem.Path
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
            sPath = CurrentBackupItem.Path
        Else
            If FindRestorePath() Then
                sPath = CurrentBackupItem.RelativeRestorePath
            End If
        End If

        mgrCommon.OpenInOS(sPath, frmGameManager_ErrorNoRestorePathExists)
    End Sub

    Private Function GetSelectedGames() As List(Of String)
        Dim sMonitorIDs As New List(Of String)
        Dim oApp As clsGame

        For Each oData In lstGames.SelectedItems
            oApp = DirectCast(GameData(oData.Key), clsGame)
            sMonitorIDs.Add(oApp.ID)
        Next

        Return sMonitorIDs
    End Function

    Private Sub OpenProcesses()
        Dim frm As New frmGameProcesses
        Dim sMonitorIDs As New List(Of String)

        'Show Intro Tip
        If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.LinkProcessTip) = mgrSettings.eSuppressMessages.LinkProcessTip Then
            mgrCommon.ShowMessage(frmGameManager_TipLinkProcess, MsgBoxStyle.Information)
            mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.LinkProcessTip)
            mgrSettings.SaveSettings()
        End If

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDs.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.ProcessList = oProcessesToSave
        Else
            sMonitorIDs = GetSelectedGames()
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDs
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oProcessesToSave = frm.ProcessList
        End If
    End Sub

    Private Sub HandleTags(ByVal sTags As String)
        If sTags = String.Empty Then
            lblGameTags.Text = frmGameManager_lblGameTags
            lblGameTags.LinkBehavior = LinkBehavior.SystemDefault
        Else
            lblGameTags.Text = sTags
            lblGameTags.LinkBehavior = LinkBehavior.HoverUnderline
        End If
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmGameTags
        Dim sMonitorIDs As New List(Of String)
        Dim bSingleSelected As Boolean = False
        Dim sTags As String

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDs.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.TagList = oTagsToSave
        Else
            sMonitorIDs = GetSelectedGames()
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDs
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oTagsToSave = frm.TagList
            sTags = mgrGameTags.PrintTagsbyList(frm.TagList)
            HandleTags(sTags)
        Else
            'Only update visible tags if one item is selected
            If lstGames.SelectedItems.Count = 1 Then
                sTags = mgrGameTags.PrintTagsbyID(CurrentGame.ID)
                HandleTags(sTags)
            End If

            'If a tag filter is enabled, reload list to reflect changes
            If cboFilters.SelectedValue = 3 And Not IsDirty Then
                If lstGames.SelectedItems.Count = 1 Then bSingleSelected = True
                LoadData()
                If bSingleSelected Then lstGames.SelectedItem = New KeyValuePair(Of String, String)(CurrentGame.ID, CurrentGame.Name)
            End If

            'If the selected game(s) no longer match the filter, disable the form
            If lstGames.SelectedIndex = -1 Then eCurrentMode = eModes.Disabled
            ModeChange()
        End If
    End Sub

    Private Sub OpenConfigLinks()
        Dim frm As New frmConfigLinks
        Dim sMonitorIDs As New List(Of String)

        'Show Intro Tip
        If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.LinkConfigTip) = mgrSettings.eSuppressMessages.LinkConfigTip Then
            mgrCommon.ShowMessage(frmGameManager_TipLinkConfiguration, MsgBoxStyle.Information)
            mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.LinkConfigTip)
            mgrSettings.SaveSettings()
        End If

        If eCurrentMode = eModes.Add Then
            'Use a dummy ID
            sMonitorIDs.Add(Guid.NewGuid.ToString)
            frm.GameName = txtName.Text
            frm.NewMode = True
            frm.ConfigLinkList = oConfigLinksToSave
        Else
            sMonitorIDs = GetSelectedGames()
            frm.GameName = CurrentGame.Name
            frm.NewMode = False
        End If

        frm.IDList = sMonitorIDs
        frm.ShowDialog()

        If eCurrentMode = eModes.Add Then
            oConfigLinksToSave = frm.ConfigLinkList
        End If
    End Sub

    Public Sub OpenWineConfiguration()
        Dim frm As New frmWineConfiguration
        frm.MonitorID = CurrentGame.ID
        frm.GameName = CurrentGame.CroppedName
        frm.ShowDialog()
    End Sub

    Private Sub OpenLauncherConfig()
        Dim frm As New frmLaunchConfiguration
        frm.Game = CurrentGame
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
            lblRestorePathData.Text = oApp.ProcessPath & Path.DirectorySeparatorChar & CurrentBackupItem.Path
        Else
            If mgrSettings.ShowResolvedPaths Then
                lblRestorePathData.Text = CurrentBackupItem.Path
                sttRestorePath = CurrentBackupItem.TruePath
            Else
                lblRestorePathData.Text = CurrentBackupItem.TruePath
                sttRestorePath = CurrentBackupItem.Path
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

            lblBackupFileData.Enabled = True
            btnOpenBackupFolder.Enabled = True
            lblRestorePathData.Enabled = True
            btnRestore.Enabled = True
            cmsDeleteOne.Enabled = True
            cmsDeleteAll.Enabled = True

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
            lblBackupFileData.Enabled = False
            btnOpenBackupFolder.Enabled = False
            lblRestorePathData.Enabled = False
            btnRestore.Enabled = False
            cmsDeleteOne.Enabled = False
            cmsDeleteAll.Enabled = False
        End If

        cboRemoteBackup.DataSource = oComboItems

        If cboRemoteBackup.Items.Count > 1 Then
            cmsDeleteAll.Enabled = True
        Else
            cmsDeleteAll.Enabled = False
        End If

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

        If cmsMonitorOnly.Checked Then
            cmsImportData.Enabled = False
        Else
            cmsImportData.Enabled = True
        End If

        If mgrPath.IsSupportedRegistryPath(oApp.TruePath) Then
            cmsImportData.Enabled = False
            lblBackupFileData.Enabled = False
            btnOpenBackupFolder.Enabled = False
            lblRestorePathData.Enabled = False
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
            GetBackupInfo(CurrentGame)
        End If
    End Sub

    Private Sub DeleteBackup()
        Dim sMessage As String
        Dim oDiffChildren As New List(Of clsBackup)

        If CurrentBackupItem.IsDifferentialParent Then
            oDiffChildren = mgrManifest.DoManfiestGetDifferentialChildren(CurrentBackupItem, mgrSQLite.Database.Remote)
            sMessage = frmGameManager_ConfirmBackupDeleteDiffParent
        Else
            sMessage = frmGameManager_ConfirmBackupDelete
        End If

        If mgrCommon.ShowMessage(sMessage, Path.GetFileName(CurrentBackupItem.FileName), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            'Delete the specific remote manifest entry
            mgrManifest.DoManifestDeleteByManifestID(CurrentBackupItem, mgrSQLite.Database.Remote)

            'If a remote manifest entry no longer exists for this game, delete the local entry
            If Not mgrManifest.DoManifestCheck(CurrentBackupItem.MonitorID, mgrSQLite.Database.Remote) Then
                mgrManifest.DoManifestDeleteByMonitorID(CurrentBackupItem, mgrSQLite.Database.Local)
            End If

            'Delete referenced backup file from the backup folder
            mgrCommon.DeleteFile(BackupFolder & CurrentBackupItem.FileName)

            'Delete any differential backups that relied upon this file
            For Each oDiffChild As clsBackup In oDiffChildren
                mgrCommon.DeleteFile(BackupFolder & oDiffChild.FileName)
            Next

            'Check for sub-directory and delete if empty
            mgrCommon.DeleteDirectoryByBackup(BackupFolder, CurrentBackupItem)

            LoadBackupData()
            GetBackupInfo(CurrentGame)
        End If
    End Sub

    Private Sub FillData()
        IsLoading = True

        Dim oData As KeyValuePair(Of String, String) = lstGames.SelectedItems(0)
        Dim oApp As clsGame = DirectCast(GameData(oData.Key), clsGame)
        Dim sCachedIcon As String = mgrCommon.GetCachedIconPath(oApp.ID)
        Dim sTags As String = mgrGameTags.PrintTagsbyID(oApp.ID)
        Dim sttPath As String

        'Core
        txtID.Text = oApp.ID
        txtName.Text = oApp.Name
        txtProcess.Text = oApp.ProcessName
        cmsRegEx.Checked = oApp.IsRegEx
        cmsUseWindowTitle.Checked = oApp.UseWindowTitle
        txtParameter.Text = oApp.Parameter
        cboOS.SelectedValue = CInt(oApp.OS)
        If mgrSettings.ShowResolvedPaths Then
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
        chkDifferentialBackup.Checked = oApp.Differential
        nudInterval.Value = oApp.DiffInterval
        txtComments.Text = oApp.Comments
        cmsEnabled.Checked = oApp.Enabled
        cmsMonitorOnly.Checked = oApp.MonitorOnly

        'Update Buttons
        UpdateBuilderButtonLabel(oApp.FileType, frmGameManager_IncludeShortcut, btnInclude, False)
        UpdateBuilderButtonLabel(oApp.ExcludeList, frmGameManager_ExcludeShortcut, btnExclude, False)
        HandleWineConfig()

        'Extra
        If mgrSettings.ShowResolvedPaths Then
            txtAppPath.Text = oApp.ProcessPath
            ttFullPath.SetToolTip(txtAppPath, oApp.TrueProcessPath)
            txtIcon.Text = oApp.Icon
            ttFullPath.SetToolTip(txtIcon, oApp.TrueIcon)
        Else
            txtAppPath.Text = oApp.TrueProcessPath
            ttFullPath.SetToolTip(txtAppPath, oApp.ProcessPath)
            txtIcon.Text = oApp.TrueIcon
            ttFullPath.SetToolTip(txtIcon, oApp.Icon)
        End If
        txtCompany.Text = oApp.Company
        txtVersion.Text = oApp.Version

        HandleTags(sTags)

        'Icon
        If IO.File.Exists(oApp.Icon) Then
            pbIcon.Image = mgrCommon.SafeIconFromFile(oApp.Icon)
        ElseIf IO.File.Exists(sCachedIcon) Then
            pbIcon.Image = mgrCommon.SafeIconFromFile(sCachedIcon)
        Else
            pbIcon.Image = Multi_Unknown
        End If

        'Stats
        nudHours.Value = oApp.Hours

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
            ElseIf TypeOf ctl Is ComboBox Then
                AddHandler DirectCast(ctl, ComboBox).SelectedValueChanged, AddressOf DirtyCheck_ValueChanged
            End If
        Next
    End Sub

    Private Sub ToggleControls(ByVal oCtls As GroupBox.ControlCollection, ByVal bEnabled As Boolean)
        For Each ctl As Control In oCtls
            ctl.Enabled = bEnabled
        Next
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

    Private Sub ModeChange()
        IsLoading = True

        Select Case eLastMode
            Case eModes.MultiSelect
                ToggleControls(grpCoreConfig.Controls, True)
                ToggleControls(grpGameInfo.Controls, True)
                ToggleControls(grpBackupInfo.Controls, True)
        End Select

        Select Case eCurrentMode
            Case eModes.Add
                eLastMode = eModes.Add
                tabGameManager.SelectTab(0)
                oTagsToSave.Clear()
                oProcessesToSave.Clear()
                oConfigLinksToSave.Clear()
                lblFilters.Enabled = False
                cboFilters.Enabled = False
                lstGames.Enabled = False
                lblSearch.Enabled = False
                txtSearch.Enabled = False
                tbConfig.Enabled = True
                cmsMonitorOnly.Enabled = True
                tbGameInfo.Enabled = True
                tbBackupInfo.Enabled = True
                WipeControls(grpCoreConfig.Controls)
                WipeControls(grpGameInfo.Controls)
                WipeControls(grpBackupInfo.Controls)
                cmsRegEx.Checked = False
                cmsUseWindowTitle.Checked = False
                chkCleanFolder.Enabled = False
                pbIcon.Image = Multi_Unknown
                cmsEnabled.Enabled = True
                cmsMonitorOnly.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                btnBackup.Enabled = False
                btnAdvanced.Enabled = False
                tbBackupInfo.Enabled = False
                cmsEnabled.Checked = True
                cmsMonitorOnly.Checked = False
                chkRecurseSubFolders.Checked = True
                lblGameTags.Text = frmGameManager_lblGameTags
                lblGameTags.LinkBehavior = LinkBehavior.SystemDefault
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = False
                btnExport.Enabled = False
                cboOS.SelectedValue = CInt(mgrCommon.GetCurrentOS)
                ModeChangeHandler()
            Case eModes.Edit
                eLastMode = eModes.Edit
                lblFilters.Enabled = False
                cboFilters.Enabled = False
                lstGames.Enabled = False
                lblSearch.Enabled = False
                txtSearch.Enabled = False
                tbConfig.Enabled = True
                cmsEnabled.Enabled = True
                cmsMonitorOnly.Enabled = True
                tbGameInfo.Enabled = True
                tbBackupInfo.Enabled = False
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                btnAdvanced.Enabled = False
                btnImport.Enabled = False
                btnExport.Enabled = False
            Case eModes.View
                eLastMode = eModes.View
                GetBackupInfo(CurrentGame)
                lblFilters.Enabled = True
                cboFilters.Enabled = True
                lstGames.Enabled = True
                lblSearch.Enabled = True
                txtSearch.Enabled = True
                tbConfig.Enabled = True
                cmsEnabled.Enabled = True
                cmsMonitorOnly.Enabled = True
                tbGameInfo.Enabled = True
                tbBackupInfo.Enabled = True
                btnGameID.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
                btnBackup.Enabled = True
                btnAdvanced.Enabled = True
                btnImport.Enabled = True
                btnExport.Enabled = True
                ModeChangeHandler()
            Case eModes.Disabled
                eLastMode = eModes.Disabled
                lblFilters.Enabled = True
                cboFilters.Enabled = True
                lstGames.Enabled = True
                lblSearch.Enabled = True
                txtSearch.Enabled = True
                WipeControls(grpCoreConfig.Controls)
                WipeControls(grpGameInfo.Controls)
                WipeControls(grpBackupInfo.Controls)
                pbIcon.Image = Multi_Unknown
                btnSave.Enabled = False
                btnCancel.Enabled = False
                tbConfig.Enabled = False
                cmsEnabled.Enabled = False
                cmsMonitorOnly.Enabled = False
                tbGameInfo.Enabled = False
                tbBackupInfo.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
                btnAdvanced.Enabled = False
                lblGameTags.Text = frmGameManager_lblGameTags
                lblGameTags.LinkBehavior = LinkBehavior.HoverUnderline
                btnInclude.Text = frmGameManager_btnInclude
                btnExclude.Text = frmGameManager_btnExclude
                btnImport.Enabled = True
                btnExport.Enabled = True
                cboOS.SelectedValue = CInt(mgrCommon.GetCurrentOS)
                UpdateGenericButtonLabel(frmGameManager_IncludeShortcut, btnInclude, False)
                UpdateGenericButtonLabel(frmGameManager_ExcludeShortcut, btnExclude, False)
            Case eModes.MultiSelect
                eLastMode = eModes.MultiSelect
                lstGames.Enabled = True
                lblSearch.Enabled = False
                txtSearch.Enabled = False
                WipeControls(grpCoreConfig.Controls)
                WipeControls(grpGameInfo.Controls)
                WipeControls(grpBackupInfo.Controls)
                pbIcon.Image = Multi_Unknown
                btnSave.Enabled = True
                btnCancel.Enabled = False
                cmsMonitorOnly.Checked = False
                cmsEnabled.Checked = False
                ToggleControls(grpCoreConfig.Controls, False)
                ToggleControls(grpGameInfo.Controls, False)
                ToggleControls(grpBackupInfo.Controls, False)
                btnLinks.Enabled = True
                btnMonitorOptions.Enabled = True
                cmsMonitorOnly.Enabled = True
                cmsEnabled.Enabled = True
                btnMarkAsRestored.Enabled = True
                lblGameTags.Enabled = True
                HandleTags(mgrGameTags.PrintTagsbyIDMulti(GetSelectedGames))
                btnAdd.Enabled = False
                btnDelete.Enabled = True
                btnBackup.Enabled = True
                btnRestore.Enabled = True
                cmsDeleteAll.Enabled = False
                cmsDeleteOne.Enabled = False
                cmsImportData.Enabled = False
                btnAdvanced.Enabled = False
                btnGameID.Enabled = False
                btnImport.Enabled = True
                btnExport.Enabled = True
        End Select

        IsLoading = False
    End Sub

    'This function handles any "sub modes" based on the current state of the form
    Private Sub ModeChangeHandler(Optional ByVal bResetValues As Boolean = False)
        'In Monitor Only mode, no other mode changes currently matter
        If cmsMonitorOnly.Checked Then
            chkFolderSave.Enabled = False
            chkTimeStamp.Enabled = False
            lblLimit.Enabled = False
            nudLimit.Enabled = False
            chkDifferentialBackup.Enabled = False
            lblInterval.Enabled = False
            nudInterval.Enabled = False
            lblSavePath.Enabled = False
            txtSavePath.Enabled = False
            btnSavePathBrowse.Enabled = False
            btnInclude.Enabled = False
            btnExclude.Enabled = False
            chkCleanFolder.Enabled = False
            cmsWineConfig.Enabled = False
        Else
            'Enable any controls that are not handled by other mode updates
            chkTimeStamp.Enabled = True
            chkDifferentialBackup.Enabled = True
            lblSavePath.Enabled = True
            txtSavePath.Enabled = True
            btnSavePathBrowse.Enabled = True
            cmsWineConfig.Enabled = True

            'Handle "Save Multiple Backups" mode change
            If chkTimeStamp.Checked Then
                nudLimit.Enabled = True
                lblLimit.Enabled = True
            Else
                nudLimit.Enabled = False
                lblLimit.Enabled = False
            End If
            If bResetValues Then nudLimit.Value = nudLimit.Minimum

            'Handle "Differential Backups" mode change
            If chkDifferentialBackup.Checked Then
                nudInterval.Enabled = True
                lblInterval.Enabled = True
                lblLimit.Text = frmGameManager_lblLimit_Alt
                If bResetValues Then nudInterval.Value = 6
            Else
                nudInterval.Enabled = False
                lblInterval.Enabled = False
                lblLimit.Text = frmGameManager_lblLimit
                If bResetValues Then nudInterval.Value = nudInterval.Minimum
            End If

            'Handle "Registry" mode change
            If mgrPath.IsSupportedRegistryPath(txtSavePath.Text) Then
                cboOS.SelectedValue = CInt(clsGame.eOS.Windows)
                chkFolderSave.Checked = True
                chkFolderSave.Enabled = False
                btnExclude.Enabled = False
                chkDifferentialBackup.Enabled = False
            Else
                chkFolderSave.Enabled = True
                btnExclude.Enabled = True
                chkDifferentialBackup.Enabled = True
            End If

            'Handle "Save entire folder" mode change
            If chkFolderSave.Checked Then
                btnInclude.Enabled = False
                If txtFileType.Text <> String.Empty Then
                    txtFileType.Text = String.Empty
                    UpdateBuilderButtonLabel(txtFileType.Text, frmGameManager_IncludeShortcut, btnInclude, False)
                End If
            Else
                btnInclude.Enabled = True
            End If

            'Handle "Delete folder on restore" mode change
            If (chkFolderSave.Checked = True And txtExclude.Text = String.Empty And txtSavePath.Text <> String.Empty) And Not mgrPath.IsSupportedRegistryPath(txtSavePath.Text) And Not cmsMonitorOnly.Checked Then
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
        If IsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveApp()
                Case MsgBoxResult.No
                    If lstGames.SelectedItems.Count > 0 Then
                        eCurrentMode = eModes.View
                        FillData()
                        ModeChange()
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
                FillData()
                ModeChange()
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
        oApp.IsRegEx = cmsRegEx.Checked
        oApp.UseWindowTitle = cmsUseWindowTitle.Checked

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

        oApp.FileType = txtFileType.Text
        oApp.ExcludeList = txtExclude.Text
        oApp.FolderSave = chkFolderSave.Checked
        oApp.RecurseSubFolders = chkRecurseSubFolders.Checked
        oApp.CleanFolder = chkCleanFolder.Checked
        oApp.AppendTimeStamp = chkTimeStamp.Checked
        oApp.BackupLimit = nudLimit.Value
        oApp.Differential = chkDifferentialBackup.Checked
        oApp.DiffInterval = nudInterval.Value
        oApp.Comments = txtComments.Text
        oApp.Enabled = cmsEnabled.Checked
        oApp.MonitorOnly = cmsMonitorOnly.Checked
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
                End If
            Case eModes.Edit
                If CoreValidatation(oApp, False) Then
                    If CheckManifestandUpdate(CurrentGame, oApp, mgrSettings.UseGameID) Then
                        bSuccess = True
                        mgrMonitorList.DoListUpdate(oApp, CurrentGame.ID)
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
                End If
        End Select

        If bSuccess Then
            CurrentGame = oApp
            LoadBackupData()
            IsDirty = False
            LoadData()
        End If
    End Sub

    Private Sub DeleteApp()
        Dim oData As KeyValuePair(Of String, String)
        Dim oApp As clsGame

        If lstGames.SelectedItems.Count >= 1 Then
            Dim sMonitorIDs As New List(Of String)

            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(GameData(oData.Key), clsGame)
                sMonitorIDs.Add(oApp.ID)
            Next

            If mgrCommon.ShowMessage(frmGameManager_ConfirmMultiGameDelete, sMonitorIDs.Count, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrMonitorList.DoListDelete(sMonitorIDs)
                LoadData()
            End If
        End If
    End Sub

    Private Sub SwitchApp()
        If Not IsLoading Then
            If lstGames.SelectedItems.Count = 1 Then
                eCurrentMode = eModes.View
                FillData()
                ModeChange()
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

        If (chkFolderSave.Checked = False And txtFileType.Text = String.Empty) And Not cmsMonitorOnly.Checked Then
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
                    mgrCommon.OpenInOS(mgrCommon.FormatString(AppURL_RegExr, oApp.ProcessName), , True)
                End If
                Return False
            End If
        End If

        If txtProcess.Text.Trim = String.Empty Then
            'Show one time warning
            If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.EmptyProcessWarning) = mgrSettings.eSuppressMessages.EmptyProcessWarning Then
                mgrCommon.ShowMessage(frmGameManager_WarningEmptyProcess, MsgBoxStyle.Information)
                mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.EmptyProcessWarning)
                mgrSettings.SaveSettings()
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
                If cboFilters.SelectedValue = 0 Then
                    If lstGames.SelectedItems.Count = 1 Then
                        lstGames.SelectedIndex = lstGames.Items.IndexOf(New KeyValuePair(Of String, String)(CurrentGame.ID, CurrentGame.Name))
                        GetBackupInfo(CurrentGame)
                    End If
                Else
                    LoadData()
                End If
            End If
        End If
    End Sub

    Private Sub TriggerSelectedImportBackup()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim oBackup As New mgrBackup
        Dim sConfirm As String = frmGameManager_ConfirmBackupImport
        Dim sFiles As String()
        Dim oExtensions As New SortedList

        oExtensions.Add(frmGameManager_7zBackup, "*.7z")
        sFiles = mgrCommon.OpenMultiFileBrowser("GM_ImportBackup", frmGameManager_Choose7zImport, oExtensions, 1, sDefaultFolder, True)

        If sFiles.Length > 0 Then
            ImportBackupList = sFiles
            ImportBackupGame = CurrentGame

            If sFiles.Length > 1 And Not CurrentGame.AppendTimeStamp Then
                mgrCommon.ShowMessage(frmGameManager_WarningImportBackupSaveMulti, MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If sFiles.Length = 1 And Not CurrentGame.AppendTimeStamp And mgrManifest.DoManifestCheck(CurrentGame.ID, mgrSQLite.Database.Remote) Then
                sConfirm = frmGameManager_ConfirmBackupImportOverwriteSingle
            End If

            If mgrCommon.ShowMessage(sConfirm, CurrentGame.CroppedName, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
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

                If Not mgrRestore.CheckManifest(oGame.ID) Then
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestoreAnyway, oGame.Name)
                Else
                    sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestore, oGame.Name)
                End If

                If lstGames.SelectedItems.Count = 1 Then
                    'Replace backup entry with currently selected backup item in case the user wants to restore an older backup.                
                    If oBackup.DateUpdatedUnix > CurrentBackupItem.DateUpdatedUnix Then
                        NoRestoreQueue = True
                        RestoreList.Clear()
                        RestoreList.Add(oGame, CurrentBackupItem)
                        sMsg = mgrCommon.FormatString(frmGameManager_ConfirmRestoreSelected, New String() {oGame.CroppedName, CurrentBackupItem.DateUpdated, CurrentBackupItem.UpdatedBy})
                    End If

                    If mgrConfigLinks.CheckForLinks(oGame.ID) And (oBackup.DateUpdatedUnix > CurrentBackupItem.DateUpdatedUnix) Then
                        If mgrCommon.ShowMessage(mgrCommon.FormatString(frmGameManager_RestoreLinkWarning, oGame.CroppedName), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            NoRestoreQueue = True
                        Else
                            bDoRestore = False
                        End If
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

    Private Sub LoadCombos()
        Dim oOSItems As New List(Of KeyValuePair(Of Integer, String))
        Dim oFilterItems As New List(Of KeyValuePair(Of Integer, String))

        'cboOS
        cboOS.ValueMember = "Key"
        cboOS.DisplayMember = "Value"

        oOSItems.Add(New KeyValuePair(Of Integer, String)(clsGame.eOS.Windows, App_WindowsOS))
        oOSItems.Add(New KeyValuePair(Of Integer, String)(clsGame.eOS.Linux, App_LinuxOS))

        cboOS.DataSource = oOSItems

        'cboFilters
        cboFilters.ValueMember = "Key"
        cboFilters.DisplayMember = "Value"

        oFilterItems.Add(New KeyValuePair(Of Integer, String)(0, frmGameManager_cboFilters_All))
        oFilterItems.Add(New KeyValuePair(Of Integer, String)(1, frmGameManager_cboFilters_Backups))
        oFilterItems.Add(New KeyValuePair(Of Integer, String)(2, frmGameManager_cboFilters_Pending))
        oFilterItems.Add(New KeyValuePair(Of Integer, String)(3, frmGameManager_cboFilters_Custom))

        cboFilters.DataSource = oFilterItems
        cboFilters.SelectedValue = 0
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmGameManager_FormName
        Me.Icon = GBM_Icon

        'Set Form text        
        tbConfig.Text = frmGameManager_tbConfig
        tbGameInfo.Text = frmGameManager_tbGameInfo
        tbBackupInfo.Text = frmGameManager_tbBackupInfo
        btnExport.Text = frmGameManager_btnExport
        btnExport.Image = Multi_Export
        btnImport.Text = frmGameManager_btnImport
        btnImport.Image = Multi_Import
        lblFilters.Text = frmGameManager_lblFilters
        cmsEnabled.Text = frmGameManager_cmsEnabled
        btnCancel.Text = frmGameManager_btnCancel
        btnCancel.Image = Multi_Cancel
        cmsMonitorOnly.Text = frmGameManager_cmsMonitorOnly
        btnRestore.Text = frmGameManager_btnRestore
        btnBackup.Text = frmGameManager_btnBackup
        cmsImportData.Text = frmGameManager_cmsImportData
        btnSave.Text = frmGameManager_btnSave
        btnSave.Image = Multi_Save
        lblRestorePath.Text = frmGameManager_lblRestorePath
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
        btnExclude.Image = frmGameManager_Exclude_Items
        btnInclude.Text = frmGameManager_btnInclude
        btnInclude.Image = frmGameManager_Include_Items
        btnSavePathBrowse.Text = frmGameManager_btnSavePathBrowse
        btnProcessBrowse.Text = frmGameManager_btnProcessBrowse
        lblSavePath.Text = frmGameManager_lblSavePath
        lblProcess.Text = frmGameManager_lblProcess
        lblParameter.Text = frmGameManager_lblParameter
        lblName.Text = frmGameManager_lblName
        chkTimeStamp.Text = frmGameManager_chkTimeStamp
        chkFolderSave.Text = frmGameManager_chkFolderSave
        chkCleanFolder.Text = frmGameManager_chkCleanFolder
        btnDelete.Text = frmGameManager_btnDelete
        btnDelete.Image = Multi_Delete
        btnAdd.Text = frmGameManager_btnAdd
        btnAdd.Image = Multi_Add
        cmsOfficial.Text = frmGameManager_cmsOfficial
        cmsOfficialLinux.Text = frmGameManager_cmsOfficialLinux
        cmsOfficialWindows.Text = frmGameManager_cmsOfficialWindows
        cmsLudusavi.Text = frmGameManager_cmsLudusavi
        cmsFile.Text = frmGameManager_cmsFile
        cmsURL.Text = frmGameManager_cmsURL
        lblSearch.Text = frmGameManager_lblSearch
        lblLimit.Text = frmGameManager_lblLimit
        lblInterval.Text = frmGameManager_lblInterval
        chkDifferentialBackup.Text = frmGameManager_chkDifferentialBackup
        cmsDeleteOne.Text = frmGameManager_cmsDeleteOne
        cmsDeleteAll.Text = frmGameManager_cmsDeleteAll
        btnLinks.Image = frmGameManager_Link
        btnMonitorOptions.Image = Multi_Search
        btnGameID.Image = frmGameManager_GameID
        lblComments.Text = frmGameManager_lblComments
        cmsRegEx.Text = frmGameManager_cmsRegEx
        cmsUseWindowTitle.Text = frmGameManager_cmsUseWindowTitle
        btnProcessOptions.Image = frmGameManager_Process
        btnAdvanced.Text = frmGameManager_btnAdvanced
        btnAdvanced.Image = frmGameManager_Advanced
        cmsWineConfig.Text = frmGameManager_cmsWineConfig
        cmsLinkProcess.Text = frmGameManager_cmsLinkProcess
        cmsLinkConfiguration.Text = frmGameManager_cmsLinkConfiguration
        cmsLaunchSettings.Text = frmGamemanager_cmsLaunchSettings
        lblGameTags.Text = frmGameManager_lblGameTags
        lblTags.Text = frmGameManager_lblTags
        btnBackupData.Image = frmGameManager_Backup_Data
        btnMarkAsRestored.Image = frmGameManager_Mark
        btnOpenBackupFolder.Image = frmGameManager_Folder_Open

        'Tool Tips
        ttHelp.SetToolTip(btnBackupData, frmGameManager_ttHelp_btnBackupData)
        ttHelp.SetToolTip(btnMarkAsRestored, frmGameManager_ttHelp_btnMarkAsRestored)
        ttHelp.SetToolTip(btnOpenBackupFolder, frmGameManager_ttHelp_btnOpenBackupFolder)
        ttHelp.SetToolTip(lblBackupFileData, frmGameManager_ttHelp_lblBackupFileData)
        ttHelp.SetToolTip(lblGameTags, frmGameManager_ttHelp_lblTags)
        ttHelp.SetToolTip(btnProcessOptions, frmGameManager_ttHelp_btnProcessOptions)
        ttHelp.SetToolTip(btnLinks, frmGameManager_ttHelp_btnLinks)
        ttHelp.SetToolTip(btnMonitorOptions, frmGameManager_ttHelp_btnMonitorOptions)
        ttHelp.SetToolTip(btnGameID, frmGameManager_ttHelp_btnGameID)
        ttHelp.SetToolTip(cboOS, frmGameManager_ttHelp_cboOS)
        ttHelp.SetToolTip(nudLimit, frmGameManager_ttHelp_nudLimit)
        ttHelp.SetToolTip(nudInterval, frmGameManager_ttHelp_nudInterval)

        LoadCombos()

        'Hide advanced button when not needed
        If Not mgrCommon.IsUnix And Not mgrSettings.EnableLauncher Then
            btnAdvanced.Visible = False
        End If

        'Hide OS on Windows
        If Not mgrCommon.IsUnix Then
            cboOS.Visible = False
            cmsWineConfig.Visible = False
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
            btnBackupData.Visible = False
        End If

        If Not mgrSettings.EnableLauncher Then
            cmsLaunchSettings.Visible = False
        End If

        LoadBackupData()

        If PendingRestores Then
            cboFilters.SelectedValue = 2
            tabGameManager.SelectedTab = tbBackupInfo
        Else
            cboFilters.SelectedValue = 0
        End If

        AssignDirtyHandlers(grpCoreConfig.Controls)
        AssignDirtyHandlers(grpGameInfo.Controls)
        AddHandler cmsRegEx.CheckedChanged, AddressOf DirtyCheck_ValueChanged
        AddHandler cmsUseWindowTitle.CheckedChanged, AddressOf DirtyCheck_ValueChanged
        AddHandler cmsEnabled.CheckedChanged, AddressOf DirtyCheck_ValueChanged
        AddHandler cmsMonitorOnly.CheckedChanged, AddressOf DirtyCheck_ValueChanged

        LoadData(False)

        If PendingRestores And lstGames.Items.Count >= 1 Then lstGames.SelectedIndex = 0

        InitialLoad = False
    End Sub

    Private Sub cboFilters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilters.SelectedIndexChanged
        If Not InitialLoad Then
            lstGames.ClearSelected()
            LoadData(False)
        End If
    End Sub

    Private Sub lstGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstGames.SelectedIndexChanged
        SwitchApp()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
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

    Private Sub frmGameManager_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveApp()
                    If IsDirty Then e.Cancel = True
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

    Private Sub lblBackupFileData_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblBackupFileData.LinkClicked
        OpenBackupFile()
    End Sub

    Private Sub btnOpenBackupFolder_Click(sender As Object, e As EventArgs) Handles btnOpenBackupFolder.Click
        OpenBackupFolder()
    End Sub

    Private Sub lblRestorePathData_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblRestorePathData.LinkClicked
        OpenRestorePath()
    End Sub

    Private Sub btnProcessOptions_Click(sender As Object, e As EventArgs) Handles btnProcessOptions.Click
        mgrCommon.OpenButtonSubMenu(cmsProcessOptions, btnProcessOptions)
    End Sub

    Private Sub btnLink_Click(sender As Object, e As EventArgs) Handles btnAdvanced.Click
        mgrCommon.OpenButtonSubMenu(cmsAdvanced, btnAdvanced)
    End Sub

    Private Sub lblGameTags_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblGameTags.LinkClicked
        OpenTags()
    End Sub

    Private Sub ModifyGameIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles btnGameID.Click
        OpenGameIDEdit()
    End Sub

    Private Sub cmsLink_Click(sender As Object, e As EventArgs) Handles cmsLinkConfiguration.Click
        OpenConfigLinks()
    End Sub

    Private Sub cmsProcess_Click(sender As Object, e As EventArgs) Handles cmsLinkProcess.Click
        OpenProcesses()
    End Sub

    Private Sub cmsLaunchSettings_Click(sender As Object, e As EventArgs) Handles cmsLaunchSettings.Click
        OpenLauncherConfig()
    End Sub

    Private Sub cmsWineConfig_Click(sender As Object, e As EventArgs) Handles cmsWineConfig.Click
        OpenWineConfiguration()
    End Sub

    Private Sub btnBackupData_Click(sender As Object, e As EventArgs) Handles btnBackupData.Click
        mgrCommon.OpenButtonSubMenu(cmsBackupData, btnBackupData)
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        TriggerSelectedRestore()
    End Sub

    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        TriggerSelectedBackup()
    End Sub

    Private Sub cmsImportData_Click(sender As Object, e As EventArgs) Handles cmsImportData.Click
        TriggerSelectedImportBackup()
    End Sub

    Private Sub cmsDeleteOne_Click(sender As Object, e As EventArgs) Handles cmsDeleteOne.Click
        DeleteBackup()
    End Sub

    Private Sub cmsDeleteAll_Click(sender As Object, e As EventArgs) Handles cmsDeleteAll.Click
        DeleteAllBackups()
    End Sub

    Private Sub btnMarkAsRestored_CheckedChanged(sender As Object, e As EventArgs) Handles btnMarkAsRestored.Click
        MarkAsRestored()
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
        If Not IsLoading Then ModeChangeHandler()
    End Sub

    Private Sub chkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles chkTimeStamp.CheckedChanged
        If Not IsLoading Then ModeChangeHandler(True)
    End Sub

    Private Sub chkDifferentialBackup_CheckedChanged(sender As Object, e As EventArgs) Handles chkDifferentialBackup.CheckedChanged
        If Not IsLoading Then ModeChangeHandler(True)
    End Sub

    Private Sub chkMonitorOnly_CheckedChanged(sender As Object, e As EventArgs) Handles cmsMonitorOnly.CheckedChanged
        If Not IsLoading Then ModeChangeHandler()
    End Sub

    Private Sub cboRemoteBackup_Enter(sender As Object, e As EventArgs) Handles cboRemoteBackup.Enter, cboRemoteBackup.Click
        If Not CurrentGame Is Nothing Then VerifyBackups(CurrentGame)
    End Sub

    Private Sub cboRemoteBackup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRemoteBackup.SelectedIndexChanged
        If Not IsLoading Then
            UpdateBackupInfo(DirectCast(cboRemoteBackup.SelectedItem, KeyValuePair(Of String, String)).Key)
        End If
    End Sub

    Private Sub cboOS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOS.SelectedIndexChanged
        If Not IsLoading And Not eCurrentMode = eModes.Add Then
            HandleWineConfig()
        End If
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        mgrCommon.OpenButtonSubMenu(cmsImport, btnImport)
    End Sub

    Private Sub btnMonitorOptions_Click(sender As Object, e As EventArgs) Handles btnMonitorOptions.Click
        mgrCommon.OpenButtonSubMenu(cmsMonitorOptions, btnMonitorOptions)
    End Sub

    Private Sub btnLinks_Click(sender As Object, e As EventArgs) Handles btnLinks.Click
        mgrCommon.OpenButtonSubMenu(cmsLinks, btnLinks)
    End Sub

    Private Sub cmsOfficialWindows_Click(sender As Object, e As EventArgs) Handles cmsOfficialWindows.Click, cmsOfficial.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        If mgrMonitorList.ImportOfficialGameList(App_URLImport, True) Then
            LoadData()
            LoadBackupData()
        End If
    End Sub

    Private Sub cmsOfficialLinux_Click(sender As Object, e As EventArgs) Handles cmsOfficialLinux.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        If mgrMonitorList.ImportOfficialGameList(App_URLImportLinux) Then
            LoadData()
            LoadBackupData()
        End If
    End Sub

    Private Sub cmsLudusavi_Click(sender As Object, e As EventArgs) Handles cmsLudusavi.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        If mgrMonitorList.ImportLudusaviManifest(App_URLImportLudusavi) Then
            LoadData()
            LoadBackupData()
        End If
    End Sub

    Private Sub cmsFile_Click(sender As Object, e As EventArgs) Handles cmsFile.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        If mgrMonitorList.ImportGameListFile Then
            LoadData()
            LoadBackupData()
        End If
    End Sub

    Private Sub cmsURL_Click(sender As Object, e As EventArgs) Handles cmsURL.Click
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        If mgrMonitorList.ImportGameListURL Then
            LoadData()
            LoadBackupData()
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        mgrMonitorList.ExportGameList()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not eCurrentMode = eModes.Disabled Then
            eCurrentMode = eModes.Disabled
            ModeChange()
            lstGames.ClearSelected()
        End If

        If Not tmFilterTimer.Enabled Then
            lstGames.Enabled = False
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub txtSavePath_TextChanged(sender As Object, e As EventArgs) Handles txtSavePath.TextChanged
        If Not IsLoading Then
            ttFullPath.RemoveAll()
            ModeChangeHandler()
        End If
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        LoadData()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
        lstGames.Enabled = True
    End Sub

    Private Sub frmGameManager_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        'Load Last Played Game
        If Not OpenToGame Is Nothing Then
            lstGames.SelectedItem = New KeyValuePair(Of String, String)(OpenToGame.ID, OpenToGame.Name)
        End If

        txtSearch.Focus()
    End Sub

    Private Sub cmsUseWindowTitle_CheckChanged(sender As Object, e As EventArgs) Handles cmsUseWindowTitle.CheckedChanged
        If cmsUseWindowTitle.Checked Then
            lblProcess.Text = frmGameManager_lblProcess_WindowTitle
            btnProcessBrowse.Enabled = False
        Else
            lblProcess.Text = frmGameManager_lblProcess
            btnProcessBrowse.Enabled = True
        End If
    End Sub
End Class
