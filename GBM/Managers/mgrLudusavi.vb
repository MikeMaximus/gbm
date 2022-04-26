Imports GBM.My.Resources
Imports YamlDotNet.Serialization
Imports System.IO

Public Class mgrLudusavi

    Public Enum OsTypes
        dos
        linux
        mac
        windows
    End Enum

    Public Enum StoreTypes
        discord
        epic
        gog
        microsoft
        origin
        steam
        uplay
    End Enum

    Public Enum TagTypes
        config
        save
    End Enum

    'We can't currently handle some of the path variables used by ludusavi manifest 
    'TODO: Check into implementing support for steam userdata(<root> and <storeUserId>).
    'TODO: Check into supporting <winDir>
    Private Shared Function SupportedPath(ByVal sPath As String) As Boolean
        Dim sNotSupported As String() = {"<root>", "<game>", "<storeUserId>", "<osUserName>", "<winDir>"}

        For Each s As String In sNotSupported
            If sPath.Contains(s) Then Return False
        Next

        Return True
    End Function

    'We can try to detect and convert path entries that are file includes to something that GBM can understand.
    Private Shared Function ConvertInclude(ByRef sPath As String, ByRef sInclude As String, ByVal bIsRootedOverride As Boolean) As Boolean
        Dim bRooted As Boolean
        Dim bHasExt As Boolean = False
        Dim iExtLength As Integer

        Try
            bRooted = Path.IsPathRooted(sPath) Or bIsRootedOverride

            'Only recognize a file extension of 4 characters or less. This will make detecting includes more reliable, especially for Linux configurations.
            If Path.HasExtension(sPath) Then
                iExtLength = Path.GetExtension(sPath).Length - 1
                If iExtLength <= 4 Then
                    bHasExt = True
                End If
            End If

            If (Not bRooted And bHasExt) Or (Not bRooted And (sPath.Contains("*") Or sPath.Contains("?"))) Then
                sInclude = sPath
                sPath = String.Empty
                Return False
            End If

            If (bRooted And bHasExt) Or (bRooted And (sPath.Contains("*") Or sPath.Contains("?"))) Then
                sInclude = Path.GetFileName(sPath)
                sPath = Path.GetDirectoryName(sPath)
                Return False
            End If
        Catch
            'Do Nothing
        End Try

        Return True
    End Function

    'If a game is using a multi-file configuration we can combine them into a single GBM config when it makes sense.
    Private Shared Function ConvertConfigurations(ByVal oConfigurations As List(Of clsGame)) As List(Of clsGame)
        Dim oFinal As New List(Of clsGame)
        Dim oFrom As clsGame
        Dim oTo As clsGame

        For i = 0 To oConfigurations.Count - 1
            oFrom = oConfigurations(i)
            For j = i + 1 To oConfigurations.Count - 1
                oTo = oConfigurations(j)
                If oFrom.Path = oTo.Path Then
                    If oTo.FileType <> String.Empty Then
                        If oFrom.FileType = String.Empty Then
                            oFrom.FileType = oTo.FileType
                        Else
                            oFrom.FileType &= ":" & oTo.FileType
                        End If
                    End If
                    i += 1
                End If
            Next
            oFinal.Add(oFrom)
        Next

        Return oFinal
    End Function

    'We need to convert ludusavi manifest path variables to ones that GBM can understand
    Private Shared Function ConvertPath(ByVal sPath As String, ByVal oOS As clsGame.eOS) As String

        'Replacing <base> with an empty string should make relative locations compatible with GBM
        sPath = sPath.Replace("<base>/", String.Empty)

        Select Case oOS
            Case clsGame.eOS.Windows
                sPath = sPath.Replace("<home>", "%USERPROFILE%")
                sPath = sPath.Replace("<winAppData>", "%APPDATA%")
                sPath = sPath.Replace("<winLocalAppData>", "%LOCALAPPDATA%")
                sPath = sPath.Replace("<winDocuments>", "%USERDOCUMENTS%")
                sPath = sPath.Replace("<winPublic>", "%COMMONDOCUMENTS%")
                sPath = sPath.Replace("<winProgramData>", "%PROGRAMDATA%")
            Case clsGame.eOS.Linux
                sPath = sPath.Replace("<home>", "~")
                sPath = sPath.Replace("<xdgData>", "${XDG_DATA_HOME}")
                sPath = sPath.Replace("<xdgConfig>", "${XDG_CONFIG_HOME}")
        End Select

        'Once we reach this point we need to make sure the path still doesn't contain any invalid characters.
        Return mgrPath.ValidatePath(sPath)
    End Function

    'This function will convert store tags used in Ludusavi manifest to tag values that GBM currently uses.
    Private Shared Function ConvertStore(ByVal sStore As String) As String
        Select Case sStore
            Case StoreTypes.discord.ToString
                Return "Discord"
            Case StoreTypes.epic.ToString
                Return "EGS"
            Case StoreTypes.gog.ToString
                Return "GOG"
            Case StoreTypes.origin.ToString
                Return "Origin"
            Case StoreTypes.uplay.ToString
                Return "Ubisoft Connect"
            Case StoreTypes.microsoft.ToString
                Return "Microsoft Store"
            Case StoreTypes.steam.ToString
                Return "Steam"
            Case Else
                Return "Unknown Store"
        End Select
    End Function

    'This function converts ludusavi manifest data into a structure that can be imported.
    Private Shared Function ConvertYAML(ByRef hshList As Hashtable, ByRef oList As Dictionary(Of String, LudusaviGame)) As Boolean
        Dim oLudusaviGamePair As KeyValuePair(Of String, LudusaviGame)
        Dim oLudusaviGame As LudusaviGame
        Dim oLudusaviPathPair As KeyValuePair(Of String, LudusaviPath)
        Dim oLudusaviPath As LudusaviPath
        Dim oPlatform As clsGame.eOS = mgrCommon.GetCurrentOS
        Dim oGame As clsGame
        Dim oConfigurations As New List(Of clsGame)
        Dim bSupportedPlatform As Boolean
        Dim bForcedWinConvert As Boolean

        Try
            For Each oLudusaviGamePair In oList
                oConfigurations.Clear()
                oLudusaviGame = DirectCast(oLudusaviGamePair.Value, LudusaviGame)

                If Not oLudusaviGame.files Is Nothing Then
                    For Each oLudusaviPathPair In oLudusaviGame.files
                        If SupportedPath(oLudusaviPathPair.Key) Then
                            oLudusaviPath = DirectCast(oLudusaviPathPair.Value, LudusaviPath)
                            If Not oLudusaviPath.when Is Nothing Then
                                For Each w As LudusaviWhen In oLudusaviPath.when
                                    bForcedWinConvert = False
                                    If w.os Is Nothing Then w.os = OsTypes.windows.ToString

                                    Select Case oPlatform
                                        Case clsGame.eOS.Windows
                                            Select Case w.os
                                                Case OsTypes.dos.ToString, OsTypes.windows.ToString
                                                    bSupportedPlatform = True
                                                Case Else
                                                    bSupportedPlatform = False
                                            End Select
                                        Case clsGame.eOS.Linux
                                            Select Case w.os
                                                Case OsTypes.dos.ToString, OsTypes.windows.ToString
                                                    bSupportedPlatform = True
                                                    bForcedWinConvert = True
                                                Case OsTypes.linux.ToString
                                                    bSupportedPlatform = True
                                                Case Else
                                                    bSupportedPlatform = False
                                            End Select
                                    End Select

                                    If bSupportedPlatform Then
                                        If Not oLudusaviPath.tags Is Nothing Then
                                            For Each t As String In oLudusaviPath.tags
                                                If t = TagTypes.save.ToString Then
                                                    oGame = New clsGame
                                                    oGame.ID = mgrHash.Generate_MD5_GUID(oLudusaviGamePair.Key & oLudusaviPathPair.Key)
                                                    oGame.Name = oLudusaviGamePair.Key

                                                    If bForcedWinConvert Then
                                                        oGame.Path = ConvertPath(oLudusaviPathPair.Key, clsGame.eOS.Windows)
                                                        oGame.OS = clsGame.eOS.Windows
                                                        oGame.FolderSave = ConvertInclude(oGame.Path, oGame.FileType, True)
                                                    Else
                                                        oGame.Path = ConvertPath(oLudusaviPathPair.Key, oPlatform)
                                                        oGame.OS = oPlatform
                                                        oGame.FolderSave = ConvertInclude(oGame.Path, oGame.FileType, False)
                                                    End If

                                                    If oGame.OS = clsGame.eOS.Windows Then
                                                        oGame.Path = oGame.Path.Replace("/", "\")
                                                    End If

                                                    If Not w.store Is Nothing Then
                                                        oGame.ImportTags.Add(New Tag(ConvertStore(w.store)))
                                                    End If
                                                    oGame.ImportTags.Add(New Tag("Ludusavi"))
                                                    oConfigurations.Add(oGame)
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If Not oLudusaviGame.registry Is Nothing Then
                    For Each oLudusaviPathPair In oLudusaviGame.registry
                        If mgrPath.IsSupportedRegistryPath(oLudusaviPathPair.Key.Replace("/", "\")) Then
                            oLudusaviPath = DirectCast(oLudusaviPathPair.Value, LudusaviPath)
                            If Not oLudusaviPath.tags Is Nothing Then
                                For Each t As String In oLudusaviPath.tags
                                    If t = TagTypes.save.ToString Then
                                        oGame = New clsGame
                                        oGame.ID = mgrHash.Generate_MD5_GUID(oLudusaviGamePair.Key & oLudusaviPathPair.Key)
                                        oGame.Name = oLudusaviGamePair.Key
                                        oGame.Path = oLudusaviPathPair.Key.Replace("/", "\")
                                        oGame.OS = clsGame.eOS.Windows
                                        oGame.ImportTags.Add(New Tag("Registry"))
                                        oGame.ImportTags.Add(New Tag("Ludusavi"))
                                        oConfigurations.Add(oGame)
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                If oConfigurations.Count = 1 Then
                    If Not hshList.ContainsKey(oConfigurations(0).ID) Then hshList.Add(oConfigurations(0).ID, oConfigurations(0))
                ElseIf oConfigurations.Count > 1 Then
                    oConfigurations = ConvertConfigurations(oConfigurations)
                    For Each o As clsGame In oConfigurations
                        If Not hshList.ContainsKey(o.ID) Then hshList.Add(o.ID, o)
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrLudusavi_ErrorConverting, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

    Private Shared Sub FormatYAML(ByRef sYAML As String)
        'DotNetYAML can't properly parse lines that end in <space>*: so we will remove the space.
        sYAML = sYAML.Replace(" *:", "*:")
    End Sub

    Public Shared Function ReadLudusaviManifest(ByVal sLocation As String, ByRef oExportInfo As ExportData, ByRef hshList As Hashtable, Optional ByVal bWedRead As Boolean = False) As Boolean
        Dim oBuilder As DeserializerBuilder
        Dim oDeserializer As Deserializer
        Dim oReader As StreamReader
        Dim sYAML As String
        Dim oList As Dictionary(Of String, LudusaviGame)


        Try
            oBuilder = New DeserializerBuilder
            oBuilder.IgnoreUnmatchedProperties()
            oDeserializer = oBuilder.Build()

            If bWedRead Then
                oReader = mgrCommon.ReadTextFromCache(sLocation, True, oExportInfo.Exported)
                sYAML = oReader.ReadToEnd
                oReader.Close()
            Else
                sYAML = mgrCommon.ReadText(sLocation)
            End If

            FormatYAML(sYAML)
            oList = oDeserializer.Deserialize(Of Dictionary(Of String, LudusaviGame))(sYAML)
            ConvertYAML(hshList, oList)

            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrLudusavi_ErrorReading, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

    End Function
End Class