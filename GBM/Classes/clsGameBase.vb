Imports System.Xml.Serialization
Imports System.IO.Path

<Serializable()>
Public MustInherit Class clsGameBase

    Private sGameID As String = Guid.NewGuid.ToString
    Private sGameName As String = String.Empty
    Private sProcessName As String = String.Empty
    Private sParameter As String = String.Empty
    Private sPath As String = String.Empty
    Private bFolderSave As Boolean = False
    Private sFileType As String = String.Empty
    Private bAppendTimeStamp As Boolean = False
    Private iBackupLimit As Integer = 0
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
    Private bRecurseSubFolders As Boolean = True
    Private iOS As eOS = mgrCommon.GetCurrentOS()

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

    Property ID As String
        Set(value As String)
            If Not value Is Nothing Then
                sGameID = mgrPath.ValidateFileName(value)
            End If
        End Set
        Get
            Return sGameID
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
            Return mgrPath.ValidateFileName(sGameName)
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

    ReadOnly Property AbsolutePath As Boolean
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
            sProcessPath = mgrPath.ReverseSpecialPaths(value)
        End Set
        Get
            Return mgrPath.ReplaceSpecialPaths(sProcessPath)
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

    Property RecurseSubFolders As Boolean
        Get
            Return bRecurseSubFolders
        End Get
        Set(value As Boolean)
            bRecurseSubFolders = value
        End Set
    End Property

    Property OS As eOS
        Get
            Return iOS
        End Get
        Set(value As eOS)
            iOS = value
        End Set
    End Property

    Property TruePath As String
        Set(value As String)
            sPath = value
        End Set
        Get
            Return sPath
        End Get
    End Property

    ReadOnly Property TrueProcessPath As String
        Get
            Return sProcessPath
        End Get
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
End Class
