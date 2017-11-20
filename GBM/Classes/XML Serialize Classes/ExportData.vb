Public Class ExportData
    Dim oExportInformation As ExportInformation
    Dim oConfigs As List(Of Game)

    Property Information As ExportInformation
        Set(value As ExportInformation)
            oExportInformation = value
        End Set
        Get
            Return oExportInformation
        End Get
    End Property

    Property Configurations As List(Of Game)
        Set(value As List(Of Game))
            oConfigs = value
        End Set
        Get
            Return oConfigs
        End Get
    End Property

    Public Sub New()
        oExportInformation = New ExportInformation()
        oConfigs = New List(Of Game)
    End Sub

    Public Sub New(ByVal oInitExportInformation As ExportInformation, ByVal oInitConfigs As List(Of Game))
        oExportInformation = oInitExportInformation
        oConfigs = oInitConfigs
    End Sub
End Class
