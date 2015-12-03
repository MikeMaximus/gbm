<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseGame
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
        Me.lblChoose = New System.Windows.Forms.Label()
        Me.btnChoose = New System.Windows.Forms.Button()
        Me.lstGameBox = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblChoose
        '
        Me.lblChoose.AutoSize = True
        Me.lblChoose.Location = New System.Drawing.Point(12, 9)
        Me.lblChoose.Name = "lblChoose"
        Me.lblChoose.Size = New System.Drawing.Size(209, 13)
        Me.lblChoose.TabIndex = 0
        Me.lblChoose.Text = "Please choose the game you were playing:"
        '
        'btnChoose
        '
        Me.btnChoose.Location = New System.Drawing.Point(72, 135)
        Me.btnChoose.Name = "btnChoose"
        Me.btnChoose.Size = New System.Drawing.Size(90, 23)
        Me.btnChoose.TabIndex = 2
        Me.btnChoose.Text = "C&hoose Game"
        Me.btnChoose.UseVisualStyleBackColor = True
        '
        'lstGameBox
        '
        Me.lstGameBox.FormattingEnabled = True
        Me.lstGameBox.Location = New System.Drawing.Point(15, 34)
        Me.lstGameBox.Name = "lstGameBox"
        Me.lstGameBox.Size = New System.Drawing.Size(228, 95)
        Me.lstGameBox.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(168, 135)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmChooseGame
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(255, 166)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lstGameBox)
        Me.Controls.Add(Me.btnChoose)
        Me.Controls.Add(Me.lblChoose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChooseGame"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Choose Game"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblChoose As System.Windows.Forms.Label
    Friend WithEvents btnChoose As System.Windows.Forms.Button
    Friend WithEvents lstGameBox As System.Windows.Forms.ListBox
    Friend WithEvents btnCancel As Button
End Class
