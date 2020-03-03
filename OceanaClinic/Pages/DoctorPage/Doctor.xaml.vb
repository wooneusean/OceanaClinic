Imports MaterialDesignThemes.Wpf

Public Class Doctor
    Dim msgQ As New SnackbarMessageQueue(TimeSpan.FromSeconds(3))
    Dim ViewModel As New DoctorViewModel
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MySnackbar.MessageQueue = msgQ
        DataContext = ViewModel
    End Sub

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        GoToPage(0)
        ViewModel.Prescriptions = Nothing
        ViewModel.Treatments = Nothing
    End Sub

    Private Async Sub btnFindPatient_Click(sender As Object, e As RoutedEventArgs) Handles btnFindPatient.Click
        Dim i As Integer = 0
        Dim q = txtSearch.Text.Trim()
        If Integer.TryParse(q, i) Then
            Dim p As Patient = gVars.dbDoctor.FindPatient(i)
            If p IsNot Nothing Then
                ViewModel.Patient = p
                ViewModel.Treatments = gVars.dbDoctor.GetTreatments(p.PatientId)
                GoToPage(1)

            Else
                msgQ.Enqueue("No patient found.")
            End If
        Else
            Dim pl As List(Of Patient) = gVars.dbDoctor.FindPatient(q)
            If pl.Count > 0 Then
                If pl.Count = 1 Then
                    ViewModel.Patient = pl.First
                    ViewModel.Treatments = gVars.dbDoctor.GetTreatments(pl.First.PatientId)
                    GoToPage(1)
                ElseIf pl.Count > 1 Then
                    Dim result As Patient = Await DialogHost.Show(New MultiplePatient(pl), "RootDialog")
                    If result IsNot Nothing Then
                        If result.PatientId <> -1 Then
                            ViewModel.Patient = result
                            ViewModel.Treatments = gVars.dbDoctor.GetTreatments(result.PatientId)
                            GoToPage(1)
                        End If
                    End If
                End If
            Else
                msgQ.Enqueue("No patient found.")
            End If
        End If
    End Sub
    Public Sub GoToPage(index As Integer)
        ViewModel.SelectedPage = index
    End Sub
    Private Sub RefreshTreatments()
        ViewModel.Treatments = gVars.dbDoctor.GetTreatments(ViewModel.Patient.PatientId)
    End Sub
    Private Sub RefreshPrescription()
        If dgTreatments.SelectedIndex = -1 Then
            Return
        End If
        Dim t As Treatment = dgTreatments.SelectedValue
        ViewModel.Prescriptions = gVars.dbDoctor.GetPrescriptions(t.TreatmentId)
    End Sub
    Private Sub dgTreatments_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dgTreatments.SelectionChanged
        RefreshPrescription()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Me.Close()
    End Sub
    Private Sub btnReloadPrescriptions_Click(sender As Object, e As RoutedEventArgs) Handles btnReloadPrescriptions.Click
        RefreshPrescription()
    End Sub

    Private Sub btnReloadTreatments_Click(sender As Object, e As RoutedEventArgs) Handles btnReloadTreatments.Click
        RefreshTreatments()
    End Sub

    Private Async Sub btnAddTreatment_Click(sender As Object, e As RoutedEventArgs) Handles btnAddTreatment.Click
        Dim t As Treatment = New Treatment
        t.PatientId = ViewModel.Patient.PatientId
        Dim result As Boolean = Await DialogHost.Show(New TreatmentEditor(t), "RootDialog")
        If result = True Then
            If gVars.dbDoctor.InsertTreatment(t) > 0 Then
                msgQ.Enqueue("Success! Added treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            Else
                msgQ.Enqueue("Failed! Couldn't add treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            End If
        End If
        RefreshTreatments()
    End Sub

    Private Async Sub btnAddPrescription_Click(sender As Object, e As RoutedEventArgs) Handles btnAddPrescription.Click
        If dgTreatments.SelectedIndex = -1 Then
            msgQ.Enqueue("Please select a treatment to add a prescription to!")
        Else
            Dim trxn As Transaction = New Transaction
            Dim p As Prescription = New Prescription

            Dim t As Treatment = dgTreatments.SelectedValue
            p.TreatmentId = t.TreatmentId

            Dim result As Boolean = Await DialogHost.Show(New AddItem(trxn), "RootDialog")
            If result = True Then
                If gVars.dbDoctor.InsertPrescription(trxn, p, ViewModel.Patient.PatientId) > 0 Then
                    msgQ.Enqueue("Success! Added prescription for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
                Else
                    msgQ.Enqueue("Failed! Couldn't add prescription for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
                End If
            End If
            RefreshPrescription()
        End If
    End Sub
    Private Async Sub btnEditTreatment_Click(sender As Object, e As RoutedEventArgs) Handles btnEditTreatment.Click
        If dgTreatments.SelectedIndex = -1 Then
            msgQ.Enqueue("Please select a treatment to edit!")
        Else
            Dim t As Treatment = dgTreatments.SelectedValue
            t.PatientId = ViewModel.Patient.PatientId
            Dim result As Boolean = Await DialogHost.Show(New TreatmentEditor(t), "RootDialog")
            If result = True Then
                If gVars.dbDoctor.UpdateTreatment(t) > 0 Then
                    msgQ.Enqueue("Success! Edited treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
                Else
                    msgQ.Enqueue("Failed! Couldn't edit treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
                End If
            End If
            RefreshTreatments()
        End If
    End Sub

    Private Async Sub btnEditPrescription_Click(sender As Object, e As RoutedEventArgs) Handles btnEditPrescription.Click
        If dgPrescriptions.SelectedIndex = -1 Then
            Return
        End If
        Dim p As Prescription = dgPrescriptions.SelectedValue
        Dim trxn As Transaction = gVars.dbReception.GetTransaction(p.TransactionId)
        Dim result As Boolean = Await DialogHost.Show(New EditItem(trxn), "RootDialog")
        If result = True Then
            If gVars.dbReception.UpdateTransaction(trxn) > 0 Then
                msgQ.Enqueue("Success! Updated prescription for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            Else
                msgQ.Enqueue("Failure! Failed to update prescription!")
            End If
            RefreshPrescription()
        End If
    End Sub
    Private Async Sub btnRemoveTreatment_Click(sender As Object, e As RoutedEventArgs) Handles btnRemoveTreatment.Click
        If dgTreatments.SelectedIndex = -1 Then
            Return
        End If
        Dim t As List(Of Treatment) = UtilityConverter.SelectedItemsToListOfTreatments(dgTreatments.SelectedItems)
        Dim result As Boolean = Await DialogHost.Show(New RemoveTreatment(t), "RootDialog")
        If result = True Then
            Dim i As Integer = gVars.dbDoctor.RemoveTreatments(t)
            If i > 0 Then
                msgQ.Enqueue("Success! Removed (" & i & ") treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            Else
                msgQ.Enqueue("Failure! Failed to remove treatment!")
            End If
        End If
        RefreshPrescription()
        RefreshTreatments()
    End Sub

    Private Async Sub btnRemovePrescription_Click(sender As Object, e As RoutedEventArgs) Handles btnRemovePrescription.Click
        If dgPrescriptions.SelectedIndex = -1 Then
            Return
        End If
        Dim p As List(Of Prescription) = UtilityConverter.SelectedItemsToListOfPrescriptions(dgPrescriptions.SelectedItems)
        Dim result As Boolean = Await DialogHost.Show(New RemovePrescription(p), "RootDialog")
        If result = True Then
            Dim i As Integer = gVars.dbDoctor.RemovePrescriptions(p)
        End If
        RefreshPrescription()
    End Sub
End Class
Public Class DoctorViewModel
    Inherits ObservableObject
    Private _patient As Patient
    Public Property Patient() As Patient
        Get
            Return _patient
        End Get
        Set(value As Patient)
            _patient = value
            OnPropertyChanged(NameOf(Patient))
        End Set
    End Property
    Private _treatments As List(Of Treatment)
    Public Property Treatments() As List(Of Treatment)
        Get
            Return _treatments
        End Get
        Set(ByVal value As List(Of Treatment))
            _treatments = value
            OnPropertyChanged(NameOf(Treatments))
        End Set
    End Property
    Private _prescriptions As List(Of Prescription)
    Public Property Prescriptions() As List(Of Prescription)
        Get
            Return _prescriptions
        End Get
        Set(ByVal value As List(Of Prescription))
            _prescriptions = value
            OnPropertyChanged(NameOf(Prescriptions))
        End Set
    End Property
    Private _selectedPage As Integer
    Public Property SelectedPage As Integer
        Get
            Return _selectedPage
        End Get
        Set(value As Integer)
            _selectedPage = value
            OnPropertyChanged(NameOf(SelectedPage))
            OnPropertyChanged(NameOf(IsOnPatientPage))
        End Set
    End Property
    Public ReadOnly Property IsOnPatientPage() As Visibility
        Get
            If SelectedPage = 1 Then
                Return Visibility.Visible
            Else
                Return Visibility.Hidden
            End If
        End Get
    End Property
End Class