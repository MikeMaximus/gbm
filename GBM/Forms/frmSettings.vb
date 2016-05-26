Imports GBM.My.Resources
Imports System.IO

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

        If oSettings.Custom7zArguments <> txt7zArguments.Text.Trim And txt7zArguments.Text.Trim <> String.Empty Then
            mgrCommon.ShowMessage(frmSettings_WarningArguments, MsgBoxStyle.Exclamation)
        End If

        oSettings.Custom7zArguments = txt7zArguments.Text.Trim
        oSettings.Custom7zLocation = txt7zLocation.Text.Trim

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

        If Directory.Exists(txtBackupFolder.Text) Then
            If oSettings.BackupFolder <> txtBackupFolder.Text Then
                If chkSync.Checked Then bBackupLocationChanged = True
            End If
            oSettings.BackupFolder = txtBackupFolder.Text
        Else
            mgrCommon.ShowMessage(frmSettings_ErrorBackupFolder, MsgBoxStyle.Exclamation)
            Return False
        End If

        If oSettings.Custom7zLocation <> String.Empty Then
            If File.Exists(oSettings.Custom7zLocation) Then
                If Path.GetFileNameWithoutExtension(oSettings.Custom7zLocation) <> "7za" Then
                    mgrCommon.ShowMessage(frmSettings_WarningLocation, MsgBoxStyle.Exclamation)
                End If
            Else
                mgrCommon.ShowMessage(frmSettings_ErrorLocation, oSettings.Custom7zLocation, MsgBoxStyle.Critical)
                Return False
            End If
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

    Private Sub Get7zInfo(ByVal sLocation As String)
        Dim bDefault As Boolean = False
        Try
            'Use default when no custom location is set
            If sLocation = String.Empty Then
                sLocation = mgrPath.Default7zLocation
                bDefault = True
            End If

            'Get info
            Dim oFileInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(sLocation)
            lbl7zProduct.Text = oFileInfo.FileDescription & " " & oFileInfo.ProductVersion
            lbl7zCopyright.Text = oFileInfo.LegalCopyright

            'Set Status
            If bDefault Then
                If oSettings.Is7zUtilityValid Then
                    pbUtilityStatus.Image = Utility_Valid
                    ttUtilityStatus.ToolTipTitle = frmSettings_ttUtilityStatus_Title
                    ttUtilityStatus.SetToolTip(pbUtilityStatus, frmSettings_ttUtilityStatus_Valid7z)
                Else
                    pbUtilityStatus.Image = Utility_Invalid
                    ttUtilityStatus.ToolTipTitle = frmSettings_ttUtilityStatus_Title
                    ttUtilityStatus.SetToolTip(pbUtilityStatus, frmSettings_ttUtilityStatus_Invalid7z)
                End If
            Else
                pbUtilityStatus.Image = Utility_Custom
                ttUtilityStatus.ToolTipTitle = frmSettings_ttUtilityStatus_Title
                ttUtilityStatus.SetToolTip(pbUtilityStatus, frmSettings_ttUtilityStatus_Custom7z)
            End If
        Catch ex As Exception
            lbl7zProduct.Text = String.Empty
            lbl7zCopyright.Text = String.Empty
            pbUtilityStatus.Image = Utility_Invalid
            ttUtilityStatus.ToolTipTitle = frmSettings_ttUtilityStatus_Title
            ttUtilityStatus.SetToolTip(pbUtilityStatus, frmSettings_ttUtilityStatus_Failure7z)
        End Try
    End Sub

    Private Sub SetDefaults()
        If mgrCommon.ShowMessage(frmSettings_ConfirmDefaults, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            oSettings = New mgrSettings
            LoadSettings()
        End If
    End Sub

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
        txt7zArguments.Text = oSettings.Custom7zArguments
        txt7zLocation.Text = oSettings.Custom7zLocation

        'Unix Handler
        If mgrCommon.IsUnix Then
            chkStartToTray.Checked = False
            chkStartWindows.Checked = False
        End If

        'Retrieve 7z Info
        Get7zInfo(oSettings.Custom7zLocation)
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
        btnDefaults.Text = frmSettings_btnDefaults
        lblArguments.Text = frmSettings_lblArguments
        lblLocation.Text = frmSettings_lblLocation

        'Unix Handler
        If mgrCommon.IsUnix Then
            chkStartToTray.Enabled = False
            chkStartWindows.Enabled = False
        End If
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

    Private Sub btn7zLocation_Click(sender As Object, e As EventArgs) Handles btn7zLocation.Click
        Dim sNewLocation As String
        sNewLocation = mgrCommon.OpenFileBrowser(frmSettings_Browse7za, "exe", frmSettings_7zaFileType, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)
        If sNewLocation <> String.Empty Then
            txt7zLocation.Text = sNewLocation
            Get7zInfo(txt7zLocation.Text)
        End If
    End Sub

    Private Sub chkSupressBackup_CheckedChanged(sender As Object, e As EventArgs) Handles chkSupressBackup.CheckedChanged
        nudSupressBackupThreshold.Enabled = chkSupressBackup.Checked
    End Sub

    Private Sub txt7zLocation_Leave(sender As Object, e As EventArgs) Handles txt7zLocation.Leave
        Get7zInfo(txt7zLocation.Text.Trim)
    End Sub

    Private Sub btnDefaults_Click(sender As Object, e As EventArgs) Handles btnDefaults.Click
        SetDefaults()
    End Sub
End Class