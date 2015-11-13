Public Class clsTag
    Private sTagID As String = Guid.NewGuid.ToString
    Private sTagName As String = String.Empty
    
    Property ID As String
        Get
            Return sTagID
        End Get
        Set(value As String)
            sTagID = value
        End Set
    End Property

    Property Name As String
        Get
            Return sTagName
        End Get
        Set(value As String)
            sTagName = value
        End Set
    End Property

End Class
