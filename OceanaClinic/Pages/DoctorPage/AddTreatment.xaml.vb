Public Class AddTreatment
    Dim ViewModel As New AddTreatmentViewModel
    Sub New(ByRef t As Treatment)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewModel.Treatment = t
        DataContext = ViewModel
    End Sub
End Class
Public Class AddTreatmentViewModel
    Property Treatment() As Treatment
End Class
