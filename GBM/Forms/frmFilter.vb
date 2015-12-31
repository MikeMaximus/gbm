Imports GBM.My.Resources

Public Class frmFilter

    Public Enum eFilterType As Integer
        NoFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
        FieldAnd = 5
        FieldOr = 6
    End Enum

    Dim oTagFilters As New List(Of clsTag)
    Dim hshStringFilters As New Hashtable
    Dim eCurrentFilterType As eFilterType = eFilterType.AnyTag
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


        If optGameInfo.Checked Then
            'Set Filter Type
            If optAnd.Checked Then
                eCurrentFilterType = eFilterType.FieldAnd
            Else
                eCurrentFilterType = eFilterType.FieldOr
            End If

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
        Else
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
        optTag.Text = frmFilter_optTag
        optGameInfo.Text = frmFilter_optGameInfo
    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        optGameInfo.Checked = True
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

    Private Sub optGameInfo_Click(sender As Object, e As EventArgs) Handles optGameInfo.Click, optTag.Click
        If optGameInfo.Checked = True Then
            grpGameFilter.Enabled = True
            grpTagFilter.Enabled = False
        Else
            grpGameFilter.Enabled = False
            grpTagFilter.Enabled = True
        End If
    End Sub
End Class