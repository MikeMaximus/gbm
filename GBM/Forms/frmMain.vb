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

    Private iProcessType As System.Reflection.ProcessorArchitecture = System.Reflection.AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture
    Private sVersion As String = "Version: " & My.Application.Info.Version.Major & "." & _
        My.Application.Info.Version.Minor & " Beta (" & [Enum].GetName(GetType(System.Reflection.ProcessorArchitecture), iProcessType) & ")"
    Private sRevision As String = "Build: " & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
    Const sConstCopyright As String = "2015 Michael J. Seiferling"

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
        sStatus2 = "Updated by " & oRestoreInfo.UpdatedBy & " on " & oRestoreInfo.DateUpdated
        If oRestoreInfo.AbsolutePath Then
            sStatus3 = oRestoreInfo.RestorePath
        Else
            sStatus3 = oRestoreInfo.RelativeRestorePath
        End If

        WorkingGameInfo("Restore in progress...", sStatus1, sStatus2, sStatus3)
        UpdateStatus("Restore in progress...")
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

        WorkingGameInfo("Backup in Progress...", sStatus1, sStatus2, sStatus3)
        UpdateStatus("Backup in progress...")
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
                UpdateLog(oRestoreInfo.Name & " restore was cancelled due to unknown restore path.", False, ToolTipIcon.Error, True)
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

            UpdateLog("A manaul backup of " & oGame.Name & " was triggered.", False)

            If oGame.AbsolutePath = False Then
                If oGame.ProcessPath = String.Empty Then
                    If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                    oGame.ProcessPath = mgrPath.ProcessPathSearch(oGame.Name, oGame.TrueProcess, oGame.Name & " uses a relative path and has never been detected on this computer.", bNoAuto)
                End If

                If oGame.ProcessPath <> String.Empty Then
                    oReadyList.Add(oGame)
                Else
                    UpdateLog(oGame.Name & " backup was cancelled due to unknown path.", True, ToolTipIcon.Error, True)
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
            UpdateLog(oProcess.GameInfo.Name & " backup was cancelled due to session length.", False)
            SetLastAction(oProcess.GameInfo.CroppedName & " backup was cancelled due to session length")
            OperationEnded()
        Else
            If oProcess.GameInfo.MonitorOnly = False Then
                If oSettings.DisableConfirmation Then
                    bDoBackup = True
                Else
                    If MsgBox("Do you wish to backup data from " & oProcess.GameInfo.Name & "?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                        bDoBackup = True
                    Else
                        bDoBackup = False
                        UpdateLog(oProcess.GameInfo.Name & " backup was cancelled.", False)
                        SetLastAction(oProcess.GameInfo.CroppedName & " backup was cancelled")
                        OperationEnded()
                    End If
                End If
            Else
                bDoBackup = False
                UpdateLog(oProcess.GameInfo.Name & " is set to monitor only.", False)
                SetLastAction(oProcess.GameInfo.CroppedName & " monitoring ended")
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

        If slRestoreData.Count > 0 Then
            gMonNotification.Image = My.Resources.Inbox
            gMonNotification.Text = slRestoreData.Count & " pending restore(s)"
            gMonNotification.Visible = True
        End If

    End Sub

    'Functions to handle monitor list features
    Private Sub ImportMonitorList()
        Dim sLocation As String

        PauseScan()

        sLocation = mgrCommon.OpenFileBrowser("Choose a valid xml file to import", "xml", "XML", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)

        If sLocation <> String.Empty Then
            If mgrMonitorList.DoImport(sLocation) Then
                LoadGameSettings()
                If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
            End If
        End If

        ResumeScan()
    End Sub

    Private Sub ExportMonitorList()
        Dim sLocation As String

        PauseScan()

        sLocation = mgrCommon.SaveFileBrowser("Choose a location for the export file", "xml", "XML", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Game Backup Monitor Export " & Date.Now.ToString("dd-MMM-yyyy"))

        If sLocation <> String.Empty Then
            mgrMonitorList.ExportMonitorList(sLocation)
        End If

        ResumeScan()
    End Sub

    Private Sub DownloadOfficialList()
        PauseScan()

        If MsgBox("Would you like to import from the latest pre-configured game list?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(mgrPath.OfficialImportURL) Then
                LoadGameSettings()
                If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
            End If
        End If

        ResumeScan()
    End Sub

    'Functions handling the display of game information
    Private Sub SetIcon()
        Dim sIcon As String
        Dim fbBrowser As New OpenFileDialog

        fbBrowser.Title = "Choose icon for " & oProcess.GameInfo.Name
        fbBrowser.DefaultExt = "ico"
        fbBrowser.Filter = "Icon files (*.ico)|*.ico"
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
            lblGameTitle.Text = "Last Game: " & oProcess.GameInfo.CroppedName
            pbIcon.Image = oPriorImage
            lblStatus1.Text = sPriorPath
            lblStatus2.Text = sPriorCompany
            lblStatus3.Text = sPriorVersion
            If oSettings.TimeTracking Then
                lblTimeTitle.Visible = True
                lblTimeSpent.Visible = True
            End If
        Else
            pbIcon.Image = My.Resources.Searching
            lblGameTitle.Text = "No Game Detected"
            lblStatus1.Text = String.Empty
            lblStatus2.Text = String.Empty
            lblStatus3.Text = String.Empty
            lblTimeTitle.Visible = False
            lblTimeSpent.Visible = False
        End If

        If eCurrentStatus = eStatus.Stopped Then
            UpdateStatus("Not Scanning")
        Else
            UpdateStatus("No Game Detected")
        End If

    End Sub

    Private Sub WorkingGameInfo(ByVal sTitle As String, ByVal sStatus1 As String, ByVal sStatus2 As String, ByVal sStatus3 As String)
        'Thread Safe (If one control requires an invoke assume they all do)
        If pbIcon.InvokeRequired = True Then
            Dim d As New WorkingGameInfoCallBack(AddressOf WorkingGameInfo)
            Me.Invoke(d, New Object() {sTitle, sStatus1, sStatus2, sStatus3})
        Else
            lblTimeTitle.Visible = False
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
            lblGameTitle.Text = "Multiple Games"
            lblTimeTitle.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = My.Resources.Unknown
            lblStatus1.Text = "Game details are unavailable."
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
                    sFileName = oProcess.GameInfo.ProcessPath & " (Executable Path)"
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
                lblTimeTitle.Visible = False
                lblTimeSpent.Visible = False
            End If

            'Set Details
            If sFileName = String.Empty Then
                lblStatus1.Text = "N/A"
            Else
                lblStatus1.Text = sFileName
            End If

            If sCompanyName = String.Empty Then
                lblStatus2.Text = "N/A"
            Else
                lblStatus2.Text = sCompanyName
            End If

            If sFileVersion = String.Empty Then
                lblStatus3.Text = "N/A"
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
            sTotalTime = Math.Round((dTotalTime * 100) * 0.6) & " minutes [Total]"
        Else
            sTotalTime = Math.Round(dTotalTime, 1) & " hours [Total]"
        End If

        If dSessionTime < 1 Then
            sSessionTime = Math.Round((dSessionTime * 100) * 0.6) & " minutes [Session]"
        Else
            sSessionTime = Math.Round(dSessionTime, 1) & " hours [Session]"
        End If

        If dSessionTime > 0 Then
            lblTimeSpent.Text = sSessionTime & " | " & sTotalTime
        Else
            lblTimeSpent.Text = sTotalTime
        End If

        lblTimeTitle.Visible = True
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
        MsgBox("Game Backup Monitor" & vbCrLf & sVersion & vbCrLf & sRevision & vbCrLf & Chr(169) & sConstCopyright & vbCrLf & vbCrLf &
               "This program comes with ABSOLUTELY NO WARRANTY." & vbCrLf &
               "This is free software, and you are welcome to redistribute it under certain conditions." & vbCrLf & vbCrLf &
               "See gpl-3.0.html in the program folder for details.", MsgBoxStyle.Information, "Game Backup Monitor")
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

        UpdateLog("Game List (" & hshScanList.Keys.Count & ") Loaded.", False)
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
            UpdateLog("The master game list has been changed by a program other than GBM.", False, ToolTipIcon.Info, True)
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
            MsgBox("7-Zip was not found in the Game Backup Monitor utilities folder.  The application cannot continue.", MsgBoxStyle.Critical, "Game Backup Monitor")
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
                UpdateLog("GBM is running from a new location, the Windows startup entry has been updated.", False, ToolTipIcon.Info)
            End If
        End If

    End Sub

    'Functions that handle buttons, menus and other GUI features on this form
    Private Sub ToggleLog()
        If bLogToggle = False Then
            txtLog.Visible = True
            Me.Size = New System.Drawing.Size(540, 410)
            bLogToggle = True
            btnLogToggle.Text = "Hide &Log"
            txtLog.Select(txtLog.TextLength, 0)
            txtLog.ScrollToCaret()
        Else
            txtLog.Visible = False
            Me.Size = New System.Drawing.Size(540, 225)
            bLogToggle = False
            btnLogToggle.Text = "Show &Log"
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
                Dim sGame As String = oProcess.GameInfo.Name

                If bProcessIsAdmin Then
                    MsgBox(sGame & " is running as Administrator and GBM is not." &
                           vbCrLf & "You cannot cancel monitoring at this time." _
                           & vbCrLf & vbCrLf & "Run GBM as Administrator to prevent this issue.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
                    RestartAsAdmin()
                    Exit Sub
                End If

                If oProcess.Duplicate Then
                    sGame = "the unknown game"
                End If

                If MsgBox("Do you wish to cancel the monitoring of " & sGame & "?" & vbCrLf & vbCrLf & "Warning: When monitoring is cancelled, session time is NOT saved.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    UpdateLog("Monitoring of " & sGame & " was cancelled.", False)
                    SetLastAction("Monitoring of " & oProcess.GameInfo.CroppedName & " was cancelled")

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
            If MsgBox("Are you sure you want to exit?  Your games will no longer be monitored.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
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
            gMonTraySettings.Enabled = True
            bMenuEnabled = True
        End If
    End Sub

    Private Sub ToggleMenuText()
        Select Case eCurrentStatus
            Case eStatus.Running
                gMonFileMonitor.Text = "Stop &Monitoring"
                gMonTrayMon.Text = "Stop &Monitoring"
            Case eStatus.Stopped
                gMonFileMonitor.Text = "Start &Monitoring"
                gMonTrayMon.Text = "Start &Monitoring"
            Case eStatus.Paused
                gMonFileMonitor.Text = "Cancel &Monitoring"
                gMonTrayMon.Text = "Cancel &Monitoring"
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
        If mgrCommon.IsElevated Then
            gMonStripAdminButton.Image = My.Resources.Admin
            gMonStripAdminButton.ToolTipText = "GBM is running with Administrator privileges."

        Else
            gMonStripAdminButton.Image = My.Resources.User
            gMonStripAdminButton.ToolTipText = "GBM is running with normal privileges.  Click to restart as Administrator."
        End If
        btnCancelOperation.Visible = False
        txtLog.Visible = False
        lblLastActionTitle.Visible = False
        lblLastAction.Text = String.Empty
        Me.Size = New System.Drawing.Size(540, 225)
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
            UpdateStatus("Not Scanning")
            gMonStripStatusButton.Image = My.Resources.Stopped
            gMonTray.Icon = My.Resources.GBM_Tray_Stopped
        Else
            StartScan()
            StartSyncWatcher()
            eCurrentStatus = eStatus.Running
            UpdateStatus("No Game Detected")
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
            UpdateStatus("Not Scanning")
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
            UpdateStatus("No Game Detected")
        End If
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    Private Sub StopScan()
        StopSyncWatcher()
        tmScanTimer.Stop()
        eCurrentStatus = eStatus.Stopped
        UpdateStatus("Not Scanning")
        gMonStripStatusButton.Image = My.Resources.Stopped
        gMonTray.Icon = My.Resources.GBM_Tray_Stopped
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    'Functions to handle verification
    Private Sub VerifyCustomPathVariables()
        Dim sGames As String = String.Empty
        If Not mgrPath.VerifyCustomVariables(hshScanList, sGames) Then
            MsgBox("The following monitored game(s) contain a custom path variable that is not set." & vbCrLf & sGames & vbCrLf & vbCrLf & "You will encounter backup/restore errors with these games until the variables are set.", MsgBoxStyle.Critical, "Game Backup Monitor")
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
                MsgBox("An error occured creating application settings folder.  The application cannot proceed." & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Game Backup Monitor")
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
                    MsgBox("Your local GBM data (Version " & iDBVer & ") is too new for your version of GBM (Version " & mgrCommon.AppVersion & ")." & vbCrLf & vbCrLf & "Please upgrade GBM or restore the settings file appropriate for your version.  The application cannot proceed.", MsgBoxStyle.Critical, "Game Backup Monitor")
                Case mgrSQLite.Database.Remote
                    MsgBox("The GBM data (Version " & iDBVer & ") in your backup folder is too new for your version of GBM (Version " & mgrCommon.AppVersion & ")." & vbCrLf & vbCrLf & "All computers sharing a backup folder must use the same version of GBM.  The application cannot proceed.", MsgBoxStyle.Critical, "Game Backup Monitor")
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
            MsgBox("Game Backup Monitor is already running as Administrator.", MsgBoxStyle.Information, "Game Backup Monitor")
        Else
            If MsgBox("Do you want to restart Game Backup Monitor as Administrator?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                mgrCommon.RestartAsAdmin()
                bShutdown = True
                ShutdownApp(False)
            End If
        End If
    End Sub

    Private Sub SyncManifest()
        Dim slItems As SortedList

        PauseScan()

        If MsgBox("This removes orphaned backup information from the local manifest." & vbCrLf & vbCrLf &
                  "Do you want to sync the local backup manfiest with the current backup folder?" & vbCrLf & vbCrLf &
                  "Not recommended if alternating between more than one backup folder.", MsgBoxStyle.YesNo _
                  , "Game Backup Monitor") = MsgBoxResult.Yes Then

            slItems = mgrRestore.SyncLocalManifest()

            If slItems.Count > 0 Then
                For Each oItem As clsBackup In slItems.Values
                    UpdateLog(oItem.Name & " entry removed from local manfiest.", False)
                Next
                MsgBox(slItems.Count & " entries removed from the local manifest.")
            Else
                MsgBox("No orphaned entries found.  Local manifest is already in sync.")
            End If
        End If

        ResumeScan()
    End Sub

    Private Sub CompactDatabases()
        Dim oLocalDatabase As mgrSQLite
        Dim oRemoteDatabase As mgrSQLite

        PauseScan()

        If MsgBox("This will rebuild all databases and shrink them to an optimal size." & vbCrLf &
                  "This should only be used if your gbm.s3db files are becoming very large." & vbCrLf & vbCrLf &
                  "Do you wish to continue?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then

            oLocalDatabase = New mgrSQLite(mgrSQLite.Database.Local)
            oRemoteDatabase = New mgrSQLite(mgrSQLite.Database.Remote)

            UpdateLog("Local Database Vacuum Initialized: " & oLocalDatabase.GetDBSize & " KB", False)
            oLocalDatabase.CompactDatabase()
            UpdateLog("Local Database Vacuum Completed: " & oLocalDatabase.GetDBSize & " KB", False)

            UpdateLog("Remote Database Vacuum Initialized: " & oRemoteDatabase.GetDBSize & " KB", False)
            oRemoteDatabase.CompactDatabase()
            UpdateLog("Remote Database Vacuum Completed: " & oRemoteDatabase.GetDBSize & " KB", False)
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

    Private Sub gMonToolsSync_Click(sender As Object, e As EventArgs) Handles gMonToolsSyncMan.Click, gMonTrayToolsSyncMan.Click
        SyncManifest()
    End Sub

    Private Sub gMonToolsCompact_Click(sender As Object, e As EventArgs) Handles gMonToolsCompact.Click, gMonTrayToolsCompact.Click
        CompactDatabases()
    End Sub

    Private Sub gMonToolsGameExportList_Click(sender As Object, e As EventArgs) Handles gMonToolsGameExportList.Click, gMonTrayToolsGameExportList.Click
        ExportMonitorList()
    End Sub

    Private Sub gMonToolsGameImportList_Click(sender As Object, e As EventArgs) Handles gMonToolsGameImportList.Click, gMonTrayToolsGameImportList.Click
        ImportMonitorList()
    End Sub

    Private Sub gMonToolsGameImportOfficialList_Click(sender As Object, e As EventArgs) Handles gMonToolsGameImportOfficialList.Click, gMonTrayToolsGameImportOfficialList.Click
        DownloadOfficialList()
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

    Private Sub gMonHelpManual_Click(sender As Object, e As EventArgs) Handles gMonHelpManual.Click
        OpenOnlineManual()
    End Sub

    Private Sub gMonHelpCheckforUpdates_Click(sender As Object, e As EventArgs) Handles gMonHelpCheckforUpdates.Click
        OpenCheckforUpdates()
    End Sub

    Private Sub gMonNotification_Click(sender As Object, e As EventArgs) Handles gMonNotification.Click
        gMonNotification.Visible = False
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
                        sErrorMessage = "Multiple possible games have been detected running as Administrator and GBM is not, GBM cannot detect the path to identify your game or save your backup." & vbCrLf & vbCrLf &
                        "Please run GBM as Administrator to properly detect and backup this game."
                        MsgBox(sErrorMessage, MsgBoxStyle.Critical, "Game Backup Monitor")
                        bAskForRestart = True
                    Else
                        If Not CheckForSavedPath() Then
                            sErrorMessage = oProcess.GameInfo.Name & " is running as Administrator and GBM is not, GBM cannot detect the required information to save your backup."
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
                        sErrorMessage = "Multiple possible 64-bit games have been detected, GBM cannot detect the path to identify your game or save your backup." & vbCrLf & vbCrLf &
                          "Please install the 64-bit version of GBM to detect and backup this game properly."
                        MsgBox(sErrorMessage, MsgBoxStyle.Critical, "Game Backup Monitor")
                    Else
                        If Not CheckForSavedPath() Then
                            sErrorMessage = oProcess.GameInfo.Name & " is a 64-bit game, GBM cannot detect the required information to save your backup."
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
                    UpdateLog("Multiple Games Detected", oSettings.ShowDetectionToolTips)
                    UpdateStatus("Multiple Games Detected")
                    SetGameInfo(True)
                Else
                    UpdateLog(oProcess.GameInfo.Name & " Detected", oSettings.ShowDetectionToolTips)
                    UpdateStatus(oProcess.GameInfo.CroppedName & " Detected")
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
                UpdateLog(oProcess.GameInfo.Name & " has ended.", False)
                If oSettings.TimeTracking Then HandleTimeSpent()
                RunBackup()
            Else
                UpdateLog("The unidentified game has ended.", False)
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