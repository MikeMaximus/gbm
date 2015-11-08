Imports System.IO

Public Class frmGameManager

    Private sBackupFolder As String
    Private bPendingRestores As Boolean = False
    Private oCurrentBackupItem As clsBackup
    Private oCurrentGame As clsGame
    Private bDisableExternalFunctions As Boolean = False
    Private bTriggerBackup As Boolean = False
    Private bTriggerRestore As Boolean = False
    Private oBackupList As New List(Of clsGame)
    Private oRestoreList As New List(Of clsGame)
    Private oAppData As Hashtable
    Private oLocalBackupData As SortedList
    Private oRemoteBackupData As SortedList
    Private bIsDirty As Boolean = False
    Private bIsLoading As Boolean = False

    Private Enum eModes As Integer
        View = 1
        Edit = 2
        Add = 3
        Disabled = 4
        MultiSelect = 5
        ViewTemp = 6
    End Enum

    Private eCurrentMode As eModes = eModes.Disabled

    Property BackupFolder As String
        Get
            Return sBackupFolder & "\"
        End Get
        Set(value As String)
            sBackupFolder = value
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

    Private Property AppData As Hashtable
        Get
            Return oAppData
        End Get
        Set(value As Hashtable)
            oAppData = value
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

    Property RestoreList As List(Of clsGame)
        Get
            Return oRestoreList
        End Get
        Set(value As List(Of clsGame))
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
        oRemoteBackupData = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)
        oLocalBackupData = mgrManifest.ReadManifest(mgrSQLite.Database.Local)
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
        Dim oBackupItem As clsBackup
        Dim sDirectory As String
        Dim sNewDirectory As String
        Dim sFileName As String
        Dim sNewFileName As String

        'If there is a name change,  check and update the manifest
        If oNewApp.Name <> oOriginalApp.Name Then
            'Local
            If mgrManifest.DoManifestCheck(oOriginalApp.Name, mgrSQLite.Database.Local) Then
                oBackupItem = mgrManifest.DoManifestGetByName(oOriginalApp.Name, mgrSQLite.Database.Local)

                'Rename Current Backup File & Folder
                sFileName = BackupFolder & oBackupItem.FileName

                'Rename Backup File
                sNewFileName = Path.GetDirectoryName(sFileName) & "\" & Path.GetFileName(sFileName).Replace(oOriginalApp.Name, oNewApp.Name)
                If File.Exists(sFileName) Then
                    FileSystem.Rename(sFileName, sNewFileName)
                End If

                'Rename Directory
                sDirectory = Path.GetDirectoryName(sFileName)
                sNewDirectory = sDirectory.Replace(oOriginalApp.Name, oNewApp.Name)
                If sDirectory <> sNewDirectory Then
                    If Directory.Exists(sDirectory) Then
                        FileSystem.Rename(sDirectory, sNewDirectory)
                    End If
                End If

                oBackupItem.Name = oNewApp.Name
                oBackupItem.FileName = oBackupItem.FileName.Replace(oOriginalApp.Name, oNewApp.Name)
                mgrManifest.DoManifestNameUpdate(oOriginalApp.Name, oBackupItem, mgrSQLite.Database.Local)
                oLocalBackupData = mgrManifest.ReadManifest(mgrSQLite.Database.Local)
            End If
            'Remote
            If mgrManifest.DoManifestCheck(oOriginalApp.Name, mgrSQLite.Database.Remote) Then
                oBackupItem = mgrManifest.DoManifestGetByName(oOriginalApp.Name, mgrSQLite.Database.Remote)
                oBackupItem.Name = oNewApp.Name
                oBackupItem.FileName = oBackupItem.FileName.Replace(oOriginalApp.Name, oNewApp.Name)
                mgrManifest.DoManifestNameUpdate(oOriginalApp.Name, oBackupItem, mgrSQLite.Database.Remote)
                oRemoteBackupData = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)
            End If
        End If
    End Sub

    Private Sub LoadData()
        Dim oRestoreData As New SortedList
        Dim oGame As clsGame
        Dim oBackup As clsBackup

        AppData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.ListByKey)

        If optPendingRestores.Checked Then
            oRestoreData = mgrRestore.CompareManifests

            'Only show games with data to restore
            Dim oTemporaryList As Hashtable = AppData.Clone
            For Each de As DictionaryEntry In oTemporaryList
                oGame = DirectCast(de.Value, clsGame)
                If Not oRestoreData.ContainsKey(oGame.Name) Then
                    AppData.Remove(de.Key)
                Else
                    oRestoreData.Remove(oGame.Name)
                End If
            Next
        ElseIf optBackupData.Checked Then
            'Only show games with backup data
            Dim oTemporaryList As Hashtable = AppData.Clone
            oRestoreData = oRemoteBackupData.Clone

            For Each de As DictionaryEntry In oTemporaryList
                oGame = DirectCast(de.Value, clsGame)
                If Not oRemoteBackupData.ContainsKey(oGame.Name) Then
                    AppData.Remove(de.Key)
                Else
                    oRestoreData.Remove(oGame.Name)
                End If
            Next
        End If

        'Handle any orphaned backup files and add them to list
        If oRestoreData.Count <> 0 Then
            For Each oBackup In oRestoreData.Values
                oGame = New clsGame
                oGame.Name = oBackup.Name
                oGame.Temporary = True
                AppData.Add(oGame.ID, oGame)
            Next
        End If

        lstGames.Items.Clear()
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

        sNewPath = mgrCommon.OpenFileBrowser("Choose exe file that starts the application", "exe", _
                                          "Executable", sDefaultFolder, False)

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

        sNewPath = mgrCommon.OpenFolderBrowser("Choose the location of application's exe file:", sDefaultFolder, False)

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

        sNewPath = mgrCommon.OpenFolderBrowser("Choose the application save folder:", sDefaultFolder, False)

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

        sNewPath = mgrCommon.OpenFileBrowser("Choose a custom icon for the application", "ico", _
                                          "Icon", sDefaultFolder, False)

        If sNewPath <> String.Empty Then
            txtIcon.Text = sNewPath
            If IO.File.Exists(sNewPath) Then
                pbIcon.Image = Image.FromFile(sNewPath)
            End If
        End If
    End Sub

    Private Function HandleDirty() As MsgBoxResult
        Dim oResult As MsgBoxResult

        oResult = MsgBox("There are unsaved changes on this form.  Do you want to save?", MsgBoxStyle.YesNoCancel, "Game Backup Monitor")

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

        lstGames.ValueMember = "Key"
        lstGames.DisplayMember = "Value"

        For Each de As DictionaryEntry In AppData
            oApp = DirectCast(de.Value, clsGame)
            oData = New KeyValuePair(Of String, String)(oApp.ID, oApp.Name)
            lstGames.Items.Add(oData)
        Next

        IsLoading = False
    End Sub

    Private Sub OpenBackupFile()
        Dim sFileName As String
        sFileName = BackupFolder & CurrentBackupItem.FileName

        If File.Exists(sFileName) Then
            Process.Start("explorer.exe", "/select," & sFileName)
        Else
            MsgBox("The backup file does not exist.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
        End If

    End Sub

    Private Function FindRestorePath() As Boolean
        Dim sProcess As String
        Dim sRestorePath As String
        Dim bNoAuto As Boolean

        If Not CurrentBackupItem.AbsolutePath Then
            If CurrentGame.ProcessPath <> String.Empty Then
                CurrentBackupItem.RelativeRestorePath = CurrentGame.ProcessPath & "\" & CurrentBackupItem.RestorePath
            Else
                sProcess = CurrentGame.TrueProcess
                If CurrentGame.Duplicate = True Or CurrentGame.ProcessName.Contains("dosbox") Then bNoAuto = True
                sRestorePath = mgrPath.ProcessPathSearch(CurrentBackupItem.Name, sProcess, CurrentBackupItem.Name & " uses a relative path and the game path has not been set.", bNoAuto)

                If sRestorePath <> String.Empty Then
                    CurrentBackupItem.RelativeRestorePath = sRestorePath & "\" & CurrentBackupItem.RestorePath
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

        If Directory.Exists(sPath) Then
            Process.Start("explorer.exe", sPath)
        Else
            MsgBox("The restore path does not exist.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
        End If

    End Sub

    Private Sub GetBackupInfo(ByVal oApp As clsGame)
        Dim oBackupInfo As clsBackup
        Dim sFileName As String
        Dim oFileInfo As FileInfo
        Dim dFileSize As Double

        If oRemoteBackupData.Contains(oApp.Name) Then
            CurrentBackupItem = DirectCast(oRemoteBackupData(oApp.Name), clsBackup)
            txtCurrentBackup.Text = CurrentBackupItem.DateUpdated & " by " & CurrentBackupItem.UpdatedBy
            sFileName = BackupFolder & CurrentBackupItem.FileName

            btnOpenBackupFile.Enabled = True
            btnOpenRestorePath.Enabled = True
            btnRestore.Enabled = True
            btnDeleteBackup.Enabled = True

            If File.Exists(sFileName) Then
                oFileInfo = New FileInfo(sFileName)
                dFileSize = oFileInfo.Length
                If dFileSize > 1048576 Then
                    txtFileSize.Text = Math.Round(dFileSize / 1048576, 2) & " MB"
                Else
                    txtFileSize.Text = Math.Round(dFileSize / 1024, 2) & " KB"
                End If
            Else
                txtFileSize.Text = "Backup file was not found!"
            End If
        Else
            txtCurrentBackup.Text = "Never"
            txtFileSize.Text = String.Empty
            btnOpenBackupFile.Enabled = False
            btnOpenRestorePath.Enabled = False
            btnRestore.Enabled = False
            btnDeleteBackup.Enabled = False
        End If

        If oLocalBackupData.Contains(oApp.Name) Then
            oBackupInfo = DirectCast(oLocalBackupData(oApp.Name), clsBackup)
            txtLocalBackup.Text = oBackupInfo.DateUpdated & " by " & oBackupInfo.UpdatedBy
        Else
            txtLocalBackup.Text = "Never"
        End If

        If txtCurrentBackup.Text = "Never" And txtLocalBackup.Text = "Never" Then
            lblSync.Visible = False
            btnMarkAsRestored.Enabled = False
        ElseIf txtCurrentBackup.Text <> txtLocalBackup.Text Then
            lblSync.ForeColor = Color.Red
            lblSync.Text = "Out of Sync!"
            lblSync.Visible = True
            btnMarkAsRestored.Enabled = True
        Else
            lblSync.ForeColor = Color.Green
            lblSync.Text = "Up to Date!"
            lblSync.Visible = True
            btnMarkAsRestored.Enabled = False
        End If

    End Sub

    Private Sub DeleteBackup()
        Dim oDir As DirectoryInfo
        Dim sSubDir As String

        If MsgBox("This will delete the backup file and all records of this backup.  This cannot be undone. " & vbCrLf & vbCrLf & "Do you want to remove the data for " _
                  & CurrentBackupItem.Name & "?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
            mgrManifest.DoManifestDelete(CurrentBackupItem, mgrSQLite.Database.Local)
            mgrManifest.DoManifestDelete(CurrentBackupItem, mgrSQLite.Database.Remote)

            'Delete referenced backup file from the backup folder
            If File.Exists(BackupFolder & CurrentBackupItem.FileName) Then File.Delete(BackupFolder & CurrentBackupItem.FileName)

            'Check if using backup sub-directories (Probably not the best way to check for this)
            If CurrentBackupItem.FileName.StartsWith(CurrentBackupItem.Name & "\") Then
                'Build sub-dir backup path
                sSubDir = BackupFolder & CurrentBackupItem.Name

                If Directory.Exists(sSubDir) Then
                    'Check if there's any sub-directories or files remaining
                    oDir = New DirectoryInfo(sSubDir)
                    If oDir.GetDirectories.Length > 0 Or oDir.GetFiles.Length > 0 Then
                        'Confirm
                        If MsgBox("The backup folder " & sSubDir & " still contains " & oDir.GetDirectories.Length & " directories and " & _
                                  oDir.GetFiles.Length & " files." & vbCrLf & vbCrLf & "Do you want to delete the contents and remove the sub-folder for this game?", _
                                  MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                            If Directory.Exists(sSubDir) Then Directory.Delete(sSubDir, True)
                        End If
                    Else
                        'Folder is empty,  delete the empty sub-folder
                        If Directory.Exists(sSubDir) Then Directory.Delete(sSubDir)
                    End If
                End If
            End If

            LoadBackupData()

            If oCurrentGame.Temporary Then
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            Else
                FillData()
            End If
        End If
    End Sub

    Private Sub FillData()
        IsLoading = True

        Dim oData As KeyValuePair(Of String, String) = lstGames.SelectedItems(0)
        Dim oApp As clsGame = DirectCast(AppData(oData.Key), clsGame)

        'Core
        txtID.Text = oApp.ID
        txtName.Text = oApp.Name
        txtProcess.Text = oApp.TrueProcess
        txtSavePath.Text = oApp.Path
        txtFileType.Text = oApp.FileType
        txtExclude.Text = oApp.ExcludeList
        chkFolderSave.Checked = oApp.FolderSave
        chkTimeStamp.Checked = oApp.AppendTimeStamp
        chkEnabled.Checked = oApp.Enabled
        chkMonitorOnly.Checked = oApp.MonitorOnly

        'Extra
        txtAppPath.Text = oApp.ProcessPath
        txtCompany.Text = oApp.Company
        txtVersion.Text = oApp.Version
        txtIcon.Text = oApp.Icon

        'Icon
        If IO.File.Exists(oApp.Icon) Then
            pbIcon.Image = Image.FromFile(oApp.Icon)
        Else
            pbIcon.Image = My.Resources.Unknown
        End If

        'Stats
        nudHours.Value = oApp.Hours
        GetBackupInfo(oApp)

        'Set Current
        CurrentGame = oApp

        'Change view to temporary if we only have backup data for the game
        If CurrentGame.Temporary Then
            eCurrentMode = eModes.ViewTemp
            ModeChange()
        End If

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
    End Sub

    Private Sub WipeControls(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Text = String.Empty
            ElseIf TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Checked = False
            ElseIf TypeOf ctl Is NumericUpDown Then
                DirectCast(ctl, NumericUpDown).Value = 0
            End If
        Next
    End Sub

    Private Sub ModeChange()
        IsLoading = True

        Select Case eCurrentMode
            Case eModes.Add
                grpFilter.Enabled = False
                lstGames.Enabled = False
                grpConfig.Enabled = True
                chkMonitorOnly.Enabled = True
                grpExtra.Enabled = True
                grpStats.Enabled = True
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                pbIcon.Image = My.Resources.Unknown
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
                lblSync.Visible = False
                chkEnabled.Checked = True
                chkMonitorOnly.Checked = False
            Case eModes.Edit
                grpFilter.Enabled = False
                lstGames.Enabled = False
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
            Case eModes.View
                grpFilter.Enabled = True
                lstGames.Enabled = True
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
            Case eModes.ViewTemp
                grpFilter.Enabled = True
                lstGames.Enabled = True
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
            Case eModes.Disabled
                grpFilter.Enabled = True
                lstGames.Enabled = True
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                pbIcon.Image = My.Resources.Unknown
                lblSync.Visible = False
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
            Case eModes.MultiSelect
                lstGames.Enabled = True
                WipeControls(grpConfig.Controls)
                WipeControls(grpExtra.Controls)
                WipeControls(grpStats.Controls)
                pbIcon.Image = My.Resources.Unknown
                lblSync.Visible = False
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
        End Select

        IsLoading = False
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

    Private Sub SaveApp()
        Dim oData As KeyValuePair(Of String, String)
        Dim oApp As New clsGame
        Dim bSuccess As Boolean = False

        If txtID.Text <> String.Empty Then
            oApp.ID = txtID.Text
        End If
        oApp.Name = mgrPath.ValidateForFileSystem(txtName.Text)
        If Path.HasExtension(txtProcess.Text) Then
            If txtProcess.Text.ToLower.EndsWith(".exe") Then
                oApp.ProcessName = Path.GetFileNameWithoutExtension(txtProcess.Text)
            Else
                oApp.ProcessName = txtProcess.Text
            End If
        Else
            oApp.ProcessName = txtProcess.Text
        End If
        oApp.Path = txtSavePath.Text
        'Only do a simple root check here in case the user doesn't really understand creating a proper configuration
        oApp.AbsolutePath = Path.IsPathRooted(txtSavePath.Text)
        oApp.FileType = txtFileType.Text
        oApp.ExcludeList = txtExclude.Text
        oApp.FolderSave = chkFolderSave.Checked
        oApp.AppendTimeStamp = chkTimeStamp.Checked
        oApp.Enabled = chkEnabled.Checked
        oApp.MonitorOnly = chkMonitorOnly.Checked
        oApp.ProcessPath = txtAppPath.Text
        oApp.Company = txtCompany.Text
        oApp.Version = txtVersion.Text
        oApp.Icon = txtIcon.Text
        oApp.Hours = CDbl(nudHours.Value)

        Select Case eCurrentMode
            Case eModes.Add
                If CoreValidatation(oApp) Then
                    bSuccess = True
                    mgrMonitorList.DoListAdd(oApp)
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oApp) Then
                    bSuccess = True
                    mgrMonitorList.DoListUpdate(oApp)
                    CheckManifestandUpdate(oCurrentGame, oApp)
                    eCurrentMode = eModes.View
                End If
            Case eModes.MultiSelect
                Dim sMonitorIDs As New List(Of String)
                Dim sChanges As String
                For Each oData In lstGames.SelectedItems
                    sMonitorIDs.Add(AppData(oData.Key))
                Next

                sChanges = vbCrLf & vbCrLf & "Monitor this game:  " & mgrCommon.BooleanYesNo(oApp.Enabled) & vbCrLf & "Monitor only (No backup):  " & mgrCommon.BooleanYesNo(oApp.MonitorOnly)
                If MsgBox("Are you sure you want to save the following changes to " & sMonitorIDs.Count & " selected games?" & sChanges, MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    bSuccess = True
                    mgrMonitorList.DoListUpdateMulti(sMonitorIDs, oApp)
                    eCurrentMode = eModes.Disabled
                End If
        End Select

        If bSuccess Then
            Dim iSelected As Integer
            IsDirty = False
            LoadData()
            iSelected = lstGames.Items.IndexOf(New KeyValuePair(Of String, String)(oApp.ID, oApp.Name))
            If iSelected = -1 Then eCurrentMode = eModes.Disabled
            ModeChange()
            If eCurrentMode = eModes.View Then lstGames.SelectedIndex = iSelected
        End If
    End Sub

    Private Sub DeleteApp()
        Dim oData As KeyValuePair(Of String, String)
        Dim oApp As clsGame

        If lstGames.SelectedItems.Count = 1 Then
            oData = lstGames.SelectedItems(0)
            oApp = DirectCast(AppData(oData.Key), clsGame)

            If MsgBox("Are you sure you want to delete " & oApp.Name & "?  This cannot be undone." & vbCrLf & vbCrLf & "This will not delete any backup files that already exist for this game.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                mgrMonitorList.DoListDelete(oApp.ID)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        ElseIf lstGames.SelectedItems.Count > 1 Then
            Dim sMonitorIDs As New List(Of String)

            For Each oData In lstGames.SelectedItems
                oApp = DirectCast(AppData(oData.Key), clsGame)
                sMonitorIDs.Add(oApp.ID)
            Next

            If MsgBox("Are you sure you want to delete the " & sMonitorIDs.Count & " selected games?  This cannot be undone.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                mgrMonitorList.DoListDeleteMulti(sMonitorIDs)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SwitchApp()
        If lstGames.SelectedItems.Count = 1 Then
            eCurrentMode = eModes.View
            FillData()
            ModeChange()
        ElseIf lstGames.SelectedItems.Count > 1 Then
            eCurrentMode = eModes.MultiSelect
            ModeChange()
        End If
    End Sub

    Private Function CoreValidatation(ByVal oApp As clsGame) As Boolean
        If txtName.Text = String.Empty Then
            MsgBox("You must enter a valid application name.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
            txtName.Focus()
            Return False
        End If

        If txtProcess.Text = String.Empty Then
            MsgBox("You must enter a valid process name.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
            txtProcess.Focus()
            Return False
        End If

        If mgrMonitorList.DoDuplicateListCheck(oApp.Name, oApp.ProcessName, , oApp.ID) Then
            MsgBox("A game with this exact name and process already exists.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
            txtName.Focus()
            Return False
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
                If MsgBox("Do you want to mark " & oMarkList(0).Name & " as restored?  This cannot be undone.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    bWasUpdated = True
                    If mgrManifest.DoManifestCheck(oMarkList(0).Name, mgrSQLite.Database.Local) Then
                        mgrManifest.DoManifestUpdate(oMarkList(0), mgrSQLite.Database.Local)
                    Else
                        mgrManifest.DoManifestAdd(oMarkList(0), mgrSQLite.Database.Local)
                    End If
                End If
            ElseIf oMarkList.Count > 1 Then
                If MsgBox("Do you want to mark " & oMarkList.Count & " games as restored?  This cannot be undone.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    bWasUpdated = True
                    For Each oGameBackup In oMarkList
                        If mgrManifest.DoManifestCheck(oGameBackup.Name, mgrSQLite.Database.Local) Then
                            mgrManifest.DoManifestUpdate(oGameBackup, mgrSQLite.Database.Local)
                        Else
                            mgrManifest.DoManifestAdd(oGameBackup, mgrSQLite.Database.Local)
                        End If
                    Next
                End If
            Else
                MsgBox("The selected game(s) have no backup data.", MsgBoxStyle.Information, "Game Backup Monitor")
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

    Private Sub TriggerSelectedBackup()
        Dim oData As KeyValuePair(Of String, String)
        Dim sMsg As String = String.Empty
        Dim oGame As clsGame

        If lstGames.SelectedItems.Count > 0 Then
            BackupList.Clear()

            For Each oData In lstGames.SelectedItems
                oGame = DirectCast(AppData(oData.Key), clsGame)
                BackupList.Add(oGame)
            Next

            If BackupList.Count = 1 Then
                sMsg = "Are you sure you want to run a backup for " & BackupList(0).Name & "?  This will close the form."
            ElseIf BackupList.Count > 1 Then
                sMsg = "Are you sure you want to run a backup for " & BackupList.Count & " games?  This will close the form."
            End If

            If MsgBox(sMsg, MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                TriggerBackup = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub TriggerSelectedRestore()
        Dim oData As KeyValuePair(Of String, String)
        Dim sMsg As String = String.Empty
        Dim oGame As clsGame
        Dim bDoRestore As Boolean = False

        If lstGames.SelectedItems.Count > 0 Then
            RestoreList.Clear()

            For Each oData In lstGames.SelectedItems
                If oRemoteBackupData.Contains(oData.Value) Then
                    oGame = DirectCast(AppData(oData.Key), clsGame)
                    RestoreList.Add(oGame)
                End If
            Next

            If RestoreList.Count = 1 Then
                bDoRestore = True
                If Not mgrRestore.CheckManifest(RestoreList(0).Name) Then
                    sMsg = RestoreList(0).Name & " is already up to date." & vbCrLf & vbCrLf & "Would you like to restore this backup anyway?"
                Else
                    sMsg = "Are you sure you want to restore the backup for " & RestoreList(0).Name & "?  This will close the form."
                End If
            ElseIf RestoreList.Count > 1 Then
                bDoRestore = True
                sMsg = "Are you sure you want to restore the backups for " & RestoreList.Count & " games?  This will close the form."
            Else
                MsgBox("The selected game(s) have no backup data.", MsgBoxStyle.Information, "Game Backup Monitor")
            End If

            'We need this check in case a bunch of games with no backups are multi-selected
            If bDoRestore Then
                If MsgBox(sMsg, MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    TriggerRestore = True
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub frmGameManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

    Private Sub btnDeleteBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteBackup.Click
        DeleteBackup()
    End Sub

    Private Sub btnMarkAsRestored_Click(sender As Object, e As EventArgs) Handles btnMarkAsRestored.Click
        MarkAsRestored()
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        TriggerSelectedRestore()
    End Sub

    Private Sub optGamesFilter_CheckedChanged(sender As Object, e As EventArgs) Handles optPendingRestores.CheckedChanged, optAllGames.CheckedChanged
        lstGames.ClearSelected()
        eCurrentMode = eModes.Disabled
        ModeChange()
        LoadData()
    End Sub

End Class