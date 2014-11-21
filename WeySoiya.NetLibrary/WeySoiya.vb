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

    Public Event EndedEvent(ByVal sender As WeySoiya, ByVal e As AsyncResultEventArgs)

    Public Event ErrorEvent(ByVal sender As WeySoiya, ByVal e As AsyncResultEventArgs, ByVal ex As Exception)

    Public Shared Setting As New WeySoiyaSettings

    Public Encode As System.Text.Encoding = Text.Encoding.BigEndianUnicode


    Public val_() As String =
    {"ウェイ", "ソイヤ", "うぇい", "そいや",
     "ウェい", "ソイや", "ウぇイ", "ソぃヤ",
     "ウぇい", "ソいや", "うェイ", "そイヤ",
     "うぇイ", "そいヤ", "うェい", "そイや"}
    '{"うぇい", "そいや", "ウェイ", "ソイヤ"}

    ''' <summary>
    ''' 文字列の一覧
    ''' </summary>
    ''' <value>配列</value>
    ''' <remarks></remarks>
    Public Property Val As String()
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
            Return Encode.GetString(bytes)
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
        bytes = Encode.GetBytes(Plain)
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
    ''' <returns>成功時に0</returns>
    ''' <remarks></remarks>
    Public Function GetCipherFile(SrcPath As String, DestPath As String) As String
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
                    dest = New IO.StreamWriter(DestPath, False, Encode)
                Catch ex As Exception
                    Return "ファイルを開けませんでした"
                End Try
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
            Else
                Return "書き込み先のフォルダが見つかりません"
            End If
        Else
            Return "読込元のファイルがありません"
        End If

        Return ""
    End Function

    ''' <summary>
    ''' ファイルを非同期で読み取りウェイファイルに変換する
    ''' </summary>
    ''' <param name="SrcPath">読み取り元のパス</param>
    ''' <param name="DestPath">書き込み先のパス</param>
    ''' <returns>書き込み結果のオブジェクト</returns>
    ''' <remarks>書き込み終了時、エラー時にイベントを起こします</remarks>
    Public Function GetCipherFileAsync(SrcPath As String, DestPath As String) As AsyncResult
        Dim Result = New AsyncResult
        If (SrcPath = "" Or DestPath = "") Or (SrcPath = DestPath) Then
            Result.ErrorMessage = "書き込み先のパスと読込元のパスが不正です"
            Result.ErrorState = True
            Return Result
        End If
        If My.Computer.FileSystem.FileExists(SrcPath) Then
            '読み込むファイルが存在する
            Dim src As IO.Stream = Nothing
            Dim dest As IO.StreamWriter = Nothing

            If DestPath.IndexOf("\") = -1 Then
                'ファイルのパスでない
                Result.ErrorMessage = "書き込み先のパスが不正です"
                Result.ErrorState = True
                Return Result
            End If
            If My.Computer.FileSystem.DirectoryExists(DestPath.Substring(0, DestPath.LastIndexOf("\"))) Then
                '書き込むファイルのフォルダが存在する
                If My.Computer.FileSystem.FileExists(DestPath) Then
                    '書き込み先のファイルが存在する
                    If MsgBox("書き込み先のファイル「" & DestPath & "」を上書きします。" &
                              vbNewLine & "よろしいですか?",
                              MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                        '上書きしない
                        Result.ErrorMessage = "上書きを中止しました"
                        Result.ErrorState = True
                        Return Result
                    End If

                Else
                    '書き込み先のファイルを新規作成する
                End If
                Try
                    src = New IO.FileStream(SrcPath, IO.FileMode.Open, IO.FileAccess.Read)
                    dest = New IO.StreamWriter(DestPath, False, Encode)
                Catch ex As Exception
                    Result.ErrorMessage = "ファイルを開けませんでした"
                    Result.ErrorState = True
                    Return Result
                End Try
                Dim th As New Threading.Thread(
                           Sub()
                               Try
                                   Dim l As Long = 0
                                   Result.Tasks = src.Length
                                   While l < src.Length
                                       Dim b As Byte = src.ReadByte()
                                       For i = 0 To 7 Step bits
                                           dest.Write(GetCipherBit(b, i))
                                       Next
                                       l += 1
                                       Result.Progress = l
                                   End While
                                   src.Close()
                                   dest.Close()
                                   src.Dispose()
                                   dest.Dispose()
                                   Result.Ended = True
                                   RaiseEvent EndedEvent(Me, New AsyncResultEventArgs(SrcPath, DestPath))
                                   Return
                               Catch ex As Exception
                                   Try
                                       src.Close()
                                       dest.Close()
                                   Catch ex2 As Exception
                                       src.Dispose()
                                       dest.Dispose()
                                   End Try
                                   Result.ErrorMessage = ex.Message
                                   Result.ErrorState = True
                                   RaiseEvent ErrorEvent(Me, New AsyncResultEventArgs(SrcPath, DestPath), ex)
                               End Try
                           End Sub)
                th.Start()
            Else
                Result.ErrorMessage = "書き込み先のフォルダが見つかりません"
                Result.ErrorState = True
                Return Result
            End If
        Else
            Result.ErrorMessage = "読込元のファイルがありません"
            Result.ErrorState = True
            Return Result
        End If

        Return Result
    End Function

    'Private Function GetCipherFileAsync_(SrcPath As String, DestPath As String) As AsyncResult
    '    Dim Result = New AsyncResult
    '    If (SrcPath = "" Or DestPath = "") Or (SrcPath = DestPath) Then
    '        Return "書き込み先のパスと読込元のパスが不正です"
    '    End If
    '    If My.Computer.FileSystem.FileExists(SrcPath) Then
    '        '読み込むファイルが存在する
    '        Dim src As IO.Stream = Nothing
    '        Dim dest As IO.StreamWriter = Nothing
    '        If DestPath.IndexOf("\") = -1 Then
    '            'ファイルのパスでない
    '            Return "書き込み先のパスが不正です"
    '        End If
    '        If My.Computer.FileSystem.DirectoryExists(DestPath.Substring(0, DestPath.LastIndexOf("\"))) Then
    '            '書き込むファイルのフォルダが存在する
    '            If My.Computer.FileSystem.FileExists(DestPath) Then
    '                '書き込み先のファイルが存在する
    '                If MsgBox("書き込み先のファイル「" & DestPath & "」を上書きします。" &
    '                          vbNewLine & "よろしいですか?",
    '                          MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
    '                    '上書きしない
    '                    Return "上書きを中止しました"
    '                End If
    '            Else
    '                '書き込み先のファイルを新規作成する
    '            End If
    '            Try
    '                src = New IO.FileStream(SrcPath, IO.FileMode.Open, IO.FileAccess.Read)
    '                dest = New IO.StreamWriter(DestPath, False, Encode)
    '            Catch ex As Exception
    '                Return "ファイルを開けませんでした"
    '            End Try
    '            Dim th As New Threading.Thread(
    '                       Sub()
    '                           Dim l As Long = 0
    '                           While l < src.Length
    '                               Dim b As Byte = src.ReadByte()
    '                               For i = 0 To 7 Step bits
    '                                   dest.Write(GetCipherBit(b, i))
    '                               Next
    '                           End While
    '                           src.Close()
    '                           dest.Close()
    '                           src.Dispose()
    '                           dest.Dispose()
    '                       End Sub)
    '            th.Start()
    '        Else
    '            Return "書き込み先のフォルダが見つかりません"
    '        End If
    '    Else
    '        Return "読込元のファイルがありません"
    '    End If

    '    Return ""
    'End Function

    Public Class AsyncResult

        Private Ended_ As Boolean
        ''' <summary>
        ''' 終了状態
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ended() As Boolean
            Get
                Return Ended_
            End Get
            Set(ByVal value As Boolean)
                Ended_ = value
            End Set
        End Property

        Private ErrorMessage_ As String
        ''' <summary>
        ''' エラーメッセージ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorMessage() As String
            Get
                Return ErrorMessage_
            End Get
            Set(ByVal value As String)
                ErrorMessage_ = value
            End Set
        End Property

        Private Progress_ As Long
        ''' <summary>
        ''' 変換し終わった量
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Progress() As Long
            Get
                Return Progress_
            End Get
            Set(ByVal value As Long)
                Progress_ = value
            End Set
        End Property

        Private Tasks_ As Long
        ''' <summary>
        ''' 変換する量
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tasks() As Long
            Get
                Return Tasks_
            End Get
            Set(ByVal value As Long)
                Tasks_ = value
            End Set
        End Property

        Private ErrorState_ As Boolean
        ''' <summary>
        ''' エラーの状態
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorState() As Boolean
            Get
                Return ErrorState_
            End Get
            Set(ByVal value As Boolean)
                ErrorState_ = value
            End Set
        End Property

    End Class

    Public Class AsyncResultEventArgs
        Inherits EventArgs
        Private SourcePath_ As String
        ''' <summary>
        ''' 読み込み元のファイル
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SourcePath() As String
            Get
                Return SourcePath_
            End Get
        End Property

        Private DestinationPath_ As String
        ''' <summary>
        ''' 書き込み先のファイル
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DestinationPath() As String
            Get
                Return DestinationPath_
            End Get
        End Property

        Public Sub New(Src As String, Dest As String)
            SourcePath_ = Src
            DestinationPath_ = Dest
        End Sub


    End Class


    ''' <summary>
    ''' 長く一致しているものを返す
    ''' </summary>
    ''' <param name="txt">検証するテキスト</param>
    ''' <returns>一致するインデックス</returns>
    ''' <remarks></remarks>
    Private Function chkAll(txt As String) As Integer
        Dim l As Integer = 0
        chkAll = -1
        For i As Integer = 0 To Val.Length - 1
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
        For i As Integer = 0 To Val.Length
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
