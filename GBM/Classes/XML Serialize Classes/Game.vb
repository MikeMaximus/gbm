Public Class Game
    Public Property ID As String
    Public Property Name As String
    Public Property ProcessName As String
    Public Property Parameter As String
    Public Property Path As String
    Public Property FolderSave As Boolean
    Public Property AppendTimeStamp As Boolean
    Public Property BackupLimit As Integer
    Public Property FileType As String
    Public Property ExcludeList As String
    Public Property MonitorOnly As Boolean
    Public Property Comments As String
    Public Property IsRegEx As Boolean
    Public Property RecurseSubFolders As Boolean
    Public Property OS As clsGame.eOS
    Public Property UseWindowTitle As Boolean
    Public Property Differential As Boolean
    Public Property DiffInterval As Integer
    Public Property Tags As List(Of Tag)
    Public Property ConfigLinks As List(Of ConfigLink)

    Public Function ConvertClass() As clsGame
        Dim oGame As New clsGame

        oGame.ID = ID
        oGame.Name = Name
        oGame.ProcessName = ProcessName
        oGame.Parameter = Parameter
        oGame.Path = Path
        oGame.FolderSave = FolderSave
        oGame.AppendTimeStamp = AppendTimeStamp
        oGame.BackupLimit = BackupLimit
        oGame.FileType = FileType
        oGame.ExcludeList = ExcludeList
        oGame.MonitorOnly = MonitorOnly
        oGame.Comments = Comments
        oGame.IsRegEx = IsRegEx
        oGame.RecurseSubFolders = RecurseSubFolders
        oGame.OS = OS
        oGame.UseWindowTitle = UseWindowTitle
        oGame.Differential = Differential
        oGame.DiffInterval = DiffInterval
        oGame.ImportTags = Tags
        oGame.ImportConfigLinks = ConfigLinks

        Return oGame
    End Function
End Class
