Public Class clsLauncher
    Private sLauncherID As String = Guid.NewGuid.ToString
    Private bIsUri As Boolean = True
    Private sName As String = String.Empty
    Private sLaunchString As String = String.Empty
    Private sArgs As String = String.Empty

    Property LauncherID As String
        Set(value As String)
            sLauncherID = value
        End Set
        Get
            Return sLauncherID
        End Get
    End Property

    Property IsUri As Boolean
        Set(value As Boolean)
            bIsUri = value
        End Set
        Get
            Return bIsUri
        End Get
    End Property

    Property Name As String
        Set(value As String)
            sName = value
        End Set
        Get
            Return sName
        End Get
    End Property

    Property LaunchString As String
        Set(value As String)
            sLaunchString = value
        End Set
        Get
            Return sLaunchString
        End Get
    End Property

    Property LaunchParameters As String
        Set(value As String)
            sArgs = value
        End Set
        Get
            Return sArgs
        End Get
    End Property

    Sub New()
        'Empty
    End Sub

    Sub New(ByVal sName As String, ByVal sLaunchString As String)
        IsUri = bIsUri
        Name = sName
        LaunchString = sLaunchString
        LaunchParameters = sArgs
    End Sub
End Class
