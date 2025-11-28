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
        Me.cboOS = New System.Windows.Forms.ComboBox()
        Me.chkRecurseSubFolders = New System.Windows.Forms.CheckBox()
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
        Me.btnIconBrowse = New System.Windows.Forms.Button()
        Me.txtIcon = New System.Windows.Forms.TextBox()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.txtVersion = New System.Windows.Forms.TextBox()
        Me.txtCompany = New System.Windows.Forms.TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.lblIcon = New System.Windows.Forms.Label()
        Me.nudHours = New System.Windows.Forms.NumericUpDown()
        Me.lblHours = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lstGames = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cmsImport = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsOfficialWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsOfficialLinux = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsLudusavi = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsURL = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.cmsBackupData = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsDeleteOne = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsDeleteAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsImportData = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnAdvanced = New System.Windows.Forms.Button()
        Me.ttFullPath = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmsAdvanced = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsLaunchSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsWineConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.ttHelp = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabGameManager = New System.Windows.Forms.TabControl()
        Me.tbConfig = New System.Windows.Forms.TabPage()
        Me.grpCoreConfig = New System.Windows.Forms.GroupBox()
        Me.lblTimeIntervalMinutes = New System.Windows.Forms.Label()
        Me.nudTimedInterval = New System.Windows.Forms.NumericUpDown()
        Me.chkTimedBackup = New System.Windows.Forms.CheckBox()
        Me.btnOpenSaveFolder = New System.Windows.Forms.Button()
        Me.btnOpenGameFolder = New System.Windows.Forms.Button()
        Me.lblInterval = New System.Windows.Forms.Label()
        Me.nudInterval = New System.Windows.Forms.NumericUpDown()
        Me.chkDifferentialBackup = New System.Windows.Forms.CheckBox()
        Me.btnGameID = New System.Windows.Forms.Button()
        Me.lblGameTags = New System.Windows.Forms.LinkLabel()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.btnProcessOptions = New System.Windows.Forms.Button()
        Me.btnAppPathBrowse = New System.Windows.Forms.Button()
        Me.btnLinks = New System.Windows.Forms.Button()
        Me.btnMonitorOptions = New System.Windows.Forms.Button()
        Me.lblGamePath = New System.Windows.Forms.Label()
        Me.txtAppPath = New System.Windows.Forms.TextBox()
        Me.tbGameInfo = New System.Windows.Forms.TabPage()
        Me.grpGameInfo = New System.Windows.Forms.GroupBox()
        Me.lblComments = New System.Windows.Forms.Label()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.tbBackupInfo = New System.Windows.Forms.TabPage()
        Me.grpBackupInfo = New System.Windows.Forms.GroupBox()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.btnMarkAsRestored = New System.Windows.Forms.Button()
        Me.lblRemote = New System.Windows.Forms.Label()
        Me.btnBackupData = New System.Windows.Forms.Button()
        Me.lblRestorePath = New System.Windows.Forms.Label()
        Me.lblLocalBackupData = New System.Windows.Forms.Label()
        Me.btnOpenBackupFolder = New System.Windows.Forms.Button()
        Me.lblBackupFile = New System.Windows.Forms.Label()
        Me.cboRemoteBackup = New System.Windows.Forms.ComboBox()
        Me.lblBackupFileData = New System.Windows.Forms.LinkLabel()
        Me.lblLocalData = New System.Windows.Forms.Label()
        Me.lblRestorePathData = New System.Windows.Forms.LinkLabel()
        Me.cboFilters = New System.Windows.Forms.ComboBox()
        Me.lblFilters = New System.Windows.Forms.Label()
        Me.cmsProcessOptions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsRegEx = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsUseWindowTitle = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsLinks = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsLinkConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsLinkProcess = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsMonitorOptions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsEnabled = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsMonitorOnly = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnCopy = New System.Windows.Forms.Button()
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsImport.SuspendLayout()
        Me.cmsBackupData.SuspendLayout()
        Me.cmsAdvanced.SuspendLayout()
        Me.tabGameManager.SuspendLayout()
        Me.tbConfig.SuspendLayout()
        Me.grpCoreConfig.SuspendLayout()
        CType(Me.nudTimedInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbGameInfo.SuspendLayout()
        Me.grpGameInfo.SuspendLayout()
        Me.tbBackupInfo.SuspendLayout()
        Me.grpBackupInfo.SuspendLayout()
        Me.cmsProcessOptions.SuspendLayout()
        Me.cmsLinks.SuspendLayout()
        Me.cmsMonitorOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Image = Global.GBM.My.Resources.Resources.Multi_Add
        Me.btnAdd.Location = New System.Drawing.Point(12, 329)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(80, 50)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "New"
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = Global.GBM.My.Resources.Resources.Multi_Delete
        Me.btnDelete.Location = New System.Drawing.Point(184, 329)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(80, 50)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'cboOS
        '
        Me.cboOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOS.FormattingEnabled = True
        Me.cboOS.Location = New System.Drawing.Point(82, 226)
        Me.cboOS.Name = "cboOS"
        Me.cboOS.Size = New System.Drawing.Size(130, 21)
        Me.cboOS.TabIndex = 28
        '
        'chkRecurseSubFolders
        '
        Me.chkRecurseSubFolders.AutoSize = True
        Me.chkRecurseSubFolders.Location = New System.Drawing.Point(114, 257)
        Me.chkRecurseSubFolders.Name = "chkRecurseSubFolders"
        Me.chkRecurseSubFolders.Size = New System.Drawing.Size(15, 14)
        Me.chkRecurseSubFolders.TabIndex = 0
        Me.chkRecurseSubFolders.TabStop = False
        Me.chkRecurseSubFolders.UseVisualStyleBackColor = True
        Me.chkRecurseSubFolders.Visible = False
        '
        'txtParameter
        '
        Me.txtParameter.Location = New System.Drawing.Point(78, 64)
        Me.txtParameter.Name = "txtParameter"
        Me.txtParameter.Size = New System.Drawing.Size(425, 20)
        Me.txtParameter.TabIndex = 8
        '
        'lblParameter
        '
        Me.lblParameter.AutoSize = True
        Me.lblParameter.Location = New System.Drawing.Point(6, 67)
        Me.lblParameter.Name = "lblParameter"
        Me.lblParameter.Size = New System.Drawing.Size(58, 13)
        Me.lblParameter.TabIndex = 7
        Me.lblParameter.Text = "Parameter:"
        '
        'chkCleanFolder
        '
        Me.chkCleanFolder.Location = New System.Drawing.Point(378, 168)
        Me.chkCleanFolder.Name = "chkCleanFolder"
        Me.chkCleanFolder.Size = New System.Drawing.Size(153, 17)
        Me.chkCleanFolder.TabIndex = 22
        Me.chkCleanFolder.Text = "Delete folder on restore"
        Me.chkCleanFolder.UseVisualStyleBackColor = True
        '
        'lblLimit
        '
        Me.lblLimit.Location = New System.Drawing.Point(424, 197)
        Me.lblLimit.Name = "lblLimit"
        Me.lblLimit.Size = New System.Drawing.Size(107, 13)
        Me.lblLimit.TabIndex = 25
        Me.lblLimit.Text = "Backup limit"
        '
        'nudLimit
        '
        Me.nudLimit.Location = New System.Drawing.Point(378, 195)
        Me.nudLimit.Name = "nudLimit"
        Me.nudLimit.Size = New System.Drawing.Size(40, 20)
        Me.nudLimit.TabIndex = 24
        '
        'btnExclude
        '
        Me.btnExclude.Image = Global.GBM.My.Resources.Resources.frmGameManager_Exclude_Items
        Me.btnExclude.Location = New System.Drawing.Point(112, 168)
        Me.btnExclude.Name = "btnExclude"
        Me.btnExclude.Size = New System.Drawing.Size(100, 45)
        Me.btnExclude.TabIndex = 20
        Me.btnExclude.Text = "E&xclude..."
        Me.btnExclude.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnExclude.UseVisualStyleBackColor = True
        '
        'btnInclude
        '
        Me.btnInclude.Image = Global.GBM.My.Resources.Resources.frmGameManager_Include_Items
        Me.btnInclude.Location = New System.Drawing.Point(6, 168)
        Me.btnInclude.Name = "btnInclude"
        Me.btnInclude.Size = New System.Drawing.Size(100, 45)
        Me.btnInclude.TabIndex = 19
        Me.btnInclude.Text = "Incl&ude..."
        Me.btnInclude.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnInclude.UseVisualStyleBackColor = True
        '
        'txtID
        '
        Me.txtID.Enabled = False
        Me.txtID.Location = New System.Drawing.Point(78, 254)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(30, 20)
        Me.txtID.TabIndex = 0
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'btnSavePathBrowse
        '
        Me.btnSavePathBrowse.Location = New System.Drawing.Point(473, 115)
        Me.btnSavePathBrowse.Name = "btnSavePathBrowse"
        Me.btnSavePathBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnSavePathBrowse.TabIndex = 15
        Me.btnSavePathBrowse.Text = "..."
        Me.btnSavePathBrowse.UseVisualStyleBackColor = True
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(501, 39)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnProcessBrowse.TabIndex = 6
        Me.btnProcessBrowse.Text = "..."
        Me.btnProcessBrowse.UseVisualStyleBackColor = True
        '
        'lblSavePath
        '
        Me.lblSavePath.AutoSize = True
        Me.lblSavePath.Location = New System.Drawing.Point(6, 120)
        Me.lblSavePath.Name = "lblSavePath"
        Me.lblSavePath.Size = New System.Drawing.Size(60, 13)
        Me.lblSavePath.TabIndex = 13
        Me.lblSavePath.Text = "Save Path:"
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(6, 42)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(48, 13)
        Me.lblProcess.TabIndex = 3
        Me.lblProcess.Text = "Process:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(6, 17)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Name:"
        '
        'txtExclude
        '
        Me.txtExclude.Location = New System.Drawing.Point(42, 254)
        Me.txtExclude.Name = "txtExclude"
        Me.txtExclude.Size = New System.Drawing.Size(30, 20)
        Me.txtExclude.TabIndex = 0
        Me.txtExclude.TabStop = False
        Me.txtExclude.Visible = False
        '
        'txtFileType
        '
        Me.txtFileType.Location = New System.Drawing.Point(6, 254)
        Me.txtFileType.Name = "txtFileType"
        Me.txtFileType.Size = New System.Drawing.Size(30, 20)
        Me.txtFileType.TabIndex = 0
        Me.txtFileType.TabStop = False
        Me.txtFileType.Visible = False
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(218, 196)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(133, 17)
        Me.chkTimeStamp.TabIndex = 23
        Me.chkTimeStamp.Text = "Save multiple backups"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'chkFolderSave
        '
        Me.chkFolderSave.AutoSize = True
        Me.chkFolderSave.Location = New System.Drawing.Point(218, 168)
        Me.chkFolderSave.Name = "chkFolderSave"
        Me.chkFolderSave.Size = New System.Drawing.Size(109, 17)
        Me.chkFolderSave.TabIndex = 21
        Me.chkFolderSave.Text = "Save entire folder"
        Me.chkFolderSave.UseVisualStyleBackColor = True
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(78, 116)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.Size = New System.Drawing.Size(389, 20)
        Me.txtSavePath.TabIndex = 14
        '
        'txtProcess
        '
        Me.txtProcess.Location = New System.Drawing.Point(78, 39)
        Me.txtProcess.Name = "txtProcess"
        Me.txtProcess.Size = New System.Drawing.Size(389, 20)
        Me.txtProcess.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(78, 13)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(425, 20)
        Me.txtName.TabIndex = 1
        '
        'btnIconBrowse
        '
        Me.btnIconBrowse.Location = New System.Drawing.Point(447, 114)
        Me.btnIconBrowse.Name = "btnIconBrowse"
        Me.btnIconBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnIconBrowse.TabIndex = 8
        Me.btnIconBrowse.Text = "..."
        Me.btnIconBrowse.UseVisualStyleBackColor = True
        '
        'txtIcon
        '
        Me.txtIcon.Location = New System.Drawing.Point(74, 114)
        Me.txtIcon.Name = "txtIcon"
        Me.txtIcon.Size = New System.Drawing.Size(367, 20)
        Me.txtIcon.TabIndex = 7
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(6, 91)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 4
        Me.lblVersion.Text = "Version:"
        '
        'txtVersion
        '
        Me.txtVersion.Location = New System.Drawing.Point(74, 88)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.Size = New System.Drawing.Size(457, 20)
        Me.txtVersion.TabIndex = 5
        '
        'txtCompany
        '
        Me.txtCompany.Location = New System.Drawing.Point(74, 62)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(457, 20)
        Me.txtCompany.TabIndex = 3
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.Location = New System.Drawing.Point(6, 65)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(54, 13)
        Me.lblCompany.TabIndex = 2
        Me.lblCompany.Text = "Company:"
        '
        'pbIcon
        '
        Me.pbIcon.InitialImage = Nothing
        Me.pbIcon.Location = New System.Drawing.Point(483, 114)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 15
        Me.pbIcon.TabStop = False
        '
        'lblIcon
        '
        Me.lblIcon.AutoSize = True
        Me.lblIcon.Location = New System.Drawing.Point(6, 118)
        Me.lblIcon.Name = "lblIcon"
        Me.lblIcon.Size = New System.Drawing.Size(31, 13)
        Me.lblIcon.TabIndex = 6
        Me.lblIcon.Text = "Icon:"
        '
        'nudHours
        '
        Me.nudHours.DecimalPlaces = 1
        Me.nudHours.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudHours.Location = New System.Drawing.Point(74, 142)
        Me.nudHours.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudHours.Name = "nudHours"
        Me.nudHours.Size = New System.Drawing.Size(88, 20)
        Me.nudHours.TabIndex = 10
        '
        'lblHours
        '
        Me.lblHours.AutoSize = True
        Me.lblHours.Location = New System.Drawing.Point(6, 144)
        Me.lblHours.Name = "lblHours"
        Me.lblHours.Size = New System.Drawing.Size(38, 13)
        Me.lblHours.TabIndex = 9
        Me.lblHours.Text = "Hours:"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GBM.My.Resources.Resources.Multi_Save
        Me.btnSave.Location = New System.Drawing.Point(624, 329)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 50)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lstGames
        '
        Me.lstGames.FormattingEnabled = True
        Me.lstGames.IntegralHeight = False
        Me.lstGames.Location = New System.Drawing.Point(12, 69)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstGames.Size = New System.Drawing.Size(228, 251)
        Me.lstGames.TabIndex = 4
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(710, 329)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 50)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Ca&ncel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Image = Global.GBM.My.Resources.Resources.Multi_Import
        Me.btnImport.Location = New System.Drawing.Point(270, 329)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(80, 50)
        Me.btnImport.TabIndex = 8
        Me.btnImport.Text = "&Import"
        Me.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Image = Global.GBM.My.Resources.Resources.Multi_Export
        Me.btnExport.Location = New System.Drawing.Point(356, 329)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(80, 50)
        Me.btnExport.TabIndex = 9
        Me.btnExport.Text = "&Export"
        Me.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmsImport
        '
        Me.cmsImport.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsOfficial, Me.cmsLudusavi, Me.cmsFile, Me.cmsURL})
        Me.cmsImport.Name = "cmsImport"
        Me.cmsImport.ShowImageMargin = False
        Me.cmsImport.Size = New System.Drawing.Size(155, 92)
        '
        'cmsOfficial
        '
        Me.cmsOfficial.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsOfficialWindows, Me.cmsOfficialLinux})
        Me.cmsOfficial.Name = "cmsOfficial"
        Me.cmsOfficial.Size = New System.Drawing.Size(154, 22)
        Me.cmsOfficial.Text = "&Official List..."
        '
        'cmsOfficialWindows
        '
        Me.cmsOfficialWindows.Name = "cmsOfficialWindows"
        Me.cmsOfficialWindows.Size = New System.Drawing.Size(132, 22)
        Me.cmsOfficialWindows.Text = "&Windows..."
        '
        'cmsOfficialLinux
        '
        Me.cmsOfficialLinux.Name = "cmsOfficialLinux"
        Me.cmsOfficialLinux.Size = New System.Drawing.Size(132, 22)
        Me.cmsOfficialLinux.Text = "&Linux..."
        '
        'cmsLudusavi
        '
        Me.cmsLudusavi.Name = "cmsLudusavi"
        Me.cmsLudusavi.Size = New System.Drawing.Size(154, 22)
        Me.cmsLudusavi.Text = "&Ludusavi Manifest..."
        '
        'cmsFile
        '
        Me.cmsFile.Name = "cmsFile"
        Me.cmsFile.Size = New System.Drawing.Size(154, 22)
        Me.cmsFile.Text = "&File..."
        '
        'cmsURL
        '
        Me.cmsURL.Name = "cmsURL"
        Me.cmsURL.Size = New System.Drawing.Size(154, 22)
        Me.cmsURL.Text = "&URL..."
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(75, 40)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(165, 20)
        Me.txtSearch.TabIndex = 3
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(9, 43)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 2
        Me.lblSearch.Text = "Search:"
        '
        'cmsBackupData
        '
        Me.cmsBackupData.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsDeleteOne, Me.cmsDeleteAll, Me.cmsImportData})
        Me.cmsBackupData.Name = "cmsDeleteBackup"
        Me.cmsBackupData.ShowImageMargin = False
        Me.cmsBackupData.Size = New System.Drawing.Size(130, 70)
        '
        'cmsDeleteOne
        '
        Me.cmsDeleteOne.Name = "cmsDeleteOne"
        Me.cmsDeleteOne.Size = New System.Drawing.Size(129, 22)
        Me.cmsDeleteOne.Text = "Delete &Selected"
        '
        'cmsDeleteAll
        '
        Me.cmsDeleteAll.Name = "cmsDeleteAll"
        Me.cmsDeleteAll.Size = New System.Drawing.Size(129, 22)
        Me.cmsDeleteAll.Text = "Delete &All"
        '
        'cmsImportData
        '
        Me.cmsImportData.Name = "cmsImportData"
        Me.cmsImportData.Size = New System.Drawing.Size(129, 22)
        Me.cmsImportData.Text = "&Import Backup"
        '
        'btnAdvanced
        '
        Me.btnAdvanced.Image = Global.GBM.My.Resources.Resources.frmGameManager_Advanced
        Me.btnAdvanced.Location = New System.Drawing.Point(538, 329)
        Me.btnAdvanced.Name = "btnAdvanced"
        Me.btnAdvanced.Size = New System.Drawing.Size(80, 50)
        Me.btnAdvanced.TabIndex = 11
        Me.btnAdvanced.Text = "&Advanced"
        Me.btnAdvanced.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAdvanced.UseVisualStyleBackColor = True
        '
        'ttFullPath
        '
        Me.ttFullPath.AutoPopDelay = 5000
        Me.ttFullPath.InitialDelay = 300
        Me.ttFullPath.ReshowDelay = 60
        '
        'cmsAdvanced
        '
        Me.cmsAdvanced.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsLaunchSettings, Me.cmsWineConfig})
        Me.cmsAdvanced.Name = "cmsLinks"
        Me.cmsAdvanced.ShowImageMargin = False
        Me.cmsAdvanced.Size = New System.Drawing.Size(163, 48)
        '
        'cmsLaunchSettings
        '
        Me.cmsLaunchSettings.Name = "cmsLaunchSettings"
        Me.cmsLaunchSettings.Size = New System.Drawing.Size(162, 22)
        Me.cmsLaunchSettings.Text = "&Launch Settings..."
        '
        'cmsWineConfig
        '
        Me.cmsWineConfig.Name = "cmsWineConfig"
        Me.cmsWineConfig.Size = New System.Drawing.Size(162, 22)
        Me.cmsWineConfig.Text = "&Wine Configuration..."
        '
        'ttHelp
        '
        Me.ttHelp.AutoPopDelay = 5000
        Me.ttHelp.InitialDelay = 300
        Me.ttHelp.ReshowDelay = 60
        '
        'tabGameManager
        '
        Me.tabGameManager.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabGameManager.Controls.Add(Me.tbConfig)
        Me.tabGameManager.Controls.Add(Me.tbGameInfo)
        Me.tabGameManager.Controls.Add(Me.tbBackupInfo)
        Me.tabGameManager.Location = New System.Drawing.Point(246, 12)
        Me.tabGameManager.Multiline = True
        Me.tabGameManager.Name = "tabGameManager"
        Me.tabGameManager.SelectedIndex = 0
        Me.tabGameManager.Size = New System.Drawing.Size(554, 315)
        Me.tabGameManager.TabIndex = 10
        '
        'tbConfig
        '
        Me.tbConfig.BackColor = System.Drawing.SystemColors.Control
        Me.tbConfig.Controls.Add(Me.grpCoreConfig)
        Me.tbConfig.Location = New System.Drawing.Point(4, 25)
        Me.tbConfig.Name = "tbConfig"
        Me.tbConfig.Size = New System.Drawing.Size(546, 286)
        Me.tbConfig.TabIndex = 0
        Me.tbConfig.Text = "Core Configuration"
        '
        'grpCoreConfig
        '
        Me.grpCoreConfig.Controls.Add(Me.lblTimeIntervalMinutes)
        Me.grpCoreConfig.Controls.Add(Me.nudTimedInterval)
        Me.grpCoreConfig.Controls.Add(Me.chkTimedBackup)
        Me.grpCoreConfig.Controls.Add(Me.btnOpenSaveFolder)
        Me.grpCoreConfig.Controls.Add(Me.btnOpenGameFolder)
        Me.grpCoreConfig.Controls.Add(Me.lblInterval)
        Me.grpCoreConfig.Controls.Add(Me.nudInterval)
        Me.grpCoreConfig.Controls.Add(Me.chkDifferentialBackup)
        Me.grpCoreConfig.Controls.Add(Me.btnGameID)
        Me.grpCoreConfig.Controls.Add(Me.lblGameTags)
        Me.grpCoreConfig.Controls.Add(Me.lblTags)
        Me.grpCoreConfig.Controls.Add(Me.btnProcessOptions)
        Me.grpCoreConfig.Controls.Add(Me.btnAppPathBrowse)
        Me.grpCoreConfig.Controls.Add(Me.lblName)
        Me.grpCoreConfig.Controls.Add(Me.btnLinks)
        Me.grpCoreConfig.Controls.Add(Me.btnMonitorOptions)
        Me.grpCoreConfig.Controls.Add(Me.txtFileType)
        Me.grpCoreConfig.Controls.Add(Me.txtExclude)
        Me.grpCoreConfig.Controls.Add(Me.lblGamePath)
        Me.grpCoreConfig.Controls.Add(Me.chkTimeStamp)
        Me.grpCoreConfig.Controls.Add(Me.txtAppPath)
        Me.grpCoreConfig.Controls.Add(Me.chkFolderSave)
        Me.grpCoreConfig.Controls.Add(Me.txtName)
        Me.grpCoreConfig.Controls.Add(Me.btnSavePathBrowse)
        Me.grpCoreConfig.Controls.Add(Me.txtSavePath)
        Me.grpCoreConfig.Controls.Add(Me.lblSavePath)
        Me.grpCoreConfig.Controls.Add(Me.txtProcess)
        Me.grpCoreConfig.Controls.Add(Me.btnInclude)
        Me.grpCoreConfig.Controls.Add(Me.cboOS)
        Me.grpCoreConfig.Controls.Add(Me.txtID)
        Me.grpCoreConfig.Controls.Add(Me.btnProcessBrowse)
        Me.grpCoreConfig.Controls.Add(Me.btnExclude)
        Me.grpCoreConfig.Controls.Add(Me.chkRecurseSubFolders)
        Me.grpCoreConfig.Controls.Add(Me.txtParameter)
        Me.grpCoreConfig.Controls.Add(Me.nudLimit)
        Me.grpCoreConfig.Controls.Add(Me.chkCleanFolder)
        Me.grpCoreConfig.Controls.Add(Me.lblProcess)
        Me.grpCoreConfig.Controls.Add(Me.lblParameter)
        Me.grpCoreConfig.Controls.Add(Me.lblLimit)
        Me.grpCoreConfig.Location = New System.Drawing.Point(3, 3)
        Me.grpCoreConfig.Name = "grpCoreConfig"
        Me.grpCoreConfig.Size = New System.Drawing.Size(537, 280)
        Me.grpCoreConfig.TabIndex = 0
        Me.grpCoreConfig.TabStop = False
        '
        'lblTimeIntervalMinutes
        '
        Me.lblTimeIntervalMinutes.Location = New System.Drawing.Point(424, 251)
        Me.lblTimeIntervalMinutes.Name = "lblTimeIntervalMinutes"
        Me.lblTimeIntervalMinutes.Size = New System.Drawing.Size(107, 13)
        Me.lblTimeIntervalMinutes.TabIndex = 34
        Me.lblTimeIntervalMinutes.Text = "Minutes"
        '
        'nudTimedInterval
        '
        Me.nudTimedInterval.Location = New System.Drawing.Point(378, 249)
        Me.nudTimedInterval.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudTimedInterval.Name = "nudTimedInterval"
        Me.nudTimedInterval.Size = New System.Drawing.Size(40, 20)
        Me.nudTimedInterval.TabIndex = 33
        Me.nudTimedInterval.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'chkTimedBackup
        '
        Me.chkTimedBackup.AutoSize = True
        Me.chkTimedBackup.Location = New System.Drawing.Point(218, 250)
        Me.chkTimedBackup.Name = "chkTimedBackup"
        Me.chkTimedBackup.Size = New System.Drawing.Size(125, 17)
        Me.chkTimedBackup.TabIndex = 32
        Me.chkTimedBackup.Text = "Time interval backup"
        Me.chkTimedBackup.UseVisualStyleBackColor = True
        '
        'btnOpenSaveFolder
        '
        Me.btnOpenSaveFolder.Image = Global.GBM.My.Resources.Resources.frmGameManager_Folder_Open
        Me.btnOpenSaveFolder.Location = New System.Drawing.Point(509, 115)
        Me.btnOpenSaveFolder.Name = "btnOpenSaveFolder"
        Me.btnOpenSaveFolder.Size = New System.Drawing.Size(22, 22)
        Me.btnOpenSaveFolder.TabIndex = 16
        Me.btnOpenSaveFolder.UseVisualStyleBackColor = True
        '
        'btnOpenGameFolder
        '
        Me.btnOpenGameFolder.Image = Global.GBM.My.Resources.Resources.frmGameManager_Folder_Open
        Me.btnOpenGameFolder.Location = New System.Drawing.Point(509, 88)
        Me.btnOpenGameFolder.Name = "btnOpenGameFolder"
        Me.btnOpenGameFolder.Size = New System.Drawing.Size(22, 22)
        Me.btnOpenGameFolder.TabIndex = 12
        Me.btnOpenGameFolder.UseVisualStyleBackColor = True
        '
        'lblInterval
        '
        Me.lblInterval.Location = New System.Drawing.Point(424, 225)
        Me.lblInterval.Name = "lblInterval"
        Me.lblInterval.Size = New System.Drawing.Size(107, 13)
        Me.lblInterval.TabIndex = 31
        Me.lblInterval.Text = "Full backup interval"
        '
        'nudInterval
        '
        Me.nudInterval.Location = New System.Drawing.Point(378, 223)
        Me.nudInterval.Name = "nudInterval"
        Me.nudInterval.Size = New System.Drawing.Size(40, 20)
        Me.nudInterval.TabIndex = 30
        '
        'chkDifferentialBackup
        '
        Me.chkDifferentialBackup.AutoSize = True
        Me.chkDifferentialBackup.Location = New System.Drawing.Point(218, 224)
        Me.chkDifferentialBackup.Name = "chkDifferentialBackup"
        Me.chkDifferentialBackup.Size = New System.Drawing.Size(115, 17)
        Me.chkDifferentialBackup.TabIndex = 29
        Me.chkDifferentialBackup.Text = "Differential backup"
        Me.chkDifferentialBackup.UseVisualStyleBackColor = True
        '
        'btnGameID
        '
        Me.btnGameID.Image = Global.GBM.My.Resources.Resources.frmGameManager_GameID
        Me.btnGameID.Location = New System.Drawing.Point(509, 13)
        Me.btnGameID.Name = "btnGameID"
        Me.btnGameID.Size = New System.Drawing.Size(22, 22)
        Me.btnGameID.TabIndex = 2
        Me.btnGameID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGameID.UseVisualStyleBackColor = True
        '
        'lblGameTags
        '
        Me.lblGameTags.ActiveLinkColor = System.Drawing.SystemColors.ControlText
        Me.lblGameTags.AutoEllipsis = True
        Me.lblGameTags.LinkColor = System.Drawing.SystemColors.ControlText
        Me.lblGameTags.Location = New System.Drawing.Point(75, 145)
        Me.lblGameTags.Name = "lblGameTags"
        Me.lblGameTags.Size = New System.Drawing.Size(426, 13)
        Me.lblGameTags.TabIndex = 18
        Me.lblGameTags.TabStop = True
        Me.lblGameTags.Text = "Manage Tags"
        '
        'lblTags
        '
        Me.lblTags.AutoSize = True
        Me.lblTags.Location = New System.Drawing.Point(6, 145)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(34, 13)
        Me.lblTags.TabIndex = 17
        Me.lblTags.Text = "Tags:"
        '
        'btnProcessOptions
        '
        Me.btnProcessOptions.Image = Global.GBM.My.Resources.Resources.frmGameManager_Process
        Me.btnProcessOptions.Location = New System.Drawing.Point(473, 38)
        Me.btnProcessOptions.Name = "btnProcessOptions"
        Me.btnProcessOptions.Size = New System.Drawing.Size(22, 22)
        Me.btnProcessOptions.TabIndex = 5
        Me.btnProcessOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnProcessOptions.UseVisualStyleBackColor = True
        '
        'btnAppPathBrowse
        '
        Me.btnAppPathBrowse.Location = New System.Drawing.Point(473, 89)
        Me.btnAppPathBrowse.Name = "btnAppPathBrowse"
        Me.btnAppPathBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnAppPathBrowse.TabIndex = 11
        Me.btnAppPathBrowse.Text = "..."
        Me.btnAppPathBrowse.UseVisualStyleBackColor = True
        '
        'btnLinks
        '
        Me.btnLinks.Image = Global.GBM.My.Resources.Resources.frmGameManager_Link
        Me.btnLinks.Location = New System.Drawing.Point(44, 219)
        Me.btnLinks.Name = "btnLinks"
        Me.btnLinks.Size = New System.Drawing.Size(32, 32)
        Me.btnLinks.TabIndex = 27
        Me.btnLinks.UseVisualStyleBackColor = True
        '
        'btnMonitorOptions
        '
        Me.btnMonitorOptions.Image = Global.GBM.My.Resources.Resources.Multi_Search
        Me.btnMonitorOptions.Location = New System.Drawing.Point(6, 219)
        Me.btnMonitorOptions.Name = "btnMonitorOptions"
        Me.btnMonitorOptions.Size = New System.Drawing.Size(32, 32)
        Me.btnMonitorOptions.TabIndex = 26
        Me.btnMonitorOptions.UseVisualStyleBackColor = True
        '
        'lblGamePath
        '
        Me.lblGamePath.AutoSize = True
        Me.lblGamePath.Location = New System.Drawing.Point(6, 93)
        Me.lblGamePath.Name = "lblGamePath"
        Me.lblGamePath.Size = New System.Drawing.Size(63, 13)
        Me.lblGamePath.TabIndex = 9
        Me.lblGamePath.Text = "Game Path:"
        '
        'txtAppPath
        '
        Me.txtAppPath.Location = New System.Drawing.Point(78, 90)
        Me.txtAppPath.Name = "txtAppPath"
        Me.txtAppPath.Size = New System.Drawing.Size(389, 20)
        Me.txtAppPath.TabIndex = 10
        '
        'tbGameInfo
        '
        Me.tbGameInfo.BackColor = System.Drawing.SystemColors.Control
        Me.tbGameInfo.Controls.Add(Me.grpGameInfo)
        Me.tbGameInfo.Location = New System.Drawing.Point(4, 25)
        Me.tbGameInfo.Name = "tbGameInfo"
        Me.tbGameInfo.Size = New System.Drawing.Size(546, 286)
        Me.tbGameInfo.TabIndex = 1
        Me.tbGameInfo.Text = "Game Information"
        '
        'grpGameInfo
        '
        Me.grpGameInfo.Controls.Add(Me.lblComments)
        Me.grpGameInfo.Controls.Add(Me.txtComments)
        Me.grpGameInfo.Controls.Add(Me.pbIcon)
        Me.grpGameInfo.Controls.Add(Me.lblIcon)
        Me.grpGameInfo.Controls.Add(Me.btnIconBrowse)
        Me.grpGameInfo.Controls.Add(Me.nudHours)
        Me.grpGameInfo.Controls.Add(Me.txtIcon)
        Me.grpGameInfo.Controls.Add(Me.txtCompany)
        Me.grpGameInfo.Controls.Add(Me.lblVersion)
        Me.grpGameInfo.Controls.Add(Me.lblHours)
        Me.grpGameInfo.Controls.Add(Me.lblCompany)
        Me.grpGameInfo.Controls.Add(Me.txtVersion)
        Me.grpGameInfo.Location = New System.Drawing.Point(3, 3)
        Me.grpGameInfo.Name = "grpGameInfo"
        Me.grpGameInfo.Size = New System.Drawing.Size(537, 280)
        Me.grpGameInfo.TabIndex = 0
        Me.grpGameInfo.TabStop = False
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Location = New System.Drawing.Point(6, 16)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(59, 13)
        Me.lblComments.TabIndex = 0
        Me.lblComments.Text = "Comments:"
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(74, 16)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(457, 40)
        Me.txtComments.TabIndex = 1
        '
        'tbBackupInfo
        '
        Me.tbBackupInfo.BackColor = System.Drawing.SystemColors.Control
        Me.tbBackupInfo.Controls.Add(Me.grpBackupInfo)
        Me.tbBackupInfo.Location = New System.Drawing.Point(4, 25)
        Me.tbBackupInfo.Name = "tbBackupInfo"
        Me.tbBackupInfo.Size = New System.Drawing.Size(546, 286)
        Me.tbBackupInfo.TabIndex = 2
        Me.tbBackupInfo.Text = "Backup Management"
        '
        'grpBackupInfo
        '
        Me.grpBackupInfo.Controls.Add(Me.btnRestore)
        Me.grpBackupInfo.Controls.Add(Me.btnBackup)
        Me.grpBackupInfo.Controls.Add(Me.btnMarkAsRestored)
        Me.grpBackupInfo.Controls.Add(Me.lblRemote)
        Me.grpBackupInfo.Controls.Add(Me.btnBackupData)
        Me.grpBackupInfo.Controls.Add(Me.lblRestorePath)
        Me.grpBackupInfo.Controls.Add(Me.lblLocalBackupData)
        Me.grpBackupInfo.Controls.Add(Me.btnOpenBackupFolder)
        Me.grpBackupInfo.Controls.Add(Me.lblBackupFile)
        Me.grpBackupInfo.Controls.Add(Me.cboRemoteBackup)
        Me.grpBackupInfo.Controls.Add(Me.lblBackupFileData)
        Me.grpBackupInfo.Controls.Add(Me.lblLocalData)
        Me.grpBackupInfo.Controls.Add(Me.lblRestorePathData)
        Me.grpBackupInfo.Location = New System.Drawing.Point(3, 3)
        Me.grpBackupInfo.Name = "grpBackupInfo"
        Me.grpBackupInfo.Size = New System.Drawing.Size(537, 280)
        Me.grpBackupInfo.TabIndex = 0
        Me.grpBackupInfo.TabStop = False
        '
        'btnRestore
        '
        Me.btnRestore.Image = Global.GBM.My.Resources.Resources.Multi_Restore
        Me.btnRestore.Location = New System.Drawing.Point(423, 116)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(80, 55)
        Me.btnRestore.TabIndex = 12
        Me.btnRestore.Text = "&Restore"
        Me.btnRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnBackup
        '
        Me.btnBackup.Image = Global.GBM.My.Resources.Resources.Multi_Backup
        Me.btnBackup.Location = New System.Drawing.Point(337, 116)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(80, 55)
        Me.btnBackup.TabIndex = 13
        Me.btnBackup.Text = "&Backup"
        Me.btnBackup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'btnMarkAsRestored
        '
        Me.btnMarkAsRestored.Image = Global.GBM.My.Resources.Resources.frmGameManager_Mark
        Me.btnMarkAsRestored.Location = New System.Drawing.Point(509, 39)
        Me.btnMarkAsRestored.Name = "btnMarkAsRestored"
        Me.btnMarkAsRestored.Size = New System.Drawing.Size(22, 22)
        Me.btnMarkAsRestored.TabIndex = 5
        Me.btnMarkAsRestored.UseVisualStyleBackColor = True
        '
        'lblRemote
        '
        Me.lblRemote.AutoSize = True
        Me.lblRemote.Location = New System.Drawing.Point(6, 18)
        Me.lblRemote.Name = "lblRemote"
        Me.lblRemote.Size = New System.Drawing.Size(73, 13)
        Me.lblRemote.TabIndex = 0
        Me.lblRemote.Text = "Backup Data:"
        '
        'btnBackupData
        '
        Me.btnBackupData.Image = Global.GBM.My.Resources.Resources.frmGameManager_Backup_Data
        Me.btnBackupData.Location = New System.Drawing.Point(509, 13)
        Me.btnBackupData.Name = "btnBackupData"
        Me.btnBackupData.Size = New System.Drawing.Size(22, 22)
        Me.btnBackupData.TabIndex = 2
        Me.btnBackupData.UseVisualStyleBackColor = True
        '
        'lblRestorePath
        '
        Me.lblRestorePath.AutoSize = True
        Me.lblRestorePath.Location = New System.Drawing.Point(6, 97)
        Me.lblRestorePath.Name = "lblRestorePath"
        Me.lblRestorePath.Size = New System.Drawing.Size(72, 13)
        Me.lblRestorePath.TabIndex = 9
        Me.lblRestorePath.Text = "Restore Path:"
        '
        'lblLocalBackupData
        '
        Me.lblLocalBackupData.AutoEllipsis = True
        Me.lblLocalBackupData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLocalBackupData.Location = New System.Drawing.Point(104, 40)
        Me.lblLocalBackupData.Name = "lblLocalBackupData"
        Me.lblLocalBackupData.Size = New System.Drawing.Size(399, 20)
        Me.lblLocalBackupData.TabIndex = 4
        Me.lblLocalBackupData.Tag = "wipe"
        Me.lblLocalBackupData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblLocalBackupData.UseMnemonic = False
        '
        'btnOpenBackupFolder
        '
        Me.btnOpenBackupFolder.Image = Global.GBM.My.Resources.Resources.frmGameManager_Folder_Open
        Me.btnOpenBackupFolder.Location = New System.Drawing.Point(509, 66)
        Me.btnOpenBackupFolder.Name = "btnOpenBackupFolder"
        Me.btnOpenBackupFolder.Size = New System.Drawing.Size(22, 22)
        Me.btnOpenBackupFolder.TabIndex = 8
        Me.btnOpenBackupFolder.UseVisualStyleBackColor = True
        '
        'lblBackupFile
        '
        Me.lblBackupFile.AutoSize = True
        Me.lblBackupFile.Location = New System.Drawing.Point(6, 71)
        Me.lblBackupFile.Name = "lblBackupFile"
        Me.lblBackupFile.Size = New System.Drawing.Size(66, 13)
        Me.lblBackupFile.TabIndex = 6
        Me.lblBackupFile.Text = "Backup File:"
        '
        'cboRemoteBackup
        '
        Me.cboRemoteBackup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRemoteBackup.FormattingEnabled = True
        Me.cboRemoteBackup.Location = New System.Drawing.Point(104, 13)
        Me.cboRemoteBackup.Name = "cboRemoteBackup"
        Me.cboRemoteBackup.Size = New System.Drawing.Size(399, 21)
        Me.cboRemoteBackup.TabIndex = 1
        Me.cboRemoteBackup.Tag = "wipe"
        '
        'lblBackupFileData
        '
        Me.lblBackupFileData.ActiveLinkColor = System.Drawing.SystemColors.ControlText
        Me.lblBackupFileData.AutoEllipsis = True
        Me.lblBackupFileData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBackupFileData.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblBackupFileData.LinkColor = System.Drawing.SystemColors.ControlText
        Me.lblBackupFileData.Location = New System.Drawing.Point(104, 67)
        Me.lblBackupFileData.Name = "lblBackupFileData"
        Me.lblBackupFileData.Size = New System.Drawing.Size(399, 20)
        Me.lblBackupFileData.TabIndex = 7
        Me.lblBackupFileData.Tag = "wipe"
        Me.lblBackupFileData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblBackupFileData.UseMnemonic = False
        '
        'lblLocalData
        '
        Me.lblLocalData.AutoSize = True
        Me.lblLocalData.Location = New System.Drawing.Point(6, 44)
        Me.lblLocalData.Name = "lblLocalData"
        Me.lblLocalData.Size = New System.Drawing.Size(62, 13)
        Me.lblLocalData.TabIndex = 3
        Me.lblLocalData.Text = "Local Data:"
        '
        'lblRestorePathData
        '
        Me.lblRestorePathData.ActiveLinkColor = System.Drawing.SystemColors.ControlText
        Me.lblRestorePathData.AutoEllipsis = True
        Me.lblRestorePathData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRestorePathData.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lblRestorePathData.LinkColor = System.Drawing.SystemColors.ControlText
        Me.lblRestorePathData.Location = New System.Drawing.Point(104, 93)
        Me.lblRestorePathData.Name = "lblRestorePathData"
        Me.lblRestorePathData.Size = New System.Drawing.Size(399, 20)
        Me.lblRestorePathData.TabIndex = 10
        Me.lblRestorePathData.Tag = "wipe"
        Me.lblRestorePathData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblRestorePathData.UseMnemonic = False
        '
        'cboFilters
        '
        Me.cboFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilters.FormattingEnabled = True
        Me.cboFilters.Location = New System.Drawing.Point(75, 12)
        Me.cboFilters.Name = "cboFilters"
        Me.cboFilters.Size = New System.Drawing.Size(165, 21)
        Me.cboFilters.TabIndex = 1
        '
        'lblFilters
        '
        Me.lblFilters.AutoSize = True
        Me.lblFilters.Location = New System.Drawing.Point(9, 15)
        Me.lblFilters.Name = "lblFilters"
        Me.lblFilters.Size = New System.Drawing.Size(37, 13)
        Me.lblFilters.TabIndex = 0
        Me.lblFilters.Text = "Filters:"
        '
        'cmsProcessOptions
        '
        Me.cmsProcessOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsRegEx, Me.cmsUseWindowTitle})
        Me.cmsProcessOptions.Name = "ContextMenuStrip1"
        Me.cmsProcessOptions.Size = New System.Drawing.Size(173, 48)
        '
        'cmsRegEx
        '
        Me.cmsRegEx.CheckOnClick = True
        Me.cmsRegEx.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.cmsRegEx.Name = "cmsRegEx"
        Me.cmsRegEx.Size = New System.Drawing.Size(172, 22)
        Me.cmsRegEx.Text = "&Regular Expression"
        '
        'cmsUseWindowTitle
        '
        Me.cmsUseWindowTitle.CheckOnClick = True
        Me.cmsUseWindowTitle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.cmsUseWindowTitle.Name = "cmsUseWindowTitle"
        Me.cmsUseWindowTitle.Size = New System.Drawing.Size(172, 22)
        Me.cmsUseWindowTitle.Text = "Use &Window Title"
        '
        'cmsLinks
        '
        Me.cmsLinks.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsLinkConfiguration, Me.cmsLinkProcess})
        Me.cmsLinks.Name = "cmsLinks"
        Me.cmsLinks.ShowImageMargin = False
        Me.cmsLinks.Size = New System.Drawing.Size(158, 48)
        '
        'cmsLinkConfiguration
        '
        Me.cmsLinkConfiguration.Name = "cmsLinkConfiguration"
        Me.cmsLinkConfiguration.Size = New System.Drawing.Size(157, 22)
        Me.cmsLinkConfiguration.Text = "Link &Configuration..."
        '
        'cmsLinkProcess
        '
        Me.cmsLinkProcess.Name = "cmsLinkProcess"
        Me.cmsLinkProcess.Size = New System.Drawing.Size(157, 22)
        Me.cmsLinkProcess.Text = "Link &Process..."
        '
        'cmsMonitorOptions
        '
        Me.cmsMonitorOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsEnabled, Me.cmsMonitorOnly})
        Me.cmsMonitorOptions.Name = "cmsMonitorOptions"
        Me.cmsMonitorOptions.ShowCheckMargin = True
        Me.cmsMonitorOptions.ShowImageMargin = False
        Me.cmsMonitorOptions.Size = New System.Drawing.Size(226, 48)
        '
        'cmsEnabled
        '
        Me.cmsEnabled.CheckOnClick = True
        Me.cmsEnabled.Name = "cmsEnabled"
        Me.cmsEnabled.Size = New System.Drawing.Size(225, 22)
        Me.cmsEnabled.Text = "&Allow monitoring"
        '
        'cmsMonitorOnly
        '
        Me.cmsMonitorOnly.CheckOnClick = True
        Me.cmsMonitorOnly.Name = "cmsMonitorOnly"
        Me.cmsMonitorOnly.Size = New System.Drawing.Size(225, 22)
        Me.cmsMonitorOnly.Text = "&No backup when game ends"
        '
        'btnCopy
        '
        Me.btnCopy.Image = Global.GBM.My.Resources.Resources.frmGameManager_Copy
        Me.btnCopy.Location = New System.Drawing.Point(98, 329)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(80, 50)
        Me.btnCopy.TabIndex = 6
        Me.btnCopy.Text = "&Copy"
        Me.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'frmGameManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(809, 386)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.lblFilters)
        Me.Controls.Add(Me.cboFilters)
        Me.Controls.Add(Me.tabGameManager)
        Me.Controls.Add(Me.btnAdvanced)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lstGames)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGameManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Manager"
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHours, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsImport.ResumeLayout(False)
        Me.cmsBackupData.ResumeLayout(False)
        Me.cmsAdvanced.ResumeLayout(False)
        Me.tabGameManager.ResumeLayout(False)
        Me.tbConfig.ResumeLayout(False)
        Me.grpCoreConfig.ResumeLayout(False)
        Me.grpCoreConfig.PerformLayout()
        CType(Me.nudTimedInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbGameInfo.ResumeLayout(False)
        Me.grpGameInfo.ResumeLayout(False)
        Me.grpGameInfo.PerformLayout()
        Me.tbBackupInfo.ResumeLayout(False)
        Me.grpBackupInfo.ResumeLayout(False)
        Me.grpBackupInfo.PerformLayout()
        Me.cmsProcessOptions.ResumeLayout(False)
        Me.cmsLinks.ResumeLayout(False)
        Me.cmsMonitorOptions.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents txtSavePath As System.Windows.Forms.TextBox
    Friend WithEvents txtProcess As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
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
    Friend WithEvents lblIcon As System.Windows.Forms.Label
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents txtVersion As System.Windows.Forms.TextBox
    Friend WithEvents txtCompany As System.Windows.Forms.TextBox
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents nudHours As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHours As System.Windows.Forms.Label
    Friend WithEvents lstGames As System.Windows.Forms.ListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnIconBrowse As System.Windows.Forms.Button
    Friend WithEvents txtIcon As System.Windows.Forms.TextBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents btnInclude As System.Windows.Forms.Button
    Friend WithEvents btnExclude As System.Windows.Forms.Button
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmsImport As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmsOfficial As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents lblLimit As Label
    Friend WithEvents nudLimit As NumericUpDown
    Friend WithEvents cmsBackupData As ContextMenuStrip
    Friend WithEvents cmsDeleteOne As ToolStripMenuItem
    Friend WithEvents cmsDeleteAll As ToolStripMenuItem
    Friend WithEvents chkCleanFolder As CheckBox
    Friend WithEvents txtParameter As TextBox
    Friend WithEvents lblParameter As Label
    Friend WithEvents btnAdvanced As Button
    Friend WithEvents ttFullPath As ToolTip
    Friend WithEvents cmsOfficialWindows As ToolStripMenuItem
    Friend WithEvents cmsOfficialLinux As ToolStripMenuItem
    Friend WithEvents chkRecurseSubFolders As CheckBox
    Friend WithEvents cboOS As ComboBox
    Friend WithEvents cmsAdvanced As ContextMenuStrip
    Friend WithEvents ttHelp As ToolTip
    Friend WithEvents cmsLaunchSettings As ToolStripMenuItem
    Friend WithEvents tabGameManager As TabControl
    Friend WithEvents tbConfig As TabPage
    Friend WithEvents tbGameInfo As TabPage
    Friend WithEvents tbBackupInfo As TabPage
    Friend WithEvents btnAppPathBrowse As Button
    Friend WithEvents lblGamePath As Label
    Friend WithEvents txtAppPath As TextBox
    Friend WithEvents lblComments As Label
    Friend WithEvents txtComments As TextBox
    Friend WithEvents btnMarkAsRestored As Button
    Friend WithEvents btnBackupData As Button
    Friend WithEvents btnOpenBackupFolder As Button
    Friend WithEvents lblBackupFileData As LinkLabel
    Friend WithEvents lblRemote As Label
    Friend WithEvents lblRestorePathData As LinkLabel
    Friend WithEvents lblLocalData As Label
    Friend WithEvents cboRemoteBackup As ComboBox
    Friend WithEvents lblBackupFile As Label
    Friend WithEvents lblLocalBackupData As Label
    Friend WithEvents lblRestorePath As Label
    Friend WithEvents grpCoreConfig As GroupBox
    Friend WithEvents grpGameInfo As GroupBox
    Friend WithEvents grpBackupInfo As GroupBox
    Friend WithEvents cboFilters As ComboBox
    Friend WithEvents lblFilters As Label
    Friend WithEvents cmsWineConfig As ToolStripMenuItem
    Friend WithEvents btnProcessOptions As Button
    Friend WithEvents cmsProcessOptions As ContextMenuStrip
    Friend WithEvents cmsRegEx As ToolStripMenuItem
    Friend WithEvents cmsUseWindowTitle As ToolStripMenuItem
    Friend WithEvents lblGameTags As LinkLabel
    Friend WithEvents lblTags As Label
    Friend WithEvents btnRestore As Button
    Friend WithEvents btnBackup As Button
    Friend WithEvents cmsImportData As ToolStripMenuItem
    Friend WithEvents btnLinks As Button
    Friend WithEvents cmsLinks As ContextMenuStrip
    Friend WithEvents cmsMonitorOptions As ContextMenuStrip
    Friend WithEvents btnMonitorOptions As Button
    Friend WithEvents cmsEnabled As ToolStripMenuItem
    Friend WithEvents cmsMonitorOnly As ToolStripMenuItem
    Friend WithEvents cmsLinkConfiguration As ToolStripMenuItem
    Friend WithEvents cmsLinkProcess As ToolStripMenuItem
    Friend WithEvents btnGameID As Button
    Friend WithEvents chkDifferentialBackup As CheckBox
    Friend WithEvents lblInterval As Label
    Friend WithEvents nudInterval As NumericUpDown
    Friend WithEvents cmsURL As ToolStripMenuItem
    Friend WithEvents cmsLudusavi As ToolStripMenuItem
    Friend WithEvents btnCopy As Button
    Friend WithEvents btnOpenSaveFolder As Button
    Friend WithEvents btnOpenGameFolder As Button
    Friend WithEvents lblTimeIntervalMinutes As Label
    Friend WithEvents nudTimedInterval As NumericUpDown
    Friend WithEvents chkTimedBackup As CheckBox
End Class
