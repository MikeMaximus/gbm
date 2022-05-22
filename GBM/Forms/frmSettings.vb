Imports GBM.My.Resources
Imports System.IO

Public Class frmSettings
    Dim bIsLoading As Boolean = False
    Dim bShutdown As Boolean = False
    Dim bSyncSettingsChanged As Boolean = False
    Dim eCurrentSyncFields As clsGame.eOptionalSyncFields

    Private Sub HandleLinuxAutoStart(ByVal bToggle As Boolean)
        Dim oProcess As Process
        Dim sDesktopFile = String.Empty
        Dim sAutoStartFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & Path.DirectorySeparatorChar & ".config/autostart/"

        If bToggle And mgrPath.VerifyLinuxDesktopFileLocation(sDesktopFile) Then
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
        Else
            'Delete link
            If File.Exists(sAutoStartFolder & Path.DirectorySeparatorChar & "gbm.desktop") Then
                File.Delete(sAutoStartFolder & Path.DirectorySeparatorChar & "gbm.desktop")
            End If
        End If
    End Sub

    Private Sub HandleRegistryUpdate(ByVal bToggle As Boolean)
        Dim oKey As Microsoft.Win32.RegistryKey
        Dim sAppName As String = Application.ProductName
        Dim sAppPath As String = Application.ExecutablePath

        If bToggle Then
            oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            oKey.SetValue(sAppName, """" & sAppPath & """")
            oKey.Close()
        Else
            oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            oKey.DeleteValue(sAppName, False)
            oKey.Close()
        End If
    End Sub

    Private Function ValidateSettings() As Boolean

        'Show Start with Windows warning if running as admin
        If Not mgrCommon.IsUnix And chkAutoStart.Checked And mgrCommon.IsElevated Then
            mgrCommon.ShowMessage(frmSettings_WarningAdminStart, MsgBoxStyle.Exclamation)
        End If

        'Only modify when the value changed
        If chkAutoStart.Checked <> mgrSettings.StartWithWindows Then
            If mgrCommon.IsUnix Then
                HandleLinuxAutoStart(chkAutoStart.Checked)
            Else
                HandleRegistryUpdate(chkAutoStart.Checked)
            End If
        End If
        mgrSettings.StartWithWindows = chkAutoStart.Checked

        mgrSettings.MonitorOnStartup = chkMonitorOnStartup.Checked
        mgrSettings.StartToTray = chkStartMinimized.Checked
        mgrSettings.BackupOnLaunch = chkBackupOnLaunch.Checked
        mgrSettings.ExitOnClose = chkExitOnClose.Checked
        mgrSettings.ExitNoWarning = chkExitNoWarning.Checked
        mgrSettings.MainHideLog = chkHideLog.Checked
        mgrSettings.MainHideGameList = chkHideGameList.Checked
        mgrSettings.MainHideButtons = chkHideButtons.Checked
        mgrSettings.ShowDetectionToolTips = chkShowDetectionTips.Checked
        mgrSettings.DetectionSpeed = cboDetectSpeed.SelectedValue
        mgrSettings.TwoPassDetection = chkTwoPassDetection.Checked
        mgrSettings.DisableSyncMessages = chkDisableSyncMessages.Checked
        mgrSettings.AutoSaveLog = chkAutoSaveLog.Checked
        mgrSettings.DisableConfirmation = chkBackupConfirm.Checked
        mgrSettings.DisableDiskSpaceCheck = chkDisableDiskSpaceCheck.Checked
        mgrSettings.CreateSubFolder = chkCreateFolder.Checked
        mgrSettings.UseGameID = chkUseGameID.Checked
        mgrSettings.StorePathAutoConfig = chkStorePathAutoConfig.Checked
        mgrSettings.ShowOverwriteWarning = chkOverwriteWarning.Checked
        mgrSettings.BackupNotification = chkBackupNotification.Checked
        mgrSettings.RestoreOnLaunch = chkRestoreNotify.Checked
        mgrSettings.AutoRestore = chkAutoRestore.Checked
        mgrSettings.AutoMark = chkAutoMark.Checked
        mgrSettings.TimeTracking = chkTimeTracking.Checked
        mgrSettings.SessionTracking = chkSessionTracking.Checked
        mgrSettings.EnableLauncher = chkEnableLauncher.Checked
        mgrSettings.ShowResolvedPaths = chkShowResolvedPaths.Checked
        mgrSettings.SuppressBackup = chkSuppressBackup.Checked
        mgrSettings.SuppressBackupThreshold = nudSuppressBackupThreshold.Value
        mgrSettings.CompressionLevel = cboCompression.SelectedValue

        If mgrSettings.Custom7zArguments <> txt7zArguments.Text.Trim And txt7zArguments.Text.Trim <> String.Empty Then
            mgrCommon.ShowMessage(frmSettings_WarningArguments, MsgBoxStyle.Exclamation)
        End If

        mgrSettings.Custom7zArguments = txt7zArguments.Text.Trim
        mgrSettings.Custom7zLocation = txt7zLocation.Text.Trim

        If Directory.Exists(txtBackupFolder.Text) Then
            If mgrSettings.BackupFolder <> txtBackupFolder.Text Then
                bSyncSettingsChanged = True
            End If
            mgrSettings.BackupFolder = txtBackupFolder.Text
        Else
            mgrCommon.ShowMessage(frmSettings_ErrorBackupFolder, MsgBoxStyle.Exclamation)
            Return False
        End If

        If Directory.Exists(txtTempFolder.Text) Then
            mgrSettings.TemporaryFolder = txtTempFolder.Text
        Else
            mgrCommon.ShowMessage(frmSettings_ErrorTempFolder, MsgBoxStyle.Exclamation)
            Return False
        End If

        If mgrSettings.Custom7zLocation <> String.Empty Then
            If File.Exists(mgrSettings.Custom7zLocation) Then
                If Path.GetFileNameWithoutExtension(mgrSettings.Custom7zLocation) <> "7za" Then
                    mgrCommon.ShowMessage(frmSettings_WarningLocation, MsgBoxStyle.Critical)
                End If
            Else
                mgrCommon.ShowMessage(frmSettings_ErrorLocation, mgrSettings.Custom7zLocation, MsgBoxStyle.Critical)
                Return False
            End If
        End If

        'We must trigger a sync if optional fields have changed
        If eCurrentSyncFields <> mgrSettings.SyncFields Then
            bSyncSettingsChanged = True
        End If

        Return True
    End Function

    Private Function SaveSettings() As Boolean
        If ValidateSettings() Then
            mgrSettings.SaveSettings()
            If bSyncSettingsChanged Then mgrMonitorList.HandleBackupLocationChange()
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub GetUtilityInfo(ByVal sLocation As String)
        Dim bDefault As Boolean = False
        Dim sFileDescription As String
        Dim sVersion As String
        Dim sCopyright As String

        'Ignore this function when on Unix and hide the information data
        If mgrCommon.IsUnix Then
            grp7zInformation.Visible = False
        Else
            Try
                grp7zInformation.Visible = True

                'Use default when no custom location is set
                If sLocation = String.Empty Then
                    sLocation = mgrPath.Default7zLocation
                    bDefault = True
                End If

                'Get info
                Dim oFileInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(sLocation)

                If oFileInfo.FileDescription = String.Empty Then
                    sFileDescription = App_NotAvailable
                Else
                    sFileDescription = oFileInfo.FileDescription
                End If

                If oFileInfo.ProductVersion = String.Empty Then
                    sVersion = App_NotAvailable
                Else
                    sVersion = oFileInfo.ProductVersion
                End If

                If oFileInfo.LegalCopyright = String.Empty Then
                    sCopyright = App_NotAvailable
                Else
                    sCopyright = oFileInfo.LegalCopyright
                End If

                lbl7zProduct.Text = sFileDescription & " - " & sVersion
                lbl7zCopyright.Text = sCopyright
            Catch ex As Exception
                grp7zInformation.Visible = False
            End Try
        End If
    End Sub

    Private Sub SetDefaults()
        If mgrCommon.ShowMessage(frmSettings_ConfirmDefaults, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            mgrSettings.SetDefaults()
            LoadSettings()
        End If
    End Sub

    Private Sub ResetMessages()
        If mgrCommon.ShowMessage(frmSettings_ConfirmMessageReset, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            mgrSettings.SuppressMessages = mgrSettings.eSuppressMessages.None
        End If
    End Sub

    Private Sub LoadSettings()
        chkAutoStart.Checked = mgrSettings.StartWithWindows
        chkMonitorOnStartup.Checked = mgrSettings.MonitorOnStartup
        chkStartMinimized.Checked = mgrSettings.StartToTray
        chkBackupOnLaunch.Checked = mgrSettings.BackupOnLaunch
        chkExitOnClose.Checked = mgrSettings.ExitOnClose
        chkExitNoWarning.Checked = mgrSettings.ExitNoWarning
        chkHideLog.Checked = mgrSettings.MainHideLog
        chkHideGameList.Checked = mgrSettings.MainHideGameList
        chkHideButtons.Checked = mgrSettings.MainHideButtons
        chkShowDetectionTips.Checked = mgrSettings.ShowDetectionToolTips
        cboDetectSpeed.SelectedValue = mgrSettings.DetectionSpeed
        chkTwoPassDetection.Checked = mgrSettings.TwoPassDetection
        chkDisableSyncMessages.Checked = mgrSettings.DisableSyncMessages
        chkAutoSaveLog.Checked = mgrSettings.AutoSaveLog
        chkBackupConfirm.Checked = mgrSettings.DisableConfirmation
        chkDisableDiskSpaceCheck.Checked = mgrSettings.DisableDiskSpaceCheck
        chkCreateFolder.Checked = mgrSettings.CreateSubFolder
        chkUseGameID.Checked = mgrSettings.UseGameID
        chkStorePathAutoConfig.Checked = mgrSettings.StorePathAutoConfig
        chkOverwriteWarning.Checked = mgrSettings.ShowOverwriteWarning
        chkBackupNotification.Checked = mgrSettings.BackupNotification
        chkRestoreNotify.Checked = mgrSettings.RestoreOnLaunch
        chkAutoRestore.Checked = mgrSettings.AutoRestore
        chkAutoMark.Checked = mgrSettings.AutoMark
        txtBackupFolder.Text = mgrSettings.BackupFolder
        txtTempFolder.Text = mgrSettings.TemporaryFolder
        chkTimeTracking.Checked = mgrSettings.TimeTracking
        chkSessionTracking.Checked = mgrSettings.SessionTracking
        chkEnableLauncher.Checked = mgrSettings.EnableLauncher
        chkShowResolvedPaths.Checked = mgrSettings.ShowResolvedPaths
        chkSuppressBackup.Checked = mgrSettings.SuppressBackup
        nudSuppressBackupThreshold.Value = mgrSettings.SuppressBackupThreshold
        nudSuppressBackupThreshold.Enabled = chkSuppressBackup.Checked
        cboCompression.SelectedValue = mgrSettings.CompressionLevel
        txt7zArguments.Text = mgrSettings.Custom7zArguments
        txt7zLocation.Text = mgrSettings.Custom7zLocation
        eCurrentSyncFields = mgrSettings.SyncFields

        'Retrieve 7z Info
        GetUtilityInfo(mgrSettings.Custom7zLocation)
    End Sub

    Private Sub LoadCombos()
        Dim oCompressionItems As New List(Of KeyValuePair(Of Integer, String))
        Dim oDetectSpeedItems As New List(Of KeyValuePair(Of Integer, String))
        Dim oSettingsItems As New List(Of KeyValuePair(Of Integer, String))

        'cboCompression
        cboCompression.ValueMember = "Key"
        cboCompression.DisplayMember = "Value"

        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(0, frmSettings_cboCompression_None))
        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(1, frmSettings_cboCompression_Fastest))
        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(3, frmSettings_cboCompression_Fast))
        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(5, frmSettings_cboCompression_Normal))
        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(7, frmSettings_cboCompression_Maximum))
        oCompressionItems.Add(New KeyValuePair(Of Integer, String)(9, frmSettings_cboCompression_Ultra))

        cboCompression.DataSource = oCompressionItems

        'cboDetectSpeed
        cboDetectSpeed.ValueMember = "Key"
        cboDetectSpeed.DisplayMember = "Value"

        oDetectSpeedItems.Add(New KeyValuePair(Of Integer, String)(5000, frmSettings_cboDetectSpeed_Fast))
        oDetectSpeedItems.Add(New KeyValuePair(Of Integer, String)(10000, frmSettings_cboDetectSpeed_Moderate))
        oDetectSpeedItems.Add(New KeyValuePair(Of Integer, String)(15000, frmSettings_cboDetectSpeed_Slow))
        oDetectSpeedItems.Add(New KeyValuePair(Of Integer, String)(30000, frmSettings_cboDetectSpeed_VerySlow))

        cboDetectSpeed.DataSource = oDetectSpeedItems

        'lstSettings
        lstSettings.ValueMember = "Key"
        lstSettings.DisplayMember = "Value"

        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(0, frmSettings_lstSettings_General))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(1, frmSettings_lstSettings_FilesAndFolders))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(2, frmSettings_lstSettings_BackupRestore))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(3, frmSettings_lstSettings_Startup))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(4, frmSettings_lstSettings_UserInterface))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(5, frmSettings_lstSettings_7z))

        lstSettings.DataSource = oSettingsItems

        'Select Default
        lstSettings.SelectedIndex = 0
    End Sub

    Private Sub OpenOptionalFields()
        Dim frm As New frmSyncFields
        frm.SyncFields = mgrSettings.SyncFields
        frm.ShowDialog()
        If frm.DialogResult = DialogResult.OK Then
            mgrSettings.SyncFields = frm.SyncFields
        End If
    End Sub

    Private Sub ChangePanel()
        If lstSettings.SelectedItems.Count > 0 Then
            Dim mgrSettingsItem As KeyValuePair(Of Integer, String) = lstSettings.SelectedItems(0)

            Select Case mgrSettingsItem.Key
                Case 0
                    pnlGeneral.Visible = True
                    pnlFilesAndFolders.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = False
                    pnlInterface.Visible = False
                    pnl7z.Visible = False
                Case 1
                    pnlGeneral.Visible = False
                    pnlFilesAndFolders.Visible = True
                    pnlBackup.Visible = False
                    pnlStartup.Visible = False
                    pnlInterface.Visible = False
                    pnl7z.Visible = False
                Case 2
                    pnlGeneral.Visible = False
                    pnlFilesAndFolders.Visible = False
                    pnlBackup.Visible = True
                    pnlStartup.Visible = False
                    pnlInterface.Visible = False
                    pnl7z.Visible = False
                Case 3
                    pnlGeneral.Visible = False
                    pnlFilesAndFolders.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = True
                    pnlInterface.Visible = False
                    pnl7z.Visible = False
                Case 4
                    pnlGeneral.Visible = False
                    pnlFilesAndFolders.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = False
                    pnlInterface.Visible = True
                    pnl7z.Visible = False
                Case 5
                    pnlGeneral.Visible = False
                    pnlFilesAndFolders.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = False
                    pnlInterface.Visible = False
                    pnl7z.Visible = True
            End Select
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmSettings_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        lblMinutes.Text = frmSettings_lblMinutes
        chkSuppressBackup.Text = frmSettings_chkSuppressBackup
        grpBackupHandling.Text = frmSettings_grpBackupHandling
        chkRestoreNotify.Text = frmSettings_chkRestoreNotify
        chkAutoRestore.Text = frmSettings_chkAutoRestore
        chkAutoMark.Text = frmSettings_chkAutoMark
        chkOverwriteWarning.Text = frmSettings_chkOverwriteWarning
        chkBackupNotification.Text = frmSettings_chkBackupNotification
        chkCreateFolder.Text = frmSettings_chkCreateFolder
        chkUseGameID.Text = frmSettings_chkUseGameID
        chkStorePathAutoConfig.Text = frmSettings_chkStorePathAutoConfig
        chkBackupConfirm.Text = frmSettings_chkBackupConfirm
        btnCancel.Text = frmSettings_btnCancel
        btnCancel.Image = Multi_Cancel
        btnSave.Text = frmSettings_btnSave
        btnSave.Image = Multi_Save
        grpFolderOptions.Text = frmSettings_grpFolderOptions
        btnBackupFolder.Text = frmSettings_btnBackupFolder
        lblBackupFolder.Text = frmSettings_lblBackupFolder
        lblTempFolder.Text = frmSettings_lblTempFolder
        grpStartup.Text = frmSettings_grpStartup
        grpOptionalFeeatures.Text = frmSettings_grpOptionalFeatures
        chkTimeTracking.Text = frmSettings_chkTimeTracking
        chkSessionTracking.Text = frmSettings_chkSessionTracking
        chkEnableLauncher.Text = frmSettings_chkEnableLauncher
        chkAutoStart.Text = frmSettings_chkAutoStart
        chkShowDetectionTips.Text = frmSettings_chkShowDetectionTips
        chkAutoSaveLog.Text = frmSettings_chkAutoSaveLog
        chkStartMinimized.Text = frmSettings_chkStartMinimized
        chkMonitorOnStartup.Text = frmSettings_chkMonitorOnStartup
        grp7zGeneral.Text = frmSettings_grp7zGeneral
        grp7zAdvanced.Text = frmSettings_grp7zAdvanced
        grp7zInformation.Text = frmSettings_grp7zInformation
        lblCompression.Text = frmSettings_lblCompression
        btnDefaults.Text = frmSettings_btnDefaults
        btnDefaults.Image = Multi_Reset
        lblArguments.Text = frmSettings_lblArguments
        lblLocation.Text = frmSettings_lblLocation
        btnOptionalFields.Text = frmSettings_btnOptionalFields
        btnResetMessages.Text = frmSettings_btnResetMessages
        btnResetMessages.Image = Multi_Reset
        chkBackupOnLaunch.Text = frmSettings_chkBackupOnLaunch
        grpBackupConfirmations.Text = frmSettings_grpBackupConfirmations
        grpLogOptions.Text = frmSettings_grpLogOptions
        chkDisableSyncMessages.Text = frmSettings_chkDisableSyncMessages
        grpGameMonitoringOptions.Text = frmSettings_grpGameMonitoringOptions
        chkShowResolvedPaths.Text = frmSettings_chkShowResolvedPaths
        chkDisableDiskSpaceCheck.Text = frmSettings_chkDisableDiskSpaceCheck
        grpMainWindowOptions.Text = frmSettings_grpMainWindowOptions
        chkExitOnClose.Text = frmSettings_chkExitOnClose
        chkExitNoWarning.Text = frmSettings_chkExitNoWarning
        chkHideLog.Text = frmSettings_chkHideLog
        chkHideGameList.Text = frmSettings_chkHideGameList
        chkHideButtons.Text = frmSettings_chkHideButtons
        grpGameManagerOptions.Text = frmSettings_grpGameManagerOptions
        lblDetectSpeed.Text = frmSettings_lblDetectSpeed
        chkTwoPassDetection.Text = frmSettings_chkTwoPassDetection

        If mgrCommon.IsUnix Then
            'Only enable this option on Linux if GBM was installed with an official method
            If Not mgrPath.VerifyLinuxDesktopFileLocation() Then
                chkAutoStart.Enabled = False
            End If
        End If

        'Handle Panels
        pnlGeneral.Visible = False
        pnlBackup.Visible = False
        pnl7z.Visible = False
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If SaveSettings() Then
            bShutdown = True
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        bShutdown = True
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        bIsLoading = True
        SetForm()
        LoadCombos()
        LoadSettings()
        bIsLoading = False
    End Sub

    Private Sub btnBackupFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnBackupFolder.Click
        Dim sNewFolder As String
        sNewFolder = mgrCommon.OpenClassicFolderBrowser("Settings_Backup_Path", frmSettings_BrowseFolder, mgrSettings.BackupFolder, True, False)
        If sNewFolder <> String.Empty Then txtBackupFolder.Text = sNewFolder
    End Sub

    Private Sub btnTempFolder_Click(sender As Object, e As EventArgs) Handles btnTempFolder.Click
        Dim sNewFolder As String
        sNewFolder = mgrCommon.OpenClassicFolderBrowser("Settings_Temp_Path", frmSettings_BrowseTempFolder, mgrSettings.TemporaryFolder, True, False)
        If sNewFolder <> String.Empty Then txtTempFolder.Text = sNewFolder
    End Sub

    Private Sub btn7zLocation_Click(sender As Object, e As EventArgs) Handles btn7zLocation.Click
        Dim sNewLocation As String
        Dim oExtensions As New SortedList
        oExtensions.Add(frmSettings_7zaFileType, "*.exe")
        sNewLocation = mgrCommon.OpenFileBrowser("7z_Browse", frmSettings_Browse7za, oExtensions, 1, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)
        If sNewLocation <> String.Empty Then
            txt7zLocation.Text = sNewLocation
            GetUtilityInfo(txt7zLocation.Text)
        End If
    End Sub

    Private Sub chkSuppressBackup_CheckedChanged(sender As Object, e As EventArgs) Handles chkSuppressBackup.CheckedChanged
        nudSuppressBackupThreshold.Enabled = chkSuppressBackup.Checked
    End Sub

    Private Sub txt7zLocation_Leave(sender As Object, e As EventArgs) Handles txt7zLocation.Leave
        GetUtilityInfo(txt7zLocation.Text.Trim)
    End Sub

    Private Sub btnDefaults_Click(sender As Object, e As EventArgs) Handles btnDefaults.Click
        SetDefaults()
    End Sub

    Private Sub btnResetMessages_Click(sender As Object, e As EventArgs) Handles btnResetMessages.Click
        ResetMessages()
    End Sub

    Private Sub btnOptionalFields_Click(sender As Object, e As EventArgs) Handles btnOptionalFields.Click
        OpenOptionalFields()
    End Sub

    Private Sub lstSettings_SelectedValueChanged(sender As Object, e As EventArgs) Handles lstSettings.SelectedValueChanged
        ChangePanel()
    End Sub

    Private Sub chkEnableLauncher_CheckedChanged(sender As Object, e As EventArgs) Handles chkEnableLauncher.CheckedChanged
        If Not bIsLoading Then
            If chkSessionTracking.Checked = False And chkEnableLauncher.Checked = True Then
                If mgrCommon.ShowMessage(frmSettings_ConfirmEnableLauncherSessions, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    chkSessionTracking.Checked = True
                End If
            End If
        End If
    End Sub
End Class