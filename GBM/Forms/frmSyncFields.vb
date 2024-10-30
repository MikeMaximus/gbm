﻿Imports GBM.My.Resources

Public Class frmSyncFields
    Public Property SyncFields As clsGame.eOptionalSyncFields
    Private Property IsDirty As Boolean = False

    Private Sub LoadForm()
        'Load fields
        If (SyncFields And clsGame.eOptionalSyncFields.Company) = clsGame.eOptionalSyncFields.Company Then
            chkCompany.Checked = True
        End If
        If (SyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then
            chkGamePath.Checked = True
        End If
        If (SyncFields And clsGame.eOptionalSyncFields.Icon) = clsGame.eOptionalSyncFields.Icon Then
            chkIcon.Checked = True
        End If
        If (SyncFields And clsGame.eOptionalSyncFields.MonitorGame) = clsGame.eOptionalSyncFields.MonitorGame Then
            chkMonitorGame.Checked = True
        End If
        If (SyncFields And clsGame.eOptionalSyncFields.Version) = clsGame.eOptionalSyncFields.Version Then
            chkVersion.Checked = True
        End If
    End Sub

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        IsDirty = True
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is CheckBox Then AddHandler DirectCast(ctl, CheckBox).CheckedChanged, AddressOf DirtyCheck_ValueChanged
        Next
    End Sub

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = frmSyncFields_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        btnCancel.Text = frmSyncFields_btnCancel
        btnCancel.Image = Multi_Cancel
        btnSave.Text = frmSyncFields_btnSave
        btnSave.Image = Multi_Save
        grpFields.Text = frmSyncFields_grpFields
        chkMonitorGame.Text = frmSyncFields_chkMonitorGame
        chkIcon.Text = frmSyncFields_chkIcon
        chkVersion.Text = frmSyncFields_chkVersion
        chkCompany.Text = frmSyncFields_chkCompany
        chkGamePath.Text = frmSyncFields_chkGamePath
    End Sub

    Private Sub frmSyncFields_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadForm()
        AssignDirtyHandlers(grpFields.Controls)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        IsDirty = False
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub chkGamePath_CheckedChanged(sender As Object, e As EventArgs) Handles chkGamePath.CheckedChanged
        If chkGamePath.Checked Then
            SyncFields = clsGame.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.GamePath)
        Else
            SyncFields = clsGame.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.GamePath)
        End If
    End Sub

    Private Sub chkCompany_CheckedChanged(sender As Object, e As EventArgs) Handles chkCompany.CheckedChanged
        If chkCompany.Checked Then
            SyncFields = clsGame.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Company)
        Else
            SyncFields = clsGame.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Company)
        End If
    End Sub

    Private Sub chkVersion_CheckedChanged(sender As Object, e As EventArgs) Handles chkVersion.CheckedChanged
        If chkVersion.Checked Then
            SyncFields = clsGame.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Version)
        Else
            SyncFields = clsGame.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Version)
        End If
    End Sub

    Private Sub chkIcon_CheckedChanged(sender As Object, e As EventArgs) Handles chkIcon.CheckedChanged
        If chkIcon.Checked Then
            SyncFields = clsGame.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Icon)
        Else
            SyncFields = clsGame.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Icon)
        End If
    End Sub

    Private Sub chkMonitorGame_CheckedChanged(sender As Object, e As EventArgs) Handles chkMonitorGame.CheckedChanged
        If chkMonitorGame.Checked Then
            SyncFields = clsGame.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.MonitorGame)
        Else
            SyncFields = clsGame.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.MonitorGame)
        End If
    End Sub

    Private Sub frmSyncFields_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsDirty Then
            Select Case mgrCommon.ConfirmDirty()
                Case MsgBoxResult.No
                    btnCancel.PerformClick()
                Case MsgBoxResult.Yes
                    btnSave.PerformClick()
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub frmSyncFields_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class