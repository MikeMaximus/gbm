Public Class clsBackup
    Inherits clsGameBase

    Private sBackupID As String = Guid.NewGuid.ToString
    Private sMonitorID As String = String.Empty
    Private sFileName As String = String.Empty
    Private sRelativeRestorePath As String = String.Empty
    Private dDateUpdated As DateTime = Date.Now
    Private sUpdatedBy As String = String.Empty
    Private sCheckSum As String = String.Empty

    Property ManifestID As String
        Get
            Return sBackupID
        End Get
        Set(value As String)
            sBackupID = value
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

    Property FileName As String
        Get
            If mgrCommon.IsUnix Then
                Return sFileName.Replace("\", "/")
            Else
                Return sFileName.Replace("/", "\")
            End If
        End Get
        Set(value As String)
            sFileName = value
        End Set
    End Property

    Property RelativeRestorePath As String
        Get
            Return sRelativeRestorePath
        End Get
        Set(value As String)
            sRelativeRestorePath = value
        End Set
    End Property

    Property DateUpdated As DateTime
        Get
            Return dDateUpdated
        End Get
        Set(value As DateTime)
            dDateUpdated = value
        End Set
    End Property

    ReadOnly Property DateUpdatedUnix As Int64
        Get
            Return mgrCommon.DateToUnix(DateUpdated)
        End Get
    End Property

    Property UpdatedBy As String
        Get
            Return sUpdatedBy
        End Get
        Set(value As String)
            sUpdatedBy = value
        End Set
    End Property

    Property CheckSum As String
        Get
            Return sCheckSum
        End Get
        Set(value As String)
            sCheckSum = value
        End Set
    End Property
End Class
