Imports System.IO

Public Class mgrMonitorList

    Public Enum eListTypes As Integer
        FullList = 1
        ScanList = 2
    End Enum

    Public Shared Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)

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
                If MsgBox("GBM data already exists in the backup folder." & vbCrLf & vbCrLf & _
                          "Do you want to make your local game list the new master game list in this folder? (Recommended)" & vbCrLf & vbCrLf & _
                          "Choosing No will sync your local game list to the current master game list in this folder.", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
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

    Public Shared Sub ExportMonitorList(ByVal sLocation As String)
        Dim oList As List(Of Game)
        Dim bSuccess As Boolean = False
        Dim oTagFilters As New List(Of clsTag)
        Dim oStringFilters As New Hashtable
        Dim eCurrentFilter As frmFilter.eFilterType = frmFilter.eFilterType.NoFilter

        If MsgBox("Would you like to apply a filter to your export?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
            Dim frm As New frmFilter
            frm.ShowDialog()
            oTagFilters = frm.TagFilters
            oStringFilters = frm.StringFilters
            eCurrentFilter = frm.FilterType
        End If

        oList = ReadListForExport(oTagFilters, oStringFilters, eCurrentFilter)

        bSuccess = mgrXML.SerializeAndExport(oList, sLocation)
        
        If bSuccess Then
            MsgBox("Export Complete.  " & oList.Count & " item(s) have been exported.", MsgBoxStyle.Information, "Game Backup Monitor")
        End If
    End Sub

    Public Shared Sub DoListAddUpdateSync(ByVal hshGames As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT OR REPLACE INTO monitorlist (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly) "
        sSQL &= "VALUES (@ID, @Name, @Process, @Path, @AbsolutePath, @FolderSave, @FileType, "
        sSQL &= "@TimeStamp, @ExcludeList, (SELECT ProcessPath FROM monitorlist WHERE MonitorID=@ID), "
        sSQL &= "(SELECT Icon FROM monitorlist WHERE MonitorID=@ID), @Hours, (SELECT Version FROM monitorlist WHERE MonitorID=@ID), "
        sSQL &= "(SELECT Company FROM monitorlist WHERE MonitorID=@ID), COALESCE((SELECT Enabled FROM monitorlist WHERE MonitorID=@ID),1), COALESCE((SELECT MonitorOnly FROM monitorlist WHERE MonitorID=@ID),0));"

        For Each oGame As clsGame In hshGames.Values
            hshParams = New Hashtable

            'Parameters
            hshParams.Add("ID", oGame.ID)
            hshParams.Add("Name", oGame.Name)
            hshParams.Add("Process", oGame.TrueProcess)
            hshParams.Add("Path", oGame.TruePath)
            hshParams.Add("AbsolutePath", oGame.AbsolutePath)
            hshParams.Add("FolderSave", oGame.FolderSave)
            hshParams.Add("FileType", oGame.FileType)
            hshParams.Add("TimeStamp", oGame.AppendTimeStamp)
            hshParams.Add("ExcludeList", oGame.ExcludeList)
            hshParams.Add("Hours", oGame.Hours)

            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)

    End Sub

    Public Shared Sub DoListDeleteSync(ByVal hshGames As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID = @MonitorID;"
        sSQL &= "DELETE FROM monitorlist "
        sSQL &= "WHERE Name = @Name AND Process= @Process;"

        For Each oGame As clsGame In hshGames.Values
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oGame.ID)
            hshParams.Add("Name", oGame.Name)
            hshParams.Add("Process", oGame.TrueProcess)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub SyncMonitorLists(Optional ByVal bToRemote As Boolean = True)
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame
        Dim iChanges As Integer

        Cursor.Current = Cursors.WaitCursor

        If bToRemote Then
            RaiseEvent UpdateLog("A sync to the master game list has been triggered.", False, ToolTipIcon.Info, True)
        Else
            RaiseEvent UpdateLog("A sync from the master game list has been triggered.", False, ToolTipIcon.Info, True)
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
            If hshCompareTo.Contains(oFromItem.CompoundKey) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.CompoundKey), clsGame)
                If oFromItem.SyncEquals(oToItem) Then
                    hshSyncItems.Remove(oFromItem.CompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Remote)
            
        Else
            DoListAddUpdateSync(hshSyncItems, mgrSQLite.Database.Local)
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
            If hshCompareFrom.Contains(oToItem.CompoundKey) Then
                oFromItem = DirectCast(hshCompareFrom(oToItem.CompoundKey), clsGame)
                If oToItem.MinimalEquals(oFromItem) Then
                    hshDeleteItems.Remove(oToItem.CompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoListDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        RaiseEvent UpdateLog(hshDeleteItems.Count + hshSyncItems.Count + iChanges & " change(s) synced.", False, ToolTipIcon.Info, True)
        Cursor.Current = Cursors.Default
        Application.DoEvents()
    End Sub

    Private Shared Sub ImportMonitorList(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False)
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim oFromItem As clsGame
        Dim oToItem As clsGame

        Cursor.Current = Cursors.WaitCursor

        'Add / Update Sync
        hshCompareFrom = mgrXML.ReadMonitorList(sLocation, bWebRead)
        hshCompareTo = ReadList(eListTypes.FullList, mgrSQLite.Database.Local)

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.CompoundKey) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.CompoundKey), clsGame)
                If oFromItem.CoreEquals(oToItem) Then
                    hshSyncItems.Remove(oFromItem.CompoundKey)
                End If
            End If
        Next

        Cursor.Current = Cursors.Default

        If hshSyncItems.Count > 0 Then
            Dim frm As New frmAdvancedImport
            frm.ImportData = hshSyncItems
            If frm.ShowDialog() = DialogResult.OK Then
                Cursor.Current = Cursors.WaitCursor

                DoListAddUpdateSync(frm.ImportData)
                mgrTags.DoTagAddImport(frm.ImportData)

                Cursor.Current = Cursors.Default
                MsgBox("Import Complete.", MsgBoxStyle.Information, "Game Backup Monitor")
            End If
        Else
            MsgBox("This list does not contain any new games to import.", MsgBoxStyle.Information, "Game Backup Monitor")
        End If

        Application.DoEvents()
    End Sub

    Public Shared Function DoImport(ByVal sPath As String) As Boolean
        If (sPath.IndexOf("http://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or _
           (sPath.IndexOf("https://", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            If mgrCommon.CheckAddress(sPath) Then
                ImportMonitorList(sPath, True)
                Return True
            Else
                MsgBox("There's no response from:" & vbCrLf & vbCrLf & sPath & vbCrLf & vbCrLf & "Either the server is not responding or the URL is invalid.")
                Return False
            End If
        Else
            If File.Exists(sPath) Then
                ImportMonitorList(sPath)
                Return True
            Else
                MsgBox("The file:" & vbCrLf & sPath & vbCrLf & "cannot be found.")
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function BuildFilterQuery(ByVal oTagFilters As List(Of clsTag), ByVal hshStringFilters As Hashtable, eFilterType As frmFilter.eFilterType, ByRef hshParams As Hashtable) As String
        Dim sSQL As String = String.Empty
        Dim iCounter As Integer = 0

        Select Case eFilterType
            Case frmFilter.eFilterType.NoFilter
                sSQL = "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist ORDER BY Name Asc"
            Case frmFilter.eFilterType.FieldAnd, frmFilter.eFilterType.FieldOr
                sSQL = "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist"

                If hshStringFilters.Count > 0 Then
                    sSQL &= " WHERE ("
                    For Each de As DictionaryEntry In hshStringFilters
                        sSQL &= de.Key & " LIKE @" & de.Key
                        hshParams.Add(de.Key, "%" & de.Value.ToString & "%")
                        iCounter += 1
                        If iCounter <> hshStringFilters.Count Then
                            Select Case eFilterType
                                Case frmFilter.eFilterType.FieldAnd
                                    sSQL &= " AND "
                                Case frmFilter.eFilterType.FieldOr
                                    sSQL &= " OR "
                            End Select
                        End If

                    Next
                    sSQL &= ")"
                End If
                sSQL &= " ORDER BY Name Asc"
            Case frmFilter.eFilterType.AnyTag
                sSQL = "SELECT DISTINCT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist "
                sSQL &= "NATURAL JOIN gametags WHERE gametags.TagID IN ("

                For Each oTag As clsTag In oTagFilters
                    sSQL &= "@TagID" & iCounter & ","
                    hshParams.Add("TagID" & iCounter, oTag.ID)
                    iCounter += 1
                Next

                sSQL = sSQL.TrimEnd(",")
                sSQL &= ") ORDER BY Name Asc"
            Case frmFilter.eFilterType.AllTags
                sSQL = "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist WHERE MonitorID IN "

                For Each oTag As clsTag In oTagFilters
                    sSQL &= "(SELECT MonitorID FROM gametags WHERE monitorlist.MonitorID = gametags.MonitorID And TagID = @TagID" & iCounter & ")"
                    If iCounter <> oTagFilters.Count - 1 Then
                        sSQL &= " AND MonitorID IN "
                    End If
                    hshParams.Add("TagID" & iCounter, oTag.ID)
                    iCounter += 1
                Next

                sSQL &= " ORDER BY Name Asc"
            Case frmFilter.eFilterType.NoTags
                sSQL = "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist WHERE MonitorID NOT IN (SELECT MonitorID FROM gametags) ORDER BY Name Asc"
        End Select

        Return sSQL

    End Function

    Public Shared Function ReadFilteredList(ByVal oTagFilters As List(Of clsTag), ByVal hshStringFilters As Hashtable, eFilterType As frmFilter.eFilterType, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String = String.Empty
        Dim hshList As New Hashtable
        Dim oGame As clsGame
        Dim hshParams As New Hashtable
        Dim iCounter As Integer = 0

        sSQL = BuildFilterQuery(oTagFilters, hshStringFilters, eFilterType, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New clsGame
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

            hshList.Add(oGame.ID, oGame)
        Next

        Return hshList
    End Function

    Public Shared Function ReadListForExport(ByVal oTagFilters As List(Of clsTag), ByVal hshStringFilters As Hashtable, ByVal eFilterType As frmFilter.eFilterType, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As List(Of Game)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String = String.Empty
        Dim sID As String
        Dim oList As New List(Of Game)
        Dim oGame As Game
        Dim hshParams As New Hashtable

        sSQL = BuildFilterQuery(oTagFilters, hshStringFilters, eFilterType, hshParams)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New Game
            sID = CStr(dr("MonitorID"))
            oGame.Name = CStr(dr("Name"))
            oGame.ProcessName = CStr(dr("Process"))
            If Not IsDBNull(dr("Path")) Then oGame.Path = CStr(dr("Path"))
            oGame.AbsolutePath = CBool(dr("AbsolutePath"))
            oGame.FolderSave = CBool(dr("FolderSave"))
            If Not IsDBNull(dr("FileType")) Then oGame.FileType = CStr(dr("FileType"))
            If Not IsDBNull(dr("ExcludeList")) Then oGame.ExcludeList = CStr(dr("ExcludeList"))
            oGame.Tags = mgrGameTags.GetTagsByGameForExport(sID)
            oList.Add(oGame)
        Next

        Return oList
    End Function

    Public Shared Function ReadList(ByVal eListType As eListTypes, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame

        sSQL = "Select * from monitorlist ORDER BY Name Asc"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New clsGame
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

            Select Case eListType
                Case eListTypes.FullList
                    'Don't wrap this, if it fails there's a problem with the database
                    hshList.Add(oGame.ProcessName & ":" & oGame.Name, oGame)
                Case eListTypes.ScanList
                    If hshList.Contains(oGame.ProcessName) Then
                        DirectCast(hshList.Item(oGame.ProcessName), clsGame).Duplicate = True
                        oGame.ProcessName = oGame.ProcessName & ":" & oGame.Name
                        oGame.Duplicate = True
                    End If
                    If oGame.Enabled Then hshList.Add(oGame.ProcessName, oGame)
            End Select
        Next

        Return hshList
    End Function

    Public Shared Sub DoListAdd(ByVal oGame As clsGame, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO monitorlist VALUES (@ID, @Name, @Process, @Path, @AbsolutePath, @FolderSave, @FileType, @TimeStamp, "
        sSQL &= "@ExcludeList, @ProcessPath, @Icon, @Hours, @Version, @Company, @Enabled, @MonitorOnly)"

        'Parameters
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

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoListUpdate(ByVal oGame As clsGame, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE monitorlist SET Name=@Name, Process=@Process, Path=@Path, AbsolutePath=@AbsolutePath, FolderSave=@FolderSave, "
        sSQL &= "FileType=@FileType, TimeStamp=@TimeStamp, ExcludeList=@ExcludeList, ProcessPath=@ProcessPath, Icon=@Icon, "
        sSQL &= "Hours=@Hours, Version=@Version, Company=@Company, Enabled=@Enabled, MonitorOnly=@MonitorOnly WHERE MonitorID=@ID"

        'Parameters
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
        hshParams.Add("ID", oGame.ID)

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

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID = @MonitorID;"
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

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ");"

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
            oGame = New clsGame
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
        Next

        Return oGame
    End Function

    Public Shared Function DoListGetbyName(ByVal sName As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oGame As New clsGame
        Dim hshGames As New Hashtable
        Dim hshParams As New Hashtable
        Dim iCounter As Integer = 0

        sSQL = "SELECT * from monitorlist "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGame = New clsGame
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
            hshGames.Add(iCounter, oGame)
            iCounter += 1
        Next

        Return hshGames
    End Function

    Public Shared Function DoDuplicateListCheck(ByVal sName As String, ByVal sProcess As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM monitorlist WHERE Name = @Name AND Process= @Process"

        hshParams.Add("Name", sName)
        hshParams.Add("Process", sProcess)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND MonitorID <> @MonitorID"
            hshParams.Add("MonitorID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
