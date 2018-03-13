Imports GBM.My.Resources

Public Class frmGameProcesses
    Private sMonitorIDs As List(Of String)
    Private sGameName As String = String.Empty
    Private bNewMode As Boolean = False
    Private oProcessList As List(Of KeyValuePair(Of String, String))

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

    Public Property NewMode As Boolean
        Get
            Return bNewMode
        End Get
        Set(value As Boolean)
            bNewMode = value
        End Set
    End Property

    Public Property ProcessList As List(Of KeyValuePair(Of String, String))
        Get
            Return oProcessList
        End Get
        Set(value As List(Of KeyValuePair(Of String, String)))
            oProcessList = value
        End Set
    End Property

    Private Sub AddProcess()
        Dim oData As KeyValuePair(Of String, String)
        Dim oProcesss As List(Of KeyValuePair(Of String, String))
        Dim oGameProcess As clsGameProcess
        Dim oGameProcesses As List(Of clsGameProcess)

        If lstProcesses.SelectedItems.Count = 1 Then
            oData = lstProcesses.SelectedItems(0)

            oGameProcesses = New List(Of clsGameProcess)
            For Each sID As String In IDList
                oGameProcess = New clsGameProcess
                oGameProcess.MonitorID = sID
                oGameProcess.ProcessID = oData.Key
                oGameProcesses.Add(oGameProcess)
            Next

            If Not bNewMode Then mgrGameProcesses.DoGameProcessAddBatch(oGameProcesses)

            lstGameProcesses.Items.Add(oData)
            lstProcesses.Items.Remove(oData)
        ElseIf lstProcesses.SelectedItems.Count > 1 Then
            oProcesss = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstProcesses.SelectedItems
                oProcesss.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oProcesss
                oGameProcesses = New List(Of clsGameProcess)
                For Each sID As String In IDList
                    oGameProcess = New clsGameProcess
                    oGameProcess.MonitorID = sID
                    oGameProcess.ProcessID = kp.Key
                    oGameProcesses.Add(oGameProcess)
                Next

                If Not bNewMode Then mgrGameProcesses.DoGameProcessAddBatch(oGameProcesses)

                lstGameProcesses.Items.Add(kp)
                lstProcesses.Items.Remove(kp)
            Next
        End If

    End Sub

    Private Sub RemoveProcess()
        Dim oData As KeyValuePair(Of String, String)
        Dim oProcesses As List(Of KeyValuePair(Of String, String))
        Dim oGameProcess As clsGameProcess
        Dim oGameProcesses As List(Of clsGameProcess)

        If lstGameProcesses.SelectedItems.Count = 1 Then
            oData = lstGameProcesses.SelectedItems(0)

            oGameProcesses = New List(Of clsGameProcess)
            For Each sID As String In IDList
                oGameProcess = New clsGameProcess
                oGameProcess.MonitorID = sID
                oGameProcess.ProcessID = oData.Key
                oGameProcesses.Add(oGameProcess)
            Next

            If Not bNewMode Then mgrGameProcesses.DoGameProcessDelete(oGameProcesses)

            lstGameProcesses.Items.Remove(oData)
            lstProcesses.Items.Add(oData)
        ElseIf lstGameProcesses.SelectedItems.Count > 1 Then
            oProcesses = New List(Of KeyValuePair(Of String, String))

            For Each oData In lstGameProcesses.SelectedItems
                oProcesses.Add(oData)
            Next

            For Each kp As KeyValuePair(Of String, String) In oProcesses
                oGameProcesses = New List(Of clsGameProcess)
                For Each sID As String In IDList
                    oGameProcess = New clsGameProcess
                    oGameProcess.MonitorID = sID
                    oGameProcess.ProcessID = kp.Key
                    oGameProcesses.Add(oGameProcess)
                Next

                If Not bNewMode Then mgrGameProcesses.DoGameProcessDelete(oGameProcesses)

                lstGameProcesses.Items.Remove(kp)
                lstProcesses.Items.Add(kp)
            Next
        End If

    End Sub

    Private Sub LoadData()
        Dim hshProcesses As Hashtable
        Dim hshGameProcesses As Hashtable
        Dim oProcess As clsProcess
        Dim oData As KeyValuePair(Of String, String)

        'Load Processes
        hshProcesses = mgrProcess.ReadProcesses()

        'Handle Lists
        lstProcesses.Items.Clear()
        lstGameProcesses.Items.Clear()

        lstProcesses.ValueMember = "Key"
        lstProcesses.DisplayMember = "Value"
        lstGameProcesses.ValueMember = "Key"
        lstGameProcesses.DisplayMember = "Value"

        If bNewMode Then
            For Each kp As KeyValuePair(Of String, String) In oProcessList
                'We need to be sure the tags still exist if the "Process Manager" form was used
                If hshProcesses.ContainsKey(kp.Value) Then
                    lstGameProcesses.Items.Add(kp)
                End If
            Next

            For Each kp As KeyValuePair(Of String, String) In oProcessList
                If hshProcesses.ContainsKey(kp.Value) Then
                    hshProcesses.Remove(kp.Value)
                End If
            Next
        Else
            hshGameProcesses = mgrGameProcesses.GetProcessesByGameMulti(IDList)

            For Each de As DictionaryEntry In hshGameProcesses
                oProcess = DirectCast(de.Value, clsProcess)
                If hshProcesses.ContainsKey(oProcess.Name) Then
                    hshProcesses.Remove(oProcess.Name)
                End If
            Next

            For Each de As DictionaryEntry In hshGameProcesses
                oProcess = DirectCast(de.Value, clsProcess)
                oData = New KeyValuePair(Of String, String)(oProcess.ID, oProcess.Name)
                lstGameProcesses.Items.Add(oData)
            Next
        End If

        For Each de As DictionaryEntry In hshProcesses
            oProcess = DirectCast(de.Value, clsProcess)
            oData = New KeyValuePair(Of String, String)(oProcess.ID, oProcess.Name)
            lstProcesses.Items.Add(oData)
        Next

    End Sub

    Private Sub BuildProcessList()
        Dim oData As KeyValuePair(Of String, String)
        oProcessList.Clear()
        For Each oData In lstGameProcesses.Items
            oProcessList.Add(oData)
        Next
    End Sub

    Private Sub OpenProcessManager()
        Dim frm As New frmProcessManager
        frm.ShowDialog()
        LoadData()
    End Sub

    Private Sub SetForm()
        'Set Form Name
        If IDList.Count > 1 Then
            Me.Text = frmGameProcesses_FormNameMulti
        Else
            Me.Text = mgrCommon.FormatString(frmGameProcesses_FormNameSingle, GameName)
        End If

        'Set Form Text
        btnOpenProcesses.Text = frmGameProcesses_btnOpenProcesses
        btnClose.Text = frmGameProcesses_btnClose
        lblGameProcesses.Text = frmGameProcesses_lblGameProccesses
        lblProcesses.Text = frmGameProcesses_lblProcesses
        btnRemove.Text = frmGameProcesses_btnRemove
        btnAdd.Text = frmGameProcesses_btnAdd

    End Sub

    Private Sub frmGameProcesses_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        SetForm()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If bNewMode Then BuildProcessList()
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddProcess()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveProcess()
    End Sub

    Private Sub btnOpenProcesses_Click(sender As Object, e As EventArgs) Handles btnOpenProcesses.Click
        If bNewMode Then BuildProcessList()
        OpenProcessManager()
    End Sub
End Class