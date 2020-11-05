Imports GBM.My.Resources
Imports System.IO

Public Class frmLaunchConfiguration
    Private sGameName As String
    Private sMonitorID As String

    Property GameName As String
        Get
            Return sGameName
        End Get
        Set(value As String)
            sGameName = value
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
        Me.Text = mgrCommon.FormatString(frmLaunchConfiguration_FormName, GameName)
        Me.Icon = GBM_Icon

        'Set Form Text
        grpStoreLauncher.Text = frmLaunchConfiguration_grpStoreLauncher
        lblLauncher.Text = frmLaunchConfiguration_lblLauncher
        btnOpenLaunchers.Text = frmLaunchConfiguration_btnOpenLaunchers
        lblGameID.Text = frmLaunchConfiguration_lblGameID
        grpOtherConfig.Text = frmLaunchConfiguration_grpOtherConfig
        lblExe.Text = frmLaunchConfiguration_lblExe
        lblArguments.Text = frmLaunchConfiguration_lblArguments
        btnSave.Text = frmLaunchConfiguration_btnSave
        btnCancel.Text = frmLaunchConfiguration_btnCancel
    End Sub

    Private Sub LoadCombos()
        Dim oComboItems As New List(Of KeyValuePair(Of String, String))
        Dim hshLaunchers As Hashtable = mgrLaunchers.ReadLaunchers
        Dim oLauncher As clsLauncher

        'cboLauncher
        cboLauncher.DataSource = Nothing
        cboLauncher.ValueMember = "Key"
        cboLauncher.DisplayMember = "Value"

        oComboItems.Add(New KeyValuePair(Of String, String)("nolauncher", frmLaunchConfiguration_NoLauncher))

        For Each de As DictionaryEntry In hshLaunchers
            oLauncher = DirectCast(de.Value, clsLauncher)
            oComboItems.Add(New KeyValuePair(Of String, String)(oLauncher.LauncherID, oLauncher.Name))
        Next

        cboLauncher.DataSource = oComboItems
    End Sub

    Private Sub LoadData()
        Dim oLaunchData As New clsLaunchData
        oLaunchData = mgrLaunchData.DoLaunchDataGetbyID(sMonitorID)
        LoadCombos()
        If oLaunchData.LauncherID = String.Empty Then
            cboLauncher.SelectedValue = "nolauncher"
        Else
            cboLauncher.SelectedValue = oLaunchData.LauncherID
        End If
        txtGameID.Text = oLaunchData.LauncherGameID
        txtExePath.Text = oLaunchData.Path
        txtArguments.Text = oLaunchData.Args
    End Sub

    Private Function ValidateData() As Boolean
        If cboLauncher.SelectedValue <> "nolauncher" And txtGameID.Text = String.Empty Then
            mgrCommon.ShowMessage(frmLaunchConfiguration_ErrorNoGameID, MsgBoxStyle.Exclamation)
            Return False
            txtGameID.Focus()
            Return False
        ElseIf Not txtExePath.Text = String.Empty Then
            If Not File.Exists(txtExePath.Text) Then
                mgrCommon.ShowMessage(frmLaunchConfiguration_ErrorExeNotFound, MsgBoxStyle.Exclamation)
                txtExePath.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub SaveData()
        Dim oLaunchData As clsLaunchData
        If cboLauncher.SelectedValue = "nolauncher" And txtGameID.Text = String.Empty And txtExePath.Text = String.Empty And txtArguments.Text = String.Empty Then
            mgrLaunchData.DoLaunchDataDelete(sMonitorID)
            Me.DialogResult = DialogResult.OK
        Else
            If ValidateData() Then
                oLaunchData = New clsLaunchData
                oLaunchData.MonitorID = sMonitorID
                If cboLauncher.SelectedValue = "nolauncher" Then
                    oLaunchData.LauncherID = String.Empty
                Else
                    oLaunchData.LauncherID = cboLauncher.SelectedValue
                End If
                oLaunchData.LauncherGameID = txtGameID.Text
                oLaunchData.Path = txtExePath.Text
                oLaunchData.Args = txtArguments.Text
                mgrLaunchData.DoLaunchDataAddUpdate(oLaunchData)
                Me.DialogResult = DialogResult.OK
            End If
        End If

    End Sub

    Private Sub ExeBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = txtExePath.Text
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            sCurrentPath = Path.GetDirectoryName(txtExePath.Text)
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFileBrowser("LC_Exe", frmProcessManager_ChooseProcess, "exe",
                                          frmLaunchConfiguration_Executable, sDefaultFolder, True)

        If sNewPath <> String.Empty Then
            txtExePath.Text = sNewPath
        End If

    End Sub

    Private Sub OpenLaunchers()
        Dim frm As New frmLauncherManager
        Dim oValue As Object = cboLauncher.SelectedValue
        frm.ShowDialog()
        LoadCombos()
        cboLauncher.SelectedValue = oValue
    End Sub

    Private Sub frmAdvancedConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveData()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub btnOpenLaunchers_Click(sender As Object, e As EventArgs) Handles btnOpenLaunchers.Click
        OpenLaunchers()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        ExeBrowse()
    End Sub
End Class