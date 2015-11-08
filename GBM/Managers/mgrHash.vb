Imports System.IO
Imports System.Security
Imports System.Security.Cryptography

Public Class mgrHash

    'Generate SHA256 Hash
    Public Shared Function Generate_SHA256_Hash(ByVal sPath As String)

        Dim bHashValue() As Byte
        Dim oSHA As SHA256 = SHA256.Create()
        Dim sHash As String

        If File.Exists(sPath) Then
            Dim fileStream As FileStream = File.OpenRead(sPath)
            fileStream.Position = 0

            bHashValue = oSHA.ComputeHash(fileStream)

            sHash = PrintByteArray(bHashValue)

            fileStream.Close()
        Else
            sHash = String.Empty
        End If

        Return sHash
    End Function

    ' Print the byte array in a readable format.
    Public Shared Function PrintByteArray(ByVal bArray() As Byte) As String

        Dim sHex As String = String.Empty

        Dim i As Integer
        For i = 0 To bArray.Length - 1
            sHex &= String.Format("{0:X2}", bArray(i))
        Next i

        Return sHex
    End Function

End Class
