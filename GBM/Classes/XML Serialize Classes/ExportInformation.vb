Public Class ExportInformation
    Private dExported As Int64
    Private iAppVer As Integer

    Property Exported As Int64
        Set(value As Int64)
            dExported = value
        End Set
        Get
            Return dExported
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
        iAppVer = 0
    End Sub

    Public Sub New(ByVal dInitExported As Int64, ByVal iInitAppVer As Integer)
        dExported = dInitExported
        iAppVer = iInitAppVer
    End Sub
End Class
