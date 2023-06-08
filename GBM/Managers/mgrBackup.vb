﻿Imports GBM.My.Resources
Imports System.IO

Public Class mgrBackup
    Public Property CancelOperation As Boolean

    Public Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)
    Public Event UpdateBackupInfo(oGame As clsGame)
    Public Event UpdateImportInfo(sPath As String)
    Public Event SetLastAction(sMessage As String)

    Public WithEvents oMetadata As New mgrMetadata

    'Handlers
    Public Sub UpdateLogFromMetadata(sLogUpdate As String, Optional bTrayUpdate As Boolean = True, Optional objIcon As System.Windows.Forms.ToolTipIcon = ToolTipIcon.Info, Optional bTimeStamp As Boolean = True) Handles oMetadata.UpdateLog
        RaiseEvent UpdateLog(sLogUpdate, bTrayUpdate, objIcon, bTimeStamp)
    End Sub

    Public Function CheckForUtilities(ByVal strPath As String) As Boolean
        If File.Exists(strPath) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function DoManifestUpdate(ByRef oBackup As clsBackup, ByVal bMultipleBackups As Boolean, ByVal sBackupFile As String, ByVal sCheckSum As String) As Boolean

        'Keep the path relative to the manifest location
        oBackup.FileName = sBackupFile.Replace(mgrSettings.BackupFolder & Path.DirectorySeparatorChar, String.Empty)
        oBackup.CheckSum = sCheckSum

        'Save Remote Manifest
        If Not bMultipleBackups Then
            If Not mgrManifest.DoUpdateLatestManifest(oBackup, mgrSQLite.Database.Remote) Then
                mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
            End If
        Else
            mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
        End If

        'Save Local Manifest
        If mgrManifest.DoManifestCheck(oBackup.MonitorID, mgrSQLite.Database.Local) Then
            mgrManifest.DoManifestUpdateByMonitorID(oBackup, mgrSQLite.Database.Local)
        Else
            mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Local)
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
        Return oGame.FileSafeName
    End Function

    Private Sub ShowBackupSizeInfo(ByVal lAvailableTempSpace As Long, ByVal lAvailableSpace As Long, ByVal lBackupSize As Long)
        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrCommon_AvailableDiskSpace, New String() {mgrBackup_TemporaryFolder, mgrCommon.FormatDiskSpace(lAvailableTempSpace)}), False, ToolTipIcon.Info, True)
        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrCommon_AvailableDiskSpace, New String() {mgrBackup_BackupFolder, mgrCommon.FormatDiskSpace(lAvailableSpace)}), False, ToolTipIcon.Info, True)
        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupBatchSize, mgrCommon.FormatDiskSpace(lBackupSize)), False, ToolTipIcon.Info, True)
    End Sub

    Public Function CheckBackupPrereq(ByVal oGame As clsGame, ByRef lBackupSize As Long, Optional ByVal bFastMode As Boolean = False) As Boolean
        Dim sBackupFile As String = mgrSettings.BackupFolder
        Dim sSavePath As String
        Dim sOverwriteMessage As String
        Dim lFolderSize As Long
        Dim lAvailableSpace As Long
        Dim lAvailableTempSpace As Long
        Dim sDeepFolder As String
        Dim bRegistry As Boolean
        Dim sExtension As String

        'Check if this is a registry backup
        bRegistry = mgrPath.IsSupportedRegistryPath(oGame.TruePath)

        If bRegistry Then
            sExtension = ".reg"
        Else
            'Verify saved game path
            sSavePath = VerifySavePath(oGame)

            'Check if disk space check should be disabled (UNC path, Setting, forced fast mode)
            If Not mgrPath.IsPathUNC(mgrSettings.BackupFolder) And Not mgrPath.IsPathUNC(mgrSettings.TemporaryFolder) And Not mgrSettings.DisableDiskSpaceCheck Then
                'Calculate space
                lAvailableSpace = mgrCommon.GetAvailableDiskSpace(mgrSettings.BackupFolder)
                lAvailableTempSpace = mgrCommon.GetAvailableDiskSpace(mgrSettings.TemporaryFolder)

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
                lFolderSize = mgrCommon.GetFolderSize(sSavePath, oGame.IncludeArray, oGame.ExcludeArray, oGame.RecurseSubFolders)
                lBackupSize += lFolderSize

                'Show Save Folder Size
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_SavedGameFolderSize, New String() {oGame.Name, mgrCommon.FormatDiskSpace(lFolderSize)}), False, ToolTipIcon.Info, True)

                If lBackupSize >= lAvailableSpace Or lBackupSize >= lAvailableTempSpace Then
                    If Not bFastMode Then
                        If mgrCommon.ShowMessage(mgrBackup_ConfirmDiskSpace, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            CancelOperation = True
                            ShowBackupSizeInfo(lAvailableTempSpace, lAvailableSpace, lBackupSize)
                            RaiseEvent UpdateLog(mgrBackup_ErrorDiskSpace, False, ToolTipIcon.Error, True)
                            Return False
                        End If
                    Else
                        CancelOperation = True
                        ShowBackupSizeInfo(lAvailableTempSpace, lAvailableSpace, lBackupSize)
                        RaiseEvent UpdateLog(mgrBackup_ErrorDiskSpace, False, ToolTipIcon.Error, True)
                        Return False
                    End If
                End If
            ElseIf (mgrPath.IsPathUNC(mgrSettings.BackupFolder) Or mgrPath.IsPathUNC(mgrSettings.TemporaryFolder)) And Not mgrSettings.DisableDiskSpaceCheck And Not bFastMode Then
                'Show that disk space check was skipped due to UNC path                
                RaiseEvent UpdateLog(mgrBackup_ErrorPathIsUNC, False, ToolTipIcon.Info, True)
            End If

            sExtension = ".7z"
        End If

        If mgrSettings.CreateSubFolder Then sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)

        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & sExtension

        'A manifest check is only required when "Save Multiple Backups" is disabled
        If Not oGame.AppendTimeStamp And Not bFastMode Then
            If mgrRestore.CheckManifest(oGame.ID) Then
                If mgrCommon.ShowMessage(mgrBackup_ConfirmManifestConflict, oGame.Name, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    lBackupSize -= lFolderSize
                    RaiseEvent UpdateLog(mgrBackup_ErrorManifestConflict, False, ToolTipIcon.Error, True)
                    Return False
                End If
            End If
        End If

        If mgrSettings.ShowOverwriteWarning And File.Exists(sBackupFile) And Not oGame.AppendTimeStamp And Not bFastMode Then
            If oGame.AbsolutePath Then
                sOverwriteMessage = mgrCommon.FormatString(mgrBackup_ConfirmOverwrite, Path.GetFileName(sBackupFile))
            Else
                sOverwriteMessage = mgrCommon.FormatString(mgrBackup_ConfirmOverwriteRelative, Path.GetFileName(sBackupFile))
            End If

            If mgrCommon.ShowMessage(sOverwriteMessage, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorOverwriteAbort, oGame.Name), False, ToolTipIcon.Error, True)
                lBackupSize -= lFolderSize
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub DeleteOldBackups(ByVal sGameID As String, ByVal iBackupLimit As Integer)
        Dim oGameBackups As List(Of clsBackup) = mgrManifest.DoManifestGetByMonitorID(sGameID, mgrSQLite.Database.Remote, True)
        Dim oGameBackup As clsBackup
        Dim sOldBackup As String
        Dim iBackupCount As Integer = oGameBackups.Count
        Dim iDelCount As Integer

        'If we've hit or exceeded the maximum backup limit
        If oGameBackups.Count >= iBackupLimit Then
            'How many do we need to delete
            iDelCount = (oGameBackups.Count - iBackupLimit)

            'Delete the oldest backup(s) (Manifest entry and backup file)
            For i = 1 To iDelCount
                oGameBackup = oGameBackups(oGameBackups.Count - i)
                sOldBackup = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oGameBackup.FileName

                mgrManifest.DoManifestDeleteByManifestID(oGameBackup, mgrSQLite.Database.Remote)
                mgrCommon.DeleteFile(sOldBackup)
                mgrCommon.DeleteEmptyDirectory(mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oGameBackup.FileSafeName)

                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupLimitExceeded, Path.GetFileName(sOldBackup)), False, ToolTipIcon.Info, True)
            Next
        End If
    End Sub

    Private Sub DeleteOldDiffBackups(ByVal sGameID As String, ByVal iBackupLimit As Integer)
        Dim oBackupSets As List(Of clsBackup) = mgrManifest.DoManfiestGetDifferentialParents(sGameID, mgrSQLite.Database.Remote)
        Dim oCurrentParent As clsBackup
        Dim sCurrentFileName As String
        Dim oBackupChildren As List(Of clsBackup)
        Dim iDelCount As Integer

        'Have we exceeded the maximum set of backups
        If oBackupSets.Count >= iBackupLimit Then
            'How many sets of backups do we need to delete
            iDelCount = (oBackupSets.Count - iBackupLimit)

            'Delete the oldest sets of backups
            For i = 1 To iDelCount
                oCurrentParent = oBackupSets(oBackupSets.Count - i)
                oBackupChildren = mgrManifest.DoManfiestGetDifferentialChildren(oCurrentParent, mgrSQLite.Database.Remote)

                'Delete all associated diffs
                For Each oChild As clsBackup In oBackupChildren
                    sCurrentFileName = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oChild.FileName
                    mgrManifest.DoManifestDeleteByManifestID(oChild, mgrSQLite.Database.Remote)
                    mgrCommon.DeleteFile(sCurrentFileName)
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupLimitExceeded, Path.GetFileName(sCurrentFileName)), False, ToolTipIcon.Info, True)
                Next

                'Delete full
                sCurrentFileName = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oCurrentParent.FileName
                mgrManifest.DoManifestDeleteByManifestID(oCurrentParent, mgrSQLite.Database.Remote)
                mgrCommon.DeleteFile(sCurrentFileName)
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupLimitExceeded, Path.GetFileName(sCurrentFileName)), False, ToolTipIcon.Info, True)

                'Delete directory if empty
                mgrCommon.DeleteEmptyDirectory(mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oCurrentParent.FileSafeName)
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

    Public Sub ImportBackupFiles(ByVal sFileList As String(), Optional ByVal oExistingGame As clsGame = Nothing, Optional ByVal bDiffPass As Boolean = False)
        Dim oGame As New clsGame
        Dim hshGame As Hashtable
        Dim bForcedMode As Boolean
        Dim sFileToImportShort As String
        Dim bOverwriteCurrent As Boolean
        Dim bContinue As Boolean
        Dim bImportComplete As Boolean
        Dim bUpdateManifest As Boolean
        Dim sBackupFile As String
        Dim oBackup As clsBackup
        Dim oBackupMetadata As BackupMetadata = Nothing
        Dim iFilesImported As Integer = 0
        Dim iGamesAdded As Integer = 0
        Dim sDiffLabel As String
        Dim oDiffParent As clsBackup
        Dim oDiffQueue As New List(Of String)

        'When an specific game object is passed in, we enter a less strict "forced" import mode.
        If oExistingGame IsNot Nothing Then bForcedMode = True

        For Each sFileToImport As String In sFileList
            RaiseEvent UpdateImportInfo(sFileToImport)
            sFileToImportShort = Path.GetFileName(sFileToImport)
            bContinue = True
            bImportComplete = False
            bUpdateManifest = False
            bOverwriteCurrent = False
            sDiffLabel = String.Empty
            oBackup = New clsBackup

            If File.Exists(sFileToImport) Then
                sBackupFile = mgrSettings.BackupFolder

                If oMetadata.CheckForMetadata(sFileToImport) Then
                    If oMetadata.ExtractMetadataFromArchive(sFileToImport) Then
                        oBackupMetadata = New BackupMetadata
                        If oMetadata.ImportandDeserialize(oBackupMetadata) Then
                            'Version warning
                            If oBackupMetadata.AppVer <= 128 Then
                                RaiseEvent UpdateLog(mgrBackup_WarningOldMetadata, False, ToolTipIcon.Warning, True)
                            End If

                            oGame = oBackupMetadata.Game.ConvertClass
                            oBackup = oBackupMetadata.CreateBackupInfo(mgrBackup_ImportedFile, File.GetLastWriteTime(sFileToImport))

                            If bForcedMode Then
                                If oExistingGame.CoreEquals(oGame) Then
                                    'When the metadata exactly matches the game selected, we can exit forced mode for the rest of the function.
                                    bForcedMode = False
                                Else
                                    If mgrCommon.ShowMessage(mgrBackup_WarningMetadataMismatch, sFileToImportShort, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                                        bContinue = False
                                    End If
                                End If
                            Else
                                If oGame.OS = mgrCommon.GetCurrentOS Or mgrCommon.GetCurrentOS = clsGame.eOS.Linux Then
                                    If mgrMonitorList.DoDuplicateListCheck(oGame.ID) Then
                                        hshGame = mgrMonitorList.DoListGetbyMonitorID(oGame.ID)
                                        If hshGame.Count = 1 Then
                                            oExistingGame = DirectCast(hshGame(0), clsGame)
                                            If Not oExistingGame.CoreEquals(oGame) Then
                                                oGame.Hours = oExistingGame.Hours
                                                oGame.CleanFolder = oExistingGame.CleanFolder
                                                mgrMonitorList.DoListUpdate(oGame)
                                                mgrMonitorList.DoListUpdate(oGame,, mgrSQLite.Database.Remote)
                                                iGamesAdded += 1
                                            End If
                                        End If
                                    Else
                                        Dim oTagsToAdd As New Hashtable
                                        oTagsToAdd.Add(0, oGame)
                                        mgrMonitorList.DoListAdd(oGame)
                                        mgrMonitorList.DoListAdd(oGame, mgrSQLite.Database.Remote)
                                        mgrTags.DoTagAddImport(oTagsToAdd)
                                        mgrTags.DoTagAddImport(oTagsToAdd, mgrSQLite.Database.Remote)
                                        iGamesAdded += 1
                                    End If
                                Else
                                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportOSMismatch, New String() {sFileToImportShort, oGame.OS.ToString, mgrCommon.GetCurrentOS.ToString}), False, ToolTipIcon.Warning, True)
                                    bContinue = False
                                End If
                            End If
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorReadingMetadata, sFileToImportShort), False, ToolTipIcon.Warning, True)
                            bContinue = False
                        End If
                    End If
                Else
                    If bForcedMode Then
                        If mgrCommon.ShowMessage(mgrBackup_WarningNoMetadata, sFileToImportShort, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            bContinue = False
                        End If
                    Else
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorNoMetadata, sFileToImportShort), False, ToolTipIcon.Warning, True)
                        bContinue = False
                    End If
                End If

                'If we are still in forced mode at this point we need to initialize required objects.
                If bContinue And bForcedMode Then
                    oGame = oExistingGame
                    oBackup = New clsBackup
                    oBackup.MonitorID = oGame.ID
                    oBackup.DateUpdated = File.GetLastWriteTime(sFileToImport)
                    oBackup.UpdatedBy = mgrBackup_ImportedFile
                End If

                If bContinue Then
                    'Enter overwite mode if "Save Multiple Backups" and "Differential Backup" are disabled.
                    If Not oGame.AppendTimeStamp And Not oGame.Differential Then bOverwriteCurrent = True

                    If mgrSettings.CreateSubFolder Then
                        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)
                        bContinue = HandleSubFolder(oGame, sBackupFile)
                    End If
                End If

                If bContinue Then
                    'Handle differential backups
                    If oGame.Differential Then
                        If oBackupMetadata IsNot Nothing Then
                            If oBackupMetadata.Backup.IsDifferentialParent Then
                                sDiffLabel = "-" & mgrBackup_Label_DiffFull
                                oBackup.IsDifferentialParent = True
                            Else
                                sDiffLabel = "-" & mgrBackup_Label_Diff
                                oDiffParent = mgrManifest.DoManifestGetByManifestID(oBackupMetadata.Backup.DifferentialParent, mgrSQLite.Database.Remote)
                                If oDiffParent IsNot Nothing Then
                                    oBackup.DifferentialParent = oDiffParent.ManifestID
                                Else
                                    If bDiffPass Or sFileList.Length = 1 Then
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportNoDifferentialParent, sFileToImportShort), False, ToolTipIcon.Error, True)
                                    Else
                                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_WarningImportNoDifferentialParent, sFileToImportShort), False, ToolTipIcon.Error, True)
                                        oDiffQueue.Add(sFileToImport)
                                    End If
                                    bContinue = False
                                End If
                            End If
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportDifferentialWithNoMetadata, New String() {oGame.Name, sFileToImportShort}), False, ToolTipIcon.Error, True)
                            bContinue = False
                        End If
                    End If
                End If

                If bContinue Then
                    If bOverwriteCurrent Then
                        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & ".7z"
                    Else
                        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & BuildFileTimeStamp(oBackup.DateUpdated) & sDiffLabel & ".7z"
                    End If

                    oBackup.FileName = sBackupFile.Replace(mgrSettings.BackupFolder & Path.DirectorySeparatorChar, String.Empty)

                    If sFileToImport = sBackupFile Then
                        bUpdateManifest = True
                    Else
                        If mgrCommon.CopyFile(sFileToImport, sBackupFile, True) Then
                            bUpdateManifest = True
                        Else
                            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportBackupCopy, sFileToImportShort), False, ToolTipIcon.Error, True)
                        End If
                    End If

                    If bUpdateManifest Then
                        oBackup.CheckSum = mgrHash.Generate_SHA256_Hash(sBackupFile)
                        If bOverwriteCurrent Then
                            If Not mgrManifest.DoUpdateLatestManifest(oBackup, mgrSQLite.Database.Remote) Then
                                mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
                            End If
                        Else
                            If Not mgrManifest.DoManifestDuplicateCheck(oBackup, mgrSQLite.Database.Remote) Then
                                mgrManifest.DoManifestAdd(oBackup, mgrSQLite.Database.Remote)
                            End If
                        End If
                        iFilesImported += 1
                        bImportComplete = True
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ImportSuccess, New String() {sFileToImportShort, oGame.Name}), False, ToolTipIcon.Info, True)
                    End If
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorImportCancel, sFileToImportShort), False, ToolTipIcon.Error, True)
                End If

                If bImportComplete Then
                    RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionImportComplete, oGame.CroppedName))
                Else
                    RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionImportFailed, oGame.CroppedName))
                End If
            End If

            If CancelOperation Then
                Exit For
            End If
        Next

        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupsImported, iFilesImported.ToString), False, ToolTipIcon.Info, True)
        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_GamesAddedDuringImport, iGamesAdded.ToString), False, ToolTipIcon.Info, True)

        If Not bDiffPass And oDiffQueue.Count > 0 Then
            If bForcedMode Then
                ImportBackupFiles(oDiffQueue.ToArray, oExistingGame, True)
            Else
                ImportBackupFiles(oDiffQueue.ToArray, , True)
            End If
        End If
    End Sub

    Private Function RunRegistryBackup(ByVal oGame As clsGame, ByVal sBackupFile As String, Optional ByVal bAdmin As Boolean = False) As Boolean
        Dim prsReg As New Process
        Dim sBinaryPath As String
        Dim sArguments As String
        Dim oWineData As clsWineData
        Dim sWineReg As String
        Dim bPathVerified As Boolean = False
        Dim bBackupCompleted As Boolean = False

        sArguments = "export """ & oGame.TruePath & """ """ & sBackupFile & """ /y"

        If mgrCommon.IsUnix Then
            oWineData = mgrWineData.DoWineDataGetbyID(oGame.ID)
            prsReg.StartInfo.EnvironmentVariables.Add("WINEPREFIX", oWineData.Prefix)
            sBinaryPath = oWineData.BinaryPath & Path.DirectorySeparatorChar & "wine"
            sWineReg = oWineData.Prefix & Path.DirectorySeparatorChar & "drive_c/windows/system32/reg.exe"
            sArguments = """" & sWineReg & """ " & sArguments
            If File.Exists(sBinaryPath) Then
                If File.Exists(sWineReg) Then
                    bPathVerified = True
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorRegNotFound, sWineReg), False, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorWineNotFound, sBinaryPath), False, ToolTipIcon.Error, True)
            End If
        Else
            sBinaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & Path.DirectorySeparatorChar & "system32\reg.exe"
            If File.Exists(sBinaryPath) Then
                bPathVerified = True
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorRegNotFound, sBinaryPath), False, ToolTipIcon.Error, True)
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
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupInProgress, oGame.TruePath), False, ToolTipIcon.Info, True)
                While Not prsReg.StandardOutput.EndOfStream
                    If CancelOperation Then
                        prsReg.Kill()
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorFullAbort, oGame.Name), True, ToolTipIcon.Error, True)
                        Exit While
                    End If
                    RaiseEvent UpdateLog(prsReg.StandardOutput.ReadLine, False, ToolTipIcon.Info, False)
                End While
                prsReg.WaitForExit()
                Select Case prsReg.ExitCode
                    Case 0
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_BackupComplete, New String() {oGame.Name, mgrCommon.FormatDiskSpace(mgrCommon.GetFileSize(sBackupFile))}), False, ToolTipIcon.Info, True)
                        bBackupCompleted = True
                    Case Else
                        RaiseEvent UpdateLog(mgrBackup_ErrorRegBackupFailed, False, ToolTipIcon.Info, True)
                End Select
                prsReg.Dispose()
            Catch exWin32 As System.ComponentModel.Win32Exception
                'If the launch fails due to required elevation, try it again and request elevation.
                If exWin32.ErrorCode = 740 Then
                    RunRegistryBackup(oGame, sBackupFile, True)
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Backup, exWin32.Message}), False, ToolTipIcon.Error, True)
                End If
            Catch exAll As Exception
                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Backup, exAll.Message}), False, ToolTipIcon.Error, True)
            End Try
        End If

        Return bBackupCompleted
    End Function

    Private Function Run7zBackup(ByVal oGame As clsGame, ByVal sBackupFile As String, ByVal bRunDifferential As Boolean, ByVal sDiffParentFile As String) As Boolean
        Dim prs7z As New Process
        Dim sSavePath As String
        Dim sArguments As String
        Dim bBackupCompleted As Boolean = False

        sSavePath = VerifySavePath(oGame)

        If oGame.FolderSave = True Then
            BuildFileList("*", mgrSettings.IncludeFileLocation)
        Else
            BuildFileList(oGame.FileType, mgrSettings.IncludeFileLocation)
        End If

        BuildFileList(oGame.ExcludeList, mgrSettings.ExcludeFileLocation)

        If bRunDifferential Then
            sArguments = "u" & " """ & sDiffParentFile & """" & mgrSettings.Prepared7zArguments & "-t7z -mx" & mgrSettings.CompressionLevel & " -i@""" & mgrSettings.IncludeFileLocation & """ -x@""" & mgrSettings.ExcludeFileLocation & """ -u- -up0q3r2x2y2z0w2!""" & sBackupFile & """"
        Else
            sArguments = "a" & mgrSettings.Prepared7zArguments & "-t7z -mx" & mgrSettings.CompressionLevel & " -i@""" & mgrSettings.IncludeFileLocation & """ -x@""" & mgrSettings.ExcludeFileLocation & """ """ & sBackupFile & """"
        End If

        If oGame.RecurseSubFolders Then sArguments &= " -r"

        Try
            If Directory.Exists(sSavePath) Then
                If mgrSettings.Is7zUtilityValid Then
                    'Need to delete any prior archive if it exists, the 7za utility does not support overwriting or deleting existing archives.
                    'If we let 7za update existing archives it will lead to excessive bloat with games that routinely add and remove files with many different file names.
                    If File.Exists(sBackupFile) Then
                        File.Delete(sBackupFile)
                    End If

                    prs7z.StartInfo.Arguments = sArguments
                    prs7z.StartInfo.FileName = mgrSettings.Utility7zLocation
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
                                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Backup), True, ToolTipIcon.Warning, True)
                                bBackupCompleted = True
                            Case 2
                                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FatalError, App_OperationType_Backup), True, ToolTipIcon.Error, True)
                            Case 7
                                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_CommandFailure, App_OperationType_Backup), True, ToolTipIcon.Error, True)
                        End Select
                    End If
                    prs7z.Dispose()
                Else
                    RaiseEvent UpdateLog(App_Invalid7zDetected, True, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorNoSavePath, oGame.Name), True, ToolTipIcon.Error, True)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Backup, ex.Message}), False, ToolTipIcon.Error, True)
        End Try

        Return bBackupCompleted
    End Function

    Private Function MoveBackupToFinalLocation(ByRef sSourceFile As String, ByVal oGame As clsGame) As Boolean
        Dim sBackupFile As String = mgrSettings.BackupFolder
        Dim sFileName As String = Path.GetFileName(sSourceFile)
        Dim bDoMove As Boolean = True

        If mgrSettings.CreateSubFolder Then
            sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame)
            bDoMove = HandleSubFolder(oGame, sBackupFile)
        End If

        sBackupFile = sBackupFile & Path.DirectorySeparatorChar & sFileName

        If bDoMove Then
            If mgrCommon.MoveFile(sSourceFile, sBackupFile, True) Then
                sSourceFile = sBackupFile
                Return True
            End If
        End If

        Return False
    End Function

    Public Sub DoBackup(ByVal oBackupList As List(Of clsGame), ByVal bPlaySound As Boolean)
        Dim oGame As clsGame
        Dim oBackup As clsBackup
        Dim sBackupFile As String
        Dim sBackupExt As String
        Dim dTimeStamp As DateTime
        Dim sTimeStamp As String
        Dim sHash As String
        Dim bRunDifferential As Boolean
        Dim oDiffParent As clsBackup
        Dim oDiffChildren As List(Of clsBackup)
        Dim sDiffParentID As String
        Dim sDiffParentFile As String
        Dim sDiffLabel As String
        Dim bMetadataGenerated As Boolean
        Dim bBackupCompleted As Boolean

        For Each oGame In oBackupList
            'Break out when a cancel signal is received
            If CancelOperation Then
                RaiseEvent UpdateLog(mgrBackup_FullAbort, False, ToolTipIcon.Warning, True)
                Exit For
            End If

            'Init
            oBackup = New clsBackup
            sBackupFile = mgrSettings.TemporaryFolder
            dTimeStamp = Date.Now
            sTimeStamp = BuildFileTimeStamp(dTimeStamp)
            sHash = String.Empty
            sDiffParentID = String.Empty
            sDiffLabel = String.Empty
            bBackupCompleted = False
            bMetadataGenerated = False
            CancelOperation = False
            RaiseEvent UpdateBackupInfo(oGame)

            If mgrPath.IsSupportedRegistryPath(oGame.TruePath) Then
                sBackupExt = ".reg"
            Else
                sBackupExt = ".7z"
            End If

            bRunDifferential = False
            sDiffParentFile = String.Empty

            If oGame.Differential Then
                If mgrManifest.DoManifestParentCheck(oGame.ID, mgrSQLite.Database.Remote) Then
                    oDiffParent = mgrManifest.DoManfiestGetLatestDifferentialParent(oGame.ID, mgrSQLite.Database.Remote)
                    oDiffChildren = mgrManifest.DoManfiestGetDifferentialChildren(oDiffParent, mgrSQLite.Database.Remote)
                    sDiffParentFile = mgrSettings.BackupFolder & Path.DirectorySeparatorChar & oDiffParent.FileName
                    If File.Exists(sDiffParentFile) And (oGame.DiffInterval = 0 Or oDiffChildren.Count <= oGame.DiffInterval - 1) Then
                        bRunDifferential = True
                        sDiffParentID = oDiffParent.ManifestID
                    Else
                        bRunDifferential = False
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorDiffParentNotFound, oGame.CroppedName), False, ToolTipIcon.Warning, True)
                    End If
                End If
                If bRunDifferential Then
                    sDiffLabel = "-" & mgrBackup_Label_Diff
                Else
                    sDiffLabel = "-" & mgrBackup_Label_DiffFull
                End If
            End If

            If oGame.AppendTimeStamp Or oGame.Differential Then
                sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & sTimeStamp & sDiffLabel & sBackupExt
            Else
                sBackupFile = sBackupFile & Path.DirectorySeparatorChar & GetFileName(oGame) & sBackupExt
            End If

            'Build manifest
            oBackup.MonitorID = oGame.ID
            oBackup.DateUpdated = dTimeStamp
            oBackup.UpdatedBy = My.Computer.Name
            oBackup.IsDifferentialParent = oGame.Differential And Not bRunDifferential
            oBackup.DifferentialParent = sDiffParentID

            'Choose Backup Type
            If mgrPath.IsSupportedRegistryPath(oGame.TruePath) Then
                bBackupCompleted = RunRegistryBackup(oGame, sBackupFile)
                If bBackupCompleted Then
                    If Not MoveBackupToFinalLocation(sBackupFile, oGame) Then
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorMovingBackupFile, oGame.Name), True, ToolTipIcon.Error, True)
                        bBackupCompleted = False
                    End If
                End If
            Else
                bBackupCompleted = Run7zBackup(oGame, sBackupFile, bRunDifferential, sDiffParentFile)
                If bBackupCompleted Then
                    bMetadataGenerated = oMetadata.SerializeAndExport(mgrSettings.MetadataLocation, oGame.ID, oBackup)
                    If bMetadataGenerated Then
                        If oMetadata.AddMetadataToArchive(sBackupFile, App_MetadataFilename) Then
                            If Not MoveBackupToFinalLocation(sBackupFile, oGame) Then
                                bBackupCompleted = False
                                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorMovingBackupFile, oGame.Name), True, ToolTipIcon.Error, True)
                            End If
                        Else
                            bBackupCompleted = False
                        End If
                    Else
                        bBackupCompleted = False
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorMetadataFailure, oGame.Name), True, ToolTipIcon.Error, True)
                    End If
                End If
            End If

            If bBackupCompleted Then
                'Generate checksum for new backup
                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_GenerateHash, oGame.Name), False, ToolTipIcon.Info, True)
                sHash = mgrHash.Generate_SHA256_Hash(sBackupFile)

                'Write Main Manifest
                If Not DoManifestUpdate(oBackup, oGame.AppendTimeStamp Or oGame.Differential, sBackupFile, sHash) Then
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_ErrorManifestFailure, oGame.Name), True, ToolTipIcon.Error, True)
                End If

                'Handle Single Notification
                If oBackupList.Count = 1 And mgrSettings.BackupNotification Then
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_NotificationSingle, oGame.CroppedName), True, ToolTipIcon.Info, True)
                End If

                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionComplete, oGame.CroppedName))

                'Handle old backups if required
                If oGame.AppendTimeStamp And Not oGame.Differential And oGame.BackupLimit > 0 Then
                    DeleteOldBackups(oGame.ID, oGame.BackupLimit)
                ElseIf oGame.AppendTimeStamp And oGame.Differential And oGame.BackupLimit > 0 Then
                    DeleteOldDiffBackups(oGame.ID, oGame.BackupLimit)
                ElseIf oGame.Differential And Not oGame.AppendTimeStamp Then
                    DeleteOldDiffBackups(oGame.ID, 1)
                End If

                'Play success audio if this operation was triggered by a hot key
                If bPlaySound Then mgrCommon.PlaySound(mgrCommon.eSounds.Success)
            Else
                'Delete the temporary backup file on failures
                mgrCommon.DeleteFile(sBackupFile, False)
                RaiseEvent SetLastAction(mgrCommon.FormatString(mgrBackup_ActionFailed, oGame.CroppedName))

                'Play failure audio if this operation was triggered by a hot key
                If bPlaySound Then mgrCommon.PlaySound(mgrCommon.eSounds.Failure)
            End If

            'Remove from the failsafe queue
            mgrBackupQueue.DoBackupQueueDeleteByID(oGame.ID)
        Next

        'Handle Multi Notification
        If oBackupList.Count > 1 And mgrSettings.BackupNotification Then
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrBackup_NotificationMulti, oBackupList.Count.ToString), True, ToolTipIcon.Info, True)
        End If
    End Sub

End Class
