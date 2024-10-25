Imports GBM.My.Resources
Imports System.IO

Public Class frmFileFolderSearch
    Private Enum eStopStatus As Integer
        Cancel = 1
        ChangeDrive = 2
        FoundResult = 3
        FinishedDrive = 4
    End Enum

    Private oDrives As List(Of DriveInfo)
    Private oSearchDrive As DirectoryInfo
    Dim bShutdown As Boolean = False
    Private iStopStatus As eStopStatus

    Private Property IsLoading As Boolean = False

    Public Property GameName As String = String.Empty
    Public Property SearchItem As String
    Public Property FolderSearch As Boolean
    Public Property FoundItem As String

    Delegate Sub UpdateInfoCallBack(ByVal sCurrentFolder As String)
    Delegate Sub UpdateResultsCallBack(ByVal sItem As String)

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
            'It's possible the same location will be searched twice when using Linux.  Filter out duplicate results
            If Not lstResults.Items.Contains(sItem) Then
                lstResults.Items.Add(sItem)
            End If
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
        Dim oComboItems As New List(Of KeyValuePair(Of Integer, String))
        Dim iCount As Integer = 0

        'cboDrive
        cboDrive.ValueMember = "Key"
        cboDrive.DisplayMember = "Value"

        oDrives = New List(Of DriveInfo)
        For Each oDrive As DriveInfo In My.Computer.FileSystem.Drives
            If oDrive.DriveType = IO.DriveType.Fixed Then
                oDrives.Add(oDrive)
                oComboItems.Add(New KeyValuePair(Of Integer, String)(iCount, oDrive.RootDirectory.ToString))
                iCount += 1
            End If
        Next
        cboDrive.DataSource = oComboItems
    End Sub

    Private Sub Search(ByVal oDrive As DriveInfo)
        iStopStatus = eStopStatus.FinishedDrive
        oSearchDrive = oDrive.RootDirectory
        bwSearch.RunWorkerAsync()
    End Sub

    Private Sub EndSearch()
        If FoundItem = "Cancel" Then FoundItem = String.Empty

        Select Case iStopStatus
            Case eStopStatus.Cancel
                SearchComplete(frmFileFolderSearch_SearchCancel)
            Case eStopStatus.ChangeDrive
                Search(oDrives(cboDrive.SelectedValue))
            Case eStopStatus.FinishedDrive
                'Attempt to move onto the next drive it one exists and there's been no results
                If oDrives.Count > 1 And lstResults.Items.Count = 0 Then
                    If cboDrive.SelectedIndex = (cboDrive.Items.Count - 1) Then
                        cboDrive.SelectedIndex = 0
                    Else
                        cboDrive.SelectedIndex = cboDrive.SelectedIndex + 1
                    End If
                Else
                    SearchComplete(frmFileFolderSearch_SearchComplete)
                End If
            Case eStopStatus.FoundResult
                FoundItem = lstResults.SelectedItem.ToString
                bShutdown = True
                Me.Close()
        End Select
    End Sub

    Private Sub SearchComplete(ByVal sStopMessage As String)
        txtCurrentLocation.Text = sStopMessage
        If lstResults.Items.Count > 0 Then
            lstResults.SelectedIndex = 0
        End If
    End Sub

    Private Sub SetForm()
        'Init Dark Mode
        mgrDarkMode.SetDarkMode(Me)

        'Set Form Name
        Me.Text = frmFileFolderSearch_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        lblResults.Text = frmFileFolderSearch_lblResults
        btnOk.Text = frmFileFolderSearch_btnOk
        btnOk.Image = Multi_Ok
        btnCancel.Text = frmFileFolderSearch_btnCancel
        btnCancel.Image = Multi_Cancel
    End Sub

    Private Sub frmFileFolderSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IsLoading = True
        SetForm()
        GetDrives()
        IsLoading = False
        Search(oDrives(0))
    End Sub

    Private Sub bwSearch_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwSearch.DoWork
        If FolderSearch Then
            FoundItem = SearchDirectory(oSearchDrive, SearchItem)
        Else
            FoundItem = SearchFile(oSearchDrive, SearchItem)
        End If
    End Sub

    Private Sub bwSearch_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwSearch.RunWorkerCompleted
        EndSearch()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If bwSearch.IsBusy Then
            iStopStatus = eStopStatus.Cancel
            bwSearch.CancelAsync()
        Else
            bShutdown = True
            Me.Close()
        End If
    End Sub

    Private Sub frmFileFolderSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not bShutdown Then e.Cancel = True
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Dim sItem As String

        If lstResults.SelectedItems.Count = 1 Then
            sItem = lstResults.SelectedItem.ToString
            If mgrCommon.ShowMessage(mgrPath_ConfirmPathCorrect, New String() {GameName, sItem}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                If bwSearch.IsBusy Then
                    iStopStatus = eStopStatus.FoundResult
                    bwSearch.CancelAsync()
                Else
                    FoundItem = sItem
                    bShutdown = True
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub cboDrive_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDrive.SelectedIndexChanged
        If Not IsLoading Then
            Dim oResult As MsgBoxResult

            oResult = mgrCommon.ShowMessage(frmFileFolderSearch_SwitchDrives, New String() {oDrives(cboDrive.SelectedValue).RootDirectory.ToString}, MsgBoxStyle.YesNo)

            If oResult = MsgBoxResult.Yes Then
                If bwSearch.IsBusy Then
                    iStopStatus = eStopStatus.ChangeDrive
                    bwSearch.CancelAsync()
                Else
                    Search(oDrives(cboDrive.SelectedValue))
                End If
            Else
                iStopStatus = eStopStatus.FinishedDrive
                SearchComplete(frmFileFolderSearch_SearchCancel)
            End If
        End If
    End Sub

    Private Sub frmFileFolderSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                btnCancel.PerformClick()
        End Select
    End Sub
End Class