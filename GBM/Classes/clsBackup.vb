Public Class clsBackup
    Inherits clsGame

    Private sFileName As String

    Public Property ManifestID As String
    Public Property MonitorID As String
    Public Property FileName As String
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
    Public Property RelativeRestorePath As String
    Public Property DateUpdated As DateTime = Date.Now
    Public ReadOnly Property DateUpdatedUnix As Int64
        Get
            Return mgrCommon.DateToUnix(DateUpdated)
        End Get
    End Property
    Public Property UpdatedBy As String
    Public Property CheckSum As String
    Public Property IsDifferentialParent As Boolean
    Public Property DifferentialParent As String

    Public Function ConvertToBackup() As Backup
        Dim oBackup As New Backup

        oBackup.ManifestID = ManifestID
        oBackup.DateUpdated = DateUpdatedUnix
        oBackup.UpdatedBy = UpdatedBy
        oBackup.IsDifferentialParent = IsDifferentialParent
        oBackup.DifferentialParent = DifferentialParent

        Return oBackup
    End Function

    Sub New()
        ManifestID = Guid.NewGuid.ToString
        MonitorID = String.Empty
        FileName = String.Empty
        RelativeRestorePath = String.Empty
        DateUpdated = Date.Now
        UpdatedBy = String.Empty
        CheckSum = String.Empty
        IsDifferentialParent = False
        DifferentialParent = String.Empty
    End Sub

    Sub New(sManifestID As String, sMonitorID As String, sFileName As String, sRelativeRestorePath As String, dDateUpdated As DateTime, sUpdatedBy As String, sCheckSum As String,
            bIsDifferentialParent As Boolean, sDifferentialParent As String)
        ManifestID = sManifestID
        MonitorID = sMonitorID
        FileName = sFileName
        RelativeRestorePath = sRelativeRestorePath
        DateUpdated = dDateUpdated
        UpdatedBy = sUpdatedBy
        CheckSum = sCheckSum
        IsDifferentialParent = bIsDifferentialParent
        DifferentialParent = sDifferentialParent
    End Sub
End Class
