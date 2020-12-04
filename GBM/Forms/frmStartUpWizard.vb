Imports GBM.My.Resources
Imports System.IO

Public Class frmStartUpWizard

    Private oGameData As Hashtable
    Private bShutdown As Boolean = False

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
        Me.Icon = GBM_Icon

        'Set Form Text
        btnNext.Text = frmStartUpWizard_btnNext
        btnBack.Text = frmStartUpWizard_btnBack
        lblStep1Instructions2.Text = frmStartUpWizard_lblStep1Instructions2
        llbManual.Text = frmStartUpWizard_llbManual
        lblStep1Title.Text = frmStartUpWizard_lblStep1Title
        lblStep1Instructions.Text = frmStartUpWizard_lblStep1Instructions
        chkCreateFolder.Text = frmStartUpWizard_chkCreateFolder
        lblStep2Title.Text = frmStartUpWizard_lblStep2Title
        lblStep2Instructions.Text = frmStartUpWizard_lblStep2Instructions
        lblStep2Warning.Text = frmStartUpWizard_lblStep2Warning
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
        txtBackupPath.Text = mgrSettings.BackupFolder

        StepHandler()
    End Sub

    Private Sub CheckSync()
        Dim oDatabase As New mgrSQLite(mgrSQLite.Database.Remote)
        Dim bExistingData As Boolean = False
        Dim sMessage As String

        'Check if a remote database already exists in the new backup location, if not, then check for possible existing backup files to import
        If oDatabase.CheckDB() Then
            'Make sure database is the latest version
            oDatabase.DatabaseUpgrade()
            mgrMonitorList.SyncMonitorLists(False)
            bExistingData = True
            mgrCommon.ShowMessage(frmStartUpWizard_ExistingData, MsgBoxStyle.Information)
        End If

        'Scan for any archives to import
        Cursor.Current = Cursors.WaitCursor
        Dim sFilesFound As List(Of String)
        sFilesFound = mgrCommon.GetFileListByFolder(mgrSettings.BackupFolder, New String() {"*.7z"})
        Dim sFilesToImport(sFilesFound.Count) As String
        sFilesFound.CopyTo(sFilesToImport)
        Cursor.Current = Cursors.Default

        'Ask if the user wants to verify and import any files
        If sFilesFound.Count > 0 Then
            'Use a different message depending on if an existing database was also found
            If bExistingData Then
                sMessage = frmStartUpWizard_ExistingDataAndFilesDetected
            Else
                sMessage = frmStartUpWizard_ExistingFilesDetected
            End If

            If mgrCommon.ShowMessage(sMessage, sFilesFound.Count, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Cursor.Current = Cursors.WaitCursor
                Dim oBackup As New mgrBackup
                oBackup.ImportBackupFiles(sFilesToImport)
                Cursor.Current = Cursors.Default
            End If
        End If
    End Sub

    Private Sub StepHandler()
        Select Case eCurrentStep
            Case eSteps.Step1
                btnBack.Enabled = False
                btnNext.Enabled = True
                tabWizard.SelectTab(0)
            Case eSteps.Step2
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
            If mgrMonitorList.DoImport(sImportURL, True) Then
                oGameData = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)
                mgrMonitorList.SyncMonitorLists()
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
        mgrMonitorList.SyncMonitorLists()
    End Sub

    Private Sub OpenMonitorList()
        Dim frm As New frmGameManager
        frm.DisableExternalFunctions = True
        frm.ShowDialog()
        LoadGameSettings()
    End Sub

    Private Function ValidateBackupPath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = frmStartUpWizard_ErrorNoFolder
            txtBackupPath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = frmStartUpWizard_ErrorBadFolder
            txtBackupPath.Focus()
            Return False
        End If

        If Not Directory.Exists(strPath) Then
            Try
                Directory.CreateDirectory(strPath)
            Catch ex As Exception
                sErrorMessage = frmStartUpWizard_ErrorNoFolderExists
                txtBackupPath.Focus()
                Return False
            End Try
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
                chkCreateFolder.Checked = True
            Case eSteps.Step2
                If ValidateBackupPath(txtBackupPath.Text, sErrorMessage) Then
                    mgrSettings.BackupFolder = txtBackupPath.Text
                    mgrSettings.CreateSubFolder = chkCreateFolder.Checked
                    mgrSettings.SaveSettings()
                    CheckSync()
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

        sNewPath = mgrCommon.OpenClassicFolderBrowser("Wizard_Backup_Path", frmStartUpWizard_BrowseFolder, sDefaultFolder, True, False)

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
        mgrCommon.OpenInOS(e.Link.LinkData.ToString, , True)
    End Sub
End Class