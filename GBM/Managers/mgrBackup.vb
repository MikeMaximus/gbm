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
        oItem.Name = oGameInfo.Name
        'Keep the path relative to the manifest location
        oItem.FileName = sBackupFile.Replace(Path.GetDirectoryName(mgrPath.RemoteDatabaseLocation) & "\", "")
        oItem.RestorePath = oGameInfo.TruePath
        oItem.AbsolutePath = oGameInfo.AbsolutePath
        oItem.DateUpdated = dTimeStamp
        oItem.UpdatedBy = My.Computer.Name
        oItem.CheckSum = sCheckSum

        'Save Remote Manifest
        If mgrManifest.DoManifestCheck(oItem.Name, mgrSQLite.Database.Remote) Then
            mgrManifest.DoManifestUpdate(oItem, mgrSQLite.Database.Remote)
        Else
            mgrManifest.DoManifestAdd(oItem, mgrSQLite.Database.Remote)
        End If

        'Save Local Manifest
        If mgrManifest.DoManifestCheck(oItem.Name, mgrSQLite.Database.Local) Then
            mgrManifest.DoManifestUpdate(oItem, mgrSQLite.Database.Local)
        Else
            mgrManifest.DoManifestAdd(oItem, mgrSQLite.Database.Local)
        End If

        Return True
    End Function

    Private Sub BuildFileList(ByVal sBackupPath As String, ByVal sList As String, ByVal sPath As String)
        Dim oStream As StreamWriter

        Try
            If File.Exists(sPath) Then File.Delete(sPath)
            oStream = New StreamWriter(sPath)
            Using oStream
                If sList <> String.Empty Then
                    For Each sTypeItem As String In sList.Split(":")
                        oStream.WriteLine("""" & sBackupPath & "\" & sTypeItem & """")
                    Next
                End If
                oStream.Flush()
            End Using
        Catch ex As Exception
            RaiseEvent UpdateLog("An error occured creating a file list: " & ex.Message, False, ToolTipIcon.Error, True)
        End Try
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

        For Each oGame In oBackupList
            'Init
            prs7z = New Process
            sBackupFile = oSettings.BackupFolder
            sSavePath = String.Empty
            dTimeStamp = Date.Now
            sTimeStamp = " " & dTimeStamp.Month & "-" & dTimeStamp.Day & "-" & dTimeStamp.Year & "-" & dTimeStamp.Hour & "-" & dTimeStamp.Minute & "-" & dTimeStamp.Second
            sHash = String.Empty
            bDoBackup = True
            bBackupCompleted = False
            CancelOperation = False
            RaiseEvent UpdateBackupInfo(oGame)

            If mgrRestore.CheckManifest(oGame.Name) Then
                If mgrCommon.ShowMessage("The manifest shows the backup folder contains a backup for " & oGame.Name & " that has not been restored on this computer." & vbCrLf & vbCrLf & "Do you want to overwrite this file anyway?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    RaiseEvent UpdateLog("Backup aborted by user due to manifest conflict.", False, ToolTipIcon.Error, True)
                    bDoBackup = False
                End If
            End If

            If oSettings.CreateSubFolder Then
                sBackupFile = sBackupFile & "\" & oGame.Name
                Try
                    If Not Directory.Exists(sBackupFile) Then
                        Directory.CreateDirectory(sBackupFile)
                    End If
                Catch ex As Exception
                    RaiseEvent UpdateLog("Backup Aborted.  A failure occured while creating backup sub-folder for " & oGame.Name & vbCrLf & ex.Message, False, ToolTipIcon.Error, True)
                    bDoBackup = False
                End Try
            End If

            If oGame.AppendTimeStamp Then
                sBackupFile = sBackupFile & "\" & oGame.Name & sTimeStamp & ".7z"
            Else
                sBackupFile = sBackupFile & "\" & oGame.Name & ".7z"
            End If

            If oSettings.ShowOverwriteWarning And File.Exists(sBackupFile) Then
                If mgrCommon.ShowMessage("A file with the same name already exists in the backup folder." & vbCrLf & vbCrLf & "Do you want to overwrite this file?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    RaiseEvent UpdateLog(oGame.Name & " backup aborted by user due to overwrite.", False, ToolTipIcon.Error, True)
                    bDoBackup = False
                End If
            End If

            If bDoBackup Then
                If oGame.AbsolutePath = False Then
                    If oGame.Path <> String.Empty Then
                        sSavePath = oGame.ProcessPath & "\" & oGame.Path
                    Else
                        sSavePath = oGame.ProcessPath
                    End If
                Else
                    sSavePath = oGame.Path
                End If

                If oGame.FolderSave = True Then
                    BuildFileList(sSavePath, "*", mgrPath.IncludeFileLocation)
                Else
                    BuildFileList(sSavePath, oGame.FileType, mgrPath.IncludeFileLocation)
                End If

                BuildFileList(sSavePath, oGame.ExcludeList, mgrPath.ExcludeFileLocation)

                Try
                    'Need to delete any prior archive if it exists, the 7za utility does not support overwriting or deleting existing archives.
                    'If we let 7za update existing archives it will lead to excessive bloat with games that routinely add and remove files with many different file names.
                    If File.Exists(sBackupFile) Then
                        File.Delete(sBackupFile)
                    End If

                    If Directory.Exists(sSavePath) Then
                        prs7z.StartInfo.Arguments = "a -t7z " & "-i@""" & mgrPath.IncludeFileLocation & """ -x@""" & mgrPath.ExcludeFileLocation & """ """ & sBackupFile & """ -r"
                        prs7z.StartInfo.FileName = mgrPath.Utility7zLocation
                        prs7z.StartInfo.UseShellExecute = False
                        prs7z.StartInfo.RedirectStandardOutput = True
                        prs7z.StartInfo.CreateNoWindow = True
                        prs7z.Start()
                        RaiseEvent UpdateLog("Backup of " & sSavePath & " in progress...", False, ToolTipIcon.Info, True)
                        While Not prs7z.StandardOutput.EndOfStream
                            If CancelOperation Then
                                prs7z.Kill()
                                RaiseEvent UpdateLog("Backup Aborted.  The backup file for " & oGame.Name & " will be unusable.", True, ToolTipIcon.Error, True)
                                Exit While
                            End If
                            RaiseEvent UpdateLog(prs7z.StandardOutput.ReadLine, False, ToolTipIcon.Info, False)
                        End While
                        prs7z.WaitForExit()
                        If Not CancelOperation Then
                            If prs7z.ExitCode = 0 Then
                                RaiseEvent UpdateLog(oGame.Name & " backup completed.", False, ToolTipIcon.Info, True)
                                bBackupCompleted = True
                            Else
                                RaiseEvent UpdateLog(oGame.Name & " backup finished with warnings or errors.", True, ToolTipIcon.Warning, True)
                                bBackupCompleted = False
                            End If
                        End If
                        prs7z.Dispose()
                    Else
                        RaiseEvent UpdateLog("Backup Aborted.  The saved game path for " & oGame.Name & " does not exist.", True, ToolTipIcon.Error, True)
                        bBackupCompleted = False
                    End If

                    'Write Main Manifest
                    If bBackupCompleted Then
                        If oSettings.CheckSum Then
                            RaiseEvent UpdateLog("Generating SHA-256 hash for " & oGame.Name & " backup file.", False, ToolTipIcon.Info, True)
                            sHash = mgrHash.Generate_SHA256_Hash(sBackupFile)                            
                        End If

                        If Not DoManifestUpdate(oGame, sBackupFile, dTimeStamp, sHash) Then
                            RaiseEvent UpdateLog("The manifest update for " & oGame.Name & " failed.", True, ToolTipIcon.Error, True)
                        End If

                        'Write the process path if we have it
                        If oGame.AbsolutePath = False Then
                            mgrMonitorList.DoListUpdate(oGame)
                        End If
                    End If
                Catch ex As Exception
                    RaiseEvent UpdateLog("An unexpected error occured during the backup of " & oGame.Name & vbCrLf & ex.Message, False, ToolTipIcon.Error, True)
                End Try
            End If

            If bBackupCompleted Then
                RaiseEvent SetLastAction(oGame.CroppedName & " backup completed")
            Else
                RaiseEvent SetLastAction(oGame.CroppedName & " backup failed")
            End If
        Next
    End Sub

End Class
