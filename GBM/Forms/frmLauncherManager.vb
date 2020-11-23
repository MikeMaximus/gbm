Imports GBM.My.Resources

Public Class frmLauncherManager
    Dim hshLauncherData As Hashtable
    Private bIsDirty As Boolean = False
    Private bIsLoading As Boolean = False
    Private oCurrentLauncher As clsLauncher

    Private Property IsDirty As Boolean
        Get
            Return bIsDirty
        End Get
        Set(value As Boolean)
            bIsDirty = value
        End Set
    End Property

    Private Property IsLoading As Boolean
        Get
            Return bIsLoading
        End Get
        Set(value As Boolean)
            bIsLoading = value
        End Set
    End Property

    Private Enum eModes As Integer
        View = 1
        Edit = 2
        Add = 3
        Disabled = 4
    End Enum

    Private eCurrentMode As eModes = eModes.Disabled

    Private Property LauncherData As Hashtable
        Get
            Return hshLauncherData
        End Get
        Set(value As Hashtable)
            hshLauncherData = value
        End Set
    End Property

    Private Sub LoadData()
        LauncherData = mgrLaunchers.ReadLaunchers
        lstLaunchers.Items.Clear()
        FormatAndFillList()
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

    Private Sub FormatAndFillList()
        IsLoading = True

        For Each oLauncher As clsLauncher In LauncherData.Values
            lstLaunchers.Items.Add(oLauncher.Name)
        Next

        IsLoading = False
    End Sub

    Private Sub FillData()
        IsLoading = True

        oCurrentLauncher = DirectCast(LauncherData(lstLaunchers.SelectedItems(0).ToString), clsLauncher)

        txtID.Text = oCurrentLauncher.LauncherID
        txtName.Text = oCurrentLauncher.Name
        txtLaunchString.Text = oCurrentLauncher.LaunchString

        IsLoading = False
    End Sub

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            IsDirty = True
            If Not eCurrentMode = eModes.Add Then EditLauncher()
        End If
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                AddHandler DirectCast(ctl, TextBox).TextChanged, AddressOf DirtyCheck_ValueChanged
            End If
        Next
    End Sub

    Private Sub WipeControls(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Text = String.Empty
            End If
        Next
        txtID.Text = String.Empty
    End Sub

    Private Sub ModeChange()
        IsLoading = True

        Select Case eCurrentMode
            Case eModes.Add
                grpLauncher.Enabled = True
                WipeControls(grpLauncher.Controls)
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                lstLaunchers.Enabled = False
            Case eModes.Edit
                lstLaunchers.Enabled = False
                grpLauncher.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
            Case eModes.View
                lstLaunchers.Enabled = True
                grpLauncher.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
            Case eModes.Disabled
                lstLaunchers.Enabled = True
                WipeControls(grpLauncher.Controls)
                grpLauncher.Enabled = False
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
        End Select

        IsLoading = False
    End Sub

    Private Sub EditLauncher()
        eCurrentMode = eModes.Edit
        ModeChange()
    End Sub

    Private Sub AddLauncher()
        eCurrentMode = eModes.Add
        ModeChange()
        txtName.Focus()
    End Sub

    Private Sub CancelEdit()
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveLauncher()
                Case MsgBoxResult.No
                    If lstLaunchers.SelectedItems.Count > 0 Then
                        eCurrentMode = eModes.View
                        ModeChange()
                        FillData()
                        lstLaunchers.Focus()
                    Else
                        eCurrentMode = eModes.Disabled
                        ModeChange()
                    End If
                Case MsgBoxResult.Cancel
                    'Do Nothing
            End Select
        Else
            If lstLaunchers.SelectedItems.Count > 0 Then
                eCurrentMode = eModes.View
                ModeChange()
                FillData()
                lstLaunchers.Focus()
            Else
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SaveLauncher()
        Dim oLauncher As New clsLauncher
        Dim bSuccess As Boolean = False

        If txtID.Text <> String.Empty Then
            oLauncher.LauncherID = txtID.Text
        End If
        oLauncher.Name = txtName.Text
        oLauncher.LaunchString = txtLaunchString.Text

        Select Case eCurrentMode
            Case eModes.Add
                If CoreValidatation(oLauncher) Then
                    bSuccess = True
                    mgrLaunchers.DoLauncherAdd(oLauncher)
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oLauncher) Then
                    bSuccess = True
                    mgrLaunchers.DoLauncherUpdate(oLauncher)
                    eCurrentMode = eModes.View
                End If
        End Select

        If bSuccess Then
            IsDirty = False
            LoadData()
            ModeChange()
            If eCurrentMode = eModes.View Then lstLaunchers.SelectedIndex = lstLaunchers.Items.IndexOf(oLauncher.Name)
        End If
    End Sub

    Private Sub DeleteLauncher()
        Dim oLauncher As clsLauncher

        If lstLaunchers.SelectedItems.Count > 0 Then
            oLauncher = DirectCast(LauncherData(lstLaunchers.SelectedItems(0).ToString), clsLauncher)

            If mgrCommon.ShowMessage(frmLauncherManager_ConfirmDelete, oLauncher.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrLaunchers.DoLauncherDelete(oLauncher.LauncherID)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SwitchLauncher()
        If lstLaunchers.SelectedItems.Count > 0 Then
            eCurrentMode = eModes.View
            FillData()
            ModeChange()
        End If
    End Sub

    Private Function CoreValidatation(ByVal oLauncher As clsLauncher) As Boolean
        If txtName.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmLauncherManager_ErrorValidName, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        If txtLaunchString.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmLauncherManager_ErrorValidCommand, MsgBoxStyle.Exclamation)
            txtLaunchString.Focus()
            Return False
        Else
            If Not mgrCommon.IsURI(txtLaunchString.Text) Then
                mgrCommon.ShowMessage(frmLauncherManager_ErrorInvalidURI, MsgBoxStyle.Exclamation)
                txtLaunchString.Focus()
                Return False
            End If
            If Not txtLaunchString.Text.Contains("%ID%") Then
                mgrCommon.ShowMessage(frmLauncherManager_WarningMissingVariable, MsgBoxStyle.Information)
            End If
        End If

        If mgrLaunchers.DoCheckDuplicate(oLauncher.Name, oLauncher.LauncherID) Then
            mgrCommon.ShowMessage(frmLauncherManager_ErrorDupe, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmLauncherManager_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        btnCancel.Text = frmLauncherManager_btnCancel
        btnSave.Text = frmLauncherManager_btnSave
        grpLauncher.Text = frmLauncherManager_grpLauncher
        lblCommand.Text = frmLauncherManager_lblCommand
        lblName.Text = frmLauncherManager_lblName
        btnClose.Text = frmLauncherManager_btnClose
        btnDelete.Text = frmLauncherManager_btnDelete
        btnAdd.Text = frmLauncherManager_btnAdd
    End Sub

    Private Sub frmLauncherManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        ModeChange()
        AssignDirtyHandlers(grpLauncher.Controls)
    End Sub

    Private Sub lstLauncheres_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLaunchers.SelectedIndexChanged
        SwitchLauncher()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddLauncher()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteLauncher()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveLauncher()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        CancelEdit()
    End Sub

    Private Sub btnAddDefaults_Click(sender As Object, e As EventArgs) Handles btnAddDefaults.Click
        If mgrCommon.ShowMessage(frmLauncherManager_ConfirmAddDefaults, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            mgrLaunchers.AddDefaultLaunchers()
            LoadData()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmLauncherManager_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveLauncher()
                    If bIsDirty Then e.Cancel = True
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub
End Class