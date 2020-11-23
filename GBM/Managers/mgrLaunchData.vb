Public Class mgrLaunchData

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsLaunchData
        Dim oLaunchData As New clsLaunchData

        oLaunchData.MonitorID = CStr(dr("MonitorID"))
        oLaunchData.Path = CStr(dr("Path"))
        oLaunchData.Args = CStr(dr("Args"))
        oLaunchData.NoArgs = CBool(dr("NoArgs"))
        oLaunchData.LauncherID = CStr(dr("LauncherID"))
        oLaunchData.LauncherGameID = CStr(dr("LauncherGameID"))
        Return oLaunchData
    End Function

    Private Shared Function SetCoreParameters(ByVal oLaunchData As clsLaunchData) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("MonitorID", oLaunchData.MonitorID)
        hshParams.Add("Path", oLaunchData.Path)
        hshParams.Add("Args", oLaunchData.Args)
        hshParams.Add("NoArgs", oLaunchData.NoArgs)
        hshParams.Add("LauncherID", oLaunchData.LauncherID)
        hshParams.Add("LauncherGameID", oLaunchData.LauncherGameID)

        Return hshParams
    End Function

    Public Shared Sub DoLaunchDataAddUpdate(ByVal oLaunchData As clsLaunchData)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT OR REPLACE INTO launchdata VALUES (@MonitorID, @Path, @Args, @NoArgs, @LauncherID, @LauncherGameID)"

        'Parameters
        hshParams = SetCoreParameters(oLaunchData)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoLaunchDataDelete(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM launchdata "
        sSQL &= "WHERE MonitorID = @MonitorID;"

        hshParams.Add("MonitorID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoLaunchDataGetbyID(ByVal sMonitorID As String) As clsLaunchData
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oLaunchData As New clsLaunchData
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM launchdata "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oLaunchData = MapToObject(dr)
        Next

        Return oLaunchData
    End Function

End Class
