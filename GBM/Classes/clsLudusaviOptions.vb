Public Class clsLudusaviOptions

    <Flags()> Public Enum eSupportedOS
        None = 0
        Windows = 1
        Linux = 2
    End Enum

    Private bIncludeSaves As Boolean
    Private bIncludeConfigs As Boolean
    Private eIncludeOS As eSupportedOS

    Property IncludeSaves As Boolean
        Get
            Return bIncludeSaves
        End Get
        Set(value As Boolean)
            bIncludeSaves = value
        End Set
    End Property

    Property IncludeConfigs As Boolean
        Get
            Return bIncludeConfigs
        End Get
        Set(value As Boolean)
            bIncludeConfigs = value
        End Set
    End Property

    Property IncludeOS As eSupportedOS
        Get
            Return eIncludeOS
        End Get
        Set(value As eSupportedOS)
            eIncludeOS = value
        End Set
    End Property

    Sub New()
        IncludeSaves = False
        IncludeConfigs = False
        IncludeOS = eSupportedOS.None
    End Sub
End Class
