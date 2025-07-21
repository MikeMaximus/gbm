Imports DarkModeForms

Public Class mgrDarkMode

    Private Shared Function ConvertMsgStyleToDarkMode(ByRef oType As MsgBoxStyle) As MessageBoxButtons
        Select Case oType
            Case MsgBoxStyle.AbortRetryIgnore
                Return MessageBoxButtons.AbortRetryIgnore
            Case MsgBoxStyle.OkCancel
                Return MessageBoxButtons.OKCancel
            Case MsgBoxStyle.RetryCancel
                Return MessageBoxButtons.RetryCancel
            Case MsgBoxStyle.YesNo
                Return MessageBoxButtons.YesNo
            Case MsgBoxStyle.YesNoCancel
                Return MessageBoxButtons.YesNoCancel
            Case Else
                Return MessageBoxButtons.OK
        End Select
    End Function

    Private Shared Function ConvertMsgIconToDarkMode(ByRef oType As MsgBoxStyle) As MessageBoxIcon
        Select Case oType
            Case MsgBoxStyle.AbortRetryIgnore, MsgBoxStyle.RetryCancel, MsgBoxStyle.Critical
                Return MessageBoxIcon.Error
            Case MsgBoxStyle.Exclamation
                Return MessageBoxIcon.Exclamation
            Case MsgBoxStyle.OkCancel, MsgBoxStyle.YesNo, MessageBoxButtons.YesNoCancel
                Return MessageBoxIcon.Question
            Case Else
                Return MessageBoxIcon.Information
        End Select
    End Function

    Public Shared Function ShowDarkModeInput(ByVal sPrompt As String, ByVal sTitle As String, ByVal sDefaultValue As String) As String
        Dim oResult As MsgBoxResult
        Dim sInputValue As New List(Of KeyValue)
        Dim sValue As String

        'The dark mode input window can handle multiple fields, but for the purposes of GBM we only support a single field.
        sInputValue.Add(New KeyValue("Input", sDefaultValue, KeyValue.ValueTypes.String))

        oResult = Messenger.InputBox(sTitle, sPrompt, sInputValue)

        If oResult = MsgBoxResult.Ok Then
            sValue = sInputValue(0).Value
        Else
            sValue = sDefaultValue
        End If

        Return sValue
    End Function

    Public Shared Function ShowDarkModeMessage(ByVal sMsg As String, ByVal oType As MsgBoxStyle) As MsgBoxResult
        Dim oResult As MsgBoxResult
        oResult = Messenger.MessageBox(sMsg, My.Resources.App_NameLong, ConvertMsgStyleToDarkMode(oType), ConvertMsgIconToDarkMode(oType))
        Return oResult
    End Function

    Private Shared Sub ApplyCustomFixes(ByRef oParentControl As Control, ByRef oDarkMode As DarkModeCS)
        Dim lst As ListBox
        Dim btn As Button

        'Apply custom Dark Mode changes
        Try
            For Each ctl As Control In oParentControl.Controls
                If ctl.HasChildren Then ApplyCustomFixes(ctl, oDarkMode)

                If TypeOf ctl Is ListBox Then
                    lst = DirectCast(ctl, ListBox)

                    lst.BorderStyle = BorderStyle.FixedSingle
                ElseIf TypeOf ctl Is Button Then
                    btn = DirectCast(ctl, Button)

                    If btn.Text = String.Empty Then
                        btn.FlatStyle = FlatStyle.Flat
                    End If
                End If
            Next
        Catch
            'Do Nothing
        End Try
    End Sub
    Public Shared Sub SetDarkMode(ByRef oForm As Form, Optional ByRef oCustomThemed As List(Of Control) = Nothing, Optional ByVal bColorizeIcons As Boolean = False, Optional ByVal bRoundedPanels As Boolean = False)
        Dim oDarkMode As DarkModeCS

        If UseDarkMode() Then
            oDarkMode = New DarkModeCS(oForm, bColorizeIcons, bRoundedPanels)

            'Apply any custom formatting to make the app look better in dark mode
            ApplyCustomFixes(oForm, oDarkMode)

            'Apply dark theme to controls that are not on the form at runtime
            If oCustomThemed IsNot Nothing Then
                For Each ctl In oCustomThemed
                    oDarkMode.ThemeControl(ctl)
                Next
            End If
        End If

    End Sub

    Public Shared Function UseDarkMode() As Boolean
        'Note: Initializing DarkModeCS in Mono will crash due to Win32 calls.
        'Only use the OS theme if we are running Windows and if the option is enabled in settings.
        If Not mgrCommon.IsUnix And mgrSettings.EnableOSTheme Then
            'Check if OS is Windows 10 or higher
            If Environment.OSVersion.Version.Major >= 10 Then
                'Check if dark mode is enabled in Windows
                If DarkModeCS.GetWindowsColorMode <= 0 Then Return True
            End If
        End If

        Return False
    End Function
End Class
