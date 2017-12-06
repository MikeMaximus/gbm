Public Class mgrSession

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsSession
        Dim oSession As New clsSession

        oSession.MonitorID = CStr(dr("MonitorID"))
        oSession.SessionStart = CStr(dr("Start"))
        oSession.SessionEnd = CStr(dr("End"))

        Return oSession
    End Function

    Private Shared Function SetCoreParameters(ByVal oSession As clsSession) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("MonitorID", oSession.MonitorID)
        hshParams.Add("Start", oSession.SessionStart)
        hshParams.Add("End", oSession.SessionEnd)

        Return hshParams
    End Function

    Public Shared Sub DoSessionAdd(ByVal oSession As clsSession, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO sessions VALUES (@MonitorID, @Start, @End)"

        hshParams = SetCoreParameters(oSession)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

End Class
