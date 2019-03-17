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
        Me.txtCurrentLocation = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.bwSearch = New System.ComponentModel.BackgroundWorker()
        Me.lstResults = New System.Windows.Forms.ListBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblResults = New System.Windows.Forms.Label()
        Me.cboDrive = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'txtCurrentLocation
        '
        Me.txtCurrentLocation.Location = New System.Drawing.Point(102, 13)
        Me.txtCurrentLocation.Name = "txtCurrentLocation"
        Me.txtCurrentLocation.ReadOnly = True
        Me.txtCurrentLocation.Size = New System.Drawing.Size(370, 20)
        Me.txtCurrentLocation.TabIndex = 0
        Me.txtCurrentLocation.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(397, 146)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'bwSearch
        '
        Me.bwSearch.WorkerSupportsCancellation = True
        '
        'lstResults
        '
        Me.lstResults.FormattingEnabled = True
        Me.lstResults.Location = New System.Drawing.Point(12, 58)
        Me.lstResults.Name = "lstResults"
        Me.lstResults.Size = New System.Drawing.Size(460, 82)
        Me.lstResults.TabIndex = 2
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(316, 146)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 3
        Me.btnOk.Text = "&OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lblResults
        '
        Me.lblResults.Location = New System.Drawing.Point(9, 42)
        Me.lblResults.Name = "lblResults"
        Me.lblResults.Size = New System.Drawing.Size(463, 13)
        Me.lblResults.TabIndex = 1
        Me.lblResults.Text = "Search Results"
        Me.lblResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cboDrive
        '
        Me.cboDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDrive.FormattingEnabled = True
        Me.cboDrive.Location = New System.Drawing.Point(12, 13)
        Me.cboDrive.Name = "cboDrive"
        Me.cboDrive.Size = New System.Drawing.Size(85, 21)
        Me.cboDrive.TabIndex = 5
        '
        'frmFileFolderSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 181)
        Me.Controls.Add(Me.cboDrive)
        Me.Controls.Add(Me.lblResults)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lstResults)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtCurrentLocation)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFileFolderSearch"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCurrentLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents bwSearch As System.ComponentModel.BackgroundWorker
    Friend WithEvents lstResults As System.Windows.Forms.ListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents cboDrive As System.Windows.Forms.ComboBox
End Class
