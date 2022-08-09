Imports System.Xml.Serialization

<XmlRoot("GBM_Backup")>
Public Class BackupMetadata
    <XmlAttribute("AppVer")>
    Public Property AppVer As Integer
    <XmlElement("BackupData")>
    Public Property Backup As Backup
    <XmlElement("GameData")>
    Public Property Game As Game

    Public Function CreateBackupInfo(ByVal sDefaultUpdatedBy As String, ByVal dDefaultDate As DateTime) As clsBackup
        Dim oBackup As New clsBackup

        oBackup.MonitorID = Game.ID
        oBackup.DateUpdated = dDefaultDate
        oBackup.UpdatedBy = sDefaultUpdatedBy

        If AppVer > 128 Then
            oBackup.ManifestID = Backup.ManifestID
            oBackup.DateUpdated = mgrCommon.UnixToDate(Backup.DateUpdated)
            oBackup.UpdatedBy = Backup.UpdatedBy
            oBackup.IsDifferentialParent = Backup.IsDifferentialParent
            oBackup.DifferentialParent = Backup.DifferentialParent
        End If

        Return oBackup
    End Function

    Public Sub New()
        AppVer = 0
        Backup = New Backup
        Game = New Game
    End Sub

    Public Sub New(iAppVer As Integer, oBackup As Backup, oGame As Game)
        AppVer = iAppVer
        Backup = oBackup
        Game = oGame
    End Sub
End Class
