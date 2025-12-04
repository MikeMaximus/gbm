Public Class clsProcessToStart
    Public Property Name As String
    Public Property Delay As Integer
    Public Property Process As Process

    Sub New()
        Name = String.Empty
        Process = New Process
        Delay = 0
    End Sub

    Sub New(sName As String, iDelay As Integer, oProcess As Process)
        Name = sName
        Delay = iDelay
        Process = oProcess
    End Sub
End Class
