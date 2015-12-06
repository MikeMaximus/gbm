Public Class frmFilter

    Public Enum eFilterType As Integer
        NoFilter = 1
        AnyTag = 2
        AllTags = 3
        NoTags = 4
    End Enum

    Dim oFilters As New List(Of clsTag)
    Dim eCurrentFilterType As eFilterType = eFilterType.AnyTag
    Dim hshTags As New Hashtable
    Dim bShutdown As Boolean = False

    Public ReadOnly Property Filters As List(Of clsTag)
        Get
            Return oFilters
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

        'Set Tags
        For Each oData In lstFilter.Items
            oTag = DirectCast(hshTags(oData.Value), clsTag)
            Filters.Add(oTag)
        Next

        'Set Filter Type
        If Filters.Count = 0 Then
            eCurrentFilterType = eFilterType.NoTags
        ElseIf optAll.Checked Then
            eCurrentFilterType = eFilterType.AllTags
        Else
            eCurrentFilterType = eFilterType.AnyTag
        End If

    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        GetFilters()
        bShutdown = True
        Me.close
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
End Class