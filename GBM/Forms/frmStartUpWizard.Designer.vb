<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStartUpWizard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStartUpWizard))
        Me.tabWizard = New System.Windows.Forms.TabControl()
        Me.tbPage1 = New System.Windows.Forms.TabPage()
        Me.lblLanguage = New System.Windows.Forms.Label()
        Me.cboLanguage = New System.Windows.Forms.ComboBox()
        Me.lblStep1Instructions2 = New System.Windows.Forms.Label()
        Me.llbManual = New System.Windows.Forms.LinkLabel()
        Me.lblStep1Title = New System.Windows.Forms.Label()
        Me.lblStep1Instructions = New System.Windows.Forms.Label()
        Me.tbPage2 = New System.Windows.Forms.TabPage()
        Me.lblStep2Warning = New System.Windows.Forms.Label()
        Me.chkCreateFolder = New System.Windows.Forms.CheckBox()
        Me.lblStep2Title = New System.Windows.Forms.Label()
        Me.lblStep2Instructions = New System.Windows.Forms.Label()
        Me.btnFolderBrowse = New System.Windows.Forms.Button()
        Me.txtBackupPath = New System.Windows.Forms.TextBox()
        Me.lblStep2Intro = New System.Windows.Forms.Label()
        Me.tbPage3 = New System.Windows.Forms.TabPage()
        Me.btnOpenWizard = New System.Windows.Forms.Button()
        Me.btnOpenMonitorList = New System.Windows.Forms.Button()
        Me.btnDownloadList = New System.Windows.Forms.Button()
        Me.lblStep3Title = New System.Windows.Forms.Label()
        Me.lblStep3Intro = New System.Windows.Forms.Label()
        Me.tbPage4 = New System.Windows.Forms.TabPage()
        Me.lblStep4Instructions3 = New System.Windows.Forms.Label()
        Me.lblStep4Instructions2 = New System.Windows.Forms.Label()
        Me.lblStep4Title = New System.Windows.Forms.Label()
        Me.lblStep4Instructions = New System.Windows.Forms.Label()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.tabWizard.SuspendLayout()
        Me.tbPage1.SuspendLayout()
        Me.tbPage2.SuspendLayout()
        Me.tbPage3.SuspendLayout()
        Me.tbPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabWizard
        '
        Me.tabWizard.Controls.Add(Me.tbPage1)
        Me.tabWizard.Controls.Add(Me.tbPage2)
        Me.tabWizard.Controls.Add(Me.tbPage3)
        Me.tabWizard.Controls.Add(Me.tbPage4)
        Me.tabWizard.Location = New System.Drawing.Point(-6, -24)
        Me.tabWizard.Name = "tabWizard"
        Me.tabWizard.SelectedIndex = 0
        Me.tabWizard.Size = New System.Drawing.Size(446, 272)
        Me.tabWizard.TabIndex = 0
        Me.tabWizard.TabStop = False
        '
        'tbPage1
        '
        Me.tbPage1.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage1.Controls.Add(Me.lblLanguage)
        Me.tbPage1.Controls.Add(Me.cboLanguage)
        Me.tbPage1.Controls.Add(Me.lblStep1Instructions2)
        Me.tbPage1.Controls.Add(Me.llbManual)
        Me.tbPage1.Controls.Add(Me.lblStep1Title)
        Me.tbPage1.Controls.Add(Me.lblStep1Instructions)
        Me.tbPage1.Location = New System.Drawing.Point(4, 22)
        Me.tbPage1.Name = "tbPage1"
        Me.tbPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage1.Size = New System.Drawing.Size(438, 246)
        Me.tbPage1.TabIndex = 0
        Me.tbPage1.Text = "TabPage1"
        '
        'lblLanguage
        '
        Me.lblLanguage.AutoSize = True
        Me.lblLanguage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblLanguage.Location = New System.Drawing.Point(15, 161)
        Me.lblLanguage.Name = "lblLanguage"
        Me.lblLanguage.Size = New System.Drawing.Size(72, 16)
        Me.lblLanguage.TabIndex = 4
        Me.lblLanguage.Text = "Language:"
        '
        'cboLanguage
        '
        Me.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanguage.FormattingEnabled = True
        Me.cboLanguage.Location = New System.Drawing.Point(18, 180)
        Me.cboLanguage.Name = "cboLanguage"
        Me.cboLanguage.Size = New System.Drawing.Size(300, 21)
        Me.cboLanguage.TabIndex = 0
        '
        'lblStep1Instructions2
        '
        Me.lblStep1Instructions2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Instructions2.Location = New System.Drawing.Point(15, 96)
        Me.lblStep1Instructions2.Name = "lblStep1Instructions2"
        Me.lblStep1Instructions2.Size = New System.Drawing.Size(409, 53)
        Me.lblStep1Instructions2.TabIndex = 2
        Me.lblStep1Instructions2.Text = "If you'd like to learn about advanced features or have any other questions before" &
    " you get started, there is a detailed online manual available."
        '
        'llbManual
        '
        Me.llbManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llbManual.Location = New System.Drawing.Point(15, 213)
        Me.llbManual.Name = "llbManual"
        Me.llbManual.Size = New System.Drawing.Size(409, 20)
        Me.llbManual.TabIndex = 1
        Me.llbManual.TabStop = True
        Me.llbManual.Text = "Game Backup Monitor Manual"
        '
        'lblStep1Title
        '
        Me.lblStep1Title.AutoSize = True
        Me.lblStep1Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep1Title.Name = "lblStep1Title"
        Me.lblStep1Title.Size = New System.Drawing.Size(286, 20)
        Me.lblStep1Title.TabIndex = 0
        Me.lblStep1Title.Text = "Welcome to Game Backup Monitor"
        '
        'lblStep1Instructions
        '
        Me.lblStep1Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep1Instructions.Location = New System.Drawing.Point(15, 46)
        Me.lblStep1Instructions.Name = "lblStep1Instructions"
        Me.lblStep1Instructions.Size = New System.Drawing.Size(409, 36)
        Me.lblStep1Instructions.TabIndex = 1
        Me.lblStep1Instructions.Text = "This guide will help you through some quick and easy steps to get started."
        '
        'tbPage2
        '
        Me.tbPage2.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage2.Controls.Add(Me.lblStep2Warning)
        Me.tbPage2.Controls.Add(Me.chkCreateFolder)
        Me.tbPage2.Controls.Add(Me.lblStep2Title)
        Me.tbPage2.Controls.Add(Me.lblStep2Instructions)
        Me.tbPage2.Controls.Add(Me.btnFolderBrowse)
        Me.tbPage2.Controls.Add(Me.txtBackupPath)
        Me.tbPage2.Controls.Add(Me.lblStep2Intro)
        Me.tbPage2.Location = New System.Drawing.Point(4, 22)
        Me.tbPage2.Name = "tbPage2"
        Me.tbPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tbPage2.Size = New System.Drawing.Size(438, 246)
        Me.tbPage2.TabIndex = 1
        Me.tbPage2.Text = "TabPage2"
        '
        'lblStep2Warning
        '
        Me.lblStep2Warning.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Warning.Location = New System.Drawing.Point(15, 177)
        Me.lblStep2Warning.Name = "lblStep2Warning"
        Me.lblStep2Warning.Size = New System.Drawing.Size(410, 33)
        Me.lblStep2Warning.TabIndex = 7
        Me.lblStep2Warning.Text = "You cannot return to this step after clicking Next.  The Backup Location can be c" &
    "hanged any time once Setup is complete."
        '
        'chkCreateFolder
        '
        Me.chkCreateFolder.AutoSize = True
        Me.chkCreateFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCreateFolder.Location = New System.Drawing.Point(17, 93)
        Me.chkCreateFolder.Name = "chkCreateFolder"
        Me.chkCreateFolder.Size = New System.Drawing.Size(186, 17)
        Me.chkCreateFolder.TabIndex = 2
        Me.chkCreateFolder.Text = "Create a sub-folder for each game"
        Me.chkCreateFolder.UseVisualStyleBackColor = True
        '
        'lblStep2Title
        '
        Me.lblStep2Title.AutoSize = True
        Me.lblStep2Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep2Title.Name = "lblStep2Title"
        Me.lblStep2Title.Size = New System.Drawing.Size(143, 20)
        Me.lblStep2Title.TabIndex = 0
        Me.lblStep2Title.Text = "Backup Location"
        '
        'lblStep2Instructions
        '
        Me.lblStep2Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Instructions.Location = New System.Drawing.Point(14, 121)
        Me.lblStep2Instructions.Name = "lblStep2Instructions"
        Me.lblStep2Instructions.Size = New System.Drawing.Size(409, 51)
        Me.lblStep2Instructions.TabIndex = 6
        Me.lblStep2Instructions.Text = "All your backup files along with a manifest database (gbm.s3db) will be stored in" &
    " this location.  Any existing GBM data in this folder will be automatically impo" &
    "rted."
        '
        'btnFolderBrowse
        '
        Me.btnFolderBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFolderBrowse.Location = New System.Drawing.Point(323, 67)
        Me.btnFolderBrowse.Name = "btnFolderBrowse"
        Me.btnFolderBrowse.Size = New System.Drawing.Size(27, 20)
        Me.btnFolderBrowse.TabIndex = 1
        Me.btnFolderBrowse.Text = "..."
        Me.btnFolderBrowse.UseVisualStyleBackColor = True
        '
        'txtBackupPath
        '
        Me.txtBackupPath.AllowDrop = True
        Me.txtBackupPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBackupPath.Location = New System.Drawing.Point(17, 67)
        Me.txtBackupPath.Name = "txtBackupPath"
        Me.txtBackupPath.Size = New System.Drawing.Size(300, 20)
        Me.txtBackupPath.TabIndex = 0
        '
        'lblStep2Intro
        '
        Me.lblStep2Intro.AutoSize = True
        Me.lblStep2Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep2Intro.Location = New System.Drawing.Point(14, 46)
        Me.lblStep2Intro.Name = "lblStep2Intro"
        Me.lblStep2Intro.Size = New System.Drawing.Size(253, 16)
        Me.lblStep2Intro.TabIndex = 1
        Me.lblStep2Intro.Text = "Choose where backup files will be saved:"
        '
        'tbPage3
        '
        Me.tbPage3.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage3.Controls.Add(Me.btnOpenWizard)
        Me.tbPage3.Controls.Add(Me.btnOpenMonitorList)
        Me.tbPage3.Controls.Add(Me.btnDownloadList)
        Me.tbPage3.Controls.Add(Me.lblStep3Title)
        Me.tbPage3.Controls.Add(Me.lblStep3Intro)
        Me.tbPage3.Location = New System.Drawing.Point(4, 22)
        Me.tbPage3.Name = "tbPage3"
        Me.tbPage3.Size = New System.Drawing.Size(438, 246)
        Me.tbPage3.TabIndex = 2
        Me.tbPage3.Text = "TabPage3"
        '
        'btnOpenWizard
        '
        Me.btnOpenWizard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenWizard.Location = New System.Drawing.Point(121, 173)
        Me.btnOpenWizard.Name = "btnOpenWizard"
        Me.btnOpenWizard.Size = New System.Drawing.Size(196, 23)
        Me.btnOpenWizard.TabIndex = 1
        Me.btnOpenWizard.Text = "Add Game Wizard"
        Me.btnOpenWizard.UseVisualStyleBackColor = True
        '
        'btnOpenMonitorList
        '
        Me.btnOpenMonitorList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenMonitorList.Location = New System.Drawing.Point(121, 202)
        Me.btnOpenMonitorList.Name = "btnOpenMonitorList"
        Me.btnOpenMonitorList.Size = New System.Drawing.Size(196, 23)
        Me.btnOpenMonitorList.TabIndex = 2
        Me.btnOpenMonitorList.Text = "Game Manager"
        Me.btnOpenMonitorList.UseVisualStyleBackColor = True
        '
        'btnDownloadList
        '
        Me.btnDownloadList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDownloadList.Location = New System.Drawing.Point(121, 144)
        Me.btnDownloadList.Name = "btnDownloadList"
        Me.btnDownloadList.Size = New System.Drawing.Size(196, 23)
        Me.btnDownloadList.TabIndex = 0
        Me.btnDownloadList.Text = "Import from Official List"
        Me.btnDownloadList.UseVisualStyleBackColor = True
        '
        'lblStep3Title
        '
        Me.lblStep3Title.AutoSize = True
        Me.lblStep3Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep3Title.Name = "lblStep3Title"
        Me.lblStep3Title.Size = New System.Drawing.Size(155, 20)
        Me.lblStep3Title.TabIndex = 0
        Me.lblStep3Title.Text = "Monitoring Games"
        '
        'lblStep3Intro
        '
        Me.lblStep3Intro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep3Intro.Location = New System.Drawing.Point(14, 46)
        Me.lblStep3Intro.Name = "lblStep3Intro"
        Me.lblStep3Intro.Size = New System.Drawing.Size(410, 95)
        Me.lblStep3Intro.TabIndex = 0
        Me.lblStep3Intro.Text = resources.GetString("lblStep3Intro.Text")
        '
        'tbPage4
        '
        Me.tbPage4.BackColor = System.Drawing.SystemColors.Control
        Me.tbPage4.Controls.Add(Me.lblStep4Instructions3)
        Me.tbPage4.Controls.Add(Me.lblStep4Instructions2)
        Me.tbPage4.Controls.Add(Me.lblStep4Title)
        Me.tbPage4.Controls.Add(Me.lblStep4Instructions)
        Me.tbPage4.Location = New System.Drawing.Point(4, 22)
        Me.tbPage4.Name = "tbPage4"
        Me.tbPage4.Size = New System.Drawing.Size(438, 246)
        Me.tbPage4.TabIndex = 4
        Me.tbPage4.Text = "TabPage5"
        '
        'lblStep4Instructions3
        '
        Me.lblStep4Instructions3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Instructions3.Location = New System.Drawing.Point(14, 130)
        Me.lblStep4Instructions3.Name = "lblStep4Instructions3"
        Me.lblStep4Instructions3.Size = New System.Drawing.Size(409, 52)
        Me.lblStep4Instructions3.TabIndex = 0
        Me.lblStep4Instructions3.Text = "You can change anything you've setup in this wizard and find more settings and fe" &
    "atures by exploring the menus. "
        '
        'lblStep4Instructions2
        '
        Me.lblStep4Instructions2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Instructions2.Location = New System.Drawing.Point(14, 98)
        Me.lblStep4Instructions2.Name = "lblStep4Instructions2"
        Me.lblStep4Instructions2.Size = New System.Drawing.Size(410, 32)
        Me.lblStep4Instructions2.TabIndex = 0
        Me.lblStep4Instructions2.Text = "Just remember GBM can only currently monitor one game at a time."
        '
        'lblStep4Title
        '
        Me.lblStep4Title.AutoSize = True
        Me.lblStep4Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Title.Location = New System.Drawing.Point(14, 15)
        Me.lblStep4Title.Name = "lblStep4Title"
        Me.lblStep4Title.Size = New System.Drawing.Size(143, 20)
        Me.lblStep4Title.TabIndex = 0
        Me.lblStep4Title.Text = "Setup Complete!"
        '
        'lblStep4Instructions
        '
        Me.lblStep4Instructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep4Instructions.Location = New System.Drawing.Point(14, 46)
        Me.lblStep4Instructions.Name = "lblStep4Instructions"
        Me.lblStep4Instructions.Size = New System.Drawing.Size(410, 42)
        Me.lblStep4Instructions.TabIndex = 0
        Me.lblStep4Instructions.Text = "Game Backup Monitor will automatically monitor and backup your games each time th" &
    "ey are closed."
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Image = Global.GBM.My.Resources.Resources.Multi_Next
        Me.btnNext.Location = New System.Drawing.Point(342, 254)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(80, 50)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "&Next"
        Me.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.Image = Global.GBM.My.Resources.Resources.Multi_Back
        Me.btnBack.Location = New System.Drawing.Point(256, 254)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(80, 50)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "&Back"
        Me.btnBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'frmStartUpWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 311)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.tabWizard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStartUpWizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Backup Monitor - Setup"
        Me.tabWizard.ResumeLayout(False)
        Me.tbPage1.ResumeLayout(False)
        Me.tbPage1.PerformLayout()
        Me.tbPage2.ResumeLayout(False)
        Me.tbPage2.PerformLayout()
        Me.tbPage3.ResumeLayout(False)
        Me.tbPage3.PerformLayout()
        Me.tbPage4.ResumeLayout(False)
        Me.tbPage4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabWizard As System.Windows.Forms.TabControl
    Friend WithEvents tbPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tbPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents tbPage3 As System.Windows.Forms.TabPage
    Friend WithEvents tbPage4 As System.Windows.Forms.TabPage
    Friend WithEvents lblStep1Instructions As System.Windows.Forms.Label
    Friend WithEvents lblStep2Instructions As System.Windows.Forms.Label
    Friend WithEvents btnFolderBrowse As System.Windows.Forms.Button
    Friend WithEvents txtBackupPath As System.Windows.Forms.TextBox
    Friend WithEvents lblStep2Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep3Intro As System.Windows.Forms.Label
    Friend WithEvents lblStep4Instructions As System.Windows.Forms.Label
    Friend WithEvents lblStep1Title As System.Windows.Forms.Label
    Friend WithEvents lblStep2Title As System.Windows.Forms.Label
    Friend WithEvents lblStep3Title As System.Windows.Forms.Label
    Friend WithEvents lblStep4Title As System.Windows.Forms.Label
    Friend WithEvents chkCreateFolder As System.Windows.Forms.CheckBox
    Friend WithEvents btnDownloadList As System.Windows.Forms.Button
    Friend WithEvents btnOpenWizard As System.Windows.Forms.Button
    Friend WithEvents btnOpenMonitorList As System.Windows.Forms.Button
    Friend WithEvents lblStep4Instructions3 As System.Windows.Forms.Label
    Friend WithEvents lblStep4Instructions2 As System.Windows.Forms.Label
    Friend WithEvents lblStep1Instructions2 As System.Windows.Forms.Label
    Friend WithEvents llbManual As System.Windows.Forms.LinkLabel
    Friend WithEvents lblStep2Warning As Label
    Friend WithEvents cboLanguage As ComboBox
    Friend WithEvents lblLanguage As Label
End Class
