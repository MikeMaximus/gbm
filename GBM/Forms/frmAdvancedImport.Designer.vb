﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.btnImport = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblGames = New System.Windows.Forms.Label()
        Me.lblSelected = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lstGames = New System.Windows.Forms.ListView()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.chkSelectedOnly = New System.Windows.Forms.CheckBox()
        Me.btnDetectSavedGames = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImport.Location = New System.Drawing.Point(616, 575)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 5
        Me.btnImport.Text = "&Import"
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
        'lblGames
        '
        Me.lblGames.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblGames.Location = New System.Drawing.Point(12, 580)
        Me.lblGames.Name = "lblGames"
        Me.lblGames.Size = New System.Drawing.Size(760, 14)
        Me.lblGames.TabIndex = 0
        Me.lblGames.Text = "Games"
        Me.lblGames.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSelected
        '
        Me.lblSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSelected.AutoSize = True
        Me.lblSelected.Location = New System.Drawing.Point(9, 580)
        Me.lblSelected.Name = "lblSelected"
        Me.lblSelected.Size = New System.Drawing.Size(77, 13)
        Me.lblSelected.TabIndex = 0
        Me.lblSelected.Text = "Selected Items"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(697, 575)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
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
        Me.lstGames.Size = New System.Drawing.Size(760, 529)
        Me.lstGames.TabIndex = 4
        Me.lstGames.UseCompatibleStateImageBehavior = False
        Me.lstGames.View = System.Windows.Forms.View.Details
        '
        'txtFilter
        '
        Me.txtFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilter.Location = New System.Drawing.Point(616, 9)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(156, 20)
        Me.txtFilter.TabIndex = 3
        '
        'lblFilter
        '
        Me.lblFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilter.Location = New System.Drawing.Point(571, 12)
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
        Me.btnDetectSavedGames.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDetectSavedGames.Location = New System.Drawing.Point(425, 7)
        Me.btnDetectSavedGames.Name = "btnDetectSavedGames"
        Me.btnDetectSavedGames.Size = New System.Drawing.Size(140, 23)
        Me.btnDetectSavedGames.TabIndex = 2
        Me.btnDetectSavedGames.Text = "Detect Saved Games"
        Me.btnDetectSavedGames.UseVisualStyleBackColor = True
        '
        'frmAdvancedImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 611)
        Me.Controls.Add(Me.btnDetectSavedGames)
        Me.Controls.Add(Me.chkSelectedOnly)
        Me.Controls.Add(Me.lblFilter)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.lstGames)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelected)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.lblGames)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmAdvancedImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Game Configurations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblGames As System.Windows.Forms.Label
    Friend WithEvents lblSelected As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lstGames As System.Windows.Forms.ListView
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents chkSelectedOnly As CheckBox
    Friend WithEvents btnDetectSavedGames As Button
End Class
