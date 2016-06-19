Imports GBM.My.Resources
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

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmStartUpWizard_FormName

        'Set Form Text
        btnNext.Text = frmStartUpWizard_btnNext
        btnBack.Text = frmStartUpWizard_btnBack
        lblStep1Instructions2.Text = frmStartUpWizard_lblStep1Instructions2
        llbManual.Text = frmStartUpWizard_llbManual
        lblStep1Title.Text = frmStartUpWizard_lblStep1Title
        lblStep1Instructions.Text = frmStartUpWizard_lblStep1Instructions
        chkSync.Text = frmStartUpWizard_chkSync
        chkCreateFolder.Text = frmStartUpWizard_chkCreateFolder
        lblStep2Title.Text = frmStartUpWizard_lblStep2Title
        lblStep2Instructions.Text = frmStartUpWizard_lblStep2Instructions
        btnFolderBrowse.Text = frmStartUpWizard_btnFolderBrowse
        lblStep2Intro.Text = frmStartUpWizard_lblStep2Intro
        btnOpenWizard.Text = frmStartUpWizard_btnOpenWizard
        btnOpenMonitorList.Text = frmStartUpWizard_btnOpenMonitorList
        btnDownloadList.Text = frmStartUpWizard_btnDownloadList
        lblStep3Title.Text = frmStartUpWizard_lblStep3Title
        lblStep3Intro.Text = frmStartUpWizard_lblStep3Intro
        lblStep4Instructions3.Text = frmStartUpWizard_lblStep4Instructions3
        lblStep4Instructions2.Text = frmStartUpWizard_lblStep4Instructions2
        lblStep4Title.Text = frmStartUpWizard_lblStep4Title
        lblStep4Instructions.Text = frmStartUpWizard_lblStep4Instructions

        llbManual.Links.Add(0, 26, App_URLManual)
        LoadGameSettings()
        StepHandler()
    End Sub

    Private Sub CheckSync()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Remote)

        'Check if a remote database already exists in the new backup location
        If oDatabase.CheckDB() Then
            'Make sure database is the latest version
            oDatabase.DatabaseUpgrade()
            mgrMonitorList.SyncMonitorLists(oSettings.SyncFields, False)
            mgrCommon.ShowMessage(frmStartUpWizard_ExistingData, MsgBoxStyle.Information)
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
                btnNext.Text = frmStartUpWizard_btnNext
                tabWizard.SelectTab(2)
            Case eSteps.Step4
                btnBack.Enabled = True
                btnNext.Enabled = True
                btnNext.Text = frmStartUpWizard_btnNext_Finish
                tabWizard.SelectTab(3)
        End Select
    End Sub

    Private Sub DownloadSettings()
        Dim sImportURL As String

        If mgrCommon.IsUnix Then
            sImportURL = App_URLImportLinux
        Else
            sImportURL = App_URLImport
        End If

        If mgrCommon.ShowMessage(frmStartUpWizard_ConfirmOfficialImport, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If mgrMonitorList.DoImport(sImportURL) Then
                oGameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
                If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
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
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
    End Sub

    Private Sub OpenMonitorList()
        Dim frm As New frmGameManager
        frm.BackupFolder = oSettings.BackupFolder
        frm.DisableExternalFunctions = True
        frm.ShowDialog()
        LoadGameSettings()
        If oSettings.Sync Then mgrMonitorList.SyncMonitorLists(oSettings.SyncFields)
    End Sub

    Private Function ValidateBackupPath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = frmStartUpWizard_ErrorNoFolder
            txtBackupPath.Focus()
            Return False
        End If

        If Not Directory.Exists(strPath) Then
            sErrorMessage = frmStartUpWizard_ErrorNoFolderExists
            txtBackupPath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = frmStartUpWizard_ErrorBadFolder
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

        sNewPath = mgrCommon.OpenFolderBrowser(frmStartUpWizard_BrowseFolder, sDefaultFolder, True)

        If sNewPath <> String.Empty Then txtBackupPath.Text = sNewPath
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        ValidateBack()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        ValidateNext()
    End Sub

    Private Sub frmStartUpWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
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