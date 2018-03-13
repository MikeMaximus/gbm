Imports GBM.My.Resources

Public Class frmChooseGame

    Private oProcess As mgrProcessDetection
    Private oGame As clsGame
    Private oGamesHash As New Hashtable
    Private bGameSelected As Boolean = False

    Property Process As mgrProcessDetection
        Get
            Return oProcess
        End Get
        Set(value As mgrProcessDetection)
            oProcess = value
        End Set
    End Property

    Property Game As clsGame
        Get
            Return oGame
        End Get
        Set(value As clsGame)
            oGame = value
        End Set
    End Property

    Private Sub FillComboBox()
        lstGameBox.ValueMember = "Key"
        lstGameBox.DisplayMember = "Value"

        For Each o As clsGame In Process.DuplicateList
            lstGameBox.Items.Add(New KeyValuePair(Of String, String)(o.ID, o.Name))
            oGamesHash.Add(o.ID, o)
        Next

        lstGameBox.SelectedIndex = 0
    End Sub

    Private Sub SaveSelection()
        oGame.ProcessPath = oProcess.GameInfo.ProcessPath
        mgrMonitorList.DoListUpdate(oGame)
    End Sub

    Private Sub GetSelection()
        Dim oSelected As KeyValuePair(Of String, String) = lstGameBox.SelectedItem
        oGame = DirectCast(oGamesHash.Item(oSelected.Key), clsGame)
        SaveSelection()
        bGameSelected = True
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmChooseGame_FormName

        'Set Form Text
        btnCancel.Text = frmChooseGame_btnCancel
        btnChoose.Text = frmChooseGame_btnChoose
        lblChoose.Text = frmChooseGame_lblChoose
    End Sub

    Private Sub frmChooseGame_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetForm()
        FillComboBox()
        Me.Focus()
    End Sub

    Private Sub btnChoose_Click(sender As System.Object, e As System.EventArgs) Handles btnChoose.Click
        GetSelection()
    End Sub

    Private Sub frmChooseGame_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If bGameSelected = False Then
            Me.DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class