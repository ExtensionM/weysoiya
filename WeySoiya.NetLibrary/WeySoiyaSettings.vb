Public Class WeySoiyaSettings

    Private Encode_ As Text.Encoding
    ''' <summary>
    ''' エンコードの形式
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Encoding() As Text.Encoding
        Get
            Return Encode_
        End Get
    End Property

    ''' <summary>
    ''' エンコードの種類を設定する
    ''' </summary>
    ''' <param name="EncodeName">エンコードの種類</param>
    ''' <remarks></remarks>
    Public Sub SetEncoding(EncodeName As EncodeKind)
        Select Case EncodeName
            Case EncodeKind.UTF8
                Encode_ = Text.Encoding.UTF8
            Case EncodeKind.UTF16L
                Encode_ = Text.Encoding.Unicode
            Case EncodeKind.UTF16B
                Encode_ = Text.Encoding.BigEndianUnicode
            Case EncodeKind.UTF32
                Encode_ = Text.Encoding.UTF32
                'Case EncodeKind.SJIS
                '    Encode_ = Text.Encoding.GetEncoding("shift_jis")
            Case Else

        End Select

    End Sub

    ''' <summary>
    ''' エンコード方式
    ''' </summary>
    ''' <remarks></remarks>
    Enum EncodeKind As Byte
        UTF8
        UTF16L
        UTF16B
        UTF32
    End Enum

    Private SettingNo_ As Integer = 0
    Public Property SettingNo() As Integer
        Get
            Return SettingNo_
        End Get
        Set(ByVal value As Integer)
            SettingNo_ = value
        End Set
    End Property


    Public ReadOnly Property Texts As String()
        Get
            Return TextSets(SettingNo).Strings
        End Get
    End Property


    Private TextSets_ As New List(Of TextSet)
    ''' <summary>
    ''' テキストの種類
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TextSets() As List(Of TextSet)
        Get
            Return TextSets_
        End Get
    End Property



End Class


''' <summary>
''' 置き換え用のテキスト
''' </summary>
''' <remarks></remarks>

Public Class TextSet

    Public Name_ As String
    ''' <summary>
    ''' このコンバータの名前
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Name() As String
        Get
            Return Name_
        End Get
    End Property


    Public Strings_ As String()
    ''' <summary>
    ''' 変換結果の文字
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Strings() As String()
        Get
            Return Strings_
        End Get
    End Property

    ''' <summary>
    ''' ビット数
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Bits() As Byte
        Get
            'Return System.Math.Log(Strings.Length, 2)
            Select Case Strings.Length
                Case 2
                    Return 1
                Case 4
                    Return 2
                Case 16
                    Return 4
                Case 256
                    Return 8
            End Select
            Return 0
        End Get
    End Property

    ''' <summary>
    ''' 文字のセットの長さからビット数を確認した後文字を格納
    ''' </summary>
    ''' <param name="Str">文字列の配列</param>
    ''' <remarks></remarks>
    Public Function SetStrings(Str As String()) As Integer
        Select Case Str.Length
            Case 2
            Case 4
            Case 16
            Case 256
            Case Else
                Return -1
        End Select
        For i As Integer = 0 To Str.Length - 2
            For j As Integer = i + 1 To Str.Length - 1
                If Str(i) = Str(j) Then
                    Return -2
                End If
            Next
        Next
        Strings_ = Str
        Return 0
    End Function

    ''' <summary>
    ''' 名前を設定したうえで初期化します
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <remarks></remarks>
    Public Sub New(Name As String, FileName As String)
        Me.Name_ = Name
        Me.FileName_ = FileName
    End Sub

    Private FileName_ As String
    ''' <summary>
    ''' このテキストセットのソースファイルを記憶しています
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FileName() As String
        Get
            Return FileName_
        End Get
    End Property

End Class
