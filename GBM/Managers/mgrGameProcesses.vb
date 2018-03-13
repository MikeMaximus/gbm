Public Class mgrGameProcesses

    Public Shared Sub DoGameProcessAdd(ByVal oGameProcess As clsGameProcess)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO gameprocesses VALUES (@ProcessID, @MonitorID)"
        hshParams.Add("ProcessID", oGameProcess.ProcessID)
        hshParams.Add("MonitorID", oGameProcess.MonitorID)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoGameProcessAddBatch(ByVal oGameProcesss As List(Of clsGameProcess))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO gameprocesses VALUES (@ProcessID, @MonitorID);"

        For Each oGameProcess As clsGameProcess In oGameProcesss
            hshParams = New Hashtable
            hshParams.Add("ProcessID", oGameProcess.ProcessID)
            hshParams.Add("MonitorID", oGameProcess.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoGameProcessDelete(ByVal oGameProcesss As List(Of clsGameProcess))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM gameprocesses "
        sSQL &= "WHERE ProcessID = @ProcessID AND MonitorID = @MonitorID;"

        For Each oGameProcess As clsGameProcess In oGameProcesss
            hshParams = New Hashtable
            hshParams.Add("ProcessID", oGameProcess.ProcessID)
            hshParams.Add("MonitorID", oGameProcess.MonitorID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoGameProcessDeleteByGame(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gameprocesses "
        sSQL &= "WHERE MonitorID = @ID;"

        hshParams.Add("ID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub


    Public Shared Sub DoGameProcessDeleteByProcess(ByVal sProcessID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gameprocesses "
        sSQL &= "WHERE ProcessID = @ID;"

        hshParams.Add("ID", sProcessID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function GetProcessesByGame(ByVal sMonitorID As String) As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshParams As New Hashtable
        Dim oProcess As clsProcess

        sSQL = "SELECT ProcessID, processes.Name, processes.Path, processes.Args, processes.Kill FROM gameprocesses NATURAL JOIN processes WHERE MonitorID = @ID"

        hshParams.Add("ID", sMonitorID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oProcess = New clsProcess
            oProcess.ID = CStr(dr("ProcessID"))
            oProcess.Name = CStr(dr("Name"))
            oProcess.Path = CStr(dr("Path"))
            If Not IsDBNull(dr("Args")) Then oProcess.Args = CStr(dr("Args"))
            oProcess.Kill = CBool(dr("Kill"))

            hshList.Add(oProcess.ID, oProcess)
        Next

        Return hshList
    End Function

    Public Shared Function GetProcessesByGameMulti(ByVal sMonitorIDs As List(Of String)) As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim hshParams As New Hashtable
        Dim oProcess As clsProcess
        Dim iCounter As Integer

        sSQL = "SELECT DISTINCT ProcessID, processes.Name, processes.Path, processes.Args, processes.Kill FROM gameprocesses NATURAL JOIN processes WHERE MonitorID IN ("

        For Each s As String In sMonitorIDs
            sSQL &= "@MonitorID" & iCounter & ","
            hshParams.Add("MonitorID" & iCounter, s)
            iCounter += 1
        Next

        sSQL = sSQL.TrimEnd(",")
        sSQL &= ")"

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oProcess = New clsProcess
            oProcess.ID = CStr(dr("ProcessID"))
            oProcess.Name = CStr(dr("Name"))
            oProcess.Path = CStr(dr("Path"))
            If Not IsDBNull(dr("Args")) Then oProcess.Args = CStr(dr("Args"))
            oProcess.Kill = CBool(dr("Kill"))
            hshList.Add(oProcess.ID, oProcess)
        Next

        Return hshList
    End Function

    Public Shared Function ReadGameProcesss() As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim sCompoundKey As String
        Dim hshList As New Hashtable
        Dim oGameProcess As clsGameProcess

        sSQL = "SELECT * from gameprocesses"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oGameProcess = New clsGameProcess
            oGameProcess.ProcessID = CStr(dr("ProcessID"))
            oGameProcess.MonitorID = CStr(dr("MonitorID"))
            sCompoundKey = oGameProcess.ProcessID & ":" & oGameProcess.MonitorID
            hshList.Add(sCompoundKey, oGameProcess)
        Next

        Return hshList
    End Function
End Class
