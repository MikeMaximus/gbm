Imports GBM.My.Resources

Public Class frmSession

    Private oGame As clsGame

    Property Game As clsGame
        Set(value As clsGame)
            oGame = value
        End Set
        Get
            Return oGame
        End Get
    End Property

    Private Sub FormatGrid()
        dgSessions.Columns.Add("Start", frmSession_ColumnStart)
        dgSessions.Columns.Add("End", frmSession_ColumnEnd)
        dgSessions.Columns.Add("Name", frmSession_ColumnComputerName)
    End Sub

    Private Sub LoadData()
        Me.Text = Game.Name & " " & frmSession_Name

        Dim oData As DataSet = mgrSessions.GetSessionsByGame(Game.ID)

        For Each dr As DataRow In oData.Tables(0).Rows
            dgSessions.Rows.Add(New Object() {mgrCommon.UnixToDate(dr("Start")), mgrCommon.UnixToDate(dr("End")), dr("ComputerName")})
        Next
        dgSessions.AutoResizeColumns()
    End Sub

    Private Sub frmSession_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormatGrid()
        LoadData()
    End Sub
End Class