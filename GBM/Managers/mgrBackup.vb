Imports GBM.My.Resources
Imports System.IO

Public Class mgrBackup

    Private oSettings As mgrSettings
    Private bCancelOperation As Boolean

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Property CancelOperation As Boolean
        Get
            Return bCancelOperation
        End Get
        Set(value As Boolean)
            bCancelOperation = value
        End Set
    End Property

    Public Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)
    Public Event UpdateBackupInfo(oGame As clsGame)
    Public Event SetLastAction(sMessage As String)

    Public Function CheckForUtilities(ByVal strPath As String) As Boolean
        If File.Exists(strPath) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function DoManifestUpdate(ByVal oGameInfo As clsGame, ByVal sBackupFile As String, ByVal dTimeStamp As DateTime, ByVal sCheckSum As String) As Boolean
        Dim oItem As New clsBackup

        'Create manifest item
        oItem.MonitorID = oGameInfo.ID
        'Keep the path relative to the manifest location
        oItem.FileName = sBackupFile.Replace(Settings.BackupFolder & Path.DirectorySeparatorChar, String.Empty)
        oItem.DateUpdated = dTimeStamp
        oItem.UpdatedBy = My.Computer.Name
        oItem.CheckSum = sCheckSum

        'Save Remote Manifest
        If Not oGameInfo.AppendTimeStamp Then
            If Not mgrManifest.DoUpdateLatestManifest(oItem, mgrSQLite.Database.Remote) Then
                mgrManifest.DoManifestAdd(oItem, mgrSQLite.Database.Remote)
            End If
        Else
                mgrManifest.DoManifestAdd(oItem, mgrSQLite.Database.Remote)
        End If

        'Save Local Manifest
        If mgrManifest.DoManifestCheck(oItem.MonitorID, mgrSQLite.Database.Local) Then
            mgrManifest.DoManifestUpdateByMonitorID(oItem, mgrSQLite.Database.Local)
        Else
            mgrManifest.DoManifestAdd(oItem, mgrSQLite.Database.Local)
        End If

        Return True
    End Function

    Private Sub BuildFileList(ByVal sList As String, ByVal sPath As String)
        Dim oStream As StreamWriter

        Try
            If File.Exists(sPath) Then File.Delete(sPath)
            oStream = New StreamWriter(sPath)
            Using oStream
                If sList <> String.Empty Then
                    For Each sTypeItem As String In sList.Split(":")
                        oStream.WriteLine("""" & sTypeItem & """")
                    Next
                End If
                oStream.Flush()
            End Using
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorFileList, ex.Message), False, ToolTipIcon.Error, True)
        End Try
    End Sub

    Private Function VerifySavePath(ByVal oGame As clsGame) As String
        Dim sSavePath As String

        If oGame.AbsolutePath = False Then
            If oGame.Path <> String.Empty Then
                sSavePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oGame.Path
            Else
                sSavePath = oGame.ProcessPath
            End If
        Else
            sSavePath = oGame.Path
        End If

        Return sSavePath
    End Function

    Private Function GetFileName(ByVal oGame As clsGame) As String
        Dim sName As String

        If oSettings.UseGameID Then
            sName = oGame.ID
        Else
            sName = oGame.FileSafeName
        End If

        Return sName
    End Function

    Public Function CheckBackupPrereq(ByVal oGame As clsGame) As Boolean
        Dim sBackupFile As String = oSettings.BackupFolder
        Dim sSavePath As String
        Dim sOverwriteMessage As String
        Dim lAvailableSpace As Long
        Dim lFolderSize As Long = 0
        Dim sDeepFolder As String

        If oSettings.CreateSubFolder Then sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)
        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & ".7z"

        'Verify saved game path
        sSavePath = VerifySavePath(oGame)

        'Check if disk space check should be disabled (UNC path or Setting)
        If Not mgrPath.IsPathUNC(oSettings.BackupFolder) And Not Settings.DisableDiskSpaceCheck Then
            'Calculate space
            lAvailableSpace = mgrCommon.GetAvailableDiskSpace(oSettings.BackupFolder)

            'If any includes are using a deep path and we aren't using recursion,  we need to go directly to folders to do file size calculations or they will be missed.
            If Not oGame.RecurseSubFolders Then
                For Each s As String In oGame.IncludeArray
                    If s.Contains(Path.DirectorySeparatorChar) Then
                        sDeepFolder = Path.GetDirectoryName(sSavePath & Path.DirectorySeparatorChar & s)
                        If Directory.Exists(sDeepFolder) Then
                            lFolderSize += mgrCommon.GetFolderSize(sDeepFolder, oGame.IncludeArray, oGame.ExcludeArray, oGame.RecurseSubFolders)
                        End If
                    End If
                Next
            End If
            lFolderSize += mgrCommon.GetFolderSize(sSavePath, oGame.IncludeArray, oGame.ExcludeArray, oGame.RecurseSubFolders)

            'Show Available Space
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrCommon_AvailableDiskSpace, mgrCommon.FormatDiskSpace(lAvailableSpace)), False, ToolTipIcon.Info, True)

            'Show Save Folder Size
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrCommon_SavedGameFolderSize, New String() {oGame.Name, mgrCommon.FormatDiskSpace(lFolderSize)}), False, ToolTipIcon.Info, True)

            If lFolderSize >= lAvailableSpace Then
                If mgrCommon.ShowMessage(mgrBackup_ConfirmDiskSpace, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    RaiseEvent UpdateLog(mgrBackup_ErrorDiskSpace, False, ToolTipIcon.Error, True)
                    Return False
                End If
            End If
        Else
            'Show that disk space check was skipped due to UNC path
            If Not Settings.DisableDiskSpaceCheck Then RaiseEvent UpdateLog(mgrBackup_ErrorBackupPathIsUNC, False, ToolTipIcon.Info, True)
        End If

        'A manifest check is only required when "Save Multiple Backups" is disabled
        If Not oGame.AppendTimeStamp Then
            If mgrRestore.CheckManifest(oGame.ID) Then
                If mgrCommon.ShowMessage(mgrBackup_ConfirmManifestConflict, oGame.Name, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    RaiseEvent UpdateLog(mgrBackup_ErrorManifestConflict, False, ToolTipIcon.Error, True)
                    Return False
                End If
            End If
        End If

        If oSettings.ShowOverwriteWarning And File.Exists(sBackupFile) And Not oGame.AppendTimeStamp Then
            If oGame.AbsolutePath Then
                sOverwriteMessage = mgrBackup_ConfirmOverwrite
            Else
                sOverwriteMessage = mgrBackup_ConfirmOverwriteRelative
            End If

            If mgrCommon.ShowMessage(sOverwriteMessage, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorOverwriteAbort, oGame.Name), False, ToolTipIcon.Error, True)
                Return False
            End If
        End If

            Return True
    End Function

    Private Sub CheckOldBackups(ByVal oGame As clsGame)
        Dim oGameBackups As List(Of clsBackup) = mgrManifest.DoManifestGetByMonitorID(oGame.ID, mgrSQLite.Database.Remote)
        Dim oGameBackup As clsBackup
        Dim sOldBackup As String
        Dim iBackupCount As Integer = oGameBackups.Count
        Dim iDelCount As Integer

        'If we've hit or exceeded the maximum backup limit
        If oGameBackups.Count >= oGame.BackupLimit Then
            'How many do we need to delete
            iDelCount = (oGameBackups.Count - oGame.BackupLimit) + 1

            'Delete the oldest backup(s) (Manifest entry and backup file)
            For i = 1 To iDelCount
                oGameBackup = oGameBackups(oGameBackups.Count - i)
                sOldBackup = Settings.BackupFolder & Path.DirectorySeparatorChar & oGameBackup.FileName

                mgrManifest.DoManifestDeleteByManifestID(oGameBackup, mgrSQLite.Database.Remote)
                mgrManifest.DoManifestDeleteByManifestID(oGameBackup, mgrSQLite.Database.Local)
                mgrCommon.DeleteFile(sOldBackup)
                mgrCommon.DeleteDirectoryByBackup(Settings.BackupFolder & Path.DirectorySeparatorChar, oGameBackup)

                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupLimitExceeded, Path.GetFileName(sOldBackup)), False, ToolTipIcon.Info, True)
            Next
        End If
    End Sub

    Private Function BuildFileTimeStamp(ByVal dDate As Date) As String
        Return " " & dDate.Month & "-" & dDate.Day & "-" & dDate.Year & "-" & dDate.Hour & "-" & dDate.Minute & "-" & dDate.Second
    End Function

    Private Function HandleSubFolder(ByVal oGame As clsGame, ByVal sPath As String) As Boolean
        Try
            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorSubFolderCreate, New String() {oGame.Name, ex.Message}), False, ToolTipIcon.Error, True)
            Return False
        End Try

        Return True
    End Function

    Public Sub ImportBackupFiles(ByVal hshImportList As Hashtable)
        Dim oGame As clsGame
        Dim bOverwriteCurrent As Boolean = False
        Dim bContinue As Boolean = True
        Dim sFileToImport As String
        Dim sBackupFile As String
        Dim oBackup As clsBackup

        For Each de As DictionaryEntry In hshImportList
            sFileToImport = CStr(de.Key)
            oGame = DirectCast(de.Value, clsGame)

            'Enter overwite mode if we are importing a single backup and "Save Multiple Backups" is not enabled.
            If hshImportList.Count = 1 And Not oGame.AppendTimeStamp Then bOverwriteCurrent = True

            If File.Exists(sFileToImport) Then
                sBackupFile = oSettings.BackupFolder

                If oSettings.CreateSubFolder Then
                    sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)
                    bContinue = HandleSubFolder(oGame, sBackupFile)
                End If

                If bContinue Then
                    oBackup = New clsBackup
                    oBackup.MonitorID = oGame.ID
                    oBackup.DateUpdated = File.GetLastWriteTime(sFileToImport)
                    oBackup.UpdatedBy = mgrBackup_ImportedFile
                    If bOverwriteCurrent Then
                        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & ".7z"
                    Else
                        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & BuildFileTimeStamp(oBackup.DateUpdated) & ".7z"
                    End If

                    oBackup.FileName = sBackupFile.Replace(Settings.BackupFolder & Path.DirectorySeparatorChar, String.Empty)

                    If bOverwriteCurrent Then
                        If mgrCommon.CopyFile(sFileToImport, sBackupFile, True) Then
                            oBackup.CheckSum = mgrHash.Generate_SHA256_Hash(sBackupFile)
                            If Not mgrManifest.DoUpdateLatestManifest(oBackup, mgrSQLite.Database.Remote) Then
                                mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
                            End If
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ImportSuccess, New String() {sFileToImport, oGame.Name}), False, ToolTipIcon.Info, True)
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportBackupCopy, sFileToImport), False, ToolTipIcon.Error, True)
                        End If
                    Else
                        If mgrCommon.CopyFile(sFileToImport, sBackupFile, False) Then
                            oBackup.CheckSum = mgrHash.Generate_SHA256_Hash(sBackupFile)
                            mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ImportSuccess, New String() {sFileToImport, oGame.Name}), False, ToolTipIcon.Info, True)
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportBackupCopy, sFileToImport), False, ToolTipIcon.Error, True)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub DoBackup(ByVal oBackupList As List(Of clsGame))
        Dim oGame As clsGame
        Dim bDoBackup As Boolean
        Dim bBackupCompleted As Boolean
        Dim prs7z As Process
        Dim sBackupFile As String
        Dim sSavePath As String
        Dim dTimeStamp As DateTime
        Dim sTimeStamp As String
        Dim sHash As String
        Dim sArguments As String

        For Each oGame In oBackupList
            'Init
            prs7z = New Process
            sBackupFile = oSettings.BackupFolder
            sSavePath = String.Empty
            dTimeStamp = Date.Now
            sTimeStamp = BuildFileTimeStamp(dTimeStamp)
            sHash = String.Empty
            bDoBackup = True
            bBackupCompleted = False
            CancelOperation = False
            RaiseEvent UpdateBackupInfo(oGame)

            If oSettings.CreateSubFolder Then
                sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)
                bDoBackup = HandleSubFolder(oGame, sBackupFile)
            End If

            If oGame.AppendTimeStamp Then
                If oGame.BackupLimit > 0 Then CheckOldBackups(oGame)
                sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & sTimeStamp & ".7z"
            Else
                sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & ".7z"
            End If

            If bDoBackup Then

                sSavePath = VerifySavePath(oGame)

                If oGame.FolderSave = True Then
                    BuildFileList("*", mgrPath.IncludeFileLocation)
                Else
                    BuildFileList(oGame.FileType, mgrPath.IncludeFileLocation)
                End If

                BuildFileList(oGame.ExcludeList, mgrPath.ExcludeFileLocation)

                sArguments = "a" & oSettings.Prepared7zArguments & "-t7z -mx" & oSettings.CompressionLevel & " -i@""" & mgrPath.IncludeFileLocation & """ -x@""" & mgrPath.ExcludeFileLocation & """ """ & sBackupFile & """"

                If oGame.RecurseSubFolders Then sArguments &= " -r"

                Try
                    If Directory.Exists(sSavePath) Then
                        If Settings.Is7zUtilityValid Then
                            'Need to delete any prior archive if it exists, the 7za utility does not support overwriting or deleting existing archives.
                            'If we let 7za update existing archives it will lead to excessive bloat with games that routinely add and remove files with many different file names.
                            If File.Exists(sBackupFile) Then
                                File.Delete(sBackupFile)
                            End If

                            prs7z.StartInfo.Arguments = sArguments
                            prs7z.StartInfo.FileName = oSettings.Utility7zLocation
                            prs7z.StartInfo.WorkingDirectory = sSavePath
                            prs7z.StartInfo.UseShellExecute = False
                            prs7z.StartInfo.RedirectStandardOutput = True
                            prs7z.StartInfo.CreateNoWindow = True
                            prs7z.Start()
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupInProgress, sSavePath), False, ToolTipIcon.Info, True)
                            While Not prs7z.StandardOutput.EndOfStream
                                If CancelOperation Then
                                    prs7z.Kill()
                                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorFullAbort, oGame.Name), True, ToolTipIcon.Error, True)
                                    Exit While
                                End If
                                RaiseEvent UpdateLog(prs7z.StandardOutput.ReadLine, False, ToolTipIcon.Info, False)
                            End While
                            prs7z.WaitForExit()
                            If Not CancelOperation Then
                                Select Case prs7z.ExitCode
                                    Case 0
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupComplete, New String() {oGame.Name, mgrCommon.FormatDiskSpace(mgrCommon.GetFileSize(sBackupFile))}), False, ToolTipIcon.Info, True)
                                        bBackupCompleted = True
                                    Case 1
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_7zWarnings, oGame.Name), True, ToolTipIcon.Warning, True)
                                        bBackupCompleted = True
                                    Case 2
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_7zFatalError, oGame.Name), True, ToolTipIcon.Error, True)
                                        bBackupCompleted = False
                                    Case 7
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_7zCommandFailure, oGame.Name), True, ToolTipIcon.Error, True)
                                        bBackupCompleted = False
                                End Select
                            End If
                            prs7z.Dispose()
                        Else
                            RaiseEvent UpdateLog(App_Invalid7zDetected, True, ToolTipIcon.Error, True)
                            bBackupCompleted = False
                        End If
                    Else
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorNoSavePath, oGame.Name), True, ToolTipIcon.Error, True)
                        bBackupCompleted = False
                    End If

                    'Write Main Manifest
                    If bBackupCompleted Then

                        'Generate checksum for new backup
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_GenerateHash, oGame.Name), False, ToolTipIcon.Info, True)
                        sHash = mgrHash.Generate_SHA256_Hash(sBackupFile)

                        If Not DoManifestUpdate(oGame, sBackupFile, dTimeStamp, sHash) Then
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorManifestFailure, oGame.Name), True, ToolTipIcon.Error, True)
                        End If

                        'Write the process path if we have it
                        If oGame.AbsolutePath = False Then
                            mgrMonitorList.DoListFieldUpdate("ProcessPath", oGame.ProcessPath, oGame.ID)
                        End If
                    End If
                Catch ex As Exception
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorOtherFailure, New String() {oGame.Name, ex.Message}), False, ToolTipIcon.Error, True)
                End Try
            End If

            If bBackupCompleted Then
                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionComplete, oGame.CroppedName))
            Else
                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionFailed, oGame.CroppedName))
            End If
        Next
    End Sub

End Class
