Imports System.Xml.Serialization

<XmlRoot("Session")>
Public Class Session
    Private sGame As String
    Private sStart As String
    Private sEnd As String
    Private sHours As String

    <XmlElement("Game")>
    Public Property GameName As String
        Set(value As String)
            sGame = value
        End Set
        Get
            Return sGame
        End Get
    End Property

    <XmlElement("Start")>
    Public Property StartDate As String
        Set(value As String)
            sStart = value
        End Set
        Get
            Return sStart
        End Get
    End Property

    <XmlElement("End")>
    Public Property EndDate As String
        Set(value As String)
            sEnd = value
        End Set
        Get
            Return sEnd
        End Get
    End Property

    <XmlElement("Hours")>
    Public Property Hours As String
        Set(value As String)
            sHours = value
        End Set
        Get
            Return sHours
        End Get
    End Property
End Class
