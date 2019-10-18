<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfigLinks
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblLinkedConfigs = New System.Windows.Forms.Label()
        Me.lblConfigs = New System.Windows.Forms.Label()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstLinks = New System.Windows.Forms.ListBox()
        Me.lstConfigs = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(397, 331)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblLinkedConfigs
        '
        Me.lblLinkedConfigs.AutoSize = True
        Me.lblLinkedConfigs.Location = New System.Drawing.Point(318, 6)
        Me.lblLinkedConfigs.Name = "lblLinkedConfigs"
        Me.lblLinkedConfigs.Size = New System.Drawing.Size(109, 13)
        Me.lblLinkedConfigs.TabIndex = 4
        Me.lblLinkedConfigs.Text = "Linked Configurations"
        '
        'lblConfigs
        '
        Me.lblConfigs.AutoSize = True
        Me.lblConfigs.Location = New System.Drawing.Point(52, 6)
        Me.lblConfigs.Name = "lblConfigs"
        Me.lblConfigs.Size = New System.Drawing.Size(120, 13)
        Me.lblConfigs.TabIndex = 0
        Me.lblConfigs.Text = "Available Configurations"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(218, 164)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(218, 135)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = ">"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstLinks
        '
        Me.lstLinks.FormattingEnabled = True
        Me.lstLinks.Location = New System.Drawing.Point(272, 22)
        Me.lstLinks.Name = "lstLinks"
        Me.lstLinks.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstLinks.Size = New System.Drawing.Size(200, 303)
        Me.lstLinks.Sorted = True
        Me.lstLinks.TabIndex = 5
        '
        'lstConfigs
        '
        Me.lstConfigs.FormattingEnabled = True
        Me.lstConfigs.Location = New System.Drawing.Point(12, 22)
        Me.lstConfigs.Name = "lstConfigs"
        Me.lstConfigs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstConfigs.Size = New System.Drawing.Size(200, 303)
        Me.lstConfigs.Sorted = True
        Me.lstConfigs.TabIndex = 1
        '
        'frmConfigLinks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 361)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblLinkedConfigs)
        Me.Controls.Add(Me.lblConfigs)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstLinks)
        Me.Controls.Add(Me.lstConfigs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmConfigLinks"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Configuration Links"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents lblLinkedConfigs As Label
    Friend WithEvents lblConfigs As Label
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents lstLinks As ListBox
    Friend WithEvents lstConfigs As ListBox
End Class
