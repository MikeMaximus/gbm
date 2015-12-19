'Name: frmMain
'Description: Game Backup Monitor Main Screen
'Author: Michael J. Seiferling
Public Class frmMain

    'Used to denote the current status of the app
    Private Enum eStatus As Integer
        Running = 1
        Paused = 2
        Stopped = 3
    End Enum

    'Used to denote the currently running operation
    Private Enum eOperation As Integer
        None = 1
        Backup = 2
        Restore = 3
    End Enum

    Private eCurrentStatus As eStatus = eStatus.Stopped
    Private eCurrentOperation As eOperation = eOperation.None
    Private bCancelledByUser As Boolean = False
    Private bShutdown As Boolean = False
    Private bMenuEnabled As Boolean = True
    Private bLockdown As Boolean = True
    Private bFirstRun As Boolean = False
    Private bProcessIsAdmin As Boolean = False
    Private bLogToggle As Boolean = False
    Private bAllowIcon As Boolean = False
    Private bAllowDetails As Boolean = False
    Private oPriorImage As Image
    Private sPriorPath As String
    Private sPriorCompany As String
    Private sPriorVersion As String

    WithEvents oFileWatcher As New System.IO.FileSystemWatcher
    WithEvents tmScanTimer As New Timer

    Public WithEvents oProcess As New mgrProcesses
    Public WithEvents oBackup As New mgrBackup
    Public WithEvents oRestore As New mgrRestore
    Public hshScanList As Hashtable
    Public oSettings As New mgrSettings

    Delegate Sub UpdateLogCallBack(ByVal sLogUpdate As String, ByVal bTrayUpdate As Boolean, ByVal objIcon As System.Windows.Forms.ToolTipIcon, ByVal bTimeStamp As Boolean)
    Delegate Sub WorkingGameInfoCallBack(ByVal sTitle As String, ByVal sStatus1 As String, ByVal sStatus2 As String, ByVal sStatus3 As String)
    Delegate Sub UpdateStatusCallBack(ByVal sStatus As String)
    Delegate Sub SetLastActionCallBack(ByVal sString As String)
    Delegate Sub OperationEndedCallBack()
    Delegate Sub RestoreCompletedCallBack()

    'Handlers
    Private Sub SetLastAction(ByVal sMessage As String) Handles oBackup.SetLastAction, oRestore.SetLastAction
        'Thread Safe
        If lblLastAction.InvokeRequired = True Then
            Dim d As New SetLastActionCallBack(AddressOf SetLastAction)
            Me.Invoke(d, New Object() {sMessage})
        Else
            Dim sPattern As String = "h:mm tt"
            lblLastActionTitle.Visible = True
            lblLastAction.Text = sMessage & " at " & TimeOfDay.ToString(sPattern)
        End If
    End Sub

    Private Sub SetRestoreInfo(ByVal oRestoreInfo As clsBackup) Handles oRestore.UpdateRestoreInfo
        Dim sStatus1 As String
        Dim sStatus2 As String
        Dim sStatus3 As String

        'Build Info
        sStatus1 = IO.Path.GetFileName(oRestoreInfo.FileName)
        sStatus2 = mgrCommon.FormatString(My.Resources.frmMain_UpdatedBy, New String() {oRestoreInfo.UpdatedBy, oRestoreInfo.DateUpdated})
        If oRestoreInfo.AbsolutePath Then
            sStatus3 = oRestoreInfo.RestorePath
        Else
            sStatus3 = oRestoreInfo.RelativeRestorePath
        End If

        WorkingGameInfo(My.Resources.frmMain_RestoreInProgress, sStatus1, sStatus2, sStatus3)
        UpdateStatus(My.Resources.frmMain_RestoreInProgress)
    End Sub

    Private Sub SetBackupInfo(ByVal oGame As clsGame) Handles oBackup.UpdateBackupInfo
        Dim sStatus1 As String
        Dim sStatus2 As String
        Dim sStatus3 As String

        'Build Info
        sStatus1 = oGame.CroppedName
        If oGame.AbsolutePath Then
            sStatus2 = oGame.Path
        Else
            sStatus2 = oGame.ProcessPath & "\" & oGame.Path
        End If
        sStatus3 = String.Empty

        WorkingGameInfo(My.Resources.frmMain_BackupInProgress, sStatus1, sStatus2, sStatus3)
        UpdateStatus(My.Resources.frmMain_BackupInProgress)
    End Sub

    Private Sub OperationStarted(Optional ByVal bPause As Boolean = True)
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New OperationEndedCallBack(AddressOf OperationEnded)
            Me.Invoke(d, New Object() {})
        Else
            btnCancelOperation.Visible = True            
            LockDownMenuEnable()
            If bPause Then PauseScan()
        End If
    End Sub

    Private Sub OperationEnded(Optional ByVal bResume As Boolean = True)
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New OperationEndedCallBack(AddressOf OperationEnded)
            Me.Invoke(d, New Object() {})
        Else
            ResetGameInfo(True)
            btnCancelOperation.Visible = False
            btnCancelOperation.Enabled = True
            eCurrentOperation = eOperation.None
            LockDownMenuEnable()
            If bResume Then ResumeScan()
        End If
    End Sub

    Private Sub OperationCancel()
        Select Case eCurrentOperation
            Case eOperation.None
                'Nothing
            Case eOperation.Backup
                oBackup.CancelOperation = True
                btnCancelOperation.Enabled = False
            Case eOperation.Restore
                oRestore.CancelOperation = True
                btnCancelOperation.Enabled = False
        End Select
    End Sub

    Private Sub ExecuteBackup(ByVal oBackupList As List(Of clsGame))
        'Init Backup Settings
        oBackup.Settings = oSettings
        oBackup.DoBackup(oBackupList)        
        OperationEnded()
    End Sub

    Private Sub ExecuteRestore(ByVal oRestoreList As List(Of clsBackup))
        'Init Restore Settings
        oRestore.Settings = oSettings
        oRestore.DoRestore(oRestoreList)
        OperationEnded()
    End Sub

    Private Sub RunRestore(ByVal oRestoreList As List(Of clsGame))
        Dim oBackupData As SortedList = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)
        Dim oGame As clsGame
        Dim oReadyList As New List(Of clsBackup)
        Dim oRestoreInfo As clsBackup
        Dim bTriggerReload As Boolean = False

        eCurrentOperation = eOperation.Restore
        OperationStarted()

        'Build Restore List
        For Each oGame In oRestoreList
            oRestoreInfo = oBackupData(oGame.Name)

            If mgrRestore.CheckPath(oRestoreInfo, oGame, bTriggerReload) Then                
                oReadyList.Add(oRestoreInfo)
            Else
                UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_ErrorRestorePath, oRestoreInfo.Name), False, ToolTipIcon.Error, True)
            End If
        Next

        'Reload the monitor list if any game data was changed during the path checks
        If bTriggerReload Then LoadGameSettings()

        'Run restores
        If oReadyList.Count > 0 Then
            Dim oThread As New System.Threading.Thread(AddressOf ExecuteRestore)
            oThread.IsBackground = True
            oThread.Start(oReadyList)
        Else
            OperationEnded()
        End If

    End Sub

    Private Sub RunManualBackup(ByVal oBackupList As List(Of clsGame))
        Dim oGame As clsGame
        Dim bNoAuto As Boolean
        Dim oReadyList As New List(Of clsGame)

        eCurrentOperation = eOperation.Backup
        OperationStarted()

        'Build Backup List
        For Each oGame In oBackupList
            bNoAuto = False
            gMonStripStatusButton.Enabled = False

            UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_ManualBackup, oGame.Name), False)

            If oGame.AbsolutePath = False Then
                If oGame.ProcessPath = String.Empty Then
                    If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                    oGame.ProcessPath = mgrPath.ProcessPathSearch(oGame.Name, oGame.TrueProcess, mgrCommon.FormatString(My.Resources.frmMain_ErrorRelativePath, oGame.Name), bNoAuto)
                End If

                If oGame.ProcessPath <> String.Empty Then
                    oReadyList.Add(oGame)
                Else
                    UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_ErrorBackupUnknownPath, oGame.Name), True, ToolTipIcon.Error, True)
                End If
            Else
                oReadyList.Add(oGame)
            End If
        Next

        'Run backups
        If oReadyList.Count > 0 Then
            Dim oThread As New System.Threading.Thread(AddressOf ExecuteBackup)
            oThread.IsBackground = True
            oThread.Start(oReadyList)
        Else
            OperationEnded()
        End If

    End Sub

    Private Function DoMultiGameCheck() As Boolean
        Dim oResult As DialogResult

        If oProcess.Duplicate = True Then
            Dim frm As New frmChooseGame
            frm.Process = oProcess
            oResult = frm.ShowDialog()
            If oResult = DialogResult.OK Then
                Dim sProcessPath As String
                'Reload settings
                LoadGameSettings()
                'Retain the process path from old object
                sProcessPath = oProcess.GameInfo.ProcessPath
                oProcess.GameInfo = frm.Game
                'Set the process path into the new object
                oProcess.GameInfo.ProcessPath = sProcessPath
                'A game was set, return and continue
                Return True
            Else
                'No game was set, return to cancel
                Return False
            End If
        Else
            'The game is not a duplicate, return and continue
            Return True
        End If

    End Function

    Private Sub RunBackup()
        Dim bDoBackup As Boolean
        Dim oReadyList As New List(Of clsGame)

        eCurrentOperation = eOperation.Backup
        OperationStarted(False)

        If SupressBackup() Then
            bDoBackup = False
            UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_ErrorBackupSessionLength, oProcess.GameInfo.Name), False)
            SetLastAction(mgrCommon.FormatString(My.Resources.frmMain_ErrorBackupSessionLength, oProcess.GameInfo.CroppedName))
            OperationEnded()
        Else
            If oProcess.GameInfo.MonitorOnly = False Then
                If oSettings.DisableConfirmation Then
                    bDoBackup = True
                Else
                    If mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ConfirmBackup, oProcess.GameInfo.Name), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        bDoBackup = True
                    Else
                        bDoBackup = False
                        UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_ErrorBackupCancel, oProcess.GameInfo.Name), False)
                        SetLastAction(mgrCommon.FormatString(My.Resources.frmMain_ErrorBackupCancel, oProcess.GameInfo.CroppedName))
                        OperationEnded()
                    End If
                End If
            Else
                bDoBackup = False
                UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_MonitorEnded, oProcess.GameInfo.Name), False)
                SetLastAction(mgrCommon.FormatString(My.Resources.frmMain_MonitorEnded, oProcess.GameInfo.CroppedName))
                OperationEnded()
            End If
        End If

        If bDoBackup Then
            'Run the backup
            oReadyList.Add(oProcess.GameInfo)
            Dim trd As New System.Threading.Thread(AddressOf ExecuteBackup)
            trd.IsBackground = True
            trd.Start(oReadyList)
        End If
    End Sub

    Private Sub CheckRestore()
        Dim slRestoreData As SortedList = mgrRestore.CompareManifests()
        Dim sNotification As String

        If slRestoreData.Count > 0 Then
            If slRestoreData.Count > 1 Then
                sNotification = mgrCommon.FormatString(My.Resources.frmMain_NewSaveNotificationMulti, slRestoreData.Count)
            Else
                sNotification = mgrCommon.FormatString(My.Resources.frmMain_NewSaveNotificationSingle, slRestoreData.Count)
            End If
            gMonNotification.Image = My.Resources.Inbox
            gMonTrayNotification.Image = My.Resources.Inbox
            gMonNotification.Text = sNotification
            gMonTrayNotification.Text = sNotification
            gMonNotification.Visible = True
            gMonTrayNotification.Visible = True
        End If
    End Sub

    'Functions handling the display of game information
    Private Sub SetIcon()
        Dim sIcon As String
        Dim fbBrowser As New OpenFileDialog

        fbBrowser.Title = mgrCommon.FormatString(My.Resources.frmMain_ChooseIcon, oProcess.GameInfo.CroppedName)
        fbBrowser.DefaultExt = "ico"
        fbBrowser.Filter = My.Resources.frmMain_IconFilter
        Try
            fbBrowser.InitialDirectory = IO.Path.GetDirectoryName(oProcess.FoundProcess.MainModule.FileName)
        Catch ex As Exception
            fbBrowser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End Try
        fbBrowser.Multiselect = False

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            sIcon = fbBrowser.FileName
            If IO.File.Exists(sIcon) Then
                oProcess.GameInfo.Icon = sIcon
                pbIcon.Image = Image.FromFile(sIcon)
                mgrMonitorList.DoListUpdate(oProcess.GameInfo)
            End If
        End If
    End Sub

    Private Sub ResetGameInfo(Optional ByVal bKeepInfo As Boolean = False)
        If bKeepInfo And Not oProcess.GameInfo Is Nothing Then
            lblGameTitle.Text = mgrCommon.FormatString(My.Resources.frmMain_LastGame, oProcess.GameInfo.CroppedName)
            pbIcon.Image = oPriorImage
            lblStatus1.Text = sPriorPath
            lblStatus2.Text = sPriorCompany
            lblStatus3.Text = sPriorVersion
            If oSettings.TimeTracking Then
                pbTime.Visible = True
                lblTimeSpent.Visible = True
            End If
        Else
            pbIcon.Image = My.Resources.Searching
            lblGameTitle.Text = My.Resources.frmMain_NoGameDetected
            lblStatus1.Text = String.Empty
            lblStatus2.Text = String.Empty
            lblStatus3.Text = String.Empty
            pbTime.Visible = False
            lblTimeSpent.Visible = False
        End If

        If eCurrentStatus = eStatus.Stopped Then
            UpdateStatus(My.Resources.frmMain_NotScanning)
        Else
            UpdateStatus(My.Resources.frmMain_NoGameDetected)
        End If

    End Sub

    Private Sub WorkingGameInfo(ByVal sTitle As String, ByVal sStatus1 As String, ByVal sStatus2 As String, ByVal sStatus3 As String)
        'Thread Safe (If one control requires an invoke assume they all do)
        If pbIcon.InvokeRequired = True Then
            Dim d As New WorkingGameInfoCallBack(AddressOf WorkingGameInfo)
            Me.Invoke(d, New Object() {sTitle, sStatus1, sStatus2, sStatus3})
        Else
            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = My.Resources.Working
            lblGameTitle.Text = sTitle
            lblStatus1.Text = sStatus1
            lblStatus2.Text = sStatus2
            lblStatus3.Text = sStatus3
        End If
    End Sub

    Private Sub SetGameInfo(Optional ByVal bMulti As Boolean = False)
        Dim sFileName As String = String.Empty
        Dim sFileVersion As String = String.Empty
        Dim sCompanyName As String = String.Empty

        'Wipe Game Info
        lblStatus1.Text = String.Empty
        lblStatus2.Text = String.Empty
        lblStatus3.Text = String.Empty

        'Get Game Details 
        If bMulti Then
            bAllowIcon = False
            bAllowDetails = False
            lblGameTitle.Text = My.Resources.frmMain_MultipleGames
            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = My.Resources.Unknown
            lblStatus1.Text = My.Resources.frmMain_NoDetails
        Else
            bAllowIcon = True
            bAllowDetails = True
            lblGameTitle.Text = oProcess.GameInfo.CroppedName

            Try
                Dim ic As Icon = System.Drawing.Icon.ExtractAssociatedIcon(oProcess.FoundProcess.MainModule.FileName)
                pbIcon.Image = ic.ToBitmap

                'Set Game Details
                sFileName = oProcess.FoundProcess.MainModule.FileName
                sFileVersion = oProcess.FoundProcess.MainModule.FileVersionInfo.FileVersion
                sCompanyName = oProcess.FoundProcess.MainModule.FileVersionInfo.CompanyName

            Catch ex As Exception
                pbIcon.Image = My.Resources.Unknown
            End Try

            'Check for a custom icon & details            
            If IO.File.Exists(oProcess.GameInfo.Icon) Then
                pbIcon.Image = Image.FromFile(oProcess.GameInfo.Icon)
            End If
            If sFileName = String.Empty Then
                If oProcess.GameInfo.ProcessPath <> String.Empty Then
                    sFileName = mgrCommon.FormatString(My.Resources.frmMain_ExePath, oProcess.GameInfo.ProcessPath)
                End If
            End If
            If oProcess.GameInfo.Version <> String.Empty Then
                sFileVersion = oProcess.GameInfo.Version
            End If
            If oProcess.GameInfo.Company <> String.Empty Then
                sCompanyName = oProcess.GameInfo.Company
            End If

            'Do Time Update
            If oSettings.TimeTracking Then
                UpdateTimeSpent(oProcess.GameInfo.Hours, 0)
            Else
                pbTime.Visible = False
                lblTimeSpent.Visible = False
            End If

            'Set Details
            If sFileName = String.Empty Then
                lblStatus1.Text = My.Resources.frmMain_NotAvailable
            Else
                lblStatus1.Text = sFileName
            End If

            If sCompanyName = String.Empty Then
                lblStatus2.Text = My.Resources.frmMain_NotAvailable
            Else
                lblStatus2.Text = sCompanyName
            End If

            If sFileVersion = String.Empty Then
                lblStatus3.Text = My.Resources.frmMain_NotAvailable
            Else
                lblStatus3.Text = sFileVersion
            End If

        End If

        'Set Prior Info
        oPriorImage = pbIcon.Image
        sPriorPath = lblStatus1.Text
        sPriorCompany = lblStatus2.Text
        sPriorVersion = lblStatus3.Text
    End Sub

    Private Sub UpdateTimeSpent(ByVal dTotalTime As Double, ByVal dSessionTime As Double)
        Dim sTotalTime As String
        Dim sSessionTime As String

        If dTotalTime < 1 Then
            sTotalTime = mgrCommon.FormatString(My.Resources.frmMain_SessionMinutes, Math.Round((dTotalTime * 100) * 0.6).ToString)
        Else
            sTotalTime = mgrCommon.FormatString(My.Resources.frmMain_SessionHours, Math.Round(dTotalTime, 1).ToString)
        End If

        If dSessionTime < 1 Then
            sSessionTime = mgrCommon.FormatString(My.Resources.frmMain_SessionMinutes, Math.Round((dSessionTime * 100) * 0.6).ToString)
        Else
            sSessionTime = mgrCommon.FormatString(My.Resources.frmMain_SessionHours, Math.Round(dSessionTime, 1).ToString)
        End If

        If dSessionTime > 0 Then
            lblTimeSpent.Text = sSessionTime & " (" & sTotalTime & ")"
        Else
            lblTimeSpent.Text = sTotalTime
        End If

        pbTime.Visible = True
        lblTimeSpent.Visible = True
    End Sub

    Private Sub HandleTimeSpent()
        Dim dCurrentHours As Double

        If oProcess.GameInfo.Hours > 0 Then
            dCurrentHours = oProcess.GameInfo.Hours
            dCurrentHours = dCurrentHours + oProcess.TimeSpent.TotalHours
        Else
            dCurrentHours = oProcess.TimeSpent.TotalHours
        End If

        oProcess.GameInfo.Hours = Math.Round(dCurrentHours, 5)

        'Update original object with the new hours without reloading entire list. 
        If hshScanList.Contains(oProcess.GameInfo.ProcessName) Then
            DirectCast(hshScanList.Item(oProcess.GameInfo.ProcessName), clsGame).Hours = oProcess.GameInfo.Hours
        End If

        mgrMonitorList.DoListUpdate(oProcess.GameInfo)
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()

        UpdateTimeSpent(dCurrentHours, oProcess.TimeSpent.TotalHours)
    End Sub

    Private Function SupressBackup() As Boolean
        Dim iSession As Integer
        If oSettings.SupressBackup Then
            iSession = Math.Ceiling(oProcess.TimeSpent.TotalMinutes)
            If iSession > oSettings.SupressBackupThreshold Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    'Functions handling the opening of other windows
    Private Sub OpenAbout()
        Dim iProcessType As System.Reflection.ProcessorArchitecture = System.Reflection.AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture
        Dim sVersion As String = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor
        Dim sProcessType = [Enum].GetName(GetType(System.Reflection.ProcessorArchitecture), iProcessType)
        Dim sRevision As String = My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
        Dim sConstCopyright As String = Chr(169) & My.Resources.AppCopyright

        mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.AppAbout, New String() {sVersion, sProcessType, sRevision, sConstCopyright}), MsgBoxStyle.Information)
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmTags
        PauseScan()
        frm.ShowDialog()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
        ResumeScan()
    End Sub

    Private Sub OpenGameManager(Optional ByVal bPendingRestores As Boolean = False)
        Dim frm As New frmGameManager
        PauseScan()
        frm.BackupFolder = oSettings.BackupFolder
        frm.PendingRestores = bPendingRestores
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
        ResumeScan()

        'Handle backup trigger
        If frm.TriggerBackup Then
            RunManualBackup(frm.BackupList)
        End If

        'Handle restore trigger
        If frm.TriggerRestore Then
            RunRestore(frm.RestoreList)
        End If
    End Sub

    Private Sub OpenSettings()
        Dim frm As New frmSettings
        frm.Settings = oSettings
        PauseScan()
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            oSettings = frm.Settings
            'Set Remote Database Location
            mgrPath.RemoteDatabaseLocation = oSettings.BackupFolder
            SetupSyncWatcher()
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub OpenGameWizard()
        Dim frm As New frmAddWizard
        PauseScan()
        frm.GameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
        ResumeScan()
    End Sub

    Private Sub OpenCustomVariables()
        Dim frm As New frmVariableManager
        PauseScan()
        frm.ShowDialog()
        mgrPath.CustomVariablesReload()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
        ResumeScan()
    End Sub

    Private Sub OpenStartupWizard()
        Dim frm As New frmStartUpWizard()
        frm.Settings = oSettings
        PauseScan()
        frm.ShowDialog()
        LoadAndVerify()
        bFirstRun = False
        ResumeScan()
    End Sub

    Private Sub OpenWebSite()
        Process.Start(mgrPath.OfficialWebURL)
    End Sub

    Private Sub OpenOnlineManual()
        Process.Start(mgrPath.OfficialManualURL)
    End Sub

    Private Sub OpenCheckforUpdates()
        Process.Start(mgrPath.OfficialUpdatesURL)
    End Sub

    Private Sub CheckForNewBackups()
        If oSettings.RestoreOnLaunch Then
            CheckRestore()
        End If
    End Sub

    'Functions handling the loading/sync of settings    
    Private Sub LoadGameSettings()
        'Load Monitor List
        hshScanList = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.ScanList)

        UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_GameListLoaded, hshScanList.Keys.Count), False)
    End Sub

    Private Sub StartSyncWatcher()
        If oSettings.Sync Then
            oFileWatcher.EnableRaisingEvents = True
        End If
    End Sub

    Private Sub StopSyncWatcher()
        If oSettings.Sync Then
            oFileWatcher.EnableRaisingEvents = False
        End If
    End Sub

    Private Sub SetupSyncWatcher()
        If oSettings.Sync Then
            oFileWatcher.Path = oSettings.BackupFolder
            oFileWatcher.Filter = "gbm.s3db"
            oFileWatcher.NotifyFilter = IO.NotifyFilters.LastWrite
        End If
    End Sub

    Private Sub HandleSyncWatcher() Handles oFileWatcher.Changed
        If oSettings.Sync Then
            UpdateLog(My.Resources.frmMain_MasterListChanged, False, ToolTipIcon.Info, True)
            SyncGameSettings()
            LoadGameSettings()
            CheckForNewBackups()
        End If
    End Sub

    Private Sub SyncGameSettings()
        'Sync Monitor List
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(False)
    End Sub

    Private Sub LocalDatabaseCheck()
        Dim oLocalDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        oLocalDatabase.DatabaseUpgrade()
    End Sub

    Private Sub RemoteDatabaseCheck()
        Dim oRemoteDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        oRemoteDatabase.DatabaseUpgrade()
    End Sub

    Private Sub LoadAndVerify()

        'The application cannot continue if this fails
        If Not oBackup.CheckForUtilities(mgrPath.Utility7zLocation) Then
            mgrCommon.ShowMessage(My.Resources.frmMain_Error7zip, MsgBoxStyle.Critical)
            bShutdown = True
            Me.Close()
        End If

        'Local Database Check
        VerifyDBVersion(mgrSQLite.Database.Local)
        LocalDatabaseCheck()

        'Load Settings
        oSettings.LoadSettings()

        If Not bFirstRun Then
            'The application cannot continue if this fails
            If Not VerifyBackupLocation() Then
                bShutdown = True
                Me.Close()
            End If

            'Remote Database Check
            VerifyDBVersion(mgrSQLite.Database.Remote)
            RemoteDatabaseCheck()

            'Sync Game Settings
            SyncGameSettings()
        End If

        'Setup Sync Watcher
        SetupSyncWatcher()

        'Load Game Settings
        LoadGameSettings()

        'Verify the "Start with Windows" setting
        If oSettings.StartWithWindows Then
            If Not VerifyStartWithWindows() Then
                UpdateLog(My.Resources.frmMain_ErrorAppLocationChanged, False, ToolTipIcon.Info)
            End If
        End If

    End Sub

    'Functions that handle buttons, menus and other GUI features on this form
    Private Sub ToggleLog()
        If bLogToggle = False Then
            txtLog.Visible = True
            Me.Size = New System.Drawing.Size(540, 425)
            bLogToggle = True
            btnLogToggle.Text = My.Resources.frmMain_btnToggleLog_Hide
            txtLog.Select(txtLog.TextLength, 0)
            txtLog.ScrollToCaret()
        Else
            txtLog.Visible = False
            Me.Size = New System.Drawing.Size(540, 245)
            bLogToggle = False
            btnLogToggle.Text = My.Resources.frmMain_btnToggleLog_Show
        End If
    End Sub

    Private Sub ToggleState()
        'Toggle State with Tray Clicks
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Visible = True
            Me.ShowInTaskbar = True
            Me.WindowState = FormWindowState.Normal
            Me.Focus()
        Else
            Me.Visible = False
            Me.ShowInTaskbar = False
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub ScanToggle()
        Select Case eCurrentStatus
            Case eStatus.Running
                HandleScan()
            Case eStatus.Paused
                Dim sGame As String = oProcess.GameInfo.CroppedName

                If bProcessIsAdmin Then
                    mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ErrorAdminDetect, sGame), MsgBoxStyle.Exclamation)
                    RestartAsAdmin()
                    Exit Sub
                End If

                If oProcess.Duplicate Then
                    sGame = My.Resources.frmMain_UnknownGame
                End If

                If mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ConfirmMonitorCancel, sGame), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_MonitorCancel, sGame), False)
                    SetLastAction(mgrCommon.FormatString(My.Resources.frmMain_MonitorCancel, sGame))

                    bwMonitor.CancelAsync()
                    StopScan()

                    If oProcess.Duplicate Then
                        ResetGameInfo()
                    Else
                        ResetGameInfo(True)
                    End If
                End If
            Case eStatus.Stopped
                HandleScan()
        End Select
    End Sub

    Private Sub ShutdownApp(Optional ByVal bPrompt As Boolean = True)
        Dim bClose As Boolean = False

        If bPrompt Then
            If mgrCommon.ShowMessage(My.Resources.AppExit, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                bClose = True
            End If
        Else
            bClose = True
        End If

        If bClose Then
            'Close Application
            bShutdown = True
            tmScanTimer.Stop()
            If bwMonitor.IsBusy() Then bwMonitor.CancelAsync()
            Me.Close()
        End If
    End Sub

    Private Sub ToggleMenuItems(ByVal bEnable As Boolean, ByVal oDropDownItems As ToolStripMenuItem)
        'Control names are exempt from being disabled
        Dim sExempt() As String = {"gMonFileMonitor", "gMonFileExit"}
        Dim oExempt As New List(Of String)(sExempt)
        For Each mi As Object In oDropDownItems.DropDownItems
            If mi.GetType().Equals(GetType(ToolStripMenuItem)) Then
                mi = DirectCast(mi, ToolStripMenuItem)
                If Not oExempt.Contains(mi.Name) Then
                    mi.Enabled = bEnable
                End If
            End If
        Next
    End Sub

    Private Sub LockDownMenuEnable()
        If bLockdown Then
            gMonStripStatusButton.Enabled = False
            gMonFileMonitor.Enabled = False
            gMonFileExit.Enabled = False
            gMonTrayMon.Enabled = False
            gMonTrayExit.Enabled = False
            bLockdown = False
        Else
            gMonStripStatusButton.Enabled = True
            gMonFileMonitor.Enabled = True
            gMonFileExit.Enabled = True
            gMonTrayMon.Enabled = True
            gMonTrayExit.Enabled = True
            bLockdown = True
        End If
    End Sub

    Private Sub ToggleMenuEnable()
        If bMenuEnabled Then
            ToggleMenuItems(False, gMonFile)
            ToggleMenuItems(False, gMonSetup)
            ToggleMenuItems(False, gMonSetupAddWizard)
            ToggleMenuItems(False, gMonHelp)
            ToggleMenuItems(False, gMonTools)
            ToggleMenuItems(False, gMonTraySetup)
            ToggleMenuItems(False, gMonTrayTools)
            gMonNotification.Enabled = False
            gMonTrayNotification.Enabled = False
            gMonTraySettings.Enabled = False
            bMenuEnabled = False
        Else
            ToggleMenuItems(True, gMonFile)
            ToggleMenuItems(True, gMonSetup)
            ToggleMenuItems(True, gMonSetupAddWizard)
            ToggleMenuItems(True, gMonHelp)
            ToggleMenuItems(True, gMonTools)
            ToggleMenuItems(True, gMonTraySetup)
            ToggleMenuItems(True, gMonTrayTools)
            gMonNotification.Enabled = True
            gMonTrayNotification.Enabled = True
            gMonTraySettings.Enabled = True
            bMenuEnabled = True
        End If
    End Sub

    Private Sub ToggleMenuText()
        Select Case eCurrentStatus
            Case eStatus.Running
                gMonFileMonitor.Text = My.Resources.frmMain_gMonFileMonitor_Stop
                gMonTrayMon.Text = My.Resources.frmMain_gMonFileMonitor_Stop
            Case eStatus.Stopped
                gMonFileMonitor.Text = My.Resources.frmMain_gMonFileMonitor_Start
                gMonTrayMon.Text = My.Resources.frmMain_gMonFileMonitor_Start
            Case eStatus.Paused
                gMonFileMonitor.Text = My.Resources.frmMain_gMonFileMonitor_Cancel
                gMonTrayMon.Text = My.Resources.frmMain_gMonFileMonitor_Cancel
        End Select
    End Sub

    Public Sub UpdateStatus(sStatus As String)
        'Thread Safe (If one control requires an invoke assume they all do)
        If gMonStatusStrip.InvokeRequired = True Then
            Dim d As New UpdateStatusCallBack(AddressOf UpdateStatus)
            Me.Invoke(d, New Object() {sStatus})
        Else
            gMonStripTxtStatus.Text = sStatus
            gMonTray.Text = sStatus
        End If
    End Sub

    Public Sub UpdateLog(sLogUpdate As String, Optional bTrayUpdate As Boolean = True, Optional objIcon As System.Windows.Forms.ToolTipIcon = ToolTipIcon.Info, Optional bTimeStamp As Boolean = True) Handles oBackup.UpdateLog, oRestore.UpdateLog
        'Thread Safe (If one control requires an invoke assume they all do)
        If txtLog.InvokeRequired = True Then
            Dim d As New UpdateLogCallBack(AddressOf UpdateLog)
            Me.Invoke(d, New Object() {sLogUpdate, bTrayUpdate, objIcon, bTimeStamp})
        Else
            'Clear the log if we are approaching the limit
            If txtLog.TextLength > 32000 Then
                txtLog.Text = String.Empty
            End If

            'We shouldn't allow any one message to be greater than 255 characters if that same message is pushed to the tray icon
            If sLogUpdate.Length > 255 And bTrayUpdate Then
                sLogUpdate = sLogUpdate.Substring(0, 252) & "..."
            End If

            If txtLog.Text <> String.Empty Then
                txtLog.AppendText(vbCrLf)
            End If

            If bTimeStamp Then
                txtLog.AppendText("[" & Date.Now & "] " & sLogUpdate)
            Else
                txtLog.AppendText(sLogUpdate)
            End If

            txtLog.Select(txtLog.TextLength, 0)
            txtLog.ScrollToCaret()
            gMonTray.BalloonTipText = sLogUpdate
            gMonTray.BalloonTipIcon = objIcon
            If bTrayUpdate Then gMonTray.ShowBalloonTip(5000)
        End If
        Application.DoEvents()
    End Sub

    Private Sub SetForm()
        'Set Menu Text
        gMonFile.Text = My.Resources.frmMain_gMonFile
        gMonFileMonitor.Text = My.Resources.frmMain_gMonFileMonitor_Start
        gMonFileSettings.Text = My.Resources.frmMain_gMonFileSettings
        gMonFileExit.Text = My.Resources.frmMain_gMonFileExit
        gMonSetup.Text = My.Resources.frmMain_gMonSetup
        gMonSetupGameManager.Text = My.Resources.frmMain_gMonSetupGameManager
        gMonSetupAddWizard.Text = My.Resources.frmMain_gMonSetupAddWizard
        gMonSetupCustomVariables.Text = My.Resources.frmMain_gMonSetupCustomVariables
        gMonSetupTags.Text = My.Resources.frmMain_gMonSetupTags
        gMonTools.Text = My.Resources.frmMain_gMonTools
        gMonToolsCleanMan.Text = My.Resources.frmMain_gMonToolsCleanMan
        gMonToolsCompact.Text = My.Resources.frmMain_gMonToolsCompact
        gMonHelp.Text = My.Resources.frmMain_gMonHelp
        gMonHelpWebSite.Text = My.Resources.frmMain_gMonHelpWebSite
        gMonHelpManual.Text = My.Resources.frmMain_gMonHelpManual
        gMonHelpCheckforUpdates.Text = My.Resources.frmMain_gMonHelpCheckForUpdates
        gMonHelpAbout.Text = My.Resources.frmMain_gMonHelpAbout

        'Set Tray Menu Text
        gMonTrayShow.Text = My.Resources.frmMain_gMonTrayShow
        gMonTrayMon.Text = My.Resources.frmMain_gMonFileMonitor_Start
        gMonTraySettings.Text = My.Resources.frmMain_gMonFileSettings
        gMonTraySetup.Text = My.Resources.frmMain_gMonSetup
        gMonTraySetupGameManager.Text = My.Resources.frmMain_gMonSetupGameManager
        gMonTraySetupAddWizard.Text = My.Resources.frmMain_gMonSetupAddWizard
        gMonTraySetupCustomVariables.Text = My.Resources.frmMain_gMonSetupCustomVariables
        gMonTraySetupTags.Text = My.Resources.frmMain_gMonSetupTags
        gMonTrayTools.Text = My.Resources.frmMain_gMonTools
        gMonTrayToolsCleanMan.Text = My.Resources.frmMain_gMonToolsCleanMan
        gMonTrayToolsCompact.Text = My.Resources.frmMain_gMonToolsCompact
        gMonTrayExit.Text = My.Resources.frmMain_gMonFileExit

        'Set Form Text
        lblLastActionTitle.Text = My.Resources.frmMain_lblLastActionTitle
        btnCancelOperation.Text = My.Resources.frmMain_btnCancelOperation
        gMonStripStatusButton.Text = My.Resources.frmMain_gMonStripStatusButton

        If mgrCommon.IsElevated Then
            gMonStripAdminButton.Image = My.Resources.Admin
            gMonStripAdminButton.ToolTipText = My.Resources.frmMain_RunningAsAdmin

        Else
            gMonStripAdminButton.Image = My.Resources.User
            gMonStripAdminButton.ToolTipText = My.Resources.frmMain_RunningAsNormal
        End If
        btnCancelOperation.Visible = False
        txtLog.Visible = False
        lblLastActionTitle.Visible = False
        lblLastAction.Text = String.Empty
        pbTime.SizeMode = PictureBoxSizeMode.AutoSize
        pbTime.Image = My.Resources.Clock
        Me.Size = New System.Drawing.Size(540, 245)
        AddHandler mgrMonitorList.UpdateLog, AddressOf UpdateLog
        ResetGameInfo()
    End Sub

    'Functions that control the scanning for games
    Private Sub StartScan()
        tmScanTimer.Interval = 5000
        tmScanTimer.Start()
    End Sub

    Private Sub HandleScan()
        If eCurrentStatus = eStatus.Running Then
            StopSyncWatcher()
            tmScanTimer.Stop()
            eCurrentStatus = eStatus.Stopped
            UpdateStatus(My.Resources.frmMain_NotScanning)
            gMonStripStatusButton.Image = My.Resources.Stopped
            gMonTray.Icon = My.Resources.GBM_Tray_Stopped
        Else
            StartScan()
            StartSyncWatcher()
            eCurrentStatus = eStatus.Running
            UpdateStatus(My.Resources.frmMain_NoGameDetected)
            gMonStripStatusButton.Image = My.Resources.Ready
            gMonTray.Icon = My.Resources.GBM_Tray_Ready
        End If
        ToggleMenuText()
    End Sub

    Private Sub PauseScan()
        If eCurrentStatus = eStatus.Running Then
            StopSyncWatcher()
            tmScanTimer.Stop()
            eCurrentStatus = eStatus.Paused
            UpdateStatus(My.Resources.frmMain_NotScanning)
            gMonStripStatusButton.Image = My.Resources.Detected
            gMonTray.Icon = My.Resources.GBM_Tray_Detected
        End If
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    Private Sub ResumeScan()
        If eCurrentStatus = eStatus.Running Or eCurrentStatus = eStatus.Paused Then
            StartScan()
            StartSyncWatcher()
            eCurrentStatus = eStatus.Running
            gMonStripStatusButton.Image = My.Resources.Ready
            gMonTray.Icon = My.Resources.GBM_Tray_Ready
            UpdateStatus(My.Resources.frmMain_NoGameDetected)
        End If
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    Private Sub StopScan()
        StopSyncWatcher()
        tmScanTimer.Stop()
        eCurrentStatus = eStatus.Stopped
        UpdateStatus(My.Resources.frmMain_NotScanning)
        gMonStripStatusButton.Image = My.Resources.Stopped
        gMonTray.Icon = My.Resources.GBM_Tray_Stopped
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    'Functions to handle verification
    Private Sub VerifyCustomPathVariables()
        Dim sGames As String = String.Empty
        If Not mgrPath.VerifyCustomVariables(hshScanList, sGames) Then
            mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ErrorCustomVariable, sGames), MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Function VerifyBackupLocation() As Boolean
        Dim sBackupPath As String = oSettings.BackupFolder
        If mgrPath.VerifyBackupPath(sBackupPath) Then
            If oSettings.BackupFolder <> sBackupPath Then
                oSettings.BackupFolder = sBackupPath
                oSettings.SaveSettings()
                oSettings.LoadSettings()
                If oSettings.Sync Then mgrMonitorList.HandleBackupLocationChange()
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub VerifyGameDataPath()
        'Important: This function cannot access mgrPath for settings, as that will trigger a database creation and destroy the reason for this function
        Dim sSettingsRoot As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\gbm"
        Dim sDBLocation As String = sSettingsRoot & "\gbm.s3db"

        If Not IO.Directory.Exists(sSettingsRoot) Then
            Try
                IO.Directory.CreateDirectory(sSettingsRoot)
            Catch ex As Exception
                mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ErrorSettingsFolder, ex.Message), MsgBoxStyle.Critical)
                bShutdown = True
                Me.Close()
            End Try
        End If

        If Not IO.File.Exists(sDBLocation) Then bFirstRun = True
    End Sub

    Private Sub VerifyDBVersion(ByVal iDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iDB)
        Dim iDBVer As Integer

        If Not oDatabase.CheckDBVer(iDBVer) Then
            Select Case iDB
                Case mgrSQLite.Database.Local
                    mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ErrorDBVerLocal, New String() {iDBVer, mgrCommon.AppVersion}), MsgBoxStyle.Critical)
                Case mgrSQLite.Database.Remote
                    mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.frmMain_ErrorDBVerRemote, New String() {iDBVer, mgrCommon.AppVersion}), MsgBoxStyle.Critical)
            End Select

            bShutdown = True
            Me.Close()
        End If
    End Sub

    Private Function VerifyStartWithWindows() As Boolean
        Dim oKey As Microsoft.Win32.RegistryKey
        Dim sAppName As String = Application.ProductName
        Dim sAppPath As String = Application.ExecutablePath
        Dim sRegPath As String

        oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        sRegPath = oKey.GetValue(sAppName, String.Empty).ToString.Replace("""", "")
        oKey.Close()

        If sAppPath.ToLower <> sRegPath.ToLower Then
            oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            oKey.SetValue(sAppName, """" & sAppPath & """")
            oKey.Close()
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub CheckForSavedDuplicate()
        For Each o As clsGame In oProcess.DuplicateList
            If o.ProcessPath.ToLower = oProcess.GameInfo.ProcessPath.ToLower Then
                oProcess.GameInfo = o
                oProcess.Duplicate = False
            End If
        Next
    End Sub

    Private Function CheckForSavedPath() As Boolean
        If oProcess.GameInfo.ProcessPath <> String.Empty Then
            Return True
        End If

        Return False
    End Function

    'Functions to handle other features
    Private Sub RestartAsAdmin()
        If mgrCommon.IsElevated Then
            mgrCommon.ShowMessage(My.Resources.frmMain_ErrorAlreadyAdmin, MsgBoxStyle.Information)
        Else
            If mgrCommon.ShowMessage(My.Resources.frmMain_ConfirmRunAsAdmin, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrCommon.RestartAsAdmin()
                bShutdown = True
                ShutdownApp(False)
            End If
        End If
    End Sub

    Private Sub CleanLocalManifest()
        Dim slItems As SortedList

        PauseScan()

        If mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.AppConfirmClean), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            slItems = mgrRestore.SyncLocalManifest()

            If slItems.Count > 0 Then
                For Each oItem As clsBackup In slItems.Values
                    UpdateLog(mgrCommon.FormatString(My.Resources.AppCleanedGame, oItem.Name), False)
                Next
                mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.AppCleanedTotal, slItems.Count), MsgBoxStyle.Information)
            Else
                mgrCommon.ShowMessage(My.Resources.AppAlreadyClean, MsgBoxStyle.Information)
            End If
        End If

        ResumeScan()

    End Sub

    Private Sub CompactDatabases()
        Dim oLocalDatabase As mgrSQLite
        Dim oRemoteDatabase As mgrSQLite

        PauseScan()

        If mgrCommon.ShowMessage(mgrCommon.FormatString(My.Resources.AppConfirmRebuild), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            oLocalDatabase = New mgrSQLite(mgrSQLite.Database.Local)
            oRemoteDatabase = New mgrSQLite(mgrSQLite.Database.Remote)

            UpdateLog(mgrCommon.FormatString(My.Resources.AppLocalCompactInit, oLocalDatabase.GetDBSize), False)
            oLocalDatabase.CompactDatabase()
            UpdateLog(mgrCommon.FormatString(My.Resources.AppLocalCompactComplete, oLocalDatabase.GetDBSize), False)

            UpdateLog(mgrCommon.FormatString(My.Resources.AppRemoteCompactInit, oRemoteDatabase.GetDBSize), False)
            oRemoteDatabase.CompactDatabase()
            UpdateLog(mgrCommon.FormatString(My.Resources.AppRemoteCompactComplete, oRemoteDatabase.GetDBSize), False)
        End If

        ResumeScan()

    End Sub

    'Event Handlers
    Private Sub gMonFileMonitor_Click(sender As Object, e As EventArgs) Handles gMonFileMonitor.Click
        ScanToggle()
    End Sub

    Private Sub gMonTrayMon_Click(sender As System.Object, e As System.EventArgs) Handles gMonTrayMon.Click
        ScanToggle()
    End Sub

    Private Sub gMonTray_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles gMonTray.MouseDoubleClick
        ToggleState()
    End Sub

    Private Sub gMonTrayShow_Click(sender As System.Object, e As System.EventArgs) Handles gMonTrayShow.Click
        ToggleState()
    End Sub

    Private Sub FileExit_Click(sender As Object, e As EventArgs) Handles gMonFileExit.Click, gMonTrayExit.Click
        ShutdownApp()
    End Sub

    Private Sub FileSettings_Click(sender As Object, e As EventArgs) Handles gMonFileSettings.Click, gMonTraySettings.Click
        OpenSettings()
    End Sub

    Private Sub SetupGameManager_Click(sender As Object, e As EventArgs) Handles gMonSetupGameManager.Click, gMonTraySetupGameManager.Click
        OpenGameManager()
    End Sub

    Private Sub gMonToolsSync_Click(sender As Object, e As EventArgs) Handles gMonTrayToolsCleanMan.Click, gMonToolsCleanMan.Click
        CleanLocalManifest()
    End Sub

    Private Sub gMonToolsCompact_Click(sender As Object, e As EventArgs) Handles gMonToolsCompact.Click, gMonTrayToolsCompact.Click
        CompactDatabases()
    End Sub

    Private Sub gMonSetupAddWizard_Click(sender As Object, e As EventArgs) Handles gMonSetupAddWizard.Click, gMonTraySetupAddWizard.Click
        OpenGameWizard()
    End Sub

    Private Sub SetupCustomVariables_Click(sender As Object, e As EventArgs) Handles gMonSetupCustomVariables.Click, gMonTraySetupCustomVariables.Click
        OpenCustomVariables()
    End Sub

    Private Sub gMonSetupTags_Click(sender As Object, e As EventArgs) Handles gMonSetupTags.Click, gMonTraySetupTags.Click
        OpenTags()
    End Sub

    Private Sub gMonHelpAbout_Click(sender As Object, e As EventArgs) Handles gMonHelpAbout.Click
        OpenAbout()
    End Sub

    Private Sub gMonHelpWebSite_Click(sender As Object, e As EventArgs) Handles gMonHelpWebSite.Click
        OpenWebSite()
    End Sub

    Private Sub gMonHelpManual_Click(sender As Object, e As EventArgs) Handles gMonHelpManual.Click
        OpenOnlineManual()
    End Sub

    Private Sub gMonHelpCheckforUpdates_Click(sender As Object, e As EventArgs) Handles gMonHelpCheckforUpdates.Click
        OpenCheckforUpdates()
    End Sub

    Private Sub gMonNotification_Click(sender As Object, e As EventArgs) Handles gMonNotification.Click, gMonTrayNotification.Click
        gMonNotification.Visible = False
        gMonTrayNotification.Visible = False
        OpenGameManager(True)
    End Sub

    Private Sub btnLogToggle_Click(sender As Object, e As EventArgs) Handles btnLogToggle.Click
        ToggleLog()
    End Sub

    Private Sub gMonStripSplitButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripStatusButton.ButtonClick
        ScanToggle()
    End Sub

    Private Sub pbIcon_Click(sender As Object, e As EventArgs) Handles pbIcon.Click
        If bAllowIcon Then
            SetIcon()
        End If
    End Sub

    Private Sub gMonTray_BalloonTipClicked(sender As System.Object, e As System.EventArgs) Handles gMonTray.BalloonTipClicked
        Me.Visible = True
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        Me.Focus()
    End Sub

    Private Sub btnCancelOperation_Click(sender As Object, e As EventArgs) Handles btnCancelOperation.Click
        OperationCancel()
    End Sub

    Private Sub gMonStripAdminButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripAdminButton.ButtonClick
        RestartAsAdmin()
    End Sub

    Private Sub frmMain_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        txtLog.Select(txtLog.TextLength, 0)
        txtLog.ScrollToCaret()
    End Sub

    Private Sub Main_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Intercept Exit & Minimize
        If bShutdown = False Then
            e.Cancel = True
            Me.Visible = False
            Me.ShowInTaskbar = False
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub ScanTimerEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles tmScanTimer.Tick
        Dim bNeedsPath As Boolean = False
        Dim bContinue As Boolean = True
        Dim bAskForRestart As Boolean = False
        Dim iErrorCode As Integer = 0
        Dim sErrorMessage As String = String.Empty

        If oProcess.SearchRunningProcesses(hshScanList, bNeedsPath, iErrorCode) Then
            PauseScan()

            If bNeedsPath Then
                bContinue = False
                If iErrorCode = 5 Then
                    If oProcess.Duplicate Then
                        sErrorMessage = mgrCommon.FormatString(My.Resources.frmMain_ErrorMultiAdmin)
                        mgrCommon.ShowMessage(sErrorMessage, MsgBoxStyle.Exclamation)
                        bAskForRestart = True
                    Else
                        If Not CheckForSavedPath() Then
                            sErrorMessage = mgrCommon.FormatString(My.Resources.frmMain_ErrorAdminBackup, oProcess.GameInfo.Name)
                            oProcess.GameInfo.ProcessPath = mgrPath.ProcessPathSearch(oProcess.GameInfo.Name, oProcess.GameInfo.ProcessName, sErrorMessage)
                            If oProcess.GameInfo.ProcessPath <> String.Empty Then
                                'Update and reload
                                mgrMonitorList.DoListUpdate(oProcess.GameInfo)
                                LoadGameSettings()
                                bContinue = True
                            End If
                        Else
                            bContinue = True
                        End If
                    End If
                ElseIf iErrorCode = 299 Then
                    If oProcess.Duplicate Then
                        sErrorMessage = mgrCommon.FormatString(My.Resources.frmMain_ErrorMulti64)
                        mgrCommon.ShowMessage(sErrorMessage, MsgBoxStyle.Exclamation)
                    Else
                        If Not CheckForSavedPath() Then
                            sErrorMessage = mgrCommon.FormatString(My.Resources.frmMain_Error64Backup, oProcess.GameInfo.Name)
                            oProcess.GameInfo.ProcessPath = mgrPath.ProcessPathSearch(oProcess.GameInfo.Name, oProcess.GameInfo.ProcessName, sErrorMessage)
                            If oProcess.GameInfo.ProcessPath <> String.Empty Then
                                'Update and reload
                                mgrMonitorList.DoListUpdate(oProcess.GameInfo)
                                LoadGameSettings()
                                bContinue = True
                            End If
                        Else
                            bContinue = True
                        End If
                    End If
                End If
            End If

            If bContinue = True Then
                CheckForSavedDuplicate()
                If oProcess.Duplicate Then
                    UpdateLog(My.Resources.frmMain_MultipleGamesDetected, oSettings.ShowDetectionToolTips)
                    UpdateStatus(My.Resources.frmMain_MultipleGamesDetected)
                    SetGameInfo(True)
                Else
                    UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_GameDetected, oProcess.GameInfo.Name), oSettings.ShowDetectionToolTips)
                    UpdateStatus(mgrCommon.FormatString(My.Resources.frmMain_GameDetected, oProcess.GameInfo.CroppedName))
                    SetGameInfo()
                End If
                oProcess.StartTime = Now
                bwMonitor.RunWorkerAsync()
            Else
                StopScan()
                If bAskForRestart Then
                    RestartAsAdmin()
                End If
            End If
        End If
    End Sub

    Private Sub bwMonitor_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwMonitor.DoWork
        Try
            Do While Not (oProcess.FoundProcess.HasExited Or bwMonitor.CancellationPending)
                System.Threading.Thread.Sleep(3000)
            Loop
            If bwMonitor.CancellationPending Then
                bCancelledByUser = True
            End If
        Catch ex As Exception
            bProcessIsAdmin = True
            oProcess.FoundProcess.WaitForExit()
            bProcessIsAdmin = False
        End Try
    End Sub

    Private Sub bwMain_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMonitor.RunWorkerCompleted
        oProcess.EndTime = Now

        If Not bCancelledByUser Then
            If DoMultiGameCheck() Then
                UpdateLog(mgrCommon.FormatString(My.Resources.frmMain_GameEnded, oProcess.GameInfo.Name), False)
                If oSettings.TimeTracking Then HandleTimeSpent()
                RunBackup()
            Else
                UpdateLog(My.Resources.frmMain_UnknownGameEnded, False)
                ResetGameInfo()
                ResumeScan()
            End If
        End If

        bCancelledByUser = False
        oProcess.StartTime = Now : oProcess.EndTime = Now
    End Sub

    Private Sub Main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetForm()
        VerifyGameDataPath()
        LoadAndVerify()
        VerifyCustomPathVariables()

        If oSettings.StartToTray Then
            Me.Visible = False
            Me.ShowInTaskbar = False
            Me.WindowState = FormWindowState.Minimized
        End If

        If oSettings.MonitorOnStartup Then
            eCurrentStatus = eStatus.Stopped
        Else
            eCurrentStatus = eStatus.Running
        End If

        HandleScan()
        CheckForNewBackups()

    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If bFirstRun Then
            OpenStartupWizard()
        End If
    End Sub

    Private Sub txtGameInfo_Enter(sender As Object, e As EventArgs)
        btnLogToggle.Focus()
    End Sub
End Class