Public Class mgrManifest

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsBackup
        Dim oBackupItem As clsBackup

        oBackupItem = New clsBackup
        oBackupItem.ManifestID = CStr(dr("ManifestID"))
        oBackupItem.MonitorID = CStr(dr("MonitorID"))
        oBackupItem.Name = CStr(dr("Name"))
        oBackupItem.FileName = CStr(dr("FileName"))
        oBackupItem.RestorePath = CStr(dr("Path"))
        oBackupItem.AbsolutePath = CBool(dr("AbsolutePath"))
        oBackupItem.DateUpdated = mgrCommon.UnixToDate(dr("DateUpdated"))
        oBackupItem.UpdatedBy = CStr(dr("UpdatedBy"))
        If Not IsDBNull(dr("CheckSum")) Then oBackupItem.CheckSum = CStr(dr("CheckSum"))

        Return oBackupItem
    End Function

    Private Shared Function SetCoreParameters(ByVal oBackupItem As clsBackup) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("ManifestID", oBackupItem.ManifestID)
        hshParams.Add("MonitorID", oBackupItem.MonitorID)
        hshParams.Add("FileName", oBackupItem.FileName)
        hshParams.Add("DateUpdated", oBackupItem.DateUpdatedUnix)
        hshParams.Add("UpdatedBy", oBackupItem.UpdatedBy)
        hshParams.Add("CheckSum", oBackupItem.CheckSum)

        Return hshParams
    End Function

    Public Shared Function ReadFullManifest(ByVal iSelectDB As mgrSQLite.Database) As SortedList
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oBackupItem As clsBackup
        Dim slList As New SortedList

        sSQL = "SELECT * from manifest NATURAL JOIN monitorlist ORDER BY Name Asc"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
            slList.Add(oBackupItem.ManifestID, oBackupItem)
        Next

        Return slList

    End Function

    Public Shared Function ReadLatestManifest(ByVal iSelectDB As mgrSQLite.Database) As SortedList
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oBackupItem As clsBackup
        Dim slList As New SortedList

        sSQL = "SELECT ManifestID, MonitorID, Name, FileName, Path, AbsolutePath, Max(DateUpdated) As DateUpdated, UpdatedBy, CheckSum FROM manifest NATURAL JOIN monitorlist GROUP BY Name ORDER By Name ASC"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
            slList.Add(oBackupItem.MonitorID, oBackupItem)
        Next

        Return slList

    End Function

    Public Shared Function DoManifestGetByMonitorID(ByVal sMonitorID As String, ByVal iSelectDB As mgrSQLite.Database) As List(Of clsBackup)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As New clsBackup
        Dim oList As New List(Of clsBackup)


        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID ORDER BY DateUpdated Desc"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
            oList.Add(oBackupItem)
        Next

        Return oList
    End Function

    Public Shared Function DoManifestGetByManifestID(ByVal sManifestID As String, ByVal iSelectDB As mgrSQLite.Database) As clsBackup
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As New clsBackup
        Dim oList As New List(Of clsBackup)

        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE ManifestID = @ManifestID ORDER BY DateUpdated Desc"

        hshParams.Add("ManifestID", sManifestID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
        Next

        Return oBackupItem
    End Function

    'This should only be used to update specific entries in the remote manifest
    Public Shared Function DoSpecificManifestCheck(ByRef oItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID AND FileName = @FileName"

        hshParams.Add("MonitorID", oItem.MonitorID)
        hshParams.Add("FileName", oItem.FileName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In oData.Tables(0).Rows
                oItem.ManifestID = CStr(dr("ManifestID"))
            Next
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function DoManifestCheck(ByVal sMonitorID As String, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM manifest "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Sub DoManifestAdd(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO manifest VALUES (@ManifestID, @MonitorID, @FileName, @DateUpdated, @UpdatedBy, @CheckSum)"

        hshParams = SetCoreParameters(oBackupItem)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestUpdateByMonitorID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE manifest SET MonitorID = @MonitorID, FileName = @FileName, DateUpdated = @DateUpdated, "
        sSQL &= "UpdatedBy = @UpdatedBy, CheckSum = @CheckSum WHERE MonitorID = @QueryID"

        hshParams = SetCoreParameters(oBackupItem)
        hshParams.Add("QueryID", oBackupItem.MonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestUpdateByManifestID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE manifest SET MonitorID = @MonitorID, FileName = @FileName, DateUpdated = @DateUpdated, "
        sSQL &= "UpdatedBy = @UpdatedBy, CheckSum = @CheckSum WHERE ManifestID = @QueryID"

        hshParams = SetCoreParameters(oBackupItem)
        hshParams.Add("QueryID", oBackupItem.ManifestID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestDeleteByMonitorID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM manifest "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", oBackupItem.MonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestDeleteByManifestID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM manifest "
        sSQL &= "WHERE ManifestID = @ManifestID"

        hshParams.Add("ManifestID", oBackupItem.ManifestID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestHashWipe()
        Dim oLocalDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oRemoteDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE manifest SET CheckSum = @CheckSum"

        hshParams.Add("CheckSum", String.Empty)

        oLocalDatabase.RunParamQuery(sSQL, hshParams)
        oRemoteDatabase.RunParamQuery(sSQL, hshParams)
    End Sub
End Class
