Public Class ExportInformation
    Private dExported As Int64
    Private iTotalConfigs As Integer
    Private iAppVer As Integer

    Property Exported As Int64
        Set(value As Int64)
            dExported = value
        End Set
        Get
            Return dExported
        End Get
    End Property

    Property TotalConfigurations As Integer
        Set(value As Integer)
            iTotalConfigs = value
        End Set
        Get
            Return iTotalConfigs
        End Get
    End Property

    Property AppVer As Integer
        Set(value As Integer)
            iAppVer = value
        End Set
        Get
            Return iAppVer
        End Get
    End Property

    Public Sub New()
        dExported = 0
        iTotalConfigs = 0
        iAppVer = 0
    End Sub

    Public Sub New(ByVal dInitExported As Int64, ByVal iInitTotalConfigs As Integer, ByVal iInitAppVer As Integer)
        dExported = dInitExported
        iTotalConfigs = iInitTotalConfigs
        iAppVer = iInitAppVer
    End Sub
End Class
