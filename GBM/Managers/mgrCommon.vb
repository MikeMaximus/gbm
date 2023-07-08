Imports GBM.My.Resources
Imports System.Net
Imports System.IO
Imports System.Reflection
Imports System.Security.Principal
Imports System.Text.RegularExpressions

Public Class mgrCommon

    'These need to be updated when upgrading the packaged 7z utility
    Private Shared sUtility64Hash As String = "356BEA8B6E9EB84DFA0DD8674E7C03428C641A47789DF605C5BEA0730DE4AED2" 'v23.01 7za.exe x64
    Private Shared sUtility32Hash As String = "F00836A63BE7EBF14E1B8C40100C59777FE3432506B330927EA1F1B7FD47EE44" 'v23.01 7za.exe x86
    Private Shared sBlackList As String() = {"dosbox", "scummvm", "java", "python", "python.real", "python2.7", "mono", "wine"}

    Public Enum eSounds As Integer
        Success = 1
        Failure = 2
    End Enum

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

    Public Shared Function SafeIconFromFile(ByVal sPath As String) As Image
        Dim oImage As Image
        Dim oReturnImage As Image
        Dim oImageSize As Size = New Size(48, 48)

        Try
            If Path.GetExtension(sPath).ToLower = ".exe" Then
                oReturnImage = GetIconFromExecutable(sPath)
            Else
                oImage = Image.FromFile(sPath)
                oReturnImage = New Bitmap(oImage, oImageSize)
                oImage.Dispose()
            End If
        Catch
            oReturnImage = Multi_Unknown
        End Try

        Return oReturnImage
    End Function

    Public Shared Function GetIconFromExecutable(ByVal sFileName As String) As Bitmap
        Dim ic As Icon
        Dim oBitmap As Bitmap

        Try
            'Grab icon from the executable
            ic = System.Drawing.Icon.ExtractAssociatedIcon(sFileName)
            'Set the icon, we need to use an intermediary object to prevent file locking
            oBitmap = New Bitmap(ic.ToBitmap)
            ic.Dispose()
        Catch
            oBitmap = Multi_Unknown
        End Try

        Return oBitmap
    End Function

    Public Shared Function GetCachedIconPath(ByVal sID As String) As String
        Return mgrSettings.TemporaryFolder & Path.DirectorySeparatorChar & sID & ".png"
    End Function

    Public Shared Sub PlaySound(ByVal eSound As eSounds)
        Dim sBasePath As String = Application.StartupPath & Path.DirectorySeparatorChar & "Sounds" & Path.DirectorySeparatorChar

        Try
            Select Case eSound
                Case eSounds.Success
                    My.Computer.Audio.Play(sBasePath & "Success.wav", AudioPlayMode.Background)
                Case eSounds.Failure
                    My.Computer.Audio.Play(sBasePath & "Failure.wav", AudioPlayMode.Background)
            End Select
        Catch
            'Do Nothing
        End Try
    End Sub

    Public Shared Function IsURI(ByVal sURI As String) As Boolean
        If (sURI.IndexOf("://", 1, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sURI.IndexOf("://", 1, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function IsAddress(ByVal sURL As String) As Boolean
        If (sURL.IndexOf("http://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sURL.IndexOf("https://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Sub SetTLSVersion()
        'Force TLS 1.2 for all HTTPS connections
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
    End Sub

    Public Shared Function CheckAddressForUpdates(ByVal sURL As String, ByRef sETag As String) As Boolean
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse

        Try
            request = WebRequest.Create(sURL)
            request.Headers.Set("If-None-Match", sETag)
            response = request.GetResponse()
            If response.StatusCode = HttpStatusCode.OK Then
                sETag = response.Headers.Get("ETag")
            End If
            response.Close()
            Return True
        Catch exWeb As WebException
            If Not exWeb.Response Is Nothing Then
                If Not DirectCast(exWeb.Response, HttpWebResponse).StatusCode = HttpStatusCode.NotModified Then
                    ShowMessage(mgrCommon_ErrorUnexpectedWebResponse, New String() {DirectCast(exWeb.Response, HttpWebResponse).StatusCode, sURL}, MsgBoxStyle.Critical)
                End If
                exWeb.Response.Close()
            Else
                ShowMessage(mgrCommon_ErrorAccessingWebLocation, New String() {sURL, exWeb.Message}, MsgBoxStyle.Critical)
            End If
        End Try

        Return False
    End Function

    Public Shared Function CheckAddress(ByVal sURL As String) As Boolean
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse

        Try
            request = WebRequest.Create(sURL)
            response = request.GetResponse()
            response.Close()
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorAccessingWebLocation, New String() {sURL, ex.Message}, MsgBoxStyle.Critical)
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

    Private Shared Function BuildBrowserFilter(ByVal oFileTypes As SortedList)
        Dim sFilter As String = String.Empty

        For Each de As DictionaryEntry In oFileTypes
            sFilter &= FormatString(mgrCommon_FilesFilter, New String() {de.Key, de.Value, de.Value}) & "|"
        Next

        sFilter &= FormatString(mgrCommon_FilesFilterAll)

        Return sFilter
    End Function

    Public Shared Function SaveFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal oFileTypes As SortedList, ByVal iFilterIndex As Integer, ByVal sDefaultFolder As String,
                                           ByVal sDefaultFile As String, Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New SaveFileDialog
        Dim oSavedPath As New clsSavedPath

        fbBrowser.Title = sTitle
        fbBrowser.Filter = BuildBrowserFilter(oFileTypes)
        fbBrowser.FilterIndex = iFilterIndex
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

    Private Shared Function BuildFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal oFileTypes As SortedList, ByVal iFilterIndex As Integer, ByVal sDefaultFolder As String,
                                             ByVal bMulti As Boolean, ByRef fbBrowser As OpenFileDialog, Optional ByVal bSavedPath As Boolean = True) As Boolean

        Dim oSavedPath As New clsSavedPath

        fbBrowser.Title = sTitle
        fbBrowser.Filter = BuildBrowserFilter(oFileTypes)
        fbBrowser.FilterIndex = iFilterIndex
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

    Private Shared Function BuildFolderBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sDefaultFolder As String, ByRef fbBrowser As OpenFileDialog, Optional ByVal bSavedPath As Boolean = True) As Boolean

        Dim oSavedPath As New clsSavedPath

        fbBrowser.Title = sTitle
        fbBrowser.InitialDirectory = sDefaultFolder
        fbBrowser.ValidateNames = False
        fbBrowser.CheckFileExists = False
        fbBrowser.CheckPathExists = True
        fbBrowser.FileName = mgrCommon_FolderSelection

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

    Public Shared Function ConfirmDirty() As MsgBoxResult
        Return ShowMessage(App_ConfirmDirty, MsgBoxStyle.YesNoCancel)
    End Function

    Public Shared Function ConfirmDirtyWizard() As MsgBoxResult
        Return ShowMessage(App_ConfirmDirtyWizard, MsgBoxStyle.YesNo)
    End Function

    Public Shared Function OpenFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal oFileTypes As SortedList, ByVal iFilterIndex As Integer, ByVal sDefaultFolder As String,
                                           Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New OpenFileDialog
        Dim bResult As Boolean

        bResult = BuildFileBrowser(sName, sTitle, oFileTypes, iFilterIndex, sDefaultFolder, False, fbBrowser, bSavedPath)

        If bResult Then
            Return fbBrowser.FileName
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenMultiFileBrowser(ByVal sName As String, ByVal sTitle As String, ByVal oFileTypes As SortedList, ByVal iFilterIndex As Integer, ByVal sDefaultFolder As String,
                                                Optional ByVal bSavedPath As Boolean = True) As String()
        Dim fbBrowser As New OpenFileDialog
        Dim bResult As Boolean

        bResult = BuildFileBrowser(sName, sTitle, oFileTypes, iFilterIndex, sDefaultFolder, True, fbBrowser, bSavedPath)

        If bResult Then
            Return fbBrowser.FileNames
        End If

        Return New String() {}
    End Function

    Public Shared Function OpenFolderBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sDefaultFolder As String, Optional ByVal bSavedPath As Boolean = True) As String
        Dim fbBrowser As New OpenFileDialog
        Dim bResult As Boolean

        bResult = BuildFolderBrowser(sName, sTitle, sDefaultFolder, fbBrowser, bSavedPath)

        If bResult Then
            Return Path.GetDirectoryName(fbBrowser.FileName)
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenClassicFolderBrowser(ByVal sName As String, ByVal sTitle As String, ByVal sDefaultFolder As String, ByVal bEnableNewFolder As Boolean,
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
        Dim bFound As Boolean = False

        If oGame.ProcessName = String.Empty Or oGame.IsRegEx Then Return True

        For Each s As String In sBlackList
            If oGame.ProcessName.ToLower.Contains(s) Then bFound = True
        Next

        If bFound Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsProcessNotLaunchable(ByVal oGame As clsGame) As Boolean
        Dim bFound As Boolean = False

        If oGame.ProcessName = String.Empty Or oGame.ProcessPath = String.Empty Or oGame.IsRegEx Then Return True

        If IsUnix() And oGame.OS = clsGame.eOS.Windows Then Return True

        For Each s As String In sBlackList
            If oGame.ProcessName.ToLower.Contains(s) Then
                If oGame.Parameter = String.Empty Then
                    bFound = True
                End If
            End If
        Next

        If bFound Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsUnix() As Boolean
        If Type.GetType("Mono.Runtime") Is Nothing Then
            Return False
        End If
        Return True
    End Function

    Public Shared Function GetArchitecture() As ProcessorArchitecture
        Dim iProcessType As ProcessorArchitecture
        iProcessType = AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture
        Return iProcessType
    End Function

    Public Shared Function GetFrameworkInfo() As String
        If IsUnix() Then
            Dim oType As Type
            Dim oMethod As MethodInfo

            Try
                oType = Type.GetType("Mono.Runtime")
                oMethod = oType.GetMethod("GetDisplayName", BindingFlags.NonPublic Or BindingFlags.Static)
                Return FormatString(mgrCommon_Mono, oMethod.Invoke(Nothing, Nothing))
            Catch
                Return String.Empty
            End Try
        Else
            Dim oKey As Microsoft.Win32.RegistryKey
            Dim sRegKey As String

            Select Case GetArchitecture()
                Case ProcessorArchitecture.Amd64, ProcessorArchitecture.IA64
                    sRegKey = "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"
                Case ProcessorArchitecture.X86
                    sRegKey = "SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v4\Full"
                Case Else
                    Return String.Empty
            End Select

            Try
                oKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sRegKey)
                If (Not oKey Is Nothing) And (Not oKey.GetValue("Release") Is Nothing) Then
                    If Not oKey.GetValue("Version") Is Nothing Then
                        Return FormatString(mgrCommon_DotNet, oKey.GetValue("Version").ToString)
                    Else
                        Return FormatString(mgrCommon_DotNet, ">4.5")
                    End If
                Else
                    Return FormatString(mgrCommon_DotNet, "<4.5")
                End If
            Catch
                Return String.Empty
            End Try
        End If
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

    Public Shared Function RestartAsAdmin() As Boolean
        Dim oProcess As New Process
        Try
            oProcess.StartInfo.FileName = Application.ExecutablePath
            oProcess.StartInfo.UseShellExecute = True
            oProcess.StartInfo.CreateNoWindow = True
            oProcess.StartInfo.Verb = "runas"

            oProcess.Start()
            Return True
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorRestartAsAdmin, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

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

    'Get a list of files in the provided folder path
    Public Shared Function GetFileListByFolder(ByVal sPath As String, ByVal sSearchFilters As String()) As List(Of String)
        Dim sFiles As New List(Of String)
        Dim oFolder As DirectoryInfo
        Dim bMatch As Boolean
        Dim lSize As Long = 0

        Try
            If Not Directory.Exists(sPath) Then Return sFiles

            oFolder = New DirectoryInfo(sPath)

            'Files
            For Each fi As FileInfo In oFolder.EnumerateFiles()
                bMatch = CompareValueToArrayRegEx(fi.FullName, sSearchFilters)

                If bMatch Then
                    sFiles.Add(fi.FullName)
                End If
            Next

            'Folders
            For Each di As DirectoryInfo In oFolder.EnumerateDirectories()
                If Not ((di.Attributes And FileAttributes.ReparsePoint) = FileAttributes.ReparsePoint) Then
                    For Each sFile In GetFileListByFolder(di.FullName, sSearchFilters)
                        sFiles.Add(sFile)
                    Next
                End If
            Next

        Catch
            'Do Nothing
        End Try

        Return sFiles
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
            prsdf.StartInfo.Arguments = """" & sPath & """"
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

    'Move a file
    Public Shared Function MoveFile(ByVal sSourcePath As String, ByVal sDestinationPath As String, ByVal bOverWrite As Boolean) As Boolean
        Try
            If File.Exists(sSourcePath) Then
                My.Computer.FileSystem.MoveFile(sSourcePath, sDestinationPath, bOverWrite)
            End If
        Catch
            Return False
        End Try

        Return True
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
                If mgrSettings.DeleteToRecycleBin And bRecycle Then
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
                If mgrSettings.DeleteToRecycleBin Then
                    My.Computer.FileSystem.DeleteDirectory(sPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                Else
                    Directory.Delete(sPath, bRecursive)
                End If
            End If
        End If
    End Sub

    'Delete a directory if it's empty
    Public Shared Sub DeleteEmptyDirectory(ByVal sDir As String)
        Dim oDir As DirectoryInfo

        If Directory.Exists(sDir) Then
            'Check if there's any sub-directories or files remaining
            oDir = New DirectoryInfo(sDir)
            If oDir.GetDirectories.Length = 0 And oDir.GetFiles.Length = 0 Then
                'Folder is empty
                If Directory.Exists(sDir) Then DeleteDirectory(sDir)
            End If
        End If
    End Sub

    'Opens a file, folder or URL in default application determined by the OS
    Public Shared Function OpenInOS(ByVal sItem As String, Optional ByVal sNotFoundError As String = "", Optional ByVal sIsURL As Boolean = False) As Boolean
        Dim oProcessStartInfo As ProcessStartInfo

        If File.Exists(sItem) Or Directory.Exists(sItem) Or sIsURL Then
            Try
                oProcessStartInfo = New ProcessStartInfo
                If IsUnix() Then
                    oProcessStartInfo.FileName = "/usr/bin/xdg-open"
                    oProcessStartInfo.Arguments = """" & sItem & """"
                Else
                    oProcessStartInfo.FileName = sItem
                End If
                oProcessStartInfo.UseShellExecute = True
                oProcessStartInfo.Verb = "open"
                Process.Start(oProcessStartInfo)
            Catch ex As Exception
                mgrCommon.ShowMessage(App_ErrorLaunchExternal, ex.Message, MsgBoxStyle.Exclamation)
                Return False
            End Try
        Else
            mgrCommon.ShowMessage(sNotFoundError, MsgBoxStyle.Exclamation)
            Return False
        End If

        Return True
    End Function

    'Read text from a location, use local cache when appropriate for web locations.
    Public Shared Function ReadTextFromCache(ByVal sLocation As String, Optional ByRef lLastModified As Long = 0) As StreamReader
        Dim oReader As StreamReader
        Dim oURL As Uri
        Dim sContent As String = String.Empty
        Dim sCachedFile As String
        Dim sETagFile As String
        Dim sETag As String = String.Empty

        If mgrCommon.IsAddress(sLocation) Then
            'Set local file locations
            oURL = New Uri(sLocation)
            sCachedFile = mgrSettings.TemporaryFolder & Path.DirectorySeparatorChar & oURL.Segments(oURL.Segments.Length - 1)
            sETagFile = mgrSettings.TemporaryFolder & Path.DirectorySeparatorChar & Path.GetFileNameWithoutExtension(sCachedFile) & ".etag"

            'Check for a cached file with ETag
            If File.Exists(sCachedFile) And File.Exists(sETagFile) Then
                ReadText(sETag, sETagFile)
            End If

            'Check location
            If CheckAddressForUpdates(sLocation, sETag) Then
                'Download updated file
                If mgrCommon.ReadText(sContent, sLocation) Then
                    'Save File
                    If SaveText(sContent, sCachedFile) Then
                        'Save ETag
                        SaveText(sETag, sETagFile)
                    End If
                End If
            End If

            'Always use the cached file
            sLocation = sCachedFile
        End If

        'Provide last modified date to caller
        lLastModified = mgrCommon.DateToUnix(File.GetLastWriteTime(sLocation))

        oReader = New StreamReader(sLocation)
        Return oReader
    End Function

    'Read text from location
    Public Shared Function ReadText(ByRef sContent As String, ByVal sLocation As String) As Boolean
        Dim oReader As StreamReader
        Dim oWebClient As New WebClient

        Try
            oReader = New StreamReader(oWebClient.OpenRead(sLocation))
            sContent = oReader.ReadToEnd
            oReader.Close()
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorReadingTextFile, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

        Return True
    End Function

    'Save string as a local text file
    Public Shared Function SaveText(ByVal sText As String, ByVal sPath As String) As Boolean
        Dim oStream As StreamWriter
        Try
            oStream = New StreamWriter(sPath, False)
            oStream.Write(sText)
            oStream.Flush()
            oStream.Close()
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorWritingTextFile, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

        Return True
    End Function

    'Open a nice button sub-menu
    Public Shared Sub OpenButtonSubMenu(ByRef cms As ContextMenuStrip, ByRef btn As Button)
        cms.Show(btn, New Drawing.Point(btn.Size.Width - Math.Floor(btn.Size.Width * 0.1), btn.Size.Height - Math.Floor(btn.Size.Height * 0.5)), ToolStripDropDownDirection.AboveRight)
    End Sub

    'Configure a fake form to trigger focus for priority messages
    Private Shared Sub ConfigureFakeForm(ByRef frm As Form)
        frm.FormBorderStyle = FormBorderStyle.None
        frm.ShowInTaskbar = False
        frm.Size = New Size(0, 0)
        'We need to display it off-screen to hide it,  setting the visiblity to false doesn't work in Mono.
        frm.StartPosition = FormStartPosition.Manual
        frm.Location = New Point(-100, -100)
        frm.Show()
        frm.Focus()
        frm.BringToFront()
        frm.TopMost = True
    End Sub

    'Handles no extra parameters
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles no extra parameters
    Public Shared Function ShowPriorityMessage(ByVal sMsg As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim frmFake As Form

        'Create a fake mostly invisible form to get top focus
        frmFake = New Form
        ConfigureFakeForm(frmFake)

        Dim oResult As MsgBoxResult
        oResult = ShowMessage(sMsg, oType)

        frmFake.TopMost = False
        frmFake.Dispose()

        Return oResult
    End Function

    'Handles single parameter stings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParam As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParam), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles single parameter stings
    Public Shared Function ShowPriorityMessage(ByVal sMsg As String, ByVal sParam As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim frmFake As Form

        'Create a fake mostly invisible form to get top focus
        frmFake = New Form
        ConfigureFakeForm(frmFake)

        Dim oResult As MsgBoxResult
        oResult = ShowMessage(sMsg, sParam, oType)

        frmFake.TopMost = False
        frmFake.Dispose()

        Return oResult
    End Function

    'Handles multi-parameter strings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParams As String(), ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParams), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles multi-parameter strings
    Public Shared Function ShowPriorityMessage(ByVal sMsg As String, ByVal sParams As String(), ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim frmFake As Form

        'Create a fake mostly invisible form to get top focus
        frmFake = New Form
        ConfigureFakeForm(frmFake)

        Dim oResult As MsgBoxResult
        oResult = ShowMessage(sMsg, sParams, oType)

        frmFake.TopMost = False
        frmFake.Dispose()

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

    'Escape the ampersand character, for use with dynamic controls that don't have the UseMnemonic property
    Public Shared Function EscapeAmpersand(ByVal sItem As String, Optional ByVal bIsToolTip As Boolean = False) As String
        Dim sNewValue As String

        'Ampersands displayed in a tooltip require a double escape
        If bIsToolTip Then
            sNewValue = "&&&"
        Else
            sNewValue = "&&"
        End If

        Return sItem.Replace("&", sNewValue)
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
