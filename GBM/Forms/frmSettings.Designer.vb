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
        Me.grpGeneral = New System.Windows.Forms.GroupBox()
        Me.chkTimeTracking = New System.Windows.Forms.CheckBox()
        Me.chkSync = New System.Windows.Forms.CheckBox()
        Me.chkStartWindows = New System.Windows.Forms.CheckBox()
        Me.chkShowDetectionTips = New System.Windows.Forms.CheckBox()
        Me.chkStartToTray = New System.Windows.Forms.CheckBox()
        Me.grpPaths = New System.Windows.Forms.GroupBox()
        Me.btnBackupFolder = New System.Windows.Forms.Button()
        Me.lblBackupFolder = New System.Windows.Forms.Label()
        Me.txtBackupFolder = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpBackup = New System.Windows.Forms.GroupBox()
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.nudSupressBackupThreshold = New System.Windows.Forms.NumericUpDown()
        Me.chkSupressBackup = New System.Windows.Forms.CheckBox()
        Me.chkCheckSum = New System.Windows.Forms.CheckBox()
        Me.chkRestoreOnLaunch = New System.Windows.Forms.CheckBox()
        Me.chkOverwriteWarning = New System.Windows.Forms.CheckBox()
        Me.chkCreateFolder = New System.Windows.Forms.CheckBox()
        Me.grp7z = New System.Windows.Forms.GroupBox()
        Me.btn7zLocation = New System.Windows.Forms.Button()
        Me.txt7zLocation = New System.Windows.Forms.TextBox()
        Me.lblLocation = New System.Windows.Forms.Label()
        Me.txt7zArguments = New System.Windows.Forms.TextBox()
        Me.lblArguments = New System.Windows.Forms.Label()
        Me.cboCompression = New System.Windows.Forms.ComboBox()
        Me.lblCompression = New System.Windows.Forms.Label()
        Me.lbl7zCopyright = New System.Windows.Forms.Label()
        Me.lbl7zProduct = New System.Windows.Forms.Label()
        Me.btnDefaults = New System.Windows.Forms.Button()
        Me.grpGeneral.SuspendLayout()
        Me.grpPaths.SuspendLayout()
        Me.grpBackup.SuspendLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grp7z.SuspendLayout()
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
        Me.chkBackupConfirm.Location = New System.Drawing.Point(6, 42)
        Me.chkBackupConfirm.Name = "chkBackupConfirm"
        Me.chkBackupConfirm.Size = New System.Drawing.Size(160, 17)
        Me.chkBackupConfirm.TabIndex = 1
        Me.chkBackupConfirm.Text = "Disable backup confirmation"
        Me.chkBackupConfirm.UseVisualStyleBackColor = True
        '
        'grpGeneral
        '
        Me.grpGeneral.Controls.Add(Me.chkTimeTracking)
        Me.grpGeneral.Controls.Add(Me.chkSync)
        Me.grpGeneral.Controls.Add(Me.chkStartWindows)
        Me.grpGeneral.Controls.Add(Me.chkShowDetectionTips)
        Me.grpGeneral.Controls.Add(Me.chkStartToTray)
        Me.grpGeneral.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpGeneral.Location = New System.Drawing.Point(12, 12)
        Me.grpGeneral.Name = "grpGeneral"
        Me.grpGeneral.Size = New System.Drawing.Size(360, 165)
        Me.grpGeneral.TabIndex = 0
        Me.grpGeneral.TabStop = False
        Me.grpGeneral.Text = "General"
        '
        'chkTimeTracking
        '
        Me.chkTimeTracking.AutoSize = True
        Me.chkTimeTracking.Location = New System.Drawing.Point(6, 111)
        Me.chkTimeTracking.Name = "chkTimeTracking"
        Me.chkTimeTracking.Size = New System.Drawing.Size(122, 17)
        Me.chkTimeTracking.TabIndex = 4
        Me.chkTimeTracking.Text = "Enable time tracking"
        Me.chkTimeTracking.UseVisualStyleBackColor = True
        '
        'chkSync
        '
        Me.chkSync.AutoSize = True
        Me.chkSync.Location = New System.Drawing.Point(6, 134)
        Me.chkSync.Name = "chkSync"
        Me.chkSync.Size = New System.Drawing.Size(98, 17)
        Me.chkSync.TabIndex = 0
        Me.chkSync.Text = "Enable syncing"
        Me.chkSync.UseVisualStyleBackColor = True
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
        'chkShowDetectionTips
        '
        Me.chkShowDetectionTips.AutoSize = True
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(6, 88)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(159, 17)
        Me.chkShowDetectionTips.TabIndex = 3
        Me.chkShowDetectionTips.Text = "Show detection notifications"
        Me.chkShowDetectionTips.UseVisualStyleBackColor = True
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
        'grpPaths
        '
        Me.grpPaths.Controls.Add(Me.btnBackupFolder)
        Me.grpPaths.Controls.Add(Me.lblBackupFolder)
        Me.grpPaths.Controls.Add(Me.txtBackupFolder)
        Me.grpPaths.Location = New System.Drawing.Point(378, 183)
        Me.grpPaths.Name = "grpPaths"
        Me.grpPaths.Size = New System.Drawing.Size(360, 60)
        Me.grpPaths.TabIndex = 3
        Me.grpPaths.TabStop = False
        Me.grpPaths.Text = "Paths"
        '
        'btnBackupFolder
        '
        Me.btnBackupFolder.Location = New System.Drawing.Point(327, 24)
        Me.btnBackupFolder.Name = "btnBackupFolder"
        Me.btnBackupFolder.Size = New System.Drawing.Size(27, 20)
        Me.btnBackupFolder.TabIndex = 2
        Me.btnBackupFolder.Text = "..."
        Me.btnBackupFolder.UseVisualStyleBackColor = True
        '
        'lblBackupFolder
        '
        Me.lblBackupFolder.AutoSize = True
        Me.lblBackupFolder.Location = New System.Drawing.Point(6, 27)
        Me.lblBackupFolder.Name = "lblBackupFolder"
        Me.lblBackupFolder.Size = New System.Drawing.Size(79, 13)
        Me.lblBackupFolder.TabIndex = 0
        Me.lblBackupFolder.Text = "Backup Folder:"
        '
        'txtBackupFolder
        '
        Me.txtBackupFolder.Location = New System.Drawing.Point(91, 24)
        Me.txtBackupFolder.Name = "txtBackupFolder"
        Me.txtBackupFolder.Size = New System.Drawing.Size(230, 20)
        Me.txtBackupFolder.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(586, 305)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(667, 305)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpBackup
        '
        Me.grpBackup.Controls.Add(Me.lblMinutes)
        Me.grpBackup.Controls.Add(Me.nudSupressBackupThreshold)
        Me.grpBackup.Controls.Add(Me.chkSupressBackup)
        Me.grpBackup.Controls.Add(Me.chkCheckSum)
        Me.grpBackup.Controls.Add(Me.chkRestoreOnLaunch)
        Me.grpBackup.Controls.Add(Me.chkOverwriteWarning)
        Me.grpBackup.Controls.Add(Me.chkCreateFolder)
        Me.grpBackup.Controls.Add(Me.chkBackupConfirm)
        Me.grpBackup.Location = New System.Drawing.Point(378, 12)
        Me.grpBackup.Name = "grpBackup"
        Me.grpBackup.Size = New System.Drawing.Size(360, 165)
        Me.grpBackup.TabIndex = 2
        Me.grpBackup.TabStop = False
        Me.grpBackup.Text = "Backup and Restore"
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.Location = New System.Drawing.Point(286, 135)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.Size = New System.Drawing.Size(43, 13)
        Me.lblMinutes.TabIndex = 7
        Me.lblMinutes.Text = "minutes"
        '
        'nudSupressBackupThreshold
        '
        Me.nudSupressBackupThreshold.Location = New System.Drawing.Point(229, 133)
        Me.nudSupressBackupThreshold.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSupressBackupThreshold.Name = "nudSupressBackupThreshold"
        Me.nudSupressBackupThreshold.Size = New System.Drawing.Size(51, 20)
        Me.nudSupressBackupThreshold.TabIndex = 6
        '
        'chkSupressBackup
        '
        Me.chkSupressBackup.AutoSize = True
        Me.chkSupressBackup.Location = New System.Drawing.Point(6, 134)
        Me.chkSupressBackup.Name = "chkSupressBackup"
        Me.chkSupressBackup.Size = New System.Drawing.Size(217, 17)
        Me.chkSupressBackup.TabIndex = 5
        Me.chkSupressBackup.Text = "Backup only when session time exceeds"
        Me.chkSupressBackup.UseVisualStyleBackColor = True
        '
        'chkCheckSum
        '
        Me.chkCheckSum.AutoSize = True
        Me.chkCheckSum.Location = New System.Drawing.Point(6, 88)
        Me.chkCheckSum.Name = "chkCheckSum"
        Me.chkCheckSum.Size = New System.Drawing.Size(195, 17)
        Me.chkCheckSum.TabIndex = 3
        Me.chkCheckSum.Text = "Verify backup files with a checksum"
        Me.chkCheckSum.UseVisualStyleBackColor = True
        '
        'chkRestoreOnLaunch
        '
        Me.chkRestoreOnLaunch.AutoSize = True
        Me.chkRestoreOnLaunch.Location = New System.Drawing.Point(6, 111)
        Me.chkRestoreOnLaunch.Name = "chkRestoreOnLaunch"
        Me.chkRestoreOnLaunch.Size = New System.Drawing.Size(257, 17)
        Me.chkRestoreOnLaunch.TabIndex = 4
        Me.chkRestoreOnLaunch.Text = "Notify when there are new backup files to restore"
        Me.chkRestoreOnLaunch.UseVisualStyleBackColor = True
        '
        'chkOverwriteWarning
        '
        Me.chkOverwriteWarning.AutoSize = True
        Me.chkOverwriteWarning.Location = New System.Drawing.Point(6, 65)
        Me.chkOverwriteWarning.Name = "chkOverwriteWarning"
        Me.chkOverwriteWarning.Size = New System.Drawing.Size(139, 17)
        Me.chkOverwriteWarning.TabIndex = 2
        Me.chkOverwriteWarning.Text = "Show overwrite warning"
        Me.chkOverwriteWarning.UseVisualStyleBackColor = True
        '
        'chkCreateFolder
        '
        Me.chkCreateFolder.AutoSize = True
        Me.chkCreateFolder.Location = New System.Drawing.Point(6, 19)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(186, 17)
        Me.chkCreateFolder.TabIndex = 0
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'grp7z
        '
        Me.grp7z.Controls.Add(Me.btn7zLocation)
        Me.grp7z.Controls.Add(Me.txt7zLocation)
        Me.grp7z.Controls.Add(Me.lblLocation)
        Me.grp7z.Controls.Add(Me.txt7zArguments)
        Me.grp7z.Controls.Add(Me.lblArguments)
        Me.grp7z.Controls.Add(Me.cboCompression)
        Me.grp7z.Controls.Add(Me.lblCompression)
        Me.grp7z.Location = New System.Drawing.Point(12, 183)
        Me.grp7z.Name = "grp7z"
        Me.grp7z.Size = New System.Drawing.Size(360, 102)
        Me.grp7z.TabIndex = 3
        Me.grp7z.TabStop = False
        Me.grp7z.Text = "7z Options"
        '
        'btn7zLocation
        '
        Me.btn7zLocation.Location = New System.Drawing.Point(327, 70)
        Me.btn7zLocation.Name = "btn7zLocation"
        Me.btn7zLocation.Size = New System.Drawing.Size(27, 20)
        Me.btn7zLocation.TabIndex = 6
        Me.btn7zLocation.Text = "..."
        Me.btn7zLocation.UseVisualStyleBackColor = True
        '
        'txt7zLocation
        '
        Me.txt7zLocation.Location = New System.Drawing.Point(110, 70)
        Me.txt7zLocation.Name = "txt7zLocation"
        Me.txt7zLocation.Size = New System.Drawing.Size(211, 20)
        Me.txt7zLocation.TabIndex = 5
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Location = New System.Drawing.Point(6, 73)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(89, 13)
        Me.lblLocation.TabIndex = 4
        Me.lblLocation.Text = "Custom Location:"
        '
        'txt7zArguments
        '
        Me.txt7zArguments.Location = New System.Drawing.Point(110, 44)
        Me.txt7zArguments.Name = "txt7zArguments"
        Me.txt7zArguments.Size = New System.Drawing.Size(244, 20)
        Me.txt7zArguments.TabIndex = 3
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(6, 47)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(98, 13)
        Me.lblArguments.TabIndex = 2
        Me.lblArguments.Text = "Custom Arguments:"
        '
        'cboCompression
        '
        Me.cboCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCompression.FormattingEnabled = True
        Me.cboCompression.Location = New System.Drawing.Point(110, 17)
        Me.cboCompression.Name = "cboCompression"
        Me.cboCompression.Size = New System.Drawing.Size(244, 21)
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
        'lbl7zCopyright
        '
        Me.lbl7zCopyright.Location = New System.Drawing.Point(375, 265)
        Me.lbl7zCopyright.Name = "lbl7zCopyright"
        Me.lbl7zCopyright.Size = New System.Drawing.Size(363, 20)
        Me.lbl7zCopyright.TabIndex = 8
        Me.lbl7zCopyright.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lbl7zProduct
        '
        Me.lbl7zProduct.Location = New System.Drawing.Point(375, 246)
        Me.lbl7zProduct.Name = "lbl7zProduct"
        Me.lbl7zProduct.Size = New System.Drawing.Size(363, 20)
        Me.lbl7zProduct.TabIndex = 7
        Me.lbl7zProduct.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnDefaults
        '
        Me.btnDefaults.Location = New System.Drawing.Point(12, 305)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(110, 23)
        Me.btnDefaults.TabIndex = 4
        Me.btnDefaults.Text = "Set &Defaults"
        Me.btnDefaults.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(749, 336)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.grp7z)
        Me.Controls.Add(Me.grpBackup)
        Me.Controls.Add(Me.grpPaths)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lbl7zCopyright)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lbl7zProduct)
        Me.Controls.Add(Me.grpGeneral)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.grpGeneral.ResumeLayout(False)
        Me.grpGeneral.PerformLayout()
        Me.grpPaths.ResumeLayout(False)
        Me.grpPaths.PerformLayout()
        Me.grpBackup.ResumeLayout(False)
        Me.grpBackup.PerformLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grp7z.ResumeLayout(False)
        Me.grp7z.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkMonitorOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents chkBackupConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents grpGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents grpPaths As System.Windows.Forms.GroupBox
    Friend WithEvents txtBackupFolder As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBackupFolder As System.Windows.Forms.Label
    Friend WithEvents btnBackupFolder As System.Windows.Forms.Button
    Friend WithEvents chkShowDetectionTips As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartToTray As System.Windows.Forms.CheckBox
    Friend WithEvents grpBackup As System.Windows.Forms.GroupBox
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
    Friend WithEvents grp7z As GroupBox
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
End Class
