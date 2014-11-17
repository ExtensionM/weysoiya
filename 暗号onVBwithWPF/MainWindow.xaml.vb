Class MainWindow
    Dim TextConvert As New WeySoiyaLib.WeySoiya
    Dim file As New WeySoiyaLib.WeySoiya


    Private Sub OriginText_TextChanged(sender As Object, e As TextChangedEventArgs) Handles OriginText.TextChanged
        'テキスト変換
        If Me.CipherMode.IsChecked Then
            Me.ResultText.Text = TextConvert.GetCipherText(Me.OriginText.Text)
        ElseIf Me.PlaneMode.IsChecked Then
            Me.ResultText.Text = TextConvert.GetPlainText(Me.OriginText.Text)
        End If



    End Sub

    Private Sub CipherMode_Checked(sender As Object, e As RoutedEventArgs) Handles CipherMode.Checked
        If Me.TextMode.IsChecked Then
            'テキスト変換
            Plane2WeyText()
            Me.PlaneMode.IsChecked = False
        End If

        If Me.FileMode.IsChecked Then
            'ファイル変換
            Plane2WeyFile()
            Me.CipherMode.IsChecked = False
        ElseIf FolderMode.IsChecked Then
            'フォルダ変換
            Me.CipherMode.IsChecked = False
        End If

    End Sub

    Private Sub PlaneMode_Checked(sender As Object, e As RoutedEventArgs) Handles PlaneMode.Checked
        If Me.TextMode.IsChecked Then
            'テキスト変換
            Wey2PlaneText()
            Me.CipherMode.IsChecked = False
        End If

        If Me.FileMode.IsChecked Then
            'ファイル変換
            Wey2TextFile()
            Me.PlaneMode.IsChecked = False
        ElseIf FolderMode.IsChecked Then
            'フォルダ変換
            Me.PlaneMode.IsChecked = False
        End If
    End Sub

    Public Sub Plane2WeyText()
        Me.ResultText.Text = TextConvert.GetCipherText(Me.OriginText.Text)
    End Sub

    Public Sub Wey2PlaneText()
        Me.ResultText.Text = TextConvert.GetPlainText(Me.OriginText.Text)
    End Sub

    Public Sub Plane2WeyFile()
        Dim t As String = Me.InputFile.Text
        Dim t2 As String = Me.OutputFile.Text
        'If My.Computer.FileSystem.FileExists(t) Then
        '    Dim s As String = file.GetCipherText(System.IO.File.ReadAllBytes(t))
        '    'Me.Output.Text = s
        '    If t2 <> "" Then
        '        If My.Computer.FileSystem.FileExists(t2) Then
        '            If MsgBox("指定されたファイルは既に存在します。" & vbNewLine & "上書きしますか", vbYesNo) = MsgBoxResult.Yes Then
        '                System.IO.File.WriteAllText(t2, s)
        '            End If
        '        Else
        '            If (My.Computer.FileSystem.DirectoryExists(t2.Substring(0, t2.LastIndexOf("\")))) Then
        '                System.IO.File.WriteAllText(t2, s)
        '            Else
        '                MsgBox("ディレクトリが見つかりません")
        '            End If
        '        End If
        '    End If
        'Else
        '    MsgBox("ファイルが見つかりません")
        'End If
        Dim t3 = file.GetCipherFile(t, t2, WeySoiyaLib.WeySoiya.FileConvertMode.SyncFileToFile)
        If t3 <> "" Then
            MsgBox(t3)
        End If
    End Sub

    Public Sub Wey2TextFile()
        Dim t2 As String = Me.OutputFile.Text
        Dim t As String = Me.InputFile.Text
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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.OriginText.Text = Me.ResultText.Text
    End Sub

    Private Sub TextMode_Checked(sender As Object, e As RoutedEventArgs) Handles TextMode.Checked
        Tab1.Items(0).IsSelected = True
    End Sub

    Private Sub FileMode_Checked(sender As Object, e As RoutedEventArgs) Handles FileMode.Checked
        Tab1.Items(1).IsSelected = True
        Me.CipherMode.IsChecked = False
        Me.PlaneMode.IsChecked = False
    End Sub

    Private Sub FolderMode_Checked(sender As Object, e As RoutedEventArgs) Handles FolderMode.Checked
        Tab1.Items(1).IsSelected = True
        Me.CipherMode.IsChecked = False
        Me.PlaneMode.IsChecked = False
    End Sub
End Class
