Public Class mgrVariables

    Private Shared Function MapToObject(ByVal dr As DataRow) As clsPathVariable
        Dim oCustomVariable As New clsPathVariable

        oCustomVariable.ID = CStr(dr("VariableID"))
        oCustomVariable.Name = CStr(dr("Name"))
        oCustomVariable.Path = CStr(dr("Path"))

        Return oCustomVariable
    End Function

    Private Shared Function SetCoreParameters(ByVal oCustomVariable As clsPathVariable) As Hashtable
        Dim hshParams As New Hashtable

        hshParams.Add("ID", oCustomVariable.ID)
        hshParams.Add("Name", oCustomVariable.Name)
        hshParams.Add("Path", oCustomVariable.Path)

        Return hshParams
    End Function

    Public Shared Sub DoPathUpdate(ByVal sOld As String, ByVal sNew As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE monitorlist SET Path = replace(Path, @Old, @New) WHERE Path LIKE @Match"
        hshParams.Add("Old", sOld)
        hshParams.Add("New", sNew)
        hshParams.Add("Match", sOld & "%")
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoVariableAdd(ByVal oCustomVariable As clsPathVariable)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "INSERT INTO variables VALUES (@ID, @Name, @Path)"
        hshParams = SetCoreParameters(oCustomVariable)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoVariableUpdate(ByVal oCustomVariable As clsPathVariable)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable

        sSQL = "UPDATE variables SET Name=@Name, Path = @Path "
        sSQL &= "WHERE VariableID = @ID"

        hshParams = SetCoreParameters(oCustomVariable)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoVariableDelete(ByVal sVariableID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM variables "
        sSQL &= "WHERE VariableID = @ID"

        hshParams.Add("ID", sVariableID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoVariableGetbyID(ByVal sVariableID As String) As clsPathVariable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oCustomVariable As New clsPathVariable
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM variables "
        sSQL &= "WHERE VariableID = @ID"

        hshParams.Add("ID", sVariableID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oCustomVariable = MapToObject(dr)
        Next

        Return oCustomVariable
    End Function

    Public Shared Function DoVariableGetbyName(ByVal sVariableName As String) As clsPathVariable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oCustomVariable As New clsPathVariable
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM variables "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sVariableName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oCustomVariable = MapToObject(dr)
        Next

        Return oCustomVariable
    End Function

    Public Shared Function DoCheckDuplicate(ByVal sName As String, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM variables "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sName)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND VariableID <> @VariableID"
            hshParams.Add("VariableID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ReadVariables() As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim oCustomVariable As clsPathVariable

        sSQL = "SELECT * from variables"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oCustomVariable = MapToObject(dr)
            hshList.Add(oCustomVariable.Name, oCustomVariable)
        Next

        Return hshList
    End Function

    Public Shared Function GetReservedVariables() As List(Of String)
        Dim oList As New List(Of String)

        oList.Add("APPDATA")
        oList.Add("LOCALAPPDATA")
        oList.Add("USERDOCUMENTS")
        oList.Add("COMMONDOCUMENTS")
        oList.Add("USERPROFILE")

        Return oList
    End Function

    Public Shared Function CheckForReservedVariables(ByVal sPath As String) As Boolean
        Dim s As String

        For Each s In GetReservedVariables()
            s = "%" & s & "%"
            If sPath.Contains(s) Then
                Return True
            End If
        Next

        Return False
    End Function
End Class
