﻿Imports GBM.My.Resources
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class mgrPath
    'Important Note: Any changes to SettingsRoot & DatabaseLocation need to be mirrored in frmMain.vb -> VerifyGameDataPath
    Private Shared sRemoteDatabaseLocation As String
    Private Shared ReadOnly oReleaseType As ProcessorArchitecture = AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture

    Private Shared Property CustomVariables As Hashtable

    Public Shared ReadOnly Property SettingsRoot As String
    Public Shared ReadOnly Property DatabaseLocation As String
    Public Shared ReadOnly Property LogFileLocation As String
    Public Shared ReadOnly Property ReleaseType As Integer
        Get
            Select Case oReleaseType
                Case ProcessorArchitecture.Amd64
                    Return 64
                Case ProcessorArchitecture.IA64
                    Return 64
                Case ProcessorArchitecture.MSIL
                    Return 32
                Case ProcessorArchitecture.X86
                    Return 32
                Case ProcessorArchitecture.None
                    Return 32
            End Select

            Return 32
        End Get
    End Property
    Public Shared ReadOnly Property Default7zLocation As String
        Get
            If mgrCommon.IsUnix Then
                Return "/usr/bin/7za"
            End If

            Select Case oReleaseType
                Case ProcessorArchitecture.Amd64
                    Return Application.StartupPath & "\Utilities\x64\7za.exe"
                Case ProcessorArchitecture.IA64
                    Return Application.StartupPath & "\Utilities\x64\7za.exe"
                Case ProcessorArchitecture.MSIL
                    Return Application.StartupPath & "\Utilities\x86\7za.exe"
                Case ProcessorArchitecture.X86
                    Return Application.StartupPath & "\Utilities\x86\7za.exe"
                Case ProcessorArchitecture.None
                    Return Application.StartupPath & "\Utilities\x86\7za.exe"
            End Select

            Return Application.StartupPath & "\Utilities\x86\7za.exe"
        End Get
    End Property

    Public Shared Property RemoteDatabaseLocation As String
        Get
            Return sRemoteDatabaseLocation
        End Get
        Set(value As String)
            sRemoteDatabaseLocation = value & Path.DirectorySeparatorChar & "gbm.s3db"
        End Set
    End Property

    Shared Sub New()
        SettingsRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & Path.DirectorySeparatorChar & "gbm"
        DatabaseLocation = SettingsRoot & Path.DirectorySeparatorChar & "gbm.s3db"
        LogFileLocation = SettingsRoot & Path.DirectorySeparatorChar & "gbm_log_" & Date.Now.ToString("dd-MM-yyyy-HH-mm-ss") & ".txt"
        SetEnv()
        LoadCustomVariables()
    End Sub


    Public Shared Function ValidatePath(ByVal sCheckString As String) As String
        Dim cInvalidCharacters As Char() = {Chr(0), Chr(1), Chr(2), Chr(3), Chr(4), Chr(5), Chr(6), Chr(7), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13), Chr(14), Chr(15),
                                           Chr(16), Chr(17), Chr(18), Chr(19), Chr(20), Chr(21), Chr(22), Chr(23), Chr(24), Chr(25), Chr(26), Chr(27), Chr(28), Chr(29), Chr(30),
                                           Chr(31), Chr(34), Chr(60), Chr(62), Chr(124)}

        For Each c As Char In cInvalidCharacters
            sCheckString = sCheckString.Replace(c, "")
        Next

        Return sCheckString.Trim
    End Function

    Public Shared Function ValidateFileName(ByVal sCheckString As String, Optional ByVal iMaxLength As Integer = 254) As String
        Dim cInvalidCharacters As Char() = {Chr(0), Chr(1), Chr(2), Chr(3), Chr(4), Chr(5), Chr(6), Chr(7), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13), Chr(14), Chr(15),
                                           Chr(16), Chr(17), Chr(18), Chr(19), Chr(20), Chr(21), Chr(22), Chr(23), Chr(24), Chr(25), Chr(26), Chr(27), Chr(28), Chr(29), Chr(30),
                                           Chr(31), Chr(34), Chr(42), Chr(47), Chr(58), Chr(60), Chr(62), Chr(63), Chr(92), Chr(124)}

        For Each c As Char In cInvalidCharacters
            sCheckString = sCheckString.Replace(c, "")
        Next

        If sCheckString.Length > iMaxLength Then
            sCheckString = sCheckString.Substring(0, iMaxLength)
        End If

        Return sCheckString.Trim
    End Function

    Public Shared Function DetermineRelativePath(ByVal sProcessPath As String, ByVal sSavePath As String) As String
        Dim sPath1Array As String()
        Dim sPath2Array As String()
        Dim sPath1 As String
        Dim sPath2 As String
        Dim sResult As String = String.Empty
        Dim i As Integer = 0
        Dim iRemove As Integer = 0
        Dim iBackFolders As Integer = 0
        Dim bDeep As Boolean        
        Dim cDS As Char = Path.DirectorySeparatorChar 'Set the directory seperator based on the OS

        If Not mgrCommon.IsUnix Then
            'If we are using Windows, it only makes sense to continue with this function when the process path and save path are on the same drive
            If Not Path.GetPathRoot(sProcessPath) = Path.GetPathRoot(sSavePath) Then
                Return sSavePath
            End If

            'If we are working with a case insenstive file system, use a uniform case to reduce possible issues 
            sProcessPath = sProcessPath.ToLower
            sSavePath = sSavePath.ToLower
        Else
            'If we are on Unix trim the root off
            sProcessPath = sProcessPath.TrimStart(cDS)
            sSavePath = sSavePath.TrimStart(cDS)
        End If

        'We need to ensure we have a single trailing slash on the parameters
        sProcessPath = sProcessPath.TrimEnd(cDS)
        sSavePath = sSavePath.TrimEnd(cDS)
        sProcessPath &= cDS
        sSavePath &= cDS


        'Determines the direction we need to go, we always want to be relative to the process location
        If sSavePath.Split(cDS).Length > sProcessPath.Split(cDS).Length Then
            sPath1 = sProcessPath
            sPath2 = sSavePath
            bDeep = True
        Else
            sPath1 = sSavePath
            sPath2 = sProcessPath
            bDeep = False
        End If

        'Build an array of folders to work with from each path
        sPath1Array = sPath1.Split(cDS)
        sPath2Array = sPath2.Split(cDS)

        'Take the shortest path and remove the common folders from both
        For Each s As String In sPath1Array
            If s = sPath2Array(i) And s <> String.Empty Then
                sPath1 = sPath1.Remove(sPath1.IndexOf(s), s.Length + 1)
                sPath2 = sPath2.Remove(sPath2.IndexOf(s), s.Length + 1)
            End If
            i = i + 1
        Next

        'Remove the trailing slashes 
        sPath1 = sPath1.TrimEnd(cDS)
        sPath2 = sPath2.TrimEnd(cDS)

        'Determine which way we go
        If bDeep Then
            If sPath1.Length > 0 Then
                iBackFolders = sPath1.Split(cDS).Length
            End If
            sResult = sPath2
        Else
            If sPath2.Length > 0 Then
                iBackFolders = sPath2.Split(cDS).Length
            End If
            sResult = sPath1
        End If

        'Insert direction modifiers based on how many folders are left
        For i = 1 To iBackFolders
            sResult = ".." & cDS & sResult
        Next i

        'Done
        Return sResult
    End Function

    Private Shared Function BuildWinePath(ByVal sPath As String, ByVal sWinePrefix As String) As String
        Dim sRealPath As String
        Dim cDriveLetter As Char
        Dim sWineDrive As String

        Try
            'Grab Path
            sRealPath = sPath.Split("=")(1)

            'Remove Quotes
            sRealPath = sRealPath.TrimStart("""")
            sRealPath = sRealPath.TrimEnd("""")

            'Flip Seperators
            sRealPath = sRealPath.Replace("\\", Path.DirectorySeparatorChar)

            'Change Wine Drive
            cDriveLetter = sRealPath.Chars(sRealPath.IndexOf(":") - 1)
            sWineDrive = "drive_" & cDriveLetter
            sRealPath = sRealPath.Replace(cDriveLetter & ":", sWineDrive.ToLower)

            Return sWinePrefix & Path.DirectorySeparatorChar & sRealPath
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrPath_ErrorBuildingWinePath, ex.Message, MsgBoxStyle.Exclamation)
            Return String.Empty
        End Try
    End Function

    Public Shared Function GetWineSavePath(ByVal sPrefix As String, ByVal sPath As String) As String
        Dim sRegistry As String
        Dim sWinePath As String
        Dim sReplace As String
        Dim oParse As Regex
        Dim oMatch As Match

        Try
            If sPath.Contains("%APPDATA%") Then
                sReplace = "%APPDATA%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "user.reg")
                oParse = New Regex("""AppData""="".+?(?=\n)")
            ElseIf sPath.Contains("%LOCALAPPDATA%Low") Then
                sReplace = "%LOCALAPPDATA%Low"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "user.reg")
                oParse = New Regex("""{A520A1A4-1780-4FF6-BD18-167343C5AF16}""="".+?(?=\n)")
            ElseIf sPath.Contains("%LOCALAPPDATA%") Then
                sReplace = "%LOCALAPPDATA%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "user.reg")
                oParse = New Regex("""Local AppData""="".+?(?=\n)")
            ElseIf sPath.Contains("%USERDOCUMENTS%") Then
                sReplace = "%USERDOCUMENTS%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "user.reg")
                oParse = New Regex("""Personal""="".+?(?=\n)")
            ElseIf sPath.Contains("%COMMONDOCUMENTS%") Then
                sReplace = "%COMMONDOCUMENTS%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "system.reg")
                oParse = New Regex("""Common Documents""="".+?(?=\n)")
            ElseIf sPath.Contains("%PROGRAMDATA%") Then
                sReplace = "%PROGRAMDATA%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "system.reg")
                oParse = New Regex("""Common AppData""="".+?(?=\n)")
            ElseIf sPath.Contains("%USERPROFILE%") Then
                sReplace = "%USERPROFILE%"
                sRegistry = File.ReadAllText(sPrefix & Path.DirectorySeparatorChar & "user.reg")
                oParse = New Regex("""Desktop""="".+?(?=\\\\Desktop)")
            Else
                Return sPath
            End If

            If oParse.IsMatch(sRegistry) Then
                oMatch = oParse.Match(sRegistry)
                sWinePath = BuildWinePath(oMatch.Value, sPrefix)
                sPath = sPath.Replace("\", Path.DirectorySeparatorChar)
                Return sPath.Replace(sReplace, sWinePath)
            End If

            Return sPath
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrPath_ErrorConvertWineSavePath, ex.Message, MsgBoxStyle.Exclamation)
            Return sPath
        End Try
    End Function

    Public Shared Function GetWinePrefix(ByVal prs As Process) As String
        Dim sEnv As String
        Dim oParse As New Regex("WINEPREFIX=.+?(?=\x00)")
        Dim oMatch As Match

        Try
            sEnv = File.ReadAllText("/proc/" & prs.Id.ToString() & "/environ")
            If oParse.IsMatch(sEnv) Then
                oMatch = oParse.Match(sEnv)
                Return oMatch.Value.Trim("/").Split("=")(1)
            Else
                'When WINEPREFIX is not part of the command,  we will assume the default prefix.
                Return Environment.GetFolderPath(Environment.SpecialFolder.Personal) & "/.wine"
            End If
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrPath_ErrorWinePrefix, ex.Message, MsgBoxStyle.Exclamation)
            Return String.Empty
        End Try
    End Function

    Public Shared Function GetSpecialPaths() As Hashtable
        Dim hshEnvs As New Hashtable
        Dim bNoError As Boolean = True

        hshEnvs.Add("%USERDOCUMENTS%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        hshEnvs.Add("%APPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        hshEnvs.Add("%LOCALAPPDATA%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
        hshEnvs.Add("%PROGRAMDATA%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))

        If Not mgrCommon.IsUnix Then
            hshEnvs.Add("%USERPROFILE%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            hshEnvs.Add("%COMMONDOCUMENTS%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
        End If

        Return hshEnvs
    End Function

    Public Shared Function CheckForEmptySpecialPaths() As Boolean
        Dim hshEnvs As Hashtable = GetSpecialPaths()
        Dim bNoError As Boolean = True

        For Each de As DictionaryEntry In hshEnvs
            If de.Value = String.Empty Then
                mgrCommon.ShowMessage(mgrPath_SpecialPathError, de.Key, MsgBoxStyle.Critical)
                bNoError = False
            End If
        Next

        Return bNoError
    End Function

    Private Shared Sub SetEnv()
        If Not mgrCommon.IsUnix Then
            Environment.SetEnvironmentVariable("USERDOCUMENTS", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            Environment.SetEnvironmentVariable("COMMONDOCUMENTS", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
        End If
    End Sub

    Public Shared Function ReplaceSpecialPaths(ByVal sValue As String) As String
        Dim sXdgData As String = "${XDG_DATA_HOME:-~/.local/share}"
        Dim sEnvAppDataLocal As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim sXdgConfig As String = "${XDG_CONFIG_HOME:-~/.config}"
        Dim sEnvAppDataRoaming As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Dim sHomeDir As String = "${HOME}"
        Dim sEnvCurrentUser As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim oCustomVariable As clsPathVariable

        For Each oCustomVariable In CustomVariables.Values
            If sValue.Contains(oCustomVariable.FormattedName) Then
                sValue = sValue.Replace(oCustomVariable.FormattedName, oCustomVariable.Path)
            End If
        Next

        If mgrCommon.IsUnix Then
            '$VAR_iable
            Dim oParse As New Regex("\$([a-zA-Z0-9_]+)")
            '${VAR_iable} but not advanced syntax like ${VAR:-iable}
            Dim oParseBracketed As New Regex("\$\{([a-zA-Z0-9_]+?)\}")
            '~ not inside ${...}
            Dim oParseTilde As New Regex("~(?![^\$\{]*\})")
            If sEnvCurrentUser = String.Empty Then
                'Fall back
                sEnvCurrentUser = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            End If
            If sEnvCurrentUser = String.Empty Then
                'Fall back
                sEnvCurrentUser = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            End If

            '$HOME to ${HOME}
            sValue = oParse.Replace(sValue, "${$1}")
            'Special notations for home directory
            sValue = oParseTilde.Replace(sValue, "${HOME}")
            'XDG Base Directory Specification has default values
            sValue = sValue.Replace("${XDG_DATA_HOME}", sXdgData)
            sValue = sValue.Replace("${XDG_CONFIG_HOME}", sXdgConfig)

            'Replace with paths
            sValue = sValue.Replace(sXdgData, sEnvAppDataLocal)
            sValue = sValue.Replace(sXdgConfig, sEnvAppDataRoaming)
            sValue = sValue.Replace(sHomeDir, sEnvCurrentUser)

            'Escape real Windows variables
            sValue = sValue.Replace("%", "\%")
            'Transform Linux variables to Windows variables
            sValue = oParseBracketed.Replace(sValue, "%$1%")
        End If

        'On Linux real Linux environmental variables are used
        sValue = Environment.ExpandEnvironmentVariables(sValue)

        If mgrCommon.IsUnix Then
            'Transform missing variables back
            Dim oParse As New Regex("%([a-zA-Z0-9_]+?)%")
            sValue = oParse.Replace(sValue, "${$1}")
            'Unscape real Windows variables
            sValue = sValue.Replace("\%", "%")
        End If

        Return sValue
    End Function

    Public Shared Function ReverseSpecialPaths(sValue As String) As String
        Dim sMyDocs As String = "%USERDOCUMENTS%"
        Dim sEnvMyDocs As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sPublicDocs As String = "%COMMONDOCUMENTS%"
        Dim sEnvPublicDocs As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)
        Dim sAppDataLocal As String = "%LOCALAPPDATA%"
        Dim sXdgData As String = "${XDG_DATA_HOME:-~/.local/share}"
        Dim sEnvAppDataLocal As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim sAppDataRoaming As String = "%APPDATA%"
        Dim sXdgConfig As String = "${XDG_CONFIG_HOME:-~/.config}"
        Dim sEnvAppDataRoaming As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Dim sCurrentUser As String = "%USERPROFILE%"
        Dim sHomeDir As String = "~"
        Dim sEnvCurrentUser As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim sProgramData As String = "%PROGRAMDATA%"
        Dim sEnvProgramData As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        Dim oCustomVariable As clsPathVariable

        For Each oCustomVariable In CustomVariables.Values
            If sValue.Contains(oCustomVariable.Path) Then
                sValue = sValue.Replace(oCustomVariable.Path, oCustomVariable.FormattedName)
            End If
        Next

        If Not mgrCommon.IsUnix Then
            If sValue.Contains(sEnvAppDataRoaming) Then
                Return sValue.Replace(sEnvAppDataRoaming, sAppDataRoaming)
            End If

            If sValue.Contains(sEnvAppDataLocal) Then
                Return sValue.Replace(sEnvAppDataLocal, sAppDataLocal)
            End If

            If sValue.Contains(sEnvProgramData) Then
                Return sValue.Replace(sEnvProgramData, sProgramData)
            End If

            'This needs to be tested last for Unix compatability
            If sValue.Contains(sEnvMyDocs) Then
                Return sValue.Replace(sEnvMyDocs, sMyDocs)
            End If

            'Mono doesn't set a path for these folders
            If sValue.Contains(sEnvPublicDocs) Then
                Return sValue.Replace(sEnvPublicDocs, sPublicDocs)
            End If

            If sValue.Contains(sEnvCurrentUser) Then
                Return sValue.Replace(sEnvCurrentUser, sCurrentUser)
            End If
        Else
            'Use different paths on Linux
            If sValue.Contains(sEnvAppDataRoaming) Then
                Return sValue.Replace(sEnvAppDataRoaming, sXdgConfig)
            End If

            If sValue.Contains(sEnvAppDataLocal) Then
                Return sValue.Replace(sEnvAppDataLocal, sXdgData)
            End If

            'Must be last
            If sValue.Contains(sEnvCurrentUser) Then
                If sEnvCurrentUser = String.Empty Then
                    'Fall back
                    sEnvCurrentUser = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                End If
                If sEnvCurrentUser = String.Empty Then
                    'Fall back
                    sEnvCurrentUser = sMyDocs
                End If
                Return sValue.Replace(sEnvCurrentUser, sHomeDir)
            End If
        End If

        Return sValue
    End Function

    Public Shared Sub FormatRegistryPath(ByRef sPath As String, Optional ByRef bHKCU As Boolean = False)
        If sPath.ToUpper.StartsWith("HKEY_CURRENT_USER\") Or sPath.ToUpper.StartsWith("HKCU\") Then
            bHKCU = True
            sPath = sPath.Replace("HKCU\", String.Empty)
            sPath = sPath.Replace("HKEY_CURRENT_USER\", String.Empty)
        ElseIf sPath.ToUpper.StartsWith("HKEY_LOCAL_MACHINE\") Or sPath.ToUpper.StartsWith("HKLM\") Then
            bHKCU = False
            sPath = sPath.Replace("HKLM\", String.Empty)
            sPath = sPath.Replace("HKEY_LOCAL_MACHINE\", String.Empty)
        End If
    End Sub

    Public Shared Function IsPopulatedRegistryKey(ByVal sPath As String) As Boolean
        Dim oKey As Microsoft.Win32.RegistryKey
        Dim bHKCU As Boolean

        If IsSupportedRegistryPath(sPath) Then
            FormatRegistryPath(sPath, bHKCU)
            If bHKCU Then
                oKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(sPath)
            Else
                oKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sPath)
            End If
            If Not oKey Is Nothing Then
                If oKey.SubKeyCount > 0 Or oKey.ValueCount > 0 Then
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    Public Shared Function IsSupportedRegistryPath(ByVal sPath As String) As Boolean
        If sPath.ToUpper.StartsWith("HKEY_CURRENT_USER\") Or sPath.ToUpper.StartsWith("HKCU\") Then
            Return True
        ElseIf sPath.ToUpper.StartsWith("HKEY_LOCAL_MACHINE\") Or sPath.ToUpper.StartsWith("HKLM\") Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function IsPathUNC(sPath As String) As Boolean
        Dim sPrefix As String = Path.DirectorySeparatorChar & Path.DirectorySeparatorChar
        If sPath.StartsWith(sPrefix) Then Return True
        Return False
    End Function

    'This should only be used in situations where the function DetermineRelativePath is required.
    Public Shared Function IsAbsolute(sValue As String) As Boolean
        Dim hshFolders As New Hashtable
        Dim CustomVariables As Hashtable = mgrVariables.ReadVariables
        Dim oCustomVariable As clsPathVariable


        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

        'Don't use these in Unix
        If Not mgrCommon.IsUnix Then
            hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
            hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
            hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        End If

        'Load Custom Variables
        For Each oCustomVariable In CustomVariables.Values
            hshFolders.Add(Guid.NewGuid.ToString, oCustomVariable.Path)
        Next

        For Each de As DictionaryEntry In hshFolders
            If sValue.Contains(de.Value) Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Shared Function VerifyCustomVariables(ByVal hshScanlist As Hashtable, ByRef sGames As String) As Boolean
        Dim CustomVariables As Hashtable = mgrVariables.ReadVariables
        'Reserved variables will be resolved on Windows, but not on a Linux.  Therefore we an ignore list here, otherwise GBM will bitch about them when using Windows configurations for Wine.
        Dim oReservedVariables As List(Of String) = mgrVariables.GetReservedVariables
        Dim sVariableCheck As String
        Dim sPattern As String = "\%[^\%]*\%"
        Dim oGame As clsGame
        Dim oMatch As Match
        Dim bClean As Boolean = True

        For Each oGame In hshScanlist.Values
            oMatch = Regex.Match(oGame.Path, sPattern)
            If oMatch.Success Then
                sVariableCheck = oMatch.Value.Replace("%", String.Empty)
                If Not CustomVariables.ContainsKey(sVariableCheck) And Not oReservedVariables.Contains(sVariableCheck) Then
                    sGames &= vbCrLf & oGame.Name & " (" & sVariableCheck & ")"
                    bClean = False
                End If
            End If
            oMatch = Regex.Match(oGame.ProcessPath, sPattern)
            If oMatch.Success Then
                sVariableCheck = oMatch.Value.Replace("%", String.Empty)
                If Not CustomVariables.ContainsKey(sVariableCheck) And Not oReservedVariables.Contains(sVariableCheck) Then
                    sGames &= vbCrLf & oGame.Name & " (" & sVariableCheck & ")"
                    bClean = False
                End If
            End If
        Next

        Return bClean
    End Function

    Public Shared Sub LoadCustomVariables()
        CustomVariables = mgrVariables.ReadVariables

        For Each oVariable As clsPathVariable In CustomVariables.Values
            Environment.SetEnvironmentVariable(oVariable.Name, oVariable.Path)
        Next
    End Sub

    Public Shared Function SetManualGamePath() As String
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sNewPath As String

        sNewPath = mgrCommon.OpenFolderBrowser("Manual_Game_Location", mgrPath_ChoosePath, sDefaultFolder, False)

        Return sNewPath
    End Function

    Public Shared Function ProcessPathSearch(ByVal sGameName As String, ByVal sProcess As String, ByVal sSearchReason As String, Optional ByVal bNoAuto As Boolean = False) As String
        Dim frmFind As New frmFileFolderSearch
        Dim sMessage As String
        Dim sFolder As String = String.Empty
        Dim bSearchFailed As Boolean = False

        frmFind.GameName = sGameName
        frmFind.SearchItem = sProcess & ".*"
        frmFind.FolderSearch = False

        'We can't automatically search for certain game types
        If bNoAuto Then
            sMessage = mgrCommon.FormatString(mgrPath_ConfirmManualPath, sSearchReason)

            If mgrCommon.ShowPriorityMessage(sMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                sFolder = SetManualGamePath()
            End If

            Return sFolder
        End If

        sMessage = mgrCommon.FormatString(mgrPath_ConfirmAutoPath, sSearchReason)

        If mgrCommon.ShowPriorityMessage(sMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            frmFind.ShowDialog()
            frmFind.BringToFront()

            If frmFind.FoundItem <> String.Empty Then
                Return frmFind.FoundItem
            Else
                bSearchFailed = True
            End If

            If bSearchFailed Then
                sMessage = mgrCommon.FormatString(mgrPath_ConfirmAutoFailure, sGameName)
            Else
                sMessage = mgPath_ConfirmManualPathNoParam
            End If

            If mgrCommon.ShowMessage(sMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                sFolder = SetManualGamePath()
            End If

            frmFind.Dispose()
        End If

        Return sFolder
    End Function

    Public Shared Function VerifyLinuxDesktopFileLocation(Optional ByRef sFoundPath As String = "") As Boolean
        Dim sLocations As String() = New String() {"/usr/share/applications/gbm.desktop", "/usr/local/share/applications/gbm.desktop"}

        For Each s As String In sLocations
            If File.Exists(s) Then
                sFoundPath = s
                Return True
            End If
        Next

        Return False
    End Function
End Class
