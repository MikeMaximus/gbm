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
        Me.chkBackupOnLaunch = New System.Windows.Forms.CheckBox()
        Me.chkStartWindows = New System.Windows.Forms.CheckBox()
        Me.chkStartToTray = New System.Windows.Forms.CheckBox()
        Me.chkAutoSaveLog = New System.Windows.Forms.CheckBox()
        Me.btnOptionalFields = New System.Windows.Forms.Button()
        Me.chkTimeTracking = New System.Windows.Forms.CheckBox()
        Me.chkShowDetectionTips = New System.Windows.Forms.CheckBox()
        Me.grpFolderOptions = New System.Windows.Forms.GroupBox()
        Me.chkUseGameID = New System.Windows.Forms.CheckBox()
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
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.nudSupressBackupThreshold = New System.Windows.Forms.NumericUpDown()
        Me.chkSupressBackup = New System.Windows.Forms.CheckBox()
        Me.grpGameData = New System.Windows.Forms.GroupBox()
        Me.chkSessionTracking = New System.Windows.Forms.CheckBox()
        Me.lstSettings = New System.Windows.Forms.ListBox()
        Me.btnResetMessages = New System.Windows.Forms.Button()
        Me.grpStartup.SuspendLayout()
        Me.grpFolderOptions.SuspendLayout()
        Me.grp7zGeneral.SuspendLayout()
        Me.pnlBackup.SuspendLayout()
        Me.grpBackupHandling.SuspendLayout()
        Me.pnl7z.SuspendLayout()
        Me.grp7zAdvanced.SuspendLayout()
        Me.grp7zInformation.SuspendLayout()
        Me.pnlGeneral.SuspendLayout()
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpGameData.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkMonitorOnStartup
        '
        Me.chkMonitorOnStartup.AutoSize = True
        Me.chkMonitorOnStartup.Location = New System.Drawing.Point(6, 65)
        Me.chkMonitorOnStartup.Name = "chkMonitorOnStartup"
        Me.chkMonitorOnStartup.Size = New System.Drawing.Size(149, 17)
        Me.chkMonitorOnStartup.TabIndex = 2
        Me.chkMonitorOnStartup.Text = "Start monitoring on launch"
        Me.chkMonitorOnStartup.UseVisualStyleBackColor = True
        '
        'grpStartup
        '
        Me.grpStartup.Controls.Add(Me.chkBackupOnLaunch)
        Me.grpStartup.Controls.Add(Me.chkStartWindows)
        Me.grpStartup.Controls.Add(Me.chkStartToTray)
        Me.grpStartup.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpStartup.Location = New System.Drawing.Point(6, 12)
        Me.grpStartup.Name = "grpStartup"
        Me.grpStartup.Size = New System.Drawing.Size(354, 112)
        Me.grpStartup.TabIndex = 0
        Me.grpStartup.TabStop = False
        Me.grpStartup.Text = "Startup"
        '
        'chkBackupOnLaunch
        '
        Me.chkBackupOnLaunch.AutoSize = True
        Me.chkBackupOnLaunch.Location = New System.Drawing.Point(6, 88)
        Me.chkBackupOnLaunch.Name = "chkBackupOnLaunch"
        Me.chkBackupOnLaunch.Size = New System.Drawing.Size(185, 17)
        Me.chkBackupOnLaunch.TabIndex = 3
        Me.chkBackupOnLaunch.Text = "Backup GBM data files on launch"
        Me.chkBackupOnLaunch.UseVisualStyleBackColor = True
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
        Me.chkAutoSaveLog.Location = New System.Drawing.Point(12, 280)
        Me.chkAutoSaveLog.Name = "chkAutoSaveLog"
        Me.chkAutoSaveLog.Size = New System.Drawing.Size(231, 17)
        Me.chkAutoSaveLog.TabIndex = 5
        Me.chkAutoSaveLog.Text = "Autosave log when max length is exceeded"
        Me.chkAutoSaveLog.UseVisualStyleBackColor = True
        '
        'btnOptionalFields
        '
        Me.btnOptionalFields.Location = New System.Drawing.Point(6, 65)
        Me.btnOptionalFields.Name = "btnOptionalFields"
        Me.btnOptionalFields.Size = New System.Drawing.Size(216, 23)
        Me.btnOptionalFields.TabIndex = 3
        Me.btnOptionalFields.Text = "Choose &Optional Sync Fields..."
        Me.btnOptionalFields.UseVisualStyleBackColor = True
        '
        'chkTimeTracking
        '
        Me.chkTimeTracking.AutoSize = True
        Me.chkTimeTracking.Location = New System.Drawing.Point(6, 19)
        Me.chkTimeTracking.Name = "chkTimeTracking"
        Me.chkTimeTracking.Size = New System.Drawing.Size(122, 17)
        Me.chkTimeTracking.TabIndex = 0
        Me.chkTimeTracking.Text = "Enable time tracking"
        Me.chkTimeTracking.UseVisualStyleBackColor = True
        '
        'chkShowDetectionTips
        '
        Me.chkShowDetectionTips.AutoSize = True
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(12, 257)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(159, 17)
        Me.chkShowDetectionTips.TabIndex = 4
        Me.chkShowDetectionTips.Text = "Show detection notifications"
        Me.chkShowDetectionTips.UseVisualStyleBackColor = True
        '
        'grpFolderOptions
        '
        Me.grpFolderOptions.Controls.Add(Me.chkUseGameID)
        Me.grpFolderOptions.Controls.Add(Me.btnBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.lblBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.txtBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.chkCreateFolder)
        Me.grpFolderOptions.Location = New System.Drawing.Point(6, 12)
        Me.grpFolderOptions.Name = "grpFolderOptions"
        Me.grpFolderOptions.Size = New System.Drawing.Size(354, 90)
        Me.grpFolderOptions.TabIndex = 0
        Me.grpFolderOptions.TabStop = False
        Me.grpFolderOptions.Text = "Files and Folders"
        '
        'chkUseGameID
        '
        Me.chkUseGameID.AutoSize = True
        Me.chkUseGameID.Location = New System.Drawing.Point(9, 65)
        Me.chkUseGameID.Name = "chkUseGameID"
        Me.chkUseGameID.Size = New System.Drawing.Size(205, 17)
        Me.chkUseGameID.TabIndex = 3
        Me.chkUseGameID.Text = "Use Game ID for folder and file names"
        Me.chkUseGameID.UseVisualStyleBackColor = True
        '
        'btnBackupFolder
        '
        Me.btnBackupFolder.Location = New System.Drawing.Point(313, 17)
        Me.btnBackupFolder.Name = "btnBackupFolder"
        Me.btnBackupFolder.Size = New System.Drawing.Size(27, 20)
        Me.btnBackupFolder.TabIndex = 1
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
        Me.txtBackupFolder.TabIndex = 0
        '
        'chkCreateFolder
        '
        Me.chkCreateFolder.AutoSize = True
        Me.chkCreateFolder.Location = New System.Drawing.Point(9, 43)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(186, 17)
        Me.chkCreateFolder.TabIndex = 2
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(384, 321)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(465, 321)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
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
        Me.cboCompression.TabIndex = 0
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
        Me.btn7zLocation.TabIndex = 2
        Me.btn7zLocation.Text = "..."
        Me.btn7zLocation.UseVisualStyleBackColor = True
        '
        'txt7zLocation
        '
        Me.txt7zLocation.Location = New System.Drawing.Point(110, 41)
        Me.txt7zLocation.Name = "txt7zLocation"
        Me.txt7zLocation.Size = New System.Drawing.Size(197, 20)
        Me.txt7zLocation.TabIndex = 1
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
        Me.txt7zArguments.TabIndex = 0
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
        Me.pnlBackup.Controls.Add(Me.chkBackupConfirm)
        Me.pnlBackup.Controls.Add(Me.chkOverwriteWarning)
        Me.pnlBackup.Controls.Add(Me.grpBackupHandling)
        Me.pnlBackup.Controls.Add(Me.grpFolderOptions)
        Me.pnlBackup.Location = New System.Drawing.Point(180, 0)
        Me.pnlBackup.Name = "pnlBackup"
        Me.pnlBackup.Size = New System.Drawing.Size(367, 314)
        Me.pnlBackup.TabIndex = 3
        '
        'chkBackupConfirm
        '
        Me.chkBackupConfirm.AutoSize = True
        Me.chkBackupConfirm.Location = New System.Drawing.Point(14, 199)
        Me.chkBackupConfirm.Name = "chkBackupConfirm"
        Me.chkBackupConfirm.Size = New System.Drawing.Size(160, 17)
        Me.chkBackupConfirm.TabIndex = 2
        Me.chkBackupConfirm.Text = "Disable backup confirmation"
        Me.chkBackupConfirm.UseVisualStyleBackColor = True
        '
        'chkOverwriteWarning
        '
        Me.chkOverwriteWarning.AutoSize = True
        Me.chkOverwriteWarning.Location = New System.Drawing.Point(14, 222)
        Me.chkOverwriteWarning.Name = "chkOverwriteWarning"
        Me.chkOverwriteWarning.Size = New System.Drawing.Size(139, 17)
        Me.chkOverwriteWarning.TabIndex = 3
        Me.chkOverwriteWarning.Text = "Show overwrite warning"
        Me.chkOverwriteWarning.UseVisualStyleBackColor = True
        '
        'grpBackupHandling
        '
        Me.grpBackupHandling.Controls.Add(Me.chkAutoRestore)
        Me.grpBackupHandling.Controls.Add(Me.chkRestoreNotify)
        Me.grpBackupHandling.Controls.Add(Me.chkAutoMark)
        Me.grpBackupHandling.Location = New System.Drawing.Point(6, 107)
        Me.grpBackupHandling.Margin = New System.Windows.Forms.Padding(2)
        Me.grpBackupHandling.Name = "grpBackupHandling"
        Me.grpBackupHandling.Padding = New System.Windows.Forms.Padding(2)
        Me.grpBackupHandling.Size = New System.Drawing.Size(354, 87)
        Me.grpBackupHandling.TabIndex = 1
        Me.grpBackupHandling.TabStop = False
        Me.grpBackupHandling.Text = "Backup Handling"
        '
        'chkAutoRestore
        '
        Me.chkAutoRestore.AutoSize = True
        Me.chkAutoRestore.Location = New System.Drawing.Point(8, 41)
        Me.chkAutoRestore.Margin = New System.Windows.Forms.Padding(2)
        Me.chkAutoRestore.Name = "chkAutoRestore"
        Me.chkAutoRestore.Size = New System.Drawing.Size(190, 17)
        Me.chkAutoRestore.TabIndex = 1
        Me.chkAutoRestore.Text = "Automatically restore new backups"
        Me.chkAutoRestore.UseVisualStyleBackColor = True
        '
        'chkRestoreNotify
        '
        Me.chkRestoreNotify.AutoSize = True
        Me.chkRestoreNotify.Location = New System.Drawing.Point(8, 19)
        Me.chkRestoreNotify.Margin = New System.Windows.Forms.Padding(2)
        Me.chkRestoreNotify.Name = "chkRestoreNotify"
        Me.chkRestoreNotify.Size = New System.Drawing.Size(216, 17)
        Me.chkRestoreNotify.TabIndex = 0
        Me.chkRestoreNotify.Text = "Display notifications about new backups"
        Me.chkRestoreNotify.UseVisualStyleBackColor = True
        '
        'chkAutoMark
        '
        Me.chkAutoMark.AutoSize = True
        Me.chkAutoMark.Location = New System.Drawing.Point(8, 63)
        Me.chkAutoMark.Name = "chkAutoMark"
        Me.chkAutoMark.Size = New System.Drawing.Size(321, 17)
        Me.chkAutoMark.TabIndex = 2
        Me.chkAutoMark.Text = "Automatically mark new backups as restored when appropriate"
        Me.chkAutoMark.UseVisualStyleBackColor = True
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
        Me.grp7zAdvanced.TabIndex = 1
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
        Me.pnlGeneral.Location = New System.Drawing.Point(180, 0)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(367, 314)
        Me.pnlGeneral.TabIndex = 1
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.Location = New System.Drawing.Point(232, 234)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.Size = New System.Drawing.Size(43, 13)
        Me.lblMinutes.TabIndex = 17
        Me.lblMinutes.Text = "minutes"
        '
        'nudSupressBackupThreshold
        '
        Me.nudSupressBackupThreshold.Location = New System.Drawing.Point(176, 232)
        Me.nudSupressBackupThreshold.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSupressBackupThreshold.Name = "nudSupressBackupThreshold"
        Me.nudSupressBackupThreshold.Size = New System.Drawing.Size(51, 20)
        Me.nudSupressBackupThreshold.TabIndex = 3
        '
        'chkSupressBackup
        '
        Me.chkSupressBackup.AutoSize = True
        Me.chkSupressBackup.Location = New System.Drawing.Point(12, 233)
        Me.chkSupressBackup.Name = "chkSupressBackup"
        Me.chkSupressBackup.Size = New System.Drawing.Size(158, 17)
        Me.chkSupressBackup.TabIndex = 2
        Me.chkSupressBackup.Text = "Ignore sessions shorter than"
        Me.chkSupressBackup.UseVisualStyleBackColor = True
        '
        'grpGameData
        '
        Me.grpGameData.Controls.Add(Me.chkSessionTracking)
        Me.grpGameData.Controls.Add(Me.chkTimeTracking)
        Me.grpGameData.Controls.Add(Me.btnOptionalFields)
        Me.grpGameData.Location = New System.Drawing.Point(6, 130)
        Me.grpGameData.Name = "grpGameData"
        Me.grpGameData.Size = New System.Drawing.Size(354, 97)
        Me.grpGameData.TabIndex = 1
        Me.grpGameData.TabStop = False
        Me.grpGameData.Text = "Game Data"
        '
        'chkSessionTracking
        '
        Me.chkSessionTracking.AutoSize = True
        Me.chkSessionTracking.Location = New System.Drawing.Point(6, 42)
        Me.chkSessionTracking.Name = "chkSessionTracking"
        Me.chkSessionTracking.Size = New System.Drawing.Size(138, 17)
        Me.chkSessionTracking.TabIndex = 1
        Me.chkSessionTracking.Text = "Enable session tracking"
        Me.chkSessionTracking.UseVisualStyleBackColor = True
        '
        'lstSettings
        '
        Me.lstSettings.FormattingEnabled = True
        Me.lstSettings.Location = New System.Drawing.Point(12, 12)
        Me.lstSettings.Name = "lstSettings"
        Me.lstSettings.Size = New System.Drawing.Size(162, 303)
        Me.lstSettings.TabIndex = 0
        '
        'btnResetMessages
        '
        Me.btnResetMessages.Location = New System.Drawing.Point(128, 321)
        Me.btnResetMessages.Name = "btnResetMessages"
        Me.btnResetMessages.Size = New System.Drawing.Size(110, 23)
        Me.btnResetMessages.TabIndex = 5
        Me.btnResetMessages.Text = "&Reset Warnings"
        Me.btnResetMessages.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 361)
        Me.Controls.Add(Me.pnlBackup)
        Me.Controls.Add(Me.btnResetMessages)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnl7z)
        Me.Controls.Add(Me.lstSettings)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
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
        CType(Me.nudSupressBackupThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpGameData.ResumeLayout(False)
        Me.grpGameData.PerformLayout()
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
    Friend WithEvents btnResetMessages As Button
    Friend WithEvents chkBackupOnLaunch As CheckBox
    Friend WithEvents chkUseGameID As CheckBox
End Class
