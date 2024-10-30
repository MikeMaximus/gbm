﻿Imports GBM.My.Resources
Imports System.IO

Public Class frmLaunchConfiguration
    Private WithEvents tmUpdateTimer As Timer

    Public Property Game As clsGame
    Private Property IsDirty As Boolean = False
    Private Property IsLoading As Boolean = False

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = mgrCommon.FormatString(frmLaunchConfiguration_FormName, Game.CroppedName)
        Me.Icon = GBM_Icon

        'Set Form Text
        grpStoreLauncher.Text = frmLaunchConfiguration_grpStoreLauncher
        lblLauncher.Text = frmLaunchConfiguration_lblLauncher
        btnOpenLaunchers.Text = frmLaunchConfiguration_btnOpenLaunchers
        lblGameID.Text = frmLaunchConfiguration_lblGameID
        grpOtherConfig.Text = frmLaunchConfiguration_grpOtherConfig
        lblExe.Text = frmLaunchConfiguration_lblExe
        lblArguments.Text = frmLaunchConfiguration_lblArguments
        chkNoArgs.Text = frmLaunchConfiguration_chkNoArgs
        grpCommand.Text = frmLaunchConfiguration_grpCommand
        btnSave.Text = frmLaunchConfiguration_btnSave
        btnSave.Image = Multi_Save
        btnCancel.Text = frmLaunchConfiguration_btnCancel
        btnCancel.Image = Multi_Cancel

        'Init Command Update Timer
        tmUpdateTimer = New Timer()
        tmUpdateTimer.Interval = 1000
        tmUpdateTimer.Enabled = False
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

    Private Function HandleDirty() As MsgBoxResult
        Dim oResult As MsgBoxResult

        oResult = mgrCommon.ShowMessage(App_ConfirmDirty, MsgBoxStyle.YesNoCancel)

        Select Case oResult
            Case MsgBoxResult.No
                IsDirty = False
        End Select

        Return oResult
    End Function

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            IsDirty = True
        End If
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                AddHandler DirectCast(ctl, TextBox).TextChanged, AddressOf DirtyCheck_ValueChanged
            ElseIf TypeOf ctl Is ComboBox Then
                AddHandler DirectCast(ctl, ComboBox).SelectedIndexChanged, AddressOf DirtyCheck_ValueChanged
            ElseIf TypeOf ctl Is CheckBox Then
                AddHandler DirectCast(ctl, CheckBox).CheckedChanged, AddressOf DirtyCheck_ValueChanged
            End If
        Next
    End Sub

    Private Sub LoadData()
        Dim oLaunchData As New clsLaunchData
        IsLoading = True
        oLaunchData = mgrLaunchData.DoLaunchDataGetbyID(Game.ID)
        LoadCombos()
        If oLaunchData.LauncherID = String.Empty Then
            cboLauncher.SelectedValue = "nolauncher"
        Else
            cboLauncher.SelectedValue = oLaunchData.LauncherID
        End If
        txtGameID.Text = oLaunchData.LauncherGameID
        txtExePath.Text = oLaunchData.Path
        txtArguments.Text = oLaunchData.Args
        chkNoArgs.Checked = oLaunchData.NoArgs
        IsLoading = False
    End Sub

    Private Sub UpdateLaunchCommand()
        Dim oLaunchData As New clsLaunchData
        Dim eLaunchType As mgrLaunchGame.eLaunchType
        Dim sErrorMessage As String = String.Empty

        'Build a object based on current values
        oLaunchData.MonitorID = Game.ID
        If cboLauncher.SelectedValue = "nolauncher" Then
            oLaunchData.LauncherID = String.Empty
        Else
            oLaunchData.LauncherID = cboLauncher.SelectedValue
        End If
        oLaunchData.LauncherGameID = txtGameID.Text
        oLaunchData.Path = txtExePath.Text
        oLaunchData.Args = txtArguments.Text
        oLaunchData.NoArgs = chkNoArgs.Checked

        If mgrLaunchGame.CanLaunchGame(Game, oLaunchData, eLaunchType, sErrorMessage) Then
            txtCommand.Text = mgrLaunchGame.GetLaunchCommand(Game, oLaunchData, eLaunchType)
        Else
            txtCommand.Text = sErrorMessage
        End If
    End Sub

    Private Function ValidateData() As Boolean
        Dim oLauncher As clsLauncher
        Dim sCheckString As String

        If cboLauncher.SelectedValue <> "nolauncher" Then
            oLauncher = mgrLaunchers.DoLauncherGetbyID(cboLauncher.SelectedValue)
            If oLauncher.IsUri Then
                sCheckString = oLauncher.LaunchString
            Else
                sCheckString = oLauncher.LaunchParameters
            End If
            If sCheckString.Contains("%ID%") And txtGameID.Text = String.Empty Then
                mgrCommon.ShowMessage(frmLaunchConfiguration_ErrorNoGameID, oLauncher.Name, MsgBoxStyle.Exclamation)
                txtGameID.Focus()
                Return False
            End If
        ElseIf Not txtExePath.Text = String.Empty Then
            txtExePath.Text = mgrPath.ValidatePath(txtExePath.Text)
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
        If cboLauncher.SelectedValue = "nolauncher" And txtGameID.Text = String.Empty And txtExePath.Text = String.Empty And txtArguments.Text = String.Empty And chkNoArgs.Checked = False Then
            mgrLaunchData.DoLaunchDataDelete(Game.ID)
            IsDirty = False
            Me.DialogResult = DialogResult.OK
        Else
            If ValidateData() Then
                oLaunchData = New clsLaunchData
                oLaunchData.MonitorID = Game.ID
                If cboLauncher.SelectedValue = "nolauncher" Then
                    oLaunchData.LauncherID = String.Empty
                Else
                    oLaunchData.LauncherID = cboLauncher.SelectedValue
                End If
                oLaunchData.LauncherGameID = txtGameID.Text
                oLaunchData.Path = txtExePath.Text
                oLaunchData.Args = txtArguments.Text
                oLaunchData.NoArgs = chkNoArgs.Checked
                mgrLaunchData.DoLaunchDataAddUpdate(oLaunchData)
                IsDirty = False
                Me.DialogResult = DialogResult.OK
            End If
        End If
    End Sub

    Private Sub ExeBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = txtExePath.Text
        Dim oExtensions As New SortedList
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            sCurrentPath = Path.GetDirectoryName(mgrPath.ValidatePath(txtExePath.Text))
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        oExtensions.Add(frmLaunchConfiguration_Batch, "*.bat")
        oExtensions.Add(frmLaunchConfiguration_Command, "*.cmd")
        oExtensions.Add(frmLaunchConfiguration_Executable, "*.exe")

        sNewPath = mgrCommon.OpenFileBrowser("LC_Exe", frmProcessManager_ChooseProcess, oExtensions, 3, sDefaultFolder, False)

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
        If cboLauncher.SelectedValue Is Nothing Then
            cboLauncher.SelectedValue = "nolauncher"
        End If
    End Sub

    Private Sub frmAdvancedConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        AssignDirtyHandlers(grpStoreLauncher.Controls)
        AssignDirtyHandlers(grpOtherConfig.Controls)
        UpdateLaunchCommand()
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

    Private Sub tmUpdateTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmUpdateTimer.Tick
        tmUpdateTimer.Stop()
        tmUpdateTimer.Enabled = False
        UpdateLaunchCommand()
    End Sub

    Private Sub Command_Change(sender As Object, e As EventArgs) Handles cboLauncher.SelectedIndexChanged, txtGameID.TextChanged, txtExePath.TextChanged, txtArguments.TextChanged, chkNoArgs.CheckedChanged
        If Not tmUpdateTimer.Enabled Then
            tmUpdateTimer.Enabled = True
            tmUpdateTimer.Start()
        End If
    End Sub

    Private Sub frmLaunchConfiguration_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveData()
                    If IsDirty Then e.Cancel = True
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub frmLaunchConfiguration_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class