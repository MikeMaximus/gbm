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
        Me.chkStartWindows = New System.Windows.Forms.CheckBox()
        Me.chkSync = New System.Windows.Forms.CheckBox()
        Me.chkShowDetectionTips = New System.Windows.Forms.CheckBox()
        Me.chkStartToTray = New System.Windows.Forms.CheckBox()
        Me.grpPaths = New System.Windows.Forms.GroupBox()
        Me.btnBackupFolder = New System.Windows.Forms.Button()
        Me.lblBackupFolder = New System.Windows.Forms.Label()
        Me.txtBackupFolder = New System.Windows.Forms.TextBox()
        Me.fbBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpBackup = New System.Windows.Forms.GroupBox()
        Me.chkCheckSum = New System.Windows.Forms.CheckBox()
        Me.chkRestoreOnLaunch = New System.Windows.Forms.CheckBox()
        Me.chkOverwriteWarning = New System.Windows.Forms.CheckBox()
        Me.chkCreateFolder = New System.Windows.Forms.CheckBox()
        Me.grpGeneral.SuspendLayout()
        Me.grpPaths.SuspendLayout()
        Me.grpBackup.SuspendLayout()
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
        Me.chkBackupConfirm.Location = New System.Drawing.Point(6, 19)
        Me.chkBackupConfirm.Name = "chkBackupConfirm"
        Me.chkBackupConfirm.Size = New System.Drawing.Size(160, 17)
        Me.chkBackupConfirm.TabIndex = 0
        Me.chkBackupConfirm.Text = "Disable backup confirmation"
        Me.chkBackupConfirm.UseVisualStyleBackColor = True
        '
        'grpGeneral
        '
        Me.grpGeneral.Controls.Add(Me.chkStartWindows)
        Me.grpGeneral.Controls.Add(Me.chkSync)
        Me.grpGeneral.Controls.Add(Me.chkShowDetectionTips)
        Me.grpGeneral.Controls.Add(Me.chkStartToTray)
        Me.grpGeneral.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpGeneral.Location = New System.Drawing.Point(12, 12)
        Me.grpGeneral.Name = "grpGeneral"
        Me.grpGeneral.Size = New System.Drawing.Size(360, 145)
        Me.grpGeneral.TabIndex = 0
        Me.grpGeneral.TabStop = False
        Me.grpGeneral.Text = "General"
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
        'chkSync
        '
        Me.chkSync.AutoSize = True
        Me.chkSync.Location = New System.Drawing.Point(6, 111)
        Me.chkSync.Name = "chkSync"
        Me.chkSync.Size = New System.Drawing.Size(208, 17)
        Me.chkSync.TabIndex = 4
        Me.chkSync.Text = "Sync game list data with backup folder"
        Me.chkSync.UseVisualStyleBackColor = True
        '
        'chkShowDetectionTips
        '
        Me.chkShowDetectionTips.AutoSize = True
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(6, 88)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(136, 17)
        Me.chkShowDetectionTips.TabIndex = 3
        Me.chkShowDetectionTips.Text = "Show detection tooltips"
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
        Me.grpPaths.Location = New System.Drawing.Point(12, 311)
        Me.grpPaths.Name = "grpPaths"
        Me.grpPaths.Size = New System.Drawing.Size(360, 60)
        Me.grpPaths.TabIndex = 2
        Me.grpPaths.TabStop = False
        Me.grpPaths.Text = "Paths"
        '
        'btnBackupFolder
        '
        Me.btnBackupFolder.Location = New System.Drawing.Point(318, 23)
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
        Me.lblBackupFolder.Size = New System.Drawing.Size(76, 13)
        Me.lblBackupFolder.TabIndex = 0
        Me.lblBackupFolder.Text = "Backup Folder"
        '
        'txtBackupFolder
        '
        Me.txtBackupFolder.Location = New System.Drawing.Point(88, 24)
        Me.txtBackupFolder.Name = "txtBackupFolder"
        Me.txtBackupFolder.Size = New System.Drawing.Size(224, 20)
        Me.txtBackupFolder.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(216, 377)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(297, 377)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpBackup
        '
        Me.grpBackup.Controls.Add(Me.chkCheckSum)
        Me.grpBackup.Controls.Add(Me.chkRestoreOnLaunch)
        Me.grpBackup.Controls.Add(Me.chkOverwriteWarning)
        Me.grpBackup.Controls.Add(Me.chkCreateFolder)
        Me.grpBackup.Controls.Add(Me.chkBackupConfirm)
        Me.grpBackup.Location = New System.Drawing.Point(12, 163)
        Me.grpBackup.Name = "grpBackup"
        Me.grpBackup.Size = New System.Drawing.Size(360, 142)
        Me.grpBackup.TabIndex = 1
        Me.grpBackup.TabStop = False
        Me.grpBackup.Text = "Backup and Restore Options"
        '
        'chkCheckSum
        '
        Me.chkCheckSum.AutoSize = True
        Me.chkCheckSum.Location = New System.Drawing.Point(6, 111)
        Me.chkCheckSum.Name = "chkCheckSum"
        Me.chkCheckSum.Size = New System.Drawing.Size(195, 17)
        Me.chkCheckSum.TabIndex = 4
        Me.chkCheckSum.Text = "Verify backup files with a checksum"
        Me.chkCheckSum.UseVisualStyleBackColor = True
        '
        'chkRestoreOnLaunch
        '
        Me.chkRestoreOnLaunch.AutoSize = True
        Me.chkRestoreOnLaunch.Location = New System.Drawing.Point(6, 88)
        Me.chkRestoreOnLaunch.Name = "chkRestoreOnLaunch"
        Me.chkRestoreOnLaunch.Size = New System.Drawing.Size(249, 17)
        Me.chkRestoreOnLaunch.TabIndex = 3
        Me.chkRestoreOnLaunch.Text = "Check for new backup files to restore at launch"
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
        Me.chkCreateFolder.Location = New System.Drawing.Point(6, 42)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(211, 17)
        Me.chkCreateFolder.TabIndex = 1
        Me.chkCreateFolder.Text = "Create a sub-folder for each application"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 412)
        Me.Controls.Add(Me.grpBackup)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpPaths)
        Me.Controls.Add(Me.grpGeneral)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.grpGeneral.ResumeLayout(False)
        Me.grpGeneral.PerformLayout()
        Me.grpPaths.ResumeLayout(False)
        Me.grpPaths.PerformLayout()
        Me.grpBackup.ResumeLayout(False)
        Me.grpBackup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkMonitorOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents chkBackupConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents grpGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents grpPaths As System.Windows.Forms.GroupBox
    Friend WithEvents txtBackupFolder As System.Windows.Forms.TextBox
    Friend WithEvents fbBrowser As System.Windows.Forms.FolderBrowserDialog
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
End Class
