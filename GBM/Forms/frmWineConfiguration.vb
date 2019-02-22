Imports GBM.My.Resources

Public Class frmWineConfiguration
    Private oSettings As mgrSettings
    Private sMonitorID As String

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmWineConfiguration_FormName

        'Set Form Text
        grpWineConfig.Text = frmWineConfiguration_grpWineConfig
        lblWineBinaryPath.Text = frmWineConfiguration_lblWineBinaryPath
        lblWinePrefix.Text = frmWineConfiguration_lblWinePrefix
        lblWineSavePath.Text = frmWineConfiguration_lblWineSavePath
        btnSave.Text = frmWineConfiguration_btnSave
        btnCancel.Text = frmWineConfiguration_btnCancel
    End Sub

    Private Sub LoadData()
        Dim oWineData As New clsWineData
        oWineData = mgrWineData.DoWineDataGetbyID(sMonitorID)
        txtWineBinaryPath.Text = oWineData.BinaryPath
        txtWinePrefix.Text = oWineData.Prefix
        txtWineSavePath.Text = oWineData.SavePath
    End Sub

    Private Sub HandleWarning()
        If Not (oSettings.SuppressMessages And mgrSettings.eSuppressMessages.WineConfig) = mgrSettings.eSuppressMessages.WineConfig Then
            mgrCommon.ShowMessage(frmWineConfiguration_WarningSingle, MsgBoxStyle.Information)
            oSettings.SuppressMessages = oSettings.SetMessageField(oSettings.SuppressMessages, mgrSettings.eSuppressMessages.WineConfig)
            oSettings.SaveSettings()
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
            mgrWineData.DoWineDataDelete(sMonitorID)
            Me.DialogResult = DialogResult.OK
        Else
            If ValidateData() Then
                oWineData = New clsWineData
                oWineData.MonitorID = sMonitorID
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
        SaveData()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub
End Class