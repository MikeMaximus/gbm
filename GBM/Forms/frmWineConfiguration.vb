Imports GBM.My.Resources

Public Class frmWineConfiguration
    Private oSettings As mgrSettings
    Private oWineData As clsWineData
    Private bNewMode As Boolean = False

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Property WineData As clsWineData
        Get
            Return oWineData
        End Get
        Set(value As clsWineData)
            oWineData = value
        End Set
    End Property

    Public Property NewMode As Boolean
        Get
            Return bNewMode
        End Get
        Set(value As Boolean)
            bNewMode = value
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
        If txtWineBinaryPath.Text = String.Empty And txtWinePrefix.Text = String.Empty And txtWineSavePath.Text = String.Empty Then
            If bNewMode Then
                Me.DialogResult = DialogResult.OK
            Else
                mgrWineData.DoWineDataDelete(oWineData.MonitorID)
                Me.DialogResult = DialogResult.OK
            End If
        Else
            If ValidateData() Then
                oWineData.BinaryPath = txtWineBinaryPath.Text
                oWineData.Prefix = txtWinePrefix.Text
                oWineData.SavePath = txtWineSavePath.Text
                If Not bNewMode Then
                    mgrWineData.DoWineDataAddUpdate(oWineData)
                End If
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

    Private Sub frmWineConfiguration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If oWineData.BinaryPath = String.Empty And oWineData.Prefix = String.Empty And oWineData.SavePath = String.Empty Then
            oWineData = Nothing
        End If
    End Sub
End Class