Imports GBM.My.Resources

Public Class frmFilter

    Public Enum eFilterType As Integer
        BaseFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
    End Enum

    Public Class clsFilter

        Private sID As String
        Private sField As String
        Private oData As Object
        Private bAndOperator As Boolean

        Public Property ID As String
            Get
                Return sID
            End Get
            Set(value As String)
                sID = value
            End Set
        End Property

        Public Property Field As String
            Get
                Return sField
            End Get
            Set(value As String)
                sField = value
            End Set
        End Property

        Public Property Data As Object
            Get
                Return oData
            End Get
            Set(value As Object)
                oData = value
            End Set
        End Property

        Public Property AndOperator As Boolean
            Get
                Return bAndOperator
            End Get
            Set(value As Boolean)
                bAndOperator = value
            End Set
        End Property

    End Class

    Dim oTagFilters As New List(Of clsTag)
    Dim oGameFilters As New List(Of clsFilter)
    Dim eCurrentFilterType As eFilterType = eFilterType.BaseFilter
    Dim bSortAsc As Boolean = True
    Dim sSortField As String = "Name"
    Dim hshTags As New Hashtable
    Dim bShutdown As Boolean = False
    Dim iParameterIndex As Integer = 0

    Public ReadOnly Property GameFilters As List(Of clsFilter)
        Get
            Return oGameFilters
        End Get
    End Property

    Public ReadOnly Property TagFilters As List(Of clsTag)
        Get
            Return oTagFilters
        End Get
    End Property

    Public ReadOnly Property FilterType As eFilterType
        Get
            Return eCurrentFilterType
        End Get
    End Property

    Public ReadOnly Property SortAsc As Boolean
        Get
            Return bSortAsc
        End Get
    End Property

    Public ReadOnly Property SortField As String
        Get
            Return sSortField
        End Get
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

    Private Sub LoadData()
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

    Private Sub AddFilter()
        Dim oFilter As New clsFilter
        Dim sFilter As String

        lstFilter.ValueMember = "Key"
        lstFilter.DisplayMember = "Value"

        'Build Filter
        oFilter.ID = "PARAM" & iParameterIndex
        oFilter.Field = cboFilterField.SelectedValue
        oFilter.Data = txtFilterData.Text
        oFilter.AndOperator = optAnd.Checked

        oGameFilters.Add(oFilter)

        'Build String
        sFilter = oFilter.Field & " / " & oFilter.Data & " / "
        If oFilter.AndOperator Then
            sFilter &= frmFilter_optAnd
        Else
            sFilter &= frmFilter_optOr
        End If

        lstFilter.Items.Add(New KeyValuePair(Of clsFilter, String)(oFilter, sFilter))

        iParameterIndex += 1
    End Sub

    Private Sub RemoveFilter()
        Dim oFilter As Object

        If lstFilter.SelectedIndex <> -1 Then
            oFilter = lstFilter.SelectedItem
            oGameFilters.Remove(DirectCast(oFilter, KeyValuePair(Of clsFilter, String)).Key)
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
        Dim oFilterFields As New List(Of KeyValuePair(Of String, String))
        Dim oSortFields As New List(Of KeyValuePair(Of String, String))

        'cboFilterField
        cboFilterField.ValueMember = "Key"
        cboFilterField.DisplayMember = "Value"

        oFilterFields.Add(New KeyValuePair(Of String, String)("Name", frmFilter_FieldName))
        oFilterFields.Add(New KeyValuePair(Of String, String)("Process", frmFilter_FieldProcess))
        oFilterFields.Add(New KeyValuePair(Of String, String)("Parameter", frmFilter_FieldParameter))
        oFilterFields.Add(New KeyValuePair(Of String, String)("Company", frmFilter_FieldCompany))
        oFilterFields.Add(New KeyValuePair(Of String, String)("Version", frmFilter_FieldVersion))

        cboFilterField.DataSource = oFilterFields

        'cboSortField
        cboSortField.ValueMember = "Key"
        cboSortField.DisplayMember = "Value"

        oSortFields.Add(New KeyValuePair(Of String, String)("Name", frmFilter_FieldName))
        oSortFields.Add(New KeyValuePair(Of String, String)("Process", frmFilter_FieldProcess))
        oSortFields.Add(New KeyValuePair(Of String, String)("Parameter", frmFilter_FieldParameter))
        oSortFields.Add(New KeyValuePair(Of String, String)("Company", frmFilter_FieldCompany))
        oSortFields.Add(New KeyValuePair(Of String, String)("Version", frmFilter_FieldVersion))
        oSortFields.Add(New KeyValuePair(Of String, String)("Hours", frmFilter_FieldHours))

        cboSortField.DataSource = oSortFields

        'Select Defaults
        cboFilterField.SelectedIndex = 0
        cboSortField.SelectedIndex = 0
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmFilter_FormName

        'Set Form Text
        optOr.Text = frmFilter_optOr
        optAnd.Text = frmFilter_optAnd
        grpGameInfoOptions.Text = frmFilter_grpGameInfoOptions
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
        lblOrderBy.Text = frmFilter_lblOrderBy
        optSortAsc.Text = frmFilter_optSortAsc
        optSortDesc.Text = frmFilter_optSortDesc
        btnAddFilter.Text = frmFilter_btnAddFilter
        btnRemoveFilter.Text = frmFilter_btnRemoveFilter
        lblCurrentFilters.Text = frmFilter_lblCurrentFilters
        lblFields.Text = frmFilter_lblFields
        lblFilterData.Text = frmFilter_lblFilterData

        'Defaults
        optSortAsc.Checked = True
        grpGameFilter.Enabled = False
        grpTagFilter.Enabled = False
    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadCombos()
        LoadData()
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
        End If
    End Sub

    Private Sub chkTag_CheckedChanged(sender As Object, e As EventArgs) Handles chkTag.CheckedChanged
        If chkTag.Checked Then
            grpTagFilter.Enabled = True
        Else
            grpTagFilter.Enabled = False
        End If
    End Sub

    Private Sub btnAddFilter_Click(sender As Object, e As EventArgs) Handles btnAddFilter.Click
        AddFilter()
    End Sub

    Private Sub btnRemoveFilter_Click(sender As Object, e As EventArgs) Handles btnRemoveFilter.Click
        RemoveFilter()
    End Sub
End Class