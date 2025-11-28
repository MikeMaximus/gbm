<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.gMonTray = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gMonTrayMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.gMonTrayNotification = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayMon = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayQuickSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayOpenBackupFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileFullBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileFullRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportOfficialLinux = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportOfficialWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportLudusavi = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileImportURL = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFileExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupAddWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupGameManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupTags = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupLauncherManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupProcessManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupCustomVariables = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsCompact = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsImportBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsImportBackupFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsImportBackupFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayLogClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayLogSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsSessions = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayExitSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.bwMonitor = New System.ComponentModel.BackgroundWorker()
        Me.gMonStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.gMonStripCollapse = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripAdminButton = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripTxtStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripStatusButton = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonMainMenu = New System.Windows.Forms.MenuStrip()
        Me.gMonFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileMonitor = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFullSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonOpenBackupFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileFullBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileFullRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonExitSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportOfficialLinux = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportOfficialWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportLudusavi = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileImportURL = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSettingsSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonImportExportSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupGameManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupAddWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupTags = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupLauncherManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupProcessManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupCustomVariables = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsCompact = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsImportBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsImportBackupFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsImportBackupFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonLogClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonLogSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsSessions = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpWebSite = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpCheckforUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonNotification = New System.Windows.Forms.ToolStripMenuItem()
        Me.slcMain = New System.Windows.Forms.SplitContainer()
        Me.pnlLeft = New System.Windows.Forms.Panel()
        Me.btnClearSelected = New System.Windows.Forms.Button()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lstGames = New System.Windows.Forms.ListBox()
        Me.pnlRight = New System.Windows.Forms.Panel()
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnCancelOperation = New System.Windows.Forms.Button()
        Me.pbTime = New System.Windows.Forms.PictureBox()
        Me.lblLastActionTitle = New System.Windows.Forms.Label()
        Me.lblStatus3 = New System.Windows.Forms.Label()
        Me.lblLastAction = New System.Windows.Forms.Label()
        Me.lblStatus2 = New System.Windows.Forms.Label()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.lblStatus1 = New System.Windows.Forms.Label()
        Me.lblGameTitle = New System.Windows.Forms.Label()
        Me.lblTimeSpent = New System.Windows.Forms.Label()
        Me.gMonTrayMenu.SuspendLayout()
        Me.gMonStatusStrip.SuspendLayout()
        Me.gMonMainMenu.SuspendLayout()
        CType(Me.slcMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slcMain.Panel1.SuspendLayout()
        Me.slcMain.Panel2.SuspendLayout()
        Me.slcMain.SuspendLayout()
        Me.pnlLeft.SuspendLayout()
        Me.pnlRight.SuspendLayout()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gMonTray
        '
        Me.gMonTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.gMonTray.BalloonTipTitle = "GBM"
        Me.gMonTray.ContextMenuStrip = Me.gMonTrayMenu
        Me.gMonTray.Icon = CType(resources.GetObject("gMonTray.Icon"), System.Drawing.Icon)
        Me.gMonTray.Text = "GBM"
        Me.gMonTray.Visible = True
        '
        'gMonTrayMenu
        '
        Me.gMonTrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayNotification, Me.gMonTrayShow, Me.gMonTrayMon, Me.gMonTraySettings, Me.gMonTrayQuickSep, Me.gMonTrayFile, Me.gMonTraySetup, Me.gMonTrayTools, Me.gMonTrayExitSep, Me.gMonTrayExit})
        Me.gMonTrayMenu.Name = "gMonTrayMenu"
        Me.gMonTrayMenu.Size = New System.Drawing.Size(162, 192)
        '
        'gMonTrayNotification
        '
        Me.gMonTrayNotification.Name = "gMonTrayNotification"
        Me.gMonTrayNotification.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayNotification.Text = "Notification"
        Me.gMonTrayNotification.Visible = False
        '
        'gMonTrayShow
        '
        Me.gMonTrayShow.Name = "gMonTrayShow"
        Me.gMonTrayShow.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayShow.Text = "Restore &Window"
        '
        'gMonTrayMon
        '
        Me.gMonTrayMon.Name = "gMonTrayMon"
        Me.gMonTrayMon.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayMon.Text = "S&tart Monitoring"
        '
        'gMonTraySettings
        '
        Me.gMonTraySettings.Name = "gMonTraySettings"
        Me.gMonTraySettings.Size = New System.Drawing.Size(161, 22)
        Me.gMonTraySettings.Text = "S&ettings"
        '
        'gMonTrayQuickSep
        '
        Me.gMonTrayQuickSep.Name = "gMonTrayQuickSep"
        Me.gMonTrayQuickSep.Size = New System.Drawing.Size(158, 6)
        '
        'gMonTrayFile
        '
        Me.gMonTrayFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayOpenBackupFolder, Me.gMonTrayFileFullBackup, Me.gMonTrayFileFullRestore, Me.gMonTrayFileImport, Me.gMonTrayFileExport})
        Me.gMonTrayFile.Name = "gMonTrayFile"
        Me.gMonTrayFile.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayFile.Text = "&File"
        '
        'gMonTrayOpenBackupFolder
        '
        Me.gMonTrayOpenBackupFolder.Name = "gMonTrayOpenBackupFolder"
        Me.gMonTrayOpenBackupFolder.Size = New System.Drawing.Size(181, 22)
        Me.gMonTrayOpenBackupFolder.Text = "&Open Backup Folder"
        '
        'gMonTrayFileFullBackup
        '
        Me.gMonTrayFileFullBackup.Name = "gMonTrayFileFullBackup"
        Me.gMonTrayFileFullBackup.Size = New System.Drawing.Size(181, 22)
        Me.gMonTrayFileFullBackup.Text = "Run Full &Backup"
        '
        'gMonTrayFileFullRestore
        '
        Me.gMonTrayFileFullRestore.Name = "gMonTrayFileFullRestore"
        Me.gMonTrayFileFullRestore.Size = New System.Drawing.Size(181, 22)
        Me.gMonTrayFileFullRestore.Text = "Run Full &Restore"
        '
        'gMonTrayFileImport
        '
        Me.gMonTrayFileImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayFileImportOfficial, Me.gMonTrayFileImportLudusavi, Me.gMonTrayFileImportFile, Me.gMonTrayFileImportURL})
        Me.gMonTrayFileImport.Name = "gMonTrayFileImport"
        Me.gMonTrayFileImport.Size = New System.Drawing.Size(181, 22)
        Me.gMonTrayFileImport.Text = "&Import Games"
        '
        'gMonTrayFileImportOfficial
        '
        Me.gMonTrayFileImportOfficial.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayFileImportOfficialLinux, Me.gMonTrayFileImportOfficialWindows})
        Me.gMonTrayFileImportOfficial.Name = "gMonTrayFileImportOfficial"
        Me.gMonTrayFileImportOfficial.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayFileImportOfficial.Text = "&Official List..."
        '
        'gMonTrayFileImportOfficialLinux
        '
        Me.gMonTrayFileImportOfficialLinux.Name = "gMonTrayFileImportOfficialLinux"
        Me.gMonTrayFileImportOfficialLinux.Size = New System.Drawing.Size(123, 22)
        Me.gMonTrayFileImportOfficialLinux.Text = "&Linux"
        '
        'gMonTrayFileImportOfficialWindows
        '
        Me.gMonTrayFileImportOfficialWindows.Name = "gMonTrayFileImportOfficialWindows"
        Me.gMonTrayFileImportOfficialWindows.Size = New System.Drawing.Size(123, 22)
        Me.gMonTrayFileImportOfficialWindows.Text = "&Windows"
        '
        'gMonTrayFileImportLudusavi
        '
        Me.gMonTrayFileImportLudusavi.Name = "gMonTrayFileImportLudusavi"
        Me.gMonTrayFileImportLudusavi.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayFileImportLudusavi.Text = "&Ludusavi Manifest..."
        '
        'gMonTrayFileImportFile
        '
        Me.gMonTrayFileImportFile.Name = "gMonTrayFileImportFile"
        Me.gMonTrayFileImportFile.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayFileImportFile.Text = "&File..."
        '
        'gMonTrayFileImportURL
        '
        Me.gMonTrayFileImportURL.Name = "gMonTrayFileImportURL"
        Me.gMonTrayFileImportURL.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayFileImportURL.Text = "&URL..."
        '
        'gMonTrayFileExport
        '
        Me.gMonTrayFileExport.Name = "gMonTrayFileExport"
        Me.gMonTrayFileExport.Size = New System.Drawing.Size(181, 22)
        Me.gMonTrayFileExport.Text = "&Export Games..."
        '
        'gMonTraySetup
        '
        Me.gMonTraySetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTraySetupAddWizard, Me.gMonTraySetupGameManager, Me.gMonTraySetupTags, Me.gMonTraySetupLauncherManager, Me.gMonTraySetupProcessManager, Me.gMonTraySetupCustomVariables})
        Me.gMonTraySetup.Name = "gMonTraySetup"
        Me.gMonTraySetup.Size = New System.Drawing.Size(161, 22)
        Me.gMonTraySetup.Text = "&Setup"
        '
        'gMonTraySetupAddWizard
        '
        Me.gMonTraySetupAddWizard.Name = "gMonTraySetupAddWizard"
        Me.gMonTraySetupAddWizard.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupAddWizard.Text = "Add Game &Wizard..."
        '
        'gMonTraySetupGameManager
        '
        Me.gMonTraySetupGameManager.Name = "gMonTraySetupGameManager"
        Me.gMonTraySetupGameManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupGameManager.Text = "&Game Manager..."
        '
        'gMonTraySetupTags
        '
        Me.gMonTraySetupTags.Name = "gMonTraySetupTags"
        Me.gMonTraySetupTags.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupTags.Text = "&Tag Manager..."
        '
        'gMonTraySetupLauncherManager
        '
        Me.gMonTraySetupLauncherManager.Name = "gMonTraySetupLauncherManager"
        Me.gMonTraySetupLauncherManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupLauncherManager.Text = "&Launcher Manager..."
        '
        'gMonTraySetupProcessManager
        '
        Me.gMonTraySetupProcessManager.Name = "gMonTraySetupProcessManager"
        Me.gMonTraySetupProcessManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupProcessManager.Text = "&Process Manager..."
        '
        'gMonTraySetupCustomVariables
        '
        Me.gMonTraySetupCustomVariables.Name = "gMonTraySetupCustomVariables"
        Me.gMonTraySetupCustomVariables.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupCustomVariables.Text = "Custom Path &Variables..."
        '
        'gMonTrayTools
        '
        Me.gMonTrayTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsCompact, Me.gMonTrayToolsImportBackup, Me.gMonTrayToolsLog, Me.gMonTrayToolsSessions})
        Me.gMonTrayTools.Name = "gMonTrayTools"
        Me.gMonTrayTools.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayTools.Text = "&Tools"
        '
        'gMonTrayToolsCompact
        '
        Me.gMonTrayToolsCompact.Name = "gMonTrayToolsCompact"
        Me.gMonTrayToolsCompact.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsCompact.Text = "&Compact Databases"
        '
        'gMonTrayToolsImportBackup
        '
        Me.gMonTrayToolsImportBackup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsImportBackupFiles, Me.gMonTrayToolsImportBackupFolder})
        Me.gMonTrayToolsImportBackup.Name = "gMonTrayToolsImportBackup"
        Me.gMonTrayToolsImportBackup.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsImportBackup.Text = "&Import Backup"
        '
        'gMonTrayToolsImportBackupFiles
        '
        Me.gMonTrayToolsImportBackupFiles.Name = "gMonTrayToolsImportBackupFiles"
        Me.gMonTrayToolsImportBackupFiles.Size = New System.Drawing.Size(107, 22)
        Me.gMonTrayToolsImportBackupFiles.Text = "&Files"
        '
        'gMonTrayToolsImportBackupFolder
        '
        Me.gMonTrayToolsImportBackupFolder.Name = "gMonTrayToolsImportBackupFolder"
        Me.gMonTrayToolsImportBackupFolder.Size = New System.Drawing.Size(107, 22)
        Me.gMonTrayToolsImportBackupFolder.Text = "F&older"
        '
        'gMonTrayToolsLog
        '
        Me.gMonTrayToolsLog.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayLogClear, Me.gMonTrayLogSave})
        Me.gMonTrayToolsLog.Name = "gMonTrayToolsLog"
        Me.gMonTrayToolsLog.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsLog.Text = "&Log"
        '
        'gMonTrayLogClear
        '
        Me.gMonTrayLogClear.Name = "gMonTrayLogClear"
        Me.gMonTrayLogClear.Size = New System.Drawing.Size(101, 22)
        Me.gMonTrayLogClear.Text = "&Clear"
        '
        'gMonTrayLogSave
        '
        Me.gMonTrayLogSave.Name = "gMonTrayLogSave"
        Me.gMonTrayLogSave.Size = New System.Drawing.Size(101, 22)
        Me.gMonTrayLogSave.Text = "&Save"
        '
        'gMonTrayToolsSessions
        '
        Me.gMonTrayToolsSessions.Name = "gMonTrayToolsSessions"
        Me.gMonTrayToolsSessions.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsSessions.Text = "&Session Viewer..."
        '
        'gMonTrayExitSep
        '
        Me.gMonTrayExitSep.Name = "gMonTrayExitSep"
        Me.gMonTrayExitSep.Size = New System.Drawing.Size(158, 6)
        '
        'gMonTrayExit
        '
        Me.gMonTrayExit.Name = "gMonTrayExit"
        Me.gMonTrayExit.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayExit.Text = "E&xit"
        '
        'bwMonitor
        '
        Me.bwMonitor.WorkerSupportsCancellation = True
        '
        'gMonStatusStrip
        '
        Me.gMonStatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gMonStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonStripCollapse, Me.gMonStripAdminButton, Me.gMonStripTxtStatus, Me.gMonStripStatusButton})
        Me.gMonStatusStrip.Location = New System.Drawing.Point(0, 439)
        Me.gMonStatusStrip.Name = "gMonStatusStrip"
        Me.gMonStatusStrip.ShowItemToolTips = True
        Me.gMonStatusStrip.Size = New System.Drawing.Size(784, 22)
        Me.gMonStatusStrip.SizingGrip = False
        Me.gMonStatusStrip.TabIndex = 2
        '
        'gMonStripCollapse
        '
        Me.gMonStripCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.gMonStripCollapse.Image = Global.GBM.My.Resources.Resources.frmMain_Collapse_Left
        Me.gMonStripCollapse.Margin = New System.Windows.Forms.Padding(5, 3, 0, 2)
        Me.gMonStripCollapse.Name = "gMonStripCollapse"
        Me.gMonStripCollapse.Size = New System.Drawing.Size(16, 17)
        '
        'gMonStripAdminButton
        '
        Me.gMonStripAdminButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.gMonStripAdminButton.Image = Global.GBM.My.Resources.Resources.frmMain_User
        Me.gMonStripAdminButton.Margin = New System.Windows.Forms.Padding(5, 3, 0, 2)
        Me.gMonStripAdminButton.Name = "gMonStripAdminButton"
        Me.gMonStripAdminButton.Size = New System.Drawing.Size(16, 17)
        '
        'gMonStripTxtStatus
        '
        Me.gMonStripTxtStatus.Margin = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.gMonStripTxtStatus.Name = "gMonStripTxtStatus"
        Me.gMonStripTxtStatus.Size = New System.Drawing.Size(634, 22)
        Me.gMonStripTxtStatus.Spring = True
        Me.gMonStripTxtStatus.Text = "Monitor Status"
        Me.gMonStripTxtStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gMonStripStatusButton
        '
        Me.gMonStripStatusButton.Name = "gMonStripStatusButton"
        Me.gMonStripStatusButton.Size = New System.Drawing.Size(88, 17)
        Me.gMonStripStatusButton.Text = "Monitor Status:"
        Me.gMonStripStatusButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'gMonMainMenu
        '
        Me.gMonMainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFile, Me.gMonSetup, Me.gMonTools, Me.gMonHelp, Me.gMonNotification})
        Me.gMonMainMenu.Location = New System.Drawing.Point(0, 0)
        Me.gMonMainMenu.Name = "gMonMainMenu"
        Me.gMonMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.gMonMainMenu.Size = New System.Drawing.Size(784, 24)
        Me.gMonMainMenu.TabIndex = 0
        Me.gMonMainMenu.Text = "MenuStrip1"
        '
        'gMonFile
        '
        Me.gMonFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFileMonitor, Me.gMonFullSep, Me.gMonOpenBackupFolder, Me.gMonFileFullBackup, Me.gMonFileFullRestore, Me.gMonExitSep, Me.gMonFileImport, Me.gMonFileExport, Me.gMonSettingsSep, Me.gMonFileSettings, Me.gMonImportExportSep, Me.gMonFileExit})
        Me.gMonFile.Name = "gMonFile"
        Me.gMonFile.Size = New System.Drawing.Size(37, 20)
        Me.gMonFile.Text = "&File"
        '
        'gMonFileMonitor
        '
        Me.gMonFileMonitor.Name = "gMonFileMonitor"
        Me.gMonFileMonitor.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileMonitor.Text = "Start &Monitoring"
        '
        'gMonFullSep
        '
        Me.gMonFullSep.Name = "gMonFullSep"
        Me.gMonFullSep.Size = New System.Drawing.Size(178, 6)
        '
        'gMonOpenBackupFolder
        '
        Me.gMonOpenBackupFolder.Name = "gMonOpenBackupFolder"
        Me.gMonOpenBackupFolder.Size = New System.Drawing.Size(181, 22)
        Me.gMonOpenBackupFolder.Text = "&Open Backup Folder"
        '
        'gMonFileFullBackup
        '
        Me.gMonFileFullBackup.Name = "gMonFileFullBackup"
        Me.gMonFileFullBackup.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileFullBackup.Text = "Run Full &Backup"
        '
        'gMonFileFullRestore
        '
        Me.gMonFileFullRestore.Name = "gMonFileFullRestore"
        Me.gMonFileFullRestore.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileFullRestore.Text = "Run Full &Restore"
        '
        'gMonExitSep
        '
        Me.gMonExitSep.Name = "gMonExitSep"
        Me.gMonExitSep.Size = New System.Drawing.Size(178, 6)
        '
        'gMonFileImport
        '
        Me.gMonFileImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFileImportOfficial, Me.gMonFileImportLudusavi, Me.gMonFileImportFile, Me.gMonFileImportURL})
        Me.gMonFileImport.Name = "gMonFileImport"
        Me.gMonFileImport.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileImport.Text = "&Import Games"
        '
        'gMonFileImportOfficial
        '
        Me.gMonFileImportOfficial.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFileImportOfficialLinux, Me.gMonFileImportOfficialWindows})
        Me.gMonFileImportOfficial.Name = "gMonFileImportOfficial"
        Me.gMonFileImportOfficial.Size = New System.Drawing.Size(179, 22)
        Me.gMonFileImportOfficial.Text = "&Official List..."
        '
        'gMonFileImportOfficialLinux
        '
        Me.gMonFileImportOfficialLinux.Name = "gMonFileImportOfficialLinux"
        Me.gMonFileImportOfficialLinux.Size = New System.Drawing.Size(132, 22)
        Me.gMonFileImportOfficialLinux.Text = "&Linux..."
        '
        'gMonFileImportOfficialWindows
        '
        Me.gMonFileImportOfficialWindows.Name = "gMonFileImportOfficialWindows"
        Me.gMonFileImportOfficialWindows.Size = New System.Drawing.Size(132, 22)
        Me.gMonFileImportOfficialWindows.Text = "&Windows..."
        '
        'gMonFileImportLudusavi
        '
        Me.gMonFileImportLudusavi.Name = "gMonFileImportLudusavi"
        Me.gMonFileImportLudusavi.Size = New System.Drawing.Size(179, 22)
        Me.gMonFileImportLudusavi.Text = "&Ludusavi Manifest..."
        '
        'gMonFileImportFile
        '
        Me.gMonFileImportFile.Name = "gMonFileImportFile"
        Me.gMonFileImportFile.Size = New System.Drawing.Size(179, 22)
        Me.gMonFileImportFile.Text = "&File..."
        '
        'gMonFileImportURL
        '
        Me.gMonFileImportURL.Name = "gMonFileImportURL"
        Me.gMonFileImportURL.Size = New System.Drawing.Size(179, 22)
        Me.gMonFileImportURL.Text = "&URL..."
        '
        'gMonFileExport
        '
        Me.gMonFileExport.Name = "gMonFileExport"
        Me.gMonFileExport.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileExport.Text = "&Export Games..."
        '
        'gMonSettingsSep
        '
        Me.gMonSettingsSep.Name = "gMonSettingsSep"
        Me.gMonSettingsSep.Size = New System.Drawing.Size(178, 6)
        '
        'gMonFileSettings
        '
        Me.gMonFileSettings.Name = "gMonFileSettings"
        Me.gMonFileSettings.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileSettings.Text = "&Settings..."
        '
        'gMonImportExportSep
        '
        Me.gMonImportExportSep.Name = "gMonImportExportSep"
        Me.gMonImportExportSep.Size = New System.Drawing.Size(178, 6)
        '
        'gMonFileExit
        '
        Me.gMonFileExit.Name = "gMonFileExit"
        Me.gMonFileExit.Size = New System.Drawing.Size(181, 22)
        Me.gMonFileExit.Text = "E&xit"
        '
        'gMonSetup
        '
        Me.gMonSetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonSetupGameManager, Me.gMonSetupAddWizard, Me.gMonSetupTags, Me.gMonSetupLauncherManager, Me.gMonSetupProcessManager, Me.gMonSetupCustomVariables})
        Me.gMonSetup.Name = "gMonSetup"
        Me.gMonSetup.Size = New System.Drawing.Size(49, 20)
        Me.gMonSetup.Text = "&Setup"
        '
        'gMonSetupGameManager
        '
        Me.gMonSetupGameManager.Name = "gMonSetupGameManager"
        Me.gMonSetupGameManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupGameManager.Text = "&Game Manager..."
        '
        'gMonSetupAddWizard
        '
        Me.gMonSetupAddWizard.Name = "gMonSetupAddWizard"
        Me.gMonSetupAddWizard.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupAddWizard.Text = "Add Game &Wizard..."
        '
        'gMonSetupTags
        '
        Me.gMonSetupTags.Name = "gMonSetupTags"
        Me.gMonSetupTags.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupTags.Text = "&Tag Manager..."
        '
        'gMonSetupLauncherManager
        '
        Me.gMonSetupLauncherManager.Name = "gMonSetupLauncherManager"
        Me.gMonSetupLauncherManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupLauncherManager.Text = "&Launcher Manager..."
        '
        'gMonSetupProcessManager
        '
        Me.gMonSetupProcessManager.Name = "gMonSetupProcessManager"
        Me.gMonSetupProcessManager.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupProcessManager.Text = "&Process Manager..."
        '
        'gMonSetupCustomVariables
        '
        Me.gMonSetupCustomVariables.Name = "gMonSetupCustomVariables"
        Me.gMonSetupCustomVariables.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupCustomVariables.Text = "Custom Path &Variables..."
        '
        'gMonTools
        '
        Me.gMonTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsCompact, Me.gMonToolsImportBackup, Me.gMonToolsLog, Me.gMonToolsSessions})
        Me.gMonTools.Name = "gMonTools"
        Me.gMonTools.Size = New System.Drawing.Size(47, 20)
        Me.gMonTools.Text = "&Tools"
        '
        'gMonToolsCompact
        '
        Me.gMonToolsCompact.Name = "gMonToolsCompact"
        Me.gMonToolsCompact.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsCompact.Text = "&Compact Databases"
        '
        'gMonToolsImportBackup
        '
        Me.gMonToolsImportBackup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsImportBackupFiles, Me.gMonToolsImportBackupFolder})
        Me.gMonToolsImportBackup.Name = "gMonToolsImportBackup"
        Me.gMonToolsImportBackup.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsImportBackup.Text = "&Import Backup"
        '
        'gMonToolsImportBackupFiles
        '
        Me.gMonToolsImportBackupFiles.Name = "gMonToolsImportBackupFiles"
        Me.gMonToolsImportBackupFiles.Size = New System.Drawing.Size(107, 22)
        Me.gMonToolsImportBackupFiles.Text = "&Files"
        '
        'gMonToolsImportBackupFolder
        '
        Me.gMonToolsImportBackupFolder.Name = "gMonToolsImportBackupFolder"
        Me.gMonToolsImportBackupFolder.Size = New System.Drawing.Size(107, 22)
        Me.gMonToolsImportBackupFolder.Text = "F&older"
        '
        'gMonToolsLog
        '
        Me.gMonToolsLog.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonLogClear, Me.gMonLogSave})
        Me.gMonToolsLog.Name = "gMonToolsLog"
        Me.gMonToolsLog.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsLog.Text = "&Log"
        '
        'gMonLogClear
        '
        Me.gMonLogClear.Name = "gMonLogClear"
        Me.gMonLogClear.Size = New System.Drawing.Size(101, 22)
        Me.gMonLogClear.Text = "&Clear"
        '
        'gMonLogSave
        '
        Me.gMonLogSave.Name = "gMonLogSave"
        Me.gMonLogSave.Size = New System.Drawing.Size(101, 22)
        Me.gMonLogSave.Text = "&Save"
        '
        'gMonToolsSessions
        '
        Me.gMonToolsSessions.Name = "gMonToolsSessions"
        Me.gMonToolsSessions.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsSessions.Text = "&Session Viewer..."
        '
        'gMonHelp
        '
        Me.gMonHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonHelpWebSite, Me.gMonHelpManual, Me.gMonHelpCheckforUpdates, Me.gMonHelpAbout})
        Me.gMonHelp.Name = "gMonHelp"
        Me.gMonHelp.Size = New System.Drawing.Size(44, 20)
        Me.gMonHelp.Text = "&Help"
        '
        'gMonHelpWebSite
        '
        Me.gMonHelpWebSite.Name = "gMonHelpWebSite"
        Me.gMonHelpWebSite.Size = New System.Drawing.Size(180, 22)
        Me.gMonHelpWebSite.Text = "&Official Web Site..."
        '
        'gMonHelpManual
        '
        Me.gMonHelpManual.Name = "gMonHelpManual"
        Me.gMonHelpManual.Size = New System.Drawing.Size(180, 22)
        Me.gMonHelpManual.Text = "Online &Manual..."
        '
        'gMonHelpCheckforUpdates
        '
        Me.gMonHelpCheckforUpdates.Name = "gMonHelpCheckforUpdates"
        Me.gMonHelpCheckforUpdates.Size = New System.Drawing.Size(180, 22)
        Me.gMonHelpCheckforUpdates.Text = "&Check for Updates..."
        '
        'gMonHelpAbout
        '
        Me.gMonHelpAbout.Name = "gMonHelpAbout"
        Me.gMonHelpAbout.Size = New System.Drawing.Size(180, 22)
        Me.gMonHelpAbout.Text = "&About"
        '
        'gMonNotification
        '
        Me.gMonNotification.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.gMonNotification.Name = "gMonNotification"
        Me.gMonNotification.Size = New System.Drawing.Size(82, 20)
        Me.gMonNotification.Text = "Notification"
        Me.gMonNotification.Visible = False
        '
        'slcMain
        '
        Me.slcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slcMain.Location = New System.Drawing.Point(0, 24)
        Me.slcMain.Name = "slcMain"
        '
        'slcMain.Panel1
        '
        Me.slcMain.Panel1.Controls.Add(Me.pnlLeft)
        '
        'slcMain.Panel2
        '
        Me.slcMain.Panel2.Controls.Add(Me.pnlRight)
        Me.slcMain.Size = New System.Drawing.Size(784, 415)
        Me.slcMain.SplitterDistance = 258
        Me.slcMain.SplitterWidth = 5
        Me.slcMain.TabIndex = 1
        Me.slcMain.TabStop = False
        '
        'pnlLeft
        '
        Me.pnlLeft.Controls.Add(Me.btnClearSelected)
        Me.pnlLeft.Controls.Add(Me.lblSearch)
        Me.pnlLeft.Controls.Add(Me.txtSearch)
        Me.pnlLeft.Controls.Add(Me.lstGames)
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(258, 415)
        Me.pnlLeft.TabIndex = 0
        '
        'btnClearSelected
        '
        Me.btnClearSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearSelected.Image = Global.GBM.My.Resources.Resources.frmMain_Cancel_Small
        Me.btnClearSelected.Location = New System.Drawing.Point(228, 10)
        Me.btnClearSelected.Name = "btnClearSelected"
        Me.btnClearSelected.Size = New System.Drawing.Size(24, 24)
        Me.btnClearSelected.TabIndex = 6
        Me.btnClearSelected.UseVisualStyleBackColor = True
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(6, 16)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 4
        Me.lblSearch.Text = "Search:"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(56, 13)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(166, 20)
        Me.txtSearch.TabIndex = 5
        '
        'lstGames
        '
        Me.lstGames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstGames.FormattingEnabled = True
        Me.lstGames.IntegralHeight = False
        Me.lstGames.Location = New System.Drawing.Point(9, 39)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.Size = New System.Drawing.Size(243, 366)
        Me.lstGames.TabIndex = 7
        '
        'pnlRight
        '
        Me.pnlRight.Controls.Add(Me.btnBackup)
        Me.pnlRight.Controls.Add(Me.btnRestore)
        Me.pnlRight.Controls.Add(Me.btnEdit)
        Me.pnlRight.Controls.Add(Me.btnPlay)
        Me.pnlRight.Controls.Add(Me.txtLog)
        Me.pnlRight.Controls.Add(Me.btnCancelOperation)
        Me.pnlRight.Controls.Add(Me.pbTime)
        Me.pnlRight.Controls.Add(Me.lblLastActionTitle)
        Me.pnlRight.Controls.Add(Me.lblStatus3)
        Me.pnlRight.Controls.Add(Me.lblLastAction)
        Me.pnlRight.Controls.Add(Me.lblStatus2)
        Me.pnlRight.Controls.Add(Me.pbIcon)
        Me.pnlRight.Controls.Add(Me.lblStatus1)
        Me.pnlRight.Controls.Add(Me.lblGameTitle)
        Me.pnlRight.Controls.Add(Me.lblTimeSpent)
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRight.Location = New System.Drawing.Point(0, 0)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(521, 415)
        Me.pnlRight.TabIndex = 0
        '
        'btnBackup
        '
        Me.btnBackup.Image = Global.GBM.My.Resources.Resources.Multi_Backup
        Me.btnBackup.Location = New System.Drawing.Point(101, 119)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(80, 50)
        Me.btnBackup.TabIndex = 25
        Me.btnBackup.Text = "&Backup"
        Me.btnBackup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Image = Global.GBM.My.Resources.Resources.Multi_Restore
        Me.btnRestore.Location = New System.Drawing.Point(187, 119)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(80, 50)
        Me.btnRestore.TabIndex = 26
        Me.btnRestore.Text = "&Restore"
        Me.btnRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Image = Global.GBM.My.Resources.Resources.Multi_Edit
        Me.btnEdit.Location = New System.Drawing.Point(15, 119)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(80, 50)
        Me.btnEdit.TabIndex = 24
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Image = Global.GBM.My.Resources.Resources.frmMain_Play
        Me.btnPlay.Location = New System.Drawing.Point(273, 119)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(80, 50)
        Me.btnPlay.TabIndex = 27
        Me.btnPlay.Text = "&Play"
        Me.btnPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLog.Location = New System.Drawing.Point(15, 188)
        Me.txtLog.MaxLength = 524288
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(488, 215)
        Me.txtLog.TabIndex = 32
        Me.txtLog.TabStop = False
        '
        'btnCancelOperation
        '
        Me.btnCancelOperation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelOperation.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancelOperation.Location = New System.Drawing.Point(423, 119)
        Me.btnCancelOperation.Name = "btnCancelOperation"
        Me.btnCancelOperation.Size = New System.Drawing.Size(80, 50)
        Me.btnCancelOperation.TabIndex = 31
        Me.btnCancelOperation.Text = "&Cancel"
        Me.btnCancelOperation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancelOperation.UseVisualStyleBackColor = True
        '
        'pbTime
        '
        Me.pbTime.Location = New System.Drawing.Point(39, 89)
        Me.pbTime.Name = "pbTime"
        Me.pbTime.Size = New System.Drawing.Size(24, 24)
        Me.pbTime.TabIndex = 33
        Me.pbTime.TabStop = False
        '
        'lblLastActionTitle
        '
        Me.lblLastActionTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastActionTitle.Location = New System.Drawing.Point(12, 172)
        Me.lblLastActionTitle.Name = "lblLastActionTitle"
        Me.lblLastActionTitle.Size = New System.Drawing.Size(100, 13)
        Me.lblLastActionTitle.TabIndex = 28
        Me.lblLastActionTitle.Text = "Last Action:"
        '
        'lblStatus3
        '
        Me.lblStatus3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus3.AutoEllipsis = True
        Me.lblStatus3.Location = New System.Drawing.Point(69, 70)
        Me.lblStatus3.Name = "lblStatus3"
        Me.lblStatus3.Size = New System.Drawing.Size(434, 13)
        Me.lblStatus3.TabIndex = 22
        Me.lblStatus3.Text = "Status Text"
        Me.lblStatus3.UseMnemonic = False
        '
        'lblLastAction
        '
        Me.lblLastAction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLastAction.AutoEllipsis = True
        Me.lblLastAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastAction.Location = New System.Drawing.Point(118, 172)
        Me.lblLastAction.Name = "lblLastAction"
        Me.lblLastAction.Size = New System.Drawing.Size(384, 13)
        Me.lblLastAction.TabIndex = 30
        Me.lblLastAction.Text = "Last Action"
        Me.lblLastAction.UseMnemonic = False
        '
        'lblStatus2
        '
        Me.lblStatus2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus2.AutoEllipsis = True
        Me.lblStatus2.Location = New System.Drawing.Point(69, 52)
        Me.lblStatus2.Name = "lblStatus2"
        Me.lblStatus2.Size = New System.Drawing.Size(434, 13)
        Me.lblStatus2.TabIndex = 21
        Me.lblStatus2.Text = "Status Text"
        Me.lblStatus2.UseMnemonic = False
        '
        'pbIcon
        '
        Me.pbIcon.Location = New System.Drawing.Point(15, 13)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 29
        Me.pbIcon.TabStop = False
        '
        'lblStatus1
        '
        Me.lblStatus1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus1.AutoEllipsis = True
        Me.lblStatus1.Location = New System.Drawing.Point(69, 34)
        Me.lblStatus1.Name = "lblStatus1"
        Me.lblStatus1.Size = New System.Drawing.Size(433, 13)
        Me.lblStatus1.TabIndex = 20
        Me.lblStatus1.Text = "Status Text "
        Me.lblStatus1.UseMnemonic = False
        '
        'lblGameTitle
        '
        Me.lblGameTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGameTitle.AutoEllipsis = True
        Me.lblGameTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGameTitle.Location = New System.Drawing.Point(69, 10)
        Me.lblGameTitle.Name = "lblGameTitle"
        Me.lblGameTitle.Size = New System.Drawing.Size(434, 16)
        Me.lblGameTitle.TabIndex = 19
        Me.lblGameTitle.Text = "Game Title"
        Me.lblGameTitle.UseMnemonic = False
        '
        'lblTimeSpent
        '
        Me.lblTimeSpent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTimeSpent.AutoEllipsis = True
        Me.lblTimeSpent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeSpent.Location = New System.Drawing.Point(69, 93)
        Me.lblTimeSpent.Name = "lblTimeSpent"
        Me.lblTimeSpent.Size = New System.Drawing.Size(434, 16)
        Me.lblTimeSpent.TabIndex = 23
        Me.lblTimeSpent.Text = "0 Hours"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.slcMain)
        Me.Controls.Add(Me.gMonStatusStrip)
        Me.Controls.Add(Me.gMonMainMenu)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.gMonMainMenu
        Me.Name = "frmMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Backup Monitor"
        Me.gMonTrayMenu.ResumeLayout(False)
        Me.gMonStatusStrip.ResumeLayout(False)
        Me.gMonStatusStrip.PerformLayout()
        Me.gMonMainMenu.ResumeLayout(False)
        Me.gMonMainMenu.PerformLayout()
        Me.slcMain.Panel1.ResumeLayout(False)
        Me.slcMain.Panel2.ResumeLayout(False)
        CType(Me.slcMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slcMain.ResumeLayout(False)
        Me.pnlLeft.ResumeLayout(False)
        Me.pnlLeft.PerformLayout()
        Me.pnlRight.ResumeLayout(False)
        Me.pnlRight.PerformLayout()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gMonTray As System.Windows.Forms.NotifyIcon
    Friend WithEvents gMonTrayMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents gMonTrayExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayExitSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bwMonitor As System.ComponentModel.BackgroundWorker
    Friend WithEvents gMonTrayMon As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents gMonStripTxtStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents gMonTraySettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayQuickSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents gMonTraySetup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonMainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents gMonFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonFileMonitor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonFullSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents gMonFileSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonExitSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents gMonFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupGameManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupCustomVariables As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupGameManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupCustomVariables As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpCheckforUpdates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupTags As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupTags As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonNotification As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayNotification As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpWebSite As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsLog As ToolStripMenuItem
    Friend WithEvents gMonLogClear As ToolStripMenuItem
    Friend WithEvents gMonLogSave As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsLog As ToolStripMenuItem
    Friend WithEvents gMonTrayLogClear As ToolStripMenuItem
    Friend WithEvents gMonTrayLogSave As ToolStripMenuItem
    Friend WithEvents gMonStripAdminButton As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents gMonStripStatusButton As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents gMonToolsSessions As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsSessions As ToolStripMenuItem
    Friend WithEvents gMonTraySetupProcessManager As ToolStripMenuItem
    Friend WithEvents gMonSetupProcessManager As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsImportBackup As ToolStripMenuItem
    Friend WithEvents gMonToolsImportBackup As ToolStripMenuItem
    Friend WithEvents gMonToolsImportBackupFiles As ToolStripMenuItem
    Friend WithEvents gMonToolsImportBackupFolder As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsImportBackupFiles As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsImportBackupFolder As ToolStripMenuItem
    Friend WithEvents gMonFileFullBackup As ToolStripMenuItem
    Friend WithEvents gMonFileFullRestore As ToolStripMenuItem
    Friend WithEvents gMonSettingsSep As ToolStripSeparator
    Friend WithEvents gMonSetupLauncherManager As ToolStripMenuItem
    Friend WithEvents gMonTraySetupLauncherManager As ToolStripMenuItem
    Friend WithEvents slcMain As SplitContainer
    Friend WithEvents gMonStripCollapse As ToolStripStatusLabel
    Friend WithEvents gMonFileImport As ToolStripMenuItem
    Friend WithEvents gMonFileImportOfficial As ToolStripMenuItem
    Friend WithEvents gMonFileImportOfficialLinux As ToolStripMenuItem
    Friend WithEvents gMonFileImportOfficialWindows As ToolStripMenuItem
    Friend WithEvents gMonFileImportFile As ToolStripMenuItem
    Friend WithEvents gMonFileImportURL As ToolStripMenuItem
    Friend WithEvents gMonFileExport As ToolStripMenuItem
    Friend WithEvents gMonImportExportSep As ToolStripSeparator
    Friend WithEvents gMonTrayFile As ToolStripMenuItem
    Friend WithEvents gMonTrayFileFullBackup As ToolStripMenuItem
    Friend WithEvents gMonTrayFileFullRestore As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImport As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportOfficial As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportOfficialLinux As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportOfficialWindows As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportFile As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportURL As ToolStripMenuItem
    Friend WithEvents gMonTrayFileExport As ToolStripMenuItem
    Friend WithEvents gMonFileImportLudusavi As ToolStripMenuItem
    Friend WithEvents gMonTrayFileImportLudusavi As ToolStripMenuItem
    Friend WithEvents gMonOpenBackupFolder As ToolStripMenuItem
    Friend WithEvents gMonTrayOpenBackupFolder As ToolStripMenuItem
    Friend WithEvents pnlLeft As Panel
    Friend WithEvents btnClearSelected As Button
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lstGames As ListBox
    Friend WithEvents pnlRight As Panel
    Friend WithEvents btnBackup As Button
    Friend WithEvents btnRestore As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnPlay As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents btnCancelOperation As Button
    Friend WithEvents pbTime As PictureBox
    Friend WithEvents lblLastActionTitle As Label
    Friend WithEvents lblStatus3 As Label
    Friend WithEvents lblLastAction As Label
    Friend WithEvents lblStatus2 As Label
    Friend WithEvents pbIcon As PictureBox
    Friend WithEvents lblStatus1 As Label
    Friend WithEvents lblGameTitle As Label
    Friend WithEvents lblTimeSpent As Label
End Class
