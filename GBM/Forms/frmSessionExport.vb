Imports GBM.My.Resources

Public Class frmSessionExport
    Public Property XML As Boolean = True
    Public Property Unix As Boolean = False
    Public Property Headers As Boolean = True

    Private Sub SetOptions()
        If optXML.Checked Then
            XML = True
        Else
            XML = False
        End If

        If optUnix.Checked Then
            Unix = True
        Else
            Unix = False
        End If

        If chkCSVHeaders.Checked Then
            Headers = True
        Else
            Headers = False
        End If
    End Sub

    Private Sub SetForm()
        Me.Text = frmSessionExport_FormName
        Me.Icon = GBM_Icon

        grpExportType.Text = frmSessionExport_grpExportType
        grpDateType.Text = frmSessionExport_grpDateType
        grpOptions.Text = frmSessionExport_grpOptions

        optCSV.Text = frmSessionExport_optCSV
        optXML.Text = frmSessionExport_optXML
        optCurrentLocale.Text = frmSessionExport_optCurrentLocale & " - " & Now
        optUnix.Text = frmSessionExport_optUnix & " - " & mgrCommon.DateToUnix(Now)

        chkCSVHeaders.Text = frmSessionExport_chkCSVHeaders

        btnExport.Text = frmSessionExport_btnExport
        btnExport.Image = Multi_Export
        btnCancel.Text = frmSessionExport_btnCancel
        btnCancel.Image = Multi_Cancel

        optCSV.Checked = True
        optCurrentLocale.Checked = True
    End Sub

    Private Sub frmSessionExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        SetOptions()
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub optCSV_CheckedChanged(sender As Object, e As EventArgs) Handles optCSV.CheckedChanged
        If Not optCSV.Checked Then
            chkCSVHeaders.Checked = False
            chkCSVHeaders.Enabled = False
        Else
            chkCSVHeaders.Checked = True
            chkCSVHeaders.Enabled = True
        End If
    End Sub
End Class