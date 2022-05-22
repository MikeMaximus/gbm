Imports System.IO
Imports GBM.My.Resources

Public Class mgrSettings
    Shared bStartWithWindows As Boolean
    Shared bMonitoronStartup As Boolean
    Shared bStartToTray As Boolean
    Shared bShowDetectionToolTips As Boolean
    Shared bDisableConfirmation As Boolean
    Shared bCreateSubFolder As Boolean
    Shared bShowOverwriteWarning As Boolean
    Shared bRestoreOnLaunch As Boolean
    Shared bAutoRestore As Boolean
    Shared bAutoMark As Boolean
    Shared bTimeTracking As Boolean
    Shared bSessionTracking As Boolean
    Shared bSuppressBackup As Boolean
    Shared iSuppressBackupThreshold As Integer
    Shared iCompressionLevel As Integer
    Shared s7zArguments As String
    Shared s7zLocation As String
    Shared sBackupFolder As String
    Shared sTemporaryFolder As String
    Shared eSyncFields As clsGame.eOptionalSyncFields
    Shared eMessages As eSuppressMessages
    Shared bAutoSaveLog As Boolean
    Shared bBackupOnLaunch As Boolean
    Shared bUseGameID As Boolean
    Shared bStorePathAutoConfig As Boolean
    Shared bDisableSyncMessages As Boolean
    Shared bShowResolvedPaths As Boolean
    Shared bDisableDiskSpaceCheck As Boolean
    Shared bExitOnClose As Boolean
    Shared bExitNoWarning As Boolean
    Shared bEnableLauncher As Boolean
    Shared bMainHideGameList As Boolean
    Shared bMainHideButtons As Boolean
    Shared bMainHideLog As Boolean
    Shared bBackupNotification As Boolean
    Shared iDetectionSpeed As Integer
    Shared bTwoPassDetection As Boolean

    <Flags()> Public Enum eSuppressMessages
        None = 0
        EmptyProcessWarning = 1
        LinkProcessTip = 2
        WinConfigsInLinux = 4
        WineConfig = 16
        LinkConfigTip = 32
        LudusaviImportWarning = 64
    End Enum

    Shared Property StartWithWindows As Boolean
        Get
            Return bStartWithWindows
        End Get
        Set(value As Boolean)
            bStartWithWindows = value
        End Set
    End Property

    Shared Property MonitorOnStartup As Boolean
        Get
            Return bMonitoronStartup
        End Get
        Set(value As Boolean)
            bMonitoronStartup = value
        End Set
    End Property

    Shared Property StartToTray As Boolean
        Get
            Return bStartToTray
        End Get
        Set(value As Boolean)
            bStartToTray = value
        End Set
    End Property

    Shared Property ShowDetectionToolTips As Boolean
        Get
            Return bShowDetectionToolTips
        End Get
        Set(value As Boolean)
            bShowDetectionToolTips = value
        End Set
    End Property

    Shared Property DisableConfirmation As Boolean
        Get
            Return bDisableConfirmation
        End Get
        Set(value As Boolean)
            bDisableConfirmation = value
        End Set
    End Property

    Shared Property CreateSubFolder As Boolean
        Get
            Return bCreateSubFolder
        End Get
        Set(value As Boolean)
            bCreateSubFolder = value
        End Set
    End Property

    Shared Property ShowOverwriteWarning As Boolean
        Get
            Return bShowOverwriteWarning
        End Get
        Set(value As Boolean)
            bShowOverwriteWarning = value
        End Set
    End Property

    Shared Property RestoreOnLaunch As Boolean
        Get
            Return bRestoreOnLaunch
        End Get
        Set(value As Boolean)
            bRestoreOnLaunch = value
        End Set
    End Property

    Shared Property AutoRestore As Boolean
        Get
            Return bAutoRestore
        End Get
        Set(value As Boolean)
            bAutoRestore = value
        End Set
    End Property

    Shared Property AutoMark As Boolean
        Get
            Return bAutoMark
        End Get
        Set(value As Boolean)
            bAutoMark = value
        End Set
    End Property

    Shared Property TimeTracking As Boolean
        Get
            Return bTimeTracking
        End Get
        Set(value As Boolean)
            bTimeTracking = value
        End Set
    End Property

    Shared Property SessionTracking As Boolean
        Get
            Return bSessionTracking
        End Get
        Set(value As Boolean)
            bSessionTracking = value
        End Set
    End Property

    Shared Property SuppressBackup As Boolean
        Get
            Return bSuppressBackup
        End Get
        Set(value As Boolean)
            bSuppressBackup = value
        End Set
    End Property

    Shared Property SuppressBackupThreshold As Integer
        Get
            Return iSuppressBackupThreshold
        End Get
        Set(value As Integer)
            iSuppressBackupThreshold = value
        End Set
    End Property

    Shared Property CompressionLevel As Integer
        Get
            Return iCompressionLevel
        End Get
        Set(value As Integer)
            iCompressionLevel = value
        End Set
    End Property

    Shared Property Custom7zArguments As String
        Get
            Return s7zArguments
        End Get
        Set(value As String)
            s7zArguments = value
        End Set
    End Property

    Shared Property ShowResolvedPaths As Boolean
        Get
            Return bShowResolvedPaths
        End Get
        Set(value As Boolean)
            bShowResolvedPaths = value
        End Set
    End Property

    Shared ReadOnly Property Prepared7zArguments As String
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

    Shared Property Custom7zLocation As String
        Get
            Return s7zLocation
        End Get
        Set(value As String)
            s7zLocation = value
        End Set
    End Property

    Shared ReadOnly Property Is7zUtilityValid As Boolean
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

    Shared ReadOnly Property Utility7zLocation As String
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

    Shared Property BackupFolder As String
        Get
            Return sBackupFolder
        End Get
        Set(value As String)
            sBackupFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property

    Shared Property TemporaryFolder As String
        Get
            Return sTemporaryFolder
        End Get
        Set(value As String)
            sTemporaryFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property

    Shared ReadOnly Property MetadataLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_MetadataFilename
        End Get
    End Property

    Shared ReadOnly Property IncludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupIncludeFileName
        End Get
    End Property

    Shared ReadOnly Property ExcludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupExcludeFileName
        End Get
    End Property

    Shared Property SyncFields As clsGame.eOptionalSyncFields
        Get
            Return eSyncFields
        End Get
        Set(value As clsGame.eOptionalSyncFields)
            eSyncFields = value
        End Set
    End Property

    Shared Property AutoSaveLog As Boolean
        Get
            Return bAutoSaveLog
        End Get
        Set(value As Boolean)
            bAutoSaveLog = value
        End Set
    End Property

    Shared Property SuppressMessages As eSuppressMessages
        Get
            Return eMessages
        End Get
        Set(value As eSuppressMessages)
            eMessages = value
        End Set
    End Property

    Shared Property BackupOnLaunch As Boolean
        Get
            Return bBackupOnLaunch
        End Get
        Set(value As Boolean)
            bBackupOnLaunch = value
        End Set
    End Property

    Shared Property UseGameID As Boolean
        Get
            Return bUseGameID
        End Get
        Set(value As Boolean)
            bUseGameID = value
        End Set
    End Property

    Shared Property StorePathAutoConfig As Boolean
        Get
            Return bStorePathAutoConfig
        End Get
        Set(value As Boolean)
            bStorePathAutoConfig = value
        End Set
    End Property

    Shared Property DisableSyncMessages As Boolean
        Get
            Return bDisableSyncMessages
        End Get
        Set(value As Boolean)
            bDisableSyncMessages = value
        End Set
    End Property

    Shared Property DisableDiskSpaceCheck As Boolean
        Get
            Return bDisableDiskSpaceCheck
        End Get
        Set(value As Boolean)
            bDisableDiskSpaceCheck = value
        End Set
    End Property

    Shared Property ExitOnClose As Boolean
        Get
            Return bExitOnClose
        End Get
        Set(value As Boolean)
            bExitOnClose = value
        End Set
    End Property

    Shared Property ExitNoWarning As Boolean
        Get
            Return bExitNoWarning
        End Get
        Set(value As Boolean)
            bExitNoWarning = value
        End Set
    End Property

    Shared Property EnableLauncher As Boolean
        Get
            Return bEnableLauncher
        End Get
        Set(value As Boolean)
            bEnableLauncher = value
        End Set
    End Property

    Shared Property MainHideGameList As Boolean
        Get
            Return bMainHideGameList
        End Get
        Set(value As Boolean)
            bMainHideGameList = value
        End Set
    End Property

    Shared Property MainHideLog As Boolean
        Get
            Return bMainHideLog
        End Get
        Set(value As Boolean)
            bMainHideLog = value
        End Set
    End Property

    Shared Property MainHideButtons As Boolean
        Get
            Return bMainHideButtons
        End Get
        Set(value As Boolean)
            bMainHideButtons = value
        End Set
    End Property

    Shared Property BackupNotification As Boolean
        Get
            Return bBackupNotification
        End Get
        Set(value As Boolean)
            bBackupNotification = value
        End Set
    End Property

    Shared Property DetectionSpeed As Integer
        Get
            Return iDetectionSpeed
        End Get
        Set(value As Integer)
            iDetectionSpeed = value
        End Set
    End Property

    Shared Property TwoPassDetection As Boolean
        Get
            Return bTwoPassDetection
        End Get
        Set(value As Boolean)
            bTwoPassDetection = value
        End Set
    End Property

    Shared Sub New()
        SetDefaults()
    End Sub

    Public Shared Sub SetDefaults()
        'Defaults
        bStartWithWindows = False
        bMonitoronStartup = True
        bStartToTray = False
        bShowDetectionToolTips = True
        bDisableConfirmation = False
        bCreateSubFolder = False
        bShowOverwriteWarning = True
        bRestoreOnLaunch = False
        bAutoRestore = False
        bAutoMark = False
        bTimeTracking = True
        bSessionTracking = False
        bSuppressBackup = False
        iSuppressBackupThreshold = 10
        iCompressionLevel = 5
        s7zArguments = String.Empty
        s7zLocation = String.Empty
        sBackupFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & Path.DirectorySeparatorChar & App_NameLong
        sTemporaryFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & Path.DirectorySeparatorChar & "gbm"
        eSyncFields = clsGame.eOptionalSyncFields.None
        eMessages = eSuppressMessages.None
        bAutoSaveLog = False
        bBackupOnLaunch = True
        bUseGameID = False
        bStorePathAutoConfig = False
        bDisableSyncMessages = True
        bShowResolvedPaths = True
        bDisableDiskSpaceCheck = False
        bExitNoWarning = False
        bEnableLauncher = False
        bMainHideGameList = False
        bMainHideButtons = False
        bMainHideLog = False
        bBackupNotification = False
        iDetectionSpeed = 5000
        bTwoPassDetection = True

        'OS Based Defaults
        If mgrCommon.IsUnix Then
            bExitOnClose = True
        Else
            bExitOnClose = False
        End If
    End Sub

    Private Shared Sub SaveFromClass()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT OR REPLACE INTO settings VALUES (1, @MonitorOnStartup, @StartToTray, @ShowDetectionToolTips, @DisableConfirmation, "
        sSQL &= "@CreateSubFolder, @ShowOverwriteWarning, @RestoreOnLaunch, @BackupFolder, @StartWithWindows, "
        sSQL &= "@TimeTracking, @SuppressBackup, @SuppressBackupThreshold, @CompressionLevel, @Custom7zArguments, @Custom7zLocation, "
        sSQL &= "@SyncFields, @AutoSaveLog, @AutoRestore, @AutoMark, @SessionTracking, @SuppressMessages, @BackupOnLaunch, @UseGameID, "
        sSQL &= "@DisableSyncMessages, @ShowResolvedPaths, @DisableDiskSpaceCheck, @TemporaryFolder, @ExitOnClose, @ExitNoWarning, @EnableLauncher, "
        sSQL &= "@MainHideGameList, @MainHideButtons, @MainHideLog, @BackupNotification, @DetectionSpeed, @TwoPassDetection, @StorePathAutoConfig)"

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
        hshParams.Add("EnableLauncher", EnableLauncher)
        hshParams.Add("MainHideGameList", MainHideGameList)
        hshParams.Add("MainHideButtons", MainHideButtons)
        hshParams.Add("MainHideLog", MainHideLog)
        hshParams.Add("BackupNotification", BackupNotification)
        hshParams.Add("DetectionSpeed", DetectionSpeed)
        hshParams.Add("TwoPassDetection", TwoPassDetection)
        hshParams.Add("StorePathAutoConfig", StorePathAutoConfig)

        oDatabase.RunParamQuery(sSQL, hshParams)
    End Sub

    Private Shared Sub MapToClass()
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
            EnableLauncher = CBool(dr("EnableLauncher"))
            MainHideGameList = CBool(dr("MainHideGameList"))
            MainHideButtons = CBool(dr("MainHideButtons"))
            MainHideLog = CBool(dr("MainHideLog"))
            BackupNotification = CBool(dr("BackupNotification"))
            DetectionSpeed = CInt(dr("DetectionSpeed"))
            TwoPassDetection = CBool(dr("TwoPassDetection"))
            StorePathAutoConfig = CBool(dr("StorePathAutoConfig"))
        Next

        oDatabase.Disconnect()
    End Sub

    Public Shared Sub LoadSettings()
        MapToClass()

        'Set Remote Manifest Location     
        mgrPath.RemoteDatabaseLocation = BackupFolder
    End Sub

    Public Shared Sub SaveSettings()
        SaveFromClass()

        'Set Remote Manifest Location        
        mgrPath.RemoteDatabaseLocation = BackupFolder
    End Sub

    Public Shared Function SetMessageField(ByVal eMessages As eSuppressMessages, ByVal eMessage As eSuppressMessages) As eSuppressMessages
        Return eMessages Or eMessage
    End Function

    Public Shared Function RemoveMessageField(ByVal eMessages As eSuppressMessages, ByVal eMessage As eSuppressMessages) As eSuppressMessages
        Return eMessages And (Not eMessage)
    End Function

End Class
