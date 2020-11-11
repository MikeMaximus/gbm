<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLaunchConfiguration
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
        Me.grpOtherConfig = New System.Windows.Forms.GroupBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtArguments = New System.Windows.Forms.TextBox()
        Me.txtExePath = New System.Windows.Forms.TextBox()
        Me.lblArguments = New System.Windows.Forms.Label()
        Me.lblExe = New System.Windows.Forms.Label()
        Me.grpStoreLauncher = New System.Windows.Forms.GroupBox()
        Me.btnOpenLaunchers = New System.Windows.Forms.Button()
        Me.cboLauncher = New System.Windows.Forms.ComboBox()
        Me.txtGameID = New System.Windows.Forms.TextBox()
        Me.lblGameID = New System.Windows.Forms.Label()
        Me.lblLauncher = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.grpOtherConfig.SuspendLayout()
        Me.grpStoreLauncher.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOtherConfig
        '
        Me.grpOtherConfig.Controls.Add(Me.btnBrowse)
        Me.grpOtherConfig.Controls.Add(Me.txtArguments)
        Me.grpOtherConfig.Controls.Add(Me.txtExePath)
        Me.grpOtherConfig.Controls.Add(Me.lblArguments)
        Me.grpOtherConfig.Controls.Add(Me.lblExe)
        Me.grpOtherConfig.Location = New System.Drawing.Point(12, 99)
        Me.grpOtherConfig.Name = "grpOtherConfig"
        Me.grpOtherConfig.Size = New System.Drawing.Size(460, 82)
        Me.grpOtherConfig.TabIndex = 1
        Me.grpOtherConfig.TabStop = False
        Me.grpOtherConfig.Text = "Other Configuration"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(413, 23)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(30, 20)
        Me.btnBrowse.TabIndex = 1
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtArguments
        '
        Me.txtArguments.Location = New System.Drawing.Point(85, 49)
        Me.txtArguments.Name = "txtArguments"
        Me.txtArguments.Size = New System.Drawing.Size(358, 20)
        Me.txtArguments.TabIndex = 2
        '
        'txtExePath
        '
        Me.txtExePath.Location = New System.Drawing.Point(85, 23)
        Me.txtExePath.Name = "txtExePath"
        Me.txtExePath.Size = New System.Drawing.Size(322, 20)
        Me.txtExePath.TabIndex = 0
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(6, 52)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(60, 13)
        Me.lblArguments.TabIndex = 0
        Me.lblArguments.Text = "Arguments:"
        '
        'lblExe
        '
        Me.lblExe.AutoSize = True
        Me.lblExe.Location = New System.Drawing.Point(6, 26)
        Me.lblExe.Name = "lblExe"
        Me.lblExe.Size = New System.Drawing.Size(72, 13)
        Me.lblExe.TabIndex = 0
        Me.lblExe.Text = "Alternate exe:"
        '
        'grpStoreLauncher
        '
        Me.grpStoreLauncher.Controls.Add(Me.btnOpenLaunchers)
        Me.grpStoreLauncher.Controls.Add(Me.cboLauncher)
        Me.grpStoreLauncher.Controls.Add(Me.txtGameID)
        Me.grpStoreLauncher.Controls.Add(Me.lblGameID)
        Me.grpStoreLauncher.Controls.Add(Me.lblLauncher)
        Me.grpStoreLauncher.Location = New System.Drawing.Point(12, 12)
        Me.grpStoreLauncher.Name = "grpStoreLauncher"
        Me.grpStoreLauncher.Size = New System.Drawing.Size(460, 81)
        Me.grpStoreLauncher.TabIndex = 0
        Me.grpStoreLauncher.TabStop = False
        Me.grpStoreLauncher.Text = "Store Launcher Configuration"
        '
        'btnOpenLaunchers
        '
        Me.btnOpenLaunchers.Location = New System.Drawing.Point(314, 21)
        Me.btnOpenLaunchers.Name = "btnOpenLaunchers"
        Me.btnOpenLaunchers.Size = New System.Drawing.Size(129, 23)
        Me.btnOpenLaunchers.TabIndex = 1
        Me.btnOpenLaunchers.Text = "&Launcher Manager..."
        Me.btnOpenLaunchers.UseVisualStyleBackColor = True
        '
        'cboLauncher
        '
        Me.cboLauncher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLauncher.FormattingEnabled = True
        Me.cboLauncher.Location = New System.Drawing.Point(85, 23)
        Me.cboLauncher.Name = "cboLauncher"
        Me.cboLauncher.Size = New System.Drawing.Size(223, 21)
        Me.cboLauncher.TabIndex = 0
        '
        'txtGameID
        '
        Me.txtGameID.Location = New System.Drawing.Point(85, 49)
        Me.txtGameID.Name = "txtGameID"
        Me.txtGameID.Size = New System.Drawing.Size(358, 20)
        Me.txtGameID.TabIndex = 2
        '
        'lblGameID
        '
        Me.lblGameID.AutoSize = True
        Me.lblGameID.Location = New System.Drawing.Point(6, 52)
        Me.lblGameID.Name = "lblGameID"
        Me.lblGameID.Size = New System.Drawing.Size(52, 13)
        Me.lblGameID.TabIndex = 0
        Me.lblGameID.Text = "Game ID:"
        '
        'lblLauncher
        '
        Me.lblLauncher.AutoSize = True
        Me.lblLauncher.Location = New System.Drawing.Point(6, 26)
        Me.lblLauncher.Name = "lblLauncher"
        Me.lblLauncher.Size = New System.Drawing.Size(55, 13)
        Me.lblLauncher.TabIndex = 0
        Me.lblLauncher.Text = "Launcher:"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(397, 187)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(316, 187)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'frmLaunchConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(488, 223)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpStoreLauncher)
        Me.Controls.Add(Me.grpOtherConfig)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLaunchConfiguration"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Launch Configuration"
        Me.grpOtherConfig.ResumeLayout(False)
        Me.grpOtherConfig.PerformLayout()
        Me.grpStoreLauncher.ResumeLayout(False)
        Me.grpStoreLauncher.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpOtherConfig As GroupBox
    Friend WithEvents txtArguments As TextBox
    Friend WithEvents txtExePath As TextBox
    Friend WithEvents lblArguments As Label
    Friend WithEvents lblExe As Label
    Friend WithEvents grpStoreLauncher As GroupBox
    Friend WithEvents cboLauncher As ComboBox
    Friend WithEvents txtGameID As TextBox
    Friend WithEvents lblGameID As Label
    Friend WithEvents lblLauncher As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnOpenLaunchers As Button
    Friend WithEvents btnBrowse As Button
End Class
