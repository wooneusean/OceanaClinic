Public Class ReceptionistPage
    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Me.Close()
    End Sub
End Class
