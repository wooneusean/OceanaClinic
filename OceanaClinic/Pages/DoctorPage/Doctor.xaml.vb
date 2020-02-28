Public Class Doctor
    Dim ViewModel As New DoctorViewModel
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = ViewModel
    End Sub
End Class
Public Class DoctorViewModel
    Inherits ObservableObject

End Class