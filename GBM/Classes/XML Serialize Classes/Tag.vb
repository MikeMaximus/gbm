<Serializable()>
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

    Sub New()
        Name = String.Empty
    End Sub

    Sub New(ByVal sInitName As String)
        Name = sInitName
    End Sub

    Overrides Function Equals(o As Object) As Boolean
        o = DirectCast(o, Tag)

        If o.Name = Me.Name Then
            Return True
        End If

        Return False
    End Function
End Class
