Imports GBM.My.Resources

Public Class frmFilter

    Public Enum eFilterType As Integer
        BaseFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
    End Enum

    Dim oTagFilters As New List(Of clsTag)
    Dim oGameFilters As New List(Of clsGameFilter)
    Dim oValidFields As New List(Of clsGameFilterField)
    Dim eCurrentFilterType As eFilterType = eFilterType.BaseFilter
    Dim bSortAsc As Boolean = True
    Dim sSortField As String = "Name"
    Dim hshTags As New Hashtable
    Dim bShutdown As Boolean = False
    Dim iParameterIndex As Integer = 0

    Public Property GameFilters As List(Of clsGameFilter)
        Get
            Return oGameFilters
        End Get
        Set(value As List(Of clsGameFilter))
            oGameFilters = value
        End Set
    End Property

    Public Property TagFilters As List(Of clsTag)
        Get
            Return oTagFilters
        End Get
        Set(value As List(Of clsTag))
            oTagFilters = value
        End Set
    End Property

    Public Property FilterType As eFilterType
        Get
            Return eCurrentFilterType
        End Get
        Set(value As eFilterType)
            eCurrentFilterType = value
        End Set
    End Property

    Public Property SortAsc As Boolean
        Get
            Return bSortAsc
        End Get
        Set(value As Boolean)
            bSortAsc = value
        End Set
    End Property

    Public Property SortField As String
        Get
            Return sSortField
        End Get
        Set(value As String)
            sSortField = value
        End Set
    End Property

    Private Sub AddTag()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))

        If lstTags.SelectedItems.Count = 1 Then
            oData = lstTags.SelectedItems(0)
            lstTagFilter.Items.Add(oData)
            lstTags.Items.Remove(oData)
        ElseIf lstTags.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstTags.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lstTagFilter.Items.Add(kp)
                lstTags.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveTag()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))

        If lstTagFilter.SelectedItems.Count = 1 Then
            oData = lstTagFilter.SelectedItems(0)
            lstTagFilter.Items.Remove(oData)
            lstTags.Items.Add(oData)
        ElseIf lstTagFilter.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstTagFilter.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lstTagFilter.Items.Remove(kp)
                lstTags.Items.Add(kp)
            Next
        End If

    End Sub

    Private Sub LoadFilterFields()
        Dim oField As clsGameFilterField

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
        lstTagFilter.Items.Clear()

        lstTags.ValueMember = "Key"
        lstTags.DisplayMember = "Value"

        lstTagFilter.ValueMember = "Key"
        lstTagFilter.DisplayMember = "Value"

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
        If oGameFilters.Count > 0 Then
            chkGameInfo.Checked = True
            For Each oFilter As clsGameFilter In oGameFilters
                Select Case oFilter.Field.Type
                    Case clsGameFilterField.eDataType.fString
                        sFilter = oFilter.Field.FriendlyFieldName & " " & frmFilter_lstFilterContains & " """ & oFilter.Data & """ / "
                    Case clsGameFilterField.eDataType.fNumeric
                        oFilter.NumericOperator = DirectCast(cboNumericOps.SelectedValue, clsGameFilter.eNumericOperators)
                        sFilter = oFilter.Field.FriendlyFieldName & " " & oFilter.NumericOperatorAsString & " " & oFilter.Data & " / "
                    Case clsGameFilterField.eDataType.fBool
                        sFilter = oFilter.Field.FriendlyFieldName & " = " & oFilter.Data & " / "
                End Select

                If oFilter.NextBoolOperator Then
                    sFilter &= frmFilter_optAnd
                Else
                    sFilter &= frmFilter_optOr
                End If

                iParameterIndex += 1

                lstFilter.Items.Add(New KeyValuePair(Of clsGameFilter, String)(oFilter, sFilter))
            Next
        End If

        'Tag Filters
        If oTagFilters.Count > 0 Then
            chkTag.Checked = True
            For Each oTag As clsTag In oTagFilters
                oListTag = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
                lstTagFilter.Items.Add(oListTag)
                lstTags.Items.Remove(oListTag)
            Next

            If eCurrentFilterType = eFilterType.AllTags Then
                optAll.Checked = True
            Else
                optAny.Checked = True
            End If
        End If

        'Sorting
        cboSortField.SelectedValue = sSortField
        If bSortAsc Then
            optSortAsc.Checked = True
        Else
            optSortDesc.Checked = True
        End If

    End Sub

    Private Sub ChangeFilterMode()
        Dim oFilterType As clsGameFilterField.eDataType = DirectCast(cboFilterField.SelectedValue, clsGameFilterField).Type

        'Reset
        cboNumericOps.SelectedIndex = 0
        cboBoolFilter.SelectedIndex = 0
        numFilter.Value = 0
        txtStringFilter.Text = String.Empty

        'Reset Visibilty
        cboBoolFilter.Visible = False
        cboNumericOps.Visible = False
        numFilter.Visible = False
        txtStringFilter.Visible = False

        'Set Visiblity
        Select Case oFilterType
            Case clsGameFilterField.eDataType.fString
                txtStringFilter.Visible = True
            Case clsGameFilterField.eDataType.fNumeric
                cboNumericOps.Visible = True
                numFilter.Visible = True
                txtStringFilter.Visible = False
            Case clsGameFilterField.eDataType.fBool
                cboBoolFilter.Visible = True
        End Select

    End Sub

    Private Sub AddFilter()
        Dim oFilter As New clsGameFilter
        Dim sFilter As String = String.Empty

        'Build Filter
        oFilter.ID = "PARAM" & iParameterIndex
        oFilter.Field = cboFilterField.SelectedValue
        oFilter.NextBoolOperator = optAnd.Checked

        Select Case oFilter.Field.Type
            Case clsGameFilterField.eDataType.fString
                oFilter.Data = txtStringFilter.Text
                sFilter = oFilter.Field.FriendlyFieldName & " " & frmFilter_lstFilterContains & " """ & oFilter.Data & """ / "
            Case clsGameFilterField.eDataType.fNumeric
                oFilter.Data = numFilter.Value
                oFilter.NumericOperator = DirectCast(cboNumericOps.SelectedValue, clsGameFilter.eNumericOperators)
                sFilter = oFilter.Field.FriendlyFieldName & " " & oFilter.NumericOperatorAsString & " " & oFilter.Data & " / "
            Case clsGameFilterField.eDataType.fBool
                oFilter.Data = cboBoolFilter.SelectedValue
                sFilter = oFilter.Field.FriendlyFieldName & " = " & oFilter.Data & " / "
        End Select

        If oFilter.NextBoolOperator Then
            sFilter &= frmFilter_optAnd
        Else
            sFilter &= frmFilter_optOr
        End If

        oGameFilters.Add(oFilter)
        lstFilter.Items.Add(New KeyValuePair(Of clsGameFilter, String)(oFilter, sFilter))

        iParameterIndex += 1
    End Sub

    Private Sub RemoveFilter()
        Dim oFilter As Object

        If lstFilter.SelectedIndex <> -1 Then
            oFilter = lstFilter.SelectedItem
            oGameFilters.Remove(DirectCast(oFilter, KeyValuePair(Of clsGameFilter, String)).Key)
            lstFilter.Items.Remove(oFilter)
        End If

    End Sub

    Private Sub GetFilters()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTag As clsTag


        If chkGameInfo.Checked Then
            'Set Filter Type
            eCurrentFilterType = eFilterType.BaseFilter
        End If

        If chkTag.Checked Then
            'Set Tags
            TagFilters.Clear()
            For Each oData In lstTagFilter.Items
                oTag = DirectCast(hshTags(oData.Value), clsTag)
                TagFilters.Add(oTag)
            Next

            'Set Filter Type
            If TagFilters.Count = 0 Then
                eCurrentFilterType = eFilterType.NoTags
            ElseIf optAll.Checked Then
                eCurrentFilterType = eFilterType.AllTags
            Else
                eCurrentFilterType = eFilterType.AnyTag
            End If
        End If

        'Sorting
        If optSortAsc.Checked Then
            bSortAsc = True
        Else
            bSortAsc = False
        End If

        sSortField = cboSortField.SelectedValue

    End Sub

    Private Sub LoadCombos()
        Dim oFilterFields As New List(Of KeyValuePair(Of clsGameFilterField, String))
        Dim oSortFields As New List(Of KeyValuePair(Of String, String))
        Dim oNumericOperators As New List(Of KeyValuePair(Of clsGameFilter.eNumericOperators, String))
        Dim oBoolOperators As New List(Of KeyValuePair(Of Boolean, String))

        'cboBoolFilter
        cboBoolFilter.ValueMember = "Key"
        cboBoolFilter.DisplayMember = "Value"

        oBoolOperators.Add(New KeyValuePair(Of Boolean, String)(True, frmFilter_cboBoolFilterEnabled))
        oBoolOperators.Add(New KeyValuePair(Of Boolean, String)(False, frmFilter_cboBoolFilterDisabled))

        cboBoolFilter.DataSource = oBoolOperators

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

        'Set Form Text
        optOr.Text = frmFilter_optOr
        optAnd.Text = frmFilter_optAnd
        grpNextFilterOperator.Text = frmFilter_grpNextFilterOperator
        optAll.Text = frmFilter_optAll
        optAny.Text = frmFilter_optAny
        lblGameTags.Text = frmFilter_lblGameTags
        lblTags.Text = frmFilter_lblTags
        btnRemove.Text = frmFilter_btnRemove
        btnAdd.Text = frmFilter_btnAdd
        btnOK.Text = frmFilter_btnOK
        grpTagOptions.Text = frmFilter_grpTagOptions
        chkTag.Text = frmFilter_chkTag
        chkGameInfo.Text = frmFilter_chkGameInfo
        grpSorting.Text = frmFilter_grpSorting
        lblSortFields.Text = frmFilter_lblSortsFields
        optSortAsc.Text = frmFilter_optSortAsc
        optSortDesc.Text = frmFilter_optSortDesc
        btnAddFilter.Text = frmFilter_btnAddFilter
        btnRemoveFilter.Text = frmFilter_btnRemoveFilter
        lblCurrentFilters.Text = frmFilter_lblCurrentFilters
        lblFields.Text = frmFilter_lblFields
        lblFilterData.Text = frmFilter_lblFilterData
        grpSortOptions.Text = frmFilter_grpSortOptions

        'Defaults
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
        GetFilters()
        bShutdown = True
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddTag()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveTag()
    End Sub

    Private Sub frmFilter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not bShutdown Then
            e.Cancel = True
        End If
    End Sub

    Private Sub chkGameInfo_CheckedChanged(sender As Object, e As EventArgs) Handles chkGameInfo.CheckedChanged
        If chkGameInfo.Checked Then
            grpGameFilter.Enabled = True
        Else
            grpGameFilter.Enabled = False
            oGameFilters.Clear()
            lstFilter.Items.Clear()
        End If
    End Sub

    Private Sub chkTag_CheckedChanged(sender As Object, e As EventArgs) Handles chkTag.CheckedChanged
        If chkTag.Checked Then
            grpTagFilter.Enabled = True
        Else
            grpTagFilter.Enabled = False
            oTagFilters.Clear()
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

End Class