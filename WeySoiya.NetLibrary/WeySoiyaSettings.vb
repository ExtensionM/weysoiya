Public Class WeySoiyaSettings

    Private Encode_ As Text.Encoding
    Public ReadOnly Property Encoding() As Text.Encoding
        Get
            Return Encode_
        End Get
    End Property


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
            Case EncodeKind.SJIS
                Encode_ = Text.Encoding.GetEncoding("shift_jis")
            Case Else

        End Select

    End Sub

    ''' <summary>
    ''' エンコード方式
    ''' </summary>
    ''' <remarks></remarks>
    Enum EncodeKind
        UTF8
        UTF16L
        UTF16B
        UTF32
        SJIS
    End Enum


End Class
