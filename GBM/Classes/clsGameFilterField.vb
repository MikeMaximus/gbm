Public Class clsGameFilterField

    Public Enum eDataType As Integer
        fString = 1
        fNumeric = 2
        fBool = 3
        fEnum = 4
    End Enum

    Public Enum eEnumFilterField As Integer
        OS = 1
    End Enum

    <Flags()> Public Enum eFieldStatus
        None = 0
        ValidFilter = 1
        ValidSort = 2
    End Enum

    Private sFieldName As String
    Private sFriendlyFieldName As String
    Private eType As eDataType
    Private eEnumField As eEnumFilterField
    Private eStatus As eFieldStatus

    Public Property FieldName As String
        Get
            Return sFieldName
        End Get
        Set(value As String)
            sFieldName = value
        End Set
    End Property

    Public Property FriendlyFieldName As String
        Get
            Return sFriendlyFieldName
        End Get
        Set(value As String)
            sFriendlyFieldName = value
        End Set
    End Property

    Public Property Type As eDataType
        Get
            Return eType
        End Get
        Set(value As eDataType)
            eType = value
        End Set
    End Property

    Public Property EnumField As eEnumFilterField
        Get
            Return eEnumField
        End Get
        Set(value As eEnumFilterField)
            eEnumField = value
        End Set
    End Property

    'This is a flag property - Setting a value will toggle that flag on and off.
    Public Property Status As eFieldStatus
        Get
            Return eStatus
        End Get
        Set(value As eFieldStatus)
            If (eStatus And value) = value Then
                eStatus = RemoveFieldStatus(value)
            Else
                eStatus = SetFieldStatus(value)
            End If
        End Set
    End Property

    Private Function SetFieldStatus(ByVal eFlag As eFieldStatus) As eFieldStatus
        Return eStatus Or eFlag
    End Function

    Private Function RemoveFieldStatus(ByVal eFlag As eFieldStatus) As eFieldStatus
        Return eStatus And (Not eFlag)
    End Function

    Public Function CheckStatus(ByVal eFlag As eFieldStatus) As Boolean
        If (eStatus And eFlag) = eFlag Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
