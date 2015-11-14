Public Class clsGameTag
    Private sTagID As String = String.Empty
    Private sMonitorID As String = String.Empty

    Property TagID As String
        Get
            Return sTagID
        End Get
        Set(value As String)
            sTagID = value
        End Set
    End Property

    Property MonitorID As String
        Get
            Return sMonitorID
        End Get
        Set(value As String)
            sMonitorID = value
        End Set
    End Property

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

End Class
