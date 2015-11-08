<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFileFolderSearch
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
        Me.pgbProgress = New System.Windows.Forms.ProgressBar()
        Me.txtCurrentLocation = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.bwSearch = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'pgbProgress
        '
        Me.pgbProgress.Location = New System.Drawing.Point(12, 12)
        Me.pgbProgress.MarqueeAnimationSpeed = 0
        Me.pgbProgress.Name = "pgbProgress"
        Me.pgbProgress.Size = New System.Drawing.Size(460, 23)
        Me.pgbProgress.TabIndex = 0
        '
        'txtCurrentLocation
        '
        Me.txtCurrentLocation.Location = New System.Drawing.Point(12, 43)
        Me.txtCurrentLocation.Name = "txtCurrentLocation"
        Me.txtCurrentLocation.ReadOnly = True
        Me.txtCurrentLocation.Size = New System.Drawing.Size(379, 20)
        Me.txtCurrentLocation.TabIndex = 0
        Me.txtCurrentLocation.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(397, 41)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'bwSearch
        '
        Me.bwSearch.WorkerSupportsCancellation = True
        '
        'frmFileFolderSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 77)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtCurrentLocation)
        Me.Controls.Add(Me.pgbProgress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFileFolderSearch"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pgbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents txtCurrentLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents bwSearch As System.ComponentModel.BackgroundWorker
End Class
