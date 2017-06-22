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
    Private bProcessIsAdmin As Boolean = False
    Private bLogToggle As Boolean = False
    Private bShowToggle As Boolean = True
    Private bAllowIcon As Boolean = False
    Private bAllowDetails As Boolean = False
    Private oPriorImage As Image
    Private sPriorPath As String
    Private sPriorCompany As String
    Private sPriorVersion As String
    Private iRestoreTimeOut As Integer

    'Developer Debug Flags
    Private bProcessDebugMode As Boolean = False

    WithEvents oFileWatcher As New FileSystemWatcher

    'Timers - There may only be one System.Windows.Forms.Timer and it must be tmScanTimer.
    WithEvents tmScanTimer As New Timer
    WithEvents tmRestoreCheck As New System.Timers.Timer
    WithEvents tmFileWatcherQueue As New System.Timers.Timer

    Public WithEvents oProcess As New mgrProcesses
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
        oBackup.DoBackup(oBackupList)
        OperationEnded()
    End Sub

    Private Sub ExecuteRestore(ByVal oRestoreList As List(Of clsBackup))
        oRestore.DoRestore(oRestoreList)
        OperationEnded()
    End Sub

    Private Sub RunRestore(ByVal oRestoreList As Hashtable)
        Dim oGame As clsGame
        Dim oReadyList As New List(Of clsBackup)
        Dim oRestoreInfo As clsBackup
        Dim bTriggerReload As Boolean = False
        Dim bPathVerified As Boolean
        eCurrentOperation = eOperation.Restore
        OperationStarted()

        'Build Restore List
        For Each de As DictionaryEntry In oRestoreList
            bPathVerified = False
            oGame = DirectCast(de.Key, clsGame)
            oRestoreInfo = DirectCast(de.Value, clsBackup)

            If mgrRestore.CheckPath(oRestoreInfo, oGame, bTriggerReload) Then
                bPathVerified = True
            Else
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorRestorePath, oRestoreInfo.Name), False, ToolTipIcon.Error, True)
            End If

            If bPathVerified Then
                If oRestore.CheckRestorePrereq(oRestoreInfo, oGame.CleanFolder) Then
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

    Private Sub RunManualBackup(ByVal oBackupList As List(Of clsGame))
        Dim oGame As clsGame
        Dim bNoAuto As Boolean
        Dim bPathVerified As Boolean
        Dim oReadyList As New List(Of clsGame)

        eCurrentOperation = eOperation.Backup
        OperationStarted()

        'Build Backup List
        For Each oGame In oBackupList
            bNoAuto = False
            bPathVerified = False
            gMonStripStatusButton.Enabled = False

            UpdateLog(mgrCommon.FormatString(frmMain_ManualBackup, oGame.Name), False)

            If oGame.AbsolutePath = False Then
                If oGame.ProcessPath = String.Empty Then
                    If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                    oGame.ProcessPath = mgrPath.ProcessPathSearch(oGame.Name, oGame.TrueProcess, mgrCommon.FormatString(frmMain_ErrorRelativePath, oGame.Name), bNoAuto)
                End If

                If oGame.ProcessPath <> String.Empty Then
                    bPathVerified = True
                Else
                    UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oGame.Name), False, ToolTipIcon.Error, True)
                End If
            Else
                bPathVerified = True
            End If

            If bPathVerified Then
                If oBackup.CheckBackupPrereq(oGame) Then
                    oReadyList.Add(oGame)
                End If
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

        If oProcess.GameInfo.MonitorOnly = False Then
            If SupressBackup() Then
                bDoBackup = False
                UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.Name), False)
                SetLastAction(mgrCommon.FormatString(frmMain_ErrorBackupSessionLength, oProcess.GameInfo.CroppedName))
                OperationEnded()
            Else
                If oSettings.DisableConfirmation Then
                    bDoBackup = True
                Else
                    If mgrCommon.ShowMessage(frmMain_ConfirmBackup, oProcess.GameInfo.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
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
            bDoBackup = False
            UpdateLog(mgrCommon.FormatString(frmMain_MonitorEnded, oProcess.GameInfo.Name), False)
            SetLastAction(mgrCommon.FormatString(frmMain_MonitorEnded, oProcess.GameInfo.CroppedName))
            OperationEnded()
        End If

        If bDoBackup Then
            If Not oBackup.CheckBackupPrereq(oProcess.GameInfo) Then
                SetLastAction(mgrCommon.FormatString(frmMain_ErrorBackupCancel, oProcess.GameInfo.CroppedName))
                OperationEnded()
            Else
                'Run the backup
                oReadyList.Add(oProcess.GameInfo)
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
        Dim sNotReady As New List(Of String)
        Dim sNotInstalled As New List(Of String)
        Dim sNoCheckSum As New List(Of String)
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
                        sNotReady.Add(de.Key)
                        bFinished = False
                    End If
                Else
                    sNoCheckSum.Add(de.Key)
                End If

                'Check if the restore location exists,  if not we assume the game is not installed and should be auto-marked.
                hshGames = mgrMonitorList.DoListGetbyName(de.Key)
                If hshGames.Count = 1 Then
                    oGame = DirectCast(hshGames(0), clsGame)
                    mgrRestore.DoPathOverride(oBackup, oGame)
                    If oGame.ProcessPath <> String.Empty Then
                        oBackup.RelativeRestorePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oBackup.RestorePath
                    End If
                End If

                If oBackup.AbsolutePath Then
                    sExtractPath = oBackup.RestorePath
                Else
                    sExtractPath = oBackup.RelativeRestorePath
                End If

                If Not Directory.Exists(sExtractPath) Then
                    If oSettings.AutoMark Then
                        If mgrManifest.DoGlobalManifestCheck(de.Key, mgrSQLite.Database.Local) Then
                            mgrManifest.DoManifestUpdateByName(de.Value, mgrSQLite.Database.Local)
                        Else
                            mgrManifest.DoManifestAdd(de.Value, mgrSQLite.Database.Local)
                        End If
                    End If
                    sNotInstalled.Add(de.Key)
                End If
            Next

            'Remove any backup files that are not ready
            For Each s As String In sNotReady
                slRestoreData.Remove(s)
                UpdateLog(mgrCommon.FormatString(frmMain_RestoreNotReady, s), False, ToolTipIcon.Info, True)
            Next

            'Remove any backup files that should not be automatically restored
            For Each s As String In sNotInstalled
                slRestoreData.Remove(s)
                If oSettings.AutoMark Then
                    UpdateLog(mgrCommon.FormatString(frmMain_AutoMark, s), False, ToolTipIcon.Info, True)
                Else
                    UpdateLog(mgrCommon.FormatString(frmMain_NoAutoMark, s), False, ToolTipIcon.Info, True)
                End If
            Next
            For Each s As String In sNoCheckSum
                slRestoreData.Remove(s)
                UpdateLog(mgrCommon.FormatString(frmMain_NoCheckSum, s), False, ToolTipIcon.Info, True)
            Next

            'Automatically restore backup files
            If oSettings.AutoRestore Then
                If slRestoreData.Count > 0 Then
                    hshRestore = New Hashtable
                    sGame = String.Empty
                    For Each de As DictionaryEntry In slRestoreData
                        hshGames = mgrMonitorList.DoListGetbyName(de.Key)
                        If hshGames.Count = 1 Then
                            oGame = DirectCast(hshGames(0), clsGame)
                            sGame = oGame.CroppedName
                            hshRestore.Add(oGame, de.Value)
                        Else
                            UpdateLog(mgrCommon.FormatString(frmMain_AutoRestoreFailure, de.Key), False, ToolTipIcon.Info, True)
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
                pbIcon.Image = Image.FromFile(sIcon)
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
            lblGameTitle.Text = frmMain_MultipleGames
            pbTime.Visible = False
            lblTimeSpent.Visible = False
            pbIcon.Image = Icon_Unknown
            lblStatus1.Text = frmMain_NoDetails
        Else
            bAllowIcon = True
            bAllowDetails = True
            lblGameTitle.Text = oProcess.GameInfo.Name

            Try
                Dim ic As Icon = System.Drawing.Icon.ExtractAssociatedIcon(oProcess.FoundProcess.MainModule.FileName)
                pbIcon.Image = ic.ToBitmap

                'Set Game Details
                sFileName = oProcess.FoundProcess.MainModule.FileName
                sFileVersion = oProcess.FoundProcess.MainModule.FileVersionInfo.FileVersion
                sCompanyName = oProcess.FoundProcess.MainModule.FileVersionInfo.CompanyName

            Catch ex As Exception
                pbIcon.Image = Icon_Unknown
            End Try

            'Check for a custom icon & details            
            If File.Exists(oProcess.GameInfo.Icon) Then
                pbIcon.Image = Image.FromFile(oProcess.GameInfo.Icon)
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
        If hshScanList.Contains(oProcess.GameInfo.ProcessName) Then
            DirectCast(hshScanList.Item(oProcess.GameInfo.ProcessName), clsGame).Hours = oProcess.GameInfo.Hours
        End If

        mgrMonitorList.DoListUpdate(oProcess.GameInfo)
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)

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
                    End Select

                Case Else
                    mgrCommon.ShowMessage(frmMain_ErrorCommandInvalid, sMainCommand, MsgBoxStyle.Exclamation)
            End Select

        End If
    End Sub

    Private Sub OpenAbout()
        Dim iProcessType As System.Reflection.ProcessorArchitecture = System.Reflection.AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture
        Dim sVersion As String = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build
        Dim sProcessType = [Enum].GetName(GetType(System.Reflection.ProcessorArchitecture), iProcessType)
        Dim sRevision As String = My.Application.Info.Version.Revision
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSqliteVersion As String = oDatabase.ReportVersion
        Dim sConstCopyright As String = Chr(169) & mgrCommon.FormatString(App_Copyright, Now.Year.ToString)

        mgrCommon.ShowMessage(frmMain_About, New String() {sVersion, sProcessType, sRevision, sSqliteVersion, sConstCopyright}, MsgBoxStyle.Information)
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmTags
        PauseScan()
        frm.ShowDialog()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
        ResumeScan()
    End Sub

    Private Sub OpenGameManager(Optional ByVal bPendingRestores As Boolean = False)
        Dim frm As New frmGameManager
        PauseScan()
        frm.BackupFolder = oSettings.BackupFolder
        frm.PendingRestores = bPendingRestores
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
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
            oBackup.Settings = oSettings
            oRestore.Settings = oSettings
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
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
        ResumeScan()
    End Sub

    Private Sub OpenCustomVariables()
        Dim frm As New frmVariableManager
        PauseScan()
        frm.ShowDialog()
        mgrPath.CustomVariablesReload()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
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
        Process.Start(App_URLWebsite)
    End Sub

    Private Sub OpenOnlineManual()
        Process.Start(App_URLManual)
    End Sub

    Private Sub OpenCheckforUpdates()
        Process.Start(App_URLUpdates)
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
        If oSettings.Sync Then
            UpdateLog(frmMain_MasterListChanged, False, ToolTipIcon.Info, True)
            SyncGameSettings()
            LoadGameSettings()
        End If
        CheckForNewBackups()
        StartSyncWatcher()
    End Sub

    Private Sub SyncGameSettings()
        'Sync Monitor List
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields, False)
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

        'If the default utility is missing we cannot continue
        If Not oBackup.CheckForUtilities(mgrPath.Default7zLocation) Then
            mgrCommon.ShowMessage(frmMain_Error7zip, MsgBoxStyle.Critical)
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
                UpdateLog(frmMain_ErrorAppLocationChanged, False, ToolTipIcon.Info)
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
    Private Sub ToggleState()
        'Toggle State with Tray Clicks        
        If Not bShowToggle Then
            bShowToggle = True
            Me.Visible = True
            Me.ShowInTaskbar = True
            Me.Focus()
        Else
            bShowToggle = False
            Me.Visible = False
            Me.ShowInTaskbar = False
        End If
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

        If bPrompt Then
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

    Public Sub UpdateLog(sLogUpdate As String, Optional bTrayUpdate As Boolean = True, Optional objIcon As System.Windows.Forms.ToolTipIcon = ToolTipIcon.Info, Optional bTimeStamp As Boolean = True) Handles oBackup.UpdateLog, oRestore.UpdateLog
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
            gMonTray.BalloonTipText = sLogUpdate
            gMonTray.BalloonTipIcon = objIcon
            If bTrayUpdate Then gMonTray.ShowBalloonTip(10000)
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

        'Set Menu Text
        gMonFile.Text = frmMain_gMonFile
        gMonFileMonitor.Text = frmMain_gMonFileMonitor_Start
        gMonFileSettings.Text = frmMain_gMonFileSettings
        gMonFileExit.Text = frmMain_gMonFileExit
        gMonSetup.Text = frmMain_gMonSetup
        gMonSetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonSetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonSetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonSetupTags.Text = frmMain_gMonSetupTags
        gMonTools.Text = frmMain_gMonTools
        gMonToolsCleanMan.Text = frmMain_gMonToolsCleanMan
        gMonToolsCompact.Text = frmMain_gMonToolsCompact
        gMonToolsLog.Text = frmMain_gMonToolsLog
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
        gMonTraySettings.Text = frmMain_gMonFileSettings
        gMonTraySetup.Text = frmMain_gMonSetup
        gMonTraySetupGameManager.Text = frmMain_gMonSetupGameManager
        gMonTraySetupAddWizard.Text = frmMain_gMonSetupAddWizard
        gMonTraySetupCustomVariables.Text = frmMain_gMonSetupCustomVariables
        gMonTraySetupTags.Text = frmMain_gMonSetupTags
        gMonTrayTools.Text = frmMain_gMonTools
        gMonTrayToolsCleanMan.Text = frmMain_gMonToolsCleanMan
        gMonTrayToolsCompact.Text = frmMain_gMonToolsCompact
        gMonTrayToolsLog.Text = frmMain_gMonToolsLog
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

    Private Sub PauseScan()
        If eCurrentStatus = eStatus.Running Then
            StopSyncWatcher()
            tmScanTimer.Stop()
            eCurrentStatus = eStatus.Paused
            UpdateStatus(frmMain_NotScanning)
            gMonStripStatusButton.Image = Icon_Detected
            gMonTray.Icon = GBM_Tray_Detected
        End If
        ToggleMenuText()
        ToggleMenuEnable()
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
                If oSettings.Sync Then mgrMonitorList.HandleBackupLocationChange(oSettings)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub VerifyGameDataPath()
        'Important: This function cannot access mgrPath for settings, as that will trigger a database creation and destroy the reason for this function
        Dim sSettingsRoot As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "/gbm"
        Dim sDBLocation As String = sSettingsRoot & "/gbm.s3db"

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

    Private Function CheckForParametersDuplicate() As Boolean
        For Each o As clsGame In oProcess.DuplicateList
            If o.Parameter <> String.Empty And oProcess.FullCommand.Contains(o.Parameter) Then
                oProcess.GameInfo = o
                oProcess.Duplicate = False
                Return True
            End If
        Next
        Return False
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

    Private Sub CleanLocalManifest()
        Dim slItems As SortedList

        PauseScan()

        If mgrCommon.ShowMessage(frmMain_ConfirmManifestClean, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            slItems = mgrRestore.SyncLocalManifest()

            If slItems.Count > 0 Then
                For Each oItem As clsBackup In slItems.Values
                    UpdateLog(mgrCommon.FormatString(frmMain_ManifestRemovedEntry, oItem.Name), False)
                Next
                mgrCommon.ShowMessage(frmMain_ManifestTotalRemoved, slItems.Count, MsgBoxStyle.Information)
            Else
                mgrCommon.ShowMessage(frmMain_ManifestAreadyClean, MsgBoxStyle.Information)
            End If
        End If

        ResumeScan()

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

    Private Sub gMonLogClear_Click(sender As Object, e As EventArgs) Handles gMonLogClear.Click, gMonTrayLogClear.Click
        ClearLog()
    End Sub

    Private Sub gMonLogSave_Click(sender As Object, e As EventArgs) Handles gMonLogSave.Click, gMonTrayLogSave.Click
        SaveLog()
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

    Private Sub gMonTray_BalloonTipClicked(sender As System.Object, e As System.EventArgs) Handles gMonTray.BalloonTipClicked
        bShowToggle = True
        Me.Visible = True
        Me.ShowInTaskbar = True
        Me.Focus()
    End Sub

    Private Sub btnCancelOperation_Click(sender As Object, e As EventArgs) Handles btnCancelOperation.Click
        OperationCancel()
    End Sub

    Private Sub gMonStripAdminButton_ButtonClick(sender As Object, e As EventArgs) Handles gMonStripAdminButton.Click
        RestartAsAdmin()
    End Sub

    Private Sub frmMain_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        txtLog.Select(txtLog.TextLength, 0)
        txtLog.ScrollToCaret()
    End Sub

    Private Sub Main_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Unix Handler
        If mgrCommon.IsUnix And Not bShutdown Then
            ShutdownApp()
        End If

        'Intercept Exit
        If bShutdown = False Then
            e.Cancel = True
            If Not mgrCommon.IsUnix Then
                bShowToggle = False
                Me.Visible = False
                Me.ShowInTaskbar = False
            End If
        End If
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
            PauseScan()

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

            If bContinue = True Then
                If Not CheckForParametersDuplicate() Then CheckForSavedDuplicate()
                If oProcess.Duplicate Then
                        UpdateLog(frmMain_MultipleGamesDetected, oSettings.ShowDetectionToolTips)
                        UpdateStatus(frmMain_MultipleGamesDetected)
                        SetGameInfo(True)
                    Else
                        UpdateLog(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.Name), oSettings.ShowDetectionToolTips)
                        UpdateStatus(mgrCommon.FormatString(frmMain_GameDetected, oProcess.GameInfo.CroppedName))
                        SetGameInfo()
                    End If
                    oProcess.StartTime = Now
                    bwMonitor.RunWorkerAsync()
                Else
                    StopScan()
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
        Dim bContinue As Boolean = True
        oProcess.EndTime = Now

        If Not bCancelledByUser Then
            'Check if we failed to detect the game path
            If bPathDetectionFailure Then
                oProcess.GameInfo.ProcessPath = mgrPath.ProcessPathSearch(oProcess.GameInfo.Name, oProcess.GameInfo.ProcessName, sPathDetectionError)
                If oProcess.GameInfo.ProcessPath <> String.Empty Then
                    'Update and reload
                    mgrMonitorList.DoListUpdate(oProcess.GameInfo)
                    LoadGameSettings()
                Else
                    bContinue = False
                    If oSettings.TimeTracking Then HandleTimeSpent()
                    UpdateLog(mgrCommon.FormatString(frmMain_ErrorBackupUnknownPath, oProcess.GameInfo.Name), False)
                    oProcess.GameInfo = Nothing
                    ResetGameInfo()
                    ResumeScan()
                End If
            End If

            If bContinue Then
                If DoMultiGameCheck() Then
                    UpdateLog(mgrCommon.FormatString(frmMain_GameEnded, oProcess.GameInfo.Name), False)
                    If oSettings.TimeTracking Then HandleTimeSpent()
                    RunBackup()
                Else
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

    Private Sub Main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Init
        Try
            SetForm()
            VerifyGameDataPath()
            LoadAndVerify()
            If Not bInitFail Then
                VerifyCustomPathVariables()

                If oSettings.StartToTray And Not mgrCommon.IsUnix Then
                    bShowToggle = False
                    Me.Visible = False
                    Me.ShowInTaskbar = False
                End If

                If oSettings.MonitorOnStartup Then
                    eCurrentStatus = eStatus.Stopped
                Else
                    eCurrentStatus = eStatus.Running
                End If

                HandleScan()
                CheckForNewBackups()

                'Unix Handler
                If mgrCommon.IsUnix Then
                    Me.MinimizeBox = True
                Else
                    Me.gMonTray.Visible = True
                End If
            End If
        Catch ex As Exception
            If mgrCommon.ShowMessage(frmMain_ErrorInitFailure, ex.Message, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                bInitFail = True
            End If
        End Try
    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If bInitFail Then
            bShutdown = True
            Me.Close()
        End If

        If bFirstRun And Not bShutdown Then
            OpenStartupWizard()
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