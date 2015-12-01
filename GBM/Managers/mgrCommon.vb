Imports System.Net

Public Class mgrCommon

    Public Shared ReadOnly Property BuildVersion As Integer
        Get
            Return My.Application.Info.Version.Build
        End Get
    End Property

    Public Shared ReadOnly Property AppVersion As Integer
        Get
            Return (My.Application.Info.Version.Major * 100) + My.Application.Info.Version.Minor
        End Get
    End Property

    Public Shared Function CheckAddress(ByVal URL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Shared Function DateToUnix(ByVal dDate As DateTime) As Int64
        Return DateDiff(DateInterval.Second, #1/1/1970#, dDate)
    End Function

    Public Shared Function UnixToDate(ByVal iDate As Int64) As DateTime
        Return DateAdd(DateInterval.Second, iDate, #1/1/1970#)
    End Function

    Public Shared Function BooleanYesNo(ByVal bBool As Boolean) As String
        If bBool Then
            Return "Yes"
        Else
            Return "No"
        End If
    End Function

    Public Shared Function SaveFileBrowser(ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String, ByVal sDefaultFile As String) As String
        Dim fbBrowser As New SaveFileDialog
        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = sFileType & " files (*." & sExtension & ")|*." & sExtension
        fbBrowser.InitialDirectory = sDefaultFolder
        fbBrowser.FileName = sDefaultFile

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fbBrowser.FileName
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenFileBrowser(ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String, ByVal bMulti As Boolean) As String
        Dim fbBrowser As New OpenFileDialog
        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = sFileType & " files (*." & sExtension & ")|*." & sExtension
        fbBrowser.InitialDirectory = sDefaultFolder
        fbBrowser.Multiselect = bMulti

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If bMulti Then
                Dim sFileNames As String = String.Empty
                For Each sFileName As String In fbBrowser.FileNames
                    sFileNames &= sFileName & "|"
                Next
                sFileNames = sFileNames.TrimEnd("|")
                Return sFileNames
            Else
                Return fbBrowser.FileName
            End If
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenFolderBrowser(ByVal sTitle As String, ByVal sDefaultFolder As String, ByVal bEnableNewFolder As Boolean) As String
        Dim fbBrowser As New FolderBrowserDialog
        fbBrowser.Description = sTitle
        fbBrowser.SelectedPath = sDefaultFolder
        fbBrowser.ShowNewFolderButton = bEnableNewFolder
        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fbBrowser.SelectedPath
        End If

        Return String.Empty
    End Function

    Public Shared Function IsProcessNotSearchable(ByVal oGame As clsGame) As Boolean
        Dim sExemptList As String() = {"dosbox", "scummvm"}
        Dim bFound As Boolean = False

        For Each s As String In sExemptList
            If oGame.ProcessName.ToLower.Contains(s) Then bFound = True
        Next

        If bFound Or oGame.Duplicate = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsElevated() As Boolean
        If My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub RestartAsAdmin()
        Dim oProcess As New Process

        oProcess.StartInfo.FileName = Application.ExecutablePath
        oProcess.StartInfo.UseShellExecute = True
        oProcess.StartInfo.CreateNoWindow = True
        oProcess.StartInfo.Verb = "runas"

        oProcess.Start()
    End Sub

End Class
