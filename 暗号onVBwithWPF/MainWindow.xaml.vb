Imports WeySoiyaLib
Class MainWindow
    Dim TextConvert As New WeySoiya
    Dim FileConvert As New WeySoiya

    Private moving As Boolean = False

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
        Dim async = FileConvert.GetCipherFileAsync(t, t2)
        If async.ErrorState Then
            MsgBox(async.ErrorMessage)
        Else
            Me.CipherMode.IsEnabled = False
            AddHandler FileConvert.EndedEvent,
                Sub(sender As WeySoiya, e As WeySoiya.AsyncResultEventArgs)
                    Me.CipherMode.Dispatcher.Invoke(Sub() Me.CipherMode.IsEnabled = True)
                    MsgBox("Ended")
                End Sub
            AddHandler FileConvert.ErrorEvent,
                Sub(sender As WeySoiya, e As WeySoiya.AsyncResultEventArgs, ex As Exception)
                    Me.CipherMode.Dispatcher.Invoke(Sub() Me.CipherMode.IsEnabled = True)
                    MsgBox(ex.Message)
                End Sub
        End If

    End Sub

    Public Sub Wey2TextFile()
        Dim t2 As String = Me.OutputFile.Text
        Dim t As String = Me.InputFile.Text
        If My.Computer.FileSystem.FileExists(t) Then
            If t2 <> "" Then
                If My.Computer.FileSystem.FileExists(t2) Then
                    If MsgBox("指定されたファイルは既に存在します。" & vbNewLine & "上書きしますか", vbYesNo) = MsgBoxResult.Yes Then
                        Dim b As Byte() = FileConvert.GetPlainBytes(System.IO.File.ReadAllText(t))
                        System.IO.File.WriteAllBytes(t2, b)
                    End If
                Else
                    If (My.Computer.FileSystem.DirectoryExists(t2.Substring(0, t2.LastIndexOf("\")))) Then
                        Dim b As Byte() = FileConvert.GetPlainBytes(System.IO.File.ReadAllText(t))
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

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        '入力ファイルを参照
        Dim Dialog As New Microsoft.Win32.OpenFileDialog
        Dialog.InitialDirectory = My.Application.Info.DirectoryPath
        If Dialog.ShowDialog() Then
            Me.InputFile.Text = Dialog.FileName
        End If
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        '出力ファイルを参照
        Dim Dialog As New Microsoft.Win32.SaveFileDialog
        Dialog.DefaultExt = ".wey"
        Dialog.Filter = "Weyファイル(.wey)|*.wey"
        Dialog.OverwritePrompt = False
        Dialog.InitialDirectory = My.Application.Info.DirectoryPath
        If Dialog.ShowDialog() Then
            Me.OutputFile.Text = Dialog.FileName
        End If
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Clipboard.SetText(Me.ResultText.Text)
    End Sub

    Private Async Sub Button_Click_4(sender As Object, e As RoutedEventArgs)
        '設定表示
        If Not moving Then
            moving = True
            Await Task.Run(Sub() GoToSetting())
            moving = False
        End If
    End Sub

    Private Async Sub Button_Click_5(sender As Object, e As RoutedEventArgs)
        With WeySoiya.Setting
            If Not moving Then
                moving = True
                .SetEncoding(Me.EncordType.SelectedIndex)
                My.Settings.Encoding = Me.EncordType.SelectedIndex
                For i As Integer = 0 To .TextSets.Count - 1
                    If (.TextSets(i).Name = Me.Languages.SelectedValue) And
                        Convert.ToByte(CType(Me.Patterns.SelectedValue, String).Substring(0, 3)) =
                        .TextSets(i).Strings.Count Then
                        .SettingNo = i
                        My.Settings.Bits = .TextSets(.SettingNo).Bits
                        My.Settings.Language = .TextSets(.SettingNo).Name
                    End If
                Next

                Await Task.Run(Sub() BackFromSetting())
                moving = False
            End If
        End With
    End Sub
    '設定画面へ遷移する
    Private Sub GoToSetting()
        Dim d As Double = 0
        Dim time As New Stopwatch
        Dim t As Long
        Dim w As Double
        Const TotalTime As Integer = 1000
        time.Start()
        Me.ParentGrid.Dispatcher.Invoke(Sub() w = Me.ParentGrid.ActualWidth)
        Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength(w, GridUnitType.Pixel))
        Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength(1, GridUnitType.Star))
        While True
            t = time.ElapsedMilliseconds
            If t > TotalTime Then
                Exit While
            End If
            Me.ParentGrid.Dispatcher.Invoke(Sub() w = Me.ParentGrid.ActualWidth)
            Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength((Math.Cos(t / TotalTime * Math.PI) + 1) / 2 * w, GridUnitType.Pixel))
            'Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength((2 - (Math.Cos(t / TotalTime * Math.PI) + 1)) / 2, GridUnitType.Star))
            System.Threading.Thread.Sleep(10)
        End While
        Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength(0, GridUnitType.Star))
    End Sub
    '設定画面から戻る
    Private Sub BackFromSetting()
        Dim d As Double = 0
        Dim time As New Stopwatch
        Dim t As Long
        Dim w As Double
        Const TotalTime As Integer = 1000
        time.Start()
        While True
            t = time.ElapsedMilliseconds
            If t > TotalTime Then
                Exit While
            End If
            Me.ParentGrid.Dispatcher.Invoke(Sub() w = Me.ParentGrid.ActualWidth)
            'Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength((Math.Cos(t / TotalTime * Math.PI) + 1) / 2, GridUnitType.Star))
            Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength((2 - (Math.Cos(t / TotalTime * Math.PI) + 1)) / 2 * w, GridUnitType.Pixel))
            System.Threading.Thread.Sleep(10)
        End While
        Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength(0, GridUnitType.Star))
        Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength(1, GridUnitType.Star))
    End Sub

    Private Sub MainWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        My.Settings.Save()
    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        If 4 < My.Settings.Encoding Then
            My.Settings.Encoding = WeySoiyaSettings.EncodeKind.UTF16B
            My.Settings.Save()
        End If
        WeySoiya.Setting.SetEncoding(My.Settings.Encoding)


    End Sub

    Private Async Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Await Task.Run(Sub() LoadFiles())
        With WeySoiya.Setting
            If .TextSets.Count = 0 Then
                MsgBox("データがありません")
                Me.Close()
                Return
            Else
                Dim langs As New List(Of String)
                For Each ts In .TextSets
                    Dim NeedAdd As Boolean = True
                    For Each lang In langs
                        If lang = ts.Name Then
                            NeedAdd = False
                        End If
                    Next
                    If NeedAdd Then
                        langs.Add(ts.Name)
                        Me.Languages.Items.Add(ts.Name)
                    End If
                Next
            End If
        End With
        Me.EncordType.SelectedIndex = My.Settings.Encoding
        If Me.Languages.Items.Contains(My.Settings.Language) Then
            Me.Languages.SelectedValue = My.Settings.Language
            Languages_SelectionChanged(Me.Languages, Nothing)
            If Me.Patterns.Items.Contains(String.Format("{0,3}" & "パターン", 2 ^ My.Settings.Bits)) Then
                Me.Patterns.SelectedValue = String.Format("{0,3}" & "パターン", 2 ^ My.Settings.Bits)
                Patterns_SelectionChanged(Me.Patterns, Nothing)
            Else
                Me.Patterns.SelectedIndex = 0
                Patterns_SelectionChanged(Me.Patterns, Nothing)
            End If
        Else
            Me.Languages.SelectedIndex = 0
            Languages_SelectionChanged(Me.Languages, Nothing)
            Me.Patterns.SelectedIndex = 0
            Patterns_SelectionChanged(Me.Patterns, Nothing)
        End If

        WeySoiya.Setting.SetEncoding(Me.EncordType.SelectedIndex)
        For i As Integer = 0 To WeySoiya.Setting.TextSets.Count - 1
            If (WeySoiya.Setting.TextSets(i).Name = Me.Languages.SelectedValue) And
                Convert.ToByte(CType(Me.Patterns.SelectedValue, String).Substring(0, 3)) =
                WeySoiya.Setting.TextSets(i).Strings.Count Then
                WeySoiya.Setting.SettingNo = i
            End If
        Next


    End Sub

    Private Sub LoadFiles()
        Dim wssFilePath As String = My.Application.Info.DirectoryPath & "\WeySoiyaSettings\"
        Try
            My.Computer.FileSystem.CreateDirectory(wssFilePath)
            Dim Files = My.Computer.FileSystem.GetFiles(wssFilePath)
            For Each File In Files
                If File.EndsWith(".wss") Then
                    Dim ts As TextSet = Nothing
                    Dim str As New List(Of String)
                    Try
                        Using Sr As New IO.StreamReader(File, System.Text.Encoding.Unicode)
                            ts = New TextSet(Sr.ReadLine(), File.Substring(File.LastIndexOf("\") + 1))
                            While Not Sr.EndOfStream
                                str.Add(Sr.ReadLine)
                            End While
                            Sr.Close()
                        End Using
                        Dim i As Integer = ts.SetStrings(str.ToArray)
                        If i = 0 Then
                            WeySoiya.Setting.TextSets.Add(ts)
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Next

            Dim ErrorRised As Boolean = False
            With WeySoiya.Setting.TextSets
                For i As Integer = 0 To .Count - 2
                    If .Item(i).Name.Replace(" ", "").Replace(vbTab, "") = "" Then
                        MsgBox("wssファイルの設定が不正です" & vbNewLine &
                               "名前が空白もしくは空です" & vbNewLine &
                               "Path = " & """" & .Item(i).FileName & """" & vbNewLine &
                               "Name = " & """" & .Item(i).Name & """")
                        ErrorRised = True
                    End If
                    For j As Integer = i + 1 To .Count - 1
                        If .Item(i).Name = .Item(j).Name Then
                            If .Item(i).Bits = .Item(j).Bits Then
                                MsgBox("wssファイルの設定が不正です" & vbNewLine &
                                       "同じ名前、同じパターン数のものが存在します" & vbNewLine &
                                       "Path = " & """" & .Item(i).FileName & """ = """ & .Item(j).FileName & """" & vbNewLine &
                                       "Name = " & """" & .Item(i).Name & """" & vbNewLine &
                                       "パターン数 = " & .Item(i).Strings.Length)
                                ErrorRised = True
                            End If
                        End If
                    Next
                Next
                If ErrorRised Then
                    Me.Dispatcher.Invoke(Sub() Me.Close())
                End If


            End With

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Languages_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Languages.SelectionChanged

        With Me.Languages
            Dim st As String = .SelectedValue()
            Me.Patterns.Items.Clear()
            Me.LanguagePreView.Items.Clear()
            For Each ts In WeySoiya.Setting.TextSets
                If (ts.Name = st) Then
                    Me.Patterns.Items.Add(String.Format("{0,3}", ts.Strings.Count()) & "パターン")
                End If
            Next
            Me.Patterns.SelectedIndex = 0
        End With
    End Sub

    Private Sub Patterns_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Patterns.SelectionChanged
        With Me.LanguagePreView
            If Me.Patterns.SelectedIndex = -1 Then
                Return
            End If

            .Items.Clear()
            For Each ts In WeySoiya.Setting.TextSets
                If (Me.Languages.SelectedValue = ts.Name) AndAlso
                    (Convert.ToByte(CType(Me.Patterns.SelectedValue, String).Substring(0, 3)) = ts.Strings.Count) Then
                    For Each st In ts.Strings
                        .Items.Add(st)
                    Next
                End If

            Next


        End With
    End Sub
End Class
