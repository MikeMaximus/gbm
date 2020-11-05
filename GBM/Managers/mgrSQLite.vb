Imports GBM.My.Resources
Imports System.IO
Imports Mono.Data.Sqlite

Public Class mgrSQLite

    Public Enum Database As Integer
        Local = 1
        Remote = 2
    End Enum

    Private sDatabaseLocation As String
    Private sConnectString As String
    Private eDatabase As Database
    Private db As SQLiteConnection

    Public Sub New(ByVal eSelectDB As Database)
        Select Case eSelectDB
            Case Database.Local
                eDatabase = Database.Local
                sDatabaseLocation = mgrPath.DatabaseLocation
                sConnectString = "Data Source=" & mgrPath.DatabaseLocation & ";Version=3;"
            Case Database.Remote
                eDatabase = Database.Remote
                sDatabaseLocation = mgrPath.RemoteDatabaseLocation
                sConnectString = "Data Source=" & mgrPath.RemoteDatabaseLocation & ";Version=3;"
        End Select
    End Sub

    Public Sub BackupDB(ByVal sDescription As String, Optional ByVal bOverwrite As Boolean = False)
        Dim sNewFile As String = String.Empty

        Try
            Select Case eDatabase
                Case Database.Local
                    sNewFile = mgrPath.DatabaseLocation & "." & sDescription & ".bak"
                    File.Copy(mgrPath.DatabaseLocation, sNewFile, bOverwrite)
                Case Database.Remote
                    sNewFile = mgrPath.RemoteDatabaseLocation & "." & sDescription & ".bak"
                    File.Copy(mgrPath.RemoteDatabaseLocation, sNewFile, bOverwrite)
            End Select
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorBackupFailure, New String() {sNewFile, ex.Message}, MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Public Function CheckDBVer(Optional ByRef iDBVer As Integer = 0) As Boolean
        iDBVer = GetDatabaseVersion()

        If iDBVer > mgrCommon.AppVersion Then
            Return False
        End If

        Return True
    End Function

    Public Function CheckDB() As Boolean
        If File.Exists(sDatabaseLocation) Then
            Return True
        End If
        Return False
    End Function

    Private Function CreateLocalDatabase() As Boolean
        Dim sSql As String

        Try
            'Create the DB
            SQLiteConnection.CreateFile(sDatabaseLocation)

            'Add Tables (Settings)
            sSql = "CREATE TABLE settings (SettingsID INTEGER NOT NULL PRIMARY KEY, MonitorOnStartup BOOLEAN NOT NULL, StartToTray BOOLEAN NOT NULL, ShowDetectionToolTips BOOLEAN NOT NULL, " &
                   "DisableConfirmation BOOLEAN NOT NULL, CreateSubFolder BOOLEAN NOT NULL, ShowOverwriteWarning BOOLEAN NOT NULL, RestoreOnLaunch BOOLEAN NOT NULL, " &
                   "BackupFolder TEXT NOT NULL, StartWithWindows BOOLEAN NOT NULL, TimeTracking BOOLEAN NOT NULL, " &
                   "SuppressBackup BOOLEAN NOT NULL, SuppressBackupThreshold INTEGER NOT NULL, CompressionLevel INTEGER NOT NULL, Custom7zArguments TEXT, " &
                   "Custom7zLocation TEXT, SyncFields INTEGER NOT NULL, AutoSaveLog BOOLEAN NOT NULL, AutoRestore BOOLEAN NOT NULL, AutoMark BOOLEAN NOT NULL, SessionTracking BOOLEAN NOT NULL, " &
                   "SuppressMessages INTEGER NOT NULL, BackupOnLaunch BOOLEAN NOT NULL, UseGameID BOOLEAN NOT NULL, DisableSyncMessages BOOLEAN NOT NULL, ShowResolvedPaths BOOLEAN NOT NULL, " &
                   "DisableDiskSpaceCheck BOOLEAN NOT NULL, TemporaryFolder TEXT, ExitOnClose BOOLEAN NOT NULL, ExitNoWarning BOOLEAN NOT NULL);"
            'Add Tables (SavedPath)
            sSql &= "CREATE TABLE savedpath (PathName TEXT NOT NULL PRIMARY KEY, Path TEXT NOT NULL);"

            'Add Tables (Monitor List)
            sSql &= "CREATE TABLE monitorlist (MonitorID TEXT NOT NULL PRIMARY KEY, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, " &
                   "AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, " &
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, " &
                   "BackupLimit INTEGER NOT NULL, CleanFolder BOOLEAN NOT NULL, Parameter TEXT, Comments TEXT, IsRegEx BOOLEAN NOT NULL, RecurseSubFolders BOOLEAN NOT NULL, " &
                   "OS INTEGER NOT NULL);"

            'Add Tables (Tags)
            sSql &= "CREATE TABLE tags (TagID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY); "

            'Add Tables (Game Tags)
            sSql &= "CREATE TABLE gametags (TagID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(TagID, MonitorID)); "

            'Add Tables (Variables)
            sSql &= "CREATE TABLE variables (VariableID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, Path TEXT NOT NULL);"

            'Add Tables (Local Manifest)
            sSql &= "CREATE TABLE manifest (ManifestID TEXT NOT NULL PRIMARY KEY, MonitorID TEXT NOT NULL, FileName TEXT NOT NULL, " &
                   "DateUpdated TEXT NOT NULL, UpdatedBy TEXT NOT NULL, CheckSum TEXT);"

            'Add Tables (Sessions)
            sSql &= "CREATE TABLE sessions (MonitorID TEXT NOT NULL, Start INTEGER NOT NULL, End INTEGER NOT NULL, PRIMARY KEY(MonitorID, Start));"

            'Add Tables (Processes)
            sSql &= "CREATE TABLE processes (ProcessID TEXT NOT NULL PRIMARY KEY, Name Text NOT NULL, Path TEXT NOT NULL, Args TEXT, Kill BOOLEAN NOT NULL);"

            'Add Tables (Game Processes)
            sSql &= "CREATE TABLE gameprocesses (ProcessID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(ProcessID, MonitorID));"

            'Add Tables (Wine Data)
            sSql &= "CREATE TABLE winedata (MonitorID TEXT NOT NULL PRIMARY KEY, Prefix TEXT NOT NULL, SavePath TEXT NOT NULL, BinaryPath TEXT NOT NULL);"

            'Add Tables (Config Links)
            sSql &= "CREATE TABLE configlinks (MonitorID TEXT NOT NULL, LinkID TEXT NOT NULL, PRIMARY KEY(MonitorID, LinkID)); "

            'Add Tables (Launchers)
            sSql &= "CREATE TABLE launchers (LauncherID	TEXT NOT NULL PRIMARY KEY, Name	TEXT NOT NULL, LaunchString	TEXT NOT NULL);"

            'Add Tables (Launch Data)
            sSql &= "CREATE TABLE launchdata (MonitorID	TEXT NOT NULL PRIMARY KEY, Path	TEXT NOT NULL, Args TEXT NOT NULL, LauncherID TEXT NOT NULL, LauncherGameID	TEXT NOT NULL);"

            'Set Version
            sSql &= "PRAGMA user_version=" & mgrCommon.AppVersion

            RunParamQuery(sSql, New Hashtable)
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorCreatingLocalDB, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

    Private Function CreateRemoteDatabase() As Boolean
        Dim sSql As String

        Try
            'Create the DB
            SqliteConnection.CreateFile(sDatabaseLocation)

            'Add Tables (Remote Monitor List)
            sSql = "CREATE TABLE monitorlist (MonitorID TEXT NOT NULL PRIMARY KEY, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, " &
                   "AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, " &
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, " &
                   "BackupLimit INTEGER NOT NULL, CleanFolder BOOLEAN NOT NULL, Parameter TEXT, Comments TEXT, IsRegEx BOOLEAN NOT NULL, RecurseSubFolders BOOLEAN NOT NULL, " &
                   "OS INTEGER NOT NULL);"

            'Add Tables (Remote Manifest)
            sSql &= "CREATE TABLE manifest (ManifestID TEXT Not NULL PRIMARY KEY, MonitorID TEXT Not NULL, FileName TEXT Not NULL, " &
                   "DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"

            'Add Tables (Remote Tags)
            sSql &= "CREATE TABLE tags (TagID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY); "

            'Add Tables (Remote Game Tags)
            sSql &= "CREATE TABLE gametags (TagID TEXT Not NULL, MonitorID TEXT Not NULL, PRIMARY KEY(TagID, MonitorID)); "

            'Add Tables (Config Links)
            sSql &= "CREATE TABLE configlinks (MonitorID TEXT NOT NULL, LinkID TEXT NOT NULL, PRIMARY KEY(MonitorID, LinkID)); "

            'Set Version
            sSql &= "PRAGMA user_version=" & mgrCommon.AppVersion

            RunParamQuery(sSql, New Hashtable)
            Return True
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorCreatingRemoteDB, ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

    Private Function CreateDB() As Boolean
        Dim bSuccess As Boolean

        Select Case eDatabase
            Case Database.Local
                bSuccess = CreateLocalDatabase()
            Case Database.Remote
                bSuccess = CreateRemoteDatabase()
        End Select

        Return bSuccess
    End Function

    Public Sub Connect()
        If CheckDB() Then
            db = New SqliteConnection(sConnectString)
            db.Open()
        Else
            CreateDB()
            db.Open()
        End If
    End Sub

    Public Sub Disconnect()
        db.Close()
    End Sub

    Private Sub RollBack(ByRef trans As SqliteTransaction)
        Try
            trans.Rollback()
        Catch
            'SQLite may or may not perform an auto-rollback when certain failures occur, such as disk full or out of memory.
            'Multiple rollbacks will cause an exception, therefore lets just do nothing when that happens.
        End Try
    End Sub

    Private Sub BuildParams(ByRef command As SqliteCommand, ByRef hshParams As Hashtable)
        For Each de As DictionaryEntry In hshParams
            command.Parameters.AddWithValue(de.Key, de.Value)
        Next
    End Sub

    Public Function RunParamQuery(ByVal sSQL As String, ByVal hshParams As Hashtable) As Boolean
        Dim trans As SqliteTransaction
        Dim command As SqliteCommand

        Connect()
        command = New SqliteCommand(sSQL, db)
        BuildParams(command, hshParams)
        trans = db.BeginTransaction()

        Try
            command.ExecuteNonQuery()
            trans.Commit()
        Catch ex As Exception
            RollBack(trans)
            mgrCommon.ShowMessage(mgrSQLite_ErrorQueryFailure, New String() {sSQL, ex.Message}, MsgBoxStyle.Exclamation)
            Return False
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return True
    End Function

    Public Function RunMassParamQuery(ByVal sSQL As String, ByVal oParamList As List(Of Hashtable)) As Boolean
        Dim trans As SqliteTransaction
        Dim command As SqliteCommand

        Connect()
        command = New SqliteCommand(sSQL, db)
        trans = db.BeginTransaction()

        Try
            For Each hshParams In oParamList
                BuildParams(command, hshParams)
                command.ExecuteNonQuery()
            Next
            trans.Commit()
        Catch ex As Exception
            RollBack(trans)
            mgrCommon.ShowMessage(mgrSQLite_ErrorQueryFailure, New String() {sSQL, ex.Message}, MsgBoxStyle.Exclamation)
            Return False
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return True
    End Function

    Public Function ReadParamData(ByVal sSQL As String, ByVal hshParams As Hashtable) As DataSet
        Dim adapter As SqliteDataAdapter
        Dim command As SqliteCommand
        Dim oData As New DataSet

        Connect()
        command = New SqliteCommand(sSQL, db)
        BuildParams(command, hshParams)

        Try
            adapter = New SqliteDataAdapter()
            adapter.SelectCommand = command
            adapter.Fill(oData)
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorQueryFailure, New String() {sSQL, ex.Message}, MsgBoxStyle.Exclamation)
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return oData
    End Function

    Public Function ReadSingleValue(ByVal sSQL As String, ByVal hshParams As Hashtable) As Object

        Dim command As SqliteCommand
        Dim oResult As New Object

        Connect()
        command = New SqliteCommand(sSQL, db)
        BuildParams(command, hshParams)

        Try
            oResult = command.ExecuteScalar()
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorQueryFailure, New String() {sSQL, ex.Message}, MsgBoxStyle.Information)
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return oResult
    End Function

    Private Function GetDatabaseVersion() As Integer
        Dim sSQL As String
        Dim iVer As Integer
        Dim oData As DataSet

        sSQL = "PRAGMA user_version"
        oData = ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            iVer = CInt(dr(0))
        Next

        Return iVer
    End Function

    Private Function FieldExists(ByVal sField As String, ByVal sTable As String) As Boolean
        Dim sSQL As String
        Dim sCurrentField As String
        Dim oData As DataSet

        sSQL = "PRAGMA table_info(" & sTable & ")"
        oData = ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            sCurrentField = CStr(dr(1))
            If sCurrentField = sField Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Sub UpgradeToUnixTime(ByVal sTable As String, ByVal sField As String, ByVal sKeyField As String)
        Dim sSQL As String
        Dim oData As DataSet
        Dim sID As String
        Dim dDate As DateTime
        Dim iDate As Int64
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM " & sTable
        oData = ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            hshParams.Clear()
            sID = CStr(dr(sKeyField))
            Try
                'We need to fallback if the date string cannot be converted
                dDate = CDate(dr(sField))
            Catch
                'Use the current date as a fallback
                dDate = Now
            End Try
            iDate = mgrCommon.DateToUnix(dDate)
            sSQL = "UPDATE " & sTable & " SET " & sField & "= @NewDate WHERE " & sKeyField & "= @OldID;"
            hshParams.Add("OldID", sID)
            hshParams.Add("NewDate", iDate)
            RunParamQuery(sSQL, hshParams)
        Next

    End Sub

    Public Sub UpgradeToGUID(ByVal sTable As String, ByVal sField As String)
        Dim sSQL As String
        Dim iCurrentID As Integer
        Dim oData As DataSet
        Dim hshParams As New Hashtable

        sSQL = "SELECT * FROM " & sTable
        oData = ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            hshParams.Clear()
            iCurrentID = CInt(dr(sField))
            sSQL = "UPDATE " & sTable & " SET " & sField & "= @NewID WHERE " & sField & "= @OldID;"
            hshParams.Add("OldID", iCurrentID)
            hshParams.Add("NewID", Guid.NewGuid.ToString)
            RunParamQuery(sSQL, hshParams)
        Next

    End Sub

    Public Sub DatabaseUpgrade()
        Dim sSQL As String

        '0.9 Upgrade
        If GetDatabaseVersion() < 90 Then
            BackupDB("v8")
            sSQL = "ALTER TABLE monitorlist ADD COLUMN MonitorOnly BOOLEAN Not NULL DEFAULT 0;"
            sSQL &= "PRAGMA user_version=90"
            RunParamQuery(sSQL, New Hashtable)
        End If

        '0.91 Upgrade
        If GetDatabaseVersion() < 91 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v84")

                'Overhaul Monitor List Table
                sSQL = "CREATE TABLE monitorlist_new (MonitorID TEXT Not NULL UNIQUE, Name TEXT Not NULL, Process TEXT Not NULL, Path TEXT, AbsolutePath BOOLEAN Not NULL, FolderSave BOOLEAN Not NULL, FileType TEXT, TimeStamp BOOLEAN Not NULL, ExcludeList TEXT Not NULL, ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN Not NULL, MonitorOnly BOOLEAN Not NULL, PRIMARY KEY(Name, Process));"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly) "
                sSQL &= "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist;"
                sSQL &= "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"

                'Overhaul Variables Table
                sSQL &= "CREATE TABLE variables_new (VariableID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY, Path TEXT Not NULL);"
                sSQL &= "INSERT INTO variables_new (VariableID, Name, Path) SELECT VariableID, Name, Path FROM variables;"
                sSQL &= "DROP TABLE variables; ALTER TABLE variables_new RENAME TO variables;"

                'Overhaul Manifest Table
                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY, FileName TEXT Not NULL, RestorePath TEXT Not NULL, AbsolutePath BOOLEAN Not NULL, DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy) "
                sSQL &= "SELECT ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy FROM manifest;"
                sSQL &= "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                'Add new settings
                sSQL &= "ALTER TABLE settings ADD COLUMN Sync BOOLEAN Not NULL DEFAULT 1;"
                sSQL &= "ALTER TABLE settings ADD COLUMN CheckSum BOOLEAN Not NULL DEFAULT 1;"
                sSQL &= "PRAGMA user_version=91"

                RunParamQuery(sSQL, New Hashtable)

                'Upgrade IDs to GUIDs
                UpgradeToGUID("monitorlist", "MonitorID")
                UpgradeToGUID("variables", "VariableID")
                UpgradeToGUID("manifest", "ManifestID")

                'Run a compact due to the large operations
                CompactDatabase()
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v84")

                'Overhaul Monitor List Table
                sSQL = "CREATE TABLE monitorlist_new (MonitorID TEXT Not NULL UNIQUE, Name TEXT Not NULL, Process TEXT Not NULL, Path TEXT, AbsolutePath BOOLEAN Not NULL, FolderSave BOOLEAN Not NULL, FileType TEXT, TimeStamp BOOLEAN Not NULL, ExcludeList TEXT Not NULL, ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN Not NULL, MonitorOnly BOOLEAN Not NULL, PRIMARY KEY(Name, Process));"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly) "
                sSQL &= "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist;"
                sSQL &= "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"

                'Overhaul Manifest Table
                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY, FileName TEXT Not NULL, RestorePath TEXT Not NULL, AbsolutePath BOOLEAN Not NULL, DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy) "
                sSQL &= "SELECT ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy FROM manifest;"
                sSQL &= "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"
                sSQL &= "PRAGMA user_version=91"

                RunParamQuery(sSQL, New Hashtable)

                'Upgrade IDs to GUIDs
                UpgradeToGUID("monitorlist", "MonitorID")
                UpgradeToGUID("manifest", "ManifestID")

                'Run a compact due to the large operations
                CompactDatabase()
            End If
        End If

        '0.92 Upgrade
        If GetDatabaseVersion() < 92 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v91")

                'Add new setting
                sSQL = "ALTER TABLE settings ADD COLUMN StartWithWindows BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "PRAGMA user_version=92"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v91")

                sSQL = "PRAGMA user_version=92"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.93 Upgrade
        If GetDatabaseVersion() < 93 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v92")

                UpgradeToUnixTime("manifest", "DateUpdated", "ManifestID")

                sSQL = "PRAGMA user_version=93"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v92")

                UpgradeToUnixTime("manifest", "DateUpdated", "ManifestID")

                sSQL = "PRAGMA user_version=93"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.94 Upgrade
        If GetDatabaseVersion() < 94 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v93")

                'Add Tags Tables
                sSQL = "CREATE TABLE tags (TagID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY); "
                sSQL &= "CREATE TABLE gametags (TagID TEXT Not NULL, MonitorID TEXT Not NULL, PRIMARY KEY(TagID, MonitorID)); "

                'Add new setting
                sSQL &= "ALTER TABLE settings ADD COLUMN TimeTracking BOOLEAN Not NULL DEFAULT 1;"

                sSQL &= "PRAGMA user_version=94"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v93")

                'Add Tags Tables
                sSQL = "CREATE TABLE tags (TagID TEXT Not NULL UNIQUE, Name TEXT Not NULL PRIMARY KEY); "
                sSQL &= "CREATE TABLE gametags (TagID TEXT Not NULL, MonitorID TEXT Not NULL, PRIMARY KEY(TagID, MonitorID)); "

                sSQL &= "PRAGMA user_version=94"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.95 Upgrade
        If GetDatabaseVersion() < 95 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v94")

                'Add new setting
                sSQL = "ALTER TABLE settings ADD COLUMN SupressBackup BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "ALTER TABLE settings ADD COLUMN SupressBackupThreshold INTEGER Not NULL DEFAULT 10;"

                sSQL &= "PRAGMA user_version=95"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v94")

                sSQL = "PRAGMA user_version=95"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.96 Upgrade
        If GetDatabaseVersion() < 96 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v95")

                'Add new setting                
                sSQL = "ALTER TABLE settings ADD COLUMN CompressionLevel INTEGER Not NULL DEFAULT 5;"

                sSQL &= "PRAGMA user_version=96"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v95")

                sSQL = "PRAGMA user_version=96"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.97 Upgrade
        If GetDatabaseVersion() < 97 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v96")

                'Add new settings        
                sSQL = "ALTER TABLE settings ADD COLUMN Custom7zArguments TEXT;"
                sSQL &= "ALTER TABLE settings ADD COLUMN Custom7zLocation TEXT;"
                sSQL &= "ALTER TABLE settings ADD COLUMN SyncFields INTEGER Not NULL DEFAULT 32;"
                sSQL &= "ALTER TABLE settings ADD COLUMN AutoSaveLog BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "PRAGMA user_version=97"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v96")

                sSQL = "PRAGMA user_version=97"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.98 Upgrade
        If GetDatabaseVersion() < 98 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v97")

                'Overhaul Manifest Table
                sSQL = "CREATE TABLE manifest_new (ManifestID TEXT Not NULL PRIMARY KEY, Name TEXT Not NULL, FileName TEXT Not NULL, RestorePath TEXT Not NULL, AbsolutePath BOOLEAN Not NULL, DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy) "
                sSQL &= "SELECT ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy FROM manifest;"
                sSQL &= "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                'Add backup limit field
                sSQL &= "ALTER TABLE monitorlist ADD COLUMN BackupLimit INTEGER Not NULL DEFAULT 5;"

                sSQL &= "PRAGMA user_version=98"

                RunParamQuery(sSQL, New Hashtable)

                'Run a compact
                CompactDatabase()
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v97")

                'Overhaul Manifest Table
                sSQL = "CREATE TABLE manifest_new (ManifestID TEXT Not NULL PRIMARY KEY, Name TEXT Not NULL, FileName TEXT Not NULL, RestorePath TEXT Not NULL, AbsolutePath BOOLEAN Not NULL, DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy) "
                sSQL &= "SELECT ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy FROM manifest;"
                sSQL &= "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                'Add backup limit field
                sSQL &= "ALTER TABLE monitorlist ADD COLUMN BackupLimit INTEGER Not NULL DEFAULT 5;"

                sSQL &= "PRAGMA user_version=98"

                RunParamQuery(sSQL, New Hashtable)

                'Run a compact
                CompactDatabase()
            End If
        End If

        '1.01 Upgrade
        If GetDatabaseVersion() < 101 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v98")

                'Remove checksum field                
                sSQL = "CREATE TABLE settings_new (SettingsID INTEGER Not NULL PRIMARY KEY, MonitorOnStartup BOOLEAN Not NULL, StartToTray BOOLEAN Not NULL, ShowDetectionToolTips BOOLEAN Not NULL, " &
                        "DisableConfirmation BOOLEAN Not NULL, CreateSubFolder BOOLEAN Not NULL, ShowOverwriteWarning BOOLEAN Not NULL, RestoreOnLaunch BOOLEAN Not NULL, " &
                        "BackupFolder TEXT Not NULL, Sync BOOLEAN Not NULL, StartWithWindows BOOLEAN Not NULL, TimeTracking BOOLEAN Not NULL, " &
                        "SupressBackup BOOLEAN Not NULL, SupressBackupThreshold INTEGER Not NULL, CompressionLevel INTEGER Not NULL, Custom7zArguments TEXT, " &
                        "Custom7zLocation TEXT, SyncFields INTEGER Not NULL, AutoSaveLog BOOLEAN Not NULL);"
                sSQL &= "INSERT INTO settings_new (SettingsID, MonitorOnStartup, StartToTray, ShowDetectionToolTips, DisableConfirmation, CreateSubFolder, " &
                        "ShowOverwriteWarning, RestoreOnLaunch, BackupFolder, Sync, StartWithWindows, TimeTracking,  SupressBackup, SupressBackupThreshold, " &
                        "CompressionLevel, Custom7zArguments, Custom7zLocation, SyncFields, AutoSaveLog) " &
                        "SELECT SettingsID, MonitorOnStartup, StartToTray, ShowDetectionToolTips, DisableConfirmation, CreateSubFolder, " &
                        "ShowOverwriteWarning, RestoreOnLaunch, BackupFolder, Sync, StartWithWindows, TimeTracking,  SupressBackup, SupressBackupThreshold, " &
                        "CompressionLevel, Custom7zArguments, Custom7zLocation, SyncFields, AutoSaveLog FROM settings;" &
                        "DROP TABLE settings; ALTER TABLE settings_new RENAME TO settings;"
                'Add new field(s)
                sSQL &= "ALTER TABLE monitorlist ADD COLUMN CleanFolder BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "ALTER TABLE settings ADD COLUMN AutoRestore BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "ALTER TABLE settings ADD COLUMN AutoMark BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "PRAGMA user_version=101"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v98")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN CleanFolder BOOLEAN Not NULL DEFAULT 0;"
                sSQL &= "PRAGMA user_version=101"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.02 Upgrade
        If GetDatabaseVersion() < 102 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v101")

                'Add Table (SavedPath)
                sSQL = "CREATE TABLE savedpath (PathName TEXT Not NULL PRIMARY KEY, Path TEXT Not NULL);"

                'Add new field(s)
                sSQL &= "ALTER TABLE monitorlist ADD COLUMN Parameter TEXT;"

                sSQL &= "PRAGMA user_version=102"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v101")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN Parameter TEXT;"

                sSQL &= "PRAGMA user_version=102"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.05 Upgrade
        If GetDatabaseVersion() < 105 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v102")

                'Add Tables (Sessions)
                sSQL = "CREATE TABLE sessions (MonitorID TEXT Not NULL, Start INTEGER Not NULL, End INTEGER Not NULL, PRIMARY KEY(MonitorID, Start));"

                'Add new field(s)
                sSQL &= "ALTER TABLE monitorlist ADD COLUMN Comments TEXT;"
                sSQL &= "ALTER TABLE settings ADD COLUMN SessionTracking BOOLEAN DEFAULT 0;"

                sSQL &= "PRAGMA user_version=105"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v102")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN Comments TEXT;"

                sSQL &= "PRAGMA user_version=105"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.08 Upgrade
        If GetDatabaseVersion() < 108 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v105")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN IsRegEx BOOLEAN Not NULL DEFAULT 0;"

                sSQL &= "PRAGMA user_version=108"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v105")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN IsRegEx BOOLEAN Not NULL DEFAULT 0;"

                sSQL &= "PRAGMA user_version=108"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.10 Upgrade
        If GetDatabaseVersion() < 110 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v108")

                'Add Tables
                sSQL = "CREATE TABLE processes (ProcessID TEXT Not NULL PRIMARY KEY, Name Text Not NULL, Path TEXT Not NULL, Args TEXT, Kill BOOLEAN Not NULL);"
                sSQL &= "CREATE TABLE gameprocesses (ProcessID TEXT Not NULL, MonitorID TEXT Not NULL, PRIMARY KEY(ProcessID, MonitorID));"

                'Overhaul Tables
                sSQL &= "CREATE TABLE settings_new (SettingsID INTEGER Not NULL PRIMARY KEY, MonitorOnStartup BOOLEAN Not NULL, StartToTray BOOLEAN Not NULL, ShowDetectionToolTips BOOLEAN Not NULL, " &
                   "DisableConfirmation BOOLEAN Not NULL, CreateSubFolder BOOLEAN Not NULL, ShowOverwriteWarning BOOLEAN Not NULL, RestoreOnLaunch BOOLEAN Not NULL, " &
                   "BackupFolder TEXT Not NULL, StartWithWindows BOOLEAN Not NULL, TimeTracking BOOLEAN Not NULL, " &
                   "SuppressBackup BOOLEAN Not NULL, SuppressBackupThreshold INTEGER Not NULL, CompressionLevel INTEGER Not NULL, Custom7zArguments TEXT, " &
                   "Custom7zLocation TEXT, SyncFields INTEGER Not NULL, AutoSaveLog BOOLEAN Not NULL, AutoRestore BOOLEAN Not NULL, AutoMark BOOLEAN Not NULL, SessionTracking BOOLEAN Not NULL, " &
                   "SuppressMessages INTEGER Not NULL, BackupOnLaunch BOOLEAN Not NULL, UseGameID BOOLEAN Not NULL, DisableSyncMessages BOOLEAN Not NULL);"
                sSQL &= "INSERT INTO settings_new(SettingsID, MonitorOnStartup, StartToTray, ShowDetectionToolTips, DisableConfirmation, CreateSubFolder, ShowOverwriteWarning, RestoreOnLaunch, " &
                   "BackupFolder, StartWithWindows, TimeTracking, SuppressBackup, SuppressBackupThreshold, CompressionLevel, Custom7zArguments, Custom7zLocation, SyncFields, AutoSaveLog, " &
                    "AutoRestore, AutoMark, SessionTracking, SuppressMessages, BackupOnLaunch, UseGameID, DisableSyncMessages) SELECT SettingsID, MonitorOnStartup, StartToTray, ShowDetectionToolTips, DisableConfirmation, CreateSubFolder, ShowOverwriteWarning, RestoreOnLaunch, " &
                   "BackupFolder, StartWithWindows, TimeTracking, SupressBackup, SupressBackupThreshold, CompressionLevel, Custom7zArguments, Custom7zLocation, SyncFields, AutoSaveLog, " &
                    "AutoRestore, AutoMark, SessionTracking, 0, 1, 0, 1 FROM settings;" &
                    "DROP TABLE settings; ALTER TABLE settings_new RENAME TO settings;"
                sSQL &= "CREATE TABLE monitorlist_new (MonitorID TEXT Not NULL PRIMARY KEY, Name TEXT Not NULL, Process TEXT Not NULL, Path TEXT, " &
                   "AbsolutePath BOOLEAN Not NULL, FolderSave BOOLEAN Not NULL, FileType TEXT, TimeStamp BOOLEAN Not NULL, ExcludeList TEXT Not NULL, " &
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN Not NULL, MonitorOnly BOOLEAN Not NULL, " &
                   "BackupLimit INTEGER Not NULL, CleanFolder BOOLEAN Not NULL, Parameter TEXT, Comments TEXT, IsRegEx BOOLEAN Not NULL);"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, " &
                   "ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx)" &
                   "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, " &
                   "ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx FROM monitorlist;" &
                   "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"
                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT Not NULL PRIMARY KEY, MonitorID TEXT Not NULL, FileName TEXT Not NULL, " &
                   "DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, MonitorID, FileName, DateUpdated, UpdatedBy, CheckSum) " &
                   "SELECT ManifestID, MonitorID, FileName, DateUpdated, UpdatedBy, CheckSum FROM manifest NATURAL JOIN monitorlist GROUP BY ManifestID;" &
                   "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                sSQL &= "PRAGMA user_version=110"

                RunParamQuery(sSQL, New Hashtable)

                CompactDatabase()

            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v108")

                'Overhaul Tables
                sSQL = "CREATE TABLE monitorlist_new (MonitorID TEXT Not NULL PRIMARY KEY, Name TEXT Not NULL, Process TEXT Not NULL, Path TEXT, " &
                   "AbsolutePath BOOLEAN Not NULL, FolderSave BOOLEAN Not NULL, FileType TEXT, TimeStamp BOOLEAN Not NULL, ExcludeList TEXT Not NULL, " &
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN Not NULL, MonitorOnly BOOLEAN Not NULL, " &
                   "BackupLimit INTEGER Not NULL, CleanFolder BOOLEAN Not NULL, Parameter TEXT, Comments TEXT, IsRegEx BOOLEAN Not NULL);"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, " &
                   "ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx)" &
                   "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, " &
                   "ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly, BackupLimit, CleanFolder, Parameter, Comments, IsRegEx FROM monitorlist;" &
                   "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"

                'We need to push the local game list against the remote database in case they had syncing disabled
                Dim hshMonitorList As Hashtable = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList, mgrSQLite.Database.Local)
                Dim oSettings As New mgrSettings
                oSettings.LoadSettings()
                mgrMonitorList.DoListAddUpdateSync(hshMonitorList, Database.Remote, oSettings.SyncFields)
                mgrTags.SyncTags(True)
                mgrGameTags.SyncGameTags(True)

                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT Not NULL PRIMARY KEY, MonitorID TEXT Not NULL, FileName TEXT Not NULL, " &
                   "DateUpdated TEXT Not NULL, UpdatedBy TEXT Not NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, MonitorID, FileName, DateUpdated, UpdatedBy, CheckSum) " &
                   "SELECT ManifestID, MonitorID, FileName, DateUpdated, UpdatedBy, CheckSum FROM manifest NATURAL JOIN monitorlist GROUP BY ManifestID;" &
                   "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                sSQL &= "PRAGMA user_version=110"

                RunParamQuery(sSQL, New Hashtable)

                CompactDatabase()
            End If
        End If

        '1.15 Upgrade
        If GetDatabaseVersion() < 115 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v110")

                'Add new setting
                sSQL = "ALTER TABLE settings ADD COLUMN ShowResolvedPaths BOOLEAN Not NULL DEFAULT 1;"

                sSQL &= "PRAGMA user_version=115"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v110")

                'Convert core path variables to new standard
                If Not mgrCommon.IsUnix Then
                    sSQL = "UPDATE monitorlist SET Path = Replace(Path,'*appdatalocal*','%LOCALAPPDATA%');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*appdataroaming*','%APPDATA%');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*mydocs*','%USERDOCUMENTS%');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*currentuser*','%USERPROFILE%');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*publicdocs*','%COMMONDOCUMENTS%');"
                Else
                    sSQL = "UPDATE monitorlist SET Path = Replace(Path,'*appdatalocal*','${XDG_DATA_HOME:-~/.local/share}');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*appdataroaming*','${XDG_CONFIG_HOME:-~/.config}');"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'*mydocs*','~');"
                End If

                'Convert custom variables to new standard
                Dim hshVariables As Hashtable = mgrVariables.ReadVariables()
                Dim sOldVariable As String
                For Each oVariable As clsPathVariable In hshVariables.Values
                    sOldVariable = "*" & oVariable.Name & "*"
                    sSQL &= "UPDATE monitorlist SET Path = Replace(Path,'" & sOldVariable & "','" & oVariable.FormattedName & "');"
                Next

                sSQL &= "PRAGMA user_version=115"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.16 Upgrade
        If GetDatabaseVersion() < 116 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v115")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN RecurseSubFolders BOOLEAN NOT NULL DEFAULT 1;"
                sSQL &= "PRAGMA user_version=116"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v115")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN RecurseSubFolders BOOLEAN NOT NULL DEFAULT 1;"
                sSQL &= "PRAGMA user_version=116"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.18 Upgrade
        If GetDatabaseVersion() < 118 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v116")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN OS INTEGER NOT NULL DEFAULT " & mgrCommon.GetCurrentOS & ";"
                sSQL &= "ALTER TABLE settings ADD COLUMN DisableDiskSpaceCheck BOOLEAN NOT NULL DEFAULT 0;"

                'Add Tables (Wine Data)
                sSQL &= "CREATE TABLE winedata (MonitorID TEXT NOT NULL PRIMARY KEY, Prefix TEXT NOT NULL, SavePath TEXT NOT NULL, BinaryPath TEXT NOT NULL);"

                sSQL &= "PRAGMA user_version=118"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v116")

                'Add new field(s)
                sSQL = "ALTER TABLE monitorlist ADD COLUMN OS INTEGER NOT NULL DEFAULT " & mgrCommon.GetCurrentOS & ";"
                sSQL &= "PRAGMA user_version=118"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.20 Upgrade
        If GetDatabaseVersion() < 120 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v118")

                'Add Tables (Config Links)
                sSQL = "CREATE TABLE configlinks (MonitorID TEXT NOT NULL, LinkID TEXT NOT NULL, PRIMARY KEY(MonitorID, LinkID));"
                sSQL &= "ALTER TABLE settings ADD COLUMN TemporaryFolder TEXT;"
                sSQL &= "PRAGMA user_version=120"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v118")

                'Add Tables (Config Links)
                sSQL = "CREATE TABLE configlinks (MonitorID TEXT NOT NULL, LinkID TEXT NOT NULL, PRIMARY KEY(MonitorID, LinkID));"
                sSQL &= "PRAGMA user_version=120"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.22 Upgrade
        If GetDatabaseVersion() < 122 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v120")

                'Add new field(s)
                If mgrCommon.IsUnix Then
                    sSQL = "ALTER TABLE settings ADD COLUMN ExitOnClose BOOLEAN NOT NULL DEFAULT 1;"
                    sSQL &= "ALTER TABLE settings ADD COLUMN ExitNoWarning BOOLEAN NOT NULL DEFAULT 0;"
                Else
                    sSQL = "ALTER TABLE settings ADD COLUMN ExitOnClose BOOLEAN NOT NULL DEFAULT 0;"
                    sSQL &= "ALTER TABLE settings ADD COLUMN ExitNoWarning BOOLEAN NOT NULL DEFAULT 0;"
                End If

                sSQL &= "PRAGMA user_version=122"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v120")

                sSQL = "PRAGMA user_version=122"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '1.24 Upgrade
        If GetDatabaseVersion() < 124 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v122")

                'Add Tables (Launchers & Launch Data)
                sSQL = "CREATE TABLE launchers (LauncherID	TEXT NOT NULL PRIMARY KEY, Name	TEXT NOT NULL, LaunchString	TEXT NOT NULL);"
                sSQL &= "CREATE TABLE launchdata (MonitorID	TEXT NOT NULL PRIMARY KEY, Path	TEXT NOT NULL, Args TEXT NOT NULL, LauncherID TEXT NOT NULL, LauncherGameID	TEXT NOT NULL);"

                'Add new field(s)
                sSQL &= "ALTER TABLE settings ADD COLUMN EnableLauncher BOOLEAN NOT NULL DEFAULT 0;"

                sSQL &= "PRAGMA user_version=124"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v122")

                sSQL = "PRAGMA user_version=124"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If
    End Sub

    Public Function GetDBSize() As Long
        Dim oFileInfo As New FileInfo(sDatabaseLocation)
        Return Math.Round(oFileInfo.Length / 1024, 2)
    End Function

    Public Function ReportVersion() As String
        Return SqliteConnection.SQLiteVersion
    End Function

    Public Sub CompactDatabase()
        Dim sSQL As String
        Dim command As SQLiteCommand

        sSQL = "VACUUM"

        Connect()
        command = New SQLiteCommand(sSQL, db)

        Try
            command.ExecuteNonQuery()
        Catch ex As Exception
            mgrCommon.ShowMessage(mgrSQLite_ErrorQueryFailure, New String() {sSQL, ex.Message}, MsgBoxStyle.Exclamation)
        Finally
            command.Dispose()
            Disconnect()
        End Try
    End Sub

End Class