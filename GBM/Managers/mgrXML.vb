Imports System.Xml.Serialization
Imports System.IO
Imports System.Net


Public Class mgrXML

    Public Shared Function ReadMonitorList(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False) As Hashtable
        Dim oList As List(Of Game)
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame
        Dim oDupeGame As clsGame

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
            For Each t As Tag In g.Tags
                oGame.ImportTags.Add(t)
            Next

            If hshList.Contains(oGame.ProcessName) Or hshDupeList.Contains(oGame.ProcessName) Then
                oDupeGame = DirectCast(hshList.Item(oGame.ProcessName), clsGame)
                If Not hshDupeList.Contains(oGame.ProcessName) Then
                    hshDupeList.Add(oGame.ProcessName, oDupeGame)
                    hshList.Remove(oDupeGame.ProcessName)
                    oDupeGame.Duplicate = True
                    oDupeGame.ProcessName = oDupeGame.ProcessName & ":" & oDupeGame.Name
                    hshList.Add(oDupeGame.ProcessName, oDupeGame)
                End If
                oGame.ProcessName = oGame.ProcessName & ":" & oGame.Name
                oGame.Duplicate = True
            End If

            hshList.Add(oGame.ProcessName, oGame)
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
            MsgBox("The XML file cannot be read, it may be an invalid format or corrupted." & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Game Backup Monitor")
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
            MsgBox("An error occured exporting the XML data." & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Game Backup Monitor")
            Return False
        End Try
    End Function


End Class
