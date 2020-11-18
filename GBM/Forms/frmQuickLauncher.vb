Imports GBM.My.Resources

Public Class frmQuickLauncher

    Private oGame As clsGame
    Private WithEvents tmFilterTimer As Timer

    Public ReadOnly Property Game As clsGame
        Get
            Return oGame
        End Get
    End Property

    Private Sub LoadData()
        Dim oComboItems As New List(Of KeyValuePair(Of clsGame, String))
        Dim oGame As clsGame
        Dim hshGames As Hashtable = mgrMonitorList.ReadList(mgrMonitorList.eListTypes.FullList)

        cboGames.Items.Clear()

        cboGames.ValueMember = "Key"
        cboGames.DisplayMember = "Value"

        For Each de As DictionaryEntry In hshGames
            oGame = DirectCast(de.Value, clsGame)
            cboGames.Items.Add(New KeyValuePair(Of clsGame, String)(oGame, oGame.Name))
        Next

        cboGames.Sorted = True
    End Sub

    Private Sub LaunchGame()
        If Not cboGames.SelectedItem Is Nothing Then
            oGame = DirectCast(cboGames.SelectedItem, KeyValuePair(Of clsGame, String)).Key
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub SetForm()
        'Set Form Name
        Me.Text = frmQuickLauncher_FormName
        Me.Icon = GBM_Icon

        btnGo.Text = frmQuickLauncher_btnGo

        cboGames.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboGames.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub frmQuickLaunch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetForm()
        LoadData()
        cboGames.Focus()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If ((Me.ActiveControl Is cboGames) Or (Me.ActiveControl Is btnGo)) And (keyData = Keys.Enter) Then
            LaunchGame()
        ElseIf ((Me.ActiveControl Is cboGames) Or (Me.ActiveControl Is btnGo)) And (keyData = Keys.Escape) Then
            Me.DialogResult = DialogResult.Cancel
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        LaunchGame()
    End Sub
End Class