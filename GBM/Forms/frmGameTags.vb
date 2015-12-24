Imports GBM.My.Resources

Public Class frmGameTags

    Dim sMonitorIDs As List(Of String)
    Dim sGameName As String = String.Empty

    Public Property IDList As List(Of String)
        Get
            Return sMonitorIDs
        End Get
        Set(value As List(Of String))
            sMonitorIDs = value
        End Set
    End Property

    Public Property GameName As String
        Get
            Return sGameName
        End Get
        Set(value As String)
            sGameName = value
        End Set
    End Property

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

            mgrGameTags.DoGameTagAddBatch(oGameTags)

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

                mgrGameTags.DoGameTagAddBatch(oGameTags)

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

            mgrGameTags.DoGameTagDelete(oGameTags)

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

                mgrGameTags.DoGameTagDelete(oGameTags)

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

        'Handle Data
        hshTags = mgrTags.ReadTags()
        hshGameTags = mgrGameTags.GetTagsByGameMulti(IDList)

        For Each de As DictionaryEntry In hshGameTags
            oTag = DirectCast(de.Value, clsTag)
            If hshTags.ContainsKey(oTag.Name) Then
                hshTags.Remove(oTag.Name)
            End If
        Next

        'Handle Lists
        lstTags.Items.Clear()
        lstGameTags.Items.Clear()

        lstTags.ValueMember = "Key"
        lstTags.DisplayMember = "Value"
        lstGameTags.ValueMember = "Key"
        lstGameTags.DisplayMember = "Value"

        For Each de As DictionaryEntry In hshGameTags
            oTag = DirectCast(de.Value, clsTag)
            oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
            lstGameTags.Items.Add(oData)
        Next

        For Each de As DictionaryEntry In hshTags
            oTag = DirectCast(de.Value, clsTag)
            oData = New KeyValuePair(Of String, String)(oTag.ID, oTag.Name)
            lstTags.Items.Add(oData)
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

        'Set Form Text
        btnOpenTags.Text = frmGameTags_btnOpenTags
        btnClose.Text = frmGameTags_btnClose
        lblGameTags.Text = frmGameTags_lblGameTags
        lblTags.Text = frmGameTags_lblTags
        btnRemove.Text = frmGameTags_btnRemove
        btnAdd.Text = frmGameTags_btnAdd

    End Sub

    Private Sub frmGameTags_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        SetForm()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddTag()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveTag()
    End Sub

    Private Sub btnOpenTags_Click(sender As Object, e As EventArgs) Handles btnOpenTags.Click
        OpenTags()
    End Sub
End Class