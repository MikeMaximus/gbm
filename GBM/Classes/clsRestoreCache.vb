Public Class clsRestoreCache
    Private sName As String
    Private sPath As String

    Property Name As String
        Get
            Return sName
        End Get
        Set(value As String)
            sName = value
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
