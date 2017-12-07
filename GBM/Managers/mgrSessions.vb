Public Class mgrSessions

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsSession
        Dim oSession As New clsSession

        oSession.MonitorID = CStr(dr("MonitorID"))
        oSession.SessionStart = mgrCommon.UnixToDate(CInt(dr("Start")))
        oSession.SessionEnd = mgrCommon.UnixToDate(CInt(dr("End")))

        Return oSession
    End Function

    Private Shared Function SetCoreParameters(ByVal oSession As clsSession) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("MonitorID", oSession.MonitorID)
        hshParams.Add("Start", mgrCommon.DateToUnix(oSession.SessionStart))
        hshParams.Add("End", mgrCommon.DateToUnix(oSession.SessionEnd))

        Return hshParams
    End Function

    Public Shared Sub AddSession(ByVal oSession As clsSession, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO sessions (MonitorID, Start, End) VALUES (@MonitorID, @Start, @End);"

        hshParams = SetCoreParameters(oSession)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Function GetSessions(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DataSet
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT Name, Start, End FROM sessions NATURAL JOIN monitorlist;"

        Return oDatabase.ReadParamData(sSQL, hshParams)
    End Function

    Public Shared Function GetSessionsByGameName(ByVal sGameName As String, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DataSet
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT Name, Start, End FROM sessions NATURAL JOIN monitorlist WHERE monitorlist.Name LIKE @Name;"

        hshParams.Add("Name", "%" & sGameName & "%")

        Return oDatabase.ReadParamData(sSQL, hshParams)
    End Function

End Class