Public Class LudusaviGame
    Private oFiles As Dictionary(Of String, LudusaviPath)
    Private oRegistry As Dictionary(Of String, LudusaviPath)
    Private oInstallDir As Dictionary(Of String, Dictionary(Of String, String))
    Private oSteam As LudusaviSteam

    Property files As Dictionary(Of String, LudusaviPath)
        Get
            Return oFiles
        End Get
        Set(value As Dictionary(Of String, LudusaviPath))
            oFiles = value
        End Set
    End Property

    Property registry As Dictionary(Of String, LudusaviPath)
        Get
            Return oRegistry
        End Get
        Set(value As Dictionary(Of String, LudusaviPath))
            oRegistry = value
        End Set
    End Property

    Property installDir As Dictionary(Of String, Dictionary(Of String, String))
        Get
            Return oInstallDir
        End Get
        Set(value As Dictionary(Of String, Dictionary(Of String, String)))
            oInstallDir = value
        End Set
    End Property

    Property steam As LudusaviSteam
        Get
            Return oSteam
        End Get
        Set(value As LudusaviSteam)
            oSteam = value
        End Set
    End Property
End Class

Public Class LudusaviPath
    Private oTags As String()
    Private oWhen As LudusaviWhen()

    Property tags As String()
        Get
            Return oTags
        End Get
        Set(value As String())
            oTags = value
        End Set
    End Property

    Property [when] As LudusaviWhen()
        Get
            Return oWhen
        End Get
        Set(value As LudusaviWhen())
            oWhen = value
        End Set
    End Property
End Class

Public Class LudusaviWhen
    Private sOS As String
    Private sStore As String

    Property os As String
        Get
            Return sOS
        End Get
        Set(value As String)
            sOS = value
        End Set
    End Property

    Property store As String
        Get
            Return sStore
        End Get
        Set(value As String)
            sStore = value
        End Set
    End Property
End Class

Public Class LudusaviSteam
    Private iSteamID As Integer

    Property id As Integer
        Get
            Return iSteamID
        End Get
        Set(value As Integer)
            iSteamID = value
        End Set
    End Property
End Class