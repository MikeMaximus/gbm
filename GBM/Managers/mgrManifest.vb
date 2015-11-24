Imports System.IO

Public Class mgrManifest

    Public Shared Function ReadManifest(ByVal iSelectDB As mgrSQLite.Database) As SortedList
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oBackupItem As clsBackup
        Dim slList As New SortedList

        sSQL = "SELECT * from manifest ORDER BY Name Asc"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = New clsBackup
            oBackupItem.ID = CStr(dr("ManifestID"))
            oBackupItem.Name = CStr(dr("Name"))
            oBackupItem.FileName = CStr(dr("FileName"))
            oBackupItem.RestorePath = CStr(dr("RestorePath"))
            oBackupItem.AbsolutePath = CBool(dr("AbsolutePath"))
            oBackupItem.DateUpdated = mgrCommon.UnixToDate(dr("DateUpdated"))
            oBackupItem.UpdatedBy = CStr(dr("UpdatedBy"))
            If Not IsDBNull(dr("CheckSum")) Then oBackupItem.CheckSum = CStr(dr("CheckSum"))
            slList.Add(oBackupItem.Name, oBackupItem)
        Next

        Return slList

    End Function

    Public Shared Function DoManifestCheck(ByVal sName As String, ByVal iSelectDB As mgrSQLite.Database) As Boolean
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT * from manifest "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function DoManifestGetByName(ByVal sName As String, ByVal iSelectDB As mgrSQLite.Database) As clsBackup
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oBackupItem As New clsBackup

        sSQL = "SELECT * from manifest "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oBackupItem = New clsBackup
            oBackupItem.ID = CStr(dr("ManifestID"))
            oBackupItem.Name = CStr(dr("Name"))
            oBackupItem.FileName = CStr(dr("FileName"))
            oBackupItem.RestorePath = CStr(dr("RestorePath"))
            oBackupItem.AbsolutePath = CBool(dr("AbsolutePath"))
            oBackupItem.DateUpdated = mgrCommon.UnixToDate(dr("DateUpdated"))
            oBackupItem.UpdatedBy = CStr(dr("UpdatedBy"))
            If Not IsDBNull(dr("CheckSum")) Then oBackupItem.CheckSum = CStr(dr("CheckSum"))
        Next

        Return oBackupItem
    End Function

    Public Shared Sub DoManifestAdd(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO manifest VALUES (@ID, @Name, @FileName, @Path, @AbsolutePath, @DateUpdated, @UpdatedBy, @CheckSum)"

        hshParams.Add("ID", oBackupItem.ID)
        hshParams.Add("Name", oBackupItem.Name)
        hshParams.Add("FileName", oBackupItem.FileName)
        hshParams.Add("Path", oBackupItem.TruePath)
        hshParams.Add("AbsolutePath", oBackupItem.AbsolutePath)
        hshParams.Add("DateUpdated", oBackupItem.DateUpdatedUnix)
        hshParams.Add("UpdatedBy", oBackupItem.UpdatedBy)
        hshParams.Add("CheckSum", oBackupItem.CheckSum)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestUpdate(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE manifest SET Name = @Name, FileName = @FileName, RestorePath = @Path, AbsolutePath = @AbsolutePath, "
        sSQL &= "DateUpdated = @DateUpdated, UpdatedBy = @UpdatedBy, CheckSum = @CheckSum WHERE Name = @QueryName"

        hshParams.Add("Name", oBackupItem.Name)
        hshParams.Add("FileName", oBackupItem.FileName)
        hshParams.Add("Path", oBackupItem.TruePath)
        hshParams.Add("AbsolutePath", oBackupItem.AbsolutePath)
        hshParams.Add("DateUpdated", oBackupItem.DateUpdatedUnix)
        hshParams.Add("UpdatedBy", oBackupItem.UpdatedBy)
        hshParams.Add("CheckSum", oBackupItem.CheckSum)
        hshParams.Add("QueryName", oBackupItem.Name)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoManifestNameUpdate(ByVal sOriginalName As String, ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE manifest SET Name = @Name, FileName = @FileName, RestorePath = @Path, AbsolutePath = @AbsolutePath, "
        sSQL &= "DateUpdated = @DateUpdated, UpdatedBy = @UpdatedBy, CheckSum = @CheckSum WHERE Name = @QueryName"

        hshParams.Add("Name", oBackupItem.Name)
        hshParams.Add("FileName", oBackupItem.FileName)
        hshParams.Add("Path", oBackupItem.TruePath)
        hshParams.Add("AbsolutePath", oBackupItem.AbsolutePath)
        hshParams.Add("DateUpdated", oBackupItem.DateUpdatedUnix)
        hshParams.Add("UpdatedBy", oBackupItem.UpdatedBy)
        hshParams.Add("CheckSum", oBackupItem.CheckSum)
        hshParams.Add("QueryName", sOriginalName)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoManifestDelete(ByVal oBackupItem As clsBackup, ByVal iSelectDB As mgrSQLite.Database)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM manifest "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", oBackupItem.Name)

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
