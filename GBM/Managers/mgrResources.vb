Imports GBM.My.Resources
Imports System.IO

Public Class mgrResources

    Private Shared ReadOnly Property OverrideFolder As String
        Get
            Return Application.StartupPath & Path.DirectorySeparatorChar & App_FoldersOverride & Path.DirectorySeparatorChar
        End Get
    End Property

    Private Shared hshCache As New Hashtable

    Public Enum ResourceType
        Icon = 1
        Image = 2
    End Enum

    Public Shared Function GetResource(ByVal sResourceName As String, ByVal eType As ResourceType) As Object
        Dim sOverrideFile As String = OverrideFolder & sResourceName
        Dim oResource As Object

        'Use an already cached resource when possible
        If hshCache.ContainsKey(sResourceName) Then
            oResource = hshCache(sResourceName)
            Return oResource
        End If

        'Set the file type of the resource override
        'Only the same files types as the internal resources are supported
        Select Case eType
            Case ResourceType.Icon
                sOverrideFile &= ".ico"
            Case ResourceType.Image
                sOverrideFile &= ".png"
            Case Else
                Return Nothing
        End Select

        'Check for and retrieve override resource       
        If File.Exists(sOverrideFile) Then
            Try
                Select Case eType
                    Case ResourceType.Icon
                        oResource = New Icon(sOverrideFile)
                    Case ResourceType.Image
                        oResource = Image.FromFile(sOverrideFile)
                    Case Else
                        oResource = Nothing
                End Select

                'External resources are cached for the current session
                If oResource IsNot Nothing Then hshCache.Add(sResourceName, oResource)
            Catch
                'If there is any kind of problem with an override, gracefully fallback to the internal resource and cache it for this session.
                oResource = ResourceManager.GetObject(sResourceName)
                hshCache.Add(sResourceName, oResource)
            End Try
        Else
            'Use internal resource when no override exists
            oResource = ResourceManager.GetObject(sResourceName)
        End If

        'Note that this will always return Nothing if the internal resource name does not exist.
        Return oResource
    End Function
End Class