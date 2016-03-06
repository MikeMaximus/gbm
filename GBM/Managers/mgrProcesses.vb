Imports System.Diagnostics
Imports System.IO
Imports System.Threading

Public Class mgrProcesses

    Private prsFoundProcess As Process
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

    Private Sub VerifyDuplicate(oGame As clsGame, hshScanList As Hashtable)
        Dim sProcess As String
        bDuplicates = True
        oDuplicateGames.Clear()
        For Each o As clsGame In hshScanList.Values
            sProcess = o.ProcessName.Split(":")(0)

            If o.Duplicate = True And sProcess = oGame.TrueProcess Then
                oDuplicateGames.Add(o.ShallowCopy)
            End If
        Next
    End Sub

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
        Dim slsinfo As String()

        'This is the best way I can think of to determine the end point of a symlink without doing even more crazy shit
        Try
            prsls = New Process
            prsls.StartInfo.FileName = "/bin/bash"
            prsls.StartInfo.Arguments = "-c ""ls -l /proc/" & prs.Id.ToString & " | grep cwd"""
            prsls.StartInfo.UseShellExecute = False
            prsls.StartInfo.RedirectStandardOutput = True
            prsls.StartInfo.CreateNoWindow = True
            prsls.Start()
            slsinfo = prsls.StandardOutput.ReadToEnd().Split(">")
            Return slsinfo(slsinfo.Length - 1).Trim
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function SearchRunningProcesses(ByVal hshScanList As Hashtable, ByRef bNeedsPath As Boolean, ByRef iErrorCode As Integer, ByVal bDebugMode As Boolean) As Boolean
        Dim prsList() As Process = Process.GetProcesses
        Dim sProcessCheck As String = String.Empty
        Dim sProcessList As String = String.Empty
        Dim bWineProcess As Boolean = False

        For Each prsCurrent As Process In prsList
            'This needs to be wrapped due to issues with Mono.
            Try
                sProcessCheck = prsCurrent.ProcessName

                'Unix Handler
                'We need some special handling for Wine processes
                If mgrCommon.IsUnix And sProcessCheck.ToLower = "wine-preloader" Then
                    Dim sWinePath As String()
                    'We can't use Path.GetFileName here, Wine uses the Windows seperator in arguments and the Unix version of the function expects a different one.
                    sWinePath = GetUnixProcessArguments(prsCurrent)(0).Split("\")
                    sProcessCheck = sWinePath(sWinePath.Length - 1).Replace(".exe", "")
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

            If hshScanList.ContainsKey(sProcessCheck) Then
                prsFoundProcess = prsCurrent
                oGame = DirectCast(hshScanList.Item(sProcessCheck), clsGame).ShallowCopy

                If oGame.Duplicate = True Then
                    VerifyDuplicate(oGame, hshScanList)
                Else
                    bDuplicates = False
                    oDuplicateGames.Clear()
                End If

                If Not oGame.AbsolutePath Or oGame.Duplicate Then
                    Try
                        If Not bWineProcess Then
                            oGame.ProcessPath = Path.GetDirectoryName(prsCurrent.MainModule.FileName)
                        Else
                            oGame.ProcessPath = GetUnixSymLinkDirectory(prsCurrent)
                        End If
                    Catch exWin32 As System.ComponentModel.Win32Exception
                        'If an exception occurs the process is:
                        'Running as administrator and the app isn't.
                        'The process is 64-bit and the process folder is required, shouldn't happen often.                        
                        If exWin32.NativeErrorCode = 5 Then
                            bNeedsPath = True
                            iErrorCode = 5
                        ElseIf exWin32.NativeErrorCode = 299 Then
                            bNeedsPath = True
                            iErrorCode = 299
                        Else
                            If bDebugMode Then mgrCommon.ShowMessage(exWin32.NativeErrorCode & " " & exWin32.Message & vbCrLf & vbCrLf & exWin32.StackTrace, MsgBoxStyle.Critical)
                            'A different failure occured,  drop out and continue to scan.
                            Return False
                        End If
                    Catch exAll As Exception
                        If bDebugMode Then mgrCommon.ShowMessage(exAll.Message & vbCrLf & vbCrLf & exAll.StackTrace, MsgBoxStyle.Critical)
                        'A different failure occured,  drop out and continue to scan.
                        Return False
                    End Try
                End If

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

        If bDebugMode Then mgrCommon.SaveText(sProcessList, mgrPath.SettingsRoot & "/gbm_process_list.txt")

        Return False
    End Function

End Class
