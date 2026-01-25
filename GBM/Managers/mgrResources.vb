Imports GBM.My.Resources
Imports System.IO

Public Class mgrResources

    Private Shared ReadOnly Property OverrideFolder As String
        Get
            Return Application.StartupPath & Path.DirectorySeparatorChar & App_FoldersOverride & Path.DirectorySeparatorChar
        End Get
    End Property

    Private Shared ReadOnly sIcons() As String = {".ico"}
    Private Shared ReadOnly sImages() As String = {".bmp", ".gif", ".jpg", ".jpeg", ".png", ".wmf"}
    Private Shared hshCache As New Hashtable

    Public Enum ResourceType
        Icon = 1
        Image = 2
    End Enum

    Public Shared Function GetResource(ByVal sResourceName As String, ByVal eType As ResourceType) As Object
        Dim sOverrideBase As String = OverrideFolder & sResourceName
        Dim sOverrideTypes() As String
        Dim sOverrideFile As String = String.Empty
        Dim oResource As Object

        'Use an already cached resource when possible
        If hshCache.ContainsKey(sResourceName) Then
            oResource = hshCache(sResourceName)
            Return oResource
        End If

        'Get a list of valid file types for the current resource
        Select Case eType
            Case ResourceType.Icon
                sOverrideTypes = sIcons
            Case ResourceType.Image
                sOverrideTypes = sImages
            Case Else
                Return Nothing
        End Select

        'The first valid override found is used, based on the alphabetic order of the file extension.
        For Each sType As String In sOverrideTypes
            If File.Exists(sOverrideBase & sType) Then
                sOverrideFile = sOverrideBase & sType
                Exit For
            End If
        Next

        If sOverrideFile = String.Empty Then
            'Use internal resource when no override exists
            oResource = ResourceManager.GetObject(sResourceName)
        Else
            'Retrieve the override resource if one exists
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
        End If

        'Note that this will always return Nothing if the internal resource name does not exist.
        Return oResource
    End Function
End Class