Public Class clsSession

    Private sMonitorID As String
    Private iStart As Int64
    Private iEnd As Int64

    Public Property MonitorID As String
        Set(value As String)
            sMonitorID = value
        End Set
        Get
            Return sMonitorID
        End Get
    End Property

    Public Property SessionStart As Int64
        Set(value As Int64)
            iStart = value
        End Set
        Get
            Return iStart
        End Get
    End Property

    Public WriteOnly Property SessionStartFromDate As DateTime
        Set(value As DateTime)
            iStart = mgrCommon.DateToUnix(value)
        End Set
    End Property

    Public ReadOnly Property SessionStartFormatted As DateTime
        Get
            Return mgrCommon.UnixToDate(iStart)
        End Get
    End Property

    Public Property SessionEnd As Int64
        Set(value As Int64)
            iEnd = value
        End Set
        Get
            Return iEnd
        End Get
    End Property

    Public WriteOnly Property SessionEndFromDate As DateTime
        Set(value As DateTime)
            iEnd = mgrCommon.DateToUnix(value)
        End Set
    End Property

    Public ReadOnly Property SessionEndFormatted As DateTime
        Get
            Return mgrCommon.UnixToDate(iEnd)
        End Get
    End Property
End Class
