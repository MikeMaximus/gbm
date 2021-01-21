Imports GBM.My.Resources

Public Class frmTags

    Dim hshTagData As Hashtable
    Private bIsDirty As Boolean = False
    Private bIsLoading As Boolean = False
    Private oCurrentTag As clsTag

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

    Private Property TagData As Hashtable
        Get
            Return hshTagData
        End Get
        Set(value As Hashtable)
            hshTagData = value
        End Set
    End Property

    Private Sub LoadData()
        TagData = mgrTags.ReadTags
        lstTags.Items.Clear()
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

        For Each oTag As clsTag In TagData.Values
            lstTags.Items.Add(oTag.Name)
        Next

        IsLoading = False
    End Sub

    Private Sub FillData()
        IsLoading = True

        oCurrentTag = DirectCast(TagData(lstTags.SelectedItems(0).ToString), clsTag)

        txtID.Text = oCurrentTag.ID
        txtName.Text = oCurrentTag.Name

        IsLoading = False
    End Sub

    Private Sub DirtyCheck_ValueChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            IsDirty = True
            If Not eCurrentMode = eModes.Add Then EditTag()
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
                grpTag.Enabled = True
                WipeControls(grpTag.Controls)
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
                lstTags.Enabled = False
            Case eModes.Edit
                lstTags.Enabled = False
                grpTag.Enabled = True
                btnSave.Enabled = True
                btnCancel.Enabled = True
                btnAdd.Enabled = False
                btnDelete.Enabled = False
            Case eModes.View
                lstTags.Enabled = True
                grpTag.Enabled = True
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
            Case eModes.Disabled
                lstTags.Enabled = True
                WipeControls(grpTag.Controls)
                grpTag.Enabled = False
                btnSave.Enabled = False
                btnCancel.Enabled = False
                btnAdd.Enabled = True
                btnDelete.Enabled = True
        End Select

        IsLoading = False
    End Sub

    Private Sub EditTag()
        eCurrentMode = eModes.Edit
        ModeChange()
    End Sub

    Private Sub AddTag()
        eCurrentMode = eModes.Add
        ModeChange()
        txtName.Focus()
    End Sub

    Private Sub CancelEdit()
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveTag()
                Case MsgBoxResult.No
                    If lstTags.SelectedItems.Count > 0 Then
                        eCurrentMode = eModes.View
                        ModeChange()
                        FillData()
                        lstTags.Focus()
                    Else
                        eCurrentMode = eModes.Disabled
                        ModeChange()
                    End If
                Case MsgBoxResult.Cancel
                    'Do Nothing
            End Select
        Else
            If lstTags.SelectedItems.Count > 0 Then
                eCurrentMode = eModes.View
                ModeChange()
                FillData()
                lstTags.Focus()
            Else
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SaveTag()
        Dim oTag As New clsTag
        Dim bSuccess As Boolean = False

        If txtID.Text <> String.Empty Then
            oTag.ID = txtID.Text
        End If
        oTag.Name = txtName.Text

        Select Case eCurrentMode
            Case eModes.Add
                If CoreValidatation(oTag) Then
                    bSuccess = True
                    mgrTags.DoTagAdd(oTag)
                    eCurrentMode = eModes.View
                End If
            Case eModes.Edit
                If CoreValidatation(oTag) Then
                    bSuccess = True
                    mgrTags.DoTagUpdate(oTag)
                    eCurrentMode = eModes.View
                End If
        End Select

        If bSuccess Then
            IsDirty = False
            LoadData()
            ModeChange()
            If eCurrentMode = eModes.View Then lstTags.SelectedIndex = lstTags.Items.IndexOf(oTag.Name)
        End If
    End Sub

    Private Sub DeleteTag()
        Dim oTag As clsTag

        If lstTags.SelectedItems.Count > 0 Then
            oTag = DirectCast(TagData(lstTags.SelectedItems(0).ToString), clsTag)

            If mgrCommon.ShowMessage(frmTags_ConfirmDelete, oTag.Name, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                mgrTags.DoTagDelete(oTag.ID)
                LoadData()
                eCurrentMode = eModes.Disabled
                ModeChange()
            End If
        End If
    End Sub

    Private Sub SwitchTag()
        If lstTags.SelectedItems.Count > 0 Then
            eCurrentMode = eModes.View
            FillData()
            ModeChange()
        End If
    End Sub

    Private Function CoreValidatation(ByVal oTag As clsTag) As Boolean
        If txtName.Text = String.Empty Then
            mgrCommon.ShowMessage(frmTags_ErrorValidName, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        If mgrTags.DoCheckDuplicate(oTag.Name, oTag.ID) Then
            mgrCommon.ShowMessage(frmTags_ErrorTagDupe, MsgBoxStyle.Exclamation)
            txtName.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmTags_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        grpTag.Text = frmTags_grpTag
        lblName.Text = frmTags_lblName
        btnDelete.Text = frmTags_btnDelete
        btnDelete.Image = Multi_Delete
        btnAdd.Text = frmTags_btnAdd
        btnAdd.Image = Multi_Add
        btnCancel.Text = frmTags_btnCancel
        btnCancel.Image = Multi_Cancel
        btnSave.Text = frmTags_btnSave
        btnSave.Image = Multi_Save
    End Sub

    Private Sub frmTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        ModeChange()
        AssignDirtyHandlers(grpTag.Controls)
    End Sub

    Private Sub lstTags_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstTags.SelectedIndexChanged
        SwitchTag()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddTag()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteTag()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveTag()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        CancelEdit()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub frmTags_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If bIsDirty Then
            Select Case HandleDirty()
                Case MsgBoxResult.Yes
                    SaveTag()
                    If bIsDirty Then e.Cancel = True
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub
End Class