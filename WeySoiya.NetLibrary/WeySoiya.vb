Imports System.Runtime.InteropServices
'<ClassInterface(ClassInterfaceType.None)>
<ComVisible(True)>
<ComClass(WeySoiya.ClassID, WeySoiya.InterfaceID, WeySoiya.EventsID)>
Public Class WeySoiya
    'Implements IWeySoiya
#Region "COM GUID"

    Public Const ClassID As String =
        "2379828E-3BDC-4F4D-9334-D133D48F7DFC"

    Public Const InterfaceID As String =
        "32264AB0-F1A0-479B-9C1B-30A60C718C6E"

    Public Const EventsID As String =
        "8003629C-2D34-46B6-B18D-792A3A0E7668"

#End Region

    Public Shared len As Integer = 3

    Public Shared val() As String =
    {"ウェイ", "ソイヤ", "うぇい", "そいや",
     "ウェい", "ソイや", "ウぇイ", "ソぃヤ",
     "ウぇい", "ソいや", "うェイ", "そイヤ",
     "うぇイ", "そいヤ", "うェい", "そイや"}
    '{"うぇい", "そいや", "ウェイ", "ソイヤ"}

    Private Shared bits As Integer = 4

    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' 暗号化されたテキストから平文を取得する
    ''' </summary>
    ''' <param name="Cipher">暗号文</param>
    ''' <returns>平文</returns>
    ''' <remarks></remarks>
    Public Function GetPlainText(Cipher As String) As String
        Try

            Dim bytes As Byte()
            bytes = GetPlainBytes(Cipher)
            For i As Integer = 0 To bytes.Length - 1 Step 2
                Dim b As Byte
                b = bytes(i)
                bytes(i) = bytes(i + 1)
                bytes(i + 1) = b
            Next
            Return Text.Encoding.Unicode.GetString(bytes)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' 暗号化されたテキストからバイナリを取得する
    ''' </summary>
    ''' <param name="Cipher">暗号文</param>
    ''' <returns>平文</returns>
    ''' <remarks></remarks>
    Function GetPlainBytes(Cipher As String) As Byte()
        Dim bytes As New List(Of Byte)
        Dim c As Integer = 0
        If (Cipher.Length Mod ((8 / bits) * 3)) <> 0 Then
            Return {}
        End If
        While (c < Cipher.Length)
            Dim b1 As Byte = 0

            For j As Integer = 0 To 7 Step bits
                Dim b As Integer = chk(Cipher.Substring(c, len))
                If b = -1 Then
                    Return {}
                End If
                b1 = b1 Or (b << ((8 - bits) - j))
                c += len
            Next
            bytes.Add(b1)
        End While
        Return bytes.ToArray
    End Function


    ''' <summary>
    ''' 平文から暗号化されたテキストを取得する
    ''' </summary>
    ''' <param name="Plain">平文</param>
    ''' <returns>暗号文</returns>
    ''' <remarks></remarks>
    Public Function GetCipherText(Plain As String) As String
        Dim bytes As Byte()
        bytes = Text.Encoding.Unicode.GetBytes(Plain)
        For i As Integer = 0 To bytes.Length - 1 Step 2
            Dim b As Byte = bytes(i)
            bytes(i) = bytes(i + 1)
            bytes(i + 1) = b
        Next
        Return GetCipherText(bytes)
    End Function

    ''' <summary>
    ''' 平文から暗号化されたテキストを取得する
    ''' </summary>
    ''' <param name="Bytes">元のバイナリデータ</param>
    ''' <returns>暗号文</returns>
    ''' <remarks></remarks>
    Function GetCipherText(Bytes As Byte()) As String
        Dim st As New Text.StringBuilder
        st.Clear()

        For Each b In Bytes
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

#If False Then
Public Interface IWeySoiya
    Function GetPlainText(Cipher As String) As String
    Function GetPlainBytes(Cipher As String) As Byte()
    Function GetCipherText(Plain As String) As String
    Function GetCipherText(Bytes As Byte()) As String


End Interface
#End If

