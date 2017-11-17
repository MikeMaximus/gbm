<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGameManager
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grpConfig = New System.Windows.Forms.GroupBox()
        Me.txtParameter = New System.Windows.Forms.TextBox()
        Me.lblParameter = New System.Windows.Forms.Label()
        Me.chkCleanFolder = New System.Windows.Forms.CheckBox()
        Me.lblLimit = New System.Windows.Forms.Label()
        Me.nudLimit = New System.Windows.Forms.NumericUpDown()
        Me.btnExclude = New System.Windows.Forms.Button()
        Me.btnInclude = New System.Windows.Forms.Button()
        Me.txtID = New System.Windows.Forms.TextBox()
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
        Me.lblIcon = New System.Windows.Forms.Label()
        Me.btnAppPathBrowse = New System.Windows.Forms.Button()
        Me.lblGamePath = New System.Windows.Forms.Label()
        Me.txtAppPath = New System.Windows.Forms.TextBox()
        Me.nudHours = New System.Windows.Forms.NumericUpDown()
        Me.lblHours = New System.Windows.Forms.Label()
        Me.btnTags = New System.Windows.Forms.Button()
        Me.grpStats = New System.Windows.Forms.GroupBox()
        Me.cboRemoteBackup = New System.Windows.Forms.ComboBox()
        Me.lblRestorePathData = New System.Windows.Forms.Label()
        Me.lblBackupFileData = New System.Windows.Forms.Label()
        Me.lblLocalBackupData = New System.Windows.Forms.Label()
        Me.lblRestorePath = New System.Windows.Forms.Label()
        Me.btnOpenRestorePath = New System.Windows.Forms.Button()
        Me.btnOpenBackupFile = New System.Windows.Forms.Button()
        Me.btnDeleteBackup = New System.Windows.Forms.Button()
        Me.lblBackupFile = New System.Windows.Forms.Label()
        Me.lblRemote = New System.Windows.Forms.Label()
        Me.lblLocalData = New System.Windows.Forms.Label()
        Me.btnMarkAsRestored = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lstGames = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkEnabled = New System.Windows.Forms.CheckBox()
        Me.grpFilter = New System.Windows.Forms.GroupBox()
        Me.optCustom = New System.Windows.Forms.RadioButton()
        Me.optBackupData = New System.Windows.Forms.RadioButton()
        Me.optPendingRestores = New System.Windows.Forms.RadioButton()
        Me.optAllGames = New System.Windows.Forms.RadioButton()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cmsImport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtQuickFilter = New System.Windows.Forms.TextBox()
        Me.lblQuickFilter = New System.Windows.Forms.Label()
        Me.cmsDeleteBackup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsDeleteOne = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsDeleteAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSessions = New System.Windows.Forms.Button()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.lblComments = New System.Windows.Forms.Label()
        Me.grpConfig.SuspendLayout()
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpExtra.SuspendLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpStats.SuspendLayout()
        Me.grpFilter.SuspendLayout()
        Me.cmsImport.SuspendLayout()
        Me.cmsDeleteBackup.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(12, 586)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 23)
        Me.btnAdd.TabIndex = 4
        Me.btnAdd.Text = "+"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(48, 586)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(30, 23)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "-"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnBackup
        '
        Me.btnBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBackup.Location = New System.Drawing.Point(616, 586)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(75, 23)
        Me.btnBackup.TabIndex = 19
        Me.btnBackup.Text = "&Backup"
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(697, 586)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 20
        Me.btnClose.Text = "C&lose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'grpConfig
        '
        Me.grpConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpConfig.Controls.Add(Me.lblComments)
        Me.grpConfig.Controls.Add(Me.txtComments)
        Me.grpConfig.Controls.Add(Me.txtParameter)
        Me.grpConfig.Controls.Add(Me.lblParameter)
        Me.grpConfig.Controls.Add(Me.chkCleanFolder)
        Me.grpConfig.Controls.Add(Me.lblLimit)
        Me.grpConfig.Controls.Add(Me.nudLimit)
        Me.grpConfig.Controls.Add(Me.btnExclude)
        Me.grpConfig.Controls.Add(Me.btnInclude)
        Me.grpConfig.Controls.Add(Me.txtID)
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
        Me.grpConfig.Location = New System.Drawing.Point(247, 12)
        Me.grpConfig.Name = "grpConfig"
        Me.grpConfig.Size = New System.Drawing.Size(525, 215)
        Me.grpConfig.TabIndex = 8
        Me.grpConfig.TabStop = False
        Me.grpConfig.Text = "Configuration"
        '
        'txtParameter
        '
        Me.txtParameter.Location = New System.Drawing.Point(333, 45)
        Me.txtParameter.Name = "txtParameter"
        Me.txtParameter.Size = New System.Drawing.Size(150, 20)
        Me.txtParameter.TabIndex = 7
        '
        'lblParameter
        '
        Me.lblParameter.AutoSize = True
        Me.lblParameter.Location = New System.Drawing.Point(269, 48)
        Me.lblParameter.Name = "lblParameter"
        Me.lblParameter.Size = New System.Drawing.Size(58, 13)
        Me.lblParameter.TabIndex = 6
        Me.lblParameter.Text = "Parameter:"
        '
        'chkCleanFolder
        '
        Me.chkCleanFolder.AutoSize = True
        Me.chkCleanFolder.Location = New System.Drawing.Point(329, 101)
        Me.chkCleanFolder.Name = "chkCleanFolder"
        Me.chkCleanFolder.Size = New System.Drawing.Size(136, 17)
        Me.chkCleanFolder.TabIndex = 13
        Me.chkCleanFolder.Text = "Delete folder on restore"
        Me.chkCleanFolder.UseVisualStyleBackColor = True
        '
        'lblLimit
        '
        Me.lblLimit.AutoSize = True
        Me.lblLimit.Location = New System.Drawing.Point(375, 130)
        Me.lblLimit.Name = "lblLimit"
        Me.lblLimit.Size = New System.Drawing.Size(68, 13)
        Me.lblLimit.TabIndex = 16
        Me.lblLimit.Text = "Backup Limit"
        Me.lblLimit.Visible = False
        '
        'nudLimit
        '
        Me.nudLimit.Location = New System.Drawing.Point(329, 128)
        Me.nudLimit.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudLimit.Name = "nudLimit"
        Me.nudLimit.Size = New System.Drawing.Size(40, 20)
        Me.nudLimit.TabIndex = 15
        Me.nudLimit.Value = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudLimit.Visible = False
        '
        'btnExclude
        '
        Me.btnExclude.Location = New System.Drawing.Point(9, 125)
        Me.btnExclude.Name = "btnExclude"
        Me.btnExclude.Size = New System.Drawing.Size(175, 23)
        Me.btnExclude.TabIndex = 11
        Me.btnExclude.Text = "E&xclude Items..."
        Me.btnExclude.UseVisualStyleBackColor = True
        '
        'btnInclude
        '
        Me.btnInclude.Location = New System.Drawing.Point(9, 97)
        Me.btnInclude.Name = "btnInclude"
        Me.btnInclude.Size = New System.Drawing.Size(175, 23)
        Me.btnInclude.TabIndex = 10
        Me.btnInclude.Text = "In&clude Items..."
        Me.btnInclude.UseVisualStyleBackColor = True
        '
        'txtID
        '
        Me.txtID.Enabled = False
        Me.txtID.Location = New System.Drawing.Point(489, 19)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(30, 20)
        Me.txtID.TabIndex = 0
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'btnSavePathBrowse
        '
        Me.btnSavePathBrowse.Location = New System.Drawing.Point(489, 71)
        Me.btnSavePathBrowse.Name = "btnSavePathBrowse"
        Me.btnSavePathBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnSavePathBrowse.TabIndex = 9
        Me.btnSavePathBrowse.Text = "..."
        Me.btnSavePathBrowse.UseVisualStyleBackColor = True
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(225, 44)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnProcessBrowse.TabIndex = 5
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
        Me.txtExclude.Location = New System.Drawing.Point(489, 122)
        Me.txtExclude.Name = "txtExclude"
        Me.txtExclude.Size = New System.Drawing.Size(30, 20)
        Me.txtExclude.TabIndex = 0
        Me.txtExclude.TabStop = False
        Me.txtExclude.Visible = False
        '
        'txtFileType
        '
        Me.txtFileType.Location = New System.Drawing.Point(489, 99)
        Me.txtFileType.Name = "txtFileType"
        Me.txtFileType.Size = New System.Drawing.Size(30, 20)
        Me.txtFileType.TabIndex = 0
        Me.txtFileType.TabStop = False
        Me.txtFileType.Visible = False
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(190, 129)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(133, 17)
        Me.chkTimeStamp.TabIndex = 14
        Me.chkTimeStamp.Text = "Save multiple backups"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'chkFolderSave
        '
        Me.chkFolderSave.AutoSize = True
        Me.chkFolderSave.Location = New System.Drawing.Point(190, 101)
        Me.chkFolderSave.Name = "chkFolderSave"
        Me.chkFolderSave.Size = New System.Drawing.Size(109, 17)
        Me.chkFolderSave.TabIndex = 12
        Me.chkFolderSave.Text = "Save entire folder"
        Me.chkFolderSave.UseVisualStyleBackColor = True
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(69, 71)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.Size = New System.Drawing.Size(414, 20)
        Me.txtSavePath.TabIndex = 8
        '
        'txtProcess
        '
        Me.txtProcess.Location = New System.Drawing.Point(69, 45)
        Me.txtProcess.Name = "txtProcess"
        Me.txtProcess.Size = New System.Drawing.Size(150, 20)
        Me.txtProcess.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(69, 19)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(414, 20)
        Me.txtName.TabIndex = 3
        '
        'chkMonitorOnly
        '
        Me.chkMonitorOnly.AutoSize = True
        Me.chkMonitorOnly.Location = New System.Drawing.Point(363, 398)
        Me.chkMonitorOnly.Name = "chkMonitorOnly"
        Me.chkMonitorOnly.Size = New System.Drawing.Size(83, 17)
        Me.chkMonitorOnly.TabIndex = 11
        Me.chkMonitorOnly.Text = "Monitor only"
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
        Me.grpExtra.Controls.Add(Me.lblIcon)
        Me.grpExtra.Controls.Add(Me.btnAppPathBrowse)
        Me.grpExtra.Controls.Add(Me.lblGamePath)
        Me.grpExtra.Controls.Add(Me.txtAppPath)
        Me.grpExtra.Controls.Add(Me.nudHours)
        Me.grpExtra.Controls.Add(Me.lblHours)
        Me.grpExtra.Location = New System.Drawing.Point(248, 233)
        Me.grpExtra.Name = "grpExtra"
        Me.grpExtra.Size = New System.Drawing.Size(525, 155)
        Me.grpExtra.TabIndex = 9
        Me.grpExtra.TabStop = False
        Me.grpExtra.Text = "Game Information"
        '
        'lblTags
        '
        Me.lblTags.AutoEllipsis = True
        Me.lblTags.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTags.Location = New System.Drawing.Point(161, 124)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(304, 20)
        Me.lblTags.TabIndex = 0
        Me.lblTags.Text = "#Tags"
        Me.lblTags.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnIconBrowse
        '
        Me.btnIconBrowse.Location = New System.Drawing.Point(435, 97)
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
        Me.txtIcon.Size = New System.Drawing.Size(360, 20)
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
        Me.txtVersion.Size = New System.Drawing.Size(414, 20)
        Me.txtVersion.TabIndex = 8
        '
        'txtCompany
        '
        Me.txtCompany.Location = New System.Drawing.Point(69, 45)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(414, 20)
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
        Me.pbIcon.Location = New System.Drawing.Point(471, 97)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 15
        Me.pbIcon.TabStop = False
        '
        'lblIcon
        '
        Me.lblIcon.AutoSize = True
        Me.lblIcon.Location = New System.Drawing.Point(6, 100)
        Me.lblIcon.Name = "lblIcon"
        Me.lblIcon.Size = New System.Drawing.Size(31, 13)
        Me.lblIcon.TabIndex = 3
        Me.lblIcon.Text = "Icon:"
        '
        'btnAppPathBrowse
        '
        Me.btnAppPathBrowse.Location = New System.Drawing.Point(489, 19)
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
        Me.txtAppPath.Size = New System.Drawing.Size(414, 20)
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
        Me.btnTags.Location = New System.Drawing.Point(535, 394)
        Me.btnTags.Name = "btnTags"
        Me.btnTags.Size = New System.Drawing.Size(75, 23)
        Me.btnTags.TabIndex = 12
        Me.btnTags.Text = "Tags..."
        Me.btnTags.UseVisualStyleBackColor = True
        '
        'grpStats
        '
        Me.grpStats.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpStats.Controls.Add(Me.cboRemoteBackup)
        Me.grpStats.Controls.Add(Me.lblRestorePathData)
        Me.grpStats.Controls.Add(Me.lblBackupFileData)
        Me.grpStats.Controls.Add(Me.lblLocalBackupData)
        Me.grpStats.Controls.Add(Me.lblRestorePath)
        Me.grpStats.Controls.Add(Me.btnOpenRestorePath)
        Me.grpStats.Controls.Add(Me.btnOpenBackupFile)
        Me.grpStats.Controls.Add(Me.btnDeleteBackup)
        Me.grpStats.Controls.Add(Me.lblBackupFile)
        Me.grpStats.Controls.Add(Me.lblRemote)
        Me.grpStats.Controls.Add(Me.lblLocalData)
        Me.grpStats.Location = New System.Drawing.Point(247, 423)
        Me.grpStats.Name = "grpStats"
        Me.grpStats.Size = New System.Drawing.Size(525, 154)
        Me.grpStats.TabIndex = 15
        Me.grpStats.TabStop = False
        Me.grpStats.Text = "Backup Information"
        '
        'cboRemoteBackup
        '
        Me.cboRemoteBackup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRemoteBackup.FormattingEnabled = True
        Me.cboRemoteBackup.Location = New System.Drawing.Point(96, 24)
        Me.cboRemoteBackup.Name = "cboRemoteBackup"
        Me.cboRemoteBackup.Size = New System.Drawing.Size(387, 21)
        Me.cboRemoteBackup.TabIndex = 12
        '
        'lblRestorePathData
        '
        Me.lblRestorePathData.AutoEllipsis = True
        Me.lblRestorePathData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRestorePathData.Location = New System.Drawing.Point(96, 98)
        Me.lblRestorePathData.Name = "lblRestorePathData"
        Me.lblRestorePathData.Size = New System.Drawing.Size(387, 20)
        Me.lblRestorePathData.TabIndex = 7
        Me.lblRestorePathData.Tag = "wipe"
        Me.lblRestorePathData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBackupFileData
        '
        Me.lblBackupFileData.AutoEllipsis = True
        Me.lblBackupFileData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBackupFileData.Location = New System.Drawing.Point(96, 73)
        Me.lblBackupFileData.Name = "lblBackupFileData"
        Me.lblBackupFileData.Size = New System.Drawing.Size(387, 20)
        Me.lblBackupFileData.TabIndex = 6
        Me.lblBackupFileData.Tag = "wipe"
        Me.lblBackupFileData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLocalBackupData
        '
        Me.lblLocalBackupData.AutoEllipsis = True
        Me.lblLocalBackupData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocalBackupData.Location = New System.Drawing.Point(96, 49)
        Me.lblLocalBackupData.Name = "lblLocalBackupData"
        Me.lblLocalBackupData.Size = New System.Drawing.Size(387, 20)
        Me.lblLocalBackupData.TabIndex = 5
        Me.lblLocalBackupData.Tag = "wipe"
        Me.lblLocalBackupData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRestorePath
        '
        Me.lblRestorePath.AutoSize = True
        Me.lblRestorePath.Location = New System.Drawing.Point(6, 102)
        Me.lblRestorePath.Name = "lblRestorePath"
        Me.lblRestorePath.Size = New System.Drawing.Size(72, 13)
        Me.lblRestorePath.TabIndex = 3
        Me.lblRestorePath.Text = "Restore Path:"
        '
        'btnOpenRestorePath
        '
        Me.btnOpenRestorePath.Location = New System.Drawing.Point(369, 125)
        Me.btnOpenRestorePath.Name = "btnOpenRestorePath"
        Me.btnOpenRestorePath.Size = New System.Drawing.Size(114, 23)
        Me.btnOpenRestorePath.TabIndex = 11
        Me.btnOpenRestorePath.Text = "O&pen Restore Path"
        Me.btnOpenRestorePath.UseVisualStyleBackColor = True
        '
        'btnOpenBackupFile
        '
        Me.btnOpenBackupFile.Location = New System.Drawing.Point(249, 125)
        Me.btnOpenBackupFile.Name = "btnOpenBackupFile"
        Me.btnOpenBackupFile.Size = New System.Drawing.Size(114, 23)
        Me.btnOpenBackupFile.TabIndex = 10
        Me.btnOpenBackupFile.Text = "&Open Backup File"
        Me.btnOpenBackupFile.UseVisualStyleBackColor = True
        '
        'btnDeleteBackup
        '
        Me.btnDeleteBackup.Location = New System.Drawing.Point(129, 125)
        Me.btnDeleteBackup.Name = "btnDeleteBackup"
        Me.btnDeleteBackup.Size = New System.Drawing.Size(114, 23)
        Me.btnDeleteBackup.TabIndex = 8
        Me.btnDeleteBackup.Text = "&Delete Backup"
        Me.btnDeleteBackup.UseVisualStyleBackColor = True
        '
        'lblBackupFile
        '
        Me.lblBackupFile.AutoSize = True
        Me.lblBackupFile.Location = New System.Drawing.Point(6, 77)
        Me.lblBackupFile.Name = "lblBackupFile"
        Me.lblBackupFile.Size = New System.Drawing.Size(66, 13)
        Me.lblBackupFile.TabIndex = 2
        Me.lblBackupFile.Text = "Backup File:"
        '
        'lblRemote
        '
        Me.lblRemote.AutoSize = True
        Me.lblRemote.Location = New System.Drawing.Point(6, 27)
        Me.lblRemote.Name = "lblRemote"
        Me.lblRemote.Size = New System.Drawing.Size(73, 13)
        Me.lblRemote.TabIndex = 0
        Me.lblRemote.Text = "Backup Data:"
        '
        'lblLocalData
        '
        Me.lblLocalData.AutoSize = True
        Me.lblLocalData.Location = New System.Drawing.Point(6, 53)
        Me.lblLocalData.Name = "lblLocalData"
        Me.lblLocalData.Size = New System.Drawing.Size(62, 13)
        Me.lblLocalData.TabIndex = 1
        Me.lblLocalData.Text = "Local Data:"
        '
        'btnMarkAsRestored
        '
        Me.btnMarkAsRestored.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMarkAsRestored.Location = New System.Drawing.Point(429, 586)
        Me.btnMarkAsRestored.Name = "btnMarkAsRestored"
        Me.btnMarkAsRestored.Size = New System.Drawing.Size(100, 23)
        Me.btnMarkAsRestored.TabIndex = 17
        Me.btnMarkAsRestored.Text = "&Mark as Restored"
        Me.btnMarkAsRestored.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRestore.Location = New System.Drawing.Point(535, 586)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(75, 23)
        Me.btnRestore.TabIndex = 18
        Me.btnRestore.Text = "&Restore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(616, 394)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 13
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lstGames
        '
        Me.lstGames.FormattingEnabled = True
        Me.lstGames.Location = New System.Drawing.Point(12, 160)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstGames.Size = New System.Drawing.Size(228, 420)
        Me.lstGames.TabIndex = 3
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(697, 392)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'chkEnabled
        '
        Me.chkEnabled.AutoSize = True
        Me.chkEnabled.Location = New System.Drawing.Point(248, 398)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(109, 17)
        Me.chkEnabled.TabIndex = 10
        Me.chkEnabled.Text = "Monitor this game"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'grpFilter
        '
        Me.grpFilter.Controls.Add(Me.optCustom)
        Me.grpFilter.Controls.Add(Me.optBackupData)
        Me.grpFilter.Controls.Add(Me.optPendingRestores)
        Me.grpFilter.Controls.Add(Me.optAllGames)
        Me.grpFilter.Location = New System.Drawing.Point(12, 12)
        Me.grpFilter.Name = "grpFilter"
        Me.grpFilter.Size = New System.Drawing.Size(228, 113)
        Me.grpFilter.TabIndex = 0
        Me.grpFilter.TabStop = False
        Me.grpFilter.Text = "Games Filter"
        '
        'optCustom
        '
        Me.optCustom.AutoSize = True
        Me.optCustom.Location = New System.Drawing.Point(6, 87)
        Me.optCustom.Name = "optCustom"
        Me.optCustom.Size = New System.Drawing.Size(60, 17)
        Me.optCustom.TabIndex = 3
        Me.optCustom.TabStop = True
        Me.optCustom.Text = "Custom"
        Me.optCustom.UseVisualStyleBackColor = True
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
        Me.optPendingRestores.Size = New System.Drawing.Size(134, 17)
        Me.optPendingRestores.TabIndex = 2
        Me.optPendingRestores.TabStop = True
        Me.optPendingRestores.Text = "New Backups Pending"
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
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(84, 586)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 6
        Me.btnImport.Text = "&Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(165, 586)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 7
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmsImport
        '
        Me.cmsImport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsOfficial, Me.cmsFile})
        Me.cmsImport.Name = "cmsImport"
        Me.cmsImport.ShowImageMargin = False
        Me.cmsImport.Size = New System.Drawing.Size(118, 48)
        '
        'cmsOfficial
        '
        Me.cmsOfficial.Name = "cmsOfficial"
        Me.cmsOfficial.Size = New System.Drawing.Size(117, 22)
        Me.cmsOfficial.Text = "&Official List..."
        '
        'cmsFile
        '
        Me.cmsFile.Name = "cmsFile"
        Me.cmsFile.Size = New System.Drawing.Size(117, 22)
        Me.cmsFile.Text = "&File..."
        '
        'txtQuickFilter
        '
        Me.txtQuickFilter.Location = New System.Drawing.Point(80, 134)
        Me.txtQuickFilter.Name = "txtQuickFilter"
        Me.txtQuickFilter.Size = New System.Drawing.Size(160, 20)
        Me.txtQuickFilter.TabIndex = 2
        '
        'lblQuickFilter
        '
        Me.lblQuickFilter.AutoSize = True
        Me.lblQuickFilter.Location = New System.Drawing.Point(12, 137)
        Me.lblQuickFilter.Name = "lblQuickFilter"
        Me.lblQuickFilter.Size = New System.Drawing.Size(63, 13)
        Me.lblQuickFilter.TabIndex = 1
        Me.lblQuickFilter.Text = "Quick Filter:"
        '
        'cmsDeleteBackup
        '
        Me.cmsDeleteBackup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsDeleteOne, Me.cmsDeleteAll})
        Me.cmsDeleteBackup.Name = "cmsDeleteBackup"
        Me.cmsDeleteBackup.ShowImageMargin = False
        Me.cmsDeleteBackup.Size = New System.Drawing.Size(115, 48)
        '
        'cmsDeleteOne
        '
        Me.cmsDeleteOne.Name = "cmsDeleteOne"
        Me.cmsDeleteOne.Size = New System.Drawing.Size(114, 22)
        Me.cmsDeleteOne.Text = "&Selected File"
        '
        'cmsDeleteAll
        '
        Me.cmsDeleteAll.Name = "cmsDeleteAll"
        Me.cmsDeleteAll.Size = New System.Drawing.Size(114, 22)
        Me.cmsDeleteAll.Text = "&All Files"
        '
        'btnSessions
        '
        Me.btnSessions.Location = New System.Drawing.Point(323, 586)
        Me.btnSessions.Name = "btnSessions"
        Me.btnSessions.Size = New System.Drawing.Size(100, 23)
        Me.btnSessions.TabIndex = 12
        Me.btnSessions.Text = "&View Sessions..."
        Me.btnSessions.UseVisualStyleBackColor = True
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(70, 154)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(413, 54)
        Me.txtComments.TabIndex = 17
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Location = New System.Drawing.Point(7, 157)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(59, 13)
        Me.lblComments.TabIndex = 18
        Me.lblComments.Text = "Comments:"
        '
        'frmGameManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 621)
        Me.Controls.Add(Me.btnSessions)
        Me.Controls.Add(Me.lblQuickFilter)
        Me.Controls.Add(Me.txtQuickFilter)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImport)
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
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpExtra.ResumeLayout(False)
        Me.grpExtra.PerformLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpStats.ResumeLayout(False)
        Me.grpStats.PerformLayout()
        Me.grpFilter.ResumeLayout(False)
        Me.grpFilter.PerformLayout()
        Me.cmsImport.ResumeLayout(False)
        Me.cmsDeleteBackup.ResumeLayout(False)
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
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnAppPathBrowse As System.Windows.Forms.Button
    Friend WithEvents lblGamePath As System.Windows.Forms.Label
    Friend WithEvents txtAppPath As System.Windows.Forms.TextBox
    Friend WithEvents lblIcon As System.Windows.Forms.Label
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
    Friend WithEvents lblRemote As System.Windows.Forms.Label
    Friend WithEvents lblLocalData As System.Windows.Forms.Label
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents lblBackupFile As System.Windows.Forms.Label
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
    Friend WithEvents optCustom As System.Windows.Forms.RadioButton
    Friend WithEvents btnInclude As System.Windows.Forms.Button
    Friend WithEvents btnExclude As System.Windows.Forms.Button
    Friend WithEvents lblRestorePath As Label
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmsImport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmsOfficial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtQuickFilter As TextBox
    Friend WithEvents lblQuickFilter As Label
    Friend WithEvents lblLocalBackupData As Label
    Friend WithEvents lblRestorePathData As Label
    Friend WithEvents lblBackupFileData As Label
    Friend WithEvents lblLimit As Label
    Friend WithEvents nudLimit As NumericUpDown
    Friend WithEvents cboRemoteBackup As ComboBox
    Friend WithEvents cmsDeleteBackup As ContextMenuStrip
    Friend WithEvents cmsDeleteOne As ToolStripMenuItem
    Friend WithEvents cmsDeleteAll As ToolStripMenuItem
    Friend WithEvents chkCleanFolder As CheckBox
    Friend WithEvents txtParameter As TextBox
    Friend WithEvents lblParameter As Label
    Friend WithEvents btnSessions As Button
    Friend WithEvents lblComments As Label
    Friend WithEvents txtComments As TextBox
End Class
