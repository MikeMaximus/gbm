Imports GBM.My.Resources
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports System.IO

Public Class mgrMonitorList

    Public Enum eListTypes As Integer
        FullList = 1
        ScanList = 2
    End Enum

    Public Shared Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsGame
        Dim oGame As New clsGame

        oGame.ID = CStr(dr("MonitorID"))
        oGame.Name = CStr(dr("Name"))
        oGame.ProcessName = CStr(dr("Process"))
        If Not IsDBNull(dr("Path")) Then oGame.Path = CStr(dr("Path"))
        oGame.AbsolutePath = CBool(dr("AbsolutePath"))
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

        Return oGame
    End Function

    Private Shared Function SetCoreParameters(ByVal oGame As clsGame) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("ID", oGame.ID)
        hshParams.Add("Name", oGame.Name)
        hshParams.Add("Process", oGame.TrueProcess)
        hshParams.Add("Path", oGame.TruePath)
        hshParams.Add("AbsolutePath", oGame.AbsolutePath)
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

        Return hshParams
    End Function

    Public Shared Function ReadList(ByVal eListType As eListTypes, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame
        Dim oCompareGame As clsGame
        Dim bIsDupe As Boolean

        sSQL = "Select * FROM monitorlist ORDER BY Name Asc"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)
            Select Case eListType
                Case eListTypes.FullList
                    hshList.Add(oGame.ID, oGame)
                Case eListTypes.ScanList
                    For Each de As DictionaryEntry In hshList
                        bIsDupe = False
                        oCompareGame = DirectCast(de.Value, clsGame)

                        If oCompareGame.IsRegEx Then
                            If oGame.IsRegEx Then
                                If oCompareGame.ProcessName = oGame.ProcessName Then
                                    bIsDupe = True
                                End If
                            Else
                                If Regex.IsMatch(oGame.ProcessName, oCompareGame.ProcessName) Then
                                    bIsDupe = True
                                End If
                            End If
                        Else
                            If oGame.IsRegEx Then
                                If Regex.IsMatch(oCompareGame.ProcessName, oGame.ProcessName) Then
                                    bIsDupe = True
                                End If
                            Else
                                If oGame.ProcessName = oCompareGame.ProcessName Then
                                    bIsDupe = True
                                End If
                            End If
                        End If

                        If bIsDupe Then
                            DirectCast(hshList.Item(oCompareGame.ProcessName), clsGame).Duplicate = True
                            oGame.ProcessName = oGame.CompoundKey
                            oGame.Duplicate = True
                        End If
                    Next
                    If oGame.Enabled Then hshList.Add(oGame.ProcessName, oGame)
            End Select
        Next

        Return hshList
    End Function

    Public Shared Sub DoListAdd(ByVal oGame As clsGame, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO monitorlist VALUES (@ID, @Name, @Process, @Path, @AbsolutePath, @FolderSave, @FileType, @TimeStamp, "
        sSQL &= "@ExcludeList, @ProcessPath, @Icon, @Hours, @Version, @Company, @Enabled, @MonitorOnly, @BackupLimit, @CleanFolder, "
        sSQL &= "@Parameter, @Comments, @IsRegEx)"

        'Parameters
        hshParams = SetCoreParameters(oGame)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoListUpdate(ByVal oGame As clsGame, Optional ByVal sQueryID As String = "", Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE monitorlist SET MonitorID=@ID, Name=@Name, Process=@Process, Path=@Path, AbsolutePath=@AbsolutePath, FolderSave=@FolderSave, "
        sSQL &= "FileType=@FileType, TimeStamp=@TimeStamp, ExcludeList=@ExcludeList, ProcessPath=@ProcessPath, Icon=@Icon, "
        sSQL &= "Hours=@Hours, Version=@Version, Company=@Company, Enabled=@Enabled, MonitorOnly=@MonitorOnly, BackupLimit=@BackupLimit, "
        sSQL &= "CleanFolder=@CleanFolder, Parameter=@Parameter, Comments=@Comments, IsRegEx=@IsRegEx WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE gametags SET MonitorID=@ID WHERE MonitorID=@QueryID;"

        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "UPDATE gameprocesses SET MonitorID=@ID WHERE MonitorID=@QueryID;"
            sSQL &= "UPDATE sessions SET MonitorID=@ID WHERE MonitorID=@QueryID;"
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
        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "DELETE FROM gameprocesses "
            sSQL &= "WHERE MonitorID = @MonitorID;"
            sSQL &= "DELETE FROM sessions "
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

    Public Shared Function DoListGetbyID(ByVal iMonitorID As Integer, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As clsGame
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oGame As New clsGame
        Dim hshParams As New Hashtable

        sSQL = "SELECT * from monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", iMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)
        Next

        Return oGame
    End Function

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
        Dim sTimeStamp As String
        Dim sBackupLimit As String

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
        If (eSyncFields And clsGame.eOptionalSyncFields.TimeStamp) = clsGame.eOptionalSyncFields.TimeStamp Then
            sTimeStamp = "@TimeStamp"
            sBackupLimit = "@BackupLimit"
        Else
            sTimeStamp = "COALESCE((SELECT TimeStamp FROM monitorlist WHERE MonitorID=@ID),0)"
            sBackupLimit = "COALESCE((SELECT BackupLimit FROM monitorlist WHERE MonitorID=@ID),2)"
        End If
        If (eSyncFields And clsGame.eOptionalSyncFields.Version) = clsGame.eOptionalSyncFields.Version Then
            sVersion = "@Version"
        Else
            sVersion = "(SELECT Version FROM monitorlist WHERE MonitorID=@ID)"
        End If

        sSQL = "INSERT OR REPLACE INTO monitorlist (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx) "
        sSQL &= "VALUES (@ID, @Name, @Process, @Path, @AbsolutePath, @FolderSave, @FileType, "
        sSQL &= sTimeStamp & ", @ExcludeList, " & sGamePath & ", "
        sSQL &= sIcon & ", @Hours, " & sVersion & ", "
        sSQL &= sCompany & ", " & sMonitorGame & ", @MonitorOnly, " & sBackupLimit & ", @CleanFolder, @Parameter, @Comments, @IsRegEx);"

        For Each oGame As clsGame In hshGames.Values
            hshParams = New Hashtable

            'Core Parameters
            hshParams.Add("ID", oGame.ID)
            hshParams.Add("Name", oGame.Name)
            hshParams.Add("Process", oGame.TrueProcess)
            hshParams.Add("Path", oGame.TruePath)
            hshParams.Add("AbsolutePath", oGame.AbsolutePath)
            hshParams.Add("FolderSave", oGame.FolderSave)
            hshParams.Add("FileType", oGame.FileType)
            hshParams.Add("ExcludeList", oGame.ExcludeList)
            hshParams.Add("Hours", oGame.Hours)
            hshParams.Add("MonitorOnly", oGame.MonitorOnly)
            hshParams.Add("CleanFolder", oGame.CleanFolder)
            hshParams.Add("Parameter", oGame.Parameter)
            hshParams.Add("Comments", oGame.Comments)
            hshParams.Add("IsRegEx", oGame.IsRegEx)

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
            If (eSyncFields And clsGame.eOptionalSyncFields.TimeStamp) = clsGame.eOptionalSyncFields.TimeStamp Then
                hshParams.Add("TimeStamp", oGame.AppendTimeStamp)
                hshParams.Add("BackupLimit", oGame.BackupLimit)
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
        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "DELETE FROM gameprocesses "
            sSQL &= "WHERE MonitorID = @MonitorID;"
            sSQL &= "DELETE FROM sessions "
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

    Public Shared Sub SyncMonitorLists(ByVal eSyncFields As clsGame.eOptionalSyncFields, Optional ByVal bToRemote As Boolean = True)
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim iChanges As Integer

        Cursor.Current = Cursors.WaitCursor

        If bToRemote Then
            RaiseEvent UpdateLog(mgrMonitorList_SyncToMaster, False, ToolTipIcon.Info, True)
        Else
            RaiseEvent UpdateLog(mgrMonitorList_SyncFromMaster, False, ToolTipIcon.Info, True)
        End If

        'Add / Update Sync
        If bToRemote Then
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadList(eListTypes.FullList, mgrSQLite.Database.Remote)
            hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)
        End If

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.ID) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.ID), clsGame)
                If oFromItem.SyncEquals(oToItem, eSyncFields) Then
                    hshSyncItems.Remove(oFromItem.ID)
                End If
            End If
        Next

        If bToRemote Then
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Remote, eSyncFields)
        Else
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Local, eSyncFields)
        End If

        'Sync Tags
        iChanges = mgrTags.SyncTags(bToRemote)
        iChanges += mgrGameTags.SyncGameTags(bToRemote)

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

        RaiseEvent UpdateLog(mgrCommon.FormatString(mgrMonitorList_SyncChanges, (hshDeleteItems.Count + hshSyncItems.Count + iChanges).ToString), False, ToolTipIcon.Info, True)
        Cursor.Current = Cursors.Default
        Application.DoEvents()
    End Sub

    'Filter Functions
    Private Shared Function BuildFilterQuery(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter),
                                             ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean, ByVal bSortAsc As Boolean, ByVal sSortField As String,
                                             ByRef hshParams As Hashtable) As String
        Dim sSQL As String = String.Empty
        Dim iCounter As Integer = 0
        Dim sBaseSelect As String = "MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx FROM monitorlist"
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
                    Case clsGameFilterField.eDataType.fBool
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

        'Handle Sorting
        sSQL &= sSort

        Return sSQL

    End Function

    Public Shared Function ReadFilteredList(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter), ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean,
                                            ByVal bSortAsc As Boolean, ByVal sSortField As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As OrderedDictionary
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String = String.Empty
        Dim oList As New OrderedDictionary
        Dim oGame As clsGame
        Dim hshParams As New Hashtable
        Dim iCounter As Integer = 0

        sSQL = BuildFilterQuery(oIncludeTagFilters, oExcludeTagFilters, oFilters, eFilterType, bAndOperator, bSortAsc, sSortField, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = MapToObject(dr)

            oList.Add(oGame.ID, oGame)
        Next

        Return oList
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

        sSQL = BuildFilterQuery(oIncludeTagFilters, oExcludeTagFilters, oFilters, eFilterType, bAndOperator, bSortAsc, sSortField, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New Game
            oGame.ID = CStr(dr("MonitorID"))
            oGame.Name = CStr(dr("Name"))
            oGame.ProcessName = CStr(dr("Process"))
            If Not IsDBNull(dr("Path")) Then oGame.Path = CStr(dr("Path"))
            oGame.AbsolutePath = CBool(dr("AbsolutePath"))
            oGame.FolderSave = CBool(dr("FolderSave"))
            If Not IsDBNull(dr("FileType")) Then oGame.FileType = CStr(dr("FileType"))
            If Not IsDBNull(dr("ExcludeList")) Then oGame.ExcludeList = CStr(dr("ExcludeList"))
            oGame.MonitorOnly = CBool(dr("MonitorOnly"))
            If Not IsDBNull(dr("Parameter")) Then oGame.Parameter = CStr(dr("Parameter"))
            If Not IsDBNull(dr("Comments")) Then oGame.Comments = CStr(dr("Comments"))
            oGame.IsRegEx = CBool(dr("IsRegEx"))
            oGame.Tags = mgrGameTags.GetTagsByGameForExport(oGame.ID)
            oList.Add(oGame)
        Next

        Return oList
    End Function

    Public Shared Function SyncGameIDs(ByVal sPath As String, ByRef oSettings As mgrSettings, ByVal bOfficial As Boolean) As Boolean
        Dim sWarning As String

        If bOfficial Then
            If (oSettings.SupressMessages And mgrSettings.eSupressMessages.GameIDSync) = mgrSettings.eSupressMessages.GameIDSync Then
                sWarning = mgrMonitorList_ConfirmOfficialGameIDSync
            Else
                sWarning = mgrMonitorList_ConfirmInitialOfficialGameIDSync
                oSettings.SupressMessages = oSettings.SetMessageField(oSettings.SupressMessages, mgrSettings.eSupressMessages.GameIDSync)
                oSettings.SaveSettings()
            End If
        Else
                sWarning = mgrMonitorList_ConfirmFileGameIDSync
        End If

        If mgrCommon.ShowMessage(sWarning, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrCommon.IsAddress(sPath) Then
                If mgrCommon.CheckAddress(sPath) Then
                    DoGameIDSync(sPath, True)
                Else
                    mgrCommon.ShowMessage(mgrMonitorList_WebNoReponse, sPath, MsgBoxStyle.Exclamation)
                    Return False
                End If
            Else
                If File.Exists(sPath) Then
                    DoGameIDSync(sPath)
                Else
                    mgrCommon.ShowMessage(mgrMonitorList_FileNotFound, sPath, MsgBoxStyle.Exclamation)
                    Return False
                End If
            End If
        End If

        Return True
    End Function

    Public Shared Function DoImport(ByVal sPath As String, ByVal bOfficial As Boolean, ByRef oSettings As mgrSettings, Optional ByVal bStartUpWizard As Boolean = False) As Boolean
        If mgrCommon.IsAddress(sPath) Then
            If mgrCommon.CheckAddress(sPath) Then
                If bOfficial And Not bStartUpWizard And Not ((oSettings.SupressMessages And mgrSettings.eSupressMessages.GameIDSync) = mgrSettings.eSupressMessages.GameIDSync) Then
                    SyncGameIDs(sPath, oSettings, True)
                End If
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

        If oExportInfo.AppVer < 110 Then
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

                Cursor.Current = Cursors.Default
                mgrCommon.ShowMessage(mgrMonitorList_ImportComplete, MsgBoxStyle.Information)
            End If
        Else
            mgrCommon.ShowMessage(mgrMonitorList_ImportNothing, MsgBoxStyle.Information)
        End If

        Application.DoEvents()
    End Sub

    Private Shared Sub DoGameIDSync(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False)
        Dim oLocalDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oRemoteDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)
        Dim hshCompareFrom As New Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncIDs As New Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim oExportInfo As New ExportData

        Cursor.Current = Cursors.WaitCursor

        If Not mgrXML.ReadMonitorList(sLocation, oExportInfo, hshCompareFrom, bWebRead) Then
            Exit Sub
        End If

        If oExportInfo.AppVer < 110 Then
            mgrCommon.ShowMessage(mgrMonitorList_ErrorGameIDVerFailure, MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)

        For Each oFromItem In hshCompareFrom.Values
            If Not hshCompareTo.Contains(oFromItem.ID) Then
                For Each oToItem In hshCompareTo.Values
                    'Strip all special characters and compare names
                    If Regex.Replace(oToItem.Name, "[^\w]+", "").ToLower = Regex.Replace(oFromItem.Name, "[^\w]+", "").ToLower Then
                        'Ignore games with duplicate names
                        If Not hshSyncIDs.Contains(oFromItem.ID) Then
                            hshSyncIDs.Add(oFromItem.ID, oToItem.ID)
                        End If
                    End If
                Next
            End If
        Next

        For Each de As DictionaryEntry In hshSyncIDs
            hshParams = New Hashtable
            hshParams.Add("MonitorID", CStr(de.Key))
            hshParams.Add("QueryID", CStr(de.Value))
            oParamList.Add(hshParams)
        Next

        sSQL = "UPDATE monitorlist SET MonitorID=@MonitorID WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE gametags SET MonitorID=@MonitorID WHERE MonitorID=@QueryID;"
        sSQL &= "UPDATE manifest SET MonitorID=@MonitorID WHERE MonitorID=@QueryID;"

        oRemoteDatabase.RunMassParamQuery(sSQL, oParamList)

        sSQL &= "UPDATE sessions SET MonitorID=@MonitorID WHERE MonitorID=@QueryID;"

        oLocalDatabase.RunMassParamQuery(sSQL, oParamList)

        Cursor.Current = Cursors.Default

        mgrCommon.ShowMessage(mgrMonitorList_GameIDSyncCompleted, hshSyncIDs.Count.ToString, MsgBoxStyle.Information)

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

    'Other Functions
    Public Shared Sub HandleBackupLocationChange(ByVal oSettings As mgrSettings)
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
                If mgrCommon.ShowMessage(mgrMonitorList_ConfirmExistingData, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
                Else
                    mgrMonitorList.SyncMonitorLists(oSettings.SyncFields, False)
                End If
            Else
                mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
            End If
        Else
            mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
        End If
    End Sub
End Class
