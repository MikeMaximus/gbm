<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSyncFields
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
        Me.grpFields = New System.Windows.Forms.GroupBox()
        Me.chkMonitorGame = New System.Windows.Forms.CheckBox()
        Me.chkIcon = New System.Windows.Forms.CheckBox()
        Me.chkVersion = New System.Windows.Forms.CheckBox()
        Me.chkCompany = New System.Windows.Forms.CheckBox()
        Me.chkGamePath = New System.Windows.Forms.CheckBox()
        Me.chkTimeStamp = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.grpFields.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFields
        '
        Me.grpFields.Controls.Add(Me.chkMonitorGame)
        Me.grpFields.Controls.Add(Me.chkIcon)
        Me.grpFields.Controls.Add(Me.chkVersion)
        Me.grpFields.Controls.Add(Me.chkCompany)
        Me.grpFields.Controls.Add(Me.chkGamePath)
        Me.grpFields.Controls.Add(Me.chkTimeStamp)
        Me.grpFields.Location = New System.Drawing.Point(12, 12)
        Me.grpFields.Name = "grpFields"
        Me.grpFields.Size = New System.Drawing.Size(195, 162)
        Me.grpFields.TabIndex = 0
        Me.grpFields.TabStop = False
        Me.grpFields.Text = "Available Fields"
        '
        'chkMonitorGame
        '
        Me.chkMonitorGame.AutoSize = True
        Me.chkMonitorGame.Location = New System.Drawing.Point(6, 134)
        Me.chkMonitorGame.Name = "chkMonitorGame"
        Me.chkMonitorGame.Size = New System.Drawing.Size(109, 17)
        Me.chkMonitorGame.TabIndex = 5
        Me.chkMonitorGame.Text = "Monitor this game"
        Me.chkMonitorGame.UseVisualStyleBackColor = True
        '
        'chkIcon
        '
        Me.chkIcon.AutoSize = True
        Me.chkIcon.Location = New System.Drawing.Point(6, 111)
        Me.chkIcon.Name = "chkIcon"
        Me.chkIcon.Size = New System.Drawing.Size(148, 17)
        Me.chkIcon.TabIndex = 4
        Me.chkIcon.Text = "Icon (Not Recommended)"
        Me.chkIcon.UseVisualStyleBackColor = True
        '
        'chkVersion
        '
        Me.chkVersion.AutoSize = True
        Me.chkVersion.Location = New System.Drawing.Point(6, 88)
        Me.chkVersion.Name = "chkVersion"
        Me.chkVersion.Size = New System.Drawing.Size(61, 17)
        Me.chkVersion.TabIndex = 3
        Me.chkVersion.Text = "Version"
        Me.chkVersion.UseVisualStyleBackColor = True
        '
        'chkCompany
        '
        Me.chkCompany.AutoSize = True
        Me.chkCompany.Location = New System.Drawing.Point(6, 65)
        Me.chkCompany.Name = "chkCompany"
        Me.chkCompany.Size = New System.Drawing.Size(70, 17)
        Me.chkCompany.TabIndex = 2
        Me.chkCompany.Text = "Company"
        Me.chkCompany.UseVisualStyleBackColor = True
        '
        'chkGamePath
        '
        Me.chkGamePath.AutoSize = True
        Me.chkGamePath.Location = New System.Drawing.Point(6, 42)
        Me.chkGamePath.Name = "chkGamePath"
        Me.chkGamePath.Size = New System.Drawing.Size(180, 17)
        Me.chkGamePath.TabIndex = 1
        Me.chkGamePath.Text = "Game Path (Not Recommended)"
        Me.chkGamePath.UseVisualStyleBackColor = True
        '
        'chkTimeStamp
        '
        Me.chkTimeStamp.AutoSize = True
        Me.chkTimeStamp.Location = New System.Drawing.Point(6, 19)
        Me.chkTimeStamp.Name = "chkTimeStamp"
        Me.chkTimeStamp.Size = New System.Drawing.Size(133, 17)
        Me.chkTimeStamp.TabIndex = 0
        Me.chkTimeStamp.Text = "Save multiple backups"
        Me.chkTimeStamp.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(132, 180)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.Location = New System.Drawing.Point(52, 180)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'frmSyncFields
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(219, 211)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpFields)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSyncFields"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Optional Sync Fields"
        Me.grpFields.ResumeLayout(False)
        Me.grpFields.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpFields As GroupBox
    Friend WithEvents chkMonitorGame As CheckBox
    Friend WithEvents chkIcon As CheckBox
    Friend WithEvents chkVersion As CheckBox
    Friend WithEvents chkCompany As CheckBox
    Friend WithEvents chkGamePath As CheckBox
    Friend WithEvents chkTimeStamp As CheckBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
End Class
