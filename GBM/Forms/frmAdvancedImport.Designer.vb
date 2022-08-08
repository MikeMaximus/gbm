<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAdvancedImport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lstGames = New System.Windows.Forms.ListView()
        Me.cmsOptions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmiIgnore = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.chkSelectedOnly = New System.Windows.Forms.CheckBox()
        Me.btnDetectSavedGames = New System.Windows.Forms.Button()
        Me.bwLoader = New System.ComponentModel.BackgroundWorker()
        Me.btnClearSelected = New System.Windows.Forms.Button()
        Me.bwDetect = New System.ComponentModel.BackgroundWorker()
        Me.bwImport = New System.ComponentModel.BackgroundWorker()
        Me.chkHideIgnored = New System.Windows.Forms.CheckBox()
        Me.cmsOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImport.Image = Global.GBM.My.Resources.Resources.Multi_Import
        Me.btnImport.Location = New System.Drawing.Point(646, 559)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(60, 45)
        Me.btnImport.TabIndex = 7
        Me.btnImport.Text = "&Import"
        Me.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(12, 11)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(70, 17)
        Me.chkSelectAll.TabIndex = 0
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblStatus.Location = New System.Drawing.Point(12, 575)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(760, 14)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Status"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Image = Global.GBM.My.Resources.Resources.Multi_Cancel
        Me.btnCancel.Location = New System.Drawing.Point(712, 559)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 45)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lstGames
        '
        Me.lstGames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstGames.CheckBoxes = True
        Me.lstGames.FullRowSelect = True
        Me.lstGames.HideSelection = False
        Me.lstGames.Location = New System.Drawing.Point(12, 35)
        Me.lstGames.Name = "lstGames"
        Me.lstGames.Size = New System.Drawing.Size(760, 518)
        Me.lstGames.TabIndex = 5
        Me.lstGames.UseCompatibleStateImageBehavior = False
        Me.lstGames.View = System.Windows.Forms.View.Details
        '
        'cmsOptions
        '
        Me.cmsOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmiIgnore})
        Me.cmsOptions.Name = "cmsOptions"
        Me.cmsOptions.Size = New System.Drawing.Size(109, 26)
        '
        'cmiIgnore
        '
        Me.cmiIgnore.Name = "cmiIgnore"
        Me.cmiIgnore.Size = New System.Drawing.Size(108, 22)
        Me.cmiIgnore.Text = "&Ignore"
        '
        'txtFilter
        '
        Me.txtFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilter.Location = New System.Drawing.Point(586, 8)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(156, 20)
        Me.txtFilter.TabIndex = 3
        '
        'lblFilter
        '
        Me.lblFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilter.Location = New System.Drawing.Point(541, 11)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(39, 14)
        Me.lblFilter.TabIndex = 0
        Me.lblFilter.Text = "Filter:"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkSelectedOnly
        '
        Me.chkSelectedOnly.AutoSize = True
        Me.chkSelectedOnly.Location = New System.Drawing.Point(88, 11)
        Me.chkSelectedOnly.Name = "chkSelectedOnly"
        Me.chkSelectedOnly.Size = New System.Drawing.Size(122, 17)
        Me.chkSelectedOnly.TabIndex = 1
        Me.chkSelectedOnly.Text = "Show Only Selected"
        Me.chkSelectedOnly.UseVisualStyleBackColor = True
        '
        'btnDetectSavedGames
        '
        Me.btnDetectSavedGames.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDetectSavedGames.Image = Global.GBM.My.Resources.Resources.Multi_Search
        Me.btnDetectSavedGames.Location = New System.Drawing.Point(12, 559)
        Me.btnDetectSavedGames.Name = "btnDetectSavedGames"
        Me.btnDetectSavedGames.Size = New System.Drawing.Size(140, 45)
        Me.btnDetectSavedGames.TabIndex = 6
        Me.btnDetectSavedGames.Text = "&Detect Saved Games"
        Me.btnDetectSavedGames.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDetectSavedGames.UseVisualStyleBackColor = True
        '
        'bwLoader
        '
        '
        'btnClearSelected
        '
        Me.btnClearSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearSelected.Image = Global.GBM.My.Resources.Resources.frmMain_Cancel_Small
        Me.btnClearSelected.Location = New System.Drawing.Point(748, 5)
        Me.btnClearSelected.Name = "btnClearSelected"
        Me.btnClearSelected.Size = New System.Drawing.Size(24, 24)
        Me.btnClearSelected.TabIndex = 4
        Me.btnClearSelected.UseVisualStyleBackColor = True
        '
        'bwDetect
        '
        '
        'bwImport
        '
        '
        'chkHideIgnored
        '
        Me.chkHideIgnored.AutoSize = True
        Me.chkHideIgnored.Location = New System.Drawing.Point(216, 11)
        Me.chkHideIgnored.Name = "chkHideIgnored"
        Me.chkHideIgnored.Size = New System.Drawing.Size(87, 17)
        Me.chkHideIgnored.TabIndex = 2
        Me.chkHideIgnored.Text = "Hide Ignored"
        Me.chkHideIgnored.UseVisualStyleBackColor = True
        '
        'frmAdvancedImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 611)
        Me.Controls.Add(Me.chkHideIgnored)
        Me.Controls.Add(Me.btnClearSelected)
        Me.Controls.Add(Me.btnDetectSavedGames)
        Me.Controls.Add(Me.chkSelectedOnly)
        Me.Controls.Add(Me.lblFilter)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.lstGames)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.lblStatus)
        Me.KeyPreview = True
        Me.Name = "frmAdvancedImport"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Game Configurations"
        Me.cmsOptions.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lstGames As System.Windows.Forms.ListView
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents chkSelectedOnly As CheckBox
    Friend WithEvents btnDetectSavedGames As Button
    Friend WithEvents bwLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents btnClearSelected As Button
    Friend WithEvents bwDetect As System.ComponentModel.BackgroundWorker
    Friend WithEvents bwImport As System.ComponentModel.BackgroundWorker
    Friend WithEvents cmsOptions As ContextMenuStrip
    Friend WithEvents cmiIgnore As ToolStripMenuItem
    Friend WithEvents chkHideIgnored As CheckBox
End Class
