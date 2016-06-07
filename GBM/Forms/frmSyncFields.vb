Imports GBM.My.Resources

Public Class frmSyncFields
    Private eSyncFields As clsGame.eOptionalSyncFields

    Public Property SyncFields As clsGame.eOptionalSyncFields
        Get
            Return eSyncFields
        End Get
        Set(value As clsGame.eOptionalSyncFields)
            eSyncFields = value
        End Set
    End Property

    Private Sub LoadForm()
        'Load fields
        If (eSyncFields And clsGame.eOptionalSyncFields.Company) = clsGame.eOptionalSyncFields.Company Then
            chkCompany.Checked = True
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then
            chkGamePath.Checked = True
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.Icon) = clsGame.eOptionalSyncFields.Icon Then
            chkIcon.Checked = True
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.MonitorGame) = clsGame.eOptionalSyncFields.MonitorGame Then
            chkMonitorGame.Checked = True
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.TimeStamp) = clsGame.eOptionalSyncFields.TimeStamp Then
            chkTimeStamp.Checked = True
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.Version) = clsGame.eOptionalSyncFields.Version Then
            chkVersion.Checked = True
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmSyncFields_FormName

        'Set Form Text
        btnCancel.Text = frmSyncFields_btnCancel
        btnSave.Text = frmSyncFields_btnSave
        grpFields.Text = frmSyncFields_grpFields
        chkMonitorGame.Text = frmSyncFields_chkMonitorGame
        chkIcon.Text = frmSyncFields_chkIcon
        chkVersion.Text = frmSyncFields_chkVersion
        chkCompany.Text = frmSyncFields_chkCompany
        chkGamePath.Text = frmSyncFields_chkGamePath
        chkTimeStamp.Text = frmSyncFields_chkTimeStamp
    End Sub

    Private Sub frmSyncFields_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadForm()
        Dim sResources As String = String.Empty
        Dim sCode As String = String.Empty
        mgrCommon.GetAllStrings(Me, sResources, sCode, "frmSyncFields")
        Clipboard.SetText(sResources & vbCrLf & vbCrLf & sCode)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub chkTimeStamp_CheckedChanged(sender As Object, e As EventArgs) Handles chkTimeStamp.CheckedChanged
        If chkTimeStamp.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.TimeStamp)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.TimeStamp)
        End If
    End Sub

    Private Sub chkGamePath_CheckedChanged(sender As Object, e As EventArgs) Handles chkGamePath.CheckedChanged
        If chkGamePath.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.GamePath)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.GamePath)
        End If
    End Sub

    Private Sub chkCompany_CheckedChanged(sender As Object, e As EventArgs) Handles chkCompany.CheckedChanged
        If chkCompany.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Company)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Company)
        End If
    End Sub

    Private Sub chkVersion_CheckedChanged(sender As Object, e As EventArgs) Handles chkVersion.CheckedChanged
        If chkVersion.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Version)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Version)
        End If
    End Sub

    Private Sub chkIcon_CheckedChanged(sender As Object, e As EventArgs) Handles chkIcon.CheckedChanged
        If chkIcon.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.Icon)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.Icon)
        End If
    End Sub

    Private Sub chkMonitorGame_CheckedChanged(sender As Object, e As EventArgs) Handles chkMonitorGame.CheckedChanged
        If chkMonitorGame.Checked Then
            SyncFields = mgrCommon.SetSyncField(SyncFields, clsGame.eOptionalSyncFields.MonitorGame)
        Else
            SyncFields = mgrCommon.RemoveSyncField(SyncFields, clsGame.eOptionalSyncFields.MonitorGame)
        End If
    End Sub
End Class