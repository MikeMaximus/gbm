Imports System.Xml.Serialization

<XmlRoot("gbm")>
Public Class ExportData
    <XmlAttribute("Exported")>
    Public Property Exported As Int64
    <XmlAttribute("TotalConfigurations")>
    Public Property TotalConfigurations As Integer
    <XmlAttribute("AppVer")>
    Public Property AppVer As Integer
    <XmlElement("Game")>
    Public Property Configurations As List(Of Game)

    Public Sub New()
        Exported = 0
        TotalConfigurations = 0
        AppVer = 0
        Configurations = New List(Of Game)
    End Sub

    Public Sub New(ByVal dExported As Int64, ByVal iTotalConfigs As Integer, ByVal iAppVer As Integer, ByVal oConfigs As List(Of Game))
        Exported = dExported
        TotalConfigurations = iTotalConfigs
        AppVer = iAppVer
        Configurations = oConfigs
    End Sub
End Class
