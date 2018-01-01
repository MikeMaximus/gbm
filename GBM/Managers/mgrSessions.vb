Public Class mgrSessions

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsSession
        Dim oSession As New clsSession

        oSession.MonitorID = CStr(dr("MonitorID"))
        oSession.SessionStart = CInt(dr("Start"))
        oSession.SessionEnd = CInt(dr("End"))

        Return oSession
    End Function

    Private Shared Function SetCoreParameters(ByVal oSession As clsSession) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("MonitorID", oSession.MonitorID)
        hshParams.Add("Start", oSession.SessionStart)
        hshParams.Add("End", oSession.SessionEnd)

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

        sSQL = "SELECT sessions.MonitorID, monitorlist.Name, Start, End FROM sessions NATURAL JOIN monitorlist;"

        Return oDatabase.ReadParamData(sSQL, hshParams)
    End Function

    Public Shared Function GetSessionRange(ByVal dtStart As DateTime, ByVal dtEnd As DateTime, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DataSet
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT sessions.MonitorID, monitorlist.Name, Start, End FROM sessions NATURAL JOIN monitorlist WHERE Start >= @Start AND End <= @End;"

        hshParams.Add("Start", mgrCommon.DateToUnix(dtStart))
        hshParams.Add("End", mgrCommon.DateToUnix(dtEnd))

        Return oDatabase.ReadParamData(sSQL, hshParams)
    End Function

    Public Shared Function GetSessionsByGameNameAndRange(ByVal sGameName As String, ByVal dtStart As DateTime, ByVal dtEnd As DateTime, Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DataSet
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "SELECT sessions.MonitorID, monitorlist.Name, Start, End FROM sessions NATURAL JOIN monitorlist WHERE monitorlist.Name LIKE @Name AND (Start >= @Start AND End <= @End);"

        hshParams.Add("Name", "%" & sGameName & "%")
        hshParams.Add("Start", mgrCommon.DateToUnix(dtStart))
        hshParams.Add("End", mgrCommon.DateToUnix(dtEnd))

        Return oDatabase.ReadParamData(sSQL, hshParams)
    End Function

    Public Shared Sub DeleteSession(ByVal oSessions As List(Of clsSession), Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local)
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM sessions WHERE MonitorID = @MonitorID AND Start = @Start;"

        For Each oSession As clsSession In oSessions
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oSession.MonitorID)
            hshParams.Add("Start", oSession.SessionStart)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Function GetMinimumDateTime(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DateTime
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim iUnixDate As Int64

        sSQL = "SELECT Start FROM sessions ORDER BY Start ASC LIMIT 1"

        iUnixDate = CInt(oDatabase.ReadSingleValue(sSQL, hshParams))
        Return mgrCommon.UnixToDate(iUnixDate)
    End Function

    Public Shared Function GetMaximumDateTime(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As DateTime
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim iUnixDate As Int64

        sSQL = "SELECT End FROM sessions ORDER BY Start DESC LIMIT 1"

        iUnixDate = CInt(oDatabase.ReadSingleValue(sSQL, hshParams))
        Return mgrCommon.UnixToDate(iUnixDate)
    End Function

    Public Shared Function CountRows(Optional ByVal iSelectDB As mgrSQLite.Database = mgrSQLite.Database.Local) As Integer
        Dim oDatabase As New mgrSQLite(iSelectDB)
        Dim sSQL As String
        Dim hshParams As New Hashtable
        Dim iRowCount As Integer

        sSQL = "SELECT COUNT(MonitorID) FROM sessions;"

        iRowCount = CInt(oDatabase.ReadSingleValue(sSQL, hshParams))

        Return iRowCount
    End Function
End Class