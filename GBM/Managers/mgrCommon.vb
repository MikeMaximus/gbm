Imports GBM.My.Resources
Imports System.Net
Imports System.IO

Public Class mgrCommon

    Public Shared ReadOnly Property BuildVersion As Integer
        Get
            Return My.Application.Info.Version.Build
        End Get
    End Property

    Public Shared ReadOnly Property AppVersion As Integer
        Get
            Return (My.Application.Info.Version.Major * 100) + My.Application.Info.Version.Minor
        End Get
    End Property

    Public Shared Function CheckAddress(ByVal URL As String) As Boolean
        Try
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Shared Function DateToUnix(ByVal dDate As DateTime) As Int64
        Return DateDiff(DateInterval.Second, #1/1/1970#, dDate)
    End Function

    Public Shared Function UnixToDate(ByVal iDate As Int64) As DateTime
        Return DateAdd(DateInterval.Second, iDate, #1/1/1970#)
    End Function

    Public Shared Function BooleanYesNo(ByVal bBool As Boolean) As String
        If bBool Then
            Return mgrCommon_Yes
        Else
            Return mgrCommon_No
        End If
    End Function

    Public Shared Function SaveFileBrowser(ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String, ByVal sDefaultFile As String) As String
        Dim fbBrowser As New SaveFileDialog
        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
        fbBrowser.InitialDirectory = sDefaultFolder
        fbBrowser.FileName = sDefaultFile

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fbBrowser.FileName
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenFileBrowser(ByVal sTitle As String, ByVal sExtension As String, ByVal sFileType As String, ByVal sDefaultFolder As String, ByVal bMulti As Boolean) As String
        Dim fbBrowser As New OpenFileDialog
        fbBrowser.Title = sTitle
        fbBrowser.DefaultExt = sExtension
        fbBrowser.Filter = FormatString(mgrCommon_FilesFilter, New String() {sFileType, sExtension, sExtension})
        fbBrowser.InitialDirectory = sDefaultFolder
        fbBrowser.Multiselect = bMulti

        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If bMulti Then
                Dim sFileNames As String = String.Empty
                For Each sFileName As String In fbBrowser.FileNames
                    sFileNames &= sFileName & "|"
                Next
                sFileNames = sFileNames.TrimEnd("|")
                Return sFileNames
            Else
                Return fbBrowser.FileName
            End If
        End If

        Return String.Empty
    End Function

    Public Shared Function OpenFolderBrowser(ByVal sTitle As String, ByVal sDefaultFolder As String, ByVal bEnableNewFolder As Boolean) As String
        Dim fbBrowser As New FolderBrowserDialog
        fbBrowser.Description = sTitle
        fbBrowser.SelectedPath = sDefaultFolder
        fbBrowser.ShowNewFolderButton = bEnableNewFolder
        If fbBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fbBrowser.SelectedPath
        End If

        Return String.Empty
    End Function

    Public Shared Function IsProcessNotSearchable(ByVal oGame As clsGame) As Boolean
        Dim sExemptList As String() = {"dosbox", "scummvm"}
        Dim bFound As Boolean = False

        For Each s As String In sExemptList
            If oGame.ProcessName.ToLower.Contains(s) Then bFound = True
        Next

        If bFound Or oGame.Duplicate = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function IsElevated() As Boolean
        If My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Sub RestartAsAdmin()
        Dim oProcess As New Process

        oProcess.StartInfo.FileName = Application.ExecutablePath
        oProcess.StartInfo.UseShellExecute = True
        oProcess.StartInfo.CreateNoWindow = True
        oProcess.StartInfo.Verb = "runas"

        oProcess.Start()
    End Sub

    'Save string as text file
    Public Shared Sub SaveText(ByVal sText As String, ByVal sPath As String)
        Dim oStream As StreamWriter

        Try
            If File.Exists(sPath) Then My.Computer.FileSystem.DeleteFile(sPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            oStream = New StreamWriter(sPath)
            oStream.Write(sText)
            oStream.Flush()
            oStream.Close()
        Catch ex As Exception
            ShowMessage(mgrCommon_ErrorWritingTextFile, ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    'Get a file size
    Public Shared Function GetFileSize(ByVal sFile As String) As String
        Dim oFileInfo As FileInfo
        Dim dFileSize As Double

        Try
            oFileInfo = New FileInfo(sFile)
            dFileSize = oFileInfo.Length
            If dFileSize > 1048576 Then
                Return FormatString(App_MB, Math.Round(dFileSize / 1048576, 2).ToString)
            Else
                Return FormatString(App_KB, Math.Round(dFileSize / 1024, 2).ToString)
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function



    'Handles no extra parameters
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles single parameter stings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParam As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParam), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles multi-parameter strings
    Public Shared Function ShowMessage(ByVal sMsg As String, ByVal sParams As String(), ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = MsgBox(FormatString(sMsg, sParams), oType, My.Resources.App_NameLong)
        Return oResult
    End Function

    'Handles no extra parameters
    Public Shared Function FormatString(ByVal sString As String) As String
        sString = sString.Replace("[BR]", vbCrLf)

        Return sString
    End Function


    'Handles single parameter stings
    Public Shared Function FormatString(ByVal sString As String, ByVal sParam As String) As String
        sString = sString.Replace("[BR]", vbCrLf)
        sString = sString.Replace("[PARAM]", sParam)

        Return sString
    End Function

    'Handles multi-parameter strings
    Public Shared Function FormatString(ByVal sString As String, ByVal sParams As String()) As String
        Dim iParam As Integer

        sString = sString.Replace("[BR]", vbCrLf)

        For Each s As String In sParams
            iParam = sString.IndexOf("[PARAM]")
            sString = sString.Remove(iParam, 7)
            sString = sString.Insert(iParam, s)
        Next

        Return sString
    End Function

    'Maintenance Only - Function for string management
    Public Shared Sub GetAllStrings(ByVal ctlParent As Control, ByRef sResource As String, ByRef sCode As String, ByVal sFormName As String)
        For Each ctl As Control In ctlParent.Controls
            If TypeOf ctl Is GroupBox Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
                GetAllStrings(ctl, sResource, sCode, sFormName)
            ElseIf TypeOf ctl Is TabControl Then
                For Each tb As TabPage In ctl.Controls
                    GetAllStrings(tb, sResource, sCode, sFormName)
                Next
            ElseIf TypeOf ctl Is Label Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is Button Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is RadioButton Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            ElseIf TypeOf ctl Is CheckBox Then
                sResource &= sFormName & "_" & ctl.Name & vbTab & ctl.Text & vbCrLf
                sCode &= ctl.Name & ".Text = " & sFormName & "_" & ctl.Name & vbCrLf
            End If
        Next
    End Sub

End Class
