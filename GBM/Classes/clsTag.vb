Public Class clsTag
    Public Property ID As String
    Public Property Name As String

    Public Function CoreEquals(obj As Object) As Boolean
        Dim oTag As clsTag = TryCast(obj, clsTag)
        If oTag Is Nothing Then
            Return False
        Else
            If ID <> oTag.ID Then
                Return False
            End If
            If Name <> oTag.Name Then
                Return False
            End If
            Return True
        End If
    End Function

    Public Function MinimalEquals(obj As Object) As Boolean
        Dim oTag As clsTag = TryCast(obj, clsTag)
        If oTag Is Nothing Then
            Return False
        Else
            If ID <> oTag.ID Then
                Return False
            End If
            Return True
        End If
    End Function

    Sub New()
        ID = Guid.NewGuid.ToString
        Name = String.Empty
    End Sub

    Sub New(sName As String)
        ID = Guid.NewGuid.ToString
        Name = sName
    End Sub
End Class
