Public Class mgrTags

    Public Shared Sub DoTagAdd(ByVal oTag As clsTag)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT INTO tags VALUES (@ID, @Name)"
        hshParams.Add("ID", oTag.ID)
        hshParams.Add("Name", oTag.Name)
        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Public Shared Sub DoTagUpdate(ByVal oTag As clsTag)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "UPDATE tags SET Name=@Name "
        sSQL &= "WHERE TagID = @ID"

        hshParams.Add("Name", oTag.Name)
        hshParams.Add("ID", oTag.ID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Sub DoTagDelete(ByVal sTagID As String)
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "DELETE FROM gametags "
        sSQL &= "WHERE TagID = @ID;"
        sSQL = "DELETE FROM tags "
        sSQL &= "WHERE TagID = @ID;"

        hshParams.Add("ID", sTagID)

        oDatabase.RunParamQuery(sSQL, hshParams)

    End Sub

    Public Shared Function DoTagGetbyID(ByVal sTagID As String) As clsTag
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oTag As New clsTag
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE TagID = @ID"

        hshParams.Add("ID", sTagID)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))            
        Next

        Return oTag
    End Function

    Public Shared Function DoTagGetbyName(ByVal sTagName As String) As clsTag
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim oTag As New clsTag
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sTagName)

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))            
        Next

        Return oTag
    End Function

    Public Shared Function DoCheckDuplicate(ByVal sTagName As String, Optional ByVal sExcludeID As String = "") As Boolean
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM tags "
        sSQL &= "WHERE Name = @Name"

        hshParams.Add("Name", sTagName)

        If sExcludeID <> String.Empty Then
            sSQL &= " AND TagID <> @TagID"
            hshParams.Add("TagID", sExcludeID)
        End If

        oData = oDatabase.ReadParamData(sSQL, hshParams)

        If oData.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function ReadTags() As Hashtable
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String
        Dim hshList As New Hashtable
        Dim oTag As clsTag

        sSQL = "SELECT * from tags"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            oTag = New clsTag
            oTag.ID = CStr(dr(0))
            oTag.Name = CStr(dr(1))            
            hshList.Add(oTag.Name, oTag)
        Next

        Return hshList
    End Function

End Class
