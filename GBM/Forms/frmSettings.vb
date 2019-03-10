Imports GBM.My.Resources
Imports System.IO

Public Class frmSettings
    Dim bShutdown As Boolean = False
    Dim bSyncSettingsChanged As Boolean = False
    Dim eCurrentSyncFields As clsGame.eOptionalSyncFields
    Private oSettings As mgrSettings

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Private Sub HandleLinuxAutoStart(ByVal bToggle As Boolean)
        Dim oProcess As Process
        Dim sAutoStartFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & Path.DirectorySeparatorChar & ".config/autostart/"

        If bToggle Then
            'Create the autostart folder if it doesn't exist yet
            If Not Directory.Exists(sAutoStartFolder) Then
                Directory.CreateDirectory(sAutoStartFolder)
            End If
            'Create link
            Try
                oProcess = New Process
                oProcess.StartInfo.FileName = "/bin/ln"
                oProcess.StartInfo.Arguments = "-s /usr/share/applications/gbm.desktop " & sAutoStartFolder
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
        If chkAutoStart.Checked <> oSettings.StartWithWindows Then
            If mgrCommon.IsUnix Then
                HandleLinuxAutoStart(chkAutoStart.Checked)
            Else
                HandleRegistryUpdate(chkAutoStart.Checked)
            End If
        End If
        oSettings.StartWithWindows = chkAutoStart.Checked

        oSettings.MonitorOnStartup = chkMonitorOnStartup.Checked
        oSettings.StartToTray = chkStartMinimized.Checked
        oSettings.BackupOnLaunch = chkBackupOnLaunch.Checked
        oSettings.ShowDetectionToolTips = chkShowDetectionTips.Checked
        oSettings.DisableSyncMessages = chkDisableSyncMessages.Checked
        oSettings.AutoSaveLog = chkAutoSaveLog.Checked
        oSettings.DisableConfirmation = chkBackupConfirm.Checked
        oSettings.DisableDiskSpaceCheck = chkDisableDiskSpaceCheck.Checked
        oSettings.CreateSubFolder = chkCreateFolder.Checked
        oSettings.UseGameID = chkUseGameID.Checked
        oSettings.ShowOverwriteWarning = chkOverwriteWarning.Checked
        oSettings.RestoreOnLaunch = chkRestoreNotify.Checked
        oSettings.AutoRestore = chkAutoRestore.Checked
        oSettings.AutoMark = chkAutoMark.Checked
        oSettings.TimeTracking = chkTimeTracking.Checked
        oSettings.SessionTracking = chkSessionTracking.Checked
        oSettings.ShowResolvedPaths = chkShowResolvedPaths.Checked
        oSettings.SuppressBackup = chkSuppressBackup.Checked
        oSettings.SuppressBackupThreshold = nudSuppressBackupThreshold.Value
        oSettings.CompressionLevel = cboCompression.SelectedValue

        If oSettings.Custom7zArguments <> txt7zArguments.Text.Trim And txt7zArguments.Text.Trim <> String.Empty Then
            mgrCommon.ShowMessage(frmSettings_WarningArguments, MsgBoxStyle.Exclamation)
        End If

        oSettings.Custom7zArguments = txt7zArguments.Text.Trim
        oSettings.Custom7zLocation = txt7zLocation.Text.Trim

        If Directory.Exists(txtBackupFolder.Text) Then
            If oSettings.BackupFolder <> txtBackupFolder.Text Then
                bSyncSettingsChanged = True
            End If
            oSettings.BackupFolder = txtBackupFolder.Text
        Else
            mgrCommon.ShowMessage(frmSettings_ErrorBackupFolder, MsgBoxStyle.Exclamation)
            Return False
        End If

        If oSettings.Custom7zLocation <> String.Empty Then
            If File.Exists(oSettings.Custom7zLocation) Then
                If Path.GetFileNameWithoutExtension(oSettings.Custom7zLocation) <> "7za" Then
                    mgrCommon.ShowMessage(frmSettings_WarningLocation, MsgBoxStyle.Critical)
                End If
            Else
                mgrCommon.ShowMessage(frmSettings_ErrorLocation, oSettings.Custom7zLocation, MsgBoxStyle.Critical)
                Return False
            End If
        End If

        'We must trigger a sync if optional fields have changed
        If eCurrentSyncFields <> Settings.SyncFields Then
            bSyncSettingsChanged = True
        End If

        Return True
    End Function

    Private Function SaveSettings() As Boolean
        If ValidateSettings() Then
            oSettings.SaveSettings()
            If bSyncSettingsChanged Then mgrMonitorList.HandleBackupLocationChange(Settings)
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
            oSettings = New mgrSettings
            LoadSettings()
        End If
    End Sub

    Private Sub ResetMessages()
        If mgrCommon.ShowMessage(frmSettings_ConfirmMessageReset, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            oSettings.SuppressMessages = mgrSettings.eSuppressMessages.None
        End If
    End Sub

    Private Sub LoadSettings()
        chkAutoStart.Checked = oSettings.StartWithWindows
        chkMonitorOnStartup.Checked = oSettings.MonitorOnStartup
        chkStartMinimized.Checked = oSettings.StartToTray
        chkBackupOnLaunch.Checked = oSettings.BackupOnLaunch
        chkShowDetectionTips.Checked = oSettings.ShowDetectionToolTips
        chkDisableSyncMessages.Checked = oSettings.DisableSyncMessages
        chkAutoSaveLog.Checked = oSettings.AutoSaveLog
        chkBackupConfirm.Checked = oSettings.DisableConfirmation
        chkDisableDiskSpaceCheck.Checked = oSettings.DisableDiskSpaceCheck
        chkCreateFolder.Checked = oSettings.CreateSubFolder
        chkUseGameID.Checked = oSettings.UseGameID
        chkOverwriteWarning.Checked = oSettings.ShowOverwriteWarning
        chkRestoreNotify.Checked = oSettings.RestoreOnLaunch
        chkAutoRestore.Checked = oSettings.AutoRestore
        chkAutoMark.Checked = oSettings.AutoMark
        txtBackupFolder.Text = oSettings.BackupFolder
        chkTimeTracking.Checked = oSettings.TimeTracking
        chkSessionTracking.Checked = oSettings.SessionTracking
        chkShowResolvedPaths.Checked = oSettings.ShowResolvedPaths
        chkSuppressBackup.Checked = oSettings.SuppressBackup
        nudSuppressBackupThreshold.Value = oSettings.SuppressBackupThreshold
        nudSuppressBackupThreshold.Enabled = chkSuppressBackup.Checked
        cboCompression.SelectedValue = oSettings.CompressionLevel
        txt7zArguments.Text = oSettings.Custom7zArguments
        txt7zLocation.Text = oSettings.Custom7zLocation
        eCurrentSyncFields = oSettings.SyncFields

        'Retrieve 7z Info
        GetUtilityInfo(oSettings.Custom7zLocation)

    End Sub

    Private Sub LoadCombos()
        Dim oComboItems As New List(Of KeyValuePair(Of Integer, String))
        Dim oSettingsItems As New List(Of KeyValuePair(Of Integer, String))

        'cboCompression
        cboCompression.ValueMember = "Key"
        cboCompression.DisplayMember = "Value"

        oComboItems.Add(New KeyValuePair(Of Integer, String)(0, frmSettings_cboCompression_None))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(1, frmSettings_cboCompression_Fastest))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(3, frmSettings_cboCompression_Fast))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(5, frmSettings_cboCompression_Normal))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(7, frmSettings_cboCompression_Maximum))
        oComboItems.Add(New KeyValuePair(Of Integer, String)(9, frmSettings_cboCompression_Ultra))

        cboCompression.DataSource = oComboItems

        'lstSettings
        lstSettings.ValueMember = "Key"
        lstSettings.DisplayMember = "Value"

        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(0, frmSettings_lstSettings_General))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(1, frmSettings_lstSettings_BackupRestore))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(2, frmSettings_lstSettings_Startup))
        oSettingsItems.Add(New KeyValuePair(Of Integer, String)(3, frmSettings_lstSettings_7z))

        lstSettings.DataSource = oSettingsItems

        'Select Default
        lstSettings.SelectedIndex = 0
    End Sub

    Private Sub OpenOptionalFields()
        Dim frm As New frmSyncFields
        frm.SyncFields = Settings.SyncFields
        frm.ShowDialog()
        If frm.DialogResult = DialogResult.OK Then
            Settings.SyncFields = frm.SyncFields
        End If
    End Sub

    Private Sub ChangePanel()
        If lstSettings.SelectedItems.Count > 0 Then
            Dim oSettingsItem As KeyValuePair(Of Integer, String) = lstSettings.SelectedItems(0)

            Select Case oSettingsItem.Key
                Case 0
                    pnlGeneral.Visible = True
                    pnlStartup.Visible = False
                    pnlBackup.Visible = False
                    pnl7z.Visible = False
                Case 1
                    pnlGeneral.Visible = False
                    pnlBackup.Visible = True
                    pnlStartup.Visible = False
                    pnl7z.Visible = False
                Case 2
                    pnlGeneral.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = True
                    pnl7z.Visible = False

                Case 3
                    pnlGeneral.Visible = False
                    pnlBackup.Visible = False
                    pnlStartup.Visible = False
                    pnl7z.Visible = True
            End Select
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmSettings_FormName

        'Set Form Text
        lblMinutes.Text = frmSettings_lblMinutes
        chkSuppressBackup.Text = frmSettings_chkSuppressBackup
        grpBackupHandling.Text = frmSettings_grpBackupHandling
        chkRestoreNotify.Text = frmSettings_chkRestoreNotify
        chkAutoRestore.Text = frmSettings_chkAutoRestore
        chkAutoMark.Text = frmSettings_chkAutoMark
        chkOverwriteWarning.Text = frmSettings_chkOverwriteWarning
        chkCreateFolder.Text = frmSettings_chkCreateFolder
        chkUseGameID.Text = frmSettings_chkUseGameID
        chkBackupConfirm.Text = frmSettings_chkBackupConfirm
        btnCancel.Text = frmSettings_btnCancel
        btnSave.Text = frmSettings_btnSave
        grpFolderOptions.Text = frmSettings_grpFolderOptions
        btnBackupFolder.Text = frmSettings_btnBackupFolder
        lblBackupFolder.Text = frmSettings_lblBackupFolder
        grpStartup.Text = frmSettings_grpStartup
        grpGameData.Text = frmSettings_grpGameData
        chkTimeTracking.Text = frmSettings_chkTimeTracking
        chkSessionTracking.Text = frmSettings_chkSessionTracking
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
        lblArguments.Text = frmSettings_lblArguments
        lblLocation.Text = frmSettings_lblLocation
        btnOptionalFields.Text = frmSettings_btnOptionalFields
        btnResetMessages.Text = frmSettings_btnResetMessages
        chkBackupOnLaunch.Text = frmSettings_chkBackupOnLaunch
        grpBackupConfirmations.Text = frmSettings_grpBackupConfirmations
        grpLogOptions.Text = frmSettings_grpLogOptions
        chkDisableSyncMessages.Text = frmSettings_chkDisableSyncMessages
        grpGameMonitoringOptions.Text = frmSettings_grpGameMonitoringOptions
        chkShowResolvedPaths.Text = frmSettings_chkShowResolvedPaths
        chkDisableDiskSpaceCheck.Text = frmSettings_chkDisableDiskSpaceCheck

        If mgrCommon.IsUnix Then
            'Only enable this option on Linux if GBM was installed with an official method
            If Not File.Exists("/usr/share/applications/gbm.desktop") Then
                chkAutoStart.Enabled = False
            End If
            chkStartMinimized.Enabled = False
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
        SetForm()
        LoadCombos()
        LoadSettings()
    End Sub

    Private Sub btnBackupFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnBackupFolder.Click
        Dim sNewFolder As String
        sNewFolder = mgrCommon.OpenClassicFolderBrowser("Settings_Backup_Path", frmSettings_BrowseFolder, oSettings.BackupFolder, True, False)
        If sNewFolder <> String.Empty Then txtBackupFolder.Text = sNewFolder
    End Sub

    Private Sub btn7zLocation_Click(sender As Object, e As EventArgs) Handles btn7zLocation.Click
        Dim sNewLocation As String
        sNewLocation = mgrCommon.OpenFileBrowser("7z_Browse", frmSettings_Browse7za, "exe", frmSettings_7zaFileType, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)
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
End Class