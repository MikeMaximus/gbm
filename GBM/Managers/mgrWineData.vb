Public Class mgrWineData

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsWineData
        Dim oWineGame As New clsWineData

        oWineGame.MonitorID = CStr(dr("MonitorID"))
        If Not IsDBNull(dr("Prefix")) Then oWineGame.Prefix = CStr(dr("Prefix"))
        If Not IsDBNull(dr("SavePath")) Then oWineGame.Prefix = CStr(dr("SavePath"))
        If Not IsDBNull(dr("BinaryPath")) Then oWineGame.Prefix = CStr(dr("BinaryPath"))

        Return oWineGame
    End Function

    Private Shared Function SetCoreParameters(ByVal oWineGame As clsWineData) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("MonitorID", oWineGame.MonitorID)
        hshParams.Add("Prefix", oWineGame.Prefix)
        hshParams.Add("SavePath", oWineGame.SavePath)
        hshParams.Add("BinaryPath", oWineGame.BinaryPath)

        Return hshParams
    End Function

    Public Shared Sub DoWineDataAddUpdate(ByVal oWineGame As clsWineData)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT OR REPLACE INTO winedata VALUES (@MonitorID, @Prefix, @SavePath, @BinaryPath)"

        'Parameters
        hshParams = SetCoreParameters(oWineGame)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoWineDataDelete(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM winedata "
        sSQL &= "WHERE MonitorID = @MonitorID;"

        hshParams.Add("MonitorID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoWineDataGetbyID(ByVal sMonitorID As String) As clsWineData
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oWineData As New clsWineData
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM winedata "
        sSQL &= "WHERE MonitorID = @MonitorID"

        hshParams.Add("MonitorID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oWineData = MapToObject(dr)
        Next

        Return oWineData
    End Function
End Class
