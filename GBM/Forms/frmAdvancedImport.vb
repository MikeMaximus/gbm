Public Class frmAdvancedImport

    Private hshImportData As Hashtable
    Private bSelectAll As Boolean = False
    Private bIsLoading As Boolean = False

    Public Property ImportData As Hashtable
        Set(value As Hashtable)
            hshImportData = value
        End Set
        Get
            Return hshImportData
        End Get
    End Property

    Private Sub SelectToggle()
        bSelectAll = Not bSelectAll
        For i As Integer = 0 To lstGames.Items.Count - 1
            lstGames.SetItemChecked(i, bSelectAll)
        Next
        UpdateSelected()
    End Sub

    Private Sub LoadData()
        Dim oApp As clsGame
        Dim oData As KeyValuePair(Of String, String)

        lstGames.ValueMember = "Key"
        lstGames.DisplayMember = "Value"

        For Each de As DictionaryEntry In ImportData
            oApp = DirectCast(de.Value, clsGame)
            oData = New KeyValuePair(Of String, String)(oApp.CompoundKey, oApp.Name & " (" & oApp.TrueProcess & ")")
            lstGames.Items.Add(oData)
        Next
    End Sub

    Private Sub SetForm()
        chkSelectAll.Checked = True
        lblGames.Text = ImportData.Count & " new configurations available."
    End Sub

    Private Sub BuildList()
        Dim oData As KeyValuePair(Of String, String)

        For i As Integer = 0 To lstGames.Items.Count - 1
            If Not lstGames.GetItemChecked(i) Then
                oData = lstGames.Items(i)
                ImportData.Remove(oData.Key)
            End If
        Next
    End Sub

    Private Sub UpdateSelected()
        lblSelected.Text = lstGames.CheckedItems.Count & " Selected"
    End Sub

    Private Sub frmAdvancedImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        bIsLoading = True
        SetForm()
        LoadData()
        SelectToggle()
        bIsLoading = False
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If Not bIsLoading Then SelectToggle()
    End Sub

    Private Sub lstGames_SelectedValueChanged(sender As Object, e As EventArgs) Handles lstGames.SelectedValueChanged
        If Not bIsLoading Then UpdateSelected()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        BuildList()
        If ImportData.Count > 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class