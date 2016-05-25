Imports GBM.My.Resources
Imports System.Net
Imports System.IO
Imports System.Security.Principal

Public Class mgrCommon

    'These need to be updated when upgrading the packaged 7z utility
    Private Shared sUtility64Hash As String = "05ACEE3BAC0C6C4E396116EF27B953F992DE8D28DD14D317977F45692304C318" 'v16.02 7za.exe x64
    Private Shared sUtility32Hash As String = "7AA7056DB4348229A288EEF49027B94C0D8D1A3C3AEDC6FA89B640334C7B37E9" 'v16.02 7za.exe x86

    Public Shared ReadOnly Property UtilityHash As String
        Get
            Select Case mgrPath.ReleaseType
                Case 64
                    Return sUtility64Hash
                Case 32
                    Return sUtility32Hash
                Case Else
                    Return sUtility32Hash
            End Select
        End Get
    End Property

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
            response.Close()
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
            Return mgrCommon_Yes
        Else
            Return mgrCommon_No
        End If
    End Function

    Public Shared Function SaveFileBrowser(ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String, ByVal sDefaultFile As String) As String
        Dim fbBrowser As New SaveFileDialog
        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
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
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
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

        'We can't search if we don't have a configuration
        If oGame.Temporary Then
            Return True
        End If

        For Each s As String In sExemptList
            If oGame.ProcessName.ToLower.Contains(s) Then bFound = True
        Next

        If bFound Or oGame.Duplicate = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsUnix() As Boolean
        If Path.DirectorySeparatorChar = "/" Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function IsElevated() As Boolean
        Dim oID As WindowsIdentity = WindowsIdentity.GetCurrent
        Dim oPrincipal As New WindowsPrincipal(oID)
        Return oPrincipal.IsInRole(WindowsBuiltInRole.Administrator)
    End Function

    Public Shared Sub RestartAsAdmin()
        Dim oProcess As New Process

        oProcess.StartInfo.FileName = Application.ExecutablePath
        oProcess.StartInfo.UseShellExecute = True
        oProcess.StartInfo.CreateNoWindow = True
        oProcess.StartInfo.Verb = "runas"

        oProcess.Start()
    End Sub

    'Delete file based on OS type
    Public Shared Sub DeleteFile(ByVal sPath As String, Optional ByVal bRecycle As Boolean = True)
        If File.Exists(sPath) Then
            If IsUnix() Then
                File.Delete(sPath)
            Else
                If bRecycle Then
                    My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                Else
                    File.Delete(sPath)
                End If
            End If
        End If
    End Sub

    'Delete directory based on OS type
    Public Shared Sub DeleteDirectory(ByVal sPath As String, Optional ByVal bRecursive As Boolean = False)
        If Directory.Exists(sPath) Then
            If IsUnix() Then
                Directory.Delete(sPath, bRecursive)
            Else
                My.Computer.FileSystem.DeleteDirectory(sPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            End If
        End If
    End Sub

    'Save string as text file
    Public Shared Sub SaveText(ByVal sText As String, ByVal sPath As String)
        Dim oStream As StreamWriter

        Try
            If File.Exists(sPath) Then DeleteFile(sPath, False)
            oStream = New StreamWriter(sPath)
            oStream.Write(sText)
            oStream.Flush()
            oStream.Close()
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorWritingTextFile, ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    'Get a file size
    Public Shared Function GetFileSize(ByVal sFile As String) As String
        Dim oFileInfo As FileInfo
        Dim dFileSize As Double

        Try
            oFileInfo = New FileInfo(sFile)
            dFileSize = oFileInfo.Length
            If dFileSize > 1048576 Then
                Return FormatString(App_MB, Math.Round(dFileSize / 1048576, 2).ToString)
            Else
                Return FormatString(App_KB, Math.Round(dFileSize / 1024, 2).ToString)
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    'Handles no extra parameters
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles single parameter stings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParam As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParam), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles multi-parameter strings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParams As String(), ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParams), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles no extra parameters
    Public Shared Function FormatString(ByVal sString As String) As String
        sString = sString.Replace("[BR]", vbCrLf)

        Return sString
    End Function


    'Handles single parameter stings
    Public Shared Function FormatString(ByVal sString As String, ByVal sParam As String) As String
        sString = sString.Replace("[BR]", vbCrLf)
        sString = sString.Replace("[PARAM]", sParam)

        Return sString
    End Function

    'Handles multi-parameter strings
    Public Shared Function FormatString(ByVal sString As String, ByVal sParams As String()) As String
        Dim iParam As Integer

        sString = sString.Replace("[BR]", vbCrLf)

        For Each s As String In sParams
            iParam = sString.IndexOf("[PARAM]")
            sString = sString.Remove(iParam, 7)
            sString = sString.Insert(iParam, s)
        Next

        Return sString
    End Function

    'Compare functions
    Public Shared Function CompareImportTagsByName(oItem1 As Tag, oItem2 As Tag) As Integer
        Return String.Compare(oItem1.Name, oItem2.Name)
    End Function

    Public Shared Function CompareByListBoxItemByValue(sItem1 As KeyValuePair(Of String, String), sItem2 As KeyValuePair(Of String, String)) As Integer
        Return String.Compare(sItem1.Value, sItem2.Value)
    End Function

    'Maintenance Only - Function for string management
    Public Shared Sub GetAllStrings(ByVal ctlParent As Control, ByRef sResource As String, ByRef sCode As String, ByVal sFormName As String)
        For Each ctl As Control In ctlParent.Controls
            If TypeOf ctl Is GroupBox Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
                GetAllStrings(ctl, sResource, sCode, sFormName)
            ElseIf TypeOf ctl Is TabControl Then
                For Each tb As TabPage In ctl.Controls
                    GetAllStrings(tb, sResource, sCode, sFormName)
                Next
            ElseIf TypeOf ctl Is Label Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is Button Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is RadioButton Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is CheckBox Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            End If
        Next
    End Sub

End Class
