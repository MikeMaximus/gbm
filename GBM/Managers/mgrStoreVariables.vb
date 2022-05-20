Imports System.IO

Public Class mgrStoreVariables

    Enum SupportedAutoConfigApps
        Steam
    End Enum

    Private Shared Function DetectSteam() As String
        Dim oKey As Microsoft.Win32.RegistryKey
        Dim sSteamFolder As String = String.Empty

        If mgrCommon.IsUnix() Then
            sSteamFolder = mgrPath.ReplaceSpecialPaths("$HOME/.steam/steam")
        Else
            Try
                'Steam always seems to use this registry node regardless of architecture.
                oKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Valve\Steam")

                If Not oKey Is Nothing Then
                    If Not oKey.GetValue("InstallPath") Is Nothing Then
                        sSteamFolder = oKey.GetValue("InstallPath").ToString
                    End If
                End If
            Catch
                'Do Nothing
            End Try
        End If

        If Directory.Exists(sSteamFolder) Then
            Return sSteamFolder
        Else
            Return String.Empty
        End If
    End Function

    Private Shared Function DetectSteamUser(ByVal sSteamFolder As String) As String
        Dim sFolders As String()
        Dim sSteamUserData As String
        Dim sSteamID3 As String = String.Empty

        Try
            If sSteamFolder <> String.Empty Then
                sSteamUserData = sSteamFolder & Path.DirectorySeparatorChar & "userdata"
                If Directory.Exists(sSteamUserData) Then
                    sFolders = Directory.GetDirectories(sSteamUserData, "*", SearchOption.TopDirectoryOnly)
                    'If more than one folder exists here (multiple steam accounts), we are just going to take the first one.
                    If sFolders.Length >= 1 Then
                        sSteamID3 = Path.GetFileName(sFolders(0))
                    End If
                End If
            End If
        Catch
            'Do Nothing
        End Try

        Return sSteamID3
    End Function

    Public Shared Function IsAppConfigured(ByVal iSupported As SupportedAutoConfigApps) As Boolean
        Select Case iSupported
            Case SupportedAutoConfigApps.Steam
                If mgrVariables.DoCheck("Steam") And mgrVariables.DoCheck("SteamID3") Then
                    Return True
                End If
        End Select

        Return False
    End Function

    Public Shared Sub AutoConfigureSteamVariables()
        Dim sSteam As String
        Dim sStoreID As String
        Dim oExistingVariable As clsPathVariable
        Dim oVariables As New List(Of clsPathVariable)

        'Auto configure Steam 
        sSteam = DetectSteam()
        sStoreID = DetectSteamUser(sSteam)

        If (Not sSteam = String.Empty And Not sStoreID = String.Empty) Then
            oVariables.Add(New clsPathVariable("Steam", sSteam))
            oVariables.Add(New clsPathVariable("SteamID3", sStoreID))

            'Create or migrate existing variables
            For Each oVariable In oVariables
                oExistingVariable = mgrVariables.DoVariableGetbyNameOrPath(oVariable)

                If oExistingVariable Is Nothing Then
                    mgrVariables.DoVariableAdd(oVariable)
                ElseIf Not oExistingVariable.CoreEquals(oVariable) Then
                    oVariable.ID = oExistingVariable.ID
                    mgrVariables.DoVariableUpdate(oVariable, oExistingVariable)
                End If
            Next
        End If
    End Sub
End Class
