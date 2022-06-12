<Serializable()>
Public Class Tag
    Property Name As String

    Sub New()
        Name = String.Empty
    End Sub

    Sub New(ByVal sName As String)
        Name = sName
    End Sub

    Overrides Function Equals(o As Object) As Boolean
        o = DirectCast(o, Tag)

        If o.Name = Me.Name Then
            Return True
        End If

        Return False
    End Function
End Class
