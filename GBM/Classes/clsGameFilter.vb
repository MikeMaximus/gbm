Public Class clsGameFilter
    Public Enum eNumericOperators
        Equals = 1
        Greater = 2
        Lesser = 3
        GreaterEquals = 4
        LesserEquals = 5
    End Enum

    Public Property ID As String
    Public Property Field As clsGameFilterField
    Public Property NotCondition As Boolean
    Public Property Data As Object
    Public Property NumericOperator As eNumericOperators
    Public ReadOnly Property NumericOperatorAsString As String
        Get
            Select Case NumericOperator
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

    Sub New()
        ID = String.Empty
        Field = Nothing
        NotCondition = False
        Data = Nothing
        NumericOperator = eNumericOperators.Equals
    End Sub

    Sub New(sID As String, oField As clsGameFilterField, bNotCondition As Boolean, oData As Object, eNumericOperator As eNumericOperators)
        ID = sID
        Field = oField
        NotCondition = bNotCondition
        Data = oData
        NumericOperator = eNumericOperator
    End Sub
End Class
