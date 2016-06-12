<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.chkMonitorOnStartup = New System.Windows.Forms.CheckBox()
        Me.chkBackupConfirm = New System.Windows.Forms.CheckBox()
        Me.grpStartup = New System.Windows.Forms.GroupBox()
        Me.chkStartWindows = New System.Windows.Forms.CheckBox()
        Me.chkStartToTray = New System.Windows.Forms.CheckBox()
        Me.chkAutoSaveLog = New System.Windows.Forms.CheckBox()
        Me.btnOptionalFields = New System.Windows.Forms.Button()
        Me.chkTimeTracking = New System.Windows.Forms.CheckBox()
        Me.chkSync = New System.Windows.Forms.CheckBox()
        Me.chkShowDetectionTips = New System.Windows.Forms.CheckBox()
        Me.grpFolderOptions = New System.Windows.Forms.GroupBox()
        Me.btnBackupFolder = New System.Windows.Forms.Button()
        Me.lblBackupFolder = New System.Windows.Forms.Label()
        Me.txtBackupFolder = New System.Windows.Forms.TextBox()
        Me.chkCreateFolder = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.nudSupressBackupThreshold = New System.Windows.Forms.NumericUpDown()
        Me.chkSupressBackup = New System.Windows.Forms.CheckBox()
        Me.chkCheckSum = New System.Windows.Forms.CheckBox()
        Me.chkRestoreOnLaunch = New System.Windows.Forms.CheckBox()
        Me.chkOverwriteWarning = New System.Windows.Forms.CheckBox()
        Me.grp7zGeneral = New System.Windows.Forms.GroupBox()
        Me.cboCompression = New System.Windows.Forms.ComboBox()
        Me.lblCompression = New System.Windows.Forms.Label()
        Me.btn7zLocation = New System.Windows.Forms.Button()
        Me.txt7zLocation = New System.Windows.Forms.TextBox()
        Me.lblLocation = New System.Windows.Forms.Label()
        Me.txt7zArguments = New System.Windows.Forms.TextBox()
        Me.lblArguments = New System.Windows.Forms.Label()
        Me.lbl7zCopyright = New System.Windows.Forms.Label()
        Me.lbl7zProduct = New System.Windows.Forms.Label()
        Me.btnDefaults = New System.Windows.Forms.Button()
        Me.pnlBackup = New System.Windows.Forms.Panel()
        Me.pnl7z = New System.Windows.Forms.Panel()
        Me.grp7zAdvanced = New System.Windows.Forms.GroupBox()
        Me.grp7zInformation = New System.Windows.Forms.GroupBox()
        Me.pnlGeneral = New System.Windows.Forms.Panel()
        Me.grpGameData = New System.Windows.Forms.GroupBox()
        Me.lstSettings = New System.Windows.Forms.ListBox()
        Me.grpStartup.SuspendLayout()
        Me.grpFolderOptions.SuspendLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp7zGeneral.SuspendLayout()
        Me.pnlBackup.SuspendLayout()
        Me.pnl7z.SuspendLayout()
        Me.grp7zAdvanced.SuspendLayout()
        Me.grp7zInformation.SuspendLayout()
        Me.pnlGeneral.SuspendLayout()
        Me.grpGameData.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkMonitorOnStartup
        '
        Me.chkMonitorOnStartup.AutoSize = True
        Me.chkMonitorOnStartup.Location = New System.Drawing.Point(6, 65)
        Me.chkMonitorOnStartup.Name = "chkMonitorOnStartup"
        Me.chkMonitorOnStartup.Size = New System.Drawing.Size(146, 17)
        Me.chkMonitorOnStartup.TabIndex = 2
        Me.chkMonitorOnStartup.Text = "Start monitoring at launch"
        Me.chkMonitorOnStartup.UseVisualStyleBackColor = True
        '
        'chkBackupConfirm
        '
        Me.chkBackupConfirm.AutoSize = True
        Me.chkBackupConfirm.Location = New System.Drawing.Point(6, 88)
        Me.chkBackupConfirm.Name = "chkBackupConfirm"
        Me.chkBackupConfirm.Size = New System.Drawing.Size(160, 17)
        Me.chkBackupConfirm.TabIndex = 1
        Me.chkBackupConfirm.Text = "Disable backup confirmation"
        Me.chkBackupConfirm.UseVisualStyleBackColor = True
        '
        'grpStartup
        '
        Me.grpStartup.Controls.Add(Me.chkStartWindows)
        Me.grpStartup.Controls.Add(Me.chkStartToTray)
        Me.grpStartup.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpStartup.Location = New System.Drawing.Point(6, 12)
        Me.grpStartup.Name = "grpStartup"
        Me.grpStartup.Size = New System.Drawing.Size(354, 90)
        Me.grpStartup.TabIndex = 0
        Me.grpStartup.TabStop = False
        Me.grpStartup.Text = "Startup"
        '
        'chkStartWindows
        '
        Me.chkStartWindows.AutoSize = True
        Me.chkStartWindows.Location = New System.Drawing.Point(6, 19)
        Me.chkStartWindows.Name = "chkStartWindows"
        Me.chkStartWindows.Size = New System.Drawing.Size(117, 17)
        Me.chkStartWindows.TabIndex = 0
        Me.chkStartWindows.Text = "Start with Windows"
        Me.chkStartWindows.UseVisualStyleBackColor = True
        '
        'chkStartToTray
        '
        Me.chkStartToTray.AutoSize = True
        Me.chkStartToTray.Location = New System.Drawing.Point(6, 42)
        Me.chkStartToTray.Name = "chkStartToTray"
        Me.chkStartToTray.Size = New System.Drawing.Size(115, 17)
        Me.chkStartToTray.TabIndex = 1
        Me.chkStartToTray.Text = "Start to system tray"
        Me.chkStartToTray.UseVisualStyleBackColor = True
        '
        'chkAutoSaveLog
        '
        Me.chkAutoSaveLog.AutoSize = True
        Me.chkAutoSaveLog.Location = New System.Drawing.Point(6, 204)
        Me.chkAutoSaveLog.Name = "chkAutoSaveLog"
        Me.chkAutoSaveLog.Size = New System.Drawing.Size(231, 17)
        Me.chkAutoSaveLog.TabIndex = 7
        Me.chkAutoSaveLog.Text = "Autosave log when max length is exceeded"
        Me.chkAutoSaveLog.UseVisualStyleBackColor = True
        '
        'btnOptionalFields
        '
        Me.btnOptionalFields.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOptionalFields.Location = New System.Drawing.Point(110, 38)
        Me.btnOptionalFields.Name = "btnOptionalFields"
        Me.btnOptionalFields.Size = New System.Drawing.Size(134, 23)
        Me.btnOptionalFields.TabIndex = 6
        Me.btnOptionalFields.Text = "Choose &Optional Fields..."
        Me.btnOptionalFields.UseVisualStyleBackColor = True
        '
        'chkTimeTracking
        '
        Me.chkTimeTracking.AutoSize = True
        Me.chkTimeTracking.Location = New System.Drawing.Point(6, 19)
        Me.chkTimeTracking.Name = "chkTimeTracking"
        Me.chkTimeTracking.Size = New System.Drawing.Size(122, 17)
        Me.chkTimeTracking.TabIndex = 4
        Me.chkTimeTracking.Text = "Enable time tracking"
        Me.chkTimeTracking.UseVisualStyleBackColor = True
        '
        'chkSync
        '
        Me.chkSync.AutoSize = True
        Me.chkSync.Location = New System.Drawing.Point(6, 42)
        Me.chkSync.Name = "chkSync"
        Me.chkSync.Size = New System.Drawing.Size(98, 17)
        Me.chkSync.TabIndex = 5
        Me.chkSync.Text = "Enable syncing"
        Me.chkSync.UseVisualStyleBackColor = True
        '
        'chkShowDetectionTips
        '
        Me.chkShowDetectionTips.AutoSize = True
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(6, 181)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(159, 17)
        Me.chkShowDetectionTips.TabIndex = 3
        Me.chkShowDetectionTips.Text = "Show detection notifications"
        Me.chkShowDetectionTips.UseVisualStyleBackColor = True
        '
        'grpFolderOptions
        '
        Me.grpFolderOptions.Controls.Add(Me.btnBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.lblBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.txtBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.chkCreateFolder)
        Me.grpFolderOptions.Location = New System.Drawing.Point(6, 12)
        Me.grpFolderOptions.Name = "grpFolderOptions"
        Me.grpFolderOptions.Size = New System.Drawing.Size(354, 70)
        Me.grpFolderOptions.TabIndex = 1
        Me.grpFolderOptions.TabStop = False
        Me.grpFolderOptions.Text = "Folders"
        '
        'btnBackupFolder
        '
        Me.btnBackupFolder.Location = New System.Drawing.Point(313, 17)
        Me.btnBackupFolder.Name = "btnBackupFolder"
        Me.btnBackupFolder.Size = New System.Drawing.Size(27, 20)
        Me.btnBackupFolder.TabIndex = 2
        Me.btnBackupFolder.Text = "..."
        Me.btnBackupFolder.UseVisualStyleBackColor = True
        '
        'lblBackupFolder
        '
        Me.lblBackupFolder.AutoSize = True
        Me.lblBackupFolder.Location = New System.Drawing.Point(6, 20)
        Me.lblBackupFolder.Name = "lblBackupFolder"
        Me.lblBackupFolder.Size = New System.Drawing.Size(79, 13)
        Me.lblBackupFolder.TabIndex = 0
        Me.lblBackupFolder.Text = "Backup Folder:"
        '
        'txtBackupFolder
        '
        Me.txtBackupFolder.Location = New System.Drawing.Point(91, 17)
        Me.txtBackupFolder.Name = "txtBackupFolder"
        Me.txtBackupFolder.Size = New System.Drawing.Size(216, 20)
        Me.txtBackupFolder.TabIndex = 1
        '
        'chkCreateFolder
        '
        Me.chkCreateFolder.AutoSize = True
        Me.chkCreateFolder.Location = New System.Drawing.Point(9, 43)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(186, 17)
        Me.chkCreateFolder.TabIndex = 3
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(384, 321)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(465, 321)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.Location = New System.Drawing.Point(286, 181)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.Size = New System.Drawing.Size(43, 13)
        Me.lblMinutes.TabIndex = 7
        Me.lblMinutes.Text = "minutes"
        '
        'nudSupressBackupThreshold
        '
        Me.nudSupressBackupThreshold.Location = New System.Drawing.Point(229, 179)
        Me.nudSupressBackupThreshold.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSupressBackupThreshold.Name = "nudSupressBackupThreshold"
        Me.nudSupressBackupThreshold.Size = New System.Drawing.Size(51, 20)
        Me.nudSupressBackupThreshold.TabIndex = 6
        '
        'chkSupressBackup
        '
        Me.chkSupressBackup.AutoSize = True
        Me.chkSupressBackup.Location = New System.Drawing.Point(6, 180)
        Me.chkSupressBackup.Name = "chkSupressBackup"
        Me.chkSupressBackup.Size = New System.Drawing.Size(217, 17)
        Me.chkSupressBackup.TabIndex = 5
        Me.chkSupressBackup.Text = "Backup only when session time exceeds"
        Me.chkSupressBackup.UseVisualStyleBackColor = True
        '
        'chkCheckSum
        '
        Me.chkCheckSum.AutoSize = True
        Me.chkCheckSum.Location = New System.Drawing.Point(6, 134)
        Me.chkCheckSum.Name = "chkCheckSum"
        Me.chkCheckSum.Size = New System.Drawing.Size(195, 17)
        Me.chkCheckSum.TabIndex = 3
        Me.chkCheckSum.Text = "Verify backup files with a checksum"
        Me.chkCheckSum.UseVisualStyleBackColor = True
        '
        'chkRestoreOnLaunch
        '
        Me.chkRestoreOnLaunch.AutoSize = True
        Me.chkRestoreOnLaunch.Location = New System.Drawing.Point(6, 157)
        Me.chkRestoreOnLaunch.Name = "chkRestoreOnLaunch"
        Me.chkRestoreOnLaunch.Size = New System.Drawing.Size(257, 17)
        Me.chkRestoreOnLaunch.TabIndex = 4
        Me.chkRestoreOnLaunch.Text = "Notify when there are new backup files to restore"
        Me.chkRestoreOnLaunch.UseVisualStyleBackColor = True
        '
        'chkOverwriteWarning
        '
        Me.chkOverwriteWarning.AutoSize = True
        Me.chkOverwriteWarning.Location = New System.Drawing.Point(6, 111)
        Me.chkOverwriteWarning.Name = "chkOverwriteWarning"
        Me.chkOverwriteWarning.Size = New System.Drawing.Size(139, 17)
        Me.chkOverwriteWarning.TabIndex = 2
        Me.chkOverwriteWarning.Text = "Show overwrite warning"
        Me.chkOverwriteWarning.UseVisualStyleBackColor = True
        '
        'grp7zGeneral
        '
        Me.grp7zGeneral.Controls.Add(Me.cboCompression)
        Me.grp7zGeneral.Controls.Add(Me.lblCompression)
        Me.grp7zGeneral.Location = New System.Drawing.Point(6, 12)
        Me.grp7zGeneral.Name = "grp7zGeneral"
        Me.grp7zGeneral.Size = New System.Drawing.Size(354, 50)
        Me.grp7zGeneral.TabIndex = 0
        Me.grp7zGeneral.TabStop = False
        Me.grp7zGeneral.Text = "General"
        '
        'cboCompression
        '
        Me.cboCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCompression.FormattingEnabled = True
        Me.cboCompression.Location = New System.Drawing.Point(110, 17)
        Me.cboCompression.Name = "cboCompression"
        Me.cboCompression.Size = New System.Drawing.Size(238, 21)
        Me.cboCompression.TabIndex = 1
        '
        'lblCompression
        '
        Me.lblCompression.AutoSize = True
        Me.lblCompression.Location = New System.Drawing.Point(6, 20)
        Me.lblCompression.Name = "lblCompression"
        Me.lblCompression.Size = New System.Drawing.Size(70, 13)
        Me.lblCompression.TabIndex = 0
        Me.lblCompression.Text = "Compression:"
        '
        'btn7zLocation
        '
        Me.btn7zLocation.Location = New System.Drawing.Point(313, 41)
        Me.btn7zLocation.Name = "btn7zLocation"
        Me.btn7zLocation.Size = New System.Drawing.Size(27, 20)
        Me.btn7zLocation.TabIndex = 5
        Me.btn7zLocation.Text = "..."
        Me.btn7zLocation.UseVisualStyleBackColor = True
        '
        'txt7zLocation
        '
        Me.txt7zLocation.Location = New System.Drawing.Point(110, 41)
        Me.txt7zLocation.Name = "txt7zLocation"
        Me.txt7zLocation.Size = New System.Drawing.Size(197, 20)
        Me.txt7zLocation.TabIndex = 4
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Location = New System.Drawing.Point(6, 44)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblLocation.TabIndex = 4
        Me.lblLocation.Text = "Custom Location:"
        '
        'txt7zArguments
        '
        Me.txt7zArguments.Location = New System.Drawing.Point(110, 15)
        Me.txt7zArguments.Name = "txt7zArguments"
        Me.txt7zArguments.Size = New System.Drawing.Size(238, 20)
        Me.txt7zArguments.TabIndex = 3
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(6, 20)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(98, 13)
        Me.lblArguments.TabIndex = 2
        Me.lblArguments.Text = "Custom Arguments:"
        '
        'lbl7zCopyright
        '
        Me.lbl7zCopyright.AutoEllipsis = True
        Me.lbl7zCopyright.Location = New System.Drawing.Point(9, 34)
        Me.lbl7zCopyright.Name = "lbl7zCopyright"
        Me.lbl7zCopyright.Size = New System.Drawing.Size(339, 17)
        Me.lbl7zCopyright.TabIndex = 8
        Me.lbl7zCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl7zProduct
        '
        Me.lbl7zProduct.AutoEllipsis = True
        Me.lbl7zProduct.Location = New System.Drawing.Point(9, 17)
        Me.lbl7zProduct.Name = "lbl7zProduct"
        Me.lbl7zProduct.Size = New System.Drawing.Size(339, 17)
        Me.lbl7zProduct.TabIndex = 7
        Me.lbl7zProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDefaults
        '
        Me.btnDefaults.Location = New System.Drawing.Point(12, 321)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(110, 23)
        Me.btnDefaults.TabIndex = 4
        Me.btnDefaults.Text = "Set &Defaults"
        Me.btnDefaults.UseVisualStyleBackColor = True
        '
        'pnlBackup
        '
        Me.pnlBackup.Controls.Add(Me.lblMinutes)
        Me.pnlBackup.Controls.Add(Me.nudSupressBackupThreshold)
        Me.pnlBackup.Controls.Add(Me.grpFolderOptions)
        Me.pnlBackup.Controls.Add(Me.chkSupressBackup)
        Me.pnlBackup.Controls.Add(Me.chkBackupConfirm)
        Me.pnlBackup.Controls.Add(Me.chkCheckSum)
        Me.pnlBackup.Controls.Add(Me.chkOverwriteWarning)
        Me.pnlBackup.Controls.Add(Me.chkRestoreOnLaunch)
        Me.pnlBackup.Location = New System.Drawing.Point(180, 0)
        Me.pnlBackup.Name = "pnlBackup"
        Me.pnlBackup.Size = New System.Drawing.Size(367, 314)
        Me.pnlBackup.TabIndex = 3
        '
        'pnl7z
        '
        Me.pnl7z.Controls.Add(Me.grp7zAdvanced)
        Me.pnl7z.Controls.Add(Me.grp7zInformation)
        Me.pnl7z.Controls.Add(Me.grp7zGeneral)
        Me.pnl7z.Location = New System.Drawing.Point(180, 0)
        Me.pnl7z.Name = "pnl7z"
        Me.pnl7z.Size = New System.Drawing.Size(367, 314)
        Me.pnl7z.TabIndex = 2
        '
        'grp7zAdvanced
        '
        Me.grp7zAdvanced.Controls.Add(Me.btn7zLocation)
        Me.grp7zAdvanced.Controls.Add(Me.lblArguments)
        Me.grp7zAdvanced.Controls.Add(Me.txt7zLocation)
        Me.grp7zAdvanced.Controls.Add(Me.txt7zArguments)
        Me.grp7zAdvanced.Controls.Add(Me.lblLocation)
        Me.grp7zAdvanced.Location = New System.Drawing.Point(6, 68)
        Me.grp7zAdvanced.Name = "grp7zAdvanced"
        Me.grp7zAdvanced.Size = New System.Drawing.Size(354, 73)
        Me.grp7zAdvanced.TabIndex = 2
        Me.grp7zAdvanced.TabStop = False
        Me.grp7zAdvanced.Text = "Advanced"
        '
        'grp7zInformation
        '
        Me.grp7zInformation.Controls.Add(Me.lbl7zProduct)
        Me.grp7zInformation.Controls.Add(Me.lbl7zCopyright)
        Me.grp7zInformation.Location = New System.Drawing.Point(6, 146)
        Me.grp7zInformation.Name = "grp7zInformation"
        Me.grp7zInformation.Size = New System.Drawing.Size(354, 63)
        Me.grp7zInformation.TabIndex = 1
        Me.grp7zInformation.TabStop = False
        Me.grp7zInformation.Text = "Utility Information"
        '
        'pnlGeneral
        '
        Me.pnlGeneral.Controls.Add(Me.chkAutoSaveLog)
        Me.pnlGeneral.Controls.Add(Me.grpGameData)
        Me.pnlGeneral.Controls.Add(Me.chkShowDetectionTips)
        Me.pnlGeneral.Controls.Add(Me.grpStartup)
        Me.pnlGeneral.Location = New System.Drawing.Point(180, 0)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(367, 314)
        Me.pnlGeneral.TabIndex = 1
        '
        'grpGameData
        '
        Me.grpGameData.Controls.Add(Me.chkTimeTracking)
        Me.grpGameData.Controls.Add(Me.chkSync)
        Me.grpGameData.Controls.Add(Me.btnOptionalFields)
        Me.grpGameData.Location = New System.Drawing.Point(6, 106)
        Me.grpGameData.Name = "grpGameData"
        Me.grpGameData.Size = New System.Drawing.Size(354, 69)
        Me.grpGameData.TabIndex = 1
        Me.grpGameData.TabStop = False
        Me.grpGameData.Text = "Game Data"
        '
        'lstSettings
        '
        Me.lstSettings.FormattingEnabled = True
        Me.lstSettings.Location = New System.Drawing.Point(12, 12)
        Me.lstSettings.Name = "lstSettings"
        Me.lstSettings.Size = New System.Drawing.Size(162, 303)
        Me.lstSettings.TabIndex = 7
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 361)
        Me.Controls.Add(Me.lstSettings)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.pnlBackup)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnl7z)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.grpStartup.ResumeLayout(False)
        Me.grpStartup.PerformLayout()
        Me.grpFolderOptions.ResumeLayout(False)
        Me.grpFolderOptions.PerformLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp7zGeneral.ResumeLayout(False)
        Me.grp7zGeneral.PerformLayout()
        Me.pnlBackup.ResumeLayout(False)
        Me.pnlBackup.PerformLayout()
        Me.pnl7z.ResumeLayout(False)
        Me.grp7zAdvanced.ResumeLayout(False)
        Me.grp7zAdvanced.PerformLayout()
        Me.grp7zInformation.ResumeLayout(False)
        Me.pnlGeneral.ResumeLayout(False)
        Me.pnlGeneral.PerformLayout()
        Me.grpGameData.ResumeLayout(False)
        Me.grpGameData.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkMonitorOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents chkBackupConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents grpStartup As System.Windows.Forms.GroupBox
    Friend WithEvents grpFolderOptions As System.Windows.Forms.GroupBox
    Friend WithEvents txtBackupFolder As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBackupFolder As System.Windows.Forms.Label
    Friend WithEvents btnBackupFolder As System.Windows.Forms.Button
    Friend WithEvents chkShowDetectionTips As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartToTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwriteWarning As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateFolder As System.Windows.Forms.CheckBox
    Friend WithEvents chkRestoreOnLaunch As System.Windows.Forms.CheckBox
    Friend WithEvents chkSync As System.Windows.Forms.CheckBox
    Friend WithEvents chkCheckSum As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartWindows As System.Windows.Forms.CheckBox
    Friend WithEvents chkTimeTracking As System.Windows.Forms.CheckBox
    Friend WithEvents lblMinutes As Label
    Friend WithEvents nudSupressBackupThreshold As NumericUpDown
    Friend WithEvents chkSupressBackup As CheckBox
    Friend WithEvents grp7zGeneral As GroupBox
    Friend WithEvents cboCompression As ComboBox
    Friend WithEvents lblCompression As Label
    Friend WithEvents lbl7zProduct As Label
    Friend WithEvents lbl7zCopyright As Label
    Friend WithEvents btn7zLocation As Button
    Friend WithEvents txt7zLocation As TextBox
    Friend WithEvents lblLocation As Label
    Friend WithEvents txt7zArguments As TextBox
    Friend WithEvents lblArguments As Label
    Friend WithEvents btnDefaults As Button
    Friend WithEvents btnOptionalFields As Button
    Friend WithEvents chkAutoSaveLog As CheckBox
    Friend WithEvents pnlBackup As Panel
    Friend WithEvents pnl7z As Panel
    Friend WithEvents pnlGeneral As Panel
    Friend WithEvents grpGameData As GroupBox
    Friend WithEvents lstSettings As ListBox
    Friend WithEvents grp7zAdvanced As GroupBox
    Friend WithEvents grp7zInformation As GroupBox
End Class
