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
        RootTransitioner.SelectedIndex = 0
        ViewModel.Prescriptions = Nothing
        ViewModel.Treatments = Nothing
    End Sub

    Private Sub btnFindPatient_Click(sender As Object, e As RoutedEventArgs) Handles btnFindPatient.Click
        Dim i As Integer = 0
        Dim q = txtSearch.Text
        q.Trim()
        If Integer.TryParse(q, i) Then
            Dim p As Patient = gVars.dbDoctor.FindPatient(i)
            If p IsNot Nothing Then
                ViewModel.Patient = p
                ViewModel.Treatments = gVars.dbDoctor.GetTreatments(p.PatientId)
                RootTransitioner.SelectedIndex = 1
            Else
                msgQ.Enqueue("No patient found.")
            End If
        Else
            Dim pl As List(Of Patient) = gVars.dbDoctor.FindPatient(q)
            If pl IsNot Nothing Then
                If pl.Count = 1 Then
                    ViewModel.Patient = pl.First
                    ViewModel.Treatments = gVars.dbDoctor.GetTreatments(pl.First.PatientId)
                    RootTransitioner.SelectedIndex = 1
                Else
                    If pl.Count > 1 Then
                        'handle this idk
                    Else
                        msgQ.Enqueue("No patient found.")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub ReloadTreatments()
        ViewModel.Treatments = gVars.dbDoctor.GetTreatments(ViewModel.Patient.PatientId)
    End Sub
    Private Sub ReloadPrescription()
        If dgTreatments.SelectedIndex = -1 Then
            Return
        End If
        Dim t As Treatment = dgTreatments.SelectedValue
        ViewModel.Prescriptions = gVars.dbDoctor.GetPrescriptions(t.TreatmentId)
    End Sub
    Private Sub dgTreatments_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dgTreatments.SelectionChanged
        ReloadPrescription()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        Dim x As MainWindow = New MainWindow
        x.Show()
        Me.Close()
    End Sub
    Private Sub btnReloadPrescriptions_Click(sender As Object, e As RoutedEventArgs) Handles btnReloadPrescriptions.Click
        ReloadPrescription()
    End Sub

    Private Sub btnReloadTreatments_Click(sender As Object, e As RoutedEventArgs) Handles btnReloadTreatments.Click
        ReloadTreatments()
    End Sub

    Private Async Sub btnAddTreatment_Click(sender As Object, e As RoutedEventArgs) Handles btnAddTreatment.Click
        Dim t As Treatment = New Treatment
        t.PatientId = ViewModel.Patient.PatientId
        Dim result As Boolean = Await DialogHost.Show(New AddTreatment(t), "RootDialog")
        If result = True Then
            If gVars.dbDoctor.InsertTreatment(t) > 0 Then
                msgQ.Enqueue("Success! Added treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            Else
                msgQ.Enqueue("Failed! Couldn't add treatment for " & ViewModel.Patient.Firstname & " " & ViewModel.Patient.Lastname & "!")
            End If
        End If
        ReloadTreatments()
    End Sub

    Private Async Sub btnAddPrescription_Click(sender As Object, e As RoutedEventArgs) Handles btnAddPrescription.Click
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
        ReloadPrescription()
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
End Class