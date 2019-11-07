<Serializable()>
Public Class ConfigLink

    Private sID As String

    Property ID As String
        Get
            Return sID
        End Get
        Set(value As String)
            sID = value
        End Set
    End Property
End Class
