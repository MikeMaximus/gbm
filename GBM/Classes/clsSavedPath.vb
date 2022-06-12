Public Class clsSavedPath
    Public Property PathName As String
    Public Property Path As String

    Sub New()
        PathName = String.Empty
        Path = String.Empty
    End Sub

    Sub New(sPathName As String, sPath As String)
        PathName = sPathName
        Path = sPath
    End Sub
End Class
