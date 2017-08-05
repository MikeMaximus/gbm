Imports GBM.My.Resources

Public Class frmFilter

    Public Enum eFilterType As Integer
        BaseFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
    End Enum

    Dim oTagFilters As New List(Of clsTag)
    Dim hshStringFilters As New Hashtable
    Dim eCurrentFilterType As eFilterType = eFilterType.BaseFilter
    Dim bSortAsc As Boolean = True
    Dim sSortField As String = "Name"
    Dim hshTags As New Hashtable
    Dim bShutdown As Boolean = False

    Public ReadOnly Property StringFilters As Hashtable
        Get
            Return hshStringFilters
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
            lstFilter.Items.Add(oData)
            lstTags.Items.Remove(oData)
        ElseIf lstTags.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstTags.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lstFilter.Items.Add(kp)
                lstTags.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveTag()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))

        If lstFilter.SelectedItems.Count = 1 Then
            oData = lstFilter.SelectedItems(0)
            lstFilter.Items.Remove(oData)
            lstTags.Items.Add(oData)
        ElseIf lstFilter.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstFilter.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                lstFilter.Items.Remove(kp)
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
        lstFilter.Items.Clear()

        lstTags.ValueMember = "Key"
        lstTags.DisplayMember = "Value"
        lstFilter.ValueMember = "Key"
        lstFilter.DisplayMember = "Value"

        For Each de As DictionaryEntry In hshTags
            oTag = DirectCast(de.Value, clsTag)
            oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
            lstTags.Items.Add(oData)
        Next

    End Sub

    Private Sub GetFilters()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTag As clsTag


        If chkGameInfo.Checked Then
            'Set Filter Type
            eCurrentFilterType = eFilterType.BaseFilter

            'Set String Filter
            If txtName.Text <> String.Empty Then
                hshStringFilters.Add("Name", txtName.Text)
            End If
            If txtProcess.Text <> String.Empty Then
                hshStringFilters.Add("Process", txtProcess.Text)
            End If
            If txtCompany.Text <> String.Empty Then
                hshStringFilters.Add("Company", txtCompany.Text)
            End If
        End If

        If chkTag.Checked Then
            'Set Tags
            For Each oData In lstFilter.Items
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
        Dim oSortFields As New List(Of KeyValuePair(Of String, String))

        'cboSortField
        cboSortField.ValueMember = "Key"
        cboSortField.DisplayMember = "Value"

        oSortFields.Add(New KeyValuePair(Of String, String)("Name", frmFilter_SortName))
        oSortFields.Add(New KeyValuePair(Of String, String)("Process", frmFilter_SortProcess))
        oSortFields.Add(New KeyValuePair(Of String, String)("Company", frmFilter_SortCompany))
        oSortFields.Add(New KeyValuePair(Of String, String)("Hours", frmFilter_SortHours))

        cboSortField.DataSource = oSortFields

        'Select Default
        cboSortField.SelectedIndex = 0
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmFilter_FormName

        'Set Form Text
        optOr.Text = frmFilter_optOr
        optAnd.Text = frmFilter_optAnd
        lblCompany.Text = frmFilter_lblCompany
        lblProcess.Text = frmFilter_lblProcess
        lblName.Text = frmFilter_lblName
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
End Class