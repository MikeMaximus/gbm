Public Class clsLaunchData
    Public Property MonitorID As String
    Public Property Path As String
    Public Property Args As String
    Public Property LauncherID As String
    Public Property LauncherGameID As String
    Public Property NoArgs As Boolean

    Sub New()
        MonitorID = String.Empty
        Path = String.Empty
        Args = String.Empty
        LauncherID = String.Empty
        LauncherGameID = String.Empty
        NoArgs = False
    End Sub

    Sub New(sMonitorID As String, sPath As String, sArgs As String, sLauncherID As String, sLauncherGameID As String, bNoArgs As Boolean)
        MonitorID = sMonitorID
        Path = sPath
        Args = sArgs
        LauncherID = sLauncherID
        LauncherGameID = sLauncherGameID
        NoArgs = bNoArgs
    End Sub
End Class
