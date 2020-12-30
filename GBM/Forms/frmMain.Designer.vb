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
        Me.gMonTraySep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayFullBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayFullRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTraySettings = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.gMonTrayToolsSyncGameID = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsSyncGameIDOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsSyncGameIDFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.bwMonitor = New System.ComponentModel.BackgroundWorker()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.gMonStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.gMonStripCollapse = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripAdminButton = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripTxtStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripStatusButton = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonMainMenu = New System.Windows.Forms.MenuStrip()
        Me.gMonFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileMonitor = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFullSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileFullBackup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileFullRestore = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSettingsSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonExitSep = New System.Windows.Forms.ToolStripSeparator()
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
        Me.gMonToolsSyncGameID = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsSyncGameIDOfficial = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsSyncGameIDFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpWebSite = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpCheckforUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonNotification = New System.Windows.Forms.ToolStripMenuItem()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.lblGameTitle = New System.Windows.Forms.Label()
        Me.lblLastAction = New System.Windows.Forms.Label()
        Me.lblLastActionTitle = New System.Windows.Forms.Label()
        Me.lblTimeSpent = New System.Windows.Forms.Label()
        Me.btnCancelOperation = New System.Windows.Forms.Button()
        Me.lblStatus1 = New System.Windows.Forms.Label()
        Me.lblStatus2 = New System.Windows.Forms.Label()
        Me.lblStatus3 = New System.Windows.Forms.Label()
        Me.pbTime = New System.Windows.Forms.PictureBox()
        Me.slcMain = New System.Windows.Forms.SplitContainer()
        Me.btnClearSelected = New System.Windows.Forms.Button()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lstGames = New System.Windows.Forms.ListBox()
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.gMonTrayMenu.SuspendLayout()
        Me.gMonStatusStrip.SuspendLayout()
        Me.gMonMainMenu.SuspendLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slcMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slcMain.Panel1.SuspendLayout()
        Me.slcMain.Panel2.SuspendLayout()
        Me.slcMain.SuspendLayout()
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
        Me.gMonTrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayNotification, Me.gMonTrayShow, Me.gMonTrayMon, Me.gMonTraySep2, Me.gMonTrayFullBackup, Me.gMonTrayFullRestore, Me.gMonTraySep3, Me.gMonTraySettings, Me.gMonTraySetup, Me.gMonTrayTools, Me.gMonTraySep1, Me.gMonTrayExit})
        Me.gMonTrayMenu.Name = "gMonTrayMenu"
        Me.gMonTrayMenu.Size = New System.Drawing.Size(162, 220)
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
        'gMonTraySep2
        '
        Me.gMonTraySep2.Name = "gMonTraySep2"
        Me.gMonTraySep2.Size = New System.Drawing.Size(158, 6)
        '
        'gMonTrayFullBackup
        '
        Me.gMonTrayFullBackup.Name = "gMonTrayFullBackup"
        Me.gMonTrayFullBackup.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayFullBackup.Text = "Run Full &Backup"
        '
        'gMonTrayFullRestore
        '
        Me.gMonTrayFullRestore.Name = "gMonTrayFullRestore"
        Me.gMonTrayFullRestore.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayFullRestore.Text = "Run Full &Restore"
        '
        'gMonTraySep3
        '
        Me.gMonTraySep3.Name = "gMonTraySep3"
        Me.gMonTraySep3.Size = New System.Drawing.Size(158, 6)
        '
        'gMonTraySettings
        '
        Me.gMonTraySettings.Name = "gMonTraySettings"
        Me.gMonTraySettings.Size = New System.Drawing.Size(161, 22)
        Me.gMonTraySettings.Text = "S&ettings"
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
        Me.gMonTrayTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsCompact, Me.gMonTrayToolsImportBackup, Me.gMonTrayToolsLog, Me.gMonTrayToolsSessions, Me.gMonTrayToolsSyncGameID})
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
        'gMonTrayToolsSyncGameID
        '
        Me.gMonTrayToolsSyncGameID.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsSyncGameIDOfficial, Me.gMonTrayToolsSyncGameIDFile})
        Me.gMonTrayToolsSyncGameID.Name = "gMonTrayToolsSyncGameID"
        Me.gMonTrayToolsSyncGameID.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsSyncGameID.Text = "S&ync Game IDs"
        '
        'gMonTrayToolsSyncGameIDOfficial
        '
        Me.gMonTrayToolsSyncGameIDOfficial.Name = "gMonTrayToolsSyncGameIDOfficial"
        Me.gMonTrayToolsSyncGameIDOfficial.Size = New System.Drawing.Size(142, 22)
        Me.gMonTrayToolsSyncGameIDOfficial.Text = "&Official List..."
        '
        'gMonTrayToolsSyncGameIDFile
        '
        Me.gMonTrayToolsSyncGameIDFile.Name = "gMonTrayToolsSyncGameIDFile"
        Me.gMonTrayToolsSyncGameIDFile.Size = New System.Drawing.Size(142, 22)
        Me.gMonTrayToolsSyncGameIDFile.Text = "&File..."
        '
        'gMonTraySep1
        '
        Me.gMonTraySep1.Name = "gMonTraySep1"
        Me.gMonTraySep1.Size = New System.Drawing.Size(158, 6)
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
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLog.Location = New System.Drawing.Point(15, 187)
        Me.txtLog.MaxLength = 524288
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(459, 167)
        Me.txtLog.TabIndex = 12
        Me.txtLog.TabStop = False
        '
        'gMonStatusStrip
        '
        Me.gMonStatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gMonStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonStripCollapse, Me.gMonStripAdminButton, Me.gMonStripTxtStatus, Me.gMonStripStatusButton})
        Me.gMonStatusStrip.Location = New System.Drawing.Point(0, 389)
        Me.gMonStatusStrip.Name = "gMonStatusStrip"
        Me.gMonStatusStrip.ShowItemToolTips = True
        Me.gMonStatusStrip.Size = New System.Drawing.Size(734, 22)
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
        Me.gMonStripTxtStatus.Size = New System.Drawing.Size(584, 22)
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
        Me.gMonMainMenu.Size = New System.Drawing.Size(734, 24)
        Me.gMonMainMenu.TabIndex = 0
        Me.gMonMainMenu.Text = "MenuStrip1"
        '
        'gMonFile
        '
        Me.gMonFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFileMonitor, Me.gMonFullSep, Me.gMonFileFullBackup, Me.gMonFileFullRestore, Me.gMonSettingsSep, Me.gMonFileSettings, Me.gMonExitSep, Me.gMonFileExit})
        Me.gMonFile.Name = "gMonFile"
        Me.gMonFile.Size = New System.Drawing.Size(37, 20)
        Me.gMonFile.Text = "&File"
        '
        'gMonFileMonitor
        '
        Me.gMonFileMonitor.Name = "gMonFileMonitor"
        Me.gMonFileMonitor.Size = New System.Drawing.Size(161, 22)
        Me.gMonFileMonitor.Text = "Start &Monitoring"
        '
        'gMonFullSep
        '
        Me.gMonFullSep.Name = "gMonFullSep"
        Me.gMonFullSep.Size = New System.Drawing.Size(158, 6)
        '
        'gMonFileFullBackup
        '
        Me.gMonFileFullBackup.Name = "gMonFileFullBackup"
        Me.gMonFileFullBackup.Size = New System.Drawing.Size(161, 22)
        Me.gMonFileFullBackup.Text = "Run Full &Backup"
        '
        'gMonFileFullRestore
        '
        Me.gMonFileFullRestore.Name = "gMonFileFullRestore"
        Me.gMonFileFullRestore.Size = New System.Drawing.Size(161, 22)
        Me.gMonFileFullRestore.Text = "Run Full &Restore"
        '
        'gMonSettingsSep
        '
        Me.gMonSettingsSep.Name = "gMonSettingsSep"
        Me.gMonSettingsSep.Size = New System.Drawing.Size(158, 6)
        '
        'gMonFileSettings
        '
        Me.gMonFileSettings.Name = "gMonFileSettings"
        Me.gMonFileSettings.Size = New System.Drawing.Size(161, 22)
        Me.gMonFileSettings.Text = "&Settings..."
        '
        'gMonExitSep
        '
        Me.gMonExitSep.Name = "gMonExitSep"
        Me.gMonExitSep.Size = New System.Drawing.Size(158, 6)
        '
        'gMonFileExit
        '
        Me.gMonFileExit.Name = "gMonFileExit"
        Me.gMonFileExit.Size = New System.Drawing.Size(161, 22)
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
        Me.gMonTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsCompact, Me.gMonToolsImportBackup, Me.gMonToolsLog, Me.gMonToolsSessions, Me.gMonToolsSyncGameID})
        Me.gMonTools.Name = "gMonTools"
        Me.gMonTools.Size = New System.Drawing.Size(46, 20)
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
        'gMonToolsSyncGameID
        '
        Me.gMonToolsSyncGameID.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsSyncGameIDOfficial, Me.gMonToolsSyncGameIDFile})
        Me.gMonToolsSyncGameID.Name = "gMonToolsSyncGameID"
        Me.gMonToolsSyncGameID.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsSyncGameID.Text = "S&ync Game IDs"
        '
        'gMonToolsSyncGameIDOfficial
        '
        Me.gMonToolsSyncGameIDOfficial.Name = "gMonToolsSyncGameIDOfficial"
        Me.gMonToolsSyncGameIDOfficial.Size = New System.Drawing.Size(142, 22)
        Me.gMonToolsSyncGameIDOfficial.Text = "&Official List..."
        '
        'gMonToolsSyncGameIDFile
        '
        Me.gMonToolsSyncGameIDFile.Name = "gMonToolsSyncGameIDFile"
        Me.gMonToolsSyncGameIDFile.Size = New System.Drawing.Size(142, 22)
        Me.gMonToolsSyncGameIDFile.Text = "&File..."
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
        'pbIcon
        '
        Me.pbIcon.Location = New System.Drawing.Point(15, 12)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 9
        Me.pbIcon.TabStop = False
        '
        'lblGameTitle
        '
        Me.lblGameTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGameTitle.AutoEllipsis = True
        Me.lblGameTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGameTitle.Location = New System.Drawing.Point(69, 9)
        Me.lblGameTitle.Name = "lblGameTitle"
        Me.lblGameTitle.Size = New System.Drawing.Size(405, 16)
        Me.lblGameTitle.TabIndex = 0
        Me.lblGameTitle.Text = "Game Title"
        Me.lblGameTitle.UseMnemonic = False
        '
        'lblLastAction
        '
        Me.lblLastAction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLastAction.AutoEllipsis = True
        Me.lblLastAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastAction.Location = New System.Drawing.Point(94, 171)
        Me.lblLastAction.Name = "lblLastAction"
        Me.lblLastAction.Size = New System.Drawing.Size(380, 13)
        Me.lblLastAction.TabIndex = 10
        Me.lblLastAction.Text = "Last Action"
        Me.lblLastAction.UseMnemonic = False
        '
        'lblLastActionTitle
        '
        Me.lblLastActionTitle.AutoSize = True
        Me.lblLastActionTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastActionTitle.Location = New System.Drawing.Point(13, 171)
        Me.lblLastActionTitle.Name = "lblLastActionTitle"
        Me.lblLastActionTitle.Size = New System.Drawing.Size(75, 13)
        Me.lblLastActionTitle.TabIndex = 9
        Me.lblLastActionTitle.Text = "Last Action:"
        '
        'lblTimeSpent
        '
        Me.lblTimeSpent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTimeSpent.AutoEllipsis = True
        Me.lblTimeSpent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeSpent.Location = New System.Drawing.Point(69, 92)
        Me.lblTimeSpent.Name = "lblTimeSpent"
        Me.lblTimeSpent.Size = New System.Drawing.Size(405, 16)
        Me.lblTimeSpent.TabIndex = 4
        Me.lblTimeSpent.Text = "0 Hours"
        '
        'btnCancelOperation
        '
        Me.btnCancelOperation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelOperation.Image = Global.GBM.My.Resources.Resources.frmMain_Cancel
        Me.btnCancelOperation.Location = New System.Drawing.Point(414, 118)
        Me.btnCancelOperation.Name = "btnCancelOperation"
        Me.btnCancelOperation.Size = New System.Drawing.Size(60, 45)
        Me.btnCancelOperation.TabIndex = 11
        Me.btnCancelOperation.Text = "&Cancel"
        Me.btnCancelOperation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancelOperation.UseVisualStyleBackColor = True
        '
        'lblStatus1
        '
        Me.lblStatus1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus1.AutoEllipsis = True
        Me.lblStatus1.Location = New System.Drawing.Point(69, 33)
        Me.lblStatus1.Name = "lblStatus1"
        Me.lblStatus1.Size = New System.Drawing.Size(405, 13)
        Me.lblStatus1.TabIndex = 1
        Me.lblStatus1.Text = "Status Text "
        Me.lblStatus1.UseMnemonic = False
        '
        'lblStatus2
        '
        Me.lblStatus2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus2.AutoEllipsis = True
        Me.lblStatus2.Location = New System.Drawing.Point(69, 51)
        Me.lblStatus2.Name = "lblStatus2"
        Me.lblStatus2.Size = New System.Drawing.Size(405, 13)
        Me.lblStatus2.TabIndex = 2
        Me.lblStatus2.Text = "Status Text"
        Me.lblStatus2.UseMnemonic = False
        '
        'lblStatus3
        '
        Me.lblStatus3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus3.AutoEllipsis = True
        Me.lblStatus3.Location = New System.Drawing.Point(69, 69)
        Me.lblStatus3.Name = "lblStatus3"
        Me.lblStatus3.Size = New System.Drawing.Size(405, 13)
        Me.lblStatus3.TabIndex = 3
        Me.lblStatus3.Text = "Status Text"
        Me.lblStatus3.UseMnemonic = False
        '
        'pbTime
        '
        Me.pbTime.Location = New System.Drawing.Point(39, 88)
        Me.pbTime.Name = "pbTime"
        Me.pbTime.Size = New System.Drawing.Size(24, 24)
        Me.pbTime.TabIndex = 18
        Me.pbTime.TabStop = False
        '
        'slcMain
        '
        Me.slcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slcMain.Location = New System.Drawing.Point(0, 24)
        Me.slcMain.Name = "slcMain"
        '
        'slcMain.Panel1
        '
        Me.slcMain.Panel1.Controls.Add(Me.btnClearSelected)
        Me.slcMain.Panel1.Controls.Add(Me.lblSearch)
        Me.slcMain.Panel1.Controls.Add(Me.txtSearch)
        Me.slcMain.Panel1.Controls.Add(Me.lstGames)
        '
        'slcMain.Panel2
        '
        Me.slcMain.Panel2.Controls.Add(Me.btnBackup)
        Me.slcMain.Panel2.Controls.Add(Me.btnRestore)
        Me.slcMain.Panel2.Controls.Add(Me.btnEdit)
        Me.slcMain.Panel2.Controls.Add(Me.btnPlay)
        Me.slcMain.Panel2.Controls.Add(Me.txtLog)
        Me.slcMain.Panel2.Controls.Add(Me.btnCancelOperation)
        Me.slcMain.Panel2.Controls.Add(Me.pbTime)
        Me.slcMain.Panel2.Controls.Add(Me.lblLastActionTitle)
        Me.slcMain.Panel2.Controls.Add(Me.lblStatus3)
        Me.slcMain.Panel2.Controls.Add(Me.lblLastAction)
        Me.slcMain.Panel2.Controls.Add(Me.lblStatus2)
        Me.slcMain.Panel2.Controls.Add(Me.pbIcon)
        Me.slcMain.Panel2.Controls.Add(Me.lblStatus1)
        Me.slcMain.Panel2.Controls.Add(Me.lblGameTitle)
        Me.slcMain.Panel2.Controls.Add(Me.lblTimeSpent)
        Me.slcMain.Size = New System.Drawing.Size(734, 365)
        Me.slcMain.SplitterDistance = 243
        Me.slcMain.SplitterWidth = 5
        Me.slcMain.TabIndex = 1
        Me.slcMain.TabStop = False
        '
        'btnClearSelected
        '
        Me.btnClearSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearSelected.Image = Global.GBM.My.Resources.Resources.frmMain_Cancel_Small
        Me.btnClearSelected.Location = New System.Drawing.Point(216, 9)
        Me.btnClearSelected.Name = "btnClearSelected"
        Me.btnClearSelected.Size = New System.Drawing.Size(24, 24)
        Me.btnClearSelected.TabIndex = 2
        Me.btnClearSelected.UseVisualStyleBackColor = True
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(9, 15)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(44, 13)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search:"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(59, 12)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(151, 20)
        Me.txtSearch.TabIndex = 1
        '
        'lstGames
        '
        Me.lstGames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstGames.FormattingEnabled = True
        Me.lstGames.IntegralHeight = False
        Me.lstGames.Location = New System.Drawing.Point(12, 38)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.Size = New System.Drawing.Size(228, 316)
        Me.lstGames.TabIndex = 3
        '
        'btnBackup
        '
        Me.btnBackup.Image = Global.GBM.My.Resources.Resources.Multi_Backup
        Me.btnBackup.Location = New System.Drawing.Point(81, 118)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(60, 45)
        Me.btnBackup.TabIndex = 6
        Me.btnBackup.Text = "&Backup"
        Me.btnBackup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Image = Global.GBM.My.Resources.Resources.Multi_Restore
        Me.btnRestore.Location = New System.Drawing.Point(147, 118)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(60, 45)
        Me.btnRestore.TabIndex = 7
        Me.btnRestore.Text = "&Restore"
        Me.btnRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Image = Global.GBM.My.Resources.Resources.frmMain_Edit
        Me.btnEdit.Location = New System.Drawing.Point(15, 118)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(60, 45)
        Me.btnEdit.TabIndex = 5
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Image = Global.GBM.My.Resources.Resources.frmMain_Play
        Me.btnPlay.Location = New System.Drawing.Point(213, 118)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(60, 45)
        Me.btnPlay.TabIndex = 8
        Me.btnPlay.Text = "&Play"
        Me.btnPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 411)
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
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slcMain.Panel1.ResumeLayout(False)
        Me.slcMain.Panel1.PerformLayout()
        Me.slcMain.Panel2.ResumeLayout(False)
        Me.slcMain.Panel2.PerformLayout()
        CType(Me.slcMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slcMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gMonTray As System.Windows.Forms.NotifyIcon
    Friend WithEvents gMonTrayMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents gMonTrayExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bwMonitor As System.ComponentModel.BackgroundWorker
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents gMonTrayMon As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents gMonStripTxtStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents gMonTraySettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySep2 As System.Windows.Forms.ToolStripSeparator
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
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents lblGameTitle As System.Windows.Forms.Label
    Friend WithEvents lblLastAction As System.Windows.Forms.Label
    Friend WithEvents lblLastActionTitle As System.Windows.Forms.Label
    Friend WithEvents gMonTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTimeSpent As System.Windows.Forms.Label
    Friend WithEvents gMonSetupGameManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupCustomVariables As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpCheckforUpdates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnCancelOperation As System.Windows.Forms.Button
    Friend WithEvents gMonTraySetupTags As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupTags As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus1 As Label
    Friend WithEvents lblStatus2 As Label
    Friend WithEvents lblStatus3 As Label
    Friend WithEvents gMonNotification As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayNotification As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpWebSite As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbTime As System.Windows.Forms.PictureBox
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
    Friend WithEvents gMonToolsSyncGameID As ToolStripMenuItem
    Friend WithEvents gMonToolsSyncGameIDOfficial As ToolStripMenuItem
    Friend WithEvents gMonToolsSyncGameIDFile As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsSyncGameID As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsSyncGameIDOfficial As ToolStripMenuItem
    Friend WithEvents gMonTrayToolsSyncGameIDFile As ToolStripMenuItem
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
    Friend WithEvents gMonTrayFullBackup As ToolStripMenuItem
    Friend WithEvents gMonTrayFullRestore As ToolStripMenuItem
    Friend WithEvents gMonTraySep3 As ToolStripSeparator
    Friend WithEvents gMonSetupLauncherManager As ToolStripMenuItem
    Friend WithEvents gMonTraySetupLauncherManager As ToolStripMenuItem
    Friend WithEvents slcMain As SplitContainer
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lstGames As ListBox
    Friend WithEvents gMonStripCollapse As ToolStripStatusLabel
    Friend WithEvents btnBackup As Button
    Friend WithEvents btnRestore As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnPlay As Button
    Friend WithEvents btnClearSelected As Button
End Class
