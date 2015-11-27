Imports System.IO

Public Class frmIncludeExclude

    Dim sFormName As String = "Include Exclude"
    Dim sRootFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

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
            sRootFolder = value
        End Set
    End Property

    Private Sub BuildTreeNodes(ByVal sDirectory As String, ByVal oNode As TreeNode)
        Dim sFolders As String()
        Dim sFiles As String()
        Dim oChild As TreeNode

        Try
            sFolders = Directory.GetDirectories(sDirectory)
            sFiles = Directory.GetFiles(sDirectory)

            If sFolders.Length <> 0 Then
                For Each sFolder As String In sFolders
                    oChild = New TreeNode(sFolder.Replace(sDirectory & "\", String.Empty), 0, 0)
                    oChild.Name = sFolder
                    oNode.Nodes.Add(oChild)
                    BuildTreeNodes(sFolder, oChild)
                Next
            End If

            If sFiles.Length <> 0 Then
                For Each sFile As String In sFiles
                    oChild = New TreeNode(sFile.Replace(sDirectory & "\", String.Empty), 1, 1)
                    oNode.Nodes.Add(oChild)
                Next
            End If
        Catch uaex As UnauthorizedAccessException
            'Do Nothing
        Catch ex As Exception
            MsgBox("An unexcepted error occured while reading the file system: " & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Game Backup Monitor")
        End Try
    End Sub

    Private Sub BuildTree()
        Cursor.Current = Cursors.WaitCursor
        treFiles.BeginUpdate()
        treFiles.Nodes.Clear()
        Dim oRootNode As TreeNode
        oRootNode = New TreeNode(Path.GetFileName(txtRootFolder.Text), 0, 0)
        treFiles.Nodes.Add(oRootNode)
        BuildTreeNodes(txtRootFolder.Text, oRootNode)
        treFiles.EndUpdate()
        Cursor.Current = Cursors.Default
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

        sNewPath = mgrCommon.OpenFolderBrowser("Choose the location of the save folder:", sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtRootFolder.Text = sNewPath
    End Sub

    Private Sub frmIncludeExclude_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = FormName
        txtRootFolder.Text = RootFolder
        BuildTree()
    End Sub

    Private Sub frmIncludeExclude_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        treFiles.Select()
        treFiles.SelectedNode.Expand()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        RootPathBrowse()
        BuildTree()
        treFiles.Select()
        treFiles.SelectedNode.Expand()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class