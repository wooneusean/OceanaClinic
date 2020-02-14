Imports MaterialDesignThemes.Wpf
Public Class ReceptionistPage
    Dim msgQ As New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
    Dim _patients As ObservablePatients
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _patients = Me.Resources("patients")
        refreshPatients()
        MySnackbar.MessageQueue = msgQ
        DataContext = Me
    End Sub
    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Me.Close()
    End Sub
    Public Sub refreshPatients()
        _patients.Clear()
        For Each patient As Patient In gVars.dbReception.GetAllPatients()
            _patients.Add(patient)
        Next
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtSearch.TextChanged
        CollectionViewSource.GetDefaultView(dgPatients.ItemsSource).Refresh()
    End Sub

    Private Sub btnReload_Click(sender As Object, e As RoutedEventArgs) Handles btnReload.Click
        refreshPatients()
    End Sub
    Public Sub CollectionViewSource_Filter(sender As Object, e As FilterEventArgs)
        Dim p As Patient = e.Item
        If p IsNot Nothing Then
            If (Not String.IsNullOrEmpty(txtSearch.Text)) Then
                Dim q As String = txtSearch.Text.ToLower
                If (p.PatientId.ToString.Contains(q) Or p.Firstname.ToLower.Contains(q) Or p.Lastname.ToLower.Contains(q) Or
                p.Identity.Contains(q) Or p.Mobile.Contains(q) Or p.Address.ToLower.Contains(q) Or p.Email.ToLower.Contains(q) Or
                p.Weight.ToString.Contains(q) Or p.Height.ToString.Contains(q) Or p.BloodType.ToString.ToLower.Contains(q) Or
                p.Allergies.ToLower.Contains(q)) Then
                    e.Accepted = True
                Else
                    e.Accepted = False
                End If
            Else
                e.Accepted = True
            End If
        End If
    End Sub

    Private Sub dgUsers_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles dgPatients.MouseDoubleClick
        'btnEdit_Click(sender, Nothing)
    End Sub
    Private Sub Window_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Delete Then
            'btnRemoveUser_Click(sender, Nothing)
        End If
    End Sub
    Private Async Sub btnAddPatient_Click(sender As Object, e As RoutedEventArgs) Handles btnAddPatient.Click
        Dim inPatient As Patient = New Patient()
        Dim result As Boolean = Await DialogHost.Show(New AddPatient(inPatient), "RootDialog")
        If result = True Then
            gVars.dbReception.InsertNewPatient(inPatient)
            refreshPatients()
            msgQ.Enqueue("New user (" + inPatient.Identity + ") successfully updated!")
        End If
    End Sub
End Class