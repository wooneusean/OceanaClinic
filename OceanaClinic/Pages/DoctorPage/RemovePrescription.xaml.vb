Public Class RemovePrescription
    Dim ViewModel As New RemovePrescriptionViewModel
    Sub New(ByRef pl As List(Of Prescription))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.Prescriptions = pl
        DataContext = ViewModel
    End Sub
End Class
Public Class RemovePrescriptionViewModel
    Inherits ObservableObject
    Property Prescriptions As List(Of Prescription)
End Class