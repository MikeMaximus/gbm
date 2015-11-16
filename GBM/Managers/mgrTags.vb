Public Class mgrTags

    Public Shared Sub DoTagAdd(ByVal oTag As clsTag, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO tags VALUES (@ID, @Name)"
        hshParams.Add("ID", oTag.ID)
        hshParams.Add("Name", oTag.Name)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoTagUpdate(ByVal oTag As clsTag, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE tags SET Name=@Name "
        sSQL &= "WHERE TagID = @ID"

        hshParams.Add("Name", oTag.Name)
        hshParams.Add("ID", oTag.ID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoTagDelete(ByVal sTagID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @ID;"
        sSQL = "DELETE FROM tags "
        sSQL &= "WHERE TagID = @ID;"

        hshParams.Add("ID", sTagID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoTagGetbyID(ByVal sTagID As String) As clsTag
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oTag As New clsTag
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE TagID = @ID"

        hshParams.Add("ID", sTagID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))
        Next

        Return oTag
    End Function

    Public Shared Function DoTagGetbyName(ByVal sTagName As String) As clsTag
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oTag As New clsTag
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sTagName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))            
        Next

        Return oTag
    End Function

    Public Shared Function DoCheckDuplicate(ByVal sTagName As String, Optional ByVal sExcludeID As String = "", Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sTagName)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND TagID <> @TagID"
            hshParams.Add("TagID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ReadTags(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim oTag As clsTag

        sSQL = "SELECT * from tags"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))
            hshList.Add(oTag.Name, oTag)
        Next

        Return hshList
    End Function

    Public Shared Sub DoTagAddImport(ByVal hshTags As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim sMonitorID As String
        Dim oTag As clsTag
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT OR REPLACE INTO tags VALUES (COALESCE((SELECT TagID FROM tags WHERE Name = @Name), @ID), @Name); INSERT INTO gametags VALUES ((SELECT TagID from tags WHERE Name=@Name), @MonitorID);"        
        For Each oGame As clsGame In hshTags.Values
            sMonitorID = oGame.ID
            For Each t As Tag In oGame.ImportTags
                hshParams = New Hashtable
                oTag = New clsTag
                oTag.Name = t.Name
                hshParams.Add("ID", oTag.ID)
                hshParams.Add("Name", oTag.Name)                
                hshParams.Add("MonitorID", sMonitorID)
                oParamList.Add(hshParams)
            Next
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoTagAddUpdateSync(ByVal hshTags As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT OR REPLACE INTO tags VALUES (@ID, @Name);"

        For Each oTag As clsTag In hshTags.Values
            hshParams = New Hashtable
            hshParams.Add("ID", oTag.ID)
            hshParams.Add("Name", oTag.Name)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Private Shared Sub DoTagDeleteSync(ByVal hshTags As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @ID;"
        sSQL = "DELETE FROM tags "
        sSQL &= "WHERE TagID = @ID;"

        For Each oTag As clsTag In hshTags.Values
            hshParams = New Hashtable
            hshParams.Add("ID", oTag.ID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)

    End Sub

    Public Shared Function SyncTags(Optional ByVal bToRemote As Boolean = True) As Integer
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsTag
        Dim oToItem As clsTag

        'Add / Update Sync
        If bToRemote Then
            hshCompareFrom = ReadTags(mgrSQLite.Database.Local)
            hshCompareTo = ReadTags(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadTags(mgrSQLite.Database.Remote)
            hshCompareTo = ReadTags(mgrSQLite.Database.Local)
        End If

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            If hshCompareTo.Contains(oFromItem.Name) Then
                oToItem = DirectCast(hshCompareTo(oFromItem.Name), clsTag)
                If oFromItem.CoreEquals(oToItem) Then
                    hshSyncItems.Remove(oFromItem.Name)
                End If
            End If
        Next

        If bToRemote Then
            DoTagAddUpdateSync(hshSyncItems, mgrSQLite.Database.Remote)
        Else
            DoTagAddUpdateSync(hshSyncItems, mgrSQLite.Database.Local)
        End If

        'Delete Sync
        If bToRemote Then
            hshCompareFrom = ReadTags(mgrSQLite.Database.Local)
            hshCompareTo = ReadTags(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadTags(mgrSQLite.Database.Remote)
            hshCompareTo = ReadTags(mgrSQLite.Database.Local)
        End If

        hshDeleteItems = hshCompareTo.Clone

        For Each oToItem In hshCompareTo.Values
            If hshCompareFrom.Contains(oToItem.Name) Then
                oFromItem = DirectCast(hshCompareFrom(oToItem.Name), clsTag)
                If oToItem.MinimalEquals(oFromItem) Then
                    hshDeleteItems.Remove(oToItem.Name)
                End If
            End If
        Next

        If bToRemote Then
            DoTagDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoTagDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        Return hshDeleteItems.Count + hshSyncItems.Count

    End Function

End Class
