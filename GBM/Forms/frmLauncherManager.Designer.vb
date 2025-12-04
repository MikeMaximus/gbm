<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLauncherManager
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
        Me.grpLauncher = New System.Windows.Forms.GroupBox()
        Me.btnLauncherBrowse = New System.Windows.Forms.Button()
        Me.txtLaunchParameters = New System.Windows.Forms.TextBox()
        Me.lblParameters = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtLaunchString = New System.Windows.Forms.TextBox()
        Me.lblCommand = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstLaunchers = New System.Windows.Forms.ListBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnAddDefaults = New System.Windows.Forms.Button()
        Me.grpLauncherType = New System.Windows.Forms.GroupBox()
        Me.optExecutable = New System.Windows.Forms.RadioButton()
        Me.optURI = New System.Windows.Forms.RadioButton()
        Me.grpLauncher.SuspendLayout()
        Me.grpLauncherType.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpLauncher
        '
        Me.grpLauncher.Controls.Add(Me.btnLauncherBrowse)
        Me.grpLauncher.Controls.Add(Me.txtLaunchParameters)
        Me.grpLauncher.Controls.Add(Me.lblParameters)
        Me.grpLauncher.Controls.Add(Me.txtName)
        Me.grpLauncher.Controls.Add(Me.txtLaunchString)
        Me.grpLauncher.Controls.Add(Me.lblCommand)
        Me.grpLauncher.Controls.Add(Me.lblName)
        Me.grpLauncher.Location = New System.Drawing.Point(210, 55)
        Me.grpLauncher.Name = "grpLauncher"
        Me.grpLauncher.Size = New System.Drawing.Size(362, 104)
        Me.grpLauncher.TabIndex = 5
        Me.grpLauncher.TabStop = False
        Me.grpLauncher.Text = "Configuration"
        '
        'btnLauncherBrowse
        '
        Me.btnLauncherBrowse.Location = New System.Drawing.Point(326, 45)
        Me.btnLauncherBrowse.Name = "btnLauncherBrowse"
        Me.btnLauncherBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnLauncherBrowse.TabIndex = 4
        Me.btnLauncherBrowse.Text = "..."
        Me.btnLauncherBrowse.UseVisualStyleBackColor = True
        '
        'txtLaunchParameters
        '
        Me.txtLaunchParameters.Location = New System.Drawing.Point(72, 71)
        Me.txtLaunchParameters.Name = "txtLaunchParameters"
        Me.txtLaunchParameters.Size = New System.Drawing.Size(284, 20)
        Me.txtLaunchParameters.TabIndex = 6
        '
        'lblParameters
        '
        Me.lblParameters.AutoSize = True
        Me.lblParameters.Location = New System.Drawing.Point(6, 74)
        Me.lblParameters.Name = "lblParameters"
        Me.lblParameters.Size = New System.Drawing.Size(63, 13)
        Me.lblParameters.TabIndex = 5
        Me.lblParameters.Text = "Parameters:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(72, 19)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(284, 20)
        Me.txtName.TabIndex = 1
        '
        'txtLaunchString
        '
        Me.txtLaunchString.Location = New System.Drawing.Point(72, 45)
        Me.txtLaunchString.Name = "txtLaunchString"
        Me.txtLaunchString.Size = New System.Drawing.Size(248, 20)
        Me.txtLaunchString.TabIndex = 3
        '
        'lblCommand
        '
        Me.lblCommand.AutoSize = True
        Me.lblCommand.Location = New System.Drawing.Point(6, 48)
        Me.lblCommand.Name = "lblCommand"
        Me.lblCommand.Size = New System.Drawing.Size(57, 13)
        Me.lblCommand.TabIndex = 2
        Me.lblCommand.Text = "Command:"
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
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Image = Global.GBM.My.Resources.Resources.Multi_Delete
        Me.btnDelete.Location = New System.Drawing.Point(98, 165)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(80, 50)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Image = Global.GBM.My.Resources.Resources.Multi_Add
        Me.btnAdd.Location = New System.Drawing.Point(12, 165)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(80, 50)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "&New"
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstLaunchers
        '
        Me.lstLaunchers.FormattingEnabled = True
        Me.lstLaunchers.Location = New System.Drawing.Point(12, 12)
        Me.lstLaunchers.Name = "lstLaunchers"
        Me.lstLaunchers.Size = New System.Drawing.Size(192, 147)
        Me.lstLaunchers.Sorted = True
        Me.lstLaunchers.TabIndex = 0
        '
        'txtID
        '
        Me.txtID.Enabled = False
        Me.txtID.Location = New System.Drawing.Point(367, 181)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(33, 20)
        Me.txtID.TabIndex = 0
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(492, 165)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 50)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GBM.My.Resources.Resources.Multi_Save
        Me.btnSave.Location = New System.Drawing.Point(406, 165)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 50)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnAddDefaults
        '
        Me.btnAddDefaults.Image = Global.GBM.My.Resources.Resources.Multi_Reset
        Me.btnAddDefaults.Location = New System.Drawing.Point(184, 165)
        Me.btnAddDefaults.Name = "btnAddDefaults"
        Me.btnAddDefaults.Size = New System.Drawing.Size(80, 50)
        Me.btnAddDefaults.TabIndex = 3
        Me.btnAddDefaults.Text = "D&efaults"
        Me.btnAddDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAddDefaults.UseVisualStyleBackColor = True
        '
        'grpLauncherType
        '
        Me.grpLauncherType.Controls.Add(Me.optExecutable)
        Me.grpLauncherType.Controls.Add(Me.optURI)
        Me.grpLauncherType.Location = New System.Drawing.Point(210, 12)
        Me.grpLauncherType.Name = "grpLauncherType"
        Me.grpLauncherType.Size = New System.Drawing.Size(362, 37)
        Me.grpLauncherType.TabIndex = 4
        Me.grpLauncherType.TabStop = False
        Me.grpLauncherType.Text = "Launcher Type"
        '
        'optExecutable
        '
        Me.optExecutable.AutoSize = True
        Me.optExecutable.Location = New System.Drawing.Point(59, 14)
        Me.optExecutable.Name = "optExecutable"
        Me.optExecutable.Size = New System.Drawing.Size(78, 17)
        Me.optExecutable.TabIndex = 1
        Me.optExecutable.TabStop = True
        Me.optExecutable.Text = "Executable"
        Me.optExecutable.UseVisualStyleBackColor = True
        '
        'optURI
        '
        Me.optURI.AutoSize = True
        Me.optURI.Location = New System.Drawing.Point(9, 14)
        Me.optURI.Name = "optURI"
        Me.optURI.Size = New System.Drawing.Size(44, 17)
        Me.optURI.TabIndex = 0
        Me.optURI.TabStop = True
        Me.optURI.Text = "URI"
        Me.optURI.UseVisualStyleBackColor = True
        '
        'frmLauncherManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 221)
        Me.Controls.Add(Me.grpLauncherType)
        Me.Controls.Add(Me.btnAddDefaults)
        Me.Controls.Add(Me.grpLauncher)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstLaunchers)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLauncherManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Launcher Manager"
        Me.grpLauncher.ResumeLayout(False)
        Me.grpLauncher.PerformLayout()
        Me.grpLauncherType.ResumeLayout(False)
        Me.grpLauncherType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpLauncher As GroupBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtLaunchString As TextBox
    Friend WithEvents lblCommand As Label
    Friend WithEvents lblName As Label
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents lstLaunchers As ListBox
    Friend WithEvents txtID As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnAddDefaults As Button
    Friend WithEvents txtLaunchParameters As TextBox
    Friend WithEvents lblParameters As Label
    Friend WithEvents grpLauncherType As GroupBox
    Friend WithEvents optExecutable As RadioButton
    Friend WithEvents optURI As RadioButton
    Friend WithEvents btnLauncherBrowse As Button
End Class
