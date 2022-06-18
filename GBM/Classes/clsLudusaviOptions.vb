Imports System.Text.RegularExpressions

Public Class clsLudusaviOptions
    <Flags()> Public Enum eSupportedOS
        None = 0
        Windows = 1
        Linux = 2
    End Enum

    Private sQuery As String

    Public Property Query As String
        Get
            Return sQuery
        End Get
        Set(value As String)
            sQuery = value
            If sQuery = String.Empty Or sQuery Is Nothing Then
                QueryAsRegEx = Nothing
            Else
                Try
                    QueryAsRegEx = New Regex(Regex.Escape(sQuery), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                Catch
                    QueryAsRegEx = Nothing
                End Try
            End If
        End Set
    End Property
    Public Property QueryAsRegEx As Regex
    Public Property IncludeSaves As Boolean
    Public Property IncludeConfigs As Boolean
    Public Property IncludeOS As eSupportedOS

    Sub New()
        Query = String.Empty
        IncludeSaves = False
        IncludeConfigs = False
        IncludeOS = eSupportedOS.None
    End Sub

    Sub New(sQuery As String, bIncludeSaves As Boolean, bIncludeConfigs As Boolean, eIncludeOS As eSupportedOS)
        Query = sQuery
        IncludeSaves = bIncludeSaves
        IncludeConfigs = bIncludeConfigs
        IncludeOS = eIncludeOS
    End Sub
End Class
