<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWineConfiguration
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
        Me.grpWineConfig = New System.Windows.Forms.GroupBox()
        Me.txtWineSavePath = New System.Windows.Forms.TextBox()
        Me.txtWinePrefix = New System.Windows.Forms.TextBox()
        Me.txtWineBinaryPath = New System.Windows.Forms.TextBox()
        Me.lblWinePrefix = New System.Windows.Forms.Label()
        Me.lblWineSavePath = New System.Windows.Forms.Label()
        Me.lblWineBinaryPath = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpWineConfig.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpWineConfig
        '
        Me.grpWineConfig.Controls.Add(Me.txtWineSavePath)
        Me.grpWineConfig.Controls.Add(Me.txtWinePrefix)
        Me.grpWineConfig.Controls.Add(Me.txtWineBinaryPath)
        Me.grpWineConfig.Controls.Add(Me.lblWinePrefix)
        Me.grpWineConfig.Controls.Add(Me.lblWineSavePath)
        Me.grpWineConfig.Controls.Add(Me.lblWineBinaryPath)
        Me.grpWineConfig.Location = New System.Drawing.Point(12, 12)
        Me.grpWineConfig.Name = "grpWineConfig"
        Me.grpWineConfig.Size = New System.Drawing.Size(460, 111)
        Me.grpWineConfig.TabIndex = 0
        Me.grpWineConfig.TabStop = False
        Me.grpWineConfig.Text = "Configuration"
        '
        'txtWineSavePath
        '
        Me.txtWineSavePath.Location = New System.Drawing.Point(76, 75)
        Me.txtWineSavePath.Name = "txtWineSavePath"
        Me.txtWineSavePath.Size = New System.Drawing.Size(367, 20)
        Me.txtWineSavePath.TabIndex = 5
        '
        'txtWinePrefix
        '
        Me.txtWinePrefix.Location = New System.Drawing.Point(76, 49)
        Me.txtWinePrefix.Name = "txtWinePrefix"
        Me.txtWinePrefix.Size = New System.Drawing.Size(367, 20)
        Me.txtWinePrefix.TabIndex = 3
        '
        'txtWineBinaryPath
        '
        Me.txtWineBinaryPath.Location = New System.Drawing.Point(76, 23)
        Me.txtWineBinaryPath.Name = "txtWineBinaryPath"
        Me.txtWineBinaryPath.Size = New System.Drawing.Size(367, 20)
        Me.txtWineBinaryPath.TabIndex = 1
        '
        'lblWinePrefix
        '
        Me.lblWinePrefix.AutoSize = True
        Me.lblWinePrefix.Location = New System.Drawing.Point(6, 52)
        Me.lblWinePrefix.Name = "lblWinePrefix"
        Me.lblWinePrefix.Size = New System.Drawing.Size(36, 13)
        Me.lblWinePrefix.TabIndex = 2
        Me.lblWinePrefix.Text = "Prefix:"
        '
        'lblWineSavePath
        '
        Me.lblWineSavePath.AutoSize = True
        Me.lblWineSavePath.Location = New System.Drawing.Point(6, 78)
        Me.lblWineSavePath.Name = "lblWineSavePath"
        Me.lblWineSavePath.Size = New System.Drawing.Size(60, 13)
        Me.lblWineSavePath.TabIndex = 4
        Me.lblWineSavePath.Text = "Save Path:"
        '
        'lblWineBinaryPath
        '
        Me.lblWineBinaryPath.AutoSize = True
        Me.lblWineBinaryPath.Location = New System.Drawing.Point(6, 26)
        Me.lblWineBinaryPath.Name = "lblWineBinaryPath"
        Me.lblWineBinaryPath.Size = New System.Drawing.Size(64, 13)
        Me.lblWineBinaryPath.TabIndex = 0
        Me.lblWineBinaryPath.Text = "Binary Path:"
        '
        'btnSave
        '
        Me.btnSave.Image = Global.GBM.My.Resources.Resources.Multi_Save
        Me.btnSave.Location = New System.Drawing.Point(346, 129)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(60, 45)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(412, 129)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmWineConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 186)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpWineConfig)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWineConfiguration"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Wine Configuration"
        Me.grpWineConfig.ResumeLayout(False)
        Me.grpWineConfig.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpWineConfig As GroupBox
    Friend WithEvents txtWineSavePath As TextBox
    Friend WithEvents txtWinePrefix As TextBox
    Friend WithEvents txtWineBinaryPath As TextBox
    Friend WithEvents lblWinePrefix As Label
    Friend WithEvents lblWineSavePath As Label
    Friend WithEvents lblWineBinaryPath As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
End Class
