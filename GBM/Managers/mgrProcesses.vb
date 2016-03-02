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

    Public Function SearchRunningProcesses(ByVal hshScanList As Hashtable, ByRef bNeedsPath As Boolean, ByRef iErrorCode As Integer) As Boolean
        Dim prsList() As Process = Process.GetProcesses
        Dim sProcessCheck As String = String.Empty

        For Each prsCurrent As Process In prsList
            Try
                sProcessCheck = prsCurrent.ProcessName               
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
                        oGame.ProcessPath = Path.GetDirectoryName(prsCurrent.MainModule.FileName)
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
                            MsgBox(exWin32.Message & vbCrLf & exWin32.StackTrace)
                            'A different failure occured,  drop out and continue to scan.
                            Return False
                        End If
                    Catch exAll As Exception
                        MsgBox(exAll.Message & vbCrLf & exAll.StackTrace)
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

        Return False
    End Function

End Class
