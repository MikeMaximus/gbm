<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilter
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblGameTags = New System.Windows.Forms.Label()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstFilter = New System.Windows.Forms.ListBox()
        Me.lstTags = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(297, 229)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 13
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblGameTags
        '
        Me.lblGameTags.AutoSize = True
        Me.lblGameTags.Location = New System.Drawing.Point(263, 8)
        Me.lblGameTags.Name = "lblGameTags"
        Me.lblGameTags.Size = New System.Drawing.Size(66, 13)
        Me.lblGameTags.TabIndex = 6
        Me.lblGameTags.Text = "Current Filter"
        '
        'lblTags
        '
        Me.lblTags.AutoSize = True
        Me.lblTags.Location = New System.Drawing.Point(49, 8)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(77, 13)
        Me.lblTags.TabIndex = 7
        Me.lblTags.Text = "Available Tags"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(168, 114)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 10
        Me.btnRemove.Text = "< <"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(168, 85)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 9
        Me.btnAdd.Text = "> >"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstFilter
        '
        Me.lstFilter.FormattingEnabled = True
        Me.lstFilter.Location = New System.Drawing.Point(222, 24)
        Me.lstFilter.Name = "lstFilter"
        Me.lstFilter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFilter.Size = New System.Drawing.Size(150, 199)
        Me.lstFilter.TabIndex = 11
        '
        'lstTags
        '
        Me.lstTags.FormattingEnabled = True
        Me.lstTags.Location = New System.Drawing.Point(12, 24)
        Me.lstTags.Name = "lstTags"
        Me.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTags.Size = New System.Drawing.Size(150, 199)
        Me.lstTags.TabIndex = 8
        '
        'frmFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 261)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblGameTags)
        Me.Controls.Add(Me.lblTags)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstFilter)
        Me.Controls.Add(Me.lstTags)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFilter"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filter by Tags"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblGameTags As System.Windows.Forms.Label
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lstFilter As System.Windows.Forms.ListBox
    Friend WithEvents lstTags As System.Windows.Forms.ListBox
End Class
