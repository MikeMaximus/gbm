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
            MsgBox("The backup folder does not exist.  Please choose a valid backup folder.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
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
        LoadSettings()
    End Sub

    Private Sub btnBackupFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnBackupFolder.Click
        fbBrowser.SelectedPath = oSettings.BackupFolder
        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtBackupFolder.Text = fbBrowser.SelectedPath
        End If
    End Sub

End Class