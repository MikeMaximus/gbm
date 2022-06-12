Imports GBM.My.Resources
Imports YamlDotNet.Serialization
Imports System.IO

Public Class mgrLudusavi

    Public Property FileLocation As String
    Public Property Options As clsLudusaviOptions
    Public Property ConvertedList As Hashtable
    Public Property ImportInfo As ExportData

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
    Private Function IsSupportedPath(ByVal sPath As String) As Boolean
        Dim sNotSupported As String() = {"<game>", "<osUserName>", "<winDir>"}

        For Each s As String In sNotSupported
            If sPath.Contains(s) Then Return False
        Next

        Return True
    End Function

    Private Function IsValidProcess(ByVal sProcess As String) As Boolean
        Dim sExt As String() = {".bat", ".sh", ".txt", ".pdf", ".htm", ".html"}
        Dim sName As String() = {"launch", "setup", "start_protected_game", "dowser"}
        Dim s As String

        For Each s In sExt
            If sProcess.ToLower.EndsWith(s) Then
                Return False
            End If
        Next

        For Each s In sName
            If sProcess.ToLower.Contains(s) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function RequiresArguments(ByVal sProcess As String) As Boolean
        Dim sName As String() = {"dosbox", "scummvm", "javaw", "hl2", "portal2"}
        Dim s As String

        For Each s In sName
            If sProcess.ToLower = s Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Function HasStorePath(ByVal sPath As String) As Boolean
        Return sPath.Contains("<root>")
    End Function

    Private Function HasStoreId(ByVal sPath As String) As Boolean
        Return sPath.Contains("<storeUserId>")
    End Function

    'We can try to detect and convert path entries that are file includes to something that GBM can understand.
    Private Function ConvertInclude(ByRef sPath As String, ByRef sInclude As String, ByVal bIsRootedOverride As Boolean) As Boolean
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
    Private Function ConvertConfigurations(ByVal oConfigurations As List(Of clsGame)) As List(Of clsGame)
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
                        For Each t As Tag In oTo.ImportTags
                            If Not oFrom.ImportTags.Contains(t) Then
                                oFrom.ImportTags.Add(t)
                            End If
                        Next
                    End If
                    i += 1
                End If
            Next
            oFinal.Add(oFrom)
        Next

        Return oFinal
    End Function

    'We need to convert ludusavi manifest path variables to ones that GBM can understand
    Private Function ConvertPath(ByVal sPath As String, ByVal oOS As clsGame.eOS, ByVal sStore As String) As String

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

        If Not sStore Is Nothing Then
            Select Case sStore
                'PCGamingWiki doesn't define which SteamID a path contains, we have to make a guess.
                '   - A path containing both the Steam root and Steam ID is generally a Steam Cloud path using SteamID3.
                '   - A path containing only the Steam root is generally an absolute path to a game in the Steam folder.
                '   - A path containing only a Steam ID is generally a file or folder using SteamID64.
                '       - We can't detect SteamID64, but we can strip it or replace it to try and retain a working, if sub-optimal backup configuration.
                Case StoreTypes.steam.ToString
                    If sPath.Contains("<root>") And sPath.Contains("<storeUserId>") Then
                        sPath = sPath.Replace("<root>", "%Steam%")
                        sPath = sPath.Replace("<storeUserId>", "%SteamID3%")
                    ElseIf sPath.Contains("<root>") Then
                        sPath = sPath.Replace("<root>", "%Steam%")
                    ElseIf sPath.Contains("/<storeUserId>") Then
                        sPath = sPath.Substring(0, sPath.IndexOf("/<storeUserId>"))
                    ElseIf sPath.Contains("<storeUserId>.") Then
                        sPath = sPath.Replace("<storeUserId>.", "*.")
                    ElseIf sPath.Contains("<storeUserId>") Then
                        sPath = sPath.Substring(0, sPath.IndexOf("<storeUserId>"))
                        If sPath <> String.Empty Then sPath = sPath.Substring(0, sPath.LastIndexOf("/"))
                    End If
            End Select
        End If

        'Once we reach this point we need to make sure the path still doesn't contain any invalid characters.
        Return mgrPath.ValidatePath(sPath)
    End Function

    'This function will convert store tags used in Ludusavi manifest to tag values that GBM currently uses.
    Private Function ConvertStore(ByVal sStore As String) As String
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

    Private Sub HandleTags(ByRef sTags As String(), ByRef sStore As String, ByRef oGame As clsGame)
        If Not sStore Is Nothing Then
            oGame.ImportTags.Add(New Tag(ConvertStore(sStore)))
        End If

        For Each t As String In sTags
            If t = TagTypes.config.ToString Then
                oGame.ImportTags.Add(New Tag("Configuration"))
            End If
        Next

        If mgrPath.IsSupportedRegistryPath(oGame.Path) Then
            oGame.ImportTags.Add(New Tag("Registry"))
        End If

        oGame.ImportTags.Add(New Tag("Ludusavi"))
    End Sub

    Private Sub HandleLaunch(ByRef oGame As clsGame, ByRef oLudusaviLaunchData As Dictionary(Of String, List(Of LudusaviLaunch)), ByVal sOS As String, ByVal iBit As Integer)
        Dim oLudusaviLaunchPair As KeyValuePair(Of String, List(Of LudusaviLaunch))
        Dim oLudusaviLaunch As LudusaviLaunch
        Dim oLudusaviWhen As LudusaviWhen
        Dim sProcess As String
        Dim sMatch As String(,)
        Dim iPrimaryRank As Integer = 1
        Dim iSecondaryRank As Integer = 1
        Dim oPrimaryRanked As New Dictionary(Of Integer, String(,))
        Dim oSecondaryRanked As New Dictionary(Of Integer, String(,))

        If Not oLudusaviLaunchData Is Nothing Then
            For Each oLudusaviLaunchPair In oLudusaviLaunchData
                For Each oLudusaviLaunch In oLudusaviLaunchPair.Value
                    If Not oLudusaviLaunch.when Is Nothing Then
                        For Each oLudusaviWhen In oLudusaviLaunch.when
                            If oLudusaviWhen.os Is Nothing Or oLudusaviWhen.os = sOS Then
                                sProcess = mgrPath.ValidatePath(oLudusaviLaunchPair.Key.Replace("<base>/", String.Empty))
                                If IsValidProcess(sProcess) Then
                                    If sProcess.ToLower.EndsWith(".exe") Then
                                        sProcess = Path.GetFileNameWithoutExtension(sProcess)
                                    Else
                                        sProcess = Path.GetFileName(sProcess)
                                    End If

                                    ReDim sMatch(0, 1)
                                    sMatch(0, 0) = sProcess

                                    If RequiresArguments(sProcess) Then
                                        If Not oLudusaviLaunch.arguments Is Nothing Then
                                            If oLudusaviWhen.os Is Nothing Or oLudusaviWhen.os = sOS Then
                                                sMatch(0, 1) = oLudusaviLaunch.arguments
                                            End If
                                        End If
                                    End If

                                    If oLudusaviWhen.os = sOS And oLudusaviWhen.bit = iBit Then
                                        oPrimaryRanked.Add(iPrimaryRank, sMatch)
                                        iPrimaryRank += 1
                                    Else
                                        oSecondaryRanked.Add(iSecondaryRank, sMatch)
                                        iSecondaryRank += 1
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Next
        End If

        If oPrimaryRanked.ContainsKey(1) Then
            oGame.ProcessName = oPrimaryRanked(1)(0, 0)
            oGame.Parameter = oPrimaryRanked(1)(0, 1)
        ElseIf oSecondaryRanked.ContainsKey(1) Then
            oGame.ProcessName = oSecondaryRanked(1)(0, 0)
            oGame.Parameter = oSecondaryRanked(1)(0, 1)
        End If
    End Sub

    Private Function DetectSupportedStorePaths() As List(Of String)
        Dim oStores As New List(Of String)

        If mgrStoreVariables.IsAppConfigured(mgrStoreVariables.SupportedAutoConfigApps.Steam) Then
            oStores.Add(StoreTypes.steam.ToString)
        End If

        Return oStores
    End Function

    'This function converts ludusavi manifest data into a structure that can be imported.
    Private Function ConvertYAML(ByRef oList As Dictionary(Of String, LudusaviGame)) As Boolean
        Dim oLudusaviGamePair As KeyValuePair(Of String, LudusaviGame)
        Dim oLudusaviGame As LudusaviGame
        Dim oLudusaviPathPair As KeyValuePair(Of String, LudusaviPath)
        Dim oLudusaviPath As LudusaviPath
        Dim oPlatform As clsGame.eOS = mgrCommon.GetCurrentOS
        Dim iPlatformBit As Integer = 32
        Dim oSupportedStorePaths As List(Of String) = DetectSupportedStorePaths()
        Dim oGame As clsGame
        Dim oConfigurations As New List(Of clsGame)
        Dim bSupportedPlatform As Boolean
        Dim bSupportedStore As Boolean
        Dim bForcedWinConvert As Boolean

        Try
            If Environment.Is64BitOperatingSystem Then
                iPlatformBit = 64
            End If

            For Each oLudusaviGamePair In oList
                If Not Options.QueryAsRegEx Is Nothing Then
                    If Not Options.QueryAsRegEx.IsMatch(oLudusaviGamePair.Key) Then
                        Continue For
                    End If
                End If

                oConfigurations.Clear()
                oLudusaviGame = DirectCast(oLudusaviGamePair.Value, LudusaviGame)

                If Not oLudusaviGame.files Is Nothing Then
                    For Each oLudusaviPathPair In oLudusaviGame.files
                        If IsSupportedPath(oLudusaviPathPair.Key) Then
                            oLudusaviPath = DirectCast(oLudusaviPathPair.Value, LudusaviPath)
                            If Not oLudusaviPath.when Is Nothing Then
                                For Each w As LudusaviWhen In oLudusaviPath.when
                                    bForcedWinConvert = False
                                    If w.os Is Nothing Then w.os = OsTypes.windows.ToString

                                    Select Case oPlatform
                                        Case clsGame.eOS.Windows
                                            Select Case w.os
                                                Case OsTypes.dos.ToString, OsTypes.windows.ToString
                                                    If Options.IncludeOS.HasFlag(clsLudusaviOptions.eSupportedOS.Windows) Then
                                                        bSupportedPlatform = True
                                                    Else
                                                        bSupportedPlatform = False
                                                    End If
                                                Case Else
                                                    bSupportedPlatform = False
                                            End Select
                                        Case clsGame.eOS.Linux
                                            Select Case w.os
                                                Case OsTypes.dos.ToString, OsTypes.windows.ToString
                                                    If Options.IncludeOS.HasFlag(clsLudusaviOptions.eSupportedOS.Windows) Then
                                                        bSupportedPlatform = True
                                                        bForcedWinConvert = True
                                                    Else
                                                        bSupportedPlatform = False
                                                    End If
                                                Case OsTypes.linux.ToString
                                                    If Options.IncludeOS.HasFlag(clsLudusaviOptions.eSupportedOS.Linux) Then
                                                        bSupportedPlatform = True
                                                    Else
                                                        bSupportedPlatform = False
                                                    End If
                                                Case Else
                                                    bSupportedPlatform = False
                                            End Select
                                    End Select

                                    If HasStorePath(oLudusaviPathPair.Key) Or HasStoreId(oLudusaviPathPair.Key) Then
                                        If w.store Is Nothing Then w.store = StoreTypes.steam.ToString

                                        If HasStorePath(oLudusaviPathPair.Key) Then
                                            bForcedWinConvert = False
                                        End If

                                        If oSupportedStorePaths.Contains(w.store) Then
                                            bSupportedStore = True
                                        Else
                                            bSupportedStore = False
                                        End If
                                    Else
                                        bSupportedStore = True
                                    End If

                                    If bSupportedPlatform And bSupportedStore Then
                                        If Not oLudusaviPath.tags Is Nothing Then
                                            For Each t As String In oLudusaviPath.tags
                                                If (t = TagTypes.save.ToString And Options.IncludeSaves) Or (t = TagTypes.config.ToString And Options.IncludeConfigs) Then
                                                    oGame = New clsGame
                                                    oGame.ID = mgrHash.Generate_MD5_GUID(oLudusaviGamePair.Key & oLudusaviPathPair.Key)
                                                    oGame.Name = oLudusaviGamePair.Key

                                                    If bForcedWinConvert Then
                                                        oGame.Path = ConvertPath(oLudusaviPathPair.Key, clsGame.eOS.Windows, w.store)
                                                        oGame.OS = clsGame.eOS.Windows
                                                        oGame.FolderSave = ConvertInclude(oGame.Path, oGame.FileType, True)
                                                    Else
                                                        oGame.Path = ConvertPath(oLudusaviPathPair.Key, oPlatform, w.store)
                                                        oGame.OS = oPlatform
                                                        oGame.FolderSave = ConvertInclude(oGame.Path, oGame.FileType, False)
                                                    End If

                                                    If oGame.OS = clsGame.eOS.Windows Then
                                                        oGame.Path = oGame.Path.Replace("/", "\")
                                                        oGame.FileType = oGame.FileType.Replace("/", "\")
                                                    End If

                                                    HandleTags(oLudusaviPath.tags, w.store, oGame)
                                                    If Not (t = TagTypes.config.ToString And oLudusaviPath.tags.Length = 1) Then
                                                        HandleLaunch(oGame, oLudusaviGame.launch, w.os, iPlatformBit)
                                                    End If

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
                    If Options.IncludeOS.HasFlag(clsLudusaviOptions.eSupportedOS.Windows) Then
                        For Each oLudusaviPathPair In oLudusaviGame.registry
                            If mgrPath.IsSupportedRegistryPath(oLudusaviPathPair.Key.Replace("/", "\")) Then
                                oLudusaviPath = DirectCast(oLudusaviPathPair.Value, LudusaviPath)
                                If Not oLudusaviPath.tags Is Nothing Then
                                    For Each t As String In oLudusaviPath.tags
                                        If (t = TagTypes.save.ToString And Options.IncludeSaves) Or (t = TagTypes.config.ToString And Options.IncludeConfigs) Then
                                            oGame = New clsGame
                                            oGame.ID = mgrHash.Generate_MD5_GUID(oLudusaviGamePair.Key & oLudusaviPathPair.Key)
                                            oGame.Name = oLudusaviGamePair.Key
                                            oGame.Path = oLudusaviPathPair.Key.Replace("/", "\")
                                            oGame.OS = clsGame.eOS.Windows

                                            HandleTags(oLudusaviPath.tags, Nothing, oGame)
                                            If Not (t = TagTypes.config.ToString And oLudusaviPath.tags.Length = 1) Then
                                                HandleLaunch(oGame, oLudusaviGame.launch, OsTypes.windows.ToString, iPlatformBit)
                                            End If
                                            oConfigurations.Add(oGame)
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                End If

                If oConfigurations.Count = 1 Then
                    If Not ConvertedList.ContainsKey(oConfigurations(0).ID) Then ConvertedList.Add(oConfigurations(0).ID, oConfigurations(0))
                ElseIf oConfigurations.Count > 1 Then
                    oConfigurations = ConvertConfigurations(oConfigurations)
                    For Each o As clsGame In oConfigurations
                        If Not ConvertedList.ContainsKey(o.ID) Then ConvertedList.Add(o.ID, o)
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrLudusavi_ErrorConverting, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

    Private Sub FormatYAML(ByRef sYAML As String)
        'DotNetYAML can't properly parse lines that end in <space>*: so we will remove the space.
        sYAML = sYAML.Replace(" *:", "*:")
    End Sub

    Public Function ReadLudusaviManifest() As Boolean
        Dim oBuilder As DeserializerBuilder
        Dim oDeserializer As Deserializer
        Dim oReader As StreamReader
        Dim sYAML As String

        Dim oList As Dictionary(Of String, LudusaviGame)

        Try
            oBuilder = New DeserializerBuilder
            oBuilder.IgnoreUnmatchedProperties()
            oDeserializer = oBuilder.Build()

            If mgrCommon.IsAddress(FileLocation) Then
                oReader = mgrCommon.ReadTextFromCache(FileLocation, ImportInfo.Exported)
                sYAML = oReader.ReadToEnd
                oReader.Close()
            Else
                sYAML = mgrCommon.ReadText(FileLocation)
            End If

            FormatYAML(sYAML)
            oList = oDeserializer.Deserialize(Of Dictionary(Of String, LudusaviGame))(sYAML)
            ConvertYAML(oList)

            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrLudusavi_ErrorReading, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

    End Function

    Sub New(sFileLocation As String, oOptions As clsLudusaviOptions)
        FileLocation = sFileLocation
        Options = oOptions
        ImportInfo = New ExportData
        ConvertedList = New Hashtable
    End Sub
End Class