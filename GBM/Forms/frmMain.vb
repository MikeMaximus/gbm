Imports GBM.My.Resources
Imports System.IO

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
        Import = 4
    End Enum

    Private eCurrentStatus As eStatus = eStatus.Stopped
    Private eCurrentOperation As eOperation = eOperation.None
    Private bCancelledByUser As Boolean = False
    Private bShutdown As Boolean = False
    Private bInitFail As Boolean = False
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
    Private oPriorImage As Image
    Private sPriorPath As String
    Private sPriorCompany As String
    Private sPriorVersion As String
    Private iRestoreTimeOut As Integer
    Private oChildProcesses As New Hashtable
    Private oLastGame As clsGame

    'Developer Debug Flags
    Private bProcessDebugMode As Boolean = False
    Private bMemoryDebugMode As Boolean = False
    Private bWineDebugMode As Boolean = False

    WithEvents oFileWatcher As New FileSystemWatcher

    'Timers - There may only be one System.Windows.Forms.Timer and it must be tmScanTimer.
    WithEvents tmScanTimer As New Timer
    WithEvents tmRestoreCheck As New System.Timers.Timer
    WithEvents tmFileWatcherQueue As New System.Timers.Timer

    Public WithEvents oProcess As New mgrProcessDetection
    Public WithEvents oBackup As New mgrBackup
    Public WithEvents oRestore As New mgrRestore
    Public hshScanList As Hashtable
    Public oSettings As New mgrSettings

    Delegate Sub UpdateNotifierCallBack(ByVal iCount As Integer)
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
            lblLastAction.Text = sMessage.TrimEnd(".") & " " & mgrCommon.FormatString(frmMain_AtTime, TimeOfDay.ToString(sPattern))
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
            sStatus3 = oRestoreInfo.RestorePath
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
            Select Case eCurrentOperation
                Case eOperation.Backup, eOperation.Import
                    oBackup.CancelOperation = False
                Case eOperation.Restore
                    oRestore.CancelOperation = False
            End Select
            eCurrentOperation = eOperation.None
            LockDownMenuEnable()
            If bResume Then ResumeScan()
        End If
    End Sub

    Private Sub OperationCancel()
        Select Case eCurrentOperation
            Case eOperation.None
                'Nothing
            Case eOperation.Backup, eOperation.Import
                oBackup.CancelOperation = True
                btnCancelOperation.Enabled = False
            Case eOperation.Restore
                oRestore.CancelOperation = True
                btnCancelOperation.Enabled = False
        End Select
    End Sub

    Private Sub ExecuteBackup(ByVal oBackupList As List(Of clsGame))
        oBackup.DoBackup(oBackupList)
        OperationEnded()
    End Sub

    Private Sub ExecuteRestore(ByVal oRestoreList As List(Of clsBackup))
        oRestore.DoRestore(oRestoreList)
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

    Private Sub RunRestore(ByVal oRestoreList As Hashtable, Optional ByVal bIgnoreConfigLinks As Boolean = False, Optional ByVal bFastMode As Boolean = False)
        Dim oGame As clsGame
        Dim oReadyList As New List(Of clsBackup)
        Dim oRestoreInfo As clsBackup
        Dim bTriggerReload As Boolean = False
        Dim bOSVerified As Boolean
        Dim bPathVerified As Boolean
        Dim oQueue As New Hashtable
        eCurrentOperation = eOperation.Restore
        OperationStarted()

        If bIgnoreConfigLinks Then
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

            bOSVerified = VerifyRestoreForOS(oGame, oRestoreInfo.RestorePath)

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
                    oReadyList.Add(oRestoreInfo)
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
            If Not oSettings.DisableDiskSpaceCheck And Not oReadyList.Count = 1 Then UpdateLog(mgrCommon.FormatString(mgrBackup_BackupBatchSize, mgrCommon.FormatDiskSpace(lBackupSize)), False, ToolTipIcon.Info, True)
            Dim oThread As New System.Threading.Thread(AddressOf ExecuteBackup)
            oThread.IsBackground = True
            oThread.Start(oReadyList)
        Else
            OperationEnded()
        End If

    End Sub

    Private Sub RunImportBackupByGame(ByVal oImportBackupList As Hashtable)
        eCurrentOperation = eOperation.Import
        OperationStarted()
        oBackup.ImportBackupFilesByGame(oImportBackupList)
        OperationEnded()
    End Sub

    Private Sub RunImportBackupByFile(ByVal sFilesToImport As String())
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
    End Sub

    Private Sub RunBackup()
        Dim bDoBackup As Boolean
        Dim oRootList As New List(Of clsGame)
        Dim oReadyList As New List(Of clsGame)

        eCurrentOperation = eOperation.Backup
        OperationStarted(False)

        If oProcess.GameInfo.MonitorOnly = False Then
            If SuppressSession() Then
                bDoBackup = False
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.Name), False)
                SetLastAction(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.CroppedName))
                OperationEnded()
            Else
                If oSettings.DisableConfirmation Then
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
            gMonNotification.Image = Icon_Inbox
            gMonTrayNotification.Image = Icon_Inbox
            gMonNotification.Text = sNotification
            gMonTrayNotification.Text = sNotification
            gMonNotification.Visible = True
            gMonTrayNotification.Visible = True
        End If
    End Sub

    Private Sub StartRestoreCheck()
        iRestoreTimeOut = -1
        tmRestoreCheck.Interval = 60000
        tmRestoreCheck.AutoReset = True
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

        If oSettings.AutoMark Or oSettings.AutoRestore Then
            'Increment Timer
            iRestoreTimeOut += 1

            'Check backup files
            For Each de As DictionaryEntry In slRestoreData
                oBackup = DirectCast(de.Value, clsBackup)

                'Check if backup file is ready to restore
                If oBackup.CheckSum <> String.Empty Then
                    sFileName = oSettings.BackupFolder & Path.DirectorySeparatorChar & oBackup.FileName
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
                        oBackup.RelativeRestorePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oBackup.RestorePath
                    End If
                End If

                If oBackup.AbsolutePath Then
                    sExtractPath = oBackup.RestorePath
                Else
                    sExtractPath = oBackup.RelativeRestorePath
                End If

                If Not Directory.Exists(sExtractPath) And Not mgrPath.IsSupportedRegistryPath(oBackup.RestorePath) Then
                    If oSettings.AutoMark Then
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
                If oSettings.AutoMark Then
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
            If oSettings.AutoRestore Then
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
                    If oSettings.RestoreOnLaunch Then
                        If slRestoreData.Count > 1 Then
                            UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotificationMulti, slRestoreData.Count), True, ToolTipIcon.Info, True)
                        Else
                            UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotificationSingle, sGame), True, ToolTipIcon.Info, True)
                        End If
                    End If

                    RunRestore(hshRestore)
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
        If oSettings.RestoreOnLaunch And Not oSettings.AutoRestore Then
            If slRestoreData.Count > 0 Then
                UpdateNotifier(slRestoreData.Count)
            End If
        End If
    End Sub

    'Functions handling the display of game information
    Private Sub SetIcon()
        Dim sIcon As String
        Dim fbBrowser As New OpenFileDialog

        fbBrowser.Title = mgrCommon.FormatString(frmMain_ChooseIcon, oProcess.GameInfo.CroppedName)

        'Unix Handler
        If Not mgrCommon.IsUnix Then
            fbBrowser.DefaultExt = "ico"
            fbBrowser.Filter = frmMain_IconFilter
        Else
            fbBrowser.DefaultExt = "png"
            fbBrowser.Filter = frmMain_PNGFilter
        End If

        Try
            fbBrowser.InitialDirectory = Path.GetDirectoryName(oProcess.FoundProcess.MainModule.FileName)
        Catch ex As Exception
            fbBrowser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End Try
        fbBrowser.Multiselect = False

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            sIcon = fbBrowser.FileName
            If File.Exists(sIcon) Then
                oProcess.GameInfo.Icon = sIcon
                pbIcon.Image = mgrCommon.SafeIconFromFile(sIcon)
                mgrMonitorList.DoListUpdate(oProcess.GameInfo)
            End If
        End If
    End Sub

    Private Sub ResetGameInfo(Optional ByVal bKeepInfo As Boolean = False)
        If bKeepInfo And Not oProcess.GameInfo Is Nothing Then
            lblGameTitle.Text = mgrCommon.FormatString(frmMain_LastGame, oProcess.GameInfo.Name)
            pbIcon.Image = oPriorImage
            lblStatus1.Text = sPriorPath
            lblStatus2.Text = sPriorCompany
            lblStatus3.Text = sPriorVersion
            If oSettings.TimeTracking Then
                pbTime.Visible = True
                lblTimeSpent.Visible = True
            End If
        Else
            pbIcon.Image = Icon_Searching
            lblGameTitle.Text = frmMain_NoGameDetected
            lblStatus1.Text = String.Empty
            lblStatus2.Text = String.Empty
            lblStatus3.Text = String.Empty
            pbTime.Visible = False
            lblTimeSpent.Visible = False
        End If

        If eCurrentStatus = eStatus.Stopped Then
            UpdateStatus(frmMain_NotScanning)
        Else
            UpdateStatus(frmMain_NoGameDetected)
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
            pbIcon.Image = Icon_Working
            lblGameTitle.Text = sTitle
            lblStatus1.Text = sStatus1
            lblStatus2.Text = sStatus2
            lblStatus3.Text = sStatus3
        End If
    End Sub

    Private Sub SetGameIcon()
        Dim ic As Icon
        Dim oBitmap As Bitmap

        Try
            'Grab icon from the executable
            ic = System.Drawing.Icon.ExtractAssociatedIcon(oProcess.FoundProcess.MainModule.FileName)
            oBitmap = New Bitmap(ic.ToBitmap)
            ic.Dispose()

            'Set the icon, we need to use an intermediary object to prevent file locking
            pbIcon.Image = oBitmap

        Catch ex As Exception
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorGameIcon), False, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, ex.Message), False,, False)
        End Try
    End Sub

    Private Sub SetGameInfo(Optional ByVal bMulti As Boolean = False)
        Dim sFileName As String = String.Empty
        Dim sFileVersion As String = String.Empty
        Dim sCompanyName As String = String.Empty

        'Wipe Game Info
        lblStatus1.Text = String.Empty
        lblStatus2.Text = String.Empty
        lblStatus3.Text = String.Empty
        pbIcon.Image = Icon_Unknown

        'Set Game Icon
        If Not mgrCommon.IsUnix Then SetGameIcon()

        Try
            'Set Game Details
            sFileName = oProcess.FoundProcess.MainModule.FileName
            sFileVersion = oProcess.FoundProcess.MainModule.FileVersionInfo.FileVersion
            sCompanyName = oProcess.FoundProcess.MainModule.FileVersionInfo.CompanyName
        Catch ex As Exception
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorGameDetails), False, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, ex.Message), False,, False)
        End Try

        'Get Game Details 
        If bMulti Then
            bAllowIcon = False
            bAllowDetails = False
            lblGameTitle.Text = frmMain_MultipleGames
            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = Icon_Unknown
            If sFileName = String.Empty Then
                lblStatus1.Text = frmMain_NoDetails
            Else
                lblStatus1.Text = sFileName
            End If
        Else
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

        'Set Prior Info
        oPriorImage = pbIcon.Image
        sPriorPath = lblStatus1.Text
        sPriorCompany = lblStatus2.Text
        sPriorVersion = lblStatus3.Text
    End Sub

    Private Sub HandleProcessPath()
        If hshScanList.Contains(oProcess.GameInfo.ID) Then
            If Not oProcess.GameInfo.ProcessPath = DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).ProcessPath Then
                DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).ProcessPath = oProcess.GameInfo.ProcessPath
                mgrMonitorList.DoListFieldUpdate("ProcessPath", oProcess.GameInfo.ProcessPath, oProcess.GameInfo.ID)
                If (oSettings.SyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then mgrMonitorList.SyncMonitorLists(oSettings)
            End If
        End If
    End Sub

    Private Sub UpdateTimeSpent(ByVal dTotalTime As Double, ByVal dSessionTime As Double)
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
        If hshScanList.Contains(oProcess.GameInfo.ID) Then
            DirectCast(hshScanList.Item(oProcess.GameInfo.ID), clsGame).Hours = oProcess.GameInfo.Hours
        End If

        mgrMonitorList.DoListFieldUpdate("Hours", oProcess.GameInfo.Hours, oProcess.GameInfo.ID)
        mgrMonitorList.SyncMonitorLists(oSettings)

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
    End Sub

    Private Function SuppressSession() As Boolean
        Dim iSession As Integer
        If oSettings.SuppressBackup Then
            iSession = Math.Ceiling(oProcess.TimeSpent.TotalMinutes)
            If iSession > oSettings.SuppressBackupThreshold Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

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
        mgrMonitorList.SyncMonitorLists(oSettings)
        ResumeScan()
    End Sub

    Private Sub OpenProcessManager()
        Dim frm As New frmProcessManager
        PauseScan()
        frm.ShowDialog()
        ResumeScan()
    End Sub

    Private Sub OpenGameManager(Optional ByVal bPendingRestores As Boolean = False)
        Dim frm As New frmGameManager
        PauseScan()
        frm.Settings = oSettings
        frm.PendingRestores = bPendingRestores
        frm.LastPlayedGame = oLastGame
        frm.ShowDialog()
        LoadGameSettings()
        ResumeScan()

        'Handle backup trigger
        If frm.TriggerBackup Then
            RunManualBackup(frm.BackupList)
        End If

        'Handle restore trigger
        If frm.TriggerRestore Then
            RunRestore(frm.RestoreList, frm.IgnoreConfigLinks)
        End If

        'Handle import backup trigger
        If frm.TriggerImportBackup Then
            RunImportBackupByGame(frm.ImportBackupList)
        End If
    End Sub

    Private Sub OpenSettings()
        Dim frm As New frmSettings
        frm.Settings = oSettings
        PauseScan()
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            oSettings = frm.Settings
            oBackup.Settings = oSettings
            oRestore.Settings = oSettings
            'Set Remote Database Location
            mgrPath.RemoteDatabaseLocation = oSettings.BackupFolder
            SetupSyncWatcher()
            LoadGameSettings()
        End If
        ResumeScan()
    End Sub

    Private Sub OpenSessions()
        Dim frm As New frmSessions
        PauseScan()
        If oSettings.SessionTracking = False Then
            mgrCommon.ShowMessage(frmMain_WarningSessionsDisabled, MsgBoxStyle.Exclamation)
        End If
        If mgrSessions.CountRows > 0 Then
            frm.ShowDialog()
        Else
            mgrCommon.ShowMessage(frmMain_ErrorNoSessions, MsgBoxStyle.Information)
        End If
        ResumeScan()
    End Sub

    Private Sub OpenGameWizard()
        Dim frm As New frmAddWizard
        PauseScan()
        frm.GameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
        frm.ShowDialog()
        LoadGameSettings()
        mgrMonitorList.SyncMonitorLists(oSettings)
        ResumeScan()
    End Sub

    Private Sub OpenCustomVariables()
        Dim frm As New frmVariableManager
        PauseScan()
        frm.ShowDialog()
        mgrPath.LoadCustomVariables()
        mgrMonitorList.SyncMonitorLists(oSettings)
        ResumeScan()
    End Sub

    Private Sub OpenStartupWizard()
        Dim frm As New frmStartUpWizard()
        frm.Settings = New mgrSettings
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

    Private Sub CheckForNewBackups()
        If oSettings.RestoreOnLaunch Or oSettings.AutoRestore Or oSettings.AutoMark Then
            StartRestoreCheck()
        End If
    End Sub

    'Functions handling the loading/sync of settings    
    Private Sub LoadGameSettings()
        'Load Monitor List
        hshScanList = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.ScanList)

        UpdateLog(mgrCommon.FormatString(frmMain_GameListLoaded, hshScanList.Keys.Count), False)
    End Sub

    Private Sub StartSyncWatcher()
        oFileWatcher.EnableRaisingEvents = True
    End Sub

    Private Sub StopSyncWatcher()
        oFileWatcher.EnableRaisingEvents = False
    End Sub

    Private Sub SetupSyncWatcher()
        oFileWatcher.Path = oSettings.BackupFolder
        oFileWatcher.Filter = "gbm.s3db"
        oFileWatcher.NotifyFilter = NotifyFilters.LastWrite

    End Sub

    Private Sub QueueSyncWatcher() Handles oFileWatcher.Changed
        tmFileWatcherQueue.Stop()
        tmFileWatcherQueue.AutoReset = False
        tmFileWatcherQueue.Interval = 30000
        tmFileWatcherQueue.Start()
    End Sub

    Private Sub HandleSyncWatcher() Handles tmFileWatcherQueue.Elapsed
        tmFileWatcherQueue.Stop()
        StopSyncWatcher()

        UpdateLog(frmMain_MasterListChanged, False, ToolTipIcon.Info, True)
        SyncGameSettings()
        LoadGameSettings()

        CheckForNewBackups()
        StartSyncWatcher()
    End Sub

    Private Sub SyncGameSettings()
        'Sync Monitor List
        mgrMonitorList.SyncMonitorLists(oSettings, False)
    End Sub

    Private Sub SyncGameIDs(ByVal bOfficial As Boolean)
        Dim sLocation As String

        PauseScan()

        If mgrCommon.IsUnix Then
            sLocation = App_URLImportLinux
        Else
            sLocation = App_URLImport
        End If

        If bOfficial Then
            mgrMonitorList.SyncGameIDs(sLocation, True)
        Else
            sLocation = mgrCommon.OpenFileBrowser("XML_Import", frmGameManager_ChooseImportXML, "xml", frmGameManager_XML, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)

            If sLocation <> String.Empty Then
                mgrMonitorList.SyncGameIDs(sLocation, False)
            End If
        End If

        ResumeScan()
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
        oRemoteDatabase.BackupDB(App_BackupOnLaunchFileDescription, True)
    End Sub

    Private Sub LoadAndVerify()
        'If the default utility is missing we cannot continue
        If Not oBackup.CheckForUtilities(mgrPath.Default7zLocation) Then
            mgrCommon.ShowMessage(frmMain_Error7zip, MsgBoxStyle.Critical)
            bInitFail = True
            Exit Sub
        End If

        'Check Special Paths
        If Not mgrPath.CheckSpecialPaths Then
            bInitFail = True
            Exit Sub
        End If

        'Local Database Check
        VerifyDBVersion(mgrSQLite.Database.Local)
        LocalDatabaseCheck()

        'Load Settings
        oSettings.LoadSettings()
        oBackup.Settings = oSettings
        oRestore.Settings = oSettings

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
            If oSettings.BackupOnLaunch Then
                BackupDatabases()
            End If

            'Sync Game Settings
            SyncGameSettings()
        End If

        'Setup Sync Watcher
        SetupSyncWatcher()

        'Load Game Settings
        LoadGameSettings()

        'Verify the "Start with Windows" setting
        If oSettings.StartWithWindows Then
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
        If oSettings.Custom7zLocation <> String.Empty Then
            If Not oBackup.CheckForUtilities(oSettings.Custom7zLocation) Then
                mgrCommon.ShowMessage(frmMain_Error7zCustom, oSettings.Custom7zLocation, MsgBoxStyle.Exclamation)
            End If
        End If

    End Sub

    'Functions that handle buttons, menus and other GUI features on this form
    Private Sub ToggleVisibility(ByVal bVisible As Boolean)
        'Toggling the visibility of the window(or hiding it from the taskbar) causes some very strange issues with the form in Mono.
        If bVisible Then
            Me.WindowState = FormWindowState.Normal
        Else
            Me.WindowState = FormWindowState.Minimized
        End If

        If Not mgrCommon.IsUnix Then
            Me.ShowInTaskbar = bVisible
            Me.Visible = bVisible
        End If
    End Sub

    Private Sub ShowApp()
        ToggleVisibility(True)
        Me.Focus()
    End Sub

    Private Sub ScanToggle()
        Select Case eCurrentStatus
            Case eStatus.Running
                HandleScan()
            Case eStatus.Paused
                Dim sGame As String = oProcess.GameInfo.CroppedName

                If bProcessIsAdmin Then
                    mgrCommon.ShowMessage(frmMain_ErrorAdminDetect, sGame, MsgBoxStyle.Exclamation)
                    RestartAsAdmin()
                    Exit Sub
                End If

                If oProcess.Duplicate Then
                    sGame = frmMain_UnknownGame
                End If

                If mgrCommon.ShowMessage(frmMain_ConfirmMonitorCancel, sGame, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    UpdateLog(mgrCommon.FormatString(frmMain_MonitorCancel, sGame), False)
                    SetLastAction(mgrCommon.FormatString(frmMain_MonitorCancel, sGame))

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

        If bPrompt And Not oSettings.ExitNoWarning Then
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
            ToggleMenuItems(False, gMonTraySetup)
            ToggleMenuItems(False, gMonTrayTools)
            gMonNotification.Enabled = False
            gMonTrayNotification.Enabled = False
            gMonTraySettings.Enabled = False
            gMonTrayFullBackup.Enabled = False
            gMonTrayFullRestore.Enabled = False
            If Not bGameDetected Then
                gMonTrayMon.Enabled = False
                gMonTrayExit.Enabled = False
                gMonStatusStrip.Enabled = False
            End If
            bMenuEnabled = False
        Else
            ToggleMenuItems(True, gMonFile)
            ToggleMenuItems(True, gMonSetup)
            ToggleMenuItems(True, gMonSetupAddWizard)
            ToggleMenuItems(True, gMonTools)
            ToggleMenuItems(True, gMonTraySetup)
            ToggleMenuItems(True, gMonTrayTools)
            gMonNotification.Enabled = True
            gMonTrayNotification.Enabled = True
            gMonTraySettings.Enabled = True
            gMonTrayFullBackup.Enabled = True
            gMonTrayFullRestore.Enabled = True
            gMonTrayMon.Enabled = True
            gMonTrayExit.Enabled = True
            gMonStatusStrip.Enabled = True
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
                If oSettings.AutoSaveLog Then
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

        sLocation = mgrCommon.SaveFileBrowser("Log_File", frmMain_ChooseLogFile, "txt", frmMain_Text, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), frmMain_DefaultLogFileName & " " & Date.Now.ToString("dd-MMM-yyyy"))

        If sLocation <> String.Empty Then
            mgrCommon.SaveText(txtLog.Text, sLocation)
        End If
    End Sub

    Private Sub SetForm()
        'Set Minimum Size
        Me.MinimumSize = New Size(Me.Size.Width, Me.Size.Height - txtLog.Size.Height)

        'Set Form Name
        Me.Name = App_NameLong
        Me.Icon = GBM_Icon

        'Set Menu Text
        gMonFile.Text = frmMain_gMonFile
        gMonFileMonitor.Text = frmMain_gMonFileMonitor_Start
        gMonFileFullBackup.Text = frmMain_gMonFileFullBackup
        gMonFileFullRestore.Text = frmMain_gMonFileFullRestore
        gMonFileSettings.Text = frmMain_gMonFileSettings
        gMonFileExit.Text = frmMain_gMonFileExit
        gMonSetup.Text = frmMain_gMonSetup
        gMonSetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonSetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonSetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonSetupTags.Text = frmMain_gMonSetupTags
        gMonSetupProcessManager.Text = frmMain_gMonSetupProcessManager
        gMonTools.Text = frmMain_gMonTools
        gMonToolsImportBackup.Text = frmMain_gMonToolsImportBackup
        gMonToolsImportBackupFiles.Text = frmMain_gMonToolsImportBackupFiles
        gMonToolsImportBackupFolder.Text = frmMain_gMonToolsImportBackupFolder
        gMonToolsCompact.Text = frmMain_gMonToolsCompact
        gMonToolsLog.Text = frmMain_gMonToolsLog
        gMonToolsSessions.Text = frmMain_gMonToolsSessions
        gMonToolsSyncGameID.Text = frmMain_gMonToolsSyncGameID
        gMonToolsSyncGameIDOfficial.Text = frmMain_gMonToolsSyncGameIDOfficial
        gMonToolsSyncGameIDFile.Text = frmMain_gMonToolsSyncGameIDFile
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
        gMonTrayFullBackup.Text = frmMain_gMonTrayFullBackup
        gMonTrayFullRestore.Text = frmMain_gMonTrayFullRestore
        gMonTraySettings.Text = frmMain_gMonFileSettings
        gMonTraySetup.Text = frmMain_gMonSetup
        gMonTraySetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonTraySetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonTraySetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonTraySetupTags.Text = frmMain_gMonSetupTags
        gMonTraySetupProcessManager.Text = frmMain_gMonSetupProcessManager
        gMonTrayTools.Text = frmMain_gMonTools
        gMonTrayToolsImportBackup.Text = frmMain_gMonTrayToolsImportBackup
        gMonTrayToolsImportBackupFiles.Text = frmMain_gMonTrayToolsImportBackupFiles
        gMonTrayToolsImportBackupFolder.Text = frmMain_gMonTrayToolsImportBackupFolder
        gMonTrayToolsCompact.Text = frmMain_gMonToolsCompact
        gMonTrayToolsLog.Text = frmMain_gMonToolsLog
        gMonTrayToolsSessions.Text = frmMain_gMonToolsSessions
        gMonTrayToolsSyncGameID.Text = frmMain_gMonToolsSyncGameID
        gMonTrayToolsSyncGameIDOfficial.Text = frmMain_gMonToolsSyncGameIDOfficial
        gMonTrayToolsSyncGameIDFile.Text = frmMain_gMonToolsSyncGameIDFile
        gMonTrayLogClear.Text = frmMain_gMonLogClear
        gMonTrayLogSave.Text = frmMain_gMonLogSave
        gMonTrayExit.Text = frmMain_gMonFileExit

        'Set Form Text
        lblLastActionTitle.Text = frmMain_lblLastActionTitle
        btnCancelOperation.Text = frmMain_btnCancelOperation
        gMonStripStatusButton.Text = frmMain_gMonStripStatusButton
        gMonStripStatusButton.ToolTipText = frmMain_gMonStripStatusButtonToolTip

        If mgrCommon.IsElevated Then
            gMonStripAdminButton.Image = Icon_Admin
            gMonStripAdminButton.ToolTipText = frmMain_RunningAsAdmin

        Else
            gMonStripAdminButton.Image = Icon_User
            gMonStripAdminButton.ToolTipText = frmMain_RunningAsNormal
        End If
        btnCancelOperation.Visible = False
        lblLastActionTitle.Visible = False
        lblLastAction.Text = String.Empty
        pbTime.SizeMode = PictureBoxSizeMode.AutoSize
        pbTime.Image = Icon_Clock
        AddHandler mgrMonitorList.UpdateLog, AddressOf UpdateLog
        ResetGameInfo()
    End Sub

    Private Function BuildChildProcesses() As Integer
        Dim oCurrentProcess As clsProcess
        Dim oProcessList As Hashtable
        Dim prsChild As Process

        oChildProcesses.Clear()

        oProcessList = mgrGameProcesses.GetProcessesByGame(oProcess.GameInfo.ID)

        If oProcessList.Count > 0 Then
            For Each oCurrentProcess In oProcessList.Values
                prsChild = New Process
                prsChild.StartInfo.Arguments = oCurrentProcess.Args
                prsChild.StartInfo.FileName = oCurrentProcess.Path
                prsChild.StartInfo.UseShellExecute = False
                prsChild.StartInfo.RedirectStandardOutput = True
                prsChild.StartInfo.CreateNoWindow = True
                oChildProcesses.Add(oCurrentProcess, prsChild)
            Next
        End If

        Return oChildProcesses.Count
    End Function

    Private Sub StartChildProcesses()
        Dim oCurrentProcess As clsProcess
        Dim prsChild As Process

        Try
            For Each de As DictionaryEntry In oChildProcesses
                oCurrentProcess = DirectCast(de.Key, clsProcess)
                prsChild = DirectCast(de.Value, Process)
                prsChild.Start()
                UpdateLog(mgrCommon.FormatString(frmMain_ProcessStarted, oCurrentProcess.Name), False)
            Next
        Catch ex As Exception
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorStartChildProcess, oProcess.GameInfo.CroppedName), True, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, ex.Message), False,, False)
        End Try
    End Sub

    Private Sub EndChildProcesses()
        Dim oCurrentProcess As clsProcess
        Dim prsChild As Process

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
            UpdateLog(mgrCommon.FormatString(frmMain_ErrorEndChildProcess, oProcess.GameInfo.CroppedName), True, ToolTipIcon.Error)
            UpdateLog(mgrCommon.FormatString(App_GenericError, ex.Message), False,, False)
        End Try
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
            UpdateStatus(frmMain_NotScanning)
            gMonStripStatusButton.Image = Icon_Stopped
            gMonTray.Icon = GBM_Tray_Stopped
        Else
            StartScan()
            StartSyncWatcher()
            eCurrentStatus = eStatus.Running
            UpdateStatus(frmMain_NoGameDetected)
            gMonStripStatusButton.Image = Icon_Ready
            gMonTray.Icon = GBM_Tray_Ready
        End If
        ToggleMenuText()
    End Sub

    Private Sub PauseScan(Optional ByVal bGameDetected As Boolean = False)
        If eCurrentStatus = eStatus.Running Then
            StopSyncWatcher()
            tmScanTimer.Stop()
            eCurrentStatus = eStatus.Paused
            UpdateStatus(frmMain_NotScanning)
            gMonStripStatusButton.Image = Icon_Detected
            gMonTray.Icon = GBM_Tray_Detected
        End If
        ToggleMenuText()
        ToggleMenuEnable(bGameDetected)
    End Sub

    Private Sub ResumeScan()
        If eCurrentStatus = eStatus.Running Or eCurrentStatus = eStatus.Paused Then
            StartScan()
            StartSyncWatcher()
            eCurrentStatus = eStatus.Running
            gMonStripStatusButton.Image = Icon_Ready
            gMonTray.Icon = GBM_Tray_Ready
            UpdateStatus(frmMain_NoGameDetected)
        End If
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    Private Sub StopScan()
        StopSyncWatcher()
        tmScanTimer.Stop()
        eCurrentStatus = eStatus.Stopped
        UpdateStatus(frmMain_NotScanning)
        gMonStripStatusButton.Image = Icon_Stopped
        gMonTray.Icon = GBM_Tray_Stopped
        ToggleMenuText()
        ToggleMenuEnable()
    End Sub

    'Functions to handle verification
    Private Sub VerifyCustomPathVariables()
        Dim sGames As String = String.Empty
        If Not mgrPath.VerifyCustomVariables(hshScanList, sGames) Then
            mgrCommon.ShowMessage(frmMain_ErrorCustomVariable, sGames, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Function VerifyBackupLocation() As Boolean
        Dim sBackupPath As String = oSettings.BackupFolder
        If mgrPath.VerifyBackupPath(sBackupPath) Then
            If oSettings.BackupFolder <> sBackupPath Then
                oSettings.BackupFolder = sBackupPath
                oSettings.SaveSettings()
                oSettings.LoadSettings()
                mgrMonitorList.HandleBackupLocationChange(oSettings)
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
                oSettings.StartWithWindows = False
                oSettings.SaveSettings()
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
                    mgrCommon.RestartAsAdmin()
                    bShutdown = True
                    ShutdownApp(False)
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

        sFilestoImport = mgrCommon.OpenMultiFileBrowser("Main_BackupFileImport", frmMain_ChooseImportFiles, "7z", frmGameManager_7zBackup, sDefaultFolder, True)

        If sFilestoImport.Length > 0 Then
            RunImportBackupByFile(sFilestoImport)
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
        End If
    End Sub

    'Event Handlers
    Private Sub gMonFileMonitor_Click(sender As Object, e As EventArgs) Handles gMonFileMonitor.Click, gMonTrayMon.Click
        ScanToggle()
    End Sub

    Private Sub gMonFileFullBackup_Click(sender As Object, e As EventArgs) Handles gMonFileFullBackup.Click, gMonTrayFullBackup.Click
        RunBackupAll()
    End Sub

    Private Sub gMonFileFullRestore_Click(sender As Object, e As EventArgs) Handles gMonFileFullRestore.Click, gMonTrayFullRestore.Click
        RunRestoreAll()
    End Sub

    Private Sub gMonTray_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles gMonTray.MouseDoubleClick
        ShowApp()
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

    Private Sub FileSettings_Click(sender As Object, e As EventArgs) Handles gMonFileSettings.Click, gMonTraySettings.Click
        OpenSettings()
    End Sub

    Private Sub SetupGameManager_Click(sender As Object, e As EventArgs) Handles gMonSetupGameManager.Click, gMonTraySetupGameManager.Click
        OpenGameManager()
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

    Private Sub gMonToolsSyncGameIDOfficial_Click(sender As Object, e As EventArgs) Handles gMonToolsSyncGameIDOfficial.Click, gMonTrayToolsSyncGameIDOfficial.Click
        SyncGameIDs(True)
    End Sub

    Private Sub gMonToolsSyncGameIDFile_Click(sender As Object, e As EventArgs) Handles gMonToolsSyncGameIDFile.Click, gMonTrayToolsSyncGameIDFile.Click
        SyncGameIDs(False)
    End Sub

    Private Sub gMonNotification_Click(sender As Object, e As EventArgs) Handles gMonNotification.Click, gMonTrayNotification.Click
        gMonNotification.Visible = False
        gMonTrayNotification.Visible = False
        OpenGameManager(True)
    End Sub

    Private Sub gMonStripSplitStatusButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripStatusButton.Click
        ScanToggle()
    End Sub

    Private Sub pbIcon_Click(sender As Object, e As EventArgs) Handles pbIcon.Click
        If bAllowIcon Then
            SetIcon()
        End If
    End Sub

    Private Sub btnCancelOperation_Click(sender As Object, e As EventArgs) Handles btnCancelOperation.Click
        OperationCancel()
    End Sub

    Private Sub gMonStripAdminButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripAdminButton.Click
        RestartAsAdmin()
    End Sub

    Private Sub Main_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Select Case e.CloseReason
            Case CloseReason.UserClosing
                If bShutdown = False Then
                    e.Cancel = True
                    If oSettings.ExitOnClose Then
                        ShutdownApp()
                    Else
                        ToggleVisibility(False)
                    End If
                End If
            Case Else
                ShutdownApp(False)
        End Select
    End Sub

    Private Sub AutoRestoreEventProcessor(myObject As Object, ByVal myEventArgs As EventArgs) Handles tmRestoreCheck.Elapsed
        If eCurrentStatus <> eStatus.Paused Then
            AutoRestoreCheck()
        End If
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
                            bPathDetectionFailure = True
                            sPathDetectionError = mgrCommon.FormatString(frmMain_ErrorAdminBackup, oProcess.GameInfo.Name)
                        End If
                        bContinue = True
                    End If
                ElseIf iErrorCode = 299 Then
                    If oProcess.Duplicate Then
                        sErrorMessage = mgrCommon.FormatString(frmMain_ErrorMulti64)
                        UpdateLog(sErrorMessage, True, ToolTipIcon.Warning, True)
                    Else
                        If Not CheckForSavedPath() Then
                            bPathDetectionFailure = True
                            sPathDetectionError = mgrCommon.FormatString(frmMain_Error64Backup, oProcess.GameInfo.Name)
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
                    UpdateLog(frmMain_MultipleGamesDetected, oSettings.ShowDetectionToolTips)
                    UpdateStatus(frmMain_MultipleGamesDetected)
                    SetGameInfo(True)
                Else
                    oLastGame = oProcess.GameInfo
                    UpdateLog(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.Name), oSettings.ShowDetectionToolTips)
                    UpdateStatus(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.CroppedName))
                    SetGameInfo()
                End If

                If BuildChildProcesses() > 0 And Not oProcess.Duplicate Then
                    StartChildProcesses()
                End If

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
        Dim bContinue As Boolean = True

        If oChildProcesses.Count > 0 And Not oProcess.Duplicate Then
            EndChildProcesses()
        End If

        oProcess.EndTime = Now

        If Not bCancelledByUser Then
            'Check if we failed to detect the game path
            If bPathDetectionFailure Then
                oProcess.GameInfo.ProcessPath = mgrPath.ProcessPathSearch(oProcess.GameInfo.Name, oProcess.GameInfo.ProcessName, sPathDetectionError)
                If oProcess.GameInfo.ProcessPath = String.Empty Then
                    bContinue = False
                    If oSettings.TimeTracking Then HandleTimeSpent()
                    If oSettings.SessionTracking Then HandleSession()
                    UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oProcess.GameInfo.Name), False)
                    oProcess.GameInfo = Nothing
                    ResetGameInfo()
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
                    If Not oProcess.GameInfo.AbsolutePath Then HandleProcessPath()
                    If oSettings.TimeTracking Then HandleTimeSpent()
                    If oSettings.SessionTracking Then HandleSession()
                    RunBackup()
                Else
                    oLastGame = Nothing
                    UpdateLog(frmMain_UnknownGameEnded, False)
                    oProcess.GameInfo = Nothing
                    ResetGameInfo()
                    ResumeScan()
                End If
            End If
        End If

        'Reset globals
        bPathDetectionFailure = False
        sPathDetectionError = String.Empty
        bCancelledByUser = False
        oProcess.StartTime = Now : oProcess.EndTime = Now
    End Sub

    'All initialize code is run in the form Activated event, since running it in the Load event can cause issues in Mono.
    'Using a combination of the Load and Activated events to initialize can also cause weird issues since they fire concurrently in .NET when the app starts.
    Private Sub frmMain_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        If bInitialLoad Then
            SetForm()
            Try
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

                If oSettings.StartToTray Then
                    ToggleVisibility(False)
                End If

                If oSettings.MonitorOnStartup Then
                    eCurrentStatus = eStatus.Stopped
                Else
                    eCurrentStatus = eStatus.Running
                End If

                HandleScan()
                CheckForNewBackups()
            End If

            bInitialLoad = False
        Else
            txtLog.Select(txtLog.TextLength, 0)
            txtLog.ScrollToCaret()
        End If
    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Oemtilde AndAlso e.Modifiers = Keys.Control Then
            OpenDevConsole()
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