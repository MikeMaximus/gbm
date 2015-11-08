Imports System.IO

Public Class frmAddWizard

    Private oGameData As Hashtable
    Private oGameToSave As clsGame
    Private bDisableAdminWarning As Boolean = False
    Private bIsDOSBoxGame As Boolean

    Property GameData As Hashtable
        Get
            Return oGameData
        End Get
        Set(value As Hashtable)
            oGameData = value
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
        btndBoxBrowse.Visible = False
        txtdBoxProcess.Visible = False
        lbldBox.Visible = False
        chkFolderSave.Checked = True
        chkTimeStamp.Checked = False
        optFileType.Checked = True
        optExcludeFileType.Checked = True    
        StepHandler()
    End Sub

    Private Function StringEmptyText(ByVal sString As String) As String
        If sString = String.Empty Then
            Return "None"
        Else
            Return sString
        End If
    End Function

    Private Function BuildSummaryAndData() As clsGame
        Dim oGame As New clsGame
        Dim sName As String = txtName.Text
        Dim sProcessFullPath As String = txtProcessPath.Text
        Dim sProcessPath As String = Path.GetDirectoryName(sProcessFullPath)
        Dim sProcess As String = Path.GetFileNameWithoutExtension(sProcessFullPath)
        Dim sProcessSummaryText As String = Path.GetFileName(sProcessFullPath) & " (" & sProcessPath & ")"
        Dim sdBoxProcessFullPath As String = String.Empty
        Dim sdBoxProcessPath As String = String.Empty
        Dim sdBoxProcess As String = String.Empty
        Dim sdBoxProcessSummaryText As String = String.Empty
        Dim sSavePath As String = txtSavePath.Text
        Dim bIsAbsolute As Boolean = mgrPath.IsAbsolute(sSavePath)
        Dim bFolderBackup As Boolean = chkFolderSave.Checked
        Dim bTimeStamp As Boolean = chkTimeStamp.Checked
        Dim sFileType As String = txtFileTypes.Text
        Dim sExcludeList As String = txtExcludeList.Text
        Dim sItem As String()
        Dim sItems As String()
        Dim sValues As String()
        Dim lstItem As ListViewItem

        If Not bIsAbsolute Then
            sSavePath = mgrPath.DetermineRelativePath(sProcessPath, sSavePath)
        End If

        If bIsDOSBoxGame Then
            sdBoxProcessFullPath = txtdBoxProcess.Text
            sdBoxProcessPath = Path.GetDirectoryName(sdBoxProcessFullPath)
            sdBoxProcess = Path.GetFileNameWithoutExtension(sdBoxProcessFullPath)
            sdBoxProcessSummaryText = Path.GetFileName(sdBoxProcessFullPath) & " (" & sdBoxProcessPath & ")"
            sProcess = sProcess.ToLower & ":" & sdBoxProcess
        End If

        'Build Summary Listview
        lstSummary.Clear()
        lstSummary.Columns.Add("Item")
        lstSummary.Columns.Add("Value")
        lstSummary.Columns(0).Width = 95
        lstSummary.Columns(1).Width = 210

        sItems = {"Name", "Process", "DOS Process", "Absolute Path", "Save Path", "Folder Backup", "Specific Files", "Time Stamp", "Exclude List"}
        sValues = {sName, sProcessSummaryText, StringEmptyText(sdBoxProcessSummaryText), mgrCommon.BooleanYesNo(bIsAbsolute), sSavePath, mgrCommon.BooleanYesNo(bFolderBackup), StringEmptyText(sFileType), mgrCommon.BooleanYesNo(bTimeStamp), StringEmptyText(sExcludeList)}

        For i = 0 To sItems.Length - 1
            sItem = {sItems(i), sValues(i)}
            lstItem = New ListViewItem(sItem)
            lstSummary.Items.Add(lstItem)
        Next

        'Build Save Object
        oGame.Name = sName
        oGame.ProcessName = sProcess
        oGame.Path = sSavePath
        oGame.AbsolutePath = bIsAbsolute
        oGame.FolderSave = bFolderBackup
        oGame.FileType = sFileType
        oGame.AppendTimeStamp = bTimeStamp
        oGame.ExcludeList = sExcludeList

        Return oGame
    End Function

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
                btnBack.Enabled = True
                btnNext.Enabled = True
                tabWizard.SelectTab(2)
            Case eSteps.Step3a
                btnBack.Enabled = True
                btnNext.Enabled = True
                tabWizard.SelectTab(3)
            Case eSteps.Step4
                btnBack.Enabled = True
                btnNext.Enabled = True
                btnNext.Text = "&Next"
                tabWizard.SelectTab(4)
            Case eSteps.Step5
                btnBack.Enabled = True
                btnNext.Text = "&Finish"
                tabWizard.SelectTab(5)
        End Select
    End Sub

    Private Function ValidateName(ByVal strName As String, ByRef sErrorMessage As String) As Boolean
        If txtName.Text <> String.Empty Then
            txtName.Text = mgrPath.ValidateForFileSystem(txtName.Text)
            Return True
        Else
            sErrorMessage = "You must enter a valid game name."
            txtName.Focus()
            Return False
        End If
        Return True
    End Function

    Private Function CheckforDOSBox(ByVal strpath As String) As Boolean
        If strpath.ToLower.Contains("dosbox.exe") Then
            bIsDOSBoxGame = True
            lbldBox.Visible = True
            txtdBoxProcess.Visible = True
            btndBoxBrowse.Visible = True
            If txtdBoxProcess.Text = String.Empty Then
                If MsgBox("You have selected a DOSBox executable, to monitor DOS programs you need to also select the actual DOS executable file." & vbCrLf & vbCrLf & "Would you like to do this now?", MsgBoxStyle.YesNo, "Game Backup Monitor") = MsgBoxResult.Yes Then
                    DOSProcessBrowse()
                End If
            End If
            Return True
        Else
            bIsDOSBoxGame = False
            lbldBox.Visible = False
            txtdBoxProcess.Text = String.Empty
            txtdBoxProcess.Visible = False
            btndBoxBrowse.Visible = False
            Return False
        End If
    End Function

    Private Function ValidateDOSProcessPath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = "You must select the DOS game's executable file (.exe or .com) to continue."
            txtProcessPath.Focus()
            Return False
        End If

        If Path.GetExtension(strPath.ToLower) <> ".exe" And Path.GetExtension(strPath.ToLower) <> ".com" Then
            sErrorMessage = "The DOS file you selected is not an executable file. (.exe or .com)"
            txtProcessPath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = "The path to the DOS executable must be a full path."
            txtProcessPath.Focus()
            Return False
        End If

        If Not File.Exists(strPath) Then
            sErrorMessage = "The selected DOS executable file does not exist."
            txtProcessPath.Focus()
            Return False
        End If

        Return True
    End Function

    Private Function ValidateProcessPath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = "You must select the game's executable file (.exe) to continue."
            txtProcessPath.Focus()
            Return False
        End If

        If Path.GetExtension(strPath.ToLower) <> ".exe" Then
            sErrorMessage = "The file you selected is not an executable file."
            txtProcessPath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = "The path to the executable must be a full path."
            txtProcessPath.Focus()
            Return False
        End If

        If Not File.Exists(strPath) Then
            sErrorMessage = "The selected executable file does not exist."
            txtProcessPath.Focus()
            Return False
        End If

        Return True
    End Function

    Private Function ValidateSavePath(ByVal strPath As String, ByRef sErrorMessage As String) As Boolean
        If strPath = String.Empty Then
            sErrorMessage = "You must select the game's save file path to continue."
            txtSavePath.Focus()
            Return False
        End If

        If Not Directory.Exists(strPath) Then
            sErrorMessage = "The folder you selected does not exist or is not a valid folder."
            txtSavePath.Focus()
            Return False
        End If

        If Not Path.IsPathRooted(strPath) Then
            sErrorMessage = "The selected path must be a full path."
            txtSavePath.Focus()
            Return False
        End If

        Return True
    End Function

    Private Function ValidateSaveType(ByVal strSaveType As String, ByRef sErrorMessage As String)
        If strSaveType = String.Empty Then
            sErrorMessage = "You must choose a file type or specific file to backup."
            txtFileTypes.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub DoSave()
        Dim oGames As New List(Of clsGame)
        Dim hshDupeCheck As New Hashtable
        Dim sExistingGame As String
        Dim sNewGame As String = oGameToSave.Name & ":" & oGameToSave.ProcessName

        For Each o As clsGame In GameData.Values
            oGames.Add(o)
            sExistingGame = o.Name & ":" & o.ProcessName
            hshDupeCheck.Add(sExistingGame, String.Empty)
        Next

        If hshDupeCheck.Contains(sNewGame) Then
            MsgBox("The monitor list already contains a game with this exact name and process.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
        Else
            mgrMonitorList.DoListAdd(oGameToSave)
            MsgBox(oGameToSave.Name & " has been added to the monitor list.", MsgBoxStyle.Exclamation, "Game Backup Monitor")
            Me.Close()
        End If
    End Sub

    Private Sub ValidateBack()

        Select Case eCurrentStep

            Case eSteps.Step2
                eCurrentStep = eSteps.Step1

            Case eSteps.Step3
                eCurrentStep = eSteps.Step2

            Case eSteps.Step3a
                eCurrentStep = eSteps.Step3

            Case eSteps.Step4
                If chkFolderSave.Checked Then
                    eCurrentStep = eSteps.Step3
                Else
                    eCurrentStep = eSteps.Step3a
                End If

            Case eSteps.Step5
                eCurrentStep = eSteps.Step4

        End Select

        StepHandler()
    End Sub

    Private Sub ValidateNext()
        Dim bError As Boolean = False
        Dim sErrorMessage As String = String.Empty

        Select Case eCurrentStep
            Case eSteps.Step1
                If ValidateName(txtName.Text, sErrorMessage) Then
                    eCurrentStep = eSteps.Step2
                Else
                    bError = True
                End If

            Case eSteps.Step2
                If ValidateProcessPath(txtProcessPath.Text, sErrorMessage) Then
                    If CheckforDOSBox(txtProcessPath.Text) Then
                        If ValidateDOSProcessPath(txtdBoxProcess.Text, sErrorMessage) Then
                            eCurrentStep = eSteps.Step3
                        Else
                            bError = True
                        End If
                    Else
                        eCurrentStep = eSteps.Step3
                    End If
                Else
                    bError = True
                End If

            Case eSteps.Step3
                If ValidateSavePath(txtSavePath.Text, sErrorMessage) Then
                    If chkFolderSave.Checked = False Then
                        eCurrentStep = eSteps.Step3a
                    Else
                        eCurrentStep = eSteps.Step4
                    End If
                Else
                    bError = True
                End If

            Case eSteps.Step3a
                If ValidateSaveType(txtFileTypes.Text, sErrorMessage) Then
                    eCurrentStep = eSteps.Step4
                Else
                    bError = True
                End If
            Case eSteps.Step4
                oGameToSave = BuildSummaryAndData()
                eCurrentStep = eSteps.Step5
            Case eSteps.Step5
                DoSave()
        End Select

        If bError Then MsgBox(sErrorMessage, MsgBoxStyle.Exclamation, "Game Backup Monitor")
        StepHandler()
    End Sub

    Private Sub ReadShortcut(ByVal sLinkPath As String)
        Dim oShell As Shell32.Shell
        Dim oFolder As Shell32.Folder
        Dim sDirectory As String = Path.GetDirectoryName(sLinkPath)
        Dim sFile As String = Path.GetFileName(sLinkPath)
        Dim sExtension As String = Path.GetExtension(sFile)
        Dim sTemp As String

        If sExtension = ".lnk" Then
            Try
                oShell = New Shell32.Shell
                oFolder = oShell.NameSpace(sDirectory)
                If (Not oFolder Is Nothing) Then
                    Dim oFolderItem As Shell32.FolderItem
                    oFolderItem = oFolder.ParseName(sFile)
                    If (Not oFolderItem Is Nothing) Then
                        Dim oShellLink As Shell32.ShellLinkObject
                        oShellLink = oFolderItem.GetLink
                        If (Not oShellLink Is Nothing) Then
                            If eCurrentStep = eSteps.Step1 Then
                                txtName.Text = System.IO.Path.GetFileNameWithoutExtension(sFile)
                                txtProcessPath.Text = oShellLink.Target.Path
                            Else
                                txtProcessPath.Text = oShellLink.Target.Path
                            End If
                        End If
                        oShellLink = Nothing
                    End If
                    oFolderItem = Nothing
                End If
                oFolder = Nothing
                oShell = Nothing
            Catch e1 As Exception
                If e1.Message.Contains("E_ACCESSDENIED") Then
                    sTemp = Path.GetTempPath & Path.GetFileName(sFile)
                    Try
                        File.Copy(sLinkPath, sTemp, True)
                        ReadShortcut(sTemp)
                        File.Delete(sTemp)
                    Catch e2 As Exception
                        MsgBox("An error occured working with the shortcut file." & vbCrLf & vbCrLf & e2.Message, MsgBoxStyle.Critical, "Game Backup Monitor")
                    End Try
                Else
                    MsgBox("An error occured reading the shortcut file." & vbCrLf & vbCrLf & e1.Message, MsgBoxStyle.Critical, "Game Backup Monitor")
                End If
            End Try
        Else
            MsgBox("This file is not a shorcut.")
        End If
    End Sub

    Private Sub DOSProcessBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String
        Dim sNewPath As String

        If txtProcessPath.Text <> String.Empty Then
            sCurrentPath = Path.GetDirectoryName(txtProcessPath.Text)
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFileBrowser("Choose DOS executable file that starts the game", "*", _
                                          "", sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtdBoxProcess.Text = sNewPath
    End Sub

    Private Sub ProcessBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim sCurrentPath As String
        Dim sNewPath As String

        If txtProcessPath.Text <> String.Empty Then
            sCurrentPath = Path.GetDirectoryName(txtProcessPath.Text)
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFileBrowser("Choose exe file that starts the game", "exe", _
                                          "Executable", sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtProcessPath.Text = sNewPath
    End Sub

    Private Sub SavePathBrowse()
        Dim sDefaultFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sCurrentPath As String = txtSavePath.Text
        Dim sNewPath As String

        If txtSavePath.Text <> String.Empty Then
            If Directory.Exists(sCurrentPath) Then
                sDefaultFolder = sCurrentPath
            End If
        End If

        sNewPath = mgrCommon.OpenFolderBrowser("Choose the game save folder:", sDefaultFolder, False)

        If sNewPath <> String.Empty Then txtSavePath.Text = sNewPath
    End Sub

    Private Sub SaveTypeBrowse()
        Dim sDefaultFolder As String = txtSavePath.Text
        Dim sCurrentSaveType As String = txtFileTypes.Text
        Dim sSaveType As String
        Dim sTitle As String = String.Empty
        Dim sSaveTypeList As String()

        If optFileType.Checked Then
            sTitle = "Choose the type of file to backup"
        ElseIf optSpecificFile.Checked Then
            sTitle = "Choose a specific file to backup"
        Else
            sTitle = "Choose a specific folder to backup"
        End If

        If optFileType.Checked Or optSpecificFile.Checked Then
            sSaveType = mgrCommon.OpenFileBrowser(sTitle, "*", "All", sDefaultFolder, False)
            If sSaveType <> String.Empty Then
                sSaveTypeList = sSaveType.Split("|")
                For Each s As String In sSaveTypeList
                    If optFileType.Checked Then
                        sSaveType = "*" & Path.GetExtension(s) & ":"
                    Else
                        sSaveType = Path.GetFileName(s) & ":"
                    End If

                    sSaveType = sSaveType.TrimEnd(":")

                    If sCurrentSaveType <> String.Empty Then
                        sCurrentSaveType &= ":" & sSaveType
                    Else
                        sCurrentSaveType = sSaveType
                    End If
                Next
            End If
        Else
            sSaveType = mgrCommon.OpenFolderBrowser(sTitle, sDefaultFolder, False)
            If sSaveType <> String.Empty Then
                Dim sPath As String() = sSaveType.Split("\")

                If sCurrentSaveType <> String.Empty Then
                    sCurrentSaveType &= ":" & sPath(sPath.Length - 1)
                Else
                    sCurrentSaveType = sPath(sPath.Length - 1)
                End If
            End If
        End If

        txtFileTypes.Text = sCurrentSaveType
    End Sub

    Private Sub ExcludeBrowse()
        Dim sDefaultFolder As String = txtSavePath.Text
        Dim sCurrentExclude As String = txtExcludeList.Text
        Dim sExclude As String
        Dim sExcludeList As String()
        Dim sTitle As String = String.Empty

        If optExcludeFileType.Checked Then
            sTitle = "Choose the type of file(s) to exclude"
        ElseIf optExcludeSpecificFile.Checked Then
            sTitle = "Choose any file(s) to exclude"
        Else
            sTitle = "Choose a folder to exclude"
        End If

        If optExcludeFileType.Checked Or optExcludeSpecificFile.Checked Then
            sExclude = mgrCommon.OpenFileBrowser(sTitle, "*", "All", sDefaultFolder, True)
            If sExclude <> String.Empty Then
                sExcludeList = sExclude.Split("|")
                For Each s As String In sExcludeList
                    If optExcludeFileType.Checked Then
                        sExclude = "*" & Path.GetExtension(s) & ":"
                    Else
                        sExclude = Path.GetFileName(s) & ":"
                    End If

                    sExclude = sExclude.TrimEnd(":")

                    If sCurrentExclude <> String.Empty Then
                        sCurrentExclude &= ":" & sExclude
                    Else
                        sCurrentExclude = sExclude
                    End If
                Next
            End If
        Else
            sExclude = mgrCommon.OpenFolderBrowser(sTitle, sDefaultFolder, False)
            If sExclude <> String.Empty Then
                Dim sPath As String() = sExclude.Split("\")

                If sCurrentExclude <> String.Empty Then
                    sCurrentExclude &= ":" & sPath(sPath.Length - 1)
                Else
                    sCurrentExclude = sPath(sPath.Length - 1)
                End If
            End If
        End If

        If sCurrentExclude <> String.Empty Then
            txtExcludeList.Text = sCurrentExclude
        End If

    End Sub

    

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        ValidateBack()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        ValidateNext()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmAddWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormInit()
    End Sub

    Private Sub DropTarget_DragEnter(sender As Object, e As DragEventArgs) Handles lblDrag1.DragEnter, lblDrag2.DragEnter, txtName.DragEnter, txtProcessPath.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub DropTarget_DragDrop(sender As Object, e As DragEventArgs) Handles lblDrag1.DragDrop, lblDrag2.DragDrop, txtName.DragDrop, txtProcessPath.DragDrop
        Dim oFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each sPath In oFiles
            ReadShortcut(sPath)
        Next
    End Sub

    Private Sub btndBoxBrowse_Click(sender As Object, e As EventArgs) Handles btndBoxBrowse.Click
        DOSProcessBrowse()
    End Sub

    Private Sub btnProcessBrowse_Click(sender As Object, e As EventArgs) Handles btnProcessBrowse.Click
        ProcessBrowse()
    End Sub

    Private Sub btnSaveBrowse_Click(sender As Object, e As EventArgs) Handles btnSaveBrowse.Click
        SavePathBrowse()
    End Sub

    Private Sub btnFileTypeBrowse_Click(sender As Object, e As EventArgs) Handles btnFileTypeBrowse.Click
        SaveTypeBrowse()
    End Sub

    Private Sub btnExcludeBrowse_Click(sender As Object, e As EventArgs) Handles btnExcludeBrowse.Click
        ExcludeBrowse()
    End Sub

    Private Sub btnStep4Clear_Click(sender As Object, e As EventArgs) Handles btnStep4Clear.Click
        txtExcludeList.Clear()
    End Sub

    Private Sub btnStep3aClear_Click(sender As Object, e As EventArgs) Handles btnStep3aClear.Click
        txtFileTypes.Clear()
    End Sub

    Private Sub chkFolderSave_CheckedChanged(sender As Object, e As EventArgs) Handles chkFolderSave.CheckedChanged
        txtFileTypes.Clear()
    End Sub

End Class