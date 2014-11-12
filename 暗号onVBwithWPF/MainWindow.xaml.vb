Class MainWindow 

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim wey As New WeySoiya
        Me.Cipher.Text = wey.GetCipherText(Me.Plain.Text)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
