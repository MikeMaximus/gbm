Public Class Backup
    Private sManifestID As String
    Private iDateUpdated As Int64
    Private sUpdatedBy As String
    Private bIsDifferentialParent As Boolean
    Private sDifferentialParent As String

    Property ManifestID As String
        Get
            Return sManifestID
        End Get
        Set(value As String)
            sManifestID = value
        End Set
    End Property

    Property DateUpdated As Int64
        Get
            Return iDateUpdated
        End Get
        Set(value As Int64)
            iDateUpdated = value
        End Set
    End Property

    Property UpdatedBy As String
        Get
            Return sUpdatedBy
        End Get
        Set(value As String)
            sUpdatedBy = value
        End Set
    End Property

    Property IsDifferentialParent As Boolean
        Get
            Return bIsDifferentialParent
        End Get
        Set(value As Boolean)
            bIsDifferentialParent = value
        End Set
    End Property

    Property DifferentialParent As String
        Get
            Return sDifferentialParent
        End Get
        Set(value As String)
            sDifferentialParent = value
        End Set
    End Property
End Class