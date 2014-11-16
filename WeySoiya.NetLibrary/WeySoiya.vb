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

    'Public maxLen As Integer = 5

    Public val_() As String =
    {"ウェイ", "ソイヤ", "うぇい", "そいや",
     "ウェい", "ソイや", "ウぇイ", "ソぃヤ",
     "ウぇい", "ソいや", "うェイ", "そイヤ",
     "うぇイ", "そいヤ", "うェい", "そイや"}
    '{"うぇい", "そいや", "ウェイ", "ソイヤ"}

    ''' <summary>
    ''' 文字列の書き換え(書き込み専用)
    ''' </summary>
    ''' <value>配列</value>
    ''' <remarks></remarks>
    Public Property val As String()
        Set(ByVal value As String())
            For i As Integer = 0 To 3
                If value.Length = 2 ^ (2 ^ i) Then
                    val_ = value
                    bits = 2 ^ i
                    Return
                End If
            Next
            Throw New Exception("対応していない項目数です")
        End Set
        Get
            Return val_
        End Get
    End Property
    ''' <summary>
    ''' 文字列(読み込み専用)
    ''' </summary>
    ''' <param name="index">n番目の値</param>
    ''' <value>文字列</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property val(index As Integer) As String
        Get
            Return val_(index)
        End Get
    End Property


    Private bits As Integer = 4

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
            For i As Integer = 0 To bytes.Length - 2 Step 2
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
    Public Function GetPlainBytes(Cipher As String) As Byte()
        Dim bytes As New List(Of Byte)
        Dim c As Integer = 0
        'If (Cipher.Length Mod ((8 / bits) * 3)) <> 0 Then
        '    Return {}
        'End If
        While (c < Cipher.Length)
            Dim b1 As Byte = 0

            For j As Integer = 0 To 7 Step bits
                Dim b As Integer = chkAll(Cipher.Substring(c))
                If b = -1 Then
                    Return bytes.ToArray
                End If
                b1 = b1 Or (b << ((8 - bits) - j))
                c += val(b).Length
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
        For i As Integer = 0 To bytes.Length - 2 Step 2
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
    Public Function GetCipherText(Bytes As Byte()) As String
        Dim st As New Text.StringBuilder
        st.Clear()

        For Each b In Bytes
            For i = 0 To 7 Step bits
                st.Append(GetCipherBit(b, i))
            Next
        Next

        Return st.ToString
    End Function

    Private Function GetCipherBit(b As Byte, bit As Byte) As String
        Return val(((2 ^ bits) - 1) And (b >> ((8 - bits) - bit)))
    End Function

    ''' <summary>
    ''' あるファイルから暗号化されたファイルを出力する
    ''' </summary>
    ''' <param name="SrcPath">元のファイルのパス</param>
    ''' <param name="DestPath">変更先のファイルのパス</param>
    ''' <param name="ConvertMode">読み込みの方法を選択できます</param>
    ''' <param name="EndedSub">書き込み終了時に実行するメソッド</param>
    ''' <returns>成功時に0</returns>
    ''' <remarks></remarks>
    Public Function GetCipherFile(SrcPath As String, DestPath As String, Optional ConvertMode As FileConvertMode = FileConvertMode.Async, Optional EndedSub As System.Threading.ThreadStart = Nothing) As String
        If (SrcPath = "" Or DestPath = "") Or (SrcPath = DestPath) Then
            Return "書き込み先のパスと読込元のパスが不正です"
        End If
        If My.Computer.FileSystem.FileExists(SrcPath) Then
            '読み込むファイルが存在する
            Dim src As IO.Stream = Nothing
            Dim dest As IO.StreamWriter = Nothing
            If DestPath.IndexOf("\") = -1 Then
                'ファイルのパスでない
                Return "書き込み先のパスが不正です"
            End If
            If My.Computer.FileSystem.DirectoryExists(DestPath.Substring(0, DestPath.LastIndexOf("\"))) Then
                '書き込むファイルのフォルダが存在する
                If My.Computer.FileSystem.FileExists(DestPath) Then
                    '書き込み先のファイルが存在する
                    If MsgBox("書き込み先のファイル「" & DestPath & "」を上書きします。" &
                              vbNewLine & "よろしいですか?",
                              MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                        '上書きしない
                        Return "上書きを中止しました"
                    End If
                Else
                    '書き込み先のファイルを新規作成する
                End If
                Try
                    src = New IO.FileStream(SrcPath, IO.FileMode.Open, IO.FileAccess.Read)
                    dest = New IO.StreamWriter(DestPath, False, System.Text.Encoding.Unicode)
                Catch ex As Exception
                    Return "ファイルを開けませんでした"
                End Try
                Select Case ConvertMode
                    Case FileConvertMode.SyncFileToFile
                        Dim l As Long = 0
                        While l < src.Length
                            Dim b As Byte = src.ReadByte()
                            For i = 0 To 7 Step bits
                                dest.Write(GetCipherBit(b, i))
                            Next
                            l += 1
                        End While
                        src.Close()
                        dest.Close()
                        src.Dispose()
                        dest.Dispose()
                    Case FileConvertMode.Async
                        Dim th As New Threading.Thread(
                            Sub()
                                Dim l As Long = 0
                                While l < src.Length
                                    Dim b As Byte = src.ReadByte()
                                    For i = 0 To 7 Step bits
                                        dest.Write(GetCipherBit(b, i))
                                    Next
                                End While
                                src.Close()
                                dest.Close()
                                src.Dispose()
                                dest.Dispose()
                                If EndedSub IsNot Nothing Then
                                    EndedSub.Invoke()
                                End If
                            End Sub)
                        th.Start()
                    Case Else
                        Return "変換モードが不正です"
                End Select
            Else
                Return "書き込み先のフォルダが見つかりません"
            End If
        Else
            Return "読込元のファイルがありません"
        End If

        Return ""
    End Function

    ''' <summary>
    ''' ファイルの変換方法
    ''' </summary>
    ''' <remarks></remarks>
    Enum FileConvertMode
        ''' <summary>
        ''' ファイルからファイルへ
        ''' </summary>
        ''' <remarks></remarks>
        SyncFileToFile
        ' ''' <summary>
        ' ''' 全てのデータをメモリに格納してから変換
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'SyncAllBytes
        ''' <summary>
        ''' 非同期でファイルからファイルへ
        ''' </summary>
        ''' <remarks></remarks>
        Async
    End Enum


    ''' <summary>
    ''' 長く一致しているものを返す
    ''' </summary>
    ''' <param name="txt">検証するテキスト</param>
    ''' <returns>一致するインデックス</returns>
    ''' <remarks></remarks>
    Private Function chkAll(txt As String) As Integer
        Dim l As Integer = 0
        chkAll = -1
        For i As Integer = 0 To val.Length - 1
            If txt.StartsWith(val(i)) Then
                If l < val(i).Length Then
                    chkAll = i
                    l = val(i).Length
                End If
            End If
        Next
    End Function

    ''' <summary>
    ''' 一番早く一致したものを返す
    ''' </summary>
    ''' <param name="txt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function chk(txt As String) As Integer
        Dim l As Integer = 0
        For i As Integer = 0 To val.Length
            If txt.StartsWith(val(i)) Then
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
