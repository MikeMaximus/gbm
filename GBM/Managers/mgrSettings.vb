Imports System.IO
Imports GBM.My.Resources

Public Class mgrSettings
    Private bStartWithWindows As Boolean = False
    Private bMonitoronStartup As Boolean = True
    Private bStartToTray As Boolean = False
    Private bShowDetectionToolTips As Boolean = True
    Private bDisableConfirmation As Boolean = False
    Private bCreateSubFolder As Boolean = False
    Private bShowOverwriteWarning As Boolean = True
    Private bRestoreOnLaunch As Boolean = False
    Private bAutoRestore As Boolean = False
    Private bAutoMark As Boolean = False
    Private bTimeTracking As Boolean = True
    Private bSessionTracking As Boolean = False
    Private bSuppressBackup As Boolean = False
    Private iSuppressBackupThreshold As Integer = 10
    Private iCompressionLevel As Integer = 5
    Private s7zArguments As String = String.Empty
    Private s7zLocation As String = String.Empty
    Private sBackupFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & Path.DirectorySeparatorChar & App_NameLong
    Private sTemporaryFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & Path.DirectorySeparatorChar & "gbm"
    Private eSyncFields As clsGame.eOptionalSyncFields = clsGame.eOptionalSyncFields.None
    Private eMessages As eSuppressMessages = eSuppressMessages.None
    Private bAutoSaveLog As Boolean = False
    Private bBackupOnLaunch As Boolean = True
    Private bUseGameID As Boolean = False
    Private bDisableSyncMessages As Boolean = True
    Private bShowResolvedPaths As Boolean = True
    Private bDisableDiskSpaceCheck As Boolean = False
    Private bExitOnClose As Boolean = False
    Private bExitNoWarning As Boolean = False

    <Flags()> Public Enum eSuppressMessages
        None = 0
        EmptyProcessWarning = 1
        LinkProcessTip = 2
        WinConfigsInLinux = 4
        WineConfig = 16
        LinkConfigTip = 32
    End Enum

    Property StartWithWindows As Boolean
        Get
            Return bStartWithWindows
        End Get
        Set(value As Boolean)
            bStartWithWindows = value
        End Set
    End Property

    Property MonitorOnStartup As Boolean
        Get
            Return bMonitoronStartup
        End Get
        Set(value As Boolean)
            bMonitoronStartup = value
        End Set
    End Property

    Property StartToTray As Boolean
        Get
            Return bStartToTray
        End Get
        Set(value As Boolean)
            bStartToTray = value
        End Set
    End Property

    Property ShowDetectionToolTips As Boolean
        Get
            Return bShowDetectionToolTips
        End Get
        Set(value As Boolean)
            bShowDetectionToolTips = value
        End Set
    End Property

    Property DisableConfirmation As Boolean
        Get
            Return bDisableConfirmation
        End Get
        Set(value As Boolean)
            bDisableConfirmation = value
        End Set
    End Property

    Property CreateSubFolder As Boolean
        Get
            Return bCreateSubFolder
        End Get
        Set(value As Boolean)
            bCreateSubFolder = value
        End Set
    End Property

    Property ShowOverwriteWarning As Boolean
        Get
            Return bShowOverwriteWarning
        End Get
        Set(value As Boolean)
            bShowOverwriteWarning = value
        End Set
    End Property

    Property RestoreOnLaunch As Boolean
        Get
            Return bRestoreOnLaunch
        End Get
        Set(value As Boolean)
            bRestoreOnLaunch = value
        End Set
    End Property

    Property AutoRestore As Boolean
        Get
            Return bAutoRestore
        End Get
        Set(value As Boolean)
            bAutoRestore = value
        End Set
    End Property

    Property AutoMark As Boolean
        Get
            Return bAutoMark
        End Get
        Set(value As Boolean)
            bAutoMark = value
        End Set
    End Property

    Property TimeTracking As Boolean
        Get
            Return bTimeTracking
        End Get
        Set(value As Boolean)
            bTimeTracking = value
        End Set
    End Property

    Property SessionTracking As Boolean
        Get
            Return bSessionTracking
        End Get
        Set(value As Boolean)
            bSessionTracking = value
        End Set
    End Property

    Property SuppressBackup As Boolean
        Get
            Return bSuppressBackup
        End Get
        Set(value As Boolean)
            bSuppressBackup = value
        End Set
    End Property

    Property SuppressBackupThreshold As Integer
        Get
            Return iSuppressBackupThreshold
        End Get
        Set(value As Integer)
            iSuppressBackupThreshold = value
        End Set
    End Property

    Property CompressionLevel As Integer
        Get
            Return iCompressionLevel
        End Get
        Set(value As Integer)
            iCompressionLevel = value
        End Set
    End Property

    Property Custom7zArguments As String
        Get
            Return s7zArguments
        End Get
        Set(value As String)
            s7zArguments = value
        End Set
    End Property

    Property ShowResolvedPaths As Boolean
        Get
            Return bShowResolvedPaths
        End Get
        Set(value As Boolean)
            bShowResolvedPaths = value
        End Set
    End Property

    ReadOnly Property Prepared7zArguments As String
        Get
            'Prepare custom 7z arguments
            Dim sPreparedArguments As String
            If s7zArguments <> String.Empty Then
                'Surround the arguments with spaces to be inserted into command
                sPreparedArguments = " " & s7zArguments & " "
            Else
                'The command always needs at least one space inserted
                sPreparedArguments = " "
            End If
            Return sPreparedArguments
        End Get
    End Property

    Property Custom7zLocation As String
        Get
            Return s7zLocation
        End Get
        Set(value As String)
            s7zLocation = value
        End Set
    End Property

    ReadOnly Property Is7zUtilityValid As Boolean
        Get
            'We don't use a packaged 7za on Unix, assume valid.
            If mgrCommon.IsUnix Then
                Return True
            End If

            If s7zLocation = String.Empty Then
                'Verify stored hash of the default utility if we're using it
                Return mgrCommon.UtilityHash = mgrHash.Generate_SHA256_Hash(mgrPath.Default7zLocation)
            Else
                'When using a custom utility assume it's valid, we have no way to be sure.
                Return True
            End If
        End Get
    End Property

    ReadOnly Property Utility7zLocation As String
        Get
            'Return default utility when custom setting is not used
            If s7zLocation = String.Empty Then
                Return mgrPath.Default7zLocation
            Else
                'Check if custom utility is available, if not use the default utility
                If File.Exists(s7zLocation) Then
                    Return s7zLocation
                Else
                    Return mgrPath.Default7zLocation
                End If
            End If
        End Get
    End Property

    Property BackupFolder As String
        Get
            Return sBackupFolder
        End Get
        Set(value As String)
            sBackupFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property

    Property TemporaryFolder As String
        Get
            Return sTemporaryFolder
        End Get
        Set(value As String)
            sTemporaryFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property

    ReadOnly Property MetadataLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_MetadataFilename
        End Get
    End Property

    ReadOnly Property IncludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupIncludeFileName
        End Get
    End Property

    ReadOnly Property ExcludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupExcludeFileName
        End Get
    End Property

    Property SyncFields As clsGame.eOptionalSyncFields
        Get
            Return eSyncFields
        End Get
        Set(value As clsGame.eOptionalSyncFields)
            eSyncFields = value
        End Set
    End Property

    Property AutoSaveLog As Boolean
        Get
            Return bAutoSaveLog
        End Get
        Set(value As Boolean)
            bAutoSaveLog = value
        End Set
    End Property

    Property SuppressMessages As eSuppressMessages
        Get
            Return eMessages
        End Get
        Set(value As eSuppressMessages)
            eMessages = value
        End Set
    End Property

    Property BackupOnLaunch As Boolean
        Get
            Return bBackupOnLaunch
        End Get
        Set(value As Boolean)
            bBackupOnLaunch = value
        End Set
    End Property

    Property UseGameID As Boolean
        Get
            Return bUseGameID
        End Get
        Set(value As Boolean)
            bUseGameID = value
        End Set
    End Property

    Property DisableSyncMessages As Boolean
        Get
            Return bDisableSyncMessages
        End Get
        Set(value As Boolean)
            bDisableSyncMessages = value
        End Set
    End Property

    Property DisableDiskSpaceCheck As Boolean
        Get
            Return bDisableDiskSpaceCheck
        End Get
        Set(value As Boolean)
            bDisableDiskSpaceCheck = value
        End Set
    End Property

    Property ExitOnClose As Boolean
        Get
            Return bExitOnClose
        End Get
        Set(value As Boolean)
            bExitOnClose = value
        End Set
    End Property

    Property ExitNoWarning As Boolean
        Get
            Return bExitNoWarning
        End Get
        Set(value As Boolean)
            bExitNoWarning = value
        End Set
    End Property

    Private Sub SaveFromClass()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT OR REPLACE INTO settings VALUES (1, @MonitorOnStartup, @StartToTray, @ShowDetectionToolTips, @DisableConfirmation, "
        sSQL &= "@CreateSubFolder, @ShowOverwriteWarning, @RestoreOnLaunch, @BackupFolder, @StartWithWindows, "
        sSQL &= "@TimeTracking, @SuppressBackup, @SuppressBackupThreshold, @CompressionLevel, @Custom7zArguments, @Custom7zLocation, "
        sSQL &= "@SyncFields, @AutoSaveLog, @AutoRestore, @AutoMark, @SessionTracking, @SuppressMessages, @BackupOnLaunch, @UseGameID, "
        sSQL &= "@DisableSyncMessages, @ShowResolvedPaths, @DisableDiskSpaceCheck, @TemporaryFolder, @ExitOnClose, @ExitNoWarning)"

        hshParams.Add("MonitorOnStartup", MonitorOnStartup)
        hshParams.Add("StartToTray", StartToTray)
        hshParams.Add("ShowDetectionToolTips", ShowDetectionToolTips)
        hshParams.Add("DisableConfirmation", DisableConfirmation)
        hshParams.Add("CreateSubFolder", CreateSubFolder)
        hshParams.Add("ShowOverwriteWarning", ShowOverwriteWarning)
        hshParams.Add("RestoreOnLaunch", RestoreOnLaunch)
        hshParams.Add("BackupFolder", BackupFolder)
        hshParams.Add("StartWithWindows", StartWithWindows)
        hshParams.Add("TimeTracking", TimeTracking)
        hshParams.Add("SuppressBackup", SuppressBackup)
        hshParams.Add("SuppressBackupThreshold", SuppressBackupThreshold)
        hshParams.Add("CompressionLevel", CompressionLevel)
        hshParams.Add("Custom7zArguments", Custom7zArguments)
        hshParams.Add("Custom7zLocation", Custom7zLocation)
        hshParams.Add("SyncFields", SyncFields)
        hshParams.Add("AutoSaveLog", AutoSaveLog)
        hshParams.Add("AutoRestore", AutoRestore)
        hshParams.Add("AutoMark", AutoMark)
        hshParams.Add("SessionTracking", SessionTracking)
        hshParams.Add("SuppressMessages", SuppressMessages)
        hshParams.Add("BackupOnLaunch", BackupOnLaunch)
        hshParams.Add("UseGameID", UseGameID)
        hshParams.Add("DisableSyncMessages", DisableSyncMessages)
        hshParams.Add("ShowResolvedPaths", ShowResolvedPaths)
        hshParams.Add("DisableDiskSpaceCheck", DisableDiskSpaceCheck)
        hshParams.Add("TemporaryFolder", TemporaryFolder)
        hshParams.Add("ExitOnClose", ExitOnClose)
        hshParams.Add("ExitNoWarning", ExitNoWarning)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Private Sub MapToClass()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim oData As DataSet
        Dim sSQL As String

        oDatabase.Connect()

        sSQL = "SELECT * FROM settings WHERE SettingsID = 1"
        oData = oDatabase.ReadParamData(sSQL, New Hashtable)

        For Each dr As DataRow In oData.Tables(0).Rows
            MonitorOnStartup = CBool(dr("MonitorOnStartup"))
            StartToTray = CBool(dr("StartToTray"))
            ShowDetectionToolTips = CBool(dr("ShowDetectionToolTips"))
            DisableConfirmation = CBool(dr("DisableConfirmation"))
            CreateSubFolder = CBool(dr("CreateSubFolder"))
            ShowOverwriteWarning = CBool(dr("ShowOverwriteWarning"))
            RestoreOnLaunch = CBool(dr("RestoreOnLaunch"))
            BackupFolder = CStr(dr("BackupFolder"))
            StartWithWindows = CBool(dr("StartWithWindows"))
            TimeTracking = CBool(dr("TimeTracking"))
            SuppressBackup = CBool(dr("SuppressBackup"))
            SuppressBackupThreshold = CInt(dr("SuppressBackupThreshold"))
            CompressionLevel = CInt(dr("CompressionLevel"))
            If Not IsDBNull(dr("Custom7zArguments")) Then Custom7zArguments = CStr(dr("Custom7zArguments"))
            If Not IsDBNull(dr("Custom7zLocation")) Then Custom7zLocation = CStr(dr("Custom7zLocation"))
            SyncFields = CInt(dr("SyncFields"))
            AutoSaveLog = CBool(dr("AutoSaveLog"))
            AutoRestore = CBool(dr("AutoRestore"))
            AutoMark = CBool(dr("AutoMark"))
            SessionTracking = CBool(dr("SessionTracking"))
            SuppressMessages = CInt(dr("SuppressMessages"))
            BackupOnLaunch = CBool(dr("BackupOnLaunch"))
            UseGameID = CBool(dr("UseGameID"))
            DisableSyncMessages = CBool(dr("DisableSyncMessages"))
            ShowResolvedPaths = CBool(dr("ShowResolvedPaths"))
            DisableDiskSpaceCheck = CBool(dr("DisableDiskSpaceCheck"))
            If Not IsDBNull(dr("TemporaryFolder")) Then TemporaryFolder = CStr(dr("TemporaryFolder"))
            ExitOnClose = CBool(dr("ExitOnClose"))
            ExitNoWarning = CBool(dr("ExitNoWarning"))
        Next

        oDatabase.Disconnect()
    End Sub

    Public Sub LoadSettings()
        MapToClass()

        'Set Remote Manifest Location     
        mgrPath.RemoteDatabaseLocation = Me.BackupFolder
    End Sub

    Public Sub SaveSettings()
        SaveFromClass()

        'Set Remote Manifest Location        
        mgrPath.RemoteDatabaseLocation = Me.BackupFolder
    End Sub

    Public Function SetMessageField(ByVal eMessages As eSuppressMessages, ByVal eMessage As eSuppressMessages) As eSuppressMessages
        Return eMessages Or eMessage
    End Function

    Public Function RemoveMessageField(ByVal eMessages As eSuppressMessages, ByVal eMessage As eSuppressMessages) As eSuppressMessages
        Return eMessages And (Not eMessage)
    End Function

End Class
