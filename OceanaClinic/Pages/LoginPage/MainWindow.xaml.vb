Imports MaterialDesignThemes.Wpf
' Material Design Toolkit: https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit
Class MainWindow
    Dim msgQ As New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MySnackbar.MessageQueue = msgQ
        gVars.db.Init()
    End Sub
    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs) Handles btnLogin.Click
        If txtEmail.Text = "" Or txtPassword.Password = "" Then
            msgQ.Enqueue("Email or Password cannot be empty!")
        Else
            Select Case gVars.db.Login(txtEmail.Text, txtPassword.Password)
                Case 0
                    Dim x As AdminPage = New AdminPage()
                    x.Show()
                    Me.Close()
                Case 1
                    Dim x As Doctor = New Doctor
                    x.Show()
                    Me.Close()
                Case 2
                    Dim x As Receptionist = New Receptionist()
                    x.Show()
                    Me.Close()
                Case -1
                    msgQ.Enqueue("Wrong email or password!")
            End Select
        End If
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.Key = Key.Enter Then
            btnLogin_Click(sender, Nothing)
        End If
    End Sub
End Class