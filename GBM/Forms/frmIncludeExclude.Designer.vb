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
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.grpFileOptions = New System.Windows.Forms.GroupBox()
        Me.optIndividualFiles = New System.Windows.Forms.RadioButton()
        Me.optFileTypes = New System.Windows.Forms.RadioButton()
        Me.lblItems = New System.Windows.Forms.Label()
        Me.grpFileOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'treFiles
        '
        Me.treFiles.CheckBoxes = True
        Me.treFiles.ImageIndex = 0
        Me.treFiles.ImageList = Me.imgIcons
        Me.treFiles.Location = New System.Drawing.Point(12, 38)
        Me.treFiles.Name = "treFiles"
        Me.treFiles.SelectedImageIndex = 0
        Me.treFiles.Size = New System.Drawing.Size(250, 359)
        Me.treFiles.TabIndex = 0
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.imgIcons.Images.SetKeyName(0, "folder.ico")
        Me.imgIcons.Images.SetKeyName(1, "file.ico")
        Me.imgIcons.Images.SetKeyName(2, "type.ico")
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(416, 426)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(497, 426)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtRootFolder
        '
        Me.txtRootFolder.Location = New System.Drawing.Point(12, 12)
        Me.txtRootFolder.Name = "txtRootFolder"
        Me.txtRootFolder.ReadOnly = True
        Me.txtRootFolder.Size = New System.Drawing.Size(194, 20)
        Me.txtRootFolder.TabIndex = 3
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(212, 12)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnBrowse.TabIndex = 9
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lstBuilder
        '
        Me.lstBuilder.Location = New System.Drawing.Point(322, 38)
        Me.lstBuilder.Name = "lstBuilder"
        Me.lstBuilder.Size = New System.Drawing.Size(250, 359)
        Me.lstBuilder.SmallImageList = Me.imgIcons
        Me.lstBuilder.TabIndex = 10
        Me.lstBuilder.UseCompatibleStateImageBehavior = False
        Me.lstBuilder.View = System.Windows.Forms.View.List
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(268, 201)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 12
        Me.btnRemove.Text = "< <"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(268, 172)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 11
        Me.btnAdd.Text = "> >"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'grpFileOptions
        '
        Me.grpFileOptions.Controls.Add(Me.optFileTypes)
        Me.grpFileOptions.Controls.Add(Me.optIndividualFiles)
        Me.grpFileOptions.Location = New System.Drawing.Point(12, 403)
        Me.grpFileOptions.Name = "grpFileOptions"
        Me.grpFileOptions.Size = New System.Drawing.Size(194, 46)
        Me.grpFileOptions.TabIndex = 13
        Me.grpFileOptions.TabStop = False
        Me.grpFileOptions.Text = "File Options"
        '
        'optIndividualFiles
        '
        Me.optIndividualFiles.AutoSize = True
        Me.optIndividualFiles.Location = New System.Drawing.Point(85, 19)
        Me.optIndividualFiles.Name = "optIndividualFiles"
        Me.optIndividualFiles.Size = New System.Drawing.Size(94, 17)
        Me.optIndividualFiles.TabIndex = 14
        Me.optIndividualFiles.TabStop = True
        Me.optIndividualFiles.Text = "Individual Files"
        Me.optIndividualFiles.UseVisualStyleBackColor = True
        '
        'optFileTypes
        '
        Me.optFileTypes.AutoSize = True
        Me.optFileTypes.Location = New System.Drawing.Point(6, 19)
        Me.optFileTypes.Name = "optFileTypes"
        Me.optFileTypes.Size = New System.Drawing.Size(73, 17)
        Me.optFileTypes.TabIndex = 15
        Me.optFileTypes.TabStop = True
        Me.optFileTypes.Text = "File Types"
        Me.optFileTypes.UseVisualStyleBackColor = True
        '
        'lblItems
        '
        Me.lblItems.AutoSize = True
        Me.lblItems.Location = New System.Drawing.Point(319, 16)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(32, 13)
        Me.lblItems.TabIndex = 14
        Me.lblItems.Text = "Items"
        '
        'frmIncludeExclude
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 461)
        Me.Controls.Add(Me.lblItems)
        Me.Controls.Add(Me.grpFileOptions)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstBuilder)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtRootFolder)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.treFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmIncludeExclude"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Include / Exclude Builder"
        Me.grpFileOptions.ResumeLayout(False)
        Me.grpFileOptions.PerformLayout()
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
    Friend WithEvents grpFileOptions As System.Windows.Forms.GroupBox
    Friend WithEvents optFileTypes As System.Windows.Forms.RadioButton
    Friend WithEvents optIndividualFiles As System.Windows.Forms.RadioButton
    Friend WithEvents lblItems As System.Windows.Forms.Label
End Class
