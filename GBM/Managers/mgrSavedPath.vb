Public Class mgrSavedPath


    Private Shared Function MapToObject(ByVal dr As DataRow) As clsSavedPath
        Dim oSavedPath As New clsSavedPath

        oSavedPath.PathName = CStr(dr("PathName"))
        oSavedPath.Path = CStr(dr("Path"))

        Return oSavedPath
    End Function

    Private Shared Function SetCoreParameters(ByVal oSavedPath As clsSavedPath) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("PathName", oSavedPath.PathName)
        hshParams.Add("Path", oSavedPath.Path)

        Return hshParams
    End Function

    Public Shared Function GetPathByName(ByVal sPathName As String) As clsSavedPath
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim oSavedPath As New clsSavedPath

        sSQL = "SELECT PathName, Path from savedpath WHERE PathName=@PathName;"

        hshParams.Add("PathName", sPathName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oSavedPath = MapToObject(dr)
        Next

        Return oSavedPath
    End Function

    Public Shared Sub AddUpdatePath(ByVal oSavedPath As clsSavedPath)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT OR REPLACE INTO savedpath (PathName, Path) VALUES (@PathName, @Path);"

        hshParams = SetCoreParameters(oSavedPath)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

End Class
