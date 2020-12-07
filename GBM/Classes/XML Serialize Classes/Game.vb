Public Class Game
    Private sGameID As String
    Private sGameName As String
    Private sProcessName As String
    Private sParameter As String
    Private sPath As String
    Private bFolderSave As Boolean
    Private bAppendTimeStamp As Boolean
    Private iBackupLimit As Integer
    Private sFileType As String
    Private sExcludeList As String
    Private bMonitorOnly As Boolean
    Private sComments As String
    Private bIsRegEx As Boolean
    Private bRecurseSubFolders As Boolean
    Private iOS As clsGame.eOS
    Private oTags As List(Of Tag)
    Private oConfigLinks As List(Of ConfigLink)

    Property ID As String
        Set(value As String)
            sGameID = value
        End Set
        Get
            Return sGameID
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
            sPath = value
        End Set
        Get
            Return sPath
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

    Property AppendTimeStamp As Boolean
        Set(value As Boolean)
            bAppendTimeStamp = value
        End Set
        Get
            Return bAppendTimeStamp
        End Get
    End Property

    Property BackupLimit As Integer
        Set(value As Integer)
            iBackupLimit = value
        End Set
        Get
            Return iBackupLimit
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

    Property ExcludeList As String
        Set(value As String)
            sExcludeList = value
        End Set
        Get
            Return sExcludeList
        End Get
    End Property

    Property MonitorOnly As Boolean
        Set(value As Boolean)
            bMonitorOnly = value
        End Set
        Get
            Return bMonitorOnly
        End Get
    End Property

    Property Comments As String
        Set(value As String)
            sComments = value
        End Set
        Get
            Return sComments
        End Get
    End Property

    Property IsRegEx As Boolean
        Set(value As Boolean)
            bIsRegEx = value
        End Set
        Get
            Return bIsRegEx
        End Get
    End Property

    Property RecurseSubFolders As Boolean
        Set(value As Boolean)
            bRecurseSubFolders = value
        End Set
        Get
            Return bRecurseSubFolders
        End Get
    End Property

    Property OS As clsGame.eOS
        Set(value As clsGame.eOS)
            iOS = value
        End Set
        Get
            Return iOS
        End Get
    End Property

    Property Tags As List(Of Tag)
        Get
            Return oTags
        End Get
        Set(value As List(Of Tag))
            oTags = value
        End Set
    End Property

    Property ConfigLinks As List(Of ConfigLink)
        Get
            Return oConfigLinks
        End Get
        Set(value As List(Of ConfigLink))
            oConfigLinks = value
        End Set
    End Property
    Public Function ConvertClass() As clsGame
        Dim oGame As New clsGame

        oGame.ID = ID
        oGame.Name = Name
        oGame.ProcessName = ProcessName
        oGame.Parameter = Parameter
        oGame.Path = Path
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
        oGame.ImportTags = Tags

        Return oGame
    End Function
End Class
