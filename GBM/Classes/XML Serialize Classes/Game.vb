Public Class Game
    Private sGameName As String
    Private sProcessName As String
    Private sParameter As String
    Private sPath As String
    Private bAbsolutePath As Boolean
    Private bFolderSave As Boolean
    Private sFileType As String
    Private sExcludeList As String
    Private bMonitorOnly As Boolean
    Private oTags As List(Of Tag)

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

    Property Tags As List(Of Tag)
        Get
            Return oTags
        End Get
        Set(value As List(Of Tag))
            oTags = value
        End Set
    End Property

End Class
