Imports GBM.My.Resources
Imports System.IO
Imports System.Xml.Serialization

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

    Private Shared Function EscapeCSV(ByVal sItem As String) As String
        Dim bEnclose As Boolean = False

        If sItem.Contains("""") Then
            sItem = sItem.Replace("""", """""")
            bEnclose = True
        End If

        If sItem.Contains(",") Then
            bEnclose = True
        End If

        If sItem.Contains(vbCrLf) Or sItem.Contains(vbCr) Or sItem.Contains(vbLf) Then
            bEnclose = True
        End If

        If bEnclose Then
            sItem = """" & sItem & """"
        End If

        Return sItem
    End Function

    Public Shared Function ExportAsCSV(ByVal sLocation As String, ByVal bUnixTime As Boolean, ByVal bHeaders As Boolean, ByRef dg As DataGridView) As Boolean
        Dim oWriter As StreamWriter
        Dim sHeader As String
        Dim sCurrentRow As String
        Dim oBannedColumns As New List(Of DataGridViewColumn)

        Try
            oWriter = New StreamWriter(sLocation)

            'Set Ban Columns
            oBannedColumns.Add(dg.Columns("MonitorID"))

            If bUnixTime Then
                oBannedColumns.Add(dg.Columns("Start"))
                oBannedColumns.Add(dg.Columns("End"))
            Else
                oBannedColumns.Add(dg.Columns("StartUnix"))
                oBannedColumns.Add(dg.Columns("EndUnix"))
            End If

            'Handle Headers
            If bHeaders Then
                sHeader = String.Empty
                For Each dgCol As DataGridViewColumn In dg.Columns
                    If Not oBannedColumns.Contains(dgCol) Then
                        sHeader &= dgCol.HeaderText & ","
                    End If
                Next
                sHeader = sHeader.TrimEnd(",")

                oWriter.WriteLine(sHeader)
            End If

            'Handle Rows
            For Each dgRow As DataGridViewRow In dg.Rows
                sCurrentRow = String.Empty
                For Each dgCell As DataGridViewCell In dgRow.Cells
                    If Not oBannedColumns.Contains(dg.Columns(dgCell.ColumnIndex)) Then
                        sCurrentRow &= EscapeCSV(dgCell.Value.ToString) & ","
                    End If
                Next
                sCurrentRow = sCurrentRow.TrimEnd(",")

                'Don't write a LF on the last row
                If dg.Rows.Count = (dgRow.Index + 1) Then
                    oWriter.Write(sCurrentRow)
                Else
                    oWriter.WriteLine(sCurrentRow)
                End If
            Next

            'Finish up
            oWriter.Flush()
            oWriter.Close()

            mgrCommon.ShowMessage(mgrSessions_ExportSuccess, MsgBoxStyle.Information)
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSessions_ErrorExportFailure, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

    Public Shared Function ExportAsXML(ByVal sLocation As String, ByVal bUnixTime As Boolean, ByRef dg As DataGridView) As Boolean
        Dim oSerializer As XmlSerializer
        Dim oWriter As StreamWriter
        Dim oCurrentSession As Session
        Dim oSessions As New List(Of Session)
        Dim oBannedColumns As New List(Of DataGridViewColumn)

        Try
            'Format data for export
            For Each dgRow As DataGridViewRow In dg.Rows
                oCurrentSession = New Session
                oCurrentSession.GameName = dgRow.Cells("Name").Value.ToString
                If bUnixTime Then
                    oCurrentSession.StartDate = dgRow.Cells("StartUnix").Value.ToString
                    oCurrentSession.EndDate = dgRow.Cells("EndUnix").Value.ToString
                Else
                    oCurrentSession.StartDate = dgRow.Cells("Start").Value.ToString
                    oCurrentSession.EndDate = dgRow.Cells("End").Value.ToString
                End If
                oCurrentSession.Hours = dgRow.Cells("Hours").Value.ToString
                oSessions.Add(oCurrentSession)
            Next

            'Serialize
            oSerializer = New XmlSerializer(oSessions.GetType, New XmlRootAttribute("Sessions"))
            oWriter = New StreamWriter(sLocation)
            oSerializer.Serialize(oWriter.BaseStream, oSessions)

            'Finish up
            oWriter.Flush()
            oWriter.Close()

            mgrCommon.ShowMessage(mgrSessions_ExportSuccess, MsgBoxStyle.Information)
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSessions_ErrorExportFailure, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function

End Class