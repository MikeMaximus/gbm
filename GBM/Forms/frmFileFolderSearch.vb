Imports GBM.My.Resources
Imports System.IO

Public Class frmFileFolderSearch
    Private sSearchItem As String
    Private sGameName As String = String.Empty
    Private bIsFolder As Boolean
    Private sFoundItem As String
    Private oDrives As List(Of DriveInfo)
    Private iCurrentDrive As Integer
    Private oSearchDrive As DirectoryInfo
    Dim bShutdown As Boolean = False

    Delegate Sub UpdateInfoCallBack(ByVal sCurrentFolder As String)
    Delegate Sub UpdateResultsCallBack(ByVal sItem As String)

    Public Property GameName As String
        Get
            Return sGameName
        End Get
        Set(value As String)
            sGameName = value
        End Set
    End Property

    Public Property SearchItem As String
        Get
            Return sSearchItem
        End Get
        Set(value As String)
            sSearchItem = value
        End Set
    End Property

    Public Property FolderSearch As Boolean
        Get
            Return bIsFolder
        End Get
        Set(value As Boolean)
            bIsFolder = value
        End Set
    End Property

    Public Property FoundItem As String
        Get
            Return sFoundItem
        End Get
        Set(value As String)
            sFoundItem = value
        End Set
    End Property

    Private Sub UpdateInfo(ByVal sCurrentFolder As String)
        If txtCurrentLocation.InvokeRequired = True Then
            Dim d As New UpdateInfoCallBack(AddressOf UpdateInfo)
            Me.Invoke(d, New Object() {sCurrentFolder})
        Else
            txtCurrentLocation.Text = sCurrentFolder
        End If
    End Sub

    Private Sub UpdateResults(ByVal sItem As String)
        If lstResults.InvokeRequired = True Then
            Dim d As New UpdateInfoCallBack(AddressOf UpdateResults)
            Me.Invoke(d, New Object() {sItem})
        Else
            lstResults.Items.Add(sItem)
        End If
    End Sub

    Private Function SearchDirectory(ByVal dir As DirectoryInfo, ByVal sDirectoryName As String) As String
        Dim sSubSearch As String = String.Empty
        Dim sFoundItem As String = String.Empty

        If bwSearch.CancellationPending Then
            Return "Cancel"
        End If

        'Ignore Symlinks
        If (dir.Attributes And FileAttributes.ReparsePoint) = FileAttributes.ReparsePoint Then
            Return String.Empty
        End If

        UpdateInfo(dir.FullName)

        Try
            'Search Current Directory
            If dir.GetDirectories(sDirectoryName).Length > 0 Then
                sFoundItem = dir.FullName & Path.DirectorySeparatorChar & sDirectoryName
                UpdateResults(sFoundItem)                
            End If

            'Search Sub Directory
            Dim subdirs() As DirectoryInfo = dir.GetDirectories("*")
            For Each newDir As DirectoryInfo In subdirs
                sSubSearch = SearchDirectory(newDir, sDirectoryName)
                If sSubSearch <> String.Empty Then
                    Return sSubSearch
                End If
            Next
        Catch e As System.UnauthorizedAccessException
            'Do Nothing
        Catch e As Exception
            'Do Nothing
        End Try

        Return sSubSearch
    End Function

    Private Function SearchFile(ByVal dir As DirectoryInfo, ByVal sFileName As String) As String
        Dim sSubSearch As String = String.Empty
        Dim sFoundItem As String = String.Empty

        If bwSearch.CancellationPending Then
            Return "Cancel"
        End If

        'Ignore Symlinks
        If (dir.Attributes And FileAttributes.ReparsePoint) = FileAttributes.ReparsePoint Then
            Return String.Empty
        End If

        UpdateInfo(dir.FullName)

        Try
            'Search Current Directory
            If dir.GetFiles(sFileName).Length > 0 Then
                sFoundItem = Path.GetDirectoryName(dir.FullName & Path.DirectorySeparatorChar & sFileName)
                UpdateResults(sFoundItem)
            End If

            'Search Sub Directory
            Dim subdirs() As DirectoryInfo = dir.GetDirectories("*")
            For Each newDir As DirectoryInfo In subdirs
                sSubSearch = SearchFile(newDir, sFileName)

                If sSubSearch <> String.Empty Then
                    Return sSubSearch
                End If
            Next
        Catch e As System.UnauthorizedAccessException
            'Do Nothing
        Catch e As Exception
            'Do Nothing
        End Try

        Return sSubSearch
    End Function

    Private Sub GetDrives()
        oDrives = New List(Of DriveInfo)
        For Each oDrive As DriveInfo In My.Computer.FileSystem.Drives
            If oDrive.DriveType = IO.DriveType.Fixed Then
                oDrives.Add(oDrive)
            End If
        Next
    End Sub

    Private Sub Search(ByVal oDrive As DriveInfo)
        oSearchDrive = oDrive.RootDirectory
        bwSearch.RunWorkerAsync()
        iCurrentDrive += 1
    End Sub

    Private Sub EndSearch()
        Dim oResult As MsgBoxResult

        If FoundItem = "Cancel" Then FoundItem = String.Empty

        If oDrives.Count > iCurrentDrive And FoundItem = String.Empty Then
            oResult = mgrCommon.ShowMessage(frmFileFolderSearch_SwitchDrives, New String() {oDrives(iCurrentDrive).RootDirectory.ToString}, MsgBoxStyle.YesNo)
            If oResult = MsgBoxResult.Yes Then
                Search(oDrives(iCurrentDrive))
            End If
        Else
            SearchComplete()
        End If
    End Sub

    Private Sub SearchComplete()
        If lstResults.Items.Count = 0 Then
            bShutdown = True
            Me.Close()
        Else
            txtCurrentLocation.Text = frmFileFolderSearch_SearchComplete
            lstResults.SelectedIndex = 0
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmFileFolderSearch_FormName

        'Set Form Text
        lblResults.Text = frmFileFolderSearch_lblResults
        btnOk.Text = frmFileFolderSearch_btnOk
        btnCancel.Text = frmFileFolderSearch_btnCancel
    End Sub

    Private Sub frmFileFolderSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        GetDrives()
        Search(oDrives(iCurrentDrive))
    End Sub

    Private Sub bwSearch_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwSearch.DoWork
        If bIsFolder Then
            FoundItem = SearchDirectory(oSearchDrive, sSearchItem)
        Else
            FoundItem = SearchFile(oSearchDrive, sSearchItem)
        End If
    End Sub

    Private Sub bwSearch_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwSearch.RunWorkerCompleted
        EndSearch()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If bwSearch.IsBusy Then
            bwSearch.CancelAsync()
        Else
            bShutdown = True
            Me.Close()
        End If
    End Sub

    Private Sub frmFileFolderSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        bwSearch.CancelAsync()
        If Not bShutdown Then e.Cancel = True
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Dim sItem As String

        If lstResults.SelectedItems.Count = 1 Then
            sItem = lstResults.SelectedItem.ToString
            If mgrCommon.ShowMessage(mgrPath_ConfirmPathCorrect, New String() {GameName, sItem}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                FoundItem = sItem
                bShutdown = True
                Me.Close()
            End If
        End If
    End Sub
End Class