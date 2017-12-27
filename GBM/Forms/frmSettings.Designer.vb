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
        Me.chkBackupConfirm = New System.Windows.Forms.CheckBox()
        Me.chkOverwriteWarning = New System.Windows.Forms.CheckBox()
        Me.grpBackupHandling = New System.Windows.Forms.GroupBox()
        Me.chkAutoRestore = New System.Windows.Forms.CheckBox()
        Me.chkRestoreNotify = New System.Windows.Forms.CheckBox()
        Me.chkAutoMark = New System.Windows.Forms.CheckBox()
        Me.pnl7z = New System.Windows.Forms.Panel()
        Me.grp7zAdvanced = New System.Windows.Forms.GroupBox()
        Me.grp7zInformation = New System.Windows.Forms.GroupBox()
        Me.pnlGeneral = New System.Windows.Forms.Panel()
        Me.grpGameData = New System.Windows.Forms.GroupBox()
        Me.chkSessionTracking = New System.Windows.Forms.CheckBox()
        Me.lstSettings = New System.Windows.Forms.ListBox()
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.nudSupressBackupThreshold = New System.Windows.Forms.NumericUpDown()
        Me.chkSupressBackup = New System.Windows.Forms.CheckBox()
        Me.grpStartup.SuspendLayout()
        Me.grpFolderOptions.SuspendLayout()
        Me.grp7zGeneral.SuspendLayout()
        Me.pnlBackup.SuspendLayout()
        Me.grpBackupHandling.SuspendLayout()
        Me.pnl7z.SuspendLayout()
        Me.grp7zAdvanced.SuspendLayout()
        Me.grp7zInformation.SuspendLayout()
        Me.pnlGeneral.SuspendLayout()
        Me.grpGameData.SuspendLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkMonitorOnStartup
        '
        Me.chkMonitorOnStartup.AutoSize = True
        Me.chkMonitorOnStartup.Location = New System.Drawing.Point(8, 80)
        Me.chkMonitorOnStartup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkMonitorOnStartup.Name = "chkMonitorOnStartup"
        Me.chkMonitorOnStartup.Size = New System.Drawing.Size(192, 21)
        Me.chkMonitorOnStartup.TabIndex = 2
        Me.chkMonitorOnStartup.Text = "Start monitoring at launch"
        Me.chkMonitorOnStartup.UseVisualStyleBackColor = True
        '
        'grpStartup
        '
        Me.grpStartup.Controls.Add(Me.chkStartWindows)
        Me.grpStartup.Controls.Add(Me.chkStartToTray)
        Me.grpStartup.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpStartup.Location = New System.Drawing.Point(8, 15)
        Me.grpStartup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpStartup.Name = "grpStartup"
        Me.grpStartup.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpStartup.Size = New System.Drawing.Size(472, 111)
        Me.grpStartup.TabIndex = 0
        Me.grpStartup.TabStop = False
        Me.grpStartup.Text = "Startup"
        '
        'chkStartWindows
        '
        Me.chkStartWindows.AutoSize = True
        Me.chkStartWindows.Location = New System.Drawing.Point(8, 23)
        Me.chkStartWindows.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkStartWindows.Name = "chkStartWindows"
        Me.chkStartWindows.Size = New System.Drawing.Size(148, 21)
        Me.chkStartWindows.TabIndex = 0
        Me.chkStartWindows.Text = "Start with Windows"
        Me.chkStartWindows.UseVisualStyleBackColor = True
        '
        'chkStartToTray
        '
        Me.chkStartToTray.AutoSize = True
        Me.chkStartToTray.Location = New System.Drawing.Point(8, 52)
        Me.chkStartToTray.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkStartToTray.Name = "chkStartToTray"
        Me.chkStartToTray.Size = New System.Drawing.Size(152, 21)
        Me.chkStartToTray.TabIndex = 1
        Me.chkStartToTray.Text = "Start to system tray"
        Me.chkStartToTray.UseVisualStyleBackColor = True
        '
        'chkAutoSaveLog
        '
        Me.chkAutoSaveLog.AutoSize = True
        Me.chkAutoSaveLog.Location = New System.Drawing.Point(16, 309)
        Me.chkAutoSaveLog.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkAutoSaveLog.Name = "chkAutoSaveLog"
        Me.chkAutoSaveLog.Size = New System.Drawing.Size(300, 21)
        Me.chkAutoSaveLog.TabIndex = 5
        Me.chkAutoSaveLog.Text = "Autosave log when max length is exceeded"
        Me.chkAutoSaveLog.UseVisualStyleBackColor = True
        '
        'btnOptionalFields
        '
        Me.btnOptionalFields.Location = New System.Drawing.Point(137, 75)
        Me.btnOptionalFields.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnOptionalFields.Name = "btnOptionalFields"
        Me.btnOptionalFields.Size = New System.Drawing.Size(179, 28)
        Me.btnOptionalFields.TabIndex = 3
        Me.btnOptionalFields.Text = "Choose &Optional Fields..."
        Me.btnOptionalFields.UseVisualStyleBackColor = True
        '
        'chkTimeTracking
        '
        Me.chkTimeTracking.AutoSize = True
        Me.chkTimeTracking.Location = New System.Drawing.Point(8, 23)
        Me.chkTimeTracking.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkTimeTracking.Name = "chkTimeTracking"
        Me.chkTimeTracking.Size = New System.Drawing.Size(158, 21)
        Me.chkTimeTracking.TabIndex = 0
        Me.chkTimeTracking.Text = "Enable time tracking"
        Me.chkTimeTracking.UseVisualStyleBackColor = True
        '
        'chkSync
        '
        Me.chkSync.AutoSize = True
        Me.chkSync.Location = New System.Drawing.Point(8, 80)
        Me.chkSync.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkSync.Name = "chkSync"
        Me.chkSync.Size = New System.Drawing.Size(126, 21)
        Me.chkSync.TabIndex = 2
        Me.chkSync.Text = "Enable syncing"
        Me.chkSync.UseVisualStyleBackColor = True
        '
        'chkShowDetectionTips
        '
        Me.chkShowDetectionTips.AutoSize = True
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(16, 280)
        Me.chkShowDetectionTips.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(205, 21)
        Me.chkShowDetectionTips.TabIndex = 4
        Me.chkShowDetectionTips.Text = "Show detection notifications"
        Me.chkShowDetectionTips.UseVisualStyleBackColor = True
        '
        'grpFolderOptions
        '
        Me.grpFolderOptions.Controls.Add(Me.btnBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.lblBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.txtBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.chkCreateFolder)
        Me.grpFolderOptions.Location = New System.Drawing.Point(8, 15)
        Me.grpFolderOptions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpFolderOptions.Name = "grpFolderOptions"
        Me.grpFolderOptions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpFolderOptions.Size = New System.Drawing.Size(472, 86)
        Me.grpFolderOptions.TabIndex = 0
        Me.grpFolderOptions.TabStop = False
        Me.grpFolderOptions.Text = "Folders"
        '
        'btnBackupFolder
        '
        Me.btnBackupFolder.Location = New System.Drawing.Point(417, 21)
        Me.btnBackupFolder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBackupFolder.Name = "btnBackupFolder"
        Me.btnBackupFolder.Size = New System.Drawing.Size(36, 25)
        Me.btnBackupFolder.TabIndex = 1
        Me.btnBackupFolder.Text = "..."
        Me.btnBackupFolder.UseVisualStyleBackColor = True
        '
        'lblBackupFolder
        '
        Me.lblBackupFolder.AutoSize = True
        Me.lblBackupFolder.Location = New System.Drawing.Point(8, 25)
        Me.lblBackupFolder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBackupFolder.Name = "lblBackupFolder"
        Me.lblBackupFolder.Size = New System.Drawing.Size(103, 17)
        Me.lblBackupFolder.TabIndex = 0
        Me.lblBackupFolder.Text = "Backup Folder:"
        '
        'txtBackupFolder
        '
        Me.txtBackupFolder.Location = New System.Drawing.Point(121, 21)
        Me.txtBackupFolder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtBackupFolder.Name = "txtBackupFolder"
        Me.txtBackupFolder.Size = New System.Drawing.Size(287, 22)
        Me.txtBackupFolder.TabIndex = 0
        '
        'chkCreateFolder
        '
        Me.chkCreateFolder.AutoSize = True
        Me.chkCreateFolder.Location = New System.Drawing.Point(12, 53)
        Me.chkCreateFolder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(247, 21)
        Me.chkCreateFolder.TabIndex = 2
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(512, 395)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 28)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(620, 395)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 28)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grp7zGeneral
        '
        Me.grp7zGeneral.Controls.Add(Me.cboCompression)
        Me.grp7zGeneral.Controls.Add(Me.lblCompression)
        Me.grp7zGeneral.Location = New System.Drawing.Point(8, 15)
        Me.grp7zGeneral.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zGeneral.Name = "grp7zGeneral"
        Me.grp7zGeneral.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zGeneral.Size = New System.Drawing.Size(472, 62)
        Me.grp7zGeneral.TabIndex = 0
        Me.grp7zGeneral.TabStop = False
        Me.grp7zGeneral.Text = "General"
        '
        'cboCompression
        '
        Me.cboCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCompression.FormattingEnabled = True
        Me.cboCompression.Location = New System.Drawing.Point(147, 21)
        Me.cboCompression.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboCompression.Name = "cboCompression"
        Me.cboCompression.Size = New System.Drawing.Size(316, 24)
        Me.cboCompression.TabIndex = 0
        '
        'lblCompression
        '
        Me.lblCompression.AutoSize = True
        Me.lblCompression.Location = New System.Drawing.Point(8, 25)
        Me.lblCompression.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCompression.Name = "lblCompression"
        Me.lblCompression.Size = New System.Drawing.Size(94, 17)
        Me.lblCompression.TabIndex = 0
        Me.lblCompression.Text = "Compression:"
        '
        'btn7zLocation
        '
        Me.btn7zLocation.Location = New System.Drawing.Point(417, 50)
        Me.btn7zLocation.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btn7zLocation.Name = "btn7zLocation"
        Me.btn7zLocation.Size = New System.Drawing.Size(36, 25)
        Me.btn7zLocation.TabIndex = 2
        Me.btn7zLocation.Text = "..."
        Me.btn7zLocation.UseVisualStyleBackColor = True
        '
        'txt7zLocation
        '
        Me.txt7zLocation.Location = New System.Drawing.Point(147, 50)
        Me.txt7zLocation.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txt7zLocation.Name = "txt7zLocation"
        Me.txt7zLocation.Size = New System.Drawing.Size(261, 22)
        Me.txt7zLocation.TabIndex = 1
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Location = New System.Drawing.Point(8, 54)
        Me.lblLocation.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(117, 17)
        Me.lblLocation.TabIndex = 4
        Me.lblLocation.Text = "Custom Location:"
        '
        'txt7zArguments
        '
        Me.txt7zArguments.Location = New System.Drawing.Point(147, 18)
        Me.txt7zArguments.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txt7zArguments.Name = "txt7zArguments"
        Me.txt7zArguments.Size = New System.Drawing.Size(316, 22)
        Me.txt7zArguments.TabIndex = 0
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(8, 25)
        Me.lblArguments.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(131, 17)
        Me.lblArguments.TabIndex = 2
        Me.lblArguments.Text = "Custom Arguments:"
        '
        'lbl7zCopyright
        '
        Me.lbl7zCopyright.AutoEllipsis = True
        Me.lbl7zCopyright.Location = New System.Drawing.Point(12, 42)
        Me.lbl7zCopyright.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl7zCopyright.Name = "lbl7zCopyright"
        Me.lbl7zCopyright.Size = New System.Drawing.Size(452, 21)
        Me.lbl7zCopyright.TabIndex = 8
        Me.lbl7zCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbl7zProduct
        '
        Me.lbl7zProduct.AutoEllipsis = True
        Me.lbl7zProduct.Location = New System.Drawing.Point(12, 21)
        Me.lbl7zProduct.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl7zProduct.Name = "lbl7zProduct"
        Me.lbl7zProduct.Size = New System.Drawing.Size(452, 21)
        Me.lbl7zProduct.TabIndex = 7
        Me.lbl7zProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDefaults
        '
        Me.btnDefaults.Location = New System.Drawing.Point(16, 395)
        Me.btnDefaults.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(147, 28)
        Me.btnDefaults.TabIndex = 4
        Me.btnDefaults.Text = "Set &Defaults"
        Me.btnDefaults.UseVisualStyleBackColor = True
        '
        'pnlBackup
        '
        Me.pnlBackup.Controls.Add(Me.chkBackupConfirm)
        Me.pnlBackup.Controls.Add(Me.chkOverwriteWarning)
        Me.pnlBackup.Controls.Add(Me.grpBackupHandling)
        Me.pnlBackup.Controls.Add(Me.grpFolderOptions)
        Me.pnlBackup.Location = New System.Drawing.Point(240, 0)
        Me.pnlBackup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlBackup.Name = "pnlBackup"
        Me.pnlBackup.Size = New System.Drawing.Size(489, 386)
        Me.pnlBackup.TabIndex = 3
        '
        'chkBackupConfirm
        '
        Me.chkBackupConfirm.AutoSize = True
        Me.chkBackupConfirm.Location = New System.Drawing.Point(18, 221)
        Me.chkBackupConfirm.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkBackupConfirm.Name = "chkBackupConfirm"
        Me.chkBackupConfirm.Size = New System.Drawing.Size(208, 21)
        Me.chkBackupConfirm.TabIndex = 2
        Me.chkBackupConfirm.Text = "Disable backup confirmation"
        Me.chkBackupConfirm.UseVisualStyleBackColor = True
        '
        'chkOverwriteWarning
        '
        Me.chkOverwriteWarning.AutoSize = True
        Me.chkOverwriteWarning.Location = New System.Drawing.Point(19, 250)
        Me.chkOverwriteWarning.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkOverwriteWarning.Name = "chkOverwriteWarning"
        Me.chkOverwriteWarning.Size = New System.Drawing.Size(178, 21)
        Me.chkOverwriteWarning.TabIndex = 3
        Me.chkOverwriteWarning.Text = "Show overwrite warning"
        Me.chkOverwriteWarning.UseVisualStyleBackColor = True
        '
        'grpBackupHandling
        '
        Me.grpBackupHandling.Controls.Add(Me.chkAutoRestore)
        Me.grpBackupHandling.Controls.Add(Me.chkRestoreNotify)
        Me.grpBackupHandling.Controls.Add(Me.chkAutoMark)
        Me.grpBackupHandling.Location = New System.Drawing.Point(8, 108)
        Me.grpBackupHandling.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.grpBackupHandling.Name = "grpBackupHandling"
        Me.grpBackupHandling.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.grpBackupHandling.Size = New System.Drawing.Size(472, 107)
        Me.grpBackupHandling.TabIndex = 1
        Me.grpBackupHandling.TabStop = False
        Me.grpBackupHandling.Text = "Backup Handling"
        '
        'chkAutoRestore
        '
        Me.chkAutoRestore.AutoSize = True
        Me.chkAutoRestore.Location = New System.Drawing.Point(11, 50)
        Me.chkAutoRestore.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkAutoRestore.Name = "chkAutoRestore"
        Me.chkAutoRestore.Size = New System.Drawing.Size(248, 21)
        Me.chkAutoRestore.TabIndex = 1
        Me.chkAutoRestore.Text = "Automatically restore new backups"
        Me.chkAutoRestore.UseVisualStyleBackColor = True
        '
        'chkRestoreNotify
        '
        Me.chkRestoreNotify.AutoSize = True
        Me.chkRestoreNotify.Location = New System.Drawing.Point(11, 23)
        Me.chkRestoreNotify.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkRestoreNotify.Name = "chkRestoreNotify"
        Me.chkRestoreNotify.Size = New System.Drawing.Size(281, 21)
        Me.chkRestoreNotify.TabIndex = 0
        Me.chkRestoreNotify.Text = "Display notifications about new backups"
        Me.chkRestoreNotify.UseVisualStyleBackColor = True
        '
        'chkAutoMark
        '
        Me.chkAutoMark.AutoSize = True
        Me.chkAutoMark.Location = New System.Drawing.Point(11, 78)
        Me.chkAutoMark.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkAutoMark.Name = "chkAutoMark"
        Me.chkAutoMark.Size = New System.Drawing.Size(424, 21)
        Me.chkAutoMark.TabIndex = 2
        Me.chkAutoMark.Text = "Automatically mark new backups as restored when appropriate"
        Me.chkAutoMark.UseVisualStyleBackColor = True
        '
        'pnl7z
        '
        Me.pnl7z.Controls.Add(Me.grp7zAdvanced)
        Me.pnl7z.Controls.Add(Me.grp7zInformation)
        Me.pnl7z.Controls.Add(Me.grp7zGeneral)
        Me.pnl7z.Location = New System.Drawing.Point(240, 0)
        Me.pnl7z.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnl7z.Name = "pnl7z"
        Me.pnl7z.Size = New System.Drawing.Size(489, 386)
        Me.pnl7z.TabIndex = 2
        '
        'grp7zAdvanced
        '
        Me.grp7zAdvanced.Controls.Add(Me.btn7zLocation)
        Me.grp7zAdvanced.Controls.Add(Me.lblArguments)
        Me.grp7zAdvanced.Controls.Add(Me.txt7zLocation)
        Me.grp7zAdvanced.Controls.Add(Me.txt7zArguments)
        Me.grp7zAdvanced.Controls.Add(Me.lblLocation)
        Me.grp7zAdvanced.Location = New System.Drawing.Point(8, 84)
        Me.grp7zAdvanced.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zAdvanced.Name = "grp7zAdvanced"
        Me.grp7zAdvanced.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zAdvanced.Size = New System.Drawing.Size(472, 90)
        Me.grp7zAdvanced.TabIndex = 1
        Me.grp7zAdvanced.TabStop = False
        Me.grp7zAdvanced.Text = "Advanced"
        '
        'grp7zInformation
        '
        Me.grp7zInformation.Controls.Add(Me.lbl7zProduct)
        Me.grp7zInformation.Controls.Add(Me.lbl7zCopyright)
        Me.grp7zInformation.Location = New System.Drawing.Point(8, 180)
        Me.grp7zInformation.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zInformation.Name = "grp7zInformation"
        Me.grp7zInformation.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grp7zInformation.Size = New System.Drawing.Size(472, 78)
        Me.grp7zInformation.TabIndex = 2
        Me.grp7zInformation.TabStop = False
        Me.grp7zInformation.Text = "Utility Information"
        '
        'pnlGeneral
        '
        Me.pnlGeneral.Controls.Add(Me.lblMinutes)
        Me.pnlGeneral.Controls.Add(Me.nudSupressBackupThreshold)
        Me.pnlGeneral.Controls.Add(Me.chkSupressBackup)
        Me.pnlGeneral.Controls.Add(Me.chkAutoSaveLog)
        Me.pnlGeneral.Controls.Add(Me.grpGameData)
        Me.pnlGeneral.Controls.Add(Me.chkShowDetectionTips)
        Me.pnlGeneral.Controls.Add(Me.grpStartup)
        Me.pnlGeneral.Location = New System.Drawing.Point(240, 0)
        Me.pnlGeneral.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(489, 386)
        Me.pnlGeneral.TabIndex = 1
        '
        'grpGameData
        '
        Me.grpGameData.Controls.Add(Me.chkSessionTracking)
        Me.grpGameData.Controls.Add(Me.chkTimeTracking)
        Me.grpGameData.Controls.Add(Me.chkSync)
        Me.grpGameData.Controls.Add(Me.btnOptionalFields)
        Me.grpGameData.Location = New System.Drawing.Point(8, 130)
        Me.grpGameData.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpGameData.Name = "grpGameData"
        Me.grpGameData.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpGameData.Size = New System.Drawing.Size(472, 113)
        Me.grpGameData.TabIndex = 1
        Me.grpGameData.TabStop = False
        Me.grpGameData.Text = "Game Data"
        '
        'chkSessionTracking
        '
        Me.chkSessionTracking.AutoSize = True
        Me.chkSessionTracking.Location = New System.Drawing.Point(8, 52)
        Me.chkSessionTracking.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkSessionTracking.Name = "chkSessionTracking"
        Me.chkSessionTracking.Size = New System.Drawing.Size(180, 21)
        Me.chkSessionTracking.TabIndex = 1
        Me.chkSessionTracking.Text = "Enable session tracking"
        Me.chkSessionTracking.UseVisualStyleBackColor = True
        '
        'lstSettings
        '
        Me.lstSettings.FormattingEnabled = True
        Me.lstSettings.ItemHeight = 16
        Me.lstSettings.Location = New System.Drawing.Point(16, 15)
        Me.lstSettings.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.lstSettings.Name = "lstSettings"
        Me.lstSettings.Size = New System.Drawing.Size(215, 372)
        Me.lstSettings.TabIndex = 0
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.Location = New System.Drawing.Point(310, 252)
        Me.lblMinutes.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.Size = New System.Drawing.Size(57, 17)
        Me.lblMinutes.TabIndex = 17
        Me.lblMinutes.Text = "minutes"
        '
        'nudSupressBackupThreshold
        '
        Me.nudSupressBackupThreshold.Location = New System.Drawing.Point(234, 250)
        Me.nudSupressBackupThreshold.Margin = New System.Windows.Forms.Padding(4)
        Me.nudSupressBackupThreshold.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSupressBackupThreshold.Name = "nudSupressBackupThreshold"
        Me.nudSupressBackupThreshold.Size = New System.Drawing.Size(68, 22)
        Me.nudSupressBackupThreshold.TabIndex = 3
        '
        'chkSupressBackup
        '
        Me.chkSupressBackup.AutoSize = True
        Me.chkSupressBackup.Location = New System.Drawing.Point(16, 251)
        Me.chkSupressBackup.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSupressBackup.Name = "chkSupressBackup"
        Me.chkSupressBackup.Size = New System.Drawing.Size(210, 21)
        Me.chkSupressBackup.TabIndex = 2
        Me.chkSupressBackup.Text = "Ignore sessions shorter than"
        Me.chkSupressBackup.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 444)
        Me.Controls.Add(Me.pnlBackup)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnl7z)
        Me.Controls.Add(Me.lstSettings)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
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
        Me.grp7zGeneral.ResumeLayout(False)
        Me.grp7zGeneral.PerformLayout()
        Me.pnlBackup.ResumeLayout(False)
        Me.pnlBackup.PerformLayout()
        Me.grpBackupHandling.ResumeLayout(False)
        Me.grpBackupHandling.PerformLayout()
        Me.pnl7z.ResumeLayout(False)
        Me.grp7zAdvanced.ResumeLayout(False)
        Me.grp7zAdvanced.PerformLayout()
        Me.grp7zInformation.ResumeLayout(False)
        Me.pnlGeneral.ResumeLayout(False)
        Me.pnlGeneral.PerformLayout()
        Me.grpGameData.ResumeLayout(False)
        Me.grpGameData.PerformLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkMonitorOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents grpStartup As System.Windows.Forms.GroupBox
    Friend WithEvents grpFolderOptions As System.Windows.Forms.GroupBox
    Friend WithEvents txtBackupFolder As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBackupFolder As System.Windows.Forms.Label
    Friend WithEvents btnBackupFolder As System.Windows.Forms.Button
    Friend WithEvents chkShowDetectionTips As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartToTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkCreateFolder As System.Windows.Forms.CheckBox
    Friend WithEvents chkSync As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartWindows As System.Windows.Forms.CheckBox
    Friend WithEvents chkTimeTracking As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkBackupConfirm As CheckBox
    Friend WithEvents chkOverwriteWarning As CheckBox
    Friend WithEvents grpBackupHandling As GroupBox
    Friend WithEvents chkAutoMark As CheckBox
    Friend WithEvents chkAutoRestore As CheckBox
    Friend WithEvents chkRestoreNotify As CheckBox
    Friend WithEvents chkSessionTracking As CheckBox
    Friend WithEvents lblMinutes As Label
    Friend WithEvents nudSupressBackupThreshold As NumericUpDown
    Friend WithEvents chkSupressBackup As CheckBox
End Class
