Imports System.IO

Public Class frmIncludeExclude

    Dim sFormName As String = "Include Exclude"
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

    Private Sub BuildBranch(ByVal sDirectory As String, ByVal oNode As TreeNode, ByVal bIsDriveRoot As Boolean)
        Dim sFolders As String()
        Dim sFiles As String()
        Dim oChild As TreeNode
        Dim oPlaceHolder As TreeNode

        Try
            Cursor.Current = Cursors.WaitCursor
            treFiles.BeginUpdate()

            oNode.Nodes.RemoveByKey("GBM_Tree_Placeholder")

            sFolders = Directory.GetDirectories(sDirectory)
            sFiles = Directory.GetFiles(sDirectory)

            If sFolders.Length <> 0 Then
                For Each sFolder As String In sFolders
                    If bIsDriveRoot Then
                        oChild = New TreeNode(sFolder.Replace(sDirectory, String.Empty), 0, 0)
                    Else
                        oChild = New TreeNode(sFolder.Replace(sDirectory & "\", String.Empty), 0, 0)
                    End If
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
                    If bIsDriveRoot Then
                        oChild = New TreeNode(sFile.Replace(sDirectory, String.Empty), 1, 1)
                    Else
                        oChild = New TreeNode(sFile.Replace(sDirectory & "\", String.Empty), 1, 1)
                    End If
                    oChild.Tag = 1
                    oNode.Nodes.Add(oChild)
                Next
            End If

        Catch uaex As UnauthorizedAccessException
            'Do Nothing
        Catch ex As Exception
            MsgBox("An unexcepted error occured while reading the file system: " & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Game Backup Monitor")
        Finally
            treFiles.EndUpdate()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub BuildTrunk()
        treFiles.Nodes.Clear()
        Dim oRootNode As TreeNode
        Dim bIsDriveRoot As Boolean

        If Path.GetPathRoot(txtRootFolder.Text) = txtRootFolder.Text Then
            oRootNode = New TreeNode(txtRootFolder.Text, 0, 0)
            bIsDriveRoot = True
        Else
            oRootNode = New TreeNode(Path.GetFileName(txtRootFolder.Text), 0, 0)
            bIsDriveRoot = False
        End If

        oRootNode.Name = "Root"
        treFiles.Nodes.Add(oRootNode)
        BuildBranch(txtRootFolder.Text, oRootNode, bIsDriveRoot)
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

        sNewPath = mgrCommon.OpenFolderBrowser("Choose the location of the saved game folder:", sDefaultFolder, False)

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

    Private Sub ParseBuilderString()
        Dim iType As Integer = 1
        Dim oListViewItem As ListViewItem
        Dim sItems As String() = BuilderString.Split(":")

        For Each sItem As String In sItems
            If sItem.Contains("*") Then
                iType = 2
            Else
                If txtRootFolder.Text <> String.Empty Then
                    If Directory.Exists(txtRootFolder.Text & "\" & sItem) Then
                        iType = 0
                    Else
                        iType = 1
                    End If
                End If
            End If

            oListViewItem = New ListViewItem(sItem, iType)
            oListViewItem.Name = sItem
            lstBuilder.Items.Add(oListViewItem)
        Next
    End Sub

    Private Sub CreateNewBuilderString()
        Dim sTempString As String = String.Empty

        For Each oListViewItem As ListViewItem In lstBuilder.Items
            sTempString &= oListViewItem.Text & ":"
        Next

        sTempString = sTempString.TrimEnd(":")

        BuilderString = sTempString
    End Sub

    Private Sub frmIncludeExclude_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = FormName & " Builder"        
        txtRootFolder.Text = RootFolder
        optFileTypes.Checked = True
        lblItems.Text = FormName & " Items"
        If BuilderString <> String.Empty Then ParseBuilderString()
        If txtRootFolder.Text <> String.Empty Then BuildTrunk()        
    End Sub

    Private Sub frmIncludeExclude_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        treFiles.Select()
        If Not treFiles.SelectedNode Is Nothing Then treFiles.SelectedNode.Expand()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        RootPathBrowse()
        If txtRootFolder.Text <> String.Empty Then BuildTrunk()
        treFiles.Select()
        If Not treFiles.SelectedNode Is Nothing Then treFiles.SelectedNode.Expand()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        CreateNewBuilderString()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub treFiles_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles treFiles.BeforeExpand
        If Not e.Node.Name = "Root" Then
            BuildBranch(e.Node.Name, e.Node, False)
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddItem()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        RemoveItem()
    End Sub
End Class