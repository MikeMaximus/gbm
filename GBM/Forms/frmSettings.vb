Imports GBM.My.Resources

Public Class frmSettings
    Dim bShutdown As Boolean = False
    Dim bBackupLocationChanged As Boolean = False
    Dim bCheckSumDisabled As Boolean = False
    Private oSettings As mgrSettings

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Private Property BackupLocationChanged As Boolean
        Get
            Return bBackupLocationChanged
        End Get
        Set(value As Boolean)
            bBackupLocationChanged = value
        End Set
    End Property

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

        'Only modify registry key when the value changed
        If chkStartWindows.Checked <> oSettings.StartWithWindows Then
            HandleRegistryUpdate(chkStartWindows.Checked)
        End If
        oSettings.StartWithWindows = chkStartWindows.Checked

        oSettings.MonitorOnStartup = chkMonitorOnStartup.Checked
        oSettings.StartToTray = chkStartToTray.Checked
        oSettings.ShowDetectionToolTips = chkShowDetectionTips.Checked
        oSettings.DisableConfirmation = chkBackupConfirm.Checked
        oSettings.CreateSubFolder = chkCreateFolder.Checked
        oSettings.ShowOverwriteWarning = chkOverwriteWarning.Checked
        oSettings.RestoreOnLaunch = chkRestoreOnLaunch.Checked
        oSettings.TimeTracking = chkTimeTracking.Checked
        oSettings.SupressBackup = chkSupressBackup.Checked
        oSettings.SupressBackupThreshold = nudSupressBackupThreshold.Value
        oSettings.CompressionLevel = cboCompression.SelectedValue

        'We need to clear all checksums its turned off
        If chkCheckSum.Checked = False And oSettings.CheckSum = True Then
            bCheckSumDisabled = True
        End If
        oSettings.CheckSum = chkCheckSum.Checked

        'Turning syncing from off to on is the same as changing the backup folder
        If chkSync.Checked = True And oSettings.Sync = False Then
            bBackupLocationChanged = True
        End If
        oSettings.Sync = chkSync.Checked

        If IO.Directory.Exists(txtBackupFolder.Text) Then
            If oSettings.BackupFolder <> txtBackupFolder.Text Then
                If chkSync.Checked Then bBackupLocationChanged = True
            End If
            oSettings.BackupFolder = txtBackupFolder.Text
        Else
            mgrCommon.ShowMessage(frmSettings_ErrorBackupFolder, MsgBoxStyle.Exclamation)
            Return False
        End If

        Return True
    End Function

    Private Function SaveSettings() As Boolean
        If ValidateSettings() Then
            oSettings.SaveSettings()
            If BackupLocationChanged Then mgrMonitorList.HandleBackupLocationChange()
            If bCheckSumDisabled Then mgrManifest.DoManifestHashWipe()
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadSettings()
        chkStartWindows.Checked = oSettings.StartWithWindows
        chkMonitorOnStartup.Checked = oSettings.MonitorOnStartup
        chkStartToTray.Checked = oSettings.StartToTray
        chkShowDetectionTips.Checked = oSettings.ShowDetectionToolTips
        chkBackupConfirm.Checked = oSettings.DisableConfirmation
        chkCreateFolder.Checked = oSettings.CreateSubFolder
        chkOverwriteWarning.Checked = oSettings.ShowOverwriteWarning
        chkRestoreOnLaunch.Checked = oSettings.RestoreOnLaunch
        txtBackupFolder.Text = oSettings.BackupFolder
        chkSync.Checked = oSettings.Sync
        chkCheckSum.Checked = oSettings.CheckSum
        chkTimeTracking.Checked = oSettings.TimeTracking
        chkSupressBackup.Checked = oSettings.SupressBackup
        nudSupressBackupThreshold.Value = oSettings.SupressBackupThreshold
        nudSupressBackupThreshold.Enabled = chkSupressBackup.Checked
        cboCompression.SelectedValue = oSettings.CompressionLevel
    End Sub

    Private Sub LoadCombos()
        Dim oComboItems As New List(Of KeyValuePair(Of Integer, String))

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
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmSettings_FormName

        'Set Form Text
        grpBackup.Text = frmSettings_grpBackup
        lblMinutes.Text = frmSettings_lblMinutes
        chkSupressBackup.Text = frmSettings_chkSupressBackup
        chkCheckSum.Text = frmSettings_chkCheckSum
        chkRestoreOnLaunch.Text = frmSettings_chkRestoreOnLaunch
        chkOverwriteWarning.Text = frmSettings_chkOverwriteWarning
        chkCreateFolder.Text = frmSettings_chkCreateFolder
        chkBackupConfirm.Text = frmSettings_chkBackupConfirm
        btnCancel.Text = frmSettings_btnCancel
        btnSave.Text = frmSettings_btnSave
        grpPaths.Text = frmSettings_grpPaths
        btnBackupFolder.Text = frmSettings_btnBackupFolder
        lblBackupFolder.Text = frmSettings_lblBackupFolder
        grpGeneral.Text = frmSettings_grpGeneral
        chkTimeTracking.Text = frmSettings_chkTimeTracking
        chkStartWindows.Text = frmSettings_chkStartWindows
        chkSync.Text = frmSettings_chkSync
        chkShowDetectionTips.Text = frmSettings_chkShowDetectionTips
        chkStartToTray.Text = frmSettings_chkStartToTray
        chkMonitorOnStartup.Text = frmSettings_chkMonitorOnStartup
        grp7z.Text = frmSettings_grp7z
        lblCompression.Text = frmSettings_lblCompression
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

    Private Sub frmSettings_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If bShutdown = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub frmSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetForm()
        LoadCombos()
        LoadSettings()
    End Sub

    Private Sub btnBackupFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnBackupFolder.Click
        Dim sNewFolder As String
        sNewFolder = mgrCommon.OpenFolderBrowser(frmSettings_BrowseFolder, oSettings.BackupFolder, True)
        If sNewFolder <> String.Empty Then txtBackupFolder.Text = sNewFolder
    End Sub

    Private Sub chkSupressBackup_CheckedChanged(sender As Object, e As EventArgs) Handles chkSupressBackup.CheckedChanged
        nudSupressBackupThreshold.Enabled = chkSupressBackup.Checked
    End Sub
End Class