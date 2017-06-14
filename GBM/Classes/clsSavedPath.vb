Public Class clsSavedPath
    Private sPathName As String = String.Empty
    Private sPath As String = String.Empty

    Property PathName As String
        Get
            Return sPathName
        End Get
        Set(value As String)
            sPathName = value
        End Set
    End Property

    Property Path As String
        Get
            Return sPath
        End Get
        Set(value As String)
            sPath = value
        End Set
    End Property
End Class
