Public Class clsGameProcess
    Private sProcessID As String
    Private sMonitorID As String

    Public Property ProcessID As String
        Get
            Return sProcessID
        End Get
        Set(value As String)
            sProcessID = value
        End Set
    End Property

    Public Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property
End Class
