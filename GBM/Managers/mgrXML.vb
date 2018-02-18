Imports GBM.My.Resources
Imports System.Xml.Serialization
Imports System.IO
Imports System.Net


Public Class mgrXML

    Public Shared Function ReadMonitorList(ByVal sLocation As String, ByRef oExportInfo As ExportData, Optional ByVal bWebRead As Boolean = False) As Hashtable
        Dim oList As List(Of Game)
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oExportData As ExportData
        Dim oGame As clsGame


        'If the file doesn't exist return an empty list
        If Not File.Exists(sLocation) And Not bWebRead Then
            Return hshList
        End If

        oExportData = ImportandDeserialize(sLocation, bWebRead)
        oList = oExportData.Configurations
        oExportInfo = oExportData

        For Each g As Game In oList
            oGame = New clsGame
            oGame.Name = g.Name
            oGame.ProcessName = g.ProcessName
            oGame.AbsolutePath = g.AbsolutePath
            oGame.Path = g.Path
            oGame.FolderSave = g.FolderSave
            oGame.FileType = g.FileType
            oGame.ExcludeList = g.ExcludeList
            oGame.MonitorOnly = g.MonitorOnly
            oGame.Parameter = g.Parameter
            oGame.Comments = g.Comments
            oGame.IsRegEx = g.IsRegEx
            For Each t As Tag In g.Tags
                oGame.ImportTags.Add(t)
            Next

            'This should be wrapped just in case we get some bad data
            Try
                hshList.Add(oGame.ProcessName & ":" & oGame.SafeName, oGame)
            Catch e As Exception
                'Do Nothing
            End Try
        Next

        Return hshList
    End Function

    Private Shared Function ReadImportData(ByVal sLocation As String, ByVal bWebRead As Boolean)
        Dim oReader As StreamReader
        Dim oWebClient As WebClient

        If bWebRead Then
            oWebClient = New WebClient
            oReader = New StreamReader(oWebClient.OpenRead(sLocation))
        Else
            oReader = New StreamReader(sLocation)
        End If

        Return oReader
    End Function

    Public Shared Function ImportandDeserialize(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False) As ExportData
        Dim oReader As StreamReader
        Dim oSerializer As XmlSerializer
        Dim oExportData As New ExportData

        Try
            oReader = ReadImportData(sLocation, bWebRead)
            oSerializer = New XmlSerializer(GetType(ExportData), New XmlRootAttribute("gbm"))
            oExportData = oSerializer.Deserialize(oReader)
            oReader.Close()

            'Compatability Mode
            If oExportData.AppVer = 0 Then
                oReader = ReadImportData(sLocation, bWebRead)
                oSerializer = New XmlSerializer(GetType(List(Of Game)), New XmlRootAttribute("gbm"))
                oExportData.Configurations = oSerializer.Deserialize(oReader)
                oReader.Close()
            End If

        Catch ex As Exception
            mgrCommon.ShowMessage(mgrXML_ErrorImportFailure, ex.Message, MsgBoxStyle.Exclamation)
        End Try

        Return oExportData
    End Function

    Public Shared Function SerializeAndExport(ByVal oList As List(Of Game), ByVal sLocation As String) As Boolean
        Dim oSerializer As XmlSerializer
        Dim oWriter As StreamWriter
        Dim oExportData As ExportData

        Try
            oExportData = New ExportData(mgrCommon.DateToUnix(Now), oList.Count, mgrCommon.AppVersion, oList)
            oSerializer = New XmlSerializer(oExportData.GetType())
            oWriter = New StreamWriter(sLocation)
            oSerializer.Serialize(oWriter.BaseStream, oExportData)
            oWriter.Flush()
            oWriter.Close()
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrXML_ErrorExportFailure, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function


End Class
