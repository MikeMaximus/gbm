Public Class mgrLaunchers

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsLauncher
        Dim oLauncher As New clsLauncher

        oLauncher.LauncherID = CStr(dr("LauncherID"))
        oLauncher.Name = CStr(dr("Name"))
        oLauncher.LaunchString = CStr(dr("LaunchString"))

        Return oLauncher
    End Function

    Private Shared Function SetCoreParameters(ByVal oLauncher As clsLauncher) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("LauncherID", oLauncher.LauncherID)
        hshParams.Add("Name", oLauncher.Name)
        hshParams.Add("LauncherString", oLauncher.LaunchString)

        Return hshParams
    End Function

    Public Shared Sub DoLauncherAddUpdate(ByVal oLauncher As clsLauncher)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO launchers VALUES (@LauncherID, @Name, @LauncherString)"
        hshParams = SetCoreParameters(oLauncher)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoLauncherUpdate(ByVal oLauncher As clsLauncher)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE launchers SET Name=@Name, LauncherString=@LauncherString "
        sSQL &= "WHERE LauncherID = @LauncherID"

        hshParams = SetCoreParameters(oLauncher)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoLauncherDelete(ByVal sLauncherID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM launchdata "
        sSQL &= "WHERE LauncherID = @LauncherID;"
        sSQL &= "DELETE FROM launchers "
        sSQL &= "WHERE LauncherID = @LauncherID;"

        hshParams.Add("LauncherID", sLauncherID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoLauncherGetbyID(ByVal sLauncherID As String) As clsLauncher
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oLauncher As New clsLauncher
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM launchers "
        sSQL &= "WHERE LauncherID = @LauncherID"

        hshParams.Add("LauncherID", sLauncherID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oLauncher = MapToObject(dr)
        Next

        Return oLauncher
    End Function

    Public Shared Function DoLauncherGetbyName(ByVal sLauncherName As String) As clsLauncher
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oLauncher As New clsLauncher
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM launcher "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sLauncherName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oLauncher = MapToObject(dr)
        Next

        Return oLauncher
    End Function

    Public Shared Function DoCheckDuplicate(ByVal sLauncherName As String, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM launcher "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sLauncherName)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND LauncherID <> @LauncherID"
            hshParams.Add("LauncherID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ReadLaunchers() As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim oLauncher As clsLauncher

        sSQL = "SELECT * from launchers"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oLauncher = MapToObject(dr)
            hshList.Add(oLauncher.Name, oLauncher)
        Next

        Return hshList
    End Function
End Class
