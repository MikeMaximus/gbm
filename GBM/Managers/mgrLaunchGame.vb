Imports GBM.My.Resources
Imports System.IO

Public Class mgrLaunchGame

    Public Enum eLaunchType
        ExternalLauncher = 1
        AlternateExe = 2
        UseGameConfig = 3
    End Enum

    Public Shared Function CanLaunchGame(ByVal oGame As clsGame, ByVal oLaunchData As clsLaunchData, ByRef eLaunchType As eLaunchType, ByRef sErrorMessage As String) As Boolean
        Dim sLaunchPath As String

        If oLaunchData.LauncherID <> String.Empty Then
            'We use the store launcher first if it's set
            eLaunchType = eLaunchType.ExternalLauncher
            Return True
        ElseIf oLaunchData.Path <> String.Empty Then
            'We use the alternative exe next if it's set
            eLaunchType = eLaunchType.AlternateExe
            Return True
        Else
            'And finally we attempt to use the process name and detected process path if no specific launcher settings exist
            If mgrCommon.IsProcessNotLaunchable(oGame) Then
                'Give the user a specific error message
                If mgrCommon.IsUnix And oGame.OS = clsGame.eOS.Windows Then
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorNoAutoWineSupport, oGame.CroppedName)
                ElseIf oGame.ProcessName = String.Empty Then
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorNoProcess, oGame.CroppedName)
                ElseIf oGame.ProcessPath = String.Empty Then
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorNoProcessPath, oGame.CroppedName)
                ElseIf oGame.IsRegEx Then
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorIsRegex, oGame.CroppedName)
                Else
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorIsBlacklisted, oGame.CroppedName)
                End If
                Return False
            Else
                sLaunchPath = oGame.ProcessPath.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar & oGame.ProcessName
                If Not mgrCommon.IsUnix Then sLaunchPath &= ".exe"
                If File.Exists(sLaunchPath) Then
                    eLaunchType = eLaunchType.UseGameConfig
                    Return True
                Else
                    sErrorMessage = mgrCommon.FormatString(mgrLaunchGame_ErrorNoExe, sLaunchPath)
                    Return False
                End If
            End If
        End If
    End Function

    Private Shared Function RunGameExecutable(ByVal sFullPath As String, ByVal sArgs As String, Optional ByVal bAdmin As Boolean = False) As Boolean
        Dim prsGame As New Process

        'Give a warning when elevated
        If mgrCommon.IsElevated Then
            If mgrCommon.ShowPriorityMessage(mgrLaunchGame_WarningLaunchElevation, sFullPath, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            End If
        End If

        Try
            prsGame = New Process
            prsGame.StartInfo.Arguments = sArgs
            prsGame.StartInfo.FileName = sFullPath
            prsGame.StartInfo.WorkingDirectory = Path.GetDirectoryName(sFullPath)
            prsGame.StartInfo.UseShellExecute = True
            prsGame.StartInfo.CreateNoWindow = True
            If bAdmin Then prsGame.StartInfo.Verb = "runas"
            prsGame.Start()
            Return True
        Catch exWin32 As System.ComponentModel.Win32Exception
            'If the launch fails due to required elevation, try it again and request elevation.
            If exWin32.ErrorCode = 740 Then
                RunGameExecutable(sFullPath, sArgs, True)
            Else
                mgrCommon.ShowMessage(mgrLaunchGame_ErrorException, exWin32.Message, MsgBoxStyle.Exclamation)
            End If
            Return False
        Catch exAll As Exception
            mgrCommon.ShowMessage(mgrLaunchGame_ErrorException, exAll.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    Private Shared Function GetLaunchParameters(ByVal oGame As clsGame, ByVal oLaunchData As clsLaunchData) As String
        Dim sLaunchArgs As String

        If oLaunchData.NoArgs Then Return String.Empty

        If oLaunchData.Args <> String.Empty Then
            sLaunchArgs = oLaunchData.Args
        ElseIf oGame.Parameter <> String.Empty Then
            sLaunchArgs = oGame.Parameter
        Else
            sLaunchArgs = String.Empty
        End If

        Return sLaunchArgs
    End Function

    Private Shared Function BuildLaunchPathFromGame(ByVal oGame As clsGame) As String
        Dim sLaunchPath As String

        sLaunchPath = oGame.ProcessPath.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar & oGame.ProcessName
        If Not mgrCommon.IsUnix Then sLaunchPath &= ".exe"

        Return sLaunchPath
    End Function

    Public Shared Function LaunchGame(ByVal oGame As clsGame, ByVal oLaunchData As clsLaunchData, ByVal eLaunchType As eLaunchType, ByRef sMessage As String) As Boolean
        Dim oLauncher As clsLauncher
        Dim sLaunchCommand As String
        Dim sLaunchPath As String
        Dim sLaunchArgs As String = GetLaunchParameters(oGame, oLaunchData)

        Select Case eLaunchType
            Case eLaunchType.ExternalLauncher
                oLauncher = mgrLaunchers.DoLauncherGetbyID(oLaunchData.LauncherID)
                'Replace the ID variable if it exists, if not append it to the command.
                If oLauncher.LaunchString.Contains("%ID%") Then
                    sLaunchCommand = oLauncher.LaunchString.Replace("%ID%", oLaunchData.LauncherGameID)
                Else
                    sLaunchCommand = oLauncher.LaunchString & oLaunchData.LauncherGameID
                End If
                mgrCommon.OpenInOS(sLaunchCommand,, True)
                sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, oLauncher.Name})
                Return True
            Case eLaunchType.AlternateExe
                If RunGameExecutable(oLaunchData.Path, sLaunchArgs) Then
                    sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, oLaunchData.Path})
                    Return True
                End If
            Case eLaunchType.UseGameConfig
                sLaunchPath = BuildLaunchPathFromGame(oGame)
                If RunGameExecutable(sLaunchPath, sLaunchArgs) Then
                    sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, sLaunchPath})
                    Return True
                End If
        End Select

        Return False
    End Function

    Public Shared Function GetLaunchCommand(ByVal oGame As clsGame, ByVal oLaunchData As clsLaunchData, ByVal eLaunchType As eLaunchType) As String
        Dim oLauncher As clsLauncher
        Dim sLaunchCommand As String
        Dim sLaunchArgs As String = GetLaunchParameters(oGame, oLaunchData)

        Select Case eLaunchType
            Case eLaunchType.ExternalLauncher
                oLauncher = mgrLaunchers.DoLauncherGetbyID(oLaunchData.LauncherID)
                'Replace the ID variable if it exists, if not append it to the command.
                If oLauncher.LaunchString.Contains("%ID%") Then
                    sLaunchCommand = oLauncher.LaunchString.Replace("%ID%", oLaunchData.LauncherGameID)
                Else
                    sLaunchCommand = oLauncher.LaunchString & oLaunchData.LauncherGameID
                End If
                Return sLaunchCommand
            Case eLaunchType.AlternateExe
                Return oLaunchData.Path & " " & sLaunchArgs
            Case eLaunchType.UseGameConfig
                Return BuildLaunchPathFromGame(oGame) & " " & sLaunchArgs
        End Select

        Return String.Empty
    End Function
End Class
