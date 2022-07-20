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

    Public Enum eImportTypes As Integer
        Official = 1
        Ludusavi = 2
    End Enum

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
        oGame.Locked = CBool(dr("Locked"))

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
        hshParams.Add("Locked", oGame.Locked)

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
        sSQL &= "@Parameter, @Comments, @IsRegEx, @RecurseSubFolders, @OS, @UseWindowTitle, @Differential, @DiffInterval, @Locked)"

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
        sSQL &= "OS=@OS, UseWindowTitle=@UseWindowTitle, Differential=@Differential, DiffInterval=@DiffInterval, Locked=@Locked "
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

    Public Shared Sub DoListDelete(ByVal sMonitorIDs As List(Of String), Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParams As New List(Of Hashtable)

        For Each s As String In sMonitorIDs
            hshParams = New Hashtable
            hshParams.Add("MonitorID", s)
            oParams.Add(hshParams)
        Next

        sSQL = "DELETE FROM manifest WHERE MonitorID = @MonitorID;
                DELETE FROM gametags WHERE MonitorID = @MonitorID;
                DELETE FROM configlinks WHERE MonitorID = @MonitorID;
                DELETE FROM configlinks WHERE LinkID = @MonitorID;"

        If iSelectDB = mgrSQLite.Database.Local Then
            sSQL &= "DELETE FROM gameprocesses WHERE MonitorID = @MonitorID;
                     DELETE FROM sessions WHERE MonitorID = @MonitorID;
                     DELETE FROM winedata WHERE MonitorID = @MonitorID;
                     DELETE FROM launchdata WHERE MonitorID = @MonitorID;"
        End If

        sSQL &= "DELETE FROM monitorlist WHERE MonitorID = @MonitorID;"

        oDatabase.RunMassParamQuery(sSQL, oParams)
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

    Public Shared Function IsDuplicateName(ByVal sName As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As Object
        Dim hshParams As New Hashtable

        sSQL = "SELECT Name from monitorlist WHERE NAME = @Name COLLATE NOCASE GROUP BY Name HAVING Count(Name) > 1"

        hshParams.Add("Name", sName)

        oData = oDatabase.ReadSingleValue(sSQL, hshParams)

        If oData Is Nothing Then
            Return False
        Else
            Return True
        End If
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

    'Filter Functions
    Private Shared Function BuildFilterQuery(ByVal oIncludeTagFilters As List(Of clsTag), ByVal oExcludeTagFilters As List(Of clsTag), ByVal oFilters As List(Of clsGameFilter),
                                             ByVal eFilterType As frmFilter.eFilterType, ByVal bAndOperator As Boolean, ByVal bSortAsc As Boolean, ByVal sSortField As String,
                                             ByVal sQuickFilter As String, ByRef hshParams As Hashtable) As String
        Dim sSQL As String = String.Empty
        Dim iCounter As Integer = 0
        Dim sBaseSelect As String = "MonitorID, Name, Process, Path, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, 
                                    Parameter, Comments, IsRegEx, RecurseSubFolders, OS, UseWindowTitle, Differential, DiffInterval, Locked FROM monitorlist"
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

    Public Shared Function DoImport(ByVal sPath As String) As Boolean
        Dim frmImport As New frmAdvancedImport
        Dim frmOptions As New frmLudusaviConfig
        Dim oResult As DialogResult

        Select Case Path.GetExtension(sPath)
            Case ".xml"
                frmImport.ImportType = eImportTypes.Official
            Case ".yaml", ".yml"
                If frmOptions.ShowDialog() = DialogResult.OK Then
                    frmImport.ImportType = eImportTypes.Ludusavi
                    frmImport.LudusaviOptions = frmOptions.ImportOptions
                Else
                    Return False
                End If
            Case Else
                mgrCommon.ShowMessage(mgrMonitorList_ErrorImportFileType, MsgBoxStyle.Exclamation)
                Return False
        End Select

        frmImport.ImportPath = sPath
        oResult = frmImport.ShowDialog()

        'We will force a GC here, the import form can use a large amount of memory.
        frmImport.Dispose()
        GC.Collect()

        Return oResult = DialogResult.OK
    End Function

    Public Shared Sub ExportMonitorList(ByVal sLocation As String)
        Dim oOfficialXML As New mgrXML(sLocation)
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

        bSuccess = oOfficialXML.SerializeAndExport(oList)

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
                If mgrCommon.CheckAddress(sLocation) Then
                    'Save a valid URL for next time
                    oSavedPath.PathName = "Import_Custom_URL"
                    oSavedPath.Path = sLocation
                    mgrSavedPath.AddUpdatePath(oSavedPath)

                    If DoImport(sLocation) Then
                        Return True
                    End If
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

        oExtensions.Add(mgrMonitorList_XML, "*.xml")
        oExtensions.Add(mgrMonitorList_YAML, "*.yaml;*.yml")
        sLocation = mgrCommon.OpenFileBrowser("XML_Import", mgrMonitorList_ChooseImport, oExtensions, 1, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), True)

        If sLocation <> String.Empty Then
            If DoImport(sLocation) Then
                Return True
            End If
        End If

        Return False
    End Function

    Public Shared Sub ExportGameList()
        Dim sLocation As String
        Dim oExtensions As New SortedList

        oExtensions.Add(mgrMonitorList_XML, "*.xml")
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

        If DoImport(sImportUrl) Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function ImportLudusaviManifest(ByVal sImportUrl As String) As Boolean
        If Not (mgrSettings.SuppressMessages And mgrSettings.eSuppressMessages.LudusaviImportWarning) = mgrSettings.eSuppressMessages.LudusaviImportWarning Then
            mgrCommon.ShowMessage(mgrMonitorList_WarningLudusaviImport, MsgBoxStyle.Information)
            mgrSettings.SuppressMessages = mgrSettings.SetMessageField(mgrSettings.SuppressMessages, mgrSettings.eSuppressMessages.LudusaviImportWarning)
            mgrSettings.SaveSettings()
        End If

        If DoImport(sImportUrl) Then
            Return True
        End If

        Return False
    End Function
End Class