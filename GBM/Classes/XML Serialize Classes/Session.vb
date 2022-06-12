Imports System.Xml.Serialization

<XmlRoot("Session")>
Public Class Session
    <XmlElement("Game")>
    Public Property GameName As String
    <XmlElement("Start")>
    Public Property StartDate As String
    <XmlElement("End")>
    Public Property EndDate As String
    <XmlElement("Hours")>
    Public Property Hours As String
End Class
