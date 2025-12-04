Public Class mgrProcess

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsProcess
        Dim oProcess As New clsProcess

        oProcess.ID = CStr(dr("ProcessID"))
        oProcess.Name = CStr(dr("Name"))
        oProcess.Path = CStr(dr("Path"))
        If Not IsDBNull(dr("Args")) Then oProcess.Args = CStr(dr("Args"))
        oProcess.Kill = CBool(dr("Kill"))
        oProcess.Delay = CInt(dr("Delay"))

        Return oProcess
    End Function

    Private Shared Function SetCoreParameters(ByVal oProcess As clsProcess) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("ProcessID", oProcess.ID)
        hshParams.Add("Name", oProcess.Name)
        hshParams.Add("Path", oProcess.Path)
        hshParams.Add("Args", oProcess.Args)
        hshParams.Add("Kill", oProcess.Kill)
        hshParams.Add("Delay", oProcess.Delay)

        Return hshParams
    End Function

    Public Shared Sub DoProcessAdd(ByVal oProcess As clsProcess)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO processes VALUES (@ProcessID, @Name, @Path, @Args, @Kill, @Delay)"
        hshParams = SetCoreParameters(oProcess)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoProcessUpdate(ByVal oProcess As clsProcess)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE processes SET Name=@Name, Path=@Path, Args=@Args, Kill=@Kill, Delay=@Delay "
        sSQL &= "WHERE ProcessID = @ProcessID"

        hshParams = SetCoreParameters(oProcess)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoProcessDelete(ByVal sProcessID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gameprocesses "
        sSQL &= "WHERE ProcessID = @ProcessID;"
        sSQL &= "DELETE FROM processes "
        sSQL &= "WHERE ProcessID = @ProcessID;"

        hshParams.Add("ProcessID", sProcessID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoProcessGetbyID(ByVal sProcessID As String) As clsProcess
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oProcess As New clsProcess
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM processes "
        sSQL &= "WHERE ProcessID = @ProcessID"

        hshParams.Add("ProcessID", sProcessID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oProcess = MapToObject(dr)
        Next

        Return oProcess
    End Function

    Public Shared Function DoProcessGetbyName(ByVal sProcessName As String) As clsProcess
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oProcess As New clsProcess
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM processes "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sProcessName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oProcess = MapToObject(dr)
        Next

        Return oProcess
    End Function

    Public Shared Function DoCheckDuplicate(ByVal sName As String, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM processes "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sName)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND ProcessID <> @ProcessID"
            hshParams.Add("ProcessID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ReadProcesses() As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim oProcess As clsProcess

        sSQL = "SELECT * from processes"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oProcess = MapToObject(dr)
            hshList.Add(oProcess.Name, oProcess)
        Next

        Return hshList
    End Function

End Class