Imports GBM.My.Resources
Imports System.Xml.Serialization
Imports System.IO
Imports System.Net


Public Class mgrXML

    Public Shared Function ReadMonitorList(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False) As Hashtable
        Dim oList As List(Of Game)
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame

        'If the file doesn't exist return an empty list
        If Not File.Exists(sLocation) And Not bWebRead Then
            Return hshList
        End If

        oList = ImportandDeserialize(sLocation, bWebRead)

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
            For Each t As Tag In g.Tags
                oGame.ImportTags.Add(t)
            Next

            'This should be wrapped just in case we get some bad data
            Try
                hshList.Add(oGame.ProcessName & ":" & oGame.Name, oGame)
            Catch e As Exception
                'Do Nothing
            End Try
        Next

        Return hshList
    End Function

    Public Shared Function ImportandDeserialize(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False) As List(Of Game)
        Dim oReader As StreamReader
        Dim oWebClient As WebClient
        Dim oSerializer As XmlSerializer
        Dim oList As New List(Of Game)

        Try
            If bWebRead Then
                oWebClient = New WebClient
                oReader = New StreamReader(oWebClient.OpenRead(sLocation))
            Else
                oReader = New StreamReader(sLocation)
            End If

            oSerializer = New XmlSerializer(oList.GetType(), New XmlRootAttribute("gbm"))
            oList = oSerializer.Deserialize(oReader)
            oReader.Close()
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrXML_ErrorImportFailure, ex.Message, MsgBoxStyle.Exclamation)
        End Try

        Return oList
    End Function

    Public Shared Function SerializeAndExport(ByVal oList As List(Of Game), ByVal sLocation As String) As Boolean
        Dim oSerializer As XmlSerializer
        Dim oWriter As StreamWriter

        Try
            oSerializer = New XmlSerializer(oList.GetType(), New XmlRootAttribute("gbm"))
            oWriter = New StreamWriter(sLocation)
            oSerializer.Serialize(oWriter.BaseStream, oList)
            oWriter.Flush()
            oWriter.Close()
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrXML_ErrorExportFailure, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        End Try
    End Function


End Class
