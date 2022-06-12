<Serializable()>
Public Class clsProcess
    Public Property ID As String
    Public Property Name As String
    Public Property Path As String
    Public Property Args As String
    Public Property Kill As Boolean

    Sub New()
        ID = Guid.NewGuid.ToString
        Name = String.Empty
        Path = String.Empty
        Args = String.Empty
        Kill = True
    End Sub

    Sub New(sName As String, sPath As String, sArgs As String, bKill As Boolean)
        ID = Guid.NewGuid.ToString
        Name = sName
        Path = sPath
        Args = sArgs
        Kill = bKill
    End Sub
End Class
