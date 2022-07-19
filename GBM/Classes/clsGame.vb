Imports System.Xml.Serialization
Imports System.IO.Path
Imports System.Text.RegularExpressions

<Serializable()>
Public Class clsGame
    <Flags()> Public Enum eOptionalSyncFields
        None = 0
        GamePath = 1
        Company = 2
        Version = 4
        Icon = 16
        Unused = 32 'Do not remove to maintain compatability, re-use for a future field.
        MonitorGame = 64
    End Enum

    Public Enum eOS
        <XmlEnum("1")>
        Windows = 1
        <XmlEnum("2")>
        Linux = 2
    End Enum

    Private sPath As String
    Private sProcessPath As String
    Private sIcon As String

    Public Property ID As String
    Public Property Name As String
    Public ReadOnly Property CroppedName As String
        Get
            If Name.Length > 40 Then
                Return Name.Substring(0, 41).Trim & "..."
            Else
                Return Name
            End If
        End Get
    End Property
    Public ReadOnly Property FileSafeName As String
        Get
            If mgrMonitorList.IsDuplicateName(Name) Then
                Return mgrPath.ValidateFileName(Name, 64) & " [" & mgrPath.ValidateFileName(ID, 36) & "]"
            Else
                Return mgrPath.ValidateFileName(Name, 64)
            End If
        End Get
    End Property
    Public Property ProcessName As String
    Public Property Parameter As String
    Public Property Path As String
        Set(value As String)
            sPath = mgrPath.ReverseSpecialPaths(value)
        End Set
        Get
            Return mgrPath.ReplaceSpecialPaths(sPath)
        End Get
    End Property
    Public ReadOnly Property AbsolutePath As Boolean
        Get
            'We need to handle a special case here when working with Windows configurations in Linux
            If mgrCommon.IsUnix And mgrVariables.CheckForReservedVariables(TruePath) And OS = clsGame.eOS.Windows Then
                Return True
            End If

            'This makes sure a registry key path isn't seen as a relative path.
            If mgrPath.IsSupportedRegistryPath(TruePath) Then
                Return True
            End If

            'Root Check (This should always be last check)
            If IsPathRooted(Path) Then
                Return True
            End If

            Return False
        End Get
    End Property
    Public Property FolderSave As Boolean
    Public Property FileType As String
    Public Property AppendTimeStamp As Boolean
    Public Property BackupLimit As Integer
    Public Property CleanFolder As Boolean
    Public Property ExcludeList As String
    Public Property ProcessPath As String
        Set(value As String)
            sProcessPath = mgrPath.ReverseSpecialPaths(value)
        End Set
        Get
            Return mgrPath.ReplaceSpecialPaths(sProcessPath)
        End Get
    End Property
    Public Property Icon As String
        Set(value As String)
            sIcon = mgrPath.ReverseSpecialPaths(value)
        End Set
        Get
            Return mgrPath.ReplaceSpecialPaths(sIcon)
        End Get
    End Property
    Public Property Hours As Double
    Public Property Version As String
    Public Property Company As String
    Public Property Enabled As Boolean
    Public Property MonitorOnly As Boolean
    Public Property Comments As String
    Public Property IsRegEx As Boolean
    Public Property RecurseSubFolders As Boolean
    Public Property OS As eOS
    Public Property UseWindowTitle As Boolean
    Public Property Differential As Boolean
    Public Property DiffInterval As Integer
    Public ReadOnly Property TruePath As String
        Get
            Return sPath
        End Get
    End Property
    Public ReadOnly Property TrueProcessPath As String
        Get
            Return sProcessPath
        End Get
    End Property
    Public ReadOnly Property TrueIcon As String
        Get
            Return sIcon
        End Get
    End Property
    Public ReadOnly Property IncludeArray As String()
        Get
            If FileType = String.Empty Then
                Return New String() {}
            Else
                Return FileType.Split(":")
            End If
        End Get
    End Property
    Public ReadOnly Property ExcludeArray As String()
        Get
            If ExcludeList = String.Empty Then
                Return New String() {}
            Else
                Return ExcludeList.Split(":")
            End If
        End Get
    End Property
    Public Property ImportTags As List(Of Tag)
    Public Property ImportConfigLinks As List(Of ConfigLink)
    Public Property ImportUpdate As Boolean
    Public Property CompiledRegEx As Regex

    Public Function ConvertToGame() As Game
        Dim oGame As New Game

        oGame.ID = ID
        oGame.Name = Name
        oGame.ProcessName = ProcessName
        oGame.Parameter = Parameter
        oGame.Path = TruePath
        oGame.FolderSave = FolderSave
        oGame.AppendTimeStamp = AppendTimeStamp
        oGame.BackupLimit = BackupLimit
        oGame.FileType = FileType
        oGame.ExcludeList = ExcludeList
        oGame.MonitorOnly = MonitorOnly
        oGame.Comments = Comments
        oGame.IsRegEx = IsRegEx
        oGame.RecurseSubFolders = RecurseSubFolders
        oGame.OS = OS
        oGame.UseWindowTitle = UseWindowTitle
        oGame.Differential = Differential
        oGame.DiffInterval = DiffInterval
        oGame.Tags = mgrGameTags.GetTagsByGameForExport(ID)
        oGame.ConfigLinks = mgrConfigLinks.GetConfigLinksByGameForExport(ID)

        Return oGame
    End Function

    Public Function SyncEquals(obj As Object, eSyncFields As eOptionalSyncFields) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            'Core Sync Fields
            If ID <> oGame.ID Then
                Return False
            End If
            If Name <> oGame.Name Then
                Return False
            End If
            If ProcessName <> oGame.ProcessName Then
                Return False
            End If
            If Parameter <> oGame.Parameter Then
                Return False
            End If
            If Path <> oGame.Path Then
                Return False
            End If
            If FileType <> oGame.FileType Then
                Return False
            End If
            If ExcludeList <> oGame.ExcludeList Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
                Return False
            End If
            If AppendTimeStamp <> oGame.AppendTimeStamp Then
                Return False
            End If
            If BackupLimit <> oGame.BackupLimit Then
                Return False
            End If
            If CleanFolder <> oGame.CleanFolder Then
                Return False
            End If
            If Hours <> oGame.Hours Then
                Return False
            End If
            If MonitorOnly <> oGame.MonitorOnly Then
                Return False
            End If
            If Comments <> oGame.Comments Then
                Return False
            End If
            If IsRegEx <> oGame.IsRegEx Then
                Return False
            End If
            If RecurseSubFolders <> oGame.RecurseSubFolders Then
                Return False
            End If
            If OS <> oGame.OS Then
                Return False
            End If
            If UseWindowTitle <> oGame.UseWindowTitle Then
                Return False
            End If
            If Differential <> oGame.Differential Then
                Return False
            End If
            If DiffInterval <> oGame.DiffInterval Then
                Return False
            End If

            'Optional Sync Fields
            If (eSyncFields And eOptionalSyncFields.Company) = eOptionalSyncFields.Company Then
                If Company <> oGame.Company Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.GamePath) = eOptionalSyncFields.GamePath Then
                If ProcessPath <> oGame.ProcessPath Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.Icon) = eOptionalSyncFields.Icon Then
                If Icon <> oGame.Icon Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.MonitorGame) = eOptionalSyncFields.MonitorGame Then
                If Enabled <> oGame.Enabled Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.Version) = eOptionalSyncFields.Version Then
                If Version <> oGame.Version Then
                    Return False
                End If
            End If
            Return True
        End If
    End Function

    Public Function CoreEquals(obj As Object) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            'Core Fields
            If ID <> oGame.ID Then
                Return False
            End If
            If Name <> oGame.Name Then
                Return False
            End If
            If ProcessName <> oGame.ProcessName Then
                Return False
            End If
            If Parameter <> oGame.Parameter Then
                Return False
            End If
            If Path <> oGame.Path Then
                Return False
            End If
            If FileType <> oGame.FileType Then
                Return False
            End If
            If ExcludeList <> oGame.ExcludeList Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
                Return False
            End If
            If AppendTimeStamp <> oGame.AppendTimeStamp Then
                Return False
            End If
            If MonitorOnly <> oGame.MonitorOnly Then
                Return False
            End If
            If Comments <> oGame.Comments Then
                Return False
            End If
            If IsRegEx <> oGame.IsRegEx Then
                Return False
            End If
            If RecurseSubFolders <> oGame.RecurseSubFolders Then
                Return False
            End If
            If OS <> oGame.OS Then
                Return False
            End If
            If UseWindowTitle <> oGame.UseWindowTitle Then
                Return False
            End If
            If Differential <> oGame.Differential Then
                Return False
            End If
            If DiffInterval <> oGame.DiffInterval Then
                Return False
            End If
            Return True
        End If
    End Function

    Public Function MinimalEquals(obj As Object) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            If ID <> oGame.ID Then
                Return False
            End If
            Return True
        End If
    End Function

    Public Function ShallowCopy() As clsGame
        Return DirectCast(Me.MemberwiseClone(), clsGame)
    End Function

    Public Shared Function SetSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields Or eSyncField
    End Function

    Public Shared Function RemoveSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields And (Not eSyncField)
    End Function

    Sub New()
        ID = Guid.NewGuid.ToString
        Name = String.Empty
        ProcessName = String.Empty
        Parameter = String.Empty
        Path = String.Empty
        FolderSave = False
        FileType = String.Empty
        AppendTimeStamp = False
        BackupLimit = 0
        CleanFolder = False
        ExcludeList = String.Empty
        ProcessPath = String.Empty
        Icon = String.Empty
        Hours = 0
        Version = String.Empty
        Company = String.Empty
        Enabled = True
        MonitorOnly = False
        Comments = String.Empty
        IsRegEx = False
        RecurseSubFolders = True
        OS = mgrCommon.GetCurrentOS()
        UseWindowTitle = False
        Differential = False
        DiffInterval = 0
        ImportTags = New List(Of Tag)
        ImportConfigLinks = New List(Of ConfigLink)
        ImportUpdate = False
    End Sub

    Sub New(sID As String, sName As String, sProcessName As String, sParameter As String, sPath As String, bFolderSave As Boolean, sFileType As String, bAppendTimeStamp As Boolean, iBackupLimit As Integer,
            bCleanFolder As Boolean, sExcludeList As String, sProcessPath As String, sIcon As String, dHours As Double, sVersion As String, sCompany As String, bEnabled As Boolean, bMonitorOnly As Boolean,
            sComments As String, bIsRegEx As Boolean, bRecurseSubFolders As Boolean, eOS As eOS, bUseWindowTitle As Boolean, bDifferential As Boolean, iDiffInterval As Integer, oImportTags As List(Of Tag),
            oImportConfigLinks As List(Of ConfigLink), bImportUpdate As Boolean)
        ID = sID
        Name = sName
        ProcessName = sProcessName
        Parameter = sParameter
        Path = sPath
        FolderSave = bFolderSave
        FileType = sFileType
        AppendTimeStamp = bAppendTimeStamp
        BackupLimit = iBackupLimit
        CleanFolder = bCleanFolder
        ExcludeList = sExcludeList
        ProcessPath = sProcessPath
        Icon = sIcon
        Hours = dHours
        Version = sVersion
        Company = sCompany
        Enabled = bEnabled
        MonitorOnly = bMonitorOnly
        Comments = sComments
        IsRegEx = bIsRegEx
        RecurseSubFolders = bRecurseSubFolders
        OS = eOS
        UseWindowTitle = bUseWindowTitle
        Differential = bDifferential
        DiffInterval = iDiffInterval
        ImportTags = oImportTags
        ImportConfigLinks = oImportConfigLinks
        ImportUpdate = bImportUpdate
    End Sub
End Class
