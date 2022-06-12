Imports GBM.My.Resources

Public Class frmGameTags
    Public Property IDList As List(Of String)
    Public Property GameName As String = String.Empty
    Public Property NewMode As Boolean = False
    Public Property TagList As List(Of KeyValuePair(Of String, String))

    Private Sub AddTag()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))
        Dim oGameTag As clsGameTag
        Dim oGameTags As List(Of clsGameTag)

        If lstTags.SelectedItems.Count = 1 Then
            oData = lstTags.SelectedItems(0)

            oGameTags = New List(Of clsGameTag)
            For Each sID As String In IDList
                oGameTag = New clsGameTag
                oGameTag.MonitorID = sID
                oGameTag.TagID = oData.Key
                oGameTags.Add(oGameTag)
            Next

            If Not NewMode Then mgrGameTags.DoGameTagAddBatch(oGameTags)

            lstGameTags.Items.Add(oData)
            lstTags.Items.Remove(oData)
        ElseIf lstTags.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstTags.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                oGameTags = New List(Of clsGameTag)
                For Each sID As String In IDList
                    oGameTag = New clsGameTag
                    oGameTag.MonitorID = sID
                    oGameTag.TagID = kp.Key
                    oGameTags.Add(oGameTag)
                Next

                If Not NewMode Then mgrGameTags.DoGameTagAddBatch(oGameTags)

                lstGameTags.Items.Add(kp)
                lstTags.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveTag()
        Dim oData As KeyValuePair(Of String, String)
        Dim oTags As List(Of KeyValuePair(Of String, String))
        Dim oGameTag As clsGameTag
        Dim oGameTags As List(Of clsGameTag)

        If lstGameTags.SelectedItems.Count = 1 Then
            oData = lstGameTags.SelectedItems(0)

            oGameTags = New List(Of clsGameTag)
            For Each sID As String In IDList
                oGameTag = New clsGameTag
                oGameTag.MonitorID = sID
                oGameTag.TagID = oData.Key
                oGameTags.Add(oGameTag)
            Next

            If Not NewMode Then mgrGameTags.DoGameTagDelete(oGameTags)

            lstGameTags.Items.Remove(oData)
            lstTags.Items.Add(oData)
        ElseIf lstGameTags.SelectedItems.Count > 1 Then
            oTags = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstGameTags.SelectedItems
                oTags.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oTags
                oGameTags = New List(Of clsGameTag)
                For Each sID As String In IDList
                    oGameTag = New clsGameTag
                    oGameTag.MonitorID = sID
                    oGameTag.TagID = kp.Key
                    oGameTags.Add(oGameTag)
                Next

                If Not NewMode Then mgrGameTags.DoGameTagDelete(oGameTags)

                lstGameTags.Items.Remove(kp)
                lstTags.Items.Add(kp)
            Next
        End If

    End Sub

    Private Sub LoadData()
        Dim hshTags As Hashtable
        Dim hshGameTags As Hashtable
        Dim oTag As clsTag
        Dim oData As KeyValuePair(Of String, String)

        'Load Tags
        hshTags = mgrTags.ReadTags()

        'Handle Lists
        lstTags.Items.Clear()
        lstGameTags.Items.Clear()

        lstTags.ValueMember = "Key"
        lstTags.DisplayMember = "Value"
        lstGameTags.ValueMember = "Key"
        lstGameTags.DisplayMember = "Value"

        If NewMode Then
            For Each kp As KeyValuePair(Of String, String) In TagList
                'We need to be sure the tags still exist if the "Setup Tags" form was used
                If hshTags.ContainsKey(kp.Value) Then
                    lstGameTags.Items.Add(kp)
                End If
            Next

            For Each kp As KeyValuePair(Of String, String) In TagList
                If hshTags.ContainsKey(kp.Value) Then
                    hshTags.Remove(kp.Value)
                End If
            Next
        Else
            hshGameTags = mgrGameTags.GetTagsByGameMulti(IDList)

            For Each de As DictionaryEntry In hshGameTags
                oTag = DirectCast(de.Value, clsTag)
                If hshTags.ContainsKey(oTag.Name) Then
                    hshTags.Remove(oTag.Name)
                End If
            Next

            For Each de As DictionaryEntry In hshGameTags
                oTag = DirectCast(de.Value, clsTag)
                oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
                lstGameTags.Items.Add(oData)
            Next
        End If

        For Each de As DictionaryEntry In hshTags
            oTag = DirectCast(de.Value, clsTag)
            oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
            lstTags.Items.Add(oData)
        Next

    End Sub

    Private Sub BuildTagList()
        Dim oData As KeyValuePair(Of String, String)
        TagList.Clear()
        For Each oData In lstGameTags.Items
            TagList.Add(oData)
        Next
    End Sub

    Private Sub OpenTags()
        Dim frm As New frmTags
        frm.ShowDialog()
        LoadData()
    End Sub

    Private Sub SetForm()
        'Set Form Name
        If IDList.Count > 1 Then
            Me.Text = frmGameTags_FormNameMulti
        Else
            Me.Text = mgrCommon.FormatString(frmGameTags_FormNameSingle, GameName)
        End If
        Me.Icon = GBM_Icon

        'Set Form Text
        btnOpenTags.Text = frmGameTags_btnOpenTags
        btnOpenTags.Image = Multi_Edit
        lblGameTags.Text = frmGameTags_lblGameTags
        lblTags.Text = frmGameTags_lblTags
        btnRemove.Text = frmGameTags_btnRemove
        btnAdd.Text = frmGameTags_btnAdd

    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        SetForm()
    End Sub

    Private Sub frmGameTags_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If NewMode Then BuildTagList()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddTag()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveTag()
    End Sub

    Private Sub btnOpenTags_Click(sender As Object, e As EventArgs) Handles btnOpenTags.Click
        If NewMode Then BuildTagList()
        OpenTags()
    End Sub
End Class