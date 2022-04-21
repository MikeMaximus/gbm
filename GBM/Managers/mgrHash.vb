Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.Encoding

Public Class mgrHash

    'Generate SHA256 Hash from file
    Public Shared Function Generate_SHA256_Hash(ByVal sPath As String) As String

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

    'Generate a guid using MD5 from a string
    Public Shared Function Generate_MD5_GUID(ByVal sItem As String) As String
        Dim bHashValue() As Byte
        Dim oMD5 As MD5 = MD5.Create()

        bHashValue = oMD5.ComputeHash(UTF8.GetBytes(sItem))

        Return New Guid(bHashValue).ToString
    End Function

    'Safe Compare
    Public Shared Function Compare(ByVal s1 As String, ByVal s2 As String) As Boolean
        If s1.ToUpper = s2.ToUpper Then
            Return True
        End If
        Return False
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
