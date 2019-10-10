Imports System.Xml.Serialization

<XmlRoot("GBM_Backup")>
Public Class BackupMetadata
    Private iAppVer As Integer
    Private iDateUpdated As Int64
    Private sUpdatedBy As String
    Private oGame As Game

    <XmlAttribute("AppVer")>
    Property AppVer As Integer
        Set(value As Integer)
            iAppVer = value
        End Set
        Get
            Return iAppVer
        End Get
    End Property

    <XmlAttribute("DateUpdated")>
    Property DateUpdated As Int64
        Get
            Return iDateUpdated
        End Get
        Set(value As Int64)
            iDateUpdated = value
        End Set
    End Property

    <XmlAttribute("UpdatedBy")>
    Property UpdatedBy As String
        Get
            Return sUpdatedBy
        End Get
        Set(value As String)
            sUpdatedBy = value
        End Set
    End Property

    <XmlElement("GameData")>
    Property Game As Game
        Set(value As Game)
            oGame = value
        End Set
        Get
            Return oGame
        End Get
    End Property

    Public Sub New()
        iAppVer = 0
        iDateUpdated = mgrCommon.DateToUnix(Date.Now)
        sUpdatedBy = String.Empty
        oGame = New Game
    End Sub

    Public Sub New(ByVal iInitAppVer As Integer, ByVal iInitDateUpdated As Int64, ByVal sInitUpdatedBy As String, ByVal oInitGame As Game)
        iAppVer = iInitAppVer
        iDateUpdated = iInitDateUpdated
        sUpdatedBy = sInitUpdatedBy
        oGame = oInitGame
    End Sub
End Class
