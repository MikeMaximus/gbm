<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManifestViewer
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
        Me.dgView = New System.Windows.Forms.DataGridView()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblError = New System.Windows.Forms.Label()
        Me.fbBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.cboManifest = New System.Windows.Forms.ComboBox()
        CType(Me.dgView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgView
        '
        Me.dgView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgView.BackgroundColor = System.Drawing.SystemColors.Window
        Me.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgView.Location = New System.Drawing.Point(12, 12)
        Me.dgView.Name = "dgView"
        Me.dgView.RowHeadersVisible = False
        Me.dgView.Size = New System.Drawing.Size(960, 338)
        Me.dgView.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(897, 356)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "C&lose"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblError
        '
        Me.lblError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblError.AutoSize = True
        Me.lblError.Location = New System.Drawing.Point(139, 362)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(54, 13)
        Me.lblError.TabIndex = 5
        Me.lblError.Text = "Info Label"
        '
        'fbBrowser
        '
        Me.fbBrowser.ShowNewFolderButton = False
        '
        'cboManifest
        '
        Me.cboManifest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboManifest.FormattingEnabled = True
        Me.cboManifest.Location = New System.Drawing.Point(12, 359)
        Me.cboManifest.Name = "cboManifest"
        Me.cboManifest.Size = New System.Drawing.Size(121, 21)
        Me.cboManifest.TabIndex = 6
        '
        'frmManifestViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 392)
        Me.Controls.Add(Me.cboManifest)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.dgView)
        Me.MinimizeBox = False
        Me.Name = "frmManifestViewer"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manifest Viewer"
        CType(Me.dgView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgView As System.Windows.Forms.DataGridView
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents fbBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents cboManifest As System.Windows.Forms.ComboBox
End Class
