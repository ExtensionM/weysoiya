Class MainWindow 
    Dim TextConvert As New WeySoiya
    Dim file As New WeySoiya

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Cipher.Text = TextConvert.GetCipherText(Me.Plain.Text)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Plain.Text = TextConvert.GetPlainText(Me.Cipher.Text)
    End Sub

    Private Sub Plain_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Plain.TextChanged
        Me.Cipher.Text = TextConvert.GetCipherText(Me.Plain.Text)
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim t As String = Me.OriginFile.Text
        Dim t2 As String = Me.WeySoiyaFile.Text
        If My.Computer.FileSystem.FileExists(t) Then
            Dim s As String = file.GetCipherText(System.IO.File.ReadAllBytes(t))
            Me.Output.Text = s
            If t2 <> "" Then
                If My.Computer.FileSystem.FileExists(t2) Then
                    If MsgBox("指定されたファイルは既に存在します。" & vbNewLine & "上書きしますか", vbYesNo) = MsgBoxResult.Yes Then
                        System.IO.File.WriteAllText(t2, s)
                    End If
                Else
                    If (My.Computer.FileSystem.DirectoryExists(t2.Substring(0, t2.LastIndexOf("\")))) Then
                        System.IO.File.WriteAllText(t2, s)
                    Else
                        MsgBox("ディレクトリが見つかりません")
                    End If
                End If
            End If
        Else
            MsgBox("ファイルが見つかりません")
        End If

    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Dim t2 As String = Me.OriginFile.Text
        Dim t As String = Me.WeySoiyaFile.Text
        If My.Computer.FileSystem.FileExists(t) Then
            If t2 <> "" Then
                If My.Computer.FileSystem.FileExists(t2) Then
                    If MsgBox("指定されたファイルは既に存在します。" & vbNewLine & "上書きしますか", vbYesNo) = MsgBoxResult.Yes Then
                        Dim b As Byte() = file.GetPlainBytes(System.IO.File.ReadAllText(t))
                        System.IO.File.WriteAllBytes(t2, b)
                    End If
                Else
                    If (My.Computer.FileSystem.DirectoryExists(t2.Substring(0, t2.LastIndexOf("\")))) Then
                        Dim b As Byte() = file.GetPlainBytes(System.IO.File.ReadAllText(t))
                        System.IO.File.WriteAllBytes(t2, b)
                    Else
                        MsgBox("ディレクトリが見つかりません")
                    End If
                End If
            End If
        Else
            MsgBox("ファイルが見つかりません")
        End If

    End Sub
End Class
