Imports System.IO
Imports GBM.My.Resources

Public Class mgrSettings
    <Flags()> Public Enum eSuppressMessages
        None = 0
        EmptyProcessWarning = 1
        LinkProcessTip = 2
        WinConfigsInLinux = 4
        WineConfig = 16
        LinkConfigTip = 32
        LudusaviImportWarning = 64
    End Enum

    Private Shared sBackupFolder As String
    Private Shared sTemporaryFolder As String

    Public Shared Property StartWithWindows As Boolean
    Public Shared Property MonitorOnStartup As Boolean
    Public Shared Property StartToTray As Boolean
    Public Shared Property ShowDetectionToolTips As Boolean
    Public Shared Property DisableConfirmation As Boolean
    Public Shared Property CreateSubFolder As Boolean
    Public Shared Property ShowOverwriteWarning As Boolean
    Public Shared Property RestoreOnLaunch As Boolean
    Public Shared Property AutoRestore As Boolean
    Public Shared Property AutoMark As Boolean
    Public Shared Property TimeTracking As Boolean
    Public Shared Property SessionTracking As Boolean
    Public Shared Property SuppressBackup As Boolean
    Public Shared Property SuppressBackupThreshold As Integer
    Public Shared Property CompressionLevel As Integer
    Public Shared Property Custom7zArguments As String
    Public Shared Property ShowResolvedPaths As Boolean
    Public Shared ReadOnly Property Prepared7zArguments As String
        Get
            'Prepare custom 7z arguments
            Dim sPreparedArguments As String
            If Custom7zArguments <> String.Empty Then
                'Surround the arguments with spaces to be inserted into command
                sPreparedArguments = " " & Custom7zArguments & " "
            Else
                'The command always needs at least one space inserted
                sPreparedArguments = " "
            End If
            Return sPreparedArguments
        End Get
    End Property
    Public Shared Property Custom7zLocation As String
    Public Shared ReadOnly Property Is7zUtilityValid As Boolean
        Get
            'We don't use a packaged 7za on Unix, assume valid.
            If mgrCommon.IsUnix Then
                Return True
            End If

            If Custom7zLocation = String.Empty Then
                'Verify stored hash of the default utility if we're using it
                Return mgrCommon.UtilityHash = mgrHash.Generate_SHA256_Hash(mgrPath.Default7zLocation)
            Else
                'When using a custom utility assume it's valid, we have no way to be sure.
                Return True
            End If
        End Get
    End Property
    Public Shared ReadOnly Property Utility7zLocation As String
        Get
            'Return default utility when custom setting is not used
            If Custom7zLocation = String.Empty Then
                Return mgrPath.Default7zLocation
            Else
                'Check if custom utility is available, if not use the default utility
                If File.Exists(Custom7zLocation) Then
                    Return Custom7zLocation
                Else
                    Return mgrPath.Default7zLocation
                End If
            End If
        End Get
    End Property
    Public Shared Property BackupFolder As String
        Get
            Return sBackupFolder
        End Get
        Set(value As String)
            sBackupFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property
    Public Shared Property TemporaryFolder As String
        Get
            Return sTemporaryFolder
        End Get
        Set(value As String)
            sTemporaryFolder = value.TrimEnd(New Char() {"\", "/"})
        End Set
    End Property
    Public Shared ReadOnly Property MetadataLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_MetadataFilename
        End Get
    End Property
    Public Shared ReadOnly Property IncludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupIncludeFileName
        End Get
    End Property
    Public Shared ReadOnly Property ExcludeFileLocation As String
        Get
            Return sTemporaryFolder & Path.DirectorySeparatorChar & App_BackupExcludeFileName
        End Get
    End Property
    Public Shared Property SyncFields As clsGame.eOptionalSyncFields
    Public Shared Property AutoSaveLog As Boolean
    Public Shared Property SuppressMessages As eSuppressMessages
    Public Shared Property BackupOnLaunch As Boolean
    Public Shared Property StorePathAutoConfig As Boolean
    Public Shared Property DisableSyncMessages As Boolean
    Public Shared Property DisableDiskSpaceCheck As Boolean
    Public Shared Property ExitOnClose As Boolean
    Public Shared Property ExitNoWarning As Boolean
    Public Shared Property EnableLauncher As Boolean
    Public Shared Property MainHideGameList As Boolean
    Public Shared Property MainHideLog As Boolean
    Public Shared Property MainHideButtons As Boolean
    Public Shared Property BackupNotification As Boolean
    Public Shared Property DetectionSpeed As Integer
    Public Shared Property TwoPassDetection As Boolean
    Public Shared Property DeleteToRecycleBin As Boolean
    Public Shared Property EnableHotKeys As Boolean
    Public Shared Property BackupHotKey As Keys
    Public Shared Property RestoreHotKey As Keys
    Public Shared Property EnableLiveBackup As Boolean
    Public Shared Property EnableOSTheme As Boolean

    Shared Sub New()
        SetDefaults()
    End Sub

    Public Shared Sub SetDefaults()
        'Defaults
        StartWithWindows = False
        MonitorOnStartup = True
        StartToTray = False
        ShowDetectionToolTips = True
        DisableConfirmation = False
        CreateSubFolder = False
        ShowOverwriteWarning = True
        RestoreOnLaunch = False
        AutoRestore = False
        AutoMark = False
        TimeTracking = True
        SessionTracking = False
        SuppressBackup = False
        SuppressBackupThreshold = 10
        CompressionLevel = 5
        Custom7zArguments = String.Empty
        Custom7zLocation = String.Empty
        If mgrPath.IsPortable Then
            sBackupFolder = Application.StartupPath & Path.DirectorySeparatorChar & App_FoldersBackup
        Else
            sBackupFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & Path.DirectorySeparatorChar & App_NameLong
        End If
        sTemporaryFolder = mgrPath.SettingsRoot
        SyncFields = clsGame.eOptionalSyncFields.None
        SuppressMessages = eSuppressMessages.None
        AutoSaveLog = False
        BackupOnLaunch = True
        StorePathAutoConfig = False
        DisableSyncMessages = True
        ShowResolvedPaths = True
        DisableDiskSpaceCheck = False
        ExitNoWarning = False
        EnableLauncher = False
        MainHideGameList = False
        MainHideButtons = False
        MainHideLog = False
        BackupNotification = False
        DetectionSpeed = 5000
        TwoPassDetection = True
        DeleteToRecycleBin = True
        ExitOnClose = False
        EnableHotKeys = False
        BackupHotKey = 196724
        RestoreHotKey = 196728
        EnableLiveBackup = False
        EnableOSTheme = True
    End Sub

    Private Shared Sub SaveFromClass()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Local)
        Dim sSQL As String
        Dim hshParams As New Hashtable

        sSQL = "INSERT OR REPLACE INTO settings VALUES (1, @MonitorOnStartup, @StartToTray, @ShowDetectionToolTips, @DisableConfirmation, "
        sSQL &= "@CreateSubFolder, @ShowOverwriteWarning, @RestoreOnLaunch, @BackupFolder, @StartWithWindows, "
        sSQL &= "@TimeTracking, @SuppressBackup, @SuppressBackupThreshold, @CompressionLevel, @Custom7zArguments, @Custom7zLocation, "
        sSQL &= "@SyncFields, @AutoSaveLog, @AutoRestore, @AutoMark, @SessionTracking, @SuppressMessages, @BackupOnLaunch, "
        sSQL &= "@DisableSyncMessages, @ShowResolvedPaths, @DisableDiskSpaceCheck, @TemporaryFolder, @ExitOnClose, @ExitNoWarning, @EnableLauncher, "
        sSQL &= "@MainHideGameList, @MainHideButtons, @MainHideLog, @BackupNotification, @DetectionSpeed, @TwoPassDetection, @StorePathAutoConfig, "
        sSQL &= "@DeleteToRecycleBin, @EnableHotKeys, @BackupHotKey, @RestoreHotKey, @EnableLiveBackup, @EnableOSTheme)"

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
        hshParams.Add("DeleteToRecycleBin", DeleteToRecycleBin)
        hshParams.Add("EnableHotKeys", EnableHotKeys)
        hshParams.Add("BackupHotKey", CInt(BackupHotKey))
        hshParams.Add("RestoreHotKey", CInt(RestoreHotKey))
        hshParams.Add("EnableLiveBackup", EnableLiveBackup)
        hshParams.Add("EnableOSTheme", EnableOSTheme)

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
            DeleteToRecycleBin = CBool(dr("DeleteToRecycleBin"))
            EnableHotKeys = CBool(dr("EnableHotKeys"))
            BackupHotKey = CInt(dr("BackupHotKey"))
            RestoreHotKey = CInt(dr("RestoreHotKey"))
            EnableLiveBackup = CBool(dr("EnableLiveBackup"))
            EnableOSTheme = CBool(dr("EnableOSTheme"))
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
