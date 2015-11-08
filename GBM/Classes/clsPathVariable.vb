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
            Return "*" & sVariableName & "*"
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

End Class
