Imports GBM.My.Resources

Public Class frmLudusaviConfig
    Public Property ImportOptions As clsLudusaviOptions

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmLudusaviConfig_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        grpSearch.Text = frmLudusaviConfig_grpSearch
        grpProfileTypes.Text = frmLudusaviConfig_grpProfileTypes
        chkSavedGames.Text = frmLudusaviConfig_chkSavedGames
        chkConfigurationFiles.Text = frmLudusaviConfig_chkConfigurationFiles
        grpOperatingSystems.Text = frmLudusaviConfig_grpOperatingSystems
        chkWindows.Text = frmLudusaviConfig_chkWindows
        chkLinux.Text = frmLudusaviConfig_chkLinux
        btnOK.Text = frmLudusaviConfig_btnOK
        btnOK.Image = Multi_Ok
        btnCancel.Text = frmLudusaviConfig_btnCancel
        btnCancel.Image = Multi_Cancel

        'Set Defaults
        chkSavedGames.Checked = True

        If Not mgrCommon.IsUnix Then
            chkWindows.Checked = True
            grpOperatingSystems.Enabled = False
        Else
            chkLinux.Checked = True
        End If
    End Sub

    Private Function ValidateOptions() As Boolean
        If Not chkSavedGames.Checked And Not chkConfigurationFiles.Checked Then
            mgrCommon.ShowMessage(frmLudusaviConfig_ErrorProfileType, MsgBoxStyle.Information)
            Return False
        End If
        If Not chkWindows.Checked And Not chkLinux.Checked Then
            mgrCommon.ShowMessage(frmLudusaviConfig_ErrorOperatingSystem, MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Private Sub BuildOptions()
        ImportOptions = New clsLudusaviOptions()
        ImportOptions.Query = txtQuery.Text
        ImportOptions.IncludeSaves = chkSavedGames.Checked
        ImportOptions.IncludeConfigs = chkConfigurationFiles.Checked
        If chkWindows.Checked Then ImportOptions.IncludeOS = ImportOptions.IncludeOS Or clsLudusaviOptions.eSupportedOS.Windows
        If chkLinux.Checked Then ImportOptions.IncludeOS = ImportOptions.IncludeOS Or clsLudusaviOptions.eSupportedOS.Linux
    End Sub

    Private Sub frmConfigLudusavi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If ValidateOptions() Then
            BuildOptions()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmLudusaviConfig_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                btnOK.PerformClick()
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class