﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLudusaviConfig
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
        Me.grpProfileTypes = New System.Windows.Forms.GroupBox()
        Me.chkConfigurationFiles = New System.Windows.Forms.CheckBox()
        Me.chkSavedGames = New System.Windows.Forms.CheckBox()
        Me.grpOperatingSystems = New System.Windows.Forms.GroupBox()
        Me.chkLinux = New System.Windows.Forms.CheckBox()
        Me.chkWindows = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.grpProfileTypes.SuspendLayout()
        Me.grpOperatingSystems.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpProfileTypes
        '
        Me.grpProfileTypes.Controls.Add(Me.chkConfigurationFiles)
        Me.grpProfileTypes.Controls.Add(Me.chkSavedGames)
        Me.grpProfileTypes.Location = New System.Drawing.Point(12, 12)
        Me.grpProfileTypes.Name = "grpProfileTypes"
        Me.grpProfileTypes.Size = New System.Drawing.Size(200, 69)
        Me.grpProfileTypes.TabIndex = 0
        Me.grpProfileTypes.TabStop = False
        Me.grpProfileTypes.Text = "Profile Types"
        '
        'chkConfigurationFiles
        '
        Me.chkConfigurationFiles.AutoSize = True
        Me.chkConfigurationFiles.Location = New System.Drawing.Point(6, 42)
        Me.chkConfigurationFiles.Name = "chkConfigurationFiles"
        Me.chkConfigurationFiles.Size = New System.Drawing.Size(112, 17)
        Me.chkConfigurationFiles.TabIndex = 1
        Me.chkConfigurationFiles.Text = "Configuration Files"
        Me.chkConfigurationFiles.UseVisualStyleBackColor = True
        '
        'chkSavedGames
        '
        Me.chkSavedGames.AutoSize = True
        Me.chkSavedGames.Location = New System.Drawing.Point(6, 19)
        Me.chkSavedGames.Name = "chkSavedGames"
        Me.chkSavedGames.Size = New System.Drawing.Size(93, 17)
        Me.chkSavedGames.TabIndex = 0
        Me.chkSavedGames.Text = "Saved Games"
        Me.chkSavedGames.UseVisualStyleBackColor = True
        '
        'grpOperatingSystems
        '
        Me.grpOperatingSystems.Controls.Add(Me.chkLinux)
        Me.grpOperatingSystems.Controls.Add(Me.chkWindows)
        Me.grpOperatingSystems.Location = New System.Drawing.Point(12, 87)
        Me.grpOperatingSystems.Name = "grpOperatingSystems"
        Me.grpOperatingSystems.Size = New System.Drawing.Size(200, 71)
        Me.grpOperatingSystems.TabIndex = 1
        Me.grpOperatingSystems.TabStop = False
        Me.grpOperatingSystems.Text = "Operating Systems"
        '
        'chkLinux
        '
        Me.chkLinux.AutoSize = True
        Me.chkLinux.Location = New System.Drawing.Point(6, 42)
        Me.chkLinux.Name = "chkLinux"
        Me.chkLinux.Size = New System.Drawing.Size(51, 17)
        Me.chkLinux.TabIndex = 1
        Me.chkLinux.Text = "Linux"
        Me.chkLinux.UseVisualStyleBackColor = True
        '
        'chkWindows
        '
        Me.chkWindows.AutoSize = True
        Me.chkWindows.Location = New System.Drawing.Point(6, 19)
        Me.chkWindows.Name = "chkWindows"
        Me.chkWindows.Size = New System.Drawing.Size(70, 17)
        Me.chkWindows.TabIndex = 0
        Me.chkWindows.Text = "Windows"
        Me.chkWindows.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(152, 164)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Image = Global.GBM.My.Resources.Resources.Multi_Import
        Me.btnImport.Location = New System.Drawing.Point(91, 164)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(55, 45)
        Me.btnImport.TabIndex = 2
        Me.btnImport.Text = "&Import"
        Me.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'frmLudusaviConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(224, 221)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpOperatingSystems)
        Me.Controls.Add(Me.grpProfileTypes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLudusaviConfig"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ludusavi Options"
        Me.grpProfileTypes.ResumeLayout(False)
        Me.grpProfileTypes.PerformLayout()
        Me.grpOperatingSystems.ResumeLayout(False)
        Me.grpOperatingSystems.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpProfileTypes As GroupBox
    Friend WithEvents chkConfigurationFiles As CheckBox
    Friend WithEvents chkSavedGames As CheckBox
    Friend WithEvents grpOperatingSystems As GroupBox
    Friend WithEvents chkLinux As CheckBox
    Friend WithEvents chkWindows As CheckBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnImport As Button
End Class