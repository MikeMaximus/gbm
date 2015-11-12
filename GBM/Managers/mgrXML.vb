Imports System.Xml
Imports System.IO
Imports System.Text

Public Class mgrXML

    Public Shared Function ReadMonitorList(ByVal sLocation As String, Optional ByVal bWebRead As Boolean = False) As Hashtable
        Dim xFileReader As XmlTextReader
        Dim hshList As New Hashtable
        Dim hshDupeList As New Hashtable
        Dim oGame As clsGame
        Dim oDupeGame As clsGame

        'If the file doesn't exist return an empty list
        If Not File.Exists(sLocation) And Not bWebRead Then
            Return hshList
        End If

        Try
            xFileReader = New XmlTextReader(sLocation)
            xFileReader.WhitespaceHandling = WhitespaceHandling.None

            While (xFileReader.Read())
                If xFileReader.Name = "app" Then
                    oGame = New clsGame
                    oGame.Name = xFileReader.GetAttribute("name")
                    xFileReader.Read()
                    oGame.ProcessName = xFileReader.ReadElementString("process")
                    oGame.AbsolutePath = xFileReader.ReadElementString("absolutepath")
                    oGame.Path = xFileReader.ReadElementString("savelocation")
                    oGame.FolderSave = xFileReader.ReadElementString("foldersave")
                    oGame.FileType = xFileReader.ReadElementString("filetype")                    
                    oGame.ExcludeList = xFileReader.ReadElementString("excludelist")

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
                End If
            End While

            xFileReader.Close()

            'We need to trigger a manual garbage collection here to prevent issues with the reader freezing up with multiple uses.
            'There's no way to properly dispose a xml text reader in .NET 4, that's only fixed in 4.5+.
            GC.Collect()

        Catch ex As Exception
            MsgBox("An error occured reading the monitor list import file." & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Game Backup Monitor")
        End Try

        Return hshList
    End Function

    Public Shared Function ExportMonitorList(ByVal hshList As Hashtable, ByVal sLocation As String) As Boolean
        Dim xFileWriter As XmlTextWriter

        Try
            xFileWriter = New XmlTextWriter(sLocation, System.Text.Encoding.UTF8)
            xFileWriter.Formatting = Formatting.Indented
            xFileWriter.WriteStartDocument()
            xFileWriter.WriteComment("GBM Export: " & Date.Now)
            xFileWriter.WriteComment("Entries: " & hshList.Count)
            xFileWriter.WriteStartElement("aMon")
            For Each o As clsGame In hshList.Values
                xFileWriter.WriteStartElement("app")
                xFileWriter.WriteAttributeString("name", o.Name)
                xFileWriter.WriteElementString("process", o.TrueProcess)
                xFileWriter.WriteElementString("absolutepath", o.AbsolutePath)
                xFileWriter.WriteElementString("savelocation", o.TruePath)
                xFileWriter.WriteElementString("foldersave", o.FolderSave)
                xFileWriter.WriteElementString("filetype", o.FileType)                
                xFileWriter.WriteElementString("excludelist", o.ExcludeList)
                xFileWriter.WriteEndElement()
            Next
            xFileWriter.WriteEndElement()
            xFileWriter.WriteEndDocument()
            xFileWriter.Flush()
            xFileWriter.Close()
            Return True
        Catch ex As Exception
            MsgBox("An error occured exporting the monitor list.  " & ex.Message, MsgBoxStyle.Exclamation, "Game Backup Monitor")
            Return False
        End Try
    End Function

End Class
