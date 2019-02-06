Imports GBM.My.Resources
Imports System.Net
Imports System.IO
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Public Class mgrCommon

    'These need to be updated when upgrading the packaged 7z utility
    Private Shared sUtility64Hash As String = "8BC2A3D6C37C4DB9BD487AD35039AE0DC8A1DDF2C3B1F0B76B3E678FEBB9F223" 'v18.05 7za.exe x64
    Private Shared sUtility32Hash As String = "77613CCA716EDF68B9D5BAB951463ED7FADE5BC0EC465B36190A76299C50F117" 'v18.05 7za.exe x86

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
            Return (My.Application.Info.Version.Major * 100) + (My.Application.Info.Version.Minor * 10) + My.Application.Info.Version.Build
        End Get
    End Property

    Public Shared ReadOnly Property DisplayAppVersion As String
        Get
            Return My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build
        End Get
    End Property

    'Source - https://stackoverflow.com/questions/18873152/deep-copy-of-ordereddictionary
    Public Shared Function GenericClone(ByVal oOriginal As Object) As Object
        'Construct a temporary memory stream
        Dim oStream As MemoryStream = New MemoryStream()

        'Construct a serialization formatter that does all the hard work
        Dim oFormatter As BinaryFormatter = New BinaryFormatter()

        'This line Is explained in the "Streaming Contexts" section
        oFormatter.Context = New StreamingContext(StreamingContextStates.Clone)

        'Serialize the object graph into the memory stream
        oFormatter.Serialize(oStream, oOriginal)

        'Seek back to the start of the memory stream before deserializing
        oStream.Position = 0

        'Deserialize the graph into a New set of objects
        'Return the root of the graph (deep copy) to the caller
        Return oFormatter.Deserialize(oStream)
    End Function

    Public Shared Function SafeIconFromFile(ByVal sPath As String) As Image
        Dim oImage As Image
        Dim oReturnImage As Image
        Dim oImageSize As Size = New Size(48, 48)

        oImage = Image.FromFile(sPath)
        oReturnImage = New Bitmap(oImage, oImageSize)
        oImage.Dispose()

        Return oReturnImage
    End Function

    Public Shared Function IsAddress(ByVal sURL As String) As Boolean
        If (sURL.IndexOf("http://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sURL.IndexOf("https://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function CheckAddress(ByVal sURL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(sURL)
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

    Public Shared Function SaveFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String,
                                           ByVal sDefaultFile As String, Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New SaveFileDialog
        Dim oSavedPath As New clsSavedPath

        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
        fbBrowser.FileName = sDefaultFile
        fbBrowser.InitialDirectory = sDefaultFolder

        If bSavedPath Then
            oSavedPath = mgrSavedPath.GetPathByName(sName)
            If oSavedPath.Path <> String.Empty Then
                If Directory.Exists(oSavedPath.Path) Then
                    fbBrowser.InitialDirectory = oSavedPath.Path
                End If
            End If
        End If

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If bSavedPath Then
                oSavedPath.PathName = sName
                oSavedPath.Path = Path.GetDirectoryName(fbBrowser.FileName)
                mgrSavedPath.AddUpdatePath(oSavedPath)
            End If

            Return fbBrowser.FileName
            End If

            Return String.Empty
    End Function

    Private Shared Function BuildFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String,
                                             ByVal bMulti As Boolean, ByRef fbBrowser As OpenFileDialog, Optional ByVal bSavedPath As Boolean = True) As Boolean

        Dim oSavedPath As New clsSavedPath

        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
        fbBrowser.Multiselect = bMulti
        fbBrowser.InitialDirectory = sDefaultFolder

        If bSavedPath Then
            oSavedPath = mgrSavedPath.GetPathByName(sName)
            If oSavedPath.Path <> String.Empty Then
                If Directory.Exists(oSavedPath.Path) Then
                    fbBrowser.InitialDirectory = oSavedPath.Path
                End If
            End If
        End If

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If bSavedPath Then
                oSavedPath.PathName = sName
                oSavedPath.Path = Path.GetDirectoryName(fbBrowser.FileName)
                mgrSavedPath.AddUpdatePath(oSavedPath)
            End If

            Return True
        End If

        Return False
    End Function

    Public Shared Function OpenFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String,
                                           Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New OpenFileDialog
        Dim bResult As Boolean

        bResult = BuildFileBrowser(sName, sTitle, sExtension, sFileType, sDefaultFolder, False, fbBrowser, bSavedPath)

        If bResult Then
            Return fbBrowser.FileName
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenMultiFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String,
                                                Optional ByVal bSavedPath As Boolean = True) As String()
        Dim fbBrowser As New OpenFileDialog
        Dim bResult As Boolean

        bResult = BuildFileBrowser(sName, sTitle, sExtension, sFileType, sDefaultFolder, True, fbBrowser, bSavedPath)

        If bResult Then
            Return fbBrowser.FileNames
        End If

        Return New String() {}
    End Function

    Public Shared Function OpenFolderBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sDefaultFolder As String, ByVal bEnableNewFolder As Boolean,
                                             Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New FolderBrowserDialog
        Dim oSavedPath As New clsSavedPath

        fbBrowser.Description = sTitle
        fbBrowser.SelectedPath = sDefaultFolder
        fbBrowser.ShowNewFolderButton = bEnableNewFolder

        If bSavedPath Then
            oSavedPath = mgrSavedPath.GetPathByName(sName)
            If oSavedPath.Path <> String.Empty Then
                If Directory.Exists(oSavedPath.Path) Then
                    fbBrowser.SelectedPath = oSavedPath.Path
                End If
            End If
        End If

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If bSavedPath Then
                oSavedPath.PathName = sName
                oSavedPath.Path = fbBrowser.SelectedPath
                mgrSavedPath.AddUpdatePath(oSavedPath)
            End If

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

        If bFound Then
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

    Public Shared Function GetCurrentOS() As clsGame.eOS
        If IsUnix() Then
            Return clsGame.eOS.Linux
        Else
            Return clsGame.eOS.Windows
        End If
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

    'Get a file size
    Public Shared Function GetFileSize(ByVal sFile As String) As Long
        Dim oFileInfo As FileInfo
        Dim dFileSize As Long = 0

        Try
            oFileInfo = New FileInfo(sFile)
            dFileSize = oFileInfo.Length
        Catch ex As Exception
            'Do Nothing
        End Try

        Return dFileSize
    End Function

    Public Shared Function WildcardToRegex(ByVal sPattern As String) As String
        Dim sRegEx As String
        sPattern = Regex.Escape(sPattern)
        sRegEx = sPattern.Replace("\*", ".*")
        sRegEx = sRegEx.Replace("\?", ".")
        Return sRegEx
    End Function

    Public Shared Function CompareValueToArrayRegEx(ByVal sValue As String, ByVal sValues As String()) As Boolean
        For Each se As String In sValues
            If Regex.IsMatch(sValue, WildcardToRegex(se)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Function IsRegExValid(ByVal sPattern As String) As Boolean
        Dim oRegEx As Regex
        Try
            oRegEx = New Regex(sPattern)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Calculate the current size of a folder
    Public Shared Function GetFolderSize(ByVal sPath As String, ByVal sInclude As String(), ByVal sExclude As String(), Optional ByVal b7zStyleRecurse As Boolean = True) As Long
        Dim oFolder As DirectoryInfo
        Dim bInclude As Boolean
        Dim bExclude As Boolean
        Dim lSize As Long = 0

        Try
            If Not Directory.Exists(sPath) Then Return lSize

            oFolder = New DirectoryInfo(sPath)

            'Files
            For Each fi As FileInfo In oFolder.EnumerateFiles()

                If sInclude.Length > 0 Then
                    bInclude = CompareValueToArrayRegEx(fi.FullName, sInclude)
                Else
                    bInclude = True
                End If

                If sExclude.Length > 0 Then
                    bExclude = CompareValueToArrayRegEx(fi.FullName, sExclude)
                Else
                    bExclude = False
                End If

                If bInclude And Not bExclude Then
                    lSize += fi.Length
                End If
            Next

            'Sub Folders
            For Each di As DirectoryInfo In oFolder.EnumerateDirectories()
                If Not ((di.Attributes And FileAttributes.ReparsePoint) = FileAttributes.ReparsePoint) Then
                    If b7zStyleRecurse Then
                        If sExclude.Length > 0 Then
                            bExclude = CompareValueToArrayRegEx(di.FullName, sExclude)
                        Else
                            bExclude = False
                        End If

                        If Not bExclude Then
                            lSize += GetFolderSize(di.FullName, sInclude, sExclude)
                        End If
                    Else
                        If sInclude.Length > 0 Then
                            bInclude = CompareValueToArrayRegEx(di.FullName, sInclude)
                        Else
                            bInclude = True
                        End If

                        If sExclude.Length > 0 Then
                            bExclude = CompareValueToArrayRegEx(di.FullName, sExclude)
                        Else
                            bExclude = False
                        End If

                        If bInclude And Not bExclude Then
                            lSize += GetFolderSize(di.FullName, sInclude, sExclude)
                        End If
                    End If
                End If
            Next

        Catch
            'Do Nothing
        End Try

        Return lSize
    End Function

    'Format Disk Space Amounts
    Public Shared Function FormatDiskSpace(ByVal lSize As Long)

        Select Case lSize
            Case >= 1125899906842624
                Return FormatString(mgrCommon_PB, Math.Round(lSize / 1125899906842624, 2))
            Case >= 1099511627776
                Return FormatString(mgrCommon_TB, Math.Round(lSize / 1099511627776, 2))
            Case >= 1073741824
                Return FormatString(mgrCommon_GB, Math.Round(lSize / 1073741824, 2))
            Case >= 1048576
                Return FormatString(mgrCommon_MB, Math.Round(lSize / 1048576, 2))
            Case >= 1024
                Return FormatString(mgrCommon_KB, Math.Round(lSize / 1024, 2))
            Case >= 0
                Return FormatString(mgrCommon_B, lSize)
        End Select

        Return lSize
    End Function

    'Get available disk space on a drive (Unix)
    Private Shared Function GetAvailableDiskSpaceUnix(ByVal sPath As String) As Long
        Dim prsdf As Process
        Dim sOutput As String
        Dim sAvailableSpace As String
        Try
            prsdf = New Process
            prsdf.StartInfo.FileName = "/bin/df"
            prsdf.StartInfo.Arguments = sPath
            prsdf.StartInfo.UseShellExecute = False
            prsdf.StartInfo.RedirectStandardOutput = True
            prsdf.StartInfo.CreateNoWindow = True
            prsdf.Start()
            sOutput = prsdf.StandardOutput.ReadToEnd
            'Parse df output to grab "Available" value
            sAvailableSpace = sOutput.Split(vbLf)(1).Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)(3)
            'Return value in bytes
            Return CLng(sAvailableSpace) * 1024
        Catch
            Return 0
        End Try
    End Function

    'Get available disk space on a drive (Windows)
    Private Shared Function GetAvailableDiskSpaceWindows(ByVal sPath As String) As Long
        Dim oDrive As DriveInfo
        Dim lAvailableSpace As Long = 0
        Try
            oDrive = New DriveInfo(Path.GetPathRoot(sPath))
            lAvailableSpace = oDrive.AvailableFreeSpace
        Catch
            'Do Nothing
        End Try
        Return lAvailableSpace
    End Function

    'Get available disk space on a drive 
    Public Shared Function GetAvailableDiskSpace(ByVal sPath As String) As Long
        If IsUnix() Then
            Return GetAvailableDiskSpaceUnix(sPath)
        Else
            Return GetAvailableDiskSpaceWindows(sPath)
        End If
    End Function

    'Copy a file
    Public Shared Function CopyFile(ByVal sSourcePath As String, ByVal sDestinationPath As String, ByVal bOverWrite As Boolean) As Boolean
        Try
            If File.Exists(sSourcePath) Then
                File.Copy(sSourcePath, sDestinationPath, bOverWrite)
            Else
                Return False
            End If
        Catch
            Return False
        End Try

        Return True
    End Function

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

    'Delete a sub-folder based on the provided backup information
    Public Shared Sub DeleteDirectoryByBackup(ByVal sBackupFolder As String, ByVal oBackup As clsBackup)
        Dim oDir As DirectoryInfo
        Dim sDir As String = sBackupFolder & oBackup.MonitorID

        'Check if the sub-folder is an ID or Name
        If oBackup.FileName.StartsWith(oBackup.MonitorID & Path.DirectorySeparatorChar) Then
            sDir = sBackupFolder & oBackup.MonitorID
        ElseIf oBackup.FileName.StartsWith(oBackup.FileSafeName & Path.DirectorySeparatorChar) Then
            sDir = sBackupFolder & oBackup.FileSafeName
        Else
            Exit Sub
        End If

        'Delete sub directory if it's empty
        If Directory.Exists(sDir) Then
            'Check if there's any sub-directories or files remaining
            oDir = New DirectoryInfo(sDir)
            If oDir.GetDirectories.Length = 0 And oDir.GetFiles.Length = 0 Then
                'Folder is empty,  delete the empty sub-folder
                If Directory.Exists(sDir) Then DeleteDirectory(sDir)
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
