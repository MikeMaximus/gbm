Imports GBM.My.Resources

Public Class frmConfigLinks
    Private oConfigLinkList As List(Of KeyValuePair(Of String, String))
    Private WithEvents tmFilterTimer As Timer

    Public Property IDList As List(Of String)
    Public Property GameName As String = String.Empty
    Public Property NewMode As Boolean = False
    Public Property ConfigLinkList As List(Of KeyValuePair(Of String, String))

    Private Sub AddLink()
        Dim oData As KeyValuePair(Of String, String)
        Dim oConfigs As List(Of KeyValuePair(Of String, String))
        Dim oConfigLink As clsConfigLink
        Dim oConfigLinks As List(Of clsConfigLink)

        If lstConfigs.SelectedItems.Count = 1 Then
            oData = lstConfigs.SelectedItems(0)

            oConfigLinks = New List(Of clsConfigLink)
            For Each sID As String In IDList
                oConfigLink = New clsConfigLink
                oConfigLink.MonitorID = sID
                oConfigLink.LinkID = oData.Key
                oConfigLinks.Add(oConfigLink)
            Next

            If Not NewMode Then mgrConfigLinks.DoConfigLinkAddBatch(oConfigLinks)

            lstLinks.Items.Add(oData)
            lstConfigs.Items.Remove(oData)
        ElseIf lstConfigs.SelectedItems.Count > 1 Then
            oConfigs = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstConfigs.SelectedItems
                oConfigs.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oConfigs
                oConfigLinks = New List(Of clsConfigLink)
                For Each sID As String In IDList
                    oConfigLink = New clsConfigLink
                    oConfigLink.MonitorID = sID
                    oConfigLink.LinkID = kp.Key
                    oConfigLinks.Add(oConfigLink)
                Next

                If Not NewMode Then mgrConfigLinks.DoConfigLinkAddBatch(oConfigLinks)

                lstLinks.Items.Add(kp)
                lstConfigs.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveLink()
        Dim oData As KeyValuePair(Of String, String)
        Dim oConfigs As List(Of KeyValuePair(Of String, String))
        Dim oConfigLink As clsConfigLink
        Dim oConfigLinks As List(Of clsConfigLink)

        If lstLinks.SelectedItems.Count = 1 Then
            oData = lstLinks.SelectedItems(0)

            oConfigLinks = New List(Of clsConfigLink)
            For Each sID As String In IDList
                oConfigLink = New clsConfigLink
                oConfigLink.MonitorID = sID
                oConfigLink.LinkID = oData.Key
                oConfigLinks.Add(oConfigLink)
            Next

            If Not NewMode Then mgrConfigLinks.DoConfigLinkDelete(oConfigLinks)

            lstLinks.Items.Remove(oData)
            lstConfigs.Items.Add(oData)
        ElseIf lstLinks.SelectedItems.Count > 1 Then
            oConfigs = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstLinks.SelectedItems
                oConfigs.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oConfigs
                oConfigLinks = New List(Of clsConfigLink)
                For Each sID As String In IDList
                    oConfigLink = New clsConfigLink
                    oConfigLink.MonitorID = sID
                    oConfigLink.LinkID = kp.Key
                    oConfigLinks.Add(oConfigLink)
                Next

                If Not NewMode Then mgrConfigLinks.DoConfigLinkDelete(oConfigLinks)

                lstLinks.Items.Remove(kp)
                lstConfigs.Items.Add(kp)
            Next
        End If

    End Sub

    Private Sub LoadData()
        Dim hshConfigs As Hashtable
        Dim hshLinks As Hashtable
        Dim oGame As clsGame
        Dim oData As KeyValuePair(Of String, String)
        Dim sFilter As String = txtQuickFilter.Text

        'Load Configs
        hshConfigs = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)

        'Handle Lists
        lstConfigs.BeginUpdate()
        lstLinks.BeginUpdate()
        lstConfigs.Items.Clear()
        lstLinks.Items.Clear()

        lstConfigs.ValueMember = "Key"
        lstConfigs.DisplayMember = "Value"
        lstLinks.ValueMember = "Key"
        lstLinks.DisplayMember = "Value"

        If NewMode Then
            For Each kp As KeyValuePair(Of String, String) In oConfigLinkList
                If hshConfigs.ContainsKey(kp.Key) Then
                    hshConfigs.Remove(kp.Key)
                End If
            Next

            For Each kp As KeyValuePair(Of String, String) In oConfigLinkList
                lstLinks.Items.Add(kp)
            Next
        Else
            hshLinks = mgrConfigLinks.GetConfigLinksByGameMulti(IDList)

            For Each de As DictionaryEntry In hshLinks
                If hshConfigs.ContainsKey(de.Key) Then
                    hshConfigs.Remove(de.Key)
                End If
            Next

            For Each de As DictionaryEntry In hshLinks
                oData = New KeyValuePair(Of String, String)(de.Key, de.Value)
                lstLinks.Items.Add(oData)
            Next
        End If

        For Each sID As String In IDList
            If hshConfigs.Contains(sID) Then
                hshConfigs.Remove(sID)
            End If
        Next

        For Each de As DictionaryEntry In hshConfigs
            oGame = DirectCast(de.Value, clsGame)
            oData = New KeyValuePair(Of String, String)(oGame.ID, oGame.Name)

            'Apply the quick filter if applicable
            If sFilter = String.Empty Then
                lstConfigs.Items.Add(oData)
            Else
                If oGame.Name.ToLower.Contains(sFilter.ToLower) Then
                    lstConfigs.Items.Add(oData)
                End If
            End If
        Next

        lstConfigs.EndUpdate()
        lstLinks.EndUpdate()
    End Sub

    Private Sub BuildConfigLinkList()
        Dim oData As KeyValuePair(Of String, String)
        oConfigLinkList.Clear()
        For Each oData In lstLinks.Items
            oConfigLinkList.Add(oData)
        Next
    End Sub

    Private Sub SetForm()
        'Set Form Name
        If IDList.Count > 1 Then
            Me.Text = frmConfigLinks_FormNameMulti
        Else
            Me.Text = mgrCommon.FormatString(frmConfigLinks_FormNameSingle, GameName)
        End If
        Me.Icon = GBM_Icon

        'Set Form Text
        lblFilter.Text = frmConfigLinks_lblFilter
        lblConfigs.Text = frmConfigLinks_lblConfigs
        lblLinkedConfigs.Text = frmConfigLinks_lblLinkedConfigs
        btnRemove.Text = frmConfigLinks_btnRemove
        btnAdd.Text = frmConfigLinks_btnAdd

        'Init Filter Timer
        tmFilterTimer = New Timer()
        tmFilterTimer.Interval = 1000
        tmFilterTimer.Enabled = False
    End Sub

    Private Sub frmConfigLinks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        SetForm()
    End Sub

    Private Sub frmConfigLinks_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If NewMode Then BuildConfigLinkList()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddLink()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveLink()
    End Sub

    Private Sub tmFilterTimer_Tick(sender As Object, ByVal e As EventArgs) Handles tmFilterTimer.Tick
        lstConfigs.DataSource = Nothing
        LoadData()
        tmFilterTimer.Stop()
        tmFilterTimer.Enabled = False
        lstConfigs.Enabled = True
    End Sub

    Private Sub txtQuickFilter_TextChanged(sender As Object, e As EventArgs) Handles txtQuickFilter.TextChanged
        lstConfigs.ClearSelected()

        If Not tmFilterTimer.Enabled Then
            lstConfigs.Enabled = False
            tmFilterTimer.Enabled = True
            tmFilterTimer.Start()
        End If
    End Sub
End Class