﻿Imports GBM.My.Resources
Imports System.IO

Public Class frmIncludeExclude
    Private Property IsLoading As Boolean = False
    Private Property IsDirty As Boolean = False

    Dim sRootFolder As String

    Public Property RootFolder As String
        Get
            Return sRootFolder
        End Get
        Set(value As String)
            sRootFolder = value.TrimEnd(Path.DirectorySeparatorChar)
        End Set
    End Property

    Public Property BuilderString As String
    Public Property FormName As String = String.Empty
    Public Property RecurseSubFolders As Boolean

    Private Sub BuildBranch(ByVal sDirectory As String, ByVal oNode As TreeNode)
        Dim sFolders As String()
        Dim sFiles As String()
        Dim oChild As TreeNode
        Dim oPlaceHolder As TreeNode

        If oNode.Nodes.ContainsKey("GBM_Tree_Placeholder") Then
            Try
                Cursor.Current = Cursors.WaitCursor
                treFiles.BeginUpdate()

                oNode.Nodes.RemoveByKey("GBM_Tree_Placeholder")

                sFolders = Directory.GetDirectories(sDirectory)
                sFiles = Directory.GetFiles(sDirectory)

                If sFolders.Length <> 0 Then
                    For Each sFolder As String In sFolders
                        oChild = New TreeNode(sFolder.Replace(sDirectory, String.Empty).TrimStart(Path.DirectorySeparatorChar), 0, 0)
                        oChild.Name = sFolder
                        oChild.Tag = 0
                        oNode.Nodes.Add(oChild)
                        oPlaceHolder = New TreeNode("GBM_Tree_Placeholder")
                        oPlaceHolder.Name = "GBM_Tree_Placeholder"
                        oChild.Nodes.Add(oPlaceHolder)
                    Next
                End If

                If sFiles.Length <> 0 Then
                    For Each sFile As String In sFiles
                        oChild = New TreeNode(sFile.Replace(sDirectory, String.Empty).TrimStart(Path.DirectorySeparatorChar), 1, 1)
                        oChild.Tag = 1
                        oNode.Nodes.Add(oChild)
                    Next
                End If

            Catch uaex As UnauthorizedAccessException
                'Do Nothing
            Catch ex As Exception
                mgrCommon.ShowMessage(frmIncludeExclude_ErrorFileSystemRead, ex.Message, MsgBoxStyle.Critical)
            Finally
                treFiles.EndUpdate()
                Cursor.Current = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub BuildTrunk()
        Dim oRootNode As TreeNode

        If Path.GetPathRoot(txtRootFolder.Text) = txtRootFolder.Text Then
            oRootNode = New TreeNode(txtRootFolder.Text, 0, 0)
        Else
            oRootNode = New TreeNode(Path.GetFileName(txtRootFolder.Text), 0, 0)
        End If

        treFiles.Nodes.Clear()
        oRootNode.Name = "Root"
        oRootNode.Nodes.Add("GBM_Tree_Placeholder", "GBM_Tree_Placeholder")
        treFiles.Nodes.Add(oRootNode)
        BuildBranch(txtRootFolder.Text, oRootNode)
    End Sub

    Private Sub RootPathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = txtRootFolder.Text
        Dim sNewPath As String

        If sCurrentPath <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("IE_Save_Path", frmIncludeExclude_BrowseSaveFolder, sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtRootFolder.Text = sNewPath
    End Sub

    Private Sub GetAllCheckedNodes(ByVal oRootNode As TreeNodeCollection, ByRef oNodeList As List(Of TreeNode))
        For Each oNode As TreeNode In oRootNode
            If oNode.Checked Then
                oNode.Checked = False
                oNodeList.Add(oNode)
            End If
            If oNode.Nodes.Count > 0 Then
                GetAllCheckedNodes(oNode.Nodes, oNodeList)
            End If
        Next
    End Sub

    Private Sub AddItem()
        Dim oCheckedNodes As New List(Of TreeNode)
        Dim oListViewItem As ListViewItem
        Dim sItem As String

        GetAllCheckedNodes(treFiles.Nodes, oCheckedNodes)

        lstBuilder.BeginUpdate()
        For Each oNode In oCheckedNodes
            sItem = oNode.Text
            If oNode.Tag = 1 And optFileTypes.Checked Then
                sItem = Path.GetExtension(oNode.Text)
                If sItem <> String.Empty Then
                    sItem = "*" & sItem
                    oListViewItem = New ListViewItem(sItem, 2)
                    oListViewItem.Name = sItem
                Else
                    sItem = oNode.Text
                    oListViewItem = New ListViewItem(sItem, 1)
                    oListViewItem.Name = sItem
                End If
            Else
                oListViewItem = New ListViewItem(sItem, CInt(oNode.Tag))
                oListViewItem.Name = sItem
            End If

            If Not lstBuilder.Items.ContainsKey(sItem) Then
                lstBuilder.Items.Add(oListViewItem)
            End If
        Next
        lstBuilder.EndUpdate()
        DirtyCheck()
    End Sub

    Private Sub RemoveItem()
        For Each oListViewItem As ListViewItem In lstBuilder.SelectedItems
            oListViewItem.Remove()
        Next
        DirtyCheck()
    End Sub

    Private Sub ParseBuilderString(ByVal sString As String)
        Dim iType As Integer = 1
        Dim oListViewItem As ListViewItem
        Dim sItems As String() = sString.Split(":")

        lstBuilder.BeginUpdate()
        lstBuilder.Clear()

        For Each sItem As String In sItems
            oListViewItem = New ListViewItem(sItem)
            oListViewItem.Name = sItem
            IdentifyEntry(oListViewItem, sItem)
            If Not lstBuilder.Items.ContainsKey(sItem) Then
                lstBuilder.Items.Add(oListViewItem)
            End If
        Next

        lstBuilder.EndUpdate()
    End Sub

    Private Sub IdentifyEntry(ByRef oListItem As ListViewItem, ByVal sNewLabel As String)
        Dim iType As Integer = 1
        Dim sFolderCheck As String

        If sNewLabel.Contains("*") Or sNewLabel.Contains("?") Then
            iType = 2
        Else
            If txtRootFolder.Text <> String.Empty Then
                If Path.GetFileName(txtRootFolder.Text) = sNewLabel Then
                    sFolderCheck = txtRootFolder.Text
                Else
                    sFolderCheck = txtRootFolder.Text & Path.DirectorySeparatorChar & sNewLabel
                End If
                If Directory.Exists(sFolderCheck) Then
                    iType = 0
                Else
                    iType = 1
                End If
            End If
        End If

        oListItem.ImageIndex = iType
    End Sub

    Private Function CreateNewBuilderString() As String
        Dim sTempString As String = String.Empty

        For Each oListViewItem As ListViewItem In lstBuilder.Items
            sTempString &= oListViewItem.Text & ":"
        Next

        sTempString = sTempString.TrimEnd(":")

        Return sTempString
    End Function

    Private Sub OpenRawEdit()
        Dim sCurrentString As String = CreateNewBuilderString()
        Dim sNewString As String
        sNewString = mgrCommon.ShowInputBox(frmIncludeExclude_RawEditInfo, mgrCommon.FormatString(frmIncludeExclude_RawEditTitle, FormName), sCurrentString)
        If sNewString <> String.Empty Then
            ParseBuilderString(sNewString)
        Else
            lstBuilder.Clear()
        End If
    End Sub

    Private Sub DirtyCheck()
        If BuilderString <> CreateNewBuilderString() Then IsDirty = True
    End Sub

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = mgrCommon.FormatString(frmIncludeExclude_FormName, FormName)
        Me.Icon = GBM_Icon

        'Set Form Text
        lblSaveFolder.Text = frmIncludeExclude_lblSaveFolder
        btnRawEdit.Text = frmIncludeExclude_btnRawEdit
        btnRawEdit.Image = Multi_Edit
        lblItems.Text = mgrCommon.FormatString(frmIncludeExclude_lblItems, FormName)
        grpOptions.Text = mgrCommon.FormatString(frmIncludeExclude_grpOptions, FormName)
        optFileTypes.Text = frmIncludeExclude_optFileTypes
        optIndividualFiles.Text = frmIncludeExclude_optIndividualFiles
        chkRecurseSubFolders.Text = frmIncludeExclude_chkRecurseSubFolders
        btnRemove.Text = frmIncludeExclude_btnRemove
        btnAdd.Text = frmIncludeExclude_btnAdd
        btnBrowse.Text = frmIncludeExclude_btnBrowse
        btnCancel.Text = frmIncludeExclude_btnCancel
        btnCancel.Image = Multi_Cancel
        btnSave.Text = frmIncludeExclude_btnSave
        btnSave.Image = Multi_Save
        cmsAdd.Text = frmIncludeExclude_cmsAdd
        cmsEdit.Text = frmIncludeExclude_cmsEdit
        cmsRemove.Text = frmIncludeExclude_cmsRemove

        'Set Defaults
        txtRootFolder.Text = RootFolder
        optFileTypes.Checked = True
        chkRecurseSubFolders.Checked = RecurseSubFolders
        If BuilderString <> String.Empty Then ParseBuilderString(BuilderString)
        If txtRootFolder.Text <> String.Empty Then BuildTrunk()
    End Sub

    Private Sub frmIncludeExclude_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IsLoading = True
        SetForm()
        IsLoading = False
    End Sub

    Private Sub frmIncludeExclude_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        treFiles.Select()
        If Not treFiles.SelectedNode Is Nothing Then treFiles.SelectedNode.Expand()
        If txtRootFolder.Text = String.Empty Then
            ttWarning.ToolTipTitle = frmIncludeExclude_ToolTipTitle
            ttWarning.SetToolTip(treFiles, frmIncludeExclude_ToolTipFiles)
            ttWarning.SetToolTip(txtRootFolder, frmIncludeExclude_ToolTipFolder)
            ttWarning.SetToolTip(btnBrowse, frmIncludeExclude_ToolTipBrowse)
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        RootPathBrowse()
        If txtRootFolder.Text <> String.Empty Then
            BuildTrunk()
            ttWarning.RemoveAll()
        End If
        treFiles.Select()
        If Not treFiles.SelectedNode Is Nothing Then treFiles.SelectedNode.Expand()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        IsDirty = False
        BuilderString = CreateNewBuilderString()
        RecurseSubFolders = chkRecurseSubFolders.Checked
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub treFiles_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles treFiles.BeforeExpand
        If Not e.Node.Name = "Root" Then
            BuildBranch(e.Node.Name, e.Node)
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddItem()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveItem()
    End Sub

    Private Sub cmsItems_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmsItems.Opening
        If lstBuilder.SelectedItems.Count = 0 Then
            cmsEdit.Visible = False
            cmsRemove.Visible = False
            cmsAdd.Visible = True
        Else
            cmsEdit.Visible = True
            cmsRemove.Visible = True
            cmsAdd.Visible = False
        End If
    End Sub

    Private Sub cmsEdit_Click(sender As Object, e As EventArgs) Handles cmsEdit.Click
        If lstBuilder.SelectedItems.Count > 0 Then
            lstBuilder.SelectedItems(0).BeginEdit()
        End If
        DirtyCheck()
    End Sub

    Private Sub cmsRemove_Click(sender As Object, e As EventArgs) Handles cmsRemove.Click
        RemoveItem()
    End Sub

    Private Sub cmsAdd_Click(sender As Object, e As EventArgs) Handles cmsAdd.Click
        Dim oNewItem As New ListViewItem(frmIncludeExclude_CustomItem, 1)
        oNewItem.Selected = True
        lstBuilder.Items.Add(oNewItem)
        lstBuilder.SelectedItems(0).BeginEdit()
        DirtyCheck()
    End Sub

    Private Sub lstBuilder_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles lstBuilder.AfterLabelEdit
        If Not e.Label Is Nothing Then
            If e.Label = String.Empty Then
                e.CancelEdit = True
            End If

            If lstBuilder.Items.ContainsKey(e.Label) Then
                e.CancelEdit = True
            Else
                'Unix Handler - Mono is unable to modify list items during an edit event without crashing
                If Not mgrCommon.IsUnix Then IdentifyEntry(lstBuilder.Items(e.Item), e.Label)
            End If
        End If
    End Sub

    Private Sub btnRawEdit_Click(sender As Object, e As EventArgs) Handles btnRawEdit.Click
        OpenRawEdit()
        DirtyCheck()
    End Sub

    Private Sub chkRecurseSubFolders_CheckedChanged(sender As Object, e As EventArgs) Handles chkRecurseSubFolders.CheckedChanged
        If Not IsLoading Then IsDirty = True
    End Sub

    Private Sub frmIncludeExclude_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If IsDirty Then
            Select Case mgrCommon.ConfirmDirty()
                Case MsgBoxResult.No
                    IsDirty = False
                    btnCancel.PerformClick()
                Case MsgBoxResult.Yes
                    btnSave.PerformClick()
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub frmIncludeExclude_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class