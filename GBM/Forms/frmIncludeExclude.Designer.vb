<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIncludeExclude
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIncludeExclude))
        Me.treFiles = New System.Windows.Forms.TreeView()
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtRootFolder = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.lstBuilder = New System.Windows.Forms.ListView()
        Me.cmsItems = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.grpOptions = New System.Windows.Forms.GroupBox()
        Me.chkRecurseSubFolders = New System.Windows.Forms.CheckBox()
        Me.optFileTypes = New System.Windows.Forms.RadioButton()
        Me.optIndividualFiles = New System.Windows.Forms.RadioButton()
        Me.lblItems = New System.Windows.Forms.Label()
        Me.btnRawEdit = New System.Windows.Forms.Button()
        Me.lblSaveFolder = New System.Windows.Forms.Label()
        Me.ttWarning = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmsItems.SuspendLayout()
        Me.grpOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'treFiles
        '
        Me.treFiles.CheckBoxes = True
        Me.treFiles.ImageIndex = 0
        Me.treFiles.ImageList = Me.imgIcons
        Me.treFiles.Location = New System.Drawing.Point(12, 64)
        Me.treFiles.Name = "treFiles"
        Me.treFiles.SelectedImageIndex = 0
        Me.treFiles.Size = New System.Drawing.Size(280, 333)
        Me.treFiles.TabIndex = 1
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.imgIcons.Images.SetKeyName(0, "frmIncludeExclude_Folder.ico")
        Me.imgIcons.Images.SetKeyName(1, "frmIncludeExclude_File.ico")
        Me.imgIcons.Images.SetKeyName(2, "frmIncludeExclude_Type.ico")
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GBM.My.Resources.Resources.Multi_Save
        Me.btnSave.Location = New System.Drawing.Point(465, 405)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 50)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(551, 405)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 50)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtRootFolder
        '
        Me.txtRootFolder.Location = New System.Drawing.Point(12, 38)
        Me.txtRootFolder.Name = "txtRootFolder"
        Me.txtRootFolder.ReadOnly = True
        Me.txtRootFolder.Size = New System.Drawing.Size(244, 20)
        Me.txtRootFolder.TabIndex = 0
        Me.txtRootFolder.TabStop = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(262, 37)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnBrowse.TabIndex = 0
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lstBuilder
        '
        Me.lstBuilder.ContextMenuStrip = Me.cmsItems
        Me.lstBuilder.HideSelection = False
        Me.lstBuilder.LabelEdit = True
        Me.lstBuilder.Location = New System.Drawing.Point(352, 39)
        Me.lstBuilder.Name = "lstBuilder"
        Me.lstBuilder.Size = New System.Drawing.Size(280, 359)
        Me.lstBuilder.SmallImageList = Me.imgIcons
        Me.lstBuilder.TabIndex = 5
        Me.lstBuilder.UseCompatibleStateImageBehavior = False
        Me.lstBuilder.View = System.Windows.Forms.View.List
        '
        'cmsItems
        '
        Me.cmsItems.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmsEdit, Me.cmsRemove, Me.cmsAdd})
        Me.cmsItems.Name = "cmsItems"
        Me.cmsItems.Size = New System.Drawing.Size(169, 70)
        '
        'cmsEdit
        '
        Me.cmsEdit.Name = "cmsEdit"
        Me.cmsEdit.Size = New System.Drawing.Size(168, 22)
        Me.cmsEdit.Text = "Edit"
        '
        'cmsRemove
        '
        Me.cmsRemove.Name = "cmsRemove"
        Me.cmsRemove.Size = New System.Drawing.Size(168, 22)
        Me.cmsRemove.Text = "Remove"
        '
        'cmsAdd
        '
        Me.cmsAdd.Name = "cmsAdd"
        Me.cmsAdd.Size = New System.Drawing.Size(168, 22)
        Me.cmsAdd.Text = "Add Custom Item"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(298, 201)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 4
        Me.btnRemove.Text = "<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(298, 172)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 3
        Me.btnAdd.Text = ">"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'grpOptions
        '
        Me.grpOptions.Controls.Add(Me.chkRecurseSubFolders)
        Me.grpOptions.Controls.Add(Me.optFileTypes)
        Me.grpOptions.Controls.Add(Me.optIndividualFiles)
        Me.grpOptions.Location = New System.Drawing.Point(12, 403)
        Me.grpOptions.Name = "grpOptions"
        Me.grpOptions.Size = New System.Drawing.Size(361, 46)
        Me.grpOptions.TabIndex = 2
        Me.grpOptions.TabStop = False
        Me.grpOptions.Text = "Options"
        '
        'chkRecurseSubFolders
        '
        Me.chkRecurseSubFolders.AutoSize = True
        Me.chkRecurseSubFolders.Location = New System.Drawing.Point(185, 20)
        Me.chkRecurseSubFolders.Name = "chkRecurseSubFolders"
        Me.chkRecurseSubFolders.Size = New System.Drawing.Size(120, 17)
        Me.chkRecurseSubFolders.TabIndex = 2
        Me.chkRecurseSubFolders.Text = "Recurse sub-folders"
        Me.chkRecurseSubFolders.UseVisualStyleBackColor = True
        '
        'optFileTypes
        '
        Me.optFileTypes.Location = New System.Drawing.Point(6, 19)
        Me.optFileTypes.Name = "optFileTypes"
        Me.optFileTypes.Size = New System.Drawing.Size(73, 17)
        Me.optFileTypes.TabIndex = 0
        Me.optFileTypes.TabStop = True
        Me.optFileTypes.Text = "File Types"
        Me.optFileTypes.UseVisualStyleBackColor = True
        '
        'optIndividualFiles
        '
        Me.optIndividualFiles.Location = New System.Drawing.Point(85, 19)
        Me.optIndividualFiles.Name = "optIndividualFiles"
        Me.optIndividualFiles.Size = New System.Drawing.Size(94, 17)
        Me.optIndividualFiles.TabIndex = 1
        Me.optIndividualFiles.TabStop = True
        Me.optIndividualFiles.Text = "Individual Files"
        Me.optIndividualFiles.UseVisualStyleBackColor = True
        '
        'lblItems
        '
        Me.lblItems.Location = New System.Drawing.Point(352, 17)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(280, 13)
        Me.lblItems.TabIndex = 14
        Me.lblItems.Text = "Items"
        Me.lblItems.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRawEdit
        '
        Me.btnRawEdit.Image = Global.GBM.My.Resources.Resources.Multi_Edit
        Me.btnRawEdit.Location = New System.Drawing.Point(379, 404)
        Me.btnRawEdit.Name = "btnRawEdit"
        Me.btnRawEdit.Size = New System.Drawing.Size(80, 50)
        Me.btnRawEdit.TabIndex = 6
        Me.btnRawEdit.Text = "Raw &Edit"
        Me.btnRawEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRawEdit.UseVisualStyleBackColor = True
        '
        'lblSaveFolder
        '
        Me.lblSaveFolder.Location = New System.Drawing.Point(12, 16)
        Me.lblSaveFolder.Name = "lblSaveFolder"
        Me.lblSaveFolder.Size = New System.Drawing.Size(244, 13)
        Me.lblSaveFolder.TabIndex = 15
        Me.lblSaveFolder.Text = "Saved Game Explorer"
        Me.lblSaveFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ttWarning
        '
        Me.ttWarning.AutoPopDelay = 5000
        Me.ttWarning.InitialDelay = 200
        Me.ttWarning.ReshowDelay = 50
        Me.ttWarning.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning
        '
        'frmIncludeExclude
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 461)
        Me.Controls.Add(Me.lblSaveFolder)
        Me.Controls.Add(Me.btnRawEdit)
        Me.Controls.Add(Me.lblItems)
        Me.Controls.Add(Me.grpOptions)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstBuilder)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtRootFolder)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.treFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmIncludeExclude"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Include / Exclude Builder"
        Me.cmsItems.ResumeLayout(False)
        Me.grpOptions.ResumeLayout(False)
        Me.grpOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents treFiles As System.Windows.Forms.TreeView
    Friend WithEvents imgIcons As System.Windows.Forms.ImageList
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtRootFolder As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents lstBuilder As System.Windows.Forms.ListView
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents grpOptions As System.Windows.Forms.GroupBox
    Friend WithEvents optFileTypes As System.Windows.Forms.RadioButton
    Friend WithEvents optIndividualFiles As System.Windows.Forms.RadioButton
    Friend WithEvents lblItems As System.Windows.Forms.Label
    Friend WithEvents cmsItems As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmsEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsRemove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnRawEdit As Button
    Friend WithEvents lblSaveFolder As Label
    Friend WithEvents ttWarning As ToolTip
    Friend WithEvents chkRecurseSubFolders As CheckBox
End Class
