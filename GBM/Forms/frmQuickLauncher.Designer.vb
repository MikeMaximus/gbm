<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQuickLauncher
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
        Me.cboGames = New System.Windows.Forms.ComboBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cboGames
        '
        Me.cboGames.FormattingEnabled = True
        Me.cboGames.Location = New System.Drawing.Point(12, 13)
        Me.cboGames.Name = "cboGames"
        Me.cboGames.Size = New System.Drawing.Size(343, 21)
        Me.cboGames.TabIndex = 0
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(362, 11)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(35, 23)
        Me.btnGo.TabIndex = 1
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'frmQuickLauncher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 46)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.cboGames)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQuickLauncher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Quick Launcher"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cboGames As ComboBox
    Friend WithEvents btnGo As Button
End Class
