Public Class clsGameFilterField
    Public Enum eDataType As Integer
        fString = 1
        fNumeric = 2
        fBool = 3
        fEnum = 4
    End Enum

    Public Enum eEnumFilterField As Integer
        None = 1
        OS = 2
    End Enum

    <Flags()> Public Enum eFieldStatus
        None = 1
        ValidFilter = 2
        ValidSort = 3
    End Enum

    Private eStatus As eFieldStatus

    Public Property FieldName As String
    Public Property FriendlyFieldName As String
    Public Property Type As eDataType
    Public Property EnumField As eEnumFilterField
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

    Sub New()
        FieldName = String.Empty
        FriendlyFieldName = String.Empty
        Type = eDataType.fString
        EnumField = eEnumFilterField.None
        Status = eFieldStatus.None
    End Sub

    Sub New(sFieldName As String, sFriendlyFieldName As String, eType As eDataType, eEnumField As eEnumFilterField, eStatus As eFieldStatus)
        FieldName = sFieldName
        FriendlyFieldName = sFriendlyFieldName
        Type = eType
        EnumField = eEnumField
        Status = eStatus
    End Sub

End Class
