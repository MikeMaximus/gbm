Public Class mgrBackupQueue
    Public Shared Function DoBackupQueueCount() As Integer
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oCount As Integer
        Dim sSQL As String
        Dim oList As New List(Of String)
        Dim hshParams As New Hashtable

        sSQL = "SELECT COUNT(*) FROM backupqueue;"

        oCount = CInt(oDatabase.ReadSingleValue(sSQL, hshParams))

        Return oCount
    End Function

    Public Shared Function DoGetBackupQueue() As List(Of String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim oList As New List(Of String)
        Dim hshParams As New Hashtable

        sSQL = "SELECT MonitorID FROM backupqueue;"

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oList.Add(CStr(dr("MonitorID")))
        Next

        Return oList
    End Function

    Public Shared Sub DoBackupQueueAddBatch(ByVal oBackupList As List(Of clsGame))
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "INSERT INTO backupqueue VALUES (@MonitorID);"

        For Each oGame As clsGame In oBackupList
            hshParams = New Hashtable
            hshParams.Add("MonitorID", oGame.ID)
            oParamList.Add(hshParams)
        Next

        oDatabase.RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Shared Sub DoBackupQueueDeleteByID(ByVal sMonitorID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM backupqueue WHERE MonitorID = @MonitorID;"

        hshParams.Add("MonitorID", sMonitorID)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoBackupQueueEmpty()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM backupqueue;"

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub
End Class
