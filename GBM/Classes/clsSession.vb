Public Class clsSession
    Public Property MonitorID As String
    Public Property SessionStart As Int64
    Public Property SessionEnd As Int64
    Public WriteOnly Property SessionStartFromDate As DateTime
        Set(value As DateTime)
            SessionStart = mgrCommon.DateToUnix(value)
        End Set
    End Property
    Public ReadOnly Property SessionStartFormatted As DateTime
        Get
            Return mgrCommon.UnixToDate(SessionStart)
        End Get
    End Property
    Public WriteOnly Property SessionEndFromDate As DateTime
        Set(value As DateTime)
            SessionEnd = mgrCommon.DateToUnix(value)
        End Set
    End Property
    Public ReadOnly Property SessionEndFormatted As DateTime
        Get
            Return mgrCommon.UnixToDate(SessionEnd)
        End Get
    End Property

    Sub New()
        MonitorID = String.Empty
        SessionStart = 0
        SessionEnd = 0
    End Sub

    Sub New(sMonitorID As String, iSessionStart As Int64, iSessionEnd As Int64)
        MonitorID = sMonitorID
        SessionStart = iSessionStart
        SessionEnd = iSessionEnd
    End Sub
End Class
