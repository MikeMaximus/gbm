<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSessionExport
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
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpExportType = New System.Windows.Forms.GroupBox()
        Me.grpDateType = New System.Windows.Forms.GroupBox()
        Me.optCSV = New System.Windows.Forms.RadioButton()
        Me.optXML = New System.Windows.Forms.RadioButton()
        Me.optCurrentLocale = New System.Windows.Forms.RadioButton()
        Me.optUnix = New System.Windows.Forms.RadioButton()
        Me.grpOptions = New System.Windows.Forms.GroupBox()
        Me.chkCSVHeaders = New System.Windows.Forms.CheckBox()
        Me.grpExportType.SuspendLayout()
        Me.grpDateType.SuspendLayout()
        Me.grpOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(116, 226)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(197, 226)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpExportType
        '
        Me.grpExportType.Controls.Add(Me.optXML)
        Me.grpExportType.Controls.Add(Me.optCSV)
        Me.grpExportType.Location = New System.Drawing.Point(12, 12)
        Me.grpExportType.Name = "grpExportType"
        Me.grpExportType.Size = New System.Drawing.Size(260, 70)
        Me.grpExportType.TabIndex = 0
        Me.grpExportType.TabStop = False
        Me.grpExportType.Text = "Export Type"
        '
        'grpDateType
        '
        Me.grpDateType.Controls.Add(Me.optUnix)
        Me.grpDateType.Controls.Add(Me.optCurrentLocale)
        Me.grpDateType.Location = New System.Drawing.Point(12, 88)
        Me.grpDateType.Name = "grpDateType"
        Me.grpDateType.Size = New System.Drawing.Size(260, 70)
        Me.grpDateType.TabIndex = 1
        Me.grpDateType.TabStop = False
        Me.grpDateType.Text = "Date Type"
        '
        'optCSV
        '
        Me.optCSV.AutoSize = True
        Me.optCSV.Location = New System.Drawing.Point(6, 19)
        Me.optCSV.Name = "optCSV"
        Me.optCSV.Size = New System.Drawing.Size(46, 17)
        Me.optCSV.TabIndex = 0
        Me.optCSV.TabStop = True
        Me.optCSV.Text = "CSV"
        Me.optCSV.UseVisualStyleBackColor = True
        '
        'optXML
        '
        Me.optXML.AutoSize = True
        Me.optXML.Location = New System.Drawing.Point(6, 42)
        Me.optXML.Name = "optXML"
        Me.optXML.Size = New System.Drawing.Size(47, 17)
        Me.optXML.TabIndex = 1
        Me.optXML.TabStop = True
        Me.optXML.Text = "XML"
        Me.optXML.UseVisualStyleBackColor = True
        '
        'optCurrentLocale
        '
        Me.optCurrentLocale.AutoEllipsis = True
        Me.optCurrentLocale.Location = New System.Drawing.Point(6, 19)
        Me.optCurrentLocale.Name = "optCurrentLocale"
        Me.optCurrentLocale.Size = New System.Drawing.Size(248, 17)
        Me.optCurrentLocale.TabIndex = 0
        Me.optCurrentLocale.TabStop = True
        Me.optCurrentLocale.Text = "Current Locale"
        Me.optCurrentLocale.UseVisualStyleBackColor = True
        '
        'optUnix
        '
        Me.optUnix.AutoEllipsis = True
        Me.optUnix.Location = New System.Drawing.Point(6, 42)
        Me.optUnix.Name = "optUnix"
        Me.optUnix.Size = New System.Drawing.Size(248, 17)
        Me.optUnix.TabIndex = 1
        Me.optUnix.TabStop = True
        Me.optUnix.Text = "Unix Timestamp"
        Me.optUnix.UseVisualStyleBackColor = True
        '
        'grpOptions
        '
        Me.grpOptions.Controls.Add(Me.chkCSVHeaders)
        Me.grpOptions.Location = New System.Drawing.Point(12, 164)
        Me.grpOptions.Name = "grpOptions"
        Me.grpOptions.Size = New System.Drawing.Size(260, 45)
        Me.grpOptions.TabIndex = 2
        Me.grpOptions.TabStop = False
        Me.grpOptions.Text = "Other Options"
        '
        'chkCSVHeaders
        '
        Me.chkCSVHeaders.AutoSize = True
        Me.chkCSVHeaders.Location = New System.Drawing.Point(6, 19)
        Me.chkCSVHeaders.Name = "chkCSVHeaders"
        Me.chkCSVHeaders.Size = New System.Drawing.Size(167, 17)
        Me.chkCSVHeaders.TabIndex = 0
        Me.chkCSVHeaders.Text = "Export Column Headers (CSV)"
        Me.chkCSVHeaders.UseVisualStyleBackColor = True
        '
        'frmSessionExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.grpOptions)
        Me.Controls.Add(Me.grpDateType)
        Me.Controls.Add(Me.grpExportType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnExport)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSessionExport"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Session Export Options"
        Me.grpExportType.ResumeLayout(False)
        Me.grpExportType.PerformLayout()
        Me.grpDateType.ResumeLayout(False)
        Me.grpOptions.ResumeLayout(False)
        Me.grpOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnExport As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents grpExportType As GroupBox
    Friend WithEvents optXML As RadioButton
    Friend WithEvents optCSV As RadioButton
    Friend WithEvents grpDateType As GroupBox
    Friend WithEvents optUnix As RadioButton
    Friend WithEvents optCurrentLocale As RadioButton
    Friend WithEvents grpOptions As GroupBox
    Friend WithEvents chkCSVHeaders As CheckBox
End Class
