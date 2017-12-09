Imports System.Xml.Serialization

<XmlRoot("gbm")>
Public Class ExportData
    Dim oConfigs As List(Of Game)
    Private dExported As Int64
    Private iTotalConfigs As Integer
    Private iAppVer As Integer

    <XmlAttribute("Exported")>
    Property Exported As Int64
        Set(value As Int64)
            dExported = value
        End Set
        Get
            Return dExported
        End Get
    End Property

    <XmlAttribute("TotalConfigurations")>
    Property TotalConfigurations As Integer
        Set(value As Integer)
            iTotalConfigs = value
        End Set
        Get
            Return iTotalConfigs
        End Get
    End Property

    <XmlAttribute("AppVer")>
    Property AppVer As Integer
        Set(value As Integer)
            iAppVer = value
        End Set
        Get
            Return iAppVer
        End Get
    End Property

    <XmlElement("Game")>
    Property Configurations As List(Of Game)
        Set(value As List(Of Game))
            oConfigs = value
        End Set
        Get
            Return oConfigs
        End Get
    End Property

    Public Sub New()
        dExported = 0
        iTotalConfigs = 0
        iAppVer = 0
        oConfigs = New List(Of Game)
    End Sub

    Public Sub New(ByVal dInitExported As Int64, ByVal iInitTotalConfigs As Integer, ByVal iInitAppVer As Integer, ByVal oInitConfigs As List(Of Game))
        dExported = dInitExported
        iTotalConfigs = iInitTotalConfigs
        iAppVer = iInitAppVer
        oConfigs = oInitConfigs
    End Sub
End Class
