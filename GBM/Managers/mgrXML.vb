Imports GBM.My.Resources
Imports System.Xml.Serialization
Imports System.IO

Public Class mgrXML

    Public Shared Function DeserializeAndImport(ByVal sLocation As String, ByRef oExportInfo As ExportData, ByRef hshList As Hashtable, Optional ByVal bWebRead As Boolean = False) As Boolean
        Dim oReader As StreamReader
        Dim oSerializer As XmlSerializer
        Dim oList As List(Of Game)
        Dim hshDupeList As New Hashtable
        Dim oExportData As ExportData
        Dim oGame As clsGame

        Try
            Cursor.Current = Cursors.WaitCursor

            'If the file doesn't exist return an empty list
            If Not File.Exists(sLocation) And Not bWebRead Then
                Return False
            End If

            oReader = mgrCommon.ReadTextFromCache(sLocation, bWebRead)
            oSerializer = New XmlSerializer(GetType(ExportData), New XmlRootAttribute("gbm"))
            oExportData = oSerializer.Deserialize(oReader)
            oReader.Close()

            'Compatability Mode
            If oExportData.AppVer = 0 Then
                oReader = mgrCommon.ReadTextFromCache(sLocation, bWebRead)
                oSerializer = New XmlSerializer(GetType(List(Of Game)), New XmlRootAttribute("gbm"))
                oExportData.Configurations = oSerializer.Deserialize(oReader)
                oReader.Close()
            End If

            oList = oExportData.Configurations
            oExportInfo = oExportData

            For Each g As Game In oList
                oGame = New clsGame
                oGame.ID = g.ID
                oGame.Name = g.Name
                oGame.ProcessName = g.ProcessName
                oGame.Path = g.Path
                oGame.FolderSave = g.FolderSave
                oGame.AppendTimeStamp = g.AppendTimeStamp
                oGame.BackupLimit = g.BackupLimit
                oGame.FileType = g.FileType
                oGame.ExcludeList = g.ExcludeList
                oGame.MonitorOnly = g.MonitorOnly
                oGame.Parameter = g.Parameter
                oGame.Comments = g.Comments
                oGame.IsRegEx = g.IsRegEx
                oGame.RecurseSubFolders = g.RecurseSubFolders
                oGame.OS = g.OS
                oGame.UseWindowTitle = g.UseWindowTitle
                oGame.Differential = g.Differential
                oGame.DiffInterval = g.DiffInterval

                'Retain compatability when the OS value is not set
                If oGame.OS = 0 Then
                    oGame.OS = mgrCommon.GetCurrentOS
                End If

                For Each t As Tag In g.Tags
                    oGame.ImportTags.Add(t)
                Next

                For Each c As ConfigLink In g.ConfigLinks
                    oGame.ImportConfigLinks.Add(c)
                Next

                hshList.Add(oGame.ID, oGame)
            Next

            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrXML_ErrorImportFailure, ex.Message, MsgBoxStyle.Exclamation)
            Return False
        Finally
            Cursor.Current = Cursors.Default
        End Try
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
