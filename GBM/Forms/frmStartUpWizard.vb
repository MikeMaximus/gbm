Imports System.IO

Public Class frmStartUpWizard

    Private oGameData As Hashtable
    Private oSettings As mgrSettings
    Private bShutdown As Boolean = False

    Property Settings As mgrSettings
        Get
            Return oSettings
        End Get
        Set(value As mgrSettings)
            oSettings = value
        End Set
    End Property

    Private Enum eSteps As Integer
        Step1 = 1
        Step2 = 2
        Step3 = 3
        Step3a = 4
        Step4 = 5
        Step5 = 6
    End Enum

    Private eCurrentStep As eSteps = eSteps.Step1

    Private Sub FormInit()
        llbManual.Links.Add(0, 26, "http://mikemaximus.github.io/gbm-web/manual.html")
        LoadGameSettings()
        StepHandler()
    End Sub

    Private Sub CheckSync()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Remote)

        'Check if a remote database already exists in the new backup location
        If oDatabase.CheckDB() Then
            'Make sure database is the latest version
            oDatabase.DatabaseUpgrade()
            mgrMonitorList.SyncMonitorLists(False)
            mgrCommon.ShowMessage("Existing data was detected in the backup folder and has been imported.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub StepHandler()
        Select Case eCurrentStep
            Case eSteps.Step1
                btnBack.Enabled = False
                btnNext.Enabled = True
                tabWizard.SelectTab(0)
            Case eSteps.Step2
                txtBackupPath.Text = oSettings.BackupFolder
                chkCreateFolder.Checked = oSettings.CreateSubFolder
                chkSync.Checked = oSettings.Sync
                btnBack.Enabled = True
                btnNext.Enabled = True
                tabWizard.SelectTab(1)
            Case eSteps.Step3
                btnBack.Enabled = False
                btnNext.Enabled = True
                btnNext.Text = "&Next"
                tabWizard.SelectTab(2)
            Case eSteps.Step4
                btnBack.Enabled = True
                btnNext.Enabled = True
                btnNext.Text = "&Finish"
                tabWizard.SelectTab(3)
        End Select
    End Sub

    Private Sub DownloadSettings()
        If mgrCommon.ShowMessage("Would you like to choose games to import from the official game list?" & vbCrLf & vbCrLf & "This require an active internet connection.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(mgrPath.OfficialImportURL) Then
                oGameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
                If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
            End If
        End If
    End Sub

    Private Sub LoadGameSettings()
        'Load Game XML
        oGameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
    End Sub

    Private Sub OpenGameWizard()
        Dim frm As New frmAddWizard
        frm.GameData = oGameData
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
    End Sub

    Private Sub OpenMonitorList()
        Dim frm As New frmGameManager
        frm.BackupFolder = oSettings.BackupFolder
        frm.DisableExternalFunctions = True
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists()
    End Sub

    Private Function ValidateBackupPath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = "You must select a backup path to continue."
            txtBackupPath.Focus()
            Return False
        End If

        If Not Directory.Exists(strPath) Then
            sErrorMessage = "The folder you selected does not exist or is not a valid folder."
            txtBackupPath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = "The selected path must be a full path."
            txtBackupPath.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub ValidateBack()
        Select Case eCurrentStep
            Case eSteps.Step2
                eCurrentStep = eSteps.Step1
            Case eSteps.Step3
                eCurrentStep = eSteps.Step2
            Case eSteps.Step3a
                eCurrentStep = eSteps.Step3
            Case eSteps.Step4
                eCurrentStep = eSteps.Step3
        End Select
        StepHandler()
    End Sub

    Private Sub ValidateNext()
        Dim bError As Boolean = False
        Dim sErrorMessage As String = String.Empty

        Select Case eCurrentStep
            Case eSteps.Step1
                eCurrentStep = eSteps.Step2
            Case eSteps.Step2
                If ValidateBackupPath(txtBackupPath.Text, sErrorMessage) Then
                    oSettings.BackupFolder = txtBackupPath.Text
                    oSettings.CreateSubFolder = chkCreateFolder.Checked
                    oSettings.Sync = chkSync.Checked
                    oSettings.SaveSettings()
                    oSettings.LoadSettings()
                    If oSettings.Sync Then CheckSync()
                    eCurrentStep = eSteps.Step3
                Else
                    bError = True
                End If

            Case eSteps.Step3
                eCurrentStep = eSteps.Step4

            Case eSteps.Step4
                bShutdown = True
                Me.Close()
        End Select

        If bError Then mgrCommon.ShowMessage(sErrorMessage, MsgBoxStyle.Exclamation)
        StepHandler()
    End Sub

    Private Sub BackupPathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = txtBackupPath.Text
        Dim sNewPath As String

        If txtBackupPath.Text <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("Choose GBM backup folder:", sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtBackupPath.Text = sNewPath
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        ValidateBack()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        ValidateNext()
    End Sub

    Private Sub frmStartUpWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormInit()
    End Sub

    Private Sub btnFolderBrowse_Click(sender As Object, e As EventArgs) Handles btnFolderBrowse.Click
        BackupPathBrowse()
    End Sub

    Private Sub btnDownloadList_Click(sender As Object, e As EventArgs) Handles btnDownloadList.Click
        DownloadSettings()
    End Sub

    Private Sub btnOpenWizard_Click(sender As Object, e As EventArgs) Handles btnOpenWizard.Click
        OpenGameWizard()
    End Sub

    Private Sub btnOpenMonitorList_Click(sender As Object, e As EventArgs) Handles btnOpenMonitorList.Click
        OpenMonitorList()
    End Sub

    Private Sub frmStartUpWizard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not bShutdown Then
            e.Cancel = True
        End If
    End Sub

    Private Sub llbManual_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llbManual.LinkClicked
        Process.Start(e.Link.LinkData.ToString)
    End Sub
End Class