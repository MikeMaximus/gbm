Imports System.Xml.Serialization
Imports System.IO

Public Class mgrMetadata

    Public Shared Function SerializeAndExport(ByVal sLocation As String, ByVal oGame As clsGame, ByVal sUpdatedBy As String, ByVal dTimeStamp As Date) As Boolean
        Dim oSerializer As XmlSerializer
        Dim oWriter As StreamWriter
        Dim oBackupMetadata As BackupMetadata

        Try
            oBackupMetadata = New BackupMetadata(mgrCommon.AppVersion, mgrCommon.DateToUnix(dTimeStamp), sUpdatedBy, oGame.ConvertToXMLGame)
            oSerializer = New XmlSerializer(oBackupMetadata.GetType())
            oWriter = New StreamWriter(sLocation)
            oSerializer.Serialize(oWriter.BaseStream, oBackupMetadata)
            oWriter.Flush()
            oWriter.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
