<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvancedImport
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
        Me.lstGames = New System.Windows.Forms.CheckedListBox()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblGames = New System.Windows.Forms.Label()
        Me.lblSelected = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstGames
        '
        Me.lstGames.CheckOnClick = True
        Me.lstGames.Location = New System.Drawing.Point(12, 30)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.Size = New System.Drawing.Size(335, 334)
        Me.lstGames.TabIndex = 1
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(191, 370)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 2
        Me.btnImport.Text = "&Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(15, 12)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(15, 14)
        Me.chkSelectAll.TabIndex = 0
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'lblGames
        '
        Me.lblGames.Location = New System.Drawing.Point(12, 12)
        Me.lblGames.Name = "lblGames"
        Me.lblGames.Size = New System.Drawing.Size(335, 14)
        Me.lblGames.TabIndex = 0
        Me.lblGames.Text = "Games"
        Me.lblGames.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSelected
        '
        Me.lblSelected.AutoSize = True
        Me.lblSelected.Location = New System.Drawing.Point(9, 375)
        Me.lblSelected.Name = "lblSelected"
        Me.lblSelected.Size = New System.Drawing.Size(77, 13)
        Me.lblSelected.TabIndex = 0
        Me.lblSelected.Text = "Selected Items"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(272, 370)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmAdvancedImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(359, 402)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelected)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.lstGames)
        Me.Controls.Add(Me.lblGames)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAdvancedImport"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Game Configurations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstGames As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblGames As System.Windows.Forms.Label
    Friend WithEvents lblSelected As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
