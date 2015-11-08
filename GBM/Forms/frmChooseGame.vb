Public Class frmChooseGame

    Private oProcess As mgrProcesses
    Private oGame As clsGame
    Private oGamesList As New List(Of KeyValuePair(Of String, String))
    Private oGamesHash As New Hashtable
    Private bGameSelected As Boolean = False

    Property Process As mgrProcesses
        Get
            Return oProcess
        End Get
        Set(value As mgrProcesses)
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
        For Each o As clsGame In Process.DuplicateList
            oGamesList.Add(New KeyValuePair(Of String, String)(o.ID, o.Name))
            oGamesHash.Add(o.ID, o)
        Next

        lstGameBox.DataSource = oGamesList
        lstGameBox.ValueMember = "key"
        lstGameBox.DisplayMember = "value"
        lstGameBox.SelectedIndex = 0

    End Sub

    Private Sub SaveSelection()
        oGame.ProcessPath = oProcess.GameInfo.ProcessPath
        mgrMonitorList.DoListUpdate(oGame)
    End Sub

    Private Sub GetSelection()
        Dim sSelectedGame As String
        sSelectedGame = CStr(lstGameBox.SelectedValue)
        oGame = DirectCast(oGamesHash.Item(sSelectedGame), clsGame)
        SaveSelection()
        bGameSelected = True
        Me.Close()
    End Sub

    Private Sub frmChooseGame_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        FillComboBox()
        Me.Focus()
    End Sub

    Private Sub btnChoose_Click(sender As System.Object, e As System.EventArgs) Handles btnChoose.Click
        GetSelection()
    End Sub

    Private Sub frmChooseGame_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If bGameSelected = False Then
            e.Cancel = True
        End If
    End Sub
End Class