<Serializable()>
Public Class clsProcess
    Private sProcessID As String = Guid.NewGuid.ToString
    Private sName As String = String.Empty
    Private sPath As String = String.Empty
    Private sArgs As String = String.Empty
    Private bKill As Boolean = True

    Public Property ID As String
        Get
            Return sProcessID
        End Get
        Set(value As String)
            sProcessID = value
        End Set
    End Property

    Public Property Name As String
        Get
            Return sName
        End Get
        Set(value As String)
            sName = value
        End Set
    End Property

    Public Property Path As String
        Get
            Return sPath
        End Get
        Set(value As String)
            sPath = value
        End Set
    End Property

    Public Property Args As String
        Get
            Return sArgs
        End Get
        Set(value As String)
            sArgs = value
        End Set
    End Property

    Public Property Kill As Boolean
        Get
            Return bKill
        End Get
        Set(value As Boolean)
            bKill = value
        End Set
    End Property
End Class
