<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddWizard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddWizard))
        Me.tabWizard = New System.Windows.Forms.TabControl()
        Me.tbPage1 = New System.Windows.Forms.TabPage()
        Me.lblStep1Title = New System.Windows.Forms.Label()
        Me.lblStep1Instructions = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblStep1Intro = New System.Windows.Forms.Label()
        Me.tbPage2 = New System.Windows.Forms.TabPage()
        Me.lblStep2Title = New System.Windows.Forms.Label()
        Me.lblStep2Instructions = New System.Windows.Forms.Label()
        Me.btnProcessBrowse = New System.Windows.Forms.Button()
        Me.txtProcessPath = New System.Windows.Forms.TextBox()
        Me.lblStep2Intro = New System.Windows.Forms.Label()
        Me.tbPage3 = New System.Windows.Forms.TabPage()
        Me.lblLimit = New System.Windows.Forms.Label()
        Me.nudLimit = New System.Windows.Forms.NumericUpDown()
        Me.lblStep3Title = New System.Windows.Forms.Label()
        Me.lblStep3Instructions = New System.Windows.Forms.Label()
        Me.chkTimeStamp = New System.Windows.Forms.CheckBox()
        Me.chkFolderSave = New System.Windows.Forms.CheckBox()
        Me.btnSaveBrowse = New System.Windows.Forms.Button()
        Me.txtSavePath = New System.Windows.Forms.TextBox()
        Me.lblStep3Intro = New System.Windows.Forms.Label()
        Me.tbPage3a = New System.Windows.Forms.TabPage()
        Me.chkRecurseSubFolders = New System.Windows.Forms.CheckBox()
        Me.lblIncludePathTitle = New System.Windows.Forms.Label()
        Me.lblIncludePath = New System.Windows.Forms.Label()
        Me.lblFileTypes = New System.Windows.Forms.Label()
        Me.btnInclude = New System.Windows.Forms.Button()
        Me.lblStep3aTitle = New System.Windows.Forms.Label()
        Me.lblStep3aInstructions = New System.Windows.Forms.Label()
        Me.txtFileTypes = New System.Windows.Forms.TextBox()
        Me.tbPage4 = New System.Windows.Forms.TabPage()
        Me.lblExcludePathTitle = New System.Windows.Forms.Label()
        Me.lblExcludePath = New System.Windows.Forms.Label()
        Me.lblExclude = New System.Windows.Forms.Label()
        Me.btnExclude = New System.Windows.Forms.Button()
        Me.lblStep4Title = New System.Windows.Forms.Label()
        Me.lblStep4Instructions = New System.Windows.Forms.Label()
        Me.txtExcludeList = New System.Windows.Forms.TextBox()
        Me.tbPage5 = New System.Windows.Forms.TabPage()
        Me.lblStep5Intro = New System.Windows.Forms.Label()
        Me.lblStep5Title = New System.Windows.Forms.Label()
        Me.lstSummary = New System.Windows.Forms.ListView()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.tabWizard.SuspendLayout()
        Me.tbPage1.SuspendLayout()
        Me.tbPage2.SuspendLayout()
        Me.tbPage3.SuspendLayout()
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbPage3a.SuspendLayout()
        Me.tbPage4.SuspendLayout()
        Me.tbPage5.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabWizard
        '
        Me.tabWizard.Controls.Add(Me.tbPage1)
        Me.tabWizard.Controls.Add(Me.tbPage2)
        Me.tabWizard.Controls.Add(Me.tbPage3)
        Me.tabWizard.Controls.Add(Me.tbPage3a)
        Me.tabWizard.Controls.Add(Me.tbPage4)
        Me.tabWizard.Controls.Add(Me.tbPage5)
        Me.tabWizard.Location = New System.Drawing.Point(-6, -24)
        Me.tabWizard.Name = "tabWizard"
        Me.tabWizard.SelectedIndex = 0
        Me.tabWizard.Size = New System.Drawing.Size(395, 247)
        Me.tabWizard.TabIndex = 0
        Me.tabWizard.TabStop = False
        '
        'tbPage1
        '
        Me.tbPage1.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage1.Controls.Add(Me.lblStep1Title)
        Me.tbPage1.Controls.Add(Me.lblStep1Instructions)
        Me.tbPage1.Controls.Add(Me.txtName)
        Me.tbPage1.Controls.Add(Me.lblStep1Intro)
        Me.tbPage1.Location = New System.Drawing.Point(4, 22)
        Me.tbPage1.Name = "tbPage1"
        Me.tbPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage1.Size = New System.Drawing.Size(387, 221)
        Me.tbPage1.TabIndex = 0
        Me.tbPage1.Text = "TabPage1"
        '
        'lblStep1Title
        '
        Me.lblStep1Title.AutoSize = True
        Me.lblStep1Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep1Title.Name = "lblStep1Title"
        Me.lblStep1Title.Size = New System.Drawing.Size(108, 20)
        Me.lblStep1Title.TabIndex = 8
        Me.lblStep1Title.Text = "Game Name"
        '
        'lblStep1Instructions
        '
        Me.lblStep1Instructions.AllowDrop = True
        Me.lblStep1Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Instructions.Location = New System.Drawing.Point(14, 96)
        Me.lblStep1Instructions.Name = "lblStep1Instructions"
        Me.lblStep1Instructions.Size = New System.Drawing.Size(303, 108)
        Me.lblStep1Instructions.TabIndex = 6
        Me.lblStep1Instructions.Text = "You may drag and drop a shortcut here to complete this step, only Windows shortcu" &
    "ts are currently supported."
        '
        'txtName
        '
        Me.txtName.AllowDrop = True
        Me.txtName.Location = New System.Drawing.Point(17, 67)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(300, 20)
        Me.txtName.TabIndex = 4
        '
        'lblStep1Intro
        '
        Me.lblStep1Intro.AutoSize = True
        Me.lblStep1Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep1Intro.Name = "lblStep1Intro"
        Me.lblStep1Intro.Size = New System.Drawing.Size(234, 16)
        Me.lblStep1Intro.TabIndex = 5
        Me.lblStep1Intro.Text = "Enter the name of the game to monitor:"
        '
        'tbPage2
        '
        Me.tbPage2.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage2.Controls.Add(Me.lblStep2Title)
        Me.tbPage2.Controls.Add(Me.lblStep2Instructions)
        Me.tbPage2.Controls.Add(Me.btnProcessBrowse)
        Me.tbPage2.Controls.Add(Me.txtProcessPath)
        Me.tbPage2.Controls.Add(Me.lblStep2Intro)
        Me.tbPage2.Location = New System.Drawing.Point(4, 22)
        Me.tbPage2.Name = "tbPage2"
        Me.tbPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage2.Size = New System.Drawing.Size(387, 221)
        Me.tbPage2.TabIndex = 1
        Me.tbPage2.Text = "TabPage2"
        '
        'lblStep2Title
        '
        Me.lblStep2Title.AutoSize = True
        Me.lblStep2Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep2Title.Name = "lblStep2Title"
        Me.lblStep2Title.Size = New System.Drawing.Size(159, 20)
        Me.lblStep2Title.TabIndex = 11
        Me.lblStep2Title.Text = "Process to Monitor"
        '
        'lblStep2Instructions
        '
        Me.lblStep2Instructions.AllowDrop = True
        Me.lblStep2Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Instructions.Location = New System.Drawing.Point(14, 96)
        Me.lblStep2Instructions.Name = "lblStep2Instructions"
        Me.lblStep2Instructions.Size = New System.Drawing.Size(301, 103)
        Me.lblStep2Instructions.TabIndex = 10
        Me.lblStep2Instructions.Text = resources.GetString("lblStep2Instructions.Text")
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(323, 67)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btnProcessBrowse.TabIndex = 8
        Me.btnProcessBrowse.Text = "..."
        Me.btnProcessBrowse.UseVisualStyleBackColor = True
        '
        'txtProcessPath
        '
        Me.txtProcessPath.AllowDrop = True
        Me.txtProcessPath.Location = New System.Drawing.Point(17, 67)
        Me.txtProcessPath.Name = "txtProcessPath"
        Me.txtProcessPath.Size = New System.Drawing.Size(300, 20)
        Me.txtProcessPath.TabIndex = 6
        '
        'lblStep2Intro
        '
        Me.lblStep2Intro.AutoSize = True
        Me.lblStep2Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep2Intro.Name = "lblStep2Intro"
        Me.lblStep2Intro.Size = New System.Drawing.Size(280, 16)
        Me.lblStep2Intro.TabIndex = 7
        Me.lblStep2Intro.Text = "Choose the game's executable file or shortcut:"
        '
        'tbPage3
        '
        Me.tbPage3.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage3.Controls.Add(Me.lblLimit)
        Me.tbPage3.Controls.Add(Me.nudLimit)
        Me.tbPage3.Controls.Add(Me.lblStep3Title)
        Me.tbPage3.Controls.Add(Me.lblStep3Instructions)
        Me.tbPage3.Controls.Add(Me.chkTimeStamp)
        Me.tbPage3.Controls.Add(Me.chkFolderSave)
        Me.tbPage3.Controls.Add(Me.btnSaveBrowse)
        Me.tbPage3.Controls.Add(Me.txtSavePath)
        Me.tbPage3.Controls.Add(Me.lblStep3Intro)
        Me.tbPage3.Location = New System.Drawing.Point(4, 22)
        Me.tbPage3.Name = "tbPage3"
        Me.tbPage3.Size = New System.Drawing.Size(387, 221)
        Me.tbPage3.TabIndex = 2
        Me.tbPage3.Text = "TabPage3"
        '
        'lblLimit
        '
        Me.lblLimit.AutoSize = True
        Me.lblLimit.Location = New System.Drawing.Point(203, 117)
        Me.lblLimit.Name = "lblLimit"
        Me.lblLimit.Size = New System.Drawing.Size(138, 13)
        Me.lblLimit.TabIndex = 15
        Me.lblLimit.Text = "Backup Limit (0 = Unlimited)"
        Me.lblLimit.Visible = False
        '
        'nudLimit
        '
        Me.nudLimit.Location = New System.Drawing.Point(157, 115)
        Me.nudLimit.Name = "nudLimit"
        Me.nudLimit.Size = New System.Drawing.Size(40, 20)
        Me.nudLimit.TabIndex = 14
        Me.nudLimit.Visible = False
        '
        'lblStep3Title
        '
        Me.lblStep3Title.AutoSize = True
        Me.lblStep3Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep3Title.Name = "lblStep3Title"
        Me.lblStep3Title.Size = New System.Drawing.Size(164, 20)
        Me.lblStep3Title.TabIndex = 10
        Me.lblStep3Title.Text = "Game Backup Path"
        '
        'lblStep3Instructions
        '
        Me.lblStep3Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Instructions.Location = New System.Drawing.Point(14, 138)
        Me.lblStep3Instructions.Name = "lblStep3Instructions"
        Me.lblStep3Instructions.Size = New System.Drawing.Size(327, 71)
        Me.lblStep3Instructions.TabIndex = 9
        Me.lblStep3Instructions.Text = "If you're unsure of exactly which files to backup,  make sure Save entire folder " &
    "is checked.  You can also choose to save multiple backups and set a limit on how" &
    " many to keep."
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(17, 116)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(133, 17)
        Me.chkTimeStamp.TabIndex = 8
        Me.chkTimeStamp.Text = "Save multiple backups"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'chkFolderSave
        '
        Me.chkFolderSave.AutoSize = True
        Me.chkFolderSave.Location = New System.Drawing.Point(17, 93)
        Me.chkFolderSave.Name = "chkFolderSave"
        Me.chkFolderSave.Size = New System.Drawing.Size(109, 17)
        Me.chkFolderSave.TabIndex = 7
        Me.chkFolderSave.Text = "Save entire folder"
        Me.chkFolderSave.UseVisualStyleBackColor = True
        '
        'btnSaveBrowse
        '
        Me.btnSaveBrowse.Location = New System.Drawing.Point(323, 67)
        Me.btnSaveBrowse.Name = "btnSaveBrowse"
        Me.btnSaveBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btnSaveBrowse.TabIndex = 6
        Me.btnSaveBrowse.Text = "..."
        Me.btnSaveBrowse.UseVisualStyleBackColor = True
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(17, 67)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.Size = New System.Drawing.Size(300, 20)
        Me.txtSavePath.TabIndex = 4
        '
        'lblStep3Intro
        '
        Me.lblStep3Intro.AutoSize = True
        Me.lblStep3Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep3Intro.Name = "lblStep3Intro"
        Me.lblStep3Intro.Size = New System.Drawing.Size(280, 16)
        Me.lblStep3Intro.TabIndex = 5
        Me.lblStep3Intro.Text = "Choose the location of your game's save files:"
        '
        'tbPage3a
        '
        Me.tbPage3a.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage3a.Controls.Add(Me.chkRecurseSubFolders)
        Me.tbPage3a.Controls.Add(Me.lblIncludePathTitle)
        Me.tbPage3a.Controls.Add(Me.lblIncludePath)
        Me.tbPage3a.Controls.Add(Me.lblFileTypes)
        Me.tbPage3a.Controls.Add(Me.btnInclude)
        Me.tbPage3a.Controls.Add(Me.lblStep3aTitle)
        Me.tbPage3a.Controls.Add(Me.lblStep3aInstructions)
        Me.tbPage3a.Controls.Add(Me.txtFileTypes)
        Me.tbPage3a.Location = New System.Drawing.Point(4, 22)
        Me.tbPage3a.Name = "tbPage3a"
        Me.tbPage3a.Size = New System.Drawing.Size(387, 221)
        Me.tbPage3a.TabIndex = 3
        Me.tbPage3a.Text = "TabPage4"
        '
        'chkRecurseSubFolders
        '
        Me.chkRecurseSubFolders.AutoSize = True
        Me.chkRecurseSubFolders.Location = New System.Drawing.Point(219, 45)
        Me.chkRecurseSubFolders.Name = "chkRecurseSubFolders"
        Me.chkRecurseSubFolders.Size = New System.Drawing.Size(15, 14)
        Me.chkRecurseSubFolders.TabIndex = 0
        Me.chkRecurseSubFolders.TabStop = False
        Me.chkRecurseSubFolders.UseVisualStyleBackColor = True
        Me.chkRecurseSubFolders.Visible = False
        '
        'lblIncludePathTitle
        '
        Me.lblIncludePathTitle.AutoSize = True
        Me.lblIncludePathTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIncludePathTitle.Location = New System.Drawing.Point(14, 45)
        Me.lblIncludePathTitle.Name = "lblIncludePathTitle"
        Me.lblIncludePathTitle.Size = New System.Drawing.Size(151, 16)
        Me.lblIncludePathTitle.TabIndex = 6
        Me.lblIncludePathTitle.Text = "Saved Game Folder:"
        '
        'lblIncludePath
        '
        Me.lblIncludePath.AutoEllipsis = True
        Me.lblIncludePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIncludePath.Location = New System.Drawing.Point(14, 62)
        Me.lblIncludePath.Name = "lblIncludePath"
        Me.lblIncludePath.Size = New System.Drawing.Size(360, 15)
        Me.lblIncludePath.TabIndex = 1
        Me.lblIncludePath.Text = "Save Path"
        '
        'lblFileTypes
        '
        Me.lblFileTypes.AutoSize = True
        Me.lblFileTypes.Location = New System.Drawing.Point(200, 96)
        Me.lblFileTypes.Name = "lblFileTypes"
        Me.lblFileTypes.Size = New System.Drawing.Size(89, 13)
        Me.lblFileTypes.TabIndex = 3
        Me.lblFileTypes.Text = "0 item(s) selected"
        '
        'btnInclude
        '
        Me.btnInclude.Location = New System.Drawing.Point(18, 91)
        Me.btnInclude.Name = "btnInclude"
        Me.btnInclude.Size = New System.Drawing.Size(176, 23)
        Me.btnInclude.TabIndex = 2
        Me.btnInclude.Text = "Choose items to in&clude..."
        Me.btnInclude.UseVisualStyleBackColor = True
        '
        'lblStep3aTitle
        '
        Me.lblStep3aTitle.AutoSize = True
        Me.lblStep3aTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3aTitle.Location = New System.Drawing.Point(14, 15)
        Me.lblStep3aTitle.Name = "lblStep3aTitle"
        Me.lblStep3aTitle.Size = New System.Drawing.Size(199, 20)
        Me.lblStep3aTitle.TabIndex = 0
        Me.lblStep3aTitle.Text = "Choose Files to Backup"
        '
        'lblStep3aInstructions
        '
        Me.lblStep3aInstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3aInstructions.Location = New System.Drawing.Point(14, 126)
        Me.lblStep3aInstructions.Name = "lblStep3aInstructions"
        Me.lblStep3aInstructions.Size = New System.Drawing.Size(360, 78)
        Me.lblStep3aInstructions.TabIndex = 5
        Me.lblStep3aInstructions.Text = "Choose any file types, specific files  or folders you wish to include in the back" &
    "up.  If you're unsure, go back a step and choose to save the entire folder. "
        '
        'txtFileTypes
        '
        Me.txtFileTypes.Location = New System.Drawing.Point(219, 17)
        Me.txtFileTypes.Name = "txtFileTypes"
        Me.txtFileTypes.ReadOnly = True
        Me.txtFileTypes.Size = New System.Drawing.Size(155, 20)
        Me.txtFileTypes.TabIndex = 4
        Me.txtFileTypes.TabStop = False
        Me.txtFileTypes.Visible = False
        '
        'tbPage4
        '
        Me.tbPage4.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage4.Controls.Add(Me.lblExcludePathTitle)
        Me.tbPage4.Controls.Add(Me.lblExcludePath)
        Me.tbPage4.Controls.Add(Me.lblExclude)
        Me.tbPage4.Controls.Add(Me.btnExclude)
        Me.tbPage4.Controls.Add(Me.lblStep4Title)
        Me.tbPage4.Controls.Add(Me.lblStep4Instructions)
        Me.tbPage4.Controls.Add(Me.txtExcludeList)
        Me.tbPage4.Location = New System.Drawing.Point(4, 22)
        Me.tbPage4.Name = "tbPage4"
        Me.tbPage4.Size = New System.Drawing.Size(387, 221)
        Me.tbPage4.TabIndex = 4
        Me.tbPage4.Text = "TabPage5"
        '
        'lblExcludePathTitle
        '
        Me.lblExcludePathTitle.AutoSize = True
        Me.lblExcludePathTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExcludePathTitle.Location = New System.Drawing.Point(14, 45)
        Me.lblExcludePathTitle.Name = "lblExcludePathTitle"
        Me.lblExcludePathTitle.Size = New System.Drawing.Size(151, 16)
        Me.lblExcludePathTitle.TabIndex = 7
        Me.lblExcludePathTitle.Text = "Saved Game Folder:"
        '
        'lblExcludePath
        '
        Me.lblExcludePath.AutoEllipsis = True
        Me.lblExcludePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExcludePath.Location = New System.Drawing.Point(14, 62)
        Me.lblExcludePath.Name = "lblExcludePath"
        Me.lblExcludePath.Size = New System.Drawing.Size(360, 15)
        Me.lblExcludePath.TabIndex = 1
        Me.lblExcludePath.Text = "Save Path"
        '
        'lblExclude
        '
        Me.lblExclude.AutoSize = True
        Me.lblExclude.Location = New System.Drawing.Point(200, 96)
        Me.lblExclude.Name = "lblExclude"
        Me.lblExclude.Size = New System.Drawing.Size(89, 13)
        Me.lblExclude.TabIndex = 3
        Me.lblExclude.Text = "0 item(s) selected"
        '
        'btnExclude
        '
        Me.btnExclude.Location = New System.Drawing.Point(18, 91)
        Me.btnExclude.Name = "btnExclude"
        Me.btnExclude.Size = New System.Drawing.Size(176, 23)
        Me.btnExclude.TabIndex = 2
        Me.btnExclude.Text = "Choose items to e&xclude..."
        Me.btnExclude.UseVisualStyleBackColor = True
        '
        'lblStep4Title
        '
        Me.lblStep4Title.AutoSize = True
        Me.lblStep4Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep4Title.Name = "lblStep4Title"
        Me.lblStep4Title.Size = New System.Drawing.Size(201, 20)
        Me.lblStep4Title.TabIndex = 0
        Me.lblStep4Title.Text = "Exclude Files or Folders"
        '
        'lblStep4Instructions
        '
        Me.lblStep4Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Instructions.Location = New System.Drawing.Point(14, 126)
        Me.lblStep4Instructions.Name = "lblStep4Instructions"
        Me.lblStep4Instructions.Size = New System.Drawing.Size(360, 81)
        Me.lblStep4Instructions.TabIndex = 5
        Me.lblStep4Instructions.Text = "Choose any file types, specific files  or folders you wish to exclude from the ba" &
    "ckup.  You may choose multiple items to exclude.  This step can be skipped."
        '
        'txtExcludeList
        '
        Me.txtExcludeList.Location = New System.Drawing.Point(221, 17)
        Me.txtExcludeList.Name = "txtExcludeList"
        Me.txtExcludeList.ReadOnly = True
        Me.txtExcludeList.Size = New System.Drawing.Size(153, 20)
        Me.txtExcludeList.TabIndex = 4
        Me.txtExcludeList.TabStop = False
        Me.txtExcludeList.Visible = False
        '
        'tbPage5
        '
        Me.tbPage5.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage5.Controls.Add(Me.lblStep5Intro)
        Me.tbPage5.Controls.Add(Me.lblStep5Title)
        Me.tbPage5.Controls.Add(Me.lstSummary)
        Me.tbPage5.Location = New System.Drawing.Point(4, 22)
        Me.tbPage5.Name = "tbPage5"
        Me.tbPage5.Size = New System.Drawing.Size(387, 221)
        Me.tbPage5.TabIndex = 5
        Me.tbPage5.Text = "TabPage6"
        '
        'lblStep5Intro
        '
        Me.lblStep5Intro.AutoSize = True
        Me.lblStep5Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep5Intro.Location = New System.Drawing.Point(15, 45)
        Me.lblStep5Intro.Name = "lblStep5Intro"
        Me.lblStep5Intro.Size = New System.Drawing.Size(303, 16)
        Me.lblStep5Intro.TabIndex = 18
        Me.lblStep5Intro.Text = "Verify your settings below and click Finish to save."
        '
        'lblStep5Title
        '
        Me.lblStep5Title.AutoSize = True
        Me.lblStep5Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep5Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep5Title.Name = "lblStep5Title"
        Me.lblStep5Title.Size = New System.Drawing.Size(196, 20)
        Me.lblStep5Title.TabIndex = 17
        Me.lblStep5Title.Text = "Summary of your Game"
        '
        'lstSummary
        '
        Me.lstSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstSummary.HideSelection = False
        Me.lstSummary.Location = New System.Drawing.Point(14, 64)
        Me.lstSummary.Name = "lstSummary"
        Me.lstSummary.ShowItemToolTips = True
        Me.lstSummary.Size = New System.Drawing.Size(360, 142)
        Me.lstSummary.TabIndex = 1
        Me.lstSummary.UseCompatibleStateImageBehavior = False
        Me.lstSummary.View = System.Windows.Forms.View.Details
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(312, 229)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Image = Global.GBM.My.Resources.Resources.Multi_Next
        Me.btnNext.Location = New System.Drawing.Point(246, 229)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(60, 45)
        Me.btnNext.TabIndex = 11
        Me.btnNext.Text = "&Next"
        Me.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Image = Global.GBM.My.Resources.Resources.Multi_Back
        Me.btnBack.Location = New System.Drawing.Point(180, 229)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(60, 45)
        Me.btnBack.TabIndex = 10
        Me.btnBack.Text = "&Back"
        Me.btnBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'frmAddWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 286)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.tabWizard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddWizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Game Wizard"
        Me.tabWizard.ResumeLayout(False)
        Me.tbPage1.ResumeLayout(False)
        Me.tbPage1.PerformLayout()
        Me.tbPage2.ResumeLayout(False)
        Me.tbPage2.PerformLayout()
        Me.tbPage3.ResumeLayout(False)
        Me.tbPage3.PerformLayout()
        CType(Me.nudLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbPage3a.ResumeLayout(False)
        Me.tbPage3a.PerformLayout()
        Me.tbPage4.ResumeLayout(False)
        Me.tbPage4.PerformLayout()
        Me.tbPage5.ResumeLayout(False)
        Me.tbPage5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabWizard As System.Windows.Forms.TabControl
    Friend WithEvents tbPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tbPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents tbPage3 As System.Windows.Forms.TabPage
    Friend WithEvents tbPage3a As System.Windows.Forms.TabPage
    Friend WithEvents tbPage4 As System.Windows.Forms.TabPage
    Friend WithEvents tbPage5 As System.Windows.Forms.TabPage
    Friend WithEvents lblStep1Instructions As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblStep1Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep2Instructions As System.Windows.Forms.Label
    Friend WithEvents btnProcessBrowse As System.Windows.Forms.Button
    Friend WithEvents txtProcessPath As System.Windows.Forms.TextBox
    Friend WithEvents lblStep2Intro As System.Windows.Forms.Label
    Friend WithEvents chkTimeStamp As System.Windows.Forms.CheckBox
    Friend WithEvents chkFolderSave As System.Windows.Forms.CheckBox
    Friend WithEvents btnSaveBrowse As System.Windows.Forms.Button
    Friend WithEvents txtSavePath As System.Windows.Forms.TextBox
    Friend WithEvents lblStep3Intro As System.Windows.Forms.Label
    Friend WithEvents txtFileTypes As System.Windows.Forms.TextBox
    Friend WithEvents txtExcludeList As System.Windows.Forms.TextBox
    Friend WithEvents lblStep3aInstructions As System.Windows.Forms.Label
    Friend WithEvents lblStep4Instructions As System.Windows.Forms.Label
    Friend WithEvents lblStep3Instructions As System.Windows.Forms.Label
    Friend WithEvents lstSummary As System.Windows.Forms.ListView
    Friend WithEvents lblStep1Title As System.Windows.Forms.Label
    Friend WithEvents lblStep2Title As System.Windows.Forms.Label
    Friend WithEvents lblStep3Title As System.Windows.Forms.Label
    Friend WithEvents lblStep4Title As System.Windows.Forms.Label
    Friend WithEvents lblStep3aTitle As System.Windows.Forms.Label
    Friend WithEvents lblStep5Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep5Title As System.Windows.Forms.Label
    Friend WithEvents lblFileTypes As Label
    Friend WithEvents btnInclude As Button
    Friend WithEvents lblExclude As Label
    Friend WithEvents btnExclude As Button
    Friend WithEvents lblIncludePath As Label
    Friend WithEvents lblExcludePath As Label
    Friend WithEvents lblIncludePathTitle As Label
    Friend WithEvents lblExcludePathTitle As Label
    Friend WithEvents lblLimit As Label
    Friend WithEvents nudLimit As NumericUpDown
    Friend WithEvents chkRecurseSubFolders As CheckBox
End Class
