Imports System.ComponentModel
Imports System.Globalization
Imports MaterialDesignThemes.Wpf

Public Class AdminPage
    Dim msgQ As New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dgUsers.ItemsSource = gVars.dbAdmin.GetAllUsers()
        MySnackbar.MessageQueue = msgQ
        DataContext = Me
    End Sub
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Me.Close()
    End Sub
    Private Async Sub btnRemoveUser_Click(sender As Object, e As RoutedEventArgs) Handles btnRemoveUser.Click
        If (dgUsers.SelectedIndex = -1) Then
            Return
        End If
        Dim result As Boolean = Await DialogHost.Show(MultiDeleteDialogBox, "RootDialog")
        Dim selectedUsers As List(Of User) = UtilityConverter.SelectedItemsToListOfUsers(dgUsers.SelectedItems)
        If result = True Then
            gVars.dbAdmin.RemoveUsers(selectedUsers)
            dgUsers.ItemsSource = gVars.dbAdmin.GetAllUsers()
            msgQ.Enqueue("Successfully removed " + selectedUsers.Count.ToString + " users!")
        End If
    End Sub

    Private Async Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        Dim selectedUser As User = New User(dgUsers.SelectedValue)
        Dim result As Boolean = Await DialogHost.Show(New UserDetails(selectedUser), "RootDialog")
        If result = True Then
            gVars.dbAdmin.UpdateUser(selectedUser)
            dgUsers.ItemsSource = gVars.dbAdmin.GetAllUsers()
            msgQ.Enqueue("User of UserID(" + selectedUser.UserID.ToString + ") successfully updated!")
        End If
    End Sub

    Private Async Sub btnAddUser_Click(sender As Object, e As RoutedEventArgs) Handles btnAddUser.Click
        Dim inUser As User = New User()
        Dim result As Boolean = Await DialogHost.Show(New AddUser(inUser), "RootDialog")
        If result = True Then
            gVars.dbAdmin.InsertNewUser(inUser)
            dgUsers.ItemsSource = gVars.dbAdmin.GetAllUsers()
            msgQ.Enqueue("New user (" + inUser.Email + ") successfully updated!")
        End If
    End Sub
End Class


