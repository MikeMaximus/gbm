Public Class LudusaviGame
    Public Property files As Dictionary(Of String, LudusaviPath)
    Public Property registry As Dictionary(Of String, LudusaviPath)
    Public Property launch As Dictionary(Of String, List(Of LudusaviLaunch))
    Public Property installDir As Dictionary(Of String, Dictionary(Of String, String))
    Public Property steam As LudusaviSteam
End Class
