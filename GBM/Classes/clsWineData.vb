Public Class clsWineData
    Public Property MonitorID As String
    Public Property Prefix As String
    Public Property SavePath As String
    Public Property BinaryPath As String

    Sub New()
        MonitorID = String.Empty
        Prefix = String.Empty
        SavePath = String.Empty
        BinaryPath = String.Empty
    End Sub

    Sub New(sMonitorID As String, sPrefix As String, sSavePath As String, sBinaryPath As String)
        MonitorID = sMonitorID
        Prefix = sPrefix
        SavePath = sSavePath
        BinaryPath = sBinaryPath
    End Sub
End Class
