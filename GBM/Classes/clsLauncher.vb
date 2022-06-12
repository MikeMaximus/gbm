Public Class clsLauncher
    Public Property LauncherID As String
    Public Property IsUri As Boolean
    Public Property Name As String
    Public Property LaunchString As String
    Public Property LaunchParameters As String

    Sub New()
        LauncherID = Guid.NewGuid.ToString
        IsUri = True
        Name = String.Empty
        LaunchString = String.Empty
        LaunchParameters = String.Empty
    End Sub

    Sub New(bIsUri As Boolean, sName As String, sLaunchString As String, sArgs As String)
        LauncherID = Guid.NewGuid.ToString
        IsUri = bIsUri
        Name = sName
        LaunchString = sLaunchString
        LaunchParameters = sArgs
    End Sub
End Class
