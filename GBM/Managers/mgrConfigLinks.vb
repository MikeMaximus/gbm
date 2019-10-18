Public Class mgrConfigLinks

    Public Shared Sub DoConfigLinkAdd(ByVal oConfigLink As clsConfigLink, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO configlinks VALUES (@MonitorID, @LinkID)"
        hshParams.Add("MonitorID", oConfigLink.LinkID)
        hshParams.Add("LinkID", oConfigLink.LinkID)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoConfigLinkAddBatch(ByVal oConfigLinks As List(Of clsConfigLink))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO configlinks VALUES (@MonitorID, @LinkID);"

        For Each oConfigLink As clsConfigLink In oConfigLinks
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oConfigLink.MonitorID)
            hshParams.Add("LinkID", oConfigLink.LinkID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoConfigLinkDelete(ByVal oConfigLinks As List(Of clsConfigLink))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM configlinks "
        sSQL &= "WHERE MonitorID = @MonitorID AND LinkID = @LinkID;"

        For Each oConfigLink As clsConfigLink In oConfigLinks
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oConfigLink.MonitorID)
            hshParams.Add("LinkID", oConfigLink.LinkID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoConfigLinkDeleteByID(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM configlinks "
        sSQL &= "WHERE MonitorID = @ID OR LinkID = @ID;"

        hshParams.Add("ID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoConfigLinkImport(ByVal hshConfigLinks As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT OR REPLACE INTO configlinks VALUES (@MonitorID, @LinkID);"
        For Each oGame As clsGame In hshConfigLinks.Values
            For Each c As ConfigLink In oGame.ImportConfigLinks
                hshParams = New Hashtable
                hshParams.Add("MonitorID", oGame.ID)
                hshParams.Add("LinkID", c.ID)
                oParamList.Add(hshParams)
            Next
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Function GetConfigsLinksByID(ByVal sMonitorID As String) As List(Of clsConfigLink)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oList As New List(Of clsConfigLink)
        Dim hshParams As New Hashtable
        Dim oConfigLink As clsConfigLink

        sSQL = "SELECT MonitorID, LinkID FROM configlinks WHERE MonitorID = @ID"

        hshParams.Add("ID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oConfigLink = New clsConfigLink
            oConfigLink.MonitorID = CStr(dr("MonitorID"))
            oConfigLink.LinkID = CStr(dr("LinkID"))
            oList.Add(oConfigLink)
        Next

        Return oList
    End Function

    Public Shared Function GetConfigLinksByGameForExport(ByVal sMonitorID As String) As List(Of ConfigLink)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oList As New List(Of ConfigLink)
        Dim hshParams As New Hashtable
        Dim oConfigLink As ConfigLink

        sSQL = "SELECT LinkID FROM configlinks WHERE MonitorID = @ID"

        hshParams.Add("ID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oConfigLink = New ConfigLink
            oConfigLink.ID = CStr(dr("LinkID"))
            oList.Add(oConfigLink)
        Next

        Return oList
    End Function

    Public Shared Function GetConfigLinksByGameMulti(ByVal sMonitorIDs As List(Of String)) As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshParams As New Hashtable
        Dim iCounter As Integer

        sSQL = "SELECT DISTINCT configlinks.LinkID, monitorlist.Name FROM configlinks INNER JOIN monitorlist ON configlinks.LinkID = monitorlist.MonitorID WHERE configlinks.MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ")"

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            hshList.Add(CStr(dr(0)), CStr(dr(1)))
        Next

        Return hshList
    End Function

    Public Shared Function ReadConfigLinks(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Hashtable
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim sCompoundKey As String
        Dim hshList As New Hashtable
        Dim oConfigLink As clsConfigLink

        sSQL = "SELECT * from configlinks"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oConfigLink = New clsConfigLink
            oConfigLink.MonitorID = CStr(dr("MonitorID"))
            oConfigLink.LinkID = CStr(dr("LinkID"))
            sCompoundKey = oConfigLink.MonitorID & ":" & oConfigLink.LinkID
            hshList.Add(sCompoundKey, oConfigLink)
        Next

        Return hshList
    End Function

    Public Shared Sub DoConfigLinkAddSync(ByVal hshConfigLinks As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO configlinks VALUES (@MonitorID, @LinkID);"

        For Each oConfigLink As clsConfigLink In hshConfigLinks.Values
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oConfigLink.MonitorID)
            hshParams.Add("LinkID", oConfigLink.LinkID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoConfigLinkDeleteSync(ByVal hshConfigLinks As Hashtable, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM configlinks "
        sSQL &= "WHERE MonitorID = @MonitorID AND LinkID = @LinkID;"

        For Each oConfigLink As clsConfigLink In hshConfigLinks.Values
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oConfigLink.MonitorID)
            hshParams.Add("LinkID", oConfigLink.LinkID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Function SyncConfigLinks(Optional ByVal bToRemote As Boolean = True) As Integer
        Dim hshCompareFrom As Hashtable
        Dim hshCompareTo As Hashtable
        Dim hshSyncItems As Hashtable
        Dim hshDeleteItems As Hashtable
        Dim oFromItem As clsConfigLink
        Dim oToItem As clsConfigLink
        Dim sCompoundKey As String

        'Add / Update Sync
        If bToRemote Then
            hshCompareFrom = ReadConfigLinks(mgrSQLite.Database.Local)
            hshCompareTo = ReadConfigLinks(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadConfigLinks(mgrSQLite.Database.Remote)
            hshCompareTo = ReadConfigLinks(mgrSQLite.Database.Local)
        End If

        hshSyncItems = hshCompareFrom.Clone

        For Each oFromItem In hshCompareFrom.Values
            sCompoundKey = oFromItem.MonitorID & ":" & oFromItem.LinkID
            If hshCompareTo.Contains(sCompoundKey) Then
                oToItem = DirectCast(hshCompareTo(sCompoundKey), clsConfigLink)
                If oFromItem.CoreEquals(oToItem) Then
                    hshSyncItems.Remove(sCompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoConfigLinkAddSync(hshSyncItems, mgrSQLite.Database.Remote)
        Else
            DoConfigLinkAddSync(hshSyncItems, mgrSQLite.Database.Local)
        End If

        'Delete Sync
        If bToRemote Then
            hshCompareFrom = ReadConfigLinks(mgrSQLite.Database.Local)
            hshCompareTo = ReadConfigLinks(mgrSQLite.Database.Remote)
        Else
            hshCompareFrom = ReadConfigLinks(mgrSQLite.Database.Remote)
            hshCompareTo = ReadConfigLinks(mgrSQLite.Database.Local)
        End If

        hshDeleteItems = hshCompareTo.Clone

        For Each oToItem In hshCompareTo.Values
            sCompoundKey = oToItem.MonitorID & ":" & oToItem.LinkID
            If hshCompareFrom.Contains(sCompoundKey) Then
                oFromItem = DirectCast(hshCompareFrom(sCompoundKey), clsConfigLink)
                If oToItem.CoreEquals(oFromItem) Then
                    hshDeleteItems.Remove(sCompoundKey)
                End If
            End If
        Next

        If bToRemote Then
            DoConfigLinkDeleteSync(hshDeleteItems, mgrSQLite.Database.Remote)
        Else
            DoConfigLinkDeleteSync(hshDeleteItems, mgrSQLite.Database.Local)
        End If

        Return hshDeleteItems.Count + hshSyncItems.Count

    End Function
End Class
