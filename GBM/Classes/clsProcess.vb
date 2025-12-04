<Serializable()>
Public Class clsProcess
    Public Property ID As String
    Public Property Name As String
    Public Property Path As String
    Public Property Args As String
    Public Property Kill As Boolean
    Public Property Delay As Integer

    Sub New()
        ID = Guid.NewGuid.ToString
        Name = String.Empty
        Path = String.Empty
        Args = String.Empty
        Kill = True
        Delay = 0
    End Sub

    Sub New(sName As String, sPath As String, sArgs As String, bKill As Boolean, iDelay As Integer)
        ID = Guid.NewGuid.ToString
        Name = sName
        Path = sPath
        Args = sArgs
        Kill = bKill
        Delay = iDelay
    End Sub
End Class
