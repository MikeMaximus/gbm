Public Class clsGameFilter

    Private sID As String
    Private oField As clsGameFilterField
    Private oData As Object
    Private eNumericOperator As eNumericOperators = eNumericOperators.Equals
    Private bNextBoolOperator As Boolean

    Public Enum eNumericOperators
        Equals = 1
        Greater = 2
        Lesser = 3
        GreaterEquals = 4
        LesserEquals = 5
    End Enum

    Public Property ID As String
        Get
            Return sID
        End Get
        Set(value As String)
            sID = value
        End Set
    End Property

    Public Property Field As clsGameFilterField
        Get
            Return oField
        End Get
        Set(value As clsGameFilterField)
            oField = value
        End Set
    End Property

    Public Property Data As Object
        Get
            Return oData
        End Get
        Set(value As Object)
            oData = value
        End Set
    End Property

    Public Property NextBoolOperator As Boolean
        Get
            Return bNextBoolOperator
        End Get
        Set(value As Boolean)
            bNextBoolOperator = value
        End Set
    End Property

    Public Property NumericOperator As eNumericOperators
        Get
            Return eNumericOperator
        End Get
        Set(value As eNumericOperators)
            eNumericOperator = value
        End Set
    End Property

    Public ReadOnly Property NumericOperatorAsString As String
        Get
            Select Case eNumericOperator
                Case eNumericOperators.Equals
                    Return "="
                Case eNumericOperators.Greater
                    Return ">"
                Case eNumericOperators.GreaterEquals
                    Return ">="
                Case eNumericOperators.Lesser
                    Return "<"
                Case eNumericOperators.LesserEquals
                    Return "<="
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

End Class
