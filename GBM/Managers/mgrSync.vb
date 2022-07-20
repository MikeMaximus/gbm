Imports GBM.My.Resources
Imports System.Threading

Public Class mgrSync

    Private Class SyncOptions
        Public Property ToRemote As Boolean
        Public Property SyncProtection As Boolean

        Sub New(bToRemote As Boolean, bSyncProtection As Boolean)
            ToRemote = bToRemote
            SyncProtection = bSyncProtection
        End Sub
    End Class

    Private Shared Property SyncThread As Thread

    Public Shared Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)
    Public Shared Event PushStarted()
    Public Shared Event PushEnded()

    'Sync Functions
    Public Shared Sub DoListAddUpdateSync(ByVal hshGames As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local,
                                          Optional ByVal eSyncFields As clsGame.eOptionalSyncFields = clsGame.eOptionalSyncFields.None)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        'Handle Optional Sync Fields
        Dim sGamePath As String
        Dim sIcon As String
        Dim sVersion As String
        Dim sCompany As String
        Dim sMonitorGame As String

        'Setup SQL for optional fields
        If (eSyncFields And clsGame.eOptionalSyncFields.Company) = clsGame.eOptionalSyncFields.Company Then
            sCompany = "@Company"
        Else
            sCompany = "(SELECT Company FROM monitorlist WHERE MonitorID=@ID)"
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then
            sGamePath = "@ProcessPath"
        Else
            sGamePath = "(SELECT ProcessPath FROM monitorlist WHERE MonitorID=@ID)"
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.Icon) = clsGame.eOptionalSyncFields.Icon Then
            sIcon = "@Icon"
        Else
            sIcon = "(SELECT Icon FROM monitorlist WHERE MonitorID=@ID)"
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.MonitorGame) = clsGame.eOptionalSyncFields.MonitorGame Then
            sMonitorGame = "@Enabled"
        Else
            sMonitorGame = "COALESCE((SELECT Enabled FROM monitorlist WHERE MonitorID=@ID),1)"
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.Version) = clsGame.eOptionalSyncFields.Version Then
            sVersion = "@Version"
        Else
            sVersion = "(SELECT Version FROM monitorlist WHERE MonitorID=@ID)"
        End If

        sSQL = "INSERT OR REPLACE INTO monitorlist (MonitorID, Name, Process, Path, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx, RecurseSubFolders, OS, UseWindowTitle, Differential, DiffInterval, Locked) "
        sSQL &= "VALUES (@ID, @Name, @Process, @Path, @FolderSave, @FileType, "
        sSQL &= "@TimeStamp, @ExcludeList, " & sGamePath & ", "
        sSQL &= sIcon & ", @Hours, " & sVersion & ", "
        sSQL &= sCompany & ", " & sMonitorGame & ", @MonitorOnly, @BackupLimit, @CleanFolder, @Parameter, @Comments, @IsRegEx, @RecurseSubFolders, @OS, @UseWindowTitle, @Differential, @DiffInterval, @Locked);"

        For Each oGame As clsGame In hshGames.Values
            hshParams = New Hashtable

            'Core Parameters
            hshParams.Add("ID", oGame.ID)
            hshParams.Add("Name", oGame.Name)
            hshParams.Add("Process", oGame.ProcessName)
            hshParams.Add("Path", oGame.TruePath)
            hshParams.Add("FolderSave", oGame.FolderSave)
            hshParams.Add("TimeStamp", oGame.AppendTimeStamp)
            hshParams.Add("BackupLimit", oGame.BackupLimit)
            hshParams.Add("FileType", oGame.FileType)
            hshParams.Add("ExcludeList", oGame.ExcludeList)
            hshParams.Add("Hours", oGame.Hours)
            hshParams.Add("MonitorOnly", oGame.MonitorOnly)
            hshParams.Add("CleanFolder", oGame.CleanFolder)
            hshParams.Add("Parameter", oGame.Parameter)
            hshParams.Add("Comments", oGame.Comments)
            hshParams.Add("IsRegEx", oGame.IsRegEx)
            hshParams.Add("RecurseSubFolders", oGame.RecurseSubFolders)
            hshParams.Add("OS", oGame.OS)
            hshParams.Add("UseWindowTitle", oGame.UseWindowTitle)
            hshParams.Add("Differential", oGame.Differential)
            hshParams.Add("DiffInterval", oGame.DiffInterval)
            hshParams.Add("Locked", oGame.Locked)

            'Optional Parameters
            If (eSyncFields And clsGame.eOptionalSyncFields.Company) = clsGame.eOptionalSyncFields.Company Then
                hshParams.Add("Company", oGame.Company)
            End If
            If (eSyncFields And clsGame.eOptionalSyncFields.GamePath) = clsGame.eOptionalSyncFields.GamePath Then
                hshParams.Add("ProcessPath", oGame.ProcessPath)
            End If
            If (eSyncFields And clsGame.eOptionalSyncFields.Icon) = clsGame.eOptionalSyncFields.Icon Then
                hshParams.Add("Icon", oGame.Icon)
            End If
            If (eSyncFields And clsGame.eOptionalSyncFields.MonitorGame) = clsGame.eOptionalSyncFields.MonitorGame Then
                hshParams.Add("Enabled", oGame.Enabled)
            End If
            If (eSyncFields And clsGame.eOptionalSyncFields.Version) = clsGame.eOptionalSyncFields.Version Then
                hshParams.Add("Version", oGame.Version)
            End If

            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)

    End Sub

    Public Shared Sub DoListDeleteSync(ByVal hshGames As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM manifest "
        sSQL &= "WHERE MonitorID = @MonitorID;"
        sSQL &= "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID = @MonitorID;"
        sSQL &= "DELETE FROM configlinks "
        sSQL &= "WHERE MonitorID = @MonitorID;"
        sSQL &= "DELETE FROM configlinks "
        sSQL &= "WHERE LinkID = @MonitorID;"
        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "DELETE FROM gameprocesses "
            sSQL &= "WHERE MonitorID = @MonitorID;"
            sSQL &= "DELETE FROM sessions "
            sSQL &= "WHERE MonitorID = @MonitorID;"
            sSQL &= "DELETE FROM winedata "
            sSQL &= "WHERE MonitorID = @MonitorID;"
            sSQL &= "DELETE FROM launchdata "
            sSQL &= "WHERE MonitorID = @MonitorID;"
        End If
        sSQL &= "DELETE FROM monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID;"

        For Each oGame As clsGame In hshGames.Values
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oGame.ID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub SyncData(Optional ByVal bToRemote As Boolean = True, Optional ByVal bSyncProtection As Boolean = True)
        Dim oSyncOptions As New SyncOptions(bToRemote, bSyncProtection)

        If bToRemote Then
            If Not SyncThread Is Nothing Then
                If SyncThread.ThreadState = ThreadState.Running Then
                    SyncThread.Join()
                End If
            End If
            SyncThread = New Thread(AddressOf DoSync)
            SyncThread.Start(oSyncOptions)
        Else
            DoSync(oSyncOptions)
        End If
    End Sub

    Private Shared Sub DoSync(ByVal oSyncOptions As SyncOptions)
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim iChanges As Integer

        If oSyncOptions.ToRemote Then RaiseEvent PushStarted()

        If Not mgrSettings.DisableSyncMessages Then
            If oSyncOptions.ToRemote Then
                RaiseEvent UpdateLog(mgrMonitorList_SyncToMaster, False, ToolTipIcon.Info, True)
            Else
                RaiseEvent UpdateLog(mgrMonitorList_SyncFromMaster, False, ToolTipIcon.Info, True)
            End If
        End If

        'Add / Update Sync
        If oSyncOptions.ToRemote Then
            hshCompareFrom = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)
            hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Remote)
            hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)
        End If

        'Sync Wipe Protection
        If oSyncOptions.SyncProtection Then
            If hshCompareFrom.Count = 0 And hshCompareTo.Count > 0 Then
                If mgrCommon.ShowMessage(mgrMonitorList_WarningSyncProtection, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    'We will always show this one in the log regardless of setting
                    RaiseEvent UpdateLog(mgrMonitorList_ErrorSyncCancel, False, ToolTipIcon.Warning, True)
                    Exit Sub
                End If
            End If
        End If

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.ID) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.ID), clsGame)
                If oFromItem.SyncEquals(oToItem, mgrSettings.SyncFields) Then
                    hshSyncItems.Remove(oFromItem.ID)
                End If
            End If
        Next

        If oSyncOptions.ToRemote Then
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Remote, mgrSettings.SyncFields)
        Else
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Local, mgrSettings.SyncFields)
        End If

        'Sync Tags
        iChanges = mgrTags.SyncTags(oSyncOptions.ToRemote)
        iChanges += mgrGameTags.SyncGameTags(oSyncOptions.ToRemote)
        iChanges += mgrConfigLinks.SyncConfigLinks(oSyncOptions.ToRemote)

        'Delete Sync
        If oSyncOptions.ToRemote Then
            hshCompareFrom = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)
            hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Remote)
            hshCompareTo = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)
        End If

        hshDeleteItems = hshCompareTo.Clone

        For Each oToItem In hshCompareTo.Values
            If hshCompareFrom.Contains(oToItem.ID) Then
                oFromItem = DirectCast(hshCompareFrom(oToItem.ID), clsGame)
                If oToItem.MinimalEquals(oFromItem) Then
                    hshDeleteItems.Remove(oToItem.ID)
                End If
            End If
        Next

        If oSyncOptions.ToRemote Then
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        If Not mgrSettings.DisableSyncMessages Then
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrMonitorList_SyncChanges, (hshDeleteItems.Count + hshSyncItems.Count + iChanges).ToString), False, ToolTipIcon.Info, True)
        End If

        If oSyncOptions.ToRemote Then RaiseEvent PushEnded()
    End Sub

    'Other Functions
    Public Shared Sub HandleBackupLocationChange()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        Dim iGameCount As Integer

        'Check if a remote database already exists in the new backup location
        If oDatabase.CheckDB() Then
            'Make sure database is the latest version
            oDatabase.DatabaseUpgrade()

            'See if the remote database is empty
            iGameCount = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Remote).Count

            'If the remote database actually contains a list, then ask what to do
            If iGameCount > 0 Then
                If mgrCommon.ShowPriorityMessage(mgrMonitorList_ConfirmExistingData, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    SyncData()
                Else
                    SyncData(False)
                End If
            Else
                SyncData()
            End If
        Else
            SyncData()
        End If
    End Sub
End Class
