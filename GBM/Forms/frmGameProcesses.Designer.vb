<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGameProcesses
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
        Me.btnOpenProcesses = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblGameProcesses = New System.Windows.Forms.Label()
        Me.lblProcesses = New System.Windows.Forms.Label()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstGameProcesses = New System.Windows.Forms.ListBox()
        Me.lstProcesses = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'btnOpenProcesses
        '
        Me.btnOpenProcesses.Location = New System.Drawing.Point(12, 229)
        Me.btnOpenProcesses.Name = "btnOpenProcesses"
        Me.btnOpenProcesses.Size = New System.Drawing.Size(110, 23)
        Me.btnOpenProcesses.TabIndex = 4
        Me.btnOpenProcesses.Text = "&Process Manager..."
        Me.btnOpenProcesses.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(297, 229)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblGameProcesses
        '
        Me.lblGameProcesses.AutoSize = True
        Me.lblGameProcesses.Location = New System.Drawing.Point(251, 8)
        Me.lblGameProcesses.Name = "lblGameProcesses"
        Me.lblGameProcesses.Size = New System.Drawing.Size(93, 13)
        Me.lblGameProcesses.TabIndex = 0
        Me.lblGameProcesses.Text = "Current Processes"
        '
        'lblProcesses
        '
        Me.lblProcesses.AutoSize = True
        Me.lblProcesses.Location = New System.Drawing.Point(36, 8)
        Me.lblProcesses.Name = "lblProcesses"
        Me.lblProcesses.Size = New System.Drawing.Size(102, 13)
        Me.lblProcesses.TabIndex = 0
        Me.lblProcesses.Text = "Available Processes"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(168, 114)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(48, 23)
        Me.btnRemove.TabIndex = 2
        Me.btnRemove.Text = "<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(168, 85)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 23)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = ">"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstGameProcesses
        '
        Me.lstGameProcesses.FormattingEnabled = True
        Me.lstGameProcesses.Location = New System.Drawing.Point(222, 24)
        Me.lstGameProcesses.Name = "lstGameProcesses"
        Me.lstGameProcesses.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstGameProcesses.Size = New System.Drawing.Size(150, 199)
        Me.lstGameProcesses.Sorted = True
        Me.lstGameProcesses.TabIndex = 3
        '
        'lstProcesses
        '
        Me.lstProcesses.FormattingEnabled = True
        Me.lstProcesses.Location = New System.Drawing.Point(12, 24)
        Me.lstProcesses.Name = "lstProcesses"
        Me.lstProcesses.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstProcesses.Size = New System.Drawing.Size(150, 199)
        Me.lstProcesses.Sorted = True
        Me.lstProcesses.TabIndex = 0
        '
        'frmGameProcesses
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 261)
        Me.Controls.Add(Me.btnOpenProcesses)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblGameProcesses)
        Me.Controls.Add(Me.lblProcesses)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstGameProcesses)
        Me.Controls.Add(Me.lstProcesses)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGameProcesses"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Processes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOpenProcesses As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblGameProcesses As Label
    Friend WithEvents lblProcesses As Label
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents lstGameProcesses As ListBox
    Friend WithEvents lstProcesses As ListBox
End Class
