Public Class clsGameTag
    Public Property TagID As String
    Public Property MonitorID As String

    Public Function CoreEquals(obj As Object) As Boolean
        Dim oGameTag As clsGameTag = TryCast(obj, clsGameTag)
        If oGameTag Is Nothing Then
            Return False
        Else
            If TagID <> oGameTag.TagID Then
                Return False
            End If
            If MonitorID <> oGameTag.MonitorID Then
                Return False
            End If
            Return True
        End If
    End Function

    Sub New()
        TagID = String.Empty
        MonitorID = String.Empty
    End Sub

    Sub New(sTagID As String, sMonitorID As String)
        TagID = sTagID
        MonitorID = sMonitorID
    End Sub
End Class
