Imports System.IO
Imports System.Management
Imports System.Text.RegularExpressions

Public Class mgrProcessDetection

    Private prsFoundProcess As Process
    Private sProcessPath As String
    Private dStartTime As DateTime = Now, dEndTime As DateTime = Now
    Private lTimeSpent As Long = 0
    Private oGame As clsGame
    Private oDuplicateGames As New ArrayList
    Private bDuplicates As Boolean
    Private bVerified As Boolean = False

    Property FoundProcess As Process
        Get
            Return prsFoundProcess
        End Get
        Set(value As Process)
            prsFoundProcess = value
        End Set
    End Property

    Property ProcessPath As String
        Get
            Return sProcessPath
        End Get
        Set(value As String)
            sProcessPath = value
        End Set
    End Property

    Property StartTime As DateTime
        Get
            Return dStartTime
        End Get
        Set(value As DateTime)
            dStartTime = value
        End Set
    End Property

    Property EndTime As DateTime
        Get
            Return dEndTime
        End Get
        Set(value As DateTime)
            dEndTime = value
        End Set
    End Property

    ReadOnly Property TimeSpent As TimeSpan
        Get
            Return dEndTime.Subtract(dStartTime)
        End Get
    End Property

    Property GameInfo As clsGame
        Get
            Return oGame
        End Get
        Set(value As clsGame)
            oGame = value
        End Set
    End Property

    Property Duplicate As Boolean
        Get
            Return bDuplicates
        End Get
        Set(value As Boolean)
            bDuplicates = value
        End Set
    End Property

    Property DuplicateList As ArrayList
        Get
            Return oDuplicateGames
        End Get
        Set(value As ArrayList)
            oDuplicateGames = value
        End Set
    End Property

    'This function will only work correctly on Windows
    Private Function GetWindowsCommand(ByVal prs As Process) As String
        Dim sFullCommand As String = String.Empty
        Try
            Using searcher As New ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + prs.Id.ToString)
                For Each o As ManagementObject In searcher.Get()
                    sFullCommand &= o("CommandLine") & " "
                Next
            End Using
        Catch
            'Do Nothing
        End Try
        Return sFullCommand
    End Function

    'This function will only work correctly on Unix
    Private Function GetUnixCommand(ByVal prs As Process) As String
        Dim sFullCommand As String = String.Empty
        Try
            sFullCommand = File.ReadAllText("/proc/" & prs.Id.ToString() & "/cmdline").Replace(vbNullChar, " ")
        Catch
            'Do Nothing
        End Try

        Return sFullCommand
    End Function

    'This function will only work correctly on Unix
    Private Function GetUnixProcessArguments(ByVal prs As Process) As String()
        Dim sArguments As String
        Try
            sArguments = File.ReadAllText("/proc/" & prs.Id.ToString() & "/cmdline")
            Return sArguments.Split(vbNullChar)
        Catch ex As Exception
            Return New String() {String.Empty}
        End Try
    End Function

    'This function will only work correctly on Unix
    Private Function GetUnixSymLinkDirectory(ByVal prs As Process) As String
        Dim prsls As Process
        Dim slsinfo As String

        Try
            prsls = New Process
            prsls.StartInfo.FileName = "/bin/readlink"
            prsls.StartInfo.Arguments = "-f /proc/" & prs.Id.ToString & "/cwd"
            prsls.StartInfo.UseShellExecute = False
            prsls.StartInfo.RedirectStandardOutput = True
            prsls.StartInfo.CreateNoWindow = True
            prsls.Start()
            slsinfo = prsls.StandardOutput.ReadToEnd()
            Return slsinfo.Trim()
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function IsMatch(ByRef oGame As clsGame, ByRef sProcessCheck As String) As Boolean
        If oGame.IsRegEx Then
            Try
                If Regex.IsMatch(sProcessCheck, oGame.ProcessName) Then
                    Return True
                End If
            Catch
                'Ignore malformed regular expressions that may have passed validation
            End Try
        Else
            If oGame.ProcessName = sProcessCheck Then
                Return True
            End If
        End If

        Return False
    End Function

    Private Function GetProcessPath(ByVal bWineProcess As Boolean) As String
        Try
            If Not bWineProcess Then
                Return Path.GetDirectoryName(FoundProcess.MainModule.FileName)
            Else
                Return GetUnixSymLinkDirectory(FoundProcess)
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Private Sub FilterDetected(ByVal oDetectedGames As ArrayList, ByVal bWineProcess As Boolean)
        Dim bMatch As Boolean = False
        Dim sFullCommand As String
        Dim oNotDetectedWithParameters As New ArrayList
        Dim oDetectedWithParameters As New ArrayList
        Dim oNotDetectedWithProcessPath As New ArrayList
        Dim oDetectedWithProcessPath As New ArrayList

        'Get parameters of the found process
        If mgrCommon.IsUnix Then
            sFullCommand = GetUnixCommand(FoundProcess)
        Else
            sFullCommand = GetWindowsCommand(FoundProcess)
        End If

        'Get Process Path
        ProcessPath = GetProcessPath(bWineProcess)

        'Look for any games using parameters and any matches
        For Each oDetectedGame As clsGame In oDetectedGames
            If oDetectedGame.Parameter <> String.Empty Then
                If sFullCommand.Contains(oDetectedGame.Parameter) Then
                    oDetectedWithParameters.Add(oDetectedGame)
                Else
                    oNotDetectedWithParameters.Add(oDetectedGame)
                End If

            End If
        Next

        'If we detected at least one parameter match, replace full detected list with the detected with parameter list
        If oDetectedWithParameters.Count > 0 Then
            oDetectedGames = oDetectedWithParameters
        Else
            'If there is no parameter match, remove any games using parameters from the detected list
            For Each oGameNotDetected As clsGame In oNotDetectedWithParameters
                oDetectedGames.Remove(oGameNotDetected)
            Next
        End If

        'If there's only one match after parameter detection, set it as current game and we're done.
        If oDetectedGames.Count = 1 Then
            GameInfo = oDetectedGames(0)
            Duplicate = False
        Else
            'Check if we have any exact matches based on process path
            For Each oDetectedGame As clsGame In oDetectedGames
                If oDetectedGame.ProcessPath <> String.Empty Then
                    If oDetectedGame.ProcessPath = ProcessPath Then
                        oDetectedWithProcessPath.Add(oDetectedGame)
                    Else
                        oNotDetectedWithProcessPath.Add(oDetectedGame)
                    End If
                End If
            Next

            'If there's only one match after process detection, set it as current game and we're done
            If oDetectedWithProcessPath.Count = 1 Then
                GameInfo = oDetectedWithProcessPath(0)
                Duplicate = False
            Else
                'Remove any games with a process path that does not match the current process
                For Each oGameNotDetected As clsGame In oNotDetectedWithProcessPath
                    oDetectedGames.Remove(oGameNotDetected)
                Next

                'If only a single game remains, set it as current game and we're done
                If oDetectedGames.Count = 1 Then
                    GameInfo = oDetectedGames(0)
                    Duplicate = False
                Else
                    'We've done all we can, the user must selected which game they were playing when the process ends
                    Duplicate = True
                    oDuplicateGames = oDetectedGames
                End If
            End If
        End If
    End Sub

    Public Function SearchRunningProcesses(ByVal hshScanList As Hashtable, ByRef bNeedsPath As Boolean, ByRef bWineProcess As Boolean, ByRef iErrorCode As Integer, ByVal bDebugMode As Boolean) As Boolean
        Dim prsList() As Process = Process.GetProcesses
        Dim sProcessCheck As String = String.Empty
        Dim sProcessList As String = String.Empty
        Dim oDetectedGames As New ArrayList

        For Each prsCurrent As Process In prsList

            'This needs to be wrapped due to issues with Mono.
            Try
                sProcessCheck = prsCurrent.ProcessName

                'Unix Handler
                'We need some special handling for Wine processes
                If mgrCommon.IsUnix And (sProcessCheck.ToLower = "wine-preloader" Or sProcessCheck.ToLower = "wine64-preloader") Then
                    Dim sArgs As String() = GetUnixProcessArguments(prsCurrent)
                    Dim sParameter As String
                    Dim sWinePath As String()
                    'The wine-preloader parameters can refer to a path on the host system, windows based path within in the prefix, or mixed notation.
                    sParameter = sArgs(0).Replace("\", "/")
                    sWinePath = sParameter.Split("/")
                    sProcessCheck = Path.GetFileNameWithoutExtension(sWinePath(sWinePath.Length - 1))
                    bWineProcess = True
                Else
                    bWineProcess = False
                End If

                If bDebugMode And mgrCommon.IsUnix Then
                    sProcessList &= prsCurrent.Id & " " & prsCurrent.ProcessName & " " & GetUnixProcessArguments(prsCurrent)(0) & vbCrLf
                ElseIf bDebugMode Then
                    sProcessList &= prsCurrent.Id & " " & prsCurrent.ProcessName & vbCrLf
                End If
            Catch ex As Exception
                'Do Nothing
            End Try

            For Each oCurrentGame As clsGame In hshScanList.Values
                If IsMatch(oCurrentGame, sProcessCheck) Then
                    prsFoundProcess = prsCurrent
                    oGame = oCurrentGame.ShallowCopy
                    oDetectedGames.Add(oGame.ShallowCopy)
                End If
            Next

            If oDetectedGames.Count > 0 Then
                FilterDetected(oDetectedGames, bWineProcess)
            End If

            If oDetectedGames.Count > 0 Then
                If Not oGame.AbsolutePath And Not oGame.MonitorOnly Then
                    Try
                        If Not bWineProcess Then
                            oGame.ProcessPath = Path.GetDirectoryName(prsCurrent.MainModule.FileName)
                        Else
                            oGame.ProcessPath = GetUnixSymLinkDirectory(prsCurrent)
                        End If
                    Catch exWin32 As System.ComponentModel.Win32Exception
                        If exWin32.NativeErrorCode = 5 Then
                            bNeedsPath = True
                            iErrorCode = 5
                        ElseIf exWin32.NativeErrorCode = 299 Then
                            bNeedsPath = True
                            iErrorCode = 299
                        Else
                            If bDebugMode Then mgrCommon.ShowMessage(exWin32.NativeErrorCode & " " & exWin32.Message & vbCrLf & vbCrLf & exWin32.StackTrace, MsgBoxStyle.Critical)
                            Return False
                        End If
                    Catch exAll As Exception
                        If bDebugMode Then mgrCommon.ShowMessage(exAll.Message & vbCrLf & vbCrLf & exAll.StackTrace, MsgBoxStyle.Critical)
                        Return False
                    End Try
                End If

                If bDebugMode Then mgrCommon.SaveText(sProcessList, mgrPath.SettingsRoot & "/gbm_process_list.txt")

                'This will force two cycles for detection to try and prevent issues with UAC prompt
                If Not bVerified Then
                    bVerified = True
                    Return False
                Else
                    bVerified = False
                    Return True
                End If
            End If
        Next

        Return False
    End Function

End Class
