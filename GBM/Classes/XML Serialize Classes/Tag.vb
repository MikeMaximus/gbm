Public Class Tag
    Private sTagName As String

    Property Name As String
        Get
            Return sTagName
        End Get
        Set(value As String)
            sTagName = value
        End Set
    End Property

End Class
