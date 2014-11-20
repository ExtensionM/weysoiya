Class MainWindow
    Dim TextConvert As New WeySoiyaLib.WeySoiya
    Dim file As New WeySoiyaLib.WeySoiya

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
        Dim async = file.GetCipherFileAsync(t, t2)
        If async.ErrorState Then
            MsgBox(async.ErrorMessage)
        Else
            Me.CipherMode.IsEnabled = False
            AddHandler file.EndedEvent,
                Sub(sender As WeySoiyaLib.WeySoiya, e As WeySoiyaLib.WeySoiya.AsyncResultEventArgs)
                    Me.CipherMode.Dispatcher.Invoke(Sub() Me.CipherMode.IsEnabled = True)
                    MsgBox("Ended")
                End Sub
            AddHandler file.ErrorEvent,
                Sub(sender As WeySoiyaLib.WeySoiya, e As WeySoiyaLib.WeySoiya.AsyncResultEventArgs, ex As Exception)
                    Me.CipherMode.Dispatcher.Invoke(Sub()Me.CipherMode.IsEnabled = True)
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
        'CType(Me.Resources("SettingShow"), System.Windows.Media.Animation.Storyboard).Begin()

        If Not moving Then
            moving = True
            Await Task.Run(Sub() GoToSetting())
            moving = False
        End If
    End Sub

    Private Async Sub Button_Click_5(sender As Object, e As RoutedEventArgs)
        If Not moving Then
            moving = True
            Await Task.Run(Sub() BackFromSetting())
            moving = False
        End If
    End Sub

    Private Sub GoToSetting()
        Dim d As Double = 0
        Dim time As New Stopwatch
        Dim t As Long
        Const TotalTime As Integer = 1000
        time.Start()
        While True
            t = time.ElapsedMilliseconds
            If t > TotalTime Then
                Exit While
            End If
            Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength((Math.Cos(t / TotalTime * Math.PI) + 1) / 2, GridUnitType.Star))
            Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength((2 - (Math.Cos(t / TotalTime * Math.PI) + 1)) / 2, GridUnitType.Star))
            System.Threading.Thread.Sleep(10)
        End While
        Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength(0, GridUnitType.Star))
        Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength(1, GridUnitType.Star))
    End Sub
    Private Sub BackFromSetting()
        Dim d As Double = 0
        Dim time As New Stopwatch
        Dim t As Long
        Const TotalTime As Integer = 1000
        time.Start()
        While True
            t = time.ElapsedMilliseconds
            If t > TotalTime Then
                Exit While
            End If
            Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength((Math.Cos(t / TotalTime * Math.PI) + 1) / 2, GridUnitType.Star))
            Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength((2 - (Math.Cos(t / TotalTime * Math.PI) + 1)) / 2, GridUnitType.Star))
            System.Threading.Thread.Sleep(10)
        End While
        Me.SettingGridWidth.Dispatcher.Invoke(Sub() Me.SettingGridWidth.Width = New GridLength(0, GridUnitType.Star))
        Me.WorkerGridWidth.Dispatcher.Invoke(Sub() Me.WorkerGridWidth.Width = New GridLength(1, GridUnitType.Star))
    End Sub
End Class

Class GridLengthAnimation
    Inherits Animation.AnimationTimeline


    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New GridLengthAnimation()
    End Function

    Public Overrides ReadOnly Property TargetPropertyType As Type
        Get
            Return GetType(GridLength)
        End Get
    End Property
End Class

