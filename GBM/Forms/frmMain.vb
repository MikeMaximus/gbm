Imports GBM.My.Resources
Imports System.Collections.Specialized
Imports System.IO
Imports System.Threading.Thread
Imports NHotkey.WindowsForms

'Name: frmMain
'Description: Game Backup Monitor Main Screen
'Author: Michael J. Seiferling
Public Class frmMain
    'Used to denote the current status of the app
    Private Enum eStatus As Integer
        Running = 1
        Monitoring = 2
        Paused = 3
        Stopped = 4
    End Enum

    'Used to denote the currently running operation
    Private Enum eOperation As Integer
        None = 1
        Backup = 2
        Restore = 3
        Import = 4
    End Enum

    'Used to demote which display mode the form is in
    Private Enum eDisplayModes As Integer
        Initial = 1
        Normal = 2
        Busy = 3
        GameSelected = 4
    End Enum

    Private eCurrentStatus As eStatus = eStatus.Stopped
    Private eCurrentOperation As eOperation = eOperation.None
    Private eDisplayMode As eDisplayModes = eDisplayModes.Normal
    Private bDetectionCancelled As Boolean = False
    Private bDetectionFailure As Boolean = False
    Private bShutdown As Boolean = False
    Private bInitFail As Boolean = False
    Private bInitComplete As Boolean = False
    Private bPathDetectionFailure As Boolean = False
    Private sPathDetectionError As String = String.Empty
    Private bMenuEnabled As Boolean = True
    Private bLockdown As Boolean = True
    Private bFirstRun As Boolean = False
    Private bInitialLoad As Boolean = True
    Private bProcessIsAdmin As Boolean = False
    Private bLogToggle As Boolean = False
    Private bAllowIcon As Boolean = False
    Private bAllowDetails As Boolean = False
    Private bModdedTrayMenu As Boolean = False
    Private oUIState As New clsMainUIState
    Private iRestoreTimeOut As Integer
    Private oChildProcesses As New Hashtable
    Private oLastGame As clsGame
    Private oSelectedGame As clsGame
    Private bListLoading As Boolean = False
    Private bListRefresh As Boolean = False
    Private oExecutableIcon As Bitmap
    Private bHotKeyPressed As Boolean = False

    'Developer Debug Flags
    Private bProcessDebugMode As Boolean = False
    Private bMemoryDebugMode As Boolean = False
    Private bWineDebugMode As Boolean = False

    WithEvents oFileWatcher As New FileSystemWatcher

    'Timers - There may only be one System.Windows.Forms.Timer and it must be tmScanTimer.
    WithEvents tmScanTimer As New Timer
    WithEvents tmRestoreCheck As New System.Timers.Timer
    WithEvents tmFileWatcherQueue As New System.Timers.Timer
    WithEvents tmSessionTimeUpdater As New System.Timers.Timer
    WithEvents tmFilterTimer As New System.Timers.Timer
    WithEvents tmPlayTimer As New System.Timers.Timer
    WithEvents tmTimedBackup As New System.Timers.Timer

    Public WithEvents oProcess As New mgrProcessDetection
    Public WithEvents oBackup As New mgrBackup
    Public WithEvents oRestore As New mgrRestore
    Public hshScanList As Hashtable
    Private oGameList As OrderedDictionary

    Delegate Sub UpdateNotifierCallBack(ByVal iCount As Integer)
    Delegate Sub UpdateLogCallBack(ByVal sLogUpdate As String, ByVal bTrayUpdate As Boolean, ByVal objIcon As System.Windows.Forms.ToolTipIcon, ByVal bTimeStamp As Boolean)
    Delegate Sub WorkingGameInfoCallBack(ByVal sTitle As String, ByVal sStatus1 As String, ByVal sStatus2 As String, ByVal sStatus3 As String)
    Delegate Sub UpdateTimeSpentCallback(ByVal dTotalTime As Double, ByVal dSessionTime As Double)
    Delegate Sub UpdateStatusCallBack(ByVal sStatus As String)
    Delegate Sub SetLastActionCallBack(ByVal sString As String)
    Delegate Sub OperationStartedCallBack()
    Delegate Sub OperationEndedCallBack()
    Delegate Sub FormatAndFillListCallback()
    Delegate Sub RestoreCompletedCallBack()
    Delegate Sub EnablePlayButtonCallBack()

    'Handlers
    Private Sub SetLastAction(ByVal sMessage As String) Handles oBackup.SetLastAction, oRestore.SetLastAction
        'Thread Safe
        If lblLastAction.InvokeRequired = True Then
            Dim d As New SetLastActionCallBack(AddressOf SetLastAction)
            Me.Invoke(d, New Object() {sMessage})
        Else
            lblLastAction.Text = sMessage.TrimEnd(".") & " " & mgrCommon.FormatString(frmMain_AtTime, TimeOfDay.ToShortTimeString)
        End If
    End Sub

    Private Sub SetRestoreInfo(ByVal oRestoreInfo As clsBackup) Handles oRestore.UpdateRestoreInfo
        Dim sStatus1 As String
        Dim sStatus2 As String
        Dim sStatus3 As String

        'Build Info
        sStatus1 = Path.GetFileName(oRestoreInfo.FileName)
        sStatus2 = mgrCommon.FormatString(frmMain_UpdatedBy, New String() {oRestoreInfo.UpdatedBy, oRestoreInfo.DateUpdated})
        If oRestoreInfo.AbsolutePath Then
            sStatus3 = oRestoreInfo.Path
        Else
            sStatus3 = oRestoreInfo.RelativeRestorePath
        End If

        WorkingGameInfo(frmMain_RestoreInProgress, sStatus1, sStatus2, sStatus3)
        UpdateStatus(frmMain_RestoreInProgress)
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
            sStatus2 = oGame.ProcessPath & Path.DirectorySeparatorChar & oGame.Path
        End If
        sStatus3 = String.Empty

        WorkingGameInfo(frmMain_BackupInProgress, sStatus1, sStatus2, sStatus3)
        UpdateStatus(frmMain_BackupInProgress)
    End Sub

    Private Sub SetImportInfo(ByVal sPath As String) Handles oBackup.UpdateImportInfo
        Dim sStatus1 As String
        Dim sStatus2 As String
        Dim sStatus3 As String

        'Build Info
        sStatus1 = Path.GetFileName(sPath)
        sStatus2 = Path.GetDirectoryName(sPath)
        sStatus3 = String.Empty

        WorkingGameInfo(frmMain_ImportInProgress, sStatus1, sStatus2, sStatus3)
        UpdateStatus(frmMain_ImportInProgress)
    End Sub

    Private Sub OperationStarted()
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New OperationStartedCallBack(AddressOf OperationStarted)
            Me.Invoke(d, New Object() {})
        Else
            btnCancelOperation.Visible = True

            LockDownMenuEnable()

            If eCurrentStatus = eStatus.Running Then
                PauseScan()
            End If
        End If
    End Sub

    Private Sub OperationEnded()
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New OperationEndedCallBack(AddressOf OperationEnded)
            Me.Invoke(d, New Object() {})
        Else
            bHotKeyPressed = False
            btnCancelOperation.Visible = False
            btnCancelOperation.Enabled = True

            Select Case eCurrentOperation
                Case eOperation.Backup, eOperation.Import
                    oBackup.CancelOperation = False
                Case eOperation.Restore
                    oRestore.CancelOperation = False
            End Select

            eCurrentOperation = eOperation.None

            If eCurrentStatus = eStatus.Monitoring Then
                eDisplayMode = eDisplayModes.Busy
            ElseIf eCurrentStatus = eStatus.Paused Then
                eDisplayMode = eDisplayModes.Normal
                ResumeScan()
            Else
                eDisplayMode = eDisplayModes.Normal
            End If

            LockDownMenuEnable()
            ResetCurrentInfo()
        End If
    End Sub

    Private Sub OperationCancel()
        Select Case eCurrentOperation
            Case eOperation.None
                'Nothing
            Case eOperation.Backup, eOperation.Import
                oBackup.CancelOperation = True
                btnCancelOperation.Enabled = False
                mgrBackupQueue.DoBackupQueueEmpty()
            Case eOperation.Restore
                oRestore.CancelOperation = True
                btnCancelOperation.Enabled = False
        End Select
    End Sub

    Private Sub ExecuteBackup(ByVal oBackupList As List(Of clsGame))
        oBackup.DoBackup(oBackupList, bHotKeyPressed)
        OperationEnded()
    End Sub

    Private Sub ExecuteRestore(ByVal oRestoreList As List(Of clsBackup))
        oRestore.DoRestore(oRestoreList, bHotKeyPressed)
        OperationEnded()
    End Sub

    Private Function VerifyBackupForOS(ByRef oGame As clsGame) As Boolean
        Dim bOSVerified As Boolean

        'Handle Windows configurations in Linux
        If mgrCommon.IsUnix Then
            If oGame.OS = clsGame.eOS.Windows Then
                If mgrVariables.CheckForReservedVariables(oGame.TruePath) Then
                    'Absolute Path
                    Dim oWineData As clsWineData = mgrWineData.DoWineDataGetbyID(oGame.ID)
                    If oWineData.SavePath <> String.Empty Then
                        oGame.Path = oWineData.SavePath
                        bOSVerified = True
                    Else
                        bOSVerified = False
                        UpdateLog(mgrCommon.FormatString(frmMain_ErrorNoWineSavePath, oGame.Name), True, ToolTipIcon.Error, True)
                    End If
                Else
                    'Relative Path                    
                    bOSVerified = True
                End If
                If Not oGame.AbsolutePath Then oGame.Path = oGame.Path.Replace("\", Path.DirectorySeparatorChar)
                oGame.FileType = oGame.FileType.Replace("\", Path.DirectorySeparatorChar)
                oGame.ExcludeList = oGame.ExcludeList.Replace("\", Path.DirectorySeparatorChar)
            Else
                'Linux Configuration
                bOSVerified = True
            End If
        Else
            'Windows
            bOSVerified = True
        End If

        Return bOSVerified
    End Function

    Private Function VerifyRestoreForOS(ByRef oGame As clsGame, ByRef sPath As String) As Boolean
        Dim bOSVerified As Boolean

        'Handle Windows configurations in Linux
        If mgrCommon.IsUnix Then
            If oGame.OS = clsGame.eOS.Windows Then
                If mgrVariables.CheckForReservedVariables(oGame.TruePath) Then
                    'Absolute Path
                    Dim oWineData As clsWineData = mgrWineData.DoWineDataGetbyID(oGame.ID)
                    If oWineData.SavePath <> String.Empty Then
                        sPath = oWineData.SavePath
                        bOSVerified = True
                    Else
                        bOSVerified = False
                        UpdateLog(mgrCommon.FormatString(frmMain_ErrorNoWineSavePath, oGame.Name), True, ToolTipIcon.Error, True)
                    End If
                Else
                    'Relative Path                    
                    bOSVerified = True
                End If
                If Not oGame.AbsolutePath Then sPath = oGame.Path.Replace("\", Path.DirectorySeparatorChar)
            Else
                'Linux Configuration
                bOSVerified = True
            End If
        Else
            'Windows
            bOSVerified = True
        End If

        Return bOSVerified
    End Function

    Private Sub RunRestore(ByVal oRestoreList As Hashtable, Optional ByVal bNoRestoreQueue As Boolean = False, Optional ByVal bFastMode As Boolean = False)
        Dim oGame As clsGame
        Dim oReadyList As New List(Of clsBackup)
        Dim oRestoreInfo As clsBackup
        Dim bTriggerReload As Boolean = False
        Dim bOSVerified As Boolean
        Dim bPathVerified As Boolean
        Dim oDiffParent As clsBackup
        Dim oQueue As New Hashtable

        'Prevent concurrent operations
        If eCurrentOperation <> eOperation.None Then
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorOperationInProgress, New String() {eCurrentOperation.ToString, eOperation.Restore.ToString}), True, ToolTipIcon.Warning)
            Exit Sub
        End If

        eCurrentOperation = eOperation.Restore
        OperationStarted()

        If bNoRestoreQueue Then
            oQueue = oRestoreList
        Else
            GetRestoreQueue(oRestoreList, oQueue, bFastMode)
        End If

        'Build Restore List
        For Each de As DictionaryEntry In oQueue
            bPathVerified = False
            bOSVerified = False
            oGame = DirectCast(de.Key, clsGame)
            oRestoreInfo = DirectCast(de.Value, clsBackup)

            bOSVerified = VerifyRestoreForOS(oGame, oRestoreInfo.Path)

            If mgrPath.IsSupportedRegistryPath(oRestoreInfo.TruePath) Then
                bPathVerified = True
            Else
                If mgrRestore.CheckPath(oRestoreInfo, oGame, bTriggerReload, bFastMode) Then
                    bPathVerified = True
                Else
                    UpdateLog(mgrCommon.FormatString(frmMain_ErrorRestorePath, oRestoreInfo.Name), False, ToolTipIcon.Error, True)
                End If
            End If

            If bOSVerified And bPathVerified Then
                If oRestore.CheckRestorePrereq(oRestoreInfo, oGame.CleanFolder, bFastMode) Then
                    If oGame.Differential And Not oRestoreInfo.IsDifferentialParent Then
                        oDiffParent = mgrManifest.DoManifestGetByManifestID(oRestoreInfo.DifferentialParent, mgrSQLite.Database.Remote)
                        If oDiffParent IsNot Nothing Then
                            If oRestore.CheckRestorePrereq(oDiffParent, False, True) Then
                                UpdateLog(mgrCommon.FormatString(frmMain_RestoreQueueDiffParent, New String() {oRestoreInfo.Name, oDiffParent.DateUpdated.ToString}), False, ToolTipIcon.Error, True)
                                oReadyList.Add(oDiffParent)
                                oReadyList.Add(oRestoreInfo)
                            End If
                        Else
                            UpdateLog(mgrCommon.FormatString(frmMain_ErrorDifferentialParentNotFound, oRestoreInfo.Name), False, ToolTipIcon.Error, True)
                        End If
                    Else
                        oReadyList.Add(oRestoreInfo)
                    End If
                End If
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

    Private Sub RunRestoreAll()
        Dim oBackups As SortedList
        Dim oRestoreList As New Hashtable
        Dim hshGame As Hashtable
        Dim oGame As clsGame
        Dim oBackup As clsBackup

        oBackups = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Remote)

        For Each de As DictionaryEntry In oBackups
            oBackup = DirectCast(de.Value, clsBackup)
            hshGame = mgrMonitorList.DoListGetbyMonitorID(de.Key)
            If hshGame.Count = 1 Then
                oGame = DirectCast(hshGame(0), clsGame)
                oRestoreList.Add(oGame, oBackup)
            End If
        Next

        If mgrCommon.ShowMessage(frmMain_WarningFullRestore, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            RunRestore(oRestoreList,, True)
        End If
    End Sub

    Private Sub ResumeIncompleteBackups()
        Dim hshGame As Hashtable
        Dim oGame As clsGame
        Dim oList As List(Of String)
        Dim oBackupList As New List(Of clsGame)

        oList = mgrBackupQueue.DoGetBackupQueue()

        For Each sMonitorID As String In oList
            hshGame = mgrMonitorList.DoListGetbyMonitorID(sMonitorID)
            If hshGame.Count = 1 Then
                oGame = DirectCast(hshGame(0), clsGame)
                oBackupList.Add(oGame)
            End If
        Next

        RunManualBackup(oBackupList, True)
    End Sub

    Private Sub RunBackupAll()
        Dim hshGames As Hashtable
        Dim oGames As New List(Of clsGame)
        Dim oGame As clsGame

        hshGames = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
        For Each de As DictionaryEntry In hshGames
            oGame = DirectCast(de.Value, clsGame)
            If Not oGame.MonitorOnly Then oGames.Add(oGame)
        Next

        If mgrCommon.ShowMessage(frmMain_WarningFullBackup, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            RunManualBackup(oGames, True)
        End If
    End Sub

    Private Sub RunManualBackup(ByVal oBackupList As List(Of clsGame), Optional ByVal bFastMode As Boolean = False)
        Dim oGame As clsGame
        Dim lBackupSize As Long = 0
        Dim bNoAuto As Boolean
        Dim bOSVerified As Boolean
        Dim bPathVerified As Boolean
        Dim oReadyList As New List(Of clsGame)
        Dim oQueue As New List(Of clsGame)

        'Prevent concurrent operations
        If eCurrentOperation <> eOperation.None Then
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorOperationInProgress, New String() {eCurrentOperation.ToString, eOperation.Backup.ToString}), True, ToolTipIcon.Warning)
            Exit Sub
        End If

        eCurrentOperation = eOperation.Backup
        OperationStarted()

        GetBackupQueue(oBackupList, oQueue, False, bFastMode)

        'Build Backup List
        For Each oGame In oQueue
            'Break out when a cancel signal is received
            If oBackup.CancelOperation Then Exit For

            bNoAuto = False
            bOSVerified = False
            bPathVerified = False
            gMonStripStatusButton.Enabled = False

            bOSVerified = VerifyBackupForOS(oGame)

            If oGame.AbsolutePath = False Then
                If oGame.ProcessPath = String.Empty And Not bFastMode Then
                    If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                    oGame.ProcessPath = mgrPath.ProcessPathSearch(oGame.Name, oGame.ProcessName, mgrCommon.FormatString(frmMain_ErrorRelativePath, oGame.Name), bNoAuto)
                End If

                If oGame.ProcessPath <> String.Empty Then
                    bPathVerified = True
                Else
                    If Not bFastMode Then UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oGame.Name), False, ToolTipIcon.Error, True)
                End If
            Else
                If mgrPath.IsSupportedRegistryPath(oGame.Path) Or Directory.Exists(oGame.Path) Then
                    bPathVerified = True
                Else
                    If Not bFastMode Then UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oGame.Name), False, ToolTipIcon.Error, True)
                End If
            End If

            If bOSVerified And bPathVerified Then
                If oBackup.CheckBackupPrereq(oGame, lBackupSize, bFastMode) Then
                    oReadyList.Add(oGame)
                End If
            End If
        Next

        'Run backups
        If oReadyList.Count > 0 And Not oBackup.CancelOperation Then
            If Not mgrSettings.DisableDiskSpaceCheck And Not oReadyList.Count = 1 Then UpdateLog(mgrCommon.FormatString(mgrBackup_BackupBatchSize, mgrCommon.FormatDiskSpace(lBackupSize)), False, ToolTipIcon.Info, True)

            'Populate the failsafe queue
            mgrBackupQueue.DoBackupQueueEmpty()
            mgrBackupQueue.DoBackupQueueAddBatch(oReadyList)

            Dim oThread As New System.Threading.Thread(AddressOf ExecuteBackup)
            oThread.IsBackground = True
            oThread.Start(oReadyList)
        Else
            OperationEnded()
        End If

    End Sub

    Private Sub RunImportBackupByGame(ByVal sFilesToImport As String(), ByVal oGame As clsGame)
        'Prevent concurrent operations
        If eCurrentOperation <> eOperation.None Then
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorOperationInProgress, New String() {eCurrentOperation.ToString, eOperation.Import.ToString}), True, ToolTipIcon.Warning)
            Exit Sub
        End If

        eCurrentOperation = eOperation.Import
        OperationStarted()
        oBackup.ImportBackupFiles(sFilesToImport, oGame)
        OperationEnded()
    End Sub

    Private Sub RunImportBackupByFile(ByVal sFilesToImport As String())
        'Prevent concurrent operations
        If eCurrentOperation <> eOperation.None Then
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorOperationInProgress, New String() {eCurrentOperation.ToString, eOperation.Import.ToString}), True, ToolTipIcon.Warning)
            Exit Sub
        End If

        eCurrentOperation = eOperation.Import
        OperationStarted()
        oBackup.ImportBackupFiles(sFilesToImport)
        OperationEnded()
    End Sub

    Private Function DoMultiGameCheck() As Boolean
        Dim oResult As DialogResult

        If oProcess.Duplicate = True Then
            Dim frm As New frmChooseGame
            frm.Process = oProcess
            oResult = frm.ShowDialog()
            If oResult = DialogResult.OK Then
                'Reload settings
                LoadGameSettings()
                oProcess.GameInfo = frm.Game
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

    Private Sub GetRestoreQueue(ByVal oRootList As Hashtable, ByRef oRestoreList As Hashtable, Optional ByVal bFastMode As Boolean = False)
        Dim oLinkChain As New List(Of String)
        Dim hshLatestManifest As SortedList = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Remote)
        Dim hshGame As Hashtable
        Dim oGame As clsGame
        Dim oBackup As clsBackup

        For Each de As DictionaryEntry In oRootList
            oGame = DirectCast(de.Key, clsGame)
            mgrConfigLinks.BuildLinkChain(oGame.ID, oLinkChain)
        Next

        For Each sID As String In oLinkChain
            If hshLatestManifest.Contains(sID) Then
                oBackup = DirectCast(hshLatestManifest(sID), clsBackup)
                hshGame = mgrMonitorList.DoListGetbyMonitorID(sID)
                If hshGame.Count = 1 Then
                    oGame = DirectCast(hshGame(0), clsGame)
                    If Not bFastMode Then UpdateLog(mgrCommon.FormatString(frmMain_RestoreQueue, New String() {oGame.Name, oBackup.DateUpdated.ToString}), False, ToolTipIcon.Info, True)
                    oRestoreList.Add(oGame, oBackup)
                End If
            End If
        Next
    End Sub

    Private Sub GetBackupQueue(ByVal oRootList As List(Of clsGame), ByRef oBackupList As List(Of clsGame), Optional ByVal bDoPreCheck As Boolean = True, Optional ByVal bFastMode As Boolean = False)
        Dim oLinkChain As New List(Of String)
        Dim lBackupSize As Long = 0
        Dim hshGame As Hashtable
        Dim oGame As clsGame

        For Each oRoot As clsGame In oRootList
            mgrConfigLinks.BuildLinkChain(oRoot.ID, oLinkChain)
        Next

        For Each sID As String In oLinkChain
            hshGame = mgrMonitorList.DoListGetbyMonitorID(sID)
            If hshGame.Count = 1 Then
                oGame = DirectCast(hshGame(0), clsGame)
                If Not oGame.MonitorOnly Then
                    If Not bFastMode Then UpdateLog(mgrCommon.FormatString(frmMain_BackupQueue, oGame.Name), False, ToolTipIcon.Info, True)
                    If bDoPreCheck Then
                        If VerifyBackupForOS(oGame) And oBackup.CheckBackupPrereq(oGame, lBackupSize) Then oBackupList.Add(oGame)
                    Else
                        oBackupList.Add(oGame)
                    End If
                End If
            End If
        Next

        'Only populate the failsafe queue if prerequisite checks have been completed.
        If bDoPreCheck Then
            'Empty, then generate the stored failsafe queue.
            mgrBackupQueue.DoBackupQueueEmpty()
            mgrBackupQueue.DoBackupQueueAddBatch(oBackupList)
        End If
    End Sub

    Private Sub RunBackup()
        Dim bDoBackup As Boolean
        Dim oRootList As New List(Of clsGame)
        Dim oReadyList As New List(Of clsGame)

        'Prevent concurrent operations
        If eCurrentOperation <> eOperation.None Then
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorOperationInProgress, New String() {eCurrentOperation.ToString, oProcess.GameInfo.CroppedName & " - " & eOperation.Backup.ToString}), True, ToolTipIcon.Warning)
            Exit Sub
        End If

        eCurrentOperation = eOperation.Backup
        OperationStarted()

        If oProcess.GameInfo.MonitorOnly = False Then
            If SuppressSession() Then
                bDoBackup = False
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.Name), False)
                SetLastAction(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.CroppedName))
                OperationEnded()
            Else
                If mgrSettings.DisableConfirmation Then
                    bDoBackup = True
                Else
                    If mgrCommon.ShowPriorityMessage(frmMain_ConfirmBackup, oProcess.GameInfo.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        bDoBackup = True
                    Else
                        bDoBackup = False
                        UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupCancel, oProcess.GameInfo.Name), False)
                        SetLastAction(mgrCommon.FormatString(frmMain_ErrorBackupCancel, oProcess.GameInfo.CroppedName))
                        OperationEnded()
                    End If
                End If
            End If
        Else
            bDoBackup = True
            UpdateLog(mgrCommon.FormatString(frmMain_MonitorEnded, oProcess.GameInfo.Name), False)
            SetLastAction(mgrCommon.FormatString(frmMain_MonitorEnded, oProcess.GameInfo.CroppedName))
        End If

        If bDoBackup Then
            oRootList.Add(oProcess.GameInfo)
            GetBackupQueue(oRootList, oReadyList)

            If oReadyList.Count = 0 Then
                OperationEnded()
            Else
                'Run the backup(s)
                Dim trd As New System.Threading.Thread(AddressOf ExecuteBackup)
                trd.IsBackground = True
                trd.Start(oReadyList)
            End If
        End If
    End Sub

    Private Sub UpdateNotifier(ByVal iCount As Integer)
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New UpdateNotifierCallBack(AddressOf UpdateNotifier)
            Me.Invoke(d, New Object() {iCount})
        Else
            Dim sNotification As String
            If iCount > 1 Then
                sNotification = mgrCommon.FormatString(frmMain_NewSaveNotificationMulti, iCount)
            Else
                sNotification = mgrCommon.FormatString(frmMain_NewSaveNotificationSingle, iCount)
            End If
            gMonNotification.Image = frmMain_Notification
            gMonTrayNotification.Image = frmMain_Notification
            gMonNotification.Text = sNotification
            gMonTrayNotification.Text = sNotification
            gMonNotification.Visible = True
            gMonTrayNotification.Visible = True
        End If
    End Sub

    Private Sub StartRestoreCheck()
        iRestoreTimeOut = -1
        tmRestoreCheck.Start()
        AutoRestoreCheck()
    End Sub

    Private Sub AutoRestoreCheck()
        Dim slRestoreData As SortedList = mgrRestore.CompareManifests()
        Dim oNotReady As New List(Of clsBackup)
        Dim oNotInstalled As New List(Of clsBackup)
        Dim oNoCheckSum As New List(Of clsBackup)
        Dim oBackup As clsBackup
        Dim sFileName As String
        Dim sExtractPath As String
        Dim bFinished As Boolean = True
        Dim hshRestore As Hashtable
        Dim hshGames As Hashtable
        Dim oGame As clsGame
        Dim sGame As String

        'Shut down the timer and bail out if there's nothing to do
        If slRestoreData.Count = 0 Then
            tmRestoreCheck.Stop()
            Exit Sub
        End If

        If mgrSettings.AutoMark Or mgrSettings.AutoRestore Then
            'Increment Timer
            iRestoreTimeOut += 1

            'Check backup files
            For Each de As DictionaryEntry In slRestoreData
                oBackup = DirectCast(de.Value, clsBackup)

                'Check if backup file is ready to restore
                If oBackup.CheckSum <> String.Empty Then
                    sFileName = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oBackup.FileName
                    If mgrHash.Generate_SHA256_Hash(sFileName) <> oBackup.CheckSum Then
                        oNotReady.Add(oBackup)
                        bFinished = False
                    End If
                Else
                    oNoCheckSum.Add(oBackup)
                End If

                'Check if the restore location exists,  if not we assume the game is not installed and should be auto-marked.
                hshGames = mgrMonitorList.DoListGetbyMonitorID(de.Key)
                If hshGames.Count = 1 Then
                    oGame = DirectCast(hshGames(0), clsGame)
                    If oGame.ProcessPath <> String.Empty Then
                        oBackup.RelativeRestorePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oBackup.Path
                    End If
                End If

                If oBackup.AbsolutePath Then
                    sExtractPath = oBackup.Path
                Else
                    sExtractPath = oBackup.RelativeRestorePath
                End If

                If Not Directory.Exists(sExtractPath) And Not mgrPath.IsSupportedRegistryPath(oBackup.Path) Then
                    If mgrSettings.AutoMark Then
                        If mgrManifest.DoManifestCheck(de.Key, mgrSQLite.Database.Local) Then
                            mgrManifest.DoManifestUpdateByMonitorID(de.Value, mgrSQLite.Database.Local)
                        Else
                            mgrManifest.DoManifestAdd(de.Value, mgrSQLite.Database.Local)
                        End If
                    End If
                    oNotInstalled.Add(oBackup)
                End If
            Next

            'Remove any backup files that are not ready
            For Each o As clsBackup In oNotReady
                slRestoreData.Remove(o.MonitorID)
                UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotReady, o.Name), False, ToolTipIcon.Info, True)
            Next

            'Remove any backup files that should not be automatically restored
            For Each o As clsBackup In oNotInstalled
                slRestoreData.Remove(o.MonitorID)
                If mgrSettings.AutoMark Then
                    UpdateLog(mgrCommon.FormatString(frmMain_AutoMark, o.Name), False, ToolTipIcon.Info, True)
                Else
                    UpdateLog(mgrCommon.FormatString(frmMain_NoAutoMark, o.Name), False, ToolTipIcon.Info, True)
                End If
            Next
            For Each o As clsBackup In oNoCheckSum
                slRestoreData.Remove(o.MonitorID)
                UpdateLog(mgrCommon.FormatString(frmMain_NoCheckSum, o.Name), False, ToolTipIcon.Info, True)
            Next

            'Automatically restore backup files
            If mgrSettings.AutoRestore Then
                If slRestoreData.Count > 0 Then
                    hshRestore = New Hashtable
                    sGame = String.Empty
                    For Each de As DictionaryEntry In slRestoreData
                        oBackup = DirectCast(de.Value, clsBackup)
                        hshGames = mgrMonitorList.DoListGetbyMonitorID(de.Key)
                        If hshGames.Count = 1 Then
                            oGame = DirectCast(hshGames(0), clsGame)
                            sGame = oGame.CroppedName
                            hshRestore.Add(oGame, de.Value)
                        Else
                            UpdateLog(mgrCommon.FormatString(frmMain_AutoRestoreFailure, oBackup.Name), False, ToolTipIcon.Info, True)
                        End If
                    Next

                    'Handle notifications
                    If mgrSettings.RestoreOnLaunch Then
                        If slRestoreData.Count > 1 Then
                            UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotificationMulti, slRestoreData.Count), True, ToolTipIcon.Info, True)
                        Else
                            UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotificationSingle, sGame), True, ToolTipIcon.Info, True)
                        End If
                    End If

                    RunRestore(hshRestore, True, True)
                End If
            End If

            'Shutdown if we are finished
            If bFinished Then
                tmRestoreCheck.Stop()
            End If

            'Time out after 15 minutes
            If iRestoreTimeOut = 15 Then
                tmRestoreCheck.Stop()
            End If
        End If

        'Update the menu notifier if we aren't using auto restore
        If mgrSettings.RestoreOnLaunch And Not mgrSettings.AutoRestore Then
            If slRestoreData.Count > 0 Then
                UpdateNotifier(slRestoreData.Count)
            End If
            tmRestoreCheck.Stop()
        End If
    End Sub

    'Functions handling the display of game information
    Private Sub SetIcon()
        Dim sIcon As String
        Dim sDefaultFolder As String
        Dim oExtensions As New SortedList

        Try
            sDefaultFolder = Path.GetDirectoryName(oProcess.FoundProcess.MainModule.FileName)
        Catch ex As Exception
            sDefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End Try

        'Unix Handler
        If Not mgrCommon.IsUnix Then
            oExtensions.Add(frmGameManager_Executable, "*.exe")
            oExtensions.Add(frmGameManager_Icon, "*.ico")
            oExtensions.Add(frmGameManager_Image, "*.bmp;*.gif;*.jpg;*.png;*.tif")
            sIcon = mgrCommon.OpenFileBrowser("Main_Icon", mgrCommon.FormatString(frmMain_ChooseIcon, oProcess.GameInfo.CroppedName), oExtensions, 2, sDefaultFolder, False)
        Else
            oExtensions.Add(frmGameManager_Image, "*.gif;*.jpg;*.png;*.tif")
            sIcon = mgrCommon.OpenFileBrowser("Main_Icon", mgrCommon.FormatString(frmMain_ChooseIcon, oProcess.GameInfo.CroppedName), oExtensions, 1, sDefaultFolder, False)
        End If

        If sIcon <> String.Empty Then
            If File.Exists(sIcon) Then
                oProcess.GameInfo.Icon = sIcon
                pbIcon.Image = mgrCommon.SafeIconFromFile(sIcon)
                mgrMonitorList.DoListUpdate(oProcess.GameInfo)
            End If
        End If
    End Sub

    Private Sub FormatAndFillList()
        'Thread Safe
        If lstGames.InvokeRequired = True Then
            Dim d As New FormatAndFillListCallback(AddressOf FormatAndFillList)
            Me.Invoke(d, New Object() {})
        Else
            Dim oApp As clsGame
            Dim oData As KeyValuePair(Of String, String)
            Dim oList As New List(Of KeyValuePair(Of String, String))
            Dim sFilter As String = txtSearch.Text

            For Each de As DictionaryEntry In oGameList
                oApp = DirectCast(de.Value, clsGame)
                oData = New KeyValuePair(Of String, String)(oApp.ID, oApp.Name)
                oList.Add(oData)
            Next

            bListLoading = True
            lstGames.BeginUpdate()
            lstGames.DataSource = Nothing
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
            'Prevent re-enabling the game list if it happens to be refreshed during a time when it shouldn't be available.
            If eCurrentStatus <> eStatus.Monitoring And eCurrentStatus <> eStatus.Paused Then
                lstGames.Enabled = True
            End If
            lstGames.ClearSelected()
            bListLoading = False

            'Remember last selected game if there is one
            SelectLastSelectedGame()
            If Not lstGames.SelectedIndex = -1 Then
                eDisplayMode = eDisplayModes.GameSelected
                DisplaySelectedGameInfo()
            End If

            'Automatically select the game on a single filter match
            If Not txtSearch.Text = String.Empty Then
                If lstGames.Items.Count = 1 Then
                    lstGames.SelectedIndex = 0
                    eDisplayMode = eDisplayModes.GameSelected
                    DisplaySelectedGameInfo()
                End If
            End If
        End If
    End Sub

    Private Sub SetCurrentInfo()
        Dim sFileName As String = String.Empty
        Dim sFileVersion As String = String.Empty
        Dim sCompanyName As String = String.Empty

        eDisplayMode = eDisplayModes.Busy
        lstGames.ClearSelected()
        SwitchDisplayMode()

        'Wipe Game Info
        lblStatus1.Text = String.Empty
        lblStatus2.Text = String.Empty
        lblStatus3.Text = String.Empty
        pbIcon.Image = Multi_Unknown

        Try
            'Set Game Icon
            If Not mgrCommon.IsUnix Then
                oExecutableIcon = mgrCommon.GetIconFromExecutable(oProcess.FoundProcess.MainModule.FileName)
                pbIcon.Image = oExecutableIcon
            End If

            'Set Game Details
            sFileName = oProcess.FoundProcess.MainModule.FileName
            sFileVersion = oProcess.FoundProcess.MainModule.FileVersionInfo.FileVersion
            sCompanyName = oProcess.FoundProcess.MainModule.FileVersionInfo.CompanyName
        Catch ex As Exception
            oExecutableIcon = Nothing
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorGameDetails), False, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, ex.Message), False,, False)
        End Try

        'Set Game Details 
        If oProcess.Duplicate Then
            UpdateStatus(frmMain_MultipleGamesDetected)

            bAllowIcon = False
            bAllowDetails = False
            lblGameTitle.Text = frmMain_MultipleGames
            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = Multi_Unknown

            If sFileName = String.Empty Then
                lblStatus1.Text = frmMain_NoDetails
            Else
                lblStatus1.Text = sFileName
            End If

        Else
            UpdateStatus(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.CroppedName), oProcess.GameInfo.CroppedName)

            bAllowIcon = True
            bAllowDetails = True
            lblGameTitle.Text = oProcess.GameInfo.Name

            'Check for a custom icon & details            
            If File.Exists(oProcess.GameInfo.Icon) Then
                pbIcon.Image = mgrCommon.SafeIconFromFile(oProcess.GameInfo.Icon)
            End If
            If sFileName = String.Empty Then
                If oProcess.GameInfo.ProcessPath <> String.Empty Then
                    sFileName = mgrCommon.FormatString(frmMain_ExePath, oProcess.GameInfo.ProcessPath)
                End If
            End If

            'Use custom info if it exists
            If oProcess.GameInfo.Version <> String.Empty Then
                sFileVersion = oProcess.GameInfo.Version
            End If
            If oProcess.GameInfo.Company <> String.Empty Then
                sCompanyName = oProcess.GameInfo.Company
            End If

            'Do Time Update
            If mgrSettings.TimeTracking Then
                UpdateTimeSpent(oProcess.GameInfo.Hours, 0)
            Else
                pbTime.Visible = False
                lblTimeSpent.Visible = False
            End If

            'Set Details
            If sFileName = String.Empty Then
                lblStatus1.Text = frmMain_NotAvailable
            Else
                lblStatus1.Text = sFileName
            End If

            If sCompanyName = String.Empty Then
                lblStatus2.Text = frmMain_NotAvailable
            Else
                lblStatus2.Text = sCompanyName
            End If

            If sFileVersion = String.Empty Then
                lblStatus3.Text = frmMain_NotAvailable
            Else
                lblStatus3.Text = sFileVersion
            End If

        End If

        'Save Game Details
        oUIState.Title = lblGameTitle.Text
        oUIState.Icon = pbIcon.Image
        oUIState.Status1 = lblStatus1.Text
        oUIState.Status2 = lblStatus2.Text
        oUIState.Status3 = lblStatus3.Text
        oUIState.MonitorStatus = gMonStripTxtStatus.Text
        oUIState.TrayStatus = gMonTray.Text
    End Sub

    Private Sub ResetCurrentInfo()
        If oLastGame Is Nothing Then
            eDisplayMode = eDisplayModes.Initial
        End If

        If lstGames.SelectedIndex <> -1 Then
            eDisplayMode = eDisplayModes.GameSelected
        End If

        Select Case eDisplayMode
            Case eDisplayModes.Initial
                pbIcon.Image = frmMain_Searching
                lblGameTitle.Text = frmMain_NoGameDetected
                lblStatus1.Text = String.Empty
                lblStatus2.Text = String.Empty
                lblStatus3.Text = String.Empty
                pbTime.Visible = False
                lblTimeSpent.Visible = False
            Case eDisplayModes.Normal
                lblGameTitle.Text = mgrCommon.FormatString(frmMain_LastGame, oLastGame.Name)
                pbIcon.Image = oUIState.Icon
                lblStatus1.Text = oUIState.Status1
                lblStatus2.Text = oUIState.Status2
                lblStatus3.Text = oUIState.Status3
                lblTimeSpent.Text = oUIState.Time
                If mgrSettings.TimeTracking Then
                    pbTime.Visible = True
                    lblTimeSpent.Visible = True
                End If
            Case eDisplayModes.Busy
                lblGameTitle.Text = oUIState.Title
                pbIcon.Image = oUIState.Icon
                lblStatus1.Text = oUIState.Status1
                lblStatus2.Text = oUIState.Status2
                lblStatus3.Text = oUIState.Status3
                lblTimeSpent.Text = oUIState.Time
                If mgrSettings.TimeTracking Then
                    pbTime.Visible = True
                    lblTimeSpent.Visible = True
                End If
                gMonStripTxtStatus.Text = oUIState.MonitorStatus
                gMonTray.Text = oUIState.TrayStatus
            Case eDisplayModes.GameSelected
                DisplaySelectedGameInfo()
        End Select

        Select Case eDisplayMode
            Case eDisplayModes.Initial, eDisplayModes.Normal, eDisplayModes.GameSelected
                If eCurrentStatus = eStatus.Stopped Then
                    UpdateStatus(frmMain_NotScanning)
                Else
                    UpdateStatus(frmMain_NoGameDetected)
                End If
        End Select

        SwitchDisplayMode()
    End Sub

    Private Sub WorkingGameInfo(ByVal sTitle As String, ByVal sStatus1 As String, ByVal sStatus2 As String, ByVal sStatus3 As String)
        'Thread Safe (If one control requires an invoke assume they all do)
        If pbIcon.InvokeRequired = True Then
            Dim d As New WorkingGameInfoCallBack(AddressOf WorkingGameInfo)
            Me.Invoke(d, New Object() {sTitle, sStatus1, sStatus2, sStatus3})
        Else
            eDisplayMode = eDisplayModes.Busy
            SwitchDisplayMode()

            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = frmMain_Working
            lblGameTitle.Text = sTitle
            lblStatus1.Text = sStatus1
            lblStatus2.Text = sStatus2
            lblStatus3.Text = sStatus3
        End If
    End Sub

    Private Sub DisplaySelectedGameInfo()
        Dim oBackupList As List(Of clsBackup) = mgrManifest.DoManifestGetByMonitorID(oSelectedGame.ID, mgrSQLite.Database.Remote)
        Dim oLastPlayed As Object = mgrSessions.GetLastSessionDateTime(oSelectedGame)
        Dim sCachedIcon As String = mgrCommon.GetCachedIconPath(oSelectedGame.ID)
        Dim sTags As String = mgrGameTags.PrintTagsbyID(oSelectedGame.ID)

        eDisplayMode = eDisplayModes.GameSelected
        SwitchDisplayMode()

        If mgrSettings.TimeTracking Then
            pbTime.Visible = True
            lblTimeSpent.Visible = True
            If oSelectedGame.Hours < 1 Then
                lblTimeSpent.Text = mgrCommon.FormatString(frmMain_SessionMinutes, Math.Round((oSelectedGame.Hours * 100) * 0.6).ToString)
            Else
                lblTimeSpent.Text = mgrCommon.FormatString(frmMain_SessionHours, Math.Round(oSelectedGame.Hours, 1).ToString)
            End If
        Else
            pbTime.Visible = False
            lblTimeSpent.Visible = False
        End If

        If File.Exists(oSelectedGame.Icon) Then
            pbIcon.Image = mgrCommon.SafeIconFromFile(oSelectedGame.Icon)
        ElseIf File.Exists(sCachedIcon) Then
            pbIcon.Image = mgrCommon.SafeIconFromFile(sCachedIcon)
        Else
            pbIcon.Image = Multi_Unknown
        End If

        lblGameTitle.Text = oSelectedGame.Name

        If sTags = String.Empty Then
            lblStatus1.Text = frmMain_NoTags
        Else
            lblStatus1.Text = sTags
        End If

        If oLastPlayed Is Nothing Then
            lblStatus2.Text = frmMain_NoSessions
        Else
            lblStatus2.Text = mgrCommon.FormatString(frmMain_Lastplayed, mgrCommon.UnixToDate(CLng(oLastPlayed)))
        End If

        If oBackupList.Count >= 1 Then
            lblStatus3.Text = mgrCommon.FormatString(frmMain_LastBackup, oBackupList(0).DateUpdated)
        Else
            lblStatus3.Text = frmMain_NoBackups
        End If
    End Sub

    Private Sub SetSelectedGame()
        Dim oData As KeyValuePair(Of String, String) = lstGames.SelectedItems(0)
        oSelectedGame = DirectCast(oGameList(oData.Key), clsGame)
    End Sub

    Private Sub SelectLastSelectedGame()
        If Not oSelectedGame Is Nothing Then
            eDisplayMode = eDisplayModes.GameSelected
            lstGames.SelectedItem = New KeyValuePair(Of String, String)(oSelectedGame.ID, oSelectedGame.Name)
        End If
    End Sub

    Private Sub HandleProcessPath()
        If hshScanList.Contains(oProcess.GameInfo.ID) Then
            If Not oProcess.GameInfo.ProcessPath = DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).ProcessPath Then
                DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).ProcessPath = oProcess.GameInfo.ProcessPath
                mgrMonitorList.DoListFieldUpdate("ProcessPath", oProcess.GameInfo.ProcessPath, oProcess.GameInfo.ID)
                If (mgrSettings.SyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then mgrSync.SyncData(False)
            End If
        End If
    End Sub

    Private Sub UpdateTimeSpent(ByVal dTotalTime As Double, ByVal dSessionTime As Double)
        'Thread Safe 
        If Me.InvokeRequired = True Then
            Dim d As New UpdateTimeSpentCallback(AddressOf UpdateTimeSpent)
            Me.Invoke(d, New Object() {dTotalTime, dSessionTime})
        Else
            Dim sTotalTime As String
            Dim sSessionTime As String

            If dTotalTime < 1 Then
                sTotalTime = mgrCommon.FormatString(frmMain_SessionMinutes, Math.Round((dTotalTime * 100) * 0.6).ToString)
            Else
                sTotalTime = mgrCommon.FormatString(frmMain_SessionHours, Math.Round(dTotalTime, 1).ToString)
            End If

            If dSessionTime < 1 Then
                sSessionTime = mgrCommon.FormatString(frmMain_SessionMinutes, Math.Round((dSessionTime * 100) * 0.6).ToString)
            Else
                sSessionTime = mgrCommon.FormatString(frmMain_SessionHours, Math.Round(dSessionTime, 1).ToString)
            End If

            lblTimeSpent.Text = sSessionTime & " (" & sTotalTime & ")"
            oUIState.Time = lblTimeSpent.Text
            gMonTray.Text = mgrCommon.FormatString(frmMain_GameDetectedWithSessionTime, New String() {mgrCommon.EscapeAmpersand(oProcess.GameInfo.CroppedName, True), sSessionTime})

            pbTime.Visible = True
            lblTimeSpent.Visible = True
        End If
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
        If hshScanList.Contains(oProcess.GameInfo.ID) Then
            DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).Hours = oProcess.GameInfo.Hours
        End If

        mgrMonitorList.DoListFieldUpdate("Hours", oProcess.GameInfo.Hours, oProcess.GameInfo.ID)
        mgrSync.SyncData(False)

        UpdateTimeSpent(dCurrentHours, oProcess.TimeSpent.TotalHours)
    End Sub

    Private Sub HandleSession()
        Dim oSession As clsSession

        If Not SuppressSession() Then
            'Record Session
            oSession = New clsSession
            oSession.MonitorID = oProcess.GameInfo.ID
            oSession.SessionStartFromDate = oProcess.StartTime
            oSession.SessionEndFromDate = oProcess.EndTime

            mgrSessions.AddSession(oSession)
        End If

        'Handle Launcher Menu
        HandleLauncherMenu()
    End Sub

    Private Function SuppressSession() As Boolean
        Dim iSession As Integer
        If mgrSettings.SuppressBackup Then
            iSession = Math.Ceiling(oProcess.TimeSpent.TotalMinutes)
            If iSession > mgrSettings.SuppressBackupThreshold Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Sub HandleIconCache()
        Try
            'It's normal for this to be nothing if an icon can't be extracted from the executable.
            If Not oExecutableIcon Is Nothing Then
                oExecutableIcon.Save(mgrCommon.GetCachedIconPath(oLastGame.ID), Imaging.ImageFormat.Png)
            End If
        Catch ex As Exception
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorIconCache, New String() {oLastGame.Name, ex.Message}), False, ToolTipIcon.Warning, True)
        End Try
    End Sub

    'Functions handling the opening of other windows
    Private Sub OpenDevConsole()
        Dim sFullCommand As String
        Dim sMainCommand As String
        Dim sCommand As String()
        Dim sDelimters As String = " :"
        Dim cDelimters As Char() = sDelimters.ToCharArray

        sFullCommand = InputBox(frmMain_EnterCommand, frmMain_DeveloperConsole)

        If sFullCommand <> String.Empty Then
            sMainCommand = sFullCommand.Split(cDelimters, 2)(0)

            'Parse Command
            Select Case sMainCommand.ToLower
                Case "sql"
                    'Run a SQL command directly on any database
                    'Usage: SQL {Local or Remote} SQL Command

                    Dim oDatabase As mgrSQLite
                    Dim bSuccess As Boolean

                    sCommand = sFullCommand.Split(cDelimters, 3)

                    'Check Paramters
                    If sCommand.Length < 3 Then
                        mgrCommon.ShowMessage(frmMain_ErrorMissingParams, sCommand(0), MsgBoxStyle.Exclamation)
                        Exit Select
                    End If

                    If sCommand(1).ToLower = "local" Then
                        oDatabase = New mgrSQLite(mgrSQLite.Database.Local)
                    ElseIf sCommand(1).ToLower = "remote" Then
                        oDatabase = New mgrSQLite(mgrSQLite.Database.Remote)
                    Else
                        mgrCommon.ShowMessage(frmMain_ErrorCommandBadParam, New String() {sCommand(1), sCommand(0)}, MsgBoxStyle.Exclamation)
                        Exit Select
                    End If

                    bSuccess = oDatabase.RunParamQuery(sCommand(2), New Hashtable)

                    If bSuccess Then
                        mgrCommon.ShowMessage(frmMain_CommandSucess, MsgBoxStyle.Exclamation)
                    Else
                        mgrCommon.ShowMessage(frmMain_CommandFail, MsgBoxStyle.Exclamation)
                    End If

                Case "debug"
                    'Enable or disable various debug modes
                    'Usage: DEBUG Mode {Enable or Disable} 

                    sCommand = sFullCommand.Split(cDelimters, 3)

                    Dim bDebugEnable As Boolean = False

                    'Check Paramters
                    If sCommand.Length < 3 Then
                        mgrCommon.ShowMessage(frmMain_ErrorMissingParams, sCommand(0), MsgBoxStyle.Exclamation)
                        Exit Select
                    End If

                    If sCommand(2).ToLower = "enable" Then
                        bDebugEnable = True
                    ElseIf sCommand(2).ToLower = "disable" Then
                        bDebugEnable = False
                    Else
                        mgrCommon.ShowMessage(frmMain_ErrorCommandBadParam, New String() {sCommand(1), sCommand(0)}, MsgBoxStyle.Exclamation)
                        Exit Select
                    End If

                    Select Case sCommand(1).ToLower
                        Case "process"
                            bProcessDebugMode = bDebugEnable
                            mgrCommon.ShowMessage(frmMain_CommandSucess, MsgBoxStyle.Exclamation)
                        Case "memory"
                            bMemoryDebugMode = bDebugEnable
                            mgrCommon.ShowMessage(frmMain_CommandSucess, MsgBoxStyle.Exclamation)
                        Case "wine"
                            bWineDebugMode = bDebugEnable
                            mgrCommon.ShowMessage(frmMain_CommandSucess, MsgBoxStyle.Exclamation)
                        Case Else
                            mgrCommon.ShowMessage(frmMain_ErrorInvalidMode, New String() {sCommand(1), sCommand(0)}, MsgBoxStyle.Exclamation)
                    End Select

                Case Else
                    mgrCommon.ShowMessage(frmMain_ErrorCommandInvalid, sMainCommand, MsgBoxStyle.Exclamation)
            End Select

        End If
    End Sub

    Private Sub OpenAbout()
        Dim sRevision As String = My.Resources.BuildDate
        Dim sProcessType = [Enum].GetName(GetType(System.Reflection.ProcessorArchitecture), mgrCommon.GetArchitecture)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sPlatform As String = mgrCommon.GetFrameworkInfo
        Dim sSqliteVersion As String = oDatabase.ReportVersion
        Dim sConstCopyright As String = Chr(169) & mgrCommon.FormatString(App_Copyright, Now.Year.ToString)

        mgrCommon.ShowMessage(frmMain_About, New String() {mgrCommon.DisplayAppVersion, sProcessType, sRevision, sPlatform, sSqliteVersion, sConstCopyright}, MsgBoxStyle.Information)
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmTags
        PauseScan()
        frm.ShowDialog()
        ResumeScan()
    End Sub

    Private Sub OpenProcessManager()
        Dim frm As New frmProcessManager
        PauseScan()
        frm.ShowDialog()
        ResumeScan()
    End Sub

    Private Sub OpenLauncherManager()
        Dim frm As New frmLauncherManager
        PauseScan()
        frm.ShowDialog()
        ResumeScan()
    End Sub

    Private Sub OpenGameManager(ByVal oGame As clsGame, Optional ByVal bPendingRestores As Boolean = False)
        Dim frm As New frmGameManager
        PauseScan()
        frm.PendingRestores = bPendingRestores
        frm.OpenToGame = oGame
        frm.ShowDialog()
        LoadGameSettings()
        ResetCurrentInfo()
        ResumeScan()

        'Handle backup trigger
        If frm.TriggerBackup Then
            RunManualBackup(frm.BackupList)
        End If

        'Handle restore trigger
        If frm.TriggerRestore Then
            RunRestore(frm.RestoreList, frm.NoRestoreQueue)
        End If

        'Handle import backup trigger
        If frm.TriggerImportBackup Then
            RunImportBackupByGame(frm.ImportBackupList, frm.ImportBackupGame)
        End If

        'Rebuild launch menu just in case something was deleted.
        HandleLauncherMenu()
    End Sub

    Private Sub OpenSettings()
        Dim frm As New frmSettings
        PauseScan()
        UnbindHotKeys()
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Set Remote Database Location
            mgrPath.RemoteDatabaseLocation = mgrSettings.BackupFolder
            SetupSyncWatcher()
            mgrStoreVariables.AutoConfigureStoreVariables()
            LoadGameSettings()
            HandleFeatures()
            HandleLauncherMenu()
            SetDetectionSpeed()
        Else
            mgrSettings.LoadSettings()
        End If
        ResumeScan()
        BindHotKeys()
    End Sub

    Private Sub OpenSessions()
        Dim frm As New frmSessions
        PauseScan()
        If mgrSettings.SessionTracking = False Then
            mgrCommon.ShowMessage(frmMain_WarningSessionsDisabled, MsgBoxStyle.Exclamation)
        End If
        If mgrSessions.CountRows > 0 Then
            frm.ShowDialog()
        Else
            mgrCommon.ShowMessage(frmMain_ErrorNoSessions, MsgBoxStyle.Information)
        End If
        HandleLauncherMenu()
        ResumeScan()
    End Sub

    Private Sub OpenGameWizard()
        Dim frm As New frmAddWizard
        PauseScan()
        frm.ShowDialog()
        mgrSync.SyncData()
        LoadGameSettings()
        ResumeScan()
    End Sub

    Private Sub OpenCustomVariables()
        Dim frm As New frmVariableManager
        PauseScan()
        frm.ShowDialog()
        mgrPath.LoadCustomVariables()
        ResumeScan()
    End Sub

    Private Sub OpenStartupWizard()
        Dim frm As New frmStartUpWizard()
        ToggleMenuEnable()
        frm.ShowDialog()
        ToggleMenuEnable()
        bFirstRun = False
    End Sub

    Private Sub OpenWebSite()
        mgrCommon.OpenInOS(App_URLWebsite, , True)
    End Sub

    Private Sub OpenOnlineManual()
        mgrCommon.OpenInOS(App_URLManual, , True)
    End Sub

    Private Sub OpenCheckforUpdates()
        mgrCommon.OpenInOS(App_URLUpdates, , True)
    End Sub

    Private Sub CheckForFailedBackups()
        Dim iCount As Integer = mgrBackupQueue.DoBackupQueueCount

        If iCount > 0 Then
            UpdateLog(mgrCommon.FormatString(frmMain_ResumeBackupQueue, iCount))
            ResumeIncompleteBackups()
        End If
    End Sub

    Private Sub CheckForNewBackups()
        If mgrSettings.RestoreOnLaunch Or mgrSettings.AutoRestore Or mgrSettings.AutoMark Then
            StartRestoreCheck()
        End If
    End Sub

    'Functions handling the loading/sync of settings
    Private Sub RefreshGameList()
        oGameList = mgrMonitorList.ReadFilteredList(New List(Of clsTag), New List(Of clsTag), New List(Of clsGameFilter), frmFilter.eFilterType.BaseFilter, False, True, "Name", txtSearch.Text)
        FormatAndFillList()
        bListRefresh = False
    End Sub

    Private Sub LoadGameSettings()
        'Load Monitor List
        hshScanList = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.ScanList)
        'Load the game list only if the panel is shown, otherwise queue it for when the panel state changes. 
        If slcMain.Panel1Collapsed Then
            bListRefresh = True
        Else
            RefreshGameList()
        End If
        UpdateLog(mgrCommon.FormatString(frmMain_GameListLoaded, hshScanList.Keys.Count), False)
    End Sub

    Private Sub StartSyncWatcher()
        oFileWatcher.EnableRaisingEvents = True
    End Sub

    Private Sub StopSyncWatcher()
        oFileWatcher.EnableRaisingEvents = False
    End Sub

    Private Sub SetupSyncWatcher()
        oFileWatcher.Path = mgrSettings.BackupFolder
        oFileWatcher.Filter = "gbm.s3db"
        oFileWatcher.NotifyFilter = NotifyFilters.LastWrite
    End Sub

    Private Sub QueueSyncWatcher() Handles oFileWatcher.Changed
        'This is the easiest (but probably sloppiest) way to block unnecessary sync calls after the remote db is updated by GBM itself.
        If eCurrentStatus <> eStatus.Paused And eCurrentStatus <> eStatus.Monitoring Then
            tmFileWatcherQueue.Stop()
            tmFileWatcherQueue.Start()
        End If
    End Sub

    Private Sub HandleSyncWatcher() Handles tmFileWatcherQueue.Elapsed
        UpdateLog(frmMain_MasterListChanged, False, ToolTipIcon.Info, True)
        mgrSync.SyncData(False, False)
        LoadGameSettings()
        CheckForNewBackups()
    End Sub

    Private Sub BindHotKeys()
        If Not mgrCommon.IsUnix And mgrSettings.EnableHotKeys Then
            Try
                HotkeyManager.Current.AddOrReplace("QuickSave", mgrSettings.BackupHotKey, AddressOf HandleHotKeys)
                HotkeyManager.Current.AddOrReplace("QuickLoad", mgrSettings.RestoreHotKey, AddressOf HandleHotKeys)
            Catch ex As Exception
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorBindHotKeys, ex.Message), True, ToolTipIcon.Error)
            End Try
        End If
    End Sub

    Private Sub UnbindHotKeys()
        If Not mgrCommon.IsUnix And mgrSettings.EnableHotKeys Then
            Try
                HotkeyManager.Current.Remove("QuickSave")
                HotkeyManager.Current.Remove("QuickLoad")
            Catch ex As Exception
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorUnBindHotKeys, ex.Message), True, ToolTipIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ToggleHotKeys(ByVal bEnabled As Boolean)
        If Not mgrCommon.IsUnix And mgrSettings.EnableHotKeys Then
            HotkeyManager.Current.IsEnabled = bEnabled
        End If
    End Sub

    Private Sub LocalDatabaseCheck()
        Dim oLocalDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        oLocalDatabase.DatabaseUpgrade()
    End Sub

    Private Sub RemoteDatabaseCheck()
        Dim oRemoteDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        oRemoteDatabase.DatabaseUpgrade()
    End Sub

    Private Sub BackupDatabases()
        Dim oLocalDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oRemoteDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        oLocalDatabase.BackupDB(App_BackupOnLaunchFileDescription, True)
        oRemoteDatabase.BackupDB(mgrPath.ValidateFileName(My.Computer.Name, 62) & "." & App_BackupOnLaunchFileDescription, True)
    End Sub

    Private Sub LoadAndVerify()
        'If the default utility is missing we cannot continue
        If Not oBackup.CheckForUtilities(mgrPath.Default7zLocation) Then
            mgrCommon.ShowMessage(frmMain_Error7zip, MsgBoxStyle.Critical)
            bInitFail = True
            Exit Sub
        End If

        'Check Special Paths
        If Not mgrPath.CheckForEmptySpecialPaths() Then
            bInitFail = True
            Exit Sub
        End If

        'Local Database Check
        VerifyDBVersion(mgrSQLite.Database.Local)
        LocalDatabaseCheck()

        'Load Settings
        mgrSettings.LoadSettings()

        'Set UI based on settings
        HandleUISettings()

        'Set Detection Speed based on settings
        SetDetectionSpeed()

        If Not bFirstRun Then
            'The application cannot continue if this fails
            If Not VerifyBackupLocation() Then
                bInitFail = True
                Exit Sub
            End If

            'Remote Database Check
            VerifyDBVersion(mgrSQLite.Database.Remote)
            RemoteDatabaseCheck()

            'Backup GBM data
            If mgrSettings.BackupOnLaunch Then
                BackupDatabases()
            End If

            'Sync
            mgrSync.SyncData(, False)
        End If

        'Setup Sync Watcher
        SetupSyncWatcher()

        'Automatically configure variables for supported applications
        mgrStoreVariables.AutoConfigureStoreVariables()

        'Setup Global Hotkeys
        BindHotKeys()

        'Load Game Settings
        LoadGameSettings()

        'Enable or disable options based on feature settings
        HandleFeatures()

        'Handle the launcher menu 
        HandleLauncherMenu()

        'Verify the "Start with Windows" setting
        If mgrSettings.StartWithWindows Then
            If mgrCommon.IsUnix Then
                Dim sVerifyError As String = String.Empty
                If Not VerifyAutoStartLinux(sVerifyError) Then
                    UpdateLog(sVerifyError, False, ToolTipIcon.Info)
                End If
            Else
                If Not VerifyStartWithWindows() Then
                    UpdateLog(frmMain_ErrorAppLocationChanged, False, ToolTipIcon.Info)
                End If
            End If
        End If

        'Check for any custom 7z utility and display a warning if it's missing
        If mgrSettings.Custom7zLocation <> String.Empty Then
            If Not oBackup.CheckForUtilities(mgrSettings.Custom7zLocation) Then
                mgrCommon.ShowMessage(frmMain_Error7zCustom, mgrSettings.Custom7zLocation, MsgBoxStyle.Exclamation)
            End If
        End If

    End Sub

    'Functions that handle buttons, menus and other GUI features on this form
    Private Sub ToggleVisibility(ByVal bVisible As Boolean, Optional ByVal bDelay As Boolean = False)
        Me.ShowInTaskbar = bVisible
        'This delay is a work-around for the "ghost window" issue that occurs in Linux when the form is hidden on start-up.  It just works...
        If bDelay Then Threading.Thread.Sleep(100)
        Me.Visible = bVisible
    End Sub

    Private Sub ToggleState(ByVal bVisible As Boolean)
        If bVisible Then
            'When toggling back to normal, we want to make the window visible first so the user sees the restore animation.
            ToggleVisibility(bVisible)
            Me.WindowState = FormWindowState.Normal
            If txtSearch.CanFocus Then
                txtSearch.Focus()
            Else
                lblGameTitle.Focus()
            End If
        Else
            'When toggling to hide the window, we want to make the window invisible after a minimize to prevent the odd flickering animation.
            Me.WindowState = FormWindowState.Minimized
            ToggleVisibility(bVisible)
        End If
    End Sub

    Private Sub ShowApp()
        ToggleState(True)
        Me.Activate()
    End Sub

    Private Sub ScanToggle()
        Select Case eCurrentStatus
            Case eStatus.Running
                HandleScan()
            Case eStatus.Monitoring
                Dim sGame As String = oProcess.GameInfo.CroppedName

                If bProcessIsAdmin Then
                    mgrCommon.ShowMessage(frmMain_ErrorAdminDetect, sGame, MsgBoxStyle.Exclamation)
                    RestartAsAdmin()
                    Exit Sub
                End If

                If oProcess.Duplicate Then
                    sGame = Multi_UnknownGame
                End If

                If mgrCommon.ShowMessage(frmMain_ConfirmMonitorCancel, sGame, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    UpdateLog(mgrCommon.FormatString(frmMain_MonitorCancel, sGame), False)
                    SetLastAction(mgrCommon.FormatString(frmMain_MonitorCancel, sGame))

                    bwMonitor.CancelAsync()
                    StopScan()

                    eDisplayMode = eDisplayModes.Normal
                    ResetCurrentInfo()

                End If
            Case eStatus.Stopped
                HandleScan()
        End Select
    End Sub

    Private Sub InitApp()
        SetForm()
        Try
            mgrCommon.SetTLSVersion()
            VerifyGameDataPath()
            If bFirstRun Then OpenStartupWizard()
            LoadAndVerify()
        Catch ex As Exception
            If mgrCommon.ShowMessage(frmMain_ErrorInitFailure, ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                bInitFail = True
            End If
        End Try

        If bInitFail Then
            bShutdown = True
            Me.Close()
        Else
            VerifyCustomPathVariables()

            'We only do this in .NET, for Mono it is fired from frmMain_Activated.
            If mgrSettings.StartToTray And Not mgrCommon.IsUnix Then
                ToggleState(False)
            End If
            If mgrSettings.MonitorOnStartup Then
                eCurrentStatus = eStatus.Stopped
            Else
                eCurrentStatus = eStatus.Running
            End If

            HandleScan()
            CheckForFailedBackups()

            'Mono crashes out if you try to automatically restore new backups at this point, it is fired from frmMain_Activated.
            If Not mgrCommon.IsUnix Then
                CheckForNewBackups()
            End If
            StartSyncWatcher()

            AddHandler mgrSync.UpdateLog, AddressOf UpdateLog
            AddHandler mgrSync.PushStarted, AddressOf StopSyncWatcher
            AddHandler mgrSync.PushEnded, AddressOf StartSyncWatcher

            bInitComplete = True
        End If
    End Sub

    Private Sub ShutdownApp(Optional ByVal bPrompt As Boolean = True)
        Dim bClose As Boolean = False

        If bPrompt And Not mgrSettings.ExitNoWarning Then
            If mgrCommon.ShowMessage(frmMain_Exit, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                bClose = True
            End If
        Else
            bClose = True
        End If

        If bClose Then
            'Close Application
            bShutdown = True
            tmScanTimer.Stop()
            gMonTray.Dispose() 'Mono has issues automatically disposing of this control
            If bwMonitor.IsBusy() Then bwMonitor.CancelAsync()
            Me.Close()
        End If
    End Sub

    Private Sub EnablePlayButton()
        'Thread Safe
        If btnPlay.InvokeRequired = True Then
            Dim d As New EnablePlayButtonCallBack(AddressOf EnablePlayButton)
            Me.Invoke(d, New Object() {})
        Else
            btnPlay.Enabled = True
        End If
    End Sub

    Private Sub LaunchGame(ByVal oGame As clsGame)
        Dim oLaunchData As clsLaunchData = mgrLaunchData.DoLaunchDataGetbyID(oGame.ID)
        Dim eLaunchType As mgrLaunchGame.eLaunchType
        Dim sErrorMessage As String = String.Empty
        Dim sMessage As String = String.Empty

        If mgrLaunchGame.CanLaunchGame(oGame, oLaunchData, eLaunchType, sErrorMessage) Then
            If mgrLaunchGame.LaunchGame(oGame, oLaunchData, eLaunchType, sMessage) Then
                btnPlay.Enabled = False
                tmPlayTimer.Enabled = True
                UpdateLog(sMessage, False, ToolTipIcon.Info, True)
            End If
        Else
            mgrCommon.ShowMessage(sErrorMessage, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub GameLaunchHandler(sender As Object, e As EventArgs)
        Dim hshGame As Hashtable = mgrMonitorList.DoListGetbyMonitorID(sender.Tag)
        Dim oGame As clsGame

        If hshGame.Count = 1 Then
            oGame = DirectCast(hshGame(0), clsGame)
            LaunchGame(oGame)
        End If
    End Sub

    Private Sub RemoveLauncherMenu()
        gMonTrayMenu.Items.RemoveByKey("gMonLaunchSpacer")
        For i = 0 To 4
            gMonTrayMenu.Items.RemoveByKey("gMonLaunchGame" & i)
        Next
        bModdedTrayMenu = False
    End Sub

    Private Sub HandleLauncherMenu()
        Dim oSpacer As New ToolStripSeparator
        Dim oMenuItem As ToolStripMenuItem
        Dim sID As String
        Dim sName As String
        Dim iMenuOrder As Integer = 0
        Dim oRecentGames As DataSet = mgrSessions.GetLastFiveUniqueSessions()
        Dim iRecentCount As Integer = oRecentGames.Tables(0).Rows.Count

        'If there's less than 5 recently played games, we need to rebuild
        If iRecentCount <= 5 And bModdedTrayMenu Then
            RemoveLauncherMenu()
        End If

        If iRecentCount > 0 And mgrSettings.EnableLauncher = True Then
            If Not bModdedTrayMenu Then
                oSpacer.Name = "gMonLaunchSpacer"
                gMonTrayMenu.Items.Insert(0, oSpacer)
            End If

            For Each dr As DataRow In oRecentGames.Tables(0).Rows
                sID = CStr(dr(0))
                sName = CStr(dr(1))

                If sName.Length > 30 Then sName = sName.Substring(0, 31).Trim & "..."

                If bModdedTrayMenu Then
                    gMonTrayMenu.Items.Item(iMenuOrder).Tag = sID
                    gMonTrayMenu.Items.Item(iMenuOrder).Text = mgrCommon.EscapeAmpersand(sName)
                Else
                    oMenuItem = New ToolStripMenuItem
                    oMenuItem.Name = "gMonLaunchGame" & iMenuOrder
                    oMenuItem.Tag = sID
                    oMenuItem.Text = mgrCommon.EscapeAmpersand(sName)
                    gMonTrayMenu.Items.Insert(iMenuOrder, oMenuItem)
                    AddHandler oMenuItem.Click, AddressOf GameLaunchHandler
                End If
                iMenuOrder += 1
            Next

            bModdedTrayMenu = True
        Else
            If bModdedTrayMenu Then
                RemoveLauncherMenu()
            End If
        End If
    End Sub

    Private Sub HandleFeatures()
        'Sessions
        If mgrSettings.SessionTracking Then
            gMonToolsSessions.Visible = True
            gMonTrayToolsSessions.Visible = True
        Else
            gMonToolsSessions.Visible = False
            gMonTrayToolsSessions.Visible = False
        End If

        'Game Launching
        If mgrSettings.EnableLauncher Then
            gMonSetupLauncherManager.Visible = True
            gMonTraySetupLauncherManager.Visible = True
            SwitchDisplayMode()
        Else
            gMonSetupLauncherManager.Visible = False
            gMonTraySetupLauncherManager.Visible = False
            SwitchDisplayMode()
        End If

    End Sub

    Private Sub HandleQuickBackup()
        If eCurrentStatus = eStatus.Monitoring And Not mgrSettings.EnableLiveBackup Then
            Exit Sub
        End If

        Dim oGame As New clsGame

        Select Case eDisplayMode
            Case eDisplayModes.Initial
                oGame = Nothing
            Case eDisplayModes.Normal
                oGame = oLastGame
            Case eDisplayModes.GameSelected
                oGame = oSelectedGame
            Case eDisplayModes.Busy
                oGame = oProcess.GameInfo
        End Select

        If Not oGame Is Nothing Then
            If bHotKeyPressed Then
                RunManualBackup(New List(Of clsGame)({oGame}), True)
            Else
                If mgrCommon.ShowMessage(frmMain_ConfirmManualBackup, oGame.CroppedName, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    RunManualBackup(New List(Of clsGame)({oGame}))
                End If
            End If
        End If
    End Sub

    Private Sub HandleQuickRestore()
        If eCurrentStatus = eStatus.Monitoring And Not mgrSettings.EnableLiveBackup Then
            Exit Sub
        End If

        Dim oBackup As New List(Of clsBackup)
        Dim oGame As New clsGame
        Dim hshRestoreList As New Hashtable

        Select Case eDisplayMode
            Case eDisplayModes.Initial
                oGame = Nothing
            Case eDisplayModes.Normal
                oGame = oLastGame
            Case eDisplayModes.GameSelected
                oGame = oSelectedGame
            Case eDisplayModes.Busy
                oGame = oProcess.GameInfo
        End Select

        If oGame Is Nothing Then
            oBackup = Nothing
        Else
            oBackup = mgrManifest.DoManifestGetByMonitorID(oGame.ID, mgrSQLite.Database.Remote)
        End If

        If Not oBackup Is Nothing Then
            If oBackup.Count >= 1 Then
                hshRestoreList.Add(oGame, oBackup)

                If bHotKeyPressed Then
                    RunRestore(hshRestoreList, , True)
                Else
                    If mgrCommon.ShowMessage(frmMain_ConfirmRestore, New String() {oBackup(0).CroppedName, oBackup(0).DateUpdated, oBackup(0).UpdatedBy}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        RunRestore(hshRestoreList)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub HandleHotKeys(sender As Object, e As NHotkey.HotkeyEventArgs)
        bHotKeyPressed = True

        Select Case e.Name
            Case "QuickSave"
                HandleQuickBackup()
            Case "QuickLoad"
                HandleQuickRestore()
        End Select
    End Sub

    Private Sub ToggleMenuItems(ByVal bEnable As Boolean, ByVal oDropDownItems As ToolStripMenuItem, Optional ByVal sExempt() As String = Nothing)
        If sExempt Is Nothing Then sExempt = {}
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

    Private Sub ToggleLaunchMenuItems(ByVal bEnable As Boolean)
        Dim iItem As Integer
        For i = 0 To 4
            iItem = gMonTrayMenu.Items.IndexOfKey("gMonLaunchGame" & i)
            If Not iItem = -1 Then
                gMonTrayMenu.Items.Item(iItem).Enabled = bEnable
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
            ToggleLaunchMenuItems(False)
            bLockdown = False
        Else
            gMonStripStatusButton.Enabled = True
            gMonFileMonitor.Enabled = True
            gMonFileExit.Enabled = True
            gMonTrayMon.Enabled = True
            gMonTrayExit.Enabled = True
            ToggleLaunchMenuItems(True)
            bLockdown = True
        End If
    End Sub

    Private Sub ToggleMenuEnable(Optional ByVal bGameDetected As Boolean = False)
        If bMenuEnabled Then
            If Not bGameDetected Then
                ToggleMenuItems(False, gMonFile)
            Else
                Dim sExempt() As String = {"gMonFileMonitor", "gMonFileExit"}
                ToggleMenuItems(False, gMonFile, sExempt)
            End If
            ToggleMenuItems(False, gMonSetup)
            ToggleMenuItems(False, gMonSetupAddWizard)
            ToggleMenuItems(False, gMonTools)
            ToggleMenuItems(False, gMonTrayFile)
            ToggleMenuItems(False, gMonTraySetup)
            ToggleMenuItems(False, gMonTrayTools)
            gMonNotification.Enabled = False
            gMonTrayNotification.Enabled = False
            gMonTraySettings.Enabled = False
            If Not bGameDetected Then
                gMonTrayMon.Enabled = False
                gMonTrayExit.Enabled = False
                gMonStatusStrip.Enabled = False
            End If
            ToggleLaunchMenuItems(False)
            bMenuEnabled = False
        Else
            ToggleMenuItems(True, gMonFile)
            ToggleMenuItems(True, gMonSetup)
            ToggleMenuItems(True, gMonSetupAddWizard)
            ToggleMenuItems(True, gMonTools)
            ToggleMenuItems(True, gMonTrayFile)
            ToggleMenuItems(True, gMonTraySetup)
            ToggleMenuItems(True, gMonTrayTools)
            gMonNotification.Enabled = True
            gMonTrayNotification.Enabled = True
            gMonTraySettings.Enabled = True
            gMonTrayMon.Enabled = True
            gMonTrayExit.Enabled = True
            gMonStatusStrip.Enabled = True
            ToggleLaunchMenuItems(True)
            bMenuEnabled = True
        End If
    End Sub

    Private Sub ToggleMenuText()
        Select Case eCurrentStatus
            Case eStatus.Running
                gMonFileMonitor.Text = frmMain_gMonFileMonitor_Stop
                gMonTrayMon.Text = frmMain_gMonFileMonitor_Stop
            Case eStatus.Stopped
                gMonFileMonitor.Text = frmMain_gMonFileMonitor_Start
                gMonTrayMon.Text = frmMain_gMonFileMonitor_Start
            Case eStatus.Paused
                gMonFileMonitor.Text = frmMain_gMonFileMonitor_Cancel
                gMonTrayMon.Text = frmMain_gMonFileMonitor_Cancel
        End Select
    End Sub

    Public Sub UpdateStatus(ByVal sStatus As String, Optional ByVal sTrayStatus As String = "")
        'Thread Safe
        If Me.InvokeRequired = True Then
            Dim d As New UpdateStatusCallBack(AddressOf UpdateStatus)
            Me.Invoke(d, New Object() {sStatus})
        Else
            gMonStripTxtStatus.Text = sStatus
            If sTrayStatus = String.Empty Then
                gMonTray.Text = sStatus
            Else
                gMonTray.Text = sTrayStatus
            End If
        End If
    End Sub

    Private Function NotifySendUnix(ByVal sLogUpdate As String, ByVal objIcon As System.Windows.Forms.ToolTipIcon) As Boolean
        Dim prsNotify As Process
        Dim sUrgency As String
        Dim sNotifyArgs As String
        Dim bNotifyFailed As Boolean

        'Build args for notify-send
        Select Case objIcon
            Case ToolTipIcon.Error
                sUrgency = "critical"
            Case ToolTipIcon.Warning
                sUrgency = "normal"
            Case ToolTipIcon.Info
                sUrgency = "low"
            Case Else
                sUrgency = "low"
        End Select

        sNotifyArgs = "-i gbm " & "-u " & sUrgency & " """ & App_NameLong & """ ""<i>" & sLogUpdate.Replace("""", "\""") & "</i>"""

        Try
            'Execute notify-send
            prsNotify = New Process
            prsNotify.StartInfo.FileName = "/usr/bin/notify-send"
            prsNotify.StartInfo.Arguments = sNotifyArgs
            prsNotify.StartInfo.UseShellExecute = False
            prsNotify.StartInfo.RedirectStandardOutput = True
            prsNotify.StartInfo.CreateNoWindow = True
            prsNotify.Start()
            prsNotify.WaitForExit()
            Select Case prsNotify.ExitCode
                Case 0
                    bNotifyFailed = False
                Case Else
                    bNotifyFailed = True
            End Select
        Catch
            bNotifyFailed = True
        End Try

        Return bNotifyFailed
    End Function

    Public Sub UpdateLog(sLogUpdate As String, Optional bTrayUpdate As Boolean = True, Optional objIcon As System.Windows.Forms.ToolTipIcon = ToolTipIcon.Info, Optional bTimeStamp As Boolean = True) Handles oBackup.UpdateLog, oRestore.UpdateLog
        Dim bNotifyFailed As Boolean

        'Thread Safe (If one control requires an invoke assume they all do)
        If txtLog.InvokeRequired = True Then
            Dim d As New UpdateLogCallBack(AddressOf UpdateLog)
            Me.Invoke(d, New Object() {sLogUpdate, bTrayUpdate, objIcon, bTimeStamp})
        Else
            'Auto save and/or clear the log if we are approaching the limit
            If txtLog.TextLength > 262144 Then
                If mgrSettings.AutoSaveLog Then
                    Dim sLogFile As String = mgrPath.LogFileLocation
                    mgrCommon.SaveText(txtLog.Text, sLogFile)
                    txtLog.Clear()
                    txtLog.AppendText("[" & Date.Now & "] " & mgrCommon.FormatString(frmMain_LogAutoSave, sLogFile))
                Else
                    txtLog.Clear()
                    txtLog.AppendText("[" & Date.Now & "] " & frmMain_LogAutoClear)
                End If
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

            If bTrayUpdate Then
                If mgrCommon.IsUnix Then
                    bNotifyFailed = NotifySendUnix(sLogUpdate, objIcon)
                End If

                If Not mgrCommon.IsUnix Or bNotifyFailed Then
                    gMonTray.BalloonTipText = sLogUpdate
                    gMonTray.BalloonTipIcon = objIcon
                    gMonTray.ShowBalloonTip(10000)
                End If
            End If
        End If
        Application.DoEvents()
    End Sub

    Private Sub ClearLog()
        If mgrCommon.ShowMessage(frmMain_ConfirmLogClear, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            txtLog.Clear()
        End If
    End Sub

    Private Sub SaveLog()
        Dim sLocation As String
        Dim oExtensions As New SortedList

        oExtensions.Add(frmMain_Text, "*.txt")
        sLocation = mgrCommon.SaveFileBrowser("Log_File", frmMain_ChooseLogFile, oExtensions, 1, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmMain_DefaultLogFileName & " " & Date.Now.ToString("dd-MMM-yyyy"))

        If sLocation <> String.Empty Then
            mgrCommon.SaveText(txtLog.Text, sLocation)
        End If
    End Sub

    Private Sub SwitchDisplayMode()
        Select Case eDisplayMode
            Case eDisplayModes.Initial
                btnRestore.Visible = False
                btnBackup.Visible = False
                btnEdit.Visible = False
                btnPlay.Visible = False
                lstGames.Enabled = True
                SelectLastSelectedGame()
                btnClearSelected.Enabled = True
                txtSearch.Enabled = True
                txtSearch.Focus()
            Case eDisplayModes.Normal
                If mgrSettings.MainHideButtons Then
                    btnRestore.Visible = False
                    btnBackup.Visible = False
                    btnEdit.Visible = False
                    btnPlay.Visible = False
                Else
                    btnRestore.Visible = True
                    btnBackup.Visible = True
                    btnEdit.Visible = True
                    If mgrSettings.EnableLauncher Then
                        btnPlay.Visible = True
                    Else
                        btnPlay.Visible = False
                    End If
                End If
                SelectLastSelectedGame()
                If oLastGame.MonitorOnly Then
                    btnRestore.Enabled = False
                    btnBackup.Enabled = False
                Else
                    btnRestore.Enabled = True
                    btnBackup.Enabled = True
                End If
                lstGames.Enabled = True
                btnClearSelected.Enabled = True
                txtSearch.Enabled = True
                txtSearch.Focus()
            Case eDisplayModes.Busy
                If mgrSettings.MainHideButtons Then
                    btnBackup.Visible = False
                    btnRestore.Visible = False
                    btnEdit.Visible = False
                    btnPlay.Visible = False
                Else
                    btnRestore.Visible = False
                    btnBackup.Visible = False
                    btnEdit.Visible = False
                    btnPlay.Visible = False
                    'Change these buttons only while currently monitoring a game
                    If eCurrentStatus = eStatus.Monitoring Then
                        If Not oProcess.Duplicate And Not oProcess.GameInfo.MonitorOnly And mgrSettings.EnableLiveBackup Then
                            btnRestore.Enabled = True
                            btnRestore.Visible = True
                            btnBackup.Enabled = True
                            btnBackup.Visible = True
                        End If
                    End If
                End If
                lstGames.Enabled = False
                txtSearch.Enabled = False
                btnClearSelected.Enabled = False
            Case eDisplayModes.GameSelected
                If mgrSettings.MainHideButtons Then
                    btnRestore.Visible = False
                    btnBackup.Visible = False
                    btnEdit.Visible = False
                    btnPlay.Visible = False
                Else
                    btnRestore.Visible = True
                    btnBackup.Visible = True
                    btnEdit.Visible = True
                    If mgrSettings.EnableLauncher Then
                        btnPlay.Visible = True
                    Else
                        btnPlay.Visible = False
                    End If
                End If
                If oSelectedGame.MonitorOnly Then
                    btnRestore.Enabled = False
                    btnBackup.Enabled = False
                Else
                    btnRestore.Enabled = True
                    btnBackup.Enabled = True
                End If
                lstGames.Enabled = True
                txtSearch.Enabled = True
                btnClearSelected.Enabled = True
        End Select
    End Sub

    Private Sub SetForm()
        'Set Minimum Size
        Me.MinimumSize = New Size(Me.Size.Width - slcMain.SplitterDistance, Me.Size.Height - txtLog.Size.Height)

        'Set Form Name
        Me.Text = App_NameLong
        Me.Icon = GBM_Icon

        'Set Menu Text
        gMonFile.Text = frmMain_gMonFile
        gMonFileMonitor.Text = frmMain_gMonFileMonitor_Start
        gMonFileFullBackup.Text = frmMain_gMonFileFullBackup
        gMonFileFullRestore.Text = frmMain_gMonFileFullRestore
        gMonFileImport.Text = frmMain_gMonFileImport
        gMonFileImportOfficial.Text = frmMain_gMonFileImportOfficial
        gMonFileImportOfficialWindows.Text = frmMain_gMonFileImportOfficialWindows
        gMonFileImportOfficialLinux.Text = frmMain_gMonFileImportOfficialLinux
        gMonFileImportLudusavi.Text = frmMain_gMonFileImportLudusavi
        gMonFileImportFile.Text = frmMain_gMonFileImportFile
        gMonFileImportURL.Text = frmMain_gMonFileImportURL
        gMonFileExport.Text = frmMain_gMonFileExport
        gMonOpenBackupFolder.Text = frmMain_gMonOpenBackupFolder
        gMonFileSettings.Text = frmMain_gMonFileSettings
        gMonFileExit.Text = frmMain_gMonFileExit
        gMonSetup.Text = frmMain_gMonSetup
        gMonSetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonSetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonSetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonSetupTags.Text = frmMain_gMonSetupTags
        gMonSetupLauncherManager.Text = frmMain_gMonSetupLauncherManager
        gMonSetupProcessManager.Text = frmMain_gMonSetupProcessManager
        gMonTools.Text = frmMain_gMonTools
        gMonToolsImportBackup.Text = frmMain_gMonToolsImportBackup
        gMonToolsImportBackupFiles.Text = frmMain_gMonToolsImportBackupFiles
        gMonToolsImportBackupFolder.Text = frmMain_gMonToolsImportBackupFolder
        gMonToolsCompact.Text = frmMain_gMonToolsCompact
        gMonToolsLog.Text = frmMain_gMonToolsLog
        gMonToolsSessions.Text = frmMain_gMonToolsSessions
        gMonLogClear.Text = frmMain_gMonLogClear
        gMonLogSave.Text = frmMain_gMonLogSave
        gMonHelp.Text = frmMain_gMonHelp
        gMonHelpWebSite.Text = frmMain_gMonHelpWebSite
        gMonHelpManual.Text = frmMain_gMonHelpManual
        gMonHelpCheckforUpdates.Text = frmMain_gMonHelpCheckForUpdates
        gMonHelpAbout.Text = frmMain_gMonHelpAbout

        'Set Tray Menu Text
        gMonTrayShow.Text = frmMain_gMonTrayShow
        gMonTrayMon.Text = frmMain_gMonFileMonitor_Start
        gMonTrayFile.Text = frmMain_gMonFile
        gMonTrayFileFullBackup.Text = frmMain_gMonTrayFullBackup
        gMonTrayFileFullRestore.Text = frmMain_gMonTrayFullRestore
        gMonTrayFileImport.Text = frmMain_gMonFileImport
        gMonTrayFileImportOfficial.Text = frmMain_gMonFileImportOfficial
        gMonTrayFileImportOfficialLinux.Text = frmMain_gMonFileImportOfficialLinux
        gMonTrayFileImportOfficialWindows.Text = frmMain_gMonFileImportOfficialWindows
        gMonTrayFileImportLudusavi.Text = frmMain_gMonFileImportLudusavi
        gMonTrayFileImportFile.Text = frmMain_gMonFileImportFile
        gMonTrayFileImportURL.Text = frmMain_gMonFileImportURL
        gMonTrayFileExport.Text = frmMain_gMonFileExport
        gMonTrayOpenBackupFolder.Text = frmMain_gMonOpenBackupFolder
        gMonTraySettings.Text = frmMain_gMonFileSettings
        gMonTraySetup.Text = frmMain_gMonTraySetup
        gMonTraySetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonTraySetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonTraySetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonTraySetupTags.Text = frmMain_gMonSetupTags
        gMonTraySetupLauncherManager.Text = frmMain_gMonTraySetupLauncherManager
        gMonTraySetupProcessManager.Text = frmMain_gMonSetupProcessManager
        gMonTrayTools.Text = frmMain_gMonTools
        gMonTrayToolsImportBackup.Text = frmMain_gMonTrayToolsImportBackup
        gMonTrayToolsImportBackupFiles.Text = frmMain_gMonTrayToolsImportBackupFiles
        gMonTrayToolsImportBackupFolder.Text = frmMain_gMonTrayToolsImportBackupFolder
        gMonTrayToolsCompact.Text = frmMain_gMonToolsCompact
        gMonTrayToolsLog.Text = frmMain_gMonToolsLog
        gMonTrayToolsSessions.Text = frmMain_gMonToolsSessions
        gMonTrayLogClear.Text = frmMain_gMonLogClear
        gMonTrayLogSave.Text = frmMain_gMonLogSave
        gMonTrayExit.Text = frmMain_gMonFileExit

        'Set Form Text
        lblSearch.Text = frmMain_lblSearch
        lblLastActionTitle.Text = frmMain_lblLastActionTitle
        lblLastAction.Text = frmMain_lblLastAction
        gMonStripStatusButton.Text = frmMain_gMonStripStatusButton
        gMonStripStatusButton.ToolTipText = frmMain_gMonStripStatusButtonToolTip
        gMonStripCollapse.ToolTipText = frmMain_gMonStripCollapseHideToolTip
        gMonStripCollapse.Image = frmMain_Collapse_Left
        btnRestore.Text = frmMain_btnRestore
        btnRestore.Image = Multi_Restore
        btnBackup.Text = frmMain_btnBackup
        btnBackup.Image = Multi_Backup
        btnEdit.Text = frmMain_btnEdit
        btnEdit.Image = Multi_Edit
        btnPlay.Text = frmMain_btnPlay
        btnPlay.Image = frmMain_Play
        btnCancelOperation.Text = frmMain_btnCancelOperation
        btnCancelOperation.Image = Multi_Cancel
        btnClearSelected.Image = frmMain_Cancel_Small

        If mgrCommon.IsElevated Then
            gMonStripAdminButton.Image = frmMain_Admin
            gMonStripAdminButton.ToolTipText = frmMain_RunningAsAdmin

        Else
            gMonStripAdminButton.Image = frmMain_User
            gMonStripAdminButton.ToolTipText = frmMain_RunningAsNormal
        End If
        btnCancelOperation.Visible = False
        pbTime.SizeMode = PictureBoxSizeMode.AutoSize
        pbTime.Image = frmMain_Clock

        'Init Official Import Menu
        If mgrCommon.IsUnix Then
            gMonFileImportOfficial.Text = gMonFileImportOfficial.Text.TrimEnd(".")
            gMonTrayFileImportOfficial.Text = gMonTrayFileImportOfficial.Text.TrimEnd(".")
            RemoveHandler gMonFileImportOfficial.Click, AddressOf gMonFileImportOfficial_Click
            RemoveHandler gMonTrayFileImportOfficial.Click, AddressOf gMonFileImportOfficial_Click
        Else
            gMonFileImportOfficialLinux.Visible = False
            gMonFileImportOfficialWindows.Visible = False
            gMonTrayFileImportOfficialLinux.Visible = False
            gMonTrayFileImportOfficialWindows.Visible = False
        End If

        'Init Timers
        tmScanTimer.Interval = 5000
        tmRestoreCheck.Interval = 60000
        tmFileWatcherQueue.AutoReset = False
        tmFileWatcherQueue.Interval = 30000
        tmSessionTimeUpdater.Interval = 60000
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
        tmPlayTimer.Interval = 5000
        tmPlayTimer.AutoReset = False

        ResetCurrentInfo()
    End Sub

    Private Sub SetDetectionSpeed()
        tmScanTimer.Interval = mgrSettings.DetectionSpeed
    End Sub

    Private Sub HandleUISettings()
        If mgrSettings.MainHideGameList Then
            slcMain.Panel1Collapsed = True
            gMonStripCollapse.ToolTipText = frmMain_gMonStripCollapseShowToolTip
            gMonStripCollapse.Image = frmMain_Expand_Right
        End If

        If mgrSettings.MainHideLog And mgrSettings.MainHideGameList Then
            Me.Size = New Size(Me.Size.Width - slcMain.SplitterDistance, Me.Size.Height - txtLog.Size.Height)
        ElseIf mgrSettings.MainHideLog And Not mgrSettings.MainHideGameList Then
            Me.Size = New Size(Me.Size.Width, Me.Size.Height - txtLog.Size.Height)
        ElseIf Not mgrSettings.MainHideLog And mgrSettings.MainHideGameList Then
            Me.Size = New Size(Me.Size.Width - slcMain.SplitterDistance, Me.Size.Height)
        End If
    End Sub

    Private Function BuildChildProcesses() As Integer
        Dim oCurrentProcess As clsProcess
        Dim oProcessList As Hashtable
        Dim prsChild As Process
        Dim iRunningPid As Integer

        oChildProcesses.Clear()

        oProcessList = mgrGameProcesses.GetProcessesByGame(oProcess.GameInfo.ID)

        If oProcessList.Count > 0 Then
            For Each oCurrentProcess In oProcessList.Values
                iRunningPid = mgrProcessDetection.CheckForRunningProcess(oCurrentProcess.Path)
                If iRunningPid = -1 Then
                    prsChild = New Process
                    prsChild.StartInfo.Arguments = oCurrentProcess.Args
                    prsChild.StartInfo.FileName = oCurrentProcess.Path
                    prsChild.StartInfo.WorkingDirectory = Path.GetDirectoryName(oCurrentProcess.Path)
                    prsChild.StartInfo.UseShellExecute = True
                    prsChild.StartInfo.CreateNoWindow = True
                    oChildProcesses.Add(oCurrentProcess, prsChild)
                Else
                    UpdateLog(mgrCommon.FormatString(frmMain_ProcessAlreadyRunning, New String() {oCurrentProcess.Name, iRunningPid}), False)
                End If
            Next
        End If

        Return oChildProcesses.Count
    End Function

    Private Function StartChildProcess(ByRef prsChild As Process, Optional ByVal bAdmin As Boolean = False) As Boolean
        Try
            If bAdmin Then prsChild.StartInfo.Verb = "runas"
            prsChild.Start()
            Return True
        Catch exWin32 As System.ComponentModel.Win32Exception
            'If the launch fails due to required elevation, try it again and request elevation.
            If exWin32.ErrorCode = 740 Then
                StartChildProcess(prsChild, True)
            Else
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorStartChildProcess, New String() {oProcess.GameInfo.CroppedName, exWin32.Message}), True, ToolTipIcon.Error)
            End If
            Return False
        Catch exAll As Exception
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorStartChildProcess, New String() {oProcess.GameInfo.CroppedName, exAll.Message}), True, ToolTipIcon.Error)
            Return False
        End Try
    End Function

    Private Sub StartChildProcesses()
        Dim oCurrentProcess As clsProcess
        Dim prsChild As Process

        If BuildChildProcesses() > 0 Then
            For Each de As DictionaryEntry In oChildProcesses
                oCurrentProcess = DirectCast(de.Key, clsProcess)
                prsChild = DirectCast(de.Value, Process)
                If StartChildProcess(prsChild) Then
                    UpdateLog(mgrCommon.FormatString(frmMain_ProcessStarted, oCurrentProcess.Name), False)
                End If
            Next
        End If
    End Sub

    Private Sub EndChildProcesses()
        Dim oCurrentProcess As clsProcess
        Dim prsChild As Process

        If oChildProcesses.Count > 0 Then
            Try
                For Each de As DictionaryEntry In oChildProcesses
                    oCurrentProcess = DirectCast(de.Key, clsProcess)
                    prsChild = DirectCast(de.Value, Process)
                    If oCurrentProcess.Kill Then
                        prsChild.Kill()
                        UpdateLog(mgrCommon.FormatString(frmMain_ProcessKilled, oCurrentProcess.Name), False)
                    End If
                Next

            Catch ex As Exception
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorEndChildProcess, New String() {oProcess.GameInfo.CroppedName, ex.Message}), True, ToolTipIcon.Error)
            End Try
        End If
    End Sub

    'Functions that control the scanning for games
    Private Sub HandleScan()
        If eCurrentStatus = eStatus.Running Then
            tmScanTimer.Stop()
            eCurrentStatus = eStatus.Stopped
            UpdateStatus(frmMain_NotScanning)
            gMonStripStatusButton.Image = frmMain_Stopped
            gMonTray.Icon = GBM_Icon_Stopped
        Else
            tmScanTimer.Start()
            eCurrentStatus = eStatus.Running
            UpdateStatus(frmMain_NoGameDetected)
            gMonStripStatusButton.Image = frmMain_Ready
            gMonTray.Icon = GBM_Icon_Ready
        End If
        ToggleMenuText()
    End Sub

    Private Sub PauseScan(Optional ByVal bGameDetected As Boolean = False)
        If eCurrentStatus = eStatus.Running Then
            tmScanTimer.Stop()
            If bGameDetected Then
                eCurrentStatus = eStatus.Monitoring
            Else
                eCurrentStatus = eStatus.Paused
                ToggleHotKeys(False)
            End If
            UpdateStatus(frmMain_NotScanning)
            gMonStripStatusButton.Image = frmMain_Detected
            gMonTray.Icon = GBM_Icon_Detected
        End If
        ToggleMenuText()
        ToggleMenuEnable(bGameDetected)
    End Sub

    Private Sub ResumeScan()
        If eCurrentStatus = eStatus.Running Or eCurrentStatus = eStatus.Paused Then
            tmScanTimer.Start()
            ToggleHotKeys(True)
            eCurrentStatus = eStatus.Running
            gMonStripStatusButton.Image = frmMain_Ready
            gMonTray.Icon = GBM_Icon_Ready
            UpdateStatus(frmMain_NoGameDetected)
        End If
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    Private Sub StopScan()
        tmScanTimer.Stop()
        eCurrentStatus = eStatus.Stopped
        UpdateStatus(frmMain_NotScanning)
        gMonStripStatusButton.Image = frmMain_Stopped
        gMonTray.Icon = GBM_Icon_Stopped
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    'Functions to handle verification
    Private Sub VerifyCustomPathVariables()
        Dim sGames As String = String.Empty

        If Not mgrPath.VerifyCustomVariables(hshScanList, sGames) Then
            mgrCommon.ShowPriorityMessage(frmMain_ErrorCustomVariable, sGames, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Function WaitForBackupPath(ByRef sBackupPath As String) As Boolean
        Dim dBrowser As FolderBrowserDialog
        Dim oDialogResult As DialogResult
        Dim iTotalWait As Integer
        Dim iTimeOut As Integer = 60000
        Dim iTimeRemaining As Integer

        Do While Not (Directory.Exists(sBackupPath))
            If iTotalWait = 0 Then
                gMonTray.Icon = GBM_Icon_Stopped
                gMonTray.Text = mgrCommon.FormatString(frmMain_BackupPathNotAvailable, (iTimeOut / 1000).ToString)
            End If

            Sleep(5000)

            iTotalWait += 5000
            iTimeRemaining = iTimeOut - iTotalWait
            If Not iTimeRemaining = 0 Then iTimeRemaining /= 1000
            gMonTray.Text = mgrCommon.FormatString(frmMain_BackupPathNotAvailable, iTimeRemaining.ToString)

            If iTotalWait >= iTimeOut Then
                oDialogResult = mgrCommon.ShowPriorityMessage(mgrPath_ConfirmBackupLocation, sBackupPath, MsgBoxStyle.YesNoCancel)
                If oDialogResult = MsgBoxResult.Yes Then
                    dBrowser = New FolderBrowserDialog
                    dBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    If dBrowser.ShowDialog = DialogResult.OK Then
                        sBackupPath = dBrowser.SelectedPath
                        Return True
                    Else
                        Return False
                    End If
                ElseIf oDialogResult = DialogResult.No Then
                    Return False
                Else
                    iTotalWait = 0
                End If
            End If
        Loop

        Return True
    End Function

    Private Function VerifyBackupLocation() As Boolean
        Dim sBackupPath As String = mgrSettings.BackupFolder

        If WaitForBackupPath(sBackupPath) Then
            If mgrSettings.BackupFolder <> sBackupPath Then
                mgrSettings.BackupFolder = sBackupPath
                mgrSettings.SaveSettings()
                mgrSync.HandleBackupLocationChange()
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub VerifyGameDataPath()
        'Important: This function cannot access mgrPath for settings, as that will trigger a database creation and destroy the reason for this function
        Dim sSettingsRoot As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & Path.DirectorySeparatorChar & "gbm"
        Dim sDBLocation As String = sSettingsRoot & Path.DirectorySeparatorChar & "gbm.s3db"

        If Not Directory.Exists(sSettingsRoot) Then
            Try
                Directory.CreateDirectory(sSettingsRoot)
            Catch ex As Exception
                mgrCommon.ShowMessage(frmMain_ErrorSettingsFolder, ex.Message, MsgBoxStyle.Critical)
                bShutdown = True
                Me.Close()
            End Try
        End If

        If Not File.Exists(sDBLocation) Then bFirstRun = True
    End Sub

    Private Sub VerifyDBVersion(ByVal iDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iDB)
        Dim iDBVer As Integer

        If Not oDatabase.CheckDBVer(iDBVer) Then
            Select Case iDB
                Case mgrSQLite.Database.Local
                    mgrCommon.ShowMessage(frmMain_ErrorDBVerLocal, New String() {iDBVer, mgrCommon.AppVersion}, MsgBoxStyle.Critical)
                Case mgrSQLite.Database.Remote
                    mgrCommon.ShowMessage(frmMain_ErrorDBVerRemote, New String() {iDBVer, mgrCommon.AppVersion}, MsgBoxStyle.Critical)
            End Select

            bShutdown = True
            Me.Close()
        End If
    End Sub

    Private Function VerifyAutoStartLinux(ByRef sErrorMessage As String) As Boolean
        Dim oProcess As Process
        Dim sDesktopFile As String = String.Empty
        Dim sAutoStartFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & Path.DirectorySeparatorChar & ".config/autostart/"

        'Check if the app is still properly installed
        If mgrPath.VerifyLinuxDesktopFileLocation(sDesktopFile) Then
            If File.Exists(sAutoStartFolder & Path.DirectorySeparatorChar & "gbm.desktop") Then
                Return True
            Else
                'Create the autostart folder if it doesn't exist yet
                If Not Directory.Exists(sAutoStartFolder) Then
                    Directory.CreateDirectory(sAutoStartFolder)
                End If
                'Create link
                Try
                    oProcess = New Process
                    oProcess.StartInfo.FileName = "/bin/ln"
                    oProcess.StartInfo.Arguments = "-s " & sDesktopFile & " " & sAutoStartFolder
                    oProcess.StartInfo.UseShellExecute = False
                    oProcess.StartInfo.RedirectStandardOutput = True
                    oProcess.StartInfo.CreateNoWindow = True
                    oProcess.Start()
                Catch ex As Exception
                    mgrCommon.ShowMessage(frmSettings_ErrorLinuxAutoStart, ex.Message, MsgBoxStyle.Exclamation)
                End Try

                sErrorMessage = frmMain_ErrorLinuxAutoStartMissing
                Return False
            End If
        Else
            'If the app is no longer properly installed,  disable autostart and the setting.
            Try
                mgrSettings.StartWithWindows = False
                mgrSettings.SaveSettings()
                If File.Exists(sAutoStartFolder & Path.DirectorySeparatorChar & "gbm.desktop") Then
                    File.Delete(sAutoStartFolder & Path.DirectorySeparatorChar & "gbm.desktop")
                End If
            Catch ex As Exception
                mgrCommon.ShowMessage(frmSettings_ErrorLinuxAutoStart, ex.Message, MsgBoxStyle.Exclamation)
            End Try

            sErrorMessage = frmMain_ErrorLinuxAutoStartLinkMissing
            Return False
        End If
    End Function

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

    Private Function CheckForSavedPath() As Boolean
        If oProcess.GameInfo.ProcessPath <> String.Empty Then
            Return True
        End If

        Return False
    End Function

    'Functions to handle other features
    Private Sub RestartAsAdmin()
        'Unix Hanlder
        If Not mgrCommon.IsUnix Then
            If mgrCommon.IsElevated Then
                mgrCommon.ShowMessage(frmMain_ErrorAlreadyAdmin, MsgBoxStyle.Information)
            Else
                If mgrCommon.ShowMessage(frmMain_ConfirmRunAsAdmin, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    If mgrCommon.RestartAsAdmin() Then
                        bShutdown = True
                        ShutdownApp(False)
                    End If
                End If
            End If
        Else
            mgrCommon.ShowMessage(App_ErrorUnixNotAvailable, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub CompactDatabases()
        Dim oLocalDatabase As mgrSQLite
        Dim oRemoteDatabase As mgrSQLite

        PauseScan()

        If mgrCommon.ShowMessage(frmMain_ConfirmRebuild, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            oLocalDatabase = New mgrSQLite(mgrSQLite.Database.Local)
            oRemoteDatabase = New mgrSQLite(mgrSQLite.Database.Remote)

            UpdateLog(mgrCommon.FormatString(frmMain_LocalCompactInit, oLocalDatabase.GetDBSize), False)
            oLocalDatabase.CompactDatabase()
            UpdateLog(mgrCommon.FormatString(frmMain_LocalCompactComplete, oLocalDatabase.GetDBSize), False)

            UpdateLog(mgrCommon.FormatString(frmMain_RemoteCompactInit, oRemoteDatabase.GetDBSize), False)
            oRemoteDatabase.CompactDatabase()
            UpdateLog(mgrCommon.FormatString(frmMain_RemoteCompactComplete, oRemoteDatabase.GetDBSize), False)
        End If

        ResumeScan()

    End Sub

    Private Sub ImportBackupFiles()
        Dim sFilestoImport As String()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim oExtensions As New SortedList

        oExtensions.Add(frmGameManager_7zBackup, "*.7z")
        sFilestoImport = mgrCommon.OpenMultiFileBrowser("Main_BackupFileImport", frmMain_ChooseImportFiles, oExtensions, 1, sDefaultFolder, True)

        If sFilestoImport.Length > 0 Then
            RunImportBackupByFile(sFilestoImport)
            LoadGameSettings()
        End If
    End Sub

    Private Sub ImportBackupFolder()
        Dim sFilesFound As List(Of String)
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sRootFolder As String

        sRootFolder = mgrCommon.OpenFolderBrowser("Main_BackupFolderImport", frmMain_ChooseImportFolder, sDefaultFolder, True)

        If Not sRootFolder = String.Empty Then
            sFilesFound = mgrCommon.GetFileListByFolder(sRootFolder, New String() {"*.7z"})
            Dim sFilesToImport(sFilesFound.Count - 1) As String
            sFilesFound.CopyTo(sFilesToImport)
            RunImportBackupByFile(sFilesToImport)
            LoadGameSettings()
        End If
    End Sub

    'Event Handlers
    Private Sub gMonFileMonitor_Click(sender As Object, e As EventArgs) Handles gMonFileMonitor.Click, gMonTrayMon.Click
        ScanToggle()
    End Sub

    Private Sub gMonFileFullBackup_Click(sender As Object, e As EventArgs) Handles gMonFileFullBackup.Click, gMonTrayFileFullBackup.Click
        RunBackupAll()
    End Sub

    Private Sub gMonFileFullRestore_Click(sender As Object, e As EventArgs) Handles gMonFileFullRestore.Click, gMonTrayFileFullRestore.Click
        RunRestoreAll()
    End Sub

    Private Sub gMonTray_MouseDoubleClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles gMonTray.MouseDoubleClick
        If e.Button = MouseButtons.Left Then ShowApp()
    End Sub

    Private Sub gMonTray_BalloonTipClicked(sender As Object, e As EventArgs) Handles gMonTray.BalloonTipClicked
        ShowApp()
    End Sub

    Private Sub gMonTrayShow_Click(sender As System.Object, e As System.EventArgs) Handles gMonTrayShow.Click
        ShowApp()
    End Sub

    Private Sub FileExit_Click(sender As Object, e As EventArgs) Handles gMonFileExit.Click, gMonTrayExit.Click
        ShutdownApp()
    End Sub

    Private Sub gMonFileImportOfficial_Click(sender As Object, e As EventArgs) Handles gMonFileImportOfficialWindows.Click, gMonFileImportOfficial.Click, gMonTrayFileImportOfficialWindows.Click, gMonTrayFileImportOfficial.Click
        PauseScan()
        If mgrMonitorList.ImportOfficialGameList(App_URLImport, True) Then
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub gMonFileImportOfficialLinux_Click(sender As Object, e As EventArgs) Handles gMonFileImportOfficialLinux.Click, gMonTrayFileImportOfficialLinux.Click
        PauseScan()
        If mgrMonitorList.ImportOfficialGameList(App_URLImportLinux) Then
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub gMonFileImportLudusavi_Click(sender As Object, e As EventArgs) Handles gMonFileImportLudusavi.Click, gMonTrayFileImportLudusavi.Click
        PauseScan()
        If mgrMonitorList.ImportLudusaviManifest(App_URLImportLudusavi) Then
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub gMonFileImportFile_Click(sender As Object, e As EventArgs) Handles gMonFileImportFile.Click, gMonTrayFileImportFile.Click
        PauseScan()
        If mgrMonitorList.ImportGameListFile Then
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub gMonFileImportURL_Click(sender As Object, e As EventArgs) Handles gMonFileImportURL.Click, gMonTrayFileImportURL.Click
        PauseScan()
        If mgrMonitorList.ImportGameListURL Then
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub gMonFileExport_Click(sender As Object, e As EventArgs) Handles gMonFileExport.Click, gMonTrayFileExport.Click
        mgrMonitorList.ExportGameList()
    End Sub

    Private Sub gMonOpenBackupFolder_Click(sender As Object, e As EventArgs) Handles gMonOpenBackupFolder.Click, gMonTrayOpenBackupFolder.Click
        mgrCommon.OpenInOS(mgrSettings.BackupFolder)
    End Sub

    Private Sub FileSettings_Click(sender As Object, e As EventArgs) Handles gMonFileSettings.Click, gMonTraySettings.Click
        OpenSettings()
    End Sub

    Private Sub SetupGameManager_Click(sender As Object, e As EventArgs) Handles gMonSetupGameManager.Click, gMonTraySetupGameManager.Click
        OpenGameManager(oLastGame)
    End Sub

    Private Sub gMonToolsCompact_Click(sender As Object, e As EventArgs) Handles gMonToolsCompact.Click, gMonTrayToolsCompact.Click
        CompactDatabases()
    End Sub

    Private Sub gMonToolsImportBackupFiles_Click(sender As Object, e As EventArgs) Handles gMonToolsImportBackupFiles.Click, gMonTrayToolsImportBackupFiles.Click
        ImportBackupFiles()
    End Sub

    Private Sub gMonToolsImportBackupFolder_Click(sender As Object, e As EventArgs) Handles gMonToolsImportBackupFolder.Click, gMonTrayToolsImportBackupFolder.Click
        ImportBackupFolder()
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

    Private Sub gMonSetupLauncherManager_Click(sender As Object, e As EventArgs) Handles gMonSetupLauncherManager.Click, gMonTraySetupLauncherManager.Click
        OpenLauncherManager()
    End Sub

    Private Sub gMonSetupProcessManager_Click(sender As Object, e As EventArgs) Handles gMonSetupProcessManager.Click, gMonTraySetupProcessManager.Click
        OpenProcessManager()
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

    Private Sub gMonLogClear_Click(sender As Object, e As EventArgs) Handles gMonLogClear.Click, gMonTrayLogClear.Click
        ClearLog()
    End Sub

    Private Sub gMonLogSave_Click(sender As Object, e As EventArgs) Handles gMonLogSave.Click, gMonTrayLogSave.Click
        SaveLog()
    End Sub

    Private Sub gMonToolsSessions_Click(sender As Object, e As EventArgs) Handles gMonToolsSessions.Click, gMonTrayToolsSessions.Click
        OpenSessions()
    End Sub

    Private Sub gMonNotification_Click(sender As Object, e As EventArgs) Handles gMonNotification.Click, gMonTrayNotification.Click
        gMonNotification.Visible = False
        gMonTrayNotification.Visible = False
        OpenGameManager(oLastGame, True)
    End Sub

    Private Sub gMonStripSplitStatusButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripStatusButton.Click
        ScanToggle()
    End Sub

    Private Sub gMonStripCollapse_Click(sender As Object, e As EventArgs) Handles gMonStripCollapse.Click
        If bListRefresh Then RefreshGameList()
        slcMain.Panel1Collapsed = Not slcMain.Panel1Collapsed

        If slcMain.Panel1Collapsed Then
            gMonStripCollapse.Image = frmMain_Expand_Right
            gMonStripCollapse.ToolTipText = frmMain_gMonStripCollapseShowToolTip
        Else
            gMonStripCollapse.Image = frmMain_Collapse_Left
            gMonStripCollapse.ToolTipText = frmMain_gMonStripCollapseHideToolTip
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not tmFilterTimer.Enabled Then
            lstGames.Enabled = False
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub

    Private Sub pbIcon_Click(sender As Object, e As EventArgs) Handles pbIcon.Click
        If bAllowIcon Then
            SetIcon()
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Select Case eDisplayMode
            Case eDisplayModes.Normal
                OpenGameManager(oLastGame)
            Case eDisplayModes.GameSelected
                OpenGameManager(oSelectedGame)
        End Select
    End Sub

    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        HandleQuickBackup()
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        HandleQuickRestore()
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        Select Case eDisplayMode
            Case eDisplayModes.Normal
                LaunchGame(oLastGame)
            Case eDisplayModes.GameSelected
                LaunchGame(oSelectedGame)
        End Select
    End Sub

    Private Sub btnCancelOperation_Click(sender As Object, e As EventArgs) Handles btnCancelOperation.Click
        OperationCancel()
    End Sub

    Private Sub btnClearSelected_Click(sender As Object, e As EventArgs) Handles btnClearSelected.Click
        lstGames.ClearSelected()
        txtSearch.Clear()
    End Sub

    Private Sub gMonStripAdminButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripAdminButton.Click
        RestartAsAdmin()
    End Sub

    Private Sub Main_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Select Case e.CloseReason
            Case CloseReason.UserClosing
                If bShutdown = False Then
                    e.Cancel = True
                    If mgrSettings.ExitOnClose Then
                        ShutdownApp()
                    Else
                        ToggleState(False)
                    End If
                End If
            Case Else
                ShutdownApp(False)
        End Select
    End Sub

    Private Sub PlayButtonEventProcessor(sender As Object, ByVal e As EventArgs) Handles tmPlayTimer.Elapsed
        EnablePlayButton()
    End Sub

    Private Sub FilterEventProcessor(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Elapsed
        RefreshGameList()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub TimedBackupEventProcessor(sender As Object, ByVal e As EventArgs) Handles tmTimedBackup.Elapsed
        RunManualBackup(New List(Of clsGame)({oProcess.GameInfo}), True)
    End Sub

    Private Sub AutoRestoreEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles tmRestoreCheck.Elapsed
        AutoRestoreCheck()
    End Sub

    Private Sub SessionTimeUpdaterEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles tmSessionTimeUpdater.Elapsed
        UpdateTimeSpent(oProcess.GameInfo.Hours + oProcess.CurrentSessionTime.TotalHours, oProcess.CurrentSessionTime.TotalHours)
    End Sub

    Private Sub ScanTimerEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles tmScanTimer.Tick
        Dim bNeedsPath As Boolean = False
        Dim bContinue As Boolean = True
        Dim iErrorCode As Integer = 0
        Dim sErrorMessage As String = String.Empty

        If oProcess.SearchRunningProcesses(hshScanList, bNeedsPath, iErrorCode, bProcessDebugMode) Then
            PauseScan(True)

            If bNeedsPath Then
                bContinue = False
                If iErrorCode = 5 Then
                    If oProcess.Duplicate Then
                        sErrorMessage = mgrCommon.FormatString(frmMain_ErrorMultiAdmin)
                        UpdateLog(sErrorMessage, True, ToolTipIcon.Warning, True)
                    Else
                        If Not CheckForSavedPath() Then
                            If Not oProcess.GameInfo.AbsolutePath And Not oProcess.GameInfo.MonitorOnly Then
                                bPathDetectionFailure = True
                                sPathDetectionError = mgrCommon.FormatString(frmMain_ErrorAdminBackup, oProcess.GameInfo.Name)
                            Else
                                UpdateLog(mgrCommon.FormatString(frmMain_WarningAdminNoPath, oProcess.GameInfo.Name), False, ToolTipIcon.Warning, True)
                            End If
                        End If
                        bContinue = True
                    End If
                ElseIf iErrorCode = 299 Then
                    If oProcess.Duplicate Then
                        sErrorMessage = mgrCommon.FormatString(frmMain_ErrorMulti64)
                        UpdateLog(sErrorMessage, True, ToolTipIcon.Warning, True)
                    Else
                        If Not CheckForSavedPath() Then
                            If Not oProcess.GameInfo.AbsolutePath And Not oProcess.GameInfo.MonitorOnly Then
                                bPathDetectionFailure = True
                                sPathDetectionError = mgrCommon.FormatString(frmMain_Error64Backup, oProcess.GameInfo.Name)
                            Else
                                UpdateLog(mgrCommon.FormatString(frmMain_Warning64NoPath, oProcess.GameInfo.Name), False, ToolTipIcon.Warning, True)
                            End If
                        End If
                        bContinue = True
                    End If
                End If
            End If

            'We need to determine this Wine information and store it before the process ends.
            If oProcess.WineProcess Then
                Dim oWineData As New clsWineData
                oWineData.Prefix = mgrPath.GetWinePrefix(oProcess.FoundProcess)
                oWineData.BinaryPath = Path.GetDirectoryName(oProcess.FoundProcess.MainModule.FileName)
                If Not oWineData.Prefix = String.Empty Then
                    oProcess.WineData = oWineData
                Else
                    bContinue = False
                End If
                If bWineDebugMode Then
                    UpdateLog(mgrCommon.FormatString(frmMain_WineBinaryPath, oWineData.BinaryPath), False)
                    UpdateLog(mgrCommon.FormatString(frmMain_WinePrefix, oWineData.Prefix), False)
                End If
            End If

            If bContinue = True Then
                If oProcess.Duplicate Then
                    oLastGame = Nothing
                    UpdateLog(frmMain_MultipleGamesDetected, mgrSettings.ShowDetectionToolTips)
                Else
                    oLastGame = oProcess.GameInfo
                    UpdateLog(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.Name), mgrSettings.ShowDetectionToolTips)
                    StartChildProcesses()
                End If

                SetCurrentInfo()
                oProcess.StartTime = Now
                bwMonitor.RunWorkerAsync()
            Else
                StopScan()
            End If
        End If

        If bMemoryDebugMode Then
            UpdateLog(mgrCommon.FormatString(frmMain_DebugMemoryAllocation, Math.Round(GC.GetTotalMemory(False) / 1000000, 2)), False, ToolTipIcon.Info, True)
        End If
    End Sub

    Private Sub bwMonitor_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwMonitor.DoWork
        Dim oCheckProcess As Process

        If mgrSettings.TimeTracking And Not oProcess.Duplicate Then tmSessionTimeUpdater.Start()

        If mgrSettings.EnableLiveBackup And Not oProcess.Duplicate Then
            If oProcess.GameInfo.TimedBackup And oProcess.GameInfo.TimedInterval >= 1 Then
                tmTimedBackup.Interval = oProcess.GameInfo.TimedInterval * 60000
                tmTimedBackup.Enabled = True
                tmTimedBackup.Start()
            End If
        End If

        Try
            Do While Not (oProcess.FoundProcess.HasExited Or bwMonitor.CancellationPending)
                If Not oProcess.Duplicate And oProcess.GameInfo.UseWindowTitle Then
                    Try
                        'We need a new instance of the process each time we check if the window title has changed.
                        oCheckProcess = Process.GetProcessById(oProcess.FoundProcess.Id)
                        'If we are matching via a window title, we'll stop monitoring when the window title no longer matches or when the process ends
                        If Not mgrProcessDetection.IsMatch(oProcess.GameInfo, oCheckProcess.MainWindowTitle) Then
                            Exit Do
                        End If
                        oCheckProcess.Dispose()
                    Catch exArg As ArgumentException
                        'An argument exception here indicates the process ended before we could get another instance of it, we can proceed normally.
                        Exit Do
                    End Try
                End If
                'This needs to be the same as the detection speed or faster to prevent the possibility of multiple background workers trying to run concurrently
                System.Threading.Thread.Sleep(mgrSettings.DetectionSpeed)
            Loop
            If bwMonitor.CancellationPending Then
                bDetectionCancelled = True
            End If
        Catch exWin32 As System.ComponentModel.Win32Exception
            'We are only expecting "Access Denied" here, anything else is a critical failure.
            If exWin32.NativeErrorCode = 5 Then
                bProcessIsAdmin = True
                oProcess.FoundProcess.WaitForExit()
                bProcessIsAdmin = False
            Else
                bDetectionFailure = True
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorCriticalDetectionFailure, oProcess.GameInfo.CroppedName), True, ToolTipIcon.Error)
                UpdateLog(mgrCommon.FormatString(App_GenericError, exWin32.Message), False,, False)
            End If
        Catch exAll As Exception
            'Any other exception is also a critical failure.
            bDetectionFailure = True
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorCriticalDetectionFailure, oProcess.GameInfo.CroppedName), True, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, exAll.Message), False,, False)
        End Try

        tmSessionTimeUpdater.Stop()
        tmTimedBackup.Stop()
        tmTimedBackup.Enabled = False
    End Sub

    Private Sub bwMain_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMonitor.RunWorkerCompleted
        Dim bContinue As Boolean = True

        EndChildProcesses()

        oProcess.EndTime = Now

        'Stop scanning after a critical detection failure to prevent looping
        If bDetectionFailure Then
            ResetCurrentInfo()
            StopScan()
        End If

        If Not bDetectionCancelled And Not bDetectionFailure Then
            eCurrentStatus = eStatus.Paused

            'Check if we failed to detect the game path
            If bPathDetectionFailure Then
                oProcess.GameInfo.ProcessPath = mgrPath.ProcessPathSearch(oProcess.GameInfo.Name, oProcess.GameInfo.ProcessName, sPathDetectionError)
                If oProcess.GameInfo.ProcessPath = String.Empty Then
                    bContinue = False
                    If mgrSettings.TimeTracking Then HandleTimeSpent()
                    If mgrSettings.SessionTracking Then HandleSession()
                    UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oProcess.GameInfo.Name), False)
                    oProcess.GameInfo = Nothing
                    ResetCurrentInfo()
                    ResumeScan()
                End If
            End If

            If bContinue Then
                If DoMultiGameCheck() Then
                    oLastGame = oProcess.GameInfo
                    UpdateLog(mgrCommon.FormatString(frmMain_GameEnded, oProcess.GameInfo.Name), False)
                    If oProcess.WineProcess Then
                        oProcess.WineData.MonitorID = oProcess.GameInfo.ID
                        'Attempt a path conversion if the game configuration is using an absolute windows path that we can convert
                        If mgrVariables.CheckForReservedVariables(oProcess.GameInfo.TruePath) Then
                            oProcess.WineData.SavePath = mgrPath.GetWineSavePath(oProcess.WineData.Prefix, oProcess.GameInfo.TruePath)
                        End If
                        mgrWineData.DoWineDataAddUpdate(oProcess.WineData)
                    End If
                    HandleProcessPath()
                    If mgrSettings.TimeTracking Then HandleTimeSpent()
                    If mgrSettings.SessionTracking Then HandleSession()
                    If Not mgrCommon.IsUnix Then HandleIconCache()
                    RunBackup()
                Else
                    oLastGame = Nothing
                    UpdateLog(Multi_UnknownGameEnded, False)
                    oProcess.GameInfo = Nothing
                    ResetCurrentInfo()
                    ResumeScan()
                End If
            End If
        End If

        'Refresh
        bPathDetectionFailure = False
        sPathDetectionError = String.Empty
        bDetectionCancelled = False
        bDetectionFailure = False
        oProcess.StartTime = Now : oProcess.EndTime = Now
        RefreshGameList()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'We need this flag because Mono fires the Load event each time the window is shown after being hidden.
        If Not bInitComplete Then InitApp()
    End Sub

    Private Sub frmMain_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        txtLog.Select(txtLog.TextLength, 0)
        txtLog.ScrollToCaret()
    End Sub

    Private Sub frmMain_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        'This event contains functions that crash or malfunction in Mono on initial start.
        If bInitialLoad And mgrCommon.IsUnix Then
            If mgrSettings.StartToTray Then
                Me.WindowState = FormWindowState.Minimized
                ToggleVisibility(False, True)
            End If
            CheckForNewBackups()
            bInitialLoad = False
        End If
    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
            Case Keys.Enter
                If eDisplayMode = eDisplayModes.GameSelected And lstGames.Items.Count = 1 Then
                    btnPlay.PerformClick()
                End If
            Case Keys.Oemtilde
                If e.Modifiers = Keys.Control Then OpenDevConsole()
        End Select
    End Sub

    Private Sub frmMain_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        'This suppresses the Windows "ding" sound any time you use the enter key because AcceptButton is not set.
        'This must happen in KeyPress, KeyDown fires too late to prevent it.
        If e.KeyChar = Chr(13) Then
            e.Handled = True
        End If
    End Sub

    Private Sub lstGames_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstGames.MouseDoubleClick
        Dim iIndex As Integer = lstGames.IndexFromPoint(e.Location)

        If iIndex <> ListBox.NoMatches Then
            OpenGameManager(oSelectedGame)
        End If
    End Sub

    Private Sub lstGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstGames.SelectedIndexChanged
        If Not bListLoading Then
            If lstGames.SelectedIndex = -1 Then
                oSelectedGame = Nothing
                If Not eDisplayMode = eDisplayModes.Busy Then
                    eDisplayMode = eDisplayModes.Normal
                    ResetCurrentInfo()
                End If
            Else
                SetSelectedGame()
                DisplaySelectedGameInfo()
            End If
        End If
    End Sub

    'This event handler lets the user clear focus from the log by clicking anywhere on the form.
    'Due to txtLog being the only focusable control in most cases, it's impossible for it to lose focus unless a change is forced.
    Private Sub ClearLogFocus(sender As Object, e As EventArgs) Handles MyBase.Click, lblGameTitle.Click, lblStatus1.Click, lblStatus2.Click,
        lblStatus3.Click, pbTime.Click, lblTimeSpent.Click, lblLastActionTitle.Click, lblLastAction.Click, gMonMainMenu.Click, gMonStatusStrip.Click
        'Move focus to first label
        lblGameTitle.Focus()
    End Sub
End Class