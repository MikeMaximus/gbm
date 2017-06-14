Imports GBM.My.Resources
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class mgrPath
    'Important Note: Any changes to sSettingsRoot & sDBLocation need to be mirrored in frmMain.vb -> VerifyGameDataPath
    Private Shared sSettingsRoot As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & Path.DirectorySeparatorChar & "gbm"
    Private Shared sDBLocation As String = sSettingsRoot & Path.DirectorySeparatorChar & "gbm.s3db"
    Private Shared sIncludeFile As String = sSettingsRoot & Path.DirectorySeparatorChar & "gbm_include.txt"
    Private Shared sExcludeFile As String = sSettingsRoot & Path.DirectorySeparatorChar & "gbm_exclude.txt"
    Private Shared sLogFile As String = sSettingsRoot & Path.DirectorySeparatorChar & "gbm_log_" & Date.Now.ToString("dd-MM-yyyy-HH-mm-ss") & ".txt"
    Private Shared sRemoteDatabaseLocation As String
    Private Shared hshCustomVariables As Hashtable
    Private Shared oReleaseType As ProcessorArchitecture = AssemblyName.GetAssemblyName(Application.ExecutablePath()).ProcessorArchitecture

    Shared Sub New()
        hshCustomVariables = mgrVariables.ReadVariables
    End Sub

    Shared ReadOnly Property ReleaseType As Integer
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

    Shared ReadOnly Property Default7zLocation As String
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

    Shared ReadOnly Property DatabaseLocation As String
        Get
            Return sDBLocation
        End Get
    End Property

    Shared ReadOnly Property IncludeFileLocation As String
        Get
            Return sIncludeFile
        End Get
    End Property

    Shared ReadOnly Property ExcludeFileLocation As String
        Get
            Return sExcludeFile
        End Get
    End Property

    Shared ReadOnly Property LogFileLocation As String
        Get
            Return sLogFile
        End Get
    End Property

    Shared ReadOnly Property SettingsRoot As String
        Get
            Return sSettingsRoot
        End Get
    End Property

    Shared Property RemoteDatabaseLocation As String
        Get
            Return sRemoteDatabaseLocation
        End Get
        Set(value As String)
            sRemoteDatabaseLocation = value & Path.DirectorySeparatorChar & "gbm.s3db"
        End Set
    End Property

    Public Shared Function ValidateForFileSystem(ByVal sCheckString As String) As String
        Dim cInvalidCharacters As Char() = {"\", "/", ":", "*", "?", """", "<", ">", "|", "."}

        For Each c As Char In cInvalidCharacters
            sCheckString = sCheckString.Replace(c, "")
        Next

        If sCheckString.Length > 257 Then
            sCheckString = sCheckString.Substring(0, 257)
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

    Public Shared Function ReplaceSpecialPaths(sValue As String) As String
        Dim sMyDocs As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sPublicDocs As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)
        Dim sAppDataRoaming As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Dim sAppDataLocal As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim sCurrentUser As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim oCustomVariable As clsPathVariable


        For Each oCustomVariable In hshCustomVariables.Values
            If sValue.Contains(oCustomVariable.FormattedName) Then
                Return sValue.Replace(oCustomVariable.FormattedName, oCustomVariable.Path)
            End If
        Next

        If sValue.Contains("*appdatalocal*") Then
            Return sValue.Replace("*appdatalocal*", sAppDataLocal)
        End If

        If sValue.Contains("*appdataroaming*") Then
            Return sValue.Replace("*appdataroaming*", sAppDataRoaming)
        End If

        'This needs to be tested last for Unix compatability
        If sValue.Contains("*mydocs*") Then
            Return sValue.Replace("*mydocs*", sMyDocs)
        End If

        'Don't use these in Unix
        If Not mgrCommon.IsUnix Then
            If sValue.Contains("*publicdocs*") Then
                Return sValue.Replace("*publicdocs*", sPublicDocs)
            End If

            If sValue.Contains("*currentuser*") Then
                Return sValue.Replace("*currentuser*", sCurrentUser)
            End If
        End If

        Return sValue
    End Function

    Public Shared Function ReverseSpecialPaths(sValue As String) As String
        Dim sMyDocs As String = "*mydocs*"
        Dim sPublicDocs As String = "*publicdocs*"
        Dim sAppDataRoaming As String = "*appdatalocal*"
        Dim sAppDataLocal As String = "*appdataroaming*"
        Dim sCurrentUser As String = "*currentuser*"
        Dim oCustomVariable As clsPathVariable

        For Each oCustomVariable In hshCustomVariables.Values
            If sValue.Contains(oCustomVariable.Path) Then
                Return sValue.Replace(oCustomVariable.Path, oCustomVariable.FormattedName)
            End If
        Next

        If sValue.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) Then
            Return sValue.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), sAppDataLocal)
        End If

        If sValue.Contains(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) Then
            Return sValue.Replace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), sAppDataRoaming)
        End If

        'This needs to be tested last for Unix compatability
        If sValue.Contains(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) Then
            Return sValue.Replace(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), sMyDocs)
        End If

        'Don't use these in Unix
        If Not mgrCommon.IsUnix Then
            If sValue.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)) Then
                Return sValue.Replace(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), sPublicDocs)
            End If

            If sValue.Contains(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) Then
                Return sValue.Replace(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), sCurrentUser)
            End If
        End If

        Return sValue
    End Function

    Public Shared Function IsAbsolute(sValue As String) As Boolean
        Dim hshFolders As New Hashtable
        Dim hshCustomVariables As Hashtable = mgrVariables.ReadVariables
        Dim oCustomVariable As clsPathVariable


        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
        hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

        'Don't use these in Unix
        If Not mgrCommon.IsUnix Then
            hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
            hshFolders.Add(Guid.NewGuid.ToString, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        End If

        'Load Custom Variables
        For Each oCustomVariable In hshCustomVariables.Values
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
        Dim hshCustomVariables As Hashtable = mgrVariables.ReadVariables
        Dim sVariableCheck As String
        Dim sPattern As String = "\*(.*)\*"
        Dim oGame As clsGame
        Dim oMatch As Match
        Dim bClean As Boolean = True

        For Each oGame In hshScanlist.Values
            oMatch = Regex.Match(oGame.Path, sPattern)
            If oMatch.Success Then
                sVariableCheck = oMatch.Value.Replace("*", String.Empty)
                If Not hshCustomVariables.ContainsKey(sVariableCheck) Then
                    sGames &= vbCrLf & oGame.Name & " (" & sVariableCheck & ")"
                    bClean = False
                End If
            End If
        Next

        Return bClean
    End Function

    Public Shared Sub CustomVariablesReload()
        hshCustomVariables = mgrVariables.ReadVariables
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

            If mgrCommon.ShowMessage(sMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                sFolder = SetManualgamePath()
            End If

            Return sFolder
        End If

        sMessage = mgrCommon.FormatString(mgrPath_ConfirmAutoPath, sSearchReason)

        If mgrCommon.ShowMessage(sMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            frmFind.ShowDialog()

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
                sFolder = SetManualgamePath()
            End If

            frmFind.Dispose()
        End If

        Return sFolder
    End Function

    Public Shared Function VerifyBackupPath(ByRef sBackupPath As String) As Boolean
        Dim dBrowser As FolderBrowserDialog

        If Not Directory.Exists(sBackupPath) Then
            If mgrCommon.ShowMessage(mgrPath_ConfirmBackupLocation, sBackupPath, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                dBrowser = New FolderBrowserDialog
                dBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                If dBrowser.ShowDialog = DialogResult.OK Then
                    sBackupPath = dBrowser.SelectedPath
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If

        Return True
    End Function
End Class
