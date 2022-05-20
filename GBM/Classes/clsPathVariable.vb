Public Class clsPathVariable
    Private sVariableID As String = Guid.NewGuid.ToString
    Private sVariableName As String = String.Empty
    Private sVariableDescription As String = String.Empty
    Private sPath As String = String.Empty

    Property ID As String
        Get
            Return sVariableID
        End Get
        Set(value As String)
            sVariableID = value
        End Set
    End Property

    Property Name As String
        Get
            Return sVariableName
        End Get
        Set(value As String)
            sVariableName = value
        End Set
    End Property

    ReadOnly Property FormattedName As String
        Get
            Return "%" & sVariableName & "%"
        End Get
    End Property

    Property Path As String
        Get
            Return sPath
        End Get
        Set(value As String)
            sPath = value
        End Set
    End Property

    Public Sub New()
        'Empty
    End Sub

    Public Sub New(ByVal sInitName As String, ByVal sInitPath As String)
        Name = sInitName
        Path = sInitPath
    End Sub

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

End Class
