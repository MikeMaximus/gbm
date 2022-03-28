Imports GBM.My.Resources
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports System.IO

Public Class mgrMonitorList

    Public Enum eListTypes As Integer
        FullList = 1
        ScanList = 2
    End Enum

    Public Enum eSupportedClasses As Integer
        clsGame = 1
        clsBackup = 2
    End Enum

    Public Shared Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)

    'This function supports filling class types that inherit clsGameBase
    Public Shared Function MapToObject(ByVal dr As DataRow, Optional ByVal eClass As eSupportedClasses = eSupportedClasses.clsGame) As Object
        Dim bFullClass As Boolean = False
        Dim oGame As Object

        Select Case eClass
            Case eSupportedClasses.clsBackup
                oGame = New clsBackup
            Case Else
                bFullClass = True
                oGame = New clsGame
        End Select

        oGame.ID = CStr(dr("MonitorID"))
        oGame.Name = CStr(dr("Name"))
        oGame.ProcessName = CStr(dr("Process"))
        If Not IsDBNull(dr("Path")) Then oGame.Path = CStr(dr("Path"))
        oGame.FolderSave = CBool(dr("FolderSave"))
        If Not IsDBNull(dr("FileType")) Then oGame.FileType = CStr(dr("FileType"))
        oGame.AppendTimeStamp = CBool(dr("TimeStamp"))
        If Not IsDBNull(dr("ExcludeList")) Then oGame.ExcludeList = CStr(dr("ExcludeList"))
        If Not IsDBNull(dr("ProcessPath")) Then oGame.ProcessPath = CStr(dr("ProcessPath"))
        If Not IsDBNull(dr("Icon")) Then oGame.Icon = CStr(dr("Icon"))
        oGame.Hours = CDbl(dr("Hours"))
        If Not IsDBNull(dr("Version")) Then oGame.Version = CStr(dr("Version"))
        If Not IsDBNull(dr("Company")) Then oGame.Company = CStr(dr("Company"))
        oGame.Enabled = CBool(dr("Enabled"))
        oGame.MonitorOnly = CBool(dr("MonitorOnly"))
        oGame.BackupLimit = CInt(dr("BackupLimit"))
        oGame.CleanFolder = CBool(dr("CleanFolder"))
        If Not IsDBNull(dr("Parameter")) Then oGame.Parameter = CStr(dr("Parameter"))
        If Not IsDBNull(dr("Comments")) Then oGame.Comments = CStr(dr("Comments"))
        oGame.IsRegEx = CBool(dr("IsRegEx"))
        oGame.RecurseSubFolders = CBool(dr("RecurseSubFolders"))
        oGame.OS = CInt(dr("OS"))
        oGame.UseWindowTitle = CBool(dr("UseWindowTitle"))
        oGame.Differential = CBool(dr("Differential"))
        oGame.DiffInterval = CInt(dr("DiffInterval"))

        'Compile RegEx
        If oGame.IsRegEx And bFullClass Then
            oGame.CompiledRegEx = New Regex(oGame.ProcessName, RegexOptions.Compiled)
        End If

        Return oGame
    End Function

    Private Shared Function SetCoreParameters(ByVal oGame As clsGame) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("ID", oGame.ID)
        hshParams.Add("Name", oGame.Name)
        hshParams.Add("Process", oGame.ProcessName)
        hshParams.Add("Path", oGame.TruePath)
        hshParams.Add("FolderSave", oGame.FolderSave)
        hshParams.Add("FileType", oGame.FileType)
        hshParams.Add("TimeStamp", oGame.AppendTimeStamp)
        hshParams.Add("ExcludeList", oGame.ExcludeList)
        hshParams.Add("ProcessPath", oGame.ProcessPath)
        hshParams.Add("Icon", oGame.Icon)
        hshParams.Add("Hours", oGame.Hours)
        hshParams.Add("Version", oGame.Version)
        hshParams.Add("Company", oGame.Company)
        hshParams.Add("Enabled", oGame.Enabled)
        hshParams.Add("MonitorOnly", oGame.MonitorOnly)
        hshParams.Add("BackupLimit", oGame.BackupLimit)
        hshParams.Add("CleanFolder", oGame.CleanFolder)
        hshParams.Add("Parameter", oGame.Parameter)
        hshParams.Add("Comments", oGame.Comments)
        hshParams.Add("IsRegEx", oGame.IsRegEx)
        hshParams.Add("RecurseSubFolders", oGame.RecurseSubFolders)
        hshParams.Add("OS", oGame.OS)
        hshParams.Add("UseWindowTitle", oGame.UseWindowTitle)
        hshParams.Add("Differential", oGame.Differential)
        hshParams.Add("DiffInterval", oGame.DiffInterval)

        Return hshParams
    End Function

    Public Shared Function ReadList(ByVal eListType As eListTypes, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame

        sSQL = "SELECT * FROM monitorlist ORDER BY Name ASC"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)
            Select Case eListType
                Case eListTypes.FullList
                    hshList.Add(oGame.ID, oGame)
                Case eListTypes.ScanList
                    If oGame.Enabled And oGame.ProcessName <> String.Empty Then hshList.Add(oGame.ID, oGame)
            End Select
        Next

        Return hshList
    End Function

    Public Shared Function ReadFilteredList(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter), ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean,
                                            ByVal bSortAsc As Boolean, ByVal sSortField As String, Optional ByVal sQuickFilter As String = "", Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As OrderedDictionary
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String = String.Empty
        Dim oList As New OrderedDictionary
        Dim oGame As clsGame
        Dim hshParams As New Hashtable

        sSQL = BuildFilterQuery(oIncludeTagFilters, oExcludeTagFilters, oFilters, eFilterType, bAndOperator, bSortAsc, sSortField, sQuickFilter, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)
            oList.Add(oGame.ID, oGame)
        Next

        Return oList
    End Function

    Public Shared Sub DoListAdd(ByVal oGame As clsGame, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO monitorlist VALUES (@ID, @Name, @Process, @Path, @FolderSave, @FileType, @TimeStamp, "
        sSQL &= "@ExcludeList, @ProcessPath, @Icon, @Hours, @Version, @Company, @Enabled, @MonitorOnly, @BackupLimit, @CleanFolder, "
        sSQL &= "@Parameter, @Comments, @IsRegEx, @RecurseSubFolders, @OS, @UseWindowTitle, @Differential, @DiffInterval)"

        'Parameters
        hshParams = SetCoreParameters(oGame)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoListUpdate(ByVal oGame As clsGame, Optional ByVal sQueryID As String = "", Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE monitorlist SET MonitorID=@ID, Name=@Name, Process=@Process, Path=@Path, FolderSave=@FolderSave, "
        sSQL &= "FileType=@FileType, TimeStamp=@TimeStamp, ExcludeList=@ExcludeList, ProcessPath=@ProcessPath, Icon=@Icon, "
        sSQL &= "Hours=@Hours, Version=@Version, Company=@Company, Enabled=@Enabled, MonitorOnly=@MonitorOnly, BackupLimit=@BackupLimit, "
        sSQL &= "CleanFolder=@CleanFolder, Parameter=@Parameter, Comments=@Comments, IsRegEx=@IsRegEx, RecurseSubFolders=@RecurseSubFolders, "
        sSQL &= "OS=@OS, UseWindowTitle=@UseWindowTitle, Differential=@Differential, DiffInterval=@DiffInterval "
        sSQL &= "WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE gametags SET MonitorID=@ID WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE configlinks SET MonitorID=@ID WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE configlinks SET LinkID=@ID WHERE LinkID=@QueryID;"

        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "UPDATE gameprocesses SET MonitorID=@ID WHERE MonitorID=@QueryID;"
            sSQL &= "UPDATE sessions SET MonitorID=@ID WHERE MonitorID=@QueryID;"
            sSQL &= "UPDATE winedata SET MonitorID=@ID WHERE MonitorID=@QueryID;"
            sSQL &= "UPDATE launchdata SET MonitorID=@ID WHERE MonitorID=@QueryID;"
        End If

        'Parameters
        hshParams = SetCoreParameters(oGame)
        If sQueryID <> String.Empty Then
            hshParams.Add("QueryID", sQueryID)
        Else
            hshParams.Add("QueryID", oGame.ID)
        End If

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    'Do NOT change MonitorID with this function
    Public Shared Sub DoListFieldUpdate(ByVal sFieldName As String, ByVal oValue As Object, ByVal sQueryID As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE monitorlist SET " & sFieldName & "=@" & sFieldName & " WHERE MonitorID=@QueryID;"

        'Parameters
        hshParams.Add(sFieldName, oValue)
        hshParams.Add("QueryID", sQueryID)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoListUpdateMulti(ByVal sMonitorIDs As List(Of String), ByVal oGame As clsGame, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim iCounter As Integer

        sSQL = "UPDATE monitorlist SET Enabled=@Enabled, MonitorOnly=@MonitorOnly WHERE MonitorID IN ("

        'Parameters
        hshParams.Add("Enabled", oGame.Enabled)
        hshParams.Add("MonitorOnly", oGame.MonitorOnly)

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ")"

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoListDelete(ByVal sMonitorID As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

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

        hshParams.Add("MonitorID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoListDeleteMulti(ByVal sMonitorIDs As List(Of String), Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim iCounter As Integer

        sSQL = "DELETE FROM manifest "
        sSQL &= "WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

        sSQL &= "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

        sSQL &= "DELETE FROM configlinks "
        sSQL &= "WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

        sSQL &= "DELETE FROM configlinks "
        sSQL &= "WHERE LinkID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "DELETE FROM gameprocesses "
            sSQL &= "WHERE MonitorID IN ("

            For Each s As String In sMonitorIDs
                sSQL &= "@MonitorID" & iCounter & ","
                hshParams.Add("MonitorID" & iCounter, s)
                iCounter += 1
            Next

            sSQL = sSQL.TrimEnd(",")
            sSQL &= ");"

            sSQL &= "DELETE FROM sessions "
            sSQL &= "WHERE MonitorID IN ("

            For Each s As String In sMonitorIDs
                sSQL &= "@MonitorID" & iCounter & ","
                hshParams.Add("MonitorID" & iCounter, s)
                iCounter += 1
            Next

            sSQL = sSQL.TrimEnd(",")
            sSQL &= ");"

            sSQL &= "DELETE FROM winedata "
            sSQL &= "WHERE MonitorID IN ("

            For Each s As String In sMonitorIDs
                sSQL &= "@MonitorID" & iCounter & ","
                hshParams.Add("MonitorID" & iCounter, s)
                iCounter += 1
            Next

            sSQL = sSQL.TrimEnd(",")
            sSQL &= ");"

            sSQL &= "DELETE FROM launchdata "
            sSQL &= "WHERE MonitorID IN ("

            For Each s As String In sMonitorIDs
                sSQL &= "@MonitorID" & iCounter & ","
                hshParams.Add("MonitorID" & iCounter, s)
                iCounter += 1
            Next

            sSQL = sSQL.TrimEnd(",")
            sSQL &= ");"
        End If

        sSQL &= "DELETE FROM monitorlist "
        sSQL &= "WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Function DoListGetbyMonitorID(ByVal sMonitorID As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oGame As New clsGame
        Dim hshGames As New Hashtable
        Dim hshParams As New Hashtable
        Dim iCounter As Integer = 0

        sSQL = "SELECT * FROM monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)
            hshGames.Add(iCounter, oGame)
            iCounter += 1
        Next

        Return hshGames
    End Function

    Public Shared Function DoDuplicateListCheck(ByVal sMonitorID As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM monitorlist WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", sMonitorID)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND MonitorID <> @QueryID"
            hshParams.Add("QueryID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

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

        sSQL = "INSERT OR REPLACE INTO monitorlist (MonitorID, Name, Process, Path, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx, RecurseSubFolders, OS, UseWindowTitle, Differential, DiffInterval) "
        sSQL &= "VALUES (@ID, @Name, @Process, @Path, @FolderSave, @FileType, "
        sSQL &= "@TimeStamp, @ExcludeList, " & sGamePath & ", "
        sSQL &= sIcon & ", @Hours, " & sVersion & ", "
        sSQL &= sCompany & ", " & sMonitorGame & ", @MonitorOnly, @BackupLimit, @CleanFolder, @Parameter, @Comments, @IsRegEx, @RecurseSubFolders, @OS, @UseWindowTitle, @Differential, @DiffInterval);"

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

    Public Shared Sub SyncMonitorLists(Optional ByVal bToRemote As Boolean = True, Optional ByVal bSyncProtection As Boolean = True)
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim iChanges As Integer

        Cursor.Current = Cursors.WaitCursor

        If Not mgrSettings.DisableSyncMessages Then
            If bToRemote Then
                RaiseEvent UpdateLog(mgrMonitorList_SyncToMaster, False, ToolTipIcon.Info, True)
            Else
                RaiseEvent UpdateLog(mgrMonitorList_SyncFromMaster, False, ToolTipIcon.Info, True)
            End If
        End If

        'Add / Update Sync
        If bToRemote Then
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
        End If

        'Sync Wipe Protection
        If bSyncProtection Then
            If hshCompareFrom.Count = 0 And hshCompareTo.Count > 0 Then
                Cursor.Current = Cursors.Default
                If mgrCommon.ShowMessage(mgrMonitorList_WarningSyncProtection, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    'We will always show this one in the log regardless of setting
                    RaiseEvent UpdateLog(mgrMonitorList_ErrorSyncCancel, False, ToolTipIcon.Warning, True)
                    Exit Sub
                End If
                Cursor.Current = Cursors.WaitCursor
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

        If bToRemote Then
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Remote, mgrSettings.SyncFields)
        Else
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Local, mgrSettings.SyncFields)
        End If

        'Sync Tags
        iChanges = mgrTags.SyncTags(bToRemote)
        iChanges += mgrGameTags.SyncGameTags(bToRemote)
        iChanges += mgrConfigLinks.SyncConfigLinks(bToRemote)

        'Delete Sync
        If bToRemote Then
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
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

        If bToRemote Then
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        If Not mgrSettings.DisableSyncMessages Then
            RaiseEvent UpdateLog(mgrCommon.FormatString(mgrMonitorList_SyncChanges, (hshDeleteItems.Count + hshSyncItems.Count + iChanges).ToString), False, ToolTipIcon.Info, True)
        End If

        Cursor.Current = Cursors.Default
        Application.DoEvents()
    End Sub

    'Filter Functions
    Private Shared Function BuildFilterQuery(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter),
                                             ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean, ByVal bSortAsc As Boolean, ByVal sSortField As String,
                                             ByVal sQuickFilter As String, ByRef hshParams As Hashtable) As String
        Dim sSQL As String = String.Empty
        Dim iCounter As Integer = 0
        Dim sBaseSelect As String = "MonitorID, Name, Process, Path, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx, RecurseSubFolders, OS, UseWindowTitle, Differential, DiffInterval FROM monitorlist"
        Dim sSort As String = " ORDER BY " & sSortField

        If bSortAsc Then
            sSort &= " ASC"
        Else
            sSort &= " DESC"
        End If

        Select Case eFilterType
            Case frmFilter.eFilterType.BaseFilter
                sSQL = "SELECT " & sBaseSelect
            Case frmFilter.eFilterType.AnyTag

                If oExcludeTagFilters.Count > 0 And oIncludeTagFilters.Count = 0 Then
                    sSQL = "SELECT " & sBaseSelect

                    sSQL &= " WHERE MonitorID NOT IN (SELECT MonitorID FROM monitorlist NATURAL JOIN gametags WHERE gametags.TagID IN ("

                    For Each oTag As clsTag In oExcludeTagFilters
                        sSQL &= "@TagID" & iCounter & ","
                        hshParams.Add("TagID" & iCounter, oTag.ID)
                        iCounter += 1
                    Next

                    sSQL = sSQL.TrimEnd(",")
                    sSQL &= "))"
                Else
                    sSQL = "SELECT DISTINCT " & sBaseSelect

                    sSQL &= " NATURAL JOIN gametags WHERE gametags.TagID IN ("

                    For Each oTag As clsTag In oIncludeTagFilters
                        sSQL &= "@TagID" & iCounter & ","
                        hshParams.Add("TagID" & iCounter, oTag.ID)
                        iCounter += 1
                    Next

                    sSQL = sSQL.TrimEnd(",")
                    sSQL &= ")"

                    If oExcludeTagFilters.Count > 0 Then
                        sSQL &= " AND MonitorID NOT IN (SELECT MonitorID FROM monitorlist NATURAL JOIN gametags WHERE gametags.TagID IN ("

                        For Each oTag As clsTag In oExcludeTagFilters
                            sSQL &= "@TagID" & iCounter & ","
                            hshParams.Add("TagID" & iCounter, oTag.ID)
                            iCounter += 1
                        Next

                        sSQL = sSQL.TrimEnd(",")
                        sSQL &= "))"
                    End If
                End If

            Case frmFilter.eFilterType.AllTags

                If oExcludeTagFilters.Count > 0 And oIncludeTagFilters.Count = 0 Then
                    sSQL = "SELECT " & sBaseSelect & " WHERE MonitorID NOT IN "

                    For Each oTag As clsTag In oExcludeTagFilters
                        sSQL &= "(SELECT MonitorID FROM gametags WHERE monitorlist.MonitorID = gametags.MonitorID And TagID = @TagID" & iCounter & ")"
                        If iCounter <> oExcludeTagFilters.Count - 1 Then
                            sSQL &= " AND MonitorID IN "
                        End If
                        hshParams.Add("TagID" & iCounter, oTag.ID)
                        iCounter += 1
                    Next
                Else
                    sSQL = "SELECT " & sBaseSelect & " WHERE MonitorID IN "

                    For Each oTag As clsTag In oIncludeTagFilters
                        sSQL &= "(SELECT MonitorID FROM gametags WHERE monitorlist.MonitorID = gametags.MonitorID And TagID = @TagID" & iCounter & ")"
                        If iCounter <> oIncludeTagFilters.Count - 1 Then
                            sSQL &= " AND MonitorID IN "
                        End If
                        hshParams.Add("TagID" & iCounter, oTag.ID)
                        iCounter += 1
                    Next

                    If oExcludeTagFilters.Count > 0 Then
                        sSQL &= " AND MonitorID NOT IN (SELECT MonitorID FROM monitorlist NATURAL JOIN gametags WHERE gametags.TagID IN ("

                        For Each oTag As clsTag In oExcludeTagFilters
                            sSQL &= "@TagID" & iCounter & ","
                            hshParams.Add("TagID" & iCounter, oTag.ID)
                            iCounter += 1
                        Next

                        sSQL = sSQL.TrimEnd(",")
                        sSQL &= "))"
                    End If
                End If

            Case frmFilter.eFilterType.NoTags
                sSQL = "SELECT " & sBaseSelect & " WHERE MonitorID NOT IN (SELECT MonitorID FROM gametags)"
        End Select

        'Handle Other Filters
        If oFilters.Count > 0 Then
            If eFilterType = frmFilter.eFilterType.BaseFilter Then
                sSQL &= " WHERE ("
            Else
                sSQL &= " AND ("
            End If

            iCounter = 0
            For Each oFilter As clsGameFilter In oFilters
                If oFilter.NotCondition Then
                    sSQL &= " NOT "
                End If

                Select Case oFilter.Field.Type
                    Case clsGameFilterField.eDataType.fString
                        sSQL &= oFilter.Field.FieldName & " LIKE @" & oFilter.ID
                        hshParams.Add(oFilter.ID, "%" & oFilter.Data & "%")
                    Case clsGameFilterField.eDataType.fNumeric
                        sSQL &= oFilter.Field.FieldName & " " & oFilter.NumericOperatorAsString & " @" & oFilter.ID
                        hshParams.Add(oFilter.ID, oFilter.Data)
                    Case clsGameFilterField.eDataType.fBool, clsGameFilterField.eDataType.fEnum
                        sSQL &= oFilter.Field.FieldName & " = @" & oFilter.ID
                        hshParams.Add(oFilter.ID, oFilter.Data)
                End Select

                iCounter += 1
                If iCounter <> oFilters.Count Then
                    If bAndOperator Then
                        sSQL &= " AND "
                    Else
                        sSQL &= " OR "
                    End If
                End If
            Next
            sSQL &= ")"
        End If

        'Handle Quick Filter
        If Not sQuickFilter = String.Empty Then
            If eFilterType = frmFilter.eFilterType.BaseFilter And oFilters.Count = 0 Then
                sSQL &= " WHERE "
            Else
                sSQL &= " AND "
            End If
            sSQL &= "MonitorID IN (SELECT MonitorID FROM monitorlist WHERE Name LIKE @QuickName OR MonitorID IN (SELECT MonitorID FROM gametags NATURAL JOIN tags WHERE tags.Name=@QuickTag COLLATE NOCASE))"
            hshParams.Add("QuickName", "%" & sQuickFilter & "%")
            hshParams.Add("QuickTag", sQuickFilter)
        End If

        'Handle Sorting
        sSQL &= sSort

        Return sSQL
    End Function

    'Import / Export Functions
    Public Shared Function ReadListForExport(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter), ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean,
                                             ByVal bSortAsc As Boolean, ByVal sSortField As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As List(Of Game)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String = String.Empty
        Dim oList As New List(Of Game)
        Dim oGame As Game
        Dim hshParams As New Hashtable

        sSQL = BuildFilterQuery(oIncludeTagFilters, oExcludeTagFilters, oFilters, eFilterType, bAndOperator, bSortAsc, sSortField, String.Empty, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New Game
            oGame.ID = CStr(dr("MonitorID"))
            oGame.Name = CStr(dr("Name"))
            oGame.ProcessName = CStr(dr("Process"))
            If Not IsDBNull(dr("Path")) Then oGame.Path = CStr(dr("Path"))
            oGame.FolderSave = CBool(dr("FolderSave"))
            oGame.AppendTimeStamp = CBool(dr("TimeStamp"))
            oGame.BackupLimit = CInt(dr("BackupLimit"))
            If Not IsDBNull(dr("FileType")) Then oGame.FileType = CStr(dr("FileType"))
            If Not IsDBNull(dr("ExcludeList")) Then oGame.ExcludeList = CStr(dr("ExcludeList"))
            oGame.MonitorOnly = CBool(dr("MonitorOnly"))
            If Not IsDBNull(dr("Parameter")) Then oGame.Parameter = CStr(dr("Parameter"))
            If Not IsDBNull(dr("Comments")) Then oGame.Comments = CStr(dr("Comments"))
            oGame.IsRegEx = CBool(dr("IsRegEx"))
            oGame.RecurseSubFolders = CBool(dr("RecurseSubFolders"))
            oGame.OS = CInt(dr("OS"))
            oGame.UseWindowTitle = CBool(dr("UseWindowTitle"))
            oGame.Differential = CBool(dr("Differential"))
            oGame.DiffInterval = CInt(dr("DiffInterval"))
            oGame.Tags = mgrGameTags.GetTagsByGameForExport(oGame.ID)
            oGame.ConfigLinks = mgrConfigLinks.GetConfigLinksByGameForExport(oGame.ID)
            oList.Add(oGame)
        Next

        Return oList
    End Function

    Public Shared Function DoImport(ByVal sPath As String, ByVal bOfficial As Boolean) As Boolean
        If mgrCommon.IsAddress(sPath) Then
            If mgrCommon.CheckAddress(sPath) Then
                ImportMonitorList(sPath, True)
                Return True
            Else
                mgrCommon.ShowMessage(mgrMonitorList_WebNoReponse, sPath, MsgBoxStyle.Exclamation)
                Return False
            End If
        Else
            If File.Exists(sPath) Then
                ImportMonitorList(sPath)
                Return True
            Else
                mgrCommon.ShowMessage(mgrMonitorList_FileNotFound, sPath, MsgBoxStyle.Exclamation)
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Sub ImportMonitorList(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False)
        Dim hshCompareFrom As New Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim oExportInfo As New ExportData

        Cursor.Current = Cursors.WaitCursor

        If Not mgrXML.ReadMonitorList(sLocation, oExportInfo, hshCompareFrom, bWebRead) Then
            Exit Sub
        End If

        If oExportInfo.AppVer < 115 Then
            If mgrCommon.ShowMessage(mgrMonitorList_ImportVersionWarning, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.ID) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.ID), clsGame)
                If oFromItem.MinimalEquals(oToItem) Then
                    If oFromItem.CoreEquals(oToItem) Then
                        hshSyncItems.Remove(oFromItem.ID)
                    Else
                        DirectCast(hshSyncItems(oFromItem.ID), clsGame).ImportUpdate = True
                        'These fields need to be set via the object or they will be lost when the configuration is updated
                        DirectCast(hshSyncItems(oFromItem.ID), clsGame).Hours = oToItem.Hours
                        DirectCast(hshSyncItems(oFromItem.ID), clsGame).CleanFolder = oToItem.CleanFolder
                    End If

                End If
            End If
        Next

        Cursor.Current = Cursors.Default

        If hshSyncItems.Count > 0 Then
            Dim frm As New frmAdvancedImport
            frm.ImportInfo = oExportInfo
            frm.ImportData = hshSyncItems
            If frm.ShowDialog() = DialogResult.OK Then
                Cursor.Current = Cursors.WaitCursor

                DoListAddUpdateSync(frm.FinalData)
                mgrTags.DoTagAddImport(frm.FinalData)
                mgrConfigLinks.DoConfigLinkImport(frm.FinalData)

                Cursor.Current = Cursors.Default
                mgrCommon.ShowMessage(mgrMonitorList_ImportComplete, MsgBoxStyle.Information)
            End If
        Else
            mgrCommon.ShowMessage(mgrMonitorList_ImportNothing, MsgBoxStyle.Information)
        End If

        Application.DoEvents()
    End Sub

    Public Shared Sub ExportMonitorList(ByVal sLocation As String)
        Dim oList As List(Of Game)
        Dim bSuccess As Boolean = False
        Dim oIncludeTagFilters As New List(Of clsTag)
        Dim oExcludeTagFilters As New List(Of clsTag)
        Dim oFilters As New List(Of clsGameFilter)
        Dim eCurrentFilter As frmFilter.eFilterType = frmFilter.eFilterType.BaseFilter
        Dim bAndOperator As Boolean = True
        Dim bSortAsc As Boolean = True
        Dim sSortField As String = "Name"

        If mgrCommon.ShowMessage(mgrMonitorList_ConfirmApplyFilter, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim frm As New frmFilter
            frm.ShowDialog()
            oIncludeTagFilters = frm.IncludeTagFilters
            oExcludeTagFilters = frm.ExcludeTagFilters
            oFilters = frm.GameFilters
            eCurrentFilter = frm.FilterType
            bAndOperator = frm.AndOperator
            bSortAsc = frm.SortAsc
            sSortField = frm.SortField
        End If

        oList = ReadListForExport(oIncludeTagFilters, oExcludeTagFilters, oFilters, eCurrentFilter, bAndOperator, bSortAsc, sSortField)

        bSuccess = mgrXML.SerializeAndExport(oList, sLocation)

        If bSuccess Then
            mgrCommon.ShowMessage(mgrMonitorList_ExportComplete, oList.Count, MsgBoxStyle.Information)
        End If
    End Sub

    'Shared Import / Export UI Functions
    Public Shared Function ImportGameListURL() As Boolean
        Dim oSavedPath As clsSavedPath = mgrSavedPath.GetPathByName("Import_Custom_URL")
        Dim sLocation As String

        sLocation = InputBox(mgrMonitorList_CustomListURLInfo, mgrMonitorList_CustomListURLTitle, oSavedPath.Path).Trim

        If sLocation <> String.Empty Then
            If mgrCommon.IsAddress(sLocation) Then
                If DoImport(sLocation, False) Then
                    'Save valid URL for next time
                    oSavedPath.PathName = "Import_Custom_URL"
                    oSavedPath.Path = sLocation
                    mgrSavedPath.AddUpdatePath(oSavedPath)
                    Return True
                End If
            Else
                mgrCommon.ShowMessage(mgrMonitorList_CustomListURLError, MsgBoxStyle.Exclamation)
            End If
        End If

        Return False
    End Function

    Public Shared Function ImportGameListFile() As Boolean
        Dim sLocation As String
        Dim oExtensions As New SortedList

        oExtensions.Add(mgrMonitorList_XML, "xml")
        sLocation = mgrCommon.OpenFileBrowser("XML_Import", mgrMonitorList_ChooseImportXML, oExtensions, 1, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), False)

        If sLocation <> String.Empty Then
            If mgrMonitorList.DoImport(sLocation, False) Then
                Return True
            End If
        End If

        Return False
    End Function

    Public Shared Sub ExportGameList()
        Dim sLocation As String
        Dim oExtensions As New SortedList

        oExtensions.Add(mgrMonitorList_XML, "xml")
        sLocation = mgrCommon.SaveFileBrowser("XML_Export", mgrMonitorList_ChooseExportXML, oExtensions, 1, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), mgrMonitorList_DefaultExportFileName & " " & Date.Now.ToString("dd-MMM-yyyy"))

        If sLocation <> String.Empty Then
            ExportMonitorList(sLocation)
        End If

    End Sub

    Public Shared Function ImportOfficialGameList(ByVal sImportUrl As String, Optional ByVal bIsWindowsList As Boolean = False) As Boolean
        'Show one time warning about Windows configs in Linux
        If bIsWindowsList And mgrCommon.IsUnix Then
            If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.WinConfigsInLinux) = mgrSettings.eSuppressMessages.WinConfigsInLinux Then
                mgrCommon.ShowMessage(mgrMonitorList_WarningWinConfigsInLinux, MsgBoxStyle.Information)
                mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.WinConfigsInLinux)
                mgrSettings.SaveSettings()
            End If
        End If

        If mgrCommon.ShowMessage(mgrMonitorList_ConfirmOfficialImport, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(sImportUrl, True) Then
                Return True
            End If
        End If

        Return False
    End Function

    'Other Functions
    Public Shared Sub HandleBackupLocationChange()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        Dim iGameCount As Integer

        'Check if a remote database already exists in the new backup location
        If oDatabase.CheckDB() Then
            'Make sure database is the latest version
            oDatabase.DatabaseUpgrade()

            'See if the remote database is empty
            iGameCount = mgrMonitorList.ReadList(eListTypes.FullList, mgrSQLite.Database.Remote).Count

            'If the remote database actually contains a list, then ask what to do
            If iGameCount > 0 Then
                If mgrCommon.ShowPriorityMessage(mgrMonitorList_ConfirmExistingData, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    mgrMonitorList.SyncMonitorLists()
                Else
                    mgrMonitorList.SyncMonitorLists(False)
                End If
            Else
                mgrMonitorList.SyncMonitorLists()
            End If
        Else
            mgrMonitorList.SyncMonitorLists()
        End If
    End Sub
End Class
