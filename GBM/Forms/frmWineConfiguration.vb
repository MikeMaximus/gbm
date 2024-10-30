Imports GBM.My.Resources

Public Class frmWineConfiguration
    Public Property MonitorID As String
    Public Property GameName As String
    Private Property IsDirty As Boolean = False

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        IsDirty = True
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then AddHandler DirectCast(ctl, TextBox).TextChanged, AddressOf DirtyCheck_ValueChanged
        Next
    End Sub

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = mgrCommon.FormatString(frmWineConfiguration_FormName, GameName)
        Me.Icon = GBM_Icon

        'Set Form Text
        grpWineConfig.Text = frmWineConfiguration_grpWineConfig
        lblWineBinaryPath.Text = frmWineConfiguration_lblWineBinaryPath
        lblWinePrefix.Text = frmWineConfiguration_lblWinePrefix
        lblWineSavePath.Text = frmWineConfiguration_lblWineSavePath
        btnSave.Text = frmWineConfiguration_btnSave
        btnSave.Image = Multi_Save
        btnCancel.Text = frmWineConfiguration_btnCancel
        btnCancel.Image = Multi_Cancel
    End Sub

    Private Sub LoadData()
        Dim oWineData As New clsWineData
        oWineData = mgrWineData.DoWineDataGetbyID(MonitorID)
        txtWineBinaryPath.Text = oWineData.BinaryPath
        txtWinePrefix.Text = oWineData.Prefix
        txtWineSavePath.Text = oWineData.SavePath
        AssignDirtyHandlers(grpWineConfig.Controls)
    End Sub

    Private Sub HandleWarning()
        If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.WineConfig) = mgrSettings.eSuppressMessages.WineConfig Then
            mgrCommon.ShowMessage(frmWineConfiguration_WarningSingle, MsgBoxStyle.Information)
            mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.WineConfig)
            mgrSettings.SaveSettings()
        End If
    End Sub

    Private Function ValidateData() As Boolean
        If txtWineBinaryPath.Text = String.Empty Then
            mgrCommon.ShowMessage(frmWineConfiguration_ErrorValidationBinaryPath, MsgBoxStyle.Exclamation)
            Return False
        End If

        If txtWinePrefix.Text = String.Empty Then
            mgrCommon.ShowMessage(frmWineConfiguration_ErrorValidationPrefix, MsgBoxStyle.Exclamation)
            Return False
        End If

        Return True
    End Function

    Private Sub SaveData()
        Dim oWineData As clsWineData
        If txtWineBinaryPath.Text = String.Empty And txtWinePrefix.Text = String.Empty And txtWineSavePath.Text = String.Empty Then
            mgrWineData.DoWineDataDelete(MonitorID)
            Me.DialogResult = DialogResult.OK
        Else
            If ValidateData() Then
                oWineData = New clsWineData
                oWineData.MonitorID = MonitorID
                oWineData.BinaryPath = txtWineBinaryPath.Text
                oWineData.Prefix = txtWinePrefix.Text
                oWineData.SavePath = txtWineSavePath.Text
                mgrWineData.DoWineDataAddUpdate(oWineData)
                Me.DialogResult = DialogResult.OK
            End If
        End If

    End Sub

    Private Sub frmAdvancedConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        HandleWarning()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        IsDirty = False
        SaveData()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub frmWineConfiguration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
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

    Private Sub frmWineConfiguration_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class