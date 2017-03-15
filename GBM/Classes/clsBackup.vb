Public Class clsBackup
    Private sBackupID As String = Guid.NewGuid.ToString
    Private sName As String = String.Empty
    Private sFileName As String = String.Empty
    Private sRestorePath As String = String.Empty
    Private bAbsolutePath As Boolean = False
    Private sRelativeRestorePath As String = String.Empty
    Private dDateUpdated As DateTime = Date.Now
    Private sUpdatedBy As String = String.Empty
    Private sCheckSum As String = String.Empty

    Property ID As String
        Get
            Return sBackupID
        End Get
        Set(value As String)
            sBackupID = value
        End Set
    End Property

    Property Name As String
        Get
            Return sName
        End Get
        Set(value As String)
            sName = value
        End Set
    End Property

    ReadOnly Property CroppedName As String
        Get
            If Name.Length > 40 Then
                Return sName.Substring(0, 41).Trim & "..."
            Else
                Return sName
            End If
        End Get
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

    ReadOnly Property TruePath As String
        Get
            Return sRestorePath
        End Get
    End Property

    Property RestorePath As String
        Get
            Return mgrPath.ReplaceSpecialPaths(sRestorePath)
        End Get
        Set(value As String)
            sRestorePath = mgrPath.ReverseSpecialPaths(value)
        End Set
    End Property

    Property AbsolutePath As Boolean
        Get
            Return bAbsolutePath
        End Get
        Set(value As Boolean)
            bAbsolutePath = value
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
