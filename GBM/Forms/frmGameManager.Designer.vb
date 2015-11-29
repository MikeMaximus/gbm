<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGameManager
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
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grpConfig = New System.Windows.Forms.GroupBox()
        Me.btnExclude = New System.Windows.Forms.Button()
        Me.btnInclude = New System.Windows.Forms.Button()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.lblExclude = New System.Windows.Forms.Label()
        Me.lblFileType = New System.Windows.Forms.Label()
        Me.btnSavePathBrowse = New System.Windows.Forms.Button()
        Me.btnProcessBrowse = New System.Windows.Forms.Button()
        Me.lblSavePath = New System.Windows.Forms.Label()
        Me.lblProcess = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtExclude = New System.Windows.Forms.TextBox()
        Me.txtFileType = New System.Windows.Forms.TextBox()
        Me.chkTimeStamp = New System.Windows.Forms.CheckBox()
        Me.chkFolderSave = New System.Windows.Forms.CheckBox()
        Me.txtSavePath = New System.Windows.Forms.TextBox()
        Me.txtProcess = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.chkMonitorOnly = New System.Windows.Forms.CheckBox()
        Me.grpExtra = New System.Windows.Forms.GroupBox()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.btnIconBrowse = New System.Windows.Forms.Button()
        Me.txtIcon = New System.Windows.Forms.TextBox()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.txtVersion = New System.Windows.Forms.TextBox()
        Me.txtCompany = New System.Windows.Forms.TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAppPathBrowse = New System.Windows.Forms.Button()
        Me.lblGamePath = New System.Windows.Forms.Label()
        Me.txtAppPath = New System.Windows.Forms.TextBox()
        Me.nudHours = New System.Windows.Forms.NumericUpDown()
        Me.lblHours = New System.Windows.Forms.Label()
        Me.btnTags = New System.Windows.Forms.Button()
        Me.grpStats = New System.Windows.Forms.GroupBox()
        Me.btnOpenRestorePath = New System.Windows.Forms.Button()
        Me.btnOpenBackupFile = New System.Windows.Forms.Button()
        Me.txtFileSize = New System.Windows.Forms.TextBox()
        Me.btnDeleteBackup = New System.Windows.Forms.Button()
        Me.lblFileSize = New System.Windows.Forms.Label()
        Me.lblSync = New System.Windows.Forms.Label()
        Me.txtCurrentBackup = New System.Windows.Forms.TextBox()
        Me.lblCurrentBackup = New System.Windows.Forms.Label()
        Me.txtLocalBackup = New System.Windows.Forms.TextBox()
        Me.lblLastBackup = New System.Windows.Forms.Label()
        Me.btnMarkAsRestored = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lstGames = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkEnabled = New System.Windows.Forms.CheckBox()
        Me.grpFilter = New System.Windows.Forms.GroupBox()
        Me.optTag = New System.Windows.Forms.RadioButton()
        Me.optBackupData = New System.Windows.Forms.RadioButton()
        Me.optPendingRestores = New System.Windows.Forms.RadioButton()
        Me.optAllGames = New System.Windows.Forms.RadioButton()
        Me.grpConfig.SuspendLayout()
        Me.grpExtra.SuspendLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpStats.SuspendLayout()
        Me.grpFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(12, 527)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "+"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(48, 527)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(30, 23)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "-"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnBackup
        '
        Me.btnBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBackup.Location = New System.Drawing.Point(616, 526)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(75, 23)
        Me.btnBackup.TabIndex = 14
        Me.btnBackup.Text = "&Backup"
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(697, 526)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 15
        Me.btnClose.Text = "C&lose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'grpConfig
        '
        Me.grpConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpConfig.Controls.Add(Me.btnExclude)
        Me.grpConfig.Controls.Add(Me.btnInclude)
        Me.grpConfig.Controls.Add(Me.txtID)
        Me.grpConfig.Controls.Add(Me.lblExclude)
        Me.grpConfig.Controls.Add(Me.lblFileType)
        Me.grpConfig.Controls.Add(Me.btnSavePathBrowse)
        Me.grpConfig.Controls.Add(Me.btnProcessBrowse)
        Me.grpConfig.Controls.Add(Me.lblSavePath)
        Me.grpConfig.Controls.Add(Me.lblProcess)
        Me.grpConfig.Controls.Add(Me.lblName)
        Me.grpConfig.Controls.Add(Me.txtExclude)
        Me.grpConfig.Controls.Add(Me.txtFileType)
        Me.grpConfig.Controls.Add(Me.chkTimeStamp)
        Me.grpConfig.Controls.Add(Me.chkFolderSave)
        Me.grpConfig.Controls.Add(Me.txtSavePath)
        Me.grpConfig.Controls.Add(Me.txtProcess)
        Me.grpConfig.Controls.Add(Me.txtName)
        Me.grpConfig.Enabled = False
        Me.grpConfig.Location = New System.Drawing.Point(238, 12)
        Me.grpConfig.Name = "grpConfig"
        Me.grpConfig.Size = New System.Drawing.Size(534, 182)
        Me.grpConfig.TabIndex = 4
        Me.grpConfig.TabStop = False
        Me.grpConfig.Text = "Configuration"
        '
        'btnExclude
        '
        Me.btnExclude.Location = New System.Drawing.Point(69, 122)
        Me.btnExclude.Name = "btnExclude"
        Me.btnExclude.Size = New System.Drawing.Size(30, 20)
        Me.btnExclude.TabIndex = 13
        Me.btnExclude.Text = "..."
        Me.btnExclude.UseVisualStyleBackColor = True
        '
        'btnInclude
        '
        Me.btnInclude.Location = New System.Drawing.Point(69, 97)
        Me.btnInclude.Name = "btnInclude"
        Me.btnInclude.Size = New System.Drawing.Size(30, 20)
        Me.btnInclude.TabIndex = 11
        Me.btnInclude.Text = "..."
        Me.btnInclude.UseVisualStyleBackColor = True
        '
        'txtID
        '
        Me.txtID.Enabled = False
        Me.txtID.Location = New System.Drawing.Point(495, 147)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(33, 20)
        Me.txtID.TabIndex = 16
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'lblExclude
        '
        Me.lblExclude.AutoSize = True
        Me.lblExclude.Location = New System.Drawing.Point(6, 126)
        Me.lblExclude.Name = "lblExclude"
        Me.lblExclude.Size = New System.Drawing.Size(48, 13)
        Me.lblExclude.TabIndex = 4
        Me.lblExclude.Text = "Exclude:"
        '
        'lblFileType
        '
        Me.lblFileType.AutoSize = True
        Me.lblFileType.Location = New System.Drawing.Point(6, 100)
        Me.lblFileType.Name = "lblFileType"
        Me.lblFileType.Size = New System.Drawing.Size(45, 13)
        Me.lblFileType.TabIndex = 3
        Me.lblFileType.Text = "Include:"
        '
        'btnSavePathBrowse
        '
        Me.btnSavePathBrowse.Location = New System.Drawing.Point(498, 71)
        Me.btnSavePathBrowse.Name = "btnSavePathBrowse"
        Me.btnSavePathBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnSavePathBrowse.TabIndex = 9
        Me.btnSavePathBrowse.Text = "..."
        Me.btnSavePathBrowse.UseVisualStyleBackColor = True
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(498, 45)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnProcessBrowse.TabIndex = 7
        Me.btnProcessBrowse.Text = "..."
        Me.btnProcessBrowse.UseVisualStyleBackColor = True
        '
        'lblSavePath
        '
        Me.lblSavePath.AutoSize = True
        Me.lblSavePath.Location = New System.Drawing.Point(6, 74)
        Me.lblSavePath.Name = "lblSavePath"
        Me.lblSavePath.Size = New System.Drawing.Size(60, 13)
        Me.lblSavePath.TabIndex = 2
        Me.lblSavePath.Text = "Save Path:"
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(6, 48)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(48, 13)
        Me.lblProcess.TabIndex = 1
        Me.lblProcess.Text = "Process:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(6, 22)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Name:"
        '
        'txtExclude
        '
        Me.txtExclude.Location = New System.Drawing.Point(105, 123)
        Me.txtExclude.Name = "txtExclude"
        Me.txtExclude.Size = New System.Drawing.Size(423, 20)
        Me.txtExclude.TabIndex = 12
        Me.txtExclude.Visible = False
        '
        'txtFileType
        '
        Me.txtFileType.Location = New System.Drawing.Point(105, 97)
        Me.txtFileType.Name = "txtFileType"
        Me.txtFileType.Size = New System.Drawing.Size(423, 20)
        Me.txtFileType.TabIndex = 10
        Me.txtFileType.Visible = False
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(124, 149)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(146, 17)
        Me.chkTimeStamp.TabIndex = 15
        Me.chkTimeStamp.Text = "Time stamp each backup"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'chkFolderSave
        '
        Me.chkFolderSave.AutoSize = True
        Me.chkFolderSave.Location = New System.Drawing.Point(9, 149)
        Me.chkFolderSave.Name = "chkFolderSave"
        Me.chkFolderSave.Size = New System.Drawing.Size(109, 17)
        Me.chkFolderSave.TabIndex = 14
        Me.chkFolderSave.Text = "Save entire folder"
        Me.chkFolderSave.UseVisualStyleBackColor = True
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(69, 71)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.Size = New System.Drawing.Size(423, 20)
        Me.txtSavePath.TabIndex = 8
        '
        'txtProcess
        '
        Me.txtProcess.Location = New System.Drawing.Point(69, 45)
        Me.txtProcess.Name = "txtProcess"
        Me.txtProcess.Size = New System.Drawing.Size(423, 20)
        Me.txtProcess.TabIndex = 6
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(69, 19)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(459, 20)
        Me.txtName.TabIndex = 5
        '
        'chkMonitorOnly
        '
        Me.chkMonitorOnly.AutoSize = True
        Me.chkMonitorOnly.Location = New System.Drawing.Point(353, 365)
        Me.chkMonitorOnly.Name = "chkMonitorOnly"
        Me.chkMonitorOnly.Size = New System.Drawing.Size(145, 17)
        Me.chkMonitorOnly.TabIndex = 7
        Me.chkMonitorOnly.Text = "Monitor only (No backup)"
        Me.chkMonitorOnly.UseVisualStyleBackColor = True
        '
        'grpExtra
        '
        Me.grpExtra.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpExtra.Controls.Add(Me.lblTags)
        Me.grpExtra.Controls.Add(Me.btnIconBrowse)
        Me.grpExtra.Controls.Add(Me.txtIcon)
        Me.grpExtra.Controls.Add(Me.lblVersion)
        Me.grpExtra.Controls.Add(Me.txtVersion)
        Me.grpExtra.Controls.Add(Me.txtCompany)
        Me.grpExtra.Controls.Add(Me.lblCompany)
        Me.grpExtra.Controls.Add(Me.pbIcon)
        Me.grpExtra.Controls.Add(Me.Label1)
        Me.grpExtra.Controls.Add(Me.btnAppPathBrowse)
        Me.grpExtra.Controls.Add(Me.lblGamePath)
        Me.grpExtra.Controls.Add(Me.txtAppPath)
        Me.grpExtra.Controls.Add(Me.nudHours)
        Me.grpExtra.Controls.Add(Me.lblHours)
        Me.grpExtra.Location = New System.Drawing.Point(238, 200)
        Me.grpExtra.Name = "grpExtra"
        Me.grpExtra.Size = New System.Drawing.Size(534, 155)
        Me.grpExtra.TabIndex = 5
        Me.grpExtra.TabStop = False
        Me.grpExtra.Text = "Game Information"
        '
        'lblTags
        '
        Me.lblTags.AutoEllipsis = True
        Me.lblTags.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTags.Location = New System.Drawing.Point(163, 124)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(311, 20)
        Me.lblTags.TabIndex = 0
        Me.lblTags.Text = "#Tags"
        Me.lblTags.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnIconBrowse
        '
        Me.btnIconBrowse.Location = New System.Drawing.Point(444, 96)
        Me.btnIconBrowse.Name = "btnIconBrowse"
        Me.btnIconBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnIconBrowse.TabIndex = 10
        Me.btnIconBrowse.Text = "..."
        Me.btnIconBrowse.UseVisualStyleBackColor = True
        '
        'txtIcon
        '
        Me.txtIcon.Location = New System.Drawing.Point(69, 97)
        Me.txtIcon.Name = "txtIcon"
        Me.txtIcon.Size = New System.Drawing.Size(369, 20)
        Me.txtIcon.TabIndex = 9
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(6, 74)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 2
        Me.lblVersion.Text = "Version:"
        '
        'txtVersion
        '
        Me.txtVersion.Location = New System.Drawing.Point(69, 71)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.Size = New System.Drawing.Size(459, 20)
        Me.txtVersion.TabIndex = 8
        '
        'txtCompany
        '
        Me.txtCompany.Location = New System.Drawing.Point(69, 45)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(459, 20)
        Me.txtCompany.TabIndex = 7
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.Location = New System.Drawing.Point(6, 48)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(54, 13)
        Me.lblCompany.TabIndex = 1
        Me.lblCompany.Text = "Company:"
        '
        'pbIcon
        '
        Me.pbIcon.InitialImage = Nothing
        Me.pbIcon.Location = New System.Drawing.Point(480, 100)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 15
        Me.pbIcon.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Icon:"
        '
        'btnAppPathBrowse
        '
        Me.btnAppPathBrowse.Location = New System.Drawing.Point(498, 19)
        Me.btnAppPathBrowse.Name = "btnAppPathBrowse"
        Me.btnAppPathBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnAppPathBrowse.TabIndex = 6
        Me.btnAppPathBrowse.Text = "..."
        Me.btnAppPathBrowse.UseVisualStyleBackColor = True
        '
        'lblGamePath
        '
        Me.lblGamePath.AutoSize = True
        Me.lblGamePath.Location = New System.Drawing.Point(6, 23)
        Me.lblGamePath.Name = "lblGamePath"
        Me.lblGamePath.Size = New System.Drawing.Size(63, 13)
        Me.lblGamePath.TabIndex = 0
        Me.lblGamePath.Text = "Game Path:"
        '
        'txtAppPath
        '
        Me.txtAppPath.Location = New System.Drawing.Point(69, 19)
        Me.txtAppPath.Name = "txtAppPath"
        Me.txtAppPath.Size = New System.Drawing.Size(423, 20)
        Me.txtAppPath.TabIndex = 5
        '
        'nudHours
        '
        Me.nudHours.DecimalPlaces = 1
        Me.nudHours.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudHours.Location = New System.Drawing.Point(69, 124)
        Me.nudHours.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudHours.Name = "nudHours"
        Me.nudHours.Size = New System.Drawing.Size(88, 20)
        Me.nudHours.TabIndex = 12
        '
        'lblHours
        '
        Me.lblHours.AutoSize = True
        Me.lblHours.Location = New System.Drawing.Point(6, 126)
        Me.lblHours.Name = "lblHours"
        Me.lblHours.Size = New System.Drawing.Size(38, 13)
        Me.lblHours.TabIndex = 11
        Me.lblHours.Text = "Hours:"
        '
        'btnTags
        '
        Me.btnTags.Location = New System.Drawing.Point(535, 360)
        Me.btnTags.Name = "btnTags"
        Me.btnTags.Size = New System.Drawing.Size(75, 23)
        Me.btnTags.TabIndex = 8
        Me.btnTags.Text = "Tags..."
        Me.btnTags.UseVisualStyleBackColor = True
        '
        'grpStats
        '
        Me.grpStats.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpStats.Controls.Add(Me.btnOpenRestorePath)
        Me.grpStats.Controls.Add(Me.btnOpenBackupFile)
        Me.grpStats.Controls.Add(Me.txtFileSize)
        Me.grpStats.Controls.Add(Me.btnDeleteBackup)
        Me.grpStats.Controls.Add(Me.lblFileSize)
        Me.grpStats.Controls.Add(Me.lblSync)
        Me.grpStats.Controls.Add(Me.txtCurrentBackup)
        Me.grpStats.Controls.Add(Me.lblCurrentBackup)
        Me.grpStats.Controls.Add(Me.txtLocalBackup)
        Me.grpStats.Controls.Add(Me.lblLastBackup)
        Me.grpStats.Location = New System.Drawing.Point(238, 390)
        Me.grpStats.Name = "grpStats"
        Me.grpStats.Size = New System.Drawing.Size(534, 129)
        Me.grpStats.TabIndex = 11
        Me.grpStats.TabStop = False
        Me.grpStats.Text = "Backup Information"
        '
        'btnOpenRestorePath
        '
        Me.btnOpenRestorePath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenRestorePath.Location = New System.Drawing.Point(336, 100)
        Me.btnOpenRestorePath.Name = "btnOpenRestorePath"
        Me.btnOpenRestorePath.Size = New System.Drawing.Size(114, 23)
        Me.btnOpenRestorePath.TabIndex = 9
        Me.btnOpenRestorePath.Text = "O&pen Restore Path"
        Me.btnOpenRestorePath.UseVisualStyleBackColor = True
        '
        'btnOpenBackupFile
        '
        Me.btnOpenBackupFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenBackupFile.Location = New System.Drawing.Point(216, 100)
        Me.btnOpenBackupFile.Name = "btnOpenBackupFile"
        Me.btnOpenBackupFile.Size = New System.Drawing.Size(114, 23)
        Me.btnOpenBackupFile.TabIndex = 8
        Me.btnOpenBackupFile.Text = "&Open Backup File"
        Me.btnOpenBackupFile.UseVisualStyleBackColor = True
        '
        'txtFileSize
        '
        Me.txtFileSize.Location = New System.Drawing.Point(96, 74)
        Me.txtFileSize.Name = "txtFileSize"
        Me.txtFileSize.ReadOnly = True
        Me.txtFileSize.Size = New System.Drawing.Size(275, 20)
        Me.txtFileSize.TabIndex = 6
        Me.txtFileSize.TabStop = False
        '
        'btnDeleteBackup
        '
        Me.btnDeleteBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteBackup.Location = New System.Drawing.Point(96, 100)
        Me.btnDeleteBackup.Name = "btnDeleteBackup"
        Me.btnDeleteBackup.Size = New System.Drawing.Size(114, 23)
        Me.btnDeleteBackup.TabIndex = 7
        Me.btnDeleteBackup.Text = "&Delete Backup"
        Me.btnDeleteBackup.UseVisualStyleBackColor = True
        '
        'lblFileSize
        '
        Me.lblFileSize.AutoSize = True
        Me.lblFileSize.Location = New System.Drawing.Point(6, 77)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(70, 13)
        Me.lblFileSize.TabIndex = 2
        Me.lblFileSize.Text = "Backup Size:"
        '
        'lblSync
        '
        Me.lblSync.AutoSize = True
        Me.lblSync.Location = New System.Drawing.Point(6, 105)
        Me.lblSync.Name = "lblSync"
        Me.lblSync.Size = New System.Drawing.Size(62, 13)
        Me.lblSync.TabIndex = 3
        Me.lblSync.Text = "Up to Date!"
        Me.lblSync.Visible = False
        '
        'txtCurrentBackup
        '
        Me.txtCurrentBackup.Location = New System.Drawing.Point(96, 24)
        Me.txtCurrentBackup.Name = "txtCurrentBackup"
        Me.txtCurrentBackup.ReadOnly = True
        Me.txtCurrentBackup.Size = New System.Drawing.Size(275, 20)
        Me.txtCurrentBackup.TabIndex = 4
        Me.txtCurrentBackup.TabStop = False
        '
        'lblCurrentBackup
        '
        Me.lblCurrentBackup.AutoSize = True
        Me.lblCurrentBackup.Location = New System.Drawing.Point(6, 27)
        Me.lblCurrentBackup.Name = "lblCurrentBackup"
        Me.lblCurrentBackup.Size = New System.Drawing.Size(84, 13)
        Me.lblCurrentBackup.TabIndex = 0
        Me.lblCurrentBackup.Text = "Current Backup:"
        '
        'txtLocalBackup
        '
        Me.txtLocalBackup.Location = New System.Drawing.Point(96, 50)
        Me.txtLocalBackup.Name = "txtLocalBackup"
        Me.txtLocalBackup.ReadOnly = True
        Me.txtLocalBackup.Size = New System.Drawing.Size(275, 20)
        Me.txtLocalBackup.TabIndex = 5
        Me.txtLocalBackup.TabStop = False
        '
        'lblLastBackup
        '
        Me.lblLastBackup.AutoSize = True
        Me.lblLastBackup.Location = New System.Drawing.Point(6, 53)
        Me.lblLastBackup.Name = "lblLastBackup"
        Me.lblLastBackup.Size = New System.Drawing.Size(76, 13)
        Me.lblLastBackup.TabIndex = 1
        Me.lblLastBackup.Text = "Local Backup:"
        '
        'btnMarkAsRestored
        '
        Me.btnMarkAsRestored.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMarkAsRestored.Location = New System.Drawing.Point(429, 526)
        Me.btnMarkAsRestored.Name = "btnMarkAsRestored"
        Me.btnMarkAsRestored.Size = New System.Drawing.Size(100, 23)
        Me.btnMarkAsRestored.TabIndex = 12
        Me.btnMarkAsRestored.Text = "&Mark as Restored"
        Me.btnMarkAsRestored.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRestore.Location = New System.Drawing.Point(535, 526)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(75, 23)
        Me.btnRestore.TabIndex = 13
        Me.btnRestore.Text = "&Restore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(616, 360)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lstGames
        '
        Me.lstGames.FormattingEnabled = True
        Me.lstGames.Location = New System.Drawing.Point(12, 138)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstGames.Size = New System.Drawing.Size(220, 381)
        Me.lstGames.Sorted = True
        Me.lstGames.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(697, 360)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'chkEnabled
        '
        Me.chkEnabled.AutoSize = True
        Me.chkEnabled.Location = New System.Drawing.Point(238, 365)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(109, 17)
        Me.chkEnabled.TabIndex = 6
        Me.chkEnabled.Text = "Monitor this game"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'grpFilter
        '
        Me.grpFilter.Controls.Add(Me.optTag)
        Me.grpFilter.Controls.Add(Me.optBackupData)
        Me.grpFilter.Controls.Add(Me.optPendingRestores)
        Me.grpFilter.Controls.Add(Me.optAllGames)
        Me.grpFilter.Location = New System.Drawing.Point(12, 12)
        Me.grpFilter.Name = "grpFilter"
        Me.grpFilter.Size = New System.Drawing.Size(220, 113)
        Me.grpFilter.TabIndex = 0
        Me.grpFilter.TabStop = False
        Me.grpFilter.Text = "Games Filter"
        '
        'optTag
        '
        Me.optTag.AutoSize = True
        Me.optTag.Location = New System.Drawing.Point(6, 87)
        Me.optTag.Name = "optTag"
        Me.optTag.Size = New System.Drawing.Size(49, 17)
        Me.optTag.TabIndex = 3
        Me.optTag.TabStop = True
        Me.optTag.Text = "Tags"
        Me.optTag.UseVisualStyleBackColor = True
        '
        'optBackupData
        '
        Me.optBackupData.AutoSize = True
        Me.optBackupData.Location = New System.Drawing.Point(6, 41)
        Me.optBackupData.Name = "optBackupData"
        Me.optBackupData.Size = New System.Drawing.Size(91, 17)
        Me.optBackupData.TabIndex = 1
        Me.optBackupData.TabStop = True
        Me.optBackupData.Text = "Backups Only"
        Me.optBackupData.UseVisualStyleBackColor = True
        '
        'optPendingRestores
        '
        Me.optPendingRestores.AutoSize = True
        Me.optPendingRestores.Location = New System.Drawing.Point(6, 64)
        Me.optPendingRestores.Name = "optPendingRestores"
        Me.optPendingRestores.Size = New System.Drawing.Size(104, 17)
        Me.optPendingRestores.TabIndex = 2
        Me.optPendingRestores.TabStop = True
        Me.optPendingRestores.Text = "Pending Restore"
        Me.optPendingRestores.UseVisualStyleBackColor = True
        '
        'optAllGames
        '
        Me.optAllGames.AutoSize = True
        Me.optAllGames.Location = New System.Drawing.Point(6, 20)
        Me.optAllGames.Name = "optAllGames"
        Me.optAllGames.Size = New System.Drawing.Size(36, 17)
        Me.optAllGames.TabIndex = 0
        Me.optAllGames.TabStop = True
        Me.optAllGames.Text = "All"
        Me.optAllGames.UseVisualStyleBackColor = True
        '
        'frmGameManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.grpFilter)
        Me.Controls.Add(Me.btnTags)
        Me.Controls.Add(Me.chkEnabled)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chkMonitorOnly)
        Me.Controls.Add(Me.btnMarkAsRestored)
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.lstGames)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpStats)
        Me.Controls.Add(Me.grpExtra)
        Me.Controls.Add(Me.grpConfig)
        Me.Controls.Add(Me.btnBackup)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGameManager"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Manager"
        Me.grpConfig.ResumeLayout(False)
        Me.grpConfig.PerformLayout()
        Me.grpExtra.ResumeLayout(False)
        Me.grpExtra.PerformLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpStats.ResumeLayout(False)
        Me.grpStats.PerformLayout()
        Me.grpFilter.ResumeLayout(False)
        Me.grpFilter.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnBackup As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grpConfig As System.Windows.Forms.GroupBox
    Friend WithEvents txtSavePath As System.Windows.Forms.TextBox
    Friend WithEvents txtProcess As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents grpExtra As System.Windows.Forms.GroupBox
    Friend WithEvents grpStats As System.Windows.Forms.GroupBox
    Friend WithEvents chkTimeStamp As System.Windows.Forms.CheckBox
    Friend WithEvents chkFolderSave As System.Windows.Forms.CheckBox
    Friend WithEvents lblSavePath As System.Windows.Forms.Label
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtExclude As System.Windows.Forms.TextBox
    Friend WithEvents txtFileType As System.Windows.Forms.TextBox
    Friend WithEvents btnSavePathBrowse As System.Windows.Forms.Button
    Friend WithEvents btnProcessBrowse As System.Windows.Forms.Button
    Friend WithEvents lblExclude As System.Windows.Forms.Label
    Friend WithEvents lblFileType As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnAppPathBrowse As System.Windows.Forms.Button
    Friend WithEvents lblGamePath As System.Windows.Forms.Label
    Friend WithEvents txtAppPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents txtVersion As System.Windows.Forms.TextBox
    Friend WithEvents txtCompany As System.Windows.Forms.TextBox
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents nudHours As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHours As System.Windows.Forms.Label
    Friend WithEvents chkMonitorOnly As System.Windows.Forms.CheckBox
    Friend WithEvents lstGames As System.Windows.Forms.ListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnIconBrowse As System.Windows.Forms.Button
    Friend WithEvents txtIcon As System.Windows.Forms.TextBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents txtCurrentBackup As System.Windows.Forms.TextBox
    Friend WithEvents lblCurrentBackup As System.Windows.Forms.Label
    Friend WithEvents txtLocalBackup As System.Windows.Forms.TextBox
    Friend WithEvents lblLastBackup As System.Windows.Forms.Label
    Friend WithEvents lblSync As System.Windows.Forms.Label
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents txtFileSize As System.Windows.Forms.TextBox
    Friend WithEvents lblFileSize As System.Windows.Forms.Label
    Friend WithEvents btnMarkAsRestored As System.Windows.Forms.Button
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents btnDeleteBackup As System.Windows.Forms.Button
    Friend WithEvents btnOpenBackupFile As System.Windows.Forms.Button
    Friend WithEvents grpFilter As System.Windows.Forms.GroupBox
    Friend WithEvents optPendingRestores As System.Windows.Forms.RadioButton
    Friend WithEvents optAllGames As System.Windows.Forms.RadioButton
    Friend WithEvents optBackupData As System.Windows.Forms.RadioButton
    Friend WithEvents btnOpenRestorePath As System.Windows.Forms.Button
    Friend WithEvents btnTags As System.Windows.Forms.Button
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents optTag As System.Windows.Forms.RadioButton
    Friend WithEvents btnInclude As System.Windows.Forms.Button
    Friend WithEvents btnExclude As System.Windows.Forms.Button
End Class
