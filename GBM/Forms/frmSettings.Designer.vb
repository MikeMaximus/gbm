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
        Me.chkTimeTracking = New System.Windows.Forms.CheckBox()
        Me.chkShowDetectionTips = New System.Windows.Forms.CheckBox()
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
        Me.grpBackupExperimental = New System.Windows.Forms.GroupBox()
        Me.chkEnableLiveBackup = New System.Windows.Forms.CheckBox()
        Me.grpBackupConfirmations = New System.Windows.Forms.GroupBox()
        Me.chkBackupNotification = New System.Windows.Forms.CheckBox()
        Me.chkDisableDiskSpaceCheck = New System.Windows.Forms.CheckBox()
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
        Me.grpGameMonitoringOptions = New System.Windows.Forms.GroupBox()
        Me.chkTwoPassDetection = New System.Windows.Forms.CheckBox()
        Me.cboDetectSpeed = New System.Windows.Forms.ComboBox()
        Me.lblDetectSpeed = New System.Windows.Forms.Label()
        Me.chkSuppressBackup = New System.Windows.Forms.CheckBox()
        Me.lblMinutes = New System.Windows.Forms.Label()
        Me.nudSuppressBackupThreshold = New System.Windows.Forms.NumericUpDown()
        Me.grpOptionalFeeatures = New System.Windows.Forms.GroupBox()
        Me.chkStorePathAutoConfig = New System.Windows.Forms.CheckBox()
        Me.chkEnableLauncher = New System.Windows.Forms.CheckBox()
        Me.chkSessionTracking = New System.Windows.Forms.CheckBox()
        Me.pnlInterface = New System.Windows.Forms.Panel()
        Me.grpLogOptions = New System.Windows.Forms.GroupBox()
        Me.chkDisableSyncMessages = New System.Windows.Forms.CheckBox()
        Me.chkAutoSaveLog = New System.Windows.Forms.CheckBox()
        Me.grpGameManagerOptions = New System.Windows.Forms.GroupBox()
        Me.btnOptionalFields = New System.Windows.Forms.Button()
        Me.chkShowResolvedPaths = New System.Windows.Forms.CheckBox()
        Me.grpMainWindowOptions = New System.Windows.Forms.GroupBox()
        Me.chkHideLog = New System.Windows.Forms.CheckBox()
        Me.chkHideButtons = New System.Windows.Forms.CheckBox()
        Me.chkHideGameList = New System.Windows.Forms.CheckBox()
        Me.chkExitNoWarning = New System.Windows.Forms.CheckBox()
        Me.chkExitOnClose = New System.Windows.Forms.CheckBox()
        Me.lstSettings = New System.Windows.Forms.ListBox()
        Me.btnResetMessages = New System.Windows.Forms.Button()
        Me.pnlStartup = New System.Windows.Forms.Panel()
        Me.grpStartup = New System.Windows.Forms.GroupBox()
        Me.chkBackupOnLaunch = New System.Windows.Forms.CheckBox()
        Me.chkAutoStart = New System.Windows.Forms.CheckBox()
        Me.chkStartMinimized = New System.Windows.Forms.CheckBox()
        Me.chkMonitorOnStartup = New System.Windows.Forms.CheckBox()
        Me.pnlFilesAndFolders = New System.Windows.Forms.Panel()
        Me.grpFolderOptions = New System.Windows.Forms.GroupBox()
        Me.chkDeleteToRecycleBin = New System.Windows.Forms.CheckBox()
        Me.btnTempFolder = New System.Windows.Forms.Button()
        Me.lblTempFolder = New System.Windows.Forms.Label()
        Me.txtTempFolder = New System.Windows.Forms.TextBox()
        Me.btnBackupFolder = New System.Windows.Forms.Button()
        Me.lblBackupFolder = New System.Windows.Forms.Label()
        Me.txtBackupFolder = New System.Windows.Forms.TextBox()
        Me.chkCreateFolder = New System.Windows.Forms.CheckBox()
        Me.pnlGlobalHotKeys = New System.Windows.Forms.Panel()
        Me.grpHotKeyGeneral = New System.Windows.Forms.GroupBox()
        Me.chkEnableHotKeys = New System.Windows.Forms.CheckBox()
        Me.grpHotKeyBindings = New System.Windows.Forms.GroupBox()
        Me.btnResetRestoreBind = New System.Windows.Forms.Button()
        Me.txtRestoreBind = New System.Windows.Forms.TextBox()
        Me.lblRestoreBind = New System.Windows.Forms.Label()
        Me.btnResetBackupBind = New System.Windows.Forms.Button()
        Me.txtBackupBind = New System.Windows.Forms.TextBox()
        Me.lblBackupBind = New System.Windows.Forms.Label()
        Me.grp7zGeneral.SuspendLayout()
        Me.pnlBackup.SuspendLayout()
        Me.grpBackupExperimental.SuspendLayout()
        Me.grpBackupConfirmations.SuspendLayout()
        Me.grpBackupHandling.SuspendLayout()
        Me.pnl7z.SuspendLayout()
        Me.grp7zAdvanced.SuspendLayout()
        Me.grp7zInformation.SuspendLayout()
        Me.pnlGeneral.SuspendLayout()
        Me.grpGameMonitoringOptions.SuspendLayout()
        CType(Me.nudSuppressBackupThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpOptionalFeeatures.SuspendLayout()
        Me.pnlInterface.SuspendLayout()
        Me.grpLogOptions.SuspendLayout()
        Me.grpGameManagerOptions.SuspendLayout()
        Me.grpMainWindowOptions.SuspendLayout()
        Me.pnlStartup.SuspendLayout()
        Me.grpStartup.SuspendLayout()
        Me.pnlFilesAndFolders.SuspendLayout()
        Me.grpFolderOptions.SuspendLayout()
        Me.pnlGlobalHotKeys.SuspendLayout()
        Me.grpHotKeyGeneral.SuspendLayout()
        Me.grpHotKeyBindings.SuspendLayout()
        Me.SuspendLayout()
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
        Me.chkShowDetectionTips.Location = New System.Drawing.Point(6, 42)
        Me.chkShowDetectionTips.Name = "chkShowDetectionTips"
        Me.chkShowDetectionTips.Size = New System.Drawing.Size(159, 17)
        Me.chkShowDetectionTips.TabIndex = 2
        Me.chkShowDetectionTips.Text = "Show detection notifications"
        Me.chkShowDetectionTips.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GBM.My.Resources.Resources.Multi_Save
        Me.btnSave.Location = New System.Drawing.Point(421, 329)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(60, 45)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(487, 329)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
        Me.lblCompression.Location = New System.Drawing.Point(5, 20)
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
        Me.lblLocation.Location = New System.Drawing.Point(5, 44)
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
        Me.lblArguments.Location = New System.Drawing.Point(5, 20)
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
        Me.btnDefaults.Image = Global.GBM.My.Resources.Resources.Multi_Reset
        Me.btnDefaults.Location = New System.Drawing.Point(12, 329)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(60, 45)
        Me.btnDefaults.TabIndex = 2
        Me.btnDefaults.Text = "&Defaults"
        Me.btnDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDefaults.UseVisualStyleBackColor = True
        '
        'pnlBackup
        '
        Me.pnlBackup.Controls.Add(Me.grpBackupExperimental)
        Me.pnlBackup.Controls.Add(Me.grpBackupConfirmations)
        Me.pnlBackup.Controls.Add(Me.grpBackupHandling)
        Me.pnlBackup.Location = New System.Drawing.Point(180, 0)
        Me.pnlBackup.Name = "pnlBackup"
        Me.pnlBackup.Size = New System.Drawing.Size(367, 323)
        Me.pnlBackup.TabIndex = 1
        '
        'grpBackupExperimental
        '
        Me.grpBackupExperimental.Controls.Add(Me.chkEnableLiveBackup)
        Me.grpBackupExperimental.Location = New System.Drawing.Point(6, 223)
        Me.grpBackupExperimental.Name = "grpBackupExperimental"
        Me.grpBackupExperimental.Size = New System.Drawing.Size(354, 44)
        Me.grpBackupExperimental.TabIndex = 2
        Me.grpBackupExperimental.TabStop = False
        Me.grpBackupExperimental.Text = "Experimental"
        '
        'chkEnableLiveBackup
        '
        Me.chkEnableLiveBackup.AutoSize = True
        Me.chkEnableLiveBackup.Location = New System.Drawing.Point(6, 19)
        Me.chkEnableLiveBackup.Name = "chkEnableLiveBackup"
        Me.chkEnableLiveBackup.Size = New System.Drawing.Size(292, 17)
        Me.chkEnableLiveBackup.TabIndex = 0
        Me.chkEnableLiveBackup.Text = "Allow backups and restores for currently monitored game"
        Me.chkEnableLiveBackup.UseVisualStyleBackColor = True
        '
        'grpBackupConfirmations
        '
        Me.grpBackupConfirmations.Controls.Add(Me.chkBackupNotification)
        Me.grpBackupConfirmations.Controls.Add(Me.chkDisableDiskSpaceCheck)
        Me.grpBackupConfirmations.Controls.Add(Me.chkBackupConfirm)
        Me.grpBackupConfirmations.Controls.Add(Me.chkOverwriteWarning)
        Me.grpBackupConfirmations.Location = New System.Drawing.Point(6, 104)
        Me.grpBackupConfirmations.Name = "grpBackupConfirmations"
        Me.grpBackupConfirmations.Size = New System.Drawing.Size(354, 113)
        Me.grpBackupConfirmations.TabIndex = 1
        Me.grpBackupConfirmations.TabStop = False
        Me.grpBackupConfirmations.Text = "Backup Confirmations"
        '
        'chkBackupNotification
        '
        Me.chkBackupNotification.AutoSize = True
        Me.chkBackupNotification.Location = New System.Drawing.Point(6, 88)
        Me.chkBackupNotification.Name = "chkBackupNotification"
        Me.chkBackupNotification.Size = New System.Drawing.Size(246, 17)
        Me.chkBackupNotification.TabIndex = 3
        Me.chkBackupNotification.Text = "Show notification when a backup is completed"
        Me.chkBackupNotification.UseVisualStyleBackColor = True
        '
        'chkDisableDiskSpaceCheck
        '
        Me.chkDisableDiskSpaceCheck.AutoSize = True
        Me.chkDisableDiskSpaceCheck.Location = New System.Drawing.Point(6, 42)
        Me.chkDisableDiskSpaceCheck.Name = "chkDisableDiskSpaceCheck"
        Me.chkDisableDiskSpaceCheck.Size = New System.Drawing.Size(222, 17)
        Me.chkDisableDiskSpaceCheck.TabIndex = 1
        Me.chkDisableDiskSpaceCheck.Text = "Disable disk space check prior to backup"
        Me.chkDisableDiskSpaceCheck.UseVisualStyleBackColor = True
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
        'grpBackupHandling
        '
        Me.grpBackupHandling.Controls.Add(Me.chkAutoRestore)
        Me.grpBackupHandling.Controls.Add(Me.chkRestoreNotify)
        Me.grpBackupHandling.Controls.Add(Me.chkAutoMark)
        Me.grpBackupHandling.Location = New System.Drawing.Point(6, 12)
        Me.grpBackupHandling.Margin = New System.Windows.Forms.Padding(2)
        Me.grpBackupHandling.Name = "grpBackupHandling"
        Me.grpBackupHandling.Padding = New System.Windows.Forms.Padding(2)
        Me.grpBackupHandling.Size = New System.Drawing.Size(354, 87)
        Me.grpBackupHandling.TabIndex = 0
        Me.grpBackupHandling.TabStop = False
        Me.grpBackupHandling.Text = "Backup Handling"
        '
        'chkAutoRestore
        '
        Me.chkAutoRestore.AutoSize = True
        Me.chkAutoRestore.Location = New System.Drawing.Point(6, 40)
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
        Me.chkRestoreNotify.Location = New System.Drawing.Point(6, 19)
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
        Me.chkAutoMark.Location = New System.Drawing.Point(6, 62)
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
        Me.pnl7z.Size = New System.Drawing.Size(367, 323)
        Me.pnl7z.TabIndex = 1
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
        Me.pnlGeneral.Controls.Add(Me.grpGameMonitoringOptions)
        Me.pnlGeneral.Controls.Add(Me.grpOptionalFeeatures)
        Me.pnlGeneral.Location = New System.Drawing.Point(180, 0)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(367, 323)
        Me.pnlGeneral.TabIndex = 1
        '
        'grpGameMonitoringOptions
        '
        Me.grpGameMonitoringOptions.Controls.Add(Me.chkTwoPassDetection)
        Me.grpGameMonitoringOptions.Controls.Add(Me.cboDetectSpeed)
        Me.grpGameMonitoringOptions.Controls.Add(Me.lblDetectSpeed)
        Me.grpGameMonitoringOptions.Controls.Add(Me.chkSuppressBackup)
        Me.grpGameMonitoringOptions.Controls.Add(Me.lblMinutes)
        Me.grpGameMonitoringOptions.Controls.Add(Me.chkShowDetectionTips)
        Me.grpGameMonitoringOptions.Controls.Add(Me.nudSuppressBackupThreshold)
        Me.grpGameMonitoringOptions.Location = New System.Drawing.Point(6, 131)
        Me.grpGameMonitoringOptions.Name = "grpGameMonitoringOptions"
        Me.grpGameMonitoringOptions.Size = New System.Drawing.Size(354, 118)
        Me.grpGameMonitoringOptions.TabIndex = 1
        Me.grpGameMonitoringOptions.TabStop = False
        Me.grpGameMonitoringOptions.Text = "Game Monitoring Options"
        '
        'chkTwoPassDetection
        '
        Me.chkTwoPassDetection.AutoSize = True
        Me.chkTwoPassDetection.Location = New System.Drawing.Point(6, 92)
        Me.chkTwoPassDetection.Name = "chkTwoPassDetection"
        Me.chkTwoPassDetection.Size = New System.Drawing.Size(122, 17)
        Me.chkTwoPassDetection.TabIndex = 5
        Me.chkTwoPassDetection.Text = "Two-Pass Detection"
        Me.chkTwoPassDetection.UseVisualStyleBackColor = True
        '
        'cboDetectSpeed
        '
        Me.cboDetectSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDetectSpeed.FormattingEnabled = True
        Me.cboDetectSpeed.Location = New System.Drawing.Point(99, 65)
        Me.cboDetectSpeed.Name = "cboDetectSpeed"
        Me.cboDetectSpeed.Size = New System.Drawing.Size(122, 21)
        Me.cboDetectSpeed.TabIndex = 4
        '
        'lblDetectSpeed
        '
        Me.lblDetectSpeed.AutoSize = True
        Me.lblDetectSpeed.Location = New System.Drawing.Point(3, 68)
        Me.lblDetectSpeed.Name = "lblDetectSpeed"
        Me.lblDetectSpeed.Size = New System.Drawing.Size(90, 13)
        Me.lblDetectSpeed.TabIndex = 3
        Me.lblDetectSpeed.Text = "Detection Speed:"
        '
        'chkSuppressBackup
        '
        Me.chkSuppressBackup.AutoSize = True
        Me.chkSuppressBackup.Location = New System.Drawing.Point(6, 19)
        Me.chkSuppressBackup.Name = "chkSuppressBackup"
        Me.chkSuppressBackup.Size = New System.Drawing.Size(158, 17)
        Me.chkSuppressBackup.TabIndex = 0
        Me.chkSuppressBackup.Text = "Ignore sessions shorter than"
        Me.chkSuppressBackup.UseVisualStyleBackColor = True
        '
        'lblMinutes
        '
        Me.lblMinutes.AutoSize = True
        Me.lblMinutes.Location = New System.Drawing.Point(226, 20)
        Me.lblMinutes.Name = "lblMinutes"
        Me.lblMinutes.Size = New System.Drawing.Size(43, 13)
        Me.lblMinutes.TabIndex = 17
        Me.lblMinutes.Text = "minutes"
        '
        'nudSuppressBackupThreshold
        '
        Me.nudSuppressBackupThreshold.Location = New System.Drawing.Point(169, 18)
        Me.nudSuppressBackupThreshold.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSuppressBackupThreshold.Name = "nudSuppressBackupThreshold"
        Me.nudSuppressBackupThreshold.Size = New System.Drawing.Size(51, 20)
        Me.nudSuppressBackupThreshold.TabIndex = 1
        '
        'grpOptionalFeeatures
        '
        Me.grpOptionalFeeatures.Controls.Add(Me.chkStorePathAutoConfig)
        Me.grpOptionalFeeatures.Controls.Add(Me.chkEnableLauncher)
        Me.grpOptionalFeeatures.Controls.Add(Me.chkSessionTracking)
        Me.grpOptionalFeeatures.Controls.Add(Me.chkTimeTracking)
        Me.grpOptionalFeeatures.Location = New System.Drawing.Point(6, 12)
        Me.grpOptionalFeeatures.Name = "grpOptionalFeeatures"
        Me.grpOptionalFeeatures.Size = New System.Drawing.Size(354, 111)
        Me.grpOptionalFeeatures.TabIndex = 0
        Me.grpOptionalFeeatures.TabStop = False
        Me.grpOptionalFeeatures.Text = "Optional Features"
        '
        'chkStorePathAutoConfig
        '
        Me.chkStorePathAutoConfig.AutoSize = True
        Me.chkStorePathAutoConfig.Location = New System.Drawing.Point(6, 88)
        Me.chkStorePathAutoConfig.Name = "chkStorePathAutoConfig"
        Me.chkStorePathAutoConfig.Size = New System.Drawing.Size(239, 17)
        Me.chkStorePathAutoConfig.TabIndex = 3
        Me.chkStorePathAutoConfig.Text = "Enable automatic configuration of store paths"
        Me.chkStorePathAutoConfig.UseVisualStyleBackColor = True
        '
        'chkEnableLauncher
        '
        Me.chkEnableLauncher.AutoSize = True
        Me.chkEnableLauncher.Location = New System.Drawing.Point(6, 65)
        Me.chkEnableLauncher.Name = "chkEnableLauncher"
        Me.chkEnableLauncher.Size = New System.Drawing.Size(137, 17)
        Me.chkEnableLauncher.TabIndex = 2
        Me.chkEnableLauncher.Text = "Enable game launching"
        Me.chkEnableLauncher.UseVisualStyleBackColor = True
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
        'pnlInterface
        '
        Me.pnlInterface.Controls.Add(Me.grpLogOptions)
        Me.pnlInterface.Controls.Add(Me.grpGameManagerOptions)
        Me.pnlInterface.Controls.Add(Me.grpMainWindowOptions)
        Me.pnlInterface.Location = New System.Drawing.Point(180, 0)
        Me.pnlInterface.Name = "pnlInterface"
        Me.pnlInterface.Size = New System.Drawing.Size(367, 323)
        Me.pnlInterface.TabIndex = 1
        '
        'grpLogOptions
        '
        Me.grpLogOptions.Controls.Add(Me.chkDisableSyncMessages)
        Me.grpLogOptions.Controls.Add(Me.chkAutoSaveLog)
        Me.grpLogOptions.Location = New System.Drawing.Point(6, 231)
        Me.grpLogOptions.Name = "grpLogOptions"
        Me.grpLogOptions.Size = New System.Drawing.Size(356, 65)
        Me.grpLogOptions.TabIndex = 3
        Me.grpLogOptions.TabStop = False
        Me.grpLogOptions.Text = "Log Options"
        '
        'chkDisableSyncMessages
        '
        Me.chkDisableSyncMessages.AutoSize = True
        Me.chkDisableSyncMessages.Location = New System.Drawing.Point(5, 19)
        Me.chkDisableSyncMessages.Name = "chkDisableSyncMessages"
        Me.chkDisableSyncMessages.Size = New System.Drawing.Size(166, 17)
        Me.chkDisableSyncMessages.TabIndex = 0
        Me.chkDisableSyncMessages.Text = "Disable sync event messages"
        Me.chkDisableSyncMessages.UseVisualStyleBackColor = True
        '
        'chkAutoSaveLog
        '
        Me.chkAutoSaveLog.AutoSize = True
        Me.chkAutoSaveLog.Location = New System.Drawing.Point(5, 42)
        Me.chkAutoSaveLog.Name = "chkAutoSaveLog"
        Me.chkAutoSaveLog.Size = New System.Drawing.Size(231, 17)
        Me.chkAutoSaveLog.TabIndex = 1
        Me.chkAutoSaveLog.Text = "Autosave log when max length is exceeded"
        Me.chkAutoSaveLog.UseVisualStyleBackColor = True
        '
        'grpGameManagerOptions
        '
        Me.grpGameManagerOptions.Controls.Add(Me.btnOptionalFields)
        Me.grpGameManagerOptions.Controls.Add(Me.chkShowResolvedPaths)
        Me.grpGameManagerOptions.Location = New System.Drawing.Point(6, 152)
        Me.grpGameManagerOptions.Name = "grpGameManagerOptions"
        Me.grpGameManagerOptions.Size = New System.Drawing.Size(356, 73)
        Me.grpGameManagerOptions.TabIndex = 2
        Me.grpGameManagerOptions.TabStop = False
        Me.grpGameManagerOptions.Text = "Game Manager Options"
        '
        'btnOptionalFields
        '
        Me.btnOptionalFields.Location = New System.Drawing.Point(6, 42)
        Me.btnOptionalFields.Name = "btnOptionalFields"
        Me.btnOptionalFields.Size = New System.Drawing.Size(216, 23)
        Me.btnOptionalFields.TabIndex = 1
        Me.btnOptionalFields.Text = "Choose &Optional Sync Fields..."
        Me.btnOptionalFields.UseVisualStyleBackColor = True
        '
        'chkShowResolvedPaths
        '
        Me.chkShowResolvedPaths.AutoSize = True
        Me.chkShowResolvedPaths.Location = New System.Drawing.Point(6, 19)
        Me.chkShowResolvedPaths.Name = "chkShowResolvedPaths"
        Me.chkShowResolvedPaths.Size = New System.Drawing.Size(125, 17)
        Me.chkShowResolvedPaths.TabIndex = 0
        Me.chkShowResolvedPaths.Text = "Show resolved paths"
        Me.chkShowResolvedPaths.UseVisualStyleBackColor = True
        '
        'grpMainWindowOptions
        '
        Me.grpMainWindowOptions.Controls.Add(Me.chkHideLog)
        Me.grpMainWindowOptions.Controls.Add(Me.chkHideButtons)
        Me.grpMainWindowOptions.Controls.Add(Me.chkHideGameList)
        Me.grpMainWindowOptions.Controls.Add(Me.chkExitNoWarning)
        Me.grpMainWindowOptions.Controls.Add(Me.chkExitOnClose)
        Me.grpMainWindowOptions.Location = New System.Drawing.Point(6, 12)
        Me.grpMainWindowOptions.Name = "grpMainWindowOptions"
        Me.grpMainWindowOptions.Size = New System.Drawing.Size(356, 134)
        Me.grpMainWindowOptions.TabIndex = 1
        Me.grpMainWindowOptions.TabStop = False
        Me.grpMainWindowOptions.Text = "Main Window Options"
        '
        'chkHideLog
        '
        Me.chkHideLog.AutoSize = True
        Me.chkHideLog.Location = New System.Drawing.Point(6, 65)
        Me.chkHideLog.Name = "chkHideLog"
        Me.chkHideLog.Size = New System.Drawing.Size(114, 17)
        Me.chkHideLog.TabIndex = 2
        Me.chkHideLog.Text = "Hide log by default"
        Me.chkHideLog.UseVisualStyleBackColor = True
        '
        'chkHideButtons
        '
        Me.chkHideButtons.AutoSize = True
        Me.chkHideButtons.Location = New System.Drawing.Point(6, 111)
        Me.chkHideButtons.Name = "chkHideButtons"
        Me.chkHideButtons.Size = New System.Drawing.Size(86, 17)
        Me.chkHideButtons.TabIndex = 4
        Me.chkHideButtons.Text = "Hide buttons"
        Me.chkHideButtons.UseVisualStyleBackColor = True
        '
        'chkHideGameList
        '
        Me.chkHideGameList.AutoSize = True
        Me.chkHideGameList.Location = New System.Drawing.Point(6, 88)
        Me.chkHideGameList.Name = "chkHideGameList"
        Me.chkHideGameList.Size = New System.Drawing.Size(141, 17)
        Me.chkHideGameList.TabIndex = 3
        Me.chkHideGameList.Text = "Hide game list by default"
        Me.chkHideGameList.UseVisualStyleBackColor = True
        '
        'chkExitNoWarning
        '
        Me.chkExitNoWarning.AutoSize = True
        Me.chkExitNoWarning.Location = New System.Drawing.Point(6, 42)
        Me.chkExitNoWarning.Name = "chkExitNoWarning"
        Me.chkExitNoWarning.Size = New System.Drawing.Size(140, 17)
        Me.chkExitNoWarning.TabIndex = 1
        Me.chkExitNoWarning.Text = "Exit without confirmation"
        Me.chkExitNoWarning.UseVisualStyleBackColor = True
        '
        'chkExitOnClose
        '
        Me.chkExitOnClose.AutoSize = True
        Me.chkExitOnClose.Location = New System.Drawing.Point(6, 19)
        Me.chkExitOnClose.Name = "chkExitOnClose"
        Me.chkExitOnClose.Size = New System.Drawing.Size(147, 17)
        Me.chkExitOnClose.TabIndex = 0
        Me.chkExitOnClose.Text = "Exit when closing window"
        Me.chkExitOnClose.UseVisualStyleBackColor = True
        '
        'lstSettings
        '
        Me.lstSettings.FormattingEnabled = True
        Me.lstSettings.IntegralHeight = False
        Me.lstSettings.Location = New System.Drawing.Point(12, 12)
        Me.lstSettings.Name = "lstSettings"
        Me.lstSettings.Size = New System.Drawing.Size(162, 311)
        Me.lstSettings.TabIndex = 0
        '
        'btnResetMessages
        '
        Me.btnResetMessages.Image = Global.GBM.My.Resources.Resources.Multi_Reset
        Me.btnResetMessages.Location = New System.Drawing.Point(78, 329)
        Me.btnResetMessages.Name = "btnResetMessages"
        Me.btnResetMessages.Size = New System.Drawing.Size(96, 45)
        Me.btnResetMessages.TabIndex = 3
        Me.btnResetMessages.Text = "&Reset Warnings"
        Me.btnResetMessages.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnResetMessages.UseVisualStyleBackColor = True
        '
        'pnlStartup
        '
        Me.pnlStartup.Controls.Add(Me.grpStartup)
        Me.pnlStartup.Location = New System.Drawing.Point(180, 0)
        Me.pnlStartup.Name = "pnlStartup"
        Me.pnlStartup.Size = New System.Drawing.Size(367, 323)
        Me.pnlStartup.TabIndex = 1
        '
        'grpStartup
        '
        Me.grpStartup.Controls.Add(Me.chkBackupOnLaunch)
        Me.grpStartup.Controls.Add(Me.chkAutoStart)
        Me.grpStartup.Controls.Add(Me.chkStartMinimized)
        Me.grpStartup.Controls.Add(Me.chkMonitorOnStartup)
        Me.grpStartup.Location = New System.Drawing.Point(6, 12)
        Me.grpStartup.Name = "grpStartup"
        Me.grpStartup.Size = New System.Drawing.Size(354, 113)
        Me.grpStartup.TabIndex = 0
        Me.grpStartup.TabStop = False
        Me.grpStartup.Text = "Startup Options"
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
        'chkAutoStart
        '
        Me.chkAutoStart.AutoSize = True
        Me.chkAutoStart.Location = New System.Drawing.Point(6, 19)
        Me.chkAutoStart.Name = "chkAutoStart"
        Me.chkAutoStart.Size = New System.Drawing.Size(155, 17)
        Me.chkAutoStart.TabIndex = 0
        Me.chkAutoStart.Text = "Start automatically on log-in"
        Me.chkAutoStart.UseVisualStyleBackColor = True
        '
        'chkStartMinimized
        '
        Me.chkStartMinimized.AutoSize = True
        Me.chkStartMinimized.Location = New System.Drawing.Point(6, 42)
        Me.chkStartMinimized.Name = "chkStartMinimized"
        Me.chkStartMinimized.Size = New System.Drawing.Size(96, 17)
        Me.chkStartMinimized.TabIndex = 1
        Me.chkStartMinimized.Text = "Start minimized"
        Me.chkStartMinimized.UseVisualStyleBackColor = True
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
        'pnlFilesAndFolders
        '
        Me.pnlFilesAndFolders.Controls.Add(Me.grpFolderOptions)
        Me.pnlFilesAndFolders.Location = New System.Drawing.Point(180, 0)
        Me.pnlFilesAndFolders.Name = "pnlFilesAndFolders"
        Me.pnlFilesAndFolders.Size = New System.Drawing.Size(367, 323)
        Me.pnlFilesAndFolders.TabIndex = 1
        '
        'grpFolderOptions
        '
        Me.grpFolderOptions.Controls.Add(Me.chkDeleteToRecycleBin)
        Me.grpFolderOptions.Controls.Add(Me.btnTempFolder)
        Me.grpFolderOptions.Controls.Add(Me.lblTempFolder)
        Me.grpFolderOptions.Controls.Add(Me.txtTempFolder)
        Me.grpFolderOptions.Controls.Add(Me.btnBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.lblBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.txtBackupFolder)
        Me.grpFolderOptions.Controls.Add(Me.chkCreateFolder)
        Me.grpFolderOptions.Location = New System.Drawing.Point(6, 12)
        Me.grpFolderOptions.Name = "grpFolderOptions"
        Me.grpFolderOptions.Size = New System.Drawing.Size(354, 113)
        Me.grpFolderOptions.TabIndex = 0
        Me.grpFolderOptions.TabStop = False
        Me.grpFolderOptions.Text = "Files and Folders"
        '
        'chkDeleteToRecycleBin
        '
        Me.chkDeleteToRecycleBin.AutoSize = True
        Me.chkDeleteToRecycleBin.Location = New System.Drawing.Point(6, 88)
        Me.chkDeleteToRecycleBin.Name = "chkDeleteToRecycleBin"
        Me.chkDeleteToRecycleBin.Size = New System.Drawing.Size(168, 17)
        Me.chkDeleteToRecycleBin.TabIndex = 5
        Me.chkDeleteToRecycleBin.Text = "Delete files to the Recycle Bin"
        Me.chkDeleteToRecycleBin.UseVisualStyleBackColor = True
        '
        'btnTempFolder
        '
        Me.btnTempFolder.Location = New System.Drawing.Point(313, 40)
        Me.btnTempFolder.Name = "btnTempFolder"
        Me.btnTempFolder.Size = New System.Drawing.Size(27, 20)
        Me.btnTempFolder.TabIndex = 3
        Me.btnTempFolder.Text = "..."
        Me.btnTempFolder.UseVisualStyleBackColor = True
        '
        'lblTempFolder
        '
        Me.lblTempFolder.AutoSize = True
        Me.lblTempFolder.Location = New System.Drawing.Point(6, 43)
        Me.lblTempFolder.Name = "lblTempFolder"
        Me.lblTempFolder.Size = New System.Drawing.Size(69, 13)
        Me.lblTempFolder.TabIndex = 0
        Me.lblTempFolder.Text = "Temp Folder:"
        '
        'txtTempFolder
        '
        Me.txtTempFolder.Location = New System.Drawing.Point(91, 40)
        Me.txtTempFolder.Name = "txtTempFolder"
        Me.txtTempFolder.Size = New System.Drawing.Size(216, 20)
        Me.txtTempFolder.TabIndex = 2
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
        Me.chkCreateFolder.Location = New System.Drawing.Point(6, 66)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(186, 17)
        Me.chkCreateFolder.TabIndex = 4
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'pnlGlobalHotKeys
        '
        Me.pnlGlobalHotKeys.Controls.Add(Me.grpHotKeyGeneral)
        Me.pnlGlobalHotKeys.Controls.Add(Me.grpHotKeyBindings)
        Me.pnlGlobalHotKeys.Location = New System.Drawing.Point(180, 0)
        Me.pnlGlobalHotKeys.Name = "pnlGlobalHotKeys"
        Me.pnlGlobalHotKeys.Size = New System.Drawing.Size(367, 323)
        Me.pnlGlobalHotKeys.TabIndex = 1
        '
        'grpHotKeyGeneral
        '
        Me.grpHotKeyGeneral.Controls.Add(Me.chkEnableHotKeys)
        Me.grpHotKeyGeneral.Location = New System.Drawing.Point(6, 12)
        Me.grpHotKeyGeneral.Name = "grpHotKeyGeneral"
        Me.grpHotKeyGeneral.Size = New System.Drawing.Size(348, 50)
        Me.grpHotKeyGeneral.TabIndex = 0
        Me.grpHotKeyGeneral.TabStop = False
        Me.grpHotKeyGeneral.Text = "General"
        '
        'chkEnableHotKeys
        '
        Me.chkEnableHotKeys.AutoSize = True
        Me.chkEnableHotKeys.Location = New System.Drawing.Point(6, 20)
        Me.chkEnableHotKeys.Name = "chkEnableHotKeys"
        Me.chkEnableHotKeys.Size = New System.Drawing.Size(134, 17)
        Me.chkEnableHotKeys.TabIndex = 0
        Me.chkEnableHotKeys.Text = "Enable Global Hotkeys"
        Me.chkEnableHotKeys.UseVisualStyleBackColor = True
        '
        'grpHotKeyBindings
        '
        Me.grpHotKeyBindings.Controls.Add(Me.btnResetRestoreBind)
        Me.grpHotKeyBindings.Controls.Add(Me.txtRestoreBind)
        Me.grpHotKeyBindings.Controls.Add(Me.lblRestoreBind)
        Me.grpHotKeyBindings.Controls.Add(Me.btnResetBackupBind)
        Me.grpHotKeyBindings.Controls.Add(Me.txtBackupBind)
        Me.grpHotKeyBindings.Controls.Add(Me.lblBackupBind)
        Me.grpHotKeyBindings.Location = New System.Drawing.Point(6, 65)
        Me.grpHotKeyBindings.Name = "grpHotKeyBindings"
        Me.grpHotKeyBindings.Size = New System.Drawing.Size(348, 75)
        Me.grpHotKeyBindings.TabIndex = 1
        Me.grpHotKeyBindings.TabStop = False
        Me.grpHotKeyBindings.Text = "Key Bindings"
        '
        'btnResetRestoreBind
        '
        Me.btnResetRestoreBind.Image = Global.GBM.My.Resources.Resources.Multi_Reset
        Me.btnResetRestoreBind.Location = New System.Drawing.Point(320, 42)
        Me.btnResetRestoreBind.Name = "btnResetRestoreBind"
        Me.btnResetRestoreBind.Size = New System.Drawing.Size(22, 22)
        Me.btnResetRestoreBind.TabIndex = 5
        Me.btnResetRestoreBind.UseVisualStyleBackColor = True
        '
        'txtRestoreBind
        '
        Me.txtRestoreBind.Location = New System.Drawing.Point(56, 44)
        Me.txtRestoreBind.Name = "txtRestoreBind"
        Me.txtRestoreBind.Size = New System.Drawing.Size(258, 20)
        Me.txtRestoreBind.TabIndex = 4
        '
        'lblRestoreBind
        '
        Me.lblRestoreBind.AutoSize = True
        Me.lblRestoreBind.Location = New System.Drawing.Point(3, 47)
        Me.lblRestoreBind.Name = "lblRestoreBind"
        Me.lblRestoreBind.Size = New System.Drawing.Size(47, 13)
        Me.lblRestoreBind.TabIndex = 3
        Me.lblRestoreBind.Text = "Restore:"
        '
        'btnResetBackupBind
        '
        Me.btnResetBackupBind.Image = Global.GBM.My.Resources.Resources.Multi_Reset
        Me.btnResetBackupBind.Location = New System.Drawing.Point(320, 16)
        Me.btnResetBackupBind.Name = "btnResetBackupBind"
        Me.btnResetBackupBind.Size = New System.Drawing.Size(22, 22)
        Me.btnResetBackupBind.TabIndex = 2
        Me.btnResetBackupBind.UseVisualStyleBackColor = True
        '
        'txtBackupBind
        '
        Me.txtBackupBind.Location = New System.Drawing.Point(56, 18)
        Me.txtBackupBind.Name = "txtBackupBind"
        Me.txtBackupBind.Size = New System.Drawing.Size(258, 20)
        Me.txtBackupBind.TabIndex = 1
        '
        'lblBackupBind
        '
        Me.lblBackupBind.AutoSize = True
        Me.lblBackupBind.Location = New System.Drawing.Point(3, 21)
        Me.lblBackupBind.Name = "lblBackupBind"
        Me.lblBackupBind.Size = New System.Drawing.Size(47, 13)
        Me.lblBackupBind.TabIndex = 0
        Me.lblBackupBind.Text = "Backup:"
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 386)
        Me.Controls.Add(Me.pnlBackup)
        Me.Controls.Add(Me.pnlGlobalHotKeys)
        Me.Controls.Add(Me.pnl7z)
        Me.Controls.Add(Me.pnlInterface)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnlFilesAndFolders)
        Me.Controls.Add(Me.pnlStartup)
        Me.Controls.Add(Me.btnResetMessages)
        Me.Controls.Add(Me.lstSettings)
        Me.Controls.Add(Me.btnDefaults)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.grp7zGeneral.ResumeLayout(False)
        Me.grp7zGeneral.PerformLayout()
        Me.pnlBackup.ResumeLayout(False)
        Me.grpBackupExperimental.ResumeLayout(False)
        Me.grpBackupExperimental.PerformLayout()
        Me.grpBackupConfirmations.ResumeLayout(False)
        Me.grpBackupConfirmations.PerformLayout()
        Me.grpBackupHandling.ResumeLayout(False)
        Me.grpBackupHandling.PerformLayout()
        Me.pnl7z.ResumeLayout(False)
        Me.grp7zAdvanced.ResumeLayout(False)
        Me.grp7zAdvanced.PerformLayout()
        Me.grp7zInformation.ResumeLayout(False)
        Me.pnlGeneral.ResumeLayout(False)
        Me.grpGameMonitoringOptions.ResumeLayout(False)
        Me.grpGameMonitoringOptions.PerformLayout()
        CType(Me.nudSuppressBackupThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpOptionalFeeatures.ResumeLayout(False)
        Me.grpOptionalFeeatures.PerformLayout()
        Me.pnlInterface.ResumeLayout(False)
        Me.grpLogOptions.ResumeLayout(False)
        Me.grpLogOptions.PerformLayout()
        Me.grpGameManagerOptions.ResumeLayout(False)
        Me.grpGameManagerOptions.PerformLayout()
        Me.grpMainWindowOptions.ResumeLayout(False)
        Me.grpMainWindowOptions.PerformLayout()
        Me.pnlStartup.ResumeLayout(False)
        Me.grpStartup.ResumeLayout(False)
        Me.grpStartup.PerformLayout()
        Me.pnlFilesAndFolders.ResumeLayout(False)
        Me.grpFolderOptions.ResumeLayout(False)
        Me.grpFolderOptions.PerformLayout()
        Me.pnlGlobalHotKeys.ResumeLayout(False)
        Me.grpHotKeyGeneral.ResumeLayout(False)
        Me.grpHotKeyGeneral.PerformLayout()
        Me.grpHotKeyBindings.ResumeLayout(False)
        Me.grpHotKeyBindings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkShowDetectionTips As System.Windows.Forms.CheckBox
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
    Friend WithEvents pnlBackup As Panel
    Friend WithEvents pnl7z As Panel
    Friend WithEvents pnlGeneral As Panel
    Friend WithEvents grpOptionalFeeatures As GroupBox
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
    Friend WithEvents nudSuppressBackupThreshold As NumericUpDown
    Friend WithEvents chkSuppressBackup As CheckBox
    Friend WithEvents btnResetMessages As Button
    Friend WithEvents grpBackupConfirmations As GroupBox
    Friend WithEvents pnlStartup As Panel
    Friend WithEvents grpStartup As GroupBox
    Friend WithEvents chkBackupOnLaunch As CheckBox
    Friend WithEvents chkAutoStart As CheckBox
    Friend WithEvents chkStartMinimized As CheckBox
    Friend WithEvents chkMonitorOnStartup As CheckBox
    Friend WithEvents grpGameMonitoringOptions As GroupBox
    Friend WithEvents chkDisableDiskSpaceCheck As CheckBox
    Friend WithEvents chkEnableLauncher As CheckBox
    Friend WithEvents chkBackupNotification As CheckBox
    Friend WithEvents pnlInterface As Panel
    Friend WithEvents grpMainWindowOptions As GroupBox
    Friend WithEvents chkHideLog As CheckBox
    Friend WithEvents chkHideButtons As CheckBox
    Friend WithEvents chkHideGameList As CheckBox
    Friend WithEvents chkExitNoWarning As CheckBox
    Friend WithEvents chkExitOnClose As CheckBox
    Friend WithEvents grpGameManagerOptions As GroupBox
    Friend WithEvents chkShowResolvedPaths As CheckBox
    Friend WithEvents pnlFilesAndFolders As Panel
    Friend WithEvents grpFolderOptions As GroupBox
    Friend WithEvents btnTempFolder As Button
    Friend WithEvents lblTempFolder As Label
    Friend WithEvents txtTempFolder As TextBox
    Friend WithEvents btnBackupFolder As Button
    Friend WithEvents lblBackupFolder As Label
    Friend WithEvents txtBackupFolder As TextBox
    Friend WithEvents chkCreateFolder As CheckBox
    Friend WithEvents btnOptionalFields As Button
    Friend WithEvents cboDetectSpeed As ComboBox
    Friend WithEvents lblDetectSpeed As Label
    Friend WithEvents chkTwoPassDetection As CheckBox
    Friend WithEvents grpLogOptions As GroupBox
    Friend WithEvents chkDisableSyncMessages As CheckBox
    Friend WithEvents chkAutoSaveLog As CheckBox
    Friend WithEvents chkStorePathAutoConfig As CheckBox
    Friend WithEvents chkDeleteToRecycleBin As CheckBox
    Friend WithEvents pnlGlobalHotKeys As Panel
    Friend WithEvents grpHotKeyBindings As GroupBox
    Friend WithEvents grpHotKeyGeneral As GroupBox
    Friend WithEvents chkEnableHotKeys As CheckBox
    Friend WithEvents btnResetBackupBind As Button
    Friend WithEvents txtBackupBind As TextBox
    Friend WithEvents lblBackupBind As Label
    Friend WithEvents btnResetRestoreBind As Button
    Friend WithEvents txtRestoreBind As TextBox
    Friend WithEvents lblRestoreBind As Label
    Friend WithEvents grpBackupExperimental As GroupBox
    Friend WithEvents chkEnableLiveBackup As CheckBox
End Class
