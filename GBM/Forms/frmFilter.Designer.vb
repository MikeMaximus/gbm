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
        Me.optGameInfo = New System.Windows.Forms.RadioButton()
        Me.optTag = New System.Windows.Forms.RadioButton()
        Me.grpTagFilter = New System.Windows.Forms.GroupBox()
        Me.grpTagOptions = New System.Windows.Forms.GroupBox()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.optAny = New System.Windows.Forms.RadioButton()
        Me.lblGameTags = New System.Windows.Forms.Label()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstFilter = New System.Windows.Forms.ListBox()
        Me.lstTags = New System.Windows.Forms.ListBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpGameFilter = New System.Windows.Forms.GroupBox()
        Me.grpGameInfoOptions = New System.Windows.Forms.GroupBox()
        Me.optOr = New System.Windows.Forms.RadioButton()
        Me.optAnd = New System.Windows.Forms.RadioButton()
        Me.txtCompany = New System.Windows.Forms.TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.txtProcess = New System.Windows.Forms.TextBox()
        Me.lblProcess = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.grpTagFilter.SuspendLayout()
        Me.grpTagOptions.SuspendLayout()
        Me.grpGameFilter.SuspendLayout()
        Me.grpGameInfoOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'optGameInfo
        '
        Me.optGameInfo.AutoSize = True
        Me.optGameInfo.Location = New System.Drawing.Point(12, 12)
        Me.optGameInfo.Name = "optGameInfo"
        Me.optGameInfo.Size = New System.Drawing.Size(108, 17)
        Me.optGameInfo.TabIndex = 0
        Me.optGameInfo.Text = "Game Information"
        Me.optGameInfo.UseVisualStyleBackColor = True
        '
        'optTag
        '
        Me.optTag.AutoSize = True
        Me.optTag.Location = New System.Drawing.Point(12, 190)
        Me.optTag.Name = "optTag"
        Me.optTag.Size = New System.Drawing.Size(44, 17)
        Me.optTag.TabIndex = 2
        Me.optTag.Text = "Tag"
        Me.optTag.UseVisualStyleBackColor = True
        '
        'grpTagFilter
        '
        Me.grpTagFilter.Controls.Add(Me.grpTagOptions)
        Me.grpTagFilter.Controls.Add(Me.lblGameTags)
        Me.grpTagFilter.Controls.Add(Me.lblTags)
        Me.grpTagFilter.Controls.Add(Me.btnRemove)
        Me.grpTagFilter.Controls.Add(Me.btnAdd)
        Me.grpTagFilter.Controls.Add(Me.lstFilter)
        Me.grpTagFilter.Controls.Add(Me.lstTags)
        Me.grpTagFilter.Location = New System.Drawing.Point(12, 213)
        Me.grpTagFilter.Name = "grpTagFilter"
        Me.grpTagFilter.Size = New System.Drawing.Size(385, 265)
        Me.grpTagFilter.TabIndex = 3
        Me.grpTagFilter.TabStop = False
        '
        'grpTagOptions
        '
        Me.grpTagOptions.Controls.Add(Me.optAll)
        Me.grpTagOptions.Controls.Add(Me.optAny)
        Me.grpTagOptions.Location = New System.Drawing.Point(6, 211)
        Me.grpTagOptions.Name = "grpTagOptions"
        Me.grpTagOptions.Size = New System.Drawing.Size(150, 46)
        Me.grpTagOptions.TabIndex = 6
        Me.grpTagOptions.TabStop = False
        Me.grpTagOptions.Text = "Options"
        '
        'optAll
        '
        Me.optAll.AutoSize = True
        Me.optAll.Location = New System.Drawing.Point(77, 19)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New System.Drawing.Size(63, 17)
        Me.optAll.TabIndex = 1
        Me.optAll.TabStop = True
        Me.optAll.Text = "All Tags"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'optAny
        '
        Me.optAny.AutoSize = True
        Me.optAny.Checked = True
        Me.optAny.Location = New System.Drawing.Point(6, 19)
        Me.optAny.Name = "optAny"
        Me.optAny.Size = New System.Drawing.Size(65, 17)
        Me.optAny.TabIndex = 0
        Me.optAny.TabStop = True
        Me.optAny.Text = "Any Tag"
        Me.optAny.UseVisualStyleBackColor = True
        '
        'lblGameTags
        '
        Me.lblGameTags.AutoSize = True
        Me.lblGameTags.Location = New System.Drawing.Point(271, 16)
        Me.lblGameTags.Name = "lblGameTags"
        Me.lblGameTags.Size = New System.Drawing.Size(66, 13)
        Me.lblGameTags.TabIndex = 4
        Me.lblGameTags.Text = "Current Filter"
        '
        'lblTags
        '
        Me.lblTags.AutoSize = True
        Me.lblTags.Location = New System.Drawing.Point(43, 16)
        Me.lblTags.Name = "lblTags"
        Me.lblTags.Size = New System.Drawing.Size(77, 13)
        Me.lblTags.TabIndex = 0
        Me.lblTags.Text = "Available Tags"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(162, 122)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(61, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(162, 93)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(61, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = ">"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstFilter
        '
        Me.lstFilter.FormattingEnabled = True
        Me.lstFilter.Location = New System.Drawing.Point(229, 32)
        Me.lstFilter.Name = "lstFilter"
        Me.lstFilter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFilter.Size = New System.Drawing.Size(150, 173)
        Me.lstFilter.Sorted = True
        Me.lstFilter.TabIndex = 5
        '
        'lstTags
        '
        Me.lstTags.FormattingEnabled = True
        Me.lstTags.Location = New System.Drawing.Point(6, 32)
        Me.lstTags.Name = "lstTags"
        Me.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTags.Size = New System.Drawing.Size(150, 173)
        Me.lstTags.Sorted = True
        Me.lstTags.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(322, 484)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grpGameFilter
        '
        Me.grpGameFilter.Controls.Add(Me.grpGameInfoOptions)
        Me.grpGameFilter.Controls.Add(Me.txtCompany)
        Me.grpGameFilter.Controls.Add(Me.lblCompany)
        Me.grpGameFilter.Controls.Add(Me.txtProcess)
        Me.grpGameFilter.Controls.Add(Me.lblProcess)
        Me.grpGameFilter.Controls.Add(Me.lblName)
        Me.grpGameFilter.Controls.Add(Me.txtName)
        Me.grpGameFilter.Location = New System.Drawing.Point(12, 35)
        Me.grpGameFilter.Name = "grpGameFilter"
        Me.grpGameFilter.Size = New System.Drawing.Size(385, 150)
        Me.grpGameFilter.TabIndex = 1
        Me.grpGameFilter.TabStop = False
        '
        'grpGameInfoOptions
        '
        Me.grpGameInfoOptions.Controls.Add(Me.optOr)
        Me.grpGameInfoOptions.Controls.Add(Me.optAnd)
        Me.grpGameInfoOptions.Location = New System.Drawing.Point(14, 97)
        Me.grpGameInfoOptions.Name = "grpGameInfoOptions"
        Me.grpGameInfoOptions.Size = New System.Drawing.Size(106, 46)
        Me.grpGameInfoOptions.TabIndex = 6
        Me.grpGameInfoOptions.TabStop = False
        Me.grpGameInfoOptions.Text = "Options"
        '
        'optOr
        '
        Me.optOr.AutoSize = True
        Me.optOr.Location = New System.Drawing.Point(56, 19)
        Me.optOr.Name = "optOr"
        Me.optOr.Size = New System.Drawing.Size(36, 17)
        Me.optOr.TabIndex = 1
        Me.optOr.TabStop = True
        Me.optOr.Text = "Or"
        Me.optOr.UseVisualStyleBackColor = True
        '
        'optAnd
        '
        Me.optAnd.AutoSize = True
        Me.optAnd.Checked = True
        Me.optAnd.Location = New System.Drawing.Point(6, 19)
        Me.optAnd.Name = "optAnd"
        Me.optAnd.Size = New System.Drawing.Size(44, 17)
        Me.optAnd.TabIndex = 0
        Me.optAnd.TabStop = True
        Me.optAnd.Text = "And"
        Me.optAnd.UseVisualStyleBackColor = True
        '
        'txtCompany
        '
        Me.txtCompany.Location = New System.Drawing.Point(70, 71)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(309, 20)
        Me.txtCompany.TabIndex = 5
        '
        'lblCompany
        '
        Me.lblCompany.AutoSize = True
        Me.lblCompany.Location = New System.Drawing.Point(11, 74)
        Me.lblCompany.Name = "lblCompany"
        Me.lblCompany.Size = New System.Drawing.Size(54, 13)
        Me.lblCompany.TabIndex = 4
        Me.lblCompany.Text = "Company:"
        '
        'txtProcess
        '
        Me.txtProcess.Location = New System.Drawing.Point(70, 45)
        Me.txtProcess.Name = "txtProcess"
        Me.txtProcess.Size = New System.Drawing.Size(309, 20)
        Me.txtProcess.TabIndex = 3
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(11, 48)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(48, 13)
        Me.lblProcess.TabIndex = 2
        Me.lblProcess.Text = "Process:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(11, 22)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Text = "Name:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(70, 19)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(309, 20)
        Me.txtName.TabIndex = 1
        '
        'frmFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 516)
        Me.Controls.Add(Me.grpGameFilter)
        Me.Controls.Add(Me.grpTagFilter)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.optTag)
        Me.Controls.Add(Me.optGameInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFilter"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Custom Filter"
        Me.grpTagFilter.ResumeLayout(False)
        Me.grpTagFilter.PerformLayout()
        Me.grpTagOptions.ResumeLayout(False)
        Me.grpTagOptions.PerformLayout()
        Me.grpGameFilter.ResumeLayout(False)
        Me.grpGameFilter.PerformLayout()
        Me.grpGameInfoOptions.ResumeLayout(False)
        Me.grpGameInfoOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents optGameInfo As System.Windows.Forms.RadioButton
    Friend WithEvents optTag As System.Windows.Forms.RadioButton
    Friend WithEvents grpTagFilter As System.Windows.Forms.GroupBox
    Friend WithEvents grpTagOptions As System.Windows.Forms.GroupBox
    Friend WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents optAny As System.Windows.Forms.RadioButton
    Friend WithEvents lblGameTags As System.Windows.Forms.Label
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lstFilter As System.Windows.Forms.ListBox
    Friend WithEvents lstTags As System.Windows.Forms.ListBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grpGameFilter As System.Windows.Forms.GroupBox
    Friend WithEvents txtProcess As System.Windows.Forms.TextBox
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtCompany As System.Windows.Forms.TextBox
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents grpGameInfoOptions As System.Windows.Forms.GroupBox
    Friend WithEvents optOr As System.Windows.Forms.RadioButton
    Friend WithEvents optAnd As System.Windows.Forms.RadioButton
End Class
