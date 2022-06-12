Public Class clsGameProcess
    Public Property ProcessID As String
    Public Property MonitorID As String

    Sub New()
        ProcessID = String.Empty
        MonitorID = String.Empty
    End Sub

    Sub New(sProcessID As String, sMonitorID As String)
        ProcessID = sProcessID
        MonitorID = sMonitorID
    End Sub
End Class
