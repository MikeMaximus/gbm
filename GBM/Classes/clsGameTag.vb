Public Class clsGameTag
    Private sTagID As String = String.Empty
    Private sMonitorID As String = String.Empty

    Property TagID As String
        Get
            Return sTagID
        End Get
        Set(value As String)
            sTagID = value
        End Set
    End Property

    Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property

End Class
