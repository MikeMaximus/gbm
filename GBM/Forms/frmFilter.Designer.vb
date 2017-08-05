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
        Me.grpTagFilter = New System.Windows.Forms.GroupBox()
        Me.grpTagOptions = New System.Windows.Forms.GroupBox()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.optAny = New System.Windows.Forms.RadioButton()
        Me.lblGameTags = New System.Windows.Forms.Label()
        Me.lblTags = New System.Windows.Forms.Label()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.lstTagFilter = New System.Windows.Forms.ListBox()
        Me.lstTags = New System.Windows.Forms.ListBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpGameFilter = New System.Windows.Forms.GroupBox()
        Me.cboBoolFilter = New System.Windows.Forms.ComboBox()
        Me.numFilter = New System.Windows.Forms.NumericUpDown()
        Me.cboNumericOps = New System.Windows.Forms.ComboBox()
        Me.lblCurrentFilters = New System.Windows.Forms.Label()
        Me.lblFilterData = New System.Windows.Forms.Label()
        Me.lblFields = New System.Windows.Forms.Label()
        Me.btnRemoveFilter = New System.Windows.Forms.Button()
        Me.lstFilter = New System.Windows.Forms.ListBox()
        Me.btnAddFilter = New System.Windows.Forms.Button()
        Me.cboFilterField = New System.Windows.Forms.ComboBox()
        Me.grpFilterType = New System.Windows.Forms.GroupBox()
        Me.optOr = New System.Windows.Forms.RadioButton()
        Me.optAnd = New System.Windows.Forms.RadioButton()
        Me.txtStringFilter = New System.Windows.Forms.TextBox()
        Me.grpSorting = New System.Windows.Forms.GroupBox()
        Me.grpSortOptions = New System.Windows.Forms.GroupBox()
        Me.optSortAsc = New System.Windows.Forms.RadioButton()
        Me.optSortDesc = New System.Windows.Forms.RadioButton()
        Me.lblSortFields = New System.Windows.Forms.Label()
        Me.cboSortField = New System.Windows.Forms.ComboBox()
        Me.chkTag = New System.Windows.Forms.CheckBox()
        Me.chkGameInfo = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpTagFilter.SuspendLayout()
        Me.grpTagOptions.SuspendLayout()
        Me.grpGameFilter.SuspendLayout()
        CType(Me.numFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpFilterType.SuspendLayout()
        Me.grpSorting.SuspendLayout()
        Me.grpSortOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpTagFilter
        '
        Me.grpTagFilter.Controls.Add(Me.grpTagOptions)
        Me.grpTagFilter.Controls.Add(Me.lblGameTags)
        Me.grpTagFilter.Controls.Add(Me.lblTags)
        Me.grpTagFilter.Controls.Add(Me.btnRemove)
        Me.grpTagFilter.Controls.Add(Me.btnAdd)
        Me.grpTagFilter.Controls.Add(Me.lstTagFilter)
        Me.grpTagFilter.Controls.Add(Me.lstTags)
        Me.grpTagFilter.Location = New System.Drawing.Point(12, 236)
        Me.grpTagFilter.Name = "grpTagFilter"
        Me.grpTagFilter.Size = New System.Drawing.Size(385, 198)
        Me.grpTagFilter.TabIndex = 3
        Me.grpTagFilter.TabStop = False
        '
        'grpTagOptions
        '
        Me.grpTagOptions.Controls.Add(Me.optAll)
        Me.grpTagOptions.Controls.Add(Me.optAny)
        Me.grpTagOptions.Location = New System.Drawing.Point(6, 146)
        Me.grpTagOptions.Name = "grpTagOptions"
        Me.grpTagOptions.Size = New System.Drawing.Size(150, 46)
        Me.grpTagOptions.TabIndex = 6
        Me.grpTagOptions.TabStop = False
        Me.grpTagOptions.Text = "Options"
        '
        'optAll
        '
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
        Me.btnRemove.Location = New System.Drawing.Point(162, 88)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(61, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "<"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(162, 59)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(61, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = ">"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lstTagFilter
        '
        Me.lstTagFilter.FormattingEnabled = True
        Me.lstTagFilter.Location = New System.Drawing.Point(229, 32)
        Me.lstTagFilter.Name = "lstTagFilter"
        Me.lstTagFilter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTagFilter.Size = New System.Drawing.Size(150, 108)
        Me.lstTagFilter.Sorted = True
        Me.lstTagFilter.TabIndex = 5
        '
        'lstTags
        '
        Me.lstTags.FormattingEnabled = True
        Me.lstTags.Location = New System.Drawing.Point(6, 32)
        Me.lstTags.Name = "lstTags"
        Me.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstTags.Size = New System.Drawing.Size(150, 108)
        Me.lstTags.Sorted = True
        Me.lstTags.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(322, 526)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grpGameFilter
        '
        Me.grpGameFilter.Controls.Add(Me.cboBoolFilter)
        Me.grpGameFilter.Controls.Add(Me.numFilter)
        Me.grpGameFilter.Controls.Add(Me.cboNumericOps)
        Me.grpGameFilter.Controls.Add(Me.lblCurrentFilters)
        Me.grpGameFilter.Controls.Add(Me.lblFilterData)
        Me.grpGameFilter.Controls.Add(Me.lblFields)
        Me.grpGameFilter.Controls.Add(Me.btnRemoveFilter)
        Me.grpGameFilter.Controls.Add(Me.lstFilter)
        Me.grpGameFilter.Controls.Add(Me.btnAddFilter)
        Me.grpGameFilter.Controls.Add(Me.cboFilterField)
        Me.grpGameFilter.Controls.Add(Me.grpFilterType)
        Me.grpGameFilter.Controls.Add(Me.txtStringFilter)
        Me.grpGameFilter.Location = New System.Drawing.Point(12, 35)
        Me.grpGameFilter.Name = "grpGameFilter"
        Me.grpGameFilter.Size = New System.Drawing.Size(385, 172)
        Me.grpGameFilter.TabIndex = 1
        Me.grpGameFilter.TabStop = False
        '
        'cboBoolFilter
        '
        Me.cboBoolFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBoolFilter.FormattingEnabled = True
        Me.cboBoolFilter.Location = New System.Drawing.Point(162, 36)
        Me.cboBoolFilter.Name = "cboBoolFilter"
        Me.cboBoolFilter.Size = New System.Drawing.Size(136, 21)
        Me.cboBoolFilter.TabIndex = 3
        '
        'numFilter
        '
        Me.numFilter.DecimalPlaces = 1
        Me.numFilter.Location = New System.Drawing.Point(233, 37)
        Me.numFilter.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.numFilter.Name = "numFilter"
        Me.numFilter.Size = New System.Drawing.Size(65, 20)
        Me.numFilter.TabIndex = 4
        '
        'cboNumericOps
        '
        Me.cboNumericOps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNumericOps.FormattingEnabled = True
        Me.cboNumericOps.Location = New System.Drawing.Point(162, 36)
        Me.cboNumericOps.Name = "cboNumericOps"
        Me.cboNumericOps.Size = New System.Drawing.Size(65, 21)
        Me.cboNumericOps.TabIndex = 3
        '
        'lblCurrentFilters
        '
        Me.lblCurrentFilters.AutoSize = True
        Me.lblCurrentFilters.Location = New System.Drawing.Point(94, 65)
        Me.lblCurrentFilters.Name = "lblCurrentFilters"
        Me.lblCurrentFilters.Size = New System.Drawing.Size(71, 13)
        Me.lblCurrentFilters.TabIndex = 6
        Me.lblCurrentFilters.Text = "Current Filters"
        '
        'lblFilterData
        '
        Me.lblFilterData.AutoSize = True
        Me.lblFilterData.Location = New System.Drawing.Point(214, 20)
        Me.lblFilterData.Name = "lblFilterData"
        Me.lblFilterData.Size = New System.Drawing.Size(32, 13)
        Me.lblFilterData.TabIndex = 2
        Me.lblFilterData.Text = "Filter "
        '
        'lblFields
        '
        Me.lblFields.AutoSize = True
        Me.lblFields.Location = New System.Drawing.Point(41, 20)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(80, 13)
        Me.lblFields.TabIndex = 0
        Me.lblFields.Text = "Available Fields"
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Location = New System.Drawing.Point(259, 140)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnRemoveFilter.TabIndex = 9
        Me.btnRemoveFilter.Text = "Remove"
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'lstFilter
        '
        Me.lstFilter.FormattingEnabled = True
        Me.lstFilter.Location = New System.Drawing.Point(6, 81)
        Me.lstFilter.Name = "lstFilter"
        Me.lstFilter.Size = New System.Drawing.Size(247, 82)
        Me.lstFilter.TabIndex = 7
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Location = New System.Drawing.Point(304, 34)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnAddFilter.TabIndex = 5
        Me.btnAddFilter.Text = "Add"
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'cboFilterField
        '
        Me.cboFilterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilterField.FormattingEnabled = True
        Me.cboFilterField.Location = New System.Drawing.Point(6, 36)
        Me.cboFilterField.Name = "cboFilterField"
        Me.cboFilterField.Size = New System.Drawing.Size(150, 21)
        Me.cboFilterField.TabIndex = 1
        '
        'grpFilterType
        '
        Me.grpFilterType.Controls.Add(Me.optOr)
        Me.grpFilterType.Controls.Add(Me.optAnd)
        Me.grpFilterType.Location = New System.Drawing.Point(259, 81)
        Me.grpFilterType.Name = "grpFilterType"
        Me.grpFilterType.Size = New System.Drawing.Size(106, 46)
        Me.grpFilterType.TabIndex = 8
        Me.grpFilterType.TabStop = False
        Me.grpFilterType.Text = "Filter Type"
        '
        'optOr
        '
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
        Me.optAnd.Checked = True
        Me.optAnd.Location = New System.Drawing.Point(6, 19)
        Me.optAnd.Name = "optAnd"
        Me.optAnd.Size = New System.Drawing.Size(44, 17)
        Me.optAnd.TabIndex = 0
        Me.optAnd.TabStop = True
        Me.optAnd.Text = "And"
        Me.optAnd.UseVisualStyleBackColor = True
        '
        'txtStringFilter
        '
        Me.txtStringFilter.Location = New System.Drawing.Point(162, 36)
        Me.txtStringFilter.Name = "txtStringFilter"
        Me.txtStringFilter.Size = New System.Drawing.Size(136, 20)
        Me.txtStringFilter.TabIndex = 3
        '
        'grpSorting
        '
        Me.grpSorting.Controls.Add(Me.grpSortOptions)
        Me.grpSorting.Controls.Add(Me.lblSortFields)
        Me.grpSorting.Controls.Add(Me.cboSortField)
        Me.grpSorting.Location = New System.Drawing.Point(12, 440)
        Me.grpSorting.Name = "grpSorting"
        Me.grpSorting.Size = New System.Drawing.Size(385, 80)
        Me.grpSorting.TabIndex = 4
        Me.grpSorting.TabStop = False
        Me.grpSorting.Text = "Sorting"
        '
        'grpSortOptions
        '
        Me.grpSortOptions.Controls.Add(Me.optSortAsc)
        Me.grpSortOptions.Controls.Add(Me.optSortDesc)
        Me.grpSortOptions.Location = New System.Drawing.Point(162, 19)
        Me.grpSortOptions.Name = "grpSortOptions"
        Me.grpSortOptions.Size = New System.Drawing.Size(189, 43)
        Me.grpSortOptions.TabIndex = 3
        Me.grpSortOptions.TabStop = False
        Me.grpSortOptions.Text = "Sort Options"
        '
        'optSortAsc
        '
        Me.optSortAsc.AutoSize = True
        Me.optSortAsc.Location = New System.Drawing.Point(6, 17)
        Me.optSortAsc.Name = "optSortAsc"
        Me.optSortAsc.Size = New System.Drawing.Size(75, 17)
        Me.optSortAsc.TabIndex = 0
        Me.optSortAsc.TabStop = True
        Me.optSortAsc.Text = "Ascending"
        Me.optSortAsc.UseVisualStyleBackColor = True
        '
        'optSortDesc
        '
        Me.optSortDesc.AutoSize = True
        Me.optSortDesc.Location = New System.Drawing.Point(90, 17)
        Me.optSortDesc.Name = "optSortDesc"
        Me.optSortDesc.Size = New System.Drawing.Size(82, 17)
        Me.optSortDesc.TabIndex = 1
        Me.optSortDesc.TabStop = True
        Me.optSortDesc.Text = "Descending"
        Me.optSortDesc.UseVisualStyleBackColor = True
        '
        'lblSortFields
        '
        Me.lblSortFields.AutoSize = True
        Me.lblSortFields.Location = New System.Drawing.Point(41, 19)
        Me.lblSortFields.Name = "lblSortFields"
        Me.lblSortFields.Size = New System.Drawing.Size(80, 13)
        Me.lblSortFields.TabIndex = 1
        Me.lblSortFields.Text = "Available Fields"
        '
        'cboSortField
        '
        Me.cboSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSortField.FormattingEnabled = True
        Me.cboSortField.Location = New System.Drawing.Point(6, 35)
        Me.cboSortField.Name = "cboSortField"
        Me.cboSortField.Size = New System.Drawing.Size(150, 21)
        Me.cboSortField.TabIndex = 2
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.Location = New System.Drawing.Point(12, 213)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(45, 17)
        Me.chkTag.TabIndex = 2
        Me.chkTag.Text = "Tag"
        Me.chkTag.UseVisualStyleBackColor = True
        '
        'chkGameInfo
        '
        Me.chkGameInfo.AutoSize = True
        Me.chkGameInfo.Location = New System.Drawing.Point(12, 12)
        Me.chkGameInfo.Name = "chkGameInfo"
        Me.chkGameInfo.Size = New System.Drawing.Size(109, 17)
        Me.chkGameInfo.TabIndex = 0
        Me.chkGameInfo.Text = "Game Information"
        Me.chkGameInfo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 531)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(249, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "* Indicates a field that may give unexpected results."
        '
        'frmFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 561)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grpSorting)
        Me.Controls.Add(Me.chkTag)
        Me.Controls.Add(Me.grpGameFilter)
        Me.Controls.Add(Me.chkGameInfo)
        Me.Controls.Add(Me.grpTagFilter)
        Me.Controls.Add(Me.btnOK)
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
        Me.grpGameFilter.ResumeLayout(False)
        Me.grpGameFilter.PerformLayout()
        CType(Me.numFilter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpFilterType.ResumeLayout(False)
        Me.grpSorting.ResumeLayout(False)
        Me.grpSorting.PerformLayout()
        Me.grpSortOptions.ResumeLayout(False)
        Me.grpSortOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpTagFilter As System.Windows.Forms.GroupBox
    Friend WithEvents grpTagOptions As System.Windows.Forms.GroupBox
    Friend WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents optAny As System.Windows.Forms.RadioButton
    Friend WithEvents lblGameTags As System.Windows.Forms.Label
    Friend WithEvents lblTags As System.Windows.Forms.Label
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents lstTagFilter As System.Windows.Forms.ListBox
    Friend WithEvents lstTags As System.Windows.Forms.ListBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grpGameFilter As System.Windows.Forms.GroupBox
    Friend WithEvents txtStringFilter As System.Windows.Forms.TextBox
    Friend WithEvents grpFilterType As System.Windows.Forms.GroupBox
    Friend WithEvents optOr As System.Windows.Forms.RadioButton
    Friend WithEvents optAnd As System.Windows.Forms.RadioButton
    Friend WithEvents grpSorting As GroupBox
    Friend WithEvents optSortDesc As RadioButton
    Friend WithEvents optSortAsc As RadioButton
    Friend WithEvents cboSortField As ComboBox
    Friend WithEvents chkTag As CheckBox
    Friend WithEvents chkGameInfo As CheckBox
    Friend WithEvents cboFilterField As ComboBox
    Friend WithEvents lstFilter As ListBox
    Friend WithEvents btnAddFilter As Button
    Friend WithEvents btnRemoveFilter As Button
    Friend WithEvents lblCurrentFilters As Label
    Friend WithEvents lblFilterData As Label
    Friend WithEvents lblFields As Label
    Friend WithEvents cboNumericOps As ComboBox
    Friend WithEvents numFilter As NumericUpDown
    Friend WithEvents cboBoolFilter As ComboBox
    Friend WithEvents lblSortFields As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents grpSortOptions As GroupBox
End Class
