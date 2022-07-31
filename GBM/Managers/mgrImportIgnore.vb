Public Class mgrImportIgnore
    Public Shared Function DoGetIgnoreList() As List(Of String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oList As New List(Of String)
        Dim hshParams As New Hashtable

        sSQL = "SELECT MonitorID FROM importignore;"

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oList.Add(CStr(dr("MonitorID")))
        Next

        Return oList
    End Function

    Public Shared Sub DoIgnoreListAddBatch(ByVal oIgnoreList As List(Of String))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO importignore VALUES (@MonitorID);"

        For Each s As String In oIgnoreList
            hshParams = New Hashtable
            hshParams.Add("MonitorID", s)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoIgnoreListRemoveBatch(ByVal oIgnoreList As List(Of String))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "DELETE FROM importignore WHERE MonitorID = @MonitorID;"

        For Each s As String In oIgnoreList
            hshParams = New Hashtable
            hshParams.Add("MonitorID", s)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoIgnoreListEmpty()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM importignore;"

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub
End Class
