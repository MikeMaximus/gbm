Imports System.Text.RegularExpressions

Public Class clsLudusaviOptions

    <Flags()> Public Enum eSupportedOS
        None = 0
        Windows = 1
        Linux = 2
    End Enum

    Private sQuery As String
    Private oQueryAsRegEx As Regex
    Private bIncludeSaves As Boolean
    Private bIncludeConfigs As Boolean
    Private eIncludeOS As eSupportedOS

    Property Query As String
        Get
            Return sQuery
        End Get
        Set(value As String)
            sQuery = value
            If sQuery = String.Empty Or sQuery Is Nothing Then
                oQueryAsRegEx = Nothing
            Else
                Try
                    oQueryAsRegEx = New Regex(Regex.Escape(sQuery), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                Catch
                    oQueryAsRegEx = Nothing
                End Try
            End If
        End Set
    End Property

    ReadOnly Property QueryAsRegEx As Regex
        Get
            Return oQueryAsRegEx
        End Get
    End Property

    Property IncludeSaves As Boolean
        Get
            Return bIncludeSaves
        End Get
        Set(value As Boolean)
            bIncludeSaves = value
        End Set
    End Property

    Property IncludeConfigs As Boolean
        Get
            Return bIncludeConfigs
        End Get
        Set(value As Boolean)
            bIncludeConfigs = value
        End Set
    End Property

    Property IncludeOS As eSupportedOS
        Get
            Return eIncludeOS
        End Get
        Set(value As eSupportedOS)
            eIncludeOS = value
        End Set
    End Property

    Sub New()
        Query = String.Empty
        IncludeSaves = False
        IncludeConfigs = False
        IncludeOS = eSupportedOS.None
    End Sub
End Class
