Imports System.Text.RegularExpressions

Public Class clsGame
    Inherits clsGameBase

    Private oImportTags As New List(Of Tag)
    Private oImportConfigLinks As New List(Of ConfigLink)
    Private bImportUpdate As Boolean = False
    Private oCompiledRegEx As Regex

    Property ImportTags As List(Of Tag)
        Get
            Return oImportTags
        End Get
        Set(value As List(Of Tag))
            oImportTags = value
        End Set
    End Property

    Property ImportConfigLinks As List(Of ConfigLink)
        Get
            Return oImportConfigLinks
        End Get
        Set(value As List(Of ConfigLink))
            oImportConfigLinks = value
        End Set
    End Property

    Property ImportUpdate As Boolean
        Get
            Return bImportUpdate
        End Get
        Set(value As Boolean)
            bImportUpdate = value
        End Set
    End Property

    Property CompiledRegEx As Regex
        Get
            Return oCompiledRegEx
        End Get
        Set(value As Regex)
            oCompiledRegEx = value
        End Set
    End Property

    Public Function ConvertClass() As Game
        Dim oGame As New Game

        oGame.ID = ID
        oGame.Name = Name
        oGame.ProcessName = ProcessName
        oGame.Parameter = Parameter
        oGame.Path = TruePath
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
        oGame.Tags = mgrGameTags.GetTagsByGameForExport(ID)
        oGame.ConfigLinks = mgrConfigLinks.GetConfigLinksByGameForExport(ID)

        Return oGame
    End Function

    Public Function SyncEquals(obj As Object, eSyncFields As eOptionalSyncFields) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            'Core Sync Fields
            If ID <> oGame.ID Then
                Return False
            End If
            If Name <> oGame.Name Then
                Return False
            End If
            If ProcessName <> oGame.ProcessName Then
                Return False
            End If
            If Parameter <> oGame.Parameter Then
                Return False
            End If
            If Path <> oGame.Path Then
                Return False
            End If
            If FileType <> oGame.FileType Then
                Return False
            End If
            If ExcludeList <> oGame.ExcludeList Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
                Return False
            End If
            If AppendTimeStamp <> oGame.AppendTimeStamp Then
                Return False
            End If
            If BackupLimit <> oGame.BackupLimit Then
                Return False
            End If
            If CleanFolder <> oGame.CleanFolder Then
                Return False
            End If
            If Hours <> oGame.Hours Then
                Return False
            End If
            If MonitorOnly <> oGame.MonitorOnly Then
                Return False
            End If
            If Comments <> oGame.Comments Then
                Return False
            End If
            If IsRegEx <> oGame.IsRegEx Then
                Return False
            End If
            If RecurseSubFolders <> oGame.RecurseSubFolders Then
                Return False
            End If
            If OS <> oGame.OS Then
                Return False
            End If

            'Optional Sync Fields
            If (eSyncFields And eOptionalSyncFields.Company) = eOptionalSyncFields.Company Then
                If Company <> oGame.Company Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.GamePath) = eOptionalSyncFields.GamePath Then
                If ProcessPath <> oGame.ProcessPath Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.Icon) = eOptionalSyncFields.Icon Then
                If Icon <> oGame.Icon Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.MonitorGame) = eOptionalSyncFields.MonitorGame Then
                If Enabled <> oGame.Enabled Then
                    Return False
                End If
            End If
            If (eSyncFields And eOptionalSyncFields.Version) = eOptionalSyncFields.Version Then
                If Version <> oGame.Version Then
                    Return False
                End If
            End If
            Return True
        End If
    End Function

    Public Function CoreEquals(obj As Object) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            'Core Fields
            If ID <> oGame.ID Then
                Return False
            End If
            If Name <> oGame.Name Then
                Return False
            End If
            If ProcessName <> oGame.ProcessName Then
                Return False
            End If
            If Parameter <> oGame.Parameter Then
                Return False
            End If
            If Path <> oGame.Path Then
                Return False
            End If
            If FileType <> oGame.FileType Then
                Return False
            End If
            If ExcludeList <> oGame.ExcludeList Then
                Return False
            End If
            If FolderSave <> oGame.FolderSave Then
                Return False
            End If
            If AppendTimeStamp <> oGame.AppendTimeStamp Then
                Return False
            End If
            If MonitorOnly <> oGame.MonitorOnly Then
                Return False
            End If
            If Comments <> oGame.Comments Then
                Return False
            End If
            If IsRegEx <> oGame.IsRegEx Then
                Return False
            End If
            If RecurseSubFolders <> oGame.RecurseSubFolders Then
                Return False
            End If
            If OS <> oGame.OS Then
                Return False
            End If
            Return True
        End If
    End Function

    Public Function MinimalEquals(obj As Object) As Boolean
        Dim oGame As clsGame = TryCast(obj, clsGame)
        If oGame Is Nothing Then
            Return False
        Else
            If ID <> oGame.ID Then
                Return False
            End If
            Return True
        End If
    End Function

    Public Function ShallowCopy() As clsGame
        Return DirectCast(Me.MemberwiseClone(), clsGame)
    End Function

    Public Shared Function SetSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields Or eSyncField
    End Function

    Public Shared Function RemoveSyncField(ByVal eSyncFields As eOptionalSyncFields, ByVal eSyncField As eOptionalSyncFields) As eOptionalSyncFields
        Return eSyncFields And (Not eSyncField)
    End Function

End Class
