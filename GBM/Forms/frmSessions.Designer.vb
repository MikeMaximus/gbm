<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSessions
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
        Me.lblQuickFilter = New System.Windows.Forms.Label()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.dgSessions = New System.Windows.Forms.DataGridView()
        CType(Me.dgSessions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblQuickFilter
        '
        Me.lblQuickFilter.AutoSize = True
        Me.lblQuickFilter.Location = New System.Drawing.Point(12, 9)
        Me.lblQuickFilter.Name = "lblQuickFilter"
        Me.lblQuickFilter.Size = New System.Drawing.Size(63, 13)
        Me.lblQuickFilter.TabIndex = 0
        Me.lblQuickFilter.Text = "Game Filter:"
        '
        'txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(80, 6)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(160, 20)
        Me.txtFilter.TabIndex = 1
        '
        'dgSessions
        '
        Me.dgSessions.AllowUserToAddRows = False
        Me.dgSessions.AllowUserToDeleteRows = False
        Me.dgSessions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSessions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
        Me.dgSessions.Location = New System.Drawing.Point(12, 32)
        Me.dgSessions.Name = "dgSessions"
        Me.dgSessions.ReadOnly = True
        Me.dgSessions.RowHeadersVisible = False
        Me.dgSessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgSessions.Size = New System.Drawing.Size(760, 517)
        Me.dgSessions.TabIndex = 2
        '
        'frmSessions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.dgSessions)
        Me.Controls.Add(Me.lblQuickFilter)
        Me.Controls.Add(Me.txtFilter)
        Me.Name = "frmSessions"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Session Viewer"
        CType(Me.dgSessions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblQuickFilter As Label
    Friend WithEvents txtFilter As TextBox
    Friend WithEvents dgSessions As DataGridView
End Class
