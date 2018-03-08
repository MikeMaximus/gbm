<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcessManager
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
        Me.grpProcess = New System.Windows.Forms.GroupBox()
        Me.txtArguments = New System.Windows.Forms.TextBox()
        Me.lblArguments = New System.Windows.Forms.Label()
        Me.btnProcessBrowse = New System.Windows.Forms.Button()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.lblProcess = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstProcesses = New System.Windows.Forms.ListBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.chkKillProcess = New System.Windows.Forms.CheckBox()
        Me.grpProcess.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpProcess
        '
        Me.grpProcess.Controls.Add(Me.chkKillProcess)
        Me.grpProcess.Controls.Add(Me.txtArguments)
        Me.grpProcess.Controls.Add(Me.lblArguments)
        Me.grpProcess.Controls.Add(Me.btnProcessBrowse)
        Me.grpProcess.Controls.Add(Me.txtName)
        Me.grpProcess.Controls.Add(Me.txtPath)
        Me.grpProcess.Controls.Add(Me.lblProcess)
        Me.grpProcess.Controls.Add(Me.lblName)
        Me.grpProcess.Location = New System.Drawing.Point(238, 12)
        Me.grpProcess.Name = "grpProcess"
        Me.grpProcess.Size = New System.Drawing.Size(334, 120)
        Me.grpProcess.TabIndex = 11
        Me.grpProcess.TabStop = False
        Me.grpProcess.Text = "Configuration"
        '
        'txtArguments
        '
        Me.txtArguments.Location = New System.Drawing.Point(72, 70)
        Me.txtArguments.Name = "txtArguments"
        Me.txtArguments.Size = New System.Drawing.Size(256, 20)
        Me.txtArguments.TabIndex = 5
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(6, 73)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(60, 13)
        Me.lblArguments.TabIndex = 4
        Me.lblArguments.Text = "Arguments:"
        '
        'btnProcessBrowse
        '
        Me.btnProcessBrowse.Location = New System.Drawing.Point(298, 45)
        Me.btnProcessBrowse.Name = "btnProcessBrowse"
        Me.btnProcessBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnProcessBrowse.TabIndex = 3
        Me.btnProcessBrowse.Text = "..."
        Me.btnProcessBrowse.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(72, 19)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(256, 20)
        Me.txtName.TabIndex = 1
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(72, 45)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(220, 20)
        Me.txtPath.TabIndex = 2
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(6, 48)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(48, 13)
        Me.lblProcess.TabIndex = 1
        Me.lblProcess.Text = "Process:"
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
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(497, 226)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 14
        Me.btnClose.Text = "C&lose"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(48, 226)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(30, 23)
        Me.btnDelete.TabIndex = 10
        Me.btnDelete.Text = "-"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(12, 226)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 23)
        Me.btnAdd.TabIndex = 9
        Me.btnAdd.Text = "+"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstProcesses
        '
        Me.lstProcesses.FormattingEnabled = True
        Me.lstProcesses.Location = New System.Drawing.Point(12, 12)
        Me.lstProcesses.Name = "lstProcesses"
        Me.lstProcesses.Size = New System.Drawing.Size(220, 212)
        Me.lstProcesses.Sorted = True
        Me.lstProcesses.TabIndex = 7
        '
        'txtID
        '
        Me.txtID.Enabled = False
        Me.txtID.Location = New System.Drawing.Point(374, 150)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(33, 20)
        Me.txtID.TabIndex = 8
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(494, 149)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(413, 149)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'chkKillProcess
        '
        Me.chkKillProcess.AutoSize = True
        Me.chkKillProcess.Location = New System.Drawing.Point(72, 96)
        Me.chkKillProcess.Name = "chkKillProcess"
        Me.chkKillProcess.Size = New System.Drawing.Size(184, 17)
        Me.chkKillProcess.TabIndex = 6
        Me.chkKillProcess.Text = "Kill process when game is closed."
        Me.chkKillProcess.UseVisualStyleBackColor = True
        '
        'frmProcessManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 261)
        Me.Controls.Add(Me.grpProcess)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstProcesses)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProcessManager"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Process Manager"
        Me.grpProcess.ResumeLayout(False)
        Me.grpProcess.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpProcess As GroupBox
    Friend WithEvents txtArguments As TextBox
    Friend WithEvents lblArguments As Label
    Friend WithEvents btnProcessBrowse As Button
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtPath As TextBox
    Friend WithEvents lblProcess As Label
    Friend WithEvents lblName As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents lstProcesses As ListBox
    Friend WithEvents txtID As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents chkKillProcess As CheckBox
End Class
