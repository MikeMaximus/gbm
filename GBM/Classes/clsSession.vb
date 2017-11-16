Public Class clsSession

    Private sMonitorID As String
    Private dStart As DateTime
    Private dEnd As DateTime
    Private sComputerName As String = My.Computer.Name

    Public Property MonitorID As String
        Set(value As String)
            sMonitorID = value
        End Set
        Get
            Return sMonitorID
        End Get
    End Property

    Public Property SessionStart As DateTime
        Set(value As DateTime)
            dStart = value
        End Set
        Get
            Return dStart
        End Get
    End Property

    Public Property SessionEnd As DateTime
        Set(value As DateTime)
            dEnd = value
        End Set
        Get
            Return dEnd
        End Get
    End Property

    Public Property ComputerName As String
        Set(value As String)
            sComputerName = value
        End Set
        Get
            Return sComputerName
        End Get
    End Property

End Class
