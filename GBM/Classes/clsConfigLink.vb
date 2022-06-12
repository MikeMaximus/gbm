Public Class clsConfigLink
    Property MonitorID As String = String.Empty
    Property LinkID As String = String.Empty

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

    Sub New()
        MonitorID = String.Empty
        LinkID = String.Empty
    End Sub

    Sub New(sMonitorID As String, sLinkID As String)
        MonitorID = sMonitorID
        LinkID = sLinkID
    End Sub
End Class
