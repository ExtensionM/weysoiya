Public Class WeySoiya

    Public Shared len As Integer = 3

    Public Shared val() As String = {"うぇい", "そいや", "ウェイ", "ソイヤ"}

    Private Shared bits As Integer = 2

    Public Sub New()

    End Sub


    ''' <summary>
    ''' 暗号化されたテキストから平文を取得する
    ''' </summary>
    ''' <param name="Cipher">暗号文</param>
    ''' <returns>平文</returns>
    ''' <remarks></remarks>
    Public Function GetPlainText(Cipher As String) As String
        Try
            Dim bytes As New List(Of Byte)
            Dim c As Integer = 0
            While (c < Cipher.Length)
                Dim b1 As Byte = 0, b2 As Byte = 0

                For j As Integer = 0 To 7 Step bits
                    Dim b As Integer = chk(Cipher.Substring(c, len))
                    If b = -1 Then
                        Return ""
                    End If
                    b1 = b1 Or (b << ((8 - bits) - j))
                    c += len
                Next
                For j As Integer = 0 To 7 Step bits
                    Dim b As Integer = chk(Cipher.Substring(c, len))
                    If b = -1 Then
                        Return ""
                    End If
                    b2 = b2 Or (b << ((8 - bits) - j))
                    c += len
                Next
                bytes.Add(b2)
                bytes.Add(b1)
            End While
            Return Text.Encoding.Unicode.GetString(bytes.ToArray)
        Catch ex As Exception

        End Try
        Return ""
    End Function

    ''' <summary>
    ''' 平文から暗号化されたテキストを取得する
    ''' </summary>
    ''' <param name="Plain">平文</param>
    ''' <returns>暗号文</returns>
    ''' <remarks></remarks>
    Public Function GetCipherText(Plain As String) As String
        Dim st As New Text.StringBuilder
        st.Clear()
        Dim bytes As Byte()
        bytes = Text.Encoding.Unicode.GetBytes(Plain)
        For i As Integer = 0 To bytes.Length - 1 Step 2
            Dim b As Byte = bytes(i)
            bytes(i) = bytes(i + 1)
            bytes(i + 1) = b
        Next

        For Each b In bytes
            For i = 0 To 7 Step bits
                st.Append(val(((2 ^ bits) - 1) And (b >> ((8 - bits) - i))))
            Next

        Next

        Return st.ToString
    End Function

    Private Function chk(txt As String) As Integer
        For i As Integer = 0 To val.Length
            If val(i) = txt Then
                Return i
            End If
        Next
        Return -1
    End Function

End Class
