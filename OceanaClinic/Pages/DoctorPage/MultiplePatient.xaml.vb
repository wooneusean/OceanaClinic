Imports MaterialDesignThemes.Wpf

Public Class MultiplePatient
    Dim ViewModel As New MultiplePatientViewModel
    Sub New(pl As List(Of Patient))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.Patients = pl
        DataContext = ViewModel
    End Sub

    Private Sub dgPatient_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles dgPatient.SelectionChanged
        ViewModel.Selected = dgPatient.SelectedValue
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ViewModel.Selected = New Patient()
    End Sub
End Class
Public Class MultiplePatientViewModel
    Inherits ObservableObject
    Property Patients() As List(Of Patient)
    Private _selectedPatient As New Patient()
    Property Selected() As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
            OnPropertyChanged(NameOf(Selected))
        End Set
    End Property
End Class
