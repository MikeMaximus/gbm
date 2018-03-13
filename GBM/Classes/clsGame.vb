<Serializable()>
Public Class clsGame
    Private sGameID As String = Guid.NewGuid.ToString
    Private sGameName As String = String.Empty
    Private sProcessName As String = String.Empty
    Private sParameter As String = String.Empty
    Private sPath As String = String.Empty
    Private bAbsolutePath As Boolean = False
    Private bFolderSave As Boolean = False
    Private sFileType As String = String.Empty
    Private bAppendTimeStamp As Boolean = False
    Private iBackupLimit As Integer = 2
    Private bCleanFolder As Boolean = False
    Private sExcludeList As String = String.Empty
    Private sProcessPath As String = String.Empty
    Private sIcon As String = String.Empty
    Private dHours As Double = 0
    Private sVersion As String = String.Empty
    Private sCompany As String = String.Empty
    Private bEnabled As Boolean = True
    Private bMonitorOnly As Boolean = False
    Private sComments As String = String.Empty
    Private bIsRegEx As Boolean = False
    Private bDuplicate As Boolean = False
    Private oImportTags As New List(Of Tag)
    Private bImportUpdate As Boolean = False

    <Flags()> Public Enum eOptionalSyncFields
        None = 0
        GamePath = 1
        Company = 2
        Version = 4
        Icon = 16
        TimeStamp = 32
        MonitorGame = 64
    End Enum

    Property ID As String
        Set(value As String)
            If Not value Is Nothing Then
                sGameID = mgrPath.ValidateForFileSystem(value)
            End If
        End Set
        Get
            Return sGameID
        End Get
    End Property

    ReadOnly Property CompoundKey As String
        Get
            Return ProcessName & ":" & ID
        End Get
    End Property

    ReadOnly Property CroppedName As String
        Get
            If Name.Length > 40 Then
                Return sGameName.Substring(0, 41).Trim & "..."
            Else
                Return sGameName
            End If
        End Get
    End Property

    ReadOnly Property FileSafeName As String
        Get
            Return mgrPath.ValidateForFileSystem(sGameName)
        End Get
    End Property

    Property Name As String
        Set(value As String)
            sGameName = value
        End Set
        Get
            Return sGameName
        End Get
    End Property

    Property ProcessName As String
        Set(value As String)
            sProcessName = value
        End Set
        Get
            Return sProcessName
        End Get
    End Property

    Property Parameter As String
        Set(value As String)
            sParameter = value
        End Set
        Get
            Return sParameter
        End Get
    End Property

    Property Path As String
        Set(value As String)
            sPath = mgrPath.ReverseSpecialPaths(value)
        End Set
        Get
            Return mgrPath.ReplaceSpecialPaths(sPath)
        End Get
    End Property

    Property AbsolutePath As Boolean
        Set(value As Boolean)
            bAbsolutePath = value
        End Set
        Get
            Return bAbsolutePath
        End Get
    End Property

    Property FolderSave As Boolean
        Set(value As Boolean)
            bFolderSave = value
        End Set
        Get
            Return bFolderSave
        End Get
    End Property

    Property FileType As String
        Set(value As String)
            sFileType = value
        End Set
        Get
            Return sFileType
        End Get
    End Property

    Property AppendTimeStamp As Boolean
        Get
            Return bAppendTimeStamp
        End Get
        Set(value As Boolean)
            bAppendTimeStamp = value
        End Set
    End Property

    Property BackupLimit As Integer
        Get
            Return iBackupLimit
        End Get
        Set(value As Integer)
            iBackupLimit = value
        End Set
    End Property

    Property CleanFolder As Boolean
        Get
            Return bCleanFolder
        End Get
        Set(value As Boolean)
            bCleanFolder = value
        End Set
    End Property

    Property ExcludeList As String
        Set(value As String)
            sExcludeList = value
        End Set
        Get
            Return sExcludeList
        End Get
    End Property

    Property ProcessPath As String
        Set(value As String)
            sProcessPath = value
        End Set
        Get
            Return sProcessPath
        End Get
    End Property

    Property Icon As String
        Get
            Return sIcon
        End Get
        Set(value As String)
            sIcon = value
        End Set
    End Property

    Property Hours As Double
        Get
            Return dHours
        End Get
        Set(value As Double)
            dHours = value
        End Set
    End Property

    Property Version As String
        Get
            Return sVersion
        End Get
        Set(value As String)
            sVersion = value
        End Set
    End Property

    Property Company As String
        Get
            Return sCompany
        End Get
        Set(value As String)
            sCompany = value
        End Set
    End Property

    Property Enabled As Boolean
        Get
            Return bEnabled
        End Get
        Set(value As Boolean)
            bEnabled = value
        End Set
    End Property

    Property MonitorOnly As Boolean
        Get
            Return bMonitorOnly
        End Get
        Set(value As Boolean)
            bMonitorOnly = value
        End Set
    End Property

    Property Comments As String
        Get
            Return sComments
        End Get
        Set(value As String)
            sComments = value
        End Set
    End Property

    Property IsRegEx As Boolean
        Get
            Return bIsRegEx
        End Get
        Set(value As Boolean)
            bIsRegEx = value
        End Set
    End Property

    Property Duplicate As Boolean
        Get
            Return bDuplicate
        End Get
        Set(value As Boolean)
            bDuplicate = value
        End Set
    End Property

    ReadOnly Property TruePath As String
        Get
            Return sPath
        End Get
    End Property

    ReadOnly Property TrueProcess As String
        Get
            Return HandleProcessDuplicates()
        End Get
    End Property

    Property ImportTags As List(Of Tag)
        Get
            Return oImportTags
        End Get
        Set(value As List(Of Tag))
            oImportTags = value
        End Set
    End Property

    Property ImportUpdate As Boolean
        Get
            Return bImportUpdate
        End Get
        Set(value As Boolean)
            bImportUpdate = value
        End Set
    End Property

    ReadOnly Property IncludeArray As String()
        Get
            If FileType = String.Empty Then
                Return New String() {}
            Else
                Return FileType.Split(":")
            End If
        End Get
    End Property

    ReadOnly Property ExcludeArray As String()
        Get
            If ExcludeList = String.Empty Then
                Return New String() {}
            Else
                Return ExcludeList.Split(":")
            End If
        End Get
    End Property

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
            If AbsolutePath <> oGame.AbsolutePath Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
                Return False
            End If
            If CleanFolder <> oGame.CleanFolder Then
                Return False
            End If
            If AppendTimeStamp <> oGame.AppendTimeStamp Then
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
            If (eSyncFields And eOptionalSyncFields.TimeStamp) = eOptionalSyncFields.TimeStamp Then
                If AppendTimeStamp <> oGame.AppendTimeStamp Then
                    Return False
                End If
                If BackupLimit <> oGame.BackupLimit Then
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
            If AbsolutePath <> oGame.AbsolutePath Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
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

    Private Function HandleProcessDuplicates() As String
        Dim sProcessName As String

        'Handle Duplicates
        sProcessName = Me.ProcessName
        If Me.Duplicate Then
            sProcessName = Me.ProcessName.Split(":")(0)
        End If

        Return sProcessName
    End Function

    Public Shared Function SetSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields Or eSyncField
    End Function

    Public Shared Function RemoveSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields And (Not eSyncField)
    End Function

End Class
