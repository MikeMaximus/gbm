Imports GBM.My.Resources

Public Class frmFilter
    Public Enum eFilterType As Integer
        BaseFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
    End Enum

    Dim oValidFields As New List(Of clsGameFilterField)
    Dim hshTags As New Hashtable

    Public Property GameFilters As List(Of clsGameFilter) = New List(Of clsGameFilter)
    Public Property IncludeTagFilters As List(Of clsTag) = New List(Of clsTag)
    Public Property ExcludeTagFilters As List(Of clsTag) = New List(Of clsTag)
    Public Property FilterType As eFilterType = eFilterType.BaseFilter
    Public Property AndOperator As Boolean = False
    Public Property SortAsc As Boolean = True
    Public Property SortField As String = "Name"

    Private Sub AddTag(ByRef lst As ListBox)
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))

        If lstTags.SelectedItems.Count = 1 Then
            oData = lstTags.SelectedItems(0)
            lst.Items.Add(oData)
            lstTags.Items.Remove(oData)
        ElseIf lstTags.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstTags.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lst.Items.Add(kp)
                lstTags.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveTag(ByRef lst As ListBox)
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))

        If lst.SelectedItems.Count = 1 Then
            oData = lst.SelectedItems(0)
            lst.Items.Remove(oData)
            lstTags.Items.Add(oData)
        ElseIf lst.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lst.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lst.Items.Remove(kp)
                lstTags.Items.Add(kp)
            Next
        End If

    End Sub

    Private Sub LoadFilterFields()
        Dim oField As clsGameFilterField


        'Game ID
        oField = New clsGameFilterField
        oField.FieldName = "MonitorID"
        oField.FriendlyFieldName = frmFilter_FieldGameID
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)


        'Name
        oField = New clsGameFilterField
        oField.FieldName = "Name"
        oField.FriendlyFieldName = frmFilter_FieldName
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Process
        oField = New clsGameFilterField
        oField.FieldName = "Process"
        oField.FriendlyFieldName = frmFilter_FieldProcess
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Parameter
        oField = New clsGameFilterField
        oField.FieldName = "Parameter"
        oField.FriendlyFieldName = frmFilter_FieldParameter
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Save Path
        oField = New clsGameFilterField
        oField.FieldName = "Path"
        oField.FriendlyFieldName = frmFilter_FieldPath
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Include Items
        oField = New clsGameFilterField
        oField.FieldName = "FileType"
        oField.FriendlyFieldName = frmFilter_FieldFileType
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Exclude Items
        oField = New clsGameFilterField
        oField.FieldName = "ExcludeList"
        oField.FriendlyFieldName = frmFilter_FieldExcludeList
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Save Entire Folder
        oField = New clsGameFilterField
        oField.FieldName = "FolderSave"
        oField.FriendlyFieldName = frmFilter_FieldFolderSave
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Include Sub Folders
        oField = New clsGameFilterField
        oField.FieldName = "RecurseSubFolders"
        oField.FriendlyFieldName = frmFilter_FieldRecurseSubFolders
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Delete Folder on Restore
        oField = New clsGameFilterField
        oField.FieldName = "CleanFolder"
        oField.FriendlyFieldName = frmFilter_FieldCleanFolder
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Save Multiple Backups
        oField = New clsGameFilterField
        oField.FieldName = "TimeStamp"
        oField.FriendlyFieldName = frmFilter_FieldTimeStamp
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Backup Limit
        oField = New clsGameFilterField
        oField.FieldName = "BackupLimit"
        oField.FriendlyFieldName = frmFilter_FieldBackupLimit
        oField.Type = clsGameFilterField.eDataType.fNumeric
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Differential backup
        oField = New clsGameFilterField
        oField.FieldName = "Differential"
        oField.FriendlyFieldName = frmFilter_FieldDifferential
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Full backup interval
        oField = New clsGameFilterField
        oField.FieldName = "DiffInterval"
        oField.FriendlyFieldName = frmFilter_FieldDiffInterval
        oField.Type = clsGameFilterField.eDataType.fNumeric
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Timed interval backup
        oField = New clsGameFilterField
        oField.FieldName = "TimedBackup"
        oField.FriendlyFieldName = frmFilter_FieldTimedBackup
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Timed interval backup (minutes)
        oField = New clsGameFilterField
        oField.FieldName = "TimedInterval"
        oField.FriendlyFieldName = frmFilter_FieldTimedInterval
        oField.Type = clsGameFilterField.eDataType.fNumeric
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Comments
        oField = New clsGameFilterField
        oField.FieldName = "Comments"
        oField.FriendlyFieldName = frmFilter_FieldComments
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'OS
        oField = New clsGameFilterField
        oField.FieldName = "OS"
        oField.FriendlyFieldName = frmFilter_FieldOS
        oField.Type = clsGameFilterField.eDataType.fEnum
        oField.EnumField = clsGameFilterField.eEnumFilterField.OS
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'IsRegEx
        oField = New clsGameFilterField
        oField.FieldName = "IsRegEx"
        oField.FriendlyFieldName = frmFilter_FieldIsRegEx
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Game Path
        oField = New clsGameFilterField
        oField.FieldName = "ProcessPath"
        oField.FriendlyFieldName = frmFilter_FieldProcessPath
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Company
        oField = New clsGameFilterField
        oField.FieldName = "Company"
        oField.FriendlyFieldName = frmFilter_FieldCompany
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Version
        oField = New clsGameFilterField
        oField.FieldName = "Version"
        oField.FriendlyFieldName = frmFilter_FieldVersion
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Icon
        oField = New clsGameFilterField
        oField.FieldName = "Icon"
        oField.FriendlyFieldName = frmFilter_FieldIcon
        oField.Type = clsGameFilterField.eDataType.fString
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Hours
        oField = New clsGameFilterField
        oField.FieldName = "Hours"
        oField.FriendlyFieldName = frmFilter_FieldHours
        oField.Type = clsGameFilterField.eDataType.fNumeric
        oField.Status = clsGameFilterField.eFieldStatus.ValidSort
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Enabled
        oField = New clsGameFilterField
        oField.FieldName = "Enabled"
        oField.FriendlyFieldName = frmFilter_FieldEnabled
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

        'Monitor Only
        oField = New clsGameFilterField
        oField.FieldName = "MonitorOnly"
        oField.FriendlyFieldName = frmFilter_FieldMonitorOnly
        oField.Type = clsGameFilterField.eDataType.fBool
        oField.Status = clsGameFilterField.eFieldStatus.ValidFilter
        oValidFields.Add(oField)

    End Sub

    Private Sub LoadTagData()
        Dim oTag As clsTag
        Dim oData As KeyValuePair(Of String, String)

        'Handle Data
        hshTags = mgrTags.ReadTags()

        'Handle Lists
        lstTags.Items.Clear()
        lstIncludeTags.Items.Clear()
        lstExcludeTags.Items.Clear()

        lstTags.ValueMember = "Key"
        lstTags.DisplayMember = "Value"

        lstIncludeTags.ValueMember = "Key"
        lstIncludeTags.DisplayMember = "Value"

        lstExcludeTags.ValueMember = "Key"
        lstExcludeTags.DisplayMember = "Value"

        For Each de As DictionaryEntry In hshTags
            oTag = DirectCast(de.Value, clsTag)
            oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
            lstTags.Items.Add(oData)
        Next

    End Sub

    Private Sub LoadExistingFilters()
        Dim sFilter As String = String.Empty
        Dim oListTag As KeyValuePair(Of String, String)

        'Game Filters
        If AndOperator Then
            optAnd.Checked = True
        Else
            optOr.Checked = True
        End If

        If GameFilters.Count > 0 Then
            chkGameInfo.Checked = True
            For Each oFilter As clsGameFilter In GameFilters
                Select Case oFilter.Field.Type
                    Case clsGameFilterField.eDataType.fString
                        sFilter = oFilter.Field.FriendlyFieldName & " " & frmFilter_lstFilterContains & " """ & oFilter.Data & """"
                    Case clsGameFilterField.eDataType.fNumeric
                        sFilter = oFilter.Field.FriendlyFieldName & " " & oFilter.NumericOperatorAsString & " " & oFilter.Data
                    Case clsGameFilterField.eDataType.fBool, clsGameFilterField.eDataType.fEnum
                        sFilter = oFilter.Field.FriendlyFieldName & " = " & oFilter.Data
                End Select

                If oFilter.NotCondition Then
                    sFilter &= " (" & frmFilter_lblNot & ")"
                End If

                lstFilter.Items.Add(New KeyValuePair(Of clsGameFilter, String)(oFilter, sFilter))
            Next
        End If

        'Tag Filters
        If IncludeTagFilters.Count > 0 Then
            chkTag.Checked = True
            For Each oTag As clsTag In IncludeTagFilters
                oListTag = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
                lstIncludeTags.Items.Add(oListTag)
                lstTags.Items.Remove(oListTag)
            Next

            If FilterType = eFilterType.AllTags Then
                optAll.Checked = True
            Else
                optAny.Checked = True
            End If
        End If

        If ExcludeTagFilters.Count > 0 Then
            chkTag.Checked = True
            For Each oTag As clsTag In ExcludeTagFilters
                oListTag = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
                lstExcludeTags.Items.Add(oListTag)
                lstTags.Items.Remove(oListTag)
            Next

            If FilterType = eFilterType.AllTags Then
                optAll.Checked = True
            Else
                optAny.Checked = True
            End If
        End If

        'Sorting
        cboSortField.SelectedValue = SortField
        If SortAsc Then
            optSortAsc.Checked = True
        Else
            optSortDesc.Checked = True
        End If

    End Sub

    Private Sub ChangeFilterMode()
        Dim oFilter As clsGameFilterField = DirectCast(cboFilterField.SelectedValue, clsGameFilterField)

        'Reset
        cboNumericOps.SelectedIndex = 0
        numFilter.Value = 0
        txtStringFilter.Text = String.Empty
        chkNot.Checked = False

        'Reset Visibilty
        cboListFilter.Visible = False
        cboNumericOps.Visible = False
        numFilter.Visible = False
        txtStringFilter.Visible = False

        'Set Visiblity
        Select Case oFilter.Type
            Case clsGameFilterField.eDataType.fString
                txtStringFilter.Visible = True
            Case clsGameFilterField.eDataType.fNumeric
                cboNumericOps.Visible = True
                numFilter.Visible = True
                txtStringFilter.Visible = False
            Case clsGameFilterField.eDataType.fBool
                LoadComboAsBool()
                cboListFilter.SelectedIndex = 0
                cboListFilter.Visible = True
            Case clsGameFilterField.eDataType.fEnum
                LoadComboAsEnum(oFilter.EnumField)
                cboListFilter.SelectedIndex = 0
                cboListFilter.Visible = True
        End Select

    End Sub

    Private Sub AddFilter()
        Dim oFilter As New clsGameFilter
        Dim sFilter As String = String.Empty

        'Build Filter
        oFilter.ID = Guid.NewGuid.ToString.Split("-")(0)
        oFilter.Field = cboFilterField.SelectedValue

        Select Case oFilter.Field.Type
            Case clsGameFilterField.eDataType.fString
                oFilter.Data = txtStringFilter.Text
                sFilter = oFilter.Field.FriendlyFieldName & " " & frmFilter_lstFilterContains & " """ & oFilter.Data & """"
            Case clsGameFilterField.eDataType.fNumeric
                oFilter.Data = numFilter.Value
                oFilter.NumericOperator = DirectCast(cboNumericOps.SelectedValue, clsGameFilter.eNumericOperators)
                sFilter = oFilter.Field.FriendlyFieldName & " " & oFilter.NumericOperatorAsString & " " & oFilter.Data
            Case clsGameFilterField.eDataType.fBool, clsGameFilterField.eDataType.fEnum
                oFilter.Data = cboListFilter.SelectedValue
                sFilter = oFilter.Field.FriendlyFieldName & " = " & oFilter.Data
        End Select

        If chkNot.Checked Then
            oFilter.NotCondition = True
            sFilter &= " (" & frmFilter_lblNot & ")"
        Else
            oFilter.NotCondition = False
        End If

        GameFilters.Add(oFilter)
        lstFilter.Items.Add(New KeyValuePair(Of clsGameFilter, String)(oFilter, sFilter))

    End Sub

    Private Sub RemoveFilter()
        Dim oFilter As Object

        If lstFilter.SelectedIndex <> -1 Then
            oFilter = lstFilter.SelectedItem
            GameFilters.Remove(DirectCast(oFilter, KeyValuePair(Of clsGameFilter, String)).Key)
            lstFilter.Items.Remove(oFilter)
        End If

    End Sub

    Private Sub GetFilters()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTag As clsTag


        If chkGameInfo.Checked Then
            'Set Filter Type(s)
            FilterType = eFilterType.BaseFilter
            AndOperator = optAnd.Checked
        End If

        If chkTag.Checked Then
            'Set Tags
            IncludeTagFilters.Clear()
            For Each oData In lstIncludeTags.Items
                oTag = DirectCast(hshTags(oData.Value), clsTag)
                IncludeTagFilters.Add(oTag)
            Next
            ExcludeTagFilters.Clear()
            For Each oData In lstExcludeTags.Items
                oTag = DirectCast(hshTags(oData.Value), clsTag)
                ExcludeTagFilters.Add(oTag)
            Next

            'Set Filter Type
            If IncludeTagFilters.Count = 0 And ExcludeTagFilters.Count = 0 Then
                FilterType = eFilterType.NoTags
            ElseIf optAll.Checked Then
                FilterType = eFilterType.AllTags
            Else
                FilterType = eFilterType.AnyTag
            End If
        End If

        'Sorting
        If optSortAsc.Checked Then
            SortAsc = True
        Else
            SortAsc = False
        End If

        SortField = cboSortField.SelectedValue

    End Sub

    Private Sub LoadComboAsBool()
        Dim oBoolOperators As New List(Of KeyValuePair(Of Boolean, String))

        'cboListFilter (Boolean)
        cboListFilter.ValueMember = "Key"
        cboListFilter.DisplayMember = "Value"

        oBoolOperators.Add(New KeyValuePair(Of Boolean, String)(True, frmFilter_cboBoolFilterEnabled))
        oBoolOperators.Add(New KeyValuePair(Of Boolean, String)(False, frmFilter_cboBoolFilterDisabled))

        cboListFilter.DataSource = oBoolOperators
    End Sub

    Private Sub LoadComboAsEnum(ByVal eEnum As clsGameFilterField.eEnumFilterField)
        Dim oEnums As New List(Of KeyValuePair(Of Integer, String))

        'cboListFilter (Enum)
        cboListFilter.ValueMember = "Key"
        cboListFilter.DisplayMember = "Value"

        Select Case eEnum
            Case clsGameFilterField.eEnumFilterField.OS
                For Each v As Object In [Enum].GetValues(GetType(clsGame.eOS))
                    oEnums.Add(New KeyValuePair(Of Integer, String)(v, [Enum].GetName(GetType(clsGame.eOS), v)))
                Next
        End Select

        cboListFilter.DataSource = oEnums
    End Sub

    Private Sub LoadCombos()
        Dim oFilterFields As New List(Of KeyValuePair(Of clsGameFilterField, String))
        Dim oSortFields As New List(Of KeyValuePair(Of String, String))
        Dim oNumericOperators As New List(Of KeyValuePair(Of clsGameFilter.eNumericOperators, String))

        'cboNumericOps
        cboNumericOps.ValueMember = "Key"
        cboNumericOps.DisplayMember = "Value"

        oNumericOperators.Add(New KeyValuePair(Of clsGameFilter.eNumericOperators, String)(clsGameFilter.eNumericOperators.Equals, "="))
        oNumericOperators.Add(New KeyValuePair(Of clsGameFilter.eNumericOperators, String)(clsGameFilter.eNumericOperators.Greater, ">"))
        oNumericOperators.Add(New KeyValuePair(Of clsGameFilter.eNumericOperators, String)(clsGameFilter.eNumericOperators.Lesser, "<"))
        oNumericOperators.Add(New KeyValuePair(Of clsGameFilter.eNumericOperators, String)(clsGameFilter.eNumericOperators.GreaterEquals, ">="))
        oNumericOperators.Add(New KeyValuePair(Of clsGameFilter.eNumericOperators, String)(clsGameFilter.eNumericOperators.LesserEquals, "<="))

        cboNumericOps.DataSource = oNumericOperators

        'cboFilterField
        cboFilterField.ValueMember = "Key"
        cboFilterField.DisplayMember = "Value"

        For Each oField As clsGameFilterField In oValidFields
            If oField.CheckStatus(clsGameFilterField.eFieldStatus.ValidFilter) Then
                oFilterFields.Add(New KeyValuePair(Of clsGameFilterField, String)(oField, oField.FriendlyFieldName))
            End If
        Next

        cboFilterField.DataSource = oFilterFields

        'cboSortField
        cboSortField.ValueMember = "Key"
        cboSortField.DisplayMember = "Value"

        For Each oField As clsGameFilterField In oValidFields
            If oField.CheckStatus(clsGameFilterField.eFieldStatus.ValidSort) Then
                oSortFields.Add(New KeyValuePair(Of String, String)(oField.FieldName, oField.FriendlyFieldName))
            End If
        Next

        cboSortField.DataSource = oSortFields

        'Select Defaults
        cboNumericOps.SelectedIndex = 0
        cboFilterField.SelectedIndex = 0
        cboSortField.SelectedIndex = 0
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmFilter_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        optOr.Text = frmFilter_optOr
        optAnd.Text = frmFilter_optAnd
        grpFilterType.Text = frmFilter_grpFilterType
        optAll.Text = frmFilter_optAll
        optAny.Text = frmFilter_optAny
        lblIncludeTags.Text = frmFilter_lblIncludeTags
        lblExcludeTags.Text = frmFilter_lblExcludeTags
        lblTags.Text = frmFilter_lblTags
        btnIncludeRemove.Text = frmFilter_btnIncludeRemove
        btnIncludeAdd.Text = frmFilter_btnIncludeAdd
        btnExcludeRemove.Text = frmFilter_btnExcludeRemove
        btnExcludeAdd.Text = frmFilter_btnExcludeAdd
        btnOK.Text = frmFilter_btnOK
        btnOK.Image = Multi_Ok
        grpTagOptions.Text = frmFilter_grpTagOptions
        chkTag.Text = frmFilter_chkTag
        chkGameInfo.Text = frmFilter_chkGameInfo
        grpSorting.Text = frmFilter_grpSorting
        lblSortFields.Text = frmFilter_lblSortsFields
        optSortAsc.Text = frmFilter_optSortAsc
        optSortDesc.Text = frmFilter_optSortDesc
        btnAddFilter.Text = frmFilter_btnAddFilter
        btnAddFilter.Image = Multi_Add
        btnRemoveFilter.Text = frmFilter_btnRemoveFilter
        btnRemoveFilter.Image = Multi_Delete
        lblCurrentFilters.Text = frmFilter_lblCurrentFilters
        lblFields.Text = frmFilter_lblFields
        lblFilterData.Text = frmFilter_lblFilterData
        grpSortOptions.Text = frmFilter_grpSortOptions

        'Defaults
        optOr.Checked = True
        optSortAsc.Checked = True
        grpGameFilter.Enabled = False
        grpTagFilter.Enabled = False

        'Init Game Filter        
        lstFilter.ValueMember = "Key"
        lstFilter.DisplayMember = "Value"
    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadFilterFields()
        LoadCombos()
        LoadTagData()
        LoadExistingFilters()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub btnIncludeAdd_Click(sender As Object, e As EventArgs) Handles btnIncludeAdd.Click
        AddTag(lstIncludeTags)
    End Sub

    Private Sub btnExcludeAdd_Click(sender As Object, e As EventArgs) Handles btnExcludeAdd.Click
        AddTag(lstExcludeTags)
    End Sub

    Private Sub btnExcludeRemove_Click(sender As Object, e As EventArgs) Handles btnExcludeRemove.Click
        RemoveTag(lstExcludeTags)
    End Sub

    Private Sub btnIncludeRemove_Click(sender As Object, e As EventArgs) Handles btnIncludeRemove.Click
        RemoveTag(lstIncludeTags)
    End Sub

    Private Sub frmFilter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        GetFilters()
    End Sub

    Private Sub chkGameInfo_CheckedChanged(sender As Object, e As EventArgs) Handles chkGameInfo.CheckedChanged
        If chkGameInfo.Checked Then
            grpGameFilter.Enabled = True
        Else
            optOr.Checked = True
            grpGameFilter.Enabled = False
            GameFilters.Clear()
            lstFilter.Items.Clear()
        End If
    End Sub

    Private Sub chkTag_CheckedChanged(sender As Object, e As EventArgs) Handles chkTag.CheckedChanged
        If chkTag.Checked Then
            grpTagFilter.Enabled = True
        Else
            grpTagFilter.Enabled = False
            IncludeTagFilters.Clear()
            LoadTagData()
        End If
    End Sub

    Private Sub btnAddFilter_Click(sender As Object, e As EventArgs) Handles btnAddFilter.Click
        AddFilter()
    End Sub

    Private Sub btnRemoveFilter_Click(sender As Object, e As EventArgs) Handles btnRemoveFilter.Click
        RemoveFilter()
    End Sub

    Private Sub cboFilterField_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilterField.SelectedIndexChanged
        ChangeFilterMode()
    End Sub

    Private Sub frmFilter_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnOK.PerformClick()
        End Select
    End Sub
End Class