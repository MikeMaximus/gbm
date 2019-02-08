Public Class clsWineData
    Private sMonitorID As String = String.Empty
    Private sPrefix As String = String.Empty
    Private sSavePath As String = String.Empty
    Private sBinaryPath As String = String.Empty

    Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property

    Property Prefix As String
        Get
            Return sPrefix
        End Get
        Set(value As String)
            sPrefix = value
        End Set
    End Property

    Property SavePath As String
        Get
            Return sSavePath
        End Get
        Set(value As String)
            sSavePath = value
        End Set
    End Property

    Property BinaryPath As String
        Get
            Return sBinaryPath
        End Get
        Set(value As String)
            sBinaryPath = value
        End Set
    End Property

End Class
