Public Class mgrManifest

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsBackup
        Dim oBackupItem As clsBackup

        'Inherited
        oBackupItem = mgrMonitorList.MapToObject(dr, mgrMonitorList.eSupportedClasses.clsBackup)

        'Base
        oBackupItem.ManifestID = CStr(dr("ManifestID"))
        oBackupItem.MonitorID = CStr(dr("MonitorID"))
        oBackupItem.FileName = CStr(dr("FileName"))
        oBackupItem.DateUpdated = mgrCommon.UnixToDate(dr("DateUpdated"))
        oBackupItem.UpdatedBy = CStr(dr("UpdatedBy"))
        If Not IsDBNull(dr("CheckSum")) Then oBackupItem.CheckSum = CStr(dr("CheckSum"))
        oBackupItem.IsDifferentialParent = CBool(dr("IsDifferentialParent"))
        oBackupItem.DifferentialParent = CStr(dr("DifferentialParent"))

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
        hshParams.Add("IsDifferentialParent", oBackupItem.IsDifferentialParent)
        hshParams.Add("DifferentialParent", oBackupItem.DifferentialParent)

        Return hshParams
    End Function

    Public Shared Function ReadFullManifest(ByVal iSelectDB As mgrSQLite.Database) As SortedList
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oBackupItem As clsBackup
        Dim slList As New SortedList

        sSQL = "SELECT * from manifest NATURAL JOIN monitorlist ORDER BY Name ASC"
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

        sSQL = "SELECT *, Max(DateUpdated) As DateUpdated FROM manifest NATURAL JOIN monitorlist GROUP BY Name ORDER BY Name ASC"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
            slList.Add(oBackupItem.MonitorID, oBackupItem)
        Next

        Return slList

    End Function

    Public Shared Function DoManifestGetByMonitorID(ByVal sMonitorID As String, ByVal iSelectDB As mgrSQLite.Database, Optional ByVal bExcludeDiffParents As Boolean = False) As List(Of clsBackup)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As New clsBackup
        Dim oList As New List(Of clsBackup)


        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID"
        If bExcludeDiffParents Then
            sSQL &= " AND IsDifferentialParent = 0"
        End If
        sSQL &= " ORDER BY DateUpdated DESC"

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

        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE ManifestID = @ManifestID ORDER BY DateUpdated DESC"

        hshParams.Add("ManifestID", sManifestID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
        Next

        Return oBackupItem
    End Function

    Public Shared Function DoManfiestGetDifferentialParent(ByVal sMonitorID As String, ByVal iSelectDB As mgrSQLite.Database) As clsBackup
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As clsBackup = Nothing

        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID AND IsDifferentialParent = 1 ORDER BY DateUpdated DESC LIMIT 1"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
        Next

        Return oBackupItem
    End Function

    Public Shared Function DoManfiestGetDifferentialChildren(ByRef oItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database) As List(Of clsBackup)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As New clsBackup
        Dim oList As New List(Of clsBackup)

        sSQL = "SELECT * FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE DifferentialParent = @DifferentialParent ORDER BY DateUpdated DESC"

        hshParams.Add("DifferentialParent", oItem.ManifestID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = MapToObject(dr)
            oList.Add(oBackupItem)
        Next

        Return oList
    End Function

    Public Shared Function DoUpdateLatestManifest(ByRef oItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As Object
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT ManifestID FROM manifest NATURAL JOIN monitorlist "
        sSQL &= "WHERE MonitorID = @MonitorID ORDER BY DateUpdated DESC LIMIT 1"

        hshParams.Add("MonitorID", oItem.MonitorID)

        oData = oDatabase.ReadSingleValue(sSQL, hshParams)

        If Not oData Is Nothing Then
            oItem.ManifestID = CStr(oData)
            DoManifestUpdateByManifestID(oItem, mgrSQLite.Database.Remote)
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

    Public Shared Function DoManifestParentCheck(ByVal sMonitorID As String, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM manifest "
        sSQL &= "WHERE MonitorID = @MonitorID AND IsDifferentialParent = 1"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function DoManifestDuplicateCheck(ByVal oItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As Object
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT ManifestID FROM manifest "
        sSQL &= "WHERE MonitorID = @MonitorID AND FileName = @FileName AND DateUpdated = @DateUpdated AND UpdatedBy = @UpdatedBy AND CheckSum=@CheckSum AND "
        sSQL &= "IsDifferentialParent = @IsDifferentialParent AND DifferentialParent=@DifferentialParent LIMIT 1;"

        hshParams.Add("MonitorID", oItem.MonitorID)
        hshParams.Add("FileName", oItem.FileName)
        hshParams.Add("DateUpdated", oItem.DateUpdatedUnix)
        hshParams.Add("UpdatedBy", oItem.UpdatedBy)
        hshParams.Add("CheckSum", oItem.CheckSum)
        hshParams.Add("IsDifferentialParent", oItem.IsDifferentialParent)
        hshParams.Add("DifferentialParent", oItem.DifferentialParent)

        oData = oDatabase.ReadSingleValue(sSQL, hshParams)

        If Not oData Is Nothing Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Sub DoManifestAdd(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO manifest VALUES (@ManifestID, @MonitorID, @FileName, @DateUpdated, @UpdatedBy, @CheckSum, @IsDifferentialParent, @DifferentialParent)"

        hshParams = SetCoreParameters(oBackupItem)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestUpdateByMonitorID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE manifest SET MonitorID = @MonitorID, FileName = @FileName, DateUpdated = @DateUpdated, "
        sSQL &= "UpdatedBy = @UpdatedBy, CheckSum = @CheckSum, IsDifferentialParent = @IsDifferentialParent, DifferentialParent = @DifferentialParent WHERE MonitorID = @QueryID"

        hshParams = SetCoreParameters(oBackupItem)
        hshParams.Add("QueryID", oBackupItem.MonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestUpdateByManifestID(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE manifest SET MonitorID = @MonitorID, FileName = @FileName, DateUpdated = @DateUpdated, "
        sSQL &= "UpdatedBy = @UpdatedBy, CheckSum = @CheckSum, IsDifferentialParent = @IsDifferentialParent, DifferentialParent = @DifferentialParent WHERE ManifestID = @QueryID"

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

        sSQL = "DELETE FROM manifest WHERE ManifestID = @ManifestID;"

        If oBackupItem.IsDifferentialParent Then
            sSQL &= "DELETE FROM manifest WHERE DifferentialParent = @ManifestID;"
        End If

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
