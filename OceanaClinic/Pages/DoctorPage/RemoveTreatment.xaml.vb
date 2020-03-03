Public Class RemoveTreatment
    Dim ViewModel As New RemoveTreatmentViewModel
    Sub New(ByRef tl As List(Of Treatment))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.Treatments = tl
        DataContext = ViewModel
    End Sub
End Class
Public Class RemoveTreatmentViewModel
    Inherits ObservableObject
    Property Treatments As List(Of Treatment)
End Class