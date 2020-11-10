﻿Imports GBM.My.Resources
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
                    mgrCommon.ShowMessage(mgrLaunchGame_ErrorNoExe, sLaunchPath, MsgBoxStyle.Exclamation)
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

    Public Shared Function LaunchGame(ByVal oGame As clsGame, ByVal eLaunchType As eLaunchType, ByRef sMessage As String) As Boolean
        Dim oLaunchData As clsLaunchData = mgrLaunchData.DoLaunchDataGetbyID(oGame.ID)
        Dim oLauncher As clsLauncher
        Dim sLaunchCommand As String
        Dim sLaunchPath As String
        Dim sLaunchArgs As String

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
                'Handle Launch Path
                sLaunchPath = oGame.ProcessPath.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar & oGame.ProcessName
                If Not mgrCommon.IsUnix Then sLaunchPath &= ".exe"
                'Handle Launch Arguments
                If oGame.Parameter <> String.Empty Then
                    sLaunchArgs = oGame.Parameter
                Else
                    sLaunchArgs = oLaunchData.Args
                End If
                If RunGameExecutable(sLaunchPath, sLaunchArgs) Then
                    sMessage = mgrCommon.FormatString(frmMain_LaunchGame, New String() {oGame.Name, sLaunchPath})
                    Return True
                End If
        End Select

        Return False
    End Function

End Class
