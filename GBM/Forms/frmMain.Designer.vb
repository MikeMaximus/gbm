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
        Me.gMonTrayShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayMon = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupAddWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupGameManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySetupCustomVariables = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsGameList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsGameImportOfficialList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsGameImportList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsGameExportList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsSyncMan = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTrayToolsCompact = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTraySep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonTrayExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.bwMonitor = New System.ComponentModel.BackgroundWorker()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.gMonStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.gMonStripAdminButton = New System.Windows.Forms.ToolStripSplitButton()
        Me.gMonStripTxtStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.gMonStripStatusButton = New System.Windows.Forms.ToolStripSplitButton()
        Me.gMonMainMenu = New System.Windows.Forms.MenuStrip()
        Me.gMonFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonFileMonitor = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonMonitorSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonExitSep = New System.Windows.Forms.ToolStripSeparator()
        Me.gMonFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupGameManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupAddWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonSetupCustomVariables = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsGameList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsGameImportOfficialList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsGameImportList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsGameExportList = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsSyncMan = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonToolsCompact = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpCheckforUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.gMonHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.btnLogToggle = New System.Windows.Forms.Button()
        Me.lblGameTitle = New System.Windows.Forms.Label()
        Me.lblLastAction = New System.Windows.Forms.Label()
        Me.lblLastActionTitle = New System.Windows.Forms.Label()
        Me.lblTimeTitle = New System.Windows.Forms.Label()
        Me.lblTimeSpent = New System.Windows.Forms.Label()
        Me.txtGameInfo = New System.Windows.Forms.TextBox()
        Me.btnCancelOperation = New System.Windows.Forms.Button()
        Me.gMonTrayMenu.SuspendLayout()
        Me.gMonStatusStrip.SuspendLayout()
        Me.gMonMainMenu.SuspendLayout()
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
        Me.gMonTrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayShow, Me.gMonTraySep2, Me.gMonTrayMon, Me.gMonTraySettings, Me.gMonTraySetup, Me.gMonTrayTools, Me.gMonTraySep1, Me.gMonTrayExit})
        Me.gMonTrayMenu.Name = "gMonTrayMenu"
        Me.gMonTrayMenu.Size = New System.Drawing.Size(162, 148)
        '
        'gMonTrayShow
        '
        Me.gMonTrayShow.Name = "gMonTrayShow"
        Me.gMonTrayShow.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayShow.Text = "S&how / Hide"
        '
        'gMonTraySep2
        '
        Me.gMonTraySep2.Name = "gMonTraySep2"
        Me.gMonTraySep2.Size = New System.Drawing.Size(158, 6)
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
        'gMonTraySetup
        '
        Me.gMonTraySetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTraySetupAddWizard, Me.gMonTraySetupGameManager, Me.gMonTraySetupCustomVariables})
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
        'gMonTraySetupCustomVariables
        '
        Me.gMonTraySetupCustomVariables.Name = "gMonTraySetupCustomVariables"
        Me.gMonTraySetupCustomVariables.Size = New System.Drawing.Size(201, 22)
        Me.gMonTraySetupCustomVariables.Text = "Custom &Path Variables..."
        '
        'gMonTrayTools
        '
        Me.gMonTrayTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsGameList, Me.gMonTrayToolsSyncMan, Me.gMonTrayToolsCompact})
        Me.gMonTrayTools.Name = "gMonTrayTools"
        Me.gMonTrayTools.Size = New System.Drawing.Size(161, 22)
        Me.gMonTrayTools.Text = "&Tools"
        '
        'gMonTrayToolsGameList
        '
        Me.gMonTrayToolsGameList.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonTrayToolsGameImportOfficialList, Me.gMonTrayToolsGameImportList, Me.gMonTrayToolsGameExportList})
        Me.gMonTrayToolsGameList.Name = "gMonTrayToolsGameList"
        Me.gMonTrayToolsGameList.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsGameList.Text = "&Game List"
        '
        'gMonTrayToolsGameImportOfficialList
        '
        Me.gMonTrayToolsGameImportOfficialList.Name = "gMonTrayToolsGameImportOfficialList"
        Me.gMonTrayToolsGameImportOfficialList.Size = New System.Drawing.Size(201, 22)
        Me.gMonTrayToolsGameImportOfficialList.Text = "Import from &Official List"
        '
        'gMonTrayToolsGameImportList
        '
        Me.gMonTrayToolsGameImportList.Name = "gMonTrayToolsGameImportList"
        Me.gMonTrayToolsGameImportList.Size = New System.Drawing.Size(201, 22)
        Me.gMonTrayToolsGameImportList.Text = "I&mport Game List"
        '
        'gMonTrayToolsGameExportList
        '
        Me.gMonTrayToolsGameExportList.Name = "gMonTrayToolsGameExportList"
        Me.gMonTrayToolsGameExportList.Size = New System.Drawing.Size(201, 22)
        Me.gMonTrayToolsGameExportList.Text = "E&xport Game List"
        '
        'gMonTrayToolsSyncMan
        '
        Me.gMonTrayToolsSyncMan.Name = "gMonTrayToolsSyncMan"
        Me.gMonTrayToolsSyncMan.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsSyncMan.Text = "Sync Ma&nifests"
        '
        'gMonTrayToolsCompact
        '
        Me.gMonTrayToolsCompact.Name = "gMonTrayToolsCompact"
        Me.gMonTrayToolsCompact.Size = New System.Drawing.Size(179, 22)
        Me.gMonTrayToolsCompact.Text = "&Compact Databases"
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
        Me.txtLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLog.Location = New System.Drawing.Point(12, 161)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(500, 177)
        Me.txtLog.TabIndex = 2
        Me.txtLog.TabStop = False
        '
        'gMonStatusStrip
        '
        Me.gMonStatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gMonStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonStripAdminButton, Me.gMonStripTxtStatus, Me.gMonStripStatusButton})
        Me.gMonStatusStrip.Location = New System.Drawing.Point(0, 350)
        Me.gMonStatusStrip.Name = "gMonStatusStrip"
        Me.gMonStatusStrip.ShowItemToolTips = True
        Me.gMonStatusStrip.Size = New System.Drawing.Size(524, 22)
        Me.gMonStatusStrip.SizingGrip = False
        Me.gMonStatusStrip.TabIndex = 3
        '
        'gMonStripAdminButton
        '
        Me.gMonStripAdminButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.gMonStripAdminButton.DropDownButtonWidth = 0
        Me.gMonStripAdminButton.Image = Global.GBM.My.Resources.Resources.User
        Me.gMonStripAdminButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.gMonStripAdminButton.Name = "gMonStripAdminButton"
        Me.gMonStripAdminButton.Size = New System.Drawing.Size(21, 20)
        Me.gMonStripAdminButton.Text = "Elevation"
        Me.gMonStripAdminButton.ToolTipText = "Elevation"
        '
        'gMonStripTxtStatus
        '
        Me.gMonStripTxtStatus.Name = "gMonStripTxtStatus"
        Me.gMonStripTxtStatus.Size = New System.Drawing.Size(395, 17)
        Me.gMonStripTxtStatus.Spring = True
        Me.gMonStripTxtStatus.Text = "Monitor Status"
        Me.gMonStripTxtStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gMonStripStatusButton
        '
        Me.gMonStripStatusButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.gMonStripStatusButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.gMonStripStatusButton.DropDownButtonWidth = 0
        Me.gMonStripStatusButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.gMonStripStatusButton.Name = "gMonStripStatusButton"
        Me.gMonStripStatusButton.Size = New System.Drawing.Size(93, 20)
        Me.gMonStripStatusButton.Text = "Monitor Status:"
        Me.gMonStripStatusButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.gMonStripStatusButton.ToolTipText = "Click to toggle monitoring on or off."
        '
        'gMonMainMenu
        '
        Me.gMonMainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFile, Me.gMonSetup, Me.gMonTools, Me.gMonHelp})
        Me.gMonMainMenu.Location = New System.Drawing.Point(0, 0)
        Me.gMonMainMenu.Name = "gMonMainMenu"
        Me.gMonMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.gMonMainMenu.Size = New System.Drawing.Size(524, 24)
        Me.gMonMainMenu.TabIndex = 8
        Me.gMonMainMenu.Text = "MenuStrip1"
        '
        'gMonFile
        '
        Me.gMonFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonFileMonitor, Me.gMonMonitorSep, Me.gMonFileSettings, Me.gMonExitSep, Me.gMonFileExit})
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
        'gMonMonitorSep
        '
        Me.gMonMonitorSep.Name = "gMonMonitorSep"
        Me.gMonMonitorSep.Size = New System.Drawing.Size(158, 6)
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
        Me.gMonSetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonSetupGameManager, Me.gMonSetupAddWizard, Me.gMonSetupCustomVariables})
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
        'gMonSetupCustomVariables
        '
        Me.gMonSetupCustomVariables.Name = "gMonSetupCustomVariables"
        Me.gMonSetupCustomVariables.Size = New System.Drawing.Size(201, 22)
        Me.gMonSetupCustomVariables.Text = "Custom &Path Variables..."
        '
        'gMonTools
        '
        Me.gMonTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsGameList, Me.gMonToolsSyncMan, Me.gMonToolsCompact})
        Me.gMonTools.Name = "gMonTools"
        Me.gMonTools.Size = New System.Drawing.Size(47, 20)
        Me.gMonTools.Text = "&Tools"
        '
        'gMonToolsGameList
        '
        Me.gMonToolsGameList.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonToolsGameImportOfficialList, Me.gMonToolsGameImportList, Me.gMonToolsGameExportList})
        Me.gMonToolsGameList.Name = "gMonToolsGameList"
        Me.gMonToolsGameList.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsGameList.Text = "&Game List"
        '
        'gMonToolsGameImportOfficialList
        '
        Me.gMonToolsGameImportOfficialList.Name = "gMonToolsGameImportOfficialList"
        Me.gMonToolsGameImportOfficialList.Size = New System.Drawing.Size(201, 22)
        Me.gMonToolsGameImportOfficialList.Text = "Import from &Official List"
        '
        'gMonToolsGameImportList
        '
        Me.gMonToolsGameImportList.Name = "gMonToolsGameImportList"
        Me.gMonToolsGameImportList.Size = New System.Drawing.Size(201, 22)
        Me.gMonToolsGameImportList.Text = "I&mport Game List"
        '
        'gMonToolsGameExportList
        '
        Me.gMonToolsGameExportList.Name = "gMonToolsGameExportList"
        Me.gMonToolsGameExportList.Size = New System.Drawing.Size(201, 22)
        Me.gMonToolsGameExportList.Text = "E&xport Game List"
        '
        'gMonToolsSyncMan
        '
        Me.gMonToolsSyncMan.Name = "gMonToolsSyncMan"
        Me.gMonToolsSyncMan.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsSyncMan.Text = "Sync Ma&nifests"
        '
        'gMonToolsCompact
        '
        Me.gMonToolsCompact.Name = "gMonToolsCompact"
        Me.gMonToolsCompact.Size = New System.Drawing.Size(179, 22)
        Me.gMonToolsCompact.Text = "&Compact Databases"
        '
        'gMonHelp
        '
        Me.gMonHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gMonHelpManual, Me.gMonHelpCheckforUpdates, Me.gMonHelpAbout})
        Me.gMonHelp.Name = "gMonHelp"
        Me.gMonHelp.Size = New System.Drawing.Size(44, 20)
        Me.gMonHelp.Text = "&Help"
        '
        'gMonHelpManual
        '
        Me.gMonHelpManual.Name = "gMonHelpManual"
        Me.gMonHelpManual.Size = New System.Drawing.Size(171, 22)
        Me.gMonHelpManual.Text = "Online &Manual"
        '
        'gMonHelpCheckforUpdates
        '
        Me.gMonHelpCheckforUpdates.Name = "gMonHelpCheckforUpdates"
        Me.gMonHelpCheckforUpdates.Size = New System.Drawing.Size(171, 22)
        Me.gMonHelpCheckforUpdates.Text = "Check for Updates"
        '
        'gMonHelpAbout
        '
        Me.gMonHelpAbout.Name = "gMonHelpAbout"
        Me.gMonHelpAbout.Size = New System.Drawing.Size(171, 22)
        Me.gMonHelpAbout.Text = "&About"
        '
        'pbIcon
        '
        Me.pbIcon.Location = New System.Drawing.Point(12, 36)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(48, 48)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIcon.TabIndex = 9
        Me.pbIcon.TabStop = False
        '
        'btnLogToggle
        '
        Me.btnLogToggle.Location = New System.Drawing.Point(437, 132)
        Me.btnLogToggle.Name = "btnLogToggle"
        Me.btnLogToggle.Size = New System.Drawing.Size(75, 23)
        Me.btnLogToggle.TabIndex = 1
        Me.btnLogToggle.Text = "Show &Log"
        Me.btnLogToggle.UseVisualStyleBackColor = True
        '
        'lblGameTitle
        '
        Me.lblGameTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGameTitle.Location = New System.Drawing.Point(66, 36)
        Me.lblGameTitle.Name = "lblGameTitle"
        Me.lblGameTitle.Size = New System.Drawing.Size(446, 16)
        Me.lblGameTitle.TabIndex = 10
        Me.lblGameTitle.Text = "Game Title"
        '
        'lblLastAction
        '
        Me.lblLastAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastAction.Location = New System.Drawing.Point(12, 139)
        Me.lblLastAction.Name = "lblLastAction"
        Me.lblLastAction.Size = New System.Drawing.Size(419, 16)
        Me.lblLastAction.TabIndex = 11
        Me.lblLastAction.Text = "Last Action"
        '
        'lblLastActionTitle
        '
        Me.lblLastActionTitle.AutoSize = True
        Me.lblLastActionTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastActionTitle.Location = New System.Drawing.Point(12, 126)
        Me.lblLastActionTitle.Name = "lblLastActionTitle"
        Me.lblLastActionTitle.Size = New System.Drawing.Size(75, 13)
        Me.lblLastActionTitle.TabIndex = 12
        Me.lblLastActionTitle.Text = "Last Action:"
        '
        'lblTimeTitle
        '
        Me.lblTimeTitle.AutoSize = True
        Me.lblTimeTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeTitle.Location = New System.Drawing.Point(66, 55)
        Me.lblTimeTitle.Name = "lblTimeTitle"
        Me.lblTimeTitle.Size = New System.Drawing.Size(75, 13)
        Me.lblTimeTitle.TabIndex = 13
        Me.lblTimeTitle.Text = "Time Spent:"
        '
        'lblTimeSpent
        '
        Me.lblTimeSpent.AutoSize = True
        Me.lblTimeSpent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeSpent.Location = New System.Drawing.Point(139, 55)
        Me.lblTimeSpent.Name = "lblTimeSpent"
        Me.lblTimeSpent.Size = New System.Drawing.Size(44, 13)
        Me.lblTimeSpent.TabIndex = 14
        Me.lblTimeSpent.Text = "0 Hours"
        '
        'txtGameInfo
        '
        Me.txtGameInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtGameInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtGameInfo.Location = New System.Drawing.Point(69, 71)
        Me.txtGameInfo.Multiline = True
        Me.txtGameInfo.Name = "txtGameInfo"
        Me.txtGameInfo.ReadOnly = True
        Me.txtGameInfo.Size = New System.Drawing.Size(443, 52)
        Me.txtGameInfo.TabIndex = 0
        Me.txtGameInfo.TabStop = False
        Me.txtGameInfo.WordWrap = False
        '
        'btnCancelOperation
        '
        Me.btnCancelOperation.Location = New System.Drawing.Point(437, 103)
        Me.btnCancelOperation.Name = "btnCancelOperation"
        Me.btnCancelOperation.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelOperation.TabIndex = 0
        Me.btnCancelOperation.Text = "&Cancel"
        Me.btnCancelOperation.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 372)
        Me.Controls.Add(Me.btnCancelOperation)
        Me.Controls.Add(Me.txtGameInfo)
        Me.Controls.Add(Me.lblTimeSpent)
        Me.Controls.Add(Me.lblTimeTitle)
        Me.Controls.Add(Me.lblLastActionTitle)
        Me.Controls.Add(Me.lblLastAction)
        Me.Controls.Add(Me.lblGameTitle)
        Me.Controls.Add(Me.btnLogToggle)
        Me.Controls.Add(Me.pbIcon)
        Me.Controls.Add(Me.gMonStatusStrip)
        Me.Controls.Add(Me.gMonMainMenu)
        Me.Controls.Add(Me.txtLog)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.gMonMainMenu
        Me.MaximizeBox = False
        Me.MinimizeBox = False
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
    Friend WithEvents gMonMonitorSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents gMonFileSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonExitSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents gMonFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupGameManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupCustomVariables As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonStripStatusButton As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents btnLogToggle As System.Windows.Forms.Button
    Friend WithEvents lblGameTitle As System.Windows.Forms.Label
    Friend WithEvents lblLastAction As System.Windows.Forms.Label
    Friend WithEvents lblLastActionTitle As System.Windows.Forms.Label
    Friend WithEvents gMonTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsSyncMan As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsSyncMan As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTraySetupAddWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTimeTitle As System.Windows.Forms.Label
    Friend WithEvents lblTimeSpent As System.Windows.Forms.Label
    Friend WithEvents txtGameInfo As System.Windows.Forms.TextBox
    Friend WithEvents gMonToolsGameList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsGameImportOfficialList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsGameImportList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsGameExportList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsGameList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsGameImportList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsGameExportList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsGameImportOfficialList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupGameManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonSetupCustomVariables As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonTrayToolsCompact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gMonHelpCheckforUpdates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnCancelOperation As System.Windows.Forms.Button
    Friend WithEvents gMonStripAdminButton As ToolStripSplitButton
End Class
