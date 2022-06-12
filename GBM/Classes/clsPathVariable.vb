Public Class clsPathVariable
    Public Property ID As String
    Public Property Name As String
    Public Property Path As String
    Public ReadOnly Property FormattedName As String
        Get
            Return "%" & Name & "%"
        End Get
    End Property

    Public Function CoreEquals(ByVal obj As Object) As Boolean
        Dim oVariable As clsPathVariable = TryCast(obj, clsPathVariable)
        If oVariable Is Nothing Then
            Return False
        Else
            If Name <> oVariable.Name Then
                Return False
            End If
            If Path <> oVariable.Path Then
                Return False
            End If
            Return True
        End If
    End Function

    Sub New()
        ID = Guid.NewGuid.ToString
        Name = String.Empty
        Path = String.Empty
    End Sub

    Sub New(ByVal sName As String, ByVal sPath As String)
        ID = Guid.NewGuid.ToString
        Name = sName
        Path = sPath
    End Sub
End Class
