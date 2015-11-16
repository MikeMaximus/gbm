Public Class mgrGameTags

    Public Shared Sub DoGameTagAdd(ByVal oGameTag As clsGameTag, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO gametags VALUES (@TagID, @MonitorID)"
        hshParams.Add("TagID", oGameTag.TagID)
        hshParams.Add("MonitorID", oGameTag.MonitorID)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoGameTagAddBatch(ByVal oGameTags As List(Of clsGameTag))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO gametags VALUES (@TagID, @MonitorID);"

        For Each oGameTag As clsGameTag In oGameTags
            hshParams = New Hashtable
            hshParams.Add("TagID", oGameTag.TagID)
            hshParams.Add("MonitorID", oGameTag.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoGameTagDelete(ByVal oGameTags As List(Of clsGameTag))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @TagID AND MonitorID = @MonitorID;"

        For Each oGameTag As clsGameTag In oGameTags
            hshParams = New Hashtable
            hshParams.Add("TagID", oGameTag.TagID)
            hshParams.Add("MonitorID", oGameTag.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoGameTagDeleteByGame(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE MonitorID = @ID;"

        hshParams.Add("ID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    
    Public Shared Sub DoGameTagDeleteByTag(ByVal sTagID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @ID;"

        hshParams.Add("ID", sTagID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function GetTagsByGame(ByVal sMonitorID As String) As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshParams As New Hashtable
        Dim oTag As clsTag

        sSQL = "SELECT TagID, tags.Name FROM gametags NATURAL JOIN tags WHERE MonitorID = @ID"

        hshParams.Add("ID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))
            hshList.Add(oTag.Name, oTag)
        Next

        Return hshList
    End Function

    Public Shared Function GetTagsByGameForExport(ByVal sMonitorID As String) As List(Of Tag)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oList As New List(Of Tag)
        Dim hshParams As New Hashtable
        Dim oTag As Tag

        sSQL = "SELECT TagID, tags.Name FROM gametags NATURAL JOIN tags WHERE MonitorID = @ID"

        hshParams.Add("ID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New Tag            
            oTag.Name = CStr(dr(1))
            oList.Add(oTag)
        Next

        Return oList
    End Function

    Public Shared Function GetTagsByGameMulti(ByVal sMonitorIDs As List(Of String)) As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshParams As New Hashtable
        Dim oTag As clsTag
        Dim iCounter As Integer

        sSQL = "SELECT DISTINCT TagID, tags.Name FROM gametags NATURAL JOIN tags WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ")"

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))
            hshList.Add(oTag.Name, oTag)
        Next

        Return hshList
    End Function

    Public Shared Function ReadGameTags(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim sCompoundKey As String
        Dim hshList As New Hashtable
        Dim oGameTag As clsGameTag

        sSQL = "SELECT * from gametags"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGameTag = New clsGameTag
            oGameTag.TagID = CStr(dr(0))
            oGameTag.MonitorID = CStr(dr(1))
            sCompoundKey = oGameTag.TagID & ":" & oGameTag.MonitorID
            hshList.Add(sCompoundKey, oGameTag)
        Next

        Return hshList
    End Function

    Public Shared Sub DoGameTagAddSync(ByVal hshTags As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO gametags VALUES (@TagID, @MonitorID);"

        For Each oGameTag As clsGameTag In hshTags.Values
            hshParams = New Hashtable
            hshParams.Add("TagID", oGameTag.TagID)
            hshParams.Add("MonitorID", oGameTag.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoGameTagDeleteSync(ByVal hshTags As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @TagID AND MonitorID = @MonitorID;"

        For Each oGameTag As clsGameTag In hshTags.Values
            hshParams = New Hashtable
            hshParams.Add("TagID", oGameTag.TagID)
            hshParams.Add("MonitorID", oGameTag.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Function SyncGameTags(Optional ByVal bToRemote As Boolean = True) As Integer
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsGameTag
        Dim oToItem As clsGameTag
        Dim sCompoundKey As String

        'Add / Update Sync
        If bToRemote Then
            hshCompareFrom = ReadGameTags(mgrSQLite.Database.Local)
            hshCompareTo = ReadGameTags(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadGameTags(mgrSQLite.Database.Remote)
            hshCompareTo = ReadGameTags(mgrSQLite.Database.Local)
        End If

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            sCompoundKey = oFromItem.TagID & ":" & oFromItem.MonitorID
            If hshCompareTo.Contains(sCompoundKey) Then
                oToItem = DirectCast(hshCompareTo(sCompoundKey), clsGameTag)
                If oFromItem.CoreEquals(oToItem) Then
                    hshSyncItems.Remove(sCompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoGameTagAddSync(hshSyncItems, mgrSQLite.Database.Remote)
        Else
            DoGameTagAddSync(hshSyncItems, mgrSQLite.Database.Local)
        End If

        'Delete Sync
        If bToRemote Then
            hshCompareFrom = ReadGameTags(mgrSQLite.Database.Local)
            hshCompareTo = ReadGameTags(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadGameTags(mgrSQLite.Database.Remote)
            hshCompareTo = ReadGameTags(mgrSQLite.Database.Local)
        End If

        hshDeleteItems = hshCompareTo.Clone

        For Each oToItem In hshCompareTo.Values
            sCompoundKey = oToItem.TagID & ":" & oToItem.MonitorID
            If hshCompareFrom.Contains(sCompoundKey) Then
                oFromItem = DirectCast(hshCompareFrom(sCompoundKey), clsGameTag)
                If oToItem.CoreEquals(oFromItem) Then
                    hshDeleteItems.Remove(sCompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoGameTagDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoGameTagDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        Return hshDeleteItems.Count + hshSyncItems.Count

    End Function

End Class
