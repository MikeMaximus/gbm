<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGameTags
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
        Me.lstTags = New System.Windows.Forms.ListBox()
        Me.lstGameTags = New System.Windows.Forms.ListBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.lblGameTags = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOpenTags = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstTags
        '
        Me.lstTags.FormattingEnabled = True
        Me.lstTags.Location = New System.Drawing.Point(12, 25)
        Me.lstTags.Name = "lstTags"
        Me.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTags.Size = New System.Drawing.Size(150, 199)
        Me.lstTags.TabIndex = 0
        '
        'lstGameTags
        '
        Me.lstGameTags.FormattingEnabled = True
        Me.lstGameTags.Location = New System.Drawing.Point(222, 25)
        Me.lstGameTags.Name = "lstGameTags"
        Me.lstGameTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstGameTags.Size = New System.Drawing.Size(150, 199)
        Me.lstGameTags.TabIndex = 3
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(168, 86)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "> >"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(168, 115)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 2
        Me.btnRemove.Text = "< <"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'lblTags
        '
        Me.lblTags.AutoSize = True
        Me.lblTags.Location = New System.Drawing.Point(49, 9)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(77, 13)
        Me.lblTags.TabIndex = 0
        Me.lblTags.Text = "Available Tags"
        '
        'lblGameTags
        '
        Me.lblGameTags.AutoSize = True
        Me.lblGameTags.Location = New System.Drawing.Point(263, 9)
        Me.lblGameTags.Name = "lblGameTags"
        Me.lblGameTags.Size = New System.Drawing.Size(68, 13)
        Me.lblGameTags.TabIndex = 0
        Me.lblGameTags.Text = "Current Tags"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(297, 230)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnOpenTags
        '
        Me.btnOpenTags.Location = New System.Drawing.Point(12, 230)
        Me.btnOpenTags.Name = "btnOpenTags"
        Me.btnOpenTags.Size = New System.Drawing.Size(90, 23)
        Me.btnOpenTags.TabIndex = 4
        Me.btnOpenTags.Text = "Setup &Tags..."
        Me.btnOpenTags.UseVisualStyleBackColor = True
        '
        'frmGameTags
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 261)
        Me.Controls.Add(Me.btnOpenTags)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblGameTags)
        Me.Controls.Add(Me.lblTags)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstGameTags)
        Me.Controls.Add(Me.lstTags)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGameTags"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Tags"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstTags As System.Windows.Forms.ListBox
    Friend WithEvents lstGameTags As System.Windows.Forms.ListBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents lblGameTags As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnOpenTags As System.Windows.Forms.Button
End Class
