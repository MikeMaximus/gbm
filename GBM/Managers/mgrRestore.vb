Imports GBM.My.Resources
Imports System.IO

Public Class mgrRestore

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
    Public Event UpdateRestoreInfo(oRestoreInfo As clsBackup)
    Public Event SetLastAction(sMessage As String)

    Private Shared Function CheckForPathOverride(ByRef oCheckBackup As clsBackup, ByVal oCheckGame As clsGame) As Boolean
        Dim oResult As MsgBoxResult

        If oCheckBackup.RestorePath <> oCheckGame.Path Then
            oResult = mgrCommon.ShowMessage(mgrRestore_ConfirmPathMismatch, oCheckBackup.CroppedName, MsgBoxStyle.YesNoCancel)
            If oResult = MsgBoxResult.Yes Then
                If Path.IsPathRooted(oCheckGame.Path) Then
                    oCheckBackup.AbsolutePath = True
                    oCheckBackup.RestorePath = oCheckGame.Path
                Else
                    oCheckBackup.RestorePath = oCheckGame.Path
                End If
            ElseIf oResult = MsgBoxResult.Cancel Then
                Return False
            End If
        End If

        Return True
    End Function

    Public Shared Function CheckPath(ByRef oRestoreInfo As clsBackup, ByVal oGame As clsGame, ByRef bTriggerReload As Boolean) As Boolean
        Dim sProcess As String
        Dim sRestorePath As String
        Dim bNoAuto As Boolean

        'Before we do anything check if we need to override the current path
        If Not CheckForPathOverride(oRestoreInfo, oGame) Then
            Return False
        End If

        If Not oRestoreInfo.AbsolutePath Then
            If oGame.ProcessPath <> String.Empty Then
                oRestoreInfo.RelativeRestorePath = oGame.ProcessPath & Path.DirectorySeparatorChar & oRestoreInfo.RestorePath
            Else
                sProcess = oGame.TrueProcess
                If mgrCommon.IsProcessNotSearchable(oGame) Then bNoAuto = True
                sRestorePath = mgrPath.ProcessPathSearch(oRestoreInfo.Name, sProcess, mgrCommon.FormatString(mgrRestore_RelativeNeedPath, oRestoreInfo.Name), bNoAuto)

                If sRestorePath <> String.Empty Then
                    'Update the process path in game object, save it, and make sure a monitor list reload is triggered
                    oGame.ProcessPath = sRestorePath
                    mgrMonitorList.DoListUpdate(oGame)
                    bTriggerReload = True

                    'Set path for restore
                    oRestoreInfo.RelativeRestorePath = sRestorePath & Path.DirectorySeparatorChar & oRestoreInfo.RestorePath
                Else
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Public Shared Function CheckManifest(ByVal sAppName As String) As Boolean
        Dim slLocalManifest As SortedList
        Dim slRemoteManifest As SortedList
        Dim oLocalItem As New clsBackup
        Dim oRemoteItem As New clsBackup
        Dim bLocal As Boolean = False
        Dim bRemote As Boolean = False

        slLocalManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Local)
        slRemoteManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)

        If slLocalManifest.Contains(sAppName) Then
            oLocalItem = DirectCast(slLocalManifest(sAppName), clsBackup)
            bLocal = True
        End If

        If slRemoteManifest.Contains(sAppName) Then
            oRemoteItem = DirectCast(slRemoteManifest(sAppName), clsBackup)
            bRemote = True
        End If

        If bLocal And bRemote Then
            'Compare
            If oRemoteItem.DateUpdated > oLocalItem.DateUpdated Then
                oRemoteItem.LastDateUpdated = oLocalItem.DateUpdated
                oRemoteItem.LastUpdatedBy = oLocalItem.UpdatedBy
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

        slLocalManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Local)
        slRemoteManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)

        For Each oItem As clsBackup In slRemoteManifest.Values
            If slLocalManifest.Contains(oItem.Name) Then
                oLocalItem = DirectCast(slLocalManifest(oItem.Name), clsBackup)

                If oItem.DateUpdated > oLocalItem.DateUpdated Then
                    oLocalItem.FileName = oItem.FileName
                    oLocalItem.LastDateUpdated = oItem.DateUpdated
                    oLocalItem.LastUpdatedBy = oItem.UpdatedBy
                    slRestoreItems.Add(oLocalItem.Name, oLocalItem)
                End If
            Else
                oLocalItem = oItem
                oLocalItem.LastDateUpdated = oItem.DateUpdated
                oLocalItem.LastUpdatedBy = oItem.UpdatedBy
                oLocalItem.DateUpdated = Nothing
                oLocalItem.UpdatedBy = Nothing
                slRestoreItems.Add(oLocalItem.Name, oLocalItem)
            End If
        Next

        Return slRestoreItems
    End Function

    Public Shared Function SyncLocalManifest() As SortedList
        Dim slLocalManifest As SortedList
        Dim slRemoteManifest As SortedList
        Dim slRemovedItems As New SortedList

        slLocalManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Local)
        slRemoteManifest = mgrManifest.ReadManifest(mgrSQLite.Database.Remote)

        For Each oItem As clsBackup In slLocalManifest.Values
            If Not slRemoteManifest.Contains(oItem.Name) Then
                slRemovedItems.Add(oItem.Name, oItem)
                mgrManifest.DoManifestDelete(oItem, mgrSQLite.Database.Local)
            End If
        Next

        Return slRemovedItems
    End Function

    Public Sub DoRestore(ByVal oRestoreList As List(Of clsBackup))
        Dim prs7z As Process
        Dim sBackupFile As String
        Dim sExtractPath As String
        Dim bDoRestore As Boolean
        Dim bRestoreCompleted As Boolean
        Dim sHash As String

        For Each oBackupInfo In oRestoreList
            'Init
            prs7z = New Process
            sBackupFile = oSettings.BackupFolder & Path.DirectorySeparatorChar & oBackupInfo.FileName
            sExtractPath = String.Empty
            bDoRestore = True
            bRestoreCompleted = False
            CancelOperation = False
            RaiseEvent UpdateRestoreInfo(oBackupInfo)

            If oBackupInfo.AbsolutePath Then
                sExtractPath = oBackupInfo.RestorePath
            Else
                sExtractPath = oBackupInfo.RelativeRestorePath
            End If

            'Check if restore location exists, prompt to create if it doesn't.
            If Not Directory.Exists(sExtractPath) Then
                If mgrCommon.ShowMessage(mgrRestore_ConfirmCreatePath, sExtractPath, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Try
                        Directory.CreateDirectory(sExtractPath)
                    Catch ex As Exception
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorCreatePath, ex.Message), False, ToolTipIcon.Error, True)
                        bDoRestore = False
                    End Try
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorNoPath, sExtractPath), False, ToolTipIcon.Error, True)
                    bDoRestore = False
                End If
            End If

            'Check file integrity
            If oSettings.CheckSum Then
                If oBackupInfo.CheckSum <> String.Empty Then
                    sHash = mgrHash.Generate_SHA256_Hash(sBackupFile)
                    If sHash <> oBackupInfo.CheckSum Then
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorFailedCheck, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                        If mgrCommon.ShowMessage(mgrRestore_ConfirmFailedCheck, oBackupInfo.Name, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            RaiseEvent UpdateLog(mgrRestore_ErrorCheckAbort, False, ToolTipIcon.Info, True)
                            bDoRestore = False
                        End If
                    Else
                        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_Verified, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                    End If
                Else
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_NoVerify, oBackupInfo.Name), False, ToolTipIcon.Info, True)
                End If
            End If

            If bDoRestore Then
                Try
                    If File.Exists(sBackupFile) Then
                        If mgrCommon.IsUnix Then
                            prs7z.StartInfo.Arguments = "x """ & sBackupFile & """ -o""" & sExtractPath & Path.DirectorySeparatorChar & """ -aoa -r"
                        Else
                            prs7z.StartInfo.Arguments = "x -bb1 -bt """ & sBackupFile & """ -o""" & sExtractPath & Path.DirectorySeparatorChar & """ -aoa -r"
                        End If
                        prs7z.StartInfo.FileName = mgrPath.Utility7zLocation
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
                                RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_RestoreWarnings, oBackupInfo.Name), True, ToolTipIcon.Warning, True)
                                bRestoreCompleted = False
                            End If
                        End If
                        prs7z.Dispose()
                    Else
                        RaiseEvent UpdateLog(mgrRestore_ErrorNoBackup, True, ToolTipIcon.Error, True)
                    End If

                        If bRestoreCompleted Then
                            'Save Local Manifest
                            If mgrManifest.DoManifestCheck(oBackupInfo.Name, mgrSQLite.Database.Local) Then
                                mgrManifest.DoManifestUpdate(oBackupInfo, mgrSQLite.Database.Local)
                            Else
                                mgrManifest.DoManifestAdd(oBackupInfo, mgrSQLite.Database.Local)
                            End If
                        End If
                Catch ex As Exception
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrRestore_ErrorOtherFailure, ex.Message), False, ToolTipIcon.Error, True)
                End Try

                If bRestoreCompleted Then
                    RaiseEvent SetLastAction(mgrCommon.FormatString(mgrRestore_ActionComplete, oBackupInfo.CroppedName))
                Else
                    RaiseEvent SetLastAction(mgrCommon.FormatString(mgrRestore_ActionFailed, oBackupInfo.CroppedName))
                End If
            End If
        Next
    End Sub

End Class
