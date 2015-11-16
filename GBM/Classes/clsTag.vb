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

End Class
