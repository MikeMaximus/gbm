Public Class clsLaunchData
    Private sMonitorID As String = String.Empty
    Private sPath As String = String.Empty
    Private sArgs As String = String.Empty
    Private sLauncherID As String = String.Empty
    Private sLauncherGameID As String = String.Empty
    Private bNoArgs As Boolean = False

    Property MonitorID As String
        Set(value As String)
            sMonitorID = value
        End Set
        Get
            Return sMonitorID
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

    Property Args As String
        Set(value As String)
            sArgs = value
        End Set
        Get
            Return sArgs
        End Get
    End Property

    Property LauncherID As String
        Set(value As String)
            sLauncherID = value
        End Set
        Get
            Return sLauncherID
        End Get
    End Property

    Property LauncherGameID As String
        Set(value As String)
            sLauncherGameID = value
        End Set
        Get
            Return sLauncherGameID
        End Get
    End Property

    Property NoArgs As Boolean
        Set(value As Boolean)
            bNoArgs = value
        End Set
        Get
            Return bNoArgs
        End Get
    End Property
End Class
