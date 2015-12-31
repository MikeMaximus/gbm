Imports GBM.My.Resources
Imports System.IO

Public Class frmIncludeExclude

    Dim sFormName As String = String.Empty
    Dim sRootFolder As String = String.Empty
    Dim sBuilderString As String

    Public Property BuilderString As String
        Get
            Return sBuilderString
        End Get
        Set(value As String)
            sBuilderString = value
        End Set
    End Property

    Public Property FormName As String
        Get
            Return sFormName
        End Get
        Set(value As String)
            sFormName = value
        End Set
    End Property

    Public Property RootFolder As String
        Get
            Return sRootFolder
        End Get
        Set(value As String)
            sRootFolder = value.TrimEnd("\")
        End Set
    End Property

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
                        oChild = New TreeNode(sFolder.Replace(sDirectory, String.Empty).TrimStart("\"), 0, 0)
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
                        oChild = New TreeNode(sFile.Replace(sDirectory, String.Empty).TrimStart("\"), 1, 1)
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

        sNewPath = mgrCommon.OpenFolderBrowser(frmIncludeExclude_BrowseSaveFolder, sDefaultFolder, False)

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
    End Sub

    Private Sub RemoveItem()
        For Each oListViewItem As ListViewItem In lstBuilder.SelectedItems
            oListViewItem.Remove()
        Next
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
                    sFolderCheck = txtRootFolder.Text & "\" & sNewLabel
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
        sNewString = InputBox(frmIncludeExclude_RawEditInfo, mgrCommon.FormatString(frmIncludeExclude_RawEditTitle, FormName), sCurrentString)
        If sNewString <> String.Empty Then
            ParseBuilderString(sNewString)
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = mgrCommon.FormatString(frmIncludeExclude_FormName, FormName)

        'Set Form Text
        lblSaveFolder.Text = frmIncludeExclude_lblSaveFolder
        btnRawEdit.Text = frmIncludeExclude_btnRawEdit
        lblItems.Text = mgrCommon.FormatString(frmIncludeExclude_lblItems, FormName)
        grpFileOptions.Text = mgrCommon.FormatString(frmIncludeExclude_grpFileOptions, FormName)
        optFileTypes.Text = frmIncludeExclude_optFileTypes
        optIndividualFiles.Text = frmIncludeExclude_optIndividualFiles
        btnRemove.Text = frmIncludeExclude_btnRemove
        btnAdd.Text = frmIncludeExclude_btnAdd
        btnBrowse.Text = frmIncludeExclude_btnBrowse
        btnCancel.Text = frmIncludeExclude_btnCancel
        btnSave.Text = frmIncludeExclude_btnSave
        cmsAdd.Text = frmIncludeExclude_cmsAdd
        cmsEdit.Text = frmIncludeExclude_cmsEdit
        cmsRemove.Text = frmIncludeExclude_cmsRemove

        'Set Defaults
        txtRootFolder.Text = RootFolder
        optFileTypes.Checked = True
        If BuilderString <> String.Empty Then ParseBuilderString(BuilderString)
        If txtRootFolder.Text <> String.Empty Then BuildTrunk()
    End Sub

    Private Sub frmIncludeExclude_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
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
        BuilderString = CreateNewBuilderString()
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
    End Sub

    Private Sub cmsRemove_Click(sender As Object, e As EventArgs) Handles cmsRemove.Click
        RemoveItem()
    End Sub

    Private Sub cmsAdd_Click(sender As Object, e As EventArgs) Handles cmsAdd.Click
        Dim oNewItem As New ListViewItem(frmIncludeExclude_CustomItem, 1)
        oNewItem.Selected = True
        lstBuilder.Items.Add(oNewItem)        
        lstBuilder.SelectedItems(0).BeginEdit()
    End Sub

    Private Sub lstBuilder_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles lstBuilder.AfterLabelEdit
        If Not e.Label Is Nothing Then
            If lstBuilder.Items.ContainsKey(e.Label) Then
                e.CancelEdit = True
            Else
                IdentifyEntry(lstBuilder.Items(e.Item), e.Label)
            End If
        End If
    End Sub

    Private Sub btnRawEdit_Click(sender As Object, e As EventArgs) Handles btnRawEdit.Click
        OpenRawEdit()
    End Sub
End Class