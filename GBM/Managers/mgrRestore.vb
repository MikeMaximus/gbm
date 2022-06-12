Imports GBM.My.Resources
Imports System.IO

Public Class mgrRestore
    Public Property CancelOperation As Boolean

    Public Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)
    Public Event UpdateRestoreInfo(oRestoreInfo As clsBackup)
    Public Event SetLastAction(sMessage As String)

    Public Shared Function CheckPath(ByRef oRestoreInfo As clsBackup, ByVal oGame As clsGame, ByRef bTriggerReload As Boolean, Optional ByVal bFastMode As Boolean = False) As Boolean
        Dim sProcess As String
        Dim sRestorePath As String
        Dim bNoAuto As Boolean

        If Not oRestoreInfo.AbsolutePath Then
            If oGame.ProcessPath <> String.Empty Then
                oRestoreInfo.RelativeRestorePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oRestoreInfo.Path
            ElseIf Not bFastMode Then
                sProcess = oGame.ProcessName
                If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                sRestorePath = mgrPath.ProcessPathSearch(oRestoreInfo.Name, sProcess, mgrCommon.FormatString(mgrRestore_RelativeNeedPath, oRestoreInfo.Name), bNoAuto)

                If sRestorePath <> String.Empty Then
                    'Update the process path in game object, save it, and make sure a monitor list reload is triggered
                    oGame.ProcessPath = sRestorePath
                    mgrMonitorList.DoListUpdate(oGame)
                    bTriggerReload = True

                    'Set path for restore
                    oRestoreInfo.RelativeRestorePath = sRestorePath & Path.DirectorySeparatorChar & oRestoreInfo.Path
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If

        Return True
    End Function

    Public Shared Function CheckManifest(ByVal sMonitorID As String) As Boolean
        Dim slLocalManifest As SortedList
        Dim slRemoteManifest As SortedList
        Dim oLocalItem As New clsBackup
        Dim oRemoteItem As New clsBackup
        Dim bLocal As Boolean = False
        Dim bRemote As Boolean = False

        slLocalManifest = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Local)
        slRemoteManifest = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Remote)

        If slLocalManifest.Contains(sMonitorID) Then
            oLocalItem = DirectCast(slLocalManifest(sMonitorID), clsBackup)
            bLocal = True
        End If

        If slRemoteManifest.Contains(sMonitorID) Then
            oRemoteItem = DirectCast(slRemoteManifest(sMonitorID), clsBackup)
            bRemote = True
        End If

        If bLocal And bRemote Then
            'Compare
            If oRemoteItem.DateUpdated > oLocalItem.DateUpdated Then
                Return True
            End If
        End If

        If bRemote And Not bLocal Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function CompareManifests() As SortedList
        Dim slLocalManifest As SortedList
        Dim slRemoteManifest As SortedList
        Dim oLocalItem As clsBackup
        Dim slRestoreItems As New SortedList
        Dim bLocal As Boolean = False
        Dim bRemote As Boolean = False

        slLocalManifest = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Local)
        slRemoteManifest = mgrManifest.ReadLatestManifest(mgrSQLite.Database.Remote)

        For Each oItem As clsBackup In slRemoteManifest.Values
            If slLocalManifest.Contains(oItem.MonitorID) Then
                oLocalItem = DirectCast(slLocalManifest(oItem.MonitorID), clsBackup)

                If oItem.DateUpdated > oLocalItem.DateUpdated Then
                    slRestoreItems.Add(oItem.MonitorID, oItem)
                End If
            Else
                slRestoreItems.Add(oItem.MonitorID, oItem)
            End If
        Next

        Return slRestoreItems
    End Function

    Private Function CreateRestoreFolder(ByVal sExtractPath As String, Optional ByVal bCleanFolder As Boolean = False) As Boolean
        Try
            If bCleanFolder Then mgrCommon.DeleteDirectory(sExtractPath, True)
            Directory.CreateDirectory(sExtractPath)
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorCreatePath, ex.Message), False, ToolTipIcon.Error, True)
            Return False
        End Try

        Return True
    End Function

    Public Function CheckRestorePrereq(ByVal oBackupInfo As clsBackup, ByVal bCleanFolder As Boolean, Optional ByVal bFastMode As Boolean = False) As Boolean
        Dim sHash As String
        Dim sExtractPath As String
        Dim bRegistry As Boolean
        Dim sBackupFile As String = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oBackupInfo.FileName

        'Check if this is a registry backup
        bRegistry = mgrPath.IsSupportedRegistryPath(oBackupInfo.TruePath)

        If Not bRegistry Then
            If oBackupInfo.AbsolutePath Then
                sExtractPath = oBackupInfo.Path
            Else
                sExtractPath = oBackupInfo.RelativeRestorePath
            End If

            'Check if restore location exists, prompt to create if it doesn't.
            If Not Directory.Exists(sExtractPath) And Not bFastMode Then
                If mgrCommon.ShowMessage(mgrRestore_ConfirmCreatePath, sExtractPath, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    If CreateRestoreFolder(sExtractPath) = False Then
                        Return False
                    End If
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorNoPath, sExtractPath), False, ToolTipIcon.Error, True)
                    Return False
                End If
            ElseIf Not Directory.Exists(sExtractPath) And bFastMode Then
                If CreateRestoreFolder(sExtractPath) = False Then
                    Return False
                End If
            Else
                If bCleanFolder Then
                    If CreateRestoreFolder(sExtractPath, bCleanFolder) = False Then
                        Return False
                    End If
                End If
            End If
        End If

        'Check file integrity
        If oBackupInfo.CheckSum <> String.Empty Then
            sHash = mgrHash.Generate_SHA256_Hash(sBackupFile)
            If sHash <> oBackupInfo.CheckSum Then
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorFailedCheck, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                If Not bFastMode Then
                    If mgrCommon.ShowMessage(mgrRestore_ConfirmFailedCheck, oBackupInfo.Name, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        RaiseEvent UpdateLog(mgrRestore_ErrorCheckAbort, False, ToolTipIcon.Info, True)
                        Return False
                    End If
                Else
                    RaiseEvent UpdateLog(mgrRestore_ErrorCheckAbort, False, ToolTipIcon.Info, True)
                    Return False
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_Verified, New String() {oBackupInfo.Name, oBackupInfo.DateUpdated.ToString}), False, ToolTipIcon.Info, True)
            End If
        Else
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_NoVerify, New String() {oBackupInfo.Name, oBackupInfo.DateUpdated.ToString}), False, ToolTipIcon.Info, True)
        End If

        Return True
    End Function

    Private Function RunRegistryRestore(ByVal oBackupInfo As clsBackup, ByVal sBackupFile As String, Optional ByVal bAdmin As Boolean = False) As Boolean
        Dim prsReg As New Process
        Dim sBinaryPath As String
        Dim sArguments As String
        Dim oWineData As clsWineData
        Dim sWineReg As String
        Dim bPathVerified As Boolean
        Dim bRestoreCompleted As Boolean = False

        sArguments = "import """ & sBackupFile & """"

        If mgrCommon.IsUnix Then
            oWineData = mgrWineData.DoWineDataGetbyID(oBackupInfo.MonitorID)
            prsReg.StartInfo.EnvironmentVariables.Add("WINEPREFIX", oWineData.Prefix)
            sBinaryPath = oWineData.BinaryPath & Path.DirectorySeparatorChar & "wine"
            sWineReg = oWineData.Prefix & Path.DirectorySeparatorChar & "drive_c/windows/system32/reg.exe"
            sArguments = """" & sWineReg & """ " & sArguments
            If File.Exists(sBinaryPath) Then
                If File.Exists(sWineReg) Then
                    bPathVerified = True
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorRegNotFound, sWineReg), False, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorWineNotFound, sBinaryPath), False, ToolTipIcon.Error, True)
            End If
        Else
            sBinaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & Path.DirectorySeparatorChar & "system32\reg.exe"
            If File.Exists(sBinaryPath) Then
                bPathVerified = True
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorRegNotFound, sBinaryPath), False, ToolTipIcon.Error, True)
            End If
        End If

        If bPathVerified Then
            Try
                prsReg.StartInfo.Arguments = sArguments
                prsReg.StartInfo.FileName = sBinaryPath
                prsReg.StartInfo.UseShellExecute = False
                prsReg.StartInfo.RedirectStandardOutput = True
                prsReg.StartInfo.CreateNoWindow = True
                If bAdmin Then prsReg.StartInfo.Verb = "runas"
                prsReg.Start()
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_RestoreInProgress, oBackupInfo.TruePath), False, ToolTipIcon.Info, True)
                prsReg.WaitForExit()
                Select Case prsReg.ExitCode
                    Case 0
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_RestoreComplete, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                        bRestoreCompleted = True
                    Case Else
                        RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Restore), True, ToolTipIcon.Warning, True)
                End Select
                prsReg.Dispose()
            Catch exWin32 As System.ComponentModel.Win32Exception
                'If the launch fails due to required elevation, try it again and request elevation.
                If exWin32.ErrorCode = 740 Then
                    RunRegistryRestore(oBackupInfo, sBackupFile, True)
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Restore, exWin32.Message}), False, ToolTipIcon.Error, True)
                End If
            Catch exAll As Exception
                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Restore, exAll.Message}), False, ToolTipIcon.Error, True)
            End Try
        End If

        Return bRestoreCompleted
    End Function

    Private Function Run7zRestore(ByVal oBackupInfo As clsBackup, ByVal sBackupFile As String) As Boolean
        Dim prs7z As New Process
        Dim sExtractPath As String
        Dim bRestoreCompleted As Boolean = False

        If oBackupInfo.AbsolutePath Then
            sExtractPath = oBackupInfo.Path
        Else
            sExtractPath = oBackupInfo.RelativeRestorePath
        End If

        Try
            If File.Exists(sBackupFile) Then
                If mgrSettings.Is7zUtilityValid Then
                    prs7z.StartInfo.Arguments = "x" & mgrSettings.Prepared7zArguments & """" & sBackupFile & """ -o""" & sExtractPath & Path.DirectorySeparatorChar & """ -x!" & App_MetadataFilename & " -aoa -r"
                    prs7z.StartInfo.FileName = mgrSettings.Utility7zLocation
                    prs7z.StartInfo.UseShellExecute = False
                    prs7z.StartInfo.RedirectStandardOutput = True
                    prs7z.StartInfo.CreateNoWindow = True
                    prs7z.Start()
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_RestoreInProgress, sExtractPath), False, ToolTipIcon.Info, True)
                    While Not prs7z.StandardOutput.EndOfStream
                        If CancelOperation Then
                            prs7z.Kill()
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorFullAbort, oBackupInfo.Name), True, ToolTipIcon.Error, True)
                            Exit While
                        End If
                        RaiseEvent UpdateLog(prs7z.StandardOutput.ReadLine, False, ToolTipIcon.Info, False)
                    End While
                    prs7z.WaitForExit()
                    If Not CancelOperation Then
                        If prs7z.ExitCode = 0 Then
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_RestoreComplete, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                            bRestoreCompleted = True
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Restore), True, ToolTipIcon.Warning, True)
                        End If
                    End If
                    prs7z.Dispose()
                Else
                    RaiseEvent UpdateLog(App_Invalid7zDetected, True, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrRestore_ErrorNoBackup, True, ToolTipIcon.Error, True)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Restore, ex.Message}), False, ToolTipIcon.Error, True)
        End Try

        Return bRestoreCompleted
    End Function

    Public Sub DoRestore(ByVal oRestoreList As List(Of clsBackup))
        Dim sBackupFile As String
        Dim sExtractPath As String
        Dim bRestoreCompleted As Boolean

        For Each oBackupInfo In oRestoreList
            'Break out when a cancel signal is received
            If CancelOperation Then
                RaiseEvent UpdateLog(mgrRestore_FullAbort, False, ToolTipIcon.Warning, True)
                Exit For
            End If

            'Init
            sBackupFile = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oBackupInfo.FileName
            sExtractPath = String.Empty
            bRestoreCompleted = False
            CancelOperation = False
            RaiseEvent UpdateRestoreInfo(oBackupInfo)

            If mgrPath.IsSupportedRegistryPath(oBackupInfo.TruePath) Then
                bRestoreCompleted = RunRegistryRestore(oBackupInfo, sBackupFile)
            Else
                bRestoreCompleted = Run7zRestore(oBackupInfo, sBackupFile)
            End If

            If bRestoreCompleted Then
                'Save Local Manifest
                If mgrManifest.DoManifestCheck(oBackupInfo.MonitorID, mgrSQLite.Database.Local) Then
                    mgrManifest.DoManifestUpdateByMonitorID(oBackupInfo, mgrSQLite.Database.Local)
                Else
                    mgrManifest.DoManifestAdd(oBackupInfo, mgrSQLite.Database.Local)
                End If
                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrRestore_ActionComplete, oBackupInfo.CroppedName))
            Else
                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrRestore_ActionFailed, oBackupInfo.CroppedName))
            End If
        Next
    End Sub

End Class
