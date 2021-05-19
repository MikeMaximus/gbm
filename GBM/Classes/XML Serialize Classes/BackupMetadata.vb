Imports System.Xml.Serialization

<XmlRoot("GBM_Backup")>
Public Class BackupMetadata
    Private iAppVer As Integer
    Private oBackup As Backup
    Private oGame As Game

    <XmlAttribute("AppVer")>
    Property AppVer As Integer
        Set(value As Integer)
            iAppVer = value
        End Set
        Get
            Return iAppVer
        End Get
    End Property

    <XmlElement("BackupData")>
    Property Backup As Backup
        Set(value As Backup)
            oBackup = value
        End Set
        Get
            Return oBackup
        End Get
    End Property

    <XmlElement("GameData")>
    Property Game As Game
        Set(value As Game)
            oGame = value
        End Set
        Get
            Return oGame
        End Get
    End Property

    Public Sub New()
        iAppVer = 0
        oBackup = New Backup
        oGame = New Game
    End Sub

    Public Sub New(ByVal iInitAppVer As Integer, ByVal oInitBackup As Backup, ByVal oInitGame As Game)
        iAppVer = iInitAppVer
        oBackup = oInitBackup
        oGame = oInitGame
    End Sub

    Public Function CreateBackupInfo(ByVal sDefaultUpdatedBy As String, ByVal dDefaultDate As DateTime) As clsBackup
        Dim oBackup As New clsBackup

        oBackup.MonitorID = Game.ID
        oBackup.DateUpdated = dDefaultDate
        oBackup.UpdatedBy = sDefaultUpdatedBy

        If AppVer >= 128 Then
            oBackup.ManifestID = Backup.ManifestID
            oBackup.DateUpdated = mgrCommon.UnixToDate(Backup.DateUpdated)
            oBackup.UpdatedBy = Backup.UpdatedBy
            oBackup.IsDifferentialParent = Backup.IsDifferentialParent
            oBackup.DifferentialParent = Backup.DifferentialParent
        End If

        Return oBackup
    End Function
End Class
