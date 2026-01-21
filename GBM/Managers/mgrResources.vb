Imports GBM.My.Resources
Imports System.IO

Public Class mgrResources

    Private Shared ReadOnly Property OverrideFolder As String
        Get
            Return Application.StartupPath & Path.DirectorySeparatorChar & App_FoldersOverride & Path.DirectorySeparatorChar
        End Get
    End Property

    Private Shared hshCache As New Hashtable

    Public Shared Function GetResource(ByVal sResourceName As String, ByVal tType As Type) As Object
        Dim sOverrideFile As String = OverrideFolder & sResourceName
        Dim oResource As Object

        If hshCache.ContainsKey(sResourceName) Then
            oResource = hshCache(sResourceName)
            Return oResource
        End If

        Select Case tType
            Case GetType(Icon)
                sOverrideFile &= ".ico"
            Case GetType(Image)
                sOverrideFile &= ".png"
            Case Else
                Return Nothing
        End Select

        If File.Exists(sOverrideFile) Then
            Select Case tType
                Case GetType(Icon)
                    oResource = New Icon(sOverrideFile)
                Case GetType(Image)
                    oResource = Image.FromFile(sOverrideFile)
                Case Else
                    oResource = Nothing
            End Select

            If oResource IsNot Nothing Then hshCache.Add(sResourceName, oResource)
        Else
            oResource = ResourceManager.GetObject(sResourceName)
        End If

        Return oResource
    End Function
End Class