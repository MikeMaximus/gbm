﻿Imports GBM.My.Resources
Imports System.Xml.Serialization
Imports System.IO

Public Class mgrMetadata

    Public Event UpdateLog(sLogUpdate As String, bTrayUpdate As Boolean, objIcon As System.Windows.Forms.ToolTipIcon, bTimeStamp As Boolean)

    Public Function ImportandDeserialize(ByRef oBackupMetadata As BackupMetadata) As Boolean
        Dim oReader As StreamReader
        Dim oSerializer As XmlSerializer

        Try
            oReader = New StreamReader(mgrSettings.MetadataLocation)
            oSerializer = New XmlSerializer(GetType(BackupMetadata), New XmlRootAttribute("GBM_Backup"))
            oBackupMetadata = oSerializer.Deserialize(oReader)
            oReader.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function SerializeAndExport(ByVal sLocation As String, ByVal sGameID As String, ByVal oBackup As clsBackup) As Boolean
        Dim oSerializer As XmlSerializer
        Dim oWriter As StreamWriter
        Dim oBackupMetadata As BackupMetadata
        Dim oGame As clsGame
        Dim hshGame As Hashtable

        Try
            'Get a fresh copy of the game object for the metadata
            hshGame = mgrMonitorList.DoListGetbyMonitorID(sGameID)
            If hshGame.Count = 1 Then
                oGame = DirectCast(hshGame(0), clsGame)
            Else
                Return False
            End If
            oBackupMetadata = New BackupMetadata(mgrCommon.AppVersion, oBackup.ConvertToBackup, oGame.ConvertToGame)
            oSerializer = New XmlSerializer(oBackupMetadata.GetType())
            oWriter = New StreamWriter(sLocation)
            oSerializer.Serialize(oWriter.BaseStream, oBackupMetadata)
            oWriter.Flush()
            oWriter.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function AddMetadataToArchive(ByVal sBackupFile As String, ByVal sMetadata As String) As Boolean
        Dim prs7z As New Process
        Dim sArguments As String
        Dim sOutput As String = String.Empty
        Dim bOperationCompleted As Boolean = False

        sArguments = "a -t7z -mx" & mgrSettings.CompressionLevel & " """ & sBackupFile & """ """ & sMetadata & """"

        Try
            If File.Exists(sBackupFile) And File.Exists(mgrSettings.MetadataLocation) Then
                If mgrSettings.Is7zUtilityValid Then
                    prs7z.StartInfo.Arguments = sArguments
                    prs7z.StartInfo.FileName = mgrSettings.Utility7zLocation
                    prs7z.StartInfo.WorkingDirectory = mgrSettings.TemporaryFolder
                    prs7z.StartInfo.UseShellExecute = False
                    prs7z.StartInfo.RedirectStandardOutput = True
                    prs7z.StartInfo.CreateNoWindow = True
                    prs7z.Start()
                    RaiseEvent UpdateLog(mgrMetadata_InProgress, False, ToolTipIcon.Info, True)
                    While Not prs7z.StandardOutput.EndOfStream
                        sOutput &= prs7z.StandardOutput.ReadLine() & vbCrLf
                    End While
                    prs7z.WaitForExit()
                    Select Case prs7z.ExitCode
                        Case 0
                            bOperationCompleted = True
                        Case 1
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Metadata), False, ToolTipIcon.Warning, True)
                            bOperationCompleted = True
                        Case 2
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FatalError, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
                        Case 7
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_CommandFailure, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
                    End Select
                    prs7z.Dispose()
                Else
                    RaiseEvent UpdateLog(App_Invalid7zDetected, False, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FileNotFound, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Metadata, ex.Message}), False, ToolTipIcon.Error, True)
        End Try

        Return bOperationCompleted
    End Function

    Public Function ExtractMetadataFromArchive(ByVal sBackupFileWithPath As String) As Boolean
        Dim prs7z As New Process
        Dim sOutput As String = String.Empty
        Dim bOperationCompleted As Boolean = False

        Try
            If File.Exists(sBackupFileWithPath) Then
                If mgrSettings.Is7zUtilityValid Then
                    prs7z.StartInfo.Arguments = "x " & """" & sBackupFileWithPath & """ -o""" & mgrSettings.TemporaryFolder & """ -i!" & App_MetadataFilename & " -aoa"
                    prs7z.StartInfo.FileName = mgrSettings.Utility7zLocation
                    prs7z.StartInfo.UseShellExecute = False
                    prs7z.StartInfo.RedirectStandardOutput = True
                    prs7z.StartInfo.CreateNoWindow = True
                    prs7z.Start()
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrMetaData_ExtractingFromArchive, Path.GetFileName(sBackupFileWithPath)), False, ToolTipIcon.Info, True)
                    While Not prs7z.StandardOutput.EndOfStream
                        sOutput &= prs7z.StandardOutput.ReadLine() & vbCrLf
                    End While
                    prs7z.WaitForExit()
                    If prs7z.ExitCode = 0 Then
                        bOperationCompleted = True
                    Else
                        RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Metadata), False, ToolTipIcon.Warning, True)
                    End If
                    prs7z.Dispose()
                Else
                    RaiseEvent UpdateLog(App_Invalid7zDetected, False, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FileNotFound, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Metadata, ex.Message}), False, ToolTipIcon.Error, True)
        End Try

        Return bOperationCompleted
    End Function

    Public Function CheckForMetadata(ByVal sBackupFileWithPath As String) As Boolean
        Dim prs7z As New Process
        Dim sArguments As String
        Dim sOutput As String = String.Empty
        Dim bOperationCompleted As Boolean = False

        sArguments = "l -i!" & App_MetadataFilename & " """ & sBackupFileWithPath & """"

        Try
            If File.Exists(sBackupFileWithPath) Then
                If mgrSettings.Is7zUtilityValid Then
                    prs7z.StartInfo.Arguments = sArguments
                    prs7z.StartInfo.FileName = mgrSettings.Utility7zLocation
                    prs7z.StartInfo.UseShellExecute = False
                    prs7z.StartInfo.RedirectStandardOutput = True
                    prs7z.StartInfo.CreateNoWindow = True
                    prs7z.Start()
                    RaiseEvent UpdateLog(mgrCommon.FormatString(mgrMetaData_CheckingArchive, Path.GetFileName(sBackupFileWithPath)), False, ToolTipIcon.Info, True)
                    While Not prs7z.StandardOutput.EndOfStream
                        sOutput &= prs7z.StandardOutput.ReadLine() & vbCrLf
                    End While
                    prs7z.WaitForExit()
                    Select Case prs7z.ExitCode
                        Case 0
                            bOperationCompleted = True
                        Case 1
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_Warnings, App_OperationType_Metadata), False, ToolTipIcon.Warning, True)
                            bOperationCompleted = True
                        Case 2
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FatalError, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
                        Case 7
                            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_CommandFailure, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
                    End Select
                    prs7z.Dispose()
                Else
                    RaiseEvent UpdateLog(App_Invalid7zDetected, False, ToolTipIcon.Error, True)
                End If
            Else
                RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_FileNotFound, App_OperationType_Metadata), False, ToolTipIcon.Error, True)
            End If
        Catch ex As Exception
            RaiseEvent UpdateLog(mgrCommon.FormatString(App_Operation_OtherFailure, New String() {App_OperationType_Metadata, ex.Message}), False, ToolTipIcon.Error, True)
        End Try

        If bOperationCompleted Then
            If sOutput.Contains(App_MetadataFilename) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

End Class
