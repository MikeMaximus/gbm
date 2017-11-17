<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSession
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
        Me.dgSessions = New System.Windows.Forms.DataGridView()
        CType(Me.dgSessions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgSessions
        '
        Me.dgSessions.AllowUserToAddRows = False
        Me.dgSessions.AllowUserToDeleteRows = False
        Me.dgSessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSessions.Location = New System.Drawing.Point(11, 11)
        Me.dgSessions.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.dgSessions.Name = "dgSessions"
        Me.dgSessions.ReadOnly = True
        Me.dgSessions.RowHeadersVisible = False
        Me.dgSessions.RowTemplate.Height = 24
        Me.dgSessions.Size = New System.Drawing.Size(612, 389)
        Me.dgSessions.TabIndex = 0
        '
        'frmSession
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 411)
        Me.Controls.Add(Me.dgSessions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSession"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sessions"
        CType(Me.dgSessions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgSessions As DataGridView
End Class
