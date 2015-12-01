Imports System.IO
Imports System.Data.SQLite

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

    Private Sub BackupDB(ByVal sLastVer As String)
        Dim sNewFile As String = String.Empty

        Try
            Select Case eDatabase
                Case Database.Local
                    sNewFile = mgrPath.DatabaseLocation & "." & sLastVer & ".bak"
                    File.Copy(mgrPath.DatabaseLocation, sNewFile, False)
                Case Database.Remote
                    sNewFile = mgrPath.RemoteDatabaseLocation & "." & sLastVer & ".bak"
                    File.Copy(mgrPath.RemoteDatabaseLocation, sNewFile, False)
            End Select
        Catch ex As Exception
            MsgBox("An error occured creating a backup of the database file at " & sNewFile & vbCrLf & vbCrLf & ex.Message)
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
            sSql = "CREATE TABLE settings (SettingsID INTEGER NOT NULL PRIMARY KEY, MonitorOnStartup BOOLEAN NOT NULL, StartToTray BOOLEAN NOT NULL, ShowDetectionToolTips BOOLEAN NOT NULL, " & _
                   "DisableConfirmation BOOLEAN NOT NULL, CreateSubFolder BOOLEAN NOT NULL, ShowOverwriteWarning BOOLEAN NOT NULL, RestoreOnLaunch BOOLEAN NOT NULL, " & _
                   "BackupFolder TEXT NOT NULL, Sync BOOLEAN NOT NULL, CheckSum BOOLEAN NOT NULL, StartWithWindows BOOLEAN NOT NULL, TimeTracking BOOLEAN NOT NULL);"

            'Add Tables (Monitor List)
            sSql &= "CREATE TABLE monitorlist (MonitorID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, " & _
                   "AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, " & _
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, " & _
                   "PRIMARY KEY(Name, Process));"

            'Add Tables (Tags)
            sSql &= "CREATE TABLE tags (TagID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY); "

            'Add Tables (Game Tags)
            sSql &= "CREATE TABLE gametags (TagID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(TagID, MonitorID)); "

            'Add Tables (Variables)
            sSql &= "CREATE TABLE variables (VariableID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, Path TEXT NOT NULL);"

            'Add Tables (Local Manifest)
            sSql &= "CREATE TABLE manifest (ManifestID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, FileName TEXT NOT NULL, RestorePath TEXT NOT NULL, " & _
                   "AbsolutePath BOOLEAN NOT NULL, DateUpdated TEXT NOT NULL, UpdatedBy TEXT NOT NULL, CheckSum TEXT);"

            'Set Version
            sSql &= "PRAGMA user_version=" & mgrCommon.AppVersion

            RunParamQuery(sSql, New Hashtable)
            Return True
        Catch e As Exception
            MsgBox("An error has occured attempting to create the local application database: " & vbCrLf & vbCrLf & e.Message)
            Return False
        End Try
    End Function

    Private Function CreateRemoteDatabase() As Boolean
        Dim sSql As String

        Try
            'Create the DB
            SQLiteConnection.CreateFile(sDatabaseLocation)

            'Add Tables (Remote Monitor List)
            sSql = "CREATE TABLE monitorlist (MonitorID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, " & _
                   "AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, " & _
                   "ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, " & _
                   "PRIMARY KEY(Name, Process));"

            'Add Tables (Remote Manifest)
            sSql &= "CREATE TABLE manifest (ManifestID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, FileName TEXT NOT NULL, RestorePath TEXT NOT NULL, " & _
                   "AbsolutePath BOOLEAN NOT NULL, DateUpdated TEXT NOT NULL, UpdatedBy TEXT NOT NULL, CheckSum TEXT);"

            'Add Tables (Remote Tags)
            sSql &= "CREATE TABLE tags (TagID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY); "

            'Add Tables (Remote Game Tags)
            sSql &= "CREATE TABLE gametags (TagID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(TagID, MonitorID)); "

            'Set Version
            sSql &= "PRAGMA user_version=" & mgrCommon.AppVersion

            RunParamQuery(sSql, New Hashtable)
            Return True
        Catch e As Exception
            MsgBox("An error has occured attempting to create the remote application database: " & vbCrLf & vbCrLf & e.Message)
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
            db = New SQLiteConnection(sConnectString)
            db.Open()
        Else
            CreateDB()
            db.Open()
        End If
    End Sub

    Public Sub Disconnect()
        db.Close()
    End Sub

    Private Sub BuildParams(ByRef command As SQLiteCommand, ByRef hshParams As Hashtable)
        For Each de As DictionaryEntry In hshParams
            command.Parameters.AddWithValue(de.Key, de.Value)
        Next
    End Sub

    Public Function RunParamQuery(ByVal sSQL As String, ByVal hshParams As Hashtable) As Boolean
        Dim trans As SQLiteTransaction
        Dim command As SQLiteCommand

        Connect()
        command = New SQLiteCommand(sSQL, db)
        BuildParams(command, hshParams)
        trans = db.BeginTransaction()

        Try    
            command.ExecuteNonQuery()
            trans.Commit()
        Catch e As Exception
            trans.Rollback()
            MsgBox("An error has occured attempting run the query." & vbCrLf & vbCrLf & sSQL & vbCrLf & vbCrLf & e.Message)
            Return False
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return True
    End Function

    Public Function RunMassParamQuery(ByVal sSQL As String, ByVal oParamList As List(Of Hashtable)) As Boolean
        Dim trans As SQLiteTransaction
        Dim command As SQLiteCommand

        Connect()
        command = New SQLiteCommand(sSQL, db)
        trans = db.BeginTransaction()

        Try
            For Each hshParams In oParamList
                BuildParams(command, hshParams)
                command.ExecuteNonQuery()
            Next
            trans.Commit()
        Catch e As Exception
            trans.Rollback()
            MsgBox("An error has occured attempting run the query." & vbCrLf & vbCrLf & sSQL & vbCrLf & vbCrLf & e.Message)
            Return False
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return True
    End Function

    Public Function ReadParamData(ByVal sSQL As String, ByVal hshParams As Hashtable) As DataSet
        Dim adapter As SQLiteDataAdapter
        Dim command As SQLiteCommand
        Dim oData As New DataSet

        Connect()
        command = New SQLiteCommand(sSQL, db)
        BuildParams(command, hshParams)

        Try
            adapter = New SQLiteDataAdapter(command)
            adapter.Fill(oData)
        Catch e As Exception
            MsgBox("An error has occured attempting run the query." & vbCrLf & vbCrLf & sSQL & vbCrLf & vbCrLf & e.Message)
        Finally
            command.Dispose()
            Disconnect()
        End Try

        Return oData
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

    Public Sub UpgradeDOSBox()
        Dim sSQL As String
        Dim sCurrentID As String
        Dim sCurrentName As String
        Dim sCurrentProcess As String
        Dim sDosProcess As String
        Dim sNewName As String
        Dim oData As DataSet
        Dim hshParams As Hashtable
        Dim oParamList As New List(Of Hashtable)

        sSQL = "SELECT MonitorID, Name, Process FROM monitorlist WHERE Process LIKE '%dosbox:%'"
        oData = ReadParamData(sSQL, New Hashtable)

        sSQL = "UPDATE monitorlist SET Name=@NewName, Process=@NewProcess WHERE MonitorID=@ID;"

        For Each dr As DataRow In oData.Tables(0).Rows
            hshParams = New Hashtable
            sCurrentID = CStr(dr("MonitorID"))
            sCurrentName = CStr(dr("Name"))
            sCurrentProcess = CStr(dr("Process"))
            sDosProcess = sCurrentProcess.Split(":")(1)
            sNewName = sCurrentName & " (" & sDosProcess & ")"
            hshParams.Add("NewName", sNewName)
            hshParams.Add("NewProcess", "DOSBox")
            hshParams.Add("ID", sCurrentID)
            oParamList.Add(hshParams)
        Next

        RunMassParamQuery(sSQL, oParamList)
    End Sub

    Public Sub DatabaseUpgrade()
        Dim sSQL As String

        '0.9 Upgrade
        If GetDatabaseVersion() < 90 Then
            BackupDB("v8")
            sSQL = "ALTER TABLE monitorlist ADD COLUMN MonitorOnly BOOLEAN NOT NULL DEFAULT 0;"
            sSQL &= "PRAGMA user_version=90"
            RunParamQuery(sSQL, New Hashtable)
        End If

        '0.91 Upgrade
        If GetDatabaseVersion() < 91 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v84")

                'Overhaul Monitor List Table
                sSQL = "CREATE TABLE monitorlist_new (MonitorID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, PRIMARY KEY(Name, Process));"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly) "
                sSQL &= "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist;"
                sSQL &= "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"

                'Overhaul Variables Table
                sSQL &= "CREATE TABLE variables_new (VariableID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, Path TEXT NOT NULL);"
                sSQL &= "INSERT INTO variables_new (VariableID, Name, Path) SELECT VariableID, Name, Path FROM variables;"
                sSQL &= "DROP TABLE variables; ALTER TABLE variables_new RENAME TO variables;"

                'Overhaul Manifest Table
                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, FileName TEXT NOT NULL, RestorePath TEXT NOT NULL, AbsolutePath BOOLEAN NOT NULL, DateUpdated TEXT NOT NULL, UpdatedBy TEXT NOT NULL, CheckSum TEXT);"
                sSQL &= "INSERT INTO manifest_new (ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy) "
                sSQL &= "SELECT ManifestID, Name, FileName, RestorePath, AbsolutePath, DateUpdated, UpdatedBy FROM manifest;"
                sSQL &= "DROP TABLE manifest; ALTER TABLE manifest_new RENAME TO manifest;"

                'Add new settings
                sSQL &= "ALTER TABLE settings ADD COLUMN Sync BOOLEAN NOT NULL DEFAULT 1;"
                sSQL &= "ALTER TABLE settings ADD COLUMN CheckSum BOOLEAN NOT NULL DEFAULT 1;"
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
                sSQL = "CREATE TABLE monitorlist_new (MonitorID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL, Process TEXT NOT NULL, Path TEXT, AbsolutePath BOOLEAN NOT NULL, FolderSave BOOLEAN NOT NULL, FileType TEXT, TimeStamp BOOLEAN NOT NULL, ExcludeList TEXT NOT NULL, ProcessPath TEXT, Icon TEXT, Hours REAL, Version TEXT, Company TEXT, Enabled BOOLEAN NOT NULL, MonitorOnly BOOLEAN NOT NULL, PRIMARY KEY(Name, Process));"
                sSQL &= "INSERT INTO monitorlist_new (MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly) "
                sSQL &= "SELECT MonitorID, Name, Process, Path, AbsolutePath, FolderSave, FileType, TimeStamp, ExcludeList, ProcessPath, Icon, Hours, Version, Company, Enabled, MonitorOnly FROM monitorlist;"
                sSQL &= "DROP TABLE monitorlist; ALTER TABLE monitorlist_new RENAME TO monitorlist;"

                'Overhaul Manifest Table
                sSQL &= "CREATE TABLE manifest_new (ManifestID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY, FileName TEXT NOT NULL, RestorePath TEXT NOT NULL, AbsolutePath BOOLEAN NOT NULL, DateUpdated TEXT NOT NULL, UpdatedBy TEXT NOT NULL, CheckSum TEXT);"
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
                sSQL = "ALTER TABLE settings ADD COLUMN StartWithWindows BOOLEAN NOT NULL DEFAULT 0;"
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
                sSQL = "CREATE TABLE tags (TagID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY); "
                sSQL &= "CREATE TABLE gametags (TagID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(TagID, MonitorID)); "

                'Add new setting
                sSQL &= "ALTER TABLE settings ADD COLUMN TimeTracking BOOLEAN NOT NULL DEFAULT 1;"

                sSQL &= "PRAGMA user_version=94"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v93")

                'Add Tags Tables
                sSQL = "CREATE TABLE tags (TagID TEXT NOT NULL UNIQUE, Name TEXT NOT NULL PRIMARY KEY); "
                sSQL &= "CREATE TABLE gametags (TagID TEXT NOT NULL, MonitorID TEXT NOT NULL, PRIMARY KEY(TagID, MonitorID)); "

                sSQL &= "PRAGMA user_version=94"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

        '0.95 Upgrade
        If GetDatabaseVersion() < 95 Then
            If eDatabase = Database.Local Then
                'Backup DB before starting
                BackupDB("v94")

                UpgradeDOSBox()

                sSQL = "PRAGMA user_version=95"

                RunParamQuery(sSQL, New Hashtable)
            End If
            If eDatabase = Database.Remote Then
                'Backup DB before starting
                BackupDB("v94")

                UpgradeDOSBox()

                sSQL = "PRAGMA user_version=95"

                RunParamQuery(sSQL, New Hashtable)
            End If
        End If

    End Sub

    Public Function GetDBSize() As Long
        Dim oFileInfo As New FileInfo(sDatabaseLocation)
        Return Math.Round(oFileInfo.Length / 1024, 2)
    End Function

    Public Sub CompactDatabase()
        Dim sSQL As String
        Dim command As SQLiteCommand

        sSQL = "VACUUM"

        Connect()
        command = New SQLiteCommand(sSQL, db)

        Try
            command.ExecuteNonQuery()
        Catch e As Exception
            MsgBox("An error has occured attempting run the query." & vbCrLf & vbCrLf & sSQL & vbCrLf & vbCrLf & e.Message)
        Finally
            command.Dispose()
            Disconnect()
        End Try
    End Sub

End Class