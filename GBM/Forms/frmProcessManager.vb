Imports GBM.My.Resources
Imports System.IO

Public Class frmProcessManager
    Private Enum eModes As Integer
        View = 1
        Edit = 2
        Add = 3
        Disabled = 4
    End Enum

    Private eCurrentMode As eModes = eModes.Disabled
    Private oCurrentProcess As clsProcess

    Private Property IsDirty As Boolean = False
    Private Property IsLoading As Boolean = False
    Private Property ProcessData As Hashtable

    Private Sub ProcessBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String = txtPath.Text
        Dim sNewPath As String
        Dim oExtensions As New SortedList

        If sCurrentPath <> String.Empty Then
            sCurrentPath = Path.GetDirectoryName(txtPath.Text)
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        oExtensions.Add(frmProcessManager_Executable, "*.exe")
        sNewPath = mgrCommon.OpenFileBrowser("PM_Process", frmProcessManager_ChooseProcess, oExtensions, 1, sDefaultFolder, True)

        If sNewPath <> String.Empty Then
            txtPath.Text = sNewPath
        End If

    End Sub

    Private Sub LoadData()
        ProcessData = mgrProcess.ReadProcesses
        lstProcesses.Items.Clear()
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

        For Each oProcess As clsProcess In ProcessData.Values
            lstProcesses.Items.Add(oProcess.Name)
        Next

        IsLoading = False
    End Sub

    Private Sub FillData()
        IsLoading = True

        oCurrentProcess = DirectCast(ProcessData(lstProcesses.SelectedItems(0).ToString), clsProcess)

        txtID.Text = oCurrentProcess.ID
        txtName.Text = oCurrentProcess.Name
        txtPath.Text = oCurrentProcess.Path
        txtArguments.Text = oCurrentProcess.Args
        chkKillProcess.Checked = oCurrentProcess.Kill

        IsLoading = False
    End Sub

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            IsDirty = True
            If Not eCurrentMode = eModes.Add Then EditProcess()
        End If
    End Sub

    Private Sub AssignDirtyHandlers(ByVal oCtls As GroupBox.ControlCollection)
        For Each ctl As Control In oCtls
            If TypeOf ctl Is TextBox Then
                AddHandler DirectCast(ctl, TextBox).TextChanged, AddressOf DirtyCheck_ValueChanged
            ElseIf TypeOf ctl Is CheckBox Then
                AddHandler DirectCast(ctl, CheckBox).CheckedChanged, AddressOf DirtyCheck_ValueChanged
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
                grpProcess.Enabled = True
                WipeControls(grpProcess.Controls)
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                lstProcesses.Enabled = False
                chkKillProcess.Checked = False
            Case eModes.Edit
                lstProcesses.Enabled = False
                grpProcess.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
            Case eModes.View
                lstProcesses.Enabled = True
                grpProcess.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
            Case eModes.Disabled
                lstProcesses.Enabled = True
                WipeControls(grpProcess.Controls)
                grpProcess.Enabled = False
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
        End Select

        IsLoading = False
    End Sub

    Private Sub EditProcess()
        eCurrentMode = eModes.Edit
        ModeChange()
    End Sub

    Private Sub AddProcess()
        eCurrentMode = eModes.Add
        ModeChange()
        txtName.Focus()
    End Sub

    Private Sub CancelEdit()
        If IsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveProcess()
                Case MsgBoxResult.No
                    If lstProcesses.SelectedItems.Count > 0 Then
                        eCurrentMode = eModes.View
                        ModeChange()
                        FillData()
                        lstProcesses.Focus()
                    Else
                        eCurrentMode = eModes.Disabled
                        ModeChange()
                    End If
                Case MsgBoxResult.Cancel
                    'Do Nothing
            End Select
        Else
            If lstProcesses.SelectedItems.Count > 0 Then
                eCurrentMode = eModes.View
                ModeChange()
                FillData()
                lstProcesses.Focus()
            Else
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SaveProcess()
        Dim oProcess As New clsProcess
        Dim bSuccess As Boolean = False

        If txtID.Text <> String.Empty Then
            oProcess.ID = txtID.Text
        End If
        oProcess.Name = txtName.Text
        oProcess.Path = txtPath.Text
        oProcess.Args = txtArguments.Text
        oProcess.Kill = chkKillProcess.Checked

        Select Case eCurrentMode
            Case eModes.Add
                If CoreValidatation(oProcess) Then
                    bSuccess = True
                    mgrProcess.DoProcessAdd(oProcess)
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oProcess) Then
                    bSuccess = True
                    mgrProcess.DoProcessUpdate(oProcess)
                    eCurrentMode = eModes.View
                End If
        End Select

        If bSuccess Then
            IsDirty = False
            LoadData()
            ModeChange()
            If eCurrentMode = eModes.View Then lstProcesses.SelectedIndex = lstProcesses.Items.IndexOf(oProcess.Name)
        End If
    End Sub

    Private Sub DeleteProcess()
        Dim oProcess As clsProcess

        If lstProcesses.SelectedItems.Count > 0 Then
            oProcess = DirectCast(ProcessData(lstProcesses.SelectedItems(0).ToString), clsProcess)

            If mgrCommon.ShowMessage(frmProcessManager_ConfirmDelete, oProcess.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrProcess.DoProcessDelete(oProcess.ID)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SwitchProcess()
        If lstProcesses.SelectedItems.Count > 0 Then
            eCurrentMode = eModes.View
            FillData()
            ModeChange()
        End If
    End Sub

    Private Function CoreValidatation(ByVal oProcess As clsProcess) As Boolean
        If txtName.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmProcessManager_ErrorValidName, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        If txtPath.Text.Trim = String.Empty Then
            mgrCommon.ShowMessage(frmProcessManager_ErrorValidPath, MsgBoxStyle.Exclamation)
            txtPath.Focus()
            Return False
        Else
            If Not File.Exists(txtPath.Text.Trim) Then
                mgrCommon.ShowMessage(frmProcessManager_ErrorPathNotFound, MsgBoxStyle.Exclamation)
                txtPath.Focus()
                Return False
            End If
        End If

        If mgrProcess.DoCheckDuplicate(oProcess.Name, oProcess.ID) Then
            mgrCommon.ShowMessage(frmProcessManager_ErrorDupe, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmProcessManager_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        btnCancel.Text = frmProcessManager_btnCancel
        btnCancel.Image = Multi_Cancel
        btnSave.Text = frmProcessManager_btnSave
        btnSave.Image = Multi_Save
        grpProcess.Text = frmProcessManager_grpProcess
        btnProcessBrowse.Text = frmProcessManager_btnProcessBrowse
        lblProcess.Text = frmProcessManager_lblPath
        lblName.Text = frmProcessManager_lblName
        btnDelete.Text = frmProcessManager_btnDelete
        btnDelete.Image = Multi_Delete
        btnAdd.Text = frmProcessManager_btnAdd
        btnAdd.Image = Multi_Add
        chkKillProcess.Text = frmProcessManager_chkKillProcess
    End Sub

    Private Sub frmProcessManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        ModeChange()
        AssignDirtyHandlers(grpProcess.Controls)
    End Sub

    Private Sub lstProcesses_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstProcesses.SelectedIndexChanged
        SwitchProcess()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddProcess()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteProcess()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveProcess()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        CancelEdit()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnPathBrowse_Click(sender As Object, e As EventArgs) Handles btnProcessBrowse.Click
        ProcessBrowse()
    End Sub

    Private Sub frmProcessManager_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveProcess()
                    If IsDirty Then e.Cancel = True
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub frmProcessManager_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Select Case eCurrentMode
                    Case eModes.Add, eModes.Edit
                        btnCancel.PerformClick()
                    Case eModes.Disabled, eModes.View
                        Me.Close()
                End Select
        End Select
    End Sub
End Class