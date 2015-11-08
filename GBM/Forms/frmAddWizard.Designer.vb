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
        Me.lblDrag1 = New System.Windows.Forms.Label()
        Me.lblStep1Instructions = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblStep1Intro = New System.Windows.Forms.Label()
        Me.tbPage2 = New System.Windows.Forms.TabPage()
        Me.lbldBox = New System.Windows.Forms.Label()
        Me.btndBoxBrowse = New System.Windows.Forms.Button()
        Me.txtdBoxProcess = New System.Windows.Forms.TextBox()
        Me.lblStep2Title = New System.Windows.Forms.Label()
        Me.lblStep2Instructions = New System.Windows.Forms.Label()
        Me.lblDrag2 = New System.Windows.Forms.Label()
        Me.btnProcessBrowse = New System.Windows.Forms.Button()
        Me.txtProcessPath = New System.Windows.Forms.TextBox()
        Me.lblStep2Intro = New System.Windows.Forms.Label()
        Me.tbPage3 = New System.Windows.Forms.TabPage()
        Me.lblStep3Title = New System.Windows.Forms.Label()
        Me.lblStep3Instructions = New System.Windows.Forms.Label()
        Me.chkTimeStamp = New System.Windows.Forms.CheckBox()
        Me.chkFolderSave = New System.Windows.Forms.CheckBox()
        Me.btnSaveBrowse = New System.Windows.Forms.Button()
        Me.txtSavePath = New System.Windows.Forms.TextBox()
        Me.lblStep3Intro = New System.Windows.Forms.Label()
        Me.tbPage3a = New System.Windows.Forms.TabPage()
        Me.grpFileTypes = New System.Windows.Forms.GroupBox()
        Me.optSpecificFile = New System.Windows.Forms.RadioButton()
        Me.btnFileTypeBrowse = New System.Windows.Forms.Button()
        Me.optFileType = New System.Windows.Forms.RadioButton()
        Me.btnStep3aClear = New System.Windows.Forms.Button()
        Me.lblStep3aTitle = New System.Windows.Forms.Label()
        Me.lblStep3aInstructions = New System.Windows.Forms.Label()
        Me.txtFileTypes = New System.Windows.Forms.TextBox()
        Me.tbPage4 = New System.Windows.Forms.TabPage()
        Me.grpExclude = New System.Windows.Forms.GroupBox()
        Me.optExcludeSpecificFile = New System.Windows.Forms.RadioButton()
        Me.btnExcludeBrowse = New System.Windows.Forms.Button()
        Me.optExcludeFileType = New System.Windows.Forms.RadioButton()
        Me.optExcludeFolder = New System.Windows.Forms.RadioButton()
        Me.btnStep4Clear = New System.Windows.Forms.Button()
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
        Me.optFileTypeFolder = New System.Windows.Forms.RadioButton()
        Me.tabWizard.SuspendLayout()
        Me.tbPage1.SuspendLayout()
        Me.tbPage2.SuspendLayout()
        Me.tbPage3.SuspendLayout()
        Me.tbPage3a.SuspendLayout()
        Me.grpFileTypes.SuspendLayout()
        Me.tbPage4.SuspendLayout()
        Me.grpExclude.SuspendLayout()
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
        Me.tabWizard.Size = New System.Drawing.Size(370, 220)
        Me.tabWizard.TabIndex = 0
        Me.tabWizard.TabStop = False
        '
        'tbPage1
        '
        Me.tbPage1.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage1.Controls.Add(Me.lblStep1Title)
        Me.tbPage1.Controls.Add(Me.lblDrag1)
        Me.tbPage1.Controls.Add(Me.lblStep1Instructions)
        Me.tbPage1.Controls.Add(Me.txtName)
        Me.tbPage1.Controls.Add(Me.lblStep1Intro)
        Me.tbPage1.Location = New System.Drawing.Point(4, 22)
        Me.tbPage1.Name = "tbPage1"
        Me.tbPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage1.Size = New System.Drawing.Size(362, 194)
        Me.tbPage1.TabIndex = 0
        Me.tbPage1.Text = "TabPage1"
        '
        'lblStep1Title
        '
        Me.lblStep1Title.AutoSize = True
        Me.lblStep1Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Title.Location = New System.Drawing.Point(13, 11)
        Me.lblStep1Title.Name = "lblStep1Title"
        Me.lblStep1Title.Size = New System.Drawing.Size(108, 20)
        Me.lblStep1Title.TabIndex = 8
        Me.lblStep1Title.Text = "Game Name"
        '
        'lblDrag1
        '
        Me.lblDrag1.AllowDrop = True
        Me.lblDrag1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDrag1.Location = New System.Drawing.Point(14, 147)
        Me.lblDrag1.Name = "lblDrag1"
        Me.lblDrag1.Size = New System.Drawing.Size(303, 37)
        Me.lblDrag1.TabIndex = 7
        Me.lblDrag1.Text = "Drag a shortcut here to complete this step."
        '
        'lblStep1Instructions
        '
        Me.lblStep1Instructions.Location = New System.Drawing.Point(14, 93)
        Me.lblStep1Instructions.Name = "lblStep1Instructions"
        Me.lblStep1Instructions.Size = New System.Drawing.Size(303, 42)
        Me.lblStep1Instructions.TabIndex = 6
        Me.lblStep1Instructions.Text = "The name of the game is used for the backup file and must conform to Windows file" &
    " name standards.  It will be automatically filtered for length and invalid chara" &
    "cters. "
        '
        'txtName
        '
        Me.txtName.AllowDrop = True
        Me.txtName.Location = New System.Drawing.Point(17, 61)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(300, 20)
        Me.txtName.TabIndex = 4
        '
        'lblStep1Intro
        '
        Me.lblStep1Intro.AutoSize = True
        Me.lblStep1Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep1Intro.Name = "lblStep1Intro"
        Me.lblStep1Intro.Size = New System.Drawing.Size(190, 13)
        Me.lblStep1Intro.TabIndex = 5
        Me.lblStep1Intro.Text = "Enter the name of the game to monitor:"
        '
        'tbPage2
        '
        Me.tbPage2.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage2.Controls.Add(Me.lbldBox)
        Me.tbPage2.Controls.Add(Me.btndBoxBrowse)
        Me.tbPage2.Controls.Add(Me.txtdBoxProcess)
        Me.tbPage2.Controls.Add(Me.lblStep2Title)
        Me.tbPage2.Controls.Add(Me.lblStep2Instructions)
        Me.tbPage2.Controls.Add(Me.lblDrag2)
        Me.tbPage2.Controls.Add(Me.btnProcessBrowse)
        Me.tbPage2.Controls.Add(Me.txtProcessPath)
        Me.tbPage2.Controls.Add(Me.lblStep2Intro)
        Me.tbPage2.Location = New System.Drawing.Point(4, 22)
        Me.tbPage2.Name = "tbPage2"
        Me.tbPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage2.Size = New System.Drawing.Size(362, 194)
        Me.tbPage2.TabIndex = 1
        Me.tbPage2.Text = "TabPage2"
        '
        'lbldBox
        '
        Me.lbldBox.AutoSize = True
        Me.lbldBox.Location = New System.Drawing.Point(184, 18)
        Me.lbldBox.Name = "lbldBox"
        Me.lbldBox.Size = New System.Drawing.Size(52, 13)
        Me.lbldBox.TabIndex = 14
        Me.lbldBox.Text = "DOS File:"
        '
        'btndBoxBrowse
        '
        Me.btndBoxBrowse.Location = New System.Drawing.Point(322, 14)
        Me.btndBoxBrowse.Name = "btndBoxBrowse"
        Me.btndBoxBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btndBoxBrowse.TabIndex = 13
        Me.btndBoxBrowse.Text = "..."
        Me.btndBoxBrowse.UseVisualStyleBackColor = True
        '
        'txtdBoxProcess
        '
        Me.txtdBoxProcess.AllowDrop = True
        Me.txtdBoxProcess.Location = New System.Drawing.Point(244, 14)
        Me.txtdBoxProcess.Name = "txtdBoxProcess"
        Me.txtdBoxProcess.Size = New System.Drawing.Size(72, 20)
        Me.txtdBoxProcess.TabIndex = 12
        '
        'lblStep2Title
        '
        Me.lblStep2Title.AutoSize = True
        Me.lblStep2Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Title.Location = New System.Drawing.Point(13, 11)
        Me.lblStep2Title.Name = "lblStep2Title"
        Me.lblStep2Title.Size = New System.Drawing.Size(159, 20)
        Me.lblStep2Title.TabIndex = 11
        Me.lblStep2Title.Text = "Process to Monitor"
        '
        'lblStep2Instructions
        '
        Me.lblStep2Instructions.Location = New System.Drawing.Point(14, 93)
        Me.lblStep2Instructions.Name = "lblStep2Instructions"
        Me.lblStep2Instructions.Size = New System.Drawing.Size(303, 41)
        Me.lblStep2Instructions.TabIndex = 10
        Me.lblStep2Instructions.Text = "GBM needs to know what to look for when you run the application.  Some games use " &
    "launchers.  Do not monitor launchers,  choose the actual game exe file."
        '
        'lblDrag2
        '
        Me.lblDrag2.AllowDrop = True
        Me.lblDrag2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDrag2.Location = New System.Drawing.Point(14, 147)
        Me.lblDrag2.Name = "lblDrag2"
        Me.lblDrag2.Size = New System.Drawing.Size(336, 44)
        Me.lblDrag2.TabIndex = 9
        Me.lblDrag2.Text = "Drag a shortcut here to complete this step."
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(323, 60)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btnProcessBrowse.TabIndex = 8
        Me.btnProcessBrowse.Text = "..."
        Me.btnProcessBrowse.UseVisualStyleBackColor = True
        '
        'txtProcessPath
        '
        Me.txtProcessPath.AllowDrop = True
        Me.txtProcessPath.Location = New System.Drawing.Point(17, 61)
        Me.txtProcessPath.Name = "txtProcessPath"
        Me.txtProcessPath.Size = New System.Drawing.Size(300, 20)
        Me.txtProcessPath.TabIndex = 6
        '
        'lblStep2Intro
        '
        Me.lblStep2Intro.AutoSize = True
        Me.lblStep2Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep2Intro.Name = "lblStep2Intro"
        Me.lblStep2Intro.Size = New System.Drawing.Size(224, 13)
        Me.lblStep2Intro.TabIndex = 7
        Me.lblStep2Intro.Text = "Choose the game's executable file or shortcut:"
        '
        'tbPage3
        '
        Me.tbPage3.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage3.Controls.Add(Me.lblStep3Title)
        Me.tbPage3.Controls.Add(Me.lblStep3Instructions)
        Me.tbPage3.Controls.Add(Me.chkTimeStamp)
        Me.tbPage3.Controls.Add(Me.chkFolderSave)
        Me.tbPage3.Controls.Add(Me.btnSaveBrowse)
        Me.tbPage3.Controls.Add(Me.txtSavePath)
        Me.tbPage3.Controls.Add(Me.lblStep3Intro)
        Me.tbPage3.Location = New System.Drawing.Point(4, 22)
        Me.tbPage3.Name = "tbPage3"
        Me.tbPage3.Size = New System.Drawing.Size(362, 194)
        Me.tbPage3.TabIndex = 2
        Me.tbPage3.Text = "TabPage3"
        '
        'lblStep3Title
        '
        Me.lblStep3Title.AutoSize = True
        Me.lblStep3Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Title.Location = New System.Drawing.Point(14, 11)
        Me.lblStep3Title.Name = "lblStep3Title"
        Me.lblStep3Title.Size = New System.Drawing.Size(164, 20)
        Me.lblStep3Title.TabIndex = 10
        Me.lblStep3Title.Text = "Game Backup Path"
        '
        'lblStep3Instructions
        '
        Me.lblStep3Instructions.Location = New System.Drawing.Point(14, 116)
        Me.lblStep3Instructions.Name = "lblStep3Instructions"
        Me.lblStep3Instructions.Size = New System.Drawing.Size(303, 42)
        Me.lblStep3Instructions.TabIndex = 9
        Me.lblStep3Instructions.Text = "If you're unsure of exactly which files to backup,  make sure Save Entire Folder " &
    "is checked.  You can also time stamp your backup files to make incremental backu" &
    "ps."
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(139, 87)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(122, 17)
        Me.chkTimeStamp.TabIndex = 8
        Me.chkTimeStamp.Text = "Time Stamp Backup"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'chkFolderSave
        '
        Me.chkFolderSave.AutoSize = True
        Me.chkFolderSave.Location = New System.Drawing.Point(17, 87)
        Me.chkFolderSave.Name = "chkFolderSave"
        Me.chkFolderSave.Size = New System.Drawing.Size(113, 17)
        Me.chkFolderSave.TabIndex = 7
        Me.chkFolderSave.Text = "Save Entire Folder"
        Me.chkFolderSave.UseVisualStyleBackColor = True
        '
        'btnSaveBrowse
        '
        Me.btnSaveBrowse.Location = New System.Drawing.Point(323, 60)
        Me.btnSaveBrowse.Name = "btnSaveBrowse"
        Me.btnSaveBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btnSaveBrowse.TabIndex = 6
        Me.btnSaveBrowse.Text = "..."
        Me.btnSaveBrowse.UseVisualStyleBackColor = True
        '
        'txtSavePath
        '
        Me.txtSavePath.Location = New System.Drawing.Point(17, 61)
        Me.txtSavePath.Name = "txtSavePath"
        Me.txtSavePath.Size = New System.Drawing.Size(300, 20)
        Me.txtSavePath.TabIndex = 4
        '
        'lblStep3Intro
        '
        Me.lblStep3Intro.AutoSize = True
        Me.lblStep3Intro.Location = New System.Drawing.Point(14, 45)
        Me.lblStep3Intro.Name = "lblStep3Intro"
        Me.lblStep3Intro.Size = New System.Drawing.Size(222, 13)
        Me.lblStep3Intro.TabIndex = 5
        Me.lblStep3Intro.Text = "Choose the location of your game's save files:"
        '
        'tbPage3a
        '
        Me.tbPage3a.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage3a.Controls.Add(Me.grpFileTypes)
        Me.tbPage3a.Controls.Add(Me.btnStep3aClear)
        Me.tbPage3a.Controls.Add(Me.lblStep3aTitle)
        Me.tbPage3a.Controls.Add(Me.lblStep3aInstructions)
        Me.tbPage3a.Controls.Add(Me.txtFileTypes)
        Me.tbPage3a.Location = New System.Drawing.Point(4, 22)
        Me.tbPage3a.Name = "tbPage3a"
        Me.tbPage3a.Size = New System.Drawing.Size(362, 194)
        Me.tbPage3a.TabIndex = 3
        Me.tbPage3a.Text = "TabPage4"
        '
        'grpFileTypes
        '
        Me.grpFileTypes.Controls.Add(Me.optFileTypeFolder)
        Me.grpFileTypes.Controls.Add(Me.optSpecificFile)
        Me.grpFileTypes.Controls.Add(Me.btnFileTypeBrowse)
        Me.grpFileTypes.Controls.Add(Me.optFileType)
        Me.grpFileTypes.Location = New System.Drawing.Point(17, 43)
        Me.grpFileTypes.Name = "grpFileTypes"
        Me.grpFileTypes.Size = New System.Drawing.Size(310, 47)
        Me.grpFileTypes.TabIndex = 0
        Me.grpFileTypes.TabStop = False
        Me.grpFileTypes.Text = "Choose any files or folders to include in the backup"
        '
        'optSpecificFile
        '
        Me.optSpecificFile.AutoSize = True
        Me.optSpecificFile.Location = New System.Drawing.Point(80, 19)
        Me.optSpecificFile.Name = "optSpecificFile"
        Me.optSpecificFile.Size = New System.Drawing.Size(82, 17)
        Me.optSpecificFile.TabIndex = 2
        Me.optSpecificFile.TabStop = True
        Me.optSpecificFile.Text = "Specific File"
        Me.optSpecificFile.UseVisualStyleBackColor = True
        '
        'btnFileTypeBrowse
        '
        Me.btnFileTypeBrowse.Location = New System.Drawing.Point(229, 16)
        Me.btnFileTypeBrowse.Name = "btnFileTypeBrowse"
        Me.btnFileTypeBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnFileTypeBrowse.TabIndex = 3
        Me.btnFileTypeBrowse.Text = "Browse..."
        Me.btnFileTypeBrowse.UseVisualStyleBackColor = True
        '
        'optFileType
        '
        Me.optFileType.AutoSize = True
        Me.optFileType.Location = New System.Drawing.Point(6, 19)
        Me.optFileType.Name = "optFileType"
        Me.optFileType.Size = New System.Drawing.Size(68, 17)
        Me.optFileType.TabIndex = 1
        Me.optFileType.TabStop = True
        Me.optFileType.Text = "File Type"
        Me.optFileType.UseVisualStyleBackColor = True
        '
        'btnStep3aClear
        '
        Me.btnStep3aClear.Location = New System.Drawing.Point(253, 93)
        Me.btnStep3aClear.Name = "btnStep3aClear"
        Me.btnStep3aClear.Size = New System.Drawing.Size(75, 23)
        Me.btnStep3aClear.TabIndex = 5
        Me.btnStep3aClear.Text = "&Clear"
        Me.btnStep3aClear.UseVisualStyleBackColor = True
        '
        'lblStep3aTitle
        '
        Me.lblStep3aTitle.AutoSize = True
        Me.lblStep3aTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3aTitle.Location = New System.Drawing.Point(13, 11)
        Me.lblStep3aTitle.Name = "lblStep3aTitle"
        Me.lblStep3aTitle.Size = New System.Drawing.Size(199, 20)
        Me.lblStep3aTitle.TabIndex = 12
        Me.lblStep3aTitle.Text = "Choose Files to Backup"
        '
        'lblStep3aInstructions
        '
        Me.lblStep3aInstructions.Location = New System.Drawing.Point(14, 126)
        Me.lblStep3aInstructions.Name = "lblStep3aInstructions"
        Me.lblStep3aInstructions.Size = New System.Drawing.Size(303, 56)
        Me.lblStep3aInstructions.TabIndex = 11
        Me.lblStep3aInstructions.Text = "Choose any file types, specific files  or folders you wish to include in the back" &
    "up.  You may choose multiple items to include.  If you're unsure, go back a step" &
    " and choose to save the entire folder. "
        '
        'txtFileTypes
        '
        Me.txtFileTypes.Location = New System.Drawing.Point(18, 95)
        Me.txtFileTypes.Name = "txtFileTypes"
        Me.txtFileTypes.ReadOnly = True
        Me.txtFileTypes.Size = New System.Drawing.Size(229, 20)
        Me.txtFileTypes.TabIndex = 4
        Me.txtFileTypes.TabStop = False
        '
        'tbPage4
        '
        Me.tbPage4.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage4.Controls.Add(Me.grpExclude)
        Me.tbPage4.Controls.Add(Me.btnStep4Clear)
        Me.tbPage4.Controls.Add(Me.lblStep4Title)
        Me.tbPage4.Controls.Add(Me.lblStep4Instructions)
        Me.tbPage4.Controls.Add(Me.txtExcludeList)
        Me.tbPage4.Location = New System.Drawing.Point(4, 22)
        Me.tbPage4.Name = "tbPage4"
        Me.tbPage4.Size = New System.Drawing.Size(362, 194)
        Me.tbPage4.TabIndex = 4
        Me.tbPage4.Text = "TabPage5"
        '
        'grpExclude
        '
        Me.grpExclude.Controls.Add(Me.optExcludeSpecificFile)
        Me.grpExclude.Controls.Add(Me.btnExcludeBrowse)
        Me.grpExclude.Controls.Add(Me.optExcludeFileType)
        Me.grpExclude.Controls.Add(Me.optExcludeFolder)
        Me.grpExclude.Location = New System.Drawing.Point(17, 43)
        Me.grpExclude.Name = "grpExclude"
        Me.grpExclude.Size = New System.Drawing.Size(310, 47)
        Me.grpExclude.TabIndex = 0
        Me.grpExclude.TabStop = False
        Me.grpExclude.Text = "Choose any files or folders to exclude from the backup:"
        '
        'optExcludeSpecificFile
        '
        Me.optExcludeSpecificFile.AutoSize = True
        Me.optExcludeSpecificFile.Location = New System.Drawing.Point(80, 19)
        Me.optExcludeSpecificFile.Name = "optExcludeSpecificFile"
        Me.optExcludeSpecificFile.Size = New System.Drawing.Size(82, 17)
        Me.optExcludeSpecificFile.TabIndex = 2
        Me.optExcludeSpecificFile.TabStop = True
        Me.optExcludeSpecificFile.Text = "Specific File"
        Me.optExcludeSpecificFile.UseVisualStyleBackColor = True
        '
        'btnExcludeBrowse
        '
        Me.btnExcludeBrowse.Location = New System.Drawing.Point(229, 16)
        Me.btnExcludeBrowse.Name = "btnExcludeBrowse"
        Me.btnExcludeBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnExcludeBrowse.TabIndex = 4
        Me.btnExcludeBrowse.Text = "&Browse..."
        Me.btnExcludeBrowse.UseVisualStyleBackColor = True
        '
        'optExcludeFileType
        '
        Me.optExcludeFileType.AutoSize = True
        Me.optExcludeFileType.Location = New System.Drawing.Point(6, 19)
        Me.optExcludeFileType.Name = "optExcludeFileType"
        Me.optExcludeFileType.Size = New System.Drawing.Size(68, 17)
        Me.optExcludeFileType.TabIndex = 1
        Me.optExcludeFileType.TabStop = True
        Me.optExcludeFileType.Text = "File Type"
        Me.optExcludeFileType.UseVisualStyleBackColor = True
        '
        'optExcludeFolder
        '
        Me.optExcludeFolder.AutoSize = True
        Me.optExcludeFolder.Location = New System.Drawing.Point(168, 19)
        Me.optExcludeFolder.Name = "optExcludeFolder"
        Me.optExcludeFolder.Size = New System.Drawing.Size(54, 17)
        Me.optExcludeFolder.TabIndex = 3
        Me.optExcludeFolder.TabStop = True
        Me.optExcludeFolder.Text = "Folder"
        Me.optExcludeFolder.UseVisualStyleBackColor = True
        '
        'btnStep4Clear
        '
        Me.btnStep4Clear.Location = New System.Drawing.Point(253, 93)
        Me.btnStep4Clear.Name = "btnStep4Clear"
        Me.btnStep4Clear.Size = New System.Drawing.Size(75, 23)
        Me.btnStep4Clear.TabIndex = 6
        Me.btnStep4Clear.Text = "&Clear"
        Me.btnStep4Clear.UseVisualStyleBackColor = True
        '
        'lblStep4Title
        '
        Me.lblStep4Title.AutoSize = True
        Me.lblStep4Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Title.Location = New System.Drawing.Point(14, 11)
        Me.lblStep4Title.Name = "lblStep4Title"
        Me.lblStep4Title.Size = New System.Drawing.Size(201, 20)
        Me.lblStep4Title.TabIndex = 16
        Me.lblStep4Title.Text = "Exclude Files or Folders"
        '
        'lblStep4Instructions
        '
        Me.lblStep4Instructions.Location = New System.Drawing.Point(14, 126)
        Me.lblStep4Instructions.Name = "lblStep4Instructions"
        Me.lblStep4Instructions.Size = New System.Drawing.Size(303, 59)
        Me.lblStep4Instructions.TabIndex = 12
        Me.lblStep4Instructions.Text = resources.GetString("lblStep4Instructions.Text")
        '
        'txtExcludeList
        '
        Me.txtExcludeList.Location = New System.Drawing.Point(18, 95)
        Me.txtExcludeList.Name = "txtExcludeList"
        Me.txtExcludeList.ReadOnly = True
        Me.txtExcludeList.Size = New System.Drawing.Size(229, 20)
        Me.txtExcludeList.TabIndex = 5
        Me.txtExcludeList.TabStop = False
        '
        'tbPage5
        '
        Me.tbPage5.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage5.Controls.Add(Me.lblStep5Intro)
        Me.tbPage5.Controls.Add(Me.lblStep5Title)
        Me.tbPage5.Controls.Add(Me.lstSummary)
        Me.tbPage5.Location = New System.Drawing.Point(4, 22)
        Me.tbPage5.Name = "tbPage5"
        Me.tbPage5.Size = New System.Drawing.Size(362, 194)
        Me.tbPage5.TabIndex = 5
        Me.tbPage5.Text = "TabPage6"
        '
        'lblStep5Intro
        '
        Me.lblStep5Intro.AutoSize = True
        Me.lblStep5Intro.Location = New System.Drawing.Point(15, 40)
        Me.lblStep5Intro.Name = "lblStep5Intro"
        Me.lblStep5Intro.Size = New System.Drawing.Size(243, 13)
        Me.lblStep5Intro.TabIndex = 18
        Me.lblStep5Intro.Text = "Verify your settings below and click Finish to save."
        '
        'lblStep5Title
        '
        Me.lblStep5Title.AutoSize = True
        Me.lblStep5Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep5Title.Location = New System.Drawing.Point(14, 11)
        Me.lblStep5Title.Name = "lblStep5Title"
        Me.lblStep5Title.Size = New System.Drawing.Size(196, 20)
        Me.lblStep5Title.TabIndex = 17
        Me.lblStep5Title.Text = "Summary of your Game"
        '
        'lstSummary
        '
        Me.lstSummary.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstSummary.Location = New System.Drawing.Point(14, 65)
        Me.lstSummary.Name = "lstSummary"
        Me.lstSummary.Size = New System.Drawing.Size(335, 126)
        Me.lstSummary.TabIndex = 1
        Me.lstSummary.UseCompatibleStateImageBehavior = False
        Me.lstSummary.View = System.Windows.Forms.View.Details
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(272, 202)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(191, 202)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(75, 23)
        Me.btnNext.TabIndex = 11
        Me.btnNext.Text = "&Next"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(110, 202)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(75, 23)
        Me.btnBack.TabIndex = 10
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'optFileTypeFolder
        '
        Me.optFileTypeFolder.AutoSize = True
        Me.optFileTypeFolder.Location = New System.Drawing.Point(168, 19)
        Me.optFileTypeFolder.Name = "optFileTypeFolder"
        Me.optFileTypeFolder.Size = New System.Drawing.Size(54, 17)
        Me.optFileTypeFolder.TabIndex = 4
        Me.optFileTypeFolder.TabStop = True
        Me.optFileTypeFolder.Text = "Folder"
        Me.optFileTypeFolder.UseVisualStyleBackColor = True
        '
        'frmAddWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(359, 237)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.tabWizard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddWizard"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Game Wizard"
        Me.tabWizard.ResumeLayout(False)
        Me.tbPage1.ResumeLayout(False)
        Me.tbPage1.PerformLayout()
        Me.tbPage2.ResumeLayout(False)
        Me.tbPage2.PerformLayout()
        Me.tbPage3.ResumeLayout(False)
        Me.tbPage3.PerformLayout()
        Me.tbPage3a.ResumeLayout(False)
        Me.tbPage3a.PerformLayout()
        Me.grpFileTypes.ResumeLayout(False)
        Me.grpFileTypes.PerformLayout()
        Me.tbPage4.ResumeLayout(False)
        Me.tbPage4.PerformLayout()
        Me.grpExclude.ResumeLayout(False)
        Me.grpExclude.PerformLayout()
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
    Friend WithEvents lblDrag1 As System.Windows.Forms.Label
    Friend WithEvents lblStep1Instructions As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblStep1Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep2Instructions As System.Windows.Forms.Label
    Friend WithEvents lblDrag2 As System.Windows.Forms.Label
    Friend WithEvents btnProcessBrowse As System.Windows.Forms.Button
    Friend WithEvents txtProcessPath As System.Windows.Forms.TextBox
    Friend WithEvents lblStep2Intro As System.Windows.Forms.Label
    Friend WithEvents chkTimeStamp As System.Windows.Forms.CheckBox
    Friend WithEvents chkFolderSave As System.Windows.Forms.CheckBox
    Friend WithEvents btnSaveBrowse As System.Windows.Forms.Button
    Friend WithEvents txtSavePath As System.Windows.Forms.TextBox
    Friend WithEvents lblStep3Intro As System.Windows.Forms.Label
    Friend WithEvents btnFileTypeBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFileTypes As System.Windows.Forms.TextBox
    Friend WithEvents btnExcludeBrowse As System.Windows.Forms.Button
    Friend WithEvents txtExcludeList As System.Windows.Forms.TextBox
    Friend WithEvents optSpecificFile As System.Windows.Forms.RadioButton
    Friend WithEvents optFileType As System.Windows.Forms.RadioButton
    Friend WithEvents lblStep3aInstructions As System.Windows.Forms.Label
    Friend WithEvents optExcludeFolder As System.Windows.Forms.RadioButton
    Friend WithEvents optExcludeFileType As System.Windows.Forms.RadioButton
    Friend WithEvents lblStep4Instructions As System.Windows.Forms.Label
    Friend WithEvents optExcludeSpecificFile As System.Windows.Forms.RadioButton
    Friend WithEvents lblStep3Instructions As System.Windows.Forms.Label
    Friend WithEvents lstSummary As System.Windows.Forms.ListView
    Friend WithEvents lblStep1Title As System.Windows.Forms.Label
    Friend WithEvents lblStep2Title As System.Windows.Forms.Label
    Friend WithEvents lblStep3Title As System.Windows.Forms.Label
    Friend WithEvents lblStep4Title As System.Windows.Forms.Label
    Friend WithEvents lblStep3aTitle As System.Windows.Forms.Label
    Friend WithEvents lblStep5Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep5Title As System.Windows.Forms.Label
    Friend WithEvents btnStep3aClear As System.Windows.Forms.Button
    Friend WithEvents btnStep4Clear As System.Windows.Forms.Button
    Friend WithEvents grpExclude As System.Windows.Forms.GroupBox
    Friend WithEvents grpFileTypes As System.Windows.Forms.GroupBox
    Friend WithEvents lbldBox As System.Windows.Forms.Label
    Friend WithEvents btndBoxBrowse As System.Windows.Forms.Button
    Friend WithEvents txtdBoxProcess As System.Windows.Forms.TextBox
    Friend WithEvents optFileTypeFolder As RadioButton
End Class
