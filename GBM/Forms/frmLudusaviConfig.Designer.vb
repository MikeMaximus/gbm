<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.grpProfileTypes.SuspendLayout()
        Me.grpOperatingSystems.SuspendLayout()
        Me.grpSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpProfileTypes
        '
        Me.grpProfileTypes.Controls.Add(Me.chkConfigurationFiles)
        Me.grpProfileTypes.Controls.Add(Me.chkSavedGames)
        Me.grpProfileTypes.Location = New System.Drawing.Point(12, 67)
        Me.grpProfileTypes.Name = "grpProfileTypes"
        Me.grpProfileTypes.Size = New System.Drawing.Size(260, 69)
        Me.grpProfileTypes.TabIndex = 1
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
        Me.grpOperatingSystems.Location = New System.Drawing.Point(12, 142)
        Me.grpOperatingSystems.Name = "grpOperatingSystems"
        Me.grpOperatingSystems.Size = New System.Drawing.Size(260, 71)
        Me.grpOperatingSystems.TabIndex = 2
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
        Me.btnCancel.Location = New System.Drawing.Point(212, 219)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Image = Global.GBM.My.Resources.Resources.Multi_Ok
        Me.btnOK.Location = New System.Drawing.Point(146, 219)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(60, 45)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "&OK"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.txtQuery)
        Me.grpSearch.Location = New System.Drawing.Point(12, 11)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(260, 50)
        Me.grpSearch.TabIndex = 0
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "Quick Search"
        '
        'txtQuery
        '
        Me.txtQuery.Location = New System.Drawing.Point(6, 19)
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(248, 20)
        Me.txtQuery.TabIndex = 0
        '
        'frmLudusaviConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 276)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpOperatingSystems)
        Me.Controls.Add(Me.grpProfileTypes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLudusaviConfig"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ludusavi Options"
        Me.grpProfileTypes.ResumeLayout(False)
        Me.grpProfileTypes.PerformLayout()
        Me.grpOperatingSystems.ResumeLayout(False)
        Me.grpOperatingSystems.PerformLayout()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpProfileTypes As GroupBox
    Friend WithEvents chkConfigurationFiles As CheckBox
    Friend WithEvents chkSavedGames As CheckBox
    Friend WithEvents grpOperatingSystems As GroupBox
    Friend WithEvents chkLinux As CheckBox
    Friend WithEvents chkWindows As CheckBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents grpSearch As GroupBox
    Friend WithEvents txtQuery As TextBox
End Class
