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

    Private Sub FillList()
        Dim sTags As String
        Dim oListViewItem As ListViewItem

        lstGameBox.BeginUpdate()

        lstGameBox.Columns.Add(frmChooseGame_ColumnName, 180)
        lstGameBox.Columns.Add(frmChooseGame_ColumnTags, 175)

        For Each o As clsGame In Process.DuplicateList
            sTags = o.PrintTags
            oListViewItem = New ListViewItem(New String() {o.Name, sTags})
            oListViewItem.Tag = o.ID
            If lstGameBox.Items.Count = 0 Then oListViewItem.Selected = True
            lstGameBox.Items.Add(oListViewItem)
            oGamesHash.Add(o.ID, o)
        Next

        lstGameBox.EndUpdate()
    End Sub

    Private Sub SaveSelection()
        oGame.ProcessPath = oProcess.ProcessPath
        mgrMonitorList.DoListUpdate(oGame)
    End Sub

    Private Sub GetSelection()
        Dim oSelected As String = lstGameBox.SelectedItems(0).Tag
        oGame = DirectCast(oGamesHash.Item(oSelected), clsGame)
        SaveSelection()
        bGameSelected = True
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmChooseGame_FormName
        Me.Icon = GBM_Icon

        'Set Form Text
        btnCancel.Text = frmChooseGame_btnCancel
        btnChoose.Text = frmChooseGame_btnChoose
        lblChoose.Text = frmChooseGame_lblChoose
    End Sub

    Private Sub frmChooseGame_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetForm()
        FillList()
    End Sub

    Private Sub frmChooseGame_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Focus()
        Me.BringToFront()
        Me.TopMost = True
    End Sub

    Private Sub btnChoose_Click(sender As System.Object, e As System.EventArgs) Handles btnChoose.Click
        If lstGameBox.SelectedItems.Count > 0 Then
            GetSelection()
        End If
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