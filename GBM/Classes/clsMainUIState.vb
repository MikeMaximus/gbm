Public Class clsMainUIState

    Public Property Title As String
    Public Property Icon As Image
    Public Property Status1 As String
    Public Property Status2 As String
    Public Property Status3 As String
    Public Property Time As String
    Public Property MonitorStatus As String
    Public Property TrayStatus As String

    Sub New()
        Title = String.Empty
        Icon = Nothing
        Status1 = String.Empty
        Status2 = String.Empty
        Status3 = String.Empty
        Time = String.Empty
        MonitorStatus = String.Empty
        TrayStatus = String.Empty
    End Sub

    Sub New(sTitle As String, oIcon As Image, sStatus1 As String, sStatus2 As String, sStatus3 As String, sTime As String, sMonitorStatus As String, sTrayStatus As String)
        Title = sTitle
        Icon = oIcon
        Status1 = sStatus1
        Status2 = sStatus2
        Status3 = sStatus3
        Time = sTime
        MonitorStatus = sMonitorStatus
        TrayStatus = sTrayStatus
    End Sub
End Class
