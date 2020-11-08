Imports GBM.My.Resources
Imports System.IO

Public Class mgrLaunchGame

    Public Enum eLaunchType
        ExternalLauncher = 1
        AlternateExe = 2
        UseGameConfig = 3
    End Enum

    Public Shared Function CanLaunchGame(ByVal oGame As clsGame, ByRef eLaunchType As eLaunchType) As Boolean
        Dim oLaunchData As clsLaunchData = mgrLaunchData.DoLaunchDataGetbyID(oGame.ID)
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
                If oGame.ProcessName = String.Empty Then
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorNoProcess, oGame.Name, MsgBoxStyle.Exclamation)
                ElseIf oGame.ProcessPath = String.Empty Then
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorNoProcessPath, oGame.Name, MsgBoxStyle.Exclamation)
                ElseIf oGame.IsRegEx Then
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorIsRegex, oGame.Name, MsgBoxStyle.Exclamation)
                Else
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorIsBlacklisted, oGame.Name, MsgBoxStyle.Exclamation)
                End If
                Return False
            Else
                sLaunchPath = oGame.ProcessPath.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar & oGame.ProcessName
                If Not mgrCommon.IsUnix Then sLaunchPath &= ".exe"
                If File.Exists(sLaunchPath) Then
                    eLaunchType = eLaunchType.UseGameConfig
                    Return True
                Else
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorNoExe, oGame.Name, MsgBoxStyle.Exclamation)
                    Return False
                End If
            End If
        End If
    End Function

    Private Shared Function RunGameExecutable(ByVal sFullPath As String, ByVal sArgs As String) As Boolean
        Dim prsGame As New Process

        Try
            prsGame = New Process
            prsGame.StartInfo.Arguments = sArgs
            prsGame.StartInfo.FileName = sFullPath
            prsGame.StartInfo.WorkingDirectory = Path.GetDirectoryName(sFullPath)
            prsGame.StartInfo.UseShellExecute = False
            prsGame.StartInfo.CreateNoWindow = True
            prsGame.Start()
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrLaunchGame_ErrorException, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    Public Shared Function LaunchGame(ByVal oGame As clsGame, ByVal eLaunchType As eLaunchType, ByRef sMessage As String) As Boolean
        Dim oLaunchData As clsLaunchData = mgrLaunchData.DoLaunchDataGetbyID(oGame.ID)
        Dim oLauncher As clsLauncher
        Dim sLaunchCommand As String
        Dim sLaunchPath As String

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
                If RunGameExecutable(oLaunchData.Path, oLaunchData.Args) Then
                    sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, oLaunchData.Path})
                    Return True
                End If
            Case eLaunchType.UseGameConfig
                sLaunchPath = oGame.ProcessPath.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar & oGame.ProcessName
                If Not mgrCommon.IsUnix Then sLaunchPath &= ".exe"
                If RunGameExecutable(sLaunchPath, oLaunchData.Args) Then
                    sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, sLaunchPath})
                    Return True
                End If
        End Select

        Return False
    End Function

End Class
