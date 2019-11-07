Public Class clsConfigLink
    Private sMonitorID As String = String.Empty
    Private sLinkID As String = String.Empty

    Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property

    Property LinkID As String
        Get
            Return sLinkID
        End Get
        Set(value As String)
            sLinkID = value
        End Set
    End Property

    Public Function CoreEquals(obj As Object) As Boolean
        Dim oConfigLink As clsConfigLink = TryCast(obj, clsConfigLink)
        If oConfigLink Is Nothing Then
            Return False
        Else
            If MonitorID <> oConfigLink.MonitorID Then
                Return False
            End If
            If LinkID <> oConfigLink.LinkID Then
                Return False
            End If
            Return True
        End If
    End Function
End Class
